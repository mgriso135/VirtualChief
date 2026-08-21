using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace VirtualChief.Pages.Analysis
{
    public class analysisModel : PageModel
    {
        public List<ProductionAnalysisStruct> AnalysisData { get; set; }

        private readonly KisApp.App_Sources.Analysis _analysis;

        public analysisModel()
        {
            _analysis = new KisApp.App_Sources.Analysis("defaulttenant");
        }

        public void OnGet()
        {
            // Try to load analysis data
            try
            {
                // Get the raw data from App_Sources
                var rawData = _analysis.loadProductionAnalysis();
                
                // Convert to the type expected by the PageModel
                AnalysisData = new List<ProductionAnalysisStruct>();
                foreach (var item in rawData)
                {
                    AnalysisData.Add(new ProductionAnalysisStruct
                    {
                        CustomerName = item.CustomerName,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    });
                }
            }
            catch
            {
                AnalysisData = new List<ProductionAnalysisStruct>
                {
                    new ProductionAnalysisStruct { CustomerName = "Error loading data", TotalPrice = 0 }
                };
            }
        }
    }

    public struct ProductionAnalysisStruct
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}