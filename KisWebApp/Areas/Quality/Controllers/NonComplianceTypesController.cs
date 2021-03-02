using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KIS.App_Sources;
using KIS.App_Code;

namespace KIS.Areas.Quality.Controllers
{
    public class NonComplianceTypesController : Controller
    {
        // GET: Quality/NonComplianceTypes
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonComplianceTypes/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceTypes/Index", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Types";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authorized = true;
                NonComplianceTypes ncTypes = new NonComplianceTypes(Session["ActiveWorkspace"].ToString());
                ncTypes.loadTypeList();
                return View(ncTypes.TypeList);
            }
            else
            {
                ViewBag.authorized = false;
                ViewBag.Message = "";
                return View();
            }
        }

        public ActionResult Create()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonComplianceTypes/Create", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceTypes/Create", "", ipAddr);
            }

            ViewBag.Submitted = false;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Types";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                var created = false;
                // Create the Client
                if (HttpContext.Request.RequestType == "POST")
                {
                    if(Request.Form["btn"]!=null)
                    { 
                    ViewBag.Submitted = true;
                    // If the request is POST, get the values from the form
                    var name = Request.Form["name"];
                    var description = Request.Form["description"];
                    KIS.App_Sources.NonComplianceTypes lstTypes = new NonComplianceTypes(Session["ActiveWorkspace"].ToString());
                    created = lstTypes.Add(Server.HtmlEncode(name), Server.HtmlEncode(description));
                    if (created)
                    {
                        Response.Redirect("~/Quality/NonComplianceTypes/Index");
                    }
                    else
                    {
                    }
                    }
                }                
                return View();
            }
            else
            {
                ViewBag.authenticated = false;
                return View();
            }

            
        }

        public ActionResult Delete(int id)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonComplianceTypes/Delete", "id="+id, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceTypes/Delete", "id=" + id, ipAddr);
            }

            ViewBag.Deleted = false;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Types";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                // Get the clients
                NonComplianceTypes ncList = new NonComplianceTypes(Session["ActiveWorkspace"].ToString());
                ViewBag.Deleted = ncList.Delete(id);
                // Add the process details to the ViewBag
                if (ViewBag.Deleted)
                {
                    Response.Redirect("~/Quality/NonComplianceTypes/Index");
                }
            }
            return View();
        }

        public ActionResult Update(int id)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User usr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(usr.username, "Controller", "/Quality/NonComplianceTypes/Update", "id="+id, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceTypes/Update", "id=" + id, ipAddr);
            }

            ViewBag.nctypeFound = false;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Types";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                if (HttpContext.Request.RequestType == "POST")
                {
                    // Request is Post type; must be a submit
                    var name = Request.Form["name"];
                    var description = Request.Form["description"];

                    NonComplianceType ncTypeU = new NonComplianceType(Session["ActiveWorkspace"].ToString(), id);
                    if (ncTypeU != null && ncTypeU.ID != -1)
                    {
                        ncTypeU.Name = Server.HtmlEncode(name);
                        ncTypeU.Description = Server.HtmlEncode(description);
                        ViewBag.nctypeFound = true;
                    }
                    else
                    {
                        ViewBag.nctypeFound = false;
                    }

                    Response.Redirect("~/Quality/NonComplianceTypes/Index");
                }


                // Create a model object.
                NonComplianceType ncType = new NonComplianceType(Session["ActiveWorkspace"].ToString(), id);
                // Get the list of clients            
                if (ncType != null && ncType.ID != -1)
                {
                    ViewBag.nctypeFound = true;
                    return View(ncType);
                }
                else
                {
                    ViewBag.nctypeFound = false;
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}