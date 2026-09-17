using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class WorkLoadSimRepartiModel : PageModel
    {
        private readonly ILogger<WorkLoadSimRepartiModel> _logger;

        public WorkLoadSimRepartiModel(ILogger<WorkLoadSimRepartiModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavWorkLoadSimReparti { get; private set; } = "Simulazione Carico di Lavoro";
        public bool HasPermission { get; private set; } = false;
        public List<Reparto> Reparti { get; private set; } = new();
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
                var elencoPermessi = new List<string[]> 
                { 
                    new[] { "Reparto", "R" },
                    new[] { "Reparto WorkLoad", "X" }
                };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

                    if (HasPermission)
                    {
                        var el = new ElencoReparti(Tenant);
                        Reparti = el.elenco;
                    }
                    else
                    {
                        ErrorMessage = "Non hai il permesso di visualizzare i reparti";
                    }
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WorkLoadSimReparti/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}