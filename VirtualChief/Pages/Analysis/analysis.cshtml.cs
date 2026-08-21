using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Analysis
{
    /// <summary>
    /// Migrated from WebForms Analysis/analysis.aspx: production analysis
    /// (orders joined with production articles and product types).
    /// </summary>
    public class analysisModel : PageModel
    {
        private readonly ILogger<analysisModel> _logger;

        public analysisModel(ILogger<analysisModel> logger)
        {
            _logger = logger;
        }

        public List<ProductionAnalysisStruct> AnalysisData { get; set; }

        public void OnGet()
        {
            var tenant = User.FindFirstValue(CurrentWorkspace.ClaimType);
            if (string.IsNullOrEmpty(tenant))
            {
                RedirectToPage("/Login/selectWorkspace");
                return;
            }

            try
            {
                var history = new ProductionHistory(tenant);
                history.loadProductionAnalysis();
                AnalysisData = history.AnalysisData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Analysis/analysis.cshtml.cs");
                AnalysisData = new List<ProductionAnalysisStruct>();
            }
        }
    }
}
