using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using Dapper;

namespace VirtualChief.Pages.Reparti
{
    public class PostazioneWorkLoadModel : PageModel
    {
        private readonly ILogger<PostazioneWorkLoadModel> _logger;

        public PostazioneWorkLoadModel(ILogger<PostazioneWorkLoadModel> logger)
        {
            _logger = logger;
        }

        public int PostId { get; private set; }
        public Postazione? Postazione { get; private set; }
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
                _logger.LogError(ex, "Reparti/PostazioneWorkLoad: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public IActionResult OnGet(int id)
        {
            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            PostId = id;

            try
            {
                var pst = new Postazione(tenant, id);
                if (pst.id == -1)
                {
                    ErrorMessage = "Postazione non trovata.";
                    return Page();
                }

                Postazione = pst;

                // Query the department ID from the database
                try
                {
                    using var conn = (new Dati.Dati()).mycon(tenant);
                    conn.Open();
                    int? repId = conn.QueryFirstOrDefault<int?>("SELECT reparto FROM postazioni WHERE idpostazioni = @p0", new { p0 = id });
                    if (repId.HasValue && repId.Value > 0)
                    {
                        Dipartimento = new Reparto(tenant, repId.Value);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "PostazioneWorkLoad: errore query reparto per postazione {Id}", id);
                }

                CanView = HasPermission("Postazione", "R");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/PostazioneWorkLoad: caricamento postazione {Id}", id);
                ErrorMessage = "Errore durante il caricamento.";
            }

            return Page();
        }
    }
}