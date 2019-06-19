﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Areas.Products.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products/Products
        public ActionResult EditTaskPanel(int TaskID, int TaskRev, int VariantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/Products/EditTaskPanel", "TaskID="+TaskID+"&TaskRev="+TaskRev+"&VariantID="+VariantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if(ViewBag.authW)
            { 
                TaskVariante tskVar = new TaskVariante(new App_Code.processo(TaskID, TaskRev), new variante(VariantID));
                return View(tskVar);
            }
            return View();
        }

        public Boolean SaveTaskDetails(int TaskID, int TaskRev, int VariantID, String TaskName, String TaskDescription)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/Products/SaveTaskDetails", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskVariante tskVar = new TaskVariante(new App_Code.processo(TaskID, TaskRev), new App_Code.variante(VariantID));
                Boolean found = false;
                if (TaskName!=tskVar.Task.processName)
                { 
                    ElencoTasks lstTasks = new ElencoTasks();
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

        public Boolean AddWorkingTimeToTask(int TaskID, int TaskRev, int VariantID, int NumOps, int SetupTime, int CycleTime, Boolean def)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/Products/AddWorkingTimeToTask", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
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
                TaskVariante tskVar = new TaskVariante(new App_Code.processo(TaskID, TaskRev), new App_Code.variante(VariantID));
                if(tskVar!=null && tskVar.Task!=null && tskVar.variant!=null && 
                    tskVar.Task.processID!=-1 && tskVar.variant.idVariante!=-1)
                {
                    tskVar.loadTempiCiclo(); ;
                    ret = tskVar.Tempi.Add(NumOps, cTime, sTime, def);
                }
            }
            return ret;
        }

        public ActionResult TaskCycleTimesList(int TaskID, int TaskRev, int VariantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/Products/TaskCycleTimesList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Processo TempiCiclo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authR || ViewBag.authW)
            {
                TaskVariante tskVar = new TaskVariante(new processo(TaskID, TaskRev), new variante(VariantID));
                if(tskVar!=null && tskVar.Task!=null && tskVar.Task.processID!=-1 &&
                    tskVar.variant!=null && tskVar.variant.idVariante!=-1)
                { 
                    tskVar.loadTempiCiclo();
                    return View(tskVar.Tempi.Tempi);
                }

            }
            return View();
        }

        public Boolean TaskCycleTimeDelete(int TaskID, int TaskRev, int VariantID, int nOps)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/Products/TaskCycleTimeDelete", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if(ViewBag.authW)
            {
                TempoCiclo tc = new TempoCiclo(TaskID, TaskRev, VariantID, nOps);
                if(tc!=null && tc.NumeroOperatori>=0 && tc.IdProcesso!=-1 && tc.Variante!=-1)
                {
                    ret = tc.Delete();
                }
            }
            return ret;
        }

        public Boolean AddTaskParam(int TaskID, int TaskRev, int variantID, String ParamName, String ParamDescription,
            int ParamCategory, Boolean ParamIsFixed, Boolean ParamIsRequired)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + variantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);

            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
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
            prmUser[0] = "Product Parameters";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            ViewBag.authX = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authX = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskVariante taskVar = new TaskVariante(new processo(TaskID, TaskRev),
                    new variante(variantID));
                if (taskVar != null && taskVar.Task != null && taskVar.Task.processID != -1 &&
                    taskVar.variant != null && taskVar.variant.idVariante != -1)
                {
                    
                    ret = taskVar.addParameter(Server.HtmlEncode(ParamName), Server.HtmlEncode(ParamDescription),
                        new ProductParametersCategory(ParamCategory), ParamIsFixed, ParamIsRequired);
                }
            }
            return ret;
        }

        public Boolean DeleteTaskParam(int TaskID, int TaskRev, int variantID, int ParamID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + variantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskVariante prcVar = new TaskVariante(new processo(TaskID, TaskRev), new variante(variantID));
                if (prcVar != null && prcVar.Task != null && prcVar.Task.processID != -1 &&
                    prcVar.variant != null && prcVar.variant.idVariante != -1)
                {
                    ret = prcVar.deleteParameter(ParamID);
                }
            }
            return ret;
        }

        public Boolean EditTaskParam(int TaskID, int TaskRev, int variantID, int ParamID,
            String paramName, String paramDescription, int paramCategory, Boolean isFixed, Boolean isRequired)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/Products/AddTaskParam", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&variantID=" + variantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ModelTaskParameter prodParam = new ModelTaskParameter(TaskID, TaskRev, variantID, ParamID);
                if (prodParam.ParameterID != -1)
                {
                    prodParam.Name = paramName;
                    prodParam.Description = paramDescription;
                    prodParam.ParameterCategory = new ProductParametersCategory(paramCategory);
                    prodParam.isFixed = isFixed;
                    prodParam.isRequired = isRequired;
                    ret = true;
                }
            }
            return ret;
        }

        public ActionResult TaskParametersList(int TaskID, int TaskRev, int VariantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/Products/TaskParametersList", "TaskID=" + TaskID + "&TaskRev=" + TaskRev + "&VariantID=" + VariantID, ipAddr);
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
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product Parameters";
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
            prmUser[0] = "Product Parameters";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            ViewBag.authX = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authX = curr.ValidatePermessi(elencoPermessi);
            }
            ViewBag.processID = "A";
            ViewBag.processRev = "B";
            ViewBag.varianteID = "C";

            if (ViewBag.authR || ViewBag.authW || ViewBag.authX)
            {
                TaskVariante tskVar = new TaskVariante(new processo(TaskID, TaskRev), new variante(VariantID));
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
        public int CheckProductIntegrityCycleTimes(int processID, int processRev, int variantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/Products/CheckProductIntegrityCycleTimes", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/Products/CheckProductIntegrityCycleTimes", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }

            int retCT = 0;
            ProcessoVariante prcVar=new ProcessoVariante(new processo(processID, processRev), new variante(variantID));
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
                    TaskVariante tskVar = new TaskVariante(prcVar.process.subProcessi[i], prcVar.variant);
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
         */
         public int LinkTaskToWorkInstruction(int TaskID, int TaskRev, int variantID, int WorkInstructionID, int WorkInstructionVersion)
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

            }
            else
            {
                ret = 2;
            }
                return ret;
        }
    }
}