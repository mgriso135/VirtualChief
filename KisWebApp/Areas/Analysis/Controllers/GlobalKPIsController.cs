using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Areas.Analysis.Controllers
{
    public class GlobalKPIsController : Controller
    {
        // GET: Analysis/GlobalKPIs
        public ActionResult GetGlobalKPIs(DateTime startPeriod, DateTime endPeriod, int periodType)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/GlobalKPIsController/GetGlobalKPIs", "startPeriod="+startPeriod.ToString("dd/MM/yyyy")
                    +"&endPeriod="+endPeriod.ToString("dd/MM/yyyy")+"&periodType="+periodType, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/GlobalKPIsController/GetGlobalKPIs", "startPeriod=" + startPeriod.ToString("dd/MM/yyyy")
                    + "&endPeriod=" + endPeriod.ToString("dd/MM/yyyy") + "&periodType=" + periodType, ipAddr);
            }

            if(Session["user"]!=null)
            { 

            ViewBag.Delay = 0;
            ViewBag.PastDelay = 0;
            ViewBag.LeadTime = 0;
            ViewBag.PastLeadTime = 0;
            ViewBag.Quantity = 0;
            ViewBag.PastQuantity = 0;

            ViewBag.startPeriod = startPeriod;
            ViewBag.endPeriod = endPeriod;

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

            ViewBag.log = "Entro in GetGlobalKPI ";

            //if (ViewBag.authR)
            //{
                if (startPeriod < endPeriod)
                {
                    ProductionHistory History = new ProductionHistory(Session["ActiveWorkspace_Name"].ToString());
                    History.loadProductionAnalysis();

                    List<ProductionAnalysisStruct> curr = History.AnalysisData.Where(x => x.ProductionOrderEndProductionDateReal > startPeriod && x.ProductionOrderEndProductionDateReal < endPeriod.AddDays(1)).ToList();
                    ViewBag.log += "History.AnalysisData.Count: " + History.AnalysisData.Count.ToString() + "<br />";

                        List<ProductionAnalysisResultStruct> res = new List<ProductionAnalysisResultStruct>();
                        if (periodType == 0)
                        {
                            // Daily
                            var result = curr.Select(k => new
                            {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderEndProductionDateReal.Day,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.RealWorkingTime,
                                k.RealDelay
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
                               Delay = group.Average(k => k.RealDelay.TotalHours)
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

                                res.Add(currRes);
                            }

                            res = res.OrderBy(x => x.Year).ThenBy(y => y.Month).ThenBy(z => z.Day).ToList();

                        // Load current performances and last week performances
                        try
                        {
                            var currPeriod = result.Find(x => x.Year == DateTime.UtcNow.Year && x.Month == DateTime.UtcNow.Month && x.Day == DateTime.UtcNow.Day);
                            ViewBag.Delay = currPeriod.Delay;
                            ViewBag.LeadTime = currPeriod.LeadTime;
                            ViewBag.Quantity = currPeriod.Quantity;
                        }
                        catch(Exception ex)
                        {
                            ViewBag.Delay = 0;
                            ViewBag.LeadTime = 0;
                            ViewBag.Quantity = 0;
                        }
                        try
                        { 

                            DateTime pastDay = DateTime.UtcNow.AddDays(-1);

                            var lastPeriod = result.Find(x => x.Year == pastDay.Year && x.Month == pastDay.Month && x.Day == pastDay.Day);
                            ViewBag.PastDelay = lastPeriod.Delay;
                            ViewBag.PastLeadTime = lastPeriod.LeadTime;
                            ViewBag.PastQuantity = lastPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.PastDelay = 0;
                            ViewBag.PastLeadTime = 0;
                            ViewBag.PastQuantity = 0;
                        }
                    }
                        else if (periodType == 1)
                        {
                        ViewBag.log += "periodType: Week<br/>";
                            // Week
                            var result = curr.Select(k => new
                            {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderEndProductionDateRealWeek,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.RealWorkingTime,
                                k.RealDelay
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
                               Delay = group.Average(k => k.RealDelay.TotalHours)
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

                                res.Add(currRes);
                            }

                            res = res.OrderBy(x => x.Year).ThenBy(y => y.Week).ToList();

                        // Load current performances and last week performances
                        try
                        {

                            var currPeriod = result.Find(x => x.Year == DateTime.UtcNow.Year && x.Week == Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow));
                            ViewBag.Delay = currPeriod.Delay;
                            ViewBag.LeadTime = currPeriod.LeadTime;
                            ViewBag.Quantity = currPeriod.Quantity;
                        }
                        catch(Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.Delay = 0;
                            ViewBag.LeadTime = 0;
                            ViewBag.Quantity = 0;
                        }

                        try { 
                            DateTime lastWeek = DateTime.UtcNow.AddDays(-7);

                            ViewBag.log += Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) + "/" + DateTime.UtcNow.Year + " " + Dati.Utilities.GetWeekOfTheYear(lastWeek) + lastWeek.Year + "<br />";

                            var lastPeriod = result.Find(x => x.Year == lastWeek.Year && x.Week == Dati.Utilities.GetWeekOfTheYear(lastWeek));
                            ViewBag.PastDelay = lastPeriod.Delay;
                            ViewBag.PastLeadTime = lastPeriod.LeadTime;
                            ViewBag.PastQuantity = lastPeriod.Quantity;
                        }
                        catch(Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.PastDelay = 0;
                            ViewBag.PastLeadTime = 0;
                            ViewBag.PastQuantity = 0;
                        }


                    }
                        else if (periodType == 2)
                        {
                            // Month
                            var result = curr.Select(k => new
                            {
                                k.ProductionOrderEndProductionDateReal.Year,
                                k.ProductionOrderEndProductionDateReal.Month,
                                k.ProductionOrderQuantityProduced,
                                k.RealLeadTime,
                                k.RealWorkingTime,
                                k.RealDelay
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
                                LeadTime = group.Average(k => k.RealLeadTime.TotalHours),
                                Delay = group.Average(k => k.RealDelay.TotalHours)
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

                                res.Add(currRes);
                            }
                            res = res.OrderBy(x => x.Year).ThenBy(y => y.Month).ToList();

                        // Load current performances and last week performances
                        try
                        {
                            var currPeriod = result.Find(x => x.Year == DateTime.UtcNow.Year && x.Month == DateTime.UtcNow.Month);
                            ViewBag.Delay = currPeriod.Delay;
                            ViewBag.LeadTime = currPeriod.LeadTime;
                            ViewBag.Quantity = currPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.Delay = 0;
                            ViewBag.LeadTime = 0;
                            ViewBag.Quantity = 0;
                        }

                        try
                        {
                            DateTime lastWeek = DateTime.UtcNow.AddDays(-30);


                            var lastPeriod = result.Find(x => x.Year == lastWeek.Year && x.Month == lastWeek.Month);
                            ViewBag.PastDelay = lastPeriod.Delay;
                            ViewBag.PastLeadTime = lastPeriod.LeadTime;
                            ViewBag.PastQuantity = lastPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.PastDelay = 0;
                            ViewBag.PastLeadTime = 0;
                            ViewBag.PastQuantity = 0;
                        }


                        return View(res);
                        }
                    
                }
                //}
            }
            return View();
        }

        public ActionResult GetWarningsKPIs()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null && Session["ActiveWorkspace_Name"] != null)
            {
                KIS.App_Sources.UserAccount curr = (KIS.App_Sources.UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.id.ToString(), "Controller", "/Analysis/GlobalKPIsController/GetWarningsKPIs", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/GlobalKPIsController/GetWarningsKPIs", "", ipAddr);
            }

            Warnings warns = new Warnings(Session["ActiveWorkspace_Name"].ToString());


            ViewBag.OpenedLastWeek = 0;
            ViewBag.OpenedThisWeek = 0;
            ViewBag.OpenWarnings = 0;
            ViewBag.ClosedLastWeek = 0;
            ViewBag.OpenedThisWeek = 0;
            ViewBag.ClosedThisWeekLeadTime = new TimeSpan(0, 0, 0);
            ViewBag.ClosedLastWeekLeadTime = new TimeSpan(0, 0, 0);
            ViewBag.ClosedLastWeekLeadTimeDays = 0.0;
            ViewBag.ClosedThisWeekLeadTimeDays = 0.0;

            try
            {
                DateTime lastWeek = DateTime.UtcNow.AddDays(-7);
                var openedLastWeek = warns.List.Where(x => x.DataChiamata.Year == lastWeek.Year && Dati.Utilities.GetWeekOfTheYear(lastWeek) == Dati.Utilities.GetWeekOfTheYear(x.DataChiamata)).ToList();
                var openedThisWeek = warns.List.Where(x => x.DataChiamata.Year == DateTime.UtcNow.Year && Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) == Dati.Utilities.GetWeekOfTheYear(x.DataChiamata)).ToList();
                ViewBag.OpenedLastWeek = openedLastWeek.Count;
                ViewBag.OpenedThisWeek = openedThisWeek.Count;

                var openWarns = warns.List.Where(x => x.isOpen == true).ToList();
                ViewBag.OpenWarnings = openWarns.Count;

                var closedLastWeek = warns.List.Where(x => !x.isOpen && x.DataRisoluzione.Year == lastWeek.Year && Dati.Utilities.GetWeekOfTheYear(lastWeek) == Dati.Utilities.GetWeekOfTheYear(x.DataRisoluzione)).ToList();
                var closedThisWeek = warns.List.Where(x => !x.isOpen && x.DataRisoluzione.Year == DateTime.UtcNow.Year && Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) == Dati.Utilities.GetWeekOfTheYear(x.DataRisoluzione)).ToList();
                ViewBag.ClosedLastWeek = closedLastWeek.Count;
                ViewBag.ClosedThisWeek = closedThisWeek.Count;

                for (int i = 0; i < closedLastWeek.Count; i++)
                {
                    ViewBag.ClosedLastWeekLeadTime = ViewBag.ClosedLastWeekLeadTime.Add(closedLastWeek[i].DataRisoluzione - closedLastWeek[i].DataChiamata);
                }

                if(closedLastWeek.Count>0)
                { 
                ViewBag.ClosedLastWeekLeadTimeDays = ViewBag.ClosedLastWeekLeadTime.TotalDays / closedLastWeek.Count;
                }

                for (int i = 0; i < closedThisWeek.Count; i++)
                {
                    ViewBag.ClosedThisWeekLeadTime = ViewBag.ClosedThisWeekLeadTime.Add(closedThisWeek[i].DataRisoluzione - closedThisWeek[i].DataChiamata);
                }

                if(closedThisWeek.Count>0)
                { 
                ViewBag.ClosedThisWeekLeadTimeDays = ViewBag.ClosedThisWeekLeadTime.TotalDays / closedThisWeek.Count;
                }
            }
            catch
            {

            }
            return View();
        }

        public ActionResult GetNonCompliancesKPIs()
        {
            ViewBag.log = "";
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/GlobalKPIsController/GetNonCompliancesKPIs", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/GlobalKPIsController/GetNonCompliancesKPIs", "", ipAddr);
            }

            NCAnalysis ncAnalysis = new NCAnalysis(Session["ActiveWorkspace_Name"].ToString());
            ncAnalysis.loadNonCompliances();

            ViewBag.OpenedLastWeek = 0;
            ViewBag.OpenedThisWeek = 0;
            ViewBag.OpenWarnings = 0;
            ViewBag.ClosedLastWeek = 0;
            ViewBag.OpenedThisWeek = 0;
            ViewBag.ClosedThisWeekLeadTime = new TimeSpan(0, 0, 0);
            ViewBag.ClosedLastWeekLeadTime = new TimeSpan(0, 0, 0);
            ViewBag.ClosedThisWeekLeadTimeDays = 0.0;
            ViewBag.ClosedLastWeekLeadTimeDays = 0.0;

            try
            {
                DateTime lastWeek = DateTime.UtcNow.AddDays(-7);
                var openedLastWeek = ncAnalysis.ncList.Where(x => x.OpeningDate.Year == lastWeek.Year && Dati.Utilities.GetWeekOfTheYear(lastWeek) == Dati.Utilities.GetWeekOfTheYear(x.OpeningDate)).ToList();
                var openedThisWeek = ncAnalysis.ncList.Where(x => x.OpeningDate.Year == DateTime.UtcNow.Year && Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) == Dati.Utilities.GetWeekOfTheYear(x.OpeningDate)).ToList();
                ViewBag.OpenedLastWeek = openedLastWeek.Count;
                ViewBag.OpenedThisWeek = openedThisWeek.Count;

                var openNcs = ncAnalysis.ncList.Where(x => x.Status == 'O').ToList();
                ViewBag.OpenWarnings = openNcs.Count;

                var closedLastWeek = ncAnalysis.ncList.Where(x => x.Status!='O' && x.ClosureDate.Year == lastWeek.Year && Dati.Utilities.GetWeekOfTheYear(lastWeek) == Dati.Utilities.GetWeekOfTheYear(x.ClosureDate)).ToList();
                var closedThisWeek = ncAnalysis.ncList.Where(x => x.Status != 'O' && x.ClosureDate.Year == DateTime.UtcNow.Year && Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) == Dati.Utilities.GetWeekOfTheYear(x.ClosureDate)).ToList();
                ViewBag.ClosedLastWeek = closedLastWeek.Count;
                ViewBag.ClosedThisWeek = closedThisWeek.Count;


                for (int i = 0; i < closedLastWeek.Count; i++)
                {
                    ViewBag.ClosedLastWeekLeadTime = ViewBag.ClosedLastWeekLeadTime.Add(closedLastWeek[i].ClosureDate - closedLastWeek[i].OpeningDate);
                }

                if(closedLastWeek.Count>0)
                { 
                ViewBag.ClosedLastWeekLeadTimeDays = ViewBag.ClosedLastWeekLeadTime.TotalDays / closedLastWeek.Count;
                }

                for (int i = 0; i < closedThisWeek.Count; i++)
                {
                    ViewBag.log += " " + closedThisWeek[i].ClosureDate.ToString("dd/MM/yyyy HH:mm:ss") + "-" + closedThisWeek[i].OpeningDate.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                    ViewBag.ClosedThisWeekLeadTime = ViewBag.ClosedThisWeekLeadTime.Add(closedThisWeek[i].ClosureDate - closedThisWeek[i].OpeningDate);
                }

                if (closedThisWeek.Count > 0) { 
                ViewBag.ClosedThisWeekLeadTimeDays = ViewBag.ClosedThisWeekLeadTime.TotalDays / closedThisWeek.Count;
                }
            }
            catch(Exception ex)
            {
                ViewBag.log = ex.Message;
            }
            return View();
        }

        public ActionResult GetImprovementActionsKPIs()
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Code.User curr = (KIS.App_Code.User)Session["user"];
                Dati.Utilities.LogAction(curr.username, "Controller", "/Analysis/GlobalKPIsController/GetImprovementActionsKPIs", "", ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/GlobalKPIsController/GetImprovementActionsKPIs", "", ipAddr);
            }

            ImprovementActionAnalysis IAAnalysis = new ImprovementActionAnalysis(Session["ActiveWorkspace_Name"].ToString());
            IAAnalysis.loadIAList();

            ViewBag.OpenedLastWeek = 0;
            ViewBag.OpenedThisWeek = 0;
            ViewBag.OpenWarnings = 0;
            ViewBag.ClosedLastWeek = 0;
            ViewBag.OpenedThisWeek = 0;
            ViewBag.ClosedThisWeekLeadTime = new TimeSpan(0, 0, 0);
            ViewBag.ClosedLastWeekLeadTime = new TimeSpan(0, 0, 0);
            ViewBag.ClosedThisWeekLeadTimeDays = 0.0;
            ViewBag.ClosedLastWeekLeadTimeDays = 0.0;

            try
            {
                DateTime lastWeek = DateTime.UtcNow.AddDays(-7);
                var openedLastWeek = IAAnalysis.IAList.Where(x => x.OpeningDate.Year == lastWeek.Year && Dati.Utilities.GetWeekOfTheYear(lastWeek) == Dati.Utilities.GetWeekOfTheYear(x.OpeningDate)).ToList();
                var openedThisWeek = IAAnalysis.IAList.Where(x => x.OpeningDate.Year == DateTime.UtcNow.Year && Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) == Dati.Utilities.GetWeekOfTheYear(x.OpeningDate)).ToList();
                ViewBag.OpenedLastWeek = openedLastWeek.Count;
                ViewBag.OpenedThisWeek = openedThisWeek.Count;

                var openIAs = IAAnalysis.IAList.Where(x => x.Status == 'O').ToList();
                ViewBag.OpenWarnings = openIAs.Count;

                var closedLastWeek = IAAnalysis.IAList.Where(x => x.Status != 'O' && x.EndDateReal.Year == lastWeek.Year && Dati.Utilities.GetWeekOfTheYear(lastWeek) == Dati.Utilities.GetWeekOfTheYear(x.EndDateReal)).ToList();
                var closedThisWeek = IAAnalysis.IAList.Where(x => x.Status != 'O' && x.EndDateReal.Year == DateTime.UtcNow.Year && Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) == Dati.Utilities.GetWeekOfTheYear(x.EndDateReal)).ToList();
                ViewBag.ClosedLastWeek = closedLastWeek.Count;
                ViewBag.ClosedThisWeek = closedThisWeek.Count;

                for (int i = 0; i < closedLastWeek.Count; i++)
                {
                    ViewBag.ClosedLastWeekLeadTime = ViewBag.ClosedLastWeekLeadTime.Add(closedLastWeek[i].EndDateReal - closedLastWeek[i].OpeningDate);
                }

                if (closedLastWeek.Count > 0)
                {
                    ViewBag.ClosedLastWeekLeadTimeDays = ViewBag.ClosedLastWeekLeadTime.TotalDays / closedLastWeek.Count;
                }

                for (int i = 0; i < closedThisWeek.Count; i++)
                {
                    ViewBag.ClosedThisWeekLeadTime = ViewBag.ClosedThisWeekLeadTime.Add(closedThisWeek[i].EndDateReal - closedThisWeek[i].OpeningDate);
                }

                if (closedThisWeek.Count > 0)
                {
                    ViewBag.ClosedThisWeekLeadTimeDays = ViewBag.ClosedThisWeekLeadTime.TotalDays / closedThisWeek.Count;
                }
            }
            catch
            {

            }
            return View();
        }

        public ActionResult GetGlobalKPIs2(DateTime startPeriod, DateTime endPeriod, int periodType)
        {
            // Register user action
            String ipAddr = Request.UserHostAddress;
            if (Session["user"] != null)
            {
                KIS.App_Sources.UserAccount curr = (KIS.App_Sources.UserAccount)Session["user"];
                Dati.Utilities.LogAction(curr.userId, "Controller", "/Analysis/GlobalKPIsController/GetGlobalKPIs", "startPeriod=" + startPeriod.ToString("dd/MM/yyyy")
                    + "&endPeriod=" + endPeriod.ToString("dd/MM/yyyy") + "&periodType=" + periodType, ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Controller", "/Analysis/GlobalKPIsController/GetGlobalKPIs", "startPeriod=" + startPeriod.ToString("dd/MM/yyyy")
                    + "&endPeriod=" + endPeriod.ToString("dd/MM/yyyy") + "&periodType=" + periodType, ipAddr);
            }

            if (Session["user"] != null)
            {

                ViewBag.Delay = 0;
                ViewBag.PastDelay = 0;
                ViewBag.LeadTime = 0;
                ViewBag.PastLeadTime = 0;
                ViewBag.Quantity = 0;
                ViewBag.PastQuantity = 0;

                ViewBag.startPeriod = startPeriod;
                ViewBag.endPeriod = endPeriod;

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

                ViewBag.log = "Entro in GetGlobalKPI ";

                //if (ViewBag.authR)
                //{
                if (startPeriod < endPeriod)
                {
                    ProductionHistory History = new ProductionHistory(Session["ActiveWorkspace_Name"].ToString());
                    History.loadProductionAnalysis();

                    List<ProductionAnalysisStruct> curr = History.AnalysisData.Where(x => x.ProductionOrderEndProductionDateReal > startPeriod && x.ProductionOrderEndProductionDateReal < endPeriod.AddDays(1)).ToList();
                    ViewBag.log += "History.AnalysisData.Count: " + History.AnalysisData.Count.ToString() + "<br />";

                    List<DepartmentKPIsStruct> res = new List<DepartmentKPIsStruct>();


                    ViewBag.log += "periodType: Week<br/>";
                    // Week
                    var result = curr.Select(k => new
                    {
                        k.DepartmentID,
                        k.DepartmentName,
                        k.ProductionOrderEndProductionDateReal.Year,
                        k.ProductionOrderEndProductionDateReal.Month,
                        k.ProductionOrderEndProductionDateRealWeek,
                        k.ProductionOrderQuantityProduced,
                        k.RealLeadTime,
                        k.PlannedWorkingTime,
                        k.RealWorkingTime,
                        k.RealDelay,
                        k.Productivity
                        //k.PlannedWorkingTime.TotalSeconds / k.RealWorkingTime.TotalSeconds,
                    })
                   .GroupBy(x => new { x.DepartmentID, x.DepartmentName, x.Year, x.ProductionOrderEndProductionDateRealWeek }, (key, group) => new
                   {
                       DepartmentID = key.DepartmentID,
                       DepartmentName = key.DepartmentName,
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
                       Productivity = group.Average(k=>k.Productivity)
                   })
                   .OrderBy(f=>f.DepartmentID).ThenBy(g=>g.Year).ThenBy(h=>h.Week)
                   .ToList();

                    int currentWeek = Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow);
                    int currentYear = DateTime.UtcNow.Year;
                    DateTime lastWeekDT = DateTime.UtcNow.AddDays(-7);
                    int lastWeek = Dati.Utilities.GetWeekOfTheYear(lastWeekDT);
                    int lastYear = lastWeekDT.Year;

                    List<DepartmentKPIsStruct> currentWeekRes = new List<DepartmentKPIsStruct>();
                    try
                    {
                        var currWeekResults = result.Where(x => (x.Week == currentWeek && x.Year == currentYear) || (x.Week==lastWeek && x.Year == lastYear));
                        foreach(var n in currWeekResults)
                        {
                            DepartmentKPIsStruct currwk = new DepartmentKPIsStruct();
                            currwk.Week = n.Week;
                            currwk.Year = n.Year;
                            currwk.Delay = n.Delay;
                            currwk.DepartmentID = n.DepartmentID;
                            currwk.DepartmentName = n.DepartmentName;
                            currwk.LeadTime = n.LeadTime;
                            currwk.Productivity = n.Productivity;
                            currwk.Quantities = n.Quantity;
                            currentWeekRes.Add(currwk);
                        }
                    }
                    catch
                    {
       
                    }

                    // Check data
                    ElencoReparti lstDepts = new ElencoReparti(Session["ActiveWorkspace_Name"].ToString());
                    for(int i = 0; i < lstDepts.elenco.Count; i++)
                    {
                        // Check if exists data for department for current week
                        try
                        {
                            var rescheck = currentWeekRes.First(x => (x.DepartmentID == lstDepts.elenco[i].id && x.Week == currentWeek && x.Year == currentYear));
                        }
                        catch
                        {
                            DepartmentKPIsStruct currwk = new DepartmentKPIsStruct();
                            currwk.Week = currentWeek;
                            currwk.Year = currentYear;
                            currwk.Delay = 0.0;
                            currwk.DepartmentID = lstDepts.elenco[i].id;
                            currwk.DepartmentName = lstDepts.elenco[i].name;
                            currwk.LeadTime = 0.0;
                            currwk.Productivity = 0.0;
                            currwk.Quantities = 0.0;
                            currentWeekRes.Add(currwk);
                        }

                        // Check if exists data for department for past week
                        try
                        {
                            var rescheck = currentWeekRes.First(x => (x.DepartmentID == lstDepts.elenco[i].id && x.Week == lastWeek && x.Year == lastYear));
                        }
                        catch
                        {
                            DepartmentKPIsStruct currwk = new DepartmentKPIsStruct();
                            currwk.Week = lastWeek;
                            currwk.Year = lastYear;
                            currwk.Delay = 0.0;
                            currwk.DepartmentID = lstDepts.elenco[i].id;
                            currwk.DepartmentName = lstDepts.elenco[i].name;
                            currwk.LeadTime = 0.0;
                            currwk.Productivity = 0.0;
                            currwk.Quantities = 0.0;
                            currentWeekRes.Add(currwk);
                        }
                    }

                    currentWeekRes = currentWeekRes.OrderBy(f => f.DepartmentID).ThenBy(g => g.Week).ThenBy(h => h.Year).ToList();

                    return View(currentWeekRes);


                    /*if (periodType == 0)
                    {
                        // Daily
                        var result = curr.Select(k => new
                        {
                            k.ProductionOrderEndProductionDateReal.Year,
                            k.ProductionOrderEndProductionDateReal.Month,
                            k.ProductionOrderEndProductionDateReal.Day,
                            k.ProductionOrderQuantityProduced,
                            k.RealLeadTime,
                            k.RealWorkingTime,
                            k.RealDelay
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
                           Delay = group.Average(k => k.RealDelay.TotalHours)
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

                            res.Add(currRes);
                        }

                        res = res.OrderBy(x => x.Year).ThenBy(y => y.Month).ThenBy(z => z.Day).ToList();

                        // Load current performances and last week performances
                        try
                        {
                            var currPeriod = result.Find(x => x.Year == DateTime.UtcNow.Year && x.Month == DateTime.UtcNow.Month && x.Day == DateTime.UtcNow.Day);
                            ViewBag.Delay = currPeriod.Delay;
                            ViewBag.LeadTime = currPeriod.LeadTime;
                            ViewBag.Quantity = currPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Delay = 0;
                            ViewBag.LeadTime = 0;
                            ViewBag.Quantity = 0;
                        }
                        try
                        {

                            DateTime pastDay = DateTime.UtcNow.AddDays(-1);

                            var lastPeriod = result.Find(x => x.Year == pastDay.Year && x.Month == pastDay.Month && x.Day == pastDay.Day);
                            ViewBag.PastDelay = lastPeriod.Delay;
                            ViewBag.PastLeadTime = lastPeriod.LeadTime;
                            ViewBag.PastQuantity = lastPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.PastDelay = 0;
                            ViewBag.PastLeadTime = 0;
                            ViewBag.PastQuantity = 0;
                        }
                    }
                    else if (periodType == 1)
                    {
                        ViewBag.log += "periodType: Week<br/>";
                        // Week
                        var result = curr.Select(k => new
                        {
                            k.ProductionOrderEndProductionDateReal.Year,
                            k.ProductionOrderEndProductionDateReal.Month,
                            k.ProductionOrderEndProductionDateRealWeek,
                            k.ProductionOrderQuantityProduced,
                            k.RealLeadTime,
                            k.RealWorkingTime,
                            k.RealDelay
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
                           Delay = group.Average(k => k.RealDelay.TotalHours)
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

                            res.Add(currRes);
                        }

                        res = res.OrderBy(x => x.Year).ThenBy(y => y.Week).ToList();

                        // Load current performances and last week performances
                        try
                        {

                            var currPeriod = result.Find(x => x.Year == DateTime.UtcNow.Year && x.Week == Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow));
                            ViewBag.Delay = currPeriod.Delay;
                            ViewBag.LeadTime = currPeriod.LeadTime;
                            ViewBag.Quantity = currPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.Delay = 0;
                            ViewBag.LeadTime = 0;
                            ViewBag.Quantity = 0;
                        }

                        try
                        {
                            DateTime lastWeek = DateTime.UtcNow.AddDays(-7);

                            ViewBag.log += Dati.Utilities.GetWeekOfTheYear(DateTime.UtcNow) + "/" + DateTime.UtcNow.Year + " " + Dati.Utilities.GetWeekOfTheYear(lastWeek) + lastWeek.Year + "<br />";

                            var lastPeriod = result.Find(x => x.Year == lastWeek.Year && x.Week == Dati.Utilities.GetWeekOfTheYear(lastWeek));
                            ViewBag.PastDelay = lastPeriod.Delay;
                            ViewBag.PastLeadTime = lastPeriod.LeadTime;
                            ViewBag.PastQuantity = lastPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.PastDelay = 0;
                            ViewBag.PastLeadTime = 0;
                            ViewBag.PastQuantity = 0;
                        }


                    }
                    else if (periodType == 2)
                    {
                        // Month
                        var result = curr.Select(k => new
                        {
                            k.ProductionOrderEndProductionDateReal.Year,
                            k.ProductionOrderEndProductionDateReal.Month,
                            k.ProductionOrderQuantityProduced,
                            k.RealLeadTime,
                            k.RealWorkingTime,
                            k.RealDelay
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
                            LeadTime = group.Average(k => k.RealLeadTime.TotalHours),
                            Delay = group.Average(k => k.RealDelay.TotalHours)
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

                            res.Add(currRes);
                        }
                        res = res.OrderBy(x => x.Year).ThenBy(y => y.Month).ToList();

                        // Load current performances and last week performances
                        try
                        {
                            var currPeriod = result.Find(x => x.Year == DateTime.UtcNow.Year && x.Month == DateTime.UtcNow.Month);
                            ViewBag.Delay = currPeriod.Delay;
                            ViewBag.LeadTime = currPeriod.LeadTime;
                            ViewBag.Quantity = currPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.Delay = 0;
                            ViewBag.LeadTime = 0;
                            ViewBag.Quantity = 0;
                        }

                        try
                        {
                            DateTime lastWeek = DateTime.UtcNow.AddDays(-30);


                            var lastPeriod = result.Find(x => x.Year == lastWeek.Year && x.Month == lastWeek.Month);
                            ViewBag.PastDelay = lastPeriod.Delay;
                            ViewBag.PastLeadTime = lastPeriod.LeadTime;
                            ViewBag.PastQuantity = lastPeriod.Quantity;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.log += ex.Message;
                            ViewBag.PastDelay = 0;
                            ViewBag.PastLeadTime = 0;
                            ViewBag.PastQuantity = 0;
                        }


                        return View(res);
                    }
                    */
                }
                //}
            }
            return View();
        }
    }
}
