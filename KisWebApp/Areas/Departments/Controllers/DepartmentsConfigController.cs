using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;
using KIS.App_Code;

namespace KIS.Areas.Departments.Controllers
{
    public class DepartmentsConfigController : Controller
    {
        [Authorize]
        public ActionResult GetAutoPauseTaskConfig(int DepartmentID)
        {
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto AutoPauseTaskConfig";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ViewBag.AutoPauseConfig = true;
                ViewBag.deptID = -1;
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), DepartmentID);
                if(rp!=null && rp.id!=-1)
                {
                    ViewBag.deptID = rp.id;
                    ViewBag.AutoPauseConfig = rp.AutoPauseTasksOutsideWorkShifts;
                    return View();
                }
            }
                return View();
        }

        /*Returns:
        * 0 if generic error
        * 1 if all is ok
        * 2 if user not allowed
        * 3 if department not found
         */
        [Authorize]
        public int SetAutoPauseTaskConfig(int DepartmentID, Boolean Flag)
        {
            int ret = 0;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto AutoPauseTaskConfig";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Reparto dept = new Reparto(Session["ActiveWorkspace_Name"].ToString(), DepartmentID);
                if(dept!=null && dept.id!=-1)
                {
                    if(!Flag)
                    {
                        dept.AutoPauseTasksOutsideWorkShifts = false;
                    }
                    else
                    {
                        dept.AutoPauseTasksOutsideWorkShifts = true;
                    }
                    ret = 1;
                }
                else
                {
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
                return ret;
        }
    }
}