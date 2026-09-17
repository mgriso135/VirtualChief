using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class StatoAvanzamentoArticoloModel : PageModel
    {
        private readonly ILogger<StatoAvanzamentoArticoloModel> _logger;

        public StatoAvanzamentoArticoloModel(ILogger<StatoAvanzamentoArticoloModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavStatoAvanzamento { get; private set; } = "Stato Avanzamento Articolo";
        public bool HasPermission { get; private set; } = false;
        public int ArtId { get; private set; }
        public int ArtYear { get; private set; }
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
                var elencoPermessi = new List<string[]> { new[] { "Articoli", "R" } };
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
                _logger.LogError(ex, "StatoAvanzamentoArticolo/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }
    }
}