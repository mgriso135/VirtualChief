using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Sources;
using KIS.App_Code;

namespace KIS.Areas.Analysis.Controllers
{
    public class TaskAnalysisController : Controller
    {
        // GET: Analysis/TaskAnalysis
        public ActionResult Index()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/TaskAnalysis/Index", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/TaskAnalysis/Index", "", ipAddr);
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Tasks";
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
                ElencoPostazioni listWst = new ElencoPostazioni();
                ViewBag.Workstations = listWst.elenco;
                ElencoTasks listTasks = new ElencoTasks();
                ViewBag.Tasks = listTasks.Elenco;
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
 * 4 = Sum of delays
 * 5 = Average of Productivity
 */
        public ActionResult TaskDataPanel(DateTime startPeriod, DateTime endPeriod, String customers, String departments, String TypeOfProducts, int periodType, int graphType, String tasks, String workstations)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/TaskAnalysis/TaskDataPanel", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/TaskAnalysis/TaskDataPanel", "", ipAddr);
            }

            ViewBag.startPeriod = startPeriod;
            ViewBag.endPeriod = endPeriod;
            ViewBag.customers = customers;
            ViewBag.departments = departments;
            ViewBag.products = TypeOfProducts;
            ViewBag.graphType = graphType;
            ViewBag.periodType = periodType;
            ViewBag.TaskFilter = "";
            ViewBag.workstations = workstations;
            ViewBag.tasks = tasks;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Tasks";
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
                    History.loadTaskProductionHistory();

                    List<TaskProductionHistoryStruct> curr = History.TaskHistoricData.Where(x => x.TaskRealEndDate > startPeriod && x.TaskRealEndDate < endPeriod.AddDays(1)).ToList();
                    ViewBag.log += History.TaskHistoricData.Count.ToString();
                    var customerFilter = customers.Split(';');

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

                    String[] workstationsFilterStr = workstations.Split(';');
                    List<int> workstationsFilter = new List<int>();
                    foreach (var wkstStr in workstationsFilterStr)
                    {
                        int wkstInt = -1;
                        try
                        {
                            wkstInt = Int32.Parse(wkstStr);
                        }
                        catch (Exception ex)
                        {
                            wkstInt = -1;
                        }
                        if (wkstInt >= 0)
                        {
                            workstationsFilter.Add(wkstInt);
                        }
                    }
                    curr = curr.Where(p => workstationsFilter.Any(l => p.WorkstationID == l)).ToList();

                    String[] productsFilterStr = TypeOfProducts.Split(';');
                    List<int[]> productsFilter = new List<int[]>();
                    foreach (var prdStr in productsFilterStr)
                    {
                        String[] prdStr1 = prdStr.Split('/');
                        int[] prdInt = new int[3];
                        prdInt[0] = -1; prdInt[1] = -1; prdInt[2] = -1;
                        try
                        {
                            prdInt[0] = Int32.Parse(prdStr1[0]);
                            prdInt[1] = Int32.Parse(prdStr1[1]);
                            prdInt[2] = Int32.Parse(prdStr1[2]);
                        }
                        catch (Exception ex)
                        {
                            prdInt[0] = -1; prdInt[1] = -1; prdInt[2] = -1;
                        }
                        if (prdInt[0] >= 0)
                        {
                            productsFilter.Add(prdInt);
                        }
                    }
                    curr = curr.Where(p => productsFilter.Any(l => p.ProductTypeID == l[0] && p.ProductTypeReview == l[1] && p.ProductID == l[2])).ToList();

                    List<TaskProductionAnalysisResultStruct> res = new List<TaskProductionAnalysisResultStruct>();
                        String[] TaskFilterStr1 = tasks.Split(';');
                        List<int> TaskFilter = new List<int>();
                        ViewBag.TaskFilter = "data.addColumn('string', '" + ResProductAnalysis.ProductPanel.lblPeriod + "');";

                        foreach (var tsk in TaskFilterStr1)
                        {
                        ViewBag.log += tsk + " ";
                        int tskID = -1;

                            try
                            {
                            tskID = Int32.Parse(tsk);
                            }
                            catch
                            {
                            tskID = -1;
                            }

                            if (tskID!=-1)
                            {
                            TaskFilter.Add(tskID);
                            processo prcCurr = new processo(tskID);
                                ViewBag.TaskFilter += "data.addColumn('number', '" + prcCurr.processName + "');";                        }
                        }


                    curr = curr.Where(p => TaskFilter.Any(l => p.TaskOriginalID == l)).ToList();


                    if (periodType == 0)
                        {
                        ViewBag.log += "Daily ";
                            // Daily
                            var result = curr.Select(k => new {
                                k.TaskRealEndDate.Year,
                                k.TaskRealEndDate.Month,
                                k.TaskRealEndDateWeek,
                                k.TaskRealEndDate.Day,
                                k.TaskQuantityProduced,
                                k.TaskRealLeadTime,
                                k.TaskRealWorkingTime,
                                k.TaskRealDelay,
                                k.TaskOriginalID,
                                k.TaskName,
                                k.TaskPlannedWorkingTime
                            })
                           .GroupBy(x => new {
                               x.Year,
                               x.Month,
                               x.TaskRealEndDateWeek,
                               x.Day,
                               x.TaskOriginalID,
                               x.TaskName
                           }, (key, group) => new
                           {
                               TaskID = key.TaskOriginalID,
                               key.TaskName,
                               Year = key.Year,
                               Month = key.Month,
                               RealEndDate = DateTime.UtcNow,
                               Week = key.TaskRealEndDateWeek,
                               Day = key.Day,
                               Quantity = group.Sum(k => k.TaskQuantityProduced),
                               WorkingTime = group.Sum(k => k.TaskRealWorkingTime.TotalHours),
                               UnitaryWorkingTime = group.Average(k => (k.TaskRealWorkingTime.TotalHours / k.TaskQuantityProduced)),
                               LeadTime = group.Average(k => k.TaskRealLeadTime.TotalHours),
                               Delay = group.Sum(k => k.TaskRealDelay.TotalHours),
                               Productivity = group.Sum(k=> k.TaskPlannedWorkingTime.TotalSeconds) / group.Sum(k=>k.TaskRealWorkingTime.TotalSeconds),
                           }).ToList();

                            foreach (var k in result)
                            {
                                TaskProductionAnalysisResultStruct currRes = new TaskProductionAnalysisResultStruct();
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
                                currRes.TaskTypeID = k.TaskID;
                                currRes.TaskID = k.TaskID;
                            currRes.TaskName = k.TaskName;
                            currRes.Productivity = k.Productivity * 100;

                                res.Add(currRes);
                            }

                            res = res.OrderBy(z => z.Year).ThenBy(y=>y.Month).ThenBy(x=>x.Day).ToList();

                            ViewBag.TaskFilter += "data.addRows([";
                            DateTime currPeriod = startPeriod;
                            while (currPeriod <= endPeriod)
                            {
                                String qtyCurr = "";
                                var currSet = res.Where(x => x.Day == currPeriod.Day && x.Month == currPeriod.Month && x.Year == currPeriod.Year).ToList();
                            ViewBag.log += "TaskFilter: ";
                                for (int i = 0; i < TaskFilter.Count; i++)
                                {
                                ViewBag.log = TaskFilter[i] + "; ";
                                    try
                                    {
                                        var currItm = currSet.First(x => x.TaskTypeID == TaskFilter[i]);

                                        /*graphType:
                                            * 0 = Quantity - per - period
                                            * 1 = Sum of Working hours per-period
                                            * 2 = Mean of unitary working hours per - period
                                            * 3 = Mean lead time
                                            * 4 = Average delays
                                            * 5 = Average Productivity
                                        */
                                        switch (graphType)
                                        {
                                            case 0: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                            case 1: qtyCurr += currItm.WorkingTime.ToString().Replace(',', '.'); break;
                                            case 2: qtyCurr += (currItm.UnitaryWorkingTime).ToString().Replace(',', '.'); break;
                                            case 3: qtyCurr += (currItm.LeadTime).ToString().Replace(',', '.'); break;
                                            case 4: qtyCurr += (currItm.Delay).ToString().Replace(',', '.'); break;
                                            case 5: qtyCurr += currItm.WorkingTime > 0.0 ? (currItm.Productivity).ToString().Replace(',', '.') : "0.0"; break;
                                        default: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                        }

                                    }
                                    catch
                                    {
                                        qtyCurr += "0";
                                    }
                                    if (i < TaskFilter.Count - 1)
                                    {
                                        qtyCurr += ", ";
                                    }
                                }
                                ViewBag.TaskFilter += "['" + currPeriod.ToString("dd/MM/yyyy") + "', " + qtyCurr + "]";
                                if (currPeriod < endPeriod)
                                {
                                    ViewBag.TaskFilter += ", ";
                                }
                                currPeriod = currPeriod.AddDays(1);
                            }
                            ViewBag.TaskFilter += "]);";
                        }
                        else if (periodType == 1)
                        {
                        ViewBag.log += "Weekly " + curr.Count;
                        // Week
                        var result = curr.Select(k => new {
                                k.TaskRealEndDate.Year,
                                k.TaskRealEndDate.Month,
                                k.TaskRealEndDateWeek,
                                k.TaskQuantityProduced,
                                k.TaskRealLeadTime,
                                k.TaskRealWorkingTime,
                                k.TaskRealDelay,
                                k.TaskOriginalID,
                                k.TaskName,
                                k.TaskPlannedWorkingTime,
                            })
                           .GroupBy(x => new {
                               x.Year,
                               x.TaskRealEndDateWeek,
                               x.TaskOriginalID,
                               x.TaskName
                           }, (key, group) => new
                           {
                               TaskID = key.TaskOriginalID,
                               TaskName = key.TaskName,
                               Year = key.Year,
                               //Month = DateTime.UtcNow.Month,
                               Week = key.TaskRealEndDateWeek,
                               Quantity = group.Sum(k => k.TaskQuantityProduced),
                               WorkingTime = group.Sum(k => k.TaskRealWorkingTime.TotalHours),
                               UnitaryWorkingTime = group.Average(k => (k.TaskRealWorkingTime.TotalHours / k.TaskQuantityProduced)),
                               LeadTime = group.Average(k => k.TaskRealLeadTime.TotalHours),
                               Delay = group.Sum(k => k.TaskRealDelay.TotalHours),
                               Productivity = group.Sum(k => k.TaskPlannedWorkingTime.TotalSeconds) / group.Sum(k => k.TaskRealWorkingTime.TotalSeconds),
                           }).ToList();

                        ViewBag.log += "Group by: " + result.Count;

                            foreach (var k in result)
                            {
                                TaskProductionAnalysisResultStruct currRes = new TaskProductionAnalysisResultStruct();
                                currRes.Delay = k.Delay;
                                currRes.LeadTime = k.LeadTime;
                                currRes.Year = k.Year;
                                currRes.Quantity = k.Quantity;
                                currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                                currRes.Week = k.Week;
                                currRes.WorkingTime = k.WorkingTime;
                            currRes.TaskID = k.TaskID;
                            currRes.TaskTypeID = k.TaskID;
                            currRes.TaskName = k.TaskName;
                            currRes.Productivity = k.Productivity * 100;

                                res.Add(currRes);
                            }
                            res = res.OrderBy(z => z.Year).ThenBy(w => w.Week).ToList();
                        ViewBag.log += "Res Group by: " + result.Count;

                        ViewBag.TaskFilter += "data.addRows([";
                            DateTime currPeriod = startPeriod;
                        ViewBag.log += "graphType: " + graphType;
                            while (currPeriod <= endPeriod)
                            {
                                String qtyCurr = "";
                                var currSet = res.Where(x => x.Week == Dati.Utilities.GetWeekOfTheYear(currPeriod) && x.Year == currPeriod.Year).ToList();
                            ViewBag.log += "TaskFilter.Count " + TaskFilter.Count + " ";
                                for (int i = 0; i < TaskFilter.Count; i++)
                                {
                                    try
                                    {
                                    ViewBag.log += TaskFilter[i] + " ";
                                        var currItm = currSet.First(x => x.TaskID == TaskFilter[i]);
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
                                            case 5: qtyCurr += currItm.WorkingTime > 0.0 ? currItm.Productivity.ToString().Replace(',', '.') : "0.0"; break;
                                        default: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                        }
                                    }
                                    catch
                                    {
                                    ViewBag.log+= "catch! ";
                                        qtyCurr += "0";
                                    }

                                ViewBag.log += "qtyCurr: " + qtyCurr + " ";
                                if (i < TaskFilter.Count - 1)
                                    {
                                        qtyCurr += ", ";
                                    }
                                }
                                ViewBag.TaskFilter += "['" + Dati.Utilities.GetWeekOfTheYear(currPeriod) + "/" + currPeriod.Year.ToString() + "', " + qtyCurr + "]";
                                if (currPeriod.AddDays(7) < endPeriod)
                                {
                                    ViewBag.TaskFilter += ", ";
                                }
                                currPeriod = currPeriod.AddDays(7);
                            }
                            ViewBag.TaskFilter += "]);";
                        ViewBag.log += ViewBag.TaskFilter;
                        }
                        else if (periodType == 2)
                        {
                        ViewBag.log += "Monthly ";
                        // Month
                        var result = curr.Select(k => new {
                                k.TaskRealEndDate.Year,
                                k.TaskRealEndDate.Month,
                                k.TaskQuantityProduced,
                                k.TaskRealLeadTime,
                                k.TaskRealWorkingTime,
                                k.TaskRealDelay,
                                k.TaskOriginalID,
                                k.TaskName,
                            k.TaskPlannedWorkingTime
                        })
                            .GroupBy(x => new {
                                x.Year,
                                x.Month,
                                x.TaskOriginalID,
                                x.TaskName
                            }, (key, group) => new
                            {
                                TaskID = key.TaskOriginalID,
                                TaskName = key.TaskName,
                                Year = key.Year,
                                Month = key.Month,
                                RealEndDate = DateTime.UtcNow,
                                Week = 0,
                                Quantity = group.Sum(k => k.TaskQuantityProduced),
                                WorkingTime = group.Sum(k => k.TaskRealWorkingTime.TotalHours),
                                UnitaryWorkingTime = group.Average(k => (k.TaskRealWorkingTime.TotalHours / k.TaskQuantityProduced)),
                                LeadTime = group.Average(k => k.TaskRealLeadTime.TotalHours),
                                Delay = group.Sum(k => k.TaskRealDelay.TotalHours),
                                Productivity = group.Sum(k=> k.TaskPlannedWorkingTime.TotalSeconds) / group.Sum(k=>k.TaskRealWorkingTime.TotalSeconds),
                            }).ToList();



                            foreach (var k in result)
                            {
                                TaskProductionAnalysisResultStruct currRes = new TaskProductionAnalysisResultStruct();
                            
                                currRes.Delay = k.Delay;
                                currRes.LeadTime = k.LeadTime;
                                currRes.Month = k.Month;
                                currRes.Year = k.Year;
                                currRes.Quantity = k.Quantity;
                                currRes.RealEndDate = k.RealEndDate;
                                currRes.UnitaryWorkingTime = k.UnitaryWorkingTime;
                                currRes.Week = k.Week;
                                currRes.WorkingTime = k.WorkingTime;
                                currRes.TaskID = k.TaskID;
                                currRes.TaskName = k.TaskName;
                                currRes.TaskTypeID = k.TaskID;
                            currRes.TaskName = k.TaskName;
                            currRes.Productivity = k.Productivity * 100;

                            res.Add(currRes);
                            }
                            res = res.OrderBy(z => z.Year).ThenBy(w => w.Month).ToList();

                            ViewBag.TaskFilter += "data.addRows([";
                            DateTime currPeriod = startPeriod;
                            while (currPeriod <= endPeriod)
                            {
                                String qtyCurr = "";
                                var currSet = res.Where(x => x.Month == currPeriod.Month && x.Year == currPeriod.Year).ToList();

                                for (int i = 0; i < TaskFilter.Count; i++)
                                {
                                    try
                                    {
                                        var currItm = currSet.First(x => x.TaskTypeID == TaskFilter[i]);
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
                                            case 5: qtyCurr += currItm.WorkingTime > 0.0 ? currItm.Productivity.ToString().Replace(',', '.') : "0.0"; break;
                                        default: qtyCurr += currItm.Quantity.ToString().Replace(',', '.'); break;
                                        }
                                    }
                                    catch
                                    {
                                        qtyCurr += "0";
                                    }

                                    if (i < TaskFilter.Count - 1)
                                    {
                                        qtyCurr += ", ";
                                    }
                                }
                                ViewBag.TaskFilter += "['" + currPeriod.Month.ToString() + "/" + currPeriod.Year.ToString() + "', " + qtyCurr + "]";
                                DateTime currCmp = new DateTime(currPeriod.Year, currPeriod.Month, 1);
                                DateTime endCmp = new DateTime(endPeriod.Year, endPeriod.Month, 1);
                                if (currCmp < endCmp)
                                {
                                    ViewBag.TaskFilter += ", ";
                                }
                                currPeriod = currPeriod.AddMonths(1);
                            }
                            ViewBag.TaskFilter += "]);";
                        }

                        return View(res);

                    }
            }
            return View();
        }
    }
}