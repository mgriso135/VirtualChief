using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Areas.Departments.Controllers
{
    public class DepartmentsConfigController : Controller
    {
        // GET: Departments/DepartmentsConfig
        public ActionResult Index()
        {
            return View();
        }

        /* Returns:
         * 0 if Date
         * 1 if Week
         */
        public ActionResult getEndProductionDateFormat(int DepartmentID)
        {
            ViewBag.DepartmentID = -1;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Department EndProductionDateFormat";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Reparto currDepartment = new Reparto(DepartmentID);
                if(currDepartment.id>=0)
                {
                    ViewBag.DepartmentID = currDepartment.id;
                    currDepartment.loadEndProductionDateFormat();
                    ViewBag.EndProductionDateFormat = currDepartment.EndProductionDateFormat;
                }
            }
                return View();
        }

        /*Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if user not authorized or DateType value invalid
         */
        public int setEndProductionDateFormat(int DepartmentID, int DateType)
        {
            int ret = 0;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Department EndProductionDateFormat";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW && (DateType==0||DateType==1))
            {
                Reparto currDepartment = new Reparto(DepartmentID);
                if (currDepartment.id >= 0)
                {
                    currDepartment.EndProductionDateFormat = DateType;
                    currDepartment.loadEndProductionDateFormat();
                    ViewBag.EndProductionDateFormat = DateType;
                    ret = 1;
                }
            }
            else
            {
                ret = 2;
            }

            return ret;
        }

        /* Returns:
 * 0 if Date
 * 1 if Week
 */
        public ActionResult getDeliveryDateFormat(int DepartmentID)
        {
            ViewBag.DepartmentID = -1;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Department DeliveryDateFormat";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Reparto currDepartment = new Reparto(DepartmentID);
                if (currDepartment.id >= 0)
                {
                    ViewBag.DepartmentID = currDepartment.id;
                    currDepartment.loadDeliveryDateFormat();
                    ViewBag.DeliveryDateFormat = currDepartment.DeliveryDateFormat;
                }
            }
            return View();
        }

        /*Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if user not authorized or DateType value invalid
         */
        public int setDeliveryDateFormat(int DepartmentID, int DateType)
        {
            int ret = 0;
            ViewBag.authW = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Department DeliveryDateFormat";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW && (DateType == 0 || DateType == 1))
            {
                Reparto currDepartment = new Reparto(DepartmentID);
                if (currDepartment.id >= 0)
                {
                    currDepartment.DeliveryDateFormat = DateType;
                    currDepartment.loadDeliveryDateFormat();
                    ViewBag.DeliveryDateFormat = DateType;
                    ret = 1;
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