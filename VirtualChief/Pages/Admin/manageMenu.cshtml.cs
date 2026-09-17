using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages.Admin
{
    /// <summary>
    /// Migrated from WebForms Admin/manageMenu.aspx + menuAddMainVoce.ascx + menuMainVociList.ascx
    /// Permissions: "Menu Voce" R (read), "Menu Voce" W (write)
    /// </summary>
    public class manageMenuModel : PageModel
    {
        private readonly ILogger<manageMenuModel> _logger;

        public manageMenuModel(ILogger<manageMenuModel> logger)
        {
            _logger = logger;
        }

        public string NavManageMenu { get; private set; } = "Gestione Menu";

        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; } = "info";

        public bool CanReadMainMenu { get; private set; }
        public bool CanWriteMainMenu { get; private set; }

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
                _logger.LogError(ex, "Admin/manageMenu: verifica permesso {Permission}", nomePermesso);
                return false;
            }
        }

        public void OnGet()
        {
            CanReadMainMenu = HasPermission("Menu Voce", "R");
            CanWriteMainMenu = HasPermission("Menu Voce", "W");
        }
    }
}