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
    /// Migrated from WebForms Reparti/configOrariTurno.aspx + configOrariTrn.ascx:
    /// working hours of a production shift — rename/recolor the shift, list,
    /// add and delete work intervals (Turno.AddOrario / IntervalloLavorativoTurno.Delete).
    /// Permission: "Turno" W.
    /// </summary>
    public class configOrariTurnoModel : PageModel
    {
        private readonly ILogger<configOrariTurnoModel> _logger;

        public configOrariTurnoModel(ILogger<configOrariTurnoModel> logger)
        {
            _logger = logger;
        }

        public class IntervalItem
        {
            public int Id { get; set; }
            public DayOfWeek GiornoInizio { get; set; }
            public TimeSpan OraInizio { get; set; }
            public DayOfWeek GiornoFine { get; set; }
            public TimeSpan OraFine { get; set; }
        }

        // Legacy resx: configOrariTrn.ascx lblErrorAdd
        public const string MsgErrorAddOrario = "Errore durante l'aggiunta dell'orario di lavoro.";

        public static readonly string[] DayNames =
        {
            "Domenica", "Luned&igrave;", "Marted&igrave;", "Mercoled&igrave;",
            "Gioved&igrave;", "Venerd&igrave;", "Sabato"
        };

        public bool CanWrite { get; private set; }

        public int TurnoId { get; private set; } = -1;
        public string Nome { get; set; } = "";
        public string ColoreHex { get; set; } = "";
        public List<IntervalItem> Orari { get; set; } = new();
        public List<(string Name, string Hex)> ColorChoices { get; set; } = new();

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
                var prm = new List<string[]> { new[] { "Turno", "W" } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configOrariTurno: verifica permesso Turno");
                return false;
            }
        }

        private void LoadShift(int id)
        {
            var tr = new Turno(Tenant!, id);
            if (tr.id == -1)
            {
                TurnoId = -1;
                return;
            }
            TurnoId = tr.id;
            Nome = tr.Nome ?? "";
            ColoreHex = System.Drawing.ColorTranslator.ToHtml(tr.Colore);
            Orari = tr.OrariDiLavoro?
                .Select(o => new IntervalItem
                {
                    Id = o.idIntervallo,
                    GiornoInizio = o.GiornoInizio,
                    OraInizio = o.OraInizio,
                    GiornoFine = o.GiornoFine,
                    OraFine = o.OraFine
                })
                .ToList() ?? new List<IntervalItem>();

            ColorChoices.Clear();
            foreach (string colorName in Enum.GetNames(typeof(System.Drawing.KnownColor)))
            {
                var c = System.Drawing.Color.FromName(colorName);
                if (!c.IsSystemColor)
                {
                    ColorChoices.Add((colorName, $"#{c.R:X2}{c.G:X2}{c.B:X2}"));
                }
            }
        }

        public IActionResult OnGet(int id)
        {
            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            CanWrite = HasWritePermission();
            if (!CanWrite)
            {
                Message = "Permesso \"Turno\" (modifica) non disponibile.";
                return Page();
            }

            LoadShift(id);
            if (TurnoId == -1)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>configOrariTrn.ascx.saveNomeTurno_Click.</summary>
        public IActionResult OnPostSaveTurno(int id, string nomeTurno, string coloreTurno)
        {
            if (!HasWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (string.IsNullOrWhiteSpace(nomeTurno) || string.IsNullOrEmpty(coloreTurno))
            {
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var t = new Turno(Tenant!, id);
                if (t.id == -1)
                {
                    return NotFound();
                }
                t.Nome = System.Net.WebUtility.HtmlEncode(nomeTurno);
                t.Colore = System.Drawing.ColorTranslator.FromHtml(coloreTurno);
                Message = $"Turno aggiornato: {nomeTurno}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configOrariTurno: salvataggio turno {Id}", id);
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configOrariTrn.ascx.saveOrario_Click.</summary>
        public IActionResult OnPostAddOrario(int id, int dInizio, int oInizioH, int oInizioM,
            int dFine, int oFineH, int oFineM)
        {
            if (!HasWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (dInizio < 0 || dInizio > 6 || dFine < 0 || dFine > 6
                || oInizioM < 0 || oInizioM > 59 || oFineM < 0 || oFineM > 59
                || oInizioH < 0 || oInizioH > 23 || oFineH < 0 || oFineH > 23)
            {
                Message = MsgErrorAddOrario;
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var t = new Turno(Tenant!, id);
                if (t.id == -1)
                {
                    return NotFound();
                }
                bool rt = t.AddOrario(
                    (DayOfWeek)dInizio,
                    new TimeSpan(oInizioH, oInizioM, 0),
                    (DayOfWeek)dFine,
                    new TimeSpan(oFineH, oFineM, 0));
                Message = rt ? "Orario di lavoro aggiunto." : MsgErrorAddOrario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configOrariTurno: aggiunta orario turno {Id}", id);
                Message = MsgErrorAddOrario;
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configOrariTrn.ascx.rptOrari_ItemCommand (delete).</summary>
        public IActionResult OnPostDeleteOrario(int id, int intervalloId)
        {
            if (!HasWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var interv = new IntervalloLavorativoTurno(Tenant!, intervalloId);
                bool rt = interv.Delete();
                Message = rt ? "Orario eliminato." : "Errore. " + interv.err;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configOrariTurno: eliminazione orario {Intervallo}", intervalloId);
                Message = "Errore durante l'eliminazione.";
            }
            return RedirectToPage(new { id, handler = "" });
        }
    }
}
