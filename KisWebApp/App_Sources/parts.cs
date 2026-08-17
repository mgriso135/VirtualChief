using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Dapper;

namespace KIS.App_Sources
{
    public class Parts
    {
        protected String Tenant;

        public String log;
        private List<Part> _List;
        public List<Part> List
        {
            get { return this._List; }
        }

        public Parts(String tenant)
        {
            this._List = new List<Part>();
            this.Tenant = tenant;
        }

        public void loadParts()
        {
            this._List = new List<Part>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT id FROM parts ORDER BY partnumber");
            foreach (var id in rows)
            {
                this._List.Add(new Part(this.Tenant, id));
            }
            conn.Close();
        }

        public void loadParts(Boolean actives)
        {
            this._List = new List<Part>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT id FROM parts WHERE enabled=@enabled ORDER BY partnumber", new { enabled = actives });
            foreach (var id in rows)
            {
                this._List.Add(new Part(this.Tenant, id));
            }
            conn.Close();
        }

        /* Returns:
         * partId if everything is ok or if partnumber
         * -1 if generic error
         * -2 if part name is not correct
         */
        public int add(String partnumber, String name, String description, String createdby, Boolean enabled)
        {
            int ret = -1;
            Part exists = new Part(this.Tenant, partnumber);
            if(exists.ID == -1)
            { 
                if(name.Length < 255)
                { 
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "INSERT INTO parts(partnumber, name, description, creationdate, createdby, lastmodifieddate, lastmodifiedby, enabled) "
                        + " VALUES(@partnumber, @name, @description, @creationdate, @createdby, @lastmodifieddate, @lastmodifiedby, @enabled)";

                    try
                    {
                        conn.Execute(sql, new { partnumber = partnumber, name = name, description = description, creationdate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), createdby = createdby, lastmodifieddate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), lastmodifiedby = createdby, enabled = enabled }, tr);
                        ret = conn.QueryFirstOrDefault<int?>("SELECT LAST_INSERT_ID()", transaction: tr) ?? 0;
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                        ret = -1;
                    }

                    conn.Close();
                }
                else
                {
                    ret = -2;
                }
            }
            else
            {
                ret = exists.ID;
            }
            return ret;
        }
    }

    public class Part
    {
        protected String Tenant;

        public String log;

        private class PartRow
        {
            public int ID { get; set; }
            public String PartNumber { get; set; }
            public String Name { get; set; }
            public String Description { get; set; }
            public DateTime CreationDate { get; set; }
            public String CreatedBy { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public String LastModifiedBy { get; set; }
            public Boolean Enabled { get; set; }
        }

        private int _ID;
        public int ID { get { return this._ID; } }

        private String _PartNumber;
        public String PartNumber {  get { return this._PartNumber; } }

        private String _Name;
        public String Name { get { return this._Name; }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE parts SET name=@value WHERE ID=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { value = value, id = this.ID }, tr);
                        tr.Commit();
                        this._Name = value;
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

        private String _Description;
        public String Description { get { return this._Description; }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE parts SET description=@value WHERE ID=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { value = value, id = this.ID }, tr);
                        tr.Commit();
                        this._Description = value;
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

        private DateTime _CreationDate;
        public DateTime CreationDate { get { return this._CreationDate; } }

        private String _CreatedBy;
        public String CreatedBy { get { return this._CreatedBy; } }

        private DateTime _LastModifiedDate;
        public DateTime LastModifiedDate { get { return this._LastModifiedDate; } }

        private String _LastModifiedBy;
        public String LastModifiedBy { get { return this._LastModifiedBy; } }

        private Boolean _Enabled;
        public Boolean Enabled  { get { return this._Enabled; }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE parts SET enabled=@value WHERE ID=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { value = value, id = this.ID }, tr);
                        tr.Commit();
                        this._Enabled = value;
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

        public List<PartSupplier> Suppliers;

        public Part(String tenant, int id)
        {
            this.Tenant = tenant;
            this._ID = -1;
            this._PartNumber = "";
            this._Name = "";
            this._Description = "";
            this._CreationDate = new DateTime(1970, 1, 1);
            this._CreatedBy = "";
            this._LastModifiedDate = new DateTime(1970, 1, 1);
            this._LastModifiedBy = "";
            this._Enabled = false;
            this.Suppliers = new List<PartSupplier>();

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<PartRow>("SELECT id, partnumber, name, description, creationdate, createdby, lastmodifieddate, lastmodifiedby, enabled "
                + " FROM parts WHERE id=@id", new { id = id });
            if (row != null)
            {
                this._ID = row.ID;
                this._PartNumber = row.PartNumber;
                this._Name = row.Name;
                this._Description = row.Description ?? "";
                this._CreationDate = row.CreationDate;
                this._CreatedBy = row.CreatedBy;
                this._LastModifiedDate = row.LastModifiedDate.HasValue ? row.LastModifiedDate.Value : new DateTime(1970, 1, 1);
                this._LastModifiedBy = row.LastModifiedBy ?? "";
                this._Enabled = row.Enabled;
            }
            conn.Close();
        }

        public Part(String tenant, String partnumber)
        {
            this.Tenant = tenant;

            this._ID = -1;
            this._PartNumber = "";
            this._Name = "";
            this._Description = "";
            this._CreationDate = new DateTime(1970, 1, 1);
            this._CreatedBy = "";
            this._LastModifiedDate = new DateTime(1970, 1, 1);
            this._LastModifiedBy = "";
            this._Enabled = false;
            this.Suppliers = new List<PartSupplier>();

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<PartRow>("SELECT id, partnumber, name, description, creationdate, createdby, lastmodifieddate, lastmodifiedby, enabled "
                + " FROM parts WHERE partnumber=@id", new { id = partnumber });
            if (row != null)
            {
                this._ID = row.ID;
                this._PartNumber = row.PartNumber;
                this._Name = row.Name;
                this._Description = row.Description ?? "";
                this._CreationDate = row.CreationDate;
                this._CreatedBy = row.CreatedBy;
                this._LastModifiedDate = row.LastModifiedDate.HasValue ? row.LastModifiedDate.Value : new DateTime(1970, 1, 1);
                this._LastModifiedBy = row.LastModifiedBy ?? "";
                this._Enabled = row.Enabled;
            }
            conn.Close();
        }

        public void loadSuppliers()
        {
            this.Suppliers = new List<PartSupplier>();
            if(this.Tenant.Length > 0 && this.ID>=0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<String>("SELECT supplierid FROM parts_suppliers WHERE parts_suppliers.partid=@partid", new { partid = this.ID });
                foreach (var supplierId in rows)
                {
                    this.Suppliers.Add(new PartSupplier(this.Tenant, this.ID, supplierId));
                }
                conn.Close();
            }
        }
    }

    public class PartSupplier
    {
        private String Tenant;

        private class PartSupplierRow
        {
            public int partid { get; set; }
            public String supplierid { get; set; }
            public DateTime creationdate { get; set; }
            public Boolean enabled { get; set; }
            public String ragsociale { get; set; }
        }

        private int _PartId;
        public int PartId { get { return this._PartId; } }

        private String _SupplierId;
        public String SupplierId { get { return this._SupplierId; } }

        private DateTime _CreationDate;
        public DateTime CreationDate { get { return this._CreationDate; } }

        private Boolean _Enabled;
        public Boolean Enabled { get { return this._Enabled; } }

        private String _BusinessName;
        public String BusinessName { get { return this._BusinessName; } }

        public PartSupplier(String tenant, int partId, String supplierId)
        {
            
            this._PartId = -1;
            this._SupplierId = "";
            this._CreationDate = new DateTime(1970, 1, 1);
            this._Enabled = false;
            if(tenant .Length > 0 && partId >= 0 && supplierId.Length > 0)
            {
                this.Tenant = tenant;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var row = conn.QueryFirstOrDefault<PartSupplierRow>("SELECT partid, supplierid, creationdate, enabled, anagraficaclienti.ragsociale FROM parts_suppliers INNER JOIN anagraficaclienti "
                    + " ON (parts_suppliers.supplierid = anagraficaclienti.codice) WHERE anagraficaclienti.provider = true "
                    + " AND parts_suppliers.partid=@partid AND parts_suppliers.supplierid=@supplierid", new { partid = partId, supplierid = supplierId });
                if (row != null)
                {
                    this._PartId = row.partid;
                    this._SupplierId = row.supplierid;
                    this._CreationDate = row.creationdate;
                    this._Enabled = row.enabled;
                    this._BusinessName = row.ragsociale;
                }
                conn.Close();
            }
        }
    }
}