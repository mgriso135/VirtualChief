using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Reparti
{
    /// <summary>
    /// Migrated from WebForms Reparti/configReparto.aspx (hub page embedding
    /// configCadenza, configSplitTasksTurni, configTurni, gestione personale,
    /// andon reparto config, …).
    /// Fully migrated: configCadenza, configSplitTasksTurni, configTurni,
    /// risorseTurno, configAndonReparto, configRepartoTimezone, configKanban,
    /// configModoCalcoloTC, calendario festivita/straordinari.
    /// </summary>
    public class configRepartoModel : PageModel
    {
        private readonly ILogger<configRepartoModel> _logger;

        public configRepartoModel(ILogger<configRepartoModel> logger)
        {
            _logger = logger;
        }

        public Reparto? Dipartimento { get; set; }

        public bool CanWrite { get; private set; }

        /// <summary>Holiday or overtime window of a shift (legacy Festivita/Straordinario).</summary>
        public class CalItem
        {
            public int Id { get; set; }
            public int TurnoId { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }

        public class ShiftCalendar
        {
            public int ShiftId { get; set; }
            public string ShiftName { get; set; } = "";
            public List<CalItem> Holidays { get; set; } = new();
            public List<CalItem> Overtimes { get; set; } = new();
        }

        /// <summary>Product with planned tasks inside a new holiday (legacy warning).</summary>
        public class AffectedProduct
        {
            public int ArticoloID { get; set; }
            public int ArticoloAnno { get; set; }
            public string Customer { get; set; } = "";
            public DateTime Delivery { get; set; }
        }

        // Legacy resx: addFestivita / addStraordinario
        public const string MsgErrorInput = "Errore nei dati di input";
        public const string MsgErrorOrario = "Errore: verifica gli orari inseriti.";
        public const string MsgErrorSovra =
            "Errore: probabilmente c'è una sovrapposizione tra lo straordinario inserito ed altri straordinari / festività, o i turni di lavoro. Verificare con il grafico nella pagina precedente.";
        public const string MsgWarnTasksPlanned =
            "Attenzione: durante la festività che stai inserendo è già pianificata l'esecuzione di alcuni tasks, appartenenti ai seguenti prodotti:";
        public const string MsgWarnTasksPlanned4 =
            "L'eventuale riprogrammazione dovrà essere fatta manualmente, per ogni prodotto. Vuoi continuare?";

        public bool CanViewCalendar { get; private set; }
        public bool CanWriteCalendar { get; private set; }
        public List<ShiftCalendar> ShiftCalendars { get; set; } = new();
        public List<AffectedProduct> PendingWarnProducts { get; set; } = new();

        // Section permissions (legacy names)
        public bool CanWriteRisorse { get; private set; }     // "Turno PostazioneRisorse" W
        public bool CanWriteAndon { get; private set; }       // "Reparto Andon VisualizzazioneNomiUtente" W
        public bool CanWriteTimezone { get; private set; }    // "Reparto Timezone" W
        public bool CanWriteKanban { get; private set; }      // "Reparto ConfigurazioneKanban" W
        public bool CanWriteModoCalcolo { get; private set; } // "Reparto ModoCalcoloTC" W

        /// <summary>Workstation resource count for one shift (legacy risorseTurno).</summary>
        public class ShiftResourceRow
        {
            public int ShiftId { get; set; }
            public string ShiftName { get; set; } = "";
            public List<ResourceRow> Workstations { get; set; } = new();
        }

        public class ResourceRow
        {
            public int PostazioneId { get; set; }
            public string PostazioneName { get; set; } = "";
            public int NumRisorse { get; set; }
        }

        public List<ShiftResourceRow> ShiftResources { get; set; } = new();

        // Andon reparto / timezone / kanban / modo calcolo
        public char AndonUsernameFormat { get; set; } = '0';
        public string DeptTimezone { get; set; } = "";
        public List<TimeZoneInfo> Timezones { get; set; } = new();
        public bool KanbanManaged { get; private set; }
        public bool ModoCalcoloTCEnabled { get; private set; }

        /// <summary>Production shift of the department (legacy Turno).</summary>
        public class TurnoItem
        {
            public int Id { get; set; }
            public string Nome { get; set; } = "";
            public string Colore { get; set; } = "";
        }

        // Legacy resx: configTurni.ascx
        public const string MsgErrorAddTurno = "Errore durante l'aggiunta del turno.";
        public const string MsgErrorDelTurno = "Errore durante l'eliminazione del turno.";

        public bool CanWriteTurni { get; private set; }
        public List<TurnoItem> Shifts { get; set; } = new();
        public List<(string Name, string Hex)> ColorChoices { get; set; } = new();

        [TempData]
        public string Message { get; set; }

        private string? Tenant => CurrentWorkspace.Of(User);

        /// <summary>Generic permission check with the legacy names/levels.</summary>
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
                _logger.LogError(ex, "Reparti/configReparto: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        private bool HasWritePermission() => HasPermission("Reparto", "W");

        public IActionResult OnGet(int id)
        {
            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            try
            {
                var rp = new Reparto(tenant, id);
                Dipartimento = rp.id != -1 ? rp : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: caricamento reparto {Id}", id);
                Dipartimento = null;
            }

            if (Dipartimento == null)
            {
                return NotFound();
            }

            CanWrite = HasWritePermission();
            CanWriteTurni = HasTurnoWritePermission();
            CanViewCalendar = HasPermission("Reparto Festivita", "R");
            CanWriteCalendar = HasPermission("Reparto Festivita", "W");
            CanWriteRisorse = HasPermission("Turno PostazioneRisorse", "W");
            CanWriteAndon = HasPermission("Reparto Andon VisualizzazioneNomiUtente", "W");
            CanWriteTimezone = HasPermission("Reparto Timezone", "W");
            CanWriteKanban = HasPermission("Reparto ConfigurazioneKanban", "W");
            CanWriteModoCalcolo = HasPermission("Reparto ModoCalcoloTC", "W");

            if (CanWriteTurni)
            {
                try
                {
                    var rep = new Reparto(Tenant!, id);
                    rep.loadTurni();
                    Shifts = rep.Turni?
                        .Select(t => new TurnoItem
                        {
                            Id = t.id,
                            Nome = t.Nome ?? "",
                            Colore = System.Drawing.ColorTranslator.ToHtml(t.Colore)
                        })
                        .ToList() ?? new List<TurnoItem>();

                    // Legacy: dropdown of all non-system KnownColors as "#RRGGBB".
                    foreach (string colorName in Enum.GetNames(typeof(System.Drawing.KnownColor)))
                    {
                        var c = System.Drawing.Color.FromName(colorName);
                        if (!c.IsSystemColor)
                        {
                            ColorChoices.Add((colorName, $"#{c.R:X2}{c.G:X2}{c.B:X2}"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Reparti/configReparto: caricamento turni reparto {Id}", id);
                }
            }

            if (CanViewCalendar)
            {
                LoadCalendar();
            }

            LoadSectionData(id);

            return Page();
        }

        /// <summary>Data for the andon-username / timezone / kanban / TC-mode sections
        /// and the per-shift workstation resources (legacy risorseTurno.ascx).</summary>
        private void LoadSectionData(int id)
        {
            var tenant = Tenant!;
            try
            {
                var rp = new Reparto(tenant, id);
                if (rp.id == -1)
                {
                    return;
                }

                KanbanManaged = rp.KanbanManaged;
                ModoCalcoloTCEnabled = rp.ModoCalcoloTC;
                DeptTimezone = rp.tzFusoOrario.Id;
                AndonUsernameFormat = rp.AndonPostazioniFormatoUsername;

                if (CanWriteTimezone)
                {
                    Timezones = TimeZoneInfo.GetSystemTimeZones().ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: caricamento dati sezioni reparto {Id}", id);
            }

            if (CanWriteRisorse)
            {
                try
                {
                    // Shifts are loaded with the "Turno" permission; resources need
                    // their own gate, so reload them here if not available yet.
                    if (Shifts.Count == 0)
                    {
                        var rep = new Reparto(tenant, id);
                        rep.loadTurni();
                        Shifts = rep.Turni?
                            .Select(t => new TurnoItem { Id = t.id, Nome = t.Nome ?? "", Colore = System.Drawing.ColorTranslator.ToHtml(t.Colore) })
                            .ToList() ?? new List<TurnoItem>();
                    }

                    var postazioni = new ElencoPostazioni(tenant).elenco?
                        .OrderBy(p => p.name).ToList() ?? new List<Postazione>();

                    foreach (var shift in Shifts)
                    {
                        var turno = new Turno(tenant, shift.Id);
                        if (turno.id == -1) continue;

                        var row = new ShiftResourceRow { ShiftId = shift.Id, ShiftName = shift.Nome };
                        foreach (var pst in postazioni)
                        {
                            var res = new RisorsePostazioneTurno(tenant, pst, turno);
                            if (res.postazione != null && res.postazione.id != -1
                                && res.turno != null && res.turno.id != -1)
                            {
                                row.Workstations.Add(new ResourceRow
                                {
                                    PostazioneId = pst.id,
                                    PostazioneName = pst.name ?? "",
                                    NumRisorse = res.NumRisorse
                                });
                            }
                        }
                        if (row.Workstations.Count > 0)
                        {
                            ShiftResources.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Reparti/configReparto: caricamento risorse turno reparto {Id}", id);
                }
            }
        }

        /// <summary>Legacy elencoFestivita/elencoStraordinari.ascx: per-shift holidays and overtime.</summary>
        private void LoadCalendar()
        {
            try
            {
                var rep = new Reparto(Tenant!, Dipartimento!.id);
                rep.loadTurni();
                foreach (var t in rep.Turni ?? new List<Turno>())
                {
                    var sc = new ShiftCalendar { ShiftId = t.id, ShiftName = t.Nome ?? "" };
                    t.loadFestivita();
                    foreach (var f in t.festivita ?? new List<Festivita>())
                    {
                        sc.Holidays.Add(new CalItem { Id = f.idFestivita, TurnoId = f.idTurno, Start = f.Inizio, End = f.Fine });
                    }
                    t.loadStraordinari();
                    foreach (var s in t.straordinari ?? new List<Straordinario>())
                    {
                        sc.Overtimes.Add(new CalItem { Id = s.idStraordinario, TurnoId = s.idTurno, Start = s.Inizio, End = s.Fine });
                    }
                    ShiftCalendars.Add(sc);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: caricamento calendario festività/straordinari");
            }
        }

        /// <summary>
        /// addFestivita.ascx.saveFest_Click + imgSave_Click: validates the window,
        /// warns about products with planned tasks inside it, then adds through
        /// ElencoFestivita.Add (overlap-checked).
        /// </summary>
        public IActionResult OnPostAddFest(int id, int turnoId, DateTime inizio, DateTime fine, bool confirmed = false)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasPermission("Reparto Festivita", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }

            try
            {
                var rp = new Reparto(tenant!, id);
                if (inizio >= fine || inizio <= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario))
                {
                    Message = MsgErrorInput;
                    return RedirectToPage(new { id, handler = "" });
                }

                if (!confirmed)
                {
                    // Legacy first step: warn about tasks already planned in the window.
                    var lstTasks = new List<TaskProduzione>();
                    foreach (var status in new[] { 'I', 'N', 'P' })
                    {
                        lstTasks.AddRange(new ElencoTaskProduzione(tenant!, inizio, fine, status).Tasks);
                    }
                    PendingWarnProducts = lstTasks
                        .GroupBy(t => new { t.ArticoloID, t.ArticoloAnno })
                        .Select(g => g.Key)
                        .Select(k =>
                        {
                            var art = new Articolo(tenant!, k.ArticoloID, k.ArticoloAnno);
                            return new AffectedProduct
                            {
                                ArticoloID = art.ID,
                                ArticoloAnno = art.Year,
                                Customer = art.RagioneSocialeCliente ?? "",
                                Delivery = art.DataPrevistaConsegna
                            };
                        })
                        .ToList();
                    if (PendingWarnProducts.Count > 0)
                    {
                        return Page(); // warning panel with confirm button rendered by the view
                    }
                }

                var elenco = new ElencoFestivita(tenant!, id);
                bool ret = elenco.Add(turnoId, inizio, fine);
                Message = ret ? "Festività aggiunta." : MsgErrorOrario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: aggiunta festività turno {Turno}", turnoId);
                Message = MsgErrorInput;
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>addStraordinario.ascx.saveStraord_Click.</summary>
        public IActionResult OnPostAddStraord(int id, int turnoId, DateTime inizio, DateTime fine)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasPermission("Reparto Festivita", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }

            try
            {
                var rp = new Reparto(tenant!, id);
                if (inizio >= fine || TimeZoneInfo.ConvertTimeToUtc(inizio, rp.tzFusoOrario) <= DateTime.UtcNow)
                {
                    Message = MsgErrorInput;
                    return RedirectToPage(new { id, handler = "" });
                }

                var elenco = new ElencoStraordinari(tenant!, id);
                bool ret = elenco.Add(turnoId, inizio, fine);
                Message = ret ? "Straordinario aggiunto." : MsgErrorSovra;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: aggiunta straordinario turno {Turno}", turnoId);
                Message = MsgErrorInput;
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>elencoFestivita.ascx delete command.</summary>
        public IActionResult OnPostDeleteFest(int id, int festId)
        {

            if (!HasPermission("Reparto Festivita", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var fs = new Festivita(Tenant!, festId);
                Message = fs.idFestivita != -1 && fs.delete() ? "Festività eliminata." : "Errore. " + fs.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: eliminazione festività {Id}", festId);
                Message = "Errore durante l'eliminazione.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>elencoStraordinari.ascx delete command.</summary>
        public IActionResult OnPostDeleteStraord(int id, int straordId)
        {
            if (!HasPermission("Reparto Festivita", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var st = new Straordinario(Tenant!, straordId);
                Message = st.idStraordinario != -1 && st.delete() ? "Straordinario eliminato." : "Errore. " + st.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: eliminazione straordinario {Id}", straordId);
                Message = "Errore durante l'eliminazione.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>risorseTurno.ascx txtRisourse_TextChanged: set workstation resources for a shift.</summary>
        public IActionResult OnPostSetResource(int id, int turnoId, int postazioneId, int numRisorse)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasPermission("Turno PostazioneRisorse", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (numRisorse < 0)
            {
                return RedirectToPage(new { id, handler = "" });
            }

            try
            {
                var pst = new Postazione(tenant!, postazioneId);
                var turno = new Turno(tenant!, turnoId);
                if (pst.id == -1 || turno.id == -1)
                {
                    Message = "Postazione o turno non trovati.";
                    return RedirectToPage(new { id, handler = "" });
                }
                var res = new RisorsePostazioneTurno(tenant!, pst, turno)
                {
                    NumRisorse = numRisorse
                };
                Message = $"Risorse impostate ({res.NumRisorse}) per {pst.name} sul turno {turno.Nome}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: risorse turno {Turno} postazione {Postazione}", turnoId, postazioneId);
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configAndonReparto.ascx.rbList_SelectedIndexChanged.</summary>
        public IActionResult OnPostAndonFormat(int id, char fmt)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasPermission("Reparto Andon VisualizzazioneNomiUtente", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (fmt < '0' || fmt > '3')
            {
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var rp = new Reparto(tenant!, id);
                rp.AndonPostazioniFormatoUsername = fmt;
                Message = $"Formato username andon impostato su {fmt}. {rp.log}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: salvataggio formato username andon");
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configRepartoTimezone.ascx.ddlTimezones_SelectedIndexChanged.</summary>
        public IActionResult OnPostTimezone(int id, string timezone)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasPermission("Reparto Timezone", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(timezone);
                var rp = new Reparto(tenant!, id);
                rp.fusoOrario = timezone;
                Message = $"Fuso orario del reparto impostato su {timezone}. {rp.log}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: salvataggio timezone reparto");
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configKanban.ascx.rb1_SelectedIndexChanged.</summary>
        public IActionResult OnPostKanban(int id, string kanban)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasPermission("Reparto ConfigurazioneKanban", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (kanban != "0" && kanban != "1")
            {
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var rp = new Reparto(tenant!, id);
                rp.KanbanManaged = kanban == "1";
                Message = $"Gestione kanban {(rp.KanbanManaged ? "abilitata" : "disabilitata")}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: salvataggio kanban");
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configModoCalcoloTC.ascx.rbList_SelectedIndexChanged.</summary>
        public IActionResult OnPostModoCalcolo(int id, string modo)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasPermission("Reparto ModoCalcoloTC", "W"))
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (modo != "0" && modo != "1")
            {
                return RedirectToPage(new { id, handler = "" });
            }
            try
            {
                var rp = new Reparto(tenant!, id);
                rp.ModoCalcoloTC = modo == "1";
                Message = rp.ModoCalcoloTC == (modo == "1")
                    ? "Modo di calcolo del tempo ciclico aggiornato."
                    : "Errore durante il salvataggio. " + rp.log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: salvataggio modo calcolo TC");
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configTurni.ascx permission: "Turno" W.</summary>
        private bool HasTurnoWritePermission()
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
                _logger.LogError(ex, "Reparti/configReparto: verifica permesso Turno");
                return false;
            }
        }

        /// <summary>configTurni.ascx.save_Click: rep.addTurno(nome, colore).</summary>
        public IActionResult OnPostAddTurno(int id, string nomeTurno, string coloreTurno)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasTurnoWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (string.IsNullOrWhiteSpace(nomeTurno) || string.IsNullOrEmpty(coloreTurno))
            {
                Message = MsgErrorAddTurno;
                return RedirectToPage(new { id, handler = "" });
            }

            try
            {
                var rep = new Reparto(tenant!, id);
                if (rep.id == -1)
                {
                    Message = "Reparto non trovato.";
                    return RedirectToPage(new { id, handler = "" });
                }
                bool rt = rep.addTurno(
                    System.Net.WebUtility.HtmlEncode(nomeTurno),
                    System.Net.WebUtility.HtmlEncode(coloreTurno));
                Message = rt ? $"Turno '{nomeTurno}' aggiunto." : MsgErrorAddTurno;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: aggiunta turno su reparto {Id}", id);
                Message = MsgErrorAddTurno;
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configTurni.ascx.rptTurni_ItemCommand (delete).</summary>
        public IActionResult OnPostDeleteTurno(int id, int turnoId)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasTurnoWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }

            try
            {
                var rep = new Reparto(tenant!, id);
                if (rep.id == -1)
                {
                    Message = "Reparto non trovato.";
                    return RedirectToPage(new { id, handler = "" });
                }
                bool rt = rep.deleteTurno(new Turno(tenant!, turnoId));
                Message = rt ? "Turno eliminato." : MsgErrorDelTurno;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: eliminazione turno {Turno} su reparto {Id}", turnoId, id);
                Message = MsgErrorDelTurno;
            }
            return RedirectToPage(new { id, handler = "" });
        }

        /// <summary>configCadenza.ascx.save_Click: rep.Cadenza = new TimeSpan(h, m, s).</summary>
        public IActionResult OnPostCadenza(int id, int ore, int minuti, int secondi)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (ore < 0 || minuti < 0 || secondi < 0 || minuti > 59 || secondi > 59)
            {
                Message = "Valori non validi: ore ≥ 0, minuti e secondi tra 0 e 59.";
                return RedirectToPage(new { id, handler = "" });
            }

            try
            {
                var rp = new Reparto(tenant!, id);
                if (rp.id == -1)
                {
                    return NotFound();
                }
                rp.Cadenza = new TimeSpan(ore, minuti, secondi);
                Message = $"Cadenza aggiornata: {ore:00}:{minuti:00}:{secondi:00}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: salvataggio cadenza reparto {Id}", id);
                Message = "Errore durante il salvataggio della cadenza.";
            }
            return RedirectToPage(new { id, handler = "" });
        }
        /// <summary>configSplitTasksTurni.ascx.splitTasks_SelectedIndexChanged.</summary>
        public IActionResult OnPostSplitTasks(int id, string splitTasks)
        {
            var tenant = CurrentWorkspace.Of(User);

            if (!HasWritePermission())
            {
                Message = "Permesso non sufficiente.";
                return RedirectToPage(new { id, handler = "" });
            }
            if (splitTasks != "0" && splitTasks != "1")
            {
                return RedirectToPage(new { id, handler = "" });
            }

            try
            {
                var rp = new Reparto(tenant!, id);
                if (rp.id == -1)
                {
                    return NotFound();
                }
                rp.splitTasks = splitTasks == "1";
                Message = $"Split tasks sui turni: {(rp.splitTasks ? "abilitato" : "disabilitato")}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/configReparto: salvataggio split tasks reparto {Id}", id);
                Message = "Errore durante il salvataggio.";
            }
            return RedirectToPage(new { id, handler = "" });
        }
    }
}
