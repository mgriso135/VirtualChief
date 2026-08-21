using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace VirtualChief.Pages.Login
{
    /// <summary>
    /// Workspace (tenant) selection, shown after sign-in when the user has
    /// access to more than one workspace. Replaces the legacy workspace
    /// switching of Session["ActiveWorkspace_Name"]. There is no default:
    /// nothing else is reachable until a workspace is chosen.
    /// </summary>
    public class selectWorkspaceModel : PageModel
    {
        private readonly ILogger<selectWorkspaceModel> _logger;

        public selectWorkspaceModel(ILogger<selectWorkspaceModel> logger)
        {
            _logger = logger;
        }

        public IReadOnlyList<string> Candidates { get; set; } = new List<string>();

        public bool HasActiveWorkspace => !string.IsNullOrEmpty(CurrentWorkspace.Of(User));

        public void OnGet()
        {
            var candidates = CurrentWorkspace.Candidates(User).Distinct().ToList();
            if (!candidates.Contains(CurrentWorkspace.Of(User)) && HasActiveWorkspace)
            {
                candidates.Add(CurrentWorkspace.Of(User));
            }
            Candidates = candidates;
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
    }
}
