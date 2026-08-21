using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Commesse
{
    public class listCommesseModel : PageModel
    {
        public List<WorkOrder> WorkOrders { get; set; } = new();

        public void OnGet()
        {
        }
    }
}
