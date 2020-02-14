using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;

namespace VCProductionEventsExport_SIAV
{
    class Program
    {
        public string log;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting export");
            String res = ExportEvents().Result;

            Console.WriteLine(res.GetType() + "\n" + res);
            System.IO.File.WriteAllText(@"C:\Users\mgris\source\repos\mgriso135\VirtualChief\VC-develop\VCProductionEventsExport-SIAV\bin\Debug\jsonsorted.txt", res.ToString());
            String resproc = processevents(res.ToString());

            Console.WriteLine(resproc);

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 

            
        }

        public static async Task<string> ExportEvents()
        {
            DateTime start1 = DateTime.UtcNow.AddDays(-100);
            DateTime start2 = new DateTime(start1.Year, start1.Month, start1.Day, 0, 0, 0);
            //DateTime end1 = start1.AddDays(1);
            DateTime end1 = DateTime.UtcNow.AddDays(-95);

            String QueryString = "?start=" + start2.ToString("yyyy-MM-dd") + "&end=" + end1.ToString("yyyy-MM-dd");

            HttpClient _httpClient = new HttpClient();
            string sBaseUrl = ConfigurationManager.AppSettings["baseurl"];
            string url = sBaseUrl + "api/EventsExport/ExportFinishedTasksEvents";

            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "TEST");

