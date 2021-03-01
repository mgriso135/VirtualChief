using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace KIS.App_Sources
{
    public class Parts
    {
        public String log;
        private List<Part> _List;
        public List<Part> List
        {
            get { return this._List; }
        }

        public Parts()
        {
            this._List = new List<Part>();
        }

        public void loadParts()
        {
            this._List = new List<Part>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM parts ORDER BY partnumber";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this._List.Add(new Part(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadParts(Boolean actives)
        {
            this._List = new List<Part>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM parts WHERE enabled=@enabled ORDER BY partnumber";
            cmd.Parameters.AddWithValue("@enabled", actives);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._List.Add(new Part(rdr.GetInt32(0)));
            }
            rdr.Close();
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
            Part exists = new Part(partnumber);
            if(exists.ID == -1)
            { 
                if(name.Length < 255)
                { 
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO parts(partnumber, name, description, creationdate, createdby, lastmodifieddate, lastmodifiedby, enabled) "
                        + " VALUES(@partnumber, @name, @description, @creationdate, @createdby, @lastmodifieddate, @lastmodifiedby, @enabled)";
                    cmd.Parameters.AddWithValue("@partnumber", partnumber);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@creationdate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@createdby", createdby);
                    cmd.Parameters.AddWithValue("@lastmodifieddate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@lastmodifiedby", createdby);
                    cmd.Parameters.AddWithValue("@enabled", enabled);

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
        public String log;

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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE parts SET name=@value WHERE ID=@id";
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@id", this.ID);
                        cmd.ExecuteNonQuery();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE parts SET description=@value WHERE ID=@id";
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@id", this.ID);
                        cmd.ExecuteNonQuery();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        cmd.CommandText = "UPDATE parts SET enabled=@value WHERE ID=@id";
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@id", this.ID);
                        cmd.ExecuteNonQuery();
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

        public Part(int id)
        {
            this._ID = -1;
            this._PartNumber = "";
            this._Name = "";
            this._Description = "";
            this._CreationDate = new DateTime(1970, 1, 1);
            this._CreatedBy = "";
            this._LastModifiedDate = new DateTime(1970, 1, 1);
            this._LastModifiedBy = "";
            this._Enabled = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, partnumber, name, description, creationdate, createdby, lastmodifieddate, lastmodifiedby, enabled "
                + " FROM parts WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this._ID = rdr.GetInt32(0);
                this._PartNumber = rdr.GetString(1);
                this._Name = rdr.GetString(2);
                this._Description = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                this._CreationDate = rdr.GetDateTime(4);
                this._CreatedBy = rdr.GetString(5);
                this._LastModifiedDate = rdr.IsDBNull(6) ? new DateTime(1970,1,1) : rdr.GetDateTime(6);
                this._LastModifiedBy = rdr.IsDBNull(7) ? "" : rdr.GetString(7);
                this._Enabled = rdr.GetBoolean(8);
            }
            rdr.Close();
            conn.Close();
        }

        public Part(String partnumber)
        {
            this._ID = -1;
            this._PartNumber = "";
            this._Name = "";
            this._Description = "";
            this._CreationDate = new DateTime(1970, 1, 1);
            this._CreatedBy = "";
            this._LastModifiedDate = new DateTime(1970, 1, 1);
            this._LastModifiedBy = "";
            this._Enabled = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, partnumber, name, description, creationdate, createdby, lastmodifieddate, lastmodifiedby, enabled "
                + " FROM parts WHERE partnumber=@id";
            cmd.Parameters.AddWithValue("@id", partnumber);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._ID = rdr.GetInt32(0);
                this._PartNumber = rdr.GetString(1);
                this._Name = rdr.GetString(2);
                this._Description = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                this._CreationDate = rdr.GetDateTime(4);
                this._CreatedBy = rdr.GetString(5);
                this._LastModifiedDate = rdr.IsDBNull(6) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(6);
                this._LastModifiedBy = rdr.IsDBNull(7) ? "" : rdr.GetString(7);
                this._Enabled = rdr.GetBoolean(8);
            }
            rdr.Close();
            conn.Close();
        }
    }
}