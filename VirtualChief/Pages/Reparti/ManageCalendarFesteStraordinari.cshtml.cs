using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Reparti
{
    public class ManageCalendarFesteStraordinariModel : PageModel
    {
        private readonly ILogger<ManageCalendarFesteStraordinariModel> _logger;

        public ManageCalendarFesteStraordinariModel(ILogger<ManageCalendarFesteStraordinariModel> logger)
        {
            _logger = logger;
        }

        public Reparto? Dipartimento { get; private set; }
        public Turno? TurnoCorrente { get; private set; }
        public DateTime InizioCalendario { get; private set; }
        public DateTime FineCalendario { get; private set; }
        public bool CanView { get; private set; }
        public string ErrorMessage { get; private set; } = "";

        public class CalendarSeries
        {
            public string Name { get; set; } = "";
            public string Color { get; set; } = "";
            public List<CalendarPoint> Points { get; set; } = new();
        }

        public class CalendarPoint
        {
            public DateTime Date { get; set; }
            public double Y { get; set; }
            public string Tooltip { get; set; } = "";
        }

        public List<CalendarSeries> Series { get; private set; } = new();

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
                _logger.LogError(ex, "Reparti/ManageCalendarFesteStraordinari: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public IActionResult OnGet(int id, DateTime? start, DateTime? end)
        {
            var tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            if (!HasPermission("Reparto Festivita", "R"))
            {
                ErrorMessage = "Non hai il permesso di visualizzare il calendario.";
                return Page();
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

                Dipartimento = rp;
                TurnoCorrente = trn;
                CanView = true;

                InizioCalendario = start ?? DateTime.Now;
                FineCalendario = end ?? DateTime.Now.AddDays(7);

                trn.loadCalendario(InizioCalendario, FineCalendario);
                LoadCalendarData(trn, InizioCalendario, FineCalendario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/ManageCalendarFesteStraordinari: caricamento calendario turno {Id}", id);
                ErrorMessage = "Errore durante il caricamento del calendario.";
            }

            return Page();
        }

        private void LoadCalendarData(Turno trn, DateTime inizioCal, DateTime fineCal)
        {
            Series.Clear();

            var rep = new Reparto(Tenant!, Dipartimento!.id);
            rep.loadTurni();

            foreach (var turno in trn.CalendarioTrn.Turni)
            {
                if (turno.Inizio >= inizioCal && turno.Fine <= fineCal)
                {
                    IntervalloLavorativoTurno intTr = new IntervalloLavorativoTurno(Tenant!, turno.idOrarioTurno);
                    Turno tr = new Turno(Tenant!, intTr.idTurno);
                    
                    var series = new CalendarSeries
                    {
                        Name = $"Turno {turno.idOrarioTurno}",
                        Color = System.Drawing.ColorTranslator.ToHtml(tr.Colore),
                    };
                    
                    series.Points.Add(new CalendarPoint
                    {
                        Date = turno.Inizio,
                        Y = 4,
                        Tooltip = $"{tr.Nome} inizio: {turno.Inizio:dd/MM/yyyy HH:mm:ss}"
                    });
                    series.Points.Add(new CalendarPoint
                    {
                        Date = turno.Fine,
                        Y = 4,
                        Tooltip = $"{tr.Nome} fine: {turno.Fine:dd/MM/yyyy HH:mm:ss}"
                    });
                    
                    Series.Add(series);
                }
            }

            trn.loadStraordinari();
            foreach (var straord in trn.straordinari ?? new List<Straordinario>())
            {
                if (straord.Inizio >= inizioCal && straord.Inizio <= fineCal)
                {
                    var series = new CalendarSeries
                    {
                        Name = $"Straordinario {straord.idStraordinario}",
                        Color = "#28a745",
                    };
                    
                    series.Points.Add(new CalendarPoint
                    {
                        Date = straord.Inizio,
                        Y = 3,
                        Tooltip = $"Straordinario inizio: {straord.Inizio:dd/MM/yyyy HH:mm:ss}"
                    });
                    
                    var fine = straord.Fine > fineCal ? fineCal : straord.Fine;
                    series.Points.Add(new CalendarPoint
                    {
                        Date = fine,
                        Y = 3,
                        Tooltip = $"Straordinario fine: {fine:dd/MM/yyyy HH:mm:ss}"
                    });
                    
                    Series.Add(series);
                }
            }

            trn.loadFestivita();
            foreach (var fest in trn.festivita ?? new List<Festivita>())
            {
                if (fest.Inizio >= inizioCal && fest.Inizio <= fineCal)
                {
                    var series = new CalendarSeries
                    {
                        Name = $"Festivit\u00e0 {fest.idFestivita}",
                        Color = "#dc3545",
                    };
                    
                    series.Points.Add(new CalendarPoint
                    {
                        Date = fest.Inizio,
                        Y = 2,
                        Tooltip = $"Festivit\u00e0 inizio: {fest.Inizio:dd/MM/yyyy HH:mm:ss}"
                    });
                    
                    var fine = fest.Fine > fineCal ? fineCal : fest.Fine;
                    series.Points.Add(new CalendarPoint
                    {
                        Date = fine,
                        Y = 2,
                        Tooltip = $"Festivit\u00e0 fine: {fine:dd/MM/yyyy HH:mm:ss}"
                    });
                    
                    Series.Add(series);
                }
            }

            for (int i = 0; i < trn.CalendarioTrn.Intervalli.Count; i++)
            {
                var intervallo = trn.CalendarioTrn.Intervalli[i];
                if (intervallo.Inizio >= inizioCal && intervallo.Inizio <= fineCal)
                {
                    var series = new CalendarSeries
                    {
                        Name = $"Intervallo {i + 1}",
                        Color = "#adff2f",
                    };
                    
                    series.Points.Add(new CalendarPoint
                    {
                        Date = intervallo.Inizio,
                        Y = 1,
                        Tooltip = $"Intervallo inizio: {intervallo.Inizio:dd/MM/yyyy HH:mm:ss}"
                    });
                    
                    var fine = intervallo.Fine > fineCal ? fineCal : intervallo.Fine;
                    series.Points.Add(new CalendarPoint
                    {
                        Date = fine,
                        Y = 1,
                        Tooltip = $"Intervallo fine: {fine:dd/MM/yyyy HH:mm:ss}"
                    });
                    
                    Series.Add(series);
                }
            }
        }
    }
}