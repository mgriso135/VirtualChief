using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using Newtonsoft.Json;

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
                ViewBag.InputPointId = ip.id;
                ip.loadDepartments();
                ip.loadWorkstations();
                ElencoReparti lstdepts = new KIS.App_Code.ElencoReparti(Session["ActiveWorkspace_Name"].ToString());
                ViewBag.lstdepts = lstdepts.elenco;
                return View(ip);
            }
            return View();
        }

        /* Returns:
         * -3 if tenant is not defined
         * -2 if user not authorized
         * -1 if InputPoint not found
         *  1 if everything is ok
         */
        [Authorize]
        public int EditInputPoint(int InputPointId, String name, String description)
        {
            int ret = -3;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                ViewBag.authW = false;
                List<String[]> elencoPermessi = new List<String[]>();
                String[] prmUser = new String[2];
                prmUser[0] = "InoutPoint Details";
                prmUser[1] = "W";
                elencoPermessi.Add(prmUser);

                if (Session["user"] != null)
                {
                    UserAccount curr = (UserAccount)Session["user"];
                    ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
                }

                if (ViewBag.authW)
                {
                    InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), InputPointId);
                    if(ip.id > -1)
                    {
                        ip.name = name;
                        ip.description = description;
                        ret = 1;
                    }
                    else
                    {
                        ret = -1;
                    }
                }
                else
                {
                    ret = -2;
                }
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if tenant not set or invalid department
         * 3 if error while writing to database
         * 4 if department is already associated with this input point
         * 5 if user not authorized
         */
        [Authorize]
        public String AddDepartment(int InputPointId, int DepartmentId)
        {
            String[] ret = new String[3];
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                ViewBag.authW = false;
                List<String[]> elencoPermessi = new List<String[]>();
                String[] prmUser = new String[2];
                prmUser[0] = "InoutPoint Details";
                prmUser[1] = "W";
                elencoPermessi.Add(prmUser);

                if (Session["user"] != null)
                {
                    UserAccount curr = (UserAccount)Session["user"];
                    ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
                }

                if (ViewBag.authW)
                {
                    InputPoint ip = new InputPoint(Session["ActiveWorkspace_Name"].ToString(), InputPointId);
                    if (ip.id > -1)
                    {
                        Reparto dept = new Reparto(Session["ActiveWorkspace_Name"].ToString(), DepartmentId);
                        if(dept.id!=-1)
                        { 
                            ret[0] = ip.addDepartment(dept).ToString();
                            ret[1] = dept.name;
                            ret[2] = dept.id.ToString();
                        }
                        else
                        {
                            ret[0] = "2";
                        }
                    }
                    else
                    {
                        ret[0] = "-1";
                    }
                }
                else
                {
                    ret[0] = "5";
                }
            }
            else
            {
                ret[0] = "2";
            }
            return JsonConvert.SerializeObject(ret);
        }

        /* Returns:
        * 0 if generic error
        * 1 if InputPointDepartment delete successfully
        * 2 if tenant, department or input point not set
        * 3 if error while deleting
        * 9 if user not authorized
        */
        [Authorize]
        public int UnlinkDepartment(int InputPointId, int DepartmentId)
        {
            int ret = 0;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0 && InputPointId>-1 && DepartmentId>-1)
            {
                ViewBag.authW = false;
                List<String[]> elencoPermessi = new List<String[]>();
                String[] prmUser = new String[2];
                prmUser[0] = "InoutPoint Details";
                prmUser[1] = "W";
                elencoPermessi.Add(prmUser);

                if (Session["user"] != null)
                {
                    UserAccount curr = (UserAccount)Session["user"];
                    ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
                }

                if (ViewBag.authW)
                {
                    InputPointDepartment ip = new InputPointDepartment(Session["ActiveWorkspace_Name"].ToString(), InputPointId, DepartmentId);
                    if (ip!=null && ip.departmentId > -1 && ip.inputpointId>-1)
                    {
                        ret = ip.delete();
                    }
                    else
                    {
                       ret = 2;
                    }
                }
                else
                {
                    ret=9;
                }
            }
            else
            {
                ret=2;
            }
            return ret;
        }
    }
}