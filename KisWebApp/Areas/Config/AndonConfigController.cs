using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Areas.Config
{
    public class AndonConfigController : Controller
    {
        // GET: Config/AndonConfig
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Config/AndonConfig/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Config/AndonConfig/Index", "", ipAddr);
            }

            return View();
        }

        public ActionResult ScrollTypeView()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Config/AndonConfig/ScrollTypeView", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Config/AndonConfig/ScrollTypeView", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonCompleto ScrollType";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW == true)
            {
                AndonCompleto andonCfg = new AndonCompleto();
                andonCfg.loadScrollType();
                ViewBag.ScrollType = andonCfg.ScrollType;
                ViewBag.ContinuousScrollGoSpeed = andonCfg.ContinuousScrollGoSpeed;
                ViewBag.ContinuousScrollBackSpeed = andonCfg.ContinuousScrollBackSpeed;
            }
            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if ok
         */
        public int ScrollTypeEdit(int ScrollType, String ScrollParams)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Config/AndonConfig/ScrollTypeEdit", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Config/AndonConfig/ScrollTypeEdit", "", ipAddr);
            }

            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonCompleto ScrollType";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW == true)
            {
                AndonCompleto andonCfg = new AndonCompleto();
                ret = andonCfg.setScrollType(ScrollType, ScrollParams);
            }
            return ret;
        }


        public ActionResult DepartmentScrollTypeView(int DepartmentID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Config/AndonConfig/DepartmentScrollTypeView", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Config/AndonConfig/DepartmentScrollTypeView", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonCompleto ScrollType";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW == true)
            {
                ViewBag.DepartmentID = DepartmentID;
                AndonReparto andonCfg = new AndonReparto(DepartmentID);
                andonCfg.loadScrollType();
                ViewBag.ScrollType = andonCfg.ScrollType;
                ViewBag.ContinuousScrollGoSpeed = andonCfg.ContinuousScrollGoSpeed;
                ViewBag.ContinuousScrollBackSpeed = andonCfg.ContinuousScrollBackSpeed;
            }
            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if ok
         */
        public int DepartmentScrollTypeEdit(int DepartmentID, int ScrollType, String ScrollParams)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Config/AndonConfig/DepartmentScrollTypeEdit", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Config/AndonConfig/DepartmentScrollTypeEdit", "", ipAddr);
            }

            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonCompleto ScrollType";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW == true)
            {
                AndonReparto andonCfg = new AndonReparto(DepartmentID);
                ret = andonCfg.setScrollType(ScrollType, ScrollParams);
            }
            return ret;
        }

    }
}