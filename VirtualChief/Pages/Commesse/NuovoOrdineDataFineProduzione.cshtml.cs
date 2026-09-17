using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Commesse
{
    /// <summary>
    /// Migrated from WebForms Commesse/wzCheckDeliveryDate.ascx.
    /// Step 3 of new order wizard: set production end date and launch in production.
    /// Permissions: "Articoli" W
    /// </summary>
    public class NuovoOrdineDataFineProduzioneModel : PageModel
    {
        private readonly ILogger<NuovoOrdineDataFineProduzioneModel> _logger;

        public NuovoOrdineDataFineProduzioneModel(ILogger<NuovoOrdineDataFineProduzioneModel> logger)
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
        public string NavDataFineProduzione { get; private set; } = "Data Fine Produzione";

        // UI Labels
        public string InfoPanelTitle { get; private set; } = "Riepilogo Ordine";
        public string DataFineProduzioneTitle { get; private set; } = "Data Prevista Fine Produzione";
        public string DataFineProdLabel { get; private set; } = "Data Fine Produzione";
        public string DataConsegnaLabel { get; private set; } = "Data Consegna";
        public string OreLabel { get; private set; } = "Ore";
        public string MinutiLabel { get; private set; } = "Minuti";
        public string SecondiLabel { get; private set; } = "Secondi";
        public string SaveButton { get; private set; } = "Salva";
        public string BackTooltip { get; private set; } = "Indietro";
        public string BackButton { get; private set; } = "Indietro";
        public string RescheduleButton { get; private set; } = "Riprogramma e Lancia";
        public string ConfirmLaunchMessage { get; private set; } = "Sei sicuro di voler lanciare in produzione?";
        public string AlreadyInProductionTitle { get; private set; } = "Articolo Già in Produzione";
        public string AlreadyInProductionMessage { get; private set; } = "Questo articolo è già stato pianificato o è in produzione.";
        public string InvalidDatesMessage { get; private set; } = "Le date non sono valide per la riprogrammazione.";
        public string PermissionDeniedMessage { get; private set; } = "Non hai il permesso di lanciare in produzione.";

        // State
        public bool HasPermission { get; private set; }
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
        public string Matricola { get; set; } = "";

        // Form data
        public DateTime? DeliveryDate { get; set; }

        public string SelectedDateString => DeliveryDate?.ToString("yyyy-MM-dd") ?? "";
        public int SelectedHour { get; set; } = DateTime.Now.Hour;
        public int SelectedMinute { get; set; } = DateTime.Now.Minute;
        public int SelectedSecond { get; set; } = DateTime.Now.Second;

        public string MaxDateString => Articolo?.DataPrevistaConsegna.ToString("yyyy-MM-dd") ?? DateTime.Today.AddDays(365).ToString("yyyy-MM-dd");

        public bool CanLaunch => DeliveryDate.HasValue && 
                                DeliveryDate.Value > DateTime.Now && 
                                DeliveryDate.Value <= Articolo?.DataPrevistaConsegna;

        public string BackUrl => $"/Commesse/NuovoOrdineCaricoLavoro?idCommessa={IdCommessa}&annoCommessa={AnnoCommessa}&idProc={IdProc}&revProc={RevProc}&idVariante={IdVariante}&idReparto={IdReparto}&idProdotto={IdProdotto}&annoProdotto={AnnoProdotto}&quantita={Quantita}";

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
                _logger.LogError(ex, "Commesse/NuovoOrdineDataFineProduzione: verifica permesso {Permission}", nomePermesso);
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
                Articolo = new Articolo(tenant, IdProdotto, AnnoProdotto);
                if (Articolo.ID != -1 && Articolo.Year != -1)
                {
                    if (Articolo.DataPrevistaFineProduzione >= DateTime.Now && Articolo.DataPrevistaConsegna >= Articolo.DataPrevistaFineProduzione)
                    {
                        DeliveryDate = Articolo.DataPrevistaFineProduzione;
                        SelectedHour = Articolo.DataPrevistaFineProduzione.Hour;
                        SelectedMinute = Articolo.DataPrevistaFineProduzione.Minute;
                        SelectedSecond = Articolo.DataPrevistaFineProduzione.Second;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineDataFineProduzione: caricamento articolo {IdProdotto}/{AnnoProdotto}", IdProdotto, AnnoProdotto);
                ErrorMessage = "Errore durante il caricamento dell'articolo.";
            }
        }

        public IActionResult OnPost(string action, DateTime? productionEndDate, int productionEndHour = 0, int productionEndMinute = 0, int productionEndSecond = 0)
        {
            HasPermission = HasPermissionCheck("Articoli", "W");

            var tenant = Tenant;
            if (string.IsNullOrEmpty(tenant) || !HasPermission)
            {
                return RedirectToPage("/Commesse/commesse");
            }

            try
            {
                Articolo = new Articolo(tenant, IdProdotto, AnnoProdotto);
                if (Articolo.ID == -1 || Articolo.Year == -1)
                {
                    ErrorMessage = "Articolo non trovato.";
                    return Page();
                }

                if (action == "saveDate" && productionEndDate.HasValue)
                {
                    var dataFineProd = new DateTime(
                        productionEndDate.Value.Year,
                        productionEndDate.Value.Month,
                        productionEndDate.Value.Day,
                        productionEndHour,
                        productionEndMinute,
                        productionEndSecond);

                    if (dataFineProd > DateTime.Now && dataFineProd <= Articolo.DataPrevistaConsegna)
                    {
                        Articolo.DataPrevistaFineProduzione = dataFineProd;
                        DeliveryDate = dataFineProd;
                        SelectedHour = productionEndHour;
                        SelectedMinute = productionEndMinute;
                        SelectedSecond = productionEndSecond;
                        SuccessMessage = "Data fine produzione salvata. Ora puoi lanciare in produzione.";
                        _logger.LogInformation("Commesse/NuovoOrdineDataFineProduzione: Saved production end date for article {Id}/{Year}", IdProdotto, AnnoProdotto);
                    }
                    else
                    {
                        ErrorMessage = "Errore: data fine produzione deve essere successiva ad oggi e antecedente o uguale alla data di consegna.";
                    }
                    return Page();
                }
                else if (action == "launch")
                {
                    if (Articolo.Status == 'N')
                    {
                        // Simulate and launch
                        var rp = new Reparto(tenant, Articolo.Reparto);
                        Articolo.Proc.process.loadFigli(Articolo.Proc.variant);
                        var lstTasks = new List<TaskConfigurato>();

                        for (int i = 0; i < Articolo.Proc.process.subProcessi.Count; i++)
                        {
                            var tskVar = new TaskVariante(tenant, new processo(tenant, Articolo.Proc.process.subProcessi[i].processID, Articolo.Proc.process.subProcessi[i].revisione), Articolo.Proc.variant);
                            tskVar.loadTempiCiclo();
                            var tc = new TempoCiclo(tenant, tskVar.Task.processID, tskVar.Task.revisione, Articolo.Proc.variant.idVariante, tskVar.getDefaultOperatori());
                            if (tc.Tempo != null)
                            {
                                lstTasks.Add(new TaskConfigurato(tenant, tskVar, tc, rp.id, Articolo.Quantita));
                            }
                        }

                        var prcCfg = new ConfigurazioneProcesso(tenant, Articolo, lstTasks, rp, Articolo.Quantita);
                        int rt1 = prcCfg.SimulaIntroduzioneInProduzione();
                        if (rt1 == 1)
                        {
                            Articolo.Planner = new UserAccount(int.Parse(User.FindFirst("uid")?.Value ?? "0"));
                            int rt = prcCfg.LanciaInProduzione();
                            if (rt == 1)
                            {
                                SuccessMessage = "Articolo lanciato in produzione con successo.";
                                _logger.LogInformation("Commesse/NuovoOrdineDataFineProduzione: Launched article {Id}/{Year} in production", IdProdotto, AnnoProdotto);
                                return RedirectToPage("/Commesse/NuovoOrdineAllarmi", new
                                {
                                    idCommessa = Articolo.Commessa,
                                    annoCommessa = Articolo.AnnoCommessa,
                                    idProc = Articolo.Proc.process.processID,
                                    revProc = Articolo.Proc.process.revisione,
                                    idVariante = Articolo.Proc.variant.idVariante,
                                    idReparto = Articolo.Reparto,
                                    idProdotto = Articolo.ID,
                                    annoProdotto = Articolo.Year
                                });
                            }
                            else if (rt == 3)
                            {
                                ErrorMessage = "L'articolo è già stato pianificato.";
                            }
                            else
                            {
                                ErrorMessage = "Errore durante il lancio: " + prcCfg.log;
                            }
                        }
                        else
                        {
                            ErrorMessage = "Errore durante la simulazione.";
                        }
                    }
                    else if (Articolo.Status == 'I' || Articolo.Status == 'P')
                    {
                        // Reschedule
                        if (DeliveryDate.HasValue && DeliveryDate.Value <= Articolo.DataPrevistaConsegna && DeliveryDate.Value >= DateTime.Now)
                        {
                            int ret = Articolo.SpostaPianificazione(DeliveryDate.Value, Articolo.DataPrevistaConsegna);
                            if (ret == 1)
                            {
                                SuccessMessage = "Articolo riprogrammato con successo.";
                                _logger.LogInformation("Commesse/NuovoOrdineDataFineProduzione: Rescheduled article {Id}/{Year}", IdProdotto, AnnoProdotto);
                                return RedirectToPage("/Commesse/NuovoOrdineAllarmi", new
                                {
                                    idCommessa = Articolo.Commessa,
                                    annoCommessa = Articolo.AnnoCommessa,
                                    idProc = Articolo.Proc.process.processID,
                                    revProc = Articolo.Proc.process.revisione,
                                    idVariante = Articolo.Proc.variant.idVariante,
                                    idReparto = Articolo.Reparto,
                                    idProdotto = Articolo.ID,
                                    annoProdotto = Articolo.Year
                                });
                            }
                            else if (ret == 3)
                            {
                                ErrorMessage = "Errore nelle date.";
                            }
                            else
                            {
                                ErrorMessage = "Errore durante la riprogrammazione.";
                            }
                        }
                        else
                        {
                            ErrorMessage = "Date non valide per la riprogrammazione.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineDataFineProduzione: azione {Action}", action);
                ErrorMessage = "Errore generico.";
            }

            return Page();
        }
    }
}