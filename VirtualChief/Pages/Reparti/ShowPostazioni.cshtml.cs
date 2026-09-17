using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using Dapper;

namespace VirtualChief.Pages.Reparti
{
    public class ShowPostazioniModel : PageModel
    {
        private readonly ILogger<ShowPostazioniModel> _logger;

        public ShowPostazioniModel(ILogger<ShowPostazioniModel> logger)
        {
            _logger = logger;
        }

        public int ProcessId { get; private set; }
        public int VariantId { get; private set; }
        public processo? Processo { get; private set; }
        public variante? Variante { get; private set; }
        public Reparto? Dipartimento { get; private set; }
        public bool CanWrite { get; private set; }
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
                _logger.LogError(ex, "Reparti/ShowPostazioni: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public IActionResult OnGet(int processId, int variantId)
        {
            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            ProcessId = processId;
            VariantId = variantId;

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

                // Query the department ID from the database
                try
                {
                    using var conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    int? repId = conn.QueryFirstOrDefault<int?>("SELECT reparto FROM repartiprocessi WHERE processID = @p0 AND variante = @p1", new { p0 = processId, p1 = variantId });
                    if (repId.HasValue && repId.Value > 0)
                    {
                        Dipartimento = new Reparto(tenant, repId.Value);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ShowPostazioni: errore query reparto per processo {ProcessId} variante {VariantId}", processId, variantId);
                }

                CanWrite = HasPermission("Processo", "W");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/ShowPostazioni: caricamento processo {ProcessId} variante {VariantId}", processId, variantId);
                ErrorMessage = "Errore durante il caricamento.";
            }

            return Page();
        }
    }
}