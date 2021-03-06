using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using System.Net;
using Newtonsoft.Json;
using KIS.App_Sources;

namespace KIS.Areas.Products.Controllers
{
    public class FinishedProductsController : Controller
    {
        // GET: Products/FinishedProducts
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/FinishedProducts/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/FinishedProducts/Index", "", ipAddr);
            }

            return View();
        }

        public ActionResult ProductDetails(int ProdID, int ProdYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/FinishedProducts/ProductDetails", "ProdID="+ProdID+"&ProdYear="+ProdYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/FinishedProducts/ProductDetails", "ProdID=" + ProdID + "&ProdYear=" + ProdYear, ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if(ViewBag.authR)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProdID, ProdYear);
                if(art!=null && art.ID!=-1 && art.Year!=-1)
                {
                    return View(art);
                }
            }
            return View();
        }

        public ActionResult ProductParametersList(int ProdID, int ProdYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/FinishedProducts/ProductParametersList", "ProdID=" + ProdID + "&ProdYear=" + ProdYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/FinishedProducts/ProductParametersList", "ProdID=" + ProdID + "&ProdYear=" + ProdYear, ipAddr);
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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProdID, ProdYear);
                if(art!=null && art.ID!=-1 && art.Year>2010)
                {
                    art.loadParameters();
                    return View(art.Parameters);
                }
            }
                return View();
        }

        public ActionResult ProductTasksParametersList(int ProdID, int ProdYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/FinishedProducts/ProductTasksParametersList", "ProdID=" + ProdID + "&ProdYear=" + ProdYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/FinishedProducts/ProductTasksParametersList", "ProdID=" + ProdID + "&ProdYear=" + ProdYear, ipAddr);
            }
            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Parameter";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProdID, ProdYear);
                if (art != null && art.ID != -1 && art.Year > 2010)
                {
                    art.loadTaskParameters();
                    return View(art.TaskParameters);
                }
            }
            return View();
        }

        public ActionResult BillingDetails(int ProdID, int ProdYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Products/FinishedProducts/BillingDetails", "ProdID=" + ProdID + "&ProdYear=" + ProdYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Products/FinishedProducts/BillingDetails", "ProdID=" + ProdID + "&ProdYear=" + ProdYear, ipAddr);
            }

            ViewBag.authR = false;
            ViewBag.authW = false;
            ViewBag.authX = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Clienti";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            String xapikey = "";
            ViewBag.BaseUrl = "";
            ViewBag.BasePath = "";
            ViewBag.CheckResponse = false;
            ViewBag.TasksResume = null;
            ViewBag.OrdersResume = null;
            ViewBag.ImprovementActions = null;
            ViewBag.NonCompliancesResume = null;
            ViewBag.CorrectiveActions = null;
            ViewBag.xapikey = "";
            ViewBag.ExpiryDate = "";
            ViewBag.log = "";
            if (ViewBag.authR)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProdID, ProdYear);
                if (art != null && art.ID != -1 && art.Year > 2010)
                {
                    art.loadTaskParameters();
                    ViewBag.x_api_key = "";
                    try
                    {
                        var BaseUrl = art.TaskParameters.First(x => x.Name == "BaseUrl")
                            .Description;
                        var BasePath = art.TaskParameters.First(x => x.Name == "BasePath")
                            .Description;

                        ViewBag.BaseUrl = BaseUrl;
                        ViewBag.BasePath = BasePath;

                        xapikey = art.TaskParameters.First(x => x.Name == "ConfigController X-API-KEY")
                            .Description;
                        ViewBag.xapikey = xapikey;
                    }
                    catch
                    {
                        ViewBag.BaseUrl = "";
                        ViewBag.BasePath = "";
                        xapikey = "";
                    }

                    if(ViewBag.BaseUrl.ToString().Length > 0 && ViewBag.BasePath.ToString().Length > 0 && xapikey.Length > 0)
                    {
                        try
                        {
                            WebClient client1 = new WebClient();
                            client1.Headers.Add("X-API-KEY", xapikey);
                            // Tasks per month
                            string responseExp = client1.UploadString(ViewBag.BaseUrl + ViewBag.BasePath + "/Config/Config/getExpiryDate", "POST", "");
                            ViewBag.ExpiryDate = JsonConvert.DeserializeObject<String>(responseExp);
                            ViewBag.CheckResponse = true;
                        }
                        catch
                        {
                        }
                        try
                        { 
                            WebClient client = new WebClient();
                            client.Headers.Add("X-API-KEY", xapikey);
                            // Tasks per month
                            string response = client.UploadString(ViewBag.BaseUrl + ViewBag.BasePath + "/Config/Config/getTasksResume", "POST", "");
                            ViewBag.TasksResume = JsonConvert.DeserializeObject<List<PeriodicReport>>(response);
                            ViewBag.CheckResponse = true;
                        }
                        catch { }

                        try
                        {
                            // Orders per month
                            WebClient client2 = new WebClient();
                            client2.Headers.Add("X-API-KEY", xapikey);
                            string responseO = client2.UploadString(ViewBag.BaseUrl + ViewBag.BasePath + "/Config/Config/getOrdersResume", "POST", "");
                            ViewBag.OrdersResume = JsonConvert.DeserializeObject<List<PeriodicReport>>(responseO);
                            ViewBag.CheckResponse = true;
                        }
                        catch { }

                        try { 
                    // Non compliances per month
                    WebClient client3 = new WebClient();
                            client3.Headers.Add("X-API-KEY", xapikey);
                            string responseNC = client3.UploadString(ViewBag.BaseUrl + ViewBag.BasePath + "/Config/Config/getNonCompliancesResume", "POST", "");
                            ViewBag.NonCompliances = JsonConvert.DeserializeObject<List<PeriodicReport>>(responseNC);
                        ViewBag.CheckResponse = true;
                        }
                        catch { }

                        try { 
                        // Corrective Actions per month
                        WebClient client = new WebClient();
                            client.Headers.Add("X-API-KEY", xapikey);
                            string responseCA = client.UploadString(ViewBag.BaseUrl + ViewBag.BasePath + "/Config/Config/getCorrectiveActionsResume", "POST", "");
                            ViewBag.CorrectiveActions = JsonConvert.DeserializeObject<List<PeriodicReport>>(responseCA);
                        ViewBag.CheckResponse = true;
                        }
                        catch { }

                        try { 
                        // Improvement Actions per month
                        WebClient client = new WebClient();
                            client.Headers.Add("X-API-KEY", xapikey);
                            string responseIA = client.UploadString(ViewBag.BaseUrl + ViewBag.BasePath + "/Config/Config/getImprovementActionsResume", "POST", "");
                            ViewBag.ImprovementActions = JsonConvert.DeserializeObject<List<PeriodicReport>>(responseIA);
                        ViewBag.CheckResponse = true;
                        }
                        catch { }
                    }
                    return View(art);
                }
            }
            return View();
        }



        public struct PeriodicReport
        {
            public int Value;
            public int Year;
            public int Month;
        }
    }
}