// Minimal stubs so the legacy .NET Framework business classes (KisWebApp/App_Sources,
// namespace KIS.App_Code / KIS.App_Sources) compile on .NET Core / Linux.
// These are COMPILE-ONLY shims: the web-only code paths are never executed by tests.
using System.Security.Claims;

namespace System.Web
{
    public class HttpContext
    {
        public static HttpContext? Current { get; set; }
        public HttpSessionState? Session { get; set; }
        public object? Server { get; set; }
        public OwinContext GetOwinContext() => new();
    }

    public class HttpSessionState
    {
        public object? this[string key] => null;
    }

    public class OwinContext
    {
        public Authentication Authentication { get; } = new();
    }

    public class Authentication
    {
        public ClaimsPrincipal User { get; set; } = new();
    }
}

namespace System.Web.Security
{
    public static class Membership
    {
        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            var s = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
            return length <= s.Length ? s[..length] : s + new string('x', length - s.Length);
        }
    }
}

// Empty namespaces to satisfy legacy `using System.Web.Mvc;` / `using System.Web.Hosting;`
// directives in files whose only MVC/Hosting usage is (in the tested layer) unused.
namespace System.Web.Mvc
{
}

namespace System.Web.Hosting
{
}

