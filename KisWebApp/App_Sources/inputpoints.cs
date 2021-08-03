using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using KIS.App_Code;

namespace KIS.App_Sources
{
    public class InputPoint
    {
        public String log;
        private String _Tenant;
        public String Tenant { get { return this._Tenant; } }

        private int _id;
        public int id { get { return this._id; } }

        private String _name;
        public String name { get { return this._name; } }

        private String _description;
        public String description { get { return this._description; } }

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
                    this._name = rdr.GetString(1);
                    this._description = rdr.GetString(2);
                    this._CreationDate = rdr.GetDateTime(3);
                    this._CreatorId = rdr.GetInt32(4);
                    this._notes = rdr.GetString(5);
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void loadDepartments()
        {
            this.departments = new List<InputPointDepartment>();
        }

        public void loadWorkstations()
        {
            this.workstations = new List<InputPointWorkstation>();
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
                    /*this._id = rdr.GetInt32(0);
                    this._name = rdr.GetString(1);
                    this._description = rdr.GetString(2);
                    this._CreationDate = rdr.GetDateTime(3);
                    this._CreatorId = rdr.GetInt32(4);
                    this._notes = rdr.GetString(5);*/
                }
                rdr.Close();
                conn.Close();
            }
        }
    }

    public class InputPointWorkstation
    {
        private String _Tenant;
        public String Tenant { get { return this._Tenant; } }
    }
}