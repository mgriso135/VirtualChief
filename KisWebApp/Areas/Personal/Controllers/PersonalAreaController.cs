﻿using KIS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;

namespace KIS.Areas.Personal.Controllers
{
    public class PersonalAreaController : Controller
    {
        // GET: Personal/PersonalArea
        [Authorize]
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Personal/PersonalArea/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Personal/PersonalArea/Index", "", ipAddr);
            }

            return View();
        }

        [Authorize]
        public ActionResult EditDestinationURL()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Personal/PersonalArea/EditDestinationURL", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Personal/PersonalArea/EditDestinationURL", "", ipAddr);
            }

            ViewBag.authW = false;
            UserAccount curr = (UserAccount)Session["user"];
            if(curr !=null && curr.userId.Length > 0)
            {
                ViewBag.authW = true;
                return View(curr);
            }
            return View();
        }

        /*Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if user not authorized
         */
        [Authorize]
        public int SaveDestinationURL(String destUrl)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Action", "/Personal/PersonalArea/SaveDestinationURL", "destUrl="+destUrl, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Personal/PersonalArea/SaveDestinationURL", "destUrl=" + destUrl, ipAddr);
            }

            int ret = 0;
            UserAccount curr = (UserAccount)Session["user"];
            if (curr != null && curr.userId.Length > 0)
            {
                curr.DestinationURL = destUrl;
                ret = 1;
            }
            else
            {
                ret = 2;
            }
            return ret;
        }
    }
}