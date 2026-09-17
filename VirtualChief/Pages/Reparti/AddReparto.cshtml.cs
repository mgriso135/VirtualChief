using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Reparti
{
    public class AddRepartoModel : PageModel
    {
        private readonly ILogger<AddRepartoModel> _logger;

        public AddRepartoModel(ILogger<AddRepartoModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavReparti { get; private set; } = "Reparti";
        public string NavAddReparto { get; private set; } = "Aggiungi Reparto";
        public bool HasPermission { get; private set; } = false;
        public string ErrorMessage { get; private set; } = "";

        public void OnGet()
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            try
            {
                var elencoPermessi = new List<string[]> { new[] { "Reparto", "W" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

                    if (!HasPermission)
                    {
                        ErrorMessage = "Non hai il permesso di aggiungere reparti";
                    }
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddReparto/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}