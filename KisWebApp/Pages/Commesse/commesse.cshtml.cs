using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Commesse
{
    public class commesseModel : PageModel
    {
        public List<WorkOrder> WorkOrders { get; set; } = new();

        public void OnGet()
        {
        }
    }

    public class WorkOrder
    {
        public int Id { get; set; }
        public string Client { get; set; } = "";
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = "";
    }
}
