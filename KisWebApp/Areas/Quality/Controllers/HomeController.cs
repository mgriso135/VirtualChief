using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Quality.Controllers
{
    public class HomeController : Controller
    {
        // GET: Quality/Home
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/Home/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/Home/Index", "", ipAddr);
            }

            ViewBag.authNCCause = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Causes";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authNCCause = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ViewBag.authNCType = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "NonCompliance Types";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authNCType = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ViewBag.authNC = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "NonCompliances";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authNC = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ViewBag.authNCAnalysisNum = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Num";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authNCAnalysisNum = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ViewBag.authNCAnalysisCost = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Cost";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authNCAnalysisCost = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ViewBag.authNCAnalysisProduct = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "NonCompliancesAnalysis Product";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authNCAnalysisProduct = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            ViewBag.authImprovementActions = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "ImprovementActions";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authImprovementActions = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            return View();
        }
    }
}