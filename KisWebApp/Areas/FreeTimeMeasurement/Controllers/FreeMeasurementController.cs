using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.FreeTimeMeasurement.Controllers
{
    public class FreeMeasurementController : Controller
    {
        // GET: FreeTimeMeasurement/FreeMeasurement
        public ActionResult Index(char Status = 'O')
        {
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                FreeTimeMeasurements measList = new FreeTimeMeasurements();
                measList.loadMeasurements(Status);
                return View(measList.MeasurementsList);
            }
                return View();
        }

        /* Returns:
         * FreeMeasurementId if everything is ok
         * -1 if generic error
         * -3 if error while adding
         * -12 if error in input data - name
         * -13 if error in input data - plannedstartdate
         * -14 if error in input data - plannedenddate
         * -15 if error in input data - department
         * -16 if error in input data - process not found
         * -17 if error in input data - serialnumber
         * -18 if error in input data - quantity
         * -19 if error in input data - measurement unit
         * -20 if user is not authorized
         */
        public int Add(DateTime plannedstartdate, DateTime plannedenddate, int DepartmentId, String name, String description, int processid, int processrev, int variantid,
            String serialnumber, Double quantity, int measurementUnitId, Boolean AllowCustomTasks, Boolean AllowExecuteFinishedTasks)
        {
            int ret = -1;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ret = name.Length > 255 ? -12 : -1;
                ret = plannedstartdate >= DateTime.UtcNow ? -13 : -1;
                ret = plannedenddate >= plannedstartdate ? -14 : -1;
                ret = quantity <= 0 ? -18 : -1;
                if (ret >= -1)
                {
                    FreeTimeMeasurements measList = new FreeTimeMeasurements();
                    User currUser = (User)Session["user"];
                    ret = measList.Add(currUser.username.ToString(), plannedstartdate, plannedenddate, DepartmentId, name, description, processid, processrev, variantid,
                        serialnumber, quantity, measurementUnitId, AllowCustomTasks, AllowExecuteFinishedTasks);
                }
                
            }
            else
            {
                ret = -20;
            }

            return ret;
        }
    }
}