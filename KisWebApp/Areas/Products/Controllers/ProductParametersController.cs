using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Products.Controllers
{
    public class ProductParametersController : Controller
    {
        // GET: Products/ProductParameters
        [Authorize]
        public ActionResult Index(int processID, int processRev, int variantID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/ProductParameters/Index", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/ProductParameters/Index", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            ViewBag.processID = "";
            ViewBag.processRev = "";
            ViewBag.varianteID = "";

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
            ViewBag.processID ="A";
            ViewBag.processRev = "B";
            ViewBag.varianteID = "C";
            if (ViewBag.authR || ViewBag.authW || ViewBag.authX)
            {
                ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), processID, processRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if(prcVar!=null && prcVar.process!=null && prcVar.process.processID > -1 &&
                    prcVar.variant != null && prcVar.variant.idVariante > -1)
                {
                    ViewBag.processID = prcVar.process.processID;
                    ViewBag.processRev = prcVar.process.revisione;
                    ViewBag.varianteID = prcVar.variant.idVariante;
                    prcVar.loadParameters();
                    return View(prcVar.Parameters);
                }
            }
                return View();
        }
        [Authorize]
        public Boolean AddParam(int processID, int processRev, int variantID, String ParamName, String ParamDescription,
            int ParamCategory, Boolean ParamIsFixed, Boolean ParamIsRequired)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/ProductParameters/AddParam", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/ProductParameters/AddParam", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
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

            if (ViewBag.authR || ViewBag.authW || ViewBag.authX)
            {
                ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), processID, processRev),
                    new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if(prcVar!=null && prcVar.process!=null && prcVar.process.processID!=-1 &&
                    prcVar.variant!=null && prcVar.variant.idVariante!=-1)
                {
                    ret = prcVar.addParameter(Server.HtmlEncode(ParamName), Server.HtmlEncode(ParamDescription),
                        new ProductParametersCategory(Session["ActiveWorkspace_Name"].ToString(), ParamCategory), ParamIsFixed, ParamIsRequired);
                }   
            }
            return ret;
        }
        [Authorize]
        public Boolean DeleteParam(int processID, int processRev, int variantID, int ParamID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/ProductParameters/DeleteParam", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/ProductParameters/DeleteParam", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
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
                ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), 
                    processID, processRev), new variante(Session["ActiveWorkspace_Name"].ToString(), variantID));
                if(prcVar!=null && prcVar.process!=null && prcVar.process.processID!=-1 &&
                    prcVar.variant!=null && prcVar.variant.idVariante!=-1)
                {
                    ret = prcVar.deleteParameter(ParamID);
                }
            }
                return ret;
        }
        [Authorize]
        public Boolean EditParam(int processID, int processRev, int variantID, int ParamID,
            String paramName, String paramDescription, int paramCategory, Boolean isFixed, Boolean isRequired)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Products/ProductParameters/EditParam", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Products/ProductParameters/EditParam", "processID=" + processID + "&processRev=" + processRev + "&variantID=" + variantID, ipAddr);
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
                ModelParameter prodParam = new ModelParameter(Session["ActiveWorkspace_Name"].ToString(), processID, processRev, variantID, ParamID);
                if(prodParam.ParameterID!=-1)
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
    }
}