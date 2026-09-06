using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class CommesseDaProdurreModel : PageModel
    {
        private readonly ILogger<CommesseDaProdurreModel> _logger;

        public CommesseDaProdurreModel(ILogger<CommesseDaProdurreModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavNuoveCommesse { get; private set; } = "Nuove Commesse";
        public bool HasPermission { get; private set; } = false;
        public List<Articolo> Articoli { get; private set; } = new();
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
                var elencoPermessi = new List<string[]> { new[] { "Articoli", "R" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

if (HasPermission)
                {
                    var elArtAperti = new ElencoArticoli(Tenant, 'N');
                    Articoli = elArtAperti.ListArticoli;
                }
                    else
                    {
                        ErrorMessage = "Non hai il permesso di visualizzare i prodotti da inserire in produzione";
                    }
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommesseDaProdurre/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}