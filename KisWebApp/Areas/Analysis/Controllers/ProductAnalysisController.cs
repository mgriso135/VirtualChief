using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Analysis.Controllers
{
    public class ProductAnalysisController : Controller
    {
        // GET: Analysis/ProductAnalysis
        [Authorize]
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"]!=null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/ProductAnalysis/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductAnalysis/Index", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi TipoProdotto";
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
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                var TypeOfProductsList = el.elencoFigli.OrderBy(x => x.NomeCombinato).ToList();
                ViewBag.TypeOfProducts = TypeOfProductsList;
                return View();
            }
            return View();
        }

        /*periodType:
         * 0 = none,
         * 1 = week,
         * 2 = month
         * 
         * graphType:
         * 0 = Quantity-per-period
         * 1 = Sum of Working hours per-period
         * 2 = Average of unitary working hours per-period
         * 3 = Average lead time
         * 4 = Average of delays
         */
        [Authorize]
        public ActionResult ProductDataPanel(DateTime startPeriod, DateTime endPeriod, String customers, String departments, String TypeOfProducts, Boolean GroupProducts, int periodType, int graphType)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"] != null)
            {
                KIS.App_Code.User cu1rr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(cu1rr.username, "Controller", "/Analysis/ProductAnalysis/ProductDataPanel", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductAnalysis/ProductDataPanel", "", ipAddr);
            }

            ViewBag.startPeriod = startPeriod;
            ViewBag.endPeriod = endPeriod;
            ViewBag.customers = customers;
            ViewBag.departments = departments;
            ViewBag.products = TypeOfProducts;
            ViewBag.graphType = graphType;
            ViewBag.periodType = periodType;
            ViewBag.groupProducts = GroupProducts;
            ViewBag.ProductsFilter = "";
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi TipoProdotto";
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
                if (startPeriod < endPeriod)
                {
                    ProductionHistory History = new ProductionHistory(Session["ActiveWorkspace_Name"].ToString());
                    History.loadProductionAnalysis();

                    List<ProductionAnalysisStruct> curr = History.AnalysisData.Where(x => x.ProductionOrderEndProductionDateReal > startPeriod && x.ProductionOrderEndProductionDateReal < endPeriod.AddDays(1)).ToList();
                    ViewBag.log += History.AnalysisData.Count.ToString();
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

                    if(GroupProducts)
                    {
                        List<ProductionAnalysisResultStruct> res = new List<ProductionAnalysisResultStruct>();
                        if(periodType == 0)
                        {
                            // Daily
                            var result = curr.Select(k => new {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderEndProductionDateReal.Day,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.RealWorkingTime,
                                k.PlannedWorkingTime,
                                k.RealDelay,
                                k.Productivity
                            })
                           .GroupBy(x => new { x.Year, x.Month, x.Day }, (key, group) => new
                           {
                               Year = key.Year,
                               Month = key.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = 0,
                               Day = key.Day,
                               Quantity = group.Sum(k => k.ProductionOrderQuantityProduced),
                               WorkingTime = group.Sum(k => k.RealWorkingTime.TotalHours),
                               UnitaryWorkingTime = group.Average(k => (k.RealWorkingTime.TotalHours / k.ProductionOrderQuantityProduced)),
                               LeadTime = group.Average(k => k.RealLeadTime.TotalHours),
                               Delay = group.Average(k => k.RealDelay.TotalHours),
                               //Productivity = group.Sum(k => k.PlannedWorkingTime.TotalSeconds) / group.Sum(k => k.RealWorkingTime.TotalSeconds)
                               Productivity = group.Average(k => k.Productivity)
                           }).ToList();

                            foreach (var k in result)
                            {
                                ProductionAnalysisResultStruct currRes = new ProductionAnalysisResultStruct();
                                currRes.Delay = k.Delay;
                                currRes.LeadTime = k.LeadTime;
                                currRes.Month = k.Month;
                                currRes.Day = k.Day;
                                currRes.Year = k.Year;
                                currRes.Quantity = k.Quantity;
                                currRes.RealEndDate = k.RealEndDate;
                                currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                                currRes.Week = k.Week;
                                currRes.WorkingTime = k.WorkingTime;
                                currRes.Productivity = k.Productivity;

                                res.Add(currRes);
                            }

                            res = res.OrderBy(x => x.Year).ThenBy(y=>y.Month).ThenBy(z=>z.Day).ToList();
                        }
                        else if(periodType==1)
                        {
                            // Week
                            var result = curr.Select(k => new {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderEndProductionDateRealWeek,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.RealWorkingTime,
                                k.PlannedWorkingTime,
                                k.RealDelay,
                                k.Productivity
                            })
                           .GroupBy(x => new { x.Year, x.ProductionOrderEndProductionDateRealWeek }, (key, group) => new
                           {
                               Year = key.Year,
                               Month = DateTime.UtcNow.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = key.ProductionOrderEndProductionDateRealWeek,
                               Quantity = group.Sum(k => k.ProductionOrderQuantityProduced),
                               WorkingTime = group.Sum(k => k.RealWorkingTime.TotalHours),
                               UnitaryWorkingTime = group.Average(k => (k.RealWorkingTime.TotalHours / k.ProductionOrderQuantityProduced)),
                               LeadTime = group.Average(k => k.RealLeadTime.TotalHours),
                               Delay = group.Average(k => k.RealDelay.TotalHours),
                               //Productivity = group.Sum(k => k.PlannedWorkingTime.TotalSeconds) / group.Sum(k => k.RealWorkingTime.TotalSeconds)
                               Productivity = group.Average(k => k.Productivity)
                           }).ToList();



                            foreach (var k in result)
                            {
                                ProductionAnalysisResultStruct currRes = new ProductionAnalysisResultStruct();
                                currRes.Delay = k.Delay;
                                currRes.LeadTime = k.LeadTime;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.Quantity = k.Quantity;
                                currRes.RealEndDate = k.RealEndDate;
                                currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                                currRes.Week = k.Week;
                                currRes.WorkingTime = k.WorkingTime;
                                currRes.Productivity = k.Productivity;
                                res.Add(currRes);
                            }

                            res = res.OrderBy(x => x.Year).ThenBy(y=>y.Week).ToList();
                        }
                        else if (periodType == 2)
                        {
                            // Month
                            var result = curr.Select(k => new {
                            k.ProductionOrderEndProductionDateReal.Year,
                            k.ProductionOrderEndProductionDateReal.Month,
                            k.ProductionOrderQuantityProduced,
                            k.RealLeadTime,
                            k.RealWorkingTime,
                                k.PlannedWorkingTime,
                                k.RealDelay,
                            k.Productivity
                            })
                            .GroupBy(x => new { x.Year, x.Month }, (key, group) => new
                            {
                                Year = key.Year,
                                Month = key.Month,
                                RealEndDate = DateTime.UtcNow,
                                Week = 0,
                                Quantity = group.Sum(k => k.ProductionOrderQuantityProduced),
                                WorkingTime = group.Sum(k => k.RealWorkingTime.TotalHours),
                                UnitaryWorkingTime = group.Average(k => (k.RealWorkingTime.TotalHours / k.ProductionOrderQuantityProduced)),
                                LeadTime = group.Average(k=>k.RealLeadTime.TotalHours),                                
                                Delay = group.Average(k=>k.RealDelay.TotalHours),
                                //Productivity = group.Sum(k => k.PlannedWorkingTime.TotalSeconds) / group.Sum(k => k.RealWorkingTime.TotalSeconds)
                                Productivity = group.Average(k => k.Productivity)
                            }).ToList();
                        

                        
                        foreach (var k in result)
                        {
                            ProductionAnalysisResultStruct currRes = new ProductionAnalysisResultStruct();
                            currRes.Delay = k.Delay;
                            currRes.LeadTime = k.LeadTime;
                            currRes.Month = k.Month;
                            currRes.Year = k.Year;
                            currRes.Quantity = k.Quantity;
                            currRes.RealEndDate = k.RealEndDate;
                            currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                            currRes.Week = k.Week;
                            currRes.WorkingTime = k.WorkingTime;
                                currRes.Productivity = k.Productivity;
                                res.Add(currRes);
                        }
                            res = res.OrderBy(x => x.Year).ThenBy(y => y.Month).ToList();
                        }

                        return View(res);
                    }
                    else // I'm not grouping all products. Just group for each type of product
                    {
                        List<ProductionAnalysisResultStruct> res = new List<ProductionAnalysisResultStruct>();
                        String[] TypeOfProductsFilterStr1 = TypeOfProducts.Split(';');
                    List<int[]> TypeOfProductsFilter = new List<int[]>();
                        ViewBag.ProductsFilter = "data.addColumn('string', '" + ResProductAnalysis.ProductPanel.lblPeriod + "');";

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
                                processo prcCurr = new processo(Session["ActiveWorkspace_Name"].ToString(), prod, rev);
                                variante varCurr = new variante(Session["ActiveWorkspace_Name"].ToString(), variant);
                                ViewBag.ProductsFilter += "data.addColumn('number', '" + prcCurr.processName + " - " + varCurr.nomeVariante + "');";
                        }
                    }

                    curr = curr.Where(p => TypeOfProductsFilter.Any(l => p.ProductTypeID == l[0] && p.ProductTypeReview == l[1] && p.ProductID == l[2])).ToList();
                        if (periodType == 0)
                        {
                            // Daily
                            var result = curr.Select(k => new {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderEndProductionDateRealWeek,
                                k.ProductionOrderEndProductionDateReal.Day,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.PlannedWorkingTime,
                                k.RealWorkingTime,
                                k.RealDelay,
                                k.ProductTypeID,
                                k.ProductTypeReview,
                                k.ProductID,
                                k.ProductTypeName,
                                k.ProductName,
                                k.Productivity
                            })
                           .GroupBy(x => new {
                               x.Year,
                               x.Month,
                               x.ProductionOrderEndProductionDateRealWeek,
                               x.Day,
                               x.ProductTypeID,
                               x.ProductTypeReview,
                               x.ProductID,
                               x.ProductTypeName,
                               x.ProductName
                           }, (key, group) => new
                           {
                               Year = key.Year,
                               Month = key.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = key.ProductionOrderEndProductionDateRealWeek,
                               Day = key.Day,
                               ProductTypeID = key.ProductTypeID,
                               ProductTypeReview = key.ProductTypeReview,
                               ProductID = key.ProductID,
                               ProductTypeName = key.ProductTypeName,
                               ProductName = key.ProductName,
                               Quantity = group.Sum(k => k.ProductionOrderQuantityProduced),
                               WorkingTime = group.Sum(k => k.RealWorkingTime.TotalHours),
                               UnitaryWorkingTime = group.Average(k => (k.RealWorkingTime.TotalHours / k.ProductionOrderQuantityProduced)),
                               LeadTime = group.Average(k => k.RealLeadTime.TotalHours),
                               Delay = group.Sum(k => k.RealDelay.TotalHours),
                               //Productivity = group.Sum(k => k.PlannedWorkingTime.TotalSeconds) / group.Sum(k => k.RealWorkingTime.TotalSeconds)
                               Productivity = group.Average(k => k.Productivity)
                           }).ToList();

                            foreach (var k in result)
                            {
                                ProductionAnalysisResultStruct currRes = new ProductionAnalysisResultStruct();
                                currRes.Delay = k.Delay;
                                currRes.LeadTime = k.LeadTime;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.Quantity = k.Quantity;
                                currRes.RealEndDate = k.RealEndDate;
                                currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                                currRes.Week = k.Week;
                                currRes.Day = k.Day;
                                currRes.WorkingTime = k.WorkingTime;
                                currRes.ProductTypeID = k.ProductTypeID;
                                currRes.ProductID = k.ProductID;
                                currRes.ProductReview = k.ProductTypeReview;
                                currRes.ProductName = k.ProductName;
                                currRes.ProductTypeName = k.ProductTypeName;
                                currRes.Productivity = k.Productivity;
                                res.Add(currRes);
                            }

                            res = res.OrderBy(z => z.RealEndDate).ToList();

                            ViewBag.ProductsFilter += "data.addRows([";
                            DateTime currPeriod = startPeriod;
                            while(currPeriod <= endPeriod)
                            {
                                String qtyCurr = "";
                                var currSet = res.Where(x => x.Day == currPeriod.Day && x.Month == currPeriod.Month && x.Year == currPeriod.Year).ToList();

                                for(int i = 0; i < TypeOfProductsFilter.Count; i++)
                                {
                                    try
                                    {
                                        var currItm = currSet.First(x => x.ProductTypeID == TypeOfProductsFilter[i][0] && x.ProductReview == TypeOfProductsFilter[i][1]
                                        && x.ProductID == TypeOfProductsFilter[i][2]);

                                        /*graphType:
                                            * 0 = Quantity - per - period
                                            * 1 = Sum of Working hours per-period
                                            * 2 = Mean of unitary working hours per - period
                                            * 3 = Mean lead time
                                            * 4 = Sum of delays
                                            * 5 = Average of Productivity
                                        */
                                        switch (graphType)
                                        {
                                            case 0:  qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                            case 1: qtyCurr += currItm.WorkingTime.ToString().Replace(',', '.'); break;
                                            case 2:  qtyCurr += currItm.UnitaryWorkingTime.ToString().Replace(',', '.'); break;
                                            case 3: qtyCurr += currItm.LeadTime.ToString().Replace(',', '.'); break;
                                            case 4:  qtyCurr += currItm.Delay.ToString().Replace(',', '.');  break;
                                            case 5: qtyCurr += currItm.Productivity.ToString().Replace(',', '.'); break;
                                            default: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                        }

                                    }
                                    catch
                                    {
                                        qtyCurr += "0";
                                    }
                                    if (i < TypeOfProductsFilter.Count - 1)
                                    {
                                        qtyCurr += ", ";
                                    }
                                }
                                ViewBag.ProductsFilter += "['" + currPeriod.ToString("dd/MM/yyyy") + "', " + qtyCurr + "]";
                                if(currPeriod < endPeriod)
                                {
                                    ViewBag.ProductsFilter += ", ";
                                }
                                currPeriod = currPeriod.AddDays(1);
                            }
                            ViewBag.ProductsFilter += "]);";
                        }
                        else if (periodType == 1)
                        {
                            // Week
                            var result = curr.Select(k => new {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderEndProductionDateRealWeek,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.RealWorkingTime,
                                k.PlannedWorkingTime,
                                k.RealDelay,
                                k.ProductTypeID,
                                k.ProductTypeReview,
                                k.ProductID,
                                k.ProductTypeName,
                                k.ProductName,
                                k.Productivity
                            })
                           .GroupBy(x => new { x.Year, x.ProductionOrderEndProductionDateRealWeek, x.ProductTypeID, x.ProductTypeReview, x.ProductID, x.ProductTypeName,
                           x.ProductName}, (key, group) => new
                           {
                               Year = key.Year,
                               Month = DateTime.UtcNow.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = key.ProductionOrderEndProductionDateRealWeek,
                               ProductTypeID = key.ProductTypeID,
                               ProductTypeReview = key.ProductTypeReview,
                               ProductID = key.ProductID,
                               ProductTypeName = key.ProductTypeName,
                               ProductName = key.ProductName,
                               Quantity = group.Sum(k => k.ProductionOrderQuantityProduced),
                               WorkingTime = group.Sum(k => k.RealWorkingTime.TotalHours),
                               UnitaryWorkingTime = group.Average(k => (k.RealWorkingTime.TotalHours / k.ProductionOrderQuantityProduced)),
                               LeadTime = group.Average(k => k.RealLeadTime.TotalHours),
                               Delay = group.Sum(k => k.RealDelay.TotalHours),
                               //Productivity = group.Sum(k => k.PlannedWorkingTime.TotalSeconds) / group.Sum(k => k.RealWorkingTime.TotalSeconds)
                               Productivity = group.Average(k => k.Productivity)
                           }).ToList();



                            foreach (var k in result)
                            {
                                ProductionAnalysisResultStruct currRes = new ProductionAnalysisResultStruct();
                                currRes.Delay = k.Delay;
                                currRes.LeadTime = k.LeadTime;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.Quantity = k.Quantity;
                                currRes.RealEndDate = k.RealEndDate;
                                currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                                currRes.Week = k.Week;
                                currRes.WorkingTime = k.WorkingTime;
                                currRes.ProductTypeID = k.ProductTypeID;
                                currRes.ProductID = k.ProductID;
                                currRes.ProductReview = k.ProductTypeReview;
                                currRes.ProductName = k.ProductName;
                                currRes.ProductTypeName = k.ProductTypeName;
                                currRes.Productivity = k.Productivity;
                                res.Add(currRes);
                            }
                            res = res.OrderBy(z => z.Year).ThenBy(w => w.Week).ToList();

                            ViewBag.ProductsFilter += "data.addRows([";
                            DateTime currPeriod = startPeriod;
                            while (currPeriod <= endPeriod)
                            {
                                String qtyCurr = "";
                                var currSet = res.Where(x=> x.Week == Dati.Utilities.GetWeekOfTheYear(currPeriod) && x.RealEndDate.Year == currPeriod.Year).ToList();

                                for (int i = 0; i < TypeOfProductsFilter.Count; i++)
                                {
                                    try
                                    {
                                        var currItm = currSet.First(x => x.ProductTypeID == TypeOfProductsFilter[i][0] && x.ProductReview == TypeOfProductsFilter[i][1]
                                        && x.ProductID == TypeOfProductsFilter[i][2]);
                                        /*graphType:
                                            * 0 = Quantity - per - period
                                            * 1 = Sum of Working hours per-period
                                            * 2 = Mean of unitary working hours per - period
                                            * 3 = Mean lead time
                                            * 4 = Sum of delays
                                        */
                                        switch (graphType)
                                        {
                                            case 0: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                            case 1: qtyCurr += currItm.WorkingTime.ToString().Replace(',', '.'); break;
                                            case 2: qtyCurr += currItm.UnitaryWorkingTime.ToString().Replace(',', '.'); break;
                                            case 3: qtyCurr += currItm.LeadTime.ToString().Replace(',', '.'); break;
                                            case 4: qtyCurr += currItm.Delay.ToString().Replace(',', '.'); break;
                                            case 5: qtyCurr += currItm.Productivity.ToString().Replace(',', '.'); break;
                                            default: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                        }
                                    }
                                    catch
                                    {
                                        qtyCurr += "0";
                                    }
                                    if (i < TypeOfProductsFilter.Count - 1)
                                    {
                                        qtyCurr += ", ";
                                    }
                                }
                                ViewBag.ProductsFilter += "['" + Dati.Utilities.GetWeekOfTheYear(currPeriod) + "/" + currPeriod.Year.ToString() + "', " + qtyCurr + "]";
                                if(currPeriod.AddDays(7) < endPeriod)
                                {
                                    ViewBag.ProductsFilter += ", ";
                                }
                                currPeriod = currPeriod.AddDays(7);
                            }
                            ViewBag.ProductsFilter += "]);";

                        }
                        else if (periodType == 2)
                        {
                            // Month
                            var result = curr.Select(k => new {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.RealWorkingTime,
                                k.PlannedWorkingTime,
                                k.RealDelay,
                                k.ProductTypeID,
                                k.ProductTypeReview,
                                k.ProductID,
                                k.ProductTypeName,
                                k.ProductName,
                                k.Productivity
                            })
                            .GroupBy(x => new { x.Year, x.Month,
                                x.ProductTypeID,
                                x.ProductTypeReview,
                                x.ProductID,
                                x.ProductTypeName,
                                x.ProductName
                            }, (key, group) => new
                            {
                                Year = key.Year,
                                Month = key.Month,
                                RealEndDate = DateTime.UtcNow,
                                Week = 0,
                                ProductTypeID = key.ProductTypeID,
                                ProductTypeReview = key.ProductTypeReview,
                                ProductID = key.ProductID,
                                ProductTypeName = key.ProductTypeName,
                                ProductName = key.ProductName,
                                Quantity = group.Sum(k => k.ProductionOrderQuantityProduced),
                                WorkingTime = group.Sum(k => k.RealWorkingTime.TotalHours),
                                UnitaryWorkingTime = group.Average(k => (k.RealWorkingTime.TotalHours / k.ProductionOrderQuantityProduced)),
                                LeadTime = group.Average(k => k.RealLeadTime.TotalHours),
                                Delay = group.Sum(k => k.RealDelay.TotalHours),
                                //Productivity = group.Sum(k => k.PlannedWorkingTime.TotalSeconds) / group.Sum(k => k.RealWorkingTime.TotalSeconds)
                                Productivity = group.Average(k => k.Productivity)
                            }).ToList();



                            foreach (var k in result)
                            {
                                ProductionAnalysisResultStruct currRes = new ProductionAnalysisResultStruct();
                                currRes.Delay = k.Delay;
                                currRes.LeadTime = k.LeadTime;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.Quantity = k.Quantity;
                                currRes.RealEndDate = k.RealEndDate;
                                currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                                currRes.Week = k.Week;
                                currRes.WorkingTime = k.WorkingTime;
                                currRes.ProductTypeID = k.ProductTypeID;
                                currRes.ProductID = k.ProductID;
                                currRes.ProductReview = k.ProductTypeReview;
                                currRes.ProductName = k.ProductName;
                                currRes.ProductTypeName = k.ProductTypeName;
                                currRes.Productivity = k.Productivity;
                                res.Add(currRes);
                            }
                            res = res.OrderBy(z => z.Year).ThenBy(w => w.Month).ToList();

                            ViewBag.ProductsFilter += "data.addRows([";
                            DateTime currPeriod = startPeriod;
                            while (currPeriod <= endPeriod)
                            {
                                String qtyCurr = "";
                                var currSet = res.Where(x => x.Month == currPeriod.Month && x.Year == currPeriod.Year).ToList();

                                for (int i = 0; i < TypeOfProductsFilter.Count; i++)
                                {
                                    try
                                    {
                                        var currItm = currSet.First(x => x.ProductTypeID == TypeOfProductsFilter[i][0] && x.ProductReview == TypeOfProductsFilter[i][1]
                                        && x.ProductID == TypeOfProductsFilter[i][2]);
                                        /*graphType:
                                            * 0 = Quantity - per - period
                                            * 1 = Sum of Working hours per-period
                                            * 2 = Mean of unitary working hours per - period
                                            * 3 = Mean lead time
                                            * 4 = Sum of delays
                                        */
                                        switch (graphType)
                                        {
                                            case 0: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                            case 1: qtyCurr += currItm.WorkingTime.ToString().Replace(',', '.'); break;
                                            case 2: qtyCurr += currItm.UnitaryWorkingTime.ToString().Replace(',', '.'); break;
                                            case 3: qtyCurr += currItm.LeadTime.ToString().Replace(',', '.'); break;
                                            case 4: qtyCurr += currItm.Delay.ToString().Replace(',', '.'); break;
                                            case 5: qtyCurr += currItm.Productivity.ToString().Replace(',', '.'); break;
                                            default: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                        }
                                    }
                                    catch
                                    {
                                        qtyCurr += "0";
                                    }

                                    if (i < TypeOfProductsFilter.Count - 1)
                                    {
                                        qtyCurr += ", ";
                                    }
                                }
                                ViewBag.ProductsFilter += "['" + currPeriod.Month.ToString() + "/" + currPeriod.Year.ToString() + "', " + qtyCurr + "]";
                                DateTime currCmp = new DateTime(currPeriod.Year, currPeriod.Month, 1);
                                DateTime endCmp = new DateTime(endPeriod.Year, endPeriod.Month, 1);
                                if (currCmp < endCmp)
                                {
                                    ViewBag.ProductsFilter += ", ";
                                }
                                currPeriod = currPeriod.AddMonths(1);
                            }
                            ViewBag.ProductsFilter += "]);";
                        }

                        return View(res);

                    }
                }
            }
            return View();
        }
    }
}