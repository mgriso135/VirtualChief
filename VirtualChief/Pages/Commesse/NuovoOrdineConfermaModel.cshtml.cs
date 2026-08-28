using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Commesse
{
    public class NuovoOrdineConfermaModel : PageModel
    {
        private readonly ILogger<NuovoOrdineConfermaModel> _logger;

        public NuovoOrdineConfermaModel(ILogger<NuovoOrdineConfermaModel> logger)
        {
            _logger = logger;
        }

        public Commessa Commessa { get; set; }
        public Articolo Articolo { get; set; }
        public Reparto Reparto { get; set; }
        public ProcessoVariante ProcessoVariante { get; set; }

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
        public int RepartoID { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public int Step { get; set; } = 4;

        public void OnGet(int idCommessa, int annoCommessa, int idProc, int revProc, int idVariante, int idProdotto, int annoProdotto, int quantita, string matricola, int idReparto)
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
            RepartoID = idReparto;

            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            LoadData(tenant);
        }

        public IActionResult OnPost(int idCommessa, int annoCommessa, int idProc, int revProc, int idVariante, int idProdotto, int annoProdotto, int quantita, string matricola, int idReparto)
        {
            // Final confirmation - redirect to commessa list or PERT editor
            return RedirectToPage("/Commesse/Articoli", new { id = idCommessa, anno = annoCommessa });
        }

        private void LoadData(string tenant)
        {
            try
            {
                Commessa = new Commessa(tenant, CommessaID, CommessaYear);
                Articolo = new Articolo(tenant, ProdottoID, ProdottoYear);
                Reparto = new Reparto(tenant, RepartoID);
                ProcessoVariante = new ProcessoVariante(tenant, new processo(tenant, ProcID, RevProc), new variante(tenant, VarID));
                ProcessoVariante.loadReparto();
                ProcessoVariante.process.loadFigli(ProcessoVariante.variant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineConfermaModel.cshtml.cs - LoadData");
                Commessa = null;
                Articolo = null;
                Reparto = null;
                ProcessoVariante = null;
            }
        }
    }
}