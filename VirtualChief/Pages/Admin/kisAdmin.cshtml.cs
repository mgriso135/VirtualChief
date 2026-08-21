using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChief.Pages.Admin
{
    /// <summary>
    /// Migrated from WebForms Admin/kisAdmin.aspx + kisBilling.ascx +
    /// configLogo.ascx + configTimezone.ascx + configWizard_TipoPERT.ascx.
    /// Each section is gated by the same permission ("Billing" R,
    /// "Configurazione Logo"/"Configurazione TimeZone"/"Wizard TipoPERT" W)
    /// through the legacy UserAccount.ValidatePermissions.
    /// </summary>
    public class kisAdminModel : PageModel
    {
        private readonly ILogger<kisAdminModel> _logger;

        public kisAdminModel(ILogger<kisAdminModel> logger)
        {
            _logger = logger;
        }

        public class BillingPoint
        {
            public int Year { get; set; }
            public int Month { get; set; } // 0 = yearly aggregate
            public int Value { get; set; }
        }

        // Section permissions (legacy names, evaluated on every request)
        public bool CanBilling { get; private set; }
        public bool CanLogo { get; private set; }
        public bool CanTimezone { get; private set; }
        public bool CanPert { get; private set; }

        // Billing
        public DateTime ExpiryDate { get; set; }
        public List<BillingPoint> OrdersByMonth { get; set; } = new();
        public List<BillingPoint> TasksByMonth { get; set; } = new();
        public List<BillingPoint> OrdersByYear { get; set; } = new();
        public List<BillingPoint> TasksByYear { get; set; } = new();

        // Logo
        public string LogoFilePath { get; set; } = "";

        // Timezone
        public string CurrentTimezone { get; set; } = "";
        public List<TimeZoneInfo> Timezones { get; set; } = new();

        // PERT wizard type
        public string PertType { get; set; } = "";

        [TempData]
        public string Message { get; set; }

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
                _logger.LogError(ex, "Admin/kisAdmin: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public void OnGet()
        {
            CanBilling = HasPermission("Billing", "R");
            CanLogo = HasPermission("Configurazione Logo", "W");
            CanTimezone = HasPermission("Configurazione TimeZone", "W");
            CanPert = HasPermission("Wizard TipoPERT", "W");

            var tenant = Tenant;
            if (string.IsNullOrEmpty(tenant))
            {
                return;
            }

            if (CanLogo)
            {
                try
                {
                    LogoFilePath = new Logo(tenant).filePath ?? "";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Admin/kisAdmin: caricamento logo");
                }
            }

            if (CanTimezone)
            {
                try
                {
                    var fo = new FusoOrario(tenant);
                    CurrentTimezone = fo.tzFusoOrario.Id;
                    Timezones = TimeZoneInfo.GetSystemTimeZones().ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Admin/kisAdmin: caricamento timezone");
                }
            }

            if (CanPert)
            {
                try
                {
                    PertType = new WizardConfig(tenant).interfacciaPERT ?? "";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Admin/kisAdmin: caricamento wizard config");
                }
            }

            if (CanBilling)
            {
                LoadBilling(tenant);
            }
        }

        /// <summary>kisBilling.ascx.Page_Load: orders and finished tasks per month/year.</summary>
        private void LoadBilling(string tenant)
        {
            try
            {
                ExpiryDate = new KISConfig(tenant).ExpiryDate;

                var elArt = new ElencoArticoli(tenant, new DateTime(1970, 1, 1), DateTime.UtcNow.AddMonths(1));
                OrdersByMonth = elArt.ListArticoli
                    .Where(y => y.DataInserimento >= DateTime.UtcNow.AddMonths(-13))
                    .OrderBy(z => z.DataInserimento)
                    .GroupBy(x => new { Year = x.DataInserimento.Year, Month = x.DataInserimento.Month })
                    .Select(x => new BillingPoint { Value = x.Count(), Year = x.Key.Year, Month = x.Key.Month })
                    .ToList();

                OrdersByYear = elArt.ListArticoli
                    .OrderBy(z => z.DataInserimento)
                    .GroupBy(x => new { Year = x.DataInserimento.Year })
                    .Select(x => new BillingPoint { Value = x.Count(), Year = x.Key.Year })
                    .ToList();

                var elTasks = new ElencoTaskProduzione(tenant, new DateTime(1970, 1, 1), DateTime.UtcNow.AddMonths(1), 'F');
                TasksByMonth = elTasks.Tasks
                    .Where(y => y.DataFineTask >= DateTime.UtcNow.AddMonths(-13))
                    .OrderBy(z => z.DataFineTask)
                    .GroupBy(x => new { Year = x.DataFineTask.Year, Month = x.DataFineTask.Month })
                    .Select(x => new BillingPoint { Value = x.Count(), Year = x.Key.Year, Month = x.Key.Month })
                    .ToList();

                TasksByYear = elTasks.Tasks
                    .Where(y => y.DataFineTask >= DateTime.UtcNow.AddMonths(-13))
                    .OrderBy(z => z.DataFineTask)
                    .GroupBy(x => new { Year = x.DataFineTask.Year })
                    .Select(x => new BillingPoint { Value = x.Count(), Year = x.Key.Year })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin/kisAdmin: caricamento billing");
            }
        }

        /// <summary>configTimezone.ascx.ddlTimezones_SelectedIndexChanged.</summary>
        public IActionResult OnPostTimezone(string timezone)
        {
            if (!HasPermission("Configurazione TimeZone", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            if (string.IsNullOrEmpty(timezone))
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(timezone);
                var current = new FusoOrario(Tenant!);
                current.fusoOrario = timezone;
                Message = $"Fuso orario impostato su {timezone}. {current.log}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin/kisAdmin: salvataggio timezone");
                Message = "Errore durante il salvataggio del fuso orario.";
            }
            return RedirectToPage(new { handler = "" });
        }

        /// <summary>configWizard_TipoPERT.ascx.rb1_SelectedIndexChanged.</summary>
        public IActionResult OnPostPert(string tipo)
        {
            if (!HasPermission("Wizard TipoPERT", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            if (tipo != "Graph" && tipo != "Table")
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var wizCfg = new WizardConfig(Tenant!)
                {
                    interfacciaPERT = tipo
                };
                Message = $"Interfaccia PERT impostata su {tipo}. {wizCfg.log}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin/kisAdmin: salvataggio tipo PERT");
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { handler = "" });
        }

        /// <summary>configLogo.ascx.Upload: max 1 MB, immagini, cartella ~/Data/Logo pulita ad ogni upload.</summary>
        public IActionResult OnPostLogo(IFormFile? logoFile)
        {
            if (!HasPermission("Configurazione Logo", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            if (logoFile == null || logoFile.Length == 0)
            {
                Message = "Nessun file selezionato.";
                return RedirectToPage(new { handler = "" });
            }
            if (logoFile.Length > 1024 * 1024)
            {
                Message = "Il file supera la dimensione massima di 1 MB.";
                return RedirectToPage(new { handler = "" });
            }

            string[] allowedExtensions = { ".BMP", ".JPG", ".PNG", ".GIF", ".JPEG" };
            var ext = Path.GetExtension(logoFile.FileName).ToUpperInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                Message = "Formato non valido: sono ammessi BMP, JPG, PNG, GIF.";
                return RedirectToPage(new { handler = "" });
            }

            try
            {
                var logoDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Logo");
                if (!Directory.Exists(logoDir))
                {
                    Directory.CreateDirectory(logoDir);
                }
                foreach (var file in Directory.GetFiles(logoDir))
                {
                    System.IO.File.Delete(file);
                }

                var fileName = Path.GetFileName(logoFile.FileName);
                using (var stream = System.IO.File.Create(Path.Combine(logoDir, fileName)))
                {
                    logoFile.CopyTo(stream);
                }

                var logo = new Logo(Tenant!)
                {
                    filePath = fileName
                };
                _logger.LogInformation("Logo aggiornato: {File} ({Log})", fileName, logo.log);
                Message = "Logo aggiornato. " + logo.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin/kisAdmin: upload logo");
                Message = "Errore durante l'upload: " + ex.Message;
            }
            return RedirectToPage(new { handler = "" });
        }
    }
}
