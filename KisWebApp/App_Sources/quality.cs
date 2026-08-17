/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

/*  Copyright of Matteo Griso.
 *  Develop started on 29/08/2017 in Berazategui, Buenos Aires, Argentina
 */

using System;
using System.Collections.Generic;
using System.Web;
using MySql.Data.MySqlClient;
using Dapper;
using KIS.App_Code;
using System.IO;


namespace KIS.App_Sources
{
    public class NonCompliance
    {
        protected String Tenant;

        public String log;

        private class NonComplianceRow
        {
            public int ID { get; set; }
            public int Year { get; set; }
            public int Quantity { get; set; }
            public DateTime? OpeningDate { get; set; }
            public String User { get; set; }
            public String Description { get; set; }
            public String ImmediateAction { get; set; }
            public double Cost { get; set; }
            public char Status { get; set; }
            public DateTime? ClosureDate { get; set; }
        }

        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private int _Year;
        public int Year
        {
            get { return this._Year; }
        }

        private int _Quantity;
        public int Quantity
        {
            get
            {
                return this._Quantity;
            }
            set
            {
                if(value > 0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliances SET Quantity = @pQuantity"
                    + " WHERE ID = @pID AND Year = @pYear";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pQuantity = value, pID = this.ID, pYear = this.Year }, tr);
                        tr.Commit();
                        this._Quantity = value;
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                conn.Close();
                }
            }
        }

        private DateTime _OpeningDate;
        public DateTime OpeningDate
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._OpeningDate, fuso.tzFusoOrario);
            }
            set
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE NonCompliances SET OpeningDate = @pOpeningDate"
                        + " WHERE ID = @pID AND Year = @pYear";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pOpeningDate = value.ToString(), pID = this.ID, pYear = this.Year }, tr);
                        tr.Commit();
                    this._OpeningDate = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
            }
        }

        private String _UserID;
        public String UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE NonCompliances SET User = @pUser"
                        + " WHERE ID = @pID AND Year = @pYear";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pUser = value.ToString(), pID = this.ID, pYear = this.Year }, tr);
                        tr.Commit();
                    this._UserID = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }

        public User user
        {
            get
            {
                User curr = new User(this.UserID);
                return curr;
            }
            set
            {
                if(value!=null && value.username.Length>0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliances SET User = @pUser"
                    + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pUser = value.username.ToString(), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            }
        }

        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliances SET Description = @pDescription"
                    + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pDescription = value.ToString(), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ImmediateAction;
        public String ImmediateAction
        {
            get
            {
                return this._ImmediateAction;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliances SET ImmediateAction = @pImmediateAction"
                    + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pImmediateAction = value.ToString(), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._ImmediateAction = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private Double _Cost;
        public Double Cost
        {
            get
            {
                return this._Cost;
            }
            set
            {
                if(value >=0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliances SET Cost = @pCost"
                    + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pCost = value, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._Cost = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
                }
            }
        }

        /* Status values:
         * O = Open
         * C = Closed
         */
        private Char _Status;
        public Char Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    string sql;
                    if(value == 'O') { 
                        sql = "UPDATE NonCompliances SET Status = @pStatus"
                        + ", ClosureDate = null WHERE ID = @pID AND Year = @pYear";
                    }
                    else
                    {
                        sql = "UPDATE NonCompliances SET Status = @pStatus"
                        + " WHERE ID = @pID AND Year = @pYear";
                    }
                    conn.Execute(sql, new { pStatus = value.ToString(), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private DateTime _ClosureDate;
        public DateTime ClosureDate
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._ClosureDate, fuso.tzFusoOrario); ;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliances SET ClosureDate = @pClosureDate"
                    + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pClosureDate = value.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._ClosureDate = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public List<NonComplianceType> Categories;

        public List<NonComplianceCause> Causes;

        public List<NonComplianceProduct> Products;

        public List<String> Files;

        public NonCompliance(String Tenant, int NCID, int NCYear)
        {
            this.Tenant = Tenant;

            this._ID = -1;
            this._Year = -1;
            this._Quantity = -1;
            this.Categories = new List<NonComplianceType>();
            this.Causes = new List<NonComplianceCause>();
            this.Products = new List<NonComplianceProduct>();
            this.Files = new List<String>();

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<NonComplianceRow>("SELECT ID, Year, Quantity, OpeningDate, User, Description, ImmediateAction, "
                + "Cost, Status, ClosureDate FROM NonCompliances WHERE ID = @pID"
                + " AND Year = @pYear", new { pID = NCID, pYear = NCYear });
            if(row != null)
            {
                this._ID = row.ID;
                this._Year = row.Year;
                this._Quantity = row.Quantity;
                this._OpeningDate = new DateTime();
                if(row.OpeningDate.HasValue)
                { 
                    this._OpeningDate = row.OpeningDate.Value;
                }
                this._UserID = row.User;
                this._Description = row.Description;
                this._ImmediateAction = row.ImmediateAction;
                this._Cost = row.Cost;
                this._Status = row.Status;
                this._ClosureDate = new DateTime();
                if(row.ClosureDate.HasValue)
                { 
                    this._ClosureDate = row.ClosureDate.Value;
                }
            }
            conn.Close();
        }

        public void CategoryLoad()
        {
            this.Categories = new List<NonComplianceType>();
            if(this.ID!=-1 && this.Year!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT TypeID FROM noncompliancestype_nc WHERE "
                    + "NCID = @pID"
                    + " AND NCYear = @pYear", new { pID = this.ID, pYear = this.Year });
                foreach (var id in rows)
                {
                    this.Categories.Add(new NonComplianceType(this.Tenant, id));
                }
                conn.Close();
            }
        }

        public Boolean CategoryAdd(int catID)
        {
            Boolean ret = false;
            if(this.ID!=-1 && this.Year!=-1)
            { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "INSERT INTO noncompliancestype_nc(TypeID, NCID, NCYear) VALUES("
                    + "@pCatID, "
                    + "@pID, "
                    + "@pYear"
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pCatID = catID, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean CategoryAdd(String catName)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int typeID = -1;
                typeID = conn.QueryFirstOrDefault<int?>("SELECT ID FROM NonCompliancesTypes WHERE name = @pName", new { pName = catName }) ?? -1;
                conn.Close();

                if(typeID!=-1)
                {
                    ret = this.CategoryAdd(typeID);
                }
            }
            return ret;
        }

        public Boolean CategoryDel(int catID)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "DELETE FROM noncompliancestype_nc WHERE TypeID = @pCatID"
                    + " AND NCID = @pID"
                    + " AND NCYear = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pCatID = catID, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    NonComplianceTypes ncCatList = new NonComplianceTypes(this.Tenant);
                    ncCatList.Delete(catID);
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public void CauseLoad()
        {
            this.Causes = new List<NonComplianceCause>();
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT CauseID FROM noncompliancescause_nc WHERE "
                    + "NCID = @pID"
                    + " AND NCYear = @pYear"
                    + " ORDER BY CauseID", new { pID = this.ID, pYear = this.Year });
                foreach (var id in rows)
                {
                    this.Causes.Add(new NonComplianceCause(this.Tenant, id));
                }
                conn.Close();
            }
        }

        public Boolean CauseAdd(int causeID)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "INSERT INTO noncompliancescause_nc(CauseID, NCID, NCYear) VALUES("
                    + "@pCauseID, "
                    + "@pID, "
                    + "@pYear"
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pCauseID = causeID, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean CauseAdd(String causeName)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int typeID = -1;
                typeID = conn.QueryFirstOrDefault<int?>("SELECT ID FROM NonCompliancesCause WHERE name = @pName", new { pName = causeName }) ?? -1;
                conn.Close();

                if (typeID != -1)
                {
                    ret = this.CauseAdd(typeID);
                }
            }
            return ret;
        }

        public Boolean CauseDel(int causeID)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "DELETE FROM noncompliancescause_nc WHERE CauseID = @pCauseID"
                    + " AND NCID = @pID"
                    + " AND NCYear = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pCauseID = causeID, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    NonComplianceCauses ncCauseList = new NonComplianceCauses(this.Tenant);
                    ncCauseList.Delete(causeID);
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public void ProductsLoad()
        {
            this.Products = new List<NonComplianceProduct>();
            if(this.ID!=-1 && this.Year > 1970) { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<(int ProductID, int ProductYear)>("SELECT ProductID, ProductYear FROM noncompliances_products WHERE "
                    + " NonComplianceID = @pID"
                    + " AND NonComplianceYear = @pYear"
                    + " ORDER BY ProductYear, ProductID", new { pID = this.ID, pYear = this.Year });
                foreach(var r in rows)
                {
                    this.Products.Add(new NonComplianceProduct(this.Tenant, this.ID, this.Year, r.ProductID, r.ProductYear));
                }
            conn.Close();
            }
        }

        public Boolean ProductAdd(int prdID, int prdYear)
        {
            this.log = "NC: " + this.ID + " " + this.Year + " Product: " + prdID + " " + prdYear;
            Boolean ret = false;
            if(this.ID!=-1 && this.Year>1970)
            {
                log = "if1. ";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            // Check if already added
            bool check = false;
            int? existing = conn.QueryFirstOrDefault<int?>("SELECT ProductID FROM noncompliances_products WHERE NonComplianceID = @pID"
                + " AND NonComplianceYear = @pYear"
                + " AND ProductID = @pProductID"
                + " AND ProductYear = @pProductYear", new { pID = this.ID, pYear = this.Year, pProductID = prdID, pProductYear = prdYear });
            if(existing.HasValue)
            {
                check = true;
            }

                log += "Check: " + check.ToString() + ". ";
            if (!check)
            {
                 Articolo art = new Articolo(this.Tenant, prdID, prdYear);
                 if(art.ID!=-1 && art.Year > 1970)
                 {
                        log += "if2. ";
                    char source = art.Status == 'F' ? 'C' : 'P';
                    int qty = art.QuantitaProdotta;
                    string sql = "INSERT INTO noncompliances_products(NonComplianceID, NonComplianceYear, "
                        + "ProductID, ProductYear, Source, WarningID, WorkStation, Quantity) VALUES("
                        + "@pID, "
                        + "@pYear, "
                        + "@pProductID, "
                        + "@pProductYear, "
                        + "@pSource, "
                        + "null, "
                        + "null, "
                        + "@pQty"
                        + ")";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pID = this.ID, pYear = this.Year, pProductID = prdID, pProductYear = prdYear, pSource = source.ToString(), pQty = qty }, tr);
                        tr.Commit();
                        ret = true;
                    }
                    catch(Exception ex)
                    {
                        log += ex.Message + " " + sql;
                        tr.Rollback();
                        ret = false;
                    }
                  }
                }
            conn.Close();
            }
            return ret;
        }

        public Boolean ProductAdd(Warning wrn)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year > 1970 && wrn.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                // Check if already added
                bool check = false;
                int? existing = conn.QueryFirstOrDefault<int?>("SELECT ProductID FROM noncompliances_products WHERE NonComplianceID = @pID"
                    + " AND NonComplianceYear = @pYear"
                    + " AND WarningID = @pWarningID", new { pID = this.ID, pYear = this.Year, pWarningID = wrn.ID });

                if (existing.HasValue)
                {
                    check = true;
                }

                if (!check)
                {
                    TaskProduzione tsk = new TaskProduzione(this.Tenant, wrn.TaskID);
                    Articolo art = new Articolo(this.Tenant, tsk.ArticoloID, tsk.ArticoloAnno);
                    char source ='P';
                    int qty = art.Quantita;
                    string sql = "INSERT INTO noncompliances_products(NonComplianceID, NonComplianceYear, "
                        + "ProductID, ProductYear, Source, WarningID, WorkStation, Quantity) VALUES("
                        + "@pID, "
                        + "@pYear, "
                        + "@pProductID, "
                        + "@pProductYear, "
                        + "@pSource, "
                        + "@pWarningID, "
                        + "@pWorkstation, "
                        + "@pQty"
                        + ")";
                    log = sql;
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pID = this.ID, pYear = this.Year, pProductID = art.ID, pProductYear = art.Year, pSource = source.ToString(), pWarningID = wrn.ID, pWorkstation = tsk.PostazioneID, pQty = qty }, tr);
                        tr.Commit();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        ret = false;
                        log = ex.Message + " " + sql;
                        tr.Rollback();
                    }
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean ProductDel(int prdID, int prdYear)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "DELETE FROM noncompliances_products WHERE NonComplianceID = @pID"
                    + " AND NonComplianceYear = @pYear"
                    + " AND ProductID = @pProductID"
                    + " AND ProductYear = @pProductYear";
                MySqlTransaction tr = conn.BeginTransaction();
                log = sql;
                try
                {
                    conn.Execute(sql, new { pID = this.ID, pYear = this.Year, pProductID = prdID, pProductYear = prdYear }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log += " " + ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public void FilesLoad()
        {
            this.Files = new List<String>();
            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString()));
                FileInfo[] Files = d.GetFiles();
                foreach (FileInfo file in Files)
                {
                    this.Files.Add(file.Name);
                }
            }
        }

        public Boolean FileDelete(String fileName)
        {
            Boolean ret = false;
            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString())))
            {
                DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString()));
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString() + "/" + fileName)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Data/Quality/" + Year.ToString() + "_" + ID.ToString() + "/" + fileName));
                    ret = true;
                }
            }
            return ret;
        }
    }

    public class NonCompliances
    {
        protected String Tenant;

        public String log;

        public List<NonCompliance> NonCompliancesList;

        public NonCompliances(String Tenant)
        {
            this.Tenant = Tenant;

            this.NonCompliancesList = new List<NonCompliance>();
        }

        public void loadNonCompliances()
        {
            this.NonCompliancesList = new List<NonCompliance>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int Year)>("SELECT ID, Year FROM NonCompliances ORDER BY Year desc, ID desc");
            foreach (var r in rows)
            {
                this.NonCompliancesList.Add(new NonCompliance(this.Tenant, r.ID, r.Year));
            }
            conn.Close();
        }

        public Boolean Add(int qty, DateTime opDate, String usr, String desc, String immAction, Double cst, char stat, DateTime closure)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT MAX(ID) From NonCompliances WHERE Year = @pYear";
            int? max = conn.QueryFirstOrDefault<int?>(sql, new { pYear = DateTime.UtcNow.Year });
            int maxID = 0;
            if (max.HasValue)
            {
                maxID = max.Value + 1;
            }

            sql = "INSERT INTO NonCompliances(ID, Year, Quantity, OpeningDate, User, Description, "
                + " ImmediateAction, Cost, Status, ClosureDate) VALUES("
                + "@pID, "
                + "@pYear"
                + "@pOpeningDate, "
                + "@pUser, "
                + "@pDescription, "
                + "@pImmediateAction, "
                + "@pCost, "
                + "@pStatus, "
                + "@pClosureDate"
                +")";
            MySqlTransaction tr = conn.BeginTransaction();

            try
            {
                conn.Execute(sql, new { pID = maxID, pYear = DateTime.UtcNow.Year, pOpeningDate = opDate.ToString(), pUser = usr.ToString(), pDescription = desc, pImmediateAction = immAction, pCost = cst, pStatus = stat.ToString(), pClosureDate = closure.ToString() }, tr);
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                ret = false;
                tr.Rollback();
                log = ex.Message + "<br />" + sql;
            }

            conn.Close();
            return ret;
        }

        public int[] Add(String usr)
        {
            int[] ret = new int[2];
            ret[0] = -1; ret[1] = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT MAX(ID) From NonCompliances WHERE Year = @pYear";
            int? max = conn.QueryFirstOrDefault<int?>(sql, new { pYear = DateTime.UtcNow.Year });
            int maxID = 0;
            if (max.HasValue)
            {
                maxID = max.Value + 1;
            }

            sql = "INSERT INTO NonCompliances(ID, Year, Quantity, OpeningDate, User, Description, "
                + " ImmediateAction, Cost, Status, ClosureDate) VALUES("
                + "@pID, "
                + "@pYear, "
                + "1, "
                + "@pOpeningDate, "
                + "@pUser, "
                + "'', "
                + "'', "
                + "0, "
                + "'O', "
                + "null"
                + ")";
            MySqlTransaction tr = conn.BeginTransaction();

            try
            {
                conn.Execute(sql, new { pID = maxID, pYear = DateTime.UtcNow.Year, pOpeningDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), pUser = usr.ToString() }, tr);
                tr.Commit();
                ret[0]= maxID; ret[1] = DateTime.UtcNow.Year;
            }
            catch (Exception ex)
            {
                ret[0] = -1; ret[1] = -1;
                tr.Rollback();
                log = ex.Message + "<br />" + sql;
            }
            conn.Close();
            return ret;
        }

        public Boolean Delete(int NCID, int NCYear)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "DELETE FROM NonCompliances WHERE ID = @pID"
                + " AND Year = @pYear";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { pID = NCID, pYear = NCYear }, tr);
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                log = ex.Message;
                ret = false;
            }
            conn.Close();
            return ret;
        }
    }

    public class NonComplianceTypes
    {
        protected String Tenant;

        public List<NonComplianceType> TypeList;

        public NonComplianceTypes(String Tenant)
        {
            this.Tenant = Tenant;

            this.TypeList = new List<NonComplianceType>();
        }

        public void loadTypeList()
        {
            this.TypeList = new List<NonComplianceType>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT ID, Name, Description FROM NonCompliancesTypes ORDER BY Name");
            foreach (var id in rows)
            {
                this.TypeList.Add(new NonComplianceType(this.Tenant, id));
            }
            conn.Close();
        }

        public Boolean Add(String name, String desc)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT MAX(ID) FROM NonCompliancesTypes";
            int? max = conn.QueryFirstOrDefault<int?>(sql);
            int maxID = 0;
            if(max.HasValue)
            {
                maxID = max.Value + 1;
            }

            sql = "INSERT INTO NonCompliancesTypes(ID, Name, Description) VALUES("
                + "@pID"
                + ", @pName, "
                + "@pDescription)";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { pID = maxID, pName = name, pDescription = desc }, tr);
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                tr.Rollback();
                ret = false;
            }
            conn.Close();
            return ret;
        }

        public Boolean Delete(int typeID)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT * FROM NonCompliancesType_nc WHERE TypeID = @pTypeID";
            var occ = conn.QueryFirstOrDefault<int?>(sql, new { pTypeID = typeID });
            bool occurences = occ.HasValue;
            if(!occurences)
            { 
                sql = "DELETE FROM NonCompliancesTypes WHERE ID = @pTypeID";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pTypeID = typeID }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    ret = false;
                }
            
            }
            conn.Close();
            return ret;
        }

        public int findIDByName(String name)
        {
            int ret = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? id = conn.QueryFirstOrDefault<int?>("SELECT ID FROM NonCompliancesTypes WHERE Name = @pName", new { pName = name });
            if (id.HasValue)
            {
                ret = id.Value;
            }
            conn.Close();
            return ret;
        }
    }

    public class NonComplianceType
    {
        protected String Tenant;

        public String log;

        private class NonComplianceTypeRow
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public String Description { get; set; }
        }

        private int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        private String _Name;
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliancesTypes SET Name = @pName WHERE ID = @pID";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pName = value, pID = this.ID }, tr);
                    tr.Commit();
                    this._Name = value;
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliancesTypes SET Description = @pDescription WHERE ID = @pID";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pDescription = value, pID = this.ID }, tr);
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public NonComplianceType(String Tenant, int typeID)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<NonComplianceTypeRow>("SELECT ID, Name, Description FROM NonCompliancesTypes WHERE ID = @pTypeID", new { pTypeID = typeID });
            if(row != null)
            {
                this._ID = row.ID;
                this._Name = row.Name;
                this._Description = row.Description;
            }
            else
            {
                this._ID = -1;
                this._Name = "";
                this._Description = "";
            }
            conn.Close();
        }
    }

    public class NonComplianceCauses
    {
        protected String Tenant;

        public List<NonComplianceCause> CausesList;

        public NonComplianceCauses(String Tenant)
        {
            this.Tenant = Tenant;

            this.CausesList = new List<NonComplianceCause>();
        }

        public void loadCausesList()
        {
            this.CausesList = new List<NonComplianceCause>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT ID, Name, Description FROM NonCompliancesCause ORDER BY Name");
            foreach (var id in rows)
            {
                this.CausesList.Add(new NonComplianceCause(this.Tenant, id));
            }
            conn.Close();
        }

        public Boolean Add(String name, String desc)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT MAX(ID) FROM NonCompliancesCause";
            int? max = conn.QueryFirstOrDefault<int?>(sql);
            int maxID = 0;
            if (max.HasValue)
            {
                maxID = max.Value + 1;
            }

            sql = "INSERT INTO NonCompliancesCause(ID, Name, Description) VALUES("
                + "@pID"
                + ", @pName, "
                + "@pDescription)";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { pID = maxID, pName = name, pDescription = desc }, tr);
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                ret = false;
            }
            conn.Close();
            return ret;
        }

        public Boolean Delete(int typeID)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT * FROM NonCompliancesCause_nc WHERE CauseID = @pTypeID";
            var occ = conn.QueryFirstOrDefault<int?>(sql, new { pTypeID = typeID });
            bool occurences = occ.HasValue;
            if (!occurences)
            {

                sql = "DELETE FROM NonCompliancesCause WHERE ID = @pTypeID";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pTypeID = typeID }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    ret = false;
                }
            }
            conn.Close();
            return ret;
        }

        public int findIDByName(String name)
        {
            int ret = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? id = conn.QueryFirstOrDefault<int?>("SELECT ID FROM NonCompliancesCause WHERE Name = @pName", new { pName = name });
            if (id.HasValue)
            {
                ret = id.Value;
            }
            conn.Close();
            return ret;
        }
    }

    public class NonComplianceCause
    {
        protected String Tenant;

        public String log;

        private class NonComplianceCauseRow
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public String Description { get; set; }
        }

        private int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        private String _Name;
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliancesCause SET Name = @pName WHERE ID = @pID";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pName = value, pID = this.ID }, tr);
                    tr.Commit();
                    this._Name = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE NonCompliancesCause SET Description = @pDescription WHERE ID = @pID";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pDescription = value, pID = this.ID }, tr);
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public NonComplianceCause(String Tenant, int typeID)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<NonComplianceCauseRow>("SELECT ID, Name, Description FROM NonCompliancesCause WHERE ID = @pTypeID", new { pTypeID = typeID });
            if (row != null)
            {
                this._ID = row.ID;
                this._Name = row.Name;
                this._Description = row.Description;
            }
            else
            {
                this._ID = -1;
                this._Name = "";
                this._Description = "";
            }
            conn.Close();
        }
    }

    public class NonComplianceProduct
    {
        protected String Tenant;

        public String log;

        private class NonComplianceProductRow
        {
            public int ID { get; set; }
            public int Anno { get; set; }
            public String ProcessName { get; set; }
            public String VarianteName { get; set; }
            public String Matricola { get; set; }
            public int Quantita { get; set; }
            public String Cliente { get; set; }
            public String Ragsociale { get; set; }
            public char Source { get; set; }
            public int? WarningID { get; set; }
            public int? Workstation { get; set; }
            public int Quantity { get; set; }
        }

        private int _NonComplianceID;
        public int NonComplianceID { get { return this._NonComplianceID; } }
        private int _NonComplianceYear;
        public int NonComplianceYear { get { return this._NonComplianceYear; } }
        private int _ProductID;
        public int ProductID { get { return this._ProductID; } }
        private int _ProductYear;
        public int ProductYear { get { return this._ProductYear; } }
        private String _processName;
        public String processName { get { return this._processName; } }
        private String _varianteName;
        public String varianteName { get { return this._varianteName; } }
        private String _SerialNumber;
        public String SerialNumber { get { return this._SerialNumber; } }
        private int _Quantity;
        public int Quantity { get { return this._Quantity; } }
        private String _CustomerID;
        public String CustomerID { get { return this._CustomerID; } }
        private String _CustomerName;
        public String CustomerName { get { return this._CustomerName; } }
        /*'C' = Customer claim
         * 'P' = Internal production
         */
        private char _Source;
        public char Source { get { return this._Source; }
            set {
                if(this.ProductID!=-1 && this.ProductYear > 1970 && this.NonComplianceID !=-1 &&this.NonComplianceYear>1970)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE noncompliances_products SET source = @pSource WHERE "
                    + " NonComplianceID = @pNCID AND NonComplianceYear = @pNCYear"
                    + " AND ProductID = @pProductID AND ProductYear = @pProductYear";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pSource = value.ToString(), pNCID = this.NonComplianceID, pNCYear = this.NonComplianceYear, pProductID = this.ProductID, pProductYear = this.ProductYear }, tr);
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message + " " + sql;
                        tr.Rollback();
                    }
                conn.Close();
                }
            }
        }
        private int _WarningID;
        public int WarningID { get { return this._WarningID; }
        }
        private int _Workstation;
        public int Workstation {  get { return this._Workstation; }
        set
            {
                if (this.ProductID != -1 && this.ProductYear > 1970 && this.NonComplianceID != -1 && this.NonComplianceYear > 1970)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql;
                    if(value == -1)
                    {
                        sql = "UPDATE noncompliances_products SET WorkStation = null WHERE "
                            + " NonComplianceID = @pNCID AND NonComplianceYear = @pNCYear"
                            + " AND ProductID = @pProductID AND ProductYear = @pProductYear";
                    }
                    else {
                        sql = "UPDATE noncompliances_products SET WorkStation = @pWorkstation WHERE "
                            + " NonComplianceID = @pNCID AND NonComplianceYear = @pNCYear"
                            + " AND ProductID = @pProductID AND ProductYear = @pProductYear";
                    }
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        if(value == -1)
                        {
                            conn.Execute(sql, new { pNCID = this.NonComplianceID, pNCYear = this.NonComplianceYear, pProductID = this.ProductID, pProductYear = this.ProductYear }, tr);
                        }
                        else
                        {
                            conn.Execute(sql, new { pWorkstation = value, pNCID = this.NonComplianceID, pNCYear = this.NonComplianceYear, pProductID = this.ProductID, pProductYear = this.ProductYear }, tr);
                        }
                        tr.Commit();
                        if(value == -1)
                        {
                            this._WorkstationName = "";
                            this._Workstation = -1;
                        }
                        else
                        {
                            Postazione p = new Postazione(this.Tenant, value);
                            this._Workstation = value;
                            this._WorkstationName = p.name;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }
        private String _WorkstationName;
        public String WorkstationName
        {
            get { return this._WorkstationName; }
        }

        private int _QuantityInvolved;
        public int QuantityInvolved { get { return this._QuantityInvolved; }
            set
            {
                Articolo art = new Articolo(this.Tenant, this.ProductID, this.ProductYear);
                if (this.ProductID != -1 && this.ProductYear > 1970 && this.NonComplianceID != -1 && this.NonComplianceYear > 1970 && value <= art.QuantitaProdotta)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE noncompliances_products SET Quantity = @pQuantity WHERE "
                        + " NonComplianceID = @pNCID AND NonComplianceYear = @pNCYear"
                        + " AND ProductID = @pProductID AND ProductYear = @pProductYear";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pQuantity = value, pNCID = this.NonComplianceID, pNCYear = this.NonComplianceYear, pProductID = this.ProductID, pProductYear = this.ProductYear }, tr);
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message + " " + sql;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public NonComplianceProduct(String Tenant, int ncID, int ncYear, int prdID, int prdYear)
        {
            this.Tenant = Tenant;

            this._ProductID = -1;
            this._ProductYear = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT productionplan.id, productionplan.anno, processo.name, varianti.nomevariante, "
                                + "productionplan.matricola, productionplan.quantita, commesse.cliente, "
                                + "anagraficaclienti.ragsociale, noncompliances_products.source, noncompliances_products.warningID, "
                                + " noncompliances_products.Workstation, noncompliances_products.Quantity FROM "
                                + "noncompliances_products INNER JOIN productionplan "
                                + "ON(noncompliances_products.ProductID = productionplan.id AND noncompliances_products.ProductYear = productionplan.anno)"
                                + " INNER JOIN variantiprocessi ON(productionplan.processo = variantiprocessi.processo "
                                + " AND productionplan.revisione = variantiprocessi.revproc AND productionplan.variante = variantiprocessi.variante)"
                                + " INNER JOIN processo ON (processo.processID = variantiprocessi.processo"
                                + " AND processo.revisione = variantiprocessi.revProc) "
                                + " INNER JOIN varianti ON (variantiprocessi.variante = varianti.idvariante)"
                                + " INNER JOIN commesse ON(productionplan.commessa = commesse.idcommesse"
                                + " AND productionplan.annocommessa = commesse.anno)"
                                + " INNER JOIN anagraficaclienti ON(commesse.cliente = anagraficaclienti.codice) "
                                + " WHERE noncompliances_products.NonComplianceID = @pNCID"
                                + " AND noncompliances_products.NonComplianceYear = @pNCYear"
                                + " AND noncompliances_products.productID = @pProductID"
                                + " AND noncompliances_products.productYear = @pProductYear"
                                + " ORDER BY productionplan.anno, productionplan.id";
            log = sql;
            var row = conn.QueryFirstOrDefault<NonComplianceProductRow>(sql, new { pNCID = ncID, pNCYear = ncYear, pProductID = prdID, pProductYear = prdYear });
            if(row != null)
            {
                this._NonComplianceID = ncID;
                this._NonComplianceYear = ncYear;
                this._ProductID = prdID;
                this._ProductYear = prdYear;
                this._processName = row.ProcessName;
                this._varianteName = row.VarianteName;
                if(row.Matricola != null)
                { 
                    this._SerialNumber = row.Matricola;
                }
                this._Quantity = row.Quantita;
                this._CustomerID = row.Cliente;
                this._CustomerName = row.Ragsociale;
                this._Source = row.Source;
                this._WarningID = row.WarningID.HasValue ? row.WarningID.Value : -1;
                this._Workstation = row.Workstation.HasValue ? row.Workstation.Value : -1;
                if (this.Workstation!=-1) { Postazione p = new Postazione(this.Tenant, this.Workstation);this._WorkstationName = p.name; }
                this._QuantityInvolved = row.Quantity;
    }
            else
            {
                this._ProductID = -1;
                this._ProductYear = -1;
                this._NonComplianceID = -1;
                this._NonComplianceYear = -1;
            }
            conn.Close();
        }
    }

    public class FreeWarnings
    {
        protected String Tenant;

        public String log;

        public List<FlatWarning> FreeWarningList;
        public FreeWarnings(String Tenant)
        {
            this.Tenant = Tenant;

            this.FreeWarningList = new List<FlatWarning>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT warningproduzione.id, warningproduzione.datachiamata, warningproduzione.task, "
            + " postazioni.idpostazioni, postazioni.name,"
            + " productionplan.id, productionplan.anno, processo.name, varianti.nomevariante, "
            + " productionplan.matricola, productionplan.quantita, commesse.cliente, anagraficaclienti.ragsociale, "
            + " warningproduzione.user, warningproduzione.motivo, warningproduzione.risoluzione "
            + " FROM noncompliances_products RIGHT JOIN warningproduzione ON"
            + " (noncompliances_products.warningID = warningproduzione.ID)"
            + " INNER JOIN tasksproduzione ON (warningproduzione.task = tasksproduzione.taskID)"
            + " INNER JOIN productionplan ON (productionplan.id = tasksproduzione.idArticolo AND"
            + " productionplan.anno = tasksproduzione.annoArticolo)"
            + " INNER JOIN variantiprocessi "
            + " ON (productionplan.processo = variantiprocessi.processo"
            + " AND productionplan.revisione = variantiprocessi.revproc"
            + " AND productionplan.variante = variantiprocessi.variante)"
            + " INNER JOIN processo ON (processo.processID = variantiprocessi.processo"
            + " AND processo.revisione = variantiprocessi.revProc)"
            + " INNER JOIN varianti ON (variantiprocessi.variante = varianti.idvariante)"
            + " INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse"
            + " AND productionplan.annocommessa = commesse.anno)"
            + " INNER JOIN anagraficaclienti ON (commesse.cliente = anagraficaclienti.codice)"
            + " INNER JOIN postazioni ON (postazioni.idpostazioni = tasksproduzione.postazione)"
            + " WHERE warningID IS NULL"
            + " ORDER BY warningproduzione.dataChiamata DESC";
            log = sql;
            var rows = conn.Query<(int WarningID, DateTime WarningDate, int TaskID, int Workstation, String WorkstationName, int ProductID, int ProductYear, String processName, String varianteName, String SerialNumber, int Quantity, String CustomerID, String CustomerName, String User, String Reason, String Resolution)>(sql);
            foreach (var r in rows)
            {
                FlatWarning curr = new FlatWarning();
                curr.WarningID = r.WarningID;
                curr.WarningDate = r.WarningDate;
                curr.TaskID = r.TaskID;
                curr.Workstation = r.Workstation;
                curr.WorkstationName = r.WorkstationName;
                curr.ProductID = r.ProductID;
                curr.ProductYear = r.ProductYear;
                curr.processName = r.processName;
                curr.varianteName = r.varianteName;
                if(r.SerialNumber != null)
                { 
                    curr.SerialNumber = r.SerialNumber;
                }
                curr.Quantity = r.Quantity;
                curr.CustomerID = r.CustomerID;
                curr.CustomerName = r.CustomerName;
                if(r.User != null)
                { 
                    curr.User = r.User;
                }
                if (r.Reason != null)
                {
                    curr.Reason = r.Reason;
                }
                if(r.Resolution != null)
                { 
                    curr.Resolution = r.Resolution;
                }

                this.FreeWarningList.Add(curr);
            }
            conn.Close();
        }
    }

    public class AnalysisNCCause
    {
        protected String Tenant;

        public int NCID;
        public int NCYear;
        public DateTime OpeningDate;
        public String User;
        public String NCDescription;
        public String ImmediateAction;
        public Double Cost;
        public char Status;
        public DateTime ClosureDate;
        public int CauseID;
        public String CauseName;
        public String CauseDescription;
    }

    public struct AnalysisNCCategory
    {
        public int NCID;
        public int NCYear;
        public DateTime OpeningDate;
        public String User;
        public String NCDescription;
        public String ImmediateAction;
        public Double Cost;
        public char Status;
        public DateTime ClosureDate;
        public int CategoryID;
        public String CategoryName;
        public String CategoryDescription;
    }

    public struct AnalysisNCProduct
    {
        public int NCID;
        public int NCYear;
        public DateTime OpeningDate;
        public String User;
        public String NCDescription;
        public String ImmediateAction;
        public Double Cost;
        public char Status;
        public DateTime ClosureDate;
        public int ProductID;
        public int ProductYear;
        public int NCQuantity;
        public char Source;
        public int WarningID;
        public int Workstation;
        public int ProcessID;
        public int ProcessRev;
        public String ProcessName;
        public int VariantID;
        public String VariantName;

    }

    public class NCAnalysis
    {
        protected String Tenant;

        public List<AnalysisNCCause> ncCauses;
        public List<AnalysisNCCategory> ncCategory;
        public List<AnalysisNCProduct> ncProduct;
        public List<NonCompliancesAnalysisStruct> ncList;

        public NCAnalysis(String Tenant)
        {
            this.Tenant = Tenant;

            this.ncCauses = new List<AnalysisNCCause>();
            this.ncCategory = new List<AnalysisNCCategory>();
            this.ncProduct = new List<AnalysisNCProduct>();
            this.ncList = new List<NonCompliancesAnalysisStruct>();
        }

        public void loadNcCauses()
        {
            this.ncCauses = new List<AnalysisNCCause>();
            MySqlConnection conn = (new Dati.Dati().mycon(this.Tenant));
            conn.Open();
            string sql = "SELECT noncompliances.ID, noncompliances.Year, noncompliances.OpeningDate, noncompliances.User, "
                + "noncompliances.Description, noncompliances.ImmediateAction, noncompliances.Cost, "
                + "noncompliances.Status, noncompliances.ClosureDate, noncompliancescause.ID, noncompliancescause.Name, "
                + "noncompliancescause.Description FROM noncompliances INNER JOIN noncompliancescause_nc ON "
                + "(noncompliances.ID = noncompliancescause_nc.NCID AND noncompliances.Year = noncompliancescause_nc.NCYear) "
                + "INNER JOIN noncompliancescause ON (noncompliancescause.ID = noncompliancescause_nc.CauseID) "
                + " ORDER BY noncompliances.OpeningDate desc, noncompliancescause.ID ASC";
            var rows = conn.Query<(int NCID, int NCYear, DateTime OpeningDate, String User, String NCDescription, String ImmediateAction, double? Cost, String Status, DateTime? ClosureDate, int CauseID, String CauseName, String CauseDescription)>(sql);
            foreach (var r in rows)
            {
                AnalysisNCCause curr = new AnalysisNCCause();
                curr.NCID = r.NCID;
                curr.NCYear = r.NCYear;
                curr.OpeningDate = r.OpeningDate;
                curr.User = r.User;
                if (r.NCDescription != null) { curr.NCDescription = r.NCDescription; }
                if (r.ImmediateAction != null) { curr.ImmediateAction = r.ImmediateAction; }
                if (r.Cost.HasValue) { curr.Cost = r.Cost.Value; }
                curr.Status = r.Status[0];
                if (r.ClosureDate.HasValue) { curr.ClosureDate = r.ClosureDate.Value; }
                curr.CauseID = r.CauseID;
                curr.CauseName = r.CauseName;
                if (r.CauseDescription != null) { curr.CauseDescription = r.CauseDescription; }

                this.ncCauses.Add(curr);
            }
            conn.Close();
        }

        public void loadNcCategories()
        {
            this.ncCategory = new List<AnalysisNCCategory>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT noncompliances.ID, noncompliances.Year, noncompliances.OpeningDate, noncompliances.User, "
                + "noncompliances.Description, noncompliances.ImmediateAction, noncompliances.Cost, "
                + " noncompliances.Status, noncompliances.ClosureDate, noncompliancestypes.ID, noncompliancestypes.Name, "
                + "noncompliancestypes.Description FROM noncompliances INNER JOIN noncompliancestype_nc ON "
                + "(noncompliances.ID = noncompliancestype_nc.NCID AND noncompliances.Year = noncompliancestype_nc.NCYear)"
                + " INNER JOIN noncompliancestypes ON (noncompliancestypes.ID = noncompliancestype_nc.TypeID)"
                + " ORDER BY noncompliances.OpeningDate desc, noncompliancestypes.ID ASC";
            var rows = conn.Query<(int NCID, int NCYear, DateTime OpeningDate, String User, String NCDescription, String ImmediateAction, double? Cost, String Status, DateTime? ClosureDate, int CategoryID, String CategoryName, String CategoryDescription)>(sql);
            foreach (var r in rows)
            {
                AnalysisNCCategory curr = new AnalysisNCCategory();
                curr.NCID = r.NCID;
                curr.NCYear = r.NCYear;
                curr.OpeningDate = r.OpeningDate;
                curr.User = r.User;
                if (r.NCDescription != null) { curr.NCDescription = r.NCDescription; }
                if (r.ImmediateAction != null) { curr.ImmediateAction = r.ImmediateAction; }
                if (r.Cost.HasValue) { curr.Cost = r.Cost.Value; }
                curr.Status = r.Status[0];
                if (r.ClosureDate.HasValue) { curr.ClosureDate = r.ClosureDate.Value; }
                curr.CategoryID = r.CategoryID;
                curr.CategoryName = r.CategoryName;
                if (r.CategoryDescription != null) { curr.CategoryDescription = r.CategoryDescription; }

                this.ncCategory.Add(curr);
            }
            conn.Close();
        }

        public void loadNcProducts()
        {
            this.ncProduct = new List<AnalysisNCProduct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT noncompliances.ID, noncompliances.Year, noncompliances.OpeningDate, "
                + "noncompliances.User, noncompliances.Description, noncompliances.ImmediateAction, "
                + "noncompliances.Cost, noncompliances.Status, noncompliances.ClosureDate, "
                + "noncompliances_products.ProductID, noncompliances_products.ProductYear, "
                + "noncompliances_products.Quantity, noncompliances_products.Source, "
                + "noncompliances_products.WarningID, noncompliances_products.Workstation, "
                + "processo.ProcessID, processo.revisione, processo.Name, varianti.idvariante, varianti.nomevariante "
                + " FROM noncompliances INNER JOIN noncompliances_products "
                + "ON(noncompliances.ID = noncompliances_products.NonComplianceID "
                + "AND noncompliances.Year = noncompliances_products.NonComplianceYear) "
                + " INNER JOIN productionplan ON(noncompliances_products.ProductID = productionplan.id "
                + "AND noncompliances_products.ProductYear = productionplan.anno) "
                + "INNER JOIN variantiprocessi ON(productionplan.processo = variantiprocessi.processo "
                + " AND productionplan.revisione = variantiprocessi.revproc "
                + " AND productionplan.variante = variantiprocessi.variante) "
                + " INNER JOIN processo ON (processo.processID = variantiprocessi.processo "
                + " AND processo.revisione = variantiprocessi.revProc) INNER JOIN "
                + " varianti ON (variantiprocessi.variante = varianti.idvariante) "
                + " INNER JOIN commesse ON(productionplan.commessa = commesse.idcommesse "
                + " AND productionplan.annocommessa = commesse.anno) "
                + " INNER JOIN anagraficaclienti ON(commesse.cliente = anagraficaclienti.codice)";
            var rows = conn.Query<(int NCID, int NCYear, DateTime OpeningDate, String User, String NCDescription, String ImmediateAction, double? Cost, String Status, DateTime? ClosureDate, int ProductID, int ProductYear, int NCQuantity, String Source, int? WarningID, int? Workstation, int ProcessID, int ProcessRev, String ProcessName, int VariantID, String VariantName)>(sql);
            foreach (var r in rows)
            {
                AnalysisNCProduct curr = new AnalysisNCProduct();
                curr.NCID = r.NCID;
                curr.NCYear = r.NCYear;
                curr.OpeningDate = r.OpeningDate;
                curr.User = r.User;
                if (r.NCDescription != null) { curr.NCDescription = r.NCDescription; }
                if (r.ImmediateAction != null) { curr.ImmediateAction = r.ImmediateAction; }
                curr.Cost = r.Cost.Value;
                curr.Status = r.Status[0];
                if (r.ClosureDate.HasValue) { curr.ClosureDate = r.ClosureDate.Value; }
                curr.ProductID = r.ProductID;
                curr.ProductYear = r.ProductYear;
                curr.NCQuantity = r.NCQuantity;
                curr.Source = r.Source[0];
                if (r.WarningID.HasValue) { curr.WarningID = r.WarningID.Value; }
                if (r.Workstation.HasValue) { curr.Workstation = r.Workstation.Value; }
                curr.ProcessID = r.ProcessID;
                curr.ProcessRev = r.ProcessRev;
                curr.ProcessName = r.ProcessName;
                curr.VariantID = r.VariantID;
                curr.VariantName = r.VariantName;

                this.ncProduct.Add(curr);
            }
        }

        public List<NonCompliancesAnalysisStruct> loadNonCompliances()
        {
            ncList = new List<NonCompliancesAnalysisStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT ID, Year, Quantity, OpeningDate, Status, ClosureDate, cost FROM noncompliances";
            var rows = conn.Query<(int ID, int Year, int Quantity, DateTime OpeningDate, String Status, DateTime? ClosureDate, double? Cost)>(sql);
            foreach(var r in rows)
            {
                NonCompliancesAnalysisStruct curr = new NonCompliancesAnalysisStruct();
                curr.ID = r.ID;
                curr.Year = r.Year;
                curr.Quantity = r.Quantity;
                curr.OpeningDate = r.OpeningDate;
                curr.Status = r.Status[0];
                if(r.ClosureDate.HasValue)
                {
                    curr.ClosureDate = r.ClosureDate.Value;
                }
                curr.Cost = r.Cost.Value;
                this.ncList.Add(curr);
            }
            conn.Close();
            return this.ncList;
        }
    }

    public struct NonCompliancesAnalysisStruct
    {
        public int ID;
        public int Year;
        public int Quantity;
        public DateTime OpeningDate;
        public char Status;
        public DateTime ClosureDate;
        public double Cost;
    }

    public class ImprovementActions
    {
        protected String Tenant;

        public String log;

        public List<ImprovementAction> ImprovementActionsList;
        public ImprovementActions(String Tenant)
        {
            this.Tenant = Tenant;

            this.ImprovementActionsList = new List<ImprovementAction>();
        }

        public void loadImprovementActions()
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int Year)>("SELECT ID, Year FROM ImprovementActions ORDER BY OpeningDate");
            foreach (var r in rows)
            {
                this.ImprovementActionsList.Add(new ImprovementAction(this.Tenant, r.ID, r.Year));
            }
            conn.Close();
        }

        public void loadImprovementActions(char stat)
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int Year)>("SELECT ID, Year FROM ImprovementActions WHERE improvementactions.status = @pStatus"
                + " ORDER BY OpeningDate", new { pStatus = stat.ToString() });
            foreach (var r in rows)
            {
                this.ImprovementActionsList.Add(new ImprovementAction(this.Tenant, r.ID, r.Year));
            }
            conn.Close();
        }

        public void loadImprovementActions(User usr)
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int Year)>("SELECT ID, Year FROM ImprovementActions INNER JOIN improvementactions_team ON "
                +"(ImprovementActions.ID = improvementactions_team.ImprovementActionID AND "
                + "improvementactions.Year = improvementactions_team.ImprovementActionYear) "
                + " WHERE improvementactions_team.user = @pUser"
                + " ORDER BY OpeningDate", new { pUser = usr.username });
            foreach (var r in rows)
            {
                this.ImprovementActionsList.Add(new ImprovementAction(this.Tenant, r.ID, r.Year));
            }
            conn.Close();
        }

        public void loadImprovementActions(User usr, Char status)
        {
            this.ImprovementActionsList = new List<ImprovementAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int Year)>("SELECT ID, Year FROM ImprovementActions INNER JOIN improvementactions_team ON "
                + "(ImprovementActions.ID = improvementactions_team.ImprovementActionID AND "
                + "improvementactions.Year = improvementactions_team.ImprovementActionYear) "
                + " WHERE improvementactions_team.user = @pUser "
                + " AND improvementactions.status = @pStatus"
                + " ORDER BY OpeningDate", new { pUser = usr.username, pStatus = status.ToString() });
            foreach (var r in rows)
            {
                this.ImprovementActionsList.Add(new ImprovementAction(this.Tenant, r.ID, r.Year));
            }
            conn.Close();
        }

        public int[] Add(UserAccount creator)
        {
            int[] ret = new int[2];
            ret[0] = -1;
            ret[1] = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT MAX(ID) FROM ImprovementActions WHERE Year = @pYear";
            int? max = conn.QueryFirstOrDefault<int?>(sql, new { pYear = DateTime.UtcNow.Year });
            int iActID = 0;
            if(max.HasValue)
            {
                iActID = max.Value + 1;
            }

            sql = "INSERT INTO ImprovementActions(ID, Year, OpeningDate, CurrentSituation, ExpectedResults, "
                + " RootCauses, ClosureNotes, Status, EndDateExpected, EndDateReal, CreatedBy, ModifiedBy, ModifiedDate) VALUES("
                + "@pID, "
                + "@pYear, "
                + "@pOpeningDate, "
                + "'', "
                + "'', "
                + "'', "
                + "'', "
                + "'O', "
                + "null, "
                + "null, "
                + "@pCreatedBy, "
                + "'', "
                + "null"
                + ")";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { pID = iActID, pYear = DateTime.UtcNow.Year, pOpeningDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), pCreatedBy = creator.userId }, tr);
                tr.Commit();
                ret[0] = iActID;
                ret[1] = DateTime.UtcNow.Year;
            }
            catch(Exception ex)
            {
                log = ex.Message + " " + sql;
                tr.Rollback();
                ret[0] = -1;
                ret[1] = -1;
            }
            conn.Close();
            return ret;
        }

        public Boolean Delete(int id, int year)
        {
            Boolean ret = false;
            ImprovementAction curr = new ImprovementAction(this.Tenant, id, year);
            if(curr.ID!=-1 && curr.Year!=-1)
            {
                curr.loadCorrectiveActions();
                for(int i = 0; i < curr.CorrectiveActions.Count; i++)
                {
                    curr.CorrectiveActions[i].loadTeamMembers();
                    for(int j = 0; j < curr.CorrectiveActions[i].TeamMembers.Count; j++)
                    {
                        curr.CorrectiveActions[i].MemberRemove(curr.CorrectiveActions[i].TeamMembers[j].User);
                    }

                    curr.CorrectiveActions[i].loadTasks();
                    for(int j = 0; j < curr.CorrectiveActions[i].Tasks.Count; j++)
                    {
                        curr.CorrectiveActions[i].TaskRemove(curr.CorrectiveActions[i].Tasks[j].TaskID);
                    }
                }
                curr.loadTeamMembers();
                for(int i =0; i < curr.TeamMembers.Count; i++)
                {
                    curr.MemberRemove(curr.TeamMembers[i].User);
                }

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "DELETE FROM improvementactions WHERE ID = @pID AND Year = @pYear";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { pID = id, pYear = year }, tr);
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                log = ex.Message;
                ret = false;
                tr.Rollback();
            }
            conn.Close();
            }
            return ret;
        }
    }

    public class ImprovementAction
    {
        protected String Tenant;

        public String log;

        private class ImprovementActionRow
        {
            public int ID { get; set; }
            public int Year { get; set; }
            public DateTime? OpeningDate { get; set; }
            public String CurrentSituation { get; set; }
            public String ExpectedResults { get; set; }
            public String RootCauses { get; set; }
            public String ClosureNotes { get; set; }
            public String Status { get; set; }
            public DateTime? EndDateExpected { get; set; }
            public DateTime? EndDateReal { get; set; }
            public String CreatedBy { get; set; }
            public String ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
        }

        private int _ID;
        public int ID { get { return this._ID; } }
        private int _Year;
        public int Year { get { return this._Year; } }
        private DateTime _OpeningDate;
        public DateTime OpeningDate
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._OpeningDate, fuso.tzFusoOrario);
            }
            set
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE ImprovementActions SET OpeningDate = @pOpeningDate"
                    + " WHERE ID = @pID AND Year = @pYear";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pOpeningDate = value.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.ID, pYear = this.Year }, tr);
                        tr.Commit();
                        this._OpeningDate = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
        }

        private String _CreatedBy;
        public String CreatedBy
        {
            get { return this._CreatedBy; }
        }

        private String _CurrentSituation;
        public String CurrentSituation
        {
            get { return this._CurrentSituation; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET CurrentSituation = @pCurrentSituation"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pCurrentSituation = value, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._CurrentSituation = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ExpectedResults;
        public String ExpectedResults
        {
            get { return this._ExpectedResults; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET ExpectedResults = @pExpectedResults"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pExpectedResults = value, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._ExpectedResults = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _RootCauses;
        public String RootCauses
        {
            get { return this._RootCauses; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET RootCauses = @pRootCauses"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pRootCauses = value, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._RootCauses = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ClosureNotes;
        public String ClosureNotes
        {
            get { return this._ClosureNotes; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET ClosureNotes = @pClosureNotes"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pClosureNotes = value, pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._ClosureNotes = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        /* O == Open
         * C == Closed
         */
        private Char _Status;
        public Char Status
        {
            get { return this._Status; }
            set
            {
                Boolean check = true;
                if(value == 'C')
                {
                    this.loadCorrectiveActions();
                    for(int i = 0; i < this.CorrectiveActions.Count; i++)
                    {
                        if(this.CorrectiveActions[i].Status != 'C')
                        {
                            check = false;
                        }
                    }
                }
                if(value == 'O' || (value == 'C' && check))
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET Status = @pStatus"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pStatus = value.ToString(), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
                }
            }
        }

        private DateTime _EndDateExpected;
        public DateTime EndDateExpected
        {
            get {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EndDateExpected, fuso.tzFusoOrario); }
            set
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET EndDateExpected = @pEndDateExpected"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pEndDateExpected = curr.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._EndDateExpected = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private DateTime _EndDateReal;
        public DateTime EndDateReal
        {
            get {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EndDateReal, fuso.tzFusoOrario); }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET EndDateReal = @pEndDateReal"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pEndDateReal = value.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._EndDateReal = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _ModifiedBy;
        public String ModifiedBy
        {
            get { return this._ModifiedBy; }
            set
            {
                User usr = new User(value);
                if(usr.username.Length > 0)
                { 
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE ImprovementActions SET ModifiedBy = @pModifiedBy"
                    + " WHERE ID = @pID AND Year = @pYear";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { pModifiedBy = value, pID = this.ID, pYear = this.Year }, tr);
                        tr.Commit();
                        this._ModifiedBy = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private DateTime _ModifiedDate;
        public DateTime ModifiedDate
        {
            get {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._ModifiedDate, fuso.tzFusoOrario); }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE ImprovementActions SET ModifiedDate = @pModifiedDate"
                + " WHERE ID = @pID AND Year = @pYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pModifiedDate = value.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.ID, pYear = this.Year }, tr);
                    tr.Commit();
                    this._ModifiedDate = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public List<ImprovementActionTeamMember> TeamMembers;

        public List<CorrectiveAction> CorrectiveActions;
        public ImprovementAction(String Tenant, int iActID, int iActYear)
        {
            this.Tenant = Tenant;

            this.TeamMembers = new List<ImprovementActionTeamMember>();
            this.CorrectiveActions = new List<CorrectiveAction>();

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT ID, Year, OpeningDate, CurrentSituation, ExpectedResults, RootCauses, "
                + " ClosureNotes, Status, EndDateExpected, EndDateReal, CreatedBy, ModifiedBy, ModifiedDate "
                + " FROM ImprovementActions WHERE ID = @pID AND Year = @pYear";
            var row = conn.QueryFirstOrDefault<ImprovementActionRow>(sql, new { pID = iActID, pYear = iActYear });
            if(row != null)
            {
                this._ID = row.ID;
                this._Year = row.Year;
                if (row.OpeningDate.HasValue)
                {
                    this._OpeningDate = row.OpeningDate.Value;
                }
                if (row.CurrentSituation != null)
                {
                    this._CurrentSituation = row.CurrentSituation;
                }
                if (row.ExpectedResults != null)
                {
                    this._ExpectedResults = row.ExpectedResults;
                }
                if (row.RootCauses != null)
                {
                    this._RootCauses = row.RootCauses;
                }
                if (row.ClosureNotes != null)
                {
                    this._ClosureNotes = row.ClosureNotes;
                }
                if(row.Status != null)
                { 
                    this._Status = row.Status[0];
                }
                else
                {
                    this._Status = 'O';
                }
                if (row.EndDateExpected.HasValue)
                {
                    this._EndDateExpected = row.EndDateExpected.Value;
                }
                else
                {
                    this._EndDateExpected = new DateTime(1970, 1, 1);
                }

                if (row.EndDateReal.HasValue)
                {
                    this._EndDateReal = row.EndDateReal.Value;
                }
                else
                {
                    this._EndDateReal = new DateTime(1970, 1, 1);
                }
                if(row.CreatedBy != null)
                {
                    this._CreatedBy = row.CreatedBy;
                }
                if (row.ModifiedBy != null)
                {
                    this._ModifiedBy = row.ModifiedBy;
                }
                if (row.ModifiedDate.HasValue)
                {
                    this._ModifiedDate = row.ModifiedDate.Value;
                }
            }
            else
            {
                this._ID = -1;
                this._Year = -1;
            }
                conn.Close();
        }

        public void loadTeamMembers()
        {
            this.TeamMembers = new List<ImprovementActionTeamMember>();

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(String user, String role)>("SELECT user, role FROM improvementactions_team WHERE ImprovementActionID = @pID"
                + " AND ImprovementActionYear = @pYear ORDER BY role, user", new { pID = this.ID, pYear = this.Year });
            foreach (var r in rows)
            {
                this.TeamMembers.Add(new ImprovementActionTeamMember(this.Tenant, this.ID, this.Year, r.user, r.role[0]));
            }
            conn.Close();
        }

        public Boolean MemberAdd(String username, Char role)
        {
            Boolean ret = false;
            if(this.ID!=-1 && this.Year!=-1)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "INSERT INTO ImprovementActions_Team(ImprovementActionID, ImprovementActionYear, user, role) "
                + " VALUES(@pID, @pYear, "
                + "@pUser, @pRole)";
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    conn.Execute(sql, new { pID = this.ID, pYear = this.Year, pUser = username, pRole = role.ToString() }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    ret = false;
                    tr.Rollback();
                    this.log = ex.Message + " " + sql;
                }
            conn.Close();
            }
            return ret;
        }

        public Boolean MemberRemove(String username)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "DELETE FROM improvementactions_team WHERE ImprovementActionID = @pID"
                + " AND ImprovementActionYear = @pYear"
                + " AND user = @pUser";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { pID = this.ID, pYear = this.Year, pUser = username }, tr);
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                ret = false;
                log = ex.Message + " " + sql;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public void loadCorrectiveActions()
        {
            this.CorrectiveActions = new List<CorrectiveAction>();
            if(this.ID!=-1 && this.Year > 1970)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var ids = conn.Query<int>("SELECT ID FROM correctiveactions WHERE ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear"
                + " ORDER BY correctiveactions.LateStart", new { pIAID = this.ID, pIAYear = this.Year });
            foreach(var id in ids)
            {
                this.CorrectiveActions.Add(new CorrectiveAction(this.Tenant, this.ID, this.Year, id));
            }
            conn.Close();
            }
        }

        public int CorrectiveActionAdd()
        {
            int ret = -1;
            if(this.ID!=-1 && this.Year != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT MAX(ID) FROM correctiveactions WHERE ImprovementActionID = @pIAID"
                    + " AND ImprovementActionYear = @pIAYear";
                int? max = conn.QueryFirstOrDefault<int?>(sql, new { pIAID = this.ID, pIAYear = this.Year });
                int caID = 0;
                if(max.HasValue)
                {
                    caID = max.Value + 1;
                }

                sql = "INSERT INTO correctiveactions(ID, ImprovementActionID, ImprovementActionYear, "
                    + " Description, LeadTimeExpected, EarlyStart, LateStart, EarlyFinish, LateFinish, Status) VALUES("
                    + "@pID, "
                    + "@pIAID, "
                    + "@pIAYear, "
                    + "'', "
                    + "0.0, "
                    + "null, "
                    + "null, "
                    + "null, "
                    + "null, "
                    + "'O'"
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pID = caID, pIAID = this.ID, pIAYear = this.Year }, tr);
                    tr.Commit();
                    if(this.Status == 'C')
                    {
                        this.Status = 'O';
                    }
                    ret = caID;
                }
                catch(Exception ex)
                {
                    ret = -1;
                    this.log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean CorrectiveActionRemove(int CorrectiveActionID)
        {
            Boolean ret = false;
            if(this.ID!=-1 && this.Year > 1970)
            {
                CorrectiveAction curr = new CorrectiveAction(this.Tenant, this.ID, this.Year, CorrectiveActionID);
                curr.loadTasks();
                for(int i = 0; i < curr.Tasks.Count; i++)
                {
                    // Remove tasks
                }

                curr.loadTeamMembers();
                for(int i = 0; i < curr.TeamMembers.Count; i++)
                {
                    curr.MemberRemove(curr.TeamMembers[i].User);
                }

                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "DELETE FROM correctiveactions WHERE ID = @pID"
                    + " AND ImprovementActionID = @pIAID AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pID = CorrectiveActionID, pIAID = this.ID, pIAYear = this.Year }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    ret = false;
                    this.log = ex.Message + " " + sql;
                    ret = false;
                }
                conn.Close();
            }
            return ret;
        }
    }

    public class ImprovementActionTeamMember
    {
        protected String Tenant;

        public String log;

        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private String _User;
        public String User
        {
            get
            {
                return this._User;
            }
        }

        /* M == Manager --> can create, edit, delete improvement action's team members and tasks
         * T == Team Member --> tasks can be assigned to him, and he can execute tasks
         */
        private Char _Role;
        public Char Role
        {
            get
            {
                return this._Role;
            }
        }

        public ImprovementActionTeamMember(String Tenant, int iActID, int iActYear, String iActUser)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<(int ImprovementActionID, int ImprovementActionYear, String User, String Role)>("SELECT ImprovementActionID, ImprovementActionYear, user, role FROM "
                + " ImprovementActions_Team WHERE ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear"
                + " AND user = @pUser", new { pIAID = iActID, pIAYear = iActYear, pUser = iActUser });
            if(row.User != null)
            {
                this._ImprovementActionID = row.ImprovementActionID;
                this._ImprovementActionYear = row.ImprovementActionYear;
                if(row.User != null)
                {
                    this._User = row.User;
                }
                if(row.Role != null)
                {
                    this._Role = row.Role[0];
                }
            }
            conn.Close();
        }

        public ImprovementActionTeamMember(String Tenant, int iActID, int iActYear, String iActUser, Char iActRole)
        {
            this.Tenant = Tenant;

            this._ImprovementActionID = iActID;
            this._ImprovementActionYear = iActYear;
            this._User = iActUser;
            this._Role = iActRole;
        }
    }

    public class CorrectiveAction
    {
        protected String Tenant;

        public String log;

        private class CorrectiveActionRow
        {
            public int ID { get; set; }
            public int ImprovementActionID { get; set; }
            public int ImprovementActionYear { get; set; }
            public String Description { get; set; }
            public double? LeadTimeExpected { get; set; }
            public DateTime? EarlyStart { get; set; }
            public DateTime? LateStart { get; set; }
            public DateTime? EarlyFinish { get; set; }
            public DateTime? LateFinish { get; set; }
            public String Status { get; set; }
        }

        private int _CorrectiveActionID;
        public int CorrectiveActionID
        {
            get
            {
                return this._CorrectiveActionID;
            }
        }
        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET Description = @pDescription"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pDescription = value, pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._Description = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private Double _LeadTimeExpected;
        public Double LeadTimeExpected
        {
            get
            {
                return this._LeadTimeExpected;
            }
            set
            {
                if(value >=0)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET LeadTimeExpected = @pLeadTimeExpected"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pLeadTimeExpected = value, pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._LeadTimeExpected = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            }
        }
        private DateTime _EarlyStart;
        public DateTime EarlyStart
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyStart, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET EarlyStart = @pEarlyStart"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pEarlyStart = curr.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._EarlyStart = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private DateTime _LateStart;
        public DateTime LateStart
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateStart, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET LateStart = @pLateStart"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pLateStart = curr.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._LateStart = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private DateTime _EarlyFinish;
        public DateTime EarlyFinish
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyFinish, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET EarlyFinish = @pEarlyFinish"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pEarlyFinish = curr.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._EarlyFinish = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        private DateTime _LateFinish;
        public DateTime LateFinish
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateFinish, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                DateTime curr = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET LateFinish = @pLateFinish"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pLateFinish = curr.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._LateFinish = curr;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        /* O == Opened
         * I = Running
         * C == Closed
         */
        private Char _Status;
        public Char Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if(Status == 'I' || Status == 'C' || Status == 'O')
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET Status = @pStatus"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pStatus = value.ToString(), pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            }
        }

        private DateTime _EndDateReal;
        public DateTime EndDateReal
        {
            get { return this._EndDateReal; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "UPDATE CorrectiveActions SET EndDateReal = @pEndDateReal"
                + " WHERE ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pEndDateReal = value.ToString("yyyy-MM-dd HH:mm:ss"), pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    this._EndDateReal = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public List<CorrectiveActionTeamMember> TeamMembers;

        public List<CorrectiveActionTask> Tasks;

        public CorrectiveAction(String Tenant, int iActID, int iActYear, int CorrectiveActionID)
        {
            this.Tenant = Tenant;

            this.TeamMembers = new List<CorrectiveActionTeamMember>();
            this.Tasks = new List<CorrectiveActionTask>();

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT ID, ImprovementActionID, ImprovementActionYear, Description, LeadTimeExpected, "
                + "EarlyStart, LateStart, EarlyFinish, LateFinish, Status FROM CorrectiveActions WHERE "
                + "ID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear";
            var row = conn.QueryFirstOrDefault<CorrectiveActionRow>(sql, new { pID = CorrectiveActionID, pIAID = iActID, pIAYear = iActYear });
            if(row != null)
            {
                this._CorrectiveActionID = row.ID;
                this._ImprovementActionID = row.ImprovementActionID;
                this._ImprovementActionYear = row.ImprovementActionYear;
                if(row.Description != null)
                {
                    this._Description = row.Description;
                }
                if(row.LeadTimeExpected.HasValue)
                {
                    this._LeadTimeExpected = row.LeadTimeExpected.Value;
                }
                if (row.EarlyStart.HasValue)
                {
                    this._EarlyStart = row.EarlyStart.Value;
                }
                if (row.LateStart.HasValue)
                {
                    this._LateStart = row.LateStart.Value;
                }
                if (row.EarlyFinish.HasValue)
                {
                    this._EarlyFinish = row.EarlyFinish.Value;
                }
                if (row.LateFinish.HasValue)
                {
                    this._LateFinish = row.LateFinish.Value;
                }
                this._Status = row.Status[0];
            }
            else
            {
                this._CorrectiveActionID = -1;
                this._ImprovementActionID = -1;
                this._ImprovementActionYear = -1;
            }
            conn.Close();
        }

        public void loadTeamMembers()
        {
            this.TeamMembers = new List<CorrectiveActionTeamMember>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(String User, String Role)>("SELECT User, role "
                + " FROM correctiveactions_team WHERE CorrectiveActionID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear"
                + " ORDER BY role, user", new { pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear });
            foreach(var r in rows)
            {
                this.TeamMembers.Add(new CorrectiveActionTeamMember(this.Tenant, this.ImprovementActionID, this.ImprovementActionYear, this.CorrectiveActionID, r.User, r.Role[0]));
            }
            conn.Close();
        }

        public Boolean MemberAdd(String username, Char role)
        {
            Boolean ret = false;
            if (this.CorrectiveActionID != -1 && this.ImprovementActionID != -1 && this.ImprovementActionYear != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "INSERT INTO CorrectiveActions_Team(CorrectiveActionID, ImprovementActionID, ImprovementActionYear, user, role) "
                    + " VALUES(@pID, @pIAID"
                    + ", @pIAYear, "
                    + "@pUser, @pRole)";
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    conn.Execute(sql, new { pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear, pUser = username, pRole = role.ToString() }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    tr.Rollback();
                    this.log = ex.Message + " " + sql;
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean MemberRemove(String username)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "DELETE FROM correctiveactions_team WHERE CorrectiveActionID = @pID"
                + " AND ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear"
               + " AND user = @pUser";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear, pUser = username }, tr);
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message + " " + sql;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public void loadTasks()
        {
            this.Tasks = new List<CorrectiveActionTask>();
            if(this.ImprovementActionYear!=-1 && this.ImprovementActionID != -1 && this.CorrectiveActionID!=-1)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var ids = conn.Query<int>("SELECT TaskID, Description, User, Date FROM correctiveactions_tasks WHERE "
                + " ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear"
                + " AND CorrectiveActionID = @pID"
                + " ORDER BY Date", new { pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear, pID = this.CorrectiveActionID });
                foreach(var id in ids)
                {
                    this.Tasks.Add(new CorrectiveActionTask(this.Tenant, this.ImprovementActionID,
                        this.ImprovementActionYear,
                        this.CorrectiveActionID,
                        id));
                }
                conn.Close();
            }
        }

        public Boolean TaskAdd(String description, UserAccount usr, DateTime date)
        {
            Boolean ret = false;
            if (this.CorrectiveActionID!= -1 && this.ImprovementActionID!=-1 && this.ImprovementActionYear > 1970 &&
                usr != null && usr.userId.Length > 0 && date!=null && date > new DateTime(1970, 1, 1))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT MAX(TaskID) FROM correctiveactions_tasks WHERE CorrectiveActionID = @pID"
                    + " AND ImprovementActionID = @pIAID"
                    + " AND ImprovementActionYear = @pIAYear";
                int? max = conn.QueryFirstOrDefault<int?>(sql, new { pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear });
                int tskID = 0;
                if(max.HasValue)
                {
                    tskID = max.Value + 1;
                }
                sql = "INSERT INTO CorrectiveActions_Tasks(ImprovementActionID, ImprovementActionYear, "
                    + "CorrectiveActionID, TaskID, Description, User, Date) "
                    + " VALUES(" 
                    + "@pIAID, " 
                    + "@pIAYear, " 
                    + "@pID, "
                    + "@pTaskID, "
                    + "@pDescription, "
                    + "@pUser, "
                    + "@pDate"
                    + ")";
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    conn.Execute(sql, new { pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear, pID = this.CorrectiveActionID, pTaskID = tskID, pDescription = description, pUser = usr.userId, pDate = date.ToString("yyyy-MM-dd HH:mm:ss") }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    tr.Rollback();
                    this.log = ex.Message + " " + sql;
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean TaskRemove(int idTask)
        {
            Boolean ret = false;
            if(this.CorrectiveActionID!=-1 && this.ImprovementActionID!=-1 && this.ImprovementActionYear>1970)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "DELETE FROM correctiveactions_tasks WHERE TaskID = @pTaskID"
                    + " AND CorrectiveActionID = @pID"
                    + " AND ImprovementActionID = @pIAID"
                    + " AND ImprovementActionYear = @pIAYear";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { pTaskID = idTask, pID = this.CorrectiveActionID, pIAID = this.ImprovementActionID, pIAYear = this.ImprovementActionYear }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    this.log = ex.Message + " " + sql;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }
    }

    public class CorrectiveActionTeamMember
    {
        protected String Tenant;

        public String log;

        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private int _CorrectiveActionID;
        public int CorrectiveActionID
        {
            get
            {
                return this._CorrectiveActionID;
            }
        }
        private String _User;
        public String User
        {
            get
            {
                return this._User;
            }
        }

        /* E == Executioner --> can create, edit, delete tasks
         * H == Helper --> can help the Executioner, read-only access to information.
         */
        private Char _Role;
        public Char Role
        {
            get
            {
                return this._Role;
            }
        }

        public CorrectiveActionTeamMember(String Tenant, int iActID, int iActYear, int cActID, String iActUser)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<(int ImprovementActionID, int ImprovementActionYear, int CorrectiveActionID, String User, String Role)>("SELECT ImprovementActionID, ImprovementActionYear, CorrectiveActionID, user, role FROM "
                + " ImprovementActions_Team WHERE ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear"
                + " AND CorrectiveActionID = @pID"
                + " AND user = @pUser", new { pIAID = iActID, pIAYear = iActYear, pID = cActID, pUser = iActUser });
            if (row.User != null)
            {
                this._ImprovementActionID = row.ImprovementActionID;
                this._ImprovementActionYear = row.ImprovementActionYear;
                this._CorrectiveActionID = row.CorrectiveActionID;
                if (row.User != null)
                {
                    this._User = row.User;
                }
                if (row.Role != null)
                {
                    this._Role = row.Role[0];
                }
            }
            conn.Close();
        }

        public CorrectiveActionTeamMember(String Tenant, int iActID, int iActYear, int cActID, String iActUser, Char iActRole)
        {
            this.Tenant = Tenant;

            this._ImprovementActionID = iActID;
            this._ImprovementActionYear = iActYear;
            this._CorrectiveActionID = cActID;
            this._User = iActUser;
            this._Role = iActRole;
        }
    }

    public class CorrectiveActionTask
    {
        protected String Tenant;

        private int _ImprovementActionID;
        public int ImprovementActionID
        {
            get
            {
                return this._ImprovementActionID;
            }
        }
        private int _ImprovementActionYear;
        public int ImprovementActionYear
        {
            get
            {
                return this._ImprovementActionYear;
            }
        }
        private int _CorrectiveActionID;
        public int CorrectiveActionID
        {
            get
            {
                return this._CorrectiveActionID;
            }
        }
        private int _TaskID;
        public int TaskID
        {
            get
            {
                return this._TaskID;
            }
        }
        private String _Description;
        public String Description
        {
            get
            {
                return this._Description;
            }
        }
        private String _User;
        public String User
        {
            get
            {
                return this._User;
            }
        }
        private DateTime _Date;
        public DateTime Date
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Date, fuso.tzFusoOrario);
            }
        }

        public CorrectiveActionTask(String Tenant, int iActID, int iActYear, int cActID, int tskID)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<(String Description, String User, DateTime? Date)>("SELECT Description, User, Date FROM correctiveactions_tasks WHERE "
                + " ImprovementActionID = @pIAID"
                + " AND ImprovementActionYear = @pIAYear"
                + " AND CorrectiveActionID = @pID"
                + " AND TaskID = @pTaskID", new { pIAID = iActID, pIAYear = iActYear, pID = cActID, pTaskID = tskID });
            if(row.Description != null)
            {
                this._ImprovementActionID = iActID;
                this._ImprovementActionYear = iActYear;
                this._CorrectiveActionID = cActID;
                this._TaskID = tskID;
                if(row.Description != null)
                {
                    this._Description = row.Description;
                }
                if (row.User != null)
                {
                    this._User = row.User;
                }
                if (row.Date.HasValue)
                {
                    this._Date = row.Date.Value;
                }
            }
            else
            {
                this._ImprovementActionID = -1;
                this._ImprovementActionYear = -1;
                this._CorrectiveActionID = -1;
                this._TaskID = -1;
            }
            conn.Close();
        }
    }

    public class ImprovementActionsEvents
    {
        protected String Tenant;

        public List<ImprovementAction> LateImprovementActions;
        public ImprovementActionsEvents(String Tenant)
        {
            this.LateImprovementActions = new List<ImprovementAction>();
        }

        public void loadLateImprovementActions()
        {
            FusoOrario fuso = new FusoOrario(this.Tenant);
            String tzOffset = "";
            tzOffset = fuso.tzFusoOrario.BaseUtcOffset.Ticks >= 0 ? "+" : "";
            tzOffset += fuso.tzFusoOrario.BaseUtcOffset.Hours + ":" + fuso.tzFusoOrario.BaseUtcOffset.Minutes;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int Year)>("SELECT ID, Year FROM improvementactions WHERE status <> 'C' AND "
                + " CONVERT_TZ(EndDateExpected, '+00:00', @pOffset) < @pToday ORDER BY EndDateExpected", new { pOffset = tzOffset, pToday = DateTime.UtcNow.ToString("yyyy-MM-dd") });
            foreach(var r in rows)
            {
                this.LateImprovementActions.Add(new ImprovementAction(this.Tenant, r.ID, r.Year));
            }
            conn.Close();
        }

        
    }

    public class CorrectiveActionsEvents
    {
        protected String Tenant;

        public List<CorrectiveAction> NotFinishedCorrectiveActions;
        public List<CorrectiveAction> NotStartedCorrectiveActions;
        public CorrectiveActionsEvents(String Tenant)
        {
            this.NotStartedCorrectiveActions = new List<CorrectiveAction>();
            this.NotFinishedCorrectiveActions = new List<CorrectiveAction>();
        }

        public void loadNotStartedCorrectiveActions()
        {
            FusoOrario fuso = new FusoOrario(this.Tenant);
            String tzOffset = "";
            tzOffset = fuso.tzFusoOrario.BaseUtcOffset.Ticks >= 0 ? "+" : "";
            tzOffset += fuso.tzFusoOrario.BaseUtcOffset.Hours + ":" + fuso.tzFusoOrario.BaseUtcOffset.Minutes;

            this.NotStartedCorrectiveActions = new List<CorrectiveAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int ImprovementActionID, int ImprovementActionYear)>("SELECT ID, ImprovementActionID, ImprovementActionYear FROM correctiveactions "
                + " WHERE status = 'O' AND CONVERT_TZ(EarlyStart, '+00:00', @pOffset)"
                + " < @pToday AND @pToday"
                + " <= CONVERT_TZ(LateFinish, '+00:00', @pOffset) ORDER BY LateStart", new { pOffset = tzOffset, pToday = DateTime.UtcNow.ToString("yyyy-MM-dd") });
            foreach (var r in rows)
            {
                this.NotStartedCorrectiveActions.Add(new CorrectiveAction(this.Tenant, r.ImprovementActionID, r.ImprovementActionYear, r.ID));
            }
            conn.Close();
        }

        public void loadNotFinishedCorrectiveActions()
        {
            FusoOrario fuso = new FusoOrario(this.Tenant);
            String tzOffset = "";
            tzOffset = fuso.tzFusoOrario.BaseUtcOffset.Ticks >= 0 ? "+" : "";
            tzOffset += fuso.tzFusoOrario.BaseUtcOffset.Hours + ":" + fuso.tzFusoOrario.BaseUtcOffset.Minutes;

            this.NotFinishedCorrectiveActions = new List<CorrectiveAction>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int ImprovementActionID, int ImprovementActionYear)>("SELECT ID, ImprovementActionID, ImprovementActionYear FROM correctiveactions "
                + " WHERE status <> 'C' AND CONVERT_TZ(LateFinish, '+00:00', @pOffset) < @pToday ORDER BY LateFinish", new { pOffset = tzOffset, pToday = DateTime.UtcNow.ToString("yyyy-MM-dd") });
            foreach (var r in rows)
            {
                this.NotFinishedCorrectiveActions.Add(new CorrectiveAction(this.Tenant, r.ImprovementActionID, r.ImprovementActionYear, r.ID));
            }
            conn.Close();
        }
    }

    public struct ImprovementActionAnalysisStruct
    {
        public int ID;
        public int Year;
        public DateTime OpeningDate;
        public char Status;
        public DateTime EndDateExpected;
        public DateTime EndDateReal;
    }

    public class ImprovementActionAnalysis
    {
        protected String Tenant;

        public List<ImprovementActionAnalysisStruct> IAList;

        public ImprovementActionAnalysis(String Tenant)
        {
            this.Tenant = Tenant;
            this.IAList = new List<ImprovementActionAnalysisStruct>();
        }

        public void loadIAList()
        {
            this.IAList = new List<ImprovementActionAnalysisStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int ID, int Year, DateTime OpeningDate, String Status, DateTime? EndDateExpected, DateTime? EndDateReal)>("SELECT ID, Year, OpeningDate, Status, EndDateExpected, EndDateReal From improvementactions");
            foreach(var r in rows)
            {
                ImprovementActionAnalysisStruct curr = new ImprovementActionAnalysisStruct();
                curr.ID = r.ID;
                curr.Year = r.Year;
                curr.OpeningDate = r.OpeningDate;
                curr.Status = r.Status[0];
                if(r.EndDateExpected.HasValue)
                { 
                curr.EndDateExpected = r.EndDateExpected.Value;
                }
                if(r.EndDateReal.HasValue)
                { 
                curr.EndDateReal = r.EndDateReal.Value;
                }
                this.IAList.Add(curr);
            }
            conn.Close();
        }
    }
}