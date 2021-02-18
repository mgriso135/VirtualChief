using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;
using KIS.App_Code;

namespace KIS.Areas.Config.Controllers
{
    public class NoProductiveTasksController : Controller
    {
        // GET: Config/NoProductiveTasks
        public ActionResult Index(int Type = 0)
        {
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config NoProductiveTasks";
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
                NoProductiveTasks nptasks = new NoProductiveTasks();
                if(Type == 1)
                {
                    nptasks.loadAll();
                }
                return View(nptasks.TaskList);
            }
            return View();
        }

        /* Returns:
         * -1 if generic error
         * TaskId if task added successfully
         * -2 if name is not valid
         * -3 if error while adding
         * -4 if invalid input
         * -5 if user not authorized
         */
        public int Add(String name, String description)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config NoProductiveTasks";
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
                if (name.Length < 255)
                {
                    NoProductiveTasks nptasks = new NoProductiveTasks();
                    ret = nptasks.Add(name, description);
                }
                else
                {
                    ret = -4;
                }
            }
            else
            {
                ret = -5;
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything went ok
         * -4 if input error
         * -5 if user not authorized
         */
        public int Edit(int NoProductiveTaskId, String name, String description, Boolean enabled)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config NoProductiveTasks";
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
                if (name.Length < 255)
                {
                    NoProductiveTask nptasks = new NoProductiveTask(NoProductiveTaskId);
                    if (nptasks.ID != -1)
                    {
                        nptasks.Name = name;
                        nptasks.Description = description;
                        nptasks.Enabled = enabled;
                        ret = 1;
                    }
                }
                else
                {
                    ret = -4;
                }
            }
            else
            {
                ret = -5;
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything went ok
         * -5 if user not authorized
         */
        public int MakeDefault(int NoProductiveTaskId)
        {
            int ret = 0;
            // Check write permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Config NoProductiveTasks";
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
                    NoProductiveTask nptasks = new NoProductiveTask(NoProductiveTaskId);
                    nptasks.IsDefault = true;
                ret = 1;
            }
            else
            {
                ret = -5;
            }
            return ret;
        }
    }
}