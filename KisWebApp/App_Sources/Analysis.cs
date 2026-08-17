using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;

namespace KIS.App_Sources
{
    public class Analysis
    {
        protected String Tenant;
    }


    // Production Analysis
    public class ProductionHistory
    {
        public String Tenant;

        public List<ProductionHistoryStruct> HistoricData;

        private class ProductionHistoryRow
        {
            public String CustomerID { get; set; }
            public String CustomerName { get; set; }
            public String CustomerVATNumber { get; set; }
            public String CustomerCodiceFiscale { get; set; }
            public String CustomerAddress { get; set; }
            public String CustomerCity { get; set; }
            public String CustomerProvince { get; set; }
            public String CustomerZipCode { get; set; }
            public String CustomerCountry { get; set; }
            public String CustomerPhoneNumber { get; set; }
            public String CustomerEMail { get; set; }
            public Boolean? CustomerKanbanManaged { get; set; }
            public int? SalesOrderID { get; set; }
            public int? SalesOrderYear { get; set; }
            public String SalesOrderCustomer { get; set; }
            public DateTime? SalesOrderDate { get; set; }
            public String SalesOrderNotes { get; set; }
            public int? ProductionOrderID { get; set; }
            public int? ProductionOrderYear { get; set; }
            public int? ProductionOrderProductTypeID { get; set; }
            public int? ProductionOrderProductTypeReview { get; set; }
            public int? ProductionOrderProductID { get; set; }
            public String ProductionOrderSerialNumber { get; set; }
            public String ProductionOrderStatus { get; set; }
            public int? ProductionOrderDepartmentID { get; set; }
            public DateTime? ProductionOrderStartTime { get; set; }
            public DateTime? ProductionOrderDeliveryDate { get; set; }
            public DateTime? ProductionOrderEndProductionDate { get; set; }
            public String ProductionOrderPlanner { get; set; }
            public int? ProductionOrderQuantityOrdered { get; set; }
            public int? ProductionOrderQuantityProduced { get; set; }
            public String ProductionOrderKanbanCardID { get; set; }
            public int? ProductTypeID { get; set; }
            public int? ProductTypeReview { get; set; }
            public DateTime? ProductTypeReviewDate { get; set; }
            public String ProductTypeName { get; set; }
            public String ProductTypeDescription { get; set; }
            public Boolean? ProductTypeEnabled { get; set; }
            public int? ProductID { get; set; }
            public String ProductName { get; set; }
            public String ProductDescription { get; set; }
            public int DepartmentID { get; set; }
            public String DepartmentName { get; set; }
            public String DepartmentDescription { get; set; }
            public Double? DepartmentTaktTime { get; set; }
            public String DepartmentTimeZone { get; set; }
            public TimeSpan? RealLeadTime { get; set; }
            public TimeSpan? RealWorkingTime { get; set; }
            public TimeSpan? RealDelay { get; set; }
            public DateTime? RealEndProductionDate { get; set; }
            public String SalesOrderExternalID { get; set; }
            public String ProductExternalID { get; set; }
            public String MeasurementUnit { get; set; }
        }

        public ProductionHistory(String Tenant)
        {
            this.Tenant = Tenant;
        }

