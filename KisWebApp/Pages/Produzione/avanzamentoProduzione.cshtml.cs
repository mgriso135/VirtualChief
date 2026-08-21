using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KisWebApp.Pages.Produzione
{
    public class avanzamentoProduzioneModel : PageModel
    {
        public List<ProductionProgress> ProductionProgress { get; set; }

        public void OnGet()
        {
        }
    }

    public class ProductionProgress
    {
        public int WorkOrderId { get; set; }
        public string Progress { get; set; }
        public string Status { get; set; }
    }
}
