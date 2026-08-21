using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Analysis
{
    public class ListAnalysisOperatoriModel : PageModel
    {
        public List<User> Users { get; set; }

        public void OnGet()
        {
        }
    }

    public class User
    {
        public string cognome { get; set; }
        public string name { get; set; }
        public string username { get; set; }
    }
}
