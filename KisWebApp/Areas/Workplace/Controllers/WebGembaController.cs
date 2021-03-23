using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using Newtonsoft.Json;

namespace KIS.Areas.Workplace.Controllers
{
    public class WebGembaController : Controller
    {
        [Authorize]
        public ActionResult Index()
        { 
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione check-in";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authX)
            {

            }

            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/Index", "", ipAddr);
            }

            return View();
        }

        [Authorize]
        public ActionResult ListWorkstationsComplete()
        {
            // Register user action
            /*String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ListWorkstationsComplete", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ListWorkstationsComplete", "", ipAddr);
            }*/

            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione check-in";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authX)
            {
                ElencoPostazioni elPost = new ElencoPostazioni(Session["ActiveWorkspace_Name"].ToString());
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                usr.loadPostazioniAttive();
                List<Postazione> results = elPost.elenco.Where(f => !usr.PostazioniAttive.Any(t => t.id == f.id)).ToList();
                return View(results);
            }
            return View();
        }

        /*Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if workstation not found
         * 3 if user not authorized
         * 4 if error while checking-in
         */
        [Authorize]
        public int WorkstationCheckIn(int workstationID)
        {
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/WorkstationCheckIn", "workstationID=" + workstationID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/WorkstationCheckIn?workstationID", "workstationID="+workstationID, ipAddr);
            }

            int ret = 1;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione check-in";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authX)
            {
                User curr = (User)Session["user"];
                Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), workstationID);
                if(p.id!=-1)
                {
                    bool rt = curr.DoCheckIn(p);
                    ret = rt ? 1 : 4;
                }
                else
                {
                    ret = 2;
                }
            }
            else
            {
                ret = 3;
            }
                return ret;
        }

        [Authorize]
        public ActionResult ListWorkstationsActives()
        {
            // Register user action
            /*String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ListWorkstationsActives","", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ListWorkstationsActives","", ipAddr);
            }*/

            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione check-in";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authX)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                usr.loadPostazioniAttive();
                return View(usr.PostazioniAttive);
            }
            return View();
        }

        /*Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if workstation not found
         * 3 if user not authorized
         * 4 if error while checking-out
         */
        [Authorize]
        public int WorkstationCheckOut(int workstationID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/WorkstationCheckOut", "workstationID"+ workstationID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/WorkstationCheckOut", "workstationID" + workstationID, ipAddr);
            }

            int ret = 1;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione check-in";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authX)
            {
                User curr = (User)Session["user"];
                Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), workstationID);
                if (p.id != -1)
                {
                    bool rt = curr.DoCheckOut(p);
                    ret = rt ? 1 : 4;
                }
                else
                {
                    ret = 2;
                }
            }
            else
            {
                ret = 3;
            }
            return ret;
        }

        [Authorize]
        public ActionResult ListWorkstationsTasks(int WorkstationID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Page", "/Workplace/WebGemba/ListWorkstationsTasks", "workstationID=" + WorkstationID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Page", "/Workplace/WebGemba/ListWorkstationsTasks", "workstationID=" + WorkstationID, ipAddr);
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

            ViewBag.WorkstationID = -1;
            ViewBag.workstationName = "";
            ViewBag.user = "";

            if (ViewBag.authX)
            {
                Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), WorkstationID);
                if(p.id!=-1)
                {
                    ViewBag.WorkstationID = p.id;
                    ViewBag.workstationName = p.name;
                    UserAccount curr = (UserAccount)Session["user"];
                    ViewBag.user = curr.userId;
                }
                else
                {
                    ViewBag.WorkstationID = -1;
                    ViewBag.workstationName = "";
                }
            }

            return View();
         }

        [Authorize]
        public ActionResult ListTasksInExecution(int WorkstationID, String user)
        {
            // Register user action
            /*String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ListTasksInExecution", "WorkstationID="+WorkstationID+"&user="+user, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ListTasksInExecution", "WorkstationID=" + WorkstationID + "&user=" + user, ipAddr);
            }*/

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

            ViewBag.WorkstationID = -1;
            ViewBag.workstationName = "";
            
            if (ViewBag.authX)
            {
                User curr = (User)Session["user"];
                Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), WorkstationID);
                bool controlloLogin = false;
                p.loadUtentiLoggati();
                for (int i = 0; i < p.UtentiLoggati.Count; i++)
                {
                    if (p.UtentiLoggati[i] == curr.username)
                    {
                        controlloLogin = true;
                    }
                }

                if (p.id != -1 && controlloLogin)
                {
                    ViewBag.WorkstationID = p.id;
                    ViewBag.workstationName = p.name;
                    p.loadTaskAvviati(curr);
                    List<TaskProduzione> tasks = new List<TaskProduzione>();
                    for(int i =0; i < p.TaskAvviatiUtente.Count; i++)
                    {
                        tasks.Add(new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), p.TaskAvviatiUtente[i]));
                    }
                    tasks = tasks.OrderBy(x => x.LateFinish).ToList();
                    return View(tasks);
                }
                else
                {
                    ViewBag.WorkstationID = -1;
                    ViewBag.workstationName = "";
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult ListTasksAvailable(int WorkstationID, String user)
        {
            // Register user action
            /*String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ListTasksAvailable", "WorkstationID=" + WorkstationID + "&user=" + user, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ListTasksAvailable", "WorkstationID=" + WorkstationID + "&user=" + user, ipAddr);
            }*/

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

            ViewBag.WorkstationID = -1;
            ViewBag.workstationName = "";
            if (ViewBag.authX)
            {
                User curr = (User)Session["user"];
                Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), WorkstationID);
                bool controlloLogin = false;
                p.loadUtentiLoggati();
                for (int i = 0; i < p.UtentiLoggati.Count; i++)
                {
                    if (p.UtentiLoggati[i] == curr.username)
                    {
                        controlloLogin = true;
                    }
                }
                if (p.id != -1 && controlloLogin)
                {
                    ViewBag.WorkstationID = p.id;
                    ViewBag.workstationName = p.name;
                    p.loadTaskAvviati(curr);
                    p.loadTaskProduzioneAvviabili();
                    List<int> results = p.IdTaskProduzioneAvviabili.Where(f => !p.TaskAvviatiUtente.Any(t => t == f)).ToList();
                    List<TaskProduzione> tasks = new List<TaskProduzione>();
                    for (int i = 0; i < results.Count; i++)
                    {
                        tasks.Add(new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), results[i]));
                    }
                    tasks = tasks.OrderBy(x => x.LateStart).ToList();
                    return View(tasks);
                }
                else
                {
                    ViewBag.WorkstationID = -1;
                    ViewBag.workstationName = "";
                }
            }

            return View();
        }

        /*Returns:
         * 1 if task has started correctly
         * 2 if user has not the needed authorization
         * 3 if user exceeded the max number of tasks he can do simultaneously
         */
        [Authorize]
        public int StartTask(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/StartTask", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/StartTask", "TaskID=" + TaskID, ipAddr);
            }

            int ret = 1;
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
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    bool rt = tsk.Start((User)Session["user"]);
                    if (rt)
                    {
                        ret = 1;
                    }
                    else
                    {
                        Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), tsk.RepartoID);
                        User curr = (User)Session["user"];
                        curr.loadTaskAvviati();
                        if (rp.TasksAvviabiliContemporaneamenteDaOperatore > 0 && curr.TaskAvviati.Count >= rp.TasksAvviabiliContemporaneamenteDaOperatore)
                        {
                            ret = 3;
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    ret = 2;
                }
            }
            else
            {
                ret = 4;
            }
            return ret;
        }

        /*Returns:
         * 1 if task has started correctly
         * 2 if an error occured
         * 4 if user has not the needed authorization
         */
        [Authorize]
        public int PauseTask(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/PauseTask", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/PauseTask", "TaskID=" + TaskID, ipAddr);
            }

            int ret = 1;
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
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    bool rt = tsk.Pause((User)Session["user"]);
                    if (rt == true)
                    {
                        ret = 1;
                    }
                    else
                    {
                        ret = 2;
                    }
                }
            }
            else
            {
                ret = 4;
            }
            return ret;
        }

        /*Returns:
         * 1 if task has started correctly
         * 2 if an error occured
         * 3 if there are some mandatory parameters missing
         * 4 if user has not the needed authorization
         * 5 if there are some previous tasks that did not end
         */
        [Authorize]
        public int CompleteTask(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/CompleteTask", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/CompleteTask", "TaskID=" + TaskID, ipAddr);
            }

            int ret = 1;
            String retS = "CompleteTask";
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
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), tsk.ArticoloID, tsk.ArticoloAnno);
                    bool rt = tsk.Complete((User)Session["user"]);
                    if (rt == true)
                    {
                        ret = 1;
                    }
                    else
                    {
                        // Set generic error, if none of the following cases appears to be true.
                        ret = 2;

                        // Check if fault is caused by previous tasks
                        tsk.loadPrecedenti();
                        for(int i = 0; i < tsk.PreviousTasks.Count; i++)
                        {
                            TaskProduzione tskPrev = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), tsk.PreviousTasks[i].NearTaskID);
                            if(tskPrev.Status!='F')
                            {
                                ret = 5;
                            }
                        }

                        if (!tsk.CheckParametersComplete())
                        {
                            ret = 3;
                        }                       
                    }
                }

                retS = tsk.log;
                
            }
            else
            {
                ret = 4;
            }
            return ret;
        }

        [Authorize]
        public ActionResult ListTaskParameters(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/ListTaskParameters", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/ListTaskParameters", "TaskID=" + TaskID, ipAddr);
            }

            ViewBag.TaskID = -1;
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tskProduzione = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if(tskProduzione!=null && tskProduzione.TaskProduzioneID!=-1)
                {
                    ViewBag.TaskID = tskProduzione.TaskProduzioneID;
                    tskProduzione.loadParameters();
                    List<StructTaskParameters> lstParams = new List<StructTaskParameters>();
                    for(int i = 0; i < tskProduzione.Parameters.Count; i++)
                    {
                        StructTaskParameters curr = new StructTaskParameters();
                        curr.CreatedBy = tskProduzione.Parameters[i].CreatedBy;
                        curr.CreatedDate = tskProduzione.Parameters[i].CreatedDate;
                        curr.Description = tskProduzione.Parameters[i].Description;
                        curr.Exists = true;
                        curr.isFixed = tskProduzione.Parameters[i].isFixed;
                        curr.isRequired = tskProduzione.Parameters[i].isRequired;
                        curr.Name = tskProduzione.Parameters[i].Name;
                        curr.ParameterCategory = tskProduzione.Parameters[i].ParameterCategory;
                        curr.ParameterID = tskProduzione.Parameters[i].ParameterID;
                        curr.Sequence = tskProduzione.Parameters[i].Sequence;
                        curr.TaskID = TaskID;
                        lstParams.Add(curr);
                    }

                    TaskVariante tskCurr = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), tskProduzione.OriginalTask,
                        tskProduzione.OriginalTaskRevisione), new App_Code.variante(Session["ActiveWorkspace_Name"].ToString(), tskProduzione.VarianteID));
                    tskCurr.loadParameters();

                    for (int i = 0; i < tskCurr.Parameters.Count; i++)
                    {
                        bool found = false;
                        try
                        {
                            var a = lstParams.First(x => x.Name == tskCurr.Parameters[i].Name &&
                            x.ParameterCategory.ID == tskCurr.Parameters[i].ParameterCategory.ID);
                            found = true;
                        }
                        catch
                        {
                            found = false;
                        }
                        ViewBag.log += tskCurr.Parameters[i].Name + " " 
                            + tskCurr.Parameters[i].ParameterCategory.ID.ToString()
                            + " found: " + found + "<br />";
                        if (!found)
                        {
                            StructTaskParameters curr = new StructTaskParameters();
                            curr.CreatedBy = "";
                            curr.CreatedDate = new DateTime(1970,1,1);
                            curr.Description = tskCurr.Parameters[i].Description;
                            curr.Exists = false;
                            curr.isFixed = tskCurr.Parameters[i].isFixed;
                            curr.isRequired = tskCurr.Parameters[i].isRequired;
                            curr.Name = tskCurr.Parameters[i].Name;
                            curr.ParameterCategory = tskCurr.Parameters[i].ParameterCategory;
                            curr.ParameterID = tskCurr.Parameters[i].ParameterID;
                            curr.Sequence = tskCurr.Parameters[i].Sequence;
                            curr.TaskID = TaskID;
                            lstParams.Add(curr);
                        }
                    }

                    return View(lstParams);
                }
                else
                {

                }
            }
            return View();
        }

        public struct StructTaskParameters
        {
            public int TaskID;
            public ProductParametersCategory ParameterCategory;
            public int ParameterID;
            public String Name;
            public String Description;
            public Boolean isFixed;
            public Boolean isRequired;
            public int Sequence;
            public String CreatedBy;
            public DateTime CreatedDate;
            public Boolean Exists;
        }

        /* Returns:
         * 0 if generic error
         * 1 if parameter added correctly
         * 2 if user not authorized
         * 3 if parameter already set
         * 4 if original parameter in product model does not exists
         * 5 if Error while adding parameter
         */
        [Authorize]
        public int AddTaskParameters(int TaskID, int ParamID, int ParamCategory, String ParamName, String ParamValue)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                String logprms = ("TaskID=" + TaskID + "&ParamID=" + ParamID + "&ParamCategory=" + ParamCategory + "&ParamName=" + ParamName + "&ParamValue=" + ParamValue);
                int substr = logprms.Length < 254 ? logprms.Length : 254;

                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/AddTaskParameters", logprms.Substring(0,substr), ipAddr);
            }
            else
            {
                String logprms = ("TaskID=" + TaskID + "&ParamID=" + ParamID + "&ParamCategory=" + ParamCategory + "&ParamName=" + ParamName + "&ParamValue=" + ParamValue);
                int substr = logprms.Length < 254 ? logprms.Length : 254;
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/AddTaskParameters", logprms.Substring(0, substr), ipAddr);
            }

            int ret = 0;
            String retS = "0";
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tskProduzione = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tskProduzione != null && tskProduzione.TaskProduzioneID != -1)
                {
                    tskProduzione.loadParameters();
                    Boolean exists = false;
                    try
                    {
                        tskProduzione.Parameters.First(x => x.Name == ParamName 
                        && x.ParameterCategory.ID == ParamCategory);
                        exists = true;
                        ret = 3;
                     }
                    catch
                    {
                        exists = false;
                    }

                    if(!exists)
                    {
                        Boolean existsOriginal = false;
                        TaskVariante origTsk = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new App_Code.processo(Session["ActiveWorkspace_Name"].ToString(), tskProduzione.OriginalTask,
                            tskProduzione.OriginalTaskRevisione), new variante(Session["ActiveWorkspace_Name"].ToString(), tskProduzione.VarianteID));
                        origTsk.loadParameters();
                        //ret += "Parameters: " + origTsk.Parameters.Count + "<br />";
                        ModelTaskParameter origParam = null;
                        //ret += ParamName + "<br />";
                        try
                        {
                            origParam = origTsk.Parameters
                                    .First(x => x.Name == ParamName && x.ParameterCategory.ID == ParamCategory);
                            existsOriginal = true;
                        }
                        catch
                        {
                            existsOriginal = false;
                        }
                        if(existsOriginal && origParam!=null && origParam.ParameterID!=-1)
                        {
                            ret = 1;
                            UserAccount curr = (UserAccount)Session["user"];
                            Boolean checkAdd = tskProduzione.addParameter(ParamName,
                                Server.HtmlEncode(ParamValue),
                                new ProductParametersCategory(Session["ActiveWorkspace_Name"].ToString(), ParamCategory),
                                origParam.isFixed,
                                origParam.isRequired,
                                curr.userId);
                            ret = checkAdd ? 1 : 5;
                            retS = tskProduzione.log;
                        }
                        else
                        {
                            ret = 4;
                        }
                    }
                }
                else
                {

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
         * 1 if parameter delete correctly
         * 2 if user not authorized
         * 3 if error while deleting parameter
         */
        [Authorize]
        public int DeleteTaskParameters(int TaskID, int ParamCategory, int ParamID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/DeleteTaskParameters", ("TaskID="+TaskID+ "&ParamCategory=" + ParamCategory + "&ParamID="+ParamID).Substring(0, 254), ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/DeleteTaskParameters", ("TaskID=" + TaskID + "&ParamCategory=" + ParamCategory + "&ParamID=" + ParamID).Substring(0, 254), ipAddr);
            }

            int ret = 0;
            String retS = "";
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tskProduzione = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tskProduzione != null && tskProduzione.TaskProduzioneID != -1)
                {
                    tskProduzione.loadParameters();
                    Boolean deleted = tskProduzione.deleteParameter(ParamID, ParamCategory);
                    ret = deleted ? 1 : 3;
                }
                else
                {

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
         * 1 if all is ok
         * 2 if user not authorized
         */
        [Authorize]
        public int GenerateWarning(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/GenerateWarning", "TaskID="+ TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/GenerateWarning", "TaskID=" + TaskID, ipAddr);
            }

            int ret = 0;
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    bool rt = tsk.generateWarning((User)Session["user"]);
                    if (rt)
                    {
                        ret = 1;
                    }
                    else
                    {
                        ret =0;
                    }
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
         * 1 if all is ok
         * 2 if user not authorized
         * 3 if incorrect quantity
         */
        [Authorize]
        public int ChangeQuantity(int TaskID, int Quantity)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/ChangeQuantity", "TaskID="+TaskID+"&Quantity="+Quantity, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/ChangeQuantity", "TaskID=" + TaskID + "&Quantity=" + Quantity, ipAddr);
            }

            int ret = 0;
            ViewBag.authW = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    if (Quantity <= tsk.QuantitaPrevista)
                    { 
                    tsk.QuantitaProdotta = Quantity;
                    ret = 1;
                    }
                    else
                    {
                        ret = 3;
                    }
                }
            }
            else
            {
                ret = 2;
            }

            return ret;
        }

        [Authorize]
        public ActionResult WorkstationKPI(int WorkstationID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Workplace/WebGemba/WorkstationKPI", "WorkstationID="+ WorkstationID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Workplace/WebGemba/WorkstationKPI", "WorkstationID=" + WorkstationID, ipAddr);
            }

            ViewBag.LeadTime = 0.0;
            ViewBag.PastLeadTime = 0.0;
            ViewBag.Delay = 0.0;
            ViewBag.PastDelay = 0.0;
            ViewBag.Quantity = 0;
            ViewBag.PastQuantity = 0;
            ViewBag.UnitaryWorkingTime = 0.0;

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

            ViewBag.WorkstationID = -1;
            ViewBag.workstationName = "";

            if (ViewBag.authX)
            {
                TaskProductionHistory History = new TaskProductionHistory(Session["ActiveWorkspace_Name"].ToString());
                History.loadTaskProductionHistory();


                var curr = History.TaskHistoricData.Where(p => p.WorkstationID == WorkstationID).ToList();

                var result = curr.Select(k => new
                {
                    k.TaskRealEndDate.Year,
                    k.TaskRealEndDate.Month,
                    k.TaskRealEndDateWeek,
                    k.TaskQuantityProduced,
                    k.TaskRealLeadTime,
                    k.TaskRealWorkingTime,
                    k.TaskRealDelay,
                    k.TaskOriginalID,
                    k.TaskName
                })
                               .GroupBy(x => new
                               {
                                   x.Year,
                                   x.TaskRealEndDateWeek,
                                   x.TaskOriginalID,
                                   x.TaskName
                               }, (key, group) => new
                               {
                                   TaskID = key.TaskOriginalID,
                                   TaskName = key.TaskName,
                                   Year = key.Year,
                                   Month = DateTime.UtcNow.Month,
                               //RealEndDate = DateTime.UtcNow,
                                    Week = key.TaskRealEndDateWeek,
                                   Quantity = group.Sum(k => k.TaskQuantityProduced),
                                   WorkingTime = group.Sum(k => k.TaskRealWorkingTime.TotalHours),
                                   UnitaryWorkingTime = group.Average(k => (k.TaskRealWorkingTime.TotalHours / k.TaskQuantityProduced)),
                                   LeadTime = group.Average(k => k.TaskRealLeadTime.TotalHours),
                                   Delay = group.Average(k => k.TaskRealDelay.TotalHours)
                               }).ToList();

                ViewBag.LeadTime = -1;
                try
                {

                    var currentWeek = result.First(x => x.Week == Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) && x.Year == DateTime.UtcNow.Year);
                    ViewBag.LeadTime = currentWeek.LeadTime;
                    ViewBag.Delay = currentWeek.Delay;
                    ViewBag.Quantity = currentWeek.Quantity;
                    ViewBag.CurrentUnitaryWorkingTime = currentWeek.UnitaryWorkingTime;
                }
                catch
                {
                    ViewBag.LeadTime = -1;
                }
                int pastWeekNum = Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) - 1;
                int pastWeekYear = DateTime.UtcNow.Year;
                if (pastWeekNum < 1)
                {
                    pastWeekNum = 52;
                    pastWeekYear--;
                }

                try
                {

                    var pastWeek = result.First(x => x.Week == pastWeekNum && x.Year == pastWeekYear);
                    ViewBag.PastLeadTime = pastWeek.LeadTime;
                    ViewBag.PastDelay = pastWeek.Delay;
                    ViewBag.PastQuantity = pastWeek.Quantity;
                    ViewBag.PastUnitaryWorkingTime = pastWeek.UnitaryWorkingTime;
                }
                catch
                {
                    ViewBag.LeadTime = -1;
                }
            

            }
            return View();
        }

        // WebGemba version 2.0
        [Authorize]
        public ActionResult ListWorkstationsTasks2(int WorkstationID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Page", "/Workplace/WebGemba/ListWorkstationsTasks", "workstationID=" + WorkstationID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Page", "/Workplace/WebGemba/ListWorkstationsTasks", "workstationID=" + WorkstationID, ipAddr);
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

            ViewBag.WorkstationID = -1;
            ViewBag.workstationName = "";
            ViewBag.user = "";

            ViewBag.TasksInExecution = new List<TaskProduzione>();
            ViewBag.TasksExecutable = new List<TaskProduzione>();

            if (ViewBag.authX)
            {
                Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), WorkstationID);
                User curr = (User)Session["user"];
                bool controlloLogin = false;
                p.loadUtentiLoggati();
                for (int i = 0; i < p.UtentiLoggati.Count; i++)
                {
                    if (p.UtentiLoggati[i] == curr.username)
                    {
                        controlloLogin = true;
                    }
                }
                if (p.id != -1 && controlloLogin)
                {
                    ViewBag.WorkstationID = p.id;
                    ViewBag.workstationName = p.name;

                    ViewBag.user = curr.username;
                    p.loadTaskAvviati(curr);
                    p.loadTaskProduzioneAvviabili();
                    List<int> results = p.IdTaskProduzioneAvviabili.Where(f => !p.TaskAvviatiUtente.Any(t => t == f)).ToList();
                    List<TaskProduzione> tasks = new List<TaskProduzione>();
                    for (int i = 0; i < results.Count; i++)
                    {
                        tasks.Add(new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), results[i]));
                    }
                    tasks = tasks.OrderBy(x => x.LateStart).ToList();

                    ViewBag.TasksExecutable = tasks;
                    List<TaskProduzione> TasksInExecution = new List<TaskProduzione>();
                    for(int i =0; i < p.TaskAvviatiUtente.Count; i++)
                    {
                        TasksInExecution.Add(new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), p.TaskAvviatiUtente[i]));
                    }

                    ViewBag.TasksInExecution = TasksInExecution;
                }
                else
                {
                    ViewBag.WorkstationID = -1;
                    ViewBag.workstationName = "";
                }

            }

            return View();
        }

        [Authorize]
        public JsonResult GetTasksAvailable(int WorkstationID, String user)
        {
            ViewBag.authX = false;
            List<JsonAvailableTasks> avTasks = new List<JsonAvailableTasks>();
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
                User curr = (User)Session["user"];
                Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), WorkstationID);
                bool controlloLogin = false;
                p.loadUtentiLoggati();
                for (int i = 0; i < p.UtentiLoggati.Count; i++)
                {
                    if (p.UtentiLoggati[i] == curr.username)
                    {
                        controlloLogin = true;
                    }
                }
                if (p.id != -1 && controlloLogin)
                {
                    p.loadTaskAvviati(curr);
                    p.loadTaskProduzioneAvviabili();
                    List<int> results = p.IdTaskProduzioneAvviabili.Where(f => !p.TaskAvviatiUtente.Any(t => t == f)).ToList();
                    for (int i = 0; i < results.Count; i++)
                    {
                        TaskProduzione currTask = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), results[i]);
                        JsonAvailableTasks currAvTask = new JsonAvailableTasks();
                        currAvTask.TaskID = currTask.TaskProduzioneID;
                        currAvTask.CustomerName = currTask.CustomerName;
                        currAvTask.ExternalID = "";
                        currAvTask.ExternalID = currTask.ExternalID;
                        Articolo currProd = new Articolo(Session["ActiveWorkspace_Name"].ToString(), currTask.ArticoloID, currTask.ArticoloAnno);
                        if (currTask.ExternalID.Length ==0)
                        {                            
                            currAvTask.ExternalID = currTask.ArticoloID.ToString() + "/" + currTask.ArticoloAnno.ToString();
                        }
                        currAvTask.ProdID = currTask.ArticoloID;
                        currAvTask.ProdYear = currTask.ArticoloAnno;
                        currAvTask.QuantityOrdered = currTask.QuantitaPrevista;
                        currAvTask.QuantityProduced = currTask.QuantitaProdotta;
                        currAvTask.EarlyStart = currTask.EarlyStart;
                        currAvTask.LateStart = currTask.LateStart;
                        currAvTask.TaskName = currTask.Name;
                        currAvTask.ProdName = currProd.Proc.variant.nomeVariante;
                        if (currAvTask.LateStart <= DateTime.UtcNow)
                        {
                            currAvTask.BgColor = "#FF0000";
                        }
                        else if (currAvTask.EarlyStart <= DateTime.UtcNow)
                        {
                            currAvTask.BgColor = "#FFFF00";
                        }
                        else
                        {
                            currAvTask.BgColor = "#FFFFFF";
                        }
                        avTasks.Add(currAvTask);
                    }
                    var avTasks2 = avTasks.OrderBy(x => x.LateStart);
                    return Json(JsonConvert.SerializeObject(avTasks2), JsonRequestBehavior.AllowGet);
                }
                else
                {
                }
            }
            var avTasks3 = avTasks.OrderBy(x => x.LateStart);
            return Json(JsonConvert.SerializeObject(avTasks3), JsonRequestBehavior.AllowGet);
        }

        public class JsonAvailableTasks
        {
            public int TaskID;
            public int ProdID;
            public int ProdYear;
            public String ExternalID;
            public String CustomerName;
            public String ProdName;
            public String TaskName;
            public Double QuantityOrdered;
            public Double QuantityProduced;
            public DateTime EarlyStart;
            public DateTime LateStart;
            public String BgColor;
        }

        [Authorize]
        public String GetWorkInstruction(int TaskID)
        {
            String ret = "";
            TaskProduzione curr = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
            if(curr!=null && curr.TaskProduzioneID!=-1)
            { 
            curr.loadWorkInstructionActive();
                if(curr.WorkInstructionActive!=null && curr.WorkInstructionActive.Path.Length > 0)
                {
                    ret = curr.WorkInstructionActive.Path;
                }
            }
            return ret;
        }

        /* Interface that shows user tasks */
        [Authorize]
        public ActionResult ListUserTasks()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Page", "/Workplace/WebGemba/ListUsersTasks", "user=" + Session["user"], ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Page", "/Workplace/WebGemba/ListWorkstationsTasks", "", ipAddr);
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

            ViewBag.user = "";

            if (ViewBag.authX)
            {
                User curr = (User)Session["user"];

                    ViewBag.user = curr.username;
                    curr.loadTaskAvviati();
                curr.LoadExecutableTasks();
                    List<int> results = curr.ExecutableTasks.Where(f => !curr.TaskAvviati.Any(t => t == f)).ToList();
                    List<TaskProduzione> tasks = new List<TaskProduzione>();
                    for (int i = 0; i < results.Count; i++)
                    {
                        tasks.Add(new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), results[i]));
                    }
                    tasks = tasks.OrderBy(x => x.LateStart).ToList();

                    ViewBag.TasksExecutableUser = tasks;
                    List<TaskProduzione> TasksInExecution = new List<TaskProduzione>();
                    for (int i = 0; i < curr.TaskAvviati.Count; i++)
                    {
                        TasksInExecution.Add(new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), curr.TaskAvviati[i]));
                    }

                    ViewBag.TasksInExecutionUser = TasksInExecution;

            }

            return View();
        }

        /* Returns:
         * 1 if task has started correctly
         * 2 if user has not the needed authorization
         * 3 if user exceeded the max number of tasks he can do simultaneously
         */
        [Authorize]
        public int StartTaskUser(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/StartTaskUser", "TaskID=" + TaskID +"&user="+Session["user"], ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/StartTaskUser", "TaskID=" + TaskID, ipAddr);
            }

            int ret = 1;
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
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    User curr = (User)Session["user"];
                    curr.DoCheckIn(new Postazione(Session["ActiveWorkspace_Name"].ToString(), tsk.PostazioneID));
                    bool rt = tsk.Start((User)Session["user"]);
                    if (rt)
                    {
                        ret = 1;
                    }
                    else
                    {
                        Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), tsk.RepartoID);
                        curr.loadTaskAvviati();
                        if (rp.TasksAvviabiliContemporaneamenteDaOperatore > 0 && curr.TaskAvviati.Count >= rp.TasksAvviabiliContemporaneamenteDaOperatore)
                        {
                            ret = 3;
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    ret = 2;
                }
            }
            else
            {
                ret = 4;
            }
            return ret;
        }

        /*Returns:
 * 1 if task has started correctly
 * 2 if an error occured
 * 3 if there are some mandatory parameters missing
 * 4 if user has not the needed authorization
 * 5 if there are some previous tasks that did not end
 */
        [Authorize]
        public int CompleteTaskUser(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/CompleteTaskUser", "TaskID=" + TaskID+"&user="+Session["user"], ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/CompleteTask", "TaskID=" + TaskID, ipAddr);
            }

            int ret = 1;
            String retS = "CompleteTask";
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
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), tsk.ArticoloID, tsk.ArticoloAnno);
                    User curr = (User)Session["user"];
                    Postazione pst = new Postazione(Session["ActiveWorkspace_Name"].ToString(), tsk.PostazioneID);
                    curr.DoCheckIn(pst);
                    bool rt = tsk.Complete((User)Session["user"]);
                    if (rt == true)
                    {
                        ret = 1;
                    }
                    else
                    {
                        // Set generic error, if none of the following cases appears to be true.
                        ret = 2;

                        // Check if fault is caused by previous tasks
                        tsk.loadPrecedenti();
                        for (int i = 0; i < tsk.PreviousTasks.Count; i++)
                        {
                            TaskProduzione tskPrev = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), tsk.PreviousTasks[i].NearTaskID);
                            if (tskPrev.Status != 'F')
                            {
                                ret = 5;
                            }
                        }

                        if (!tsk.CheckParametersComplete())
                        {
                            ret = 3;
                        }
                    }
                    curr.DoCheckOut(pst);
                }

                retS = tsk.log;

            }
            else
            {
                ret = 4;
            }
            return ret;
        }

        [Authorize]
        public JsonResult GetTasksAvailableUser(String user)
        {
            ViewBag.authX = false;
            List<JsonAvailableTasks> avTasks = new List<JsonAvailableTasks>();
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
                User curr = (User)Session["user"];

                    curr.loadTaskAvviati();
                curr.LoadExecutableTasks();
                    List<int> results = curr.ExecutableTasks.Where(f => !curr.TaskAvviati.Any(t => t == f)).ToList();
                    for (int i = 0; i < results.Count; i++)
                    {
                        TaskProduzione currTask = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), results[i]);
                        JsonAvailableTasks currAvTask = new JsonAvailableTasks();
                        currAvTask.TaskID = currTask.TaskProduzioneID;
                        currAvTask.CustomerName = currTask.CustomerName;
                        currAvTask.ExternalID = "";
                        currAvTask.ExternalID = currTask.ExternalID;
                        Articolo currProd = new Articolo(Session["ActiveWorkspace_Name"].ToString(), currTask.ArticoloID, currTask.ArticoloAnno);
                        if (currTask.ExternalID.Length == 0)
                        {
                            currAvTask.ExternalID = currTask.ArticoloID.ToString() + "/" + currTask.ArticoloAnno.ToString();
                        }
                        currAvTask.ProdID = currTask.ArticoloID;
                        currAvTask.ProdYear = currTask.ArticoloAnno;
                        currAvTask.QuantityOrdered = currTask.QuantitaPrevista;
                        currAvTask.QuantityProduced = currTask.QuantitaProdotta;
                        currAvTask.EarlyStart = currTask.EarlyStart;
                        currAvTask.LateStart = currTask.LateStart;
                        currAvTask.TaskName = currTask.Name;
                        currAvTask.ProdName = currProd.Proc.variant.nomeVariante;
                        if (currAvTask.LateStart <= DateTime.UtcNow)
                        {
                            currAvTask.BgColor = "#FF0000";
                        }
                        else if (currAvTask.EarlyStart <= DateTime.UtcNow)
                        {
                            currAvTask.BgColor = "#FFFF00";
                        }
                        else
                        {
                            currAvTask.BgColor = "#FFFFFF";
                        }
                        avTasks.Add(currAvTask);
                    }
                    var avTasks2 = avTasks.OrderBy(x => x.LateStart);
                    return Json(JsonConvert.SerializeObject(avTasks2), JsonRequestBehavior.AllowGet);

            }
            var avTasks3 = avTasks.OrderBy(x => x.LateStart);
            return Json(JsonConvert.SerializeObject(avTasks3), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult TaskOperatorNotesPanel(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Workplace/WebGemba/TaskOperatorNotesPanel", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Workplace/WebGemba/TaskOperatorNotesPanel", "TaskID=" + TaskID, ipAddr);
            }

            int ret = 0;
            ViewBag.authW = false;
            ViewBag.TaskID = -1;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if(tsk!=null && tsk.TaskProduzioneID!=-1)
                {
                    ViewBag.TaskID = tsk.TaskProduzioneID;
                    tsk.loadTaskOperatorNotes();
                    User curr = (User)Session["user"];
                    ViewBag.user = curr.username;
                    return View(tsk.TaskOperatorNotes);
                }
            }
                return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if note added/edited successfully
         * 2 if user is not authorized
         */
        [Authorize]
        public int AddTaskOperatorNote(int TaskID, int CommentID, String Note)
        {
            int ret = 0;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace_Name"].ToString(), TaskID);
                if(tsk!=null && tsk.TaskProduzioneID!=-1)
                {
                    User curr1 = (User)Session["user"];
                    ret = tsk.RegisterTaskOperatorNote(curr1.username, Note);
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
 