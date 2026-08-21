using Microsoft.AspNetCore.Mvc.RazorPages;
using KisApp.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Clienti
{
    public class ClientiModel : PageModel
    {
        public List<Cliente> Clients { get; set; }

        private readonly clienti _clientiService;

        public ClientiModel()
        {
            _clientiService = new clienti("defaulttenant");
        }

        public void OnGet()
        {
            try
            {
                Clients = _clientiService.GetAllClients();
            }
            catch
            {
                Clients = new List<Cliente>
                {
                    new Cliente { CodiceCliente = "Errore", RagioneSociale = "Impossibile caricare i dati" }
                };
            }
        }
    }
}