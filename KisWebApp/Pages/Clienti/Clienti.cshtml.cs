using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Clienti
{
    public class ClientiModel : PageModel
    {
        public List<Client> Clients { get; set; } = new();

        public void OnGet()
        {
        }
    }
}
