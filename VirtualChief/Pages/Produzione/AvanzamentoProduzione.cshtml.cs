using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    public class AvanzamentoProduzioneModel : PageModel
    {
        private readonly ILogger<AvanzamentoProduzioneModel> _logger;

        public AvanzamentoProduzioneModel(ILogger<AvanzamentoProduzioneModel> logger)
        {
            _logger = logger;
        }

        public string Tenant { get; private set; }
        public string NavProduzione { get; private set; } = "Produzione";
        public string NavAndonCompleto { get; private set; } = "Avanzamento Produzione Completo";
        public string TitleArticoliDaAvviare { get; private set; } = "Articoli da Avviare";
        public bool HasPermission { get; private set; } = false;
        public string ErrorMessage { get; private set; } = "";

        // For ArticoliDaAvviare
        public List<Articolo> ArticoliDaAvviare { get; private set; } = new();
        public List<string> VisibleFields { get; private set; } = new();
        public bool IsLoading { get; private set; } = false;
        public string LoadingMessage { get; set; } = "Caricamento...";
        public string NoProductsMessage { get; set; } = "Nessun articolo da avviare";
        public string PermissionDeniedMessage { get; set; } = "Non hai il permesso di visualizzare gli articoli da avviare";
        public string ViewArticoloTooltip { get; set; } = "Visualizza avanzamento";

        public void OnGet()
        {
            Tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(Tenant))
            {
                Response.Redirect("/Login/selectWorkspace");
                return;
            }

            try
            {
                var elencoPermessi = new List<string[]> { new[] { "Articoli", "R" } };
                var uidStr = User.FindFirst("uid")?.Value;
                if (int.TryParse(uidStr, out var uid))
                {
                    var user = new UserAccount(uid);
                    user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
                    HasPermission = user.ValidatePermissions(Tenant, elencoPermessi);

                    if (HasPermission)
                    {
                        LoadArticoliDaAvviare();
                    }
                }
                else
                {
                    ErrorMessage = "Utente non autenticato.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AvanzamentoProduzione/OnGet");
                ErrorMessage = "Errore generico.";
            }
        }

        private void LoadArticoliDaAvviare()
        {
            try
            {
                var andonCfg = new AndonCompleto(Tenant);
                andonCfg.loadCampiVisualizzati();
                VisibleFields = andonCfg.CampiVisualizzati?.Keys?.ToList() ?? new List<string>();

                ArticoliDaAvviare = AvanzamentoProduzioneService.CaricaArticoliNonPianificati(Tenant);
                ArticoliDaAvviare.Sort((p1, p2) => p1.LateStart.CompareTo(p2.LateStart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading articoli da avviare");
                ArticoliDaAvviare = new List<Articolo>();
                ErrorMessage = "Errore durante il caricamento";
            }
        }

        public string GetRowClass(Articolo art)
        {
            if (art.Status == 'N')
            {
                return "table-light";
            }
            else if (art.Status == 'P')
            {
                var stato = AvanzamentoProduzioneService.ClassificaStato(art, DateTime.Now);
                return stato switch
                {
                    KIS.App_Sources.StatoAvanzamentoArticolo.NonIniziato => "table-success",
                    KIS.App_Sources.StatoAvanzamentoArticolo.InRitardo => "table-danger",
                    KIS.App_Sources.StatoAvanzamentoArticolo.InCorso => "table-warning",
                    _ => "table-light"
                };
            }
            return "";
        }

        public string GetFieldValue(Articolo art, string field)
        {
            try
            {
                return field switch
                {
                    "CommessaID" => $"{art.Commessa}/{art.AnnoCommessa}",
                    "OrderExternalID" => new Commessa(Tenant, art.Commessa, art.AnnoCommessa).ExternalID,
                    "CommessaCodiceCliente" => art.Cliente,
                    "CommessaRagioneSocialeCliente" => art.RagioneSocialeCliente,
                    "CommessaDataInserimento" => new Commessa(Tenant, art.Commessa, art.AnnoCommessa).DataInserimento.ToString("dd/MM/yyyy"),
                    "CommessaNote" => new Commessa(Tenant, art.Commessa, art.AnnoCommessa).Note,
                    "ProdottoID" => $"{art.ID}/{art.Year}",
                    "ProdottoLineaProdotto" => art.Proc?.process?.processName,
                    "ProdottoNomeProdotto" => art.Proc?.variant?.nomeVariante,
                    "ProdottoMatricola" => art.Matricola,
                    "ProdottoStatus" => art.Status.ToString(),
                    "Reparto" => art.RepartoNome,
                    "DataPrevistaConsegna" => art.DataPrevistaConsegna.ToString("dd/MM/yyyy"),
                    "DataPrevistaFineProduzione" => art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy"),
                    "EarlyStart" => art.EarlyStart.ToString("dd/MM/yyyy HH:mm:ss"),
                    "LateStart" => art.LateStart.ToString("dd/MM/yyyy HH:mm:ss"),
                    "EarlyFinish" => art.EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss"),
                    "LateFinish" => art.LateFinish.ToString("dd/MM/yyyy HH:mm:ss"),
                    "ProdottoQuantita" => art.Quantita.ToString(),
                    "ProdottoQuantitaRealizzata" => art.QuantitaProdotta.ToString(),
                    "ProdottoRitardo" => Math.Round(art.Ritardo.TotalHours, 2).ToString(),
                    "ProdottoTempodiLavoroTotale" => GetTempoDiLavoroTotale(art),
                    "ProdottoIndicatoreCompletamentoTasks" => Math.Round(art.IndicatoreCompletamentoTasks, 1).ToString() + "%",
                    "ProdottoIndicatoreCompletamentoTempoPrevisto" => Math.Round(art.IndicatoreCompletamentoTempoPrevisto, 1).ToString() + "%",
                    "ProductExternalID" => art.Proc?.ExternalID?.ToString() ?? "",
                    "MeasurementUnit" => GetMeasurementUnit(art),
                    _ => ""
                };
            }
            catch
            {
                return "#ERROR";
            }
        }

        private string GetTempoDiLavoroTotale(Articolo art)
        {
            art.loadTempoDiLavoroTotale();
            return Math.Round(art.TempoDiLavoroTotale.TotalHours, 2).ToString();
        }

        private string GetMeasurementUnit(Articolo art)
        {
            art.Proc?.loadMeasurementUnit();
            return art.Proc?.measurementUnit?.Type?.ToString() ?? "";
        }
    }
}