using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Reparti
{
    public class ProcessoWorkLoadModel : PageModel
    {
        private readonly ILogger<ProcessoWorkLoadModel> _logger;

        public ProcessoWorkLoadModel(ILogger<ProcessoWorkLoadModel> logger)
        {
            _logger = logger;
        }

        public int ProcessId { get; private set; }
        public int VariantId { get; private set; }
        public int RepartoId { get; private set; }
        public processo? Processo { get; private set; }
        public variante? Variante { get; private set; }
        public Reparto? Dipartimento { get; private set; }
        public bool CanView { get; private set; }
        public string ErrorMessage { get; private set; } = "";

        private string? Tenant => CurrentWorkspace.Of(User);

        private bool HasPermission(string nomePermesso, string level)
        {
            var uidStr = User.FindFirst("uid")?.Value;
            var tenant = Tenant;
            if (!int.TryParse(uidStr, out var uid) || string.IsNullOrEmpty(tenant))
            {
                return false;
            }
            try
            {
                var prm = new List<string[]> { new[] { nomePermesso, level } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/ProcessoWorkLoad: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public IActionResult OnGet(int processId, int variantId, int repId)
        {
            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            ProcessId = processId;
            VariantId = variantId;
            RepartoId = repId;

            try
            {
                Processo = new processo(tenant, processId);
                if (Processo.processID == -1)
                {
                    ErrorMessage = "Processo non trovato.";
                    return Page();
                }

                Variante = new variante(tenant, variantId);
                if (Variante.idVariante == -1)
                {
                    ErrorMessage = "Variante non trovata.";
                    return Page();
                }

                Dipartimento = new Reparto(tenant, repId);
                if (Dipartimento.id == -1)
                {
                    ErrorMessage = "Reparto non trovato.";
                    return Page();
                }

                CanView = HasPermission("Postazione", "R");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/ProcessoWorkLoad: caricamento processo {ProcessId} variante {VariantId} reparto {RepartoId}", processId, variantId, repId);
                ErrorMessage = "Errore durante il caricamento.";
            }

            return Page();
        }
    }
}