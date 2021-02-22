using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;

namespace KIS.Areas.Production.Controllers
{
    public class ProductionScheduleController : Controller
    {
        // GET: Production/ProductionSchedule
        public ActionResult Index()
        {
            ViewBag.authR = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);
            }

            // Articolo Depianifica
            ViewBag.authUndoSchedule = false;
            elencoPermessi = new List<String[]>();
            prmUser = new String[2];
            prmUser[0] = "Articolo Depianifica";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authUndoSchedule = curr.ValidatePermessi(elencoPermessi);
            }

            ViewBag.Departments = null;

            if (ViewBag.authR)
            {
                ElencoReparti deptList = new ElencoReparti(Session["ActiveWorkspace"].ToString());
                ViewBag.Departments = deptList.elenco;
                PortafoglioClienti listCst = new PortafoglioClienti(Session["ActiveWorkspace"].ToString());
                ViewBag.Customers = listCst.Elenco;
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                var TypeOfProductsList = el.elencoFigli.OrderBy(x => x.NomeCombinato).ToList();
                ViewBag.TypeOfProducts = TypeOfProductsList;

            }
            return View();
        }

        public ActionResult ProductionSchedulePanel(String customers, String departments, String TypeOfProducts, String status)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/ProductionHistory/ProductionSchedulePanel", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductionHistory/ProductionSchedulePanel", "", ipAddr);
            }

            ViewBag.customers = customers;
            ViewBag.departments = departments;
            ViewBag.products = TypeOfProducts;
            ViewBag.status = status;
            ViewBag.department = -1;

            ViewBag.authR = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            ViewBag.authW = false;

            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ViewBag.authR = curr.ValidatePermessi(elencoPermessi);

                elencoPermessi = new List<String[]>();
                prmUser = new String[2];
                prmUser[0] = "Articoli";
                prmUser[1] = "W";
                elencoPermessi.Add(prmUser);
                ViewBag.authW = curr.ValidatePermessi(elencoPermessi);

                elencoPermessi = new List<String[]>();
                prmUser = new String[2];
                prmUser[0] = "Articolo Depianifica";
                prmUser[1] = "X";
                elencoPermessi.Add(prmUser);
                ViewBag.authUndoSchedule = curr.ValidatePermessi(elencoPermessi);
            }

            if(ViewBag.authR)
            {
                ProductionSchedule schdl = new ProductionSchedule(Session["ActiveWorkspace"].ToString());
                schdl.loadProductionSchedule();
                List<ProductionOrderStruct> curr = schdl.ScheduledProducts;

                var customerFilter = customers.Split(' ');
                curr = curr.Where(p => customerFilter.Any(l => p.CustomerID == l))
                           .ToList();

                var statusFilter = status.Split(';');

                char[] statusFilter1 = new char[statusFilter.Length];
                for(int i = 0; i < statusFilter.Length; i++)
                {
                    if(statusFilter[i]!=null && statusFilter[i].Length>0)
                    { 
                    statusFilter1[i] = statusFilter[i][0];
                    }
                }
                curr = curr.Where(z => statusFilter1.Any(k => z.ProductionOrderStatus == k)).ToList();

                String[] departmentsFilterStr = departments.Split(';');
                List<int> departmentsFilter = new List<int>();
                foreach (var deptStr in departmentsFilterStr)
                {
                    int deptInt = -2;
                    try
                    {
                        deptInt = Int32.Parse(deptStr);
                    }
                    catch (Exception ex)
                    {
                        deptInt = -2;
                    }
                    if (deptInt >= -1)
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

                return View(curr);
            }

            return View();
        }

    }
}