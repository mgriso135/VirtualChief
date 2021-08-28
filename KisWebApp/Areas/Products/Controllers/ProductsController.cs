using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Products.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products/Products
        [Authorize]
        public ActionResult EditTaskPanel(int TaskID, int TaskRev, int VariantID)
        {
            ViewBag.Tenant = "";

            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/Products/Products/EditTaskPanel", "TaskID="+TaskID+"&TaskRev="+TaskRev+"&VariantID="+VariantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/Products/EditTaskPanel", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }

            ViewBag.TaskID = TaskID;
            ViewBag.TaskRev = TaskRev;
            ViewBag.VariantID = VariantID;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
            ViewBag.showAddWI = false;
            if (ViewBag.authW)
            {
                ViewBag.Tenant = Session["ActiveWorkspace_Name"].ToString();
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new App_Code.processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new variante(Session["ActiveWorkspace_Name"].ToString(), VariantID));
                tskVar.loadWorkInstructions();
                ViewBag.showAddWI = tskVar.WorkInstructions.Count == 0 ? true : false;
                return View(tskVar);
            }
            return View();
        }
        [Authorize]
        public Boolean SaveTaskDetails(int TaskID, int TaskRev, int VariantID, String TaskName, String TaskDescription)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/SaveTaskDetails", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/SaveTaskDetails", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
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
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new App_Code.processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new App_Code.variante(Session["ActiveWorkspace_Name"].ToString(), VariantID));
                Boolean found = false;
                if (TaskName!=tskVar.Task.processName)
                { 
                    ElencoTasks lstTasks = new ElencoTasks(Session["ActiveWorkspace_Name"].ToString());
                    try
                    {
                        var existingtask = lstTasks.Elenco.First(x => x.processName.ToLower() == Server.HtmlEncode(TaskName).ToLower());
                        found = true;
                        ret = false;
                    }
                    catch {
                        found = false;
                    }
                }

                if (!found)
                {
                    tskVar.Task.processName = Server.HtmlEncode(TaskName);
                    tskVar.Task.processDescription = Server.HtmlEncode(TaskDescription);
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
                return ret;
        }
        [Authorize]
        public Boolean AddWorkingTimeToTask(int TaskID, int TaskRev, int VariantID, int NumOps, int SetupTime, int CycleTime, Boolean def)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/AddWorkingTimeToTask", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/AddWorkingTimeToTask", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo TempiCiclo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            int sH = SetupTime / 3600;
            int sM = (SetupTime - sH * 60) / 60;
            int sS = SetupTime - sH * 3600 - sM * 60;

            int cH = CycleTime / 3600;
            int cM = (CycleTime - cH * 60) / 60;
            int cS = CycleTime - cH * 3600 - cM * 60;

            TimeSpan sTime = new TimeSpan(sH, sM, sS);
            TimeSpan cTime = new TimeSpan(cH, cM, cS);

            String retS = sTime.TotalHours.ToString() + " " + cTime.TotalHours.ToString();

            if (ViewBag.authW && NumOps >=1 && sTime.TotalHours >= 0 && cTime.TotalHours >=0)
            {
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new App_Code.processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), 
                    new App_Code.variante(Session["ActiveWorkspace_Name"].ToString(), VariantID));
                if(tskVar!=null && tskVar.Task!=null && tskVar.variant!=null && 
                    tskVar.Task.processID!=-1 && tskVar.variant.idVariante!=-1)
                {
                    tskVar.loadTempiCiclo(); ;
                    ret = tskVar.Tempi.Add(NumOps, cTime, sTime, def);
                }
            }
            return ret;
        }
        [Authorize]
        public ActionResult TaskCycleTimesList(int TaskID, int TaskRev, int VariantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/Products/Products/TaskCycleTimesList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/Products/TaskCycleTimesList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }

            ViewBag.TaskID = TaskID;
            ViewBag.TaskRev = TaskRev;
            ViewBag.VariantID = VariantID;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo TempiCiclo";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Processo TempiCiclo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new variante(Session["ActiveWorkspace_Name"].ToString(), VariantID));
                if(tskVar!=null && tskVar.Task!=null && tskVar.Task.processID!=-1 &&
                    tskVar.variant!=null && tskVar.variant.idVariante!=-1)
                { 
                    tskVar.loadTempiCiclo();
                    return View(tskVar.Tempi.Tempi);
                }

            }
            return View();
        }
        [Authorize]
        public Boolean TaskCycleTimeDelete(int TaskID, int TaskRev, int VariantID, int nOps)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/TaskCycleTimeDelete", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/TaskCycleTimeDelete", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo TempiCiclo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if(ViewBag.authW)
            {
                TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev, VariantID, nOps);
                if(tc!=null && tc.NumeroOperatori>=0 && tc.IdProcesso!=-1 && tc.Variante!=-1)
                {
                    ret = tc.Delete();
                }
            }
            return ret;
        }
        [Authorize]
        public Boolean AddTaskParam(int TaskID, int TaskRev, int variantID, String ParamName, String ParamDescription,
            int ParamCategory, Boolean ParamIsFixed, Boolean ParamIsRequired)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + variantID, ipAddr);
            }

            Boolean ret = false;

            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Product Parameters";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);

            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            ViewBag.authX = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskVariante taskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev),
                    new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if (taskVar != null && taskVar.Task != null && taskVar.Task.processID != -1 &&
                    taskVar.variant != null && taskVar.variant.idVariante != -1)
                {
                    
                    ret = taskVar.addParameter(Server.HtmlEncode(ParamName), Server.HtmlEncode(ParamDescription),
                        new ProductParametersCategory(Session["ActiveWorkspace_Name"].ToString(), ParamCategory), ParamIsFixed, ParamIsRequired);
                }
            }
            return ret;
        }
        [Authorize]
        public Boolean DeleteTaskParam(int TaskID, int TaskRev, int variantID, int ParamID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + variantID, ipAddr);
            }

            Boolean ret = false;
            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
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
                TaskVariante prcVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), 
                    new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if (prcVar != null && prcVar.Task != null && prcVar.Task.processID != -1 &&
                    prcVar.variant != null && prcVar.variant.idVariante != -1)
                {
                    ret = prcVar.deleteParameter(ParamID);
                }
            }
            return ret;
        }
        [Authorize]
        public Boolean EditTaskParam(int TaskID, int TaskRev, int variantID, int ParamID,
            String paramName, String paramDescription, int paramCategory, Boolean isFixed, Boolean isRequired)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&variantID=" + variantID, ipAddr);
            }

            Boolean ret = false;
            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
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
                ModelTaskParameter prodParam = new ModelTaskParameter(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev, variantID, ParamID);
                if (prodParam.ParameterID != -1)
                {
                    prodParam.Name = paramName;
                    prodParam.Description = paramDescription;
                    prodParam.ParameterCategory = new ProductParametersCategory(Session["ActiveWorkspace_Name"].ToString(), paramCategory);
                    prodParam.isFixed = isFixed;
                    prodParam.isRequired = isRequired;
                    ret = true;
                }
            }
            return ret;
        }
        [Authorize]
        public ActionResult TaskParametersList(int TaskID, int TaskRev, int VariantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/Products/Products/TaskParametersList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/Products/TaskParametersList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Product Parameters";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            ViewBag.authX = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
            ViewBag.processID = "A";
            ViewBag.processRev = "B";
            ViewBag.varianteID = "C";

            if (ViewBag.authR || ViewBag.authW || ViewBag.authX)
            {
                ViewBag.Tenant = Session["ActiveWorkspace_Name"].ToString();
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new variante(Session["ActiveWorkspace_Name"].ToString(), VariantID));
                if (tskVar != null && tskVar.Task != null && tskVar.Task.processID > -1 &&
                    tskVar.variant != null && tskVar.variant.idVariante > -1)
                {
                    ViewBag.processID = tskVar.Task.processID;
                    ViewBag.processRev = tskVar.Task.revisione;
                    ViewBag.varianteID = tskVar.variant.idVariante;
                    tskVar.loadParameters();
                    return View(tskVar.Parameters);
                }
            }
            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if some task doesn't have default cycle time assigned
         * 3 if there are no tasks
         */
        [Authorize]
        public int CheckProductIntegrityCycleTimes(int processID, int processRev, int variantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/CheckProductIntegrityCycleTimes", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/CheckProductIntegrityCycleTimes", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }

            int retCT = 0;
            ProcessoVariante prcVar=new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), processID, processRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
            if(prcVar!=null && prcVar.process!= null && prcVar.variant!=null && prcVar.process.processID!=-1
                && prcVar.variant.idVariante!=-1)
            {
                retCT = 1;
                prcVar.process.loadFigli(prcVar.variant);
                if(prcVar.process.subProcessi.Count == 0)
                {
                    retCT = 3;
                }
                else
                { 
                for(int i = 0; i < prcVar.process.subProcessi.Count && retCT == 1; i++)
                {
                    TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), prcVar.process.subProcessi[i], prcVar.variant);
                    tskVar.loadTempiCiclo();
                    bool found = false;
                    foreach (var cTime in tskVar.Tempi.Tempi)
                    {
                        if(cTime.Default)
                        {
                            found = true;
                        }
                    }

                    if(!found)
                    {
                        retCT = 2;
                    }
                }
                 if(retCT == 1)
                    {
                        retCT = prcVar.process.checkConsistencyPERT(prcVar.variant);
                    }
                }
            }
            return retCT;
        }

        /* Returns:
         * 0 if generic error
         * 1 if manual connected correctly
         * 2 if user not authorized
         * 4 if WorkInstruction not found
         */
        [Authorize]
        public int LinkTaskToWorkInstruction(int TaskID, int TaskRev, int variantID, int WorkInstructionID, int WorkInstructionVersion,
             DateTime InitialValidity, DateTime EndValidity)
        {
            int ret = 0;
            // Task WorkInstructions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task WorkInstructions";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(Session["ActiveWorkspace_Name"].ToString(), WorkInstructionID, WorkInstructionVersion);
                if(currWI!=null && currWI.ID!=-1 && currWI.Version!=-1)
                {
                    TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                    if(tskVar!=null && tskVar.Task!=null && tskVar.Task.processID!=-1
                        && tskVar.variant!=null && tskVar.variant.idVariante!=-1)
                    {
                        ret = currWI.linkManualToTask(TaskID, TaskRev, variantID, InitialValidity, EndValidity, 0, true);
                    }
                }
                else
                {
                    ret = 4;
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
         * 1 if deleted correctly
         * 2 if user not authorized
         * 3 if TaskWorkstationNotFound
         */
        [Authorize]
        public int DeleteTaskWorkInstruction(int TaskID, int TaskRev, int variantID, int WorkInstructionID, int WorkInstructionVersion)
        {
            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Task WorkInstructions";
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
                TaskWorkInstruction curr = new TaskWorkInstruction(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev, variantID, WorkInstructionID, WorkInstructionVersion);
                if (curr != null)
                {
                    ret = curr.Delete();
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
        [Authorize]
        public ActionResult TaskDefaultOperatorsList(int TaskID, int TaskRev, int variantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/Products/Products/TaskDefaultOperatorsList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/Products/TaskDefaultOperatorsList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&variantID=" + variantID, ipAddr);
            }

            ViewBag.TaskID = -1;
            ViewBag.TaskRev = -1;
            ViewBag.VariantID = -1;


            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Task DefaultOperators";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Task DefaultOperators";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if (tskVar != null && tskVar.Task != null && tskVar.Task.processID != -1 &&
                    tskVar.variant != null && tskVar.variant.idVariante != -1)
                {
                    ViewBag.TaskID = tskVar.Task.processID;
                    ViewBag.TaskRev = tskVar.Task.revisione;
                    ViewBag.VariantID = tskVar.variant.idVariante;

                    tskVar.loadDefaultOperators();
                    return View(tskVar.DefaultOperators);
                }
            }
                return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if user not autorized
         * 3 if error while adding the default user
         */
        [Authorize]
        public int AddDefaultOperator(int TaskID, int TaskRev, int variantID, String user)
        {
            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Task DefaultOperators";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if(authW)
            {
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if (tskVar != null && tskVar.Task != null && tskVar.Task.processID != -1 &&
                    tskVar.variant != null && tskVar.variant.idVariante != -1)
                {
                    Boolean check = tskVar.addDefaultOperator(user);
                    ret = check ? 1 : 3;
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
  * 1 if everything is ok
  * 2 if user not autorized
  * 3 if error while adding the default user
  */
        [Authorize]
        public int DeleteDefaultOperator(int TaskID, int TaskRev, int variantID, String user)
        {
            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Task DefaultOperators";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (authW)
            {
                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), TaskID, TaskRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if (tskVar != null && tskVar.Task != null && tskVar.Task.processID != -1 &&
                    tskVar.variant != null && tskVar.variant.idVariante != -1)
                {
                    Boolean check = tskVar.deleteDefaultOperator(user);
                    ret = check ? 1 : 3;
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
         * 1 if product completed successfully
         * 2 if user not authorized
         * 3 if product not found
         */
        [Authorize]
        public int CompleteProductBruteForce(int ProductID, int ProductYear, int InputPointId)
        {
            int ret = 0;
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Action", "/Products/Products/CompleteProductBruteForce", "ProductID=" + ProductID + "&ProductYear="+ProductYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/CompleteProductBruteForce", "ProductID=" + ProductID + "&ProductYear=" + ProductYear, ipAddr);
            }

            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Produzione";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
            if (ViewBag.authX)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProductID, ProductYear);
                if(art!=null && art.ID!=-1 && art.Year > 2010)
                {
                    InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), InputPointId);
                    ret = art.CompleteProductBruteForce(ip);
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
        [Authorize]
        public JsonResult loadTempiCiclo(int procID, int rev, int varID)
        {
            List<String[]> ret = new List<String[]>();
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo padre = new processo(Session["ActiveWorkspace_Name"].ToString(), procID);
                variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), varID);
                padre.loadFigli(new variante(Session["ActiveWorkspace_Name"].ToString(), varID));

                for (int i = 0; i < padre.subProcessi.Count; i++)
                {
                    String[] element = new String[5];
                    TaskVariante tsk = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), padre.subProcessi[i], var);
                    TimeSpan tc = tsk.getDefaultTempoCiclo();
                    int n_ops = tsk.getDefaultOperatori();

                    element[0] = padre.subProcessi[i].processID.ToString();
                    element[1] = padre.subProcessi[i].processName;
                    element[2] = padre.subProcessi[i].posX.ToString();
                    element[3] = padre.subProcessi[i].posY.ToString();
                    element[4] = Math.Truncate(tc.TotalHours).ToString() + ":" + tc.Minutes.ToString() + ":"
                        + tc.Seconds.ToString() + " (" + n_ops + ")";
                    ret.Add(element);
                }
            }
            return Json(ret);

        }

        [Authorize]
        public JsonResult loadPrecedenze(int procID, int rev, int varID)
        {
            List<int[]> ret = new List<int[]>();
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo padre = new processo(Session["ActiveWorkspace_Name"].ToString(), procID, rev);
                variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), varID);
                padre.loadFigli(var);
                for (int i = 0; i < padre.subProcessi.Count; i++)
                {
                    padre.subProcessi[i].loadSuccessivi(new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                    // Costruisco l'array dei successivi
                    for (int j = 0; j < padre.subProcessi[i].processiSucc.Count; j++)
                    {
                        int[] elem = new int[5];
                        elem[0] = padre.subProcessi[i].processID;
                        elem[1] = padre.subProcessi[i].processiSucc[j];
                        elem[2] = Convert.ToInt32(padre.subProcessi[i].pauseSucc[j].TotalSeconds);
                        elem[3] = padre.subProcessi[i].revisione;
                        elem[3] = padre.subProcessi[i].revisioneSucc[j];
                        ret.Add(elem);
                    }
                }
            }
            return Json(ret);
        }

        [Authorize]
        public int addDefaultSubProcess(int procID, int rev, int varID)
        {
            int ret = -1;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo padre = new processo(Session["ActiveWorkspace_Name"].ToString(), procID);
                if (padre.processID != -1 && varID != -1)
                {
                    int procCreated = padre.createDefaultSubProcess(new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                    if (procCreated >= 0)
                    {
                        ret = procCreated;
                    }
                    else
                    {
                        ret = procCreated;
                    }
                }
            }
            return ret;
        }

        [Authorize]
        public bool linkExistingSubProcess(int procID, int rev, int varID, int taskID, int taskRev)
        {
            bool rt = false;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo pr = new processo(Session["ActiveWorkspace_Name"].ToString(), procID, rev);

                if (taskID != -1)
                {
                    rt = pr.linkProcessoVariante(new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), taskID), new variante(Session["ActiveWorkspace_Name"].ToString(), varID)));
                }
                else
                {
                    rt = false;
                }
            }
            return rt;
        }

        [Authorize]
        public bool deleteSubProcess(int procID, int rev, int varID, int taskID, int taskRev)
        {
            bool controllo = true;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo prc = new processo(Session["ActiveWorkspace_Name"].ToString(), taskID);
                variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), varID);
                TaskVariante tsk = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), prc, var);

                // Controllo che non ci siano figli associati
                if (controllo == true)
                {
                    tsk.Task.loadFigli();
                    if (tsk.Task.subProcessi.Count == 0)
                    {
                        controllo = true;
                    }
                    else
                    {
                        controllo = false;
                    }
                }

                // Controllo che non ci siano varianti associate
                if (controllo == true)
                {
                    tsk.Task.loadVariantiFigli();
                    if (tsk.Task.variantiFigli.Count == 0)
                    {
                        controllo = true;
                    }
                    else
                    {
                        controllo = false;
                    }
                }

                // Controllo che non ci siano tempi ciclo associati
                if (controllo == true)
                {
                    tsk.loadTempiCiclo();
                    if (tsk.Tempi.Tempi.Count > 0)
                    {
                        controllo = false;
                    }
                    else
                    {
                        controllo = true;
                    }
                }

                if (controllo == true)
                {
                    tsk.loadPostazioni();
                    for (int i = 0; i < tsk.PostazioniDiLavoro.Count; i++)
                    {
                        tsk.deleteLinkPostazione(tsk.PostazioniDiLavoro[i]);
                    }

                }

                // Se è tutto ok...
                if (controllo == true)
                {
                    bool rt = tsk.Delete();
                    if (rt == true)
                    {
                        int res = prc.delete();
                        controllo = true;
                    }
                    else
                    {
                        controllo = false;
                    }

                }
            }
            return controllo;
        }
    }
}