using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Sources;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChief.Pages
{
    /// <summary>
    /// Home / tenant hub. Tenant creation delegates to the legacy
    /// <c>UserAccount.addWorkspace</c> (vcmain.workspaces) exactly as the
    /// AccountsMgm area does; until authentication is ported, the first
    /// account of the default workspace acts as creator.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<Workspace> Tenants { get; set; }

        [BindProperty]
        public string TenantName { get; set; }

        [TempData]
        public string Message { get; set; }

        [TempData]
        public bool MessageIsError { get; set; }

        public void OnGet()
        {
            LoadTenants();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(TenantName))
            {
                ModelState.AddModelError("TenantName", "Il nome del tenant è obbligatorio.");
                LoadTenants();
                return Page();
            }

            try
            {
                // Temporary creator resolution until auth is migrated:
                // first account registered on the default workspace.
                var defaultWs = new Workspace("kaizenkey");
                defaultWs.loadUserAccounts();
                var creator = defaultWs.UserAccounts.FirstOrDefault();

                if (creator == null)
                {
                    Message = "Nessun utente disponibile per creare il tenant.";
                    MessageIsError = true;
                    return RedirectToPage();
                }

                int ret = creator.addWorkspace(TenantName.Trim());
                if (ret > 0)
                {
                    _logger.LogInformation("Tenant {Tenant} creato (id {Id}) da {UserId}", TenantName, ret, creator.userId);
                    Message = $"Tenant '{TenantName}' creato (id {ret}).";
                    MessageIsError = false;
                    return RedirectToPage();
                }

                _logger.LogWarning("Creazione tenant {Tenant} fallita, codice {Code}", TenantName, ret);
                Message = $"Creazione non riuscita (codice {ret}).";
                MessageIsError = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Index.cshtml.cs: creazione tenant");
                Message = "Errore durante la creazione del tenant.";
                MessageIsError = true;
            }

            LoadTenants();
            return Page();
        }

        private void LoadTenants()
        {
            try
            {
                var elenco = new Workspaces();
                elenco.loadWorkspaces();
                Tenants = elenco.workspaces;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Index.cshtml.cs: caricamento tenants");
                Tenants = new List<Workspace>();
            }
        }
    }
}
