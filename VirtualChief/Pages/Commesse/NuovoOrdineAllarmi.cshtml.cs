using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Commesse
{
    /// <summary>
    /// Migrated from WebForms Commesse/wzImpostaAllarmiArticolo.aspx.
    /// Final step of new order wizard: configure delay and warning alarms for the article.
    /// Permissions: "Articolo EventoRitardo" W, "Articolo EventoWarning" W
    /// </summary>
    public class NuovoOrdineAllarmiModel : PageModel
    {
        private readonly ILogger<NuovoOrdineAllarmiModel> _logger;

        public NuovoOrdineAllarmiModel(ILogger<NuovoOrdineAllarmiModel> logger)
        {
            _logger = logger;
        }

        // Navigation labels
        public string NavCommesse { get; private set; } = "Commesse";
        public string NavNuovoOrdine { get; private set; } = "Nuovo Ordine";
        public string NavProdotti { get; private set; } = "Prodotti";
        public string NavReparto { get; private set; } = "Reparto";
        public string NavTasks { get; private set; } = "Tasks";
        public string NavDataConsegna { get; private set; } = "Data Consegna";
        public string NavCaricoLavoro { get; private set; } = "Verifica Carico";
        public string NavDataFineProduzione { get; private set; } = "Data Fine Produzione";
        public string NavAllarmi { get; private set; } = "Imposta Allarmi";

        // UI Labels
        public string InfoPanelTitle { get; private set; } = "Riepilogo Ordine";
        public string AlarmsTitle { get; private set; } = "Configurazione Allarmi";
        public string ShowAlarmsLabel { get; private set; } = "Mostra Allarmi";
        public string HideAlarmsLabel { get; private set; } = "Nascondi Allarmi";
        public string NewProductLabel { get; private set; } = "Nuovo Prodotto";
        public string BackLabel { get; private set; } = "Indietro";
        public string PermissionDeniedMessage { get; private set; } = "Non hai il permesso di configurare gli allarmi.";

        // State
        public bool HasPermission { get; private set; }
        public bool CanConfigDelay { get; private set; }
        public bool CanConfigWarning { get; private set; }
        public Articolo Articolo { get; private set; }
        public string Tenant => CurrentWorkspace.Of(User);

        // Query parameters
        public int IdCommessa { get; set; }
        public int AnnoCommessa { get; set; }
        public int IdProc { get; set; }
        public int RevProc { get; set; }
        public int IdVariante { get; set; }
        public int IdReparto { get; set; }
        public int IdProdotto { get; set; }
        public int AnnoProdotto { get; set; }
        public int Quantita { get; set; }

        public bool ShowAlarms { get; set; } = false;

        public string BackUrl => $"/Commesse/NuovoOrdineDataFineProduzione?idCommessa={IdCommessa}&annoCommessa={AnnoCommessa}&idProc={IdProc}&revProc={RevProc}&idVariante={IdVariante}&idReparto={IdReparto}&idProdotto={IdProdotto}&annoProdotto={AnnoProdotto}&quantita={Quantita}";

        public string NewProductUrl => $"/Commesse/NuovoOrdineProdotti?idCommessa={IdCommessa}&annoCommessa={AnnoCommessa}";

        private bool HasPermissionCheck(string nomePermesso, string level)
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
                _logger.LogError(ex, "Commesse/NuovoOrdineAllarmi: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public void OnGet(int idCommessa, int annoCommessa, int idProc, int revProc, int idVariante, int idReparto, int idProdotto, int annoProdotto)
        {
            IdCommessa = idCommessa;
            AnnoCommessa = annoCommessa;
            IdProc = idProc;
            RevProc = revProc;
            IdVariante = idVariante;
            IdReparto = idReparto;
            IdProdotto = idProdotto;
            AnnoProdotto = annoProdotto;

            CanConfigDelay = HasPermissionCheck("Articolo EventoRitardo", "W");
            CanConfigWarning = HasPermissionCheck("Articolo EventoWarning", "W");
            HasPermission = CanConfigDelay || CanConfigWarning;

            var tenant = Tenant;
            if (string.IsNullOrEmpty(tenant) || !HasPermission)
            {
                return;
            }

            try
            {
                Articolo = new Articolo(tenant, IdProdotto, AnnoProdotto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineAllarmi: caricamento articolo {IdProdotto}/{AnnoProdotto}", IdProdotto, AnnoProdotto);
            }
        }
    }
}