using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChief.Pages.Andon
{
    /// <summary>
    /// Migrated from WebForms Andon/configAndonCompleto.aspx +
    /// AndonCompletoViewFields.ascx + configFormatoUsername.ascx:
    /// configuration of the "Andon Completo" board — visible fields for
    /// products and tasks (order / add / remove) and the username display
    /// format. All mutations go through the legacy KIS.App_Sources.AndonCompleto
    /// class and are gated by the same permissions.
    /// </summary>
    public class configAndonCompletoModel : PageModel
    {
        private readonly ILogger<configAndonCompletoModel> _logger;

        public configAndonCompletoModel(ILogger<configAndonCompletoModel> logger)
        {
            _logger = logger;
        }

        public bool CanViewFields { get; private set; }
        public bool CanUsernameFormat { get; private set; }
        public bool CanScrollType { get; private set; }

        /// <summary>Scroll type of a board: 0 = disabled, 1 = continuous.</summary>
        public class ScrollTypeInfo
        {
            public int DepartmentId { get; set; } = -1; // -1 = whole board
            public string DepartmentName { get; set; } = "";
            public int ScrollType { get; set; }
            public double GoSpeed { get; set; }
            public double BackSpeed { get; set; }
        }

        public ScrollTypeInfo BoardScrollType { get; set; } = new();
        public List<ScrollTypeInfo> DepartmentScrollTypes { get; set; } = new();

        // Shown fields ordered by display order, plus the still-available ones.
        public List<KeyValuePair<string, int>> ShownFields { get; set; } = new();
        public List<KeyValuePair<string, int>> AvailableFields { get; set; } = new();
        public List<KeyValuePair<string, int>> ShownFieldsTasks { get; set; } = new();
        public List<KeyValuePair<string, int>> AvailableFieldsTasks { get; set; } = new();

        // '0' username, '1' first name, '2' name + surname initial, '3' name + surname
        public char UsernameFormat { get; set; } = '0';

        [TempData]
        public string Message { get; set; }

        private string? Tenant => CurrentWorkspace.Of(User);

        private bool HasPermission(string nomePermesso)
        {
            var uidStr = User.FindFirst("uid")?.Value;
            var tenant = Tenant;
            if (!int.TryParse(uidStr, out var uid) || string.IsNullOrEmpty(tenant))
            {
                return false;
            }
            try
            {
                var prm = new List<string[]> { new[] { nomePermesso, "W" } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        private void LoadViewFields()
        {
            try
            {
                var cfg = new AndonCompleto(Tenant!);
                cfg.loadCampiVisualizzati();
                ShownFields = cfg.CampiVisualizzati.OrderBy(x => x.Value).ToList();
                AvailableFields = cfg.FieldList
                    .Where(f => !cfg.CampiVisualizzati.ContainsKey(f.Key))
                    .ToList();

                cfg.loadCampiVisualizzatiTasks();
                ShownFieldsTasks = cfg.CampiVisualizzatiTasks.OrderBy(x => x.Value).ToList();
                AvailableFieldsTasks = cfg.FieldListTasks
                    .Where(f => !cfg.CampiVisualizzatiTasks.ContainsKey(f.Key))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: caricamento campi");
            }
        }

        public void OnGet()
        {
            CanViewFields = HasPermission("AndonCompleto CampiDaVisualizzare");
            CanUsernameFormat = HasPermission("AndonCompleto VisualizzazioneNomiUtente");
            CanScrollType = HasPermission("AndonCompleto ScrollType");

            if (string.IsNullOrEmpty(Tenant))
            {
                return;
            }

            if (CanViewFields)
            {
                LoadViewFields();
            }

            if (CanUsernameFormat)
            {
                try
                {
                    UsernameFormat = new AndonCompleto(Tenant!).PostazioniFormatoUsername;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Andon/configAndonCompleto: caricamento formato username");
                }
            }

            if (CanScrollType)
            {
                LoadScrollTypes();
            }
        }

        /// <summary>Legacy AndonConfigController.ScrollTypeView / DepartmentScrollTypeView.</summary>
        private void LoadScrollTypes()
        {
            try
            {
                var board = new AndonCompleto(Tenant!);
                board.loadScrollType();
                BoardScrollType = new ScrollTypeInfo
                {
                    ScrollType = board.ScrollType,
                    GoSpeed = board.ContinuousScrollGoSpeed,
                    BackSpeed = board.ContinuousScrollBackSpeed
                };

                var reparti = new ElencoReparti(Tenant!);
                foreach (var rp in reparti.elenco)
                {
                    try
                    {
                        var cfg = new AndonReparto(Tenant!, rp.id);
                        cfg.loadScrollType();
                        DepartmentScrollTypes.Add(new ScrollTypeInfo
                        {
                            DepartmentId = rp.id,
                            DepartmentName = rp.name ?? "",
                            ScrollType = cfg.ScrollType,
                            GoSpeed = cfg.ContinuousScrollGoSpeed,
                            BackSpeed = cfg.ContinuousScrollBackSpeed
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Andon/configAndonCompleto: scroll type reparto {Reparto}", rp.id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: caricamento scroll type");
            }
        }

        private bool GuardFieldPermission()
        {
            if (!HasPermission("AndonCompleto CampiDaVisualizzare"))
            {
                Message = "Permesso non sufficiente.";
                return false;
            }
            return !string.IsNullOrEmpty(Tenant);
        }

        public IActionResult OnPostFieldAdd(string key)
            => FieldAction(cfg => cfg.addCampoVisualizzato(key), key, added: true);

        public IActionResult OnPostFieldDelete(string key)
            => FieldAction(cfg => cfg.deleteCampoVisualizzato(key), key, added: false);

        public IActionResult OnPostFieldUp(string key) => MoveField(key, -1);
        public IActionResult OnPostFieldDown(string key) => MoveField(key, +1);

        public IActionResult OnPostTaskAdd(string key)
            => TaskAction(cfg => cfg.addCampoVisualizzatoTasks(key), key, added: true);

        public IActionResult OnPostTaskDelete(string key)
            => TaskAction(cfg => cfg.deleteCampoVisualizzatoTasks(key), key, added: false);

        public IActionResult OnPostTaskUp(string key) => MoveTask(key, -1);
        public IActionResult OnPostTaskDown(string key) => MoveTask(key, +1);

        /// <summary>Legacy AndonConfigController.ScrollTypeEdit: params format "goSpeed;backSpeed", empty when disabled.</summary>
        public IActionResult OnPostScrollType(int scrollType, double? goSpeed, double? backSpeed)
        {
            if (!HasScrollTypePermission())
            {
                return RedirectToPage(new { handler = "" });
            }
            if (scrollType != 0 && scrollType != 1)
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var cfg = new AndonCompleto(Tenant!);
                var ret = cfg.setScrollType(scrollType, BuildScrollParams(scrollType, goSpeed, backSpeed));
                Message = ret == 1 ? "Scroll type aggiornato." : "Errore. " + cfg.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: salvataggio scroll type");
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { handler = "" });
        }

        /// <summary>Legacy AndonConfigController.DepartmentScrollTypeEdit.</summary>
        public IActionResult OnPostDepartmentScrollType(int departmentId, int scrollType, double? goSpeed, double? backSpeed)
        {
            if (!HasScrollTypePermission())
            {
                return RedirectToPage(new { handler = "" });
            }
            if (scrollType != 0 && scrollType != 1)
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var cfg = new AndonReparto(Tenant!, departmentId);
                var ret = cfg.setScrollType(scrollType, BuildScrollParams(scrollType, goSpeed, backSpeed));
                Message = ret == 1 ? "Scroll type del reparto aggiornato." : "Errore. " + cfg.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: salvataggio scroll type reparto {Reparto}", departmentId);
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { handler = "" });
        }

        private bool HasScrollTypePermission()
        {
            if (!HasPermission("AndonCompleto ScrollType"))
            {
                Message = "Permesso non sufficiente.";
                return false;
            }
            if (string.IsNullOrEmpty(Tenant))
            {
                return false;
            }
            return true;
        }

        private static string BuildScrollParams(int scrollType, double? goSpeed, double? backSpeed)
        {
            // Legacy ScrollTypeView.cshtml: txtScrollParams = go + ";" + back, empty when type 0.
            return scrollType == 1
                ? $"{(goSpeed ?? 0).ToString(System.Globalization.CultureInfo.InvariantCulture)};{(backSpeed ?? 0).ToString(System.Globalization.CultureInfo.InvariantCulture)}"
                : "";
        }

        /// <summary>configFormatoUsername.ascx.rbList_SelectedIndexChanged.</summary>
        public IActionResult OnPostUsernameFormat(char fmt)
        {
            if (!HasPermission("AndonCompleto VisualizzazioneNomiUtente"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { handler = "" });
            }
            if (fmt < '0' || fmt > '3')
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                new AndonCompleto(Tenant!).PostazioniFormatoUsername = fmt;
                Message = "Formato username aggiornato.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: salvataggio formato username");
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { handler = "" });
        }

        private IActionResult FieldAction(Func<AndonCompleto, bool> action, string key, bool added)
        {
            if (!GuardFieldPermission())
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var cfg = new AndonCompleto(Tenant!);
                var rt = action(cfg);
                Message = rt ? $"{key} {(added ? "aggiunto" : "eliminato")}." : "Errore. " + cfg.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: azione campo {Key}", key);
                Message = "Errore. " + ex.Message;
            }
            return RedirectToPage(new { handler = "" });
        }

        private IActionResult TaskAction(Func<AndonCompleto, bool> action, string key, bool added)
        {
            if (!GuardFieldPermission())
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var cfg = new AndonCompleto(Tenant!);
                var rt = action(cfg);
                Message = rt ? $"{key} {(added ? "aggiunto" : "eliminato")}." : "Errore. " + cfg.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: azione campo task {Key}", key);
                Message = "Errore. " + ex.Message;
            }
            return RedirectToPage(new { handler = "" });
        }

        /// <summary>Legacy rptFields_ItemCommand Up/Down: swap orders with the adjacent entry.</summary>
        private IActionResult MoveField(string key, int delta)
        {
            if (!GuardFieldPermission())
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var cfg = new AndonCompleto(Tenant!);
                cfg.loadCampiVisualizzati();
                var result = cfg.CampiVisualizzati.FirstOrDefault(x => x.Key == key);
                if (result.Key != null && result.Value + delta >= 0
                    && result.Value + delta <= cfg.CampiVisualizzati.Count - 1)
                {
                    var other = result.Value + delta;
                    var result2 = cfg.CampiVisualizzati.FirstOrDefault(y => y.Value == other);
                    if (result2.Key != null)
                    {
                        cfg.setOrdineCampoVisualizzato(result.Key, other);
                        cfg.setOrdineCampoVisualizzato(result2.Key, result.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: riordino campo {Key}", key);
                Message = "Errore. " + ex.Message;
            }
            return RedirectToPage(new { handler = "" });
        }

        private IActionResult MoveTask(string key, int delta)
        {
            if (!GuardFieldPermission())
            {
                return RedirectToPage(new { handler = "" });
            }
            try
            {
                var cfg = new AndonCompleto(Tenant!);
                cfg.loadCampiVisualizzatiTasks();
                var result = cfg.CampiVisualizzatiTasks.FirstOrDefault(x => x.Key == key);
                if (result.Key != null && result.Value + delta >= 0
                    && result.Value + delta <= cfg.CampiVisualizzatiTasks.Count - 1)
                {
                    var other = result.Value + delta;
                    var result2 = cfg.CampiVisualizzatiTasks.FirstOrDefault(y => y.Value == other);
                    if (result2.Key != null)
                    {
                        cfg.setOrdineCampoVisualizzatoTasks(result.Key, other);
                        cfg.setOrdineCampoVisualizzatoTasks(result2.Key, result.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Andon/configAndonCompleto: riordino campo task {Key}", key);
                Message = "Errore. " + ex.Message;
            }
            return RedirectToPage(new { handler = "" });
        }
    }
}
