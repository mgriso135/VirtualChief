using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChief.Pages.Login
{
    /// <summary>
    /// Workspace (tenant) selection, shown after sign-in when the user has
    /// access to more than one workspace. Replaces the legacy workspace
    /// switching of Session["ActiveWorkspace_Name"]. There is no default:
    /// nothing else is reachable until a workspace is chosen.
    ///
    /// Memberships are resolved SERVER-SIDE on every request through the legacy
    /// UserAccount.loadWorkspaces() (Account.cs): workspaces INNER JOIN
    /// useraccountworkspaces WHERE userid=@id — cookie claims are never trusted
    /// for this list.
    /// </summary>
    public class selectWorkspaceModel : PageModel
    {
        private readonly ILogger<selectWorkspaceModel> _logger;

        public selectWorkspaceModel(ILogger<selectWorkspaceModel> logger)
        {
            _logger = logger;
        }

        /// <summary>vcmain useraccounts.id of the signed-in user (-1 when unavailable).</summary>
        public int UserAccountId { get; set; } = -1;

        public List<Workspace> Workspaces { get; set; } = new List<Workspace>();

        public string ActiveWorkspace => CurrentWorkspace.Of(User);

        public void OnGet()
        {
            LoadWorkspaces();
        }

        public IActionResult OnPost(string workspace)
        {
            if (string.IsNullOrEmpty(workspace))
            {
                return RedirectToPage();
            }

            // Rebuild the identity preserving existing claims, swapping the tenant.
            var claims = User.Claims
                .Where(c => c.Type != CurrentWorkspace.ClaimType)
                .Select(c => new Claim(c.Type, c.Value))
                .ToList();
            claims.Add(new Claim(CurrentWorkspace.ClaimType, workspace));

            _logger.LogInformation("Utente {User} seleziona il workspace {Workspace}", User.Identity.Name, workspace);

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties { IsPersistent = false }).Wait();

            return LocalRedirect("~/");
        }

        private void LoadWorkspaces()
        {
            var uidStr = User.FindFirst("uid")?.Value;

            // Auth0 identities carry the vcmain useraccounts id: use the legacy loader.
            if (int.TryParse(uidStr, out var uid))
            {
                try
                {
                    var ua = new UserAccount(uid);
                    if (ua.id != -1)
                    {
                        UserAccountId = ua.id;
                        ua.loadWorkspaces();
                        Workspaces = ua.workspaces ?? new List<Workspace>();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Login/selectWorkspace: caricamento workspace per useraccount {Uid}", uid);
                }
            }

            // Forms-fallback users have no vcmain account: only the active tenant.
            var active = ActiveWorkspace;
            if (!string.IsNullOrEmpty(active))
            {
                Workspaces = new List<Workspace> { new Workspace(active) };
            }
        }
    }
}
