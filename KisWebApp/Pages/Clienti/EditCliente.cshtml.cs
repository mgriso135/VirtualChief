using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Clienti
{
    public class EditClienteModel : PageModel
    {
        public Client? Client { get; set; }

        public void OnGet()
        {
        }
    }
}
