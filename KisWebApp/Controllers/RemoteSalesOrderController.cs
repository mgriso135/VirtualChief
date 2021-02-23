﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using KIS.App_Code;
using System.Net;
using System.Net.Http;

namespace KIS.Controllers
{
    public class RemoteSalesOrderController : ApiController
    {
        // GET: RemoteSalesOrder
        [HttpGet]
        public HttpResponseMessage RemoteAddSalesOrderHeader(String tenant, String customer, String notes, String externalID)
        {
            KISConfig tpcfg = new KISConfig(tenant);
            if (tpcfg.SalesOrderImportFrom3PartySystem)
            {
                String cKey = "";
                try
                {
                    var arrCKey = Request.Headers.GetValues("X-API-KEY");
                    cKey = arrCKey.FirstOrDefault();
                }
                catch
                {
                    cKey = "";
                }
                CustomersControllerConfig cfg = new CustomersControllerConfig();
                String xKey = cfg.x_api_key;
                if (xKey == cKey && xKey.Length > 0)
                {
                    ElencoCommesse elCom = new ElencoCommesse(tenant);
                    int ret = elCom.Add(customer, notes, externalID);
                    if (ret != -1)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ret);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        /*Returns:
  * 0 if generic error
  * 1 if ok
  * 2 if user is not authorized
  * 3 if product not found
  * 4 if invalid delivery date
  * 5 if sales order not found
  * 6 if error while adding the product to the order
  */
        [HttpGet]
        public HttpResponseMessage AddProductToSalesOrderAndPlan(String tenant, int OrderID, int OrderYear, int ProdID, int ProdRev, int ProdVar, int Quantity, DateTime DeliveryDate, DateTime EndProductionDate)
        {
            int ret = 0;
            KISConfig tpcfg = new KISConfig(tenant);
            if (tpcfg.SalesOrderImportFrom3PartySystem)
            {
                // Register user action
                String cKey = "";
                try
                {
                    var arrCKey = Request.Headers.GetValues("X-API-KEY");
                    cKey = arrCKey.FirstOrDefault();
                }
                catch
                {
                    cKey = "";
                }
                CustomersControllerConfig cfg = new CustomersControllerConfig();
                String xKey = cfg.x_api_key;                

                if (xKey == cKey && xKey.Length > 0)
                {
                    ProcessoVariante prcVar = new ProcessoVariante(tenant, new processo(tenant, ProdID, ProdRev), new variante(tenant, ProdVar));
                    if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.variant.idVariante != -1)
                    {
                        FusoOrario fuso = new FusoOrario(tenant);
                        if (DeliveryDate > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario))
                        {
                            Commessa cm = new Commessa(tenant, OrderID, OrderYear);
                            if (cm != null && cm.ID != -1 && cm.Year > 2000)
                            {
                                int[] rt = cm.AddArticoloInt(prcVar, DeliveryDate, Quantity);
                                if (rt[0] != -1)
                                {
                                    Articolo art = new Articolo(tenant, rt[0], rt[1]);
                                    art.DataPrevistaFineProduzione = EndProductionDate;
                                    ret = this.PlanProduct(tenant, art.ID, art.Year, art.Reparto, EndProductionDate);
                                }
                                else
                                {
                                    ret = 6;
                                }
                            }
                            else
                            {
                                ret = 5;
                            }
                        }
                        else
                        {
                            ret = 4;
                        }
                    }
                    else
                    {
                        ret = 3;
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, ret);
                }
                else
                {
                    ret = 2;
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                ret = 2;
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    

    /*
        * Lancia il processo in produzione
        * Returns:
        * 0 se errore generico
        * 11 se tutto ok
        * 12 se non sono state impostate correttamente EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate
        * 13 se l'articolo è già stato lanciato in produzione
        * 
        * 15 se il tempo a disposizione è < del CriticalPath, quindi va ritardata la consegna
        * 16 se non sono riuscito a dare un EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate a tutti i task.
        * 17 se la data prevista di fine produzione non è reale
        */
    private int PlanProduct(String tenant, int ProductID, int ProductYear, int DepartmentID, DateTime EndProductionDate)
    {
        int ret = 0;
            List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
            Articolo art = new Articolo(tenant, ProductID, ProductYear);
            Reparto rp = new Reparto(tenant, DepartmentID);

            if (art != null && art.ID != -1 && art.Status == 'N' && rp != null && rp.id != -1 && EndProductionDate > DateTime.UtcNow && EndProductionDate <= art.DataPrevistaConsegna)
            {
                art.Reparto = rp.id;
                art.DataPrevistaFineProduzione = EndProductionDate;

                art.Proc.process.loadFigli(art.Proc.variant);
                for (int i = 0; i < art.Proc.process.subProcessi.Count; i++)
                {
                    TaskVariante tskVar = new TaskVariante(tenant, new processo(tenant, art.Proc.process.subProcessi[i].processID, art.Proc.process.subProcessi[i].revisione), art.Proc.variant);
                    tskVar.loadTempiCiclo();
                    TempoCiclo tc = new TempoCiclo(tenant, tskVar.Task.processID, tskVar.Task.revisione, art.Proc.variant.idVariante, tskVar.getDefaultOperatori());
                    if (tc.Tempo != null)
                    {
                        lstTasks.Add(new TaskConfigurato(tenant, tskVar, tc, rp.id, art.Quantita));
                    }
                }

                ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(tenant, art, lstTasks, rp, art.Quantita);
                int rt1 = prcCfg.SimulaIntroduzioneInProduzione();
                if (rt1 == 1)
                {
                    ret = prcCfg.LanciaInProduzione();
                }
                else
                {
                    switch (rt1)
                    {
                        case 2: ret = 15; break;
                        case 3: ret = 16; break;
                        case 4: ret = 17; break;
                        default:
                            break;
                    }

                    ret = rt1;
                }
            }
            else
            {
                ret = 15;
            }
        return ret;
    }
        
    
    }

}