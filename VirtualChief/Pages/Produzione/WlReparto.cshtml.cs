using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class WlRepartoModel : PageModel
    {
        private readonly ILogger<WlRepartoModel> _logger;

        public WlRepartoModel(ILogger<WlRepartoModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavWlReparto { get; private set; } = "Carico di Lavoro Reparto";
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
                var elencoPermessi = new List<string[]> { new[] { "Reparto WorkLoad", "R" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WlReparto/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}