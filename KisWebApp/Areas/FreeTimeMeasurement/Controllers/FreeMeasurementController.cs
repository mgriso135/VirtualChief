using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Web.Hosting;
using jQuery_File_Upload.MVC5.Helpers;
using System.IO;

namespace KIS.Areas.FreeTimeMeasurement.Controllers
{
    public class FreeMeasurementController : Controller
    {
        // GET: FreeTimeMeasurement/FreeMeasurement
        public ActionResult Index(char Status = 'O')
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/Index", "Status=" + Status, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/Index", "Status=" + Status, ipAddr);
            }

            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"]!=null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            ViewBag.Status = "";
            if (ViewBag.authR || ViewBag.authW)
            {
                ViewBag.Status = Status;
                FreeTimeMeasurements measList = new FreeTimeMeasurements(Session["ActiveWorkspace_Name"].ToString());
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
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/Add", 
                    "plannedstartdate=" + plannedstartdate.ToString("dd/MM/yyyy HH:mm:ss") 
                    + "&plannedenddate="+ plannedenddate.ToString("dd/MM/yyyy HH:mm:ss") 
                    + "&DepartmentId="+DepartmentId
                    + "&name=" + name
                    + "&description=" + description
                    +"&processid="+processid
                    +"&processrev="+processrev
                    +"&variantid="+variantid
                    + "&serialnumber="+serialnumber
                    + "&quantity=" + quantity
                    + "&measurementUnitId="+measurementUnitId
                    + "&AllowCustomTasks="+AllowCustomTasks
                    +"&AllowExecuteFinishedTasks="+AllowExecuteFinishedTasks, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/Add",
                    "plannedstartdate=" + plannedstartdate.ToString("dd/MM/yyyy HH:mm:ss")
                    + "&plannedenddate=" + plannedenddate.ToString("dd/MM/yyyy HH:mm:ss")
                    + "&DepartmentId=" + DepartmentId
                    + "&name=" + name
                    + "&description=" + description
                    + "&processid=" + processid
                    + "&processrev=" + processrev
                    + "&variantid=" + variantid
                    + "&serialnumber=" + serialnumber
                    + "&quantity=" + quantity
                    + "&measurementUnitId=" + measurementUnitId
                    + "&AllowCustomTasks=" + AllowCustomTasks
                    + "&AllowExecuteFinishedTasks=" + AllowExecuteFinishedTasks, ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ret = name.Length > 255 ? -12 : -1;
                ret = plannedstartdate >= DateTime.UtcNow ? -13 : -1;
                ret = plannedenddate >= plannedstartdate ? -14 : -1;
                ret = quantity <= 0 ? -18 : -1;
                if (ret >= -1)
                {
                    FreeTimeMeasurements measList = new FreeTimeMeasurements(Session["ActiveWorkspace_Name"].ToString());
                    UserAccount currUser = (UserAccount)Session["user"];
                    ret = measList.Add(currUser.id.ToString(), plannedstartdate, plannedenddate, DepartmentId, name, description, processid, processrev, variantid,
                        serialnumber, quantity, measurementUnitId, AllowCustomTasks, AllowExecuteFinishedTasks);
                }
                
            }
            else
            {
                ret = -20;
            }

            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if finished successfully
         * 2 if status is already finished
         * 3 if some task is running
         * 4 if User not authorized
         */
        public int Finish(int MeasurementId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/Finish", "MeasurementId=" + MeasurementId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/Finish", "MeasurementId=" + MeasurementId, ipAddr);
            }

            int ret = -1;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                KIS.App_Sources.FreeTimeMeasurement fm = new KIS.App_Sources.FreeTimeMeasurement(Session["ActiveWorkspace_Name"].ToString(), MeasurementId);
                ret = fm.Finish();
            }
            else
            {
                ret = 4;
            }

            return ret;
        }

        public ActionResult ChooseDepartment()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/ChooseDepartment", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/ChooseDepartment", "", ipAddr);
            }

            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ElencoReparti deptLst = new ElencoReparti(Session["ActiveWorkspace_Name"].ToString());
                return View(deptLst.elenco);
            }
            return View();
        }

        public ActionResult Execute(int DepartmentId)
        {
            ViewBag.Tenant = "";
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/Execute", "DepartmentId="+ DepartmentId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/Execute", "DepartmentId="+ DepartmentId, ipAddr);
            }

            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Id"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            ViewBag.DepartmentName = "";
            ViewBag.DepartmentId = -1;
            ViewBag.WorkspaceId = -1;
            ViewBag.Tenant = Session["ActiveWorkspace_Name"].ToString();
            if (ViewBag.authW && ViewBag.Tenant.Length > 0)
            {
                Reparto dept = new Reparto(Session["ActiveWorkspace_Name"].ToString(), DepartmentId);
                if(dept.id!=-1)
                {
                    ViewBag.WorkspaceId = Int32.Parse(Session["ActiveWorkspace_Id"].ToString());
                    ViewBag.DepartmentId = DepartmentId;
                    ViewBag.DepartmentName = dept.name;
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
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetFreeMeasurentsTasksJson", "departmentId=" + departmentId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetFreeMeasurentsTasksJson", "departmentId=" + departmentId, ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                FreeTimeMeasurements fms = new FreeTimeMeasurements(Session["ActiveWorkspace_Name"].ToString());
                List<FreeMeasurentsTasksJsonStruct> fmStruct = fms.GetFreeMeasurentsTasksJson(departmentId);
                res = Json(fmStruct);
            }
            else
            {
                res = Json("2");
            }
            return res;
        }

        public int StartProductiveTask(int inputpoint, int MeasurementId, int TaskId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/StartProductiveTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&TaskId=" + TaskId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/StartProductiveTask",
                    "inputpoint=" + inputpoint + "&MeasurementId="+MeasurementId+"&TaskId="+TaskId, ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), inputpoint);
                FreeMeasurement_Task frmTask = new FreeMeasurement_Task(Session["ActiveWorkspace_Name"].ToString(), MeasurementId, TaskId);
                // To-do: check that user it not 
                if(frmTask.TaskId !=-1 && ip.id != -1)
                {
                    ret = frmTask.Start(ip);
                }
            }
            return ret;
        }

        /* Returns:
         * -2 if this measurement does not support custom tasks
         * -1 if Task not found
         * 0 if generic error
         * 1 if task started successfully
         * 2 if task already started
         * 3 if operator not found
         * 4 if operator exceeds max number of running tasks
         * 6 if user is already running the task
         */
        public int StartNewProductiveTask(int inputpoint, int MeasurementId, String TaskName)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/StartProductiveTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&TaskName=" + TaskName, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/StartProductiveTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&TaskName=" + TaskName, ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW && Session["ActiveWorkspace_Name"]!= null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), inputpoint);
                KIS.App_Sources.FreeTimeMeasurement fm = new KIS.App_Sources.FreeTimeMeasurement(Session["ActiveWorkspace_Name"].ToString(), MeasurementId);
                 
                if(fm.id!=-1 && TaskName.Length < 255)
                {
                    if (fm.AllowCustomTasks)
                    {
                        int TaskId = fm.addTask(TaskName);
                        FreeMeasurement_Task frmTask = new FreeMeasurement_Task(Session["ActiveWorkspace_Name"].ToString(), MeasurementId, TaskId);

                        if (frmTask.TaskId != -1 && ip.id != -1)
                        {
                            ret = frmTask.Start(ip);
                        }
                    }
                    else
                    {
                        ret = -2;
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
        public int StartNoProductiveTask(int inputpoint, int MeasurementId, int NpTaskId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/StartNoProductiveTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&NpTaskId=" + NpTaskId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/StartNoProductiveTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&NpTaskId=" + NpTaskId, ipAddr);
            }

            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"] !=null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), inputpoint);
                KIS.App_Sources.FreeTimeMeasurement fm = new KIS.App_Sources.FreeTimeMeasurement(Session["ActiveWorkspace_Name"].ToString(), MeasurementId);
                NoProductiveTask npTask = new NoProductiveTask(Session["ActiveWorkspace_Name"].ToString(), NpTaskId);
                if (fm.id != -1 && npTask.ID != -1)
                {
                    int TaskId = fm.addTask(npTask);
                    FreeMeasurement_Task frmTask = new FreeMeasurement_Task(Session["ActiveWorkspace_Name"].ToString(), MeasurementId, TaskId);
                    if (frmTask.TaskId != -1 && ip.id != -1)
                    {
                        ret = frmTask.Start(ip);
                    }
                }
                else
                {
                    ret = -1;
                }
            }
            return ret;
        }

        public int PauseTask(int inputpoint, int MeasurementId, int TaskId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/PauseTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&TaskId=" + TaskId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/PauseTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&TaskId=" + TaskId, ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), inputpoint);
                FreeMeasurement_Task frmTask = new FreeMeasurement_Task(Session["ActiveWorkspace_Name"].ToString(), MeasurementId, TaskId);
                if (frmTask.TaskId != -1 && frmTask.Status == 'I' && ip.id != -1)
                {
                    ret = frmTask.Pause(ip);
                }
            }
            return ret;
        }

        public int FinishTask(int inputpoint, int MeasurementId, int TaskId, Double ProducedQuantity)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/FinishTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&TaskId=" + TaskId + "&ProducedQuantity=" + ProducedQuantity, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/FinishTask",
                    "inputpoint=" + inputpoint + "&MeasurementId=" + MeasurementId + "&TaskId=" + TaskId + "&ProducedQuantity=" + ProducedQuantity, ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), inputpoint);
                FreeMeasurement_Task frmTask = new FreeMeasurement_Task(Session["ActiveWorkspace_Name"].ToString(), MeasurementId, TaskId);
                if (frmTask.TaskId != -1 && frmTask.Status == 'I' && ip.id >= 0)
                {
                    if(ProducedQuantity > 0)
                    { 
                        frmTask.ProducedQuantity = ProducedQuantity;
                    }
                    ret = frmTask.Finish(ip);
                }
            }
            return ret;
        }

        /* Returns:
         * 3 if user not found or department not found
         */
        public JsonResult GetRunningTasks(int inputpointid, int deptId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetRunningTasks",
                    "inputpointid=" + inputpointid + "&deptId=" + deptId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetRunningTasks",
                    "inputpointid=" + inputpointid + "&deptId=" + deptId, ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), deptId);
                InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), inputpointid);
                if (ip!=null && rp.id != -1 && ip.id >= 0)
                {
                    FreeTimeMeasurements fms = new FreeTimeMeasurements(Session["ActiveWorkspace_Name"].ToString());
                    List<FreeMeasurentsTasksJsonStruct> fmStruct = fms.GetRunningTasks(rp, ip);
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

        public ActionResult ViewMeasurementDetails(int MeasurementId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/ViewMeasurementDetails",
                    "MeasurementId=" + MeasurementId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/ViewMeasurementDetails",
                    "MeasurementId=" + MeasurementId, ipAddr);
            }

            ViewBag.MeasurementId = -1;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
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
                KIS.App_Sources.FreeTimeMeasurement fm = new App_Sources.FreeTimeMeasurement(Session["ActiveWorkspace_Name"].ToString(), MeasurementId);
                if(fm.id!= -1)
                {
                    ViewBag.MeasurementId = fm.id;
                    fm.loadTasks();
                    foreach(var t in fm.Tasks)
                    {
                        t.loadEvents();
                    }
                    return View(fm);
                }
            }
            return View();
        }

        public JsonResult GetTaskEvents(int MeasurementId, int TaskId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetTaskEvents",
                    "MeasurementId=" + MeasurementId + "&TaskId=" + TaskId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetTaskEvents",
                    "MeasurementId=" + MeasurementId + "&TaskId=" + TaskId, ipAddr);
            }

            JsonResult res = Json("");
            // Check read permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                FreeMeasurement_Task tsk = new FreeMeasurement_Task(Session["ActiveWorkspace_Name"].ToString(), MeasurementId, TaskId);
                if(tsk.TaskId!=-1 && tsk.MeasurementId!=-1)
                {
                    tsk.loadEvents();
                    res = Json(tsk.TaskEvents);
                }
            }
            else
            {
                res = Json("2");
            }
            return res;
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything ok
         * 2 if there were some errors while adding timespans to the database
         * 9 if user not authorized
         */
        public int TransformEventsToTimespans()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetTaskEvents", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/FreeTimeMeasurement/FreeMeasurement/GetTaskEvents", "", ipAddr);
            }

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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                KIS.App_Sources.FreeTimeMeasurements fms = new FreeTimeMeasurements(Session["ActiveWorkspace_Name"].ToString());
                ret = fms.TransformEventsToTimespans();
            }
            else
            {
                ret = 9;
            }
            return ret;
        }

        public String FreeMeasurementDownload()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Action", "/FreeTimeMeasurement/FreeMeasurement/FreeMeasurementDownload", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/FreeTimeMeasurement/FreeMeasurement/FreeMeasurementDownload", "", ipAddr);
            }

            String ret = "";
            // Check read permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement Manage";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                String freemeasurements = 
                    "\"AllowCustomTasks\", "
                    + "\"AllowExecuteFinishedTasks\", "
                    + "\"CreatedBy\", "
                    + "\"CreationDate\", "
                    + "\"DepartmentId\", "
                    + "\"DepartmentName\", "
                    + "\"DepartmentTimeZone\", "
                    + "\"Description\", "
                    + "\"id\", "
                    + "\"MeasurementUnitId\", "
                    + "\"MeasurementUnitType\", "
                    + "\"Name\", "
                    + "\"PlannedEndDate\", "
                    + "\"PlannedStartDate\", "
                    + "\"ProcessDescription\", "
                    + "\"ProcessId\", "
                    + "\"ProcessName\", "
                    + "\"ProcessRev\", "
                    + "\"Quantity\", "
                    + "\"RealEndDate\", "
                    + "\"RealLeadTime_Hours\", "
                    + "\"RealWorkingTime_Hours\", "
                    + "\"SerialNumber\", "
                    + "\"Status\", "
                    + "\"VariantId\", "
                    + "\"VariantName\" "
                    + "\n";
                FreeTimeMeasurements fms = new FreeTimeMeasurements(Session["ActiveWorkspace_Name"].ToString());
                fms.loadMeasurements('A');
                foreach(var m in fms.MeasurementsList)
                {
                    freemeasurements += 
                        "\"" + m.AllowCustomTasks + "\", "
                        + "\"" + m.AllowExecuteFinishedTasks + "\", "
                        + "\"" + m.CreatedBy + "\", "
                        + "\"" + m.CreationDate + "\", "
                        + "\"" + m.DepartmentId + "\", "
                        + "\"" + m.DepartmentName + "\", "
                        + "\"" + m.DepartmentTimeZone + "\", "
                        + "\"" + m.Description + "\", "
                        + "\"" + m.id + "\", "
                        + "\"" + m.MeasurementUnitId + "\", "
                        + "\"" + m.MeasurementUnitType + "\", "
                        + "\"" + m.Name + "\", "
                        + "\"" + m.PlannedEndDate + "\", "
                        + "\"" + m.PlannedStartDate + "\", "
                        + "\"" + m.ProcessDescription + "\", "
                        + "\"" + m.ProcessId + "\", "
                        + "\"" + m.ProcessName + "\", "
                        + "\"" + m.ProcessRev + "\", "
                        + "\"" + m.Quantity + "\", "
                        + "\"" + m.RealEndDate + "\", "
                        + "\"" + Math.Round(m.RealLeadTime_Hours,4).ToString().Replace(",",".") + "\", "
                        + "\"" + Math.Round(m.RealWorkingTime_Hours, 4).ToString().Replace(",", ".") + "\", "
                        + "\"" + m.SerialNumber + "\", "
                        + "\"" + m.Status + "\", "
                        + "\"" + m.VariantId + "\", "
                        + "\"" + m.VariantName + "\" "
                        + "\n";
                }

                String savePath = Server.MapPath(@"~\Data\FreeMeasurements\");
                ret = DateTime.UtcNow.Ticks + ".csv";
                System.IO.File.WriteAllText(savePath + ret, freemeasurements.ToString());

            }
            return ret;
        }

        public String GetTaskEventNote(int EventId)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/FreeTimeMeasurement/FreeMeasurement/FreeMeasurementDownload", "EventId="+ EventId, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/FreeTimeMeasurement/FreeMeasurement/FreeMeasurementDownload", "EventId=" + EventId, ipAddr);
            }
            String ret = "";
            // Check read permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "FreeMeasurement ExecuteTasks";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                FreeMeasurements_Tasks_Event ev = new FreeMeasurements_Tasks_Event(Session["ActiveWorkspace_Name"].ToString(), EventId);
                if(ev.id!=-1)
                {
                    ret = ev.notes;
                }
            }
            return ret;
        }

        public int SaveTaskEventNote(int EventId, String note)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/FreeTimeMeasurement/FreeMeasurement/SaveTaskEventNote", "EventId=" + EventId + "&note="+ note, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/FreeTimeMeasurement/FreeMeasurement/SaveTaskEventNote", "EventId=" + EventId + "&note=" + note, ipAddr);
            }
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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                FreeMeasurements_Tasks_Event fmev = new FreeMeasurements_Tasks_Event(Session["ActiveWorkspace_Name"].ToString(), EventId);
                if(fmev.id != -1)
                {
                    ret = fmev.SaveNote(note);
                }
            }
            return ret;
        }
    }

    public class FileUploadMeasurementController : Controller
    {
        public String log;

        FilesHelper filesHelper;
        String tempPath = "~/Data/Measurements/tmp";
        String serverMapPath = "~/Data/Measurements";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        private string UrlBase = "/Data/Measurements/";
        String DeleteURL = "/FileUpload/DeleteFile/?file=";
        String DeleteType = "GET";
        public FileUploadMeasurementController()
        {
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
        }

        /* Returns:
         * 2 if user not logged in
         * 3 if file extension is not .csv
         */
        [HttpPost]
        public JsonResult Upload(String ddlBatchUploadProductsList)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/FreeTimeMeasurement/FileUploadMeasurement/Upload", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/FreeTimeMeasurement/FileUploadMeasurement/Upload", "", ipAddr);
            }

            if (Session["user"] != null)
            {
                User usr = (User)Session["user"];
                var resultList = new List<ViewDataUploadFilesResult>();
                var CurrentContext = HttpContext;

                filesHelper.UploadAndShowResults(CurrentContext, resultList);
                for (int i = 0; i < resultList.Count; i++)
                {
                    if(resultList[i].name.Substring(resultList[i].name.Length - 4, 4) == ".csv")
                    { 
                        String filename = usr.username + "_" + DateTime.UtcNow.Ticks + ".csv";
                        System.IO.File.Move(HostingEnvironment.MapPath(serverMapPath) + "/" + resultList[i].name,
                        HostingEnvironment.MapPath(serverMapPath) + "/" + filename);

                        this.ProcessFile(filename);
                    }
                    else
                    {
                        // return Json("3");
                    }
                }
                JsonFiles files = new JsonFiles(resultList);
                return Json(files);
            }
            else
            {
                return Json("2");
            }
        }

        public JsonResult GetFileList()
        {
            var list = filesHelper.GetFileList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            filesHelper.DeleteFile(file);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if there is some error in the data
         */
        public int LoadMeasures(String fileName)
        {
            this.MeasurementBatchList = new List<FreeMeasurentsTasksJsonStruct>();
            int ret = 0;
            Boolean check = true;
            using (var reader = new StreamReader(HostingEnvironment.MapPath(serverMapPath) + "/" + fileName))
            {
                var line = reader.ReadLine();
                int lineNo = 1;
                while (!reader.EndOfStream && check)
                {
                    line = reader.ReadLine();
                    var values = line.Split(',');

                    // Data rilievo,Prodotto,Reparto/Area,Ciclo, // 3
                    // Operatore,Macrofase,Sequenza,    // 6
                    // Tempo,Passo,Aciclico,            // 9
                    // Quantità usata,Quantità a generatore,V/NV,   // 12
                    // Ergonomia,Note,Tempo aciclico per ciclo,     // 15
                    // Tempo con passo                              // 16
                    FreeMeasurentsTasksJsonStruct curr = new FreeMeasurentsTasksJsonStruct();
                    try
                    {
                        String measurementdate = values[0];
                        curr.PlannedStartDate = DateTime.Parse(measurementdate);
                        curr.PlannedEndDate = DateTime.Parse(measurementdate);
                        curr.ProductName = values[1];
                        curr.DepartmentName = values[2];
                        curr.MeasurementName = values[3];
                        curr.Operator = values[4];
                        curr.TaskName = values[5];
                        curr.Sequence = Int32.Parse(values[6]);
                        curr.RealWorkingTime_Hour = Double.Parse(values[7]) / 3600;
                        curr.step = Int32.Parse(values[8]);
                        curr.isAcyclic = values[9].Length > 0 ? true : false;
                        curr.Acyclic_QuantityUsed = Double.Parse(values[10]);
                        curr.Acyclic_QuantityForEachProduct = values[11].ToString().Length == 0 ? 0 : Double.Parse(values[11]);
                        curr.ValueOrWaste = Char.Parse(values[12]);
                        curr.Ergonomy = Char.Parse(values[13]);
                        curr.Notes = (values[14] == null || values[14] == "" || values[14].ToString().Length == 0) ? "" : values[14];
                        curr.Acyclic_CycleTime = Double.Parse(values[15]) / 3600;
                        curr.AdjustedTime = Double.Parse(values[16]) / 3600;

                        this.MeasurementBatchList.Add(curr);
                    }
                    catch(Exception ex)
                    {
                        check = false;
                        this.log = "Error on line " + lineNo.ToString() + " " + ex.Message;
                        ret = 3;
                    }
                    lineNo++;
                }

                ret = 1;
            }

            if(!check)
            {
                ret = 2;
            }
            return ret;
        }

        public int ValidateMeasures()
        {
            int ret = 0;
            
            return ret;
        }

        public List<FreeMeasurentsTasksJsonStruct> MeasurementBatchList;

        public String ProcessFile(String filename)
        {
            UserAccount usr = (UserAccount)Session["user"];
            String ret = "0";
            int loadMeasures = this.LoadMeasures(filename);

            if (loadMeasures == 1)
            {
                FreeTimeMeasurements fms = new FreeTimeMeasurements(Session["ActiveWorkspace_Name"].ToString());
                var measurements = this.MeasurementBatchList.GroupBy(t => t.MeasurementName).Select(g => g.First()).ToList();
                foreach (var m in measurements)
                {
                    int measure = fms.AddBatch(usr.id.ToString(), m.PlannedStartDate, m.PlannedEndDate, m.MeasurementName, m.MeasurementDescription,
                        m.SerialNumber, m.Quantity, 0, 0);
                    KIS.App_Sources.FreeTimeMeasurement tm = new KIS.App_Sources.FreeTimeMeasurement(Session["ActiveWorkspace_Name"].ToString(), measure);
                    if(tm.id!=-1)
                    {
                        var tasklist = this.MeasurementBatchList.Where(p => p.MeasurementName == tm.Name);
                        KIS.App_Sources.FreeTimeMeasurement fm = new KIS.App_Sources.FreeTimeMeasurement(Session["ActiveWorkspace_Name"].ToString(), tm.id);
                        foreach(var t in tasklist)
                        {
                            fm.addTask(t.TaskName, t.RealWorkingTime_Hour, t.step, t.isAcyclic,
                                t.Acyclic_CycleTime, t.Acyclic_QuantityUsed, t.Acyclic_QuantityForEachProduct, t.ValueOrWaste, t.Ergonomy);
                        }
                    }
                }
                ret = "1";
            }
            else
            {
                ret = this.log;
            }
            return ret;
        }
    }
}