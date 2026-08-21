using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Analysis
{
    public class analysisModel : PageModel
    {
        public List<AnalysisItem> AnalysisData { get; set; } = new();

        public void OnGet()
        {
        }
    }

    public class AnalysisItem
    {
        public string CustomerName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public double Productivity { get; set; }
    }
}
