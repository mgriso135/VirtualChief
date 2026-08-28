using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Commesse
{
    public class NuovoOrdineProdottiModel : PageModel
    {
        private readonly ILogger<NuovoOrdineProdottiModel> _logger;

        public NuovoOrdineProdottiModel(ILogger<NuovoOrdineProdottiModel> logger)
        {
            _logger = logger;
        }

        public Commessa Commessa { get; set; }
        public List<ProcessoVariante> ProdottiStandard { get; set; }
        public List<Cliente> Clienti { get; set; }

        [BindProperty]
        public int CommessaID { get; set; }

        [BindProperty]
        public int CommessaYear { get; set; }

        [BindProperty]
        public string SelectedProdotto { get; set; }

        [BindProperty]
        public int Quantita { get; set; }

        [BindProperty]
        public DateTime DataPrevistaConsegna { get; set; }

        [BindProperty]
        public string Matricola { get; set; }

        // Copy existing PERT
        [BindProperty]
        public string CopyProdottoID { get; set; }

        [BindProperty]
        public string CopyProdottoNome { get; set; }

        [BindProperty]
        public string CopyProdottoDesc { get; set; }

        [BindProperty]
        public int CopyQuantita { get; set; }

        [BindProperty]
        public bool CopiaTC { get; set; }

        [BindProperty]
        public bool CopiaReparto { get; set; }

        [BindProperty]
        public bool CopiaPostazioni { get; set; }

        [BindProperty]
        public bool CopiaParametri { get; set; }

        [BindProperty]
        public bool CopiaWorkInstructions { get; set; }

        // New blank variant
        [BindProperty]
        public string NewProdottoNome { get; set; }

        [BindProperty]
        public string NewProdottoDesc { get; set; }

        [BindProperty]
        public int NewQuantita { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public int Step { get; set; } = 2;

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

        public IActionResult OnPost(int id, int anno, string action)
        {
            CommessaID = id;
            CommessaYear = anno;

            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            try
            {
                var commessa = new Commessa(tenant, id, anno);
                if (commessa.ID == -1)
                {
                    ErrorMessage = "Commesa non trovata";
                    LoadData(tenant, id, anno);
                    return Page();
                }

                if (action == "addStandard")
                {
                    return HandleAddStandard(tenant, commessa);
                }
                else if (action == "copyExisting")
                {
                    return HandleCopyExisting(tenant, commessa);
                }
                else if (action == "createNew")
                {
                    return HandleCreateNew(tenant, commessa);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineProdottiModel.cshtml.cs - OnPost");
                ErrorMessage = "Errore: " + ex.Message;
            }

            LoadData(tenant, id, anno);
            return Page();
        }

        private IActionResult HandleAddStandard(string tenant, Commessa commessa)
        {
            if (string.IsNullOrEmpty(SelectedProdotto) || Quantita <= 0)
            {
                ErrorMessage = "Selezionare un prodotto e inserire una quantità valida";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }

            if (DataPrevistaConsegna <= DateTime.Now)
            {
                ErrorMessage = "La data di consegna prevista deve essere futura";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }

            try
            {
                var parts = SelectedProdotto.Split(',');
                int procID = int.Parse(parts[0]);
                int varID = int.Parse(parts[1]);

                var processo = new processo(tenant, procID);
                var variante = new variante(tenant, varID);
                var processoVariante = new ProcessoVariante(tenant, processo, variante);

                processoVariante.loadReparto();
                processoVariante.process.loadFigli(processoVariante.variant);

                if (processoVariante.process == null || processoVariante.variant == null)
                {
                    ErrorMessage = "Prodotto non valido";
                    LoadData(tenant, CommessaID, CommessaYear);
                    return Page();
                }

                // Add to commessa
                bool ret = commessa.AddArticolo(processoVariante, DataPrevistaConsegna, Quantita);
                if (!ret)
                {
                    ErrorMessage = "Errore aggiunta articolo: " + commessa.log;
                    LoadData(tenant, CommessaID, CommessaYear);
                    return Page();
                }

                // Get the newly created article ID
                commessa.loadArticoli();
                var newArt = commessa.Articoli.LastOrDefault(a => a.Proc.process.processID == procID && a.Proc.variant.idVariante == varID);
                
                if (newArt != null)
                {
                    return RedirectToPage("/Commesse/NuovoOrdineReparto", new { 
                        idCommessa = commessa.ID, 
                        annoCommessa = commessa.Year,
                        idProc = procID,
                        revProc = processoVariante.process.revisione,
                        idVariante = varID,
                        idProdotto = newArt.ID,
                        annoProdotto = newArt.Year,
                        quantita = Quantita,
                        matricola = Matricola ?? ""
                    });
                }

                ErrorMessage = "Articolo creato ma non trovato";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Errore: " + ex.Message;
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }
        }

        private IActionResult HandleCopyExisting(string tenant, Commessa commessa)
        {
            if (string.IsNullOrEmpty(CopyProdottoID) || CopyQuantita <= 0)
            {
                ErrorMessage = "Selezionare un prodotto da copiare e inserire una quantità";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }

            if (string.IsNullOrEmpty(CopyProdottoNome))
            {
                ErrorMessage = "Inserire un nome per il nuovo prodotto";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }

            try
            {
                var parts = CopyProdottoID.Split(',');
                int sourceProcID = int.Parse(parts[0]);
                int sourceVarID = int.Parse(parts[1]);

                // Find or create macro process for this client
                var macroProcessi = new macroProcessi(tenant);
                var lstProc = macroProcessi.FindByName(commessa.Cliente);
                int prcID = -1;
                int prcRev = -1;
                
                foreach (var p in lstProc)
                {
                    var prc = new processo(tenant, p[0], p[1]);
                    if (!prc.isVSM)
                    {
                        prcID = prc.processID;
                        prcRev = prc.revisione;
                        break;
                    }
                }

                // Create macro process if not exists
                if (prcID == -1)
                {
                    bool check = macroProcessi.Add(commessa.Cliente, commessa.Cliente, false);
                    if (!check)
                    {
                        ErrorMessage = "Errore creazione macro processo";
                        LoadData(tenant, CommessaID, CommessaYear);
                        return Page();
                    }
                    lstProc = macroProcessi.FindByName(commessa.Cliente);
                    prcID = lstProc[0][0];
                    prcRev = lstProc[0][1];
                }

                var sourceProcVar = new ProcessoVariante(tenant, new processo(tenant, sourceProcID), new variante(tenant, sourceVarID));
                sourceProcVar.loadReparto();
                sourceProcVar.process.loadFigli(sourceProcVar.variant);

                var targetProc = new processo(tenant, prcID, prcRev);
                int newVarID = sourceProcVar.CopyTo(targetProc, CopyProdottoNome, CopyProdottoDesc, 
                    !CopiaReparto, CopiaTC, CopiaReparto, CopiaPostazioni, CopiaParametri, CopiaWorkInstructions);

                if (newVarID == -1)
                {
                    ErrorMessage = "Errore durante la copia del PERT: " + sourceProcVar.log;
                    LoadData(tenant, CommessaID, CommessaYear);
                    return Page();
                }

                // Add article with copied variant
                var newProcVar = new ProcessoVariante(tenant, new processo(tenant, prcID, prcRev), new variante(tenant, newVarID));
                newProcVar.loadReparto();
                newProcVar.process.loadFigli(newProcVar.variant);

                bool ret = commessa.AddArticolo(newProcVar, DateTime.Now.AddDays(7), CopyQuantita);
                if (!ret)
                {
                    ErrorMessage = "Errore aggiunta articolo: " + commessa.log;
                    LoadData(tenant, CommessaID, CommessaYear);
                    return Page();
                }

                commessa.loadArticoli();
                var newArt = commessa.Articoli.LastOrDefault(a => a.Proc.variant.idVariante == newVarID);

                if (newArt != null)
                {
                    return RedirectToPage("/Commesse/NuovoOrdineReparto", new { 
                        idCommessa = commessa.ID, 
                        annoCommessa = commessa.Year,
                        idProc = prcID,
                        revProc = prcRev,
                        idVariante = newVarID,
                        idProdotto = newArt.ID,
                        annoProdotto = newArt.Year,
                        quantita = CopyQuantita,
                        matricola = ""
                    });
                }

                ErrorMessage = "Articolo creato ma non trovato";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Errore: " + ex.Message;
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }
        }

        private IActionResult HandleCreateNew(string tenant, Commessa commessa)
        {
            if (string.IsNullOrEmpty(NewProdottoNome) || NewQuantita <= 0)
            {
                ErrorMessage = "Inserire nome prodotto e quantità";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }

            try
            {
                // Find or create macro process for this client
                var macroProcessi = new macroProcessi(tenant);
                var lstProc = macroProcessi.FindByName(commessa.Cliente);
                int prcID = -1;
                int prcRev = -1;
                
                foreach (var p in lstProc)
                {
                    var prc = new processo(tenant, p[0], p[1]);
                    if (!prc.isVSM)
                    {
                        prcID = prc.processID;
                        prcRev = prc.revisione;
                        break;
                    }
                }

                if (prcID == -1)
                {
                    bool check = macroProcessi.Add(commessa.Cliente, commessa.Cliente, false);
                    if (!check)
                    {
                        ErrorMessage = "Errore creazione macro processo";
                        LoadData(tenant, CommessaID, CommessaYear);
                        return Page();
                    }
                    lstProc = macroProcessi.FindByName(commessa.Cliente);
                    prcID = lstProc[0][0];
                    prcRev = lstProc[0][1];
                }

                var proc = new processo(tenant, prcID, prcRev);
                var var = new variante(tenant);
                int varID = var.add(NewProdottoNome, NewProdottoDesc);

                if (varID == -1)
                {
                    ErrorMessage = "Errore creazione variante: " + var.log;
                    LoadData(tenant, CommessaID, CommessaYear);
                    return Page();
                }

                bool retAddProcVar = proc.addVariante(new variante(tenant, varID));
                if (!retAddProcVar)
                {
                    ErrorMessage = "Errore associazione variante al processo";
                    LoadData(tenant, CommessaID, CommessaYear);
                    return Page();
                }

                var newProcVar = new ProcessoVariante(tenant, new processo(tenant, prcID, prcRev), new variante(tenant, varID));
                newProcVar.loadReparto();
                newProcVar.process.loadFigli(newProcVar.variant);

                bool ret = commessa.AddArticolo(newProcVar, DateTime.Now.AddDays(7), NewQuantita);
                if (!ret)
                {
                    ErrorMessage = "Errore aggiunta articolo: " + commessa.log;
                    LoadData(tenant, CommessaID, CommessaYear);
                    return Page();
                }

                commessa.loadArticoli();
                var newArt = commessa.Articoli.LastOrDefault(a => a.Proc.variant.idVariante == varID);

                if (newArt != null)
                {
                    return RedirectToPage("/Commesse/NuovoOrdineReparto", new { 
                        idCommessa = commessa.ID, 
                        annoCommessa = commessa.Year,
                        idProc = prcID,
                        revProc = prcRev,
                        idVariante = varID,
                        idProdotto = newArt.ID,
                        annoProdotto = newArt.Year,
                        quantita = NewQuantita,
                        matricola = ""
                    });
                }

                ErrorMessage = "Articolo creato ma non trovato";
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Errore: " + ex.Message;
                LoadData(tenant, CommessaID, CommessaYear);
                return Page();
            }
        }

        private void LoadData(string tenant, int id, int anno)
        {
            try
            {
                Commessa = new Commessa(tenant, id, anno);
                
                var prodotti = new ElencoProcessiVarianti(tenant, true);
                ProdottiStandard = prodotti.elencoFigli.OrderBy(x => x.NomeCombinato).ToList();

                var portafoglio = new PortafoglioClienti(tenant);
                Clienti = portafoglio.Elenco;

                DataPrevistaConsegna = DateTime.Now.AddDays(7);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineProdottiModel.cshtml.cs - LoadData");
                Commessa = null;
                ProdottiStandard = new List<ProcessoVariante>();
                Clienti = new List<Cliente>();
            }
        }
    }
}