            // The actual Get method
            using (var result = await _httpClient.GetAsync($"{url}{QueryString}"))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }

        protected static string processevents(String jstr)
        {
            List<TaskEventStruct> filteredds = new List<TaskEventStruct>();
            var origds1 = JsonConvert.DeserializeObject<List<TaskEventStruct>>(jstr);
            var origds = origds1.OrderBy(x => x.TaskID).ThenBy(z=>z.TaskEventUser).ThenBy(y => y.TaskEventTime).ToList();

            String log = origds.Count.ToString()+"\n";
            // Transform events to timespans
            List<WorkingTimeSpan> tsList = new List<WorkingTimeSpan>();
            for(int i = 0; i < origds.Count; i++)
            {
                    log += "1-Evento: " 
                    + origds[i].TaskID+"\t"
                    + origds[i].TaskEventUser+"\t"
                    + origds[i].TaskEventType + "\t" 
                    + origds[i].TaskEventTime.ToString("dd/MM/yyyy HH:mm:ss") 
                    + "\n";
                    DateTime inizio = origds[i].TaskEventTime;
                    String usrI = origds[i].TaskEventUser;
                    Char EventoI = origds[i].TaskEventType;
                int taskI = origds[i].TaskID;
                    if (i < origds.Count - 1)
                    {
                        int taskF = origds[i +1].TaskID;
                        log += "2-Evento: "
                        + origds[i+1].TaskID + "\t"
                        + origds[i+1].TaskEventUser + "\t"
                        + origds[i + 1].TaskEventType + "\t" 
                        + origds[i + 1].TaskEventTime.ToString("dd/MM/yyyy HH:mm:ss") 
                        + "\n";
                        String usrF = origds[i+1].TaskEventUser;
                        Char EventoF = origds[i+1].TaskEventType;
                        DateTime fine = origds[i+1].TaskEventTime;
                        if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF && taskI == taskF)
                        {
                            WorkingTimeSpan curr = new WorkingTimeSpan();
                        curr.EventTypeI = EventoI;
                        curr.EventTypeF = EventoF;
                            curr.user = usrI;
                            curr.Inizio = inizio;
                            curr.Fine = fine;
                            curr.Intervallo = fine - inizio;
                            curr.TaskID = taskI;
                            curr.idPostazione = origds[i].WorkstationID;
                            curr.idReparto = origds[i].DepartmentID;
                            curr.nomePostazione = origds[i].WorkstationName;
                            curr.idProdotto = origds[i].ProductionOrderID;
                            curr.annoProdotto = origds[i].ProductionOrderYear;
                            curr.idReparto = origds[i].DepartmentID;
                            curr.NomeProdotto = origds[i].ProductTypeName + " - " + origds[i].ProductName;
                        curr.RagioneSocialeCliente = origds[i].CustomerName;
                        curr.ProductionOrderDeliveryDate = origds[i].ProductionOrderDeliveryDate;
                        curr.ProductionOrderEndProductionDate = origds[i].ProductionOrderEndProductionDate;
                        curr.TaskPlannedWorkingTime = origds[i].TaskPlannedWorkingTime;
                        curr.TaskPlannedCycleTime = origds[i].TaskPlannedCycleTime;


        tsList.Add(curr);
                            i++;
                        }
                    }
            }

            log += tsList.Count+"\n";
            log += "\n";




            var tslistOrd = tsList.OrderBy(y=>y.TaskID).ThenBy(x => x.Inizio).ToList();
            for (int i = 0; i < tslistOrd.Count; i++)
            {
                log += tslistOrd[i].TaskID + "\t" + tslistOrd[i].user + "\t" + tslistOrd[i].EventTypeI
                    + "\t" + tslistOrd[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + tslistOrd[i].EventTypeF + "\t"
                    + tslistOrd[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
            }



            log += "Newlist:\n";

            for (int i = 0; i < tslistOrd.Count - 1; i++)
            {
                if (tslistOrd[i].TaskID == tslistOrd[i+1].TaskID && tslistOrd[i + 1].Inizio <= tslistOrd[i].Fine)
                {
                    DateTime minstart = tslistOrd[i].Inizio < tslistOrd[i + 1].Inizio ? tslistOrd[i].Inizio : tslistOrd[i + 1].Inizio;
                    DateTime maxend = tslistOrd[i].Fine > tslistOrd[i + 1].Fine ? tslistOrd[i].Fine : tslistOrd[i + 1].Fine;
                    int olderid = tslistOrd[i].Fine > tslistOrd[i + 1].Fine ? i : i + 1;
                    int earlierid = tslistOrd[i].Fine > tslistOrd[i + 1].Fine ? i+1 : i;
                    WorkingTimeSpan curr = new WorkingTimeSpan();
                    curr.Inizio = minstart;
                    curr.Fine = maxend;
                    curr.StartEventID = tslistOrd[earlierid].StartEventID;
                    curr.EndEventID = tslistOrd[olderid].EndEventID;
                    curr.EventTypeI = 'I';
                    curr.EventTypeF = tslistOrd[olderid].EventTypeF;
                    curr.TaskID = tslistOrd[i].TaskID;
                    curr.user = tslistOrd[i].user;
                    curr.RagioneSocialeCliente = tslistOrd[i].RagioneSocialeCliente;
                    curr.ProductionOrderDeliveryDate = tslistOrd[i].ProductionOrderDeliveryDate;
                    curr.ProductionOrderEndProductionDate = tslistOrd[i].ProductionOrderEndProductionDate;
                    curr.TaskPlannedWorkingTime = tslistOrd[i].TaskPlannedWorkingTime;
                    curr.TaskPlannedCycleTime = tslistOrd[i].TaskPlannedCycleTime;
                    tslistOrd.RemoveAt(i + 1);
                    tslistOrd.RemoveAt(i);
                    tslistOrd.Add(curr);
                    tslistOrd = tsList.OrderBy(y => y.TaskID).ThenBy(x => x.Inizio).ToList();
                    i = -1;
                }
            }

            tslistOrd = tsList.OrderBy(y => y.TaskID).ThenBy(x => x.Inizio).ToList();

            for (int i = 0; i < tslistOrd.Count; i++)
            {
                log += tslistOrd[i].TaskID + "\t" + tslistOrd[i].user + "\t" + tslistOrd[i].EventTypeI
                    + "\t" + tslistOrd[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + tslistOrd[i].EventTypeF + "\t"
                    + tslistOrd[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
            }

            // Transform tslistOrd list in events
            List<TaskEventStruct> lstEvents = new List<TaskEventStruct>();
            for(int i = 0; i < tslistOrd.Count;i++)
            {
                TaskEventStruct ei = new TaskEventStruct();
                TaskEventStruct ef = new TaskEventStruct();
                ei.CustomerAddress = "";
                ei.CustomerCity = "";
                ei.CustomerName = tslistOrd[i].RagioneSocialeCliente;
                ei.TaskEventID = tslistOrd[i].StartEventID;
                ei.TaskID = tslistOrd[i].TaskID;
                ei.TaskEventUser = tslistOrd[i].user;
                ei.ProductName = tslistOrd[i].NomeProdotto;
                ei.ProductTypeName = tslistOrd[i].NomeProdotto;
                ei.TaskStatus = tslistOrd[i].TaskStatus;
                ei.ProductionOrderID = tslistOrd[i].idProdotto;
                ei.ProductionOrderYear = tslistOrd[i].annoProdotto;
                ei.TaskName = tslistOrd[i].nomeTask;
                ei.TaskEventTime = tslistOrd[i].Inizio;
                ei.ProductionOrderDeliveryDate = tslistOrd[i].ProductionOrderDeliveryDate;
                ei.ProductionOrderEndProductionDate = tslistOrd[i].ProductionOrderEndProductionDate;
                ei.TaskPlannedWorkingTime = tslistOrd[i].TaskPlannedWorkingTime;
                ei.TaskPlannedCycleTime = tslistOrd[i].TaskPlannedCycleTime;

                if (i>0 && tslistOrd[i-1].TaskID == tslistOrd[i].TaskID && tslistOrd[i-1].user== tslistOrd[i].user)
                {
                    ei.TaskEventType = 'R';
                }
                else
                {
                    ei.TaskEventType = 'I';
                }

                lstEvents.Add(ei);

                ef.CustomerAddress = "";
                ef.CustomerCity = "";
                ef.CustomerName = tslistOrd[i].RagioneSocialeCliente;
                ef.TaskEventID = tslistOrd[i].StartEventID;
                ef.TaskID = tslistOrd[i].TaskID;
                ef.TaskEventUser = tslistOrd[i].user;
                ef.ProductName = tslistOrd[i].NomeProdotto;
                ef.ProductTypeName = tslistOrd[i].NomeProdotto;
                ef.TaskStatus = tslistOrd[i].TaskStatus;
                ef.ProductionOrderID = tslistOrd[i].idProdotto;
                ef.ProductionOrderYear = tslistOrd[i].annoProdotto;
                ef.TaskName = tslistOrd[i].nomeTask;
                ef.TaskEventTime = tslistOrd[i].Inizio;
                ef.ProductionOrderDeliveryDate = tslistOrd[i].ProductionOrderDeliveryDate;
                ef.ProductionOrderEndProductionDate = tslistOrd[i].ProductionOrderEndProductionDate;
                ef.TaskPlannedWorkingTime = tslistOrd[i].TaskPlannedWorkingTime;
                ef.TaskPlannedCycleTime = tslistOrd[i].TaskPlannedCycleTime;

                if (i > 0 && tslistOrd[i - 1].TaskID == tslistOrd[i].TaskID && tslistOrd[i - 1].user == tslistOrd[i].user)
                {
                    ef.TaskEventType = 'R';
                }
                else
                {
                    ef.TaskEventType = 'I';
                }

                lstEvents.Add(ef);
            }

            // Write the new events in a separate database!

            System.IO.File.WriteAllText(@"C:\Users\mgris\source\repos\mgriso135\VirtualChief\VC-develop\VCProductionEventsExport-SIAV\bin\Debug\sortlog.txt", log);
            return log;
        }

        public struct TaskEventStruct
        {
            public String CustomerID;
            public String CustomerName;
            public String CustomerVATNumber;
            public String CustomerCodiceFiscale;
            public String CustomerAddress;
            public String CustomerCity;
            public String CustomerProvince;
            public String CustomerZipCode;
            public String CustomerCountry;
            public String CustomerPhoneNumber;
            public String CustomerEMail;
            public Boolean CustomerKanbanManaged;
            public int SalesOrderID;
            public int SalesOrderYear;
            public String SalesOrderCustomer;
            public DateTime SalesOrderDate;
            public String SalesOrderNotes;
            public int ProductionOrderID;
            public int ProductionOrderYear;
            public int ProductionOrderProductTypeID;
            public int ProductionOrderProductTypeReview;
            public int ProductionOrderProductID;
            public String ProductionOrderSerialNumber;
            public Char ProductionOrderStatus;
            public int ProductionOrderDepartmentID;
            public DateTime ProductionOrderStartTime;
            public DateTime ProductionOrderDeliveryDate;
            public DateTime ProductionOrderEndProductionDate;
            public String ProductionOrderPlanner;
            public int ProductionOrderQuantityOrdered;
            public int ProductionOrderQuantityProduced;
            public String ProductionOrderKanbanCardID;
            public int ProductTypeID;
            public int ProductTypeReview;
            public DateTime ProductTypeReviewDate;
            public String ProductTypeName;
            public String ProductTypeDescription;
            public Boolean ProductTypeEnabled;
            public int ProductID;
            public String ProductName;
            public String ProductDescription;
            public int DepartmentID;
            public String DepartmentName;
            public String DepartmentDescription;
            public Double DepartmentTaktTime;
            public String DepartmentTimeZone;
            public TimeSpan RealWorkingTime;
            public TimeSpan RealDelay;
            public TimeSpan RealLeadTime;
            public DateTime ProductionOrderEndProductionDateReal;
            public int ProductionOrderEndProductionDateRealWeek;
            public int TaskID;
            public String TaskName;
            public String TaskDescription;
            public DateTime TaskEarlyStart;
            public DateTime TaskLateStart;
            public DateTime TaskEarlyFinish;
            public DateTime TaskLateFinish;
            public char TaskStatus;
            public int TaskNumOperators;
            public double TaskQuantityOrdered;
            public double TaskQuantityProduced;
            public TimeSpan TaskPlannedSetupTime;
            public TimeSpan TaskPlannedCycleTime;
            public TimeSpan TaskPlannedUnloadTime;
            public int WorkstationID;
            public String WorkstationName;
            public String WorkstationDescription;
            public DateTime TaskRealEndDate;
            public int TaskRealEndDateWeek;
            public TimeSpan TaskRealLeadTime;
            public TimeSpan TaskRealWorkingTime;
            public TimeSpan TaskRealDelay;
            public int TaskOriginalID;
            public int TaskOriginalRev;
            public int TaskOriginalVar;
            public TimeSpan TaskPlannedWorkingTime;
            public int TaskEventID;
            public String TaskEventUser;
            public DateTime TaskEventTime;
            public char TaskEventType; // I = start, P = pause, F = finish, W = warning
            public String TaskEventNotes;
        }

        public struct WorkingTimeSpan
        {
            public String user;
            public TimeSpan Intervallo;
            public DateTime Inizio;
            public DateTime Fine;
            public int TaskID;
            public String nomeTask;
            public int idPostazione;
            public String nomePostazione;
            public int idProdotto;
            public int annoProdotto;
            public String NomeProdotto;
            public String RagioneSocialeCliente;
            public int idReparto;
            public char EventTypeI;
            public char EventTypeF;
            public DateTime ProductionOrderDeliveryDate;
            public DateTime ProductionOrderEndProductionDate;
            public TimeSpan TaskPlannedWorkingTime;
            public TimeSpan TaskPlannedCycleTime;


            public char EndEventStatus;
            public char ProductStatus;
            public char TaskStatus;
            public int StartEventID;
            public int EndEventID;
            public TimeSpan PlannedWorkingTime;
        }
    }
}
