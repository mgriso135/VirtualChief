using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Users
{
    /// <summary>
    /// Migrated from WebForms Users/manageUsers.aspx + listUsers.ascx:
    /// user accounts registered on the workspace (vcmain.useraccounts).
    /// </summary>
    public class listUsersModel : PageModel
    {
        private readonly ILogger<listUsersModel> _logger;

        public listUsersModel(ILogger<listUsersModel> logger)
        {
            _logger = logger;
        }

        public List<UserAccount> Users { get; set; }

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
                var ws = new Workspace(tenant);
                ws.loadUserAccounts();
                Users = ws.UserAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Users/listUsers.cshtml.cs");
                Users = new List<UserAccount>();
            }
        }
    }
}
