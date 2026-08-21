using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Produzione
{
    /// <summary>
    /// Migrated from WebForms Produzione/produzione.aspx (production hub) and
    /// listArticoliINP.ascx: flat list of production articles from productionplan.
    /// </summary>
    public class produzioneModel : PageModel
    {
        private readonly ILogger<produzioneModel> _logger;

        public produzioneModel(ILogger<produzioneModel> logger)
        {
            _logger = logger;
        }

        public List<FlatProduct> Articoli { get; set; }

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
                var elenco = new ElencoArticoli(tenant);
                elenco.loadProductList();
                Articoli = elenco.ProductList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Produzione/produzione.cshtml.cs");
                Articoli = new List<FlatProduct>();
            }
        }
    }
}
