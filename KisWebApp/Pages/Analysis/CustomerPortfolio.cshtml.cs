using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Analysis
{
    public class CustomerPortfolioModel : PageModel
    {
        public List<Customer> Customers { get; set; }

        public void OnGet()
        {
        }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Revenue { get; set; }
        public string OrderHistory { get; set; }
    }
}