        public void loadProductionHistory()
        {
            this.HistoricData = new List<ProductionHistoryStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(Tenant);
            conn.Open();
            string sql = "SELECT anagraficaclienti.codice AS CustomerID, "
                + " anagraficaclienti.ragsociale AS CustomerName,"
                + " anagraficaclienti.partitaiva AS CustomerVATNumber,"
                + " anagraficaclienti.codfiscale AS CustomerCodiceFiscale,"
+ " anagraficaclienti.indirizzo AS CustomerAddress,"
+ " anagraficaclienti.citta AS CustomerCity,"
+ " anagraficaclienti.provincia AS CustomerProvince,"
+ " anagraficaclienti.CAP AS CustomerZipCode,"
+ " anagraficaclienti.stato AS CustomerCountry,"
+ " anagraficaclienti.telefono AS CustomerPhoneNumber,"
+ " anagraficaclienti.email AS CustomerEMail,"
+ " anagraficaclienti.kanbanmanaged AS CustomerKanbanManaged,"
+ " commesse.idcommesse AS SalesOrderID,"
+ " commesse.anno AS SalesOrderYear,"
+ " commesse.cliente AS SalesOrderCustomer,"
+ " commesse.dataInserimento AS SalesOrderDate,"
+ " commesse.note AS SalesOrderNotes,"
+ " productionplan.id AS ProductionOrderID,"
+ " productionplan.anno AS ProductionOrderYear,"
+ " productionplan.processo AS ProductionOrderProductTypeID,"
+ " productionplan.revisione AS ProductionOrderProductTypeReview,"
+ " productionplan.variante AS ProductionOrderProductID,"
+ " productionplan.matricola AS ProductionOrderSerialNumber,"
+ " productionplan.status AS ProductionOrderStatus,"
+ " productionplan.reparto AS ProductionOrderDepartmentID,"
+ " productionplan.startTime AS ProductionOrderStartTime,"
+ " productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,"
+ " productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,"
+ " productionplan.planner AS ProductionOrderPlanner,"
+ " productionplan.quantita AS ProductionOrderQuantityOrdered,"
+ " productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,"
+ " productionplan.kanbanCard AS ProductionOrderKanbanCardID,"
+ " processo.processID AS ProductTypeID,"
+ " processo.revisione AS ProductTypeReview,"
+ " processo.dataRevisione AS ProductTypeReviewDate,"
+ " processo.Name AS ProductTypeName,"
+ " processo.description AS ProductTypeDescription,"
+ " processo.attivo AS ProductTypeEnabled,"
+ " varianti.idvariante AS ProductID,"
+ " varianti.nomeVariante AS ProductName,"
+ " varianti.descVariante AS ProductDescription,"
+ " reparti.idreparto AS DepartmentID,"
+ " reparti.nome AS DepartmentName,"
+ " reparti.descrizione AS DepartmentDescription,"
+ " reparti.cadenza AS DepartmentTaktTime,"
+ " reparti.timezone AS DepartmentTimeZone, "
+ " productionplan.leadtime AS RealLeadTime, "
+ " productionplan.WorkingTime AS RealWorkingTime, "
+ " productionplan.Delay AS RealDelay, "
+ " productionplan.EndProductionDateReal AS RealEndProductionDate, "
+ " commesse.ExternalID AS SalesOrderExternalID, "
+ " variantiprocessi.ExternalID AS ProductExternalID, "
 + " measurementunits.type AS MeasurementUnit"
+ " FROM anagraficaclienti INNER JOIN commesse ON (anagraficaclienti.codice = commesse.cliente) INNER JOIN"
 + " productionplan ON(commesse.anno =productionplan.annoCommessa AND commesse.idcommesse = productionplan.commessa)"
 + " INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto)"
 + "INNER JOIN variantiprocessi ON (productionplan.variante = variantiprocessi.variante AND productionplan.processo = variantiprocessi.processo AND productionplan.revisione = variantiprocessi.revProc)"
 + " INNER JOIN varianti ON (varianti.idvariante = variantiprocessi.variante)"
 + " INNER JOIN processo ON (processo.ProcessID = variantiprocessi.processo AND processo.revisione = variantiprocessi.revProc) "
+  " INNER JOIN measurementunits ON(variantiprocessi.measurementUnit = measurementunits.id)"
 + " WHERE productionplan.status = 'F'"
 + " order by productionplan.anno DESC, productionplan.id DESC";
            var rows = conn.Query<ProductionHistoryRow>(sql);
            foreach (var row in rows)
            {
                ProductionHistoryStruct curr = new ProductionHistoryStruct();
                curr.DepartmentID = row.DepartmentID;
                KIS.App_Code.Reparto rp = new App_Code.Reparto(this.Tenant, curr.DepartmentID);
                if (row.CustomerID != null)
                {
                    curr.CustomerID = row.CustomerID;
                }
                if (row.CustomerName != null)
                {
                    curr.CustomerName = row.CustomerName;
                }
                if (row.CustomerVATNumber != null)
                {
                    curr.CustomerVATNumber = row.CustomerVATNumber;
                }
                if (row.CustomerCodiceFiscale != null)
                {
                    curr.CustomerCodiceFiscale = row.CustomerCodiceFiscale;
                }
                if (row.CustomerAddress != null)
                {
                    curr.CustomerAddress = row.CustomerAddress;
                }
                if (row.CustomerCity != null)
                {
                    curr.CustomerCity = row.CustomerCity;
                }
                if (row.CustomerProvince != null)
                {
                    curr.CustomerProvince = row.CustomerProvince;
                }
                if (row.CustomerZipCode != null)
                {
                    curr.CustomerZipCode = row.CustomerZipCode;
                }
                if (row.CustomerCountry != null)
                {
                    curr.CustomerCountry = row.CustomerCountry;
                }
                if (row.CustomerPhoneNumber != null)
                {
                    curr.CustomerPhoneNumber = row.CustomerPhoneNumber;
                }
                if (row.CustomerEMail != null)
                {
                    curr.CustomerEMail = row.CustomerEMail;
                }
                if (row.CustomerKanbanManaged.HasValue)
                {
                    curr.CustomerKanbanManaged = row.CustomerKanbanManaged.Value;
                }
                if (row.SalesOrderID.HasValue)
                {
                    curr.SalesOrderID = row.SalesOrderID.Value;
                }
                if (row.SalesOrderYear.HasValue)
                {
                    curr.SalesOrderYear = row.SalesOrderYear.Value;
                }
                if (row.SalesOrderCustomer != null)
                {
                    curr.SalesOrderCustomer = row.SalesOrderCustomer;
                }
                if (row.SalesOrderDate.HasValue)
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(row.SalesOrderDate.Value, rp.tzFusoOrario);
                }
                if (row.SalesOrderNotes != null)
                {
                    curr.SalesOrderNotes = row.SalesOrderNotes;
                }
                if (row.ProductionOrderID.HasValue)
                {
                    curr.ProductionOrderID = row.ProductionOrderID.Value;
                }
                if (row.ProductionOrderYear.HasValue)
                {
                    curr.ProductionOrderYear = row.ProductionOrderYear.Value;
                }
                if (row.ProductionOrderProductTypeID.HasValue)
                {
                    curr.ProductionOrderProductTypeID = row.ProductionOrderProductTypeID.Value;
                }
                if (row.ProductionOrderProductTypeReview.HasValue)
                {
                    curr.ProductionOrderProductTypeReview = row.ProductionOrderProductTypeReview.Value;
                }
                if (row.ProductionOrderProductID.HasValue)
                {
                    curr.ProductionOrderProductID = row.ProductionOrderProductID.Value;
                }
                if (row.ProductionOrderSerialNumber != null)
                {
                    curr.ProductionOrderSerialNumber = row.ProductionOrderSerialNumber;
                }
                if (row.ProductionOrderStatus != null)
                {
                    curr.ProductionOrderStatus = row.ProductionOrderStatus[0];
                }
                if (row.ProductionOrderDepartmentID.HasValue)
                {
                    curr.ProductionOrderDepartmentID = row.ProductionOrderDepartmentID.Value;
                }
                if (row.ProductionOrderStartTime.HasValue)
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderStartTime.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderDeliveryDate.HasValue)
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderDeliveryDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderEndProductionDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderPlanner != null)
                {
                    curr.ProductionOrderPlanner = row.ProductionOrderPlanner;
                }
                if (row.ProductionOrderQuantityOrdered.HasValue)
                {
                    curr.ProductionOrderQuantityOrdered = row.ProductionOrderQuantityOrdered.Value;
                }
                if (row.ProductionOrderQuantityProduced.HasValue)
                {
                    curr.ProductionOrderQuantityProduced = row.ProductionOrderQuantityProduced.Value;
                }
                if (row.ProductionOrderKanbanCardID != null)
                {
                    curr.ProductionOrderKanbanCardID = row.ProductionOrderKanbanCardID;
                }
                if (row.ProductTypeID.HasValue)
                {
                    curr.ProductTypeID = row.ProductTypeID.Value;
                }
                if (row.ProductTypeReview.HasValue)
                {
                    curr.ProductTypeReview = row.ProductTypeReview.Value;
                }
                if (row.ProductTypeReviewDate.HasValue)
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductTypeReviewDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductTypeName != null)
                {
                    curr.ProductTypeName = row.ProductTypeName;
                }
                if (row.ProductTypeDescription != null)
                {
                    curr.ProductTypeDescription = row.ProductTypeDescription;
                }
                if (row.ProductTypeEnabled.HasValue)
                {
                    curr.ProductTypeEnabled = row.ProductTypeEnabled.Value;
                }
                if (row.ProductID.HasValue)
                {
                    curr.ProductID = row.ProductID.Value;
                }
                if (row.ProductName != null)
                {
                    curr.ProductName = row.ProductName;
                }
                if (row.ProductDescription != null)
                {
                    curr.ProductDescription = row.ProductDescription;
                }
                if (row.DepartmentName != null)
                {
                    curr.DepartmentName = row.DepartmentName;
                }
                if (row.DepartmentDescription != null)
                {
                    curr.DepartmentDescription = row.DepartmentDescription;
                }
                if (row.DepartmentTaktTime.HasValue)
                {
                    curr.DepartmentTaktTime = row.DepartmentTaktTime.Value;
                }
                if (row.DepartmentTimeZone != null)
                {
                    curr.DepartmentTimeZone = row.DepartmentTimeZone;
                }
                if (row.RealLeadTime.HasValue)
                {
                    curr.RealLeadTime = row.RealLeadTime.Value;
                }
                if (row.RealWorkingTime.HasValue)
                {
                    curr.RealWorkingTime = row.RealWorkingTime.Value;
                }
                if (row.RealDelay.HasValue)
                {
                    curr.RealDelay = row.RealDelay.Value;
                }
                if (row.RealEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDateReal = row.RealEndProductionDate.Value;
                }
                if (row.SalesOrderExternalID != null)
                {
                    curr.SalesOrderExternalID = row.SalesOrderExternalID;
                }
                if(row.ProductExternalID != null)
                {
                    curr.ProductExternalID = row.ProductExternalID;
                }
                if (row.MeasurementUnit != null)
                {
                    curr.MeasurementUnit = row.MeasurementUnit;
                }
                this.HistoricData.Add(curr);
            }
            conn.Close();
        }

        public List<ProductionAnalysisStruct> AnalysisData;

        public int GetWeekOfTheYear(DateTime time)
        {
            {
                // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
                // be the same week# as whatever Thursday, Friday or Saturday are,
                // and we always get those right
                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                {
                    time = time.AddDays(3);
                }

                // Return the week of our adjusted day
                return CultureInfo
                    .InvariantCulture
                    .Calendar
                    .GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
        }

        private class ProductionAnalysisRow
        {
            public String CustomerID { get; set; }
            public String CustomerName { get; set; }
            public String CustomerVATNumber { get; set; }
            public String CustomerCodiceFiscale { get; set; }
            public String CustomerAddress { get; set; }
            public String CustomerCity { get; set; }
            public String CustomerProvince { get; set; }
            public String CustomerZipCode { get; set; }
            public String CustomerCountry { get; set; }
            public String CustomerPhoneNumber { get; set; }
            public String CustomerEMail { get; set; }
            public Boolean? CustomerKanbanManaged { get; set; }
            public int? SalesOrderID { get; set; }
            public int? SalesOrderYear { get; set; }
            public String SalesOrderCustomer { get; set; }
            public DateTime? SalesOrderDate { get; set; }
            public String SalesOrderNotes { get; set; }
            public int? ProductionOrderID { get; set; }
            public int? ProductionOrderYear { get; set; }
            public int? ProductionOrderProductTypeID { get; set; }
            public int? ProductionOrderProductTypeReview { get; set; }
            public int? ProductionOrderProductID { get; set; }
            public String ProductionOrderSerialNumber { get; set; }
            public String ProductionOrderStatus { get; set; }
            public int? ProductionOrderDepartmentID { get; set; }
            public DateTime? ProductionOrderStartTime { get; set; }
            public DateTime? ProductionOrderDeliveryDate { get; set; }
            public DateTime? ProductionOrderEndProductionDate { get; set; }
            public String ProductionOrderPlanner { get; set; }
            public int? ProductionOrderQuantityOrdered { get; set; }
            public int? ProductionOrderQuantityProduced { get; set; }
            public String ProductionOrderKanbanCardID { get; set; }
            public int? ProductTypeID { get; set; }
            public int? ProductTypeReview { get; set; }
            public DateTime? ProductTypeReviewDate { get; set; }
            public String ProductTypeName { get; set; }
            public String ProductTypeDescription { get; set; }
            public Boolean? ProductTypeEnabled { get; set; }
            public int? ProductID { get; set; }
            public String ProductName { get; set; }
            public String ProductDescription { get; set; }
            public int DepartmentID { get; set; }
            public String DepartmentName { get; set; }
            public String DepartmentDescription { get; set; }
            public Double? DepartmentTaktTime { get; set; }
            public String DepartmentTimeZone { get; set; }
            public TimeSpan? RealLeadTime { get; set; }
            public TimeSpan? RealWorkingTime { get; set; }
            public TimeSpan? RealDelay { get; set; }
            public DateTime? RealEndProductionDate { get; set; }
            public String SalesOrderExternalID { get; set; }
            public TimeSpan? PlannedWorkingTime { get; set; }
        }

        public void loadProductionAnalysis()
        {
            this.AnalysisData = new List<ProductionAnalysisStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT anagraficaclienti.codice AS CustomerID, "
                + " anagraficaclienti.ragsociale AS CustomerName,"
                + " anagraficaclienti.partitaiva AS CustomerVATNumber,"
                + " anagraficaclienti.codfiscale AS CustomerCodiceFiscale,"
+ " anagraficaclienti.indirizzo AS CustomerAddress,"
+ " anagraficaclienti.citta AS CustomerCity,"
+ " anagraficaclienti.provincia AS CustomerProvince,"
+ " anagraficaclienti.CAP AS CustomerZipCode,"
+ " anagraficaclienti.stato AS CustomerCountry,"
+ " anagraficaclienti.telefono AS CustomerPhoneNumber,"
+ " anagraficaclienti.email AS CustomerEMail,"
+ " anagraficaclienti.kanbanmanaged AS CustomerKanbanManaged,"
+ " commesse.idcommesse AS SalesOrderID,"
+ " commesse.anno AS SalesOrderYear,"
+ " commesse.cliente AS SalesOrderCustomer,"
+ " commesse.dataInserimento AS SalesOrderDate,"
+ " commesse.note AS SalesOrderNotes,"
+ " productionplan.id AS ProductionOrderID,"
+ " productionplan.anno AS ProductionOrderYear,"
+ " productionplan.processo AS ProductionOrderProductTypeID,"
+ " productionplan.revisione AS ProductionOrderProductTypeReview,"
+ " productionplan.variante AS ProductionOrderProductID,"
+ " productionplan.matricola AS ProductionOrderSerialNumber,"
+ " productionplan.status AS ProductionOrderStatus,"
+ " productionplan.reparto AS ProductionOrderDepartmentID,"
+ " productionplan.startTime AS ProductionOrderStartTime,"
+ " productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,"
+ " productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,"
+ " productionplan.planner AS ProductionOrderPlanner,"
+ " productionplan.quantita AS ProductionOrderQuantityOrdered,"
+ " productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,"
+ " productionplan.kanbanCard AS ProductionOrderKanbanCardID,"
+ " processo.processID AS ProductTypeID,"
+ " processo.revisione AS ProductTypeReview,"
+ " processo.dataRevisione AS ProductTypeReviewDate,"
+ " processo.Name AS ProductTypeName,"
+ " processo.description AS ProductTypeDescription,"
+ " processo.attivo AS ProductTypeEnabled,"
+ " varianti.idvariante AS ProductID,"
+ " varianti.nomeVariante AS ProductName,"
+ " varianti.descVariante AS ProductDescription,"
+ " reparti.idreparto AS DepartmentID,"
+ " reparti.nome AS DepartmentName,"
+ " reparti.descrizione AS DepartmentDescription,"
+ " reparti.cadenza AS DepartmentTaktTime,"
+ " reparti.timezone AS DepartmentTimeZone, "
+ " productionplan.leadtime AS RealLeadTime, "
+ " productionplan.WorkingTime AS RealWorkingTime, "
+ " productionplan.Delay AS RealDelay, "
+ " productionplan.EndProductionDateReal AS RealEndProductionDate, "
+ " commesse.ExternalID AS SalesOrderExternalID, "
+ " productionplan.WorkingTimePlanned AS PlannedWorkingTime "
+ " FROM anagraficaclienti INNER JOIN commesse ON (anagraficaclienti.codice = commesse.cliente) INNER JOIN"
 + " productionplan ON(commesse.anno =productionplan.annoCommessa AND commesse.idcommesse = productionplan.commessa)"
 + " INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto)"
 + " INNER JOIN varianti ON (varianti.idvariante = productionplan.variante)"
 + " INNER JOIN processo ON (processo.ProcessID = productionplan.processo AND processo.revisione = productionplan.revisione)"
 + " WHERE productionplan.status = 'F'"
 + " order by productionplan.anno DESC, productionplan.id DESC";
            var rows = conn.Query<ProductionAnalysisRow>(sql);
            foreach (var row in rows)
            {
                ProductionAnalysisStruct curr = new ProductionAnalysisStruct();
                curr.DepartmentID = row.DepartmentID;
                KIS.App_Code.Reparto rp = new App_Code.Reparto(this.Tenant, curr.DepartmentID);
                if (row.CustomerID != null)
                {
                    curr.CustomerID = row.CustomerID;
                }
                if (row.CustomerName != null)
                {
                    curr.CustomerName = row.CustomerName;
                }
                if (row.CustomerVATNumber != null)
                {
                    curr.CustomerVATNumber = row.CustomerVATNumber;
                }
                if (row.CustomerCodiceFiscale != null)
                {
                    curr.CustomerCodiceFiscale = row.CustomerCodiceFiscale;
                }
                if (row.CustomerAddress != null)
                {
                    curr.CustomerAddress = row.CustomerAddress;
                }
                if (row.CustomerCity != null)
                {
                    curr.CustomerCity = row.CustomerCity;
                }
                if (row.CustomerProvince != null)
                {
                    curr.CustomerProvince = row.CustomerProvince;
                }
                if (row.CustomerZipCode != null)
                {
                    curr.CustomerZipCode = row.CustomerZipCode;
                }
                if (row.CustomerCountry != null)
                {
                    curr.CustomerCountry = row.CustomerCountry;
                }
                if (row.CustomerPhoneNumber != null)
                {
                    curr.CustomerPhoneNumber = row.CustomerPhoneNumber;
                }
                if (row.CustomerEMail != null)
                {
                    curr.CustomerEMail = row.CustomerEMail;
                }
                if (row.CustomerKanbanManaged.HasValue)
                {
                    curr.CustomerKanbanManaged = row.CustomerKanbanManaged.Value;
                }
                if (row.SalesOrderID.HasValue)
                {
                    curr.SalesOrderID = row.SalesOrderID.Value;
                }
                if (row.SalesOrderYear.HasValue)
                {
                    curr.SalesOrderYear = row.SalesOrderYear.Value;
                }
                if (row.SalesOrderCustomer != null)
                {
                    curr.SalesOrderCustomer = row.SalesOrderCustomer;
                }
                if (row.SalesOrderDate.HasValue)
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(row.SalesOrderDate.Value, rp.tzFusoOrario);
                }
                if (row.SalesOrderNotes != null)
                {
                    curr.SalesOrderNotes = row.SalesOrderNotes;
                }
                if (row.ProductionOrderID.HasValue)
                {
                    curr.ProductionOrderID = row.ProductionOrderID.Value;
                }
                if (row.ProductionOrderYear.HasValue)
                {
                    curr.ProductionOrderYear = row.ProductionOrderYear.Value;
                }
                if (row.ProductionOrderProductTypeID.HasValue)
                {
                    curr.ProductionOrderProductTypeID = row.ProductionOrderProductTypeID.Value;
                }
                if (row.ProductionOrderProductTypeReview.HasValue)
                {
                    curr.ProductionOrderProductTypeReview = row.ProductionOrderProductTypeReview.Value;
                }
                if (row.ProductionOrderProductID.HasValue)
                {
                    curr.ProductionOrderProductID = row.ProductionOrderProductID.Value;
                }
                if (row.ProductionOrderSerialNumber != null)
                {
                    curr.ProductionOrderSerialNumber = row.ProductionOrderSerialNumber;
                }
                if (row.ProductionOrderStatus != null)
                {
                    curr.ProductionOrderStatus = row.ProductionOrderStatus[0];
                }
                if (row.ProductionOrderDepartmentID.HasValue)
                {
                    curr.ProductionOrderDepartmentID = row.ProductionOrderDepartmentID.Value;
                }
                if (row.ProductionOrderStartTime.HasValue)
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderStartTime.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderDeliveryDate.HasValue)
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderDeliveryDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderEndProductionDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderPlanner != null)
                {
                    curr.ProductionOrderPlanner = row.ProductionOrderPlanner;
                }
                if (row.ProductionOrderQuantityOrdered.HasValue)
                {
                    curr.ProductionOrderQuantityOrdered = row.ProductionOrderQuantityOrdered.Value;
                }
                if (row.ProductionOrderQuantityProduced.HasValue)
                {
                    curr.ProductionOrderQuantityProduced = row.ProductionOrderQuantityProduced.Value;
                }
                if (row.ProductionOrderKanbanCardID != null)
                {
                    curr.ProductionOrderKanbanCardID = row.ProductionOrderKanbanCardID;
                }
                if (row.ProductTypeID.HasValue)
                {
                    curr.ProductTypeID = row.ProductTypeID.Value;
                }
                if (row.ProductTypeReview.HasValue)
                {
                    curr.ProductTypeReview = row.ProductTypeReview.Value;
                }
                if (row.ProductTypeReviewDate.HasValue)
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductTypeReviewDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductTypeName != null)
                {
                    curr.ProductTypeName = row.ProductTypeName;
                }
                if (row.ProductTypeDescription != null)
                {
                    curr.ProductTypeDescription = row.ProductTypeDescription;
                }
                if (row.ProductTypeEnabled.HasValue)
                {
                    curr.ProductTypeEnabled = row.ProductTypeEnabled.Value;
                }
                if (row.ProductID.HasValue)
                {
                    curr.ProductID = row.ProductID.Value;
                }
                if (row.ProductName != null)
                {
                    curr.ProductName = row.ProductName;
                }
                if (row.ProductDescription != null)
                {
                    curr.ProductDescription = row.ProductDescription;
                }
                if (row.DepartmentName != null)
                {
                    curr.DepartmentName = row.DepartmentName;
                }
                if (row.DepartmentDescription != null)
                {
                    curr.DepartmentDescription = row.DepartmentDescription;
                }
                if (row.DepartmentTaktTime.HasValue)
                {
                    curr.DepartmentTaktTime = row.DepartmentTaktTime.Value;
                }
                if (row.DepartmentTimeZone != null)
                {
                    curr.DepartmentTimeZone = row.DepartmentTimeZone;
                }
                if (row.RealLeadTime.HasValue)
                {
                    curr.RealLeadTime = row.RealLeadTime.Value;
                }
                if (row.RealWorkingTime.HasValue)
                {
                    curr.RealWorkingTime = row.RealWorkingTime.Value;
                }
                if (row.RealDelay.HasValue)
                {
                    curr.RealDelay = row.RealDelay.Value;
                }
                if (row.RealEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDateReal = row.RealEndProductionDate.Value;
                    curr.ProductionOrderEndProductionDateRealWeek = GetWeekOfTheYear(row.RealEndProductionDate.Value);
                }
                if (row.SalesOrderExternalID != null)
                {
                    curr.SalesOrderExternalID = row.SalesOrderExternalID;
                }
                if(row.PlannedWorkingTime.HasValue)
                {
                    curr.PlannedWorkingTime = row.PlannedWorkingTime.Value;
                }

                if(curr.ProductionOrderStatus == 'F' && curr.RealWorkingTime.TotalHours > 0.75)
                {
                    curr.Productivity = curr.PlannedWorkingTime.TotalHours / curr.RealWorkingTime.TotalHours;
                }
                else
                {
                    curr.Productivity = 0.0;
                }

                this.AnalysisData.Add(curr);
            }
            conn.Close();
        }
    }

    public struct ProductionHistoryStruct
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
        public String SalesOrderExternalID;
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
        public String ProductExternalID;
        public String MeasurementUnit;
    }

    public struct ProductionAnalysisStruct
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
        public String SalesOrderExternalID;
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
        public TimeSpan PlannedWorkingTime;
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
        public double Productivity;
    }

    public struct ProductionAnalysisResultStruct
    {
        public int Year;
        public int Month;
        public int Week;
        public int Day;
        public DateTime RealEndDate;
        public int Quantity;
        public Double WorkingTime;
        public Double UnitaryWorkingTime;
        public Double LeadTime;
        public Double Delay;
        public int ProductID;
        public int ProductReview;
        public int ProductTypeID;
        public String ProductName;
        public String ProductTypeName;
        public Double Productivity;
    }

    public struct SalesOrderStruct
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
        public String SalesOrderExternalID;
    }


    // Tasks Analysis
    public  class TaskProductionHistory
    {
        public String Tenant;
        public TaskProductionHistory(String Tenant)
        {
            this.Tenant = Tenant;
        }

        public List<TaskProductionHistoryStruct> TaskHistoricData;

        private class TaskProductionHistoryRow
        {
            public String CustomerID { get; set; }
            public String CustomerName { get; set; }
            public String CustomerVATNumber { get; set; }
            public String CustomerCodiceFiscale { get; set; }
            public String CustomerAddress { get; set; }
            public String CustomerCity { get; set; }
            public String CustomerProvince { get; set; }
            public String CustomerZipCode { get; set; }
            public String CustomerCountry { get; set; }
            public String CustomerPhoneNumber { get; set; }
            public String CustomerEMail { get; set; }
            public Boolean? CustomerKanbanManaged { get; set; }
            public int? SalesOrderID { get; set; }
            public int? SalesOrderYear { get; set; }
            public String SalesOrderCustomer { get; set; }
            public DateTime? SalesOrderDate { get; set; }
            public String SalesOrderNotes { get; set; }
            public int? ProductionOrderID { get; set; }
            public int? ProductionOrderYear { get; set; }
            public int? ProductionOrderProductTypeID { get; set; }
            public int? ProductionOrderProductTypeReview { get; set; }
            public int? ProductionOrderProductID { get; set; }
            public String ProductionOrderSerialNumber { get; set; }
            public String ProductionOrderStatus { get; set; }
            public int? ProductionOrderDepartmentID { get; set; }
            public DateTime? ProductionOrderStartTime { get; set; }
            public DateTime? ProductionOrderDeliveryDate { get; set; }
            public DateTime? ProductionOrderEndProductionDate { get; set; }
            public String ProductionOrderPlanner { get; set; }
            public int? ProductionOrderQuantityOrdered { get; set; }
            public int? ProductionOrderQuantityProduced { get; set; }
            public String ProductionOrderKanbanCardID { get; set; }
            public int? ProductTypeID { get; set; }
            public int? ProductTypeReview { get; set; }
            public DateTime? ProductTypeReviewDate { get; set; }
            public String ProductTypeName { get; set; }
            public String ProductTypeDescription { get; set; }
            public Boolean? ProductTypeEnabled { get; set; }
            public int? ProductID { get; set; }
            public String ProductName { get; set; }
            public String ProductDescription { get; set; }
            public int DepartmentID { get; set; }
            public String DepartmentName { get; set; }
            public String DepartmentDescription { get; set; }
            public Double? DepartmentTaktTime { get; set; }
            public String DepartmentTimeZone { get; set; }
            public TimeSpan? ProductRealLeadTime { get; set; }
            public TimeSpan? ProductRealWorkingTime { get; set; }
            public TimeSpan? ProductRealDelay { get; set; }
            public DateTime? ProductRealEndProductionDate { get; set; }
            public int? TaskID { get; set; }
            public String TaskName { get; set; }
            public String TaskDescription { get; set; }
            public DateTime? TaskEarlyStart { get; set; }
            public DateTime? TaskLateStart { get; set; }
            public DateTime? TaskEarlyFinish { get; set; }
            public DateTime? TaskLateFinish { get; set; }
            public String TaskStatus { get; set; }
            public int? TaskNumOperators { get; set; }
            public Double? TaskQuantityOrdered { get; set; }
            public Double? TaskQuantityProduced { get; set; }
            public TimeSpan? TaskSetupTimePlanned { get; set; }
            public TimeSpan? TaskCycleTimePlanned { get; set; }
            public TimeSpan? TaskUnloadTimePlanned { get; set; }
            public int? WorkstationID { get; set; }
            public String WorkstationName { get; set; }
            public String WorkstationDescription { get; set; }
            public DateTime? TaskEndDateReal { get; set; }
            public TimeSpan? TaskLeadTime { get; set; }
            public TimeSpan? TaskWorkingTime { get; set; }
            public TimeSpan? TaskDelay { get; set; }
            public int? TaskOriginalTaskID { get; set; }
            public int? TaskOriginalTaskRev { get; set; }
            public int? TaskOriginalTaskVar { get; set; }
            public TimeSpan? TaskPlannedWorkingTime { get; set; }
        }

        public void loadTaskProductionHistory()
        {
            this.TaskHistoricData = new List<TaskProductionHistoryStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT anagraficaclienti.codice AS CustomerID, "
+ "anagraficaclienti.ragsociale AS CustomerName,"
+ "anagraficaclienti.partitaiva AS CustomerVATNumber,"
+ "anagraficaclienti.codfiscale AS CustomerCodiceFiscale, "
+ "anagraficaclienti.indirizzo AS CustomerAddress, "
+ "anagraficaclienti.citta AS CustomerCity,"
+ "anagraficaclienti.provincia AS CustomerProvince,"
+ "anagraficaclienti.CAP AS CustomerZipCode,"
+ "anagraficaclienti.stato AS CustomerCountry,"
+ "anagraficaclienti.telefono AS CustomerPhoneNumber,"
+ "anagraficaclienti.email AS CustomerEMail,"
+ "anagraficaclienti.kanbanmanaged AS CustomerKanbanManaged,"
+ "commesse.idcommesse AS SalesOrderID,"
+ "commesse.anno AS SalesOrderYear,"
+ "commesse.cliente AS SalesOrderCustomer,"
+ "commesse.dataInserimento AS SalesOrderDate,"
+ "commesse.note AS SalesOrderNotes,"
+ "productionplan.id AS ProductionOrderID,"
+ "productionplan.anno AS ProductionOrderYear,"
+ "productionplan.processo AS ProductionOrderProductTypeID,"
+ "productionplan.revisione AS ProductionOrderProductTypeReview,"
+ "productionplan.variante AS ProductionOrderProductID,"
+ "productionplan.matricola AS ProductionOrderSerialNumber,"
+ "productionplan.status AS ProductionOrderStatus,"
+ "productionplan.reparto AS ProductionOrderDepartmentID,"
+ "productionplan.startTime AS ProductionOrderStartTime,"
+ "productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,"
+ "productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,"
+ "productionplan.planner AS ProductionOrderPlanner,"
+ "productionplan.quantita AS ProductionOrderQuantityOrdered,"
+ "productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,"
+ "productionplan.kanbanCard AS ProductionOrderKanbanCardID,"
+ "processo.processID AS ProductTypeID,"
+ "processo.revisione AS ProductTypeReview,"
+ "processo.dataRevisione AS ProductTypeReviewDate,"
+ "processo.Name AS ProductTypeName,"
+ "processo.description AS ProductTypeDescription,"
+ "processo.attivo AS ProductTypeEnabled,"
+ "varianti.idvariante AS ProductID,"
+ "varianti.nomeVariante AS ProductName,"
+ "varianti.descVariante AS ProductDescription,"
+ "reparti.idreparto AS DepartmentID,"
+ "reparti.nome AS DepartmentName,"
+ "reparti.descrizione AS DepartmentDescription,"
+ "reparti.cadenza AS DepartmentTaktTime,"
+ "reparti.timezone AS DepartmentTimeZone,"
+ " productionplan.LeadTime AS ProductRealLeadTime,"
+ " productionplan.WorkingTime AS ProductRealWorkingTime, "
+ " productionplan.Delay AS ProductRealDelay,"
+ " productionplan.EndProductionDateReal AS ProductRealEndProductionDate,"
+ "tasksproduzione.TaskiD AS TaskID,"
+ "tasksproduzione.name AS TaskName,"
+ "tasksproduzione.description AS TaskDescription,"
+ "tasksproduzione.earlyStart As TaskEarlyStart,"
+ "tasksproduzione.lateStart AS TaskLateStart,"
+ "tasksproduzione.earlyFinish AS TaskEarlyFinish,"
+ "tasksproduzione.lateFinish AS TaskLateFinish,"
+ "tasksproduzione.status AS TaskStatus,"
+ "tasksproduzione.nOperatori AS TaskNumOperators,"
+ "tasksproduzione.qtaPrevista AS TaskQuantityOrdered,"
+ "tasksproduzione.qtaProdotta AS TaskQuantityProduced,"
+ "tempiciclo.setup AS TaskSetupTimePlanned,"
+ "tempiciclo.tempo AS TaskCycleTimePlanned,"
+ "tempiciclo.tunload AS TaskUnloadTimePlanned,"
+ "postazioni.idpostazioni AS WorkstationID,"
+ "postazioni.name AS WorkstationName,"
+ "postazioni.description AS WorkstationDescription,"
+ "tasksproduzione.endDateReal as TaskEndDateReal,"
+ "tasksproduzione.LeadTime AS TaskLeadTime,"
+ "tasksproduzione.WorkingTime AS TaskWorkingTime,"
+ " tasksproduzione.Delay AS TaskDelay, "
+ " tasksproduzione.OrigTask AS TaskOriginalTaskID, "
+ " tasksproduzione.RevOrigTask AS TaskOriginalTaskRev, "
+ " tasksproduzione.variante AS TaskOriginalTaskVar, "
+ " tasksproduzione.tempoCiclo AS TaskPlannedWorkingTime "
+ " FROM anagraficaclienti INNER JOIN commesse ON(anagraficaclienti.codice = commesse.cliente) INNER JOIN"
+ " productionplan ON(commesse.anno = productionplan.annoCommessa AND commesse.idcommesse = productionplan.commessa)"
+ " INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto)"
+ " INNER JOIN varianti ON(varianti.idvariante = productionplan.variante)"
+ " INNER JOIN processo ON(processo.ProcessID = productionplan.processo AND processo.revisione = productionplan.revisione)"
+ " INNER JOIN tasksproduzione ON(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
 + " inner join processo AS TaskProcess ON(TaskProcess.processID = tasksproduzione.origTask AND TaskProcess.revisione = TasksProduzione.revOrigTask)"
+ " INNER JOIN varianti AS TaskVariant ON(taskvariant.idvariante = tasksproduzione.variante)"
 + "INNER JOIN postazioni ON(postazioni.idpostazioni = tasksproduzione.postazione)"
+ " INNER JOIN tempiciclo ON(tempiciclo.processo = tasksproduzione.origTask AND tempiciclo.revisione= tasksproduzione.revOrigTask AND tasksproduzione.variante = tempiciclo.variante)"
 + " WHERE tasksproduzione.status = 'F' order by productionplan.anno, productionplan.id, tasksproduzione.taskid;";


            var rows = conn.Query<TaskProductionHistoryRow>(sql);
            foreach (var row in rows)
            {
                TaskProductionHistoryStruct curr = new TaskProductionHistoryStruct();
                curr.DepartmentID = row.DepartmentID;
                KIS.App_Code.Reparto rp = new App_Code.Reparto(this.Tenant, curr.DepartmentID);
                if (row.CustomerID != null)
                {
                    curr.CustomerID = row.CustomerID;
                }
                if (row.CustomerName != null)
                {
                    curr.CustomerName = row.CustomerName;
                }
                if (row.CustomerVATNumber != null)
                {
                    curr.CustomerVATNumber = row.CustomerVATNumber;
                }
                if (row.CustomerCodiceFiscale != null)
                {
                    curr.CustomerCodiceFiscale = row.CustomerCodiceFiscale;
                }
                if (row.CustomerAddress != null)
                {
                    curr.CustomerAddress = row.CustomerAddress;
                }
                if (row.CustomerCity != null)
                {
                    curr.CustomerCity = row.CustomerCity;
                }
                if (row.CustomerProvince != null)
                {
                    curr.CustomerProvince = row.CustomerProvince;
                }
                if (row.CustomerZipCode != null)
                {
                    curr.CustomerZipCode = row.CustomerZipCode;
                }
                if (row.CustomerCountry != null)
                {
                    curr.CustomerCountry = row.CustomerCountry;
                }
                if (row.CustomerPhoneNumber != null)
                {
                    curr.CustomerPhoneNumber = row.CustomerPhoneNumber;
                }
                if (row.CustomerEMail != null)
                {
                    curr.CustomerEMail = row.CustomerEMail;
                }
                if (row.CustomerKanbanManaged.HasValue)
                {
                    curr.CustomerKanbanManaged = row.CustomerKanbanManaged.Value;
                }
                if (row.SalesOrderID.HasValue)
                {
                    curr.SalesOrderID = row.SalesOrderID.Value;
                }
                if (row.SalesOrderYear.HasValue)
                {
                    curr.SalesOrderYear = row.SalesOrderYear.Value;
                }
                if (row.SalesOrderCustomer != null)
                {
                    curr.SalesOrderCustomer = row.SalesOrderCustomer;
                }
                if (row.SalesOrderDate.HasValue)
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(row.SalesOrderDate.Value, rp.tzFusoOrario);
                }
                if (row.SalesOrderNotes != null)
                {
                    curr.SalesOrderNotes = row.SalesOrderNotes;
                }
                if (row.ProductionOrderID.HasValue)
                {
                    curr.ProductionOrderID = row.ProductionOrderID.Value;
                }
                if (row.ProductionOrderYear.HasValue)
                {
                    curr.ProductionOrderYear = row.ProductionOrderYear.Value;
                }
                if (row.ProductionOrderProductTypeID.HasValue)
                {
                    curr.ProductionOrderProductTypeID = row.ProductionOrderProductTypeID.Value;
                }
                if (row.ProductionOrderProductTypeReview.HasValue)
                {
                    curr.ProductionOrderProductTypeReview = row.ProductionOrderProductTypeReview.Value;
                }
                if (row.ProductionOrderProductID.HasValue)
                {
                    curr.ProductionOrderProductID = row.ProductionOrderProductID.Value;
                }
                if (row.ProductionOrderSerialNumber != null)
                {
                    curr.ProductionOrderSerialNumber = row.ProductionOrderSerialNumber;
                }
                if (row.ProductionOrderStatus != null)
                {
                    curr.ProductionOrderStatus = row.ProductionOrderStatus[0];
                }
                if (row.ProductionOrderDepartmentID.HasValue)
                {
                    curr.ProductionOrderDepartmentID = row.ProductionOrderDepartmentID.Value;
                }
                if (row.ProductionOrderStartTime.HasValue)
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderStartTime.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderDeliveryDate.HasValue)
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderDeliveryDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderEndProductionDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderPlanner != null)
                {
                    curr.ProductionOrderPlanner = row.ProductionOrderPlanner;
                }
                if (row.ProductionOrderQuantityOrdered.HasValue)
                {
                    curr.ProductionOrderQuantityOrdered = row.ProductionOrderQuantityOrdered.Value;
                }
                if (row.ProductionOrderQuantityProduced.HasValue)
                {
                    curr.ProductionOrderQuantityProduced = row.ProductionOrderQuantityProduced.Value;
                }
                if (row.ProductionOrderKanbanCardID != null)
                {
                    curr.ProductionOrderKanbanCardID = row.ProductionOrderKanbanCardID;
                }
                if (row.ProductTypeID.HasValue)
                {
                    curr.ProductTypeID = row.ProductTypeID.Value;
                }
                if (row.ProductTypeReview.HasValue)
                {
                    curr.ProductTypeReview = row.ProductTypeReview.Value;
                }
                if (row.ProductTypeReviewDate.HasValue)
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductTypeReviewDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductTypeName != null)
                {
                    curr.ProductTypeName = row.ProductTypeName;
                }
                if (row.ProductTypeDescription != null)
                {
                    curr.ProductTypeDescription = row.ProductTypeDescription;
                }
                if (row.ProductTypeEnabled.HasValue)
                {
                    curr.ProductTypeEnabled = row.ProductTypeEnabled.Value;
                }
                if (row.ProductID.HasValue)
                {
                    curr.ProductID = row.ProductID.Value;
                }
                if (row.ProductName != null)
                {
                    curr.ProductName = row.ProductName;
                }
                if (row.ProductDescription != null)
                {
                    curr.ProductDescription = row.ProductDescription;
                }
                if (row.DepartmentName != null)
                {
                    curr.DepartmentName = row.DepartmentName;
                }
                if (row.DepartmentDescription != null)
                {
                    curr.DepartmentDescription = row.DepartmentDescription;
                }
                if (row.DepartmentTaktTime.HasValue)
                {
                    curr.DepartmentTaktTime = row.DepartmentTaktTime.Value;
                }
                if (row.DepartmentTimeZone != null)
                {
                    curr.DepartmentTimeZone = row.DepartmentTimeZone;
                }
                if (row.ProductRealLeadTime.HasValue)
                {
                    curr.RealLeadTime = row.ProductRealLeadTime.Value;
                }
                if (row.ProductRealWorkingTime.HasValue)
                {
                    curr.RealWorkingTime = row.ProductRealWorkingTime.Value;
                }
                if (row.ProductRealDelay.HasValue)
                {
                    curr.RealDelay = row.ProductRealDelay.Value;
                }
                if (row.ProductRealEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDateReal = row.ProductRealEndProductionDate.Value;
                }
                if(row.TaskID.HasValue) { curr.TaskID = row.TaskID.Value; }
                if (row.TaskName != null) { curr.TaskName = row.TaskName; }
                if (row.TaskDescription != null) { curr.TaskDescription = row.TaskDescription; }
                if (row.TaskEarlyStart.HasValue) { curr.TaskEarlyStart = row.TaskEarlyStart.Value; }
                if (row.TaskLateStart.HasValue) { curr.TaskLateStart = row.TaskLateStart.Value; }
                if (row.TaskEarlyFinish.HasValue) { curr.TaskEarlyFinish = row.TaskEarlyFinish.Value; }
                if (row.TaskLateFinish.HasValue) { curr.TaskLateFinish = row.TaskLateFinish.Value; curr.TaskLateFinishWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskLateFinish); }
                if (row.TaskStatus != null) { curr.TaskStatus = row.TaskStatus[0]; }
                if (row.TaskNumOperators.HasValue) { curr.TaskNumOperators = row.TaskNumOperators.Value; }
                if (row.TaskQuantityOrdered.HasValue) { curr.TaskQuantityOrdered = row.TaskQuantityOrdered.Value; }
                if (row.TaskQuantityProduced.HasValue) { curr.TaskQuantityProduced = row.TaskQuantityProduced.Value; }
                if (row.TaskSetupTimePlanned.HasValue) { curr.TaskPlannedSetupTime = row.TaskSetupTimePlanned.Value; }
                if (row.TaskCycleTimePlanned.HasValue) { curr.TaskPlannedCycleTime = row.TaskCycleTimePlanned.Value; }
                if (row.TaskUnloadTimePlanned.HasValue) { curr.TaskPlannedUnloadTime = row.TaskUnloadTimePlanned.Value; }
                if (row.WorkstationID.HasValue) { curr.WorkstationID = row.WorkstationID.Value; }
                if (row.WorkstationName != null) { curr.WorkstationName = row.WorkstationName; }
                if (row.WorkstationDescription != null) { curr.WorkstationDescription = row.WorkstationDescription; }
                if (row.TaskEndDateReal.HasValue) { curr.TaskRealEndDate = row.TaskEndDateReal.Value; curr.TaskRealEndDateWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskRealEndDate); }
                if (row.TaskLeadTime.HasValue) { curr.TaskRealLeadTime = row.TaskLeadTime.Value; }
                if (row.TaskWorkingTime.HasValue) { curr.TaskRealWorkingTime = row.TaskWorkingTime.Value; }
                if (row.TaskDelay.HasValue) { curr.TaskRealDelay = row.TaskDelay.Value; }
                if (row.TaskOriginalTaskID.HasValue) { curr.TaskOriginalID = row.TaskOriginalTaskID.Value; }
                if (row.TaskOriginalTaskRev.HasValue) { curr.TaskOriginalRev = row.TaskOriginalTaskRev.Value; }
                if (row.TaskOriginalTaskVar.HasValue) { curr.TaskOriginalVar = row.TaskOriginalTaskVar.Value; }
                if (row.TaskPlannedWorkingTime.HasValue) { curr.TaskPlannedWorkingTime = row.TaskPlannedWorkingTime.Value; }

                this.TaskHistoricData.Add(curr);
            }
            conn.Close();
        }

        public void loadTasksProductionWorkload()
        {
            this.TaskHistoricData = new List<TaskProductionHistoryStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT anagraficaclienti.codice AS CustomerID, "
                + "anagraficaclienti.ragsociale AS CustomerName,"
                + "anagraficaclienti.partitaiva AS CustomerVATNumber,"
                + "anagraficaclienti.codfiscale AS CustomerCodiceFiscale, "
                + "anagraficaclienti.indirizzo AS CustomerAddress, "
                + "anagraficaclienti.citta AS CustomerCity,"
                + "anagraficaclienti.provincia AS CustomerProvince,"
                + "anagraficaclienti.CAP AS CustomerZipCode,"
                + "anagraficaclienti.stato AS CustomerCountry,"
                + "anagraficaclienti.telefono AS CustomerPhoneNumber,"
                + "anagraficaclienti.email AS CustomerEMail,"
                + "anagraficaclienti.kanbanmanaged AS CustomerKanbanManaged,"
                + "commesse.idcommesse AS SalesOrderID,"
                + "commesse.anno AS SalesOrderYear,"
                + "commesse.cliente AS SalesOrderCustomer,"
                + "commesse.dataInserimento AS SalesOrderDate,"
                + "commesse.note AS SalesOrderNotes,"
                + "productionplan.id AS ProductionOrderID,"
                + "productionplan.anno AS ProductionOrderYear,"
                + "productionplan.processo AS ProductionOrderProductTypeID,"
                + "productionplan.revisione AS ProductionOrderProductTypeReview,"
                + "productionplan.variante AS ProductionOrderProductID,"
                + "productionplan.matricola AS ProductionOrderSerialNumber,"
                + "productionplan.status AS ProductionOrderStatus,"
                + "productionplan.reparto AS ProductionOrderDepartmentID,"
                + "productionplan.startTime AS ProductionOrderStartTime,"
                + "productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,"
                + "productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,"
                + "productionplan.planner AS ProductionOrderPlanner,"
                + "productionplan.quantita AS ProductionOrderQuantityOrdered,"
                + "productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,"
                + "productionplan.kanbanCard AS ProductionOrderKanbanCardID,"
                + "processo.processID AS ProductTypeID,"
                + "processo.revisione AS ProductTypeReview,"
                + "processo.dataRevisione AS ProductTypeReviewDate,"
                + "processo.Name AS ProductTypeName,"
                + "processo.description AS ProductTypeDescription,"
                + "processo.attivo AS ProductTypeEnabled,"
                + "varianti.idvariante AS ProductID,"
                + "varianti.nomeVariante AS ProductName,"
                + "varianti.descVariante AS ProductDescription,"
                + "reparti.idreparto AS DepartmentID,"
                + "reparti.nome AS DepartmentName,"
                + "reparti.descrizione AS DepartmentDescription,"
                + "reparti.cadenza AS DepartmentTaktTime,"
                + "reparti.timezone AS DepartmentTimeZone,"
                + " productionplan.LeadTime AS ProductRealLeadTime,"
                + " productionplan.WorkingTime AS ProductRealWorkingTime, "
                + " productionplan.Delay AS ProductRealDelay,"
                + " productionplan.EndProductionDateReal AS ProductRealEndProductionDate,"
                + "tasksproduzione.TaskiD AS TaskID,"
                + "tasksproduzione.name AS TaskName,"
                + "tasksproduzione.description AS TaskDescription,"
                + "tasksproduzione.earlyStart As TaskEarlyStart,"
                + "tasksproduzione.lateStart AS TaskLateStart,"
                + "tasksproduzione.earlyFinish AS TaskEarlyFinish,"
                + "tasksproduzione.lateFinish AS TaskLateFinish,"
                + "tasksproduzione.status AS TaskStatus,"
                + "tasksproduzione.nOperatori AS TaskNumOperators,"
                + "tasksproduzione.qtaPrevista AS TaskQuantityOrdered,"
                + "tasksproduzione.qtaProdotta AS TaskQuantityProduced,"
                + "tempiciclo.setup AS TaskSetupTimePlanned,"
                + "tempiciclo.tempo AS TaskCycleTimePlanned,"
                + "tempiciclo.tunload AS TaskUnloadTimePlanned,"
                + "postazioni.idpostazioni AS WorkstationID,"
                + "postazioni.name AS WorkstationName,"
                + "postazioni.description AS WorkstationDescription,"
                + "tasksproduzione.endDateReal as TaskEndDateReal,"
                + "tasksproduzione.LeadTime AS TaskLeadTime,"
                + "tasksproduzione.WorkingTime AS TaskWorkingTime,"
                + " tasksproduzione.Delay AS TaskDelay, "
                + " tasksproduzione.OrigTask AS TaskOriginalTaskID, "
                + " tasksproduzione.RevOrigTask AS TaskOriginalTaskRev, "
                + " tasksproduzione.variante AS TaskOriginalTaskVar, "
                + " tasksproduzione.tempoCiclo AS TaskPlannedWorkingTime "
                + " FROM anagraficaclienti INNER JOIN commesse ON(anagraficaclienti.codice = commesse.cliente) INNER JOIN"
                + " productionplan ON(commesse.anno = productionplan.annoCommessa AND commesse.idcommesse = productionplan.commessa)"
                + " INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto)"
                + " INNER JOIN varianti ON(varianti.idvariante = productionplan.variante)"
                + " INNER JOIN processo ON(processo.ProcessID = productionplan.processo AND processo.revisione = productionplan.revisione)"
                + " INNER JOIN tasksproduzione ON(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
                 + " inner join processo AS TaskProcess ON(TaskProcess.processID = tasksproduzione.origTask AND TaskProcess.revisione = TasksProduzione.revOrigTask)"
                + " INNER JOIN varianti AS TaskVariant ON(taskvariant.idvariante = tasksproduzione.variante)"
                 + "INNER JOIN postazioni ON(postazioni.idpostazioni = tasksproduzione.postazione)"
                + " INNER JOIN tempiciclo ON(tempiciclo.processo = tasksproduzione.origTask AND tempiciclo.revisione= tasksproduzione.revOrigTask AND tasksproduzione.variante = tempiciclo.variante)"
                 + " WHERE tasksproduzione.status <> 'F' order by productionplan.anno, productionplan.id, tasksproduzione.taskid;";


            var rows = conn.Query<TaskProductionHistoryRow>(sql);
            foreach (var row in rows)
            {
                TaskProductionHistoryStruct curr = new TaskProductionHistoryStruct();
                curr.DepartmentID = row.DepartmentID;
                KIS.App_Code.Reparto rp = new App_Code.Reparto(this.Tenant, curr.DepartmentID);
                if (row.CustomerID != null)
                {
                    curr.CustomerID = row.CustomerID;
                }
                if (row.CustomerName != null)
                {
                    curr.CustomerName = row.CustomerName;
                }
                if (row.CustomerVATNumber != null)
                {
                    curr.CustomerVATNumber = row.CustomerVATNumber;
                }
                if (row.CustomerCodiceFiscale != null)
                {
                    curr.CustomerCodiceFiscale = row.CustomerCodiceFiscale;
                }
                if (row.CustomerAddress != null)
                {
                    curr.CustomerAddress = row.CustomerAddress;
                }
                if (row.CustomerCity != null)
                {
                    curr.CustomerCity = row.CustomerCity;
                }
                if (row.CustomerProvince != null)
                {
                    curr.CustomerProvince = row.CustomerProvince;
                }
                if (row.CustomerZipCode != null)
                {
                    curr.CustomerZipCode = row.CustomerZipCode;
                }
                if (row.CustomerCountry != null)
                {
                    curr.CustomerCountry = row.CustomerCountry;
                }
                if (row.CustomerPhoneNumber != null)
                {
                    curr.CustomerPhoneNumber = row.CustomerPhoneNumber;
                }
                if (row.CustomerEMail != null)
                {
                    curr.CustomerEMail = row.CustomerEMail;
                }
                if (row.CustomerKanbanManaged.HasValue)
                {
                    curr.CustomerKanbanManaged = row.CustomerKanbanManaged.Value;
                }
                if (row.SalesOrderID.HasValue)
                {
                    curr.SalesOrderID = row.SalesOrderID.Value;
                }
                if (row.SalesOrderYear.HasValue)
                {
                    curr.SalesOrderYear = row.SalesOrderYear.Value;
                }
                if (row.SalesOrderCustomer != null)
                {
                    curr.SalesOrderCustomer = row.SalesOrderCustomer;
                }
                if (row.SalesOrderDate.HasValue)
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(row.SalesOrderDate.Value, rp.tzFusoOrario);
                }
                if (row.SalesOrderNotes != null)
                {
                    curr.SalesOrderNotes = row.SalesOrderNotes;
                }
                if (row.ProductionOrderID.HasValue)
                {
                    curr.ProductionOrderID = row.ProductionOrderID.Value;
                }
                if (row.ProductionOrderYear.HasValue)
                {
                    curr.ProductionOrderYear = row.ProductionOrderYear.Value;
                }
                if (row.ProductionOrderProductTypeID.HasValue)
                {
                    curr.ProductionOrderProductTypeID = row.ProductionOrderProductTypeID.Value;
                }
                if (row.ProductionOrderProductTypeReview.HasValue)
                {
                    curr.ProductionOrderProductTypeReview = row.ProductionOrderProductTypeReview.Value;
                }
                if (row.ProductionOrderProductID.HasValue)
                {
                    curr.ProductionOrderProductID = row.ProductionOrderProductID.Value;
                }
                if (row.ProductionOrderSerialNumber != null)
                {
                    curr.ProductionOrderSerialNumber = row.ProductionOrderSerialNumber;
                }
                if (row.ProductionOrderStatus != null)
                {
                    curr.ProductionOrderStatus = row.ProductionOrderStatus[0];
                }
                if (row.ProductionOrderDepartmentID.HasValue)
                {
                    curr.ProductionOrderDepartmentID = row.ProductionOrderDepartmentID.Value;
                }
                if (row.ProductionOrderStartTime.HasValue)
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderStartTime.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderDeliveryDate.HasValue)
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderDeliveryDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderEndProductionDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderPlanner != null)
                {
                    curr.ProductionOrderPlanner = row.ProductionOrderPlanner;
                }
                if (row.ProductionOrderQuantityOrdered.HasValue)
                {
                    curr.ProductionOrderQuantityOrdered = row.ProductionOrderQuantityOrdered.Value;
                }
                if (row.ProductionOrderQuantityProduced.HasValue)
                {
                    curr.ProductionOrderQuantityProduced = row.ProductionOrderQuantityProduced.Value;
                }
                if (row.ProductionOrderKanbanCardID != null)
                {
                    curr.ProductionOrderKanbanCardID = row.ProductionOrderKanbanCardID;
                }
                if (row.ProductTypeID.HasValue)
                {
                    curr.ProductTypeID = row.ProductTypeID.Value;
                }
                if (row.ProductTypeReview.HasValue)
                {
                    curr.ProductTypeReview = row.ProductTypeReview.Value;
                }
                if (row.ProductTypeReviewDate.HasValue)
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductTypeReviewDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductTypeName != null)
                {
                    curr.ProductTypeName = row.ProductTypeName;
                }
                if (row.ProductTypeDescription != null)
                {
                    curr.ProductTypeDescription = row.ProductTypeDescription;
                }
                if (row.ProductTypeEnabled.HasValue)
                {
                    curr.ProductTypeEnabled = row.ProductTypeEnabled.Value;
                }
                if (row.ProductID.HasValue)
                {
                    curr.ProductID = row.ProductID.Value;
                }
                if (row.ProductName != null)
                {
                    curr.ProductName = row.ProductName;
                }
                if (row.ProductDescription != null)
                {
                    curr.ProductDescription = row.ProductDescription;
                }
                if (row.DepartmentName != null)
                {
                    curr.DepartmentName = row.DepartmentName;
                }
                if (row.DepartmentDescription != null)
                {
                    curr.DepartmentDescription = row.DepartmentDescription;
                }
                if (row.DepartmentTaktTime.HasValue)
                {
                    curr.DepartmentTaktTime = row.DepartmentTaktTime.Value;
                }
                if (row.DepartmentTimeZone != null)
                {
                    curr.DepartmentTimeZone = row.DepartmentTimeZone;
                }
                if (row.ProductRealLeadTime.HasValue)
                {
                    curr.RealLeadTime = row.ProductRealLeadTime.Value;
                }
                if (row.ProductRealWorkingTime.HasValue)
                {
                    curr.RealWorkingTime = row.ProductRealWorkingTime.Value;
                }
                if (row.ProductRealDelay.HasValue)
                {
                    curr.RealDelay = row.ProductRealDelay.Value;
                }
                if (row.ProductRealEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDateReal = row.ProductRealEndProductionDate.Value;
                }
                if(row.TaskID.HasValue) { curr.TaskID = row.TaskID.Value; }
                if (row.TaskName != null) { curr.TaskName = row.TaskName; }
                if (row.TaskDescription != null) { curr.TaskDescription = row.TaskDescription; }
                if (row.TaskEarlyStart.HasValue) { curr.TaskEarlyStart = row.TaskEarlyStart.Value; }
                if (row.TaskLateStart.HasValue) { curr.TaskLateStart = row.TaskLateStart.Value; }
                if (row.TaskEarlyFinish.HasValue) { curr.TaskEarlyFinish = row.TaskEarlyFinish.Value; }
                if (row.TaskLateFinish.HasValue) { curr.TaskLateFinish = row.TaskLateFinish.Value; curr.TaskLateFinishWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskLateFinish); }
                if (row.TaskStatus != null) { curr.TaskStatus = row.TaskStatus[0]; }
                if (row.TaskNumOperators.HasValue) { curr.TaskNumOperators = row.TaskNumOperators.Value; }
                if (row.TaskQuantityOrdered.HasValue) { curr.TaskQuantityOrdered = row.TaskQuantityOrdered.Value; }
                if (row.TaskQuantityProduced.HasValue) { curr.TaskQuantityProduced = row.TaskQuantityProduced.Value; }
                if (row.TaskSetupTimePlanned.HasValue) { curr.TaskPlannedSetupTime = row.TaskSetupTimePlanned.Value; }
                if (row.TaskCycleTimePlanned.HasValue) { curr.TaskPlannedCycleTime = row.TaskCycleTimePlanned.Value; }
                if (row.TaskUnloadTimePlanned.HasValue) { curr.TaskPlannedUnloadTime = row.TaskUnloadTimePlanned.Value; }
                if (row.WorkstationID.HasValue) { curr.WorkstationID = row.WorkstationID.Value; }
                if (row.WorkstationName != null) { curr.WorkstationName = row.WorkstationName; }
                if (row.WorkstationDescription != null) { curr.WorkstationDescription = row.WorkstationDescription; }
                if (row.TaskEndDateReal.HasValue) { curr.TaskRealEndDate = row.TaskEndDateReal.Value; curr.TaskRealEndDateWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskRealEndDate); }
                if (row.TaskLeadTime.HasValue) { curr.TaskRealLeadTime = row.TaskLeadTime.Value; }
                if (row.TaskWorkingTime.HasValue) { curr.TaskRealWorkingTime = row.TaskWorkingTime.Value; }
                if (row.TaskDelay.HasValue) { curr.TaskRealDelay = row.TaskDelay.Value; }
                if (row.TaskOriginalTaskID.HasValue) { curr.TaskOriginalID = row.TaskOriginalTaskID.Value; }
                if (row.TaskOriginalTaskRev.HasValue) { curr.TaskOriginalRev = row.TaskOriginalTaskRev.Value; }
                if (row.TaskOriginalTaskVar.HasValue) { curr.TaskOriginalVar = row.TaskOriginalTaskVar.Value; }
                if (row.TaskPlannedWorkingTime.HasValue) { curr.TaskPlannedWorkingTime = row.TaskPlannedWorkingTime.Value; }

                this.TaskHistoricData.Add(curr);
            }
            conn.Close();
        }
    }

    public struct TaskProductionHistoryStruct
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
        public int TaskLateFinishWeek;
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
    }

    public struct TaskProductionAnalysisResultStruct
    {
        public int Year;
        public int Month;
        public int Week;
        public int Day;
        public DateTime RealEndDate;
        public Double Quantity;
        public Double WorkingTime;
        public Double UnitaryWorkingTime;
        public Double LeadTime;
        public Double Delay;
        public int ProductID;
        public int ProductReview;
        public int ProductTypeID;
        public int DepartmentID;
        public String DepartmentName;
        public String ProductName;
        public String ProductTypeName;
        public int TaskID;
        public String TaskName;
        public int TaskTypeID;
        public int WorkstationID;
        public int WotkstationName;
        public Double Productivity;
    }

    // KPIs structs
    public struct DepartmentKPIsStruct
        {
        public int DepartmentID;
        public String DepartmentName;
        public Double Productivity;
        public Double LeadTime;
        public Double Delay;
        public Double Quantities;
        public int Week;
        public int Year;
    }


    // EventsExportStruct and class --> SIAV
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

    public class TaskEvents
    {
        public String Tenant;

        public String log;

        public List<TaskEventStruct> TaskEventsData;

        private class TaskEventRow
        {
            public String CustomerID { get; set; }
            public String CustomerName { get; set; }
            public String CustomerVATNumber { get; set; }
            public String CustomerCodiceFiscale { get; set; }
            public String CustomerAddress { get; set; }
            public String CustomerCity { get; set; }
            public String CustomerProvince { get; set; }
            public String CustomerZipCode { get; set; }
            public String CustomerCountry { get; set; }
            public String CustomerPhoneNumber { get; set; }
            public String CustomerEMail { get; set; }
            public Boolean? CustomerKanbanManaged { get; set; }
            public int? SalesOrderID { get; set; }
            public int? SalesOrderYear { get; set; }
            public String SalesOrderCustomer { get; set; }
            public DateTime? SalesOrderDate { get; set; }
            public String SalesOrderNotes { get; set; }
            public int? ProductionOrderID { get; set; }
            public int? ProductionOrderYear { get; set; }
            public int? ProductionOrderProductTypeID { get; set; }
            public int? ProductionOrderProductTypeReview { get; set; }
            public int? ProductionOrderProductID { get; set; }
            public String ProductionOrderSerialNumber { get; set; }
            public String ProductionOrderStatus { get; set; }
            public int? ProductionOrderDepartmentID { get; set; }
            public DateTime? ProductionOrderStartTime { get; set; }
            public DateTime? ProductionOrderDeliveryDate { get; set; }
            public DateTime? ProductionOrderEndProductionDate { get; set; }
            public String ProductionOrderPlanner { get; set; }
            public int? ProductionOrderQuantityOrdered { get; set; }
            public int? ProductionOrderQuantityProduced { get; set; }
            public String ProductionOrderKanbanCardID { get; set; }
            public int? ProductTypeID { get; set; }
            public int? ProductTypeReview { get; set; }
            public DateTime? ProductTypeReviewDate { get; set; }
            public String ProductTypeName { get; set; }
            public String ProductTypeDescription { get; set; }
            public Boolean? ProductTypeEnabled { get; set; }
            public int? ProductID { get; set; }
            public String ProductName { get; set; }
            public String ProductDescription { get; set; }
            public int DepartmentID { get; set; }
            public String DepartmentName { get; set; }
            public String DepartmentDescription { get; set; }
            public Double? DepartmentTaktTime { get; set; }
            public String DepartmentTimeZone { get; set; }
            public TimeSpan? ProductRealLeadTime { get; set; }
            public TimeSpan? ProductRealWorkingTime { get; set; }
            public TimeSpan? ProductRealDelay { get; set; }
            public DateTime? ProductRealEndProductionDate { get; set; }
            public int? TaskID { get; set; }
            public String TaskName { get; set; }
            public String TaskDescription { get; set; }
            public DateTime? TaskEarlyStart { get; set; }
            public DateTime? TaskLateStart { get; set; }
            public DateTime? TaskEarlyFinish { get; set; }
            public DateTime? TaskLateFinish { get; set; }
            public String TaskStatus { get; set; }
            public int? TaskNumOperators { get; set; }
            public Double? TaskQuantityOrdered { get; set; }
            public Double? TaskQuantityProduced { get; set; }
            public TimeSpan? TaskSetupTimePlanned { get; set; }
            public TimeSpan? TaskCycleTimePlanned { get; set; }
            public TimeSpan? TaskUnloadTimePlanned { get; set; }
            public int? WorkstationID { get; set; }
            public String WorkstationName { get; set; }
            public String WorkstationDescription { get; set; }
            public DateTime? TaskEndDateReal { get; set; }
            public TimeSpan? TaskLeadTime { get; set; }
            public TimeSpan? TaskWorkingTime { get; set; }
            public TimeSpan? TaskDelay { get; set; }
            public int? TaskOriginalTaskID { get; set; }
            public int? TaskOriginalTaskRev { get; set; }
            public int? TaskOriginalTaskVar { get; set; }
            public TimeSpan? TaskPlannedWorkingTime { get; set; }
            public int? id { get; set; }
            public DateTime? data { get; set; }
            public String evento { get; set; }
            public String user { get; set; }
        }

        public TaskEvents(String Tenant)
        {
            this.Tenant = Tenant;
            this.TaskEventsData = new List<TaskEventStruct>();
        }

        public void loadTaskEvents(DateTime start, DateTime end)
        {
            this.TaskEventsData = new List<TaskEventStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT "
               + " anagraficaclienti.codice AS CustomerID, " // 0
+ "anagraficaclienti.ragsociale AS CustomerName,"
+ "anagraficaclienti.partitaiva AS CustomerVATNumber," // 2
+ "anagraficaclienti.codfiscale AS CustomerCodiceFiscale, "
+ "anagraficaclienti.indirizzo AS CustomerAddress, "            // 4
+ "anagraficaclienti.citta AS CustomerCity,"                    // 5    
+ "anagraficaclienti.provincia AS CustomerProvince,"            // 6
+ "anagraficaclienti.CAP AS CustomerZipCode,"   
+ "anagraficaclienti.stato AS CustomerCountry,"                 // 8
+ "anagraficaclienti.telefono AS CustomerPhoneNumber,"
+ "anagraficaclienti.email AS CustomerEMail,"                   // 10
+ "anagraficaclienti.kanbanmanaged AS CustomerKanbanManaged,"
+ "commesse.idcommesse AS SalesOrderID,"                        // 12
+ "commesse.anno AS SalesOrderYear,"
+ "commesse.cliente AS SalesOrderCustomer,"                     // 14
+ "commesse.dataInserimento AS SalesOrderDate,"
+ "commesse.note AS SalesOrderNotes,"                           // 16
+ "productionplan.id AS ProductionOrderID,"
+ "productionplan.anno AS ProductionOrderYear,"                 // 18
+ "productionplan.processo AS ProductionOrderProductTypeID,"    // 19
+ "productionplan.revisione AS ProductionOrderProductTypeReview,"   
+ "productionplan.variante AS ProductionOrderProductID,"        // 21
+ "productionplan.matricola AS ProductionOrderSerialNumber,"    // 22
+ "productionplan.status AS ProductionOrderStatus,"
+ "productionplan.reparto AS ProductionOrderDepartmentID,"      // 24
+ "productionplan.startTime AS ProductionOrderStartTime,"
+ "productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate," // 26
+ "productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,"
+ "productionplan.planner AS ProductionOrderPlanner,"           // 28
+ "productionplan.quantita AS ProductionOrderQuantityOrdered,"
+ "productionplan.quantitaProdotta AS ProductionOrderQuantityProduced," // 30
+ "productionplan.kanbanCard AS ProductionOrderKanbanCardID,"
+ "processo.processID AS ProductTypeID,"                        // 32
+ "processo.revisione AS ProductTypeReview,"
+ "processo.dataRevisione AS ProductTypeReviewDate,"            // 34
+ "processo.Name AS ProductTypeName,"
+ "processo.description AS ProductTypeDescription,"             // 36
+ "processo.attivo AS ProductTypeEnabled,"
+ "varianti.idvariante AS ProductID,"                           // 38
+ "varianti.nomeVariante AS ProductName,"
+ "varianti.descVariante AS ProductDescription,"                // 40
+ "reparti.idreparto AS DepartmentID,"
+ "reparti.nome AS DepartmentName,"                             // 42
+ "reparti.descrizione AS DepartmentDescription,"
+ "reparti.cadenza AS DepartmentTaktTime,"                      // 44
+ "reparti.timezone AS DepartmentTimeZone,"
+ " productionplan.LeadTime AS ProductRealLeadTime,"            // 46
+ " productionplan.WorkingTime AS ProductRealWorkingTime, "
+ " productionplan.Delay AS ProductRealDelay,"                  // 48
+ " productionplan.EndProductionDateReal AS ProductRealEndProductionDate,"
+ "tasksproduzione.TaskiD AS TaskID,"                           // 50
+ "tasksproduzione.name AS TaskName,"
+ "tasksproduzione.description AS TaskDescription,"             // 52
+ "tasksproduzione.earlyStart As TaskEarlyStart,"
+ "tasksproduzione.lateStart AS TaskLateStart,"                 // 54
+ "tasksproduzione.earlyFinish AS TaskEarlyFinish,"
+ "tasksproduzione.lateFinish AS TaskLateFinish,"               // 56
+ "tasksproduzione.status AS TaskStatus,"
+ "tasksproduzione.nOperatori AS TaskNumOperators,"             // 58
+ "tasksproduzione.qtaPrevista AS TaskQuantityOrdered,"         // 59
+ "tasksproduzione.qtaProdotta AS TaskQuantityProduced,"        // 60
+ "tempiciclo.setup AS TaskSetupTimePlanned,"                   // 61
+ "tempiciclo.tempo AS TaskCycleTimePlanned,"                   // 62
+ "tempiciclo.tunload AS TaskUnloadTimePlanned,"
+ "postazioni.idpostazioni AS WorkstationID,"                   // 64
+ "postazioni.name AS WorkstationName,"
+ "postazioni.description AS WorkstationDescription,"           // 66
+ "tasksproduzione.endDateReal as TaskEndDateReal,"
+ "tasksproduzione.LeadTime AS TaskLeadTime,"                   // 68
+ "tasksproduzione.WorkingTime AS TaskWorkingTime,"
+ " tasksproduzione.Delay AS TaskDelay, "                       // 70
+ " tasksproduzione.OrigTask AS TaskOriginalTaskID, "
+ " tasksproduzione.RevOrigTask AS TaskOriginalTaskRev, "       // 72
+ " tasksproduzione.variante AS TaskOriginalTaskVar, "
+ " tasksproduzione.tempoCiclo AS TaskPlannedWorkingTime, "      // 74
                //+ "processo.Name AS ProductTypeName, "
                //+ "tasksproduzione.taskid AS TaskID, "
                //+ "tasksproduzione.NAME AS TaskName, "
                + "registroeventitaskproduzione.id, "         // 75
                + "registroeventitaskproduzione.data, "         // 76
                + "registroeventitaskproduzione.evento, "       // 77
                + "registroeventitaskproduzione.user "          // 78
                + "FROM anagraficaclienti INNER JOIN commesse ON(anagraficaclienti.codice = commesse.cliente) "
                + "INNER JOIN productionplan ON(commesse.anno = productionplan.annocommessa AND commesse.idcommesse = productionplan.commessa) "
                + "INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto) "
                + "INNER JOIN varianti ON(varianti.idvariante = productionplan.variante) "
                + "INNER JOIN processo ON(processo.processid = productionplan.processo AND processo.revisione = productionplan.revisione) "
                + "INNER JOIN tasksproduzione ON(tasksproduzione.idarticolo = productionplan.id AND tasksproduzione.annoarticolo = productionplan.anno) "
                + "INNER JOIN processo AS TaskProcess ON(TaskProcess.processid = tasksproduzione.origtask AND TaskProcess.revisione = tasksproduzione.revorigtask) " 
                + "INNER JOIN varianti AS TaskVariant ON(taskvariant.idvariante = tasksproduzione.variante) "
                + "INNER JOIN postazioni ON(postazioni.idpostazioni = tasksproduzione.postazione) "
                + "INNER JOIN tempiciclo ON(tempiciclo.processo = tasksproduzione.origtask AND tempiciclo.revisione = tasksproduzione.revorigtask AND tasksproduzione.variante = tempiciclo.variante) "
                + "INNER JOIN registroeventitaskproduzione ON (registroeventitaskproduzione.task = tasksproduzione.taskid) "
                + " WHERE tasksproduzione.status = 'F' AND productionplan.status = 'F' "
                + " AND productionplan.EndProductionDateReal IS NOT NULL AND productionplan.EndProductionDateReal >= '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                + " AND productionplan.EndProductionDateReal <= '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                + " ORDER  BY productionplan.anno, productionplan.id asc, tasksproduzione.taskid, registroeventitaskproduzione.USER, registroeventitaskproduzione.data asc";


            var rows = conn.Query<TaskEventRow>(sql);
            foreach (var row in rows)
            {
                TaskEventStruct curr = new TaskEventStruct();

                curr.DepartmentID = row.DepartmentID;
                KIS.App_Code.Reparto rp = new App_Code.Reparto(this.Tenant, curr.DepartmentID);
                if (row.CustomerID != null)
                {
                    curr.CustomerID = row.CustomerID;
                }
                if (row.CustomerName != null)
                {
                    curr.CustomerName = row.CustomerName;
                }
                if (row.CustomerVATNumber != null)
                {
                    curr.CustomerVATNumber = row.CustomerVATNumber;
                }
                if (row.CustomerCodiceFiscale != null)
                {
                    curr.CustomerCodiceFiscale = row.CustomerCodiceFiscale;
                }
                if (row.CustomerAddress != null)
                {
                    curr.CustomerAddress = row.CustomerAddress;
                }
                if (row.CustomerCity != null)
                {
                    curr.CustomerCity = row.CustomerCity;
                }
                if (row.CustomerProvince != null)
                {
                    curr.CustomerProvince = row.CustomerProvince;
                }
                if (row.CustomerZipCode != null)
                {
                    curr.CustomerZipCode = row.CustomerZipCode;
                }
                if (row.CustomerCountry != null)
                {
                    curr.CustomerCountry = row.CustomerCountry;
                }
                if (row.CustomerPhoneNumber != null)
                {
                    curr.CustomerPhoneNumber = row.CustomerPhoneNumber;
                }
                if (row.CustomerEMail != null)
                {
                    curr.CustomerEMail = row.CustomerEMail;
                }
                if (row.CustomerKanbanManaged.HasValue)
                {
                    curr.CustomerKanbanManaged = row.CustomerKanbanManaged.Value;
                }
                if (row.SalesOrderID.HasValue)
                {
                    curr.SalesOrderID = row.SalesOrderID.Value;
                }
                if (row.SalesOrderYear.HasValue)
                {
                    curr.SalesOrderYear = row.SalesOrderYear.Value;
                }
                if (row.SalesOrderCustomer != null)
                {
                    curr.SalesOrderCustomer = row.SalesOrderCustomer;
                }
                if (row.SalesOrderDate.HasValue)
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(row.SalesOrderDate.Value, rp.tzFusoOrario);
                }
                if (row.SalesOrderNotes != null)
                {
                    curr.SalesOrderNotes = row.SalesOrderNotes;
                }
                if (row.ProductionOrderID.HasValue)
                {
                    curr.ProductionOrderID = row.ProductionOrderID.Value;
                }
                if (row.ProductionOrderYear.HasValue)
                {
                    curr.ProductionOrderYear = row.ProductionOrderYear.Value;
                }
                if (row.ProductionOrderProductTypeID.HasValue)
                {
                    curr.ProductionOrderProductTypeID = row.ProductionOrderProductTypeID.Value;
                }
                if (row.ProductionOrderProductTypeReview.HasValue)
                {
                    curr.ProductionOrderProductTypeReview = row.ProductionOrderProductTypeReview.Value;
                }
                if (row.ProductionOrderProductID.HasValue)
                {
                    curr.ProductionOrderProductID = row.ProductionOrderProductID.Value;
                }
                if (row.ProductionOrderSerialNumber != null)
                {
                    curr.ProductionOrderSerialNumber = row.ProductionOrderSerialNumber;
                }
                if (row.ProductionOrderStatus != null)
                {
                    curr.ProductionOrderStatus = row.ProductionOrderStatus[0];
                }
                if (row.ProductionOrderDepartmentID.HasValue)
                {
                    curr.ProductionOrderDepartmentID = row.ProductionOrderDepartmentID.Value;
                }
                if (row.ProductionOrderStartTime.HasValue)
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderStartTime.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderDeliveryDate.HasValue)
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderDeliveryDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderEndProductionDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductionOrderPlanner != null)
                {
                    curr.ProductionOrderPlanner = row.ProductionOrderPlanner;
                }
                if (row.ProductionOrderQuantityOrdered.HasValue)
                {
                    curr.ProductionOrderQuantityOrdered = row.ProductionOrderQuantityOrdered.Value;
                }
                if (row.ProductionOrderQuantityProduced.HasValue)
                {
                    curr.ProductionOrderQuantityProduced = row.ProductionOrderQuantityProduced.Value;
                }
                if (row.ProductionOrderKanbanCardID != null)
                {
                    curr.ProductionOrderKanbanCardID = row.ProductionOrderKanbanCardID;
                }
                if (row.ProductTypeID.HasValue)
                {
                    curr.ProductTypeID = row.ProductTypeID.Value;
                }
                if (row.ProductTypeReview.HasValue)
                {
                    curr.ProductTypeReview = row.ProductTypeReview.Value;
                }
                if (row.ProductTypeReviewDate.HasValue)
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductTypeReviewDate.Value, rp.tzFusoOrario);
                }
                if (row.ProductTypeName != null)
                {
                    curr.ProductTypeName = row.ProductTypeName;
                }
                if (row.ProductTypeDescription != null)
                {
                    curr.ProductTypeDescription = row.ProductTypeDescription;
                }
                if (row.ProductTypeEnabled.HasValue)
                {
                    curr.ProductTypeEnabled = row.ProductTypeEnabled.Value;
                }
                if (row.ProductID.HasValue)
                {
                    curr.ProductID = row.ProductID.Value;
                }
                if (row.ProductName != null)
                {
                    curr.ProductName = row.ProductName;
                }
                if (row.ProductDescription != null)
                {
                    curr.ProductDescription = row.ProductDescription;
                }
                if (row.DepartmentName != null)
                {
                    curr.DepartmentName = row.DepartmentName;
                }
                if (row.DepartmentDescription != null)
                {
                    curr.DepartmentDescription = row.DepartmentDescription;
                }
                if (row.DepartmentTaktTime.HasValue)
                {
                    curr.DepartmentTaktTime = row.DepartmentTaktTime.Value;
                }
                if (row.DepartmentTimeZone != null)
                {
                    curr.DepartmentTimeZone = row.DepartmentTimeZone;
                }
                if (row.ProductRealLeadTime.HasValue)
                {
                    curr.RealLeadTime = row.ProductRealLeadTime.Value;
                }
                if (row.ProductRealWorkingTime.HasValue)
                {
                    curr.RealWorkingTime = row.ProductRealWorkingTime.Value;
                }
                if (row.ProductRealDelay.HasValue)
                {
                    curr.RealDelay = row.ProductRealDelay.Value;
                }
                if (row.ProductRealEndProductionDate.HasValue)
                {
                    curr.ProductionOrderEndProductionDateReal = row.ProductRealEndProductionDate.Value;
                }
                if (row.TaskID.HasValue) { curr.TaskID = row.TaskID.Value; }
                if (row.TaskName != null) { curr.TaskName = row.TaskName; }
                if (row.TaskDescription != null) { curr.TaskDescription = row.TaskDescription; }
                if (row.TaskEarlyStart.HasValue) { curr.TaskEarlyStart = row.TaskEarlyStart.Value; }
                if (row.TaskLateStart.HasValue) { curr.TaskLateStart = row.TaskLateStart.Value; }
                if (row.TaskEarlyFinish.HasValue) { curr.TaskEarlyFinish = row.TaskEarlyFinish.Value; }
                if (row.TaskLateFinish.HasValue) { curr.TaskLateFinish = row.TaskLateFinish.Value; }
                if (row.TaskStatus != null) { curr.TaskStatus = row.TaskStatus[0]; }
                if (row.TaskNumOperators.HasValue) { curr.TaskNumOperators = row.TaskNumOperators.Value; }
                if (row.TaskQuantityOrdered.HasValue) { curr.TaskQuantityOrdered = row.TaskQuantityOrdered.Value; }
                if (row.TaskQuantityProduced.HasValue) { curr.TaskQuantityProduced = row.TaskQuantityProduced.Value; }
                if (row.TaskSetupTimePlanned.HasValue) { curr.TaskPlannedSetupTime = row.TaskSetupTimePlanned.Value; }
                if (row.TaskCycleTimePlanned.HasValue) { curr.TaskPlannedCycleTime = row.TaskCycleTimePlanned.Value; }
                if (row.TaskUnloadTimePlanned.HasValue) { curr.TaskPlannedUnloadTime = row.TaskUnloadTimePlanned.Value; }
                if (row.WorkstationID.HasValue) { curr.WorkstationID = row.WorkstationID.Value; }
                if (row.WorkstationName != null) { curr.WorkstationName = row.WorkstationName; }
                if (row.WorkstationDescription != null) { curr.WorkstationDescription = row.WorkstationDescription; }
                if (row.TaskEndDateReal.HasValue) { curr.TaskRealEndDate = row.TaskEndDateReal.Value; curr.TaskRealEndDateWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskRealEndDate); }
                if (row.TaskLeadTime.HasValue) { curr.TaskRealLeadTime = row.TaskLeadTime.Value; }
                if (row.TaskWorkingTime.HasValue) { curr.TaskRealWorkingTime = row.TaskWorkingTime.Value; }
                if (row.TaskDelay.HasValue) { curr.TaskRealDelay = row.TaskDelay.Value; }
                if (row.TaskOriginalTaskID.HasValue) { curr.TaskOriginalID = row.TaskOriginalTaskID.Value; }
                if (row.TaskOriginalTaskRev.HasValue) { curr.TaskOriginalRev = row.TaskOriginalTaskRev.Value; }
                if (row.TaskOriginalTaskVar.HasValue) { curr.TaskOriginalVar = row.TaskOriginalTaskVar.Value; }
                if (row.TaskPlannedWorkingTime.HasValue) { curr.TaskPlannedWorkingTime = row.TaskPlannedWorkingTime.Value; }
                if (row.id.HasValue) { curr.TaskEventID = row.id.Value; }
                if(row.data.HasValue) { curr.TaskEventTime = row.data.Value; }
                if (row.evento != null) { curr.TaskEventType = row.evento[0]; }
                if (row.user != null) { curr.TaskEventUser = row.user; }

                this.TaskEventsData.Add(curr);
            }


            conn.Close();
        }

        private class ExportTimeSpanRow
        {
            public String user { get; set; }
            public DateTime data { get; set; }
            public String evento { get; set; }
            public int id { get; set; }
            public int task { get; set; }
        }

        public void ExportTimeSpans(Boolean AllEvents)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int timespanid = 0;
            int? maxid = conn.QueryFirstOrDefault<int?>("SELECT MAX(id) FROM taskstimespans");
            if (maxid.HasValue)
            {
                timespanid = maxid.Value + 1;
            }
            string sql = "SELECT user, data, evento, id, task FROM registroeventitaskproduzione";
            object parms = null;
            if (!AllEvents)
            {
                sql += " WHERE data > @data";
                parms = new { data = DateTime.UtcNow.AddMonths(-6).ToString("yyyy-MM-dd") };
            }
            sql += " ORDER BY task, user, data";
            var rows = conn.Query<ExportTimeSpanRow>(sql, parms).ToList();
            int i = 0;
            while (i < rows.Count)
            {
                ExportTimeSpanRow r = rows[i];
                log += "1-Evento: " + r.evento[0] + " " + r.data + "<br />";
                DateTime inizio = r.data;
                String usrI = r.user;
                Char EventoI = r.evento[0];
                int IDEventoI = r.id;
                int taskIdI = r.task;
                if (EventoI == 'I')
                {
                    if (i + 1 < rows.Count)
                    {
                        ExportTimeSpanRow rF = rows[i + 1];
                        log += "2-Evento: " + rF.evento[0] + " " + rF.data + "<br />";
                        String usrF = rF.user;
                        Char EventoF = rF.evento[0];
                        DateTime fine = rF.data;
                        int IDEventoF = rF.id;
                        int taskIdF = rF.task;
                        if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF && taskIdI == taskIdF)
                        {
                            // Checks if start and end events are already in the table
                            int tsid = -1;
                            MySqlConnection conn2 = (new Dati.Dati()).mycon(this.Tenant);
                            conn2.Open();
                            int? evId = conn2.QueryFirstOrDefault<int?>("SELECT id FROM taskstimespans WHERE starteventid=@evi OR starteventid=@evf OR endeventid=@evi OR endeventid=@evf", new { evi = IDEventoI, evf = IDEventoF });
                            if (evId.HasValue && usrF != null)
                            {
                                tsid = evId.Value;
                            }


                            // If events were not already exported, write them in the taskstimespans table
                            if (tsid == -1)
                            {
                                TimeSpan duration = fine - inizio;
                                MySqlTransaction tr = conn2.BeginTransaction();
                                try
                                {
                                    conn2.Execute("INSERT INTO taskstimespans(id, userid, taskid, starteventid, starteventdate, starteventtype," +
                                        "endeventid, endeventdate, endeventtype, duration_sec)" +
                                        " VALUES(@timespanid, @user, @task, @eviID, @eviDate, @eviType, @evfID, @evfDate, @evfType, @duration)",
                                        new { timespanid, user = usrF, task = taskIdF, eviID = IDEventoI, eviDate = inizio.ToString("yyyy-MM-dd HH:mm:ss"), eviType = EventoI, evfID = IDEventoF, evfDate = fine.ToString("yyyy-MM-dd HH:mm:ss"), evfType = EventoF, duration = Math.Floor(duration.TotalSeconds) }, tr);
                                    tr.Commit();
                                    timespanid++;
                                }
                                catch (Exception ex)
                                {
                                    log = ex.Message;
                                    tr.Rollback();
                                }
                            }
                            conn2.Close();

                        }
                        i++;
                    }
                }
                i++;
            }
            conn.Close();
        }
    }

    public struct WorkloadAnalysisStruct
    {
        public DateTime Date;
        public String DateStr;
        public int Year;
        public int Month;
        public int Week;
        public int Day;
        public List<entityWorkload> EntityWorkload;
    }

    public struct entityWorkload
    {
        public int EntityID;
        public String EntityName;
        public double Workload; // hours
    }
}