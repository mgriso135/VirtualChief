using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Quality.Controllers
{
    public class NonComplianceCausesController : Controller
    {
        // GET: Quality/NonComplianceCauses
        [Authorize]
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonComplianceCauses/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceCauses/Index", "", ipAddr);
            }

            ViewBag.authorized = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Causes";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authorized = true;
                NonComplianceCauses lstCause = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
                lstCause.loadCausesList();
                return View(lstCause.CausesList);
            }
            else {
                ViewBag.authorized = false;
                return View();
            }
        }
        [Authorize]
        public ActionResult Create()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonComplianceCauses/Create", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceCauses/Create", "", ipAddr);
            }

            ViewBag.Submitted = false;

            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Causes";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                var created = false;
                // Create the Client
                if (HttpContext.Request.RequestType == "POST")
                {
                    if (Request.Form["btn"] != null)
                    {
                        ViewBag.Submitted = true;
                        // If the request is POST, get the values from the form
                        var name = Request.Form["name"];
                        var description = Request.Form["description"];
                        KIS.App_Sources.NonComplianceCauses lstCauses = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
                        created = lstCauses.Add(Server.HtmlEncode(name), Server.HtmlEncode(description));
                        if (created)
                        {
                            Response.Redirect("~/Quality/NonComplianceCauses/Index");
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
        [Authorize]
        public ActionResult Delete(int id)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonComplianceCauses/Delete", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceCauses/Delete", "", ipAddr);
            }

            ViewBag.Deleted = false;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Causes";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                // Get the clients
                NonComplianceCauses ncCause = new NonComplianceCauses(Session["ActiveWorkspace_Name"].ToString());
                ViewBag.Deleted = ncCause.Delete(id);
                // Add the process details to the ViewBag
                if (ViewBag.Deleted)
                {
                    Response.Redirect("~/Quality/NonComplianceCauses/Index");
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult Update(int id)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                UserAccount usr = (UserAccount)Session["user"];
                Dati.Utilities.LogAction(usr.id.ToString(), "Controller", "/Quality/NonComplianceCauses/Update", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Quality/NonComplianceCauses/Update", "", ipAddr);
            }

            ViewBag.nctypeFound = false;
            ViewBag.authenticated = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "NonCompliance Causes";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                ViewBag.authenticated = true;
                if (HttpContext.Request.RequestType == "POST")
                {
                    // Request is Post type; must be a submit
                    var name = Request.Form["name"];
                    var description = Request.Form["description"];

                    NonComplianceCause ncCauseU = new NonComplianceCause(Session["ActiveWorkspace_Name"].ToString(), id);
                    if (ncCauseU != null && ncCauseU.ID != -1)
                    {
                        ncCauseU.Name = Server.HtmlEncode(name);
                        ncCauseU.Description = Server.HtmlEncode(description);
                        ViewBag.ncCauseFound = true;
                        Response.Redirect("~/Quality/NonComplianceCauses/Index");
                    }
                    else
                    {
                        ViewBag.ncCauseFound = false;
                    }
                }


                // Create a model object.
                NonComplianceCause ncCause = new NonComplianceCause(Session["ActiveWorkspace_Name"].ToString(), id);
                // Get the list of clients            
                if (ncCause != null && ncCause.ID != -1)
                {
                    ViewBag.ncCauseFound = true;
                    return View(ncCause);
                }
                else
                {
                    ViewBag.ncCauseFound = false;
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