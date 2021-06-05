using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

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
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM noproductivetasks WHERE enabled=true ORDER BY isdefault DESC, name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.TaskList.Add(new NoProductiveTask(rdr.GetInt32(0)));
            }
            rdr.Close();
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
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO noproductivetasks(name, description, enabled, creationdate) VALUES(@name, @description, @enabled, @creationdate)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@enabled", true);
                cmd.Parameters.AddWithValue("@creationdate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

                try
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "SELECT LAST_INSERT_ID()";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if(rdr.Read())
                    {
                        ret = rdr.GetInt32(0); 
                    }
                    rdr.Close();
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
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM noproductivetasks WHERE enabled=false ORDER BY isdeault DESC, name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.TaskList.Add(new NoProductiveTask(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadAll()
        {
            this.TaskList = new List<NoProductiveTask>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM noproductivetasks ORDER BY isdefault DESC, name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.TaskList.Add(new NoProductiveTask(rdr.GetInt32(0)));
            }
            rdr.Close();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE noproductivetasks SET name=@name WHERE id=@id";
                    cmd.Parameters.AddWithValue("@name", value);
                    cmd.Parameters.AddWithValue("@id", this.ID);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE noproductivetasks SET description=@description WHERE id=@id";
                    cmd.Parameters.AddWithValue("@description", value);
                    cmd.Parameters.AddWithValue("@id", this.ID);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE noproductivetasks SET enabled=@enabled WHERE id=@id";
                    cmd.Parameters.AddWithValue("@enabled", value);
                    cmd.Parameters.AddWithValue("@id", this.ID);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.CommandText = "UPDATE noproductivetasks SET isdefault=@isdefaultF";
                        cmd.Parameters.AddWithValue("@isdefaultF", false);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "UPDATE noproductivetasks SET isdefault=@isdefaultT WHERE id=@id";
                        cmd.Parameters.AddWithValue("@isdefaultT", value);
                        cmd.Parameters.AddWithValue("@id", this.ID);
                        cmd.ExecuteNonQuery();
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

        public NoProductiveTask(int id)
        {
            this._ID = -1;
            this._Name = "";
            this._Description = "";
            this._Enabled = false;
            this._CreationDate = new DateTime(1970,1,1);
            this._IsDefault = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, name, description, enabled, creationdate, isdefault FROM noproductivetasks WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                this._ID = rdr.GetInt32(0);
                this._Name = rdr.GetString(1);
                this._Description = rdr.GetString(2);
                this._Enabled = rdr.GetBoolean(3);
                this._CreationDate = rdr.GetDateTime(4);
                this._IsDefault = rdr.GetBoolean(5);
            }
            rdr.Close();
            conn.Close();
        }

        
    }
}