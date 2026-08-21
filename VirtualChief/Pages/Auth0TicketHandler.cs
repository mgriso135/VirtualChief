using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.Pages
{
    /// <summary>
    /// Enrichment of the Auth0 OIDC ticket, porting the legacy flow of
    /// KisWebApp Startup.cs (SecurityTokenValidated) and AccountsMgm
    /// AccountController.AfterLogin:
    ///  1. auto-provision vcmain.useraccounts on first login (UserAccounts.Add);
    ///  2. resolve the active workspace: the user's DefaultWorkspace when set,
    ///     otherwise NO silent default — candidate workspaces are emitted as
    ///     claims and /Login/selectWorkspace asks the user (no default tenant);
    ///  3. emit profile claims consumed by the Razor Pages.
    /// </summary>
    public static class Auth0TicketHandler
    {
        public static Task OnTokenValidated(TokenValidatedContext context)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>().CreateLogger("Auth0TicketHandler");

            var principal = context.Principal;
            if (principal?.Identity?.IsAuthenticated != true)
            {
                return Task.CompletedTask;
            }

            string usrId = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            if (usrId.Length == 0)
            {
                logger.LogWarning("Ticket Auth0 privo di nameidentifier");
                context.Fail("Token Auth0 privo di nameidentifier");
                return Task.CompletedTask;
            }

            string GetString(params string[] types)
            {
                foreach (var t in types)
                {
                    var v = principal.FindFirstValue(t);
                    if (!string.IsNullOrEmpty(v)) return v;
                }
                return "";
            }

            // Claims emitted into the app cookie (long + short claim-type variants).
            var firstname = GetString(ClaimTypes.GivenName, "given_name");
            var lastname = GetString(ClaimTypes.Surname, "family_name");
            var email = GetString(ClaimTypes.Email, "email");
            bool mailVerified = GetString("email_verified", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email_verified")
                .Equals("true", StringComparison.OrdinalIgnoreCase);

            var appClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usrId),
                new Claim(ClaimTypes.Name, GetString("name", ClaimTypes.Name, usrId)),
                new Claim("firstname", firstname),
                new Claim("lastname", lastname),
                new Claim("email", email),
                new Claim("email_verified", mailVerified ? "true" : "false")
            };

            try
            {
                var curr = new UserAccount(usrId);
                if (curr.id == -1)
                {
                    // First login: auto-provision into vcmain.useraccounts
                    // (legacy AccountController.AfterLogin).
                    DateTime updatedAt = DateTimeOffset.UtcNow.UtcDateTime;
                    _ = DateTime.TryParse(
                        GetString("updated_at"),
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                        out updatedAt);

                    var addr = new System.Net.Mail.MailAddress(
                        string.IsNullOrEmpty(email) ? "unknown@localhost" : email);

                    var lst = new UserAccounts();
                    int added = lst.Add(
                        usrId, addr, firstname, lastname,
                        GetString("nickname"),
                        GetString("picture"),
                        GetString("locale"),
                        updatedAt,
                        GetString("iss", principal.FindFirstValue("issuer")),
                        "", // nonce: one-time
                        context.Properties?.GetTokenValue("id_token") ?? "",
                        context.Properties?.GetTokenValue("access_token") ?? "",
                        GetString("refresh_token"));

                    logger.LogInformation("Auto-provisioning utente {UsrId}: esito {Added}", usrId, added);
                    curr = added > 0 ? new UserAccount(usrId) : curr;
                }
                else
                {
                    curr.LastLogin = DateTime.UtcNow;
                }

                if (curr.id != -1)
                {
                    // vcmain numeric id, used to resolve groups/menu/permissions per workspace.
                    appClaims.Add(new Claim("uid", curr.id.ToString()));

                    curr.loadWorkspaces();
                    curr.loadDefaultWorkspace();

                    if (curr.DefaultWorkspace != null && curr.DefaultWorkspace.id != -1)
                    {
                        appClaims.Add(new Claim(CurrentWorkspace.ClaimType, curr.DefaultWorkspace.Name));
                        appClaims.Add(new Claim("workspace_id", curr.DefaultWorkspace.id.ToString()));
                        logger.LogInformation("{UsrId}: workspace attivo = {Ws} (default configurato)",
                            usrId, curr.DefaultWorkspace.Name);
                    }
                    else if (curr.workspaces != null && curr.workspaces.Count == 1)
                    {
                        // Single membership and no configured default: activate it directly.
                        appClaims.Add(new Claim(CurrentWorkspace.ClaimType, curr.workspaces[0].Name));
                        appClaims.Add(new Claim("workspace_id", curr.workspaces[0].id.ToString()));
                        logger.LogInformation("{UsrId}: workspace attivo = {Ws} (unico)",
                            usrId, curr.workspaces[0].Name);
                    }
                    else if (curr.workspaces != null && curr.workspaces.Count > 1)
                    {
                        foreach (var ws in curr.workspaces)
                        {
                            appClaims.Add(new Claim(CurrentWorkspace.CandidateClaimType, ws.Name));
                        }
                        logger.LogInformation("{UsrId}: {N} workspace candidati, selezione richiesta",
                            usrId, curr.workspaces.Count);
                    }
                    else
                    {
                        logger.LogInformation("{UsrId}: nessun workspace, creazione richiesta", usrId);
                    }
                }
                else if (!mailVerified)
                {
                    logger.LogWarning("{UsrId}: e-mail non verificata", usrId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Provisioning/risoluzione workspace fallita per {UsrId}", usrId);
                // Do not block authentication: user lands without active workspace.
            }

            var identity = (ClaimsIdentity)principal.Identity!;
            foreach (var c in appClaims)
            {
                var existing = identity.FindFirst(c.Type);
                if (existing != null)
                {
                    identity.TryRemoveClaim(existing);
                }
                identity.AddClaim(c);
            }

            return Task.CompletedTask;
        }
    }
}
