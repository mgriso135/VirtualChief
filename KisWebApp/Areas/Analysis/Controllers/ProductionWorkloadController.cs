﻿using System;
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
        [Authorize]
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
                ElencoPostazioni elWorkstations = new ElencoPostazioni(Session["ActiveWorkspace_Name"].ToString());
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
        [Authorize]
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
                UserAccount curr = (UserAccount)Session["user"];
                ViewBag.authR = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (ViewBag.authR)
            {
                if (startPeriod < endPeriod)
                {
                    TaskProductionHistory History = new TaskProductionHistory(Session["ActiveWorkspace_Name"].ToString());
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

                    List<WorkloadAnalysisStruct> res = new List<WorkloadAnalysisStruct>();
                    if (DepartmentsOrWorkstations == "Departments")
                    {
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

                            foreach(var dt in periods)
                            {
                                var prds = result.Where(x => x.DateStr == dt).ToList();
                                WorkloadAnalysisStruct currWlds = new WorkloadAnalysisStruct();
                                currWlds.EntityWorkload = new List<entityWorkload>();
                                currWlds.DateStr = prds[0].DateStr;
                                currWlds.Week = prds[0].Week;
                                currWlds.Year = prds[0].Year;
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
                                res.Add(currWlds);
                            }
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
                                DepartmentName = group.Select(x => x.DepartmentName).First().ToString(),
                                Year = key.Year,
                                Month = DateTime.UtcNow.Month,
                                DateStr = key.Month + "/" + key.Year,
                                date = new DateTime(key.Year, key.Month, 1),
                                PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours)
                            }).ToList();

                            var periods = result
                               .Select(element => element.DateStr)
                               .Distinct();

                            var depts = result.Select(e => new { e.DepartmentID, e.DepartmentName }).Distinct();

                            foreach (var dt in periods)
                            {
                                var prds = result.Where(x => x.DateStr == dt).ToList();
                                WorkloadAnalysisStruct currWlds = new WorkloadAnalysisStruct();
                                currWlds.EntityWorkload = new List<entityWorkload>();
                                currWlds.Date = new DateTime(prds[0].Year, prds[0].Month, 1);
                                currWlds.DateStr = prds[0].date.ToString("yyyy-MM-dd");
                                currWlds.Month = prds[0].Month;
                                currWlds.Year = prds[0].Year;
                                foreach (var dept in depts)
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
                                res.Add(currWlds);
                            }
                            res = res.OrderBy(g => g.Year).ThenBy(h => h.Month).ToList();
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
                               DepartmentName = group.Select(x => x.DepartmentName).First().ToString(),
                               Year = key.Year,
                               Month = key.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = 0,
                               Day = key.Day,
                               PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours),
                               DateStr = key.Year + "-" + key.Month + "-" + key.Day
                           }).ToList();

                            var periods = result
                               .Select(element => element.DateStr)
                               .Distinct();

                            var depts = result.Select(e => new { e.DepartmentID, e.DepartmentName }).Distinct();

                            foreach (var dt in periods)
                            {
                                var prds = result.Where(x => x.DateStr == dt).ToList();
                                WorkloadAnalysisStruct currWlds = new WorkloadAnalysisStruct();
                                currWlds.EntityWorkload = new List<entityWorkload>();
                                currWlds.Date = new DateTime(prds[0].Year, prds[0].Month, prds[0].Day);
                                currWlds.DateStr = prds[0].DateStr;
                                currWlds.Month = prds[0].Month;
                                currWlds.Year = prds[0].Year;
                                foreach (var dept in depts)
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
                                res.Add(currWlds);
                            }
                            res = res.OrderBy(g => g.Date).ToList();
                        }
                        

                    }
                    else // Visualization by Workstation
                    {
                        if (periodType == 0)
                        {
                            // Daily
                            var result = curr.Select(k => new
                            {
                                k.WorkstationID,
                                k.WorkstationName,
                                k.TaskPlannedWorkingTime
                            })
                           .GroupBy(x => new { x.WorkstationID }, (key, group) => new
                           {
                               WorkstationID = key.WorkstationID,
                               WorkstationName = group.Select(k => k.WorkstationName).First().ToString(),
                               PlannedWorkingTime = group.Sum(k => k.TaskPlannedWorkingTime.TotalHours)
                           })
                           .OrderBy(g => g.WorkstationName)
                           .ToList();

                            WorkloadAnalysisStruct wld = new WorkloadAnalysisStruct();
                            wld.EntityWorkload = new List<entityWorkload>();
                            wld.DateStr = "";
                            foreach (var row in result)
                            {
                                entityWorkload dpts = new entityWorkload();
                                dpts.EntityID = row.WorkstationID;
                                dpts.EntityName = row.WorkstationName;
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
                                k.WorkstationID,
                                k.WorkstationName,
                                k.TaskLateFinish.Year,
                                k.TaskLateFinish.Month,
                                k.TaskLateFinishWeek,
                                k.TaskPlannedWorkingTime
                            })
                           .GroupBy(x => new { x.WorkstationID, x.Year, x.TaskLateFinishWeek }, (key, group) => new
                           {
                               WorkstationID = key.WorkstationID,
                               WorkstationName = group.Select(x => x.WorkstationName).First().ToString(),
                               Year = key.Year,
                               Month = DateTime.UtcNow.Month,
                               DateStr = key.TaskLateFinishWeek + "/" + key.Year,
                               Week = key.TaskLateFinishWeek,
                               PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours)
                           }).ToList();

                            var periods = result
                               .Select(element => element.DateStr)
                               .Distinct();

                            var depts = result.Select(e => new { e.WorkstationID, e.WorkstationName }).Distinct();

                            foreach (var dt in periods)
                            {
                                var prds = result.Where(x => x.DateStr == dt).ToList();
                                WorkloadAnalysisStruct currWlds = new WorkloadAnalysisStruct();
                                currWlds.EntityWorkload = new List<entityWorkload>();
                                currWlds.DateStr = prds[0].DateStr;
                                currWlds.Week = prds[0].Week;
                                currWlds.Year = prds[0].Year;
                                foreach (var dept in depts)
                                {
                                    entityWorkload dptWld = new entityWorkload();
                                    dptWld.EntityID = dept.WorkstationID;
                                    dptWld.EntityName = dept.WorkstationName;
                                    dptWld.Workload = 0.0;
                                    try
                                    {
                                        var entWld = prds.Where(z => z.WorkstationID == dept.WorkstationID).First();
                                        dptWld.Workload = entWld.PlannedWorkingTime;
                                    }
                                    catch
                                    {
                                        dptWld.Workload = 0.0;
                                    }
                                    currWlds.EntityWorkload.Add(dptWld);
                                }
                                res.Add(currWlds);
                            }
                            res = res.OrderBy(g => g.Year).ThenBy(h => h.Week).ToList();
                        }
                        else if (periodType == 2)
                        {
                            // Month
                            var result = curr.Select(k => new
                            {
                                k.WorkstationID,
                                k.WorkstationName,
                                k.TaskLateFinish.Year,
                                k.TaskLateFinish.Month,
                                k.TaskPlannedWorkingTime
                            })
                            .GroupBy(x => new { x.WorkstationID, x.Year, x.Month }, (key, group) => new
                            {
                                WorkstationID = key.WorkstationID,
                                WorkstationName = group.Select(x => x.WorkstationName).First().ToString(),
                                Year = key.Year,
                                Month = DateTime.UtcNow.Month,
                                DateStr = key.Month + "/" + key.Year,
                                date = new DateTime(key.Year, key.Month, 1),
                                PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours)
                            }).ToList();

                            var periods = result
                               .Select(element => element.DateStr)
                               .Distinct();

                            var depts = result.Select(e => new { e.WorkstationID, e.WorkstationName }).Distinct();

                            foreach (var dt in periods)
                            {
                                var prds = result.Where(x => x.DateStr == dt).ToList();
                                WorkloadAnalysisStruct currWlds = new WorkloadAnalysisStruct();
                                currWlds.EntityWorkload = new List<entityWorkload>();
                                currWlds.Date = new DateTime(prds[0].Year, prds[0].Month, 1);
                                currWlds.DateStr = prds[0].date.ToString("yyyy-MM-dd");
                                currWlds.Month = prds[0].Month;
                                currWlds.Year = prds[0].Year;
                                foreach (var dept in depts)
                                {
                                    entityWorkload dptWld = new entityWorkload();
                                    dptWld.EntityID = dept.WorkstationID;
                                    dptWld.EntityName = dept.WorkstationName;
                                    dptWld.Workload = 0.0;
                                    try
                                    {
                                        var entWld = prds.Where(z => z.WorkstationID == dept.WorkstationID).First();
                                        dptWld.Workload = entWld.PlannedWorkingTime;
                                    }
                                    catch
                                    {
                                        dptWld.Workload = 0.0;
                                    }
                                    currWlds.EntityWorkload.Add(dptWld);
                                }
                                res.Add(currWlds);
                            }
                            res = res.OrderBy(g => g.Year).ThenBy(h => h.Month).ToList();
                        }
                        else if (periodType == 3) // Daily
                        {
                            // Daily
                            var result = curr.Select(k => new
                            {
                                k.WorkstationID,
                                k.WorkstationName,
                                k.TaskLateFinish.Year,
                                k.TaskLateFinish.Month,
                                k.TaskLateFinish.Day,
                                k.TaskPlannedWorkingTime
                            })
                           .GroupBy(x => new { x.WorkstationID, x.Year, x.Month, x.Day }, (key, group) => new
                           {
                               WorkstationID = key.WorkstationID,
                               WorkstationName = group.Select(x => x.WorkstationName).First().ToString(),
                               Year = key.Year,
                               Month = key.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = 0,
                               Day = key.Day,
                               PlannedWorkingTime = group.Sum(x => x.TaskPlannedWorkingTime.TotalHours),
                               DateStr = key.Year + "-" + key.Month + "-" + key.Day
                           }).ToList();

                            var periods = result
                               .Select(element => element.DateStr)
                               .Distinct();

                            var depts = result.Select(e => new { e.WorkstationID, e.WorkstationName }).Distinct();

                            foreach (var dt in periods)
                            {
                                var prds = result.Where(x => x.DateStr == dt).ToList();
                                WorkloadAnalysisStruct currWlds = new WorkloadAnalysisStruct();
                                currWlds.EntityWorkload = new List<entityWorkload>();
                                currWlds.Date = new DateTime(prds[0].Year, prds[0].Month, prds[0].Day);
                                currWlds.DateStr = prds[0].DateStr;
                                currWlds.Month = prds[0].Month;
                                currWlds.Year = prds[0].Year;
                                foreach (var dept in depts)
                                {
                                    entityWorkload dptWld = new entityWorkload();
                                    dptWld.EntityID = dept.WorkstationID;
                                    dptWld.EntityName = dept.WorkstationName;
                                    dptWld.Workload = 0.0;
                                    try
                                    {
                                        var entWld = prds.Where(z => z.WorkstationID == dept.WorkstationID).First();
                                        dptWld.Workload = entWld.PlannedWorkingTime;
                                    }
                                    catch
                                    {
                                        dptWld.Workload = 0.0;
                                    }
                                    currWlds.EntityWorkload.Add(dptWld);
                                }
                                res.Add(currWlds);
                            }
                            res = res.OrderBy(g => g.Date).ToList();
                        }
                    }

                    return Json(res);

                }

                
            }
            return Json("");

        }
    }
}