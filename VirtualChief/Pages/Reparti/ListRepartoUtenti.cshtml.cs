using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Reparti
{
    public class ListRepartoUtentiModel : PageModel
    {
        private readonly ILogger<ListRepartoUtentiModel> _logger;

        public ListRepartoUtentiModel(ILogger<ListRepartoUtentiModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavReparti { get; private set; } = "Reparti";
        public string NavListRepartoUtenti { get; private set; } = "Utenti Reparto";
        public bool HasPermission { get; private set; } = false;
        public int RepId { get; private set; }
        public string ErrorMessage { get; private set; } = "";

        public void OnGet(int id)
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            RepId = id;

            try
            {
                var elencoPermessi = new List<string[]> { new[] { "Reparto Operatori", "W" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

                    if (!HasPermission)
                    {
                        ErrorMessage = "Non hai il permesso di gestire gli operatori del reparto";
                    }
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListRepartoUtenti/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}