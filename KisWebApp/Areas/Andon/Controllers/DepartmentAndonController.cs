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
                    aCfgStruct.StartCurrentShift = rp.CalendarioRep.Intervalli[curr].Inizio;
                    aCfgStruct.EndCurrentShift = rp.CalendarioRep.Intervalli[curr].Fine;
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
    }

    

    /*  User AndonReparto.loadWIP2();
     *  
     *  QUERY
     *  SELECT 
commesse.idcommesse AS SalesOrderID,
commesse.anno AS SalesOrderYear,
commesse.ExternalID AS OrderExternalID,
anagraficaclienti.codice AS CustomerID, 
anagraficaclienti.ragsociale AS CustomerName,
commesse.dataInserimento AS SalesOrderDate,
commesse.note AS SalesOrderNotes,
variantiprocessi.ExternalID AS ProductExternalID,
productionplan.id AS ProductionOrderID,
productionplan.anno AS ProductionOrderYear,
productionplan.processo AS ProductionOrderProductTypeID,
productionplan.revisione AS ProductionOrderProductTypeReview,
productionplan.variante AS ProductionOrderProductID,
productionplan.matricola AS ProductionOrderSerialNumber,
productionplan.status AS ProductionOrderStatus,
productionplan.reparto AS ProductionOrderDepartmentID,
productionplan.startTime AS ProductionOrderStartTime,
productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,
productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,
productionplan.planner AS ProductionOrderPlanner,
productionplan.quantita AS ProductionOrderQuantityOrdered,
productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,
productionplan.kanbanCard AS ProductionOrderKanbanCardID,
processo.processID AS ProductTypeID,
processo.revisione AS ProductTypeReview,
processo.dataRevisione AS ProductTypeReviewDate,
processo.Name AS ProductTypeName,
processo.description AS ProductTypeDescription,
processo.attivo AS ProductTypeEnabled,
varianti.idvariante AS ProductID,
varianti.nomeVariante AS ProductName,
varianti.descVariante AS ProductDescription,
reparti.idreparto AS DepartmentID,
reparti.nome AS DepartmentName,
reparti.descrizione AS DepartmentDescription,
reparti.cadenza AS DepartmentTaktTime,
reparti.timezone AS DepartmentTimeZone,
 productionplan.LeadTime AS ProductRealLeadTime,
 productionplan.WorkingTime AS ProductRealWorkingTime, 
 productionplan.Delay AS ProductRealDelay,
 productionplan.EndProductionDateReal AS ProductRealEndProductionDate,
tasksproduzione.TaskiD AS TaskID,
tasksproduzione.name AS TaskName,
tasksproduzione.description AS TaskDescription,
tasksproduzione.earlyStart As TaskEarlyStart,
tasksproduzione.lateStart AS TaskLateStart,
tasksproduzione.earlyFinish AS TaskEarlyFinish,
tasksproduzione.lateFinish AS TaskLateFinish,
tasksproduzione.status AS TaskStatus,
tasksproduzione.nOperatori AS TaskNumOperators,
tasksproduzione.qtaPrevista AS TaskQuantityOrdered,
tasksproduzione.qtaProdotta AS TaskQuantityProduced,
tempiciclo.setup AS TaskSetupTimePlanned,
tempiciclo.tempo AS TaskCycleTimePlanned,
tempiciclo.tunload AS TaskUnloadTimePlanned,
postazioni.idpostazioni AS WorkstationID,
postazioni.name AS WorkstationName,
postazioni.description AS WorkstationDescription,
tasksproduzione.endDateReal as TaskEndDateReal,
tasksproduzione.LeadTime AS TaskLeadTime,
tasksproduzione.WorkingTime AS TaskWorkingTime,
 tasksproduzione.Delay AS TaskDelay, 
 tasksproduzione.OrigTask AS TaskOriginalTaskID, 
 tasksproduzione.RevOrigTask AS TaskOriginalTaskRev, 
 tasksproduzione.variante AS TaskOriginalTaskVar 
 FROM anagraficaclienti INNER JOIN commesse ON(anagraficaclienti.codice = commesse.cliente) INNER JOIN
 productionplan ON(commesse.anno = productionplan.annoCommessa AND commesse.idcommesse = productionplan.commessa)
 INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto)
 INNER JOIN variantiprocessi ON(productionplan.variante = variantiprocessi.variante AND productionplan.processo = variantiprocessi.processo AND productionplan.revisione=variantiprocessi.revProc)
 INNER JOIN varianti ON(varianti.idvariante = variantiprocessi.variante)
 INNER JOIN processo ON(processo.ProcessID = variantiprocessi.processo AND processo.revisione = variantiprocessi.revProc)
 INNER JOIN tasksproduzione ON(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)
  inner join processo AS TaskProcess ON(TaskProcess.processID = tasksproduzione.origTask AND TaskProcess.revisione = TasksProduzione.revOrigTask)
 INNER JOIN varianti AS TaskVariant ON(taskvariant.idvariante = tasksproduzione.variante)
 INNER JOIN postazioni ON(postazioni.idpostazioni = tasksproduzione.postazione)
 INNER JOIN tempiciclo ON(tempiciclo.processo = tasksproduzione.origTask AND tempiciclo.revisione= tasksproduzione.revOrigTask AND tasksproduzione.variante = tempiciclo.variante)

  WHERE  productionplan.status<>'F' AND reparti.idreparto=2
  order by productionplan.dataConsegnaPrevista, productionplan.anno, productionplan.id, tasksproduzione.lateStart
     */

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