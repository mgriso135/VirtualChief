using KIS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;

namespace KIS.Areas.Config.Controllers
{
    public class MeasurementUnitsController : Controller
    {
        // GET: Config/MeasurementUnits
        public ActionResult Index()
        {
            // Check read permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config MeasurementUnits";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            // Check write permissions
            elencoPermessi = new List<String[]>();
            prmUser[0] = "Config MeasurementUnits";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }
            if (ViewBag.authR)
            {
                MeasurementUnits lstMU = new MeasurementUnits(Session["ActiveWorkspace"].ToString());
                lstMU.loadMeasurementUnits();
                return View(lstMU.UnitsList);
            }
                return View();
        }

        /* Returns: 
         * 0 if generic error
         * 1 if deleted successfully
         * 2 if user is not authorized
         * */
        public int DeleteMeasurementUnit(int uID)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config MeasurementUnits";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if(ViewBag.authW)
            {
                MeasurementUnit curr = new MeasurementUnit(Session["ActiveWorkspace"].ToString(), uID);
                if(curr.ID!=-1)
                { 
                    MeasurementUnits lst = new MeasurementUnits(Session["ActiveWorkspace"].ToString());
                    Boolean deleted = lst.Delete(uID);
                    if(deleted)
                    {
                        ret = 1;
                    }
                    else
                    {
                        ret = 0;
                    }
                }
                else
                {
                    ret = 0;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if ok
         * 2 if user is not authorized
         */
        public int EditMeasurementUnits(int uID, String uType, String uDescription, Boolean uDefault)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config MeasurementUnits";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                MeasurementUnit curr = new MeasurementUnit(Session["ActiveWorkspace"].ToString(), uID);
                curr.Type = uType;
                curr.Description = uDescription;
                curr.IsDefault = uDefault;
                ret = 1;
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        /* Returns:
        * 0 if generic error
        * 1 if ok
        * 2 if user is not authorized
        */
        public int AddMeasurementUnits(String uType, String uDescription, Boolean uDefault)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config MeasurementUnits";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                MeasurementUnits lst = new MeasurementUnits(Session["ActiveWorkspace"].ToString());
                Boolean added = lst.Add(uType, uDescription, uDefault);
                if (added) { ret = 1; }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }
    }
}