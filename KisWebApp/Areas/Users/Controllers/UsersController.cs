using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Areas.Users.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users/Users
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Users/Users/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Users/Users/Index", "", ipAddr);
            }

            return View();
        }

        public ActionResult ActivateUser(String usr, String checksum)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us2r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us2r.username, "Controller", "/Users/Users/ActivateUser", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Users/Users/ActivateUser", "", ipAddr);
            }

            ViewBag.Activated = false;
            ViewBag.checksum = "NO";
            User curr = new User();
                Boolean ret = curr.Activate(usr, checksum);
                ViewBag.Activated = ret;
            ViewBag.checksum = checksum;
            return View();
        }

        public ActionResult WorkHoursManualRegistration(String usr)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/Users/Users/WorkHoursManualRegistration", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Users/Users/WorkHoursManualRegistration", "", ipAddr);
            }

            ViewBag.usr = null;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Working Hours Manual Registration";
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
                User curr = new User(usr);
                if(curr!=null && curr.username.Length > 0)
                {
                    ViewBag.usr = curr;
                    PortafoglioClienti customers = new PortafoglioClienti();
                    ViewBag.Customers = customers.Elenco.OrderBy(x => x.RagioneSociale);
                    ElencoCommesseAperte lstOpenOrders = new ElencoCommesseAperte();
                    ViewBag.OpenOrders = lstOpenOrders.Commesse.ToList();

                    ElencoArticoliAperti lstProducts = new ElencoArticoliAperti();
                    ViewBag.OpenProducts=lstProducts.ArticoliAperti.OrderBy(x=>x.Year).ThenBy(x=>x.ID).ToList();

                    List<TaskProduzione> lstOpenTasks = new List<TaskProduzione>();
                    ElencoTaskProduzione lstTasks = new ElencoTaskProduzione('I');
                    foreach (var m in lstTasks.Tasks)
                    {
                        lstOpenTasks.Add(new TaskProduzione(m.TaskProduzioneID));
                    }
                    lstTasks = new ElencoTaskProduzione('N');
                    foreach (var m in lstTasks.Tasks)
                    {
                        lstOpenTasks.Add(new TaskProduzione(m.TaskProduzioneID));
                    }
                    lstTasks = new ElencoTaskProduzione('P');
                    foreach (var m in lstTasks.Tasks)
                    {
                        lstOpenTasks.Add(new TaskProduzione(m.TaskProduzioneID));
                    }
                    ViewBag.OpenTasks = lstOpenTasks.OrderBy(x => x.TaskProduzioneID).ToList();
                }
            }
                return View();
        }

        public ActionResult ListUserWorkTimespans(String usr, DateTime date)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/Users/Users/ListUserWorkTimespans", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Users/Users/ListUserWorkTimespans", "", ipAddr);
            }

            ViewBag.usr = null;
            ViewBag.log = usr + date.ToString("dd/MM/yyyy");

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Working Hours Manual Registration";
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
                User curr = new User(usr);
                if (curr != null && curr.username.Length > 0)
                {
                    ViewBag.usr = curr;
                    ViewBag.username = curr.username;
                    DateTime dataInizio = new DateTime(date.Year, date.Month, date.Day);
                    DateTime dataFine = date.AddDays(1);
                        dataFine = new DateTime(dataFine.Year, dataFine.Month, dataFine.Day);
                    curr.loadWorkTimespansAllStatus(dataInizio, dataFine);
                    var listaIntervalli = (from interv in curr.IntervalliDiLavoroOperatore
                                           where (interv.DataInizio >= dataInizio && interv.DataInizio <= dataFine) || (interv.DataFine >= dataInizio && interv.DataFine <= dataFine)
                                           select interv).OrderBy(x => x.DataInizio).ToList();
                    ViewBag.log += listaIntervalli.Count;
                    return View(listaIntervalli);
                }
                else
                {
                    ViewBag.log = "No user found";
                }
            }
            return View();
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if start event not found
         * 3 if end event found
         */
        public int DeleteUserWorkTimespan(String usr, int StartEventID, int EndEventID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/Users/Users/DeleteUserWorkTimespan", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Users/Users/DeleteUserWorkTimespan", "", ipAddr);
            }

            int ret = 0;
            String retS = "0";
            ViewBag.log = ret;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Working Hours Manual Registration";
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
                User curr = new User(usr);
                if (usr != null)
                {
                    ret = curr.deleteIntervalloDiLavoroOperatore(StartEventID, EndEventID);
                    retS = curr.log;
                    ViewBag.log += " " + curr.log;
                }
            }
            return ret;
        }

        /* Returns:
         * 101 if user not authorized
         * 102 if task not found
         * 0 if generic error
         * 1 if everything is ok
         * 2 if start >= end
         * 3 if producedQuantity > Planned Quantity
         * 4 if registration ends in the future
         * 5 if user not found
         * 6 if user is currently working on the task
         * 7 if task not found
         * 8 if previous tasks were not ended
         * 9 if task not found or status == 'F'
         * 10 if error during the sql insert into commands in Task.Start function
         * 11 if user is already working on the max number of tasks
         * 12 if user is currently working on this task
         * 13 if user is not logged in the workstation
         * 14 if task is already in status F
         * 15 if user is not currently working on the task
         * 16 if all previous tasks are not finished
         * 17 if there are some parameters that needs to be defined
         * 18 if there are problems during the insert into queries
         */
        public int AddUserWorkTimespan(int TaskID, String usrID, bool completed, int producedQuantity, DateTime start, DateTime end)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Action", "/Users/Users/AddUserWorkTimespan", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Users/Users/AddUserWorkTimespan", "", ipAddr);
            }

            int ret = 100;
            String retS = ret.ToString();

            retS = "AddUserWorkTimespan: " + ret;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Working Hours Manual Registration";
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
                retS += "User authenticated. ";
                User usr = new User(usrID);
                TaskProduzione tsk = new TaskProduzione(TaskID);
                if (usr != null && usr.username.Length > 0 && tsk!=null && tsk.TaskProduzioneID!=-1)
                {
                    ret = usr.addIntervalloDiLavoroOperatore(tsk, usr, completed, producedQuantity, start, end);
                    retS += " " +ret+" "+ usr.log;
                }
                else { ret = 102;
                    retS += "Task not found";
                }
            }
            else { 
                ret = 101;
                retS += "User NOT authenticated";
            }
            return ret;
        }

        public JsonResult OpenOrders(String customer)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/Users/Users/OpenOrders", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Users/Users/OpenOrders", "", ipAddr);
            }

            ElencoCommesseAperte lstOpenOrder = new ElencoCommesseAperte();
            List<Commessa> lstOpenOrderCst= lstOpenOrder.Commesse;

            String[] curr3 = new String[2];
            IList<String[]> orders = new List<String[]>();

            if (customer != "")
            {
                lstOpenOrderCst = lstOpenOrder.Commesse.Where(x => x.Cliente == customer).ToList();
            }
            else
            {
                lstOpenOrderCst = lstOpenOrder.Commesse;
            }

            for (int i = 0; i < lstOpenOrderCst.Count; i++)
            {
                String[] curr = new String[4];
                curr[0] = lstOpenOrderCst[i].ID.ToString();
                curr[1] = lstOpenOrderCst[i].Year.ToString();
                curr[2] = lstOpenOrderCst[i].ExternalID;
                curr[3] = lstOpenOrderCst[i].Note;
                orders.Add(curr);
            }

            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OpenProducts(String customer, int OrderID, int OrderYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/Users/Users/OpenProducts", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Users/Users/OpenProducts", "", ipAddr);
            }

            ElencoArticoliAperti lstOpenProducts = new ElencoArticoliAperti();
            var lstOpenProductsFiltered = lstOpenProducts.ArticoliAperti;
            if(customer.Length > 0)
            {
                lstOpenProductsFiltered = lstOpenProductsFiltered.Where(x => x.Cliente == customer).OrderBy(y => y.Year).ThenBy(z => z.ID).ToList();
            }
            if(OrderID!=-1 && OrderYear!=-1)
            {
                lstOpenProductsFiltered = lstOpenProductsFiltered.Where(x => x.Commessa == OrderID && x.AnnoCommessa == OrderYear).ToList();
            }

            IList<String[]> products = new List<String[]>();

            for (int i = 0; i < lstOpenProductsFiltered.Count; i++)
            {
                String[] curr = new String[3];
                curr[0] = lstOpenProductsFiltered[i].ID.ToString();
                curr[1] = lstOpenProductsFiltered[i].Year.ToString();
                curr[2] = lstOpenProductsFiltered[i].Proc.process.processName + " - " + lstOpenProductsFiltered[i].Proc.variant.nomeVariante;
                products.Add(curr);
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OpenTasks(String customer, int OrderID, int OrderYear, int ProductID, int ProductYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User us1r = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(us1r.username, "Controller", "/Users/Users/OpenTasks", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Users/Users/OpenTasks", "", ipAddr);
            }

            List<TaskProduzione> lstOpenTasks = new List<TaskProduzione>();
            ElencoTaskProduzione lstTasks = new ElencoTaskProduzione('I');
            foreach (var m in lstTasks.Tasks)
            {
                lstOpenTasks.Add(new TaskProduzione(m.TaskProduzioneID));
            }
            lstTasks = new ElencoTaskProduzione('N');
            foreach (var m in lstTasks.Tasks)
            {
                lstOpenTasks.Add(new TaskProduzione(m.TaskProduzioneID));
            }
            lstTasks = new ElencoTaskProduzione('P');
            foreach (var m in lstTasks.Tasks)
            {
                lstOpenTasks.Add(new TaskProduzione(m.TaskProduzioneID));
            }

            lstOpenTasks = lstOpenTasks.OrderBy(y => y.ArticoloAnno).ThenBy(z => z.ArticoloID).ThenBy(w => w.TaskProduzioneID).ToList();
            
            if(customer.Length > 0)
            {
                lstOpenTasks = lstOpenTasks.Where(x => x.CustomerCode == customer).ToList();
            }

            if(OrderID !=-1 && OrderYear!=-1)
            {
                
            }

            if(ProductID!=-1 && ProductYear!=-1)
            {
                lstOpenTasks = lstOpenTasks.Where(x => x.ArticoloID == ProductID && x.ArticoloAnno == ProductYear).ToList();
            }

            IList<String[]> tasks = new List<String[]>();

            for (int i = 0; i < lstOpenTasks.Count; i++)
            {
                String[] curr = new String[2];
                curr[0] = lstOpenTasks[i].TaskProduzioneID.ToString();
                curr[1] = lstOpenTasks[i].Name;
                tasks.Add(curr);
            }

            return Json(tasks, JsonRequestBehavior.AllowGet);
        }
    }
}