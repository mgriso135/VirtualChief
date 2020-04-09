using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Areas.Analysis.Controllers
{
    public class ProcessMiningController : Controller
    {
        // GET: Analysis/ProcessMining
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User cu1rr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(cu1rr.username, "Controller", "/Analysis/ProcessMining/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProcessMining/Index", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Process Mining";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }
            ViewBag.Productivity = -1;
            if (ViewBag.authR)
            {
            }
            return View();
        }
    }
}