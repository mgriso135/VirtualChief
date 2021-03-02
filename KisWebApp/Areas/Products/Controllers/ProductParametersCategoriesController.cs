using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Products.Controllers
{
    public class ProductParametersCategoriesController : Controller
    {
        // GET: Products/ProductParametersCategories
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/ProductParametersCategories/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/ProductParametersCategories/Index", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Product ParameterCategories";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
                
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product ParameterCategories";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Product ParameterCategories";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            ViewBag.authX = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authX = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR || ViewBag.authW || ViewBag.authX)
            {
                ViewBag.authorized = true;
                ProductParametersCategories paramCats = new ProductParametersCategories(Session["ActiveWorkspace"].ToString());
                paramCats.loadCategories();
                return View(paramCats.Categories);
            }
                return View();
        }

        public ActionResult Create()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/ProductParametersCategories/Create", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/ProductParametersCategories/Create", "", ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Product ParameterCategories";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            /*if (ViewBag.authW)
            {
                ViewBag.authorized = true;
            }*/
                return View();
        }

        public Boolean Add(String CategoryName, String CategoryDescription)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/ProductParametersCategories/Add", "CategoryName="+CategoryName, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/ProductParametersCategories/Add", "CategoryName=" + CategoryName, ipAddr);
            }

            Boolean ret = false;
            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Product ParameterCategories";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                KIS.App_Code.ProductParametersCategories prms = new ProductParametersCategories(Session["ActiveWorkspace"].ToString());
                ret = prms.Add(Server.HtmlEncode(CategoryName), Server.HtmlEncode(CategoryDescription));
            }
            return ret;
        }

        public Boolean Delete(int catID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/ProductParametersCategories/Delete", "catID="+catID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/ProductParametersCategories/Delete", "catID=" + catID, ipAddr);
            }

            Boolean ret = false;
            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Product ParameterCategories";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ProductParametersCategories prms = new ProductParametersCategories(Session["ActiveWorkspace"].ToString());
                ret = prms.Delete(catID);
            }
            return ret;
        }

        public Boolean Edit(int CategoryID, String CategoryName, String CategoryDescription)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/ProductParametersCategories/Edit", "CategoryID="+CategoryID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/ProductParametersCategories/Edit", "CategoryID=" + CategoryID, ipAddr);
            }

            Boolean ret = false;
            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Product ParameterCategories";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authW = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ViewBag.authW)
            {
                ProductParametersCategory prms = new ProductParametersCategory(Session["ActiveWorkspace"].ToString(), CategoryID);
                if(prms.ID > -1)
                {
                    prms.Name = Server.HtmlEncode(CategoryName);
                    prms.Description = Server.HtmlEncode(CategoryDescription);
                    ret = true;
                }
            }
            return ret;
        }
    }
}
 
 