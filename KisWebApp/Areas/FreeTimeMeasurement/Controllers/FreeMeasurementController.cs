using System;
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

        public JsonResult GetFreeMeasurentsTasksJson(int departmentId)
        {
            List<FreeMeasurentsTasksJsonStruct> fmStruct = new List<FreeMeasurentsTasksJsonStruct>();
            JsonResult res = Json("");
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT "
                + " freemeasurements.id, "              // 0
                + " freemeasurements.creationdate, "    // 1
                + " freemeasurements.createdby, "
                + " freemeasurements.plannedstartdate, "
                + " freemeasurements.plannedenddate, "
                + " freemeasurements.departmentid, "    // 5
                + " freemeasurements.name AS MeasurementName,"
                + " freemeasurements.description AS MeasurementDescription, "
                + " freemeasurements.ProcessId, "
                + " freemeasurements.processrev, "
                + " freemeasurements.variantid, "       // 10
                + " freemeasurements.status, "
                + " freemeasurements.serialnumber, "
                + " freemeasurements.quantity, "
                + " freemeasurements.measurementUnit, "
                + " measurementunits.type, "            // 15
                + " freemeasurements_tasks.taskid, "
                + " freemeasurements_tasks.origtaskid, "
                + " freemeasurements_tasks.origtaskrev, "
                + " freemeasurements_tasks.variantid, "
                + " freemeasurements_tasks.noproductivetaskid, "    // 20
                + " freemeasurements_tasks.name AS TaskName, "
                + " freemeasurements_tasks.description AS TaskDescription, "
                + " freemeasurements_tasks.sequence, "
                + " freemeasurements_tasks.workstationid, "
                + " postazioni.name, "                      // 25
                + " freemeasurements_tasks.quantity_planned, "
                + " freemeasurements_tasks.status AS TaskStatus " // 27
                + " FROM freemeasurements INNER JOIN freemeasurements_tasks "
                + " ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
                + " INNER JOIN postazioni ON(postazioni.idpostazioni = freemeasurements_tasks.workstationid) "
                + " INNER JOIN measurementunits ON (measurementunits.id = freemeasurements.measurementUnit) "
                + " WHERE departmentid = @departmentid "
                + " AND(ExecuteFinishedTasks = true OR(ExecuteFinishedTasks = false AND freemeasurements_tasks.status <> 'F')) "
                + " ORDER BY freemeasurements.id, freemeasurements_tasks.sequence";
            cmd.Parameters.AddWithValue("@departmentid", departmentId);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                FreeMeasurentsTasksJsonStruct curr = new FreeMeasurentsTasksJsonStruct();
                curr.MeasurementId = rdr.GetInt32(0);
                curr.Creationdate = rdr.GetDateTime(1);
                curr.CreatedBy = rdr.GetString(2);
                curr.PlannedStartDate = rdr.GetDateTime(3);
                curr.PlannedEndDate = rdr.GetDateTime(4);
                curr.DepartmentId = rdr.GetInt32(5);
                curr.MeasurementName = rdr.GetString(6);
                curr.MeasurementDescription = rdr.GetString(7);
                curr.ProcessId = rdr.GetInt32(8);
                curr.ProcessRev = rdr.GetInt32(9);
                curr.VariantId = rdr.GetInt32(10);
                curr.Status = rdr.GetChar(11);
                curr.SerialNumber = rdr.GetString(12);
                curr.Quantity = rdr.GetDouble(13);
                curr.MeasurementUnitId = rdr.GetInt32(14);
                curr.MeasurementUnitType = rdr.GetString(15);
                curr.TaskId = rdr.GetInt32(16);
                curr.OrigTaskId = rdr.GetInt32(17);
                curr.OrigTaskRev = rdr.GetInt32(18);
                curr.VariantId = rdr.GetInt32(19);
                curr.NoProductiveTaskId = rdr.IsDBNull(20) ? -1 : rdr.GetInt32(20);
                curr.TaskName = rdr.GetString(21);
                curr.TaskDescription = rdr.GetString(22);
                curr.Sequence = rdr.GetInt32(23);
                curr.WorkstationId = rdr.GetInt32(24);
                curr.WorkstationName = rdr.GetString(25);
                curr.TaskQuantity = rdr.GetDouble(26);
                curr.TaskStatus = rdr.GetChar(27);
                fmStruct.Add(curr);
            }
            rdr.Close();
            conn.Close();

            res = Json(fmStruct);
            return res;
        }
    }

    public class FreeMeasurentsTasksJsonStruct
    {
        public int MeasurementId;
        public DateTime Creationdate;
        public String CreatedBy;
        public DateTime PlannedStartDate;
        public DateTime PlannedEndDate;
        public int DepartmentId;
        public String MeasurementName;
        public String MeasurementDescription;
        public int ProcessId;
        public int ProcessRev;
        public int VariantId;
        public char Status;
        public String SerialNumber;
        public Double Quantity;
        public int MeasurementUnitId;
        public String MeasurementUnitType;
        public int TaskId;
        public int OrigTaskId;
        public int OrigTaskRev;
        public int NoProductiveTaskId;
        public String TaskName;
        public String TaskDescription;
        public int Sequence;
        public int WorkstationId;
        public String WorkstationName;
        public Double TaskQuantity;
        public Char TaskStatus;
    }

    
}