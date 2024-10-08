﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using KIS.App_Code;
using KIS.App_Sources;


/* 20200211
 * This controller has been created to export events data and make available them to SIAV software
 * 
 */

namespace KIS.Controllers
{
    public class EventsExportController : ApiController
    {

        /* This methods exports only finished tasks data, where task real end date is between start and end */
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage ExportFinishedTasksEvents(String tenant, DateTime start, DateTime end)
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
            EventsExportControllerConfig cfg = new EventsExportControllerConfig();
            String xKey = cfg.x_api_key;
            if (xKey.Length > 0 && xKey == cKey)
            {
                // use TaskEventStruct in Analysis.cs
                TaskEvents tskevs = new TaskEvents(tenant);
                tskevs.loadTaskEvents(start, end);
                return Request.CreateResponse(HttpStatusCode.OK, tskevs.TaskEventsData);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage TransformEventsToTimeSpansAndExport(String tenant, Boolean AllEvents)
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
            EventsExportControllerConfig cfg = new EventsExportControllerConfig();
            String xKey = cfg.x_api_key;
            if (xKey.Length > 0 && xKey == cKey)
            {
                // use TaskEventStruct in Analysis.cs
                TaskEvents tskevs = new TaskEvents(tenant);
                tskevs.ExportTimeSpans(AllEvents);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

    }
}