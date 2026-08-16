/* Copyright © 2026 -  Tutti i diritti riservati */

using System.Security.Claims;
using System.Web;

namespace KIS.App_Code
{
    /// <summary>
    /// Host-agnostic seam over the small System.Web surface the business layer
    /// uses. On IIS it delegates to the real HttpContext / Membership with
    /// identical behavior; outside a web request (console agents, Linux tests,
    /// ASP.NET Core) it returns empty defaults or a cross-platform equivalent
    /// instead of the original null-reference path. Single place to touch when
    /// the app moves off System.Web (see migration.md §7 Step 3).
    /// </summary>
    public static class WebEnv
    {
        /// <summary>
        /// Active workspace name from the session, else from the Owin
        /// "workspace" claim; "" when unknown or outside a web request.
        /// </summary>
        public static string ActiveWorkspaceName
        {
            get
            {
                String activeWorkspace = "";
                if (HttpContext.Current != null && HttpContext.Current.Session != null
                    && HttpContext.Current.Session["ActiveWorkspace_Name"] != null
                    && HttpContext.Current.Session["ActiveWorkspace_Id"] != null)
                {
                    activeWorkspace = HttpContext.Current.Session["ActiveWorkspace_Name"]?.ToString();
                }
                if (activeWorkspace.Length == 0)
                {
                    ClaimsPrincipal user = HttpContext.Current != null
                        ? HttpContext.Current.GetOwinContext().Authentication.User
                        : null;
                    var claimsIdentity = user?.Identity as ClaimsIdentity;
                    activeWorkspace = claimsIdentity?.FindFirst(c => c.Type.Contains("workspace"))?.Value;
                    if (activeWorkspace == null || activeWorkspace.Length == 0)
                    {
                        activeWorkspace = "";
                    }
                }
                return activeWorkspace;
            }
        }

        /// <summary>
        /// Active workspace id from the session, else from the Owin
        /// "workspace_id" claim; -1 when unknown or outside a web request.
        /// </summary>
        public static int ActiveWorkspaceId
        {
            get
            {
                int activeWorkspace = -1;
                if (HttpContext.Current != null && HttpContext.Current.Session != null
                    && HttpContext.Current.Session["ActiveWorkspace_Name"] != null)
                {
                    activeWorkspace = Int32.Parse(HttpContext.Current.Session["ActiveWorkspace_Id"]?.ToString());
                }
                if (activeWorkspace == -1)
                {
                    ClaimsPrincipal user = HttpContext.Current != null
                        ? HttpContext.Current.GetOwinContext().Authentication.User
                        : null;
                    var claimsIdentity = user?.Identity as ClaimsIdentity;
                    String sactiveWorkspace = claimsIdentity?.FindFirst(c => c.Type.Contains("workspace_id"))?.Value;
                    if (sactiveWorkspace != null && sactiveWorkspace.Length > 0)
                    {
                        activeWorkspace = Int32.Parse(sactiveWorkspace);
                    }
                }
                return activeWorkspace;
            }
        }

        /// <summary>
        /// Random password. On .NET Framework this is the exact ASP.NET
        /// Membership.GeneratePassword; elsewhere a crypto-RNG equivalent.
        /// </summary>
        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
#if NETFRAMEWORK
            return System.Web.Security.Membership.GeneratePassword(length, numberOfNonAlphanumericCharacters);
#else
            const string alphabet =
                "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@#$%^&*()_-+=";
            byte[] bytes = new byte[length];
            System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
            var sb = new System.Text.StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(alphabet[bytes[i] % alphabet.Length]);
            }
            return sb.ToString();
#endif
        }
    }
}