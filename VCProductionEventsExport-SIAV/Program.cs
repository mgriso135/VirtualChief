using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

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
            //System.IO.File.WriteAllText(@"C:\Users\mgris\source\repos\mgriso135\VirtualChief\VC-develop\VCProductionEventsExport-SIAV\bin\Debug\jsonsorted.txt", res.ToString());

            Console.WriteLine("Calling processevents...\n");
            String resproc = processevents(res.ToString());

            Console.WriteLine(resproc);

            //Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 

            
        }

        public static async Task<string> ExportEvents()
        {
            //DateTime start1 = DateTime.UtcNow.AddDays(-2);
            DateTime start2 = DateTime.UtcNow.AddDays(-2);
            //DateTime end1 = start1.AddDays(1);
            DateTime end1 = DateTime.UtcNow.AddDays(-1);

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
            Console.WriteLine(jstr);
            var origds1 = JsonConvert.DeserializeObject<List<TaskEventStruct>>(jstr);
            var origds = origds1.OrderBy(x => x.TaskID).ThenBy(z=>z.TaskEventUser).ThenBy(y => y.TaskEventTime).ToList();
            String log = "";
            //String log = origds.Count.ToString()+"\n";

            // Transform events to timespans
            Console.WriteLine("Transforming events to timespans...\n");
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
                            curr.StartEventID = origds[i].TaskEventID;
                            curr.EndEventID = origds[i + 1].TaskEventID;
                            curr.RagioneSocialeCliente = origds[i].CustomerName;
                            curr.ProductionOrderDeliveryDate = origds[i].ProductionOrderDeliveryDate;
                            curr.ProductionOrderEndProductionDate = origds[i].ProductionOrderEndProductionDate;
                            curr.TaskPlannedWorkingTime = origds[i].TaskPlannedWorkingTime;
                        curr.TaskPlannedCycleTime = origds[i].TaskPlannedCycleTime;
                        curr.CustomerName = origds[i].CustomerName;
                        curr.CustomerID = origds[i].CustomerID;
                        curr.CustomerVATNumber = origds[i].CustomerVATNumber;
                        curr.CustomerCodiceFiscale = origds[i].CustomerCodiceFiscale;
                        curr.CustomerAddress = origds[i].CustomerAddress;
                        curr.CustomerCity = origds[i].CustomerCity;
                        curr.CustomerProvince = origds[i].CustomerProvince;
                        curr.CustomerZipCode = origds[i].CustomerZipCode;
                        curr.CustomerCountry = origds[i].CustomerCountry;
                        curr.CustomerPhoneNumber = origds[i].CustomerPhoneNumber;
                        curr.CustomerEMail = origds[i].CustomerEMail;
                        curr.CustomerKanbanManaged = origds[i].CustomerKanbanManaged;
                        curr.SalesOrderID = origds[i].SalesOrderID;
                        curr.SalesOrderYear = origds[i].SalesOrderYear;
                        curr.SalesOrderCustomer = origds[i].SalesOrderCustomer;
                        curr.SalesOrderDate = origds[i].SalesOrderDate;
                        curr.SalesOrderNotes = origds[i].SalesOrderNotes;
                        curr.ProductionOrderID = origds[i].ProductionOrderID;
                        curr.ProductionOrderYear = origds[i].ProductionOrderYear;
                        curr.ProductionOrderProductTypeID = origds[i].ProductionOrderProductTypeID;
                        curr.ProductionOrderProductTypeReview = origds[i].ProductionOrderProductTypeReview;
                        curr.ProductionOrderProductID = origds[i].ProductionOrderProductID;
                        curr.ProductionOrderSerialNumber = origds[i].ProductionOrderSerialNumber;
                        curr.ProductionOrderStatus = origds[i].ProductionOrderStatus;
                        curr.ProductionOrderDepartmentID = origds[i].ProductionOrderDepartmentID;
                        curr.ProductionOrderStartTime = origds[i].ProductionOrderStartTime;
                        curr.ProductionOrderDeliveryDate = origds[i].ProductionOrderDeliveryDate;
                        curr.ProductionOrderEndProductionDate = origds[i].ProductionOrderEndProductionDate;
                        curr.ProductionOrderPlanner = origds[i].ProductionOrderPlanner;
                        curr.ProductionOrderQuantityOrdered = origds[i].ProductionOrderQuantityOrdered;
                        curr.ProductionOrderQuantityProduced = origds[i].ProductionOrderQuantityProduced;
                        curr.ProductionOrderKanbanCardID = origds[i].ProductionOrderKanbanCardID;
                        curr.ProductTypeID = origds[i].ProductTypeID;
                        curr.ProductTypeReview = origds[i].ProductTypeReview;
                        curr.ProductTypeReviewDate = origds[i].ProductTypeReviewDate;
                        curr.ProductTypeName = origds[i].ProductTypeName;
                        curr.ProductTypeDescription = origds[i].ProductTypeDescription;
                        curr.ProductTypeEnabled = origds[i].ProductTypeEnabled;
                        curr.ProductID = origds[i].ProductID;
                        curr.ProductName = origds[i].ProductName;
                        curr.ProductDescription = origds[i].ProductDescription;
                        curr.DepartmentID = origds[i].DepartmentID;
                        curr.DepartmentName = origds[i].DepartmentName;
                        curr.DepartmentDescription = origds[i].DepartmentDescription;
                        curr.DepartmentTaktTime = origds[i].DepartmentTaktTime;
                        curr.DepartmentTimeZone = origds[i].DepartmentTimeZone;
                        curr.RealWorkingTime = origds[i].RealWorkingTime;
                        curr.RealDelay = origds[i].RealDelay;
                        curr.RealLeadTime = origds[i].RealLeadTime;
                        curr.ProductionOrderEndProductionDateReal = origds[i].ProductionOrderEndProductionDateReal;
                        curr.ProductionOrderEndProductionDateRealWeek = origds[i].ProductionOrderEndProductionDateRealWeek;
                        curr.TaskID = origds[i].TaskID;
                        curr.TaskName = origds[i].TaskName;
                        curr.TaskDescription = origds[i].TaskDescription;
                        curr.TaskEarlyStart = origds[i].TaskEarlyStart;
                        curr.TaskLateStart = origds[i].TaskLateStart;
                        curr.TaskEarlyFinish = origds[i].TaskEarlyFinish;
                        curr.TaskLateFinish = origds[i].TaskLateFinish;
                        curr.TaskStatus = origds[i].TaskStatus;
                        curr.TaskNumOperators = origds[i].TaskNumOperators;
                        curr.TaskQuantityOrdered = origds[i].TaskQuantityOrdered;
                        curr.TaskQuantityProduced = origds[i].TaskQuantityProduced;
                        curr.TaskPlannedSetupTime = origds[i].TaskPlannedSetupTime;
                        curr.TaskPlannedCycleTime = origds[i].TaskPlannedCycleTime;
                        curr.TaskPlannedUnloadTime = origds[i].TaskPlannedUnloadTime;
                        curr.WorkstationID = origds[i].WorkstationID;
                        curr.WorkstationName = origds[i].WorkstationName;
                        curr.WorkstationDescription = origds[i].WorkstationDescription;
                        curr.TaskRealEndDate = origds[i].TaskRealEndDate;
                        curr.TaskRealEndDateWeek = origds[i].TaskRealEndDateWeek;
                        curr.TaskRealLeadTime = origds[i].TaskRealLeadTime;
                        curr.TaskRealWorkingTime = origds[i].TaskRealWorkingTime;
                        curr.TaskRealDelay = origds[i].TaskRealDelay;
                        curr.TaskOriginalID = origds[i].TaskOriginalID;
                        curr.TaskOriginalRev = origds[i].TaskOriginalRev;
                        curr.TaskOriginalVar = origds[i].TaskOriginalVar;
                        curr.TaskPlannedWorkingTime = origds[i].TaskPlannedWorkingTime;
                        curr.TaskEventID = origds[i].TaskEventID;
                        curr.TaskEventUser = origds[i].TaskEventUser;
                        curr.TaskEventTime = origds[i].TaskEventTime;
                        curr.TaskEventType = origds[i].TaskEventType;
                        curr.TaskEventNotes = origds[i].TaskEventNotes;






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


            Console.WriteLine("Filtering timespans...\n");
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
                    curr.RagioneSocialeCliente = tslistOrd[i].CustomerName;
                    curr.ProductionOrderDeliveryDate = tslistOrd[i].ProductionOrderDeliveryDate;
                    curr.ProductionOrderEndProductionDate = tslistOrd[i].ProductionOrderEndProductionDate;
                    curr.TaskPlannedWorkingTime = tslistOrd[i].TaskPlannedWorkingTime;
                    curr.TaskPlannedCycleTime = tslistOrd[i].TaskPlannedCycleTime;
                    curr.CustomerName = tslistOrd[i].CustomerName;
                    curr.CustomerID = tslistOrd[i].CustomerID;
                    curr.CustomerVATNumber = tslistOrd[i].CustomerVATNumber;
                    curr.CustomerCodiceFiscale = tslistOrd[i].CustomerCodiceFiscale;
                    curr.CustomerAddress = tslistOrd[i].CustomerAddress;
                    curr.CustomerCity = tslistOrd[i].CustomerCity;
                    curr.CustomerProvince = tslistOrd[i].CustomerProvince;
                    curr.CustomerZipCode = tslistOrd[i].CustomerZipCode;
                    curr.CustomerCountry = tslistOrd[i].CustomerCountry;
                    curr.CustomerPhoneNumber = tslistOrd[i].CustomerPhoneNumber;
                    curr.CustomerEMail = tslistOrd[i].CustomerEMail;
                    curr.CustomerKanbanManaged = tslistOrd[i].CustomerKanbanManaged;
                    curr.SalesOrderID = tslistOrd[i].SalesOrderID;
                    curr.SalesOrderYear = tslistOrd[i].SalesOrderYear;
                    curr.SalesOrderCustomer = tslistOrd[i].SalesOrderCustomer;
                    curr.SalesOrderDate = tslistOrd[i].SalesOrderDate;
                    curr.SalesOrderNotes = tslistOrd[i].SalesOrderNotes;
                    curr.ProductionOrderID = tslistOrd[i].ProductionOrderID;
                    curr.ProductionOrderYear = tslistOrd[i].ProductionOrderYear;
                    curr.ProductionOrderProductTypeID = tslistOrd[i].ProductionOrderProductTypeID;
                    curr.ProductionOrderProductTypeReview = tslistOrd[i].ProductionOrderProductTypeReview;
                    curr.ProductionOrderProductID = tslistOrd[i].ProductionOrderProductID;
                    curr.ProductionOrderSerialNumber = tslistOrd[i].ProductionOrderSerialNumber;
                    curr.ProductionOrderStatus = tslistOrd[i].ProductionOrderStatus;
                    curr.ProductionOrderDepartmentID = tslistOrd[i].ProductionOrderDepartmentID;
                    curr.ProductionOrderStartTime = tslistOrd[i].ProductionOrderStartTime;
                    curr.ProductionOrderDeliveryDate = tslistOrd[i].ProductionOrderDeliveryDate;
                    curr.ProductionOrderEndProductionDate = tslistOrd[i].ProductionOrderEndProductionDate;
                    curr.ProductionOrderPlanner = tslistOrd[i].ProductionOrderPlanner;
                    curr.ProductionOrderQuantityOrdered = tslistOrd[i].ProductionOrderQuantityOrdered;
                    curr.ProductionOrderQuantityProduced = tslistOrd[i].ProductionOrderQuantityProduced;
                    curr.ProductionOrderKanbanCardID = tslistOrd[i].ProductionOrderKanbanCardID;
                    curr.ProductTypeID = tslistOrd[i].ProductTypeID;
                    curr.ProductTypeReview = tslistOrd[i].ProductTypeReview;
                    curr.ProductTypeReviewDate = tslistOrd[i].ProductTypeReviewDate;
                    curr.ProductTypeName = tslistOrd[i].ProductTypeName;
                    curr.ProductTypeDescription = tslistOrd[i].ProductTypeDescription;
                    curr.ProductTypeEnabled = tslistOrd[i].ProductTypeEnabled;
                    curr.ProductID = tslistOrd[i].ProductID;
                    curr.ProductName = tslistOrd[i].ProductName;
                    curr.ProductDescription = tslistOrd[i].ProductDescription;
                    curr.DepartmentID = tslistOrd[i].DepartmentID;
                    curr.DepartmentName = tslistOrd[i].DepartmentName;
                    curr.DepartmentDescription = tslistOrd[i].DepartmentDescription;
                    curr.DepartmentTaktTime = tslistOrd[i].DepartmentTaktTime;
                    curr.DepartmentTimeZone = tslistOrd[i].DepartmentTimeZone;
                    curr.RealWorkingTime = tslistOrd[i].RealWorkingTime;
                    curr.RealDelay = tslistOrd[i].RealDelay;
                    curr.RealLeadTime = tslistOrd[i].RealLeadTime;
                    curr.ProductionOrderEndProductionDateReal = tslistOrd[i].ProductionOrderEndProductionDateReal;
                    curr.ProductionOrderEndProductionDateRealWeek = tslistOrd[i].ProductionOrderEndProductionDateRealWeek;
                    curr.TaskID = tslistOrd[i].TaskID;
                    curr.TaskName = tslistOrd[i].TaskName;
                    curr.TaskDescription = tslistOrd[i].TaskDescription;
                    curr.TaskEarlyStart = tslistOrd[i].TaskEarlyStart;
                    curr.TaskLateStart = tslistOrd[i].TaskLateStart;
                    curr.TaskEarlyFinish = tslistOrd[i].TaskEarlyFinish;
                    curr.TaskLateFinish = tslistOrd[i].TaskLateFinish;
                    curr.TaskStatus = tslistOrd[i].TaskStatus;
                    curr.TaskNumOperators = tslistOrd[i].TaskNumOperators;
                    curr.TaskQuantityOrdered = tslistOrd[i].TaskQuantityOrdered;
                    curr.TaskQuantityProduced = tslistOrd[i].TaskQuantityProduced;
                    curr.TaskPlannedSetupTime = tslistOrd[i].TaskPlannedSetupTime;
                    curr.TaskPlannedCycleTime = tslistOrd[i].TaskPlannedCycleTime;
                    curr.TaskPlannedUnloadTime = tslistOrd[i].TaskPlannedUnloadTime;
                    curr.WorkstationID = tslistOrd[i].WorkstationID;
                    curr.WorkstationName = tslistOrd[i].WorkstationName;
                    curr.WorkstationDescription = tslistOrd[i].WorkstationDescription;
                    curr.TaskRealEndDate = tslistOrd[i].TaskRealEndDate;
                    curr.TaskRealEndDateWeek = tslistOrd[i].TaskRealEndDateWeek;
                    curr.TaskRealLeadTime = tslistOrd[i].TaskRealLeadTime;
                    curr.TaskRealWorkingTime = tslistOrd[i].TaskRealWorkingTime;
                    curr.TaskRealDelay = tslistOrd[i].TaskRealDelay;
                    curr.TaskOriginalID = tslistOrd[i].TaskOriginalID;
                    curr.TaskOriginalRev = tslistOrd[i].TaskOriginalRev;
                    curr.TaskOriginalVar = tslistOrd[i].TaskOriginalVar;
                    curr.TaskPlannedWorkingTime = tslistOrd[i].TaskPlannedWorkingTime;
                    curr.TaskEventID = tslistOrd[i].TaskEventID;
                    curr.TaskEventUser = tslistOrd[i].TaskEventUser;
                    curr.TaskEventTime = tslistOrd[i].TaskEventTime;
                    curr.TaskEventType = tslistOrd[i].TaskEventType;
                    curr.TaskEventNotes = tslistOrd[i].TaskEventNotes;

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
                    curr.StartEventID = tslistOrd[i].StartEventID;
                    curr.EndEventID = tslistOrd[i].EndEventID;

                    tslistOrd.RemoveAt(i);
                    tslistOrd.RemoveAt(i);
                    tslistOrd.Add(curr);
                    tslistOrd = tslistOrd.OrderBy(y => y.TaskID).ThenBy(x => x.Inizio).ToList();
                    i--;
                }
            }

            tslistOrd = tsList.OrderBy(y => y.TaskID).ThenBy(x => x.Inizio).ToList();


            String log1 = "";
            for (int i = 0; i < tslistOrd.Count; i++)
            {
                log1 += tslistOrd[i].TaskID + "\t" + tslistOrd[i].user + "\t" + tslistOrd[i].EventTypeI
                    + "\t" + tslistOrd[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + tslistOrd[i].EventTypeF + "\t"
                    + tslistOrd[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
                //Check if task finished
                bool checkiffinished = false;
                for (int c = i; c > 0 && tslistOrd[c].TaskID == tslistOrd[c - 1].TaskID; c--)
                {
                    if(tslistOrd[c-1].EventTypeF=='F')
                    {
                        checkiffinished = true;
                    }
                }

                if(checkiffinished)
                {
                    tslistOrd.RemoveAt(i);
                    i--;
                }
            }
/*            var path = @"C:\users\mgris\desktop\log.txt";
            File.WriteAllText(path, log1);

            String log2 = "";
            for (int i = 0; i < tslistOrd.Count; i++)
            {
                log2 += tslistOrd[i].TaskID + "\t" + tslistOrd[i].user + "\t" + tslistOrd[i].EventTypeI
                    + "\t" + tslistOrd[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + tslistOrd[i].EventTypeF + "\t"
                    + tslistOrd[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
            }
            var path2 = @"C:\users\mgris\desktop\log2.txt";
            File.WriteAllText(path2, log2);*/


            // Transform tslistOrd list in events
            Console.WriteLine("Transforming list in events...\n");
            List<TaskEventStruct> lstEvents = new List<TaskEventStruct>();
            for(int i = 0; i < tslistOrd.Count;i++)
            {
                TaskEventStruct ei = new TaskEventStruct();
                TaskEventStruct ef = new TaskEventStruct();       
                ei.CustomerID = tslistOrd[i].CustomerID;
                ei.CustomerName = tslistOrd[i].RagioneSocialeCliente;
                ei.CustomerVATNumber = tslistOrd[i].CustomerVATNumber;
                ei.CustomerCodiceFiscale = tslistOrd[i].CustomerCodiceFiscale;
                ei.CustomerAddress = tslistOrd[i].CustomerAddress;
                ei.CustomerCity = tslistOrd[i].CustomerCity;
                ei.CustomerProvince = tslistOrd[i].CustomerProvince;
                ei.CustomerZipCode = tslistOrd[i].CustomerZipCode;
                ei.CustomerCountry = tslistOrd[i].CustomerCountry;
                ei.CustomerPhoneNumber = tslistOrd[i].CustomerPhoneNumber;
                ei.CustomerEMail = tslistOrd[i].CustomerEMail;
                ei.CustomerKanbanManaged = tslistOrd[i].CustomerKanbanManaged;
                ei.SalesOrderID = tslistOrd[i].SalesOrderID;
                ei.SalesOrderYear = tslistOrd[i].SalesOrderYear;
                ei.SalesOrderCustomer = tslistOrd[i].SalesOrderCustomer;
                ei.SalesOrderDate = tslistOrd[i].SalesOrderDate;
                ei.SalesOrderNotes = tslistOrd[i].SalesOrderNotes;
                ei.ProductionOrderID = tslistOrd[i].idProdotto;
                ei.ProductionOrderYear = tslistOrd[i].annoProdotto;
                ei.ProductionOrderProductTypeID = tslistOrd[i].ProductionOrderProductTypeID;
                ei.ProductionOrderProductTypeReview = tslistOrd[i].ProductionOrderProductTypeReview;
                ei.ProductionOrderProductID = tslistOrd[i].ProductionOrderProductID;
                ei.ProductionOrderSerialNumber = tslistOrd[i].ProductionOrderSerialNumber;
                ei.ProductionOrderStatus = 'F';
                ei.ProductionOrderDepartmentID = tslistOrd[i].idReparto;
                ei.ProductionOrderStartTime= tslistOrd[i].ProductionOrderStartTime;
                ei.ProductionOrderDeliveryDate = tslistOrd[i].ProductionOrderDeliveryDate;
                ei.ProductionOrderEndProductionDate = tslistOrd[i].ProductionOrderEndProductionDate;
                ei.ProductionOrderPlanner = tslistOrd[i].ProductionOrderPlanner;
                ei.ProductionOrderQuantityOrdered = tslistOrd[i].ProductionOrderQuantityOrdered;
                ei.ProductionOrderQuantityProduced = tslistOrd[i].ProductionOrderQuantityProduced;
                ei.ProductionOrderKanbanCardID = tslistOrd[i].ProductionOrderKanbanCardID;
                ei.ProductTypeID = tslistOrd[i].ProductTypeID;
                ei.ProductTypeReview = tslistOrd[i].ProductTypeReview;
                ei.ProductTypeReviewDate = tslistOrd[i].ProductTypeReviewDate;
                ei.ProductTypeName = tslistOrd[i].NomeProdotto;
                ei.ProductTypeDescription = tslistOrd[i].ProductTypeDescription;
                ei.ProductTypeEnabled = tslistOrd[i].ProductTypeEnabled;
                ei.ProductID = tslistOrd[i].ProductID;
                ei.ProductName = tslistOrd[i].ProductName;
                ei.ProductDescription = tslistOrd[i].ProductDescription;
                ei.DepartmentID= tslistOrd[i].idReparto;
                ei.DepartmentName = tslistOrd[i].DepartmentName;
                ei.DepartmentDescription = tslistOrd[i].DepartmentDescription;
                ei.DepartmentTaktTime = tslistOrd[i].DepartmentTaktTime;
                ei.DepartmentTimeZone = tslistOrd[i].DepartmentTimeZone;
                ei.RealWorkingTime = tslistOrd[i].RealWorkingTime;
                ei.RealDelay = tslistOrd[i].RealDelay;
                ei.RealLeadTime = tslistOrd[i].RealLeadTime;
                ei.ProductionOrderEndProductionDateReal = tslistOrd[i].ProductionOrderEndProductionDateReal;
                ei.ProductionOrderEndProductionDateRealWeek = tslistOrd[i].ProductionOrderEndProductionDateRealWeek;
                ei.TaskID = tslistOrd[i].TaskID;
                ei.TaskName = tslistOrd[i].TaskName;
                ei.TaskDescription = tslistOrd[i].TaskDescription;
                ei.TaskEarlyStart = tslistOrd[i].TaskEarlyStart;
                ei.TaskLateStart = tslistOrd[i].TaskLateStart;
                ei.TaskEarlyFinish = tslistOrd[i].TaskEarlyFinish;
                ei.TaskLateFinish = tslistOrd[i].TaskLateFinish;
                ei.TaskStatus = tslistOrd[i].TaskStatus;
                ei.TaskNumOperators = tslistOrd[i].TaskNumOperators;
                ei.TaskQuantityOrdered = tslistOrd[i].TaskQuantityOrdered;
        ei.TaskQuantityProduced = tslistOrd[i].TaskQuantityProduced;
        ei.TaskPlannedSetupTime = tslistOrd[i].TaskPlannedSetupTime;
        ei.TaskPlannedCycleTime = tslistOrd[i].TaskPlannedCycleTime;
        ei.TaskPlannedUnloadTime = tslistOrd[i].TaskPlannedUnloadTime;
        ei.WorkstationID=tslistOrd[i].idPostazione;
                ei.WorkstationName = tslistOrd[i].nomePostazione;
                ei.WorkstationDescription = tslistOrd[i].WorkstationDescription;
                ei.TaskRealEndDate = tslistOrd[i].TaskRealEndDate;
                ei.TaskRealEndDateWeek = tslistOrd[i].TaskRealEndDateWeek;
                ei.TaskRealLeadTime = tslistOrd[i].TaskRealLeadTime;
                ei.TaskRealWorkingTime = tslistOrd[i].TaskRealWorkingTime;
                ei.TaskRealDelay= tslistOrd[i].TaskRealDelay;
                ei.TaskOriginalID= tslistOrd[i].TaskOriginalID;
                ei.TaskOriginalRev= tslistOrd[i].TaskOriginalRev;
                ei.TaskOriginalVar= tslistOrd[i].TaskOriginalVar;
        ei.TaskPlannedWorkingTime = tslistOrd[i].TaskPlannedWorkingTime;
        ei.TaskEventID = tslistOrd[i].StartEventID;
        ei.TaskEventTime = tslistOrd[i].Inizio;
        ei.TaskEventType = tslistOrd[i].EventTypeI; // I = start, P = pause, F = finish, W = warning
                if (i > 0 && tslistOrd[i - 1].TaskID == tslistOrd[i].TaskID 
                    && tslistOrd[i].EventTypeI == 'I' 
                    //&& tslistOrd[i - 1].EventTypeF == 'P'
                    ) 
                {
                    ei.TaskEventUser = tslistOrd[i - 1].user;
                    ei.TaskEventType = 'R';
                    tslistOrd[i].user = tslistOrd[i - 1].user;
                }
                else
                {
                    ei.TaskEventUser = tslistOrd[i].user;
                    ei.TaskEventType = 'I';
                }
                ei.TaskEventNotes="";

        lstEvents.Add(ei);

                ef.CustomerID = tslistOrd[i].CustomerID;
                ef.CustomerName = tslistOrd[i].RagioneSocialeCliente;
                ef.CustomerVATNumber = tslistOrd[i].CustomerVATNumber;
                ef.CustomerCodiceFiscale = tslistOrd[i].CustomerCodiceFiscale;
                ef.CustomerAddress = tslistOrd[i].CustomerAddress;
                ef.CustomerCity = tslistOrd[i].CustomerCity;
                ef.CustomerProvince = tslistOrd[i].CustomerProvince;
                ef.CustomerZipCode = tslistOrd[i].CustomerZipCode;
                ef.CustomerCountry = tslistOrd[i].CustomerCountry;
                ef.CustomerPhoneNumber = tslistOrd[i].CustomerPhoneNumber;
                ef.CustomerEMail = tslistOrd[i].CustomerEMail;
                ef.CustomerKanbanManaged = tslistOrd[i].CustomerKanbanManaged;
                ef.SalesOrderID = tslistOrd[i].SalesOrderID;
                ef.SalesOrderYear = tslistOrd[i].SalesOrderYear;
                ef.SalesOrderCustomer = tslistOrd[i].SalesOrderCustomer;
                ef.SalesOrderDate = tslistOrd[i].SalesOrderDate;
                ef.SalesOrderNotes = tslistOrd[i].SalesOrderNotes;
                ef.ProductionOrderID = tslistOrd[i].idProdotto;
                ef.ProductionOrderYear = tslistOrd[i].annoProdotto;
                ef.ProductionOrderProductTypeID = tslistOrd[i].ProductionOrderProductTypeID;
                ef.ProductionOrderProductTypeReview = tslistOrd[i].ProductionOrderProductTypeReview;
                ef.ProductionOrderProductID = tslistOrd[i].ProductionOrderProductID;
                ef.ProductionOrderSerialNumber = tslistOrd[i].ProductionOrderSerialNumber;
                ef.ProductionOrderStatus = tslistOrd[i].ProductionOrderStatus;
                ef.ProductionOrderDepartmentID = tslistOrd[i].idReparto;
                ef.ProductionOrderStartTime = new DateTime(1970, 1, 1, 0, 0, 0);
                ef.ProductionOrderDeliveryDate = tslistOrd[i].ProductionOrderDeliveryDate;
                ef.ProductionOrderEndProductionDate = tslistOrd[i].ProductionOrderEndProductionDate;
                ef.ProductionOrderPlanner = tslistOrd[i].ProductionOrderPlanner;
                ef.ProductionOrderQuantityOrdered = tslistOrd[i].ProductionOrderQuantityOrdered;
                ef.ProductionOrderQuantityProduced = tslistOrd[i].ProductionOrderQuantityProduced;
                ef.ProductionOrderKanbanCardID = tslistOrd[i].ProductionOrderKanbanCardID;
                ef.ProductTypeID = tslistOrd[i].ProductTypeID;
                ef.ProductTypeReview = tslistOrd[i].ProductTypeReview;
                ef.ProductTypeReviewDate = tslistOrd[i].ProductTypeReviewDate;
                ef.ProductTypeName = tslistOrd[i].NomeProdotto;
                ef.ProductTypeDescription = tslistOrd[i].ProductTypeDescription;
                ef.ProductTypeEnabled = tslistOrd[i].ProductTypeEnabled;
                ef.ProductID = tslistOrd[i].ProductID;
                ef.ProductName = tslistOrd[i].NomeProdotto;
                ef.ProductDescription = tslistOrd[i].ProductDescription;
                ef.DepartmentID = tslistOrd[i].idReparto;
                ef.DepartmentName = tslistOrd[i].DepartmentName;
                ef.DepartmentDescription = tslistOrd[i].DepartmentDescription;
                ef.DepartmentTaktTime = tslistOrd[i].DepartmentTaktTime;
                ef.DepartmentTimeZone = tslistOrd[i].DepartmentTimeZone;
                ef.RealWorkingTime = tslistOrd[i].RealWorkingTime;
                ef.RealDelay = tslistOrd[i].RealDelay;
                ef.RealLeadTime = tslistOrd[i].RealLeadTime;
                ef.ProductionOrderEndProductionDateReal = tslistOrd[i].ProductionOrderEndProductionDateReal;
                ef.ProductionOrderEndProductionDateRealWeek = tslistOrd[i].ProductionOrderEndProductionDateRealWeek;
                ef.TaskID = tslistOrd[i].TaskID;
                ef.TaskName = tslistOrd[i].TaskName;
                ef.TaskDescription = tslistOrd[i].TaskDescription;
                ef.TaskEarlyStart = tslistOrd[i].TaskEarlyStart;
                ef.TaskLateStart = tslistOrd[i].TaskLateStart;
                ef.TaskEarlyFinish = tslistOrd[i].TaskEarlyFinish;
                ef.TaskLateFinish = tslistOrd[i].TaskLateFinish;
                ef.TaskStatus = tslistOrd[i].TaskStatus;
                ef.TaskNumOperators = tslistOrd[i].TaskNumOperators;
                ef.TaskQuantityOrdered = tslistOrd[i].TaskQuantityOrdered;
                ef.TaskQuantityProduced = tslistOrd[i].TaskQuantityProduced;
                ef.TaskPlannedSetupTime = tslistOrd[i].TaskPlannedSetupTime;
                ef.TaskPlannedCycleTime = tslistOrd[i].TaskPlannedCycleTime;
                ef.TaskPlannedUnloadTime = tslistOrd[i].TaskPlannedUnloadTime;
                ef.WorkstationID = tslistOrd[i].idPostazione;
                ef.WorkstationName = tslistOrd[i].nomePostazione;
                ef.WorkstationDescription = tslistOrd[i].WorkstationDescription;
                ef.TaskRealEndDate = tslistOrd[i].TaskRealEndDate;
                ef.TaskRealEndDateWeek = tslistOrd[i].TaskRealEndDateWeek;
                ef.TaskRealLeadTime = tslistOrd[i].TaskRealLeadTime;
                ef.TaskRealWorkingTime = tslistOrd[i].TaskRealWorkingTime;
                ef.TaskRealDelay = tslistOrd[i].TaskRealDelay;
                ef.TaskOriginalID = tslistOrd[i].TaskOriginalID;
                ef.TaskOriginalRev = tslistOrd[i].TaskOriginalRev;
                ef.TaskOriginalVar = tslistOrd[i].TaskOriginalVar;
                ef.TaskPlannedWorkingTime = tslistOrd[i].TaskPlannedWorkingTime;
                ef.TaskEventID = tslistOrd[i].EndEventID;
                ef.TaskEventUser = tslistOrd[i].user;
                ef.TaskEventTime = tslistOrd[i].Fine;
                ef.TaskEventType = tslistOrd[i].EventTypeF; // I = start, P = pause, F = finish, W = warning; R = resume
                ef.TaskEventNotes = tslistOrd[i].TaskEventNotes;

                lstEvents.Add(ef);
            }

            // Write the new events in a separate database!
            Console.WriteLine("Writing events to db...\n");
            String db = ConfigurationManager.AppSettings["exportdbname"];
            String dbuser = ConfigurationManager.AppSettings["exportdbuser"];
            String dbpass = ConfigurationManager.AppSettings["exportdbpassword"];
            String dbtable = ConfigurationManager.AppSettings["exportdbtable"];
            String sqlConn = "server=localhost; user id=" + dbuser + "; password=" + dbpass + "; database=" + db + ";pooling=true;SslMode=none;";
            MySqlConnection conn = new MySqlConnection(sqlConn);
            conn.Open();
            DateTime exporttimestamp = DateTime.UtcNow;
            foreach(var m in lstEvents)
            {
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {

                    cmd.CommandText = "INSERT INTO " + dbtable 
                        + "(exportdate, CustomerName, CustomerID, CustomerVATNumber, CustomerCodiceFiscale, CustomerAddress, CustomerCity, TaskEventID, "
                        + "CustomerProvince, CustomerZipCode, CustomerCountry, CustomerPhoneNumber, CustomerEMail, CustomerKanbanManaged, "
                        + " SalesOrderID, SalesOrderYear, SalesOrderCustomer, SalesOrderDate, SalesOrderNotes, ProductionOrderID, ProductionOrderYear, "
                        + "ProductionOrderProductTypeID, ProductionOrderProductTypeReview, ProductionOrderProductID, ProductionOrderSerialNumber, "
                        + "ProductionOrderStatus, ProductionOrderDepartmentID, ProductionOrderStartTime, ProductionOrderDeliveryDate, "
                        + " ProductionOrderEndProductionDate, ProductionOrderPlanner, ProductionOrderQuantityOrdered, ProductionOrderQuantityProduced, "
                        + " ProductionOrderKanbanCardID, ProductTypeID, ProductTypeReview, ProductTypeReviewDate, "
                        + "ProductTypeName, ProductTypeDescription, ProductTypeEnabled, ProductID, ProductName, ProductDescription, "
                        + "DepartmentID, DepartmentName, DepartmentDescription, DepartmentTaktTime, DepartmentTimeZone, RealWorkingTime, "
                         + "RealDelay, RealLeadTime, ProductionOrderEndProductionDateReal, ProductionOrderEndProductionDateRealWeek, "
                         + "TaskID, TaskName, TaskDescription, TaskEarlyStart, TaskLateStart, TaskEarlyFinish, TaskLateFinish, "
                         + "TaskStatus, TaskNumOperators, TaskQuantityOrdered, TaskQuantityProduced, TaskPlannedSetupTime, TaskPlannedCycleTime, "
                         + "TaskPlannedUnloadTime, WorkstationID, WorkstationName, WorkstationDescription, TaskRealEndDate, "
                         + "TaskRealEndDateWeek, TaskRealLeadTime, TaskRealWorkingTime, TaskRealDelay, TaskOriginalID, TaskOriginalRev, "
                         + "TaskOriginalVar, TaskPlannedWorkingTime, TaskEventUser, TaskEventTime, TaskEventType, "
                         + "TaskEventNotes"
                        + ") "
                        + "VALUES(@exportdate, @CustomerName, @CustomerID, @CustomerVATNumber, @CustomerCodiceFiscale, @CustomerAddress, @CustomerCity, @TaskEventID, "
                        + "@CustomerProvince, @CustomerZipCode, @CustomerCountry, @CustomerPhoneNumber, @CustomerEMail, @CustomerKanbanManaged, "
                        + " @SalesOrderID, @SalesOrderYear, @SalesOrderCustomer, @SalesOrderDate, @SalesOrderNotes, @ProductionOrderID, @ProductionOrderYear, "
                        + "@ProductionOrderProductTypeID, @ProductionOrderProductTypeReview, @ProductionOrderProductID, @ProductionOrderSerialNumber, "
                        + "@ProductionOrderStatus, @ProductionOrderDepartmentID, @ProductionOrderStartTime, @ProductionOrderDeliveryDate, "
                        + " @ProductionOrderEndProductionDate, @ProductionOrderPlanner, @ProductionOrderQuantityOrdered, @ProductionOrderQuantityProduced, "
                        + " @ProductionOrderKanbanCardID, @ProductTypeID, @ProductTypeReview, @ProductTypeReviewDate, "
                        + "@ProductTypeName, @ProductTypeDescription, @ProductTypeEnabled, @ProductID, @ProductName, @ProductDescription, "
                        + "@DepartmentID, @DepartmentName, @DepartmentDescription, @DepartmentTaktTime, @DepartmentTimeZone, @RealWorkingTime, "
                         + "@RealDelay, @RealLeadTime, @ProductionOrderEndProductionDateReal, @ProductionOrderEndProductionDateRealWeek, "
                         + "@TaskID, @TaskName, @TaskDescription, @TaskEarlyStart, @TaskLateStart, @TaskEarlyFinish, @TaskLateFinish, "
                         + "@TaskStatus, @TaskNumOperators, @TaskQuantityOrdered, @TaskQuantityProduced, @TaskPlannedSetupTime, @TaskPlannedCycleTime, "
                         + "@TaskPlannedUnloadTime, @WorkstationID, @WorkstationName, @WorkstationDescription, @TaskRealEndDate, "
                         + "@TaskRealEndDateWeek, @TaskRealLeadTime, @TaskRealWorkingTime, @TaskRealDelay, @TaskOriginalID, @TaskOriginalRev, "
                         + "@TaskOriginalVar, @TaskPlannedWorkingTime, @TaskEventUser, @TaskEventTime, @TaskEventType, "
                         + "@TaskEventNotes"
                         + ")";

                    cmd.Parameters.AddWithValue("@exportdate", exporttimestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@CustomerName", m.CustomerName);
                    cmd.Parameters.AddWithValue("@CustomerID", m.CustomerID);
                    cmd.Parameters.AddWithValue("@CustomerVATNumber", m.CustomerVATNumber);
                    cmd.Parameters.AddWithValue("@CustomerCodiceFiscale", m.CustomerCodiceFiscale);
                    cmd.Parameters.AddWithValue("@CustomerAddress", m.CustomerAddress);
                    cmd.Parameters.AddWithValue("@CustomerCity", m.CustomerCity);
                    cmd.Parameters.AddWithValue("@TaskEventID", m.TaskEventID);
                    cmd.Parameters.AddWithValue("@CustomerProvince", m.CustomerProvince);
                    cmd.Parameters.AddWithValue("@CustomerZipCode", m.CustomerZipCode);
                    cmd.Parameters.AddWithValue("@CustomerCountry", m.CustomerCountry);
                    cmd.Parameters.AddWithValue("@CustomerPhoneNumber", m.CustomerPhoneNumber);
                    cmd.Parameters.AddWithValue("@CustomerEMail", m.CustomerEMail);
                    cmd.Parameters.AddWithValue("@CustomerKanbanManaged", m.CustomerKanbanManaged);
                    cmd.Parameters.AddWithValue("@SalesOrderID", m.SalesOrderID);
                    cmd.Parameters.AddWithValue("@SalesOrderYear", m.SalesOrderYear);
                    cmd.Parameters.AddWithValue("@SalesOrderCustomer", m.SalesOrderCustomer);
                    cmd.Parameters.AddWithValue("@SalesOrderDate", m.SalesOrderDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@SalesOrderNotes", m.SalesOrderNotes);
                    cmd.Parameters.AddWithValue("@ProductionOrderID", m.ProductionOrderID);
                    cmd.Parameters.AddWithValue("@ProductionOrderYear", m.ProductionOrderYear);
                    cmd.Parameters.AddWithValue("@ProductionOrderProductTypeID", m.ProductionOrderProductTypeID);
                    cmd.Parameters.AddWithValue("@ProductionOrderProductTypeReview", m.ProductionOrderProductTypeReview);
                    cmd.Parameters.AddWithValue("@ProductionOrderProductID", m.ProductionOrderProductID);
                    cmd.Parameters.AddWithValue("@ProductionOrderSerialNumber", m.ProductionOrderSerialNumber);
                    cmd.Parameters.AddWithValue("@ProductionOrderStatus", m.ProductionOrderStatus);
                    cmd.Parameters.AddWithValue("@ProductionOrderDepartmentID", m.ProductionOrderDepartmentID);
                    cmd.Parameters.AddWithValue("@ProductionOrderStartTime", m.ProductionOrderStartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@ProductionOrderDeliveryDate", m.ProductionOrderDeliveryDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@ProductionOrderEndProductionDate", m.ProductionOrderEndProductionDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@ProductionOrderPlanner", m.ProductionOrderPlanner);
                    cmd.Parameters.AddWithValue("@ProductionOrderQuantityOrdered", m.ProductionOrderQuantityOrdered);
                    cmd.Parameters.AddWithValue("@ProductionOrderQuantityProduced", m.ProductionOrderQuantityProduced);
                    cmd.Parameters.AddWithValue("@ProductionOrderKanbanCardID", m.ProductionOrderKanbanCardID);
                    cmd.Parameters.AddWithValue("@ProductTypeID", m.ProductTypeID);
                    cmd.Parameters.AddWithValue("@ProductTypeReview", m.ProductTypeReview);
                    cmd.Parameters.AddWithValue("@ProductTypeReviewDate", m.ProductTypeReviewDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@ProductTypeName", m.ProductTypeName);
                    cmd.Parameters.AddWithValue("@ProductTypeDescription", m.ProductTypeDescription);
                    cmd.Parameters.AddWithValue("@ProductTypeEnabled", m.ProductTypeEnabled);
                    cmd.Parameters.AddWithValue("@ProductID", m.ProductID);
                    cmd.Parameters.AddWithValue("@ProductName", m.ProductName);
                    cmd.Parameters.AddWithValue("@ProductDescription", m.ProductDescription);
                    cmd.Parameters.AddWithValue("@DepartmentID", m.DepartmentID);
                    cmd.Parameters.AddWithValue("@DepartmentName", m.DepartmentName);
                    cmd.Parameters.AddWithValue("@DepartmentDescription", m.DepartmentDescription);
                    cmd.Parameters.AddWithValue("@DepartmentTaktTime", m.DepartmentTaktTime);
                    cmd.Parameters.AddWithValue("@DepartmentTimeZone", m.DepartmentTimeZone);
                    cmd.Parameters.AddWithValue("@RealWorkingTime", Math.Truncate(m.RealWorkingTime.TotalHours).ToString()
                        + ":" + m.RealWorkingTime.Minutes.ToString()
                        + ":" + m.RealWorkingTime.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@RealDelay", Math.Truncate(m.RealDelay.TotalHours).ToString()
                        + ":" + m.RealDelay.Minutes.ToString()
                        + ":" + m.RealDelay.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@RealLeadTime", Math.Truncate(m.RealLeadTime.TotalHours).ToString()
                        + ":" + m.RealLeadTime.Minutes.ToString()
                        + ":" + m.RealLeadTime.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@ProductionOrderEndProductionDateReal", m.ProductionOrderEndProductionDateReal.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@ProductionOrderEndProductionDateRealWeek", 0);
                    cmd.Parameters.AddWithValue("@TaskID", m.TaskID);
                    cmd.Parameters.AddWithValue("@TaskName", m.TaskName);
                    cmd.Parameters.AddWithValue("@TaskDescription", m.TaskDescription);
                    cmd.Parameters.AddWithValue("@TaskEarlyStart", m.TaskEarlyStart.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@TaskLateStart", m.TaskLateStart.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@TaskEarlyFinish", m.TaskEarlyFinish.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@TaskLateFinish", m.TaskLateFinish.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@TaskStatus", m.TaskStatus);
                    cmd.Parameters.AddWithValue("@TaskNumOperators", m.TaskNumOperators);
                    cmd.Parameters.AddWithValue("@TaskQuantityOrdered", m.TaskQuantityOrdered);
                    cmd.Parameters.AddWithValue("@TaskQuantityProduced", m.TaskQuantityProduced);
                    cmd.Parameters.AddWithValue("@TaskPlannedSetupTime", Math.Truncate(m.TaskPlannedSetupTime.TotalHours).ToString()
                        + ":" + m.TaskPlannedSetupTime.Minutes.ToString()
                        + ":" + m.TaskPlannedSetupTime.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@TaskPlannedCycleTime", Math.Truncate(m.TaskPlannedCycleTime.TotalHours).ToString()
                        + ":" + m.TaskPlannedCycleTime.Minutes.ToString()
                        + ":" + m.TaskPlannedCycleTime.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@TaskPlannedUnloadTime", Math.Truncate(m.TaskPlannedUnloadTime.TotalHours).ToString()
                        + ":" + m.TaskPlannedUnloadTime.Minutes.ToString()
                        + ":" + m.TaskPlannedUnloadTime.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@WorkstationID", m.WorkstationID);
                    cmd.Parameters.AddWithValue("@WorkstationName", m.WorkstationName);
                    cmd.Parameters.AddWithValue("@WorkstationDescription", m.WorkstationDescription);
                    cmd.Parameters.AddWithValue("@TaskRealEndDate", m.TaskRealEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@TaskRealEndDateWeek", m.TaskRealEndDateWeek);
                    cmd.Parameters.AddWithValue("@TaskRealLeadTime", Math.Truncate(m.TaskRealLeadTime.TotalHours).ToString()
                        + ":" + m.TaskRealLeadTime.Minutes.ToString()
                        + ":" + m.TaskRealLeadTime.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@TaskRealWorkingTime", Math.Truncate(m.TaskRealWorkingTime.TotalHours).ToString()
                        + ":" + m.TaskRealWorkingTime.Minutes.ToString()
                        + ":" + m.TaskRealWorkingTime.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@TaskRealDelay", Math.Truncate(m.TaskRealDelay.TotalHours).ToString()
                        + ":" + m.TaskRealDelay.Minutes.ToString()
                        + ":" + m.TaskRealDelay.Seconds.ToString());
                    cmd.Parameters.AddWithValue("@TaskOriginalID", m.TaskOriginalID);
                    cmd.Parameters.AddWithValue("@TaskOriginalRev", m.TaskOriginalRev);
                    cmd.Parameters.AddWithValue("@TaskOriginalVar", m.TaskOriginalVar);
                    cmd.Parameters.AddWithValue("@TaskPlannedWorkingTime", Math.Truncate(m.TaskPlannedWorkingTime.TotalHours).ToString() 
                        + ":"+m.TaskPlannedWorkingTime.Minutes.ToString()
                        +":" + m.TaskPlannedWorkingTime.Seconds.ToString());
                    
                    cmd.Parameters.AddWithValue("@TaskEventUser", m.TaskEventUser);
                    cmd.Parameters.AddWithValue("@TaskEventTime", m.TaskEventTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@TaskEventType", m.TaskEventType);
                    cmd.Parameters.AddWithValue("@TaskEventNotes", m.TaskEventNotes);

                    Console.WriteLine("cmdtext: " + cmd.CommandText);

                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                    log = ex.Message;
                }
            }
            conn.Close();

        //    System.IO.File.WriteAllText(@"C:\Users\mgris\source\repos\mgriso135\VirtualChief\VC-develop\VCProductionEventsExport-SIAV\bin\Debug\sortlog.txt", log);
            log += "Done!";
            return log;
        }

        public class TaskEventStruct
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

        public class WorkingTimeSpan
        {
            public String user;
            public TimeSpan Intervallo;
            public DateTime Inizio;
            public DateTime Fine;
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
            public char EndEventStatus;
            public char ProductStatus;
            public int StartEventID;
            public int EndEventID;
            public TimeSpan PlannedWorkingTime;
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
    }
}
