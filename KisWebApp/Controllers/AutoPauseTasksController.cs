using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using KIS.App_Sources;
using KIS.App_Code;
using System.Net;
using System.IO;

namespace KIS.Controllers
{
    public class AutoPauseTasksController : ApiController
    {
        /* Returns:
         * Unauthorized if X-API-KEY is invalid
         * Ok, 1 if process terminated ok
         */
        [System.Web.Http.HttpGet]
        public HttpResponseMessage AutoPauseTasks(String tenant)
        {
            String log = "";
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


            KISConfig cfg = new KISConfig(tenant);
            String xKey = cfg.ConfigController_X_API_KEY;
            if (xKey.Length > 0 && xKey == cKey)
            {
                // Find all departments
                ElencoReparti deptLst = new ElencoReparti(tenant);
                foreach (var dept in deptLst.elenco)
                {
                    log += "Department " + dept.name + "\n";
                    if(dept.AutoPauseTasksOutsideWorkShifts)
                    {
                        log += "AutoPauseTasksOutsideWorkShifts " + dept.AutoPauseTasksOutsideWorkShifts + "\n";
                        dept.loadCalendario(DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2));
                        Boolean outsideShift = false;
                        DateTime actual = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, dept.tzFusoOrario);
                        log += "actual " + actual.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
                        if(dept.CalendarioRep!=null && dept.CalendarioRep.Intervalli!=null && dept.CalendarioRep.Intervalli.Count>0)
                        {
                            log += "dept.CalendarioRep.Intervalli.Count " + dept.CalendarioRep.Intervalli.Count + "\n";
                            if (actual < dept.CalendarioRep.Intervalli[0].Inizio)
                            {
                                log += "First if";
                                outsideShift = true;
                            }
                            else if(actual > dept.CalendarioRep.Intervalli[dept.CalendarioRep.Intervalli.Count - 1].Fine)
                            {
                                log += "Second if";
                                outsideShift = true;
                            }
                            else
                            {
                                log += "else";
                                for(int i = 0; i < dept.CalendarioRep.Intervalli.Count-1 && !outsideShift; i++)
                                {
                                    //if(i < dept.CalendarioRep.Intervalli.Count -1)
                                    {
                                        log += "Current shift: " + dept.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss")
                                            + " - " + dept.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
                                        if (dept.CalendarioRep.Intervalli[i].Fine < actual && actual < dept.CalendarioRep.Intervalli[i+1].Inizio)
                                        {
                                            log += "TRUEEEEEEE\n";
                                            outsideShift = true;
                                        }
                                    }
                                }
                            }
                        }
                        log += "Outsideshift " + outsideShift;

                        if(outsideShift)
                        {
                            log += "Close all tasks";
                            // Auto-pause all tasks
                            var lstOpenTasks = new ElencoTaskProduzione(tenant, dept, 'I');
                            foreach(var openTask in lstOpenTasks.Tasks)
                            {
                                log += "Task " + openTask.Name + " " + openTask.TaskProduzioneID.ToString() + "\n";
                                openTask.loadUtentiAttivi();
                                foreach(var usr in openTask.ActiveInputPoints)
                                {
                                    log += "Active user: " + usr;
                                    InputPoint ip = new InputPoint(tenant, usr);
                                    if(ip != null)
                                    { 
                                        Boolean ret = openTask.Pause(ip);
                                        log += "Paused " + ret.ToString() + "\n";
                                    }
                                }
                            }
                        }
                    }
                }

                // Writes log to file
                string path = @"c:\temp\AutoPauseLog.txt";
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(log);
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(log);
                }

                return Request.CreateResponse(HttpStatusCode.OK, log);
            }
            else
            {

            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}