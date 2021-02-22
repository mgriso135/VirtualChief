using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Areas.Analysis.Controllers
{
    public class OperatorsAnalysisController : Controller
    {
        // GET: Analysis/OperatorsAnalysis
        public ActionResult GetOperatorProductivity(String user, DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User cu1rr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction( cu1rr.username, "Controller", "/Analysis/OperatorsAnalysis/GetOperatorProductivity", "user="+user+"&start="+start.ToString("dd/MM/yyyy HH:mm:ss")+"&end="+end.ToString("dd/MM/yyyy HH:mm:ss"), ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/OperatorsAnalysis/GetOperatorProductivity", "user=" + user + "&start=" + start.ToString("dd/MM/yyyy HH:mm:ss") + "&end=" + end.ToString("dd/MM/yyyy HH:mm:ss"), ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Operatori Tempi";
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
                User currUsr = new User(user);
                if(currUsr!=null && currUsr.username.Length > 0)
                {
                    currUsr.LoadProductivity(start, end);
                    ViewBag.IntervalliDiLavoro = currUsr.IntervalliDiLavoroOperatore;
                    ViewBag.Productivity = Math.Round(100*currUsr.Productivity, 2);
                    return View();
                }
            }


                return View();
        }

        public ActionResult GetOperatorOccupation(String user, DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User cu1rr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(cu1rr.username, "Controller", "/Analysis/OperatorsAnalysis/GetOperatorOccupation", "user=" + user + "&start=" + start.ToString("dd/MM/yyyy HH:mm:ss") + "&end=" + end.ToString("dd/MM/yyyy HH:mm:ss"), ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/OperatorsAnalysis/GetOperatorOccupation", "user=" + user + "&start=" + start.ToString("dd/MM/yyyy HH:mm:ss") + "&end=" + end.ToString("dd/MM/yyyy HH:mm:ss"), ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Operatori Tempi";
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
                User currUsr = new User(user);
                if (currUsr != null && currUsr.username.Length > 0)
                {
                    currUsr.LoadOccupation(start, end);
                    ViewBag.Occupation = Math.Round(100 * currUsr.Occupation, 2);
                    ViewBag.log = currUsr.log;
                    return View();
                }
            }


            return View();
        }
    }
}