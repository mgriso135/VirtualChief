using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace KIS.App_Sources
{
    public class Analysis
    {
    }


    // Production Analysis
    public class ProductionHistory
    {
        public String Tenant;

        public List<ProductionHistoryStruct> HistoricData;

        public ProductionHistory(String Tenant)
        {
            this.Tenant = Tenant;
        }

        public void loadProductionHistory()
        {
            this.HistoricData = new List<ProductionHistoryStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT anagraficaclienti.codice AS CustomerID, "
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
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ProductionHistoryStruct curr = new ProductionHistoryStruct();
                curr.DepartmentID = rdr.GetInt32(41);
                KIS.App_Code.Reparto rp = new App_Code.Reparto(curr.DepartmentID);
                if (!rdr.IsDBNull(0))
                {
                    curr.CustomerID = rdr.GetString(0);
                }
                if (!rdr.IsDBNull(1))
                {
                    curr.CustomerName = rdr.GetString(1);
                }
                if (!rdr.IsDBNull(2))
                {
                    curr.CustomerVATNumber = rdr.GetString(2);
                }
                if (!rdr.IsDBNull(3))
                {
                    curr.CustomerCodiceFiscale = rdr.GetString(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    curr.CustomerAddress = rdr.GetString(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    curr.CustomerCity = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    curr.CustomerProvince = rdr.GetString(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    curr.CustomerZipCode = rdr.GetString(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    curr.CustomerCountry = rdr.GetString(8);
                }
                if (!rdr.IsDBNull(9))
                {
                    curr.CustomerPhoneNumber = rdr.GetString(9);
                }
                if (!rdr.IsDBNull(10))
                {
                    curr.CustomerEMail = rdr.GetString(10);
                }
                if (!rdr.IsDBNull(11))
                {
                    curr.CustomerKanbanManaged = rdr.GetBoolean(11);
                }
                if (!rdr.IsDBNull(12))
                {
                    curr.SalesOrderID = rdr.GetInt32(12);
                }
                if (!rdr.IsDBNull(13))
                {
                    curr.SalesOrderYear = rdr.GetInt32(13);
                }
                if (!rdr.IsDBNull(14))
                {
                    curr.SalesOrderCustomer = rdr.GetString(14);
                }
                if (!rdr.IsDBNull(15))
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(15), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(16))
                {
                    curr.SalesOrderNotes = rdr.GetString(16);
                }
                if (!rdr.IsDBNull(17))
                {
                    curr.ProductionOrderID = rdr.GetInt32(17);
                }
                if (!rdr.IsDBNull(18))
                {
                    curr.ProductionOrderYear = rdr.GetInt32(18);
                }
                if (!rdr.IsDBNull(19))
                {
                    curr.ProductionOrderProductTypeID = rdr.GetInt32(19);
                }
                if (!rdr.IsDBNull(20))
                {
                    curr.ProductionOrderProductTypeReview = rdr.GetInt32(20);
                }
                if (!rdr.IsDBNull(21))
                {
                    curr.ProductionOrderProductID = rdr.GetInt32(21);
                }
                if (!rdr.IsDBNull(22))
                {
                    curr.ProductionOrderSerialNumber = rdr.GetString(22);
                }
                if (!rdr.IsDBNull(23))
                {
                    curr.ProductionOrderStatus = rdr.GetChar(23);
                }
                if (!rdr.IsDBNull(24))
                {
                    curr.ProductionOrderDepartmentID = rdr.GetInt32(24);
                }
                if (!rdr.IsDBNull(25))
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(25), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(26))
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(26), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(27))
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(27), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(28))
                {
                    curr.ProductionOrderPlanner = rdr.GetString(28);
                }
                if (!rdr.IsDBNull(29))
                {
                    curr.ProductionOrderQuantityOrdered = rdr.GetInt32(29);
                }
                if (!rdr.IsDBNull(30))
                {
                    curr.ProductionOrderQuantityProduced = rdr.GetInt32(30);
                }
                if (!rdr.IsDBNull(31))
                {
                    curr.ProductionOrderKanbanCardID = rdr.GetString(31);
                }
                if (!rdr.IsDBNull(32))
                {
                    curr.ProductTypeID = rdr.GetInt32(32);
                }
                if (!rdr.IsDBNull(33))
                {
                    curr.ProductTypeReview = rdr.GetInt32(33);
                }
                if (!rdr.IsDBNull(34))
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(34), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(35))
                {
                    curr.ProductTypeName = rdr.GetString(35);
                }
                if (!rdr.IsDBNull(36))
                {
                    curr.ProductTypeDescription = rdr.GetString(36);
                }
                if (!rdr.IsDBNull(37))
                {
                    curr.ProductTypeEnabled = rdr.GetBoolean(37);
                }
                if (!rdr.IsDBNull(38))
                {
                    curr.ProductID = rdr.GetInt32(38);
                }
                if (!rdr.IsDBNull(39))
                {
                    curr.ProductName = rdr.GetString(39);
                }
                if (!rdr.IsDBNull(40))
                {
                    curr.ProductDescription = rdr.GetString(40);
                }
                if (!rdr.IsDBNull(41))
                {
                    curr.DepartmentID = rdr.GetInt32(41);
                }
                if (!rdr.IsDBNull(42))
                {
                    curr.DepartmentName = rdr.GetString(42);
                }
                if (!rdr.IsDBNull(43))
                {
                    curr.DepartmentDescription = rdr.GetString(43);
                }
                if (!rdr.IsDBNull(44))
                {
                    curr.DepartmentTaktTime = rdr.GetDouble(44);
                }
                if (!rdr.IsDBNull(45))
                {
                    curr.DepartmentTimeZone = rdr.GetString(45);
                }
                if (!rdr.IsDBNull(46))
                {
                    curr.RealLeadTime = rdr.GetTimeSpan(46);
                }
                if (!rdr.IsDBNull(47))
                {
                    curr.RealWorkingTime = rdr.GetTimeSpan(47);
                }
                if (!rdr.IsDBNull(48))
                {
                    curr.RealDelay = rdr.GetTimeSpan(48);
                }
                if (!rdr.IsDBNull(49))
                {
                    curr.ProductionOrderEndProductionDateReal = rdr.GetDateTime(49);
                }
                if (!rdr.IsDBNull(50))
                {
                    curr.SalesOrderExternalID = rdr.GetString(50);
                }
                if(!rdr.IsDBNull(51))
                {
                    curr.ProductExternalID = rdr.GetString(51);
                }
                if (!rdr.IsDBNull(52))
                {
                    curr.MeasurementUnit = rdr.GetString(52);
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

        public void loadProductionAnalysis()
        {
            this.AnalysisData = new List<ProductionAnalysisStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT anagraficaclienti.codice AS CustomerID, "
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
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ProductionAnalysisStruct curr = new ProductionAnalysisStruct();
                curr.DepartmentID = rdr.GetInt32(41);
                KIS.App_Code.Reparto rp = new App_Code.Reparto(curr.DepartmentID);
                if (!rdr.IsDBNull(0))
                {
                    curr.CustomerID = rdr.GetString(0);
                }
                if (!rdr.IsDBNull(1))
                {
                    curr.CustomerName = rdr.GetString(1);
                }
                if (!rdr.IsDBNull(2))
                {
                    curr.CustomerVATNumber = rdr.GetString(2);
                }
                if (!rdr.IsDBNull(3))
                {
                    curr.CustomerCodiceFiscale = rdr.GetString(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    curr.CustomerAddress = rdr.GetString(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    curr.CustomerCity = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    curr.CustomerProvince = rdr.GetString(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    curr.CustomerZipCode = rdr.GetString(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    curr.CustomerCountry = rdr.GetString(8);
                }
                if (!rdr.IsDBNull(9))
                {
                    curr.CustomerPhoneNumber = rdr.GetString(9);
                }
                if (!rdr.IsDBNull(10))
                {
                    curr.CustomerEMail = rdr.GetString(10);
                }
                if (!rdr.IsDBNull(11))
                {
                    curr.CustomerKanbanManaged = rdr.GetBoolean(11);
                }
                if (!rdr.IsDBNull(12))
                {
                    curr.SalesOrderID = rdr.GetInt32(12);
                }
                if (!rdr.IsDBNull(13))
                {
                    curr.SalesOrderYear = rdr.GetInt32(13);
                }
                if (!rdr.IsDBNull(14))
                {
                    curr.SalesOrderCustomer = rdr.GetString(14);
                }
                if (!rdr.IsDBNull(15))
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(15), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(16))
                {
                    curr.SalesOrderNotes = rdr.GetString(16);
                }
                if (!rdr.IsDBNull(17))
                {
                    curr.ProductionOrderID = rdr.GetInt32(17);
                }
                if (!rdr.IsDBNull(18))
                {
                    curr.ProductionOrderYear = rdr.GetInt32(18);
                }
                if (!rdr.IsDBNull(19))
                {
                    curr.ProductionOrderProductTypeID = rdr.GetInt32(19);
                }
                if (!rdr.IsDBNull(20))
                {
                    curr.ProductionOrderProductTypeReview = rdr.GetInt32(20);
                }
                if (!rdr.IsDBNull(21))
                {
                    curr.ProductionOrderProductID = rdr.GetInt32(21);
                }
                if (!rdr.IsDBNull(22))
                {
                    curr.ProductionOrderSerialNumber = rdr.GetString(22);
                }
                if (!rdr.IsDBNull(23))
                {
                    curr.ProductionOrderStatus = rdr.GetChar(23);
                }
                if (!rdr.IsDBNull(24))
                {
                    curr.ProductionOrderDepartmentID = rdr.GetInt32(24);
                }
                if (!rdr.IsDBNull(25))
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(25), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(26))
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(26), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(27))
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(27), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(28))
                {
                    curr.ProductionOrderPlanner = rdr.GetString(28);
                }
                if (!rdr.IsDBNull(29))
                {
                    curr.ProductionOrderQuantityOrdered = rdr.GetInt32(29);
                }
                if (!rdr.IsDBNull(30))
                {
                    curr.ProductionOrderQuantityProduced = rdr.GetInt32(30);
                }
                if (!rdr.IsDBNull(31))
                {
                    curr.ProductionOrderKanbanCardID = rdr.GetString(31);
                }
                if (!rdr.IsDBNull(32))
                {
                    curr.ProductTypeID = rdr.GetInt32(32);
                }
                if (!rdr.IsDBNull(33))
                {
                    curr.ProductTypeReview = rdr.GetInt32(33);
                }
                if (!rdr.IsDBNull(34))
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(34), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(35))
                {
                    curr.ProductTypeName = rdr.GetString(35);
                }
                if (!rdr.IsDBNull(36))
                {
                    curr.ProductTypeDescription = rdr.GetString(36);
                }
                if (!rdr.IsDBNull(37))
                {
                    curr.ProductTypeEnabled = rdr.GetBoolean(37);
                }
                if (!rdr.IsDBNull(38))
                {
                    curr.ProductID = rdr.GetInt32(38);
                }
                if (!rdr.IsDBNull(39))
                {
                    curr.ProductName = rdr.GetString(39);
                }
                if (!rdr.IsDBNull(40))
                {
                    curr.ProductDescription = rdr.GetString(40);
                }
                if (!rdr.IsDBNull(41))
                {
                    curr.DepartmentID = rdr.GetInt32(41);
                }
                if (!rdr.IsDBNull(42))
                {
                    curr.DepartmentName = rdr.GetString(42);
                }
                if (!rdr.IsDBNull(43))
                {
                    curr.DepartmentDescription = rdr.GetString(43);
                }
                if (!rdr.IsDBNull(44))
                {
                    curr.DepartmentTaktTime = rdr.GetDouble(44);
                }
                if (!rdr.IsDBNull(45))
                {
                    curr.DepartmentTimeZone = rdr.GetString(45);
                }
                if (!rdr.IsDBNull(46))
                {
                    curr.RealLeadTime = rdr.GetTimeSpan(46);
                }
                if (!rdr.IsDBNull(47))
                {
                    curr.RealWorkingTime = rdr.GetTimeSpan(47);
                }
                if (!rdr.IsDBNull(48))
                {
                    curr.RealDelay = rdr.GetTimeSpan(48);
                }
                if (!rdr.IsDBNull(49))
                {
                    curr.ProductionOrderEndProductionDateReal = rdr.GetDateTime(49);
                    curr.ProductionOrderEndProductionDateRealWeek = GetWeekOfTheYear(rdr.GetDateTime(49));
                }
                if (!rdr.IsDBNull(50))
                {
                    curr.SalesOrderExternalID = rdr.GetString(50);
                }
                if(!rdr.IsDBNull(51))
                {
                    curr.PlannedWorkingTime = rdr.GetTimeSpan(51);
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

        public void loadTaskProductionHistory()
        {
            this.TaskHistoricData = new List<TaskProductionHistoryStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT anagraficaclienti.codice AS CustomerID, "
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


            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                TaskProductionHistoryStruct curr = new TaskProductionHistoryStruct();
                curr.DepartmentID = rdr.GetInt32(41);
                KIS.App_Code.Reparto rp = new App_Code.Reparto(curr.DepartmentID);
                if (!rdr.IsDBNull(0))
                {
                    curr.CustomerID = rdr.GetString(0);
                }
                if (!rdr.IsDBNull(1))
                {
                    curr.CustomerName = rdr.GetString(1);
                }
                if (!rdr.IsDBNull(2))
                {
                    curr.CustomerVATNumber = rdr.GetString(2);
                }
                if (!rdr.IsDBNull(3))
                {
                    curr.CustomerCodiceFiscale = rdr.GetString(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    curr.CustomerAddress = rdr.GetString(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    curr.CustomerCity = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    curr.CustomerProvince = rdr.GetString(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    curr.CustomerZipCode = rdr.GetString(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    curr.CustomerCountry = rdr.GetString(8);
                }
                if (!rdr.IsDBNull(9))
                {
                    curr.CustomerPhoneNumber = rdr.GetString(9);
                }
                if (!rdr.IsDBNull(10))
                {
                    curr.CustomerEMail = rdr.GetString(10);
                }
                if (!rdr.IsDBNull(11))
                {
                    curr.CustomerKanbanManaged = rdr.GetBoolean(11);
                }
                if (!rdr.IsDBNull(12))
                {
                    curr.SalesOrderID = rdr.GetInt32(12);
                }
                if (!rdr.IsDBNull(13))
                {
                    curr.SalesOrderYear = rdr.GetInt32(13);
                }
                if (!rdr.IsDBNull(14))
                {
                    curr.SalesOrderCustomer = rdr.GetString(14);
                }
                if (!rdr.IsDBNull(15))
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(15), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(16))
                {
                    curr.SalesOrderNotes = rdr.GetString(16);
                }
                if (!rdr.IsDBNull(17))
                {
                    curr.ProductionOrderID = rdr.GetInt32(17);
                }
                if (!rdr.IsDBNull(18))
                {
                    curr.ProductionOrderYear = rdr.GetInt32(18);
                }
                if (!rdr.IsDBNull(19))
                {
                    curr.ProductionOrderProductTypeID = rdr.GetInt32(19);
                }
                if (!rdr.IsDBNull(20))
                {
                    curr.ProductionOrderProductTypeReview = rdr.GetInt32(20);
                }
                if (!rdr.IsDBNull(21))
                {
                    curr.ProductionOrderProductID = rdr.GetInt32(21);
                }
                if (!rdr.IsDBNull(22))
                {
                    curr.ProductionOrderSerialNumber = rdr.GetString(22);
                }
                if (!rdr.IsDBNull(23))
                {
                    curr.ProductionOrderStatus = rdr.GetChar(23);
                }
                if (!rdr.IsDBNull(24))
                {
                    curr.ProductionOrderDepartmentID = rdr.GetInt32(24);
                }
                if (!rdr.IsDBNull(25))
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(25), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(26))
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(26), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(27))
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(27), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(28))
                {
                    curr.ProductionOrderPlanner = rdr.GetString(28);
                }
                if (!rdr.IsDBNull(29))
                {
                    curr.ProductionOrderQuantityOrdered = rdr.GetInt32(29);
                }
                if (!rdr.IsDBNull(30))
                {
                    curr.ProductionOrderQuantityProduced = rdr.GetInt32(30);
                }
                if (!rdr.IsDBNull(31))
                {
                    curr.ProductionOrderKanbanCardID = rdr.GetString(31);
                }
                if (!rdr.IsDBNull(32))
                {
                    curr.ProductTypeID = rdr.GetInt32(32);
                }
                if (!rdr.IsDBNull(33))
                {
                    curr.ProductTypeReview = rdr.GetInt32(33);
                }
                if (!rdr.IsDBNull(34))
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(34), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(35))
                {
                    curr.ProductTypeName = rdr.GetString(35);
                }
                if (!rdr.IsDBNull(36))
                {
                    curr.ProductTypeDescription = rdr.GetString(36);
                }
                if (!rdr.IsDBNull(37))
                {
                    curr.ProductTypeEnabled = rdr.GetBoolean(37);
                }
                if (!rdr.IsDBNull(38))
                {
                    curr.ProductID = rdr.GetInt32(38);
                }
                if (!rdr.IsDBNull(39))
                {
                    curr.ProductName = rdr.GetString(39);
                }
                if (!rdr.IsDBNull(40))
                {
                    curr.ProductDescription = rdr.GetString(40);
                }
                if (!rdr.IsDBNull(41))
                {
                    curr.DepartmentID = rdr.GetInt32(41);
                }
                if (!rdr.IsDBNull(42))
                {
                    curr.DepartmentName = rdr.GetString(42);
                }
                if (!rdr.IsDBNull(43))
                {
                    curr.DepartmentDescription = rdr.GetString(43);
                }
                if (!rdr.IsDBNull(44))
                {
                    curr.DepartmentTaktTime = rdr.GetDouble(44);
                }
                if (!rdr.IsDBNull(45))
                {
                    curr.DepartmentTimeZone = rdr.GetString(45);
                }
                if (!rdr.IsDBNull(46))
                {
                    curr.RealLeadTime = rdr.GetTimeSpan(46);
                }
                if (!rdr.IsDBNull(47))
                {
                    curr.RealWorkingTime = rdr.GetTimeSpan(47);
                }
                if (!rdr.IsDBNull(48))
                {
                    curr.RealDelay = rdr.GetTimeSpan(48);
                }
                if (!rdr.IsDBNull(49))
                {
                    curr.ProductionOrderEndProductionDateReal = rdr.GetDateTime(49);
                }
                if(!rdr.IsDBNull(50)) { curr.TaskID = rdr.GetInt32(50); }
                if (!rdr.IsDBNull(51)) { curr.TaskName = rdr.GetString(51); }
                if (!rdr.IsDBNull(52)) { curr.TaskDescription = rdr.GetString(52); }
                if (!rdr.IsDBNull(53)) { curr.TaskEarlyStart = rdr.GetDateTime(53); }
                if (!rdr.IsDBNull(54)) { curr.TaskLateStart = rdr.GetDateTime(54); }
                if (!rdr.IsDBNull(55)) { curr.TaskEarlyFinish = rdr.GetDateTime(55); }
                if (!rdr.IsDBNull(56)) { curr.TaskLateFinish = rdr.GetDateTime(56); curr.TaskLateFinishWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskLateFinish); }
                if (!rdr.IsDBNull(57)) { curr.TaskStatus = rdr.GetChar(57); }
                if (!rdr.IsDBNull(58)) { curr.TaskNumOperators = rdr.GetInt32(58); }
                if (!rdr.IsDBNull(59)) { curr.TaskQuantityOrdered = rdr.GetDouble(59); }
                if (!rdr.IsDBNull(60)) { curr.TaskQuantityProduced = rdr.GetDouble(60); }
                if (!rdr.IsDBNull(61)) { curr.TaskPlannedSetupTime = rdr.GetTimeSpan(61); }
                if (!rdr.IsDBNull(62)) { curr.TaskPlannedCycleTime = rdr.GetTimeSpan(62); }
                if (!rdr.IsDBNull(63)) { curr.TaskPlannedUnloadTime = rdr.GetTimeSpan(63); }
                if (!rdr.IsDBNull(64)) { curr.WorkstationID = rdr.GetInt32(64); }
                if (!rdr.IsDBNull(65)) { curr.WorkstationName = rdr.GetString(65); }
                if (!rdr.IsDBNull(66)) { curr.WorkstationDescription = rdr.GetString(66); }
                if (!rdr.IsDBNull(67)) { curr.TaskRealEndDate = rdr.GetDateTime(67); curr.TaskRealEndDateWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskRealEndDate); }
                if (!rdr.IsDBNull(68)) { curr.TaskRealLeadTime = rdr.GetTimeSpan(68); }
                if (!rdr.IsDBNull(69)) { curr.TaskRealWorkingTime = rdr.GetTimeSpan(69); }
                if (!rdr.IsDBNull(70)) { curr.TaskRealDelay = rdr.GetTimeSpan(70); }
                if (!rdr.IsDBNull(71)) { curr.TaskOriginalID = rdr.GetInt32(71); }
                if (!rdr.IsDBNull(72)) { curr.TaskOriginalRev = rdr.GetInt32(72); }
                if (!rdr.IsDBNull(73)) { curr.TaskOriginalVar = rdr.GetInt32(73); }
                if (!rdr.IsDBNull(74)) { curr.TaskPlannedWorkingTime = rdr.GetTimeSpan(74); }

                this.TaskHistoricData.Add(curr);
            }
            conn.Close();
        }

        public void loadTasksProductionWorkload()
        {
            this.TaskHistoricData = new List<TaskProductionHistoryStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT anagraficaclienti.codice AS CustomerID, "
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


            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                TaskProductionHistoryStruct curr = new TaskProductionHistoryStruct();
                curr.DepartmentID = rdr.GetInt32(41);
                KIS.App_Code.Reparto rp = new App_Code.Reparto(curr.DepartmentID);
                if (!rdr.IsDBNull(0))
                {
                    curr.CustomerID = rdr.GetString(0);
                }
                if (!rdr.IsDBNull(1))
                {
                    curr.CustomerName = rdr.GetString(1);
                }
                if (!rdr.IsDBNull(2))
                {
                    curr.CustomerVATNumber = rdr.GetString(2);
                }
                if (!rdr.IsDBNull(3))
                {
                    curr.CustomerCodiceFiscale = rdr.GetString(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    curr.CustomerAddress = rdr.GetString(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    curr.CustomerCity = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    curr.CustomerProvince = rdr.GetString(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    curr.CustomerZipCode = rdr.GetString(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    curr.CustomerCountry = rdr.GetString(8);
                }
                if (!rdr.IsDBNull(9))
                {
                    curr.CustomerPhoneNumber = rdr.GetString(9);
                }
                if (!rdr.IsDBNull(10))
                {
                    curr.CustomerEMail = rdr.GetString(10);
                }
                if (!rdr.IsDBNull(11))
                {
                    curr.CustomerKanbanManaged = rdr.GetBoolean(11);
                }
                if (!rdr.IsDBNull(12))
                {
                    curr.SalesOrderID = rdr.GetInt32(12);
                }
                if (!rdr.IsDBNull(13))
                {
                    curr.SalesOrderYear = rdr.GetInt32(13);
                }
                if (!rdr.IsDBNull(14))
                {
                    curr.SalesOrderCustomer = rdr.GetString(14);
                }
                if (!rdr.IsDBNull(15))
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(15), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(16))
                {
                    curr.SalesOrderNotes = rdr.GetString(16);
                }
                if (!rdr.IsDBNull(17))
                {
                    curr.ProductionOrderID = rdr.GetInt32(17);
                }
                if (!rdr.IsDBNull(18))
                {
                    curr.ProductionOrderYear = rdr.GetInt32(18);
                }
                if (!rdr.IsDBNull(19))
                {
                    curr.ProductionOrderProductTypeID = rdr.GetInt32(19);
                }
                if (!rdr.IsDBNull(20))
                {
                    curr.ProductionOrderProductTypeReview = rdr.GetInt32(20);
                }
                if (!rdr.IsDBNull(21))
                {
                    curr.ProductionOrderProductID = rdr.GetInt32(21);
                }
                if (!rdr.IsDBNull(22))
                {
                    curr.ProductionOrderSerialNumber = rdr.GetString(22);
                }
                if (!rdr.IsDBNull(23))
                {
                    curr.ProductionOrderStatus = rdr.GetChar(23);
                }
                if (!rdr.IsDBNull(24))
                {
                    curr.ProductionOrderDepartmentID = rdr.GetInt32(24);
                }
                if (!rdr.IsDBNull(25))
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(25), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(26))
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(26), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(27))
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(27), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(28))
                {
                    curr.ProductionOrderPlanner = rdr.GetString(28);
                }
                if (!rdr.IsDBNull(29))
                {
                    curr.ProductionOrderQuantityOrdered = rdr.GetInt32(29);
                }
                if (!rdr.IsDBNull(30))
                {
                    curr.ProductionOrderQuantityProduced = rdr.GetInt32(30);
                }
                if (!rdr.IsDBNull(31))
                {
                    curr.ProductionOrderKanbanCardID = rdr.GetString(31);
                }
                if (!rdr.IsDBNull(32))
                {
                    curr.ProductTypeID = rdr.GetInt32(32);
                }
                if (!rdr.IsDBNull(33))
                {
                    curr.ProductTypeReview = rdr.GetInt32(33);
                }
                if (!rdr.IsDBNull(34))
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(34), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(35))
                {
                    curr.ProductTypeName = rdr.GetString(35);
                }
                if (!rdr.IsDBNull(36))
                {
                    curr.ProductTypeDescription = rdr.GetString(36);
                }
                if (!rdr.IsDBNull(37))
                {
                    curr.ProductTypeEnabled = rdr.GetBoolean(37);
                }
                if (!rdr.IsDBNull(38))
                {
                    curr.ProductID = rdr.GetInt32(38);
                }
                if (!rdr.IsDBNull(39))
                {
                    curr.ProductName = rdr.GetString(39);
                }
                if (!rdr.IsDBNull(40))
                {
                    curr.ProductDescription = rdr.GetString(40);
                }
                if (!rdr.IsDBNull(41))
                {
                    curr.DepartmentID = rdr.GetInt32(41);
                }
                if (!rdr.IsDBNull(42))
                {
                    curr.DepartmentName = rdr.GetString(42);
                }
                if (!rdr.IsDBNull(43))
                {
                    curr.DepartmentDescription = rdr.GetString(43);
                }
                if (!rdr.IsDBNull(44))
                {
                    curr.DepartmentTaktTime = rdr.GetDouble(44);
                }
                if (!rdr.IsDBNull(45))
                {
                    curr.DepartmentTimeZone = rdr.GetString(45);
                }
                if (!rdr.IsDBNull(46))
                {
                    curr.RealLeadTime = rdr.GetTimeSpan(46);
                }
                if (!rdr.IsDBNull(47))
                {
                    curr.RealWorkingTime = rdr.GetTimeSpan(47);
                }
                if (!rdr.IsDBNull(48))
                {
                    curr.RealDelay = rdr.GetTimeSpan(48);
                }
                if (!rdr.IsDBNull(49))
                {
                    curr.ProductionOrderEndProductionDateReal = rdr.GetDateTime(49);
                }
                if (!rdr.IsDBNull(50)) { curr.TaskID = rdr.GetInt32(50); }
                if (!rdr.IsDBNull(51)) { curr.TaskName = rdr.GetString(51); }
                if (!rdr.IsDBNull(52)) { curr.TaskDescription = rdr.GetString(52); }
                if (!rdr.IsDBNull(53)) { curr.TaskEarlyStart = rdr.GetDateTime(53); }
                if (!rdr.IsDBNull(54)) { curr.TaskLateStart = rdr.GetDateTime(54); }
                if (!rdr.IsDBNull(55)) { curr.TaskEarlyFinish = rdr.GetDateTime(55); }
                if (!rdr.IsDBNull(56)) { curr.TaskLateFinish = rdr.GetDateTime(56); curr.TaskLateFinishWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskLateFinish); }
                if (!rdr.IsDBNull(57)) { curr.TaskStatus = rdr.GetChar(57); }
                if (!rdr.IsDBNull(58)) { curr.TaskNumOperators = rdr.GetInt32(58); }
                if (!rdr.IsDBNull(59)) { curr.TaskQuantityOrdered = rdr.GetDouble(59); }
                if (!rdr.IsDBNull(60)) { curr.TaskQuantityProduced = rdr.GetDouble(60); }
                if (!rdr.IsDBNull(61)) { curr.TaskPlannedSetupTime = rdr.GetTimeSpan(61); }
                if (!rdr.IsDBNull(62)) { curr.TaskPlannedCycleTime = rdr.GetTimeSpan(62); }
                if (!rdr.IsDBNull(63)) { curr.TaskPlannedUnloadTime = rdr.GetTimeSpan(63); }
                if (!rdr.IsDBNull(64)) { curr.WorkstationID = rdr.GetInt32(64); }
                if (!rdr.IsDBNull(65)) { curr.WorkstationName = rdr.GetString(65); }
                if (!rdr.IsDBNull(66)) { curr.WorkstationDescription = rdr.GetString(66); }
                if (!rdr.IsDBNull(67)) { curr.TaskRealEndDate = rdr.GetDateTime(67); curr.TaskRealEndDateWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskRealEndDate); }
                if (!rdr.IsDBNull(68)) { curr.TaskRealLeadTime = rdr.GetTimeSpan(68); }
                if (!rdr.IsDBNull(69)) { curr.TaskRealWorkingTime = rdr.GetTimeSpan(69); }
                if (!rdr.IsDBNull(70)) { curr.TaskRealDelay = rdr.GetTimeSpan(70); }
                if (!rdr.IsDBNull(71)) { curr.TaskOriginalID = rdr.GetInt32(71); }
                if (!rdr.IsDBNull(72)) { curr.TaskOriginalRev = rdr.GetInt32(72); }
                if (!rdr.IsDBNull(73)) { curr.TaskOriginalVar = rdr.GetInt32(73); }
                if (!rdr.IsDBNull(74)) { curr.TaskPlannedWorkingTime = rdr.GetTimeSpan(74); }

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
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT "
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

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                TaskEventStruct curr = new TaskEventStruct();

                curr.DepartmentID = rdr.GetInt32(41);
                KIS.App_Code.Reparto rp = new App_Code.Reparto(curr.DepartmentID);
                if (!rdr.IsDBNull(0))
                {
                    curr.CustomerID = rdr.GetString(0);
                }
                if (!rdr.IsDBNull(1))
                {
                    curr.CustomerName = rdr.GetString(1);
                }
                if (!rdr.IsDBNull(2))
                {
                    curr.CustomerVATNumber = rdr.GetString(2);
                }
                if (!rdr.IsDBNull(3))
                {
                    curr.CustomerCodiceFiscale = rdr.GetString(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    curr.CustomerAddress = rdr.GetString(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    curr.CustomerCity = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    curr.CustomerProvince = rdr.GetString(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    curr.CustomerZipCode = rdr.GetString(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    curr.CustomerCountry = rdr.GetString(8);
                }
                if (!rdr.IsDBNull(9))
                {
                    curr.CustomerPhoneNumber = rdr.GetString(9);
                }
                if (!rdr.IsDBNull(10))
                {
                    curr.CustomerEMail = rdr.GetString(10);
                }
                if (!rdr.IsDBNull(11))
                {
                    curr.CustomerKanbanManaged = rdr.GetBoolean(11);
                }
                if (!rdr.IsDBNull(12))
                {
                    curr.SalesOrderID = rdr.GetInt32(12);
                }
                if (!rdr.IsDBNull(13))
                {
                    curr.SalesOrderYear = rdr.GetInt32(13);
                }
                if (!rdr.IsDBNull(14))
                {
                    curr.SalesOrderCustomer = rdr.GetString(14);
                }
                if (!rdr.IsDBNull(15))
                {
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(15), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(16))
                {
                    curr.SalesOrderNotes = rdr.GetString(16);
                }
                if (!rdr.IsDBNull(17))
                {
                    curr.ProductionOrderID = rdr.GetInt32(17);
                }
                if (!rdr.IsDBNull(18))
                {
                    curr.ProductionOrderYear = rdr.GetInt32(18);
                }
                if (!rdr.IsDBNull(19))
                {
                    curr.ProductionOrderProductTypeID = rdr.GetInt32(19);
                }
                if (!rdr.IsDBNull(20))
                {
                    curr.ProductionOrderProductTypeReview = rdr.GetInt32(20);
                }
                if (!rdr.IsDBNull(21))
                {
                    curr.ProductionOrderProductID = rdr.GetInt32(21);
                }
                if (!rdr.IsDBNull(22))
                {
                    curr.ProductionOrderSerialNumber = rdr.GetString(22);
                }
                if (!rdr.IsDBNull(23))
                {
                    curr.ProductionOrderStatus = rdr.GetChar(23);
                }
                if (!rdr.IsDBNull(24))
                {
                    curr.ProductionOrderDepartmentID = rdr.GetInt32(24);
                }
                if (!rdr.IsDBNull(25))
                {
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(25), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(26))
                {
                    curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(26), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(27))
                {
                    curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(27), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(28))
                {
                    curr.ProductionOrderPlanner = rdr.GetString(28);
                }
                if (!rdr.IsDBNull(29))
                {
                    curr.ProductionOrderQuantityOrdered = rdr.GetInt32(29);
                }
                if (!rdr.IsDBNull(30))
                {
                    curr.ProductionOrderQuantityProduced = rdr.GetInt32(30);
                }
                if (!rdr.IsDBNull(31))
                {
                    curr.ProductionOrderKanbanCardID = rdr.GetString(31);
                }
                if (!rdr.IsDBNull(32))
                {
                    curr.ProductTypeID = rdr.GetInt32(32);
                }
                if (!rdr.IsDBNull(33))
                {
                    curr.ProductTypeReview = rdr.GetInt32(33);
                }
                if (!rdr.IsDBNull(34))
                {
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(rdr.GetDateTime(34), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(35))
                {
                    curr.ProductTypeName = rdr.GetString(35);
                }
                if (!rdr.IsDBNull(36))
                {
                    curr.ProductTypeDescription = rdr.GetString(36);
                }
                if (!rdr.IsDBNull(37))
                {
                    curr.ProductTypeEnabled = rdr.GetBoolean(37);
                }
                if (!rdr.IsDBNull(38))
                {
                    curr.ProductID = rdr.GetInt32(38);
                }
                if (!rdr.IsDBNull(39))
                {
                    curr.ProductName = rdr.GetString(39);
                }
                if (!rdr.IsDBNull(40))
                {
                    curr.ProductDescription = rdr.GetString(40);
                }
                if (!rdr.IsDBNull(41))
                {
                    curr.DepartmentID = rdr.GetInt32(41);
                }
                if (!rdr.IsDBNull(42))
                {
                    curr.DepartmentName = rdr.GetString(42);
                }
                if (!rdr.IsDBNull(43))
                {
                    curr.DepartmentDescription = rdr.GetString(43);
                }
                if (!rdr.IsDBNull(44))
                {
                    curr.DepartmentTaktTime = rdr.GetDouble(44);
                }
                if (!rdr.IsDBNull(45))
                {
                    curr.DepartmentTimeZone = rdr.GetString(45);
                }
                if (!rdr.IsDBNull(46))
                {
                    curr.RealLeadTime = rdr.GetTimeSpan(46);
                }
                if (!rdr.IsDBNull(47))
                {
                    curr.RealWorkingTime = rdr.GetTimeSpan(47);
                }
                if (!rdr.IsDBNull(48))
                {
                    curr.RealDelay = rdr.GetTimeSpan(48);
                }
                if (!rdr.IsDBNull(49))
                {
                    curr.ProductionOrderEndProductionDateReal = rdr.GetDateTime(49);
                }
                if (!rdr.IsDBNull(50)) { curr.TaskID = rdr.GetInt32(50); }
                if (!rdr.IsDBNull(51)) { curr.TaskName = rdr.GetString(51); }
                if (!rdr.IsDBNull(52)) { curr.TaskDescription = rdr.GetString(52); }
                if (!rdr.IsDBNull(53)) { curr.TaskEarlyStart = rdr.GetDateTime(53); }
                if (!rdr.IsDBNull(54)) { curr.TaskLateStart = rdr.GetDateTime(54); }
                if (!rdr.IsDBNull(55)) { curr.TaskEarlyFinish = rdr.GetDateTime(55); }
                if (!rdr.IsDBNull(56)) { curr.TaskLateFinish = rdr.GetDateTime(56); }
                if (!rdr.IsDBNull(57)) { curr.TaskStatus = rdr.GetChar(57); }
                if (!rdr.IsDBNull(58)) { curr.TaskNumOperators = rdr.GetInt32(58); }
                if (!rdr.IsDBNull(59)) { curr.TaskQuantityOrdered = rdr.GetDouble(59); }
                if (!rdr.IsDBNull(60)) { curr.TaskQuantityProduced = rdr.GetDouble(60); }
                if (!rdr.IsDBNull(61)) { curr.TaskPlannedSetupTime = rdr.GetTimeSpan(61); }
                if (!rdr.IsDBNull(62)) { curr.TaskPlannedCycleTime = rdr.GetTimeSpan(62); }
                if (!rdr.IsDBNull(63)) { curr.TaskPlannedUnloadTime = rdr.GetTimeSpan(63); }
                if (!rdr.IsDBNull(64)) { curr.WorkstationID = rdr.GetInt32(64); }
                if (!rdr.IsDBNull(65)) { curr.WorkstationName = rdr.GetString(65); }
                if (!rdr.IsDBNull(66)) { curr.WorkstationDescription = rdr.GetString(66); }
                if (!rdr.IsDBNull(67)) { curr.TaskRealEndDate = rdr.GetDateTime(67); curr.TaskRealEndDateWeek = Dati.Utilities.GetWeekOfTheYear(curr.TaskRealEndDate); }
                if (!rdr.IsDBNull(68)) { curr.TaskRealLeadTime = rdr.GetTimeSpan(68); }
                if (!rdr.IsDBNull(69)) { curr.TaskRealWorkingTime = rdr.GetTimeSpan(69); }
                if (!rdr.IsDBNull(70)) { curr.TaskRealDelay = rdr.GetTimeSpan(70); }
                if (!rdr.IsDBNull(71)) { curr.TaskOriginalID = rdr.GetInt32(71); }
                if (!rdr.IsDBNull(72)) { curr.TaskOriginalRev = rdr.GetInt32(72); }
                if (!rdr.IsDBNull(73)) { curr.TaskOriginalVar = rdr.GetInt32(73); }
                if (!rdr.IsDBNull(74)) { curr.TaskPlannedWorkingTime = rdr.GetTimeSpan(74); }
                if (!rdr.IsDBNull(75)) { curr.TaskEventID = rdr.GetInt32(75); }
                if(!rdr.IsDBNull(76)) { curr.TaskEventTime = rdr.GetDateTime(76); }
                if (!rdr.IsDBNull(77)) { curr.TaskEventType = rdr.GetChar(77); }
                if (!rdr.IsDBNull(78)) { curr.TaskEventUser = rdr.GetString(78); }

                this.TaskEventsData.Add(curr);
            }


            conn.Close();
        }

        public void ExportTimeSpans(Boolean AllEvents)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            /*int maxstartev = 0;
            cmd.CommandText = "SELECT MAX(starteventid) FROM taskstimespans";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                maxstartev = rdr.GetInt32(0);
            }
            rdr.Close();*/
            int timespanid = 0;
            cmd.CommandText = "SELECT MAX(id) FROM taskstimespans";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                timespanid = rdr.GetInt32(0)+1;
            }
            rdr.Close();
            cmd.CommandText = "SELECT user, data, evento, id, task FROM registroeventitaskproduzione";
            if(!AllEvents)
            {
                cmd.CommandText += " WHERE data > '" + DateTime.UtcNow.AddMonths(-6).ToString("yyyy-MM-dd") + "'";
            }
            cmd.CommandText += " ORDER BY task, user, data";
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                DateTime inizio = rdr.GetDateTime(1);
                String usrI = rdr.GetString(0);
                Char EventoI = rdr.GetChar(2);
                int IDEventoI = rdr.GetInt32(3);
                int taskIdI = rdr.GetInt32(4);
                if (EventoI == 'I')
                {
                    if (rdr.Read())
                    {
                        log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                        String usrF = rdr.GetString(0);
                        Char EventoF = rdr.GetChar(2);
                        DateTime fine = rdr.GetDateTime(1);
                        int IDEventoF = rdr.GetInt32(3);
                        int taskIdF = rdr.GetInt32(4);
                        if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF && taskIdI == taskIdF)
                        {
                            // Checks if start and end events are already in the table
                            int tsid = -1;
                            MySqlConnection conn2 = (new Dati.Dati()).mycon(this.Tenant);
                            conn2.Open();
                            MySqlCommand cmd2 = conn2.CreateCommand();
                            cmd2.CommandText = "SELECT id FROM taskstimespans WHERE starteventid=@evi OR starteventid=@evf OR endeventid=@evi OR endeventid=@evf";
                            cmd2.Parameters.AddWithValue("@evi", IDEventoI);
                            cmd2.Parameters.AddWithValue("@evf", IDEventoF);
                            MySqlDataReader rdr2 = cmd2.ExecuteReader();
                            if (rdr2.Read() && !rdr.IsDBNull(0))
                            {
                                tsid = rdr2.GetInt32(0);
                            }
                            rdr2.Close();


                            // If events were not already exported, write them in the taskstimespans table
                            if (tsid == -1)
                            {
                                MySqlCommand cmd3 = conn2.CreateCommand();
                                cmd3.CommandText = "INSERT INTO taskstimespans(id, userid, taskid, starteventid, starteventdate, starteventtype," +
                                    "endeventid, endeventdate, endeventtype, duration_sec)" +
                                    " VALUES(@timespanid, @user, @task, @eviID, @eviDate, @eviType, @evfID, @evfDate, @evfType, @duration)";
                                cmd3.Parameters.AddWithValue("@timespanid", timespanid);
                                cmd3.Parameters.AddWithValue("@user", usrF);
                                cmd3.Parameters.AddWithValue("@task", taskIdF);
                                cmd3.Parameters.AddWithValue("@eviID", IDEventoI);
                                cmd3.Parameters.AddWithValue("@eviDate", inizio.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmd3.Parameters.AddWithValue("@eviType", EventoI);
                                cmd3.Parameters.AddWithValue("@evfID", IDEventoF);
                                cmd3.Parameters.AddWithValue("@evfDate", fine.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmd3.Parameters.AddWithValue("@evfType", EventoF);
                                TimeSpan duration = fine - inizio;
                                cmd3.Parameters.AddWithValue("@duration", Math.Floor(duration.TotalSeconds));
                                MySqlTransaction tr = conn2.BeginTransaction();
                                cmd3.Transaction = tr;
                                try
                                {
                                    cmd3.ExecuteNonQuery();
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
                    }
                }

            }
            rdr.Close();
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