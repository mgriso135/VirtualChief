﻿using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using KIS.App_Sources;
using System.Net.Mail;
using System;
using System.Linq;

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
            AuthenticationProperties properties = new AuthenticationProperties();
            properties.RedirectUri = returnUrl ?? Url.Action("AfterLogin", "Account");

            HttpContext.GetOwinContext().Authentication.Challenge(properties
                /*new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? Url.Action("Index", "Home")
            }*/,
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
            String redUr = "";
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
            Boolean mail_verified = claimsIdentity?.FindFirst(c => c.Type.Contains("email_verified"))?.Value == "true" ? true : false;
            //DateTime created_at = DateTime.UtcNow;
            ViewBag.log += "usrId = " + usrId + " <br />";
            UserAccount curr = new UserAccount(usrId);
            ViewBag.log += "curr.userId = " + curr.userId + "<br />";
            if (curr.id == -1)
            {
                UserAccounts lst = new UserAccounts();
                int added_user = lst.Add(usrId, email, firstname, lastname, nickname, picture_url, locale, update_at, iss, nonce, idtoken, access_token, refresh_token);
                ViewBag.log += "Added user: " + added_user;

                if(added_user == 1)
                {
                    ViewBag.log += "mail_verified1 " + mail_verified + "    ";
                    if (mail_verified)
                    {
                        ViewBag.log += "Redirecting1...";
                        // Then go to add a new workspace
                        redUr = "AddWorkspaceForm";
                    }
                    else
                    {
                        // Please, verify e-mail...
                        redUr = "VerifyEmail";
                    }
                }
                else
                {
                    ViewBag.log += "Error while adding user";
                }
            }
            else
            {
                // if e-mail is verified
                ViewBag.log += "mail_verified2 " + mail_verified + "    ";
                if(mail_verified)
                {
                    curr.loadWorkspaces();
                    curr.loadDefaultWorkspace();
                    if(curr.DefaultWorkspace != null)
                    {
                        Session["ActiveWorkspace"] = curr.DefaultWorkspace.Name;
                        Session["ActiveWorkspace_Id"] = curr.DefaultWorkspace.id;
                        redUr = "~/HomePage/Default.aspx";
                    }
                    else if(curr.workspaces.Count > 0)
                    {
                        Session["ActiveWorkspace"] = curr.workspaces[0].Name;
                        Session["ActiveWorkspace_Id"] = curr.workspaces[0].id;
                        redUr = "~/HomePage/Default.aspx";
                    }
                    else
                    {
                        redUr = "AddWorkspaceForm";
                    }
                    
                }
                else
                {

                    redUr = "VerifyEmail";
                    ViewBag.log = redUr;
                }
            }
            ViewBag.log = redUr;
            ViewBag.redirectUrl = redUr;
            return View(new Uri(redUr, UriKind.Relative));
        }

        [Authorize]
        public ActionResult AddWorkspaceForm()
        {
            ViewBag.log = "";
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything's ok
         * -20 if user not found or mail not verified
         * -21 if name invalid or name already exists
         */
        [Authorize]
        public int AddWorkspace(String ws_name)
        {
            int ret = 0;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string usrId = claimsIdentity?.FindFirst(c => c.Type.Contains("nameidentifier"))?.Value;
            UserAccount usr = new UserAccount(usrId);
            Boolean mail_verified = claimsIdentity?.FindFirst(c => c.Type.Contains("email_verified"))?.Value == "true" ? true : false;
            if (usr.id!=-1 && mail_verified)
            {
                Workspaces lst_ws = new Workspaces();
                lst_ws.loadWorkspaces();
                bool alreadyexists = lst_ws.workspaces.FirstOrDefault(x => x.Name.ToLower() == ws_name.ToLower()) == null ? false : true;
                if (ws_name.Length > 0 && ws_name.Length < 255 && ws_name.All(Char.IsLetter) && !alreadyexists)
                { 
                    ret = usr.addWorkspace(ws_name);
                    if(ret > 0)
                    {
                        Session["ActiveWorkspace"] = ws_name;
                        Session["ActiveWorkspace_Id"] = ret;
                    }
                }

                else
                {
                    ret = -21;
                }
            }
            else
            {
                ret = -20;
            }
            return ret;
        }

        [Authorize]
        public ActionResult VerifyEmail()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            HttpContext.GetOwinContext().Authentication.SignOut("Auth0");
            Session.Abandon();
            return View();
        }
    }

}