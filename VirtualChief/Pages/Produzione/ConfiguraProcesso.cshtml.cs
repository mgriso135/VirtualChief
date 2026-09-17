using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class ConfiguraProcessoModel : PageModel
    {
        private readonly ILogger<ConfiguraProcessoModel> _logger;

        public ConfiguraProcessoModel(ILogger<ConfiguraProcessoModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavCommesseDaProdurre { get; private set; } = "Commesse da Produrre";
        public string NavConfiguraProcesso { get; private set; } = "Configurazione Processo";
        public bool HasPermission { get; private set; } = false;
        public int ArtId { get; private set; } = -1;
        public int ArtYear { get; private set; } = -1;
        public string ErrorMessage { get; private set; } = "";

        public void OnGet(int id, int anno)
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            ArtId = id;
            ArtYear = anno;

            try
            {
                var elencoPermessi = new List<string[]> { new[] { "Articoli", "X" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

                    if (HasPermission)
                    {
                        var art = new Articolo(Tenant, ArtId, ArtYear);
                        if (art.ID == -1 || art.Proc == null)
                        {
                            HasPermission = false;
                            ErrorMessage = "Articolo non valido o processo non configurato";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Utente non autenticato.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ConfiguraProcesso/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}