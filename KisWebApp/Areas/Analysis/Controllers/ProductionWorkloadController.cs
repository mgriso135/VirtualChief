using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using Newtonsoft.Json;

namespace KIS.Areas.Analysis.Controllers
{
    public class ProductionWorkloadController : Controller
    {
        // GET: Analysis/ProductAnalysis
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/ProductionWorkload/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductionWorkload/Index", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi TipoProdotto";
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
                ElencoPostazioni elWorkstations = new ElencoPostazioni();
                ViewBag.Workstations = elWorkstations.elenco;
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
        public JsonResult ProductDataPanel(DateTime startPeriod, DateTime endPeriod, String DepartmentsOrWorkstations, String customers,
            String departments, String TypeOfProducts, int periodType, String workstations)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User cu1rr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(cu1rr.username, "Controller", "/Analysis/ProductionWorkload/ProductDataPanel", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/ProductionWorkload/ProductDataPanel", "", ipAddr);
            }

            ViewBag.DepartmentsOrWorkstations = DepartmentsOrWorkstations;
            ViewBag.startPeriod = startPeriod;
            ViewBag.endPeriod = endPeriod;
            ViewBag.customers = customers;
            ViewBag.departments = departments;
            ViewBag.products = TypeOfProducts;
            ViewBag.periodType = periodType;
            ViewBag.workstations = workstations;

