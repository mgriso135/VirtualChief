using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Login
{
    /// <summary>
    /// Migrated from WebForms Login/login.aspx + loginbox.ascx.
    /// Primary path: Auth0 OpenID Connect (legacy OWIN Startup.cs) — the button
    /// challenges the "Auth0" scheme. Fallback path: forms login against the
    /// per-tenant users tables via KIS.App_Code.User, kept for parity with the
    /// legacy forms-login box.
    ///
    /// No default tenant: after sign-in the active workspace is either the
    /// user's configured DefaultWorkspace or chosen on /Login/selectWorkspace.
    /// </summary>
    [AllowAnonymous]
    public class loginModel : PageModel
    {
        private readonly ILogger<loginModel> _logger;

        public loginModel(ILogger<loginModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        // Legacy resx: loginbox.ascx.lblErrorWrongAccess
        public const string MsgWrongAccess = "Errore: username non trovato o password non corretta.";

        public bool IsAuthenticated => User.Identity?.IsAuthenticated == true;

        public bool Auth0Failed => Request.Query["error"] == "auth0";

        public void OnGet()
        {
        }

        /// <summary>Primary login: redirect to the Auth0 Universal Login page.</summary>
        public IActionResult OnPostAuth0(string red)
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = string.IsNullOrEmpty(red) ? "/" : "/"
            };
            return Challenge(props, "Auth0");
        }

        public async Task<IActionResult> OnPostLoginAsync(string red)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Password))
            {
                Message = MsgWrongAccess;
                return Page();
            }

            var usr = HtmlEncode(Username);
            var pwd = HtmlEncode(Password);

            List<KIS.App_Sources.Workspace> workspaces;
            try
            {
                var elenco = new Workspaces();
                elenco.loadWorkspaces();
                workspaces = elenco.workspaces ?? new List<KIS.App_Sources.Workspace>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login/login.cshtml.cs: caricamento workspaces");
                Message = MsgWrongAccess;
                return Page();
            }

            var memberships = new List<(KIS.App_Sources.Workspace ws, User user)>();
            foreach (var ws in workspaces)
            {
                try
                {
                    var candidate = new User(ws.Name, usr, pwd);
                    if (candidate.authenticated)
                    {
                        memberships.Add((ws, candidate));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Login/login.cshtml.cs: verifica credenziali su {Workspace}", ws.Name);
                }
            }

            if (memberships.Count == 0)
            {
                Message = MsgWrongAccess;
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usr),
                new Claim(ClaimTypes.Name, usr)
            };

            if (memberships.Count == 1)
            {
                var (ws, user) = memberships[0];
                claims.Add(new Claim(CurrentWorkspace.ClaimType, ws.Name));
                claims.Add(new Claim("workspace_id", ws.id.ToString()));
                claims.Add(new Claim("firstname", user.name ?? ""));
                claims.Add(new Claim("lastname", user.cognome ?? ""));
                await SignInAsync(claims);
                _logger.LogInformation("Utente {User} autenticato (forms) sul workspace {Workspace}", usr, ws.Name);
                return LocalRedirect("~/");
            }

            foreach (var (ws, _) in memberships)
            {
                claims.Add(new Claim(CurrentWorkspace.CandidateClaimType, ws.Name));
            }
            await SignInAsync(claims);
            _logger.LogInformation("Utente {User} autenticato (forms) su {Count} workspace: selezione richiesta",
                usr, memberships.Count);
            return RedirectToPage("/Login/selectWorkspace");
        }

        private async Task SignInAsync(List<Claim> claims)
        {
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties { IsPersistent = false });
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Local cookie + remote Auth0 session (/v2/logout), like legacy.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return SignOut("Auth0");
        }

        /// <summary>Legacy Server.HtmlEncode equivalent for input sanitization.</summary>
        private static string HtmlEncode(string s)
        {
            return System.Net.WebUtility.HtmlEncode(s);
        }
    }
}
