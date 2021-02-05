﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

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

        public ActionResult ChooseDepartment()
        {
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                ElencoReparti deptLst = new ElencoReparti();
                return View(deptLst.elenco);
            }
            return View();
        }

        public ActionResult Execute(int DepartmentId)
        {
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                Reparto dept = new Reparto(DepartmentId);
                if(dept.id!=-1)
                { 
                    ViewBag.DepartmentId = DepartmentId;
                }
                else
                {
                    ViewBag.DepartmentId = -1;
                }
            }
            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if user not authorized
         */
        public JsonResult GetFreeMeasurentsTasksJson(int departmentId)
        {
            JsonResult res = Json("");
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                FreeTimeMeasurements fms = new FreeTimeMeasurements();
                List<FreeMeasurentsTasksJsonStruct> fmStruct = fms.GetFreeMeasurentsTasksJson(departmentId);
                res = Json(fmStruct);
            }
            else
            {
                res = Json("2");
            }
            return res;
        }

        public int StartProductiveTask(String user, int MeasurementId, int TaskId)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                User usr = new App_Code.User(user) ;
                FreeMeasurement_Task frmTask = new FreeMeasurement_Task(MeasurementId, TaskId);
                // To-do: check that user it not 
                if(frmTask.TaskId !=-1 && usr.username.Length > 0)
                {
                    ret = frmTask.Start(usr);
                }
            }
            return ret;
        }

        /* Returns:
         */
        public int StartNewProductiveTask(String user, int MeasurementId, String TaskName)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                User usr = new App_Code.User(user);
                KIS.App_Sources.FreeTimeMeasurement fm = new KIS.App_Sources.FreeTimeMeasurement(MeasurementId);
                if(fm.id!=-1 && TaskName.Length < 255)
                {
                    int TaskId = fm.addTask(TaskName);
                    FreeMeasurement_Task frmTask = new FreeMeasurement_Task(MeasurementId, TaskId);
                
                if (frmTask.TaskId != -1 && usr.username.Length > 0)
                {
                    ret = frmTask.Start(usr);
                }
                }
                else
                {
                    ret = -1;
                }
            }
            return ret;
        }

        /* Returns:
         */
        public int StartNoProductiveTask(String user, int MeasurementId, int NpTaskId)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                User usr = new App_Code.User(user);
                KIS.App_Sources.FreeTimeMeasurement fm = new KIS.App_Sources.FreeTimeMeasurement(MeasurementId);
                NoProductiveTask npTask = new NoProductiveTask(NpTaskId);
                if (fm.id != -1 && npTask.ID != -1)
                {
                    int TaskId = fm.addTask(npTask);
                    FreeMeasurement_Task frmTask = new FreeMeasurement_Task(MeasurementId, TaskId);
                    if (frmTask.TaskId != -1 && usr.username.Length > 0)
                    {
                        ret = frmTask.Start(usr);
                    }
                }
                else
                {
                    ret = -1;
                }
            }
            return ret;
        }

        public int PauseTask(String user, int MeasurementId, int TaskId)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                User usr = new App_Code.User(user);
                FreeMeasurement_Task frmTask = new FreeMeasurement_Task(MeasurementId, TaskId);
                if (frmTask.TaskId != -1 && frmTask.Status == 'I' && usr.username.Length > 0)
                {
                    ret = frmTask.Pause(usr);
                }
            }
            return ret;
        }

        /* Returns:
         * 3 if user not found or department not found
         */
        public JsonResult GetRunningTasks(String username, int deptId)
        {
            JsonResult res = Json("");
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
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
                Reparto rp = new Reparto(deptId);
                User usr = new User(username);
                if (rp.id != -1 && usr.username.Length > 0)
                {
                    FreeTimeMeasurements fms = new FreeTimeMeasurements();
                    List<FreeMeasurentsTasksJsonStruct> fmStruct = fms.GetRunningTasks(rp, usr);
                    res = Json(fmStruct);
                }
                else
                {
                    res = Json("3");
                }
            }
            else
            {
                res = Json("2");
            }
            return res;
        }
    }

   

    
}