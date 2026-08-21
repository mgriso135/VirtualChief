using System.Security.Claims;

namespace VirtualChief.Pages
{
    /// <summary>
    /// Access to the active workspace (tenant) carried in the auth cookie.
    /// Mirrors the legacy Session["ActiveWorkspace_Name"]: there is NO default
    /// tenant — every data page resolves the workspace from this claim and
    /// redirects to the selector when it is missing.
    /// </summary>
    public static class CurrentWorkspace
    {
        /// <summary>Claim holding the active workspace name.</summary>
        public const string ClaimType = "tenant";

        /// <summary>Claim listing a workspace the user may activate (multi-membership).</summary>
        public const string CandidateClaimType = "candidate_workspace";

        public static string Of(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimType)?.Value;
        }

        public static IReadOnlyList<string> Candidates(ClaimsPrincipal user)
        {
            return user?.FindAll(CandidateClaimType).Select(c => c.Value).ToList()
                   ?? new List<string>();
        }
    }
}
