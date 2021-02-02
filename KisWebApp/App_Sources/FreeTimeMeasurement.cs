using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.App_Sources
{
    public class FreeTimeMeasurement
    {
        public String log;

        private int _id;
        public int id { get { return this._id; } }
        private DateTime _CreationDate;
        public DateTime CreationDate { get { return this._CreationDate; } }
        private String _CreatedBy;
        public String CreatedBy { get { return this._CreatedBy; } }

        private DateTime _PlannedStartDate;
        public DateTime PlannedStartDate { get { return this._PlannedStartDate; } }

        private DateTime _PlannedEndDate;
        public DateTime PlannedEndDate { get { return this._PlannedEndDate; } }

        private int _DepartmentId;
        public int DepartmentId { get { return this._DepartmentId; } }

        private String _DepartmentName;
        public String DepartmentName { get { return this._DepartmentName; } }

        private String _DepartmentTimeZone;
        public String DepartmentTimeZone { get { return this._DepartmentTimeZone; } }

        private String _Name;
        public String Name { get { return this._Name; }
            set
            {
                if (this.id != -1 && value.Length < 255)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements SET name=@name WHERE id=@id";
                    cmd.Parameters.AddWithValue("@name", value);
                    cmd.Parameters.AddWithValue("@id", this.id);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Name = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private String _Description;
        public String Description {
            get { return this._Description; }
            set
            {
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements SET description=@name description id=@id";
                    cmd.Parameters.AddWithValue("@description", value);
                    cmd.Parameters.AddWithValue("@id", this.id);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Name = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private int _ProcessId;
        public int ProcessId { get { return this._ProcessId; } }

        private int _ProcessRev;
        public int ProcessRev { get { return this._ProcessRev; } }

        private int _VariantId;
        public int VariantId { get { return this._VariantId; } }

        private String _ProcessName;
        public String ProcessName { get { return this._ProcessName; } }

        private String _ProcessDescription;
        public String ProcessDescription { get { return this._ProcessDescription; } }

        private String _VariantName;
        public String VariantName { get { return this._VariantName; } }

        private char _Status;
        public char Status { get { return this._Status; } }

        private String _SerialNumber;
        public String SerialNumber { get { return this._SerialNumber; } }

        private double _Quantity;
        public double Quantity { get { return this._Quantity; } }

        private int _MeasurementUnitId;
        public int MeasurementUnitId { get { return this._MeasurementUnitId; } }

        private String _MeasurementUnitType;
        public String MeasurementUnitType { get { return this._MeasurementUnitType; } }

        private DateTime _RealEndDate;
        public DateTime RealEndDate { get { return this._RealEndDate; } }

        private Double _RealWorkingTime_Hours;
        public Double RealWorkingTime_Hours { get { return this._RealWorkingTime_Hours; } }

        private Double _RealLeadTime_Hours;
        public Double RealLeadTime_Hours { get { return this._RealLeadTime_Hours; } }

        private Boolean _AllowCustomTasks;
        public Boolean AllowCustomTasks { get { return this._AllowCustomTasks; } }

        private Boolean _AllowExecuteFinishedTasks;
        public Boolean AllowExecuteFinishedTasks { get { return this._AllowExecuteFinishedTasks; } }

        public List<FreeMeasurement_Task> Tasks;

        public FreeTimeMeasurement(int id)
        {
            this._id = -1;
            this.Tasks = new List<FreeMeasurement_Task>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT "
                        + " freemeasurements.id, "
                        + " freemeasurements.creationdate,"
                        + " freemeasurements.createdby,"
                        + " freemeasurements.plannedstartdate, "
                        + " freemeasurements.plannedenddate, "
                        + " freemeasurements.departmentid, " // 5
                        + " reparti.nome, "
                        + " reparti.timezone, "
                        + " freemeasurements.name, "
                        + " freemeasurements.description, "
                        + " freemeasurements.processid, " // 10
                        + " freemeasurements.processrev, "
                        + " freemeasurements.variantid, "
                        + " processo.name, "
                        + " processo.description, "
                        + " varianti.nomevariante, " // 15
                        + " freemeasurements.status, "
                        + " freemeasurements.serialnumber, "
                        + " freemeasurements.quantity, "
                        + " freemeasurements.measurementunit, "
                        + " measurementunits.type, " // 20
                        + " freemeasurements.realenddate, "
                        + " freemeasurements.realworkingtime_hours, "
                        + " freemeasurements.realleadtime_hours, "
                        + " freemeasurements.AllowCustomTasks, "
                        + " freemeasurements.ExecuteFinishedTasks " // 25
                        + "  FROM freemeasurements INNER JOIN "
                        + " variantiprocessi ON(variantiprocessi.variante = freemeasurements.variantid "
                        + " AND variantiprocessi.processo = freemeasurements.processid "
                        + " AND variantiprocessi.revProc = freemeasurements.processid) "
                        + " INNER JOIN processo ON(variantiprocessi.processo = processo.processId and variantiprocessi.revProc = processo.revisione) "
                        + " INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante) "
                        + " INNER JOIN reparti ON(freemeasurements.departmentid = reparti.idreparto) "
                        + " INNER JOIN measurementunits ON(measurementunits.id = freemeasurements.measurementUnit) "
                        + " WHERE freemeasurements.id = @id";

            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                this._id = rdr.GetInt32(0);
                this._CreationDate = rdr.GetDateTime(1);
                this._CreatedBy = rdr.GetString(2);
                this._PlannedStartDate = rdr.GetDateTime(3);
                this._PlannedEndDate = rdr.GetDateTime(4);
                this._DepartmentId = rdr.GetInt32(5);
                this._DepartmentName = rdr.GetString(6);
                this._DepartmentTimeZone = rdr.GetString(7);
                this._Name = rdr.GetString(8);
                this._Description = rdr.GetString(9);
                this._ProcessId = rdr.GetInt32(10);
                this._ProcessRev = rdr.GetInt32(11);
                this._VariantId = rdr.GetInt32(12);
                this._ProcessName = rdr.GetString(13);
                this._ProcessDescription = rdr.GetString(14);
                this._VariantName = rdr.GetString(15);
                this._Status = rdr.GetChar(16);
                this._SerialNumber = rdr.GetString(17);
                this._Quantity = rdr.GetDouble(18);
                this._MeasurementUnitId = rdr.GetInt32(19);
                this._MeasurementUnitType = rdr.GetString(20);
                this._RealEndDate = rdr.IsDBNull(21) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(21);
                this._RealWorkingTime_Hours = rdr.IsDBNull(22) ? 0 : rdr.GetDouble(22);
                this._RealLeadTime_Hours = rdr.IsDBNull(23) ? 0 : rdr.GetDouble(23);
                this._AllowCustomTasks = rdr.GetBoolean(24);
                this._AllowExecuteFinishedTasks = rdr.GetBoolean(25);
            }
            rdr.Close();
            conn.Close();
            /* SELECT 
freemeasurements.id,
freemeasurements.creationdate,
freemeasurements.createdby,
freemeasurements.plannedstartdate,
freemeasurements.plannedenddate,
freemeasurements.departmentid,
reparti.nome,
reparti.timezone,
freemeasurements.name,
freemeasurements.description,
freemeasurements.processid,
freemeasurements.processrev,
freemeasurements.variantid,
processo.name,
processo.description,
varianti.nomevariante,
freemeasurements.status,
freemeasurements.serialnumber,
freemeasurements.quantity,
freemeasurements.measurementunit,
measurementunits.type,
freemeasurements.realenddate,
freemeasurements.realworkingtime_hours,
freemeasurements.realleadtime_hours,
freemeasurements.AllowCustomTasks,
freemeasurements.ExecuteFinishedTasks
 FROM freemeasurements INNER JOIN 
variantiprocessi ON (variantiprocessi.variante = freemeasurements.variantid  
AND variantiprocessi.processo = freemeasurements.processid 
AND variantiprocessi.revProc = freemeasurements.processid) 
INNER JOIN processo ON(variantiprocessi.processo = processo.processId and variantiprocessi.revProc = processo.revisione) 
INNER JOIN varianti ON (variantiprocessi.variante = varianti.idvariante) 
INNER JOIN reparti ON (freemeasurements.departmentid = reparti.idreparto) 
INNER JOIN measurementunits ON (measurementunits.id = freemeasurements.measurementUnit) 
WHERE freemeasurements.id = 0
;*/
        }

        public void loadTasks()
        {
            this.Tasks = new List<FreeMeasurement_Task>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT freemeasurements_tasks.MeasurementId, freemeasurements_tasks.TaskId FROM freemeasurements_tasks "
                    + " INNER JOIN freemeasurements ON (freemeasurements.id = freemeasurements_tasks.MeasurementId)"
                    + " WHERE freemeasurements_tasks.MeasurementId=@measurementid"
                    + " ORDER BY sequence";
                cmd.Parameters.AddWithValue("@measurementid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    FreeMeasurement_Task curr = new FreeMeasurement_Task(this.id, rdr.GetInt32(1));
                    this.Tasks.Add(curr);
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void addTask(NoProductiveTask npTask)
        {
            if (this.id != -1)
            {
                this.loadTasks();
                int seq = this.Tasks.Count + 1;
                int tID = 0;
                if (this.Tasks.Count > 0)
                {
                    tID = this.Tasks.Max(t => t.TaskId);
                }

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmdTasks = conn.CreateCommand();
                /*cmdTasks.CommandText = "SELECT MAX(TaskId) FROM freemeasurements_tasks WHERE measurementid=@measurementid";
                cmdTasks.Parameters.AddWithValue("@measurementid", this.id);
                MySqlDataReader rdr = cmdTasks.ExecuteReader();
                int tID = 0;
                if(rdr.Read())
                {
                    tID = rdr.GetInt32(0) + 1;
                }*/
                MySqlTransaction tr = conn.BeginTransaction();
                cmdTasks.Transaction = tr;
                cmdTasks.CommandText = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, NoProductiveTaskId, name, "
                    + " description, sequence, workstationid, quantity_planned, status) "
                    + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @NoProductiveTaskId, @name, "
                    + " @description, @sequence, @workstationid, @quantity_planned, @status)";

                cmdTasks.Parameters.AddWithValue("@measurementid", this.id);
                cmdTasks.Parameters.AddWithValue("@taskid", tID);
                cmdTasks.Parameters.AddWithValue("@OrigTaskId", null);
                cmdTasks.Parameters.AddWithValue("@OrigTaskRev", null);
                cmdTasks.Parameters.AddWithValue("@VariantId", null);
                cmdTasks.Parameters.AddWithValue("@NoProductiveTaskId", npTask.ID);
                cmdTasks.Parameters.AddWithValue("@name", npTask.Name);
                cmdTasks.Parameters.AddWithValue("@description", npTask.Description);
                cmdTasks.Parameters.AddWithValue("@sequence", seq);
                cmdTasks.Parameters.AddWithValue("@workstationid", null);
                cmdTasks.Parameters.AddWithValue("@quantity_planned", this.Quantity);
                cmdTasks.Parameters.AddWithValue("@status", 'N');

                try
                {
                    cmdTasks.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                }
            }
        }
    }

    public class FreeTimeMeasurements
    {
        public String log;

        public List<FreeTimeMeasurement> MeasurementsList;

        public FreeTimeMeasurements()
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
        }

        public void loadAllMeasurements()
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM freemeasurements ORDER BY plannedstartdate, plannedenddate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.MeasurementsList.Add(new FreeTimeMeasurement(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        /* Loads a single status
         * if input = 'O' then loads all NOT finished Measurements
         */
        public void loadMeasurements(char Status)
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            String strWhere = " = '" + Status + "'";
            if (Status == 'O')
            {
                strWhere = " <> 'F'";
            }
            cmd.CommandText = "SELECT id FROM freemeasurements WHERE status " + strWhere + " ORDER BY plannedstartdate, plannedenddate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.MeasurementsList.Add(new FreeTimeMeasurement(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        /* Returns:
         * FreeMeasurementId if everything is ok
         * -1 if generic error
         * -2 if input error
         * -3 if error while adding
         */
        public int Add(String createdby, DateTime plannedstartdate, DateTime plannedenddate, int DepartmentId, String name, String description, int processid, int processrev, int variantid,
             String serialnumber, Double quantity, int measurementUnitId = 0, Boolean AllowCustomTasks = true, Boolean AllowExecuteFinishedTasks = true)
        {
            int ret = -1;
            if (name.Length < 255)
            {
                processo prc = new processo(processid, processrev);
                variante vr = new variante(variantid);
                ProcessoVariante prcVar = new ProcessoVariante(prc, vr);
                if (prcVar != null && prcVar.process != null && prcVar.process.processID != -1
                    && prcVar.variant != null && prcVar.variant.idVariante != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO freemeasurements(createdby, plannedstartdate, plannedenddate, departmentid, name, description,"
                        + " processid, processrev, variantid, status, serialnumber, quantity, measurementunit, "
                        + " AllowCustomTasks, ExecuteFinishedTasks) "
                        + " VALUES(@createdby, @plannedstartdate, @plannedenddate, @departmentid, @name, @description,"
                        + " @processid, @processrev, @variantid, @status, @serialnumber, @quantity, @measurementunit, "
                        + " @AllowCustomTasks, @ExecuteFinishedTasks)";

                    cmd.Parameters.AddWithValue("@createdby", createdby);
                    cmd.Parameters.AddWithValue("@plannedstartdate", plannedstartdate);
                    cmd.Parameters.AddWithValue("@plannedenddate", plannedenddate);
                    cmd.Parameters.AddWithValue("@departmentid", DepartmentId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@processid", processid);
                    cmd.Parameters.AddWithValue("@processrev", processrev);
                    cmd.Parameters.AddWithValue("@variantid", variantid);
                    cmd.Parameters.AddWithValue("@status", 'N');
                    cmd.Parameters.AddWithValue("@serialnumber", serialnumber);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@measurementunit", measurementUnitId);
                    cmd.Parameters.AddWithValue("@AllowCustomTasks", AllowCustomTasks);
                    cmd.Parameters.AddWithValue("@ExecuteFinishedTasks", AllowExecuteFinishedTasks);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT LAST_INSERT_ID()";
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            ret = rdr.GetInt32(0);
                        }
                        rdr.Close();

                        // LoadSon
                        prc.loadFigli(vr);
                        for (int i = 0; i < prc.subProcessi.Count; i++)
                        {
                            TaskVariante tskvar = new TaskVariante(prc.subProcessi[i], vr);
                            tskvar.loadPostazioni();
                            int wsId = -1;
                            if (tskvar.PostazioniDiLavoro.Count > 0)
                            {
                                wsId = tskvar.PostazioniDiLavoro[0].id;
                            }
                            MySqlCommand cmdTasks = conn.CreateCommand();
                            cmdTasks.Transaction = tr;
                            cmdTasks.CommandText = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, name, "
                                + " description, sequence, workstationid, quantity_planned, status) "
                                + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @name, "
                                + " @description, @sequence, @workstationid, @quantity_planned, @status)";
                            cmdTasks.Parameters.AddWithValue("@measurementid", ret);
                            cmdTasks.Parameters.AddWithValue("@taskid", i);
                            cmdTasks.Parameters.AddWithValue("@OrigTaskId", prc.subProcessi[i].processID);
                            cmdTasks.Parameters.AddWithValue("@OrigTaskRev", prc.subProcessi[i].revisione);
                            cmdTasks.Parameters.AddWithValue("@VariantId", vr.idVariante);
                            cmdTasks.Parameters.AddWithValue("@name", prc.subProcessi[i].processName);
                            cmdTasks.Parameters.AddWithValue("@description", prc.subProcessi[i].processDescription);
                            cmdTasks.Parameters.AddWithValue("@sequence", (i + 1));
                            if (wsId != -1)
                            {
                                cmdTasks.Parameters.AddWithValue("@workstationid", wsId);
                            }
                            else
                            {
                                cmdTasks.Parameters.AddWithValue("@workstationid", null);
                            }
                            cmdTasks.Parameters.AddWithValue("@quantity_planned", quantity);
                            cmdTasks.Parameters.AddWithValue("@status", 'N');
                            cmdTasks.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        ret = -3;
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
                ret = -2;
            }
            return ret;
        }

    }

    public class FreeMeasurement_Task
    {
        public String log;

        private int _MeasurementId;
        public int MeasurementId { get { return this._MeasurementId; } }

        private int _TaskId;
        public int TaskId { get { return this._TaskId; } }

        private int _OrigTaskId;
        public int OrigTaskId { get { return this._OrigTaskId; } }

        private int _OrigTaskRev;
        public int OrigTaskRev { get { return this._OrigTaskRev; } }

        private int _VariantId;
        public int VariantId { get { return this._VariantId; } }

        private int _NoProductiveTaskId;
        public int NoProductiveTaskId { get { return this._NoProductiveTaskId; } }

        private String _Name;
        public String Name { get { return this._Name; } }

        private String _Description;
        public String Description { get { return this._Description; } }

        private int _Sequence;
        public int Sequence { get { return this._Sequence; } }

        private int _WorkstationId;
        public int WorkstationId { get { return this._WorkstationId; } }

        private String _WorkstationName;
        public String WorkstationName { get { return this._WorkstationName; } }

        private Double _PlannedQuantity;
        public Double PlannedQuantity { get { return this._PlannedQuantity; } }

        private Double _ProducedQuantity;
        public Double ProducedQuantity 
        { 
            get { return this._ProducedQuantity; }
            set
            {
                if (this.MeasurementId != -1 && this.TaskId != -1 && value > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements_tasks SET quantity_produced=@quantity_produced WHERE measurementid=@measurementid AND taskid=@taskid";
                    cmd.Parameters.AddWithValue("@quantity_produced", value);
                    cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                    cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
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

        /* I = Running
         * N = Not started
         * P = Paused
         * F = Finished
         */
        private Char _Status;
        public Char Status 
        { 
            get { return this._Status; } 
            set 
            {
                if(this.MeasurementId != -1 && this.TaskId!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements_tasks SET status=@status WHERE measurementid=@measurementid AND taskid=@taskid";
                    cmd.Parameters.AddWithValue("@status", value);
                    cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                    cmd.Parameters.AddWithValue("@taskid", this.TaskId);
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
            }
        }

        private DateTime _StartDateReal;
        public DateTime StartDateReal { get{ return this._StartDateReal; }
            set
            {
                DateTime start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                if (this.MeasurementId != -1 && this.TaskId != -1 && value >= start)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements_tasks SET task_startdatereal=@startdatereal WHERE measurementid=@measurementid AND taskid=@taskid";
                    cmd.Parameters.AddWithValue("@startdatereal", value.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                    cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
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

        private DateTime _EndDateReal;
        public DateTime EndDateReal 
        { 
            get { return this._EndDateReal; }
            set
            {
                DateTime start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                if (this.MeasurementId != -1 && this.TaskId != -1 && value >= start)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements_tasks SET task_enddatereal=@enddatereal WHERE measurementid=@measurementid AND taskid=@taskid";
                    cmd.Parameters.AddWithValue("@enddatereal", value.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                    cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
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

        private Double _RealLeadTime_Hours;
        public Double RealLeadTime_Hours { get { return this._RealLeadTime_Hours; }
            set
            {
                if (this.MeasurementId != -1 && this.TaskId != -1 && value >= 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements_tasks SET RealLeadTime_Hours=@leadtime WHERE measurementid=@measurementid AND taskid=@taskid";
                    cmd.Parameters.AddWithValue("@leadtime", value);
                    cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                    cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
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

        private Double _RealWorkingTime_Hours;
        public Double RealWorkingTime_Hours { get { return this._RealWorkingTime_Hours; }
            set
            {
                if (this.MeasurementId != -1 && this.TaskId != -1 && value >= 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements_tasks SET RealLeadTime_Hours=@workingtime WHERE measurementid=@measurementid AND taskid=@taskid";
                    cmd.Parameters.AddWithValue("@workingtime", value);
                    cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                    cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
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

        private Boolean _AllowCustomTasks;
        public Boolean AllowCustomTasks { get { return this._AllowCustomTasks; } }

        private Boolean _AllowExecuteFinishedTasks;
        public Boolean AllowExecuteFinishedTasks { get { return this._AllowExecuteFinishedTasks; } }

        private int _DepartmentId;
        public int DepartmentId { get { return this._DepartmentId; } }

        public FreeMeasurement_Task(int measurementID, int taskID)
        {
            this._MeasurementId = -1;
            this._TaskId = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT "
                    + " freemeasurements_tasks.MeasurementId, " // 0
                    + " freemeasurements_tasks.TaskId, "        // 1
                    + " freemeasurements_tasks.OrigTaskId, "
                    + " freemeasurements_tasks.OrigTaskRev, "
                    + " freemeasurements_tasks.VariantId, "
                    + " freemeasurements_tasks.NoProductiveTaskId, "    // 5
                    + " freemeasurements_tasks.name, "
                    + " freemeasurements_tasks.description, "
                    + " freemeasurements_tasks.sequence, "
                    + " freemeasurements_tasks.workstationid, "
                    + " postazioni.name, "                      // 10
                    + " freemeasurements_tasks.quantity_planned, "
                    + " freemeasurements_tasks.quantity_produced, "
                    + " freemeasurements_tasks.status, "
                    + " freemeasurements_tasks.task_startdatereal, "
                    + " freemeasurements_tasks.task_enddatereal, "  // 15
                    + " freemeasurements_tasks.realleadtime_hours, "
                    + " freemeasurements_tasks.realworkingtime_hours, "  // 17
                    + " freemeasurements.AllowCustomTasks, "
                    + " freemeasurements.ExecuteFinishedTasks, " // 19
                    + " freemeasurements.DepartmentId "         // 20
                    + "  FROM freemeasurements_tasks "
                    + " INNER JOIN postazioni ON(freemeasurements_tasks.workstationid = postazioni.idpostazioni) "
                    + " INNER JOIN freemeasurements ON (freemeasurements_tasks.MeasurementId = freemeasurements.id)"
                    + " WHERE freemeasurements_tasks.TaskId=@taskid AND freemeasurements_tasks.MeasurementId=@measurementid"
                    + " ORDER BY sequence";
            cmd.Parameters.AddWithValue("@taskid", taskID);
            cmd.Parameters.AddWithValue("@measurementid", measurementID);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                this._MeasurementId = rdr.GetInt32(0);
                this._TaskId = rdr.GetInt32(1);
                this._OrigTaskId = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                this._OrigTaskRev = rdr.IsDBNull(3) ? -1 : rdr.GetInt32(3);
                this._VariantId = rdr.IsDBNull(4) ? -1 : rdr.GetInt32(4);
                this._NoProductiveTaskId = rdr.IsDBNull(5) ? -1 : rdr.GetInt32(5);
                this._Name = rdr.GetString(6);
                this._Description = rdr.GetString(7);
                this._Sequence = rdr.GetInt32(8);
                this._WorkstationId = rdr.GetInt32(9);
                this._WorkstationName = rdr.GetString(10);
                this._PlannedQuantity = rdr.GetDouble(11);
                this._ProducedQuantity = rdr.GetDouble(12);
                this._Status = rdr.GetChar(13);
                this._StartDateReal = rdr.IsDBNull(14) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(14);
                this._EndDateReal = rdr.IsDBNull(15) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(15);
                this._RealLeadTime_Hours = rdr.IsDBNull(16) ? 0 : rdr.GetDouble(16);
                this._RealWorkingTime_Hours = rdr.IsDBNull(17) ? 0 : rdr.GetDouble(17);
                this._AllowCustomTasks = rdr.GetBoolean(18);
                this._AllowExecuteFinishedTasks = rdr.GetBoolean(19);
                this._DepartmentId = rdr.GetInt32(20);
            }
            rdr.Close();
            conn.Close();
        }

        /* Returns:
         * 0 if generic error
         * 1 if task started successfully
         * 2 if task already started
         * 3 if operator not found
         * 4 if operator exceeds max number of running tasks
         */
        public int Start(User op)
        {
            int ret = 0;
            if(this.Status == 'N' || this.Status == 'P' || (this.Status == 'F' && this.AllowExecuteFinishedTasks))
            {
                DateTime eventtime = DateTime.UtcNow;

                Reparto dept = new Reparto(this.DepartmentId);
                int maxTasksInExecution = dept.TasksAvviabiliContemporaneamenteDaOperatore;
                int tasksInExecution = op.FreeMeasurement_RunningTasks(dept);
                Boolean checkMaxTasks = false;
                Boolean PauseDefaultNoProductiveTask = false;
                int npTask = -1;
                if(maxTasksInExecution > 0 && tasksInExecution < maxTasksInExecution)
                { 
                    checkMaxTasks = true; 
                }
                else
                {
                    op.loadFreeMeasurementRunningTasks();
                    if(op.FreeMeasurementTasks.Count == 1 && op.FreeMeasurementTasks[0].NoProductiveTaskId != -1 
                        && (op.FreeMeasurementTasks[0].TaskId != this.TaskId || op.FreeMeasurementTasks[0].MeasurementId != this.MeasurementId))
                    {
                        checkMaxTasks = true;
                        PauseDefaultNoProductiveTask = true;
                        npTask = op.FreeMeasurementTasks[0].TaskId;
                    }
                }

                if(checkMaxTasks)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    try
                    {
                        if (PauseDefaultNoProductiveTask)
                        {
                            MySqlCommand cmdDef = conn.CreateCommand();
                            cmdDef.Transaction = tr;
                            cmdDef.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                                + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                            cmdDef.Parameters.AddWithValue("@freemeasurementid", this.MeasurementId);
                            cmdDef.Parameters.AddWithValue("@taskid", npTask);
                            cmdDef.Parameters.AddWithValue("@user", op.username);
                            cmdDef.Parameters.AddWithValue("@eventtype", 'P');
                            cmdDef.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss");
                            cmdDef.Parameters.AddWithValue("@notes", "");
                            cmdDef.ExecuteNonQuery();

                            FreeMeasurement_Task defTask = new FreeMeasurement_Task(this.MeasurementId, npTask);
                            defTask.loadActiveUsers();
                            if(defTask.Users.Count == 0)
                            {
                                defTask.Status = 'P';
                            }
                        }

                        cmd.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                                + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                        cmd.Parameters.AddWithValue("@freemeasurementid", this.MeasurementId);
                        cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                        cmd.Parameters.AddWithValue("@user", op.username);
                        cmd.Parameters.AddWithValue("@eventtype", 'I');
                        cmd.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss");
                        cmd.Parameters.AddWithValue("@notes", "");
                        cmd.ExecuteNonQuery();

                        this.Status = 'I';

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
                    ret = 4;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        // Running tasks queries
        /* SELECT COUNT(DISTINCT(freemeasurements_tasks.taskid)) FROM freemeasurements_tasks INNER JOIN freemeasurements_tasks_events 
         * ON (freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) 
         * INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) WHERE freemeasurements_tasks.status = 'I' 
         * AND freemeasurements_tasks_events.user='admin' AND freemeasurements.departmentid=0 ORDER BY freemeasurements_tasks_events.eventdate;
         */

        /*
         * Returns: 
         * 0 if generic error
         * 1 if Paused succesfully
         * 2 if task is not running or operator not found
         */
        public int Pause(User op)
        {
            int ret = 0;
            if(this.Status == 'I' && op.username.Length > 0)
            {
                DateTime eventtime = DateTime.UtcNow;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                   + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                cmd.Parameters.AddWithValue("@freemeasurementid", this.MeasurementId);
                cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                cmd.Parameters.AddWithValue("@user", op.username);
                cmd.Parameters.AddWithValue("@eventtype", 'P');
                cmd.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.AddWithValue("@notes", "");
                
                try
                { 
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                }

                this.loadActiveUsers();
                if(this.Users.Count == 0)
                {
                    this.Status = 'P';
                }

                op.loadFreeMeasurementRunningTasks();
                if(op.FreeMeasurementTasks.Count == 0)
                {
                    NoProductiveTasks npts = new NoProductiveTasks();
                    var defTask = npts.TaskList.FirstOrDefault(x => x.IsDefault == true);
                    if(defTask!=null)
                    {
                        MySqlCommand cmdDef = conn.CreateCommand();
                        MySqlTransaction tr2 = conn.BeginTransaction();
                        cmdDef.Transaction = tr2;
                        
                        try
                        { 
                            // ADD NO PRODUCTIVE TASK IN TASK_LIST


                            cmdDef.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                               + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                            cmdDef.Parameters.AddWithValue("@freemeasurementid", this.MeasurementId);
                            cmdDef.Parameters.AddWithValue("@taskid", this.TaskId);
                            cmdDef.Parameters.AddWithValue("@user", op.username);
                            cmdDef.Parameters.AddWithValue("@eventtype", 'P');
                            cmdDef.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss");
                            cmdDef.Parameters.AddWithValue("@notes", "");

                            cmdDef.ExecuteNonQuery();
                            tr2.Commit();
                        }
                        catch(Exception ex)
                        {
                            this.log = ex.Message;
                            tr2.Rollback();
                        }
                    }
                }

                conn.Close();
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        public List<String> Users;
        public void loadActiveUsers()
        {
            this.Users = new List<String>();
            if (this.MeasurementId !=-1 && this.TaskId != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT user FROM "
                     + " (SELECT DISTINCT(user), freemeasurements_tasks_events.eventtype "
                     + "  FROM freemeasurements_tasks "
                     + " INNER JOIN freemeasurements_tasks_events ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND "
                     + " freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                     + " INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
                     + " WHERE freemeasurements_tasks.status = 'I' "
                     + " AND freemeasurements_tasks.measurementid = @measurementid "
                     + " AND freemeasurements_tasks.taskid = @taskid "
                     + " ORDER BY freemeasurements_tasks_events.eventdate) AS runningtasks "
                     + " WHERE runningtasks.eventtype <> 'F' AND runningtasks.eventtype <> 'P'";
                cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Users.Add(rdr.GetString(0));
                }
                rdr.Close();
                conn.Close();
            }
        }
    }
}