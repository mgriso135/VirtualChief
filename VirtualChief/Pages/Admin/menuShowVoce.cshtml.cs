using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Admin
{
    /// <summary>
    /// Migrated from WebForms Admin/menuShowVoce.aspx + menuShowFigli.ascx + menuAddVoceFiglia.ascx
    /// Permissions: "Menu Voce" R (read), "Menu Voce" W (write)
    /// </summary>
    public class menuShowVoceModel : PageModel
    {
        private readonly ILogger<menuShowVoceModel> _logger;

        public menuShowVoceModel(ILogger<menuShowVoceModel> logger)
        {
            _logger = logger;
        }

        public string NavManageMenu { get; private set; } = "Gestione Menu";
        public string NavGestioneVoce { get; private set; } = "Gestione Voce";

        public VoceMenu MenuItem { get; private set; }
        public bool CanReadChildMenu { get; private set; }
        public bool CanWriteChildMenu { get; private set; }

        public string Tenant => CurrentWorkspace.Of(User);

        private bool HasPermission(string nomePermesso, string level)
        {
            var uidStr = User.FindFirst("uid")?.Value;
            var tenant = Tenant;
            if (!int.TryParse(uidStr, out var uid) || string.IsNullOrEmpty(tenant))
            {
                return false;
            }
            try
            {
                var prm = new List<string[]> { new[] { nomePermesso, level } };
                return new UserAccount(uid).ValidatePermissions(tenant, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin/menuShowVoce: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public void OnGet(int id)
        {
            CanReadChildMenu = HasPermission("Menu Voce", "R");
            CanWriteChildMenu = HasPermission("Menu Voce", "W");

            var tenant = Tenant;
            if (string.IsNullOrEmpty(tenant) || id <= 0)
            {
                return;
            }

            try
            {
                var vm = new VoceMenu(id);
                if (vm.ID != -1)
                {
                    MenuItem = vm;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin/menuShowVoce: caricamento voce {Id}", id);
            }
        }
    }
}