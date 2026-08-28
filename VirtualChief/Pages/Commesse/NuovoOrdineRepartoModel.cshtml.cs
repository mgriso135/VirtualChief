using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Commesse
{
    public class NuovoOrdineRepartoModel : PageModel
    {
        private readonly ILogger<NuovoOrdineRepartoModel> _logger;

        public NuovoOrdineRepartoModel(ILogger<NuovoOrdineRepartoModel> logger)
        {
            _logger = logger;
        }

        public Commessa Commessa { get; set; }
        public Articolo Articolo { get; set; }
        public List<Reparto> Reparti { get; set; }
        public List<Reparto> RepartiProduttivi { get; set; }

        [BindProperty]
        public int CommessaID { get; set; }

        [BindProperty]
        public int CommessaYear { get; set; }

        [BindProperty]
        public int ProcID { get; set; }

        [BindProperty]
        public int RevProc { get; set; }

        [BindProperty]
        public int VarID { get; set; }

        [BindProperty]
        public int ProdottoID { get; set; }

        [BindProperty]
        public int ProdottoYear { get; set; }

        [BindProperty]
        public int Quantita { get; set; }

        [BindProperty]
        public string Matricola { get; set; }

        [BindProperty]
        public int SelectedRepartoID { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public int Step { get; set; } = 3;

        public void OnGet(int idCommessa, int annoCommessa, int idProc, int revProc, int idVariante, int idProdotto, int annoProdotto, int quantita, string matricola)
        {
            CommessaID = idCommessa;
            CommessaYear = annoCommessa;
            ProcID = idProc;
            RevProc = revProc;
            VarID = idVariante;
            ProdottoID = idProdotto;
            ProdottoYear = annoProdotto;
            Quantita = quantita;
            Matricola = matricola ?? "";

            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            LoadData(tenant);
        }

        public IActionResult OnPost(int idCommessa, int annoCommessa, int idProc, int revProc, int idVariante, int idProdotto, int annoProdotto, int quantita, string matricola)
        {
            CommessaID = idCommessa;
            CommessaYear = annoCommessa;
            ProcID = idProc;
            RevProc = revProc;
            VarID = idVariante;
            ProdottoID = idProdotto;
            ProdottoYear = annoProdotto;
            Quantita = quantita;
            Matricola = matricola ?? "";

            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            if (SelectedRepartoID <= 0)
            {
                ErrorMessage = "Selezionare un reparto";
                LoadData(tenant);
                return Page();
            }

            try
            {
                var processoVariante = new ProcessoVariante(tenant, new processo(tenant, ProcID, RevProc), new variante(tenant, VarID));
                processoVariante.loadReparto();
                processoVariante.process.loadFigli(processoVariante.variant);

                var reparto = new Reparto(tenant, SelectedRepartoID);
                if (reparto.id == -1)
                {
                    ErrorMessage = "Reparto non trovato";
                    LoadData(tenant);
                    return Page();
                }

                // Add reparto to processoVariante
                bool ret = processoVariante.AddReparto(reparto);
                if (!ret)
                {
                    ErrorMessage = "Errore associazione reparto: " + processoVariante.log;
                    LoadData(tenant);
                    return Page();
                }

                // Update articolo with reparto
                var articolo = new Articolo(tenant, ProdottoID, ProdottoYear);
                if (articolo.ID != -1)
                {
                    articolo.Reparto = SelectedRepartoID;
                }

                return RedirectToPage("/Commesse/NuovoOrdineConferma", new { 
                    idCommessa = CommessaID, 
                    annoCommessa = CommessaYear,
                    idProc = ProcID,
                    revProc = RevProc,
                    idVariante = VarID,
                    idProdotto = ProdottoID,
                    annoProdotto = ProdottoYear,
                    quantita = Quantita,
                    matricola = Matricola,
                    idReparto = SelectedRepartoID
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineRepartoModel.cshtml.cs - OnPost");
                ErrorMessage = "Errore: " + ex.Message;
            }

            LoadData(tenant);
            return Page();
        }

        private void LoadData(string tenant)
        {
            try
            {
                Commessa = new Commessa(tenant, CommessaID, CommessaYear);
                Articolo = new Articolo(tenant, ProdottoID, ProdottoYear);

                var processoVariante = new ProcessoVariante(tenant, new processo(tenant, ProcID, RevProc), new variante(tenant, VarID));
                processoVariante.loadReparto();
                processoVariante.process.loadFigli(processoVariante.variant);
                RepartiProduttivi = processoVariante.RepartiProduttivi;

                var elRep = new ElencoReparti(tenant);
                Reparti = elRep.elenco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineRepartoModel.cshtml.cs - LoadData");
                Commessa = null;
                Articolo = null;
                Reparti = new List<Reparto>();
                RepartiProduttivi = new List<Reparto>();
            }
        }
    }
}