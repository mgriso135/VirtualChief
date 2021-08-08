using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;

namespace KIS.Areas.Production.Controllers
{
    public class InputPointsController : Controller
    {
        // GET: Production/InputPoints
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.authR = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "InputPoints List";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            ViewBag.authW = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "InputPoints List";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                InputPoints iplist = new InputPoints(Session["ActiveWorkspace_Name"].ToString());
                iplist.loadInputPoints();
                return View(iplist.list);
            }
            return View();
        }

        /* Returns:
         * -3 if tenant is not defined
         * -2 if name or tenant are not valid
         * -1 if generic error
         * InputPointId if everything is ok
         */
        [Authorize]
        public int AddInputPoint(String name, String description)
        {
            int ret = -3;
            if(Session["ActiveWorkspace_Name"]!= null && Session["ActiveWorkspace_Name"].ToString().Length>0)
            { 
                ViewBag.authW = false;
                List<String[]> elencoPermessi = new List<String[]>();
                String[] prmUser = new String[2];
                prmUser[0] = "InputPoints List";
                prmUser[1] = "W";
                elencoPermessi.Add(prmUser);

                if (Session["user"] != null)
                {
                    UserAccount curr = (UserAccount)Session["user"];
                    ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
                }

                if(ViewBag.authW)
                {
                    UserAccount curr = (UserAccount)Session["user"];

                    InputPoints iplist = new InputPoints(Session["ActiveWorkspace_Name"].ToString());
                    ret = iplist.addInputPoint(name, description, curr.id);
                }
            }
            return ret;
        }

        [Authorize]
        public ActionResult InputPointDetail(int id)
        {
            ViewBag.authR = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "InputPoints List";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            ViewBag.authW = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "InputPoints List";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), id);
                return View(ip);
            }
            return View();
        }
    }
}