using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Reparti
{
    public class ElencoFestivitaModel : PageModel
    {
        private readonly ILogger<ElencoFestivitaModel> _logger;

        public ElencoFestivitaModel(ILogger<ElencoFestivitaModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavReparti { get; private set; } = "Reparti";
        public string NavConfigTurni { get; private set; } = "Configura Turni";
        public string NavElencoFestivita { get; private set; } = "Elenco Festività";
        public bool HasPermission { get; private set; } = false;
        public int TurnoId { get; private set; }
        public string ErrorMessage { get; private set; } = "";

        public void OnGet(int id)
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            TurnoId = id;

            try
            {
                var elencoPermessi = new List<string[]> { new[] { "Reparto Festivita", "R" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

                    if (!HasPermission)
                    {
                        ErrorMessage = "Non hai il permesso di visualizzare le festività";
                    }
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ElencoFestivita/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}