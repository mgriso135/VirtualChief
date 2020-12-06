﻿using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using KIS.App_Sources;
using System.Net.Mail;
using System;

namespace KIS.Areas.AccountsMgm.Controllers
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
            ViewBag.AccessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
            ViewBag.IdToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

            return View();
        }

        [Authorize]
        public ActionResult Claims()
        {
            return View();
        }

        /* Cases:
         * 0) If user e-mail has not been verified, ask to verify the e-mail and login again. Logout the user.
         * 1) User belongs to 1 workspace: go to that workspace
         * 2) User belongs to n > 1 workspaces: ask what workspace to use
         * 3) User e-mail has an invitation to an existing workspace
         * 4) User e-mail has nothing: propose to create a new workspace
         */

        [Authorize]
        public ActionResult AfterLogin()
        {
            ViewBag.log = "";
            var claimsIdentity = User.Identity as ClaimsIdentity;
            ViewBag.AccessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
            ViewBag.IdToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

            // If useraccount does not exist, add it
            // If useraccount exists:
            //      if user is not member of a workspace, propose to create a new one
            //      else if user is member of some workspace, go to the default one
            string usrId = claimsIdentity?.FindFirst(c => c.Type.Contains("nameidentifier"))?.Value;
            MailAddress email = new MailAddress(claimsIdentity?.FindFirst(c => c.Type.Contains("emailaddress"))?.Value);
            string firstname = claimsIdentity?.FindFirst(c => c.Type == "name")?.Value;
            string lastname = claimsIdentity?.FindFirst(c => c.Type.Contains("surname"))?.Value;
            string nickname = claimsIdentity?.FindFirst(c => c.Type == "nickname")?.Value;
            string picture_url = claimsIdentity?.FindFirst(c => c.Type == "picture")?.Value;
            string locale = claimsIdentity?.FindFirst(c => c.Type == "locale")?.Value;
            DateTime update_at = DateTime.Parse(claimsIdentity?.FindFirst(c => c.Type == "updated_at")?.Value);
            string iss = claimsIdentity?.FindFirst(c => c.Type.Contains("iss"))?.Value;
            string idtoken = claimsIdentity?.FindFirst(c => c.Type.Contains("id_token"))?.Value;
            string nonce = claimsIdentity?.FindFirst(c => c.Type == "nonce")?.Value;
            string access_token = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
            string refresh_token = claimsIdentity?.FindFirst(c => c.Type.Contains("refresh_token"))?.Value;
            //DateTime created_at = DateTime.UtcNow;
            ViewBag.log += "usrId = " + usrId + " <br />";
            UserAccount curr = new UserAccount(usrId);
            ViewBag.log += "curr.userId = " + curr.userId + "<br />";
            if (curr.id == -1)
            {
                UserAccounts lst = new UserAccounts();
                int added_user = lst.Add(usrId, email, firstname, lastname, nickname, picture_url, locale, update_at, iss, nonce, idtoken, access_token, refresh_token);
                ViewBag.log += "Added user: " + added_user;
            }

            return View();
        }
    }
}