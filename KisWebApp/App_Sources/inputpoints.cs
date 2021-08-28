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

        private List<int> _RunningTasks;
        public List<int> RunningTasks { get { return this._RunningTasks; } }

        private List<int> _RunnableTasks;
        public List<int> RunnableTasks { get { return this._RunnableTasks; } }

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
                cmd.CommandText = "SELECT departmentid, creationdate FROM inputpoints_departments WHERE enabled IS true AND inputpointId=@inputpointid";
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

        /* enabled: '0' --> only disabled
         * enabled: '1' --> only enabled
         * enabled: 'A' --> both enabled and disabled
         */
        public void loadDepartments(Char cEnabled)
        {
            this.departments = new List<InputPointDepartment>();
            if (this.id >= 0 && this.Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT departmentid, creationdate FROM inputpoints_departments WHERE inputpointId=@inputpointid";
                switch (cEnabled)
                {
                    case '0': cmd.CommandText += " AND enabled is FALSE"; break;
                    case '1': cmd.CommandText += " AND enabled is TRUE"; break;
                    case 'A': break;
                    default: break;
                }
                cmd.Parameters.AddWithValue("@inputpointid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InputPointDepartment ipdept = new InputPointDepartment(this.Tenant, this.id, rdr.GetInt32(0));
                    if (ipdept.departmentId >= 0 && ipdept.inputpointId >= 0)
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
                cmd.CommandText = "SELECT workstationid, creationdate FROM inputpoints_workstations WHERE enabled IS true AND inputpointId=@inputpointid";
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

        /* enabled: '0' --> only disabled
         * enabled: '1' --> only enabled
         * enabled: 'A' --> both enabled and disabled
         */
        public void loadWorkstations(Char cEnabled)
        {
            this.workstations = new List<InputPointWorkstation>();
            if (this.id >= 0 && this.Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT workstationid, creationdate FROM inputpoints_workstations WHERE inputpointId=@inputpointid";
                switch(cEnabled)
                {
                    case '0': cmd.CommandText += " AND enabled is FALSE"; break;
                    case '1': cmd.CommandText += " AND enabled is TRUE"; break;
                    case 'A': break;
                    default: break;
                }
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
                this.loadDepartments('A');
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
                    InputPointDepartment ipdept = new InputPointDepartment(this.Tenant, this.id, dept.id);
                    if(ipdept!=null && ipdept.departmentId>-1 && ipdept.inputpointId>-1)
                    {
                        ipdept.enabled = true;
                        ret = 1;
                    }
                    else
                    {
                        ret = 4;
                    }
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
                this.loadWorkstations('A');
                bool found = false;
                try
                {
                    var currdept = this.workstations.First(x => x.workstationId == wst.id);
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
                    cmd.CommandText = "INSERT INTO inputpoints_workstations(inputpointid, workstationid) VALUES(@ipid, @wstid)";
                    cmd.Parameters.AddWithValue("@ipid", this.id);
                    cmd.Parameters.AddWithValue("@wstid", wst.id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret = 1;
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
                    InputPointWorkstation ipwst = new InputPointWorkstation(this.Tenant, this.id, wst.id);
                    if (ipwst != null && ipwst.workstationId > -1 && ipwst.inputpointId > -1)
                    {
                        ipwst.enabled = true;
                        ret = 1;
                    }
                    else
                    {
                        ret = 4;
                    }
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        public List<FreeMeasurement_Task> FreeMeasurementTasks;
        public void loadFreeMeasurementRunningTasks(Reparto dept)
        {
            this.FreeMeasurementTasks = new List<FreeMeasurement_Task>();
            int ret = 0;
            if (this.id >= 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT measurementid, taskid FROM "
                    + " (SELECT DISTINCT(CONCAT(freemeasurements_tasks.measurementid, '_', freemeasurements_tasks.taskid)), freemeasurements_tasks.measurementid AS measurementid, freemeasurements_tasks.taskid AS taskid, "
                    + " freemeasurements_tasks_events.eventtype FROM freemeasurements_tasks "
                    + " INNER JOIN freemeasurements_tasks_events ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND "
                    + " freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    + " INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
                    + " WHERE freemeasurements_tasks.status = 'I' "
                    + " AND freemeasurements_tasks_events.inputpoint = @inputpoint "
                    + " AND freemeasurements.departmentid = @deptid "
                    + " ORDER BY freemeasurements_tasks_events.eventdate)  runningtasks "
                    + " WHERE runningtasks.eventtype <> 'F' AND runningtasks.eventtype <> 'P'";
                cmd.Parameters.AddWithValue("@inputpoint", this.id.ToString());
                cmd.Parameters.AddWithValue("@deptid", dept.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.FreeMeasurementTasks.Add(new FreeMeasurement_Task(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void loadFreeMeasurementRunningTasks()
        {
            this.FreeMeasurementTasks = new List<FreeMeasurement_Task>();
            int ret = 0;
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT "
                 + " freemeasurements.id, "
                + " freemeasurements_tasks.taskid, "
                 + "   freemeasurements_tasks.name AS TaskName, "
                  + "   postazioni.name AS WorkstationName, "
                 + "    freemeasurements_tasks.quantity_planned, "
                 + "    measurementunits.type"
                 + "        FROM"
                 + "    (SELECT MAX(runningtasks.id) AS runningtasksid "
                + " FROM "
                + " (SELECT freemeasurements_tasks_events.id, freemeasurements_tasks_events.eventtype,"
                + " freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, freemeasurements_tasks_events.eventdate, "
                + " freemeasurements_tasks_events.inputpoint AS inputpoint "
                 + "            FROM freemeasurements_tasks "
                 + "             INNER JOIN freemeasurements_tasks_events "
                    + "         ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                       + "      INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
                  + "           WHERE 1=1 "
               // + " AND freemeasurements_tasks.status = 'I' "
               + "               AND freemeasurements_tasks_events.inputpoint = @inputpoint "
               //               + "               AND freemeasurements.departmentid = 0 "
               + "              ORDER BY freemeasurements_tasks_events.eventdate DESC) AS runningtasks "
               + "              GROUP BY runningtasks.taskid, runningtasks.measurementid, runningtasks.inputpoint) AS runningtasks2 "
               + " INNER JOIN freemeasurements_tasks_events AS freemeasurements_tasks_events2 ON(freemeasurements_tasks_events2.id = runningtasks2.runningtasksid) "
               + " INNER JOIN freemeasurements_tasks ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events2.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events2.taskid) "
               + " inner join freemeasurements ON(freemeasurements.id = freemeasurements_tasks.MeasurementId) "
               + " INNER JOIN measurementunits ON(measurementunits.id = freemeasurements.measurementUnit) "
               + " LEFT JOIN postazioni ON(postazioni.idpostazioni = freemeasurements_tasks.workstationid) "
                + " WHERE eventtype = 'I'";

                cmd.Parameters.AddWithValue("@inputpoint", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.FreeMeasurementTasks.Add(new FreeMeasurement_Task(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void loadTaskAvviati()
        {
            this._RunningTasks = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tasksproduzione.taskID, evento FROM tasksproduzione INNER JOIN registroeventitaskproduzione ON("
                + "tasksproduzione.taskID = registroeventitaskproduzione.task) WHERE tasksproduzione.status = 'I' "
                + " AND registroeventitaskproduzione.inputpoint=@ip ORDER BY registroeventitaskproduzione.data DESC";
            cmd.Parameters.AddWithValue("@ip", this.id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<int> DaNonInserire = new List<int>();
            while (rdr.Read())
            {
                log += "Task: " + rdr.GetInt32(0).ToString() + " " + rdr.GetChar(1);
                if (rdr.GetChar(1) == 'P' || rdr.GetChar(1) == 'F')
                {
                    log += " da non inserire<br/>";
                    DaNonInserire.Add(rdr.GetInt32(0));
                }
                else if (rdr.GetChar(1) == 'I')
                {
                    log += " da verificare --> ";
                    // Verifico che non sia nella lista di quelli da non inserire, e nemmeno in quella dei già inseriti!
                    bool checkN = false;
                    bool checkI = false;
                    for (int q = 0; q < DaNonInserire.Count; q++)
                    {
                        if (DaNonInserire[q] == rdr.GetInt32(0))
                        {
                            log += " da non inserire";
                            checkN = true;
                        }
                    }
                    for (int q = 0; q < this._RunningTasks.Count; q++)
                    {
                        if (this._RunningTasks[q] == rdr.GetInt32(0))
                        {
                            log += " già inserito";
                            checkI = true;
                        }
                    }
                    if (checkN == false && checkI == false)
                    {
                        log += "aggiunto.<br/>";
                        this._RunningTasks.Add(rdr.GetInt32(0));
                    }
                    else
                    {
                        log += "<br/>";
                    }
                }
            }
            rdr.Close();
            conn.Close();
        }

        public void loadTaskProduzioneAvviabili()
        {
            this._RunnableTasks = new List<int>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione " 
                    + " INNER JOIN inputpoints_workstations ON(tasksproduzione.postazione = inputpoints_workstations.workstationid) "
                + " WHERE(status = 'N' OR status = 'I' OR status = 'P') AND inputpoints_workstations.inputpointid=@ipid ORDER BY lateStart, earlyStart, idArticolo";
                cmd.Parameters.AddWithValue("@ipid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    // Verifico che tutti i precedenti siano terminati (se ConstraintType=1) oppure se siano avviati (se ConstraintType=0)
                    TaskProduzione tsk = new TaskProduzione(this.Tenant, rdr.GetInt32(0));

                    if (tsk.TaskProduzioneID != -1)
                    {

                        tsk.loadPrecedenti();
                        bool controllo = true;
                        for (int i = 0; i < tsk.PreviousTasks.Count; i++)
                        {
                            TaskProduzione prec = new TaskProduzione(this.Tenant, tsk.PreviousTasks[i].NearTaskID);
                            if (tsk.PreviousTasks[i].ConstraintType == 0)
                            {
                                if (prec.Status == 'N')
                                {
                                    controllo = false;
                                }
                            }
                            else
                            {
                                if (prec.Status != 'F')
                                {
                                    controllo = false;
                                }
                            }
                        }
                        if (controllo == true)
                        {
                            this._RunnableTasks.Add(rdr.GetInt32(0));
                        }
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }
    }


    public class InputPointDepartment
    {
        public String log;

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

        private Boolean _enabled;
        public Boolean enabled { 
            get { return this._enabled; }
            set
            {
                if (this.inputpointId > -1 && this.departmentId >- 1 && this.Tenant.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE inputpoints_departments SET enabled=@enabled WHERE inputpointid=@ipid AND departmentid=@deptid";
                    cmd.Parameters.AddWithValue("@ipid", this.inputpointId);
                    cmd.Parameters.AddWithValue("@deptid", this.departmentId);
                    cmd.Parameters.AddWithValue("@enabled", value);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._enabled = value;
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }
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
                    + "reparti.idreparto, reparti.nome, reparti.descrizione, reparti.timezone, "
                    + " inputpoints_departments.enabled "
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
                    this._enabled = rdr.IsDBNull(11) ? false : rdr.GetBoolean(11);
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
        public String log;

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

        private Boolean _enabled;
        public Boolean enabled { 
            get { return this._enabled; }
            set
            {
                if (this.inputpointId > -1 && this.workstationId > -1 && this.Tenant.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE inputpoints_workstations SET enabled=@enabled WHERE inputpointid=@ipid AND workstationid=@wstid";
                    cmd.Parameters.AddWithValue("@ipid", this.inputpointId);
                    cmd.Parameters.AddWithValue("@wstid", this.workstationId);
                    cmd.Parameters.AddWithValue("@enabled", value);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._enabled = value;
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

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
                    + " postazioni.idpostazioni, postazioni.name, postazioni.description, postazioni.barcodeAutoCheckIn, inputpoints_workstations.enabled "
                    + " FROM "
                    + " inputpoints INNER JOIN inputpoints_workstations ON(inputpoints.id = inputpoints_workstations.inputpointid) "
                    + " INNER JOIN postazioni ON(inputpoints_workstations.workstationId = postazioni.idpostazioni) "
                    + " WHERE postazioni.idpostazioni = @workstationid AND inputpoints.id = @idinputpoint";

                cmd.Parameters.AddWithValue("@workstationid", workstationId);
                cmd.Parameters.AddWithValue("@idinputpoint", inputpointId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    this._inputpointId = rdr.GetInt32(0);
                    this._inputpointName = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                    this._workstationDescription = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                    this._inputpointCreationDate = rdr.IsDBNull(3) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(3);
                    this._inputpointCreatorId = rdr.IsDBNull(4) ? -1 : rdr.GetInt32(4);
                    this._inputpointNotes = rdr.IsDBNull(5)? "" : rdr.GetString(5);
                    // linkCreationDate 6
                    this._workstationId = rdr.GetInt32(7);
                    this._workstationName = rdr.IsDBNull(8) ? "" : rdr.GetString(8);
                    this._workstationDescription = rdr.IsDBNull(9) ? "" : rdr.GetString(9);
                    this._enabled = rdr.IsDBNull(10) ? false : rdr.GetBoolean(10);
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
                cmd.Parameters.AddWithValue("@wstid", this.workstationId);
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
                    this.log = ex.Message;
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