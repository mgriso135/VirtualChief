using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Production.Controllers
{
    public class ProductDetailsController : Controller
    {
        // GET: Production/ProductDetail
        [Authorize]
        public ActionResult Index(int ProductID, int ProductYear)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if(ViewBag.authR)
            {
                if(ProductID!=null && ProductYear!=null && ProductID!=-1 && ProductYear>2010)
                {
                    Articolo prod = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                        if(prod!=null && prod.ID!=-1 && prod.Year!=-1)
                    {
                        prod.loadTasksProduzione();
                        prod.loadTaskParameters();
                        prod.loadTempoDiLavoroTotale();

                        return View(prod);
                    }
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult ViewTaskParameters(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ViewTaskParameters", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ViewTaskParameters", "TaskID=" + TaskID, ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if(tsk!=null && tsk.TaskProduzioneID!=-1)
                {
                    tsk.loadParameters();
                    return View(tsk.Parameters);
                }
            }
                return View();
        }

        [Authorize]
        public ActionResult ViewTaskOperatorNotes(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ViewTaskOperatorNotes", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ViewTaskOperatorNotes", "TaskID=" + TaskID, ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk != null && tsk.TaskProduzioneID != -1)
                {
                    tsk.loadTaskOperatorNotes();
                    return View(tsk.TaskOperatorNotes);
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult ViewTaskOperationsDetails(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ViewTaskOperationsDetails", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ViewTaskOperationsDetails", "TaskID=" + TaskID, ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk != null && tsk.TaskProduzioneID != -1)
                {
                    tsk.loadIntervalliDiLavoroEffettivi();
                    var ints = tsk.Intervalli.OrderBy(x => x.Username);
                    return View(ints);
                }
            }
            return View();
        }

        [Authorize]
        public Boolean ExhumateTask(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Analysis/ProductionHistory/ExhumateTask", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Analysis/ProductionHistory/ExhumateTask", "TaskID = " + TaskID, ipAddr);
            }

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "TaskProduzione Riesuma";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ckUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ckUser == true)
            {
                TaskProduzione tskProd = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tskProd.Status == 'F')
                {
                    ret = tskProd.Riesuma();
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }

    }
}