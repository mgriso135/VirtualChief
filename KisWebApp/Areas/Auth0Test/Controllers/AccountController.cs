using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace KIS.Areas.Auth0Test.Controllers
{
    public class AccountController : Controller
    {
        public string Index()
        {
            return "TEST";
        }

        public ActionResult Login(string returnUrl)
        {
            HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? Url.Action("Index", "Home")
            },
                "Auth0");
            return new HttpUnauthorizedResult();
        }

        [Authorize]
        public void Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            HttpContext.GetOwinContext().Authentication.SignOut("Auth0");
            Session.Abandon();
        }

        [Authorize]
        public ActionResult Tokens()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            //((ClaimsIdentity)User.Identity).AddClaim(new Claim("DB", "DIOBOY"));


            // HttpContext.GetOwinContext().Set<>      <-- TEST THIS!!!



            ViewBag.AccessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
            ViewBag.IdToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

            return View();
        }

        [Authorize]
        public ActionResult Claims()
        {
            return View();
        }
    }
}