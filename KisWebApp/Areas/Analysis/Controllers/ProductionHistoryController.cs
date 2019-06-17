using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Analysis.Controllers
{
    public class ProductionHistoryController : Controller
    {
        // GET: Analysis/ProductionHistory
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/ProductionHistory/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductionHistory/Index", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Articolo Costo";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            ViewBag.authR = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }

            if (ViewBag.authR)
            {
                List<String[]> cst = new List<String[]>();
                PortafoglioClienti listCst = new PortafoglioClienti();
                ViewBag.Customers = listCst.Elenco;
                ElencoReparti listDepts = new ElencoReparti();
                ViewBag.Departments = listDepts.elenco;
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                var TypeOfProductsList = el.elencoFigli.OrderBy(x => x.NomeCombinato).ToList();
                ViewBag.TypeOfProducts = TypeOfProductsList;
                return View();
            }
            return View();
        }

        public ActionResult HistoricDataPanel(DateTime startPeriod, DateTime endPeriod, String customers, String departments, String TypeOfProducts)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/ProductionHistory/HistoricDataPanel", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductionHistory/HistoricDataPanel", "", ipAddr);
            }

            ViewBag.startPeriod = startPeriod;
            ViewBag.endPeriod = endPeriod;
            ViewBag.customers = customers;
            ViewBag.departments = departments;
            ViewBag.products = TypeOfProducts;
            if(startPeriod < endPeriod)
            { 
                ProductionHistory History = new ProductionHistory();
                History.loadProductionHistory();
                List<ProductionHistoryStruct> curr = History.HistoricData.Where(x=>x.ProductionOrderEndProductionDateReal > startPeriod && x.ProductionOrderEndProductionDateReal < endPeriod.AddDays(1)).ToList();

                var customerFilter = customers.Split(' ');

                curr = curr.Where(p => customerFilter.Any(l => p.CustomerID == l))
                           .ToList();

                String[] departmentsFilterStr = departments.Split(';');
                List<int> departmentsFilter = new List<int>();
                foreach(var deptStr in departmentsFilterStr)
                {
                    int deptInt = -1;
                    try
                    {
                        deptInt = Int32.Parse(deptStr);
                    }
                    catch(Exception ex)
                    {
                        deptInt = -1;
                    }
                    if(deptInt >=0)
                    {
                        departmentsFilter.Add(deptInt);
                    }
                }
                curr = curr.Where(p => departmentsFilter.Any(l => p.DepartmentID == l)).ToList();

                String[] TypeOfProductsFilterStr1 = TypeOfProducts.Split(';');
                List<int[]> TypeOfProductsFilter = new List<int[]>();
                foreach (var prod1 in TypeOfProductsFilterStr1)
                {
                    var prodStr = prod1.Split('_');
                    int prod = -1;
                    int rev = -1;
                    int variant = -1;
                    
                    try
                    {
                        prod = Int32.Parse(prodStr[0]);
                        rev = Int32.Parse(prodStr[1]);
                        variant = Int32.Parse(prodStr[2]);
                    }
                    catch
                    {
                        prod = -1;rev = -1; variant = -1;
                    }

                    if(prod!=-1 && rev!=-1 && variant != -1)
                    {
                        int[] prodCurr = new int[3];
                        prodCurr[0] = prod;
                        prodCurr[1] = rev;
                        prodCurr[2] = variant;
                        TypeOfProductsFilter.Add(prodCurr);
                    }
                }

                curr = curr.Where(p => TypeOfProductsFilter.Any(l => p.ProductTypeID == l[0] && p.ProductTypeReview==l[1] && p.ProductID==l[2])).ToList();

                return View(curr);
            }
            return View();
            //}

        }

        public Boolean ExhumateProduct(int ProdID, int ProdYear)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Analysis/ProductionHistory/ExhumateProduct", "ProdID="+ProdID+"&ProdYear="+ProdYear, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Analysis/ProductionHistory/ExhumateProduct", "ProdID = "+ProdID+" & ProdYear = "+ProdYear, ipAddr);
            }

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articolo Riesuma";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ckUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (ckUser == true)
            {
                Articolo art = new Articolo(ProdID, ProdYear);
                if(art!=null && art.ID!=-1)
                { 
                    ret = art.Riesuma();
                }
            }
            return ret;
        }

        public Boolean ExhumateTask(int TaskID)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Action", "/Analysis/ProductionHistory/ExhumateTask", "TaskID=" + TaskID, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Action", "/Analysis/ProductionHistory/ExhumateTask", "TaskID = " + TaskID, ipAddr);
            }

            Boolean ret = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "TaskProduzione Riesuma";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ckUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (ckUser == true)
            {
                TaskProduzione tskProd = new TaskProduzione(TaskID);
                if (tskProd.Status == 'F')
                {
                    Boolean rt = tskProd.Riesuma();
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }

    }
}