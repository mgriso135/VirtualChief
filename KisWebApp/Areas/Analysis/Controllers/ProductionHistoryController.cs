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
        [Authorize]
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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                List<String[]> cst = new List<String[]>();
                PortafoglioClienti listCst = new PortafoglioClienti(Session["ActiveWorkspace_Name"].ToString());
                ViewBag.Customers = listCst.Elenco;
                ElencoReparti listDepts = new ElencoReparti(Session["ActiveWorkspace_Name"].ToString());
                ViewBag.Departments = listDepts.elenco;
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(Session["ActiveWorkspace_Name"].ToString(), true);
                var TypeOfProductsList = el.elencoFigli.OrderBy(x => x.NomeCombinato).ToList();
                ViewBag.TypeOfProducts = TypeOfProductsList;
                return View();
            }
            return View();
        }

        [Authorize]
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
                ProductionHistory History = new ProductionHistory(Session["ActiveWorkspace_Name"].ToString());
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

        [Authorize]
        public ActionResult HistoricDataPanelSales(DateTime startPeriod, DateTime endPeriod, String customers, String departments, String TypeOfProducts)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/ProductionHistory/HistoricDataPanelSales", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductionHistory/HistoricDataPanelSales", "", ipAddr);
            }

            ViewBag.startPeriod = startPeriod;
            ViewBag.endPeriod = endPeriod;
            ViewBag.customers = customers;
            ViewBag.departments = departments;
            ViewBag.products = TypeOfProducts;
            if (startPeriod < endPeriod)
            {
                ProductionHistory History = new ProductionHistory(Session["ActiveWorkspace_Name"].ToString());
                History.loadProductionHistory();
                List<ProductionHistoryStruct> curr = History.HistoricData.Where(x => x.ProductionOrderEndProductionDateReal > startPeriod && x.ProductionOrderEndProductionDateReal < endPeriod.AddDays(1)).ToList();

                var customerFilter = customers.Split(' ');

                curr = curr.Where(p => customerFilter.Any(l => p.CustomerID == l))
                           .ToList();

                String[] departmentsFilterStr = departments.Split(';');
                List<int> departmentsFilter = new List<int>();
                foreach (var deptStr in departmentsFilterStr)
                {
                    int deptInt = -1;
                    try
                    {
                        deptInt = Int32.Parse(deptStr);
                    }
                    catch (Exception ex)
                    {
                        deptInt = -1;
                    }
                    if (deptInt >= 0)
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
                        prod = -1; rev = -1; variant = -1;
                    }

                    if (prod != -1 && rev != -1 && variant != -1)
                    {
                        int[] prodCurr = new int[3];
                        prodCurr[0] = prod;
                        prodCurr[1] = rev;
                        prodCurr[2] = variant;
                        TypeOfProductsFilter.Add(prodCurr);
                    }
                }

                curr = curr.Where(p => TypeOfProductsFilter.Any(l => p.ProductTypeID == l[0] && p.ProductTypeReview == l[1] && p.ProductID == l[2])).ToList();

                var sorders1 = curr
                    .Select(o => new
                {
                    o.CustomerID,
                    o.CustomerName,
                    o.CustomerVATNumber,
                    o.CustomerCodiceFiscale,
                    o.CustomerAddress,
                    o.CustomerCity,
                    o.CustomerProvince,
                    o.CustomerZipCode,
                    o.CustomerCountry,
                    o.CustomerPhoneNumber,
                    o.CustomerEMail,
                    o.SalesOrderID,
                    o.SalesOrderYear,
                    o.SalesOrderCustomer,
                    o.SalesOrderDate,
                    o.SalesOrderNotes,
                    o.SalesOrderExternalID
                })
                .Distinct()
                .ToList();

                List<SalesOrderStruct> sorders = new List<SalesOrderStruct>();

                foreach (var m in sorders1)
                {
                    SalesOrderStruct csales = new SalesOrderStruct();
                    csales.CustomerID = m.CustomerID;
                    csales.CustomerName = m.CustomerName;
                    csales.CustomerVATNumber = m.CustomerVATNumber;
                    csales.CustomerCodiceFiscale = m.CustomerCodiceFiscale;
                    csales.CustomerAddress = m.CustomerAddress;
                    csales.CustomerCity = m.CustomerCity;
                    csales.CustomerProvince = m.CustomerProvince;
                    csales.CustomerZipCode = m.CustomerZipCode;
                    csales.CustomerCountry = m.CustomerCountry;
                    csales.CustomerPhoneNumber = m.CustomerPhoneNumber;
                    csales.CustomerEMail = m.CustomerEMail;
                    csales.SalesOrderID = m.SalesOrderID;
                    csales.SalesOrderYear = m.SalesOrderYear;
                    csales.SalesOrderCustomer = m.SalesOrderCustomer;
                    csales.SalesOrderDate = m.SalesOrderDate;
                    csales.SalesOrderNotes = m.SalesOrderNotes;
                    csales.SalesOrderExternalID = m.SalesOrderExternalID;

                    sorders.Add(csales);
                }
                return View(sorders);
            }
            return View();
        }

        [Authorize]
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
                UserAccount curr = (UserAccount)Session["user"];
                ckUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ckUser == true)
            {
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), ProdID, ProdYear);
                if(art!=null && art.ID!=-1)
                { 
                    ret = art.Riesuma();
                }
            }
            return ret;
        }

    }
}