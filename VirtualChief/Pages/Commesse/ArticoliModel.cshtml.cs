using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Commesse
{
    public class ArticoliModel : PageModel
    {
        private readonly ILogger<ArticoliModel> _logger;

        public ArticoliModel(ILogger<ArticoliModel> logger)
        {
            _logger = logger;
        }

        public Commessa Commessa { get; set; }
        public List<Articolo> Articoli { get; set; }
        public List<ProcessoVariante> ProdottiDisponibili { get; set; }

        [BindProperty]
        public string SelectedProdotto { get; set; }

        [BindProperty]
        public DateTime DataPrevistaConsegna { get; set; }

        [BindProperty]
        public int Quantita { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public int CommessaID { get; set; }
        public int CommessaYear { get; set; }

        public void OnGet(int id, int anno)
        {
            CommessaID = id;
            CommessaYear = anno;

            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            LoadData(tenant, id, anno);
        }

        public IActionResult OnPost(int id, int anno)
        {
            CommessaID = id;
            CommessaYear = anno;

            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            if (string.IsNullOrEmpty(SelectedProdotto))
            {
                ErrorMessage = "Selezionare un prodotto";
                LoadData(tenant, id, anno);
                return Page();
            }

            if (Quantita <= 0)
            {
                ErrorMessage = "Inserire una quantità valida";
                LoadData(tenant, id, anno);
                return Page();
            }

            if (DataPrevistaConsegna <= DateTime.Now)
            {
                ErrorMessage = "La data di consegna prevista deve essere futura";
                LoadData(tenant, id, anno);
                return Page();
            }

            try
            {
                var prodotti = new ElencoProcessiVarianti(tenant, true);
                var selectedProdottoStr = SelectedProdotto.Split(',');
                int procID = int.Parse(selectedProdottoStr[0]);
                int varID = int.Parse(selectedProdottoStr[1]);

                var processo = new processo(tenant, procID);
                var variante = new variante(tenant, varID);
                var processoVariante = new ProcessoVariante(tenant, processo, variante);

                var commessa = new Commessa(tenant, id, anno);
                commessa.loadArticoli();

                bool ret = commessa.AddArticolo(processoVariante, DataPrevistaConsegna, Quantita);

                if (ret)
                {
                    SuccessMessage = "Articolo aggiunto con successo";
                }
                else
                {
                    ErrorMessage = "Errore durante l'aggiunta dell'articolo: " + commessa.log;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/ArticoliModel.cshtml.cs - OnPost, id={ID}, anno={Anno}", id, anno);
                ErrorMessage = "Errore durante l'aggiunta dell'articolo: " + ex.Message;
            }

            LoadData(tenant, id, anno);
            return Page();
        }

        public IActionResult OnPostDelete(int id, int anno, int artId, int artAnno)
        {
            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            try
            {
                var commessa = new Commessa(tenant, id, anno);
                commessa.loadArticoli();

                foreach (var art in commessa.Articoli)
                {
                    if (art.ID == artId && art.Year == artAnno)
                    {
                        if (art.Status == 'N')
                        {
                            bool ret = art.Delete();
                            if (ret)
                            {
                                SuccessMessage = "Articolo eliminato con successo";
                            }
                            else
                            {
                                ErrorMessage = "Errore durante l'eliminazione: " + art.log;
                            }
                        }
                        else
                        {
                            ErrorMessage = "Non è possibile eliminare un articolo già pianificato o in produzione";
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/ArticoliModel.cshtml.cs - OnPostDelete, id={ID}, anno={Anno}, artId={ArtId}, artAnno={ArtAnno}", id, anno, artId, artAnno);
                ErrorMessage = "Errore durante l'eliminazione dell'articolo: " + ex.Message;
            }

            LoadData(tenant, id, anno);
            return Page();
        }

        private void LoadData(string tenant, int id, int anno)
        {
            try
            {
                Commessa = new Commessa(tenant, id, anno);
                Commessa.loadArticoli();
                Articoli = Commessa.Articoli;

                var prodotti = new ElencoProcessiVarianti(tenant, true);
                ProdottiDisponibili = prodotti.elencoFigli;

                DataPrevistaConsegna = DateTime.Now.AddDays(7);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/ArticoliModel.cshtml.cs - LoadData, id={ID}, anno={Anno}", id, anno);
                Commessa = null;
                Articoli = new List<Articolo>();
                ProdottiDisponibili = new List<ProcessoVariante>();
            }
        }
    }
}