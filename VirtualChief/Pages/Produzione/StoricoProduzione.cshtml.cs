using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class StoricoProduzioneModel : PageModel
    {
        private readonly ILogger<StoricoProduzioneModel> _logger;

        public StoricoProduzioneModel(ILogger<StoricoProduzioneModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavStoricoProduzione { get; private set; } = "Storico Produzione";
        public bool HasPermission { get; private set; } = false;
        public DateTime Inizio { get; private set; } = DateTime.Now.AddDays(-7);
        public DateTime Fine { get; private set; } = DateTime.Now;
        public string ErrorMessage { get; private set; } = "";

        public void OnGet(DateTime? inizio, DateTime? fine)
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            if (inizio.HasValue) Inizio = inizio.Value;
            if (fine.HasValue) Fine = fine.Value;

            try
            {
                var elencoPermessi = new List<string[]> 
                { 
                    new[] { "Articoli", "R" },
                    new[] { "Analisi Articolo Costo", "R" }
                };
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
                _logger.LogError(ex, "StoricoProduzione/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}