            ViewBag.ProductsFilter = "";
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi TipoProdotto";
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
                if (startPeriod < endPeriod)
                {
                    TaskProductionHistory History = new TaskProductionHistory();
                    History.loadTasksProductionWorkload();

                    // Date filter
                    List<TaskProductionHistoryStruct> curr = History.TaskHistoricData.Where(x => x.TaskLateFinish >= startPeriod
                    && x.TaskLateFinish < endPeriod.AddDays(1)).ToList();
                    ViewBag.log += History.TaskHistoricData.Count.ToString();

                    // Customers filter
                    var customerFilter = customers.Split(' ');
                    curr = curr.Where(p => customerFilter.Any(l => p.CustomerID == l))
                               .ToList();

                    // Departments filter
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

                    // Workstations filter
                    String[] workstationsFilterStr = workstations.Split(';');
                    List<int> workstationsFilter = new List<int>();
                    foreach (var wstStr in workstationsFilterStr)
                    {
                        int wstInt = -1;
                        try
                        {
                            wstInt = Int32.Parse(wstStr);
                        }
                        catch (Exception ex)
                        {
                            wstInt = -1;
                        }
                        if (wstInt >= 0)
                        {
                            workstationsFilter.Add(wstInt);
                        }
                    }
                    curr = curr.Where(p => workstationsFilter.Any(l => p.WorkstationID == l)).ToList();

                    if (DepartmentsOrWorkstations == "Departments")
                    {
                        List<WorkloadAnalysisStruct> res = new List<WorkloadAnalysisStruct>();
                        if (periodType == 0)
                        {
                            // Daily
                            var result = curr.Select(k => new
                            {
                                k.DepartmentID,
                                k.DepartmentName,
                                k.TaskPlannedWorkingTime
                            })
                           .GroupBy(x => new { x.DepartmentID }, (key, group) => new
                           {
                               DepartmentID = key.DepartmentID,
                               DepartmentName = group.Select(k => k.DepartmentName).First().ToString(),
                               PlannedWorkingTime = group.Sum(k => k.TaskPlannedWorkingTime.TotalHours)
                           })
                           .OrderBy(g=>g.DepartmentName)
                           .ToList();

                            WorkloadAnalysisStruct wld = new WorkloadAnalysisStruct();
                            wld.EntityWorkload = new List<entityWorkload>();
                            wld.DateStr = "";
                            foreach (var row in result)
                            {
                                entityWorkload dpts = new entityWorkload();
                                dpts.EntityID = row.DepartmentID;
                                dpts.EntityName = row.DepartmentName;
                                dpts.Workload = row.PlannedWorkingTime;
                                wld.EntityWorkload.Add(dpts);
                            }
                            res.Add(wld);
    
                        }
                        else if (periodType == 1)
                        {
                            // Week
                            var result = curr.Select(k => new
                            {
                                k.DepartmentID,
                                k.DepartmentName,
                                k.TaskLateFinish.Year,
                                k.TaskLateFinish.Month,
                                k.TaskLateFinishWeek,
                                k.TaskPlannedWorkingTime
                            })
                           .GroupBy(x => new { x.DepartmentID, x.Year, x.TaskLateFinishWeek }, (key, group) => new
                           {
                               DepartmentID = key.DepartmentID,
                               DepartmentName = group.Select(x => x.DepartmentName).First().ToString(),
                               Year = key.Year,
                               Month = DateTime.UtcNow.Month,
                               DateStr = key.TaskLateFinishWeek + "/" + key.Year,
                               Week = key.TaskLateFinishWeek,
                               PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours)
                           }).ToList();

                            var periods = result
                               .Select(element => element.DateStr)
                               .Distinct();

                            var depts = result.Select(e => new { e.DepartmentID, e.DepartmentName}).Distinct();

                            /*foreach (var k in result)
                            {
                                TaskProductionAnalysisResultStruct currRes = new TaskProductionAnalysisResultStruct();
                                currRes.DepartmentID = k.DepartmentID;
                                currRes.DepartmentName = k.DepartmentName;
                                currRes.Week = k.Week;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.WorkingTime = k.PlannedWorkingTime;
                                res.Add(currRes);
                            }*/

                            foreach(var dt in periods)
                            {
                                var prds = result.Where(x => x.DateStr == dt).ToList();
                                WorkloadAnalysisStruct currWlds = new WorkloadAnalysisStruct();
                                currWlds.EntityWorkload = new List<entityWorkload>();
                                currWlds.DateStr = prds[0].DateStr;
                                currWlds.Week = prds[0].Week;
                                currWlds.Year = prds[0].Year;
                                //foreach (var itm in prds)
                                //{
                                    foreach(var dept in depts)
                                    {
                                        entityWorkload dptWld = new entityWorkload();
                                        dptWld.EntityID = dept.DepartmentID;
                                        dptWld.EntityName = dept.DepartmentName;
                                        dptWld.Workload = 0.0;
                                        try
                                        {
                                            var entWld = prds.Where(z => z.DepartmentID == dept.DepartmentID).First();
                                        dptWld.Workload = entWld.PlannedWorkingTime;
                                        }
                                        catch
                                        {
                                        dptWld.Workload = 0.0;
                                    }
                                    currWlds.EntityWorkload.Add(dptWld);
                                    }
                                //}
                                res.Add(currWlds);
                            }
                            //res = res.OrderBy(x => x.Year).ThenBy(y => y.Week).ThenBy(z => z.DepartmentName).ToList();
                            res = res.OrderBy(g => g.Year).ThenBy(h => h.Week).ToList();
                        }
                        else if (periodType == 2)
                        {
                            // Month
                            var result = curr.Select(k => new
                            {
                                k.DepartmentID,
                                k.DepartmentName,
                                k.TaskLateFinish.Year,
                                k.TaskLateFinish.Month,
                                k.TaskPlannedWorkingTime
                            })
                            .GroupBy(x => new { x.DepartmentID, x.Year, x.Month }, (key, group) => new
                            {
                                DepartmentID = key.DepartmentID,
                                DepartmentName = group.Select(x => x.DepartmentName).ToString(),
                                Year = key.Year,
                                Month = DateTime.UtcNow.Month,
                                RealEndDate = DateTime.UtcNow,
                                PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours)
                            }).ToList();



                            foreach (var k in result)
                            {
                                TaskProductionAnalysisResultStruct currRes = new TaskProductionAnalysisResultStruct();
                                currRes.DepartmentID = k.DepartmentID;
                                currRes.DepartmentName = k.DepartmentName;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.WorkingTime = k.PlannedWorkingTime;
                               // res.Add(currRes);
                            }
                           // res = res.OrderBy(x => x.Year).ThenBy(y => y.Month).ThenBy(z => z.DepartmentName).ToList();
                        }
                        else if (periodType == 3) // Daily
                        {
                            // Daily
                            var result = curr.Select(k => new
                            {
                                k.DepartmentID,
                                k.DepartmentName,
                                k.TaskLateFinish.Year,
                                k.TaskLateFinish.Month,
                                k.TaskLateFinish.Day,
                                k.TaskPlannedWorkingTime
                            })
                           .GroupBy(x => new { x.DepartmentID, x.Year, x.Month, x.Day }, (key, group) => new
                           {
                               DepartmentID = key.DepartmentID,
                               DepartmentName = group.Select(x => x.DepartmentName).ToString(),
                               Year = key.Year,
                               Month = key.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = 0,
                               Day = key.Day,
                               PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours)
                           }).ToList();

                            foreach (var k in result)
                            {
                                TaskProductionAnalysisResultStruct currRes = new TaskProductionAnalysisResultStruct();
                                currRes.DepartmentID = k.DepartmentID;
                                currRes.DepartmentName = k.DepartmentName;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.Day = k.Day;
                                currRes.WorkingTime = k.PlannedWorkingTime;
                                //res.Add(currRes);
                            }

                           // res = res.OrderBy(x => x.Year).ThenBy(y => y.Month).ThenBy(z => z.Day).ThenBy(r => r.DepartmentName).ToList();
                        }

                        ViewBag.content = JsonConvert.SerializeObject(res);
                        return Json(res);

                    }
                }
                
            }
            return Json("");

        }
    }
}