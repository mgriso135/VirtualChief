using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Clienti
{
    public class AddClienteModel : PageModel
    {
        public Client? Client { get; set; }

        public void OnGet()
        {
        }
    }

    public class Client
    {
        public string Name { get; set; } = "";
        public string Contact { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
