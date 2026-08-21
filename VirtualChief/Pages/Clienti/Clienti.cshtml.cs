using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using System.Collections.Generic;

namespace VirtualChief.Pages.Clienti
{
    /// <summary>
    /// Migrated from WebForms Clienti/Clienti.aspx + listClienti.ascx:
    /// customer master data list (anagraficaclienti).
    /// </summary>
    public class ClientiModel : PageModel
    {
        private readonly ILogger<ClientiModel> _logger;

        public ClientiModel(ILogger<ClientiModel> logger)
        {
            _logger = logger;
        }

        public List<Cliente> Clients { get; set; }

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
                Clients = new PortafoglioClienti(tenant).Elenco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Clienti/Clienti.cshtml.cs");
                Clients = new List<Cliente>();
            }
        }
    }
}
