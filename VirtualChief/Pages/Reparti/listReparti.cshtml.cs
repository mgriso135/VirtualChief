using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChief.Pages.Reparti
{
    /// <summary>
    /// Migrated from WebForms Reparti/listReparti.aspx + listReparti.ascx +
    /// addReparto.ascx: department list with per-row management link and the
    /// "new department" form (permission "Reparto" W, default timezone
    /// W. Europe Standard Time — same as the legacy control).
    /// </summary>
    public class listRepartiModel : PageModel
    {
        private readonly ILogger<listRepartiModel> _logger;

        public listRepartiModel(ILogger<listRepartiModel> logger)
        {
            _logger = logger;
        }

        public List<Reparto> ElencoReparti { get; set; } = new();

        public bool CanWrite { get; private set; }

        public string CurrentTimezone { get; set; } = "W. Europe Standard Time";
        public List<TimeZoneInfo> Timezones { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        private string? Tenant => CurrentWorkspace.Of(User);

        private bool HasWritePermission()
        {
            var uidStr = User.FindFirst("uid")?.Value;
            var tenant = Tenant;
            if (!int.TryParse(uidStr, out var uid) || string.IsNullOrEmpty(tenant))
            {
                return false;
            }
            try
            {
                var prm = new List<string[]> { new[] { "Reparto", "W" } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/listReparti: verifica permesso Reparto");
                return false;
            }
        }

        public void OnGet()
        {
            try
            {
                ElencoReparti = new ElencoReparti(Tenant!).elenco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/listReparti: caricamento elenco");
            }

            CanWrite = HasWritePermission();
            if (CanWrite)
            {
                try
                {
                    Timezones = TimeZoneInfo.GetSystemTimeZones().ToList();
                    CurrentTimezone = "W. Europe Standard Time"; // legacy addReparto default
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Reparti/listReparti: caricamento timezone");
                }
            }
        }

        /// <summary>addReparto.ascx.save_Click.</summary>
        public IActionResult OnPostAddReparto(string nome, string descrizione, string timezone)
        {
            if (!HasWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrEmpty(timezone))
            {
                Message = "Il nome del reparto è obbligatorio.";
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var rp = new Reparto(Tenant!);
                int rt = rp.Add(
                    System.Net.WebUtility.HtmlEncode(nome),
                    System.Net.WebUtility.HtmlEncode(descrizione ?? ""),
                    System.Net.WebUtility.HtmlEncode(timezone));
                if (rt != -1)
                {
                    _logger.LogInformation("Nuovo reparto {Nome} creato (id {Id})", nome, rt);
                    Message = $"Reparto '{nome}' creato.";
                }
                else
                {
                    Message = "Errore durante la creazione. " + rp.err;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/listReparti: creazione reparto");
                Message = "Errore durante la creazione.";
            }
            return RedirectToPage(new { handler = "" });
        }
    }
}
