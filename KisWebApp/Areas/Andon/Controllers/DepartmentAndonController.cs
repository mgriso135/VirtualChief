using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KIS.App_Code;
using KIS.App_Sources;
using Newtonsoft.Json;

namespace KIS.Areas.Andon.Controllers
{
    public class DepartmentAndonController : Controller
    {
        // GET: Andon/DepartmentAndon
        public ActionResult Index(int DepartmentID)
        {
            ViewBag.DepartmentID = -1;
            if (DepartmentID >=0)
                {
                ViewBag.DepartmentID = DepartmentID;
            }
            return View();
        }

        public JsonResult GetConfigurationParameters(int DepartmentID)
        {
            AndonConfigurationStruct aCfgStruct = new AndonConfigurationStruct();
            AndonReparto aRep = new AndonReparto(DepartmentID);
            if(aRep.RepartoID !=-1)
            {
                ViewBag.DepartmentID = aRep.RepartoID;
                aCfgStruct.DepartmentID = DepartmentID;
                aCfgStruct.DepartmentName = aRep.DepartmentName;

                Reparto rp = new Reparto(aRep.RepartoID);
                rp.loadCalendario(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-2), rp.tzFusoOrario), TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(10), rp.tzFusoOrario));
                int curr = -1;
                for (int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
                {
                    if (rp.CalendarioRep.Intervalli[i].Inizio <= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) && TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) <= rp.CalendarioRep.Intervalli[i].Fine)
                    {
                        curr = i;
                    }
                }

                if(curr!=-1)
                {
                    aCfgStruct.StartCurrentShift = TimeZoneInfo.ConvertTimeToUtc(rp.CalendarioRep.Intervalli[curr].Inizio, rp.tzFusoOrario);
                    aCfgStruct.EndCurrentShift = TimeZoneInfo.ConvertTimeToUtc(rp.CalendarioRep.Intervalli[curr].Fine, rp.tzFusoOrario);
                }
                else
                {
                    aCfgStruct.StartCurrentShift = new DateTime(1970, 1, 1);
                    aCfgStruct.EndCurrentShift = new DateTime(1970, 1, 1);
                }

                aRep.loadShowActiveUsers();
                aCfgStruct.ShowActiveUsers = aRep.ShowActiveUsers;
                aRep.loadShowProductionIndicator();
                aCfgStruct.ShowProductivityIndicators = aRep.ShowProductionIndicator;
                aRep.loadScrollType();
                aCfgStruct.ScrollType = aRep.ScrollType;
                aCfgStruct.ContinuousScrollGoSpeed = aRep.ContinuousScrollGoSpeed;
                aCfgStruct.ContinuousScrollBackSpeed = aRep.ContinuousScrollBackSpeed;
                aCfgStruct.UsernameFormat = aRep.UsernameFormat;

                aRep.loadCampiVisualizzati();
                aCfgStruct.ProductViewFields = new List<string>();
                for(int i = 0; i < aRep.CampiVisualizzati.Count; i++)
                {
                    aCfgStruct.ProductViewFields.Add(aRep.CampiVisualizzati.Keys.ElementAt(i));
                }

                aRep.loadCampiVisualizzatiTasks();
                aCfgStruct.TasksViewFields = new List<string>();
                for (int i = 0; i < aRep.CampiVisualizzatiTasks.Count; i++)
                {
                    aCfgStruct.TasksViewFields.Add(aRep.CampiVisualizzatiTasks.Keys.ElementAt(i));
                }

                aCfgStruct.DepartmentTimeZoneOffset = aRep.DepartmentTimezone.BaseUtcOffset.TotalHours;
            }
            return Json(JsonConvert.SerializeObject(aCfgStruct), JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadWIP(int DepartmentID)
        {
            List<KIS.App_Code.DepartmentAndonProductsStruct> wipList = new List<DepartmentAndonProductsStruct>();
            KIS.App_Code.AndonReparto currAndon = new AndonReparto(DepartmentID);
            if(currAndon.RepartoID!=-1)
            {
                currAndon.loadWIP2();
                wipList = currAndon.WIP;

            }
            return Json(JsonConvert.SerializeObject(wipList), JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadUserPanel(int DepartmentID)
        {
            AndonReparto currAndon = new AndonReparto(DepartmentID);
            currAndon.loadUserPanelData();
            return Json(JsonConvert.SerializeObject(currAndon.UserPanel), JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadProductivityIndicators(int DepartmentID, DateTime endShift)
        {
            AndonReparto currAndon = new AndonReparto(DepartmentID);
            ProductivityIndicatorsStruct prdIndStruct = new ProductivityIndicatorsStruct();
            prdIndStruct.OrdersToBeCompletedByTheEndOfTheShift = currAndon.OrdersToBeCompletedByTheEndOfTheShift(endShift);
            return Json(JsonConvert.SerializeObject(prdIndStruct), JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadOpenWarnings(int DepartmentID)
        {
            AndonReparto currAndon = new AndonReparto(DepartmentID);
            currAndon.loadOpenWarnings();
            return Json(JsonConvert.SerializeObject(currAndon.Warnings), JsonRequestBehavior.AllowGet);
        }
    }

    
    public struct AndonConfigurationStruct
    {
        public int DepartmentID;
        public String DepartmentName;
        public double DepartmentTimeZoneOffset;
        public Boolean ShowActiveUsers;
        public Boolean ShowProductivityIndicators;
        public int ScrollType;
        public double ContinuousScrollGoSpeed;
        public double ContinuousScrollBackSpeed;
        public DateTime StartCurrentShift;
        public DateTime EndCurrentShift;
        public char UsernameFormat;

        public List<String> ProductViewFields;
        public List<String> TasksViewFields;
    }

    
}