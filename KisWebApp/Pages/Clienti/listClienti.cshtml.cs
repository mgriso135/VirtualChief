using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Clienti
{
    public class listClientiModel : PageModel
    {
        public List<Client> Clients { get; set; } = new();

        public void OnGet()
        {
        }
    }
}
