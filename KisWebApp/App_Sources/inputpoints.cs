using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using KIS.App_Code;

namespace KIS.App_Sources
{
    public class InputPoints
    {
        public String log;
        private String _Tenant;
        public String Tenant { get { return this._Tenant; } }

        public List<InputPoint> list;

        public InputPoints(String tenant)
        {
            this._Tenant = "";
            this.list = new List<InputPoint>();
            if(tenant.Length > 0)
            {
                this._Tenant = tenant;   
            }
        }

        public void loadInputPoints()
        {
            if (this.Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id FROM inputpoints";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    InputPoint currip = new InputPoint(this.Tenant, rdr.GetInt32(0));
                    if(currip!=null && currip.id>=0)
                    {
                        this.list.Add(currip);
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * -2 if name or tenant are not valid
         * -1 if generic error
         * InputPointId if everything is ok
         */
        public int addInputPoint(String name, String description, int creator)
        {
            int ret = -1;
            if(name.Length > 0 && this.Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO inputpoints(name, description, creator) VALUES(@name, @description, @creator)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@creator", creator);
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    tr.Rollback();
                }
                if(ret == 1)
                {
                    cmd.CommandText = "SELECT LAST_INSERT_ID()";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if(rdr.Read())
                    { 
                        ret = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }
                conn.Close();
            }
            else
            {
                ret = -2;
            }
            return ret;
        }
    }

    public class InputPoint
    {
        public String log;
        private String _Tenant;
        public String Tenant { get { return this._Tenant; } }

        private int _id;
        public int id { get { return this._id; } }

        private String _name;
        public String name { get { return this._name; } 
            set
            {
                if(this.id>-1 && this.Tenant.Length > 0)
                {
                    String name = value.Length >= 255 ? value.Substring(0, 255) : value;
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE inputpoints SET name=@name WHERE id=@id";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@id", this.id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._name = name;
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        private String _description;
        public String description { 
            get { return this._description; }
            set
            {
                if (this.id > -1 && this.Tenant.Length > 0)
                {
                    String description = value.Length >= 255 ? value.Substring(0, 255) : value;
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE inputpoints SET description=@description WHERE id=@id";
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@id", this.id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._description = description;
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        private DateTime _CreationDate;
        public DateTime CreationDate { get { return this._CreationDate; } }

        private int _CreatorId;
        public int CreatorId { get { return this._CreatorId; } }

        private String _notes;
        public String notes { get { return this._notes; } }

        public List<InputPointDepartment> departments;
        public List<InputPointWorkstation> workstations;

        public InputPoint(String tenant, int id)
        {
            this._Tenant = tenant;
            this._id = -1;
            if (this._Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, name, description, creationdate, creator, notes FROM inputpoints WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    this._id = rdr.GetInt32(0);
                    this._name = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                    this._description = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                    this._CreationDate = rdr.IsDBNull(3) ? new DateTime(1970,1,1) : rdr.GetDateTime(3);
                    this._CreatorId = rdr.IsDBNull(4) ? -1 : rdr.GetInt32(4);
                    this._notes = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 2 if id not valid or tenant not defined
         * 3 if there are workstations or departments linked
         */
        public int delete()
        {
            int ret = 0;
            if(this.id>=0 && this.Tenant.Length > 0)
            {
                this.loadDepartments();
                this.loadWorkstations();
                if(this.departments.Count == 0 && this.workstations.Count == 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROm inputpoints WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", this.id);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
                else
                {
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        public void loadDepartments()
        {
            this.departments = new List<InputPointDepartment>();
            if(this.id >= 0 && this.Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT departmentid, creationdate FROM inputpoints_departments WHERE inputpointId=@inputpointid";
                cmd.Parameters.AddWithValue("@inputpointid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    InputPointDepartment ipdept = new InputPointDepartment(this.Tenant, this.id, rdr.GetInt32(0));
                    if(ipdept.departmentId>= 0 && ipdept.inputpointId >= 0)
                    { 
                        this.departments.Add(ipdept);
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void loadWorkstations()
        {
            this.workstations = new List<InputPointWorkstation>();
            if (this.id >= 0 && this.Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT workstationid, creationdate FROM inputpoints_workstations WHERE inputpointId=@inputpointid";
                cmd.Parameters.AddWithValue("@inputpointid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InputPointWorkstation ipwst = new InputPointWorkstation(this.Tenant, this.id, rdr.GetInt32(0));
                    if (ipwst.workstationId >= 0 && ipwst.inputpointId >= 0)
                    {
                        this.workstations.Add(ipwst);
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if tenant not set or invalid department
         * 3 if error while writing to database
         * 4 if department is already associated with this input point
         */
        public int addDepartment(Reparto dept)
        {
            int ret = 0;
            if(this.Tenant.Length >0 && dept!=null && dept.id>= 0 && this.id >= 0)
            {
                this.loadDepartments();
                bool found = false;
                try
                { 
                    var currdept = this.departments.First(x => x.departmentId == dept.id);
                    found = true;
                }
                catch(Exception ex)
                {
                    found = false;
                }

                if(!found)
                { 
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO inputpoints_departments(inputpointid, departmentid) VALUES(@ipid, @deptid)";
                    cmd.Parameters.AddWithValue("@ipid", this.id);
                    cmd.Parameters.AddWithValue("@deptid", dept.id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret = 1;
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                        ret = 3;
                    }
                    conn.Close();
                }
                else
                {
                    ret = 4;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything is ok
         * 2 if tenant not set or invalid workstation
         * 3 if error while writing to database
         * 4 if workstation is already associated with this input point
         */
        public int addWorkstation(Postazione wst)
        {
            int ret = 0;
            if (this.Tenant.Length > 0 && wst != null && wst.id >= 0 && this.id >= 0)
            {
                this.loadWorkstations();
                bool found = false;
                try
                {
                    var currdept = this.workstations.FirstOrDefault(x => x.workstationId == wst.id);
                    found = true;
                }
                catch (Exception ex)
                {
                    found = false;
                }

                if (!found)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO inputpoints_workstations(inputpointid, workstationid) VALES(@ipid, @wstid)";
                    cmd.Parameters.AddWithValue("@ipid", this.id);
                    cmd.Parameters.AddWithValue("@wstid", wst.id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                        ret = 3;
                    }
                    conn.Close();
                }
                else
                {
                    ret = 4;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

    }


    public class InputPointDepartment
    {
        private String _Tenant;
        public String Tenant { get { return this._Tenant; } }

        private int _inputpointId;
        public int inputpointId { get { return this._inputpointId; } }

        private String _inputpointName;
        public String inputpointName { get { return this._inputpointName; } }
        private String _inputpointDescription;
        public String inputpointDescription { get { return this._inputpointDescription; } }

        private DateTime _inputpointCreationDate;
        public DateTime inputpointCreationDate { get { return this._inputpointCreationDate; } }

        private int _inputpointCreatorId;
        public int inputpointCreatorId { get { return this._inputpointCreatorId; } }

        private String _inputpointNotes;
        public String inputpointNotes { get { return this._inputpointNotes; } }

        private int _departmentId;
        public int departmentId { get { return this._departmentId; } }

        private String _departmentName;
        public String departmentName { get { return this._departmentName; } }

        private String _departmentDescription;
        public String departmentDescription { get { return this._departmentDescription; } }

        private String _departmentTimezone;
        public String departmentTimezone { get { return this._departmentTimezone; } }

        public InputPointDepartment(String tenant, int inputpointId, int departmentId)
        {
            this._departmentId = -1;
            this._inputpointId = -1;

            this._Tenant = tenant;
            if(this._Tenant.Length>0 && inputpointId >= 0 && departmentId >= 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT inputpoints.id, inputpoints.name, inputpoints.description, inputpoints.creationdate, "
                    + "inputpoints.creator, inputpoints.notes, inputpoints_departments.creationdate as linkCreationDate, "
                    + "reparti.idreparto, reparti.nome, reparti.descrizione, reparti.timezone "
                    + " FROM inputpoints INNER JOIN inputpoints_departments ON(inputpoints.id = inputpoints_departments.inputpointid) "
                    + " INNER JOIN reparti ON(inputpoints_departments.departmentId = reparti.idreparto) "
                    + " WHERE reparti.idreparto = @idreparto AND inputpoints.id = @idinputpoint";
                cmd.Parameters.AddWithValue("@idreparto", departmentId);
                cmd.Parameters.AddWithValue("@idinputpoint", inputpointId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    this._inputpointId = rdr.GetInt32(0);
                    this._inputpointName = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                    this._departmentDescription = rdr.IsDBNull(2) ?"": rdr.GetString(2);
                    this._inputpointCreationDate = rdr.IsDBNull(3) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(3);
                    this._inputpointCreatorId = rdr.GetInt32(4);
                    this._inputpointNotes = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                    // linkCreationDate 6
                    this._departmentId = rdr.GetInt32(7);
                    this._departmentName = rdr.GetString(8);
                    this._departmentDescription = rdr.IsDBNull(9) ? "" : rdr.GetString(9);
                    this._departmentTimezone = rdr.IsDBNull(10) ? "" : rdr.GetString(10);
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if InputPointDepartment delete successfully
         * 2 if tenant, department or input point not set
         * 3 if error while deleting
         */
        public int delete()
        {
            int ret = 0;
            if(this.Tenant.Length >0 && this.departmentId>=0 && this.inputpointId >=0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM inputpoints_departments WHERE inputpointid=@ipid AND departmentid=@deptid";
                cmd.Parameters.AddWithValue("@ipid", this.inputpointId);
                cmd.Parameters.AddWithValue("@deptid", this.departmentId);
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
                }
                catch(Exception ex)
                {
                    ret = 3;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                ret = 2;
            }
            return ret;
        }
    }

    public class InputPointWorkstation
    {
        private String _Tenant;
        public String Tenant { get { return this._Tenant; } }

        private int _inputpointId;
        public int inputpointId { get { return this._inputpointId; } }

        private String _inputpointName;
        public String inputpointName { get { return this._inputpointName; } }
        private String _inputpointDescription;
        public String inputpointDescription { get { return this._inputpointDescription; } }

        private DateTime _inputpointCreationDate;
        public DateTime inputpointCreationDate { get { return this._inputpointCreationDate; } }

        private int _inputpointCreatorId;
        public int inputpointCreatorId { get { return this._inputpointCreatorId; } }

        private String _inputpointNotes;
        public String inputpointNotes { get { return this._inputpointNotes; } }

        private int _workstationId;
        public int workstationId { get { return this._workstationId; } }

        private String _workstationName;
        public String workstationName { get { return this._workstationName; } }

        private String _workstationDescription;
        public String workstationDescription { get { return this._workstationDescription; } }

        public InputPointWorkstation(String tenant, int inputpointId, int workstationId)
        {
            this._workstationId = -1;
            this._inputpointId = -1;

            this._Tenant = tenant;
            if (this._Tenant.Length > 0 && inputpointId >= 0 && workstationId >= 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT inputpoints.id, inputpoints.name, inputpoints.description, inputpoints.creationdate, "
                    + " inputpoints.creator, inputpoints.notes, inputpoints_workstations.creationdate as linkCreationDate, "
                    + " postazioni.idpostazioni, postazioni.name, postazioni.description, postazioni.barcodeAutoCheckIn "
                    + " FROM "
                    + " inputpoints INNER JOIN inputpoints_workstations ON(inputpoints.id = inputpoints_workstations.workstationid) "
                    + " INNER JOIN postazioni ON(inputpoints_workstations.workstationId = postazioni.idpostazioni) "
                    + " WHERE postazioni.idpostazioni = @workstationid AND inputpoints.id = 1";

                cmd.Parameters.AddWithValue("@workstationid", workstationId);
                cmd.Parameters.AddWithValue("@idinputpoint", inputpointId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    this._inputpointId = rdr.GetInt32(0);
                    this._inputpointName = rdr.GetString(1);
                    this._workstationDescription = rdr.GetString(2);
                    this._inputpointCreationDate = rdr.GetDateTime(3);
                    this._inputpointCreatorId = rdr.GetInt32(4);
                    this._inputpointNotes = rdr.GetString(5);
                    // linkCreationDate 6
                    this._workstationId = rdr.GetInt32(7);
                    this._workstationName = rdr.GetString(8);
                    this._workstationDescription = rdr.GetString(9);
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if InputPointDepartment delete successfully
         * 2 if tenant, department or input point not set
         * 3 if error while deleting
         */
        public int delete()
        {
            int ret = 0;
            if (this.Tenant.Length > 0 && this.workstationId >= 0 && this.inputpointId >= 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM inputpoints_workstations WHERE inputpointid=@ipid AND workstationid=@wstid";
                cmd.Parameters.AddWithValue("@ipid", this.inputpointId);
                cmd.Parameters.AddWithValue("@wsttid", this.workstationId);
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
                }
                catch (Exception ex)
                {
                    ret = 3;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                ret = 2;
            }
            return ret;
        }
    }
}