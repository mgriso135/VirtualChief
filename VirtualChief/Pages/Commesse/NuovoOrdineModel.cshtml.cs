using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Commesse
{
    public class NuovoOrdineModel : PageModel
    {
        private readonly ILogger<NuovoOrdineModel> _logger;

        public NuovoOrdineModel(ILogger<NuovoOrdineModel> logger)
        {
            _logger = logger;
        }

        public List<Cliente> Clienti { get; set; }

        [BindProperty]
        public string SelectedCliente { get; set; }

        [BindProperty]
        public string ExternalID { get; set; }

        [BindProperty]
        public string Note { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            LoadData(tenant);
        }

        public IActionResult OnPost()
        {
            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                return RedirectToPage("/Login/selectWorkspace");
            }

            if (string.IsNullOrEmpty(SelectedCliente))
            {
                ErrorMessage = "Selezionare un cliente";
                LoadData(tenant);
                return Page();
            }

            try
            {
                var elenco = new ElencoCommesse(tenant);
                int newId = elenco.Add(SelectedCliente, Note ?? "", ExternalID ?? "");

                if (newId > 0)
                {
                    var commessa = new Commessa(tenant, newId, DateTime.UtcNow.Year);
                    if (commessa.ID != -1)
                    {
                        // Auto-confirm the commessa
                        commessa.Confirmed = true;
                        // Note: ConfirmedBy and ConfirmationDate would need setters in Commessa class
                        
                        return RedirectToPage("/Commesse/NuovoOrdineProdotti", new { id = newId, anno = DateTime.UtcNow.Year });
                    }
                }
                else
                {
                    ErrorMessage = "Errore durante la creazione: " + elenco.log;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineModel.cshtml.cs - OnPost");
                ErrorMessage = "Errore durante la creazione: " + ex.Message;
            }

            LoadData(tenant);
            return Page();
        }

        private void LoadData(string tenant)
        {
            try
            {
                var portafoglio = new PortafoglioClienti(tenant);
                Clienti = portafoglio.Elenco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/NuovoOrdineModel.cshtml.cs - LoadData");
                Clienti = new List<Cliente>();
            }
        }
    }
}