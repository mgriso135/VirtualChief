using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Dapper;

namespace KIS.App_Sources
{
    public class NoProductiveTasks
    {
        protected String Tenant; 

        public String log;

        public List<NoProductiveTask> TaskList;

        public NoProductiveTasks(String tenant)
        {
            this.Tenant = tenant;
            this.TaskList = new List<NoProductiveTask>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT id FROM noproductivetasks WHERE enabled=true ORDER BY isdefault DESC, name");
            foreach (var id in rows)
            {
                this.TaskList.Add(new NoProductiveTask(this.Tenant, id));
            }
            conn.Close();
        }

        /* Returns:
         * -1 if generic error
         * TaskId if task added successfully
         * -2 if name is not valid
         * -3 if error while adding
         */
        public int Add(String name, String description)
        {
            int ret = -1;
            if(name.Length < 255)
            {
                Boolean def = false;
                if(this.TaskList.Count == 0)
                {
                    def = true;
                }

                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO noproductivetasks(name, description, enabled, creationdate) VALUES(@name, @description, @enabled, @creationdate)";

                try
                {
                    conn.Execute(sql, new { name = name, description = description, enabled = true, creationdate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") }, tr);
                    ret = conn.QueryFirstOrDefault<int?>("SELECT LAST_INSERT_ID()", transaction: tr) ?? 0;
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    tr.Rollback();
                    ret = -3;
                }
                conn.Close();
            }
            else
            {
                ret = -2;
            }
            return ret;
        }

        public void loadArchived()
        {
            this.TaskList = new List<NoProductiveTask>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT id FROM noproductivetasks WHERE enabled=false ORDER BY isdeault DESC, name");
            foreach (var id in rows)
            {
                this.TaskList.Add(new NoProductiveTask(this.Tenant, id));
            }
            conn.Close();
        }

        public void loadAll()
        {
            this.TaskList = new List<NoProductiveTask>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT id FROM noproductivetasks ORDER BY isdefault DESC, name");
            foreach (var id in rows)
            {
                this.TaskList.Add(new NoProductiveTask(this.Tenant, id));
            }
            conn.Close();
        }
    }

    public class NoProductiveTask
    {
        public String log;

        protected String Tenant;

        private int _ID;
        private String _Name;
        private String _Description;
        private Boolean _Enabled;
        private DateTime _CreationDate;
        private Boolean _IsDefault;

        private class NoProductiveTaskRow
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public String Description { get; set; }
            public Boolean Enabled { get; set; }
            public DateTime CreationDate { get; set; }
            public Boolean IsDefault { get; set; }
        }

        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if(this.ID != -1 && value.Length < 255)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE noproductivetasks SET name=@name WHERE id=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { name = value, id = this.ID }, tr);
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
        }

        public String Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE noproductivetasks SET description=@description WHERE id=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { description = value, id = this.ID }, tr);
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
        }

        public Boolean Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE noproductivetasks SET enabled=@enabled WHERE id=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { enabled = value, id = this.ID }, tr);
                        tr.Commit();
                        this._Enabled = value;

                        if(value == false)
                        {
                            this.IsDefault = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return this._CreationDate;
            }
        }

        public Boolean IsDefault
        {
            get { return this._IsDefault; }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sqlF = "UPDATE noproductivetasks SET isdefault=@isdefaultF";
                    string sqlT = "UPDATE noproductivetasks SET isdefault=@isdefaultT WHERE id=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sqlF, new { isdefaultF = false }, tr);
                        conn.Execute(sqlT, new { isdefaultT = value, id = this.ID }, tr);
                        tr.Commit();
                        this._IsDefault = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public NoProductiveTask(String tenant, int id)
        {
            this.Tenant = tenant;
            this._ID = -1;
            this._Name = "";
            this._Description = "";
            this._Enabled = false;
            this._CreationDate = new DateTime(1970,1,1);
            this._IsDefault = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<NoProductiveTaskRow>("SELECT id, name, description, enabled, creationdate, isdefault FROM noproductivetasks WHERE id=@id", new { id = id });
            if (row != null)
            {
                this._ID = row.ID;
                this._Name = row.Name;
                this._Description = row.Description;
                this._Enabled = row.Enabled;
                this._CreationDate = row.CreationDate;
                this._IsDefault = row.IsDefault;
            }
            conn.Close();
        }

        
    }
}