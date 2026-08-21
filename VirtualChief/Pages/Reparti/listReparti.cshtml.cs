using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Reparti
{
    /// <summary>
    /// Migrated from WebForms Reparti/listReparti.aspx + listReparti.ascx:
    /// department list (reparti), ordered by name.
    /// </summary>
    public class listRepartiModel : PageModel
    {
        private readonly ILogger<listRepartiModel> _logger;

        public listRepartiModel(ILogger<listRepartiModel> logger)
        {
            _logger = logger;
        }

        public List<Reparto> ElencoReparti { get; set; }

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
                ElencoReparti = new ElencoReparti(tenant).elenco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reparti/listReparti.cshtml.cs");
                ElencoReparti = new List<Reparto>();
            }
        }
    }
}
