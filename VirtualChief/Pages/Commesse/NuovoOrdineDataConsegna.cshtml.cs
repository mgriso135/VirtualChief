using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.ComponentModel.DataAnnotations;

namespace VirtualChief.Pages.Commesse
{
    /// <summary>
    /// Migrated from WebForms Commesse/wzInserisciDataConsegna.ascx.
    /// Step 1 of new order wizard: set delivery date for the article.
    /// Permissions: "Articoli" W
    /// </summary>
    public class NuovoOrdineDataConsegnaModel : PageModel
    {
        private readonly ILogger<NuovoOrdineDataConsegnaModel> _logger;

        public NuovoOrdineDataConsegnaModel(ILogger<NuovoOrdineDataConsegnaModel> logger)
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

        // UI Labels
        public string InfoPanelTitle { get; private set; } = "Riepilogo Ordine";
        public string DataConsegnaTitle { get; private set; } = "Data di Consegna Prevista";
        public string DataConsegnaLabel { get; private set; } = "Data Consegna";
        public string BackTooltip { get; private set; } = "Indietro";
        public string PermissionDeniedMessage { get; private set; } = "Non hai il permesso di inserire la data di consegna.";

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

        // Form data
        [BindProperty]
        public DateTime? DeliveryDate { get; set; }

        public string SelectedDateString => DeliveryDate?.ToString("yyyy-MM-dd") ?? "";

        public bool CanProceed => DeliveryDate.HasValue && DeliveryDate.Value >= DateTime.Today;

        public string BackUrl => $"/Commesse/NuovoOrdineTasks?idCommessa={IdCommessa}&annoCommessa={AnnoCommessa}&idProdotto={IdProdotto}&annoProdotto={AnnoProdotto}";

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

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
                _logger.LogError(ex, "Commesse/NuovoOrdineDataConsegna: verifica permesso {Permission}", nomePermesso);
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
                if (Commessa.ID == -1)
                {
                    ErrorMessage = "Commessa non trovata.";
                    return;
                }

                // If editing existing article, pre-fill delivery date
                if (IdProdotto != -1 && AnnoProdotto != -1)
                {
                    var art = new Articolo(tenant, IdProdotto, AnnoProdotto);
                    if (art.ID != -1 && art.Year != -1 && art.DataPrevistaConsegna >= DateTime.Today)
                    {
                        DeliveryDate = art.DataPrevistaConsegna;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineDataConsegna: caricamento commessa {IdCommessa}/{AnnoCommessa}", IdCommessa, AnnoCommessa);
                ErrorMessage = "Errore durante il caricamento della commessa.";
            }
        }

        public IActionResult OnPost()
        {
            HasPermission = HasPermissionCheck("Articoli", "W");

            var tenant = Tenant;
            if (string.IsNullOrEmpty(tenant) || !HasPermission)
            {
                return RedirectToPage("/Commesse/commesse");
            }

            if (!DeliveryDate.HasValue || DeliveryDate.Value < DateTime.Today)
            {
                ErrorMessage = "La data di consegna deve essere oggi o una data futura.";
                OnGet(IdCommessa, AnnoCommessa, IdProc, RevProc, IdVariante, IdReparto, IdProdotto, AnnoProdotto, Quantita, Matricola);
                return Page();
            }

            try
            {
                var cm = new Commessa(tenant, IdCommessa, AnnoCommessa);
                var prcVar = new ProcessoVariante(tenant, new processo(tenant, IdProc, RevProc), new variante(tenant, IdVariante));
                prcVar.loadReparto();
                prcVar.process.loadFigli(prcVar.variant);
                var rp = new Reparto(tenant, IdReparto);

                if (cm.ID == -1 || prcVar.process == null || prcVar.variant == null || rp.id == -1)
                {
                    ErrorMessage = "Dati non validi.";
                    return Page();
                }

                if (IdProdotto == -1 && AnnoProdotto == -1)
                {
                    // Create new article
                    var newArt = cm.AddArticoloInt(prcVar, DeliveryDate.Value, Quantita);
                    if (newArt[0] != -1 && newArt[1] != -1)
                    {
                        var art = new Articolo(tenant, newArt[0], newArt[1]);
                        art.Reparto = rp.id;
                        if (!string.IsNullOrEmpty(Matricola))
                        {
                            art.Matricola = Matricola;
                        }

                        if (art.ID != -1 && art.Year != -1)
                        {
                            _logger.LogInformation("Commesse/NuovoOrdineDataConsegna: Created new article {Id}/{Year} for commessa {CommessaId}/{CommessaYear}", art.ID, art.Year, IdCommessa, AnnoCommessa);
                            return RedirectToPage("/Commesse/NuovoOrdineCaricoLavoro", new
                            {
                                idCommessa = cm.ID,
                                annoCommessa = cm.Year,
                                idProc = prcVar.process.processID,
                                revProc = prcVar.process.revisione,
                                idVariante = prcVar.variant.idVariante,
                                idReparto = rp.id,
                                idProdotto = newArt[0],
                                annoProdotto = newArt[1],
                                quantita = Quantita,
                                matricola = Matricola
                            });
                        }
                    }
                    else
                    {
                        ErrorMessage = "Errore durante la creazione dell'articolo.";
                    }
                }
                else
                {
                    // Update existing article
                    var art = new Articolo(tenant, IdProdotto, AnnoProdotto);
                    if (art.ID != -1 && art.Year != -1)
                    {
                        art.Reparto = rp.id;
                        art.DataPrevistaConsegna = DeliveryDate.Value;
                        if (!string.IsNullOrEmpty(Matricola))
                        {
                            art.Matricola = Matricola;
                        }

                        _logger.LogInformation("Commesse/NuovoOrdineDataConsegna: Updated article {Id}/{Year} delivery date", art.ID, art.Year);
                        return RedirectToPage("/Commesse/NuovoOrdineCaricoLavoro", new
                        {
                            idCommessa = cm.ID,
                            annoCommessa = cm.Year,
                            idProc = prcVar.process.processID,
                            revProc = prcVar.process.revisione,
                            idVariante = prcVar.variant.idVariante,
                            idReparto = rp.id,
                            idProdotto = art.ID,
                            annoProdotto = art.Year,
                            quantita = Quantita,
                            matricola = Matricola
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineDataConsegna: salvataggio data consegna");
                ErrorMessage = "Errore durante il salvataggio.";
            }

            return Page();
        }
    }
}