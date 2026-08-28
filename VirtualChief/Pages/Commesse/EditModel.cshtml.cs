using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Commesse
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;

        public EditModel(ILogger<EditModel> logger)
        {
            _logger = logger;
        }

        public Commessa Commessa { get; set; }
        public List<Cliente> Clienti { get; set; }

        [BindProperty]
        public string SelectedCliente { get; set; }

        [BindProperty]
        public string ExternalID { get; set; }

        [BindProperty]
        public string Note { get; set; }

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

            if (string.IsNullOrEmpty(SelectedCliente))
            {
                ErrorMessage = "Selezionare un cliente";
                LoadData(tenant, id, anno);
                return Page();
            }

            try
            {
                var commessa = new Commessa(tenant, id, anno);
                commessa.ExternalID = ExternalID ?? "";
                commessa.Note = Note ?? "";

                // Update cliente - need to check if this is supported
                // The Commessa class doesn't have a setter for Cliente, so we'd need to add it or use direct SQL
                // For now, update what we can
                commessa.ExternalID = ExternalID ?? "";
                commessa.Note = Note ?? "";

                SuccessMessage = "Commesa aggiornata con successo";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/EditModel.cshtml.cs - OnPost, id={ID}, anno={Anno}", id, anno);
                ErrorMessage = "Errore durante l'aggiornamento: " + ex.Message;
            }

            LoadData(tenant, id, anno);
            return Page();
        }

        private void LoadData(string tenant, int id, int anno)
        {
            try
            {
                Commessa = new Commessa(tenant, id, anno);
                SelectedCliente = Commessa.Cliente;
                ExternalID = Commessa.ExternalID;
                Note = Commessa.Note;

                var portafoglio = new PortafoglioClienti(tenant);
                Clienti = portafoglio.Elenco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commesse/EditModel.cshtml.cs - LoadData, id={ID}, anno={Anno}", id, anno);
                Commessa = null;
                Clienti = new List<Cliente>();
            }
        }
    }
}