using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Reparti
{
    public class ManageStraordinarioModel : PageModel
    {
        private readonly ILogger<ManageStraordinarioModel> _logger;

        public ManageStraordinarioModel(ILogger<ManageStraordinarioModel> logger)
        {
            _logger = logger;
        }

        public Turno? TurnoCorrente { get; private set; }
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
                _logger.LogError(ex, "Reparti/ManageStraordinario: verifica permesso {Permission}", nomePermesso);
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

            try
            {
                var trn = new Turno(tenant, id);
                if (trn.id == -1)
                {
                    ErrorMessage = "Turno non trovato.";
                    return Page();
                }

                var rp = new Reparto(tenant, trn.idReparto);
                if (rp.id == -1)
                {
                    ErrorMessage = "Reparto non trovato.";
                    return Page();
                }

                TurnoCorrente = trn;
                Dipartimento = rp;
                CanWrite = HasPermission("Reparto Festivita", "W");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/ManageStraordinario: caricamento turno {Id}", id);
                ErrorMessage = "Errore durante il caricamento.";
            }

            return Page();
        }
    }
}