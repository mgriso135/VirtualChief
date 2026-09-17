using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Commesse
{
    /// <summary>
    /// Migrated from WebForms Commesse/wzQuestionWorkLoad.ascx.
    /// Step 2 of new order wizard: ask if user wants to check workload or launch directly.
    /// Permissions: "Articoli" W
    /// </summary>
    public class NuovoOrdineCaricoLavoroModel : PageModel
    {
        private readonly ILogger<NuovoOrdineCaricoLavoroModel> _logger;

        public NuovoOrdineCaricoLavoroModel(ILogger<NuovoOrdineCaricoLavoroModel> logger)
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

        // UI Labels
        public string InfoPanelTitle { get; private set; } = "Riepilogo Ordine";
        public string VerificaCaricoTitle { get; private set; } = "Verifica Carico di Lavoro";
        public string VerificaCaricoLabel { get; private set; } = "Vuoi verificare il carico di lavoro del reparto prima di lanciare in produzione?";
        public string VerificaCaricoDescription { get; private set; } = "Il controllo del carico ti permette di vedere la disponibilità del reparto e pianificare meglio la produzione.";
        public string CheckWorkLoadButton { get; private set; } = "Verifica Carico di Lavoro";
        public string PianificaDirettoButton { get; private set; } = "Pianifica Direttamente";
        public string BackLabel { get; private set; } = "Indietro";
        public string PermissionDeniedMessage { get; private set; } = "Non hai il permesso di accedere a questa funzione.";

        // State
        public bool HasPermission { get; private set; }
        public Commessa Commessa { get; private set; }
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
        public string Matricola { get; set; } = "";

        public string BackUrl => $"/Commesse/NuovoOrdineDataConsegna?idCommessa={IdCommessa}&annoCommessa={AnnoCommessa}&idProc={IdProc}&revProc={RevProc}&idVariante={IdVariante}&idReparto={IdReparto}&idProdotto={IdProdotto}&annoProdotto={AnnoProdotto}&quantita={Quantita}";

        public string UrlCheckWorkLoad => $"/Commesse/NuovoOrdineCaricoReparto?idCommessa={IdCommessa}&annoCommessa={AnnoCommessa}&idProc={IdProc}&revProc={RevProc}&idVariante={IdVariante}&idReparto={IdReparto}&idProdotto={IdProdotto}&annoProdotto={AnnoProdotto}&quantita={Quantita}&matricola={Matricola}";

        public string UrlPianificaDiretto => $"/Commesse/NuovoOrdineDataFineProduzione?idCommessa={IdCommessa}&annoCommessa={AnnoCommessa}&idProc={IdProc}&revProc={RevProc}&idVariante={IdVariante}&idReparto={IdReparto}&idProdotto={IdProdotto}&annoProdotto={AnnoProdotto}&quantita={Quantita}&matricola={Matricola}";

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
                _logger.LogError(ex, "Commesse/NuovoOrdineCaricoLavoro: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public void OnGet(int idCommessa, int annoCommessa, int idProc, int revProc, int idVariante, int idReparto, int idProdotto, int annoProdotto, int quantita, string matricola = "")
        {
            IdCommessa = idCommessa;
            AnnoCommessa = annoCommessa;
            IdProc = idProc;
            RevProc = revProc;
            IdVariante = idVariante;
            IdReparto = idReparto;
            IdProdotto = idProdotto;
            AnnoProdotto = annoProdotto;
            Quantita = quantita;
            Matricola = matricola ?? "";

            HasPermission = HasPermissionCheck("Articoli", "W");

            var tenant = Tenant;
            if (string.IsNullOrEmpty(tenant) || !HasPermission)
            {
                return;
            }

            try
            {
                Commessa = new Commessa(tenant, IdCommessa, AnnoCommessa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineCaricoLavoro: caricamento commessa {IdCommessa}/{AnnoCommessa}", IdCommessa, AnnoCommessa);
            }
        }
    }
}