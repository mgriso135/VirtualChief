using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace VirtualChief.Pages.Commesse
{
    public class CommesseModel : PageModel
    {
        public List<dynamic> Commesse { get; set; }

        public void OnGet()
        {
            // Sample data - in production would load from KisApp.App_Sources
            Commesse = new List<dynamic>
            {
                new { Numero = "001", Descrizione = "Commessa Principale", ClienteNome = "Cliente A", Stato = "Aperta" },
                new { Numero = "002", Descrizione = "Commessa Secondaria", ClienteNome = "Cliente B", Stato = "In lavorazione" }
            };
        }
    }
}