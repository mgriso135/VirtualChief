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
        public char Status { get { return this._Status; }
            set
            {
                if (this.id != -1 && (value=='I' || value=='P' || value=='F'))
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE freemeasurements SET status=@status WHERE id=@id";
                    cmd.Parameters.AddWithValue("@status", value);
                    cmd.Parameters.AddWithValue("@id", this.id);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Status = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

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
                        + " AND variantiprocessi.revProc = freemeasurements.processrev) "
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

        /* Returns:
         * -1 if generic error
         * 
         */
        public int addTask(NoProductiveTask npTask)
        {
            int ret = -1;
            if (this.id != -1 && this.Status != 'F')
            {
                this.loadTasks();
                int found = -1;
                try
                { 
                    var itm = this.Tasks.First(x => x.NoProductiveTaskId == npTask.ID);
                    found = 1;
                    ret = itm.TaskId;
                }
                catch
                {
                    found = -1;
                }

                if(found == -1)
                { 
                    int seq = this.Tasks.Count + 1;
                    int tID = 0;
                    if (this.Tasks.Count > 0)
                    {
                        tID = this.Tasks.Max(t => t.TaskId)+1;
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
                        ret = tID;
                    }
                    catch (Exception ex)
                    {
                        ret = -1;
                        tr.Rollback();
                    }
                }
                else
                {
                }
            }
            return ret;
        }

        public int addTask(String TaskName)
        {
            int ret = 0;
            if (this.id != -1 && TaskName.Length < 255 && this.Status != 'F')
            {
                this.loadTasks();
                int found = -1;
                try
                {
                    var itm = this.Tasks.First(x => x.Name == TaskName);
                    found = 1;
                    ret = itm.TaskId;
                }
                catch
                {
                    found = -1;
                }

                if (found == -1)
                {
                    int seq = this.Tasks.Count + 1;
                    int tID = 0;
                    if (this.Tasks.Count > 0)
                    {
                        tID = this.Tasks.Max(t => t.TaskId) + 1;
                    }

                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmdTasks = conn.CreateCommand();

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
                    cmdTasks.Parameters.AddWithValue("@NoProductiveTaskId", null);
                    cmdTasks.Parameters.AddWithValue("@name", TaskName);
                    cmdTasks.Parameters.AddWithValue("@description", "");
                    cmdTasks.Parameters.AddWithValue("@sequence", seq);
                    cmdTasks.Parameters.AddWithValue("@workstationid", null);
                    cmdTasks.Parameters.AddWithValue("@quantity_planned", this.Quantity);
                    cmdTasks.Parameters.AddWithValue("@status", 'N');

                    try
                    {
                        cmdTasks.ExecuteNonQuery();
                        tr.Commit();
                        ret = tID;
                    }
                    catch (Exception ex)
                    {
                        ret = -1;
                        tr.Rollback();
                    }
                }
                else
                {
                }
            }
            return ret;
        }

        public int addTask(String TaskName, Double workingtime, int step, Boolean isAcyclic, Double Acyclic_CycleTime, Double Acyclic_QtyUsed, 
            Double Acyclic_QtyForEachProduct, Char ValueOrWaste, Char Ergonomy)
        {
            int ret = 0;
            if (this.id != -1 && TaskName.Length < 255)
            {
                this.loadTasks();
                int found = -1;
                try
                {
                    var itm = this.Tasks.First(x => x.Name == TaskName);
                    found = 1;
                    ret = itm.TaskId;
                }
                catch
                {
                    found = -1;
                }

                if (found == -1)
                {
                    int seq = this.Tasks.Count + 1;
                    int tID = 0;
                    if (this.Tasks.Count > 0)
                    {
                        tID = this.Tasks.Max(t => t.TaskId) + 1;
                    }

                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmdTasks = conn.CreateCommand();

                    MySqlTransaction tr = conn.BeginTransaction();
                    cmdTasks.Transaction = tr;
                    cmdTasks.CommandText = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, NoProductiveTaskId, name, "
                        + " description, sequence, workstationid, quantity_planned, status, quantityproduced, task_startdatereal, task_enddatereal, "
                        + " realleadtime_hours, realworkingtime_hours, step, isAcyclic, Acyclic_CycleTime, Acyclic_QtyUsed, Acyclic_QtyForEachProduct, ValueOrWaste, Ergonomy"
                        + ") "
                        + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @NoProductiveTaskId, @name, "
                        + " @description, @sequence, @workstationid, @quantity_planned, @status, @quantityproduced, @task_startdatereal, @task_enddatereal, "
                        + " @realleadtime_hours, @realworkingtime_hours, @step, @isAcyclic, @Acyclic_CycleTime, @Acyclic_QtyUsed, @Acyclic_QtyForEachProduct, @ValueOrWaste, @Ergonomy)";

                    cmdTasks.Parameters.AddWithValue("@measurementid", this.id);
                    cmdTasks.Parameters.AddWithValue("@taskid", tID);
                    cmdTasks.Parameters.AddWithValue("@OrigTaskId", null);
                    cmdTasks.Parameters.AddWithValue("@OrigTaskRev", null);
                    cmdTasks.Parameters.AddWithValue("@VariantId", null);
                    cmdTasks.Parameters.AddWithValue("@NoProductiveTaskId", null);
                    cmdTasks.Parameters.AddWithValue("@name", TaskName);
                    cmdTasks.Parameters.AddWithValue("@description", "");
                    cmdTasks.Parameters.AddWithValue("@sequence", seq);
                    cmdTasks.Parameters.AddWithValue("@workstationid", null);
                    cmdTasks.Parameters.AddWithValue("@quantity_planned", this.Quantity);
                    cmdTasks.Parameters.AddWithValue("@status", 'F');
                    cmdTasks.Parameters.AddWithValue("@quantityproduced", this.Quantity);
                    cmdTasks.Parameters.AddWithValue("@task_startdatereal", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmdTasks.Parameters.AddWithValue("@task_enddatereal", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmdTasks.Parameters.AddWithValue("@realleadtime_hours", 0);
                    cmdTasks.Parameters.AddWithValue("@realworkingtime_hours", workingtime);
                    cmdTasks.Parameters.AddWithValue("@step", step);
                    cmdTasks.Parameters.AddWithValue("@isAcyclic", isAcyclic);
                    cmdTasks.Parameters.AddWithValue("@Acyclic_CycleTime", Acyclic_CycleTime);
                    cmdTasks.Parameters.AddWithValue("@Acyclic_QtyUsed", Acyclic_QtyUsed);
                    cmdTasks.Parameters.AddWithValue("@Acyclic_QtyForEachProduct", Acyclic_QtyForEachProduct);
                    cmdTasks.Parameters.AddWithValue("@ValueOrWaste", ValueOrWaste);
                    cmdTasks.Parameters.AddWithValue("@Ergonomy", Ergonomy);

                    try
                    {
                        cmdTasks.ExecuteNonQuery();
                        tr.Commit();
                        ret = tID;
                    }
                    catch (Exception ex)
                    {
                        ret = -1;
                        tr.Rollback();
                    }
                }
                else
                {
                }
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if finished successfully
         * 2 if status is already finished
         * 3 if some task is running
         */
        public int Finish()
        {
            int ret = 0;
            if(this.id!=-1 && this.Status != 'F')
            {
                this.loadTasks();
                Boolean checkTaskStatus = false;
                try
                {
                    var notPausedOrFinished = this.Tasks.First(y => y.Status == 'I' && y.NoProductiveTaskId == -1);
                    checkTaskStatus = false;
                }
                catch(Exception ex)
                {
                    checkTaskStatus = true;
                }

                if(checkTaskStatus)
                {
                    Double workingtime = this.calculateWorkingTime();
                    Double leadtime = this.calculateLeadTime();

                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.CommandText = "UPDATE freemeasurements SET status='F', realenddate=@enddate, "
                            + " realworkingtime_hours=@workingtime, realleadtime_hours=@leadtime "
                            + " WHERE id=@measurementid";
                        cmd.Parameters.AddWithValue("@enddate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@workingtime", workingtime);
                        cmd.Parameters.AddWithValue("@leadtime", leadtime);
                        cmd.Parameters.AddWithValue("@measurementid", this.id);
                        cmd.ExecuteNonQuery();

                        tr.Commit();

                        this.Status = 'F';

                        ret = 1;
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                    }

                    conn.Close();


                    this.loadTasks();
                    foreach (var t in this.Tasks)
                    {
                        if ((t.Status != 'F' && t.NoProductiveTaskId==-1) || (t.NoProductiveTaskId!=-1 && t.Status != 'F' && t.Status != 'I'))
                        {
                            DateTime enddate = DateTime.UtcNow;
                            t.Status = 'F';
                            Double wt = t.calculateWorkingTime();
                            t.RealWorkingTime_Hours = wt;
                            Double lt = t.calculateLeadTime();
                            t.RealLeadTime_Hours = lt;
                            if(t.StartDateReal <= new DateTime(2010,1,1))
                            {
                                t.StartDateReal = enddate;
                            }
                            t.EndDateReal = enddate;
                        }
                    }

                    FreeTimeMeasurements fms = new FreeTimeMeasurements();
                    fms.TransformEventsToTimespans();
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

        /* Returns total working hours of the task
        */
        protected Double calculateWorkingTime()
        {
            Double wt_Hours = 0.0;
            if (this.id != -1)
            {
                this.loadTasks();
                foreach(var t in this.Tasks)
                {
                    if(t.NoProductiveTaskId!=-1)
                    { 
                        wt_Hours += t.RealWorkingTime_Hours;
                    }
                }
            }
            return wt_Hours;
        }

        /* Returns total lead hours of the task
         */
        protected Double calculateLeadTime()
        {
            Double lt_Hours = 0.0;
            if (this.id != -1)
            {
                List<FreeMeasurements_Tasks_Event> eventsLst = new List<FreeMeasurements_Tasks_Event>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();

                DateTime start = new DateTime(1970, 1, 1);
                DateTime end = new DateTime(1970, 1, 1);
                Boolean checkstart = false;
                Boolean checkend = false;
                // Gets the first event
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, freemeasurements_tasks_events.freemeasurementid, freemeasurements_tasks_events.taskid, user, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    + " WHERE freemeasurementid=@measurementid AND eventtype='I' "
                    + " AND freemeasurements_tasks.NoProductiveTaskId=-1 "
                    + " ORDER BY eventdate";
                cmd.Parameters.AddWithValue("@measurementid", this.id);

                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    start = rdr.GetDateTime(5);
                    checkstart = true;
                }
                rdr.Close();

                cmd.CommandText = "SELECT id, freemeasurements_tasks_events.freemeasurementid, freemeasurements_tasks_events.taskid, user, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    +" WHERE freemeasurementid=@measurementid AND (eventtype='F' OR eventtype='P') "
                    + " AND freemeasurements_tasks.NoProductiveTaskId=-1 "
                    + " ORDER BY eventdate DESC";
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    end = rdr.GetDateTime(5);
                    checkend = true;
                }
                rdr.Close();
                conn.Close();

                if (checkstart && checkend && end >= start)
                {
                    lt_Hours = (end - start).TotalSeconds;
                }
            }
            return lt_Hours / 3600;
        }

        /* Returns:
       * 0 if generic error
       * 1 if everything ok
       * 2 if there were some errors while adding timespans to the database
       */
        public int TransformEventsToTimespans()
        {
            int ret = 0;
            if (this.id != -1)
            {
                List<FreeMeasurements_Tasks_Events_Timespan> timespans = new List<FreeMeasurements_Tasks_Events_Timespan>();
                List<FreeMeasurements_Tasks_Event> events = new List<FreeMeasurements_Tasks_Event>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();

                // Get all events of finished tasks there are not in the timespans table
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT freemeasurements_tasks_events.id, "
                                + " freemeasurements_tasks_events.freemeasurementid, "
                                + " freemeasurements_tasks_events.taskid, "
                                + " freemeasurements_tasks_events.user, "
                                + " freemeasurements_tasks_events.eventtype, "
                                + " freemeasurements_tasks_events.eventdate, "
                                + " freemeasurements_tasks_events.notes FROM "
                                + " freemeasurements_tasks INNER JOIN freemeasurements_tasks_events "
                                + " ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                                + " LEFT JOIN  freemeasurements_tasks_events_timespans "
                                + " ON(freemeasurements_tasks_events_timespans.starteventid = freemeasurements_tasks_events.id OR "
                                + " freemeasurements_tasks_events_timespans.endeventid = freemeasurements_tasks_events.id) "
                                + " WHERE 1 = 1 "
                                + " AND freemeasurements_tasks.measurementid=@measurementid "
                                + " AND freemeasurements_tasks.status = 'F' "
                                + " AND freemeasurements_tasks_events_timespans.id IS NULL"
                                + " ORDER BY freemeasurements_tasks_events.freemeasurementid, "
                                + " freemeasurements_tasks_events.taskid, "
                                + " freemeasurements_tasks_events.user, "
                                + " freemeasurements_tasks_events.eventdate";
                cmd.Parameters.AddWithValue("@measurementid", this.id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    FreeMeasurements_Tasks_Event currEv = new FreeMeasurements_Tasks_Event();
                    currEv.id = rdr.GetInt32(0);
                    currEv.freemeasurementid = rdr.GetInt32(1);
                    currEv.taskid = rdr.GetInt32(2);
                    currEv.user = rdr.GetString(3);
                    currEv.eventtype = rdr.GetChar(4);
                    currEv.eventdate = rdr.GetDateTime(5);
                    currEv.notes = rdr.GetString(6);
                    events.Add(currEv);
                }
                rdr.Close();

                // Transform all events to timespans
                for (int i = 0; i < events.Count; i += 2)
                {
                    if (events[i].eventtype == 'I' && (events[i + 1].eventtype == 'F' || events[i + 1].eventtype == 'P')
                        && events[i].user == events[i + 1].user
                        && events[i].freemeasurementid == events[i + 1].freemeasurementid
                        && events[i].taskid == events[i + 1].taskid)
                    {
                        FreeMeasurements_Tasks_Events_Timespan currTs = new FreeMeasurements_Tasks_Events_Timespan();
                        currTs.freemeasurementid = events[i].freemeasurementid;
                        currTs.taskid = events[i].taskid;
                        currTs.user = events[i].user;
                        currTs.starteventid = events[i].id;
                        currTs.starteventtype = events[i].eventtype;
                        currTs.starteventdate = events[i].eventdate;
                        currTs.starteventnotes = events[i].notes;
                        currTs.endeventid = events[i + 1].id;
                        currTs.endeventtype = events[i + 1].eventtype;
                        currTs.endeventdate = events[i + 1].eventdate;
                        currTs.endeventnotes = events[i + 1].notes;

                        timespans.Add(currTs);
                    }
                    else
                    {
                        ret = 3;
                        this.log += "Error in events " + events[i].id + ", " + events[i + 1].id + " \n";
                        i--;
                    }
                }

                // Write all the timespans in the database
                foreach (var ts in timespans)
                {
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmdTs = conn.CreateCommand();
                    cmdTs.CommandText = "INSERT INTO freemeasurements_tasks_events_timespans (measurementid,taskid,inputpoint,starteventid,starteventtype,starteventdate,starteventnotes, "
                        + "endeventid, endeventtype, endeventdate, endeventnotes) "
                        + " VALUES (@measurementid, @taskid, @inputpoint, @starteventid, @starteventtype, @starteventdate, @starteventnotes, "
                        + " @endeventid, @endeventtype, @endeventdate, @endeventnotes)";
                    cmdTs.Parameters.AddWithValue("@measurementid", ts.freemeasurementid);
                    cmdTs.Parameters.AddWithValue("@taskid", ts.taskid);
                    cmdTs.Parameters.AddWithValue("@inputpoint", ts.user);
                    cmdTs.Parameters.AddWithValue("@starteventid", ts.starteventid);
                    cmdTs.Parameters.AddWithValue("@starteventtype", ts.starteventtype);
                    cmdTs.Parameters.AddWithValue("@starteventdate", ts.starteventdate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmdTs.Parameters.AddWithValue("@starteventnotes", ts.starteventnotes);
                    cmdTs.Parameters.AddWithValue("@endeventid", ts.endeventid);
                    cmdTs.Parameters.AddWithValue("@endeventtype", ts.endeventtype);
                    cmdTs.Parameters.AddWithValue("@endeventdate", ts.endeventdate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmdTs.Parameters.AddWithValue("@endeventnotes", ts.endeventnotes);
                    try
                    {
                        cmdTs.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        this.log += ex.Message + " \n";
                        tr.Rollback();
                        ret = 2;
                    }
                }

                conn.Close();
            }
            return ret;
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
         * if input = 'A' then loads all
         */
        public void loadMeasurements(char Status)
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            String strWhere = "";
            if(Status != 'A')
            { 
                if (Status == 'O')
                {
                    strWhere = " WHERE status <> 'F'";
                }
                else
                {
                    strWhere = " WHERE status = '" + Status + "'";
                }
            }
            cmd.CommandText = "SELECT id FROM freemeasurements  " + strWhere + " ORDER BY plannedstartdate, plannedenddate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int measurementid = rdr.GetInt32(0);
                FreeTimeMeasurement curr = new FreeTimeMeasurement(measurementid);
                this.MeasurementsList.Add(curr);
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

        /* Returns:
         * FreeMeasurementId if everything is ok
         * -1 if generic error
         * -2 if input error
         * -3 if error while adding
         */
        public int AddBatch(String createdby, DateTime plannedstartdate, DateTime plannedenddate, String name, String description,
             String serialnumber, Double quantity, Double realleadtime, Double realworkingtime,
             int measurementUnitId = 0, Boolean AllowCustomTasks = true, Boolean AllowExecuteFinishedTasks = true)
        {
            int ret = -1;
            if (name.Length < 255)
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO freemeasurements(createdby, plannedstartdate, plannedenddate, departmentid, name, description,"
                        + " processid, processrev, variantid, status, serialnumber, quantity, measurementunit, "
                        + " AllowCustomTasks, ExecuteFinishedTasks, MeasurementType, realenddate, realleadtime, realworkingtime) "
                        + " VALUES(@createdby, @plannedstartdate, @plannedenddate, @departmentid, @name, @description,"
                        + " @processid, @processrev, @variantid, @status, @serialnumber, @quantity, @measurementunit, "
                        + " @AllowCustomTasks, @ExecuteFinishedTasks, @MeasurementType, @realenddate, @realleadtime, @realworkingtime)";

                    cmd.Parameters.AddWithValue("@createdby", createdby);
                    cmd.Parameters.AddWithValue("@plannedstartdate", plannedstartdate);
                    cmd.Parameters.AddWithValue("@plannedenddate", plannedenddate);
                    cmd.Parameters.AddWithValue("@departmentid", null);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@processid", null);
                    cmd.Parameters.AddWithValue("@processrev", null);
                    cmd.Parameters.AddWithValue("@variantid", null);
                    cmd.Parameters.AddWithValue("@status", 'F');
                    cmd.Parameters.AddWithValue("@serialnumber", serialnumber);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@measurementunit", measurementUnitId);
                    cmd.Parameters.AddWithValue("@AllowCustomTasks", AllowCustomTasks);
                    cmd.Parameters.AddWithValue("@ExecuteFinishedTasks", AllowExecuteFinishedTasks);
                cmd.Parameters.AddWithValue("@MeasurementType", "B");
                cmd.Parameters.AddWithValue("@realenddate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@realleadtime", realleadtime);
                cmd.Parameters.AddWithValue("@realworkingtime", realworkingtime);

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
            return ret;
        }

        public List<FreeMeasurentsTasksJsonStruct> GetFreeMeasurentsTasksJson(int departmentId)
        {
            List<FreeMeasurentsTasksJsonStruct> fmStruct = new List<FreeMeasurentsTasksJsonStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT "
                + " freemeasurements.id, "              // 0
                + " freemeasurements.creationdate, "    // 1
                + " freemeasurements.createdby, "
                + " freemeasurements.plannedstartdate, "
                + " freemeasurements.plannedenddate, "
                + " freemeasurements.departmentid, "    // 5
                + " freemeasurements.name AS MeasurementName,"
                + " freemeasurements.description AS MeasurementDescription, "
                + " freemeasurements.ProcessId, "
                + " freemeasurements.processrev, "
                + " freemeasurements.variantid, "       // 10
                + " freemeasurements.status, "
                + " freemeasurements.serialnumber, "
                + " freemeasurements.quantity, "
                + " freemeasurements.measurementUnit, "
                + " measurementunits.type, "            // 15
                + " freemeasurements_tasks.taskid, "
                + " freemeasurements_tasks.origtaskid, "
                + " freemeasurements_tasks.origtaskrev, "
                + " freemeasurements_tasks.variantid, "
                + " freemeasurements_tasks.noproductivetaskid, "    // 20
                + " freemeasurements_tasks.name AS TaskName, "
                + " freemeasurements_tasks.description AS TaskDescription, "
                + " freemeasurements_tasks.sequence, "
                + " freemeasurements_tasks.workstationid, "
                + " postazioni.name, "                      // 25
                + " freemeasurements_tasks.quantity_planned, "
                + " freemeasurements_tasks.status AS TaskStatus, " // 27
                + " processo.name AS ProcessName, "
                + " varianti.nomeVariante AS ProductName, "
                + " freemeasurements.AllowCustomTasks, "        // 30
                + " freemeasurements.ExecuteFinishedTasks "     // 31
                + " FROM freemeasurements INNER JOIN freemeasurements_tasks "
                + " ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
                + " LEFT JOIN postazioni ON(postazioni.idpostazioni = freemeasurements_tasks.workstationid) "
                + " LEFT JOIN measurementunits ON (measurementunits.id = freemeasurements.measurementUnit) "
                + " INNER JOIN variantiprocessi Product ON (freemeasurements.ProcessId= Product.processo AND freemeasurements.Processrev = Product.revproc "
                + " AND freemeasurements.variantid = Product.variante) "
                + " INNER JOIN processo ON(PROCESSO.processid = Product.processo AND processo.revisione = product.revproc) "
                + " INNER JOIN varianti ON(varianti.idvariante = product.variante) "
                + " WHERE departmentid = @departmentid "
                + " AND freemeasurements.status <> 'F' "
                + " AND(ExecuteFinishedTasks = true OR(ExecuteFinishedTasks = false AND freemeasurements_tasks.status <> 'F')) "
                + " ORDER BY freemeasurements.id, freemeasurements_tasks.sequence";
            cmd.Parameters.AddWithValue("@departmentid", departmentId);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                FreeMeasurentsTasksJsonStruct curr = new FreeMeasurentsTasksJsonStruct();
                curr.MeasurementId = rdr.GetInt32(0);
                curr.Creationdate = rdr.GetDateTime(1);
                curr.CreatedBy = rdr.GetString(2);
                curr.PlannedStartDate = rdr.GetDateTime(3);
                curr.PlannedEndDate = rdr.GetDateTime(4);
                curr.DepartmentId = rdr.GetInt32(5);
                curr.MeasurementName = rdr.GetString(6);
                curr.MeasurementDescription = rdr.GetString(7);
                curr.ProcessId = rdr.GetInt32(8);
                curr.ProcessRev = rdr.GetInt32(9);
                curr.VariantId = rdr.GetInt32(10);
                curr.Status = rdr.GetChar(11);
                curr.SerialNumber = rdr.GetString(12);
                curr.Quantity = rdr.GetDouble(13);
                curr.MeasurementUnitId = rdr.GetInt32(14);
                curr.MeasurementUnitType = rdr.GetString(15);
                curr.TaskId = rdr.GetInt32(16);
                curr.OrigTaskId = rdr.IsDBNull(17) ? -1 : rdr.GetInt32(17);
                curr.OrigTaskRev = rdr.IsDBNull(18) ? -1 : rdr.GetInt32(18);
                curr.VariantId = rdr.IsDBNull(19) ? -1 : rdr.GetInt32(19);
                curr.NoProductiveTaskId = rdr.IsDBNull(20) ? -1 : rdr.GetInt32(20);
                curr.TaskName = rdr.GetString(21);
                curr.TaskDescription = rdr.GetString(22);
                curr.Sequence = rdr.GetInt32(23);
                curr.WorkstationId = rdr.IsDBNull(24) ? -1 : rdr.GetInt32(24);
                curr.WorkstationName = rdr.IsDBNull(25) ? "" : rdr.GetString(25);
                curr.TaskQuantity = rdr.GetDouble(26);
                curr.TaskStatus = rdr.GetChar(27);
                curr.ProcessName = rdr.GetString(28);
                curr.VariantName = rdr.GetString(29);
                curr.AllowCustomTasks = rdr.GetBoolean(30);
                curr.ExecuteFinishedTasks = rdr.GetBoolean(31);
                fmStruct.Add(curr);
            }
            rdr.Close();
            conn.Close();

            return fmStruct;
        }

        public List<FreeMeasurentsTasksJsonStruct> GetRunningTasks(Reparto dept, User usr)
        {
            List<FreeMeasurentsTasksJsonStruct> ret = new List<FreeMeasurentsTasksJsonStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT "
                + " freemeasurements.id, "
                + " freemeasurements_tasks.taskid, "
                   + " freemeasurements_tasks.name AS TaskName, "
                   + "  postazioni.name AS WorkstationName, "
                   + "  freemeasurements_tasks.quantity_planned, "
                   + "  measurementunits.type, "
                   + " freemeasurements.name AS MeasurementName, "
                   + " runningtasksid, "
                   + " freemeasurements_tasks.NoProductiveTaskId "
                   + " FROM "
            + " (SELECT MAX(runningtasks.id) AS runningtasksid "
              + " FROM "
              + "   (SELECT freemeasurements_tasks_events.id, freemeasurements_tasks_events.eventtype, "
              + "   freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, freemeasurements_tasks_events.eventdate "
              + "               FROM freemeasurements_tasks "
              + "                INNER JOIN freemeasurements_tasks_events "
              + "               ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
              + "               INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
              + "               WHERE freemeasurements_tasks.status = 'I' "
              + "                AND freemeasurements_tasks_events.user = @usr "
              + "                AND (freemeasurements.departmentid = @departmentid OR freemeasurements_tasks.NoProductiveTaskId IS NOT NULL) "
              + "               ORDER BY freemeasurements_tasks_events.eventdate DESC) AS runningtasks "
              + "               GROUP BY runningtasks.taskid) AS runningtasks2 "
              + "  INNER JOIN freemeasurements_tasks_events AS freemeasurements_tasks_events2 ON(freemeasurements_tasks_events2.id = runningtasks2.runningtasksid) "
              + "  INNER JOIN freemeasurements_tasks ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events2.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events2.taskid) "
              + "  inner join freemeasurements ON(freemeasurements.id = freemeasurements_tasks.MeasurementId) "
              + "  INNER JOIN measurementunits ON(measurementunits.id = freemeasurements.measurementUnit) "
              + "  LEFT JOIN postazioni ON(freemeasurements_tasks.workstationid = postazioni.idpostazioni) "
                + " WHERE eventtype = 'I'";

            cmd.Parameters.AddWithValue("@usr", usr.username);
            cmd.Parameters.AddWithValue("@departmentid", dept.id);

            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                FreeMeasurentsTasksJsonStruct curr = new FreeMeasurentsTasksJsonStruct();
                curr.MeasurementId = rdr.GetInt32(0);
                curr.TaskId = rdr.GetInt32(1);
                curr.TaskName = rdr.GetString(2);
                curr.WorkstationName = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                curr.TaskQuantity = rdr.GetDouble(4);
                curr.MeasurementUnitType = rdr.GetString(5);
                curr.MeasurementName = rdr.GetString(6);
                curr.LastTaskEventId = rdr.GetInt32(7);
                curr.NoProductiveTaskId = rdr.IsDBNull(8) ? -1 : rdr.GetInt32(8);
                ret.Add(curr);
            }
            rdr.Close();
            conn.Close();
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if everything ok
         * 2 if there were some errors while adding timespans to the database
         */
        public int TransformEventsToTimespans()
        {
            int ret = 0;
            List<FreeMeasurements_Tasks_Events_Timespan> timespans = new List<FreeMeasurements_Tasks_Events_Timespan>();
            List<FreeMeasurements_Tasks_Event> events = new List<FreeMeasurements_Tasks_Event>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();

            // Get all events of finished tasks there are not in the timespans table
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT freemeasurements_tasks_events.id, "
                            + " freemeasurements_tasks_events.freemeasurementid, "
                            + " freemeasurements_tasks_events.taskid, "
                            + " freemeasurements_tasks_events.user, "
                            + " freemeasurements_tasks_events.eventtype, "
                            + " freemeasurements_tasks_events.eventdate, "
                            + " freemeasurements_tasks_events.notes FROM "
                            + " freemeasurements_tasks INNER JOIN freemeasurements_tasks_events "
                            + " ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                            + " LEFT JOIN  freemeasurements_tasks_events_timespans "
                            + " ON(freemeasurements_tasks_events_timespans.starteventid = freemeasurements_tasks_events.id OR "
                            + " freemeasurements_tasks_events_timespans.endeventid = freemeasurements_tasks_events.id) "
                            + " WHERE 1 = 1 "
                            + " AND freemeasurements_tasks.status = 'F' "
                            + " AND freemeasurements_tasks_events_timespans.id IS NULL"
                            + " ORDER BY freemeasurements_tasks_events.freemeasurementid, "
                            + " freemeasurements_tasks_events.user, "
                            + " freemeasurements_tasks_events.taskid, "
                            + " freemeasurements_tasks_events.eventdate";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                FreeMeasurements_Tasks_Event currEv = new FreeMeasurements_Tasks_Event();
                currEv.id = rdr.GetInt32(0);
                currEv.freemeasurementid = rdr.GetInt32(1);
                currEv.taskid = rdr.GetInt32(2);
                currEv.user = rdr.GetString(3);
                currEv.eventtype = rdr.GetChar(4);
                currEv.eventdate = rdr.GetDateTime(5);
                currEv.notes = rdr.GetString(6);
                events.Add(currEv);
            }
            rdr.Close();

            // Transform all events to timespans
            for(int i = 0; i < events.Count - 1; i+=2)
            {
                if(events[i].eventtype == 'I' && (events[i+1].eventtype == 'F' || events[i+1].eventtype == 'P')
                    && events[i].user == events[i + 1].user
                    && events[i].freemeasurementid == events[i+1].freemeasurementid
                    && events[i].taskid == events[i + 1].taskid)
                {
                    FreeMeasurements_Tasks_Events_Timespan currTs = new FreeMeasurements_Tasks_Events_Timespan();
                    currTs.freemeasurementid = events[i].freemeasurementid;
                    currTs.taskid = events[i].taskid;
                    currTs.user = events[i].user;
                    currTs.starteventid = events[i].id;
                    currTs.starteventtype = events[i].eventtype;
                    currTs.starteventdate = events[i].eventdate;
                    currTs.starteventnotes = events[i].notes;
                    currTs.endeventid = events[i + 1].id;
                    currTs.endeventtype = events[i+1].eventtype;
                    currTs.endeventdate = events[i+1].eventdate;
                    currTs.endeventnotes = events[i+1].notes;

                    timespans.Add(currTs);
                }
                else
                {
                    ret = 3;
                    this.log += "Error in events " + events[i].id + " - " + events[i].id + " \n";
                    i--;
                }
            }

            // Write all the timespans in the database
            foreach(var ts in timespans)
            {
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmdTs = conn.CreateCommand();
                cmdTs.CommandText = "INSERT INTO freemeasurements_tasks_events_timespans (measurementid,taskid,inputpoint,starteventid,starteventtype,starteventdate,starteventnotes, " 
                    + "endeventid, endeventtype, endeventdate, endeventnotes) "
                    + " VALUES (@measurementid, @taskid, @inputpoint, @starteventid, @starteventtype, @starteventdate, @starteventnotes, "
                    + " @endeventid, @endeventtype, @endeventdate, @endeventnotes)";
                cmdTs.Parameters.AddWithValue("@measurementid", ts.freemeasurementid);
                cmdTs.Parameters.AddWithValue("@taskid", ts.taskid);
                cmdTs.Parameters.AddWithValue("@inputpoint", ts.user);
                cmdTs.Parameters.AddWithValue("@starteventid", ts.starteventid);
                cmdTs.Parameters.AddWithValue("@starteventtype", ts.starteventtype);
                cmdTs.Parameters.AddWithValue("@starteventdate", ts.starteventdate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmdTs.Parameters.AddWithValue("@starteventnotes", ts.starteventnotes);
                cmdTs.Parameters.AddWithValue("@endeventid", ts.endeventid);
                cmdTs.Parameters.AddWithValue("@endeventtype", ts.endeventtype);
                cmdTs.Parameters.AddWithValue("@endeventdate", ts.endeventdate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmdTs.Parameters.AddWithValue("@endeventnotes", ts.endeventnotes);
                try
                {
                    cmdTs.ExecuteNonQuery();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    this.log += ex.Message + " \n";
                    tr.Rollback();
                    ret = 2;
                }
            }

            conn.Close();

            return ret;
        }

        public void loadMeasurementsToBeClosed()
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT freemeasurements.id FROM freemeasurements LEFT JOIN "
                + " (SELECT DISTINCT(measurementid) FROM freemeasurements_tasks WHERE status <> 'F' AND noproductivetaskid IS NULL) AS openmeasurements "
                + " ON(freemeasurements.id = openmeasurements.measurementid) WHERE status <> 'F' AND measurementid IS NULL";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this.MeasurementsList.Add(new FreeTimeMeasurement(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
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
                        this._Status = value;
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
                    cmd.CommandText = "UPDATE freemeasurements_tasks SET RealWorkingTime_Hours=@workingtime WHERE measurementid=@measurementid AND taskid=@taskid";
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

        /* Step is the pace used by the operator.
         * Default = 60
         * If > 60 the operator kept a fast pace
         * if lower than 60 the operator kept a slower pace
         */
        private Double _Step;
        public Double Step { get { return this._Step; } }

        private Boolean _isAcyclic;
        public Boolean isAcyclic { get { return this._isAcyclic; } }

        /* Cycle Time expressed in HOURS */
        private Double _Acyclic_CycleTime;
        public double Acyclic_CycleTime { get { return this._Acyclic_CycleTime; } }

        private Double _Acyclic_QuantityUsed;
        public Double Acyclic_QuantityUsed { get { return this._Acyclic_QuantityUsed; } }

        private Double _Acyclic_QuantityForEachProduct;
        public Double Acyclic_QuantityForEachProduct { get { return this._Acyclic_QuantityForEachProduct; } }

        /* Values admitted
         * V = Value
         * E = Evident Waste
         * H = Hidden Waste
         */
        private Char _ValueOrWaste;
        public Char ValueOrWaste { get { return this._ValueOrWaste; } }

        /* Values admitted
         * 1 = Normale, con tronco quasi fermo
         * 2 = Normale, con tronco entro 45°
         * 3 = Disagevole con movimenti entro i 45°
         * 4 = Disagevole con movimenti del tronco molto ampi, > 45°
         */
        private int _Ergonomy;
        public int Ergonomy { get { return this._Ergonomy; } }

        public List<FreeMeasurements_Tasks_Event> TaskEvents;

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
                    + " freemeasurements.DepartmentId, "         // 20
                    + " freemeasurements_tasks.step, "
                    + " freemeasurements_tasks.isacyclic, "
                    + " freemeasurements_tasks.acyclic_cycletime, "
                    + " freemeasurements_tasks.acyclic_qtyused, "
                    + " freemeasurements_tasks.acyclic_qtyforeachproduct, "  // 25
                    + " freemeasurements_tasks.valueorwaste, "
                    + " freemeasurements_tasks.ergonomy "           // 27
                    + "  FROM freemeasurements_tasks "
                    + " LEFT JOIN postazioni ON(postazioni.idpostazioni=freemeasurements_tasks.workstationid) "
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
                this._WorkstationId = rdr.IsDBNull(9) ? -1 : rdr.GetInt32(9);
                this._WorkstationName = rdr.IsDBNull(10) ? "" : rdr.GetString(10);
                this._PlannedQuantity = rdr.GetDouble(11);
                this._ProducedQuantity = rdr.IsDBNull(12) ? 0 : rdr.GetDouble(12);
                this._Status = rdr.GetChar(13);
                this._StartDateReal = rdr.IsDBNull(14) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(14);
                this._EndDateReal = rdr.IsDBNull(15) ? new DateTime(1970, 1, 1) : rdr.GetDateTime(15);
                this._RealLeadTime_Hours = rdr.IsDBNull(16) ? 0 : rdr.GetDouble(16);
                this._RealWorkingTime_Hours = rdr.IsDBNull(17) ? 0 : rdr.GetDouble(17);
                this._AllowCustomTasks = rdr.GetBoolean(18);
                this._AllowExecuteFinishedTasks = rdr.GetBoolean(19);
                this._DepartmentId = rdr.GetInt32(20);
                this._Step = rdr.IsDBNull(21) ? 60 : rdr.GetInt32(21);                this._isAcyclic = rdr.IsDBNull(22) ? false : rdr.GetBoolean(22);
                this._Acyclic_CycleTime = rdr.IsDBNull(23) ? 0 : rdr.GetDouble(23);                this._Acyclic_QuantityUsed = rdr.IsDBNull(24) ? 0 : rdr.GetDouble(24);
                this._Acyclic_QuantityForEachProduct = rdr.IsDBNull(25) ? 0 : rdr.GetDouble(25);
                this._ValueOrWaste = rdr.IsDBNull(26) ? '\0' : rdr.GetChar(26);
                this._Ergonomy = rdr.IsDBNull(27) ? -1 : rdr.GetInt32(27);
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
         * 6 if user is already running the task
         */
        public int Start(User op)
        {
            int ret = 0;
            if(this.Status == 'I' || this.Status == 'N' || this.Status == 'P' 
                || (this.Status == 'F' && this.AllowExecuteFinishedTasks && this.NoProductiveTaskId == -1)
                || this.NoProductiveTaskId != -1)
            {
                DateTime eventtime = DateTime.UtcNow;

                if(this.Status == 'N')
                {
                    this.StartDateReal = eventtime;
                }

                Reparto dept = new Reparto(this.DepartmentId);
                int maxTasksInExecution = dept.TasksAvviabiliContemporaneamenteDaOperatore;
                FreeTimeMeasurement fmMeas = new FreeTimeMeasurement(this.MeasurementId);
                FreeTimeMeasurement prevMeas = null;
                op.loadFreeMeasurementRunningTasks();
                int tasksInExecution = op.FreeMeasurementTasks.Count;
                Boolean checkMaxTasks = false;
                Boolean PauseDefaultNoProductiveTask = false;
                int npTask = -1;
                int npTaskMeasurentId = -1;
                if ((maxTasksInExecution > 0 && tasksInExecution < maxTasksInExecution) || maxTasksInExecution == 0)
                { 
                    checkMaxTasks = true; 
                }

                if(op.FreeMeasurementTasks.Count == 1 && op.FreeMeasurementTasks[0].NoProductiveTaskId != -1 
                     && (op.FreeMeasurementTasks[0].TaskId != this.TaskId || op.FreeMeasurementTasks[0].MeasurementId != this.MeasurementId))
                {
                    checkMaxTasks = true;
                    PauseDefaultNoProductiveTask = true;
                    npTask = op.FreeMeasurementTasks[0].TaskId;
                    npTaskMeasurentId = op.FreeMeasurementTasks[0].MeasurementId;
                    prevMeas = new KIS.App_Sources.FreeTimeMeasurement(op.FreeMeasurementTasks[0].MeasurementId);
                }

                // Check that user is not already running this task
                Boolean checkAlreadyInExecution = false;
                try
                {
                    var found = op.FreeMeasurementTasks.First(y => y.MeasurementId == this.MeasurementId && y.TaskId == this.TaskId); 
                    checkAlreadyInExecution = true;
                    ret = 6;
                }
                catch
                {
                    checkAlreadyInExecution = false;
                }

                if (checkMaxTasks && !checkAlreadyInExecution)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        if (PauseDefaultNoProductiveTask)
                        {
                            char npStatus = 'P';
                            if(prevMeas!= null && prevMeas.Status == 'F')
                            {
                                npStatus = 'F';
                            }
                            MySqlCommand cmdDef = conn.CreateCommand();
                            cmdDef.Transaction = tr;
                            cmdDef.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                                + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                            cmdDef.Parameters.AddWithValue("@freemeasurementid", npTaskMeasurentId);
                            cmdDef.Parameters.AddWithValue("@taskid", npTask);
                            cmdDef.Parameters.AddWithValue("@user", op.username);
                            cmdDef.Parameters.AddWithValue("@eventtype", npStatus);
                            cmdDef.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmdDef.Parameters.AddWithValue("@notes", "");
                            cmdDef.ExecuteNonQuery();
                        }


                        cmd.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                                + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                        cmd.Parameters.AddWithValue("@freemeasurementid", this.MeasurementId);
                        cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                        cmd.Parameters.AddWithValue("@user", op.username);
                        cmd.Parameters.AddWithValue("@eventtype", 'I');
                        cmd.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@notes", "");
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this.Status = 'I';
                        fmMeas.Status = 'I';


                        // Eventually, change the status of the default no productive task
                        if (PauseDefaultNoProductiveTask)
                        {
                            FreeMeasurement_Task defTask = new FreeMeasurement_Task(npTaskMeasurentId, npTask);
                            defTask.loadActiveUsers();
                            if (prevMeas.Status == 'F')
                            {
                                defTask.Status = 'F';
                                defTask.EndDateReal = eventtime;
                                Double pWt = defTask.calculateWorkingTime();
                                defTask.RealWorkingTime_Hours = pWt;
                                Double pLt = defTask.calculateLeadTime();
                                defTask.RealLeadTime_Hours = pLt;
                            }
                            else if(defTask.Users.Count == 0)
                            {
                                FreeTimeMeasurement fm = new FreeTimeMeasurement(defTask.MeasurementId);
                                if(fm.Status == 'F')
                                {
                                    defTask.Status = 'F';
                                    defTask.EndDateReal = eventtime;
                                    Double pWt = defTask.calculateWorkingTime();
                                    defTask.RealWorkingTime_Hours = pWt;
                                    Double pLt = defTask.calculateLeadTime();
                                    defTask.RealLeadTime_Hours = pLt;
                                }
                                else
                                { 
                                    defTask.Status = 'P';
                                }
                            }
                        }

                        FreeTimeMeasurements fms = new FreeTimeMeasurements();
                        fms.TransformEventsToTimespans();

                        ret = 1;
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                        ret = 5;
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
          ON (freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) 
          INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) WHERE freemeasurements_tasks.status = 'I' 
          AND freemeasurements_tasks_events.user='admin' 
          -- AND freemeasurements.departmentid=0 
          ORDER BY freemeasurements_tasks_events.eventdate;
          
          
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
            op.loadFreeMeasurementRunningTasks();
            bool running = false;
            try
            {
                op.FreeMeasurementTasks.First(y => y.MeasurementId == this.MeasurementId && y.TaskId == this.TaskId);
                running = true;
            }
            catch
            {
                running = false;
            }

            if (this.Status == 'I' && op.username.Length > 0 && running)
            {
                FreeTimeMeasurement fms = new FreeTimeMeasurement(this.MeasurementId);
                char eventtype = 'P';
                eventtype = fms.Status == 'F' ? 'F' : 'P';

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
                cmd.Parameters.AddWithValue("@eventtype", eventtype);
                cmd.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@notes", "");
                
                try
                { 
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                }

                this.loadActiveUsers();
                if(this.Users.Count == 0)
                {
                    if(fms.Status == 'F')
                    {
                        this.Status = 'F';
                        this.EndDateReal = eventtime;
                        Double pWt = this.calculateWorkingTime();
                        this.RealWorkingTime_Hours = pWt;
                        Double pLt = this.calculateLeadTime();
                        this.RealLeadTime_Hours = pLt;
                    }
                    else
                    { 
                        this.Status = 'P';
                    }
                }

                op.loadFreeMeasurementRunningTasks();
                if (op.FreeMeasurementTasks.Count == 0)
                {
                    NoProductiveTasks npts = new NoProductiveTasks();
                    var defTask = npts.TaskList.FirstOrDefault(x => x.IsDefault == true);
                    if(defTask!=null && defTask.ID != -1)
                    {
                        MySqlCommand cmdDef = conn.CreateCommand();
                        MySqlTransaction tr2 = conn.BeginTransaction();
                        cmdDef.Transaction = tr2;

                        // ADD NO PRODUCTIVE TASK IN TASK_LIST
                        int nptask = -1;
                        int npmeasurement = -1;
                        FreeTimeMeasurement fm = new FreeTimeMeasurement(this.MeasurementId);
                        if(fm.Status!='F')
                        { 
                            nptask = fm.addTask(defTask);
                            npmeasurement = this.MeasurementId;
                        }
                        else
                        {
                            FreeTimeMeasurements fmss = new FreeTimeMeasurements();
                            fmss.loadMeasurements('O');
                            if(fmss.MeasurementsList.Count > 0)
                            {
                                nptask = fmss.MeasurementsList[0].addTask(defTask);
                                npmeasurement = fmss.MeasurementsList[0].id;
                            }
                        }
                        if (nptask!=-1)
                        { 
                            try
                            {
                                cmdDef.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                                   + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                                cmdDef.Parameters.AddWithValue("@freemeasurementid", npmeasurement);
                                cmdDef.Parameters.AddWithValue("@taskid", nptask);
                                cmdDef.Parameters.AddWithValue("@user", op.username);
                                cmdDef.Parameters.AddWithValue("@eventtype", 'I');
                                cmdDef.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss"));
                                cmdDef.Parameters.AddWithValue("@notes", "");
                                cmdDef.ExecuteNonQuery();

                                cmdDef.CommandText = "UPDATE freemeasurements_tasks SET status='I', task_startdatereal=@eventdate WHERE measurementid=@freemeasurementid AND TaskId=@taskid";
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
                }

                conn.Close();
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        /*
          * Returns: 
          * 0 if generic error
          * 1 if Paused succesfully
          * 2 if task is not running or operator not found
          */
        public int Finish(User op)
        {
            int ret = 0;
            if (this.Status == 'I' && op.username.Length > 0 && this.NoProductiveTaskId == -1)
            {
                DateTime eventtime = DateTime.UtcNow;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    // Close task for all the users
                    this.loadActiveUsers();

                    foreach(var usr in this.Users)
                    {
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Transaction = tr;
                        cmd.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                           + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                        cmd.Parameters.AddWithValue("@freemeasurementid", this.MeasurementId);
                        cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                        cmd.Parameters.AddWithValue("@user", usr);
                        cmd.Parameters.AddWithValue("@eventtype", 'F');
                        cmd.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@notes", "Task finished by " + op.username);

                        cmd.ExecuteNonQuery();
                    }
                    tr.Commit();

                    this.EndDateReal = eventtime;
                    this.Status = 'F';
                }
                catch (Exception ex)
                {
                    this.log = ex.Message;
                    tr.Rollback();
                }

                Double leadtime = this.calculateLeadTime();
                Double workingtime = this.calculateWorkingTime();
                // Calculates leadtime and workingtime and writes in the database
                MySqlCommand cmd2 = conn.CreateCommand();
                tr = conn.BeginTransaction();
                cmd2.Transaction = tr;
                cmd2.CommandText = "UPDATE freemeasurements_tasks SET realleadtime_hours=@leadtime, realworkingtime_hours=@workingtime WHERE "
                   + " MeasurementId=@measurementid AND taskid=@taskid";
                cmd2.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                cmd2.Parameters.AddWithValue("@taskid", this.TaskId);
                cmd2.Parameters.AddWithValue("@leadtime", leadtime);
                cmd2.Parameters.AddWithValue("@workingtime", workingtime);
                try
                { 
                    cmd2.ExecuteNonQuery();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    tr.Rollback();
                }

                NoProductiveTasks npts = new NoProductiveTasks();
                var defTask = npts.TaskList.FirstOrDefault(x => x.IsDefault == true);
                if (defTask != null && defTask.ID != -1)
                {
                    // ADD NO PRODUCTIVE TASK IN TASK_LIST
                    FreeTimeMeasurement fm = new FreeTimeMeasurement(this.MeasurementId);
                    // int nptask = fm.addTask(defTask);
                    int nptask = -1;
                    int npmeasurement = -1;
                    if (fm.Status != 'F')
                    {
                        nptask = fm.addTask(defTask);
                        npmeasurement = this.MeasurementId;
                    }
                    else
                    {
                        FreeTimeMeasurements fmss = new FreeTimeMeasurements();
                        fmss.loadMeasurements('I');
                        if (fmss.MeasurementsList.Count > 0)
                        {
                            nptask = fmss.MeasurementsList[0].addTask(defTask);
                            npmeasurement = fmss.MeasurementsList[0].id;
                        }
                    }
                    if (nptask != -1)
                    {
                        FreeMeasurement_Task npTsk = new FreeMeasurement_Task(this.MeasurementId, nptask);

                        foreach (var usr in this.Users)
                        {
                            User cUsr = new User(usr);
                            cUsr.loadFreeMeasurementRunningTasks();
                            if (cUsr.FreeMeasurementTasks.Count == 0)
                            {
                                MySqlCommand cmdDef = conn.CreateCommand();
                                MySqlTransaction tr2 = conn.BeginTransaction();
                                cmdDef.Transaction = tr2;

                                try
                                {
                                    cmdDef.CommandText = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, user, eventtype, eventdate, notes) "
                                       + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                                    cmdDef.Parameters.AddWithValue("@freemeasurementid", npmeasurement);
                                    cmdDef.Parameters.AddWithValue("@taskid", nptask);
                                    cmdDef.Parameters.AddWithValue("@user", cUsr.username);
                                    cmdDef.Parameters.AddWithValue("@eventtype", 'I');
                                    cmdDef.Parameters.AddWithValue("@eventdate", eventtime.ToString("yyyy-MM-dd HH:mm:ss"));
                                    cmdDef.Parameters.AddWithValue("@notes", "");

                                    cmdDef.ExecuteNonQuery();
                                    tr2.Commit();
                                }
                                catch (Exception ex)
                                {
                                    this.log = ex.Message;
                                    tr2.Rollback();
                                }


                            }
                        }
                    
                        if(npTsk.Status=='N')
                        {
                            npTsk.StartDateReal = eventtime;
                        }
                        npTsk.Status = 'I';
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
                cmd.CommandText = "SELECT  "
                    + " freemeasurements.id,  "
                + " freemeasurements_tasks.taskid, "
                   + " freemeasurements_tasks.name AS TaskName, "
                    + " postazioni.name AS WorkstationName, "
                   + "  freemeasurements_tasks.quantity_planned, "
                   + "  measurementunits.type,"
                   + "  freemeasurements_tasks_events2.user "
                   + "      FROM"
                    + " (SELECT MAX(runningtasks.id) AS runningtasksid "
                    + "  FROM "
                + " (SELECT freemeasurements_tasks_events.id, freemeasurements_tasks_events.user, freemeasurements_tasks_events.eventtype, "
                + " freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, freemeasurements_tasks_events.eventdate "
                     + "        FROM freemeasurements_tasks "
                     + "         INNER JOIN freemeasurements_tasks_events "
                     + "        ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                     + "        INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
                     + "        WHERE freemeasurements_tasks.status = 'I' "
                    + " AND freemeasurements_tasks.taskid = @taskid "
                + " AND freemeasurements.id = @measurementid "
                + "             ORDER BY freemeasurements_tasks_events.eventdate DESC) AS runningtasks "
                + "             GROUP BY runningtasks.taskid, runningtasks.user) AS runningtasks2 "
               + " INNER JOIN freemeasurements_tasks_events AS freemeasurements_tasks_events2 ON(freemeasurements_tasks_events2.id = runningtasks2.runningtasksid)"
               + " INNER JOIN freemeasurements_tasks ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events2.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events2.taskid)"
               + " inner join freemeasurements ON(freemeasurements.id = freemeasurements_tasks.MeasurementId)"
               + " INNER JOIN measurementunits ON(measurementunits.id = freemeasurements.measurementUnit)"
               + " LEFT JOIN postazioni ON(postazioni.idpostazioni = freemeasurements_tasks.workstationid)"
                + " WHERE eventtype = 'I'; ";
                cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Users.Add(rdr.GetString(6));
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns total working hours of the task
         */
        public Double calculateWorkingTime()
        {
            Double wt_Hours = 0.0;
            if(this.MeasurementId!=-1 && this.TaskId!=-1 && (this.Status == 'F' || this.Status == 'P'))
            {
                List<FreeMeasurements_Tasks_Event> eventsLst = new List<FreeMeasurements_Tasks_Event>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, user, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    + " WHERE freemeasurements_tasks.measurementid=@measurementid AND freemeasurements_tasks.taskid=@taskid "
                   // + " AND freemeasurements_tasks.NoProductiveTaskId IS NULL "
                    + " ORDER BY user, eventdate";
                cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    FreeMeasurements_Tasks_Event curr = new FreeMeasurements_Tasks_Event();
                    curr.id = rdr.GetInt32(0);
                    curr.freemeasurementid = rdr.GetInt32(1);
                    curr.taskid = rdr.GetInt32(2);
                    curr.user = rdr.GetString(3);
                    curr.eventtype = rdr.GetChar(4);
                    curr.eventdate = rdr.GetDateTime(5);
                    curr.notes = rdr.GetString(6);
                    eventsLst.Add(curr);
                }
                rdr.Close();
                conn.Close();

                for (int i = 0; i < eventsLst.Count -1; i+=2)
                {
                  if(eventsLst[i].eventtype == 'I' && (eventsLst[i+1].eventtype=='F' || eventsLst[i+1].eventtype=='P'))
                    {
                        wt_Hours += (eventsLst[i+1].eventdate - eventsLst[i].eventdate).TotalSeconds;
                    }
                }

                
            }
            return wt_Hours/3600;
        }

        /* Returns total lead hours of the task
         */
        public Double calculateLeadTime()
        {
            Double lt_Hours = 0.0;
            if (this.MeasurementId != -1 && this.TaskId != -1 && (this.Status == 'F' || this.Status == 'P'))
            {
                List<FreeMeasurements_Tasks_Event> eventsLst = new List<FreeMeasurements_Tasks_Event>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();

                DateTime start = new DateTime(1970, 1, 1);
                DateTime end = new DateTime(1970, 1, 1);
                Boolean checkstart = false;
                Boolean checkend = false;
                // Gets the first event
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, user, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    + " WHERE freemeasurements_tasks.measurementid=@measurementid AND freemeasurements_tasks.taskid=@taskid AND eventtype='I' "
                   // + " AND freemeasurements_tasks.NoProductiveTaskId IS NULL "
                    + " ORDER BY eventdate";
                cmd.Parameters.AddWithValue("@measurementid", this.MeasurementId);
                cmd.Parameters.AddWithValue("@taskid", this.TaskId);

                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    start = rdr.GetDateTime(5);
                    checkstart = true;
                }
                rdr.Close();

                cmd.CommandText = "SELECT id, freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, user, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) " 
                    + " WHERE freemeasurements_tasks.measurementid=@measurementid AND freemeasurements_tasks.taskid=@taskid AND (eventtype='F' OR eventtype='P') "
                    // + " AND freemeasurements_tasks.NoProductiveTaskId IS NULL "
                    + " ORDER BY eventdate DESC";
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    end = rdr.GetDateTime(5);
                    checkend = true;
                }
                rdr.Close();
                conn.Close();

                if(checkstart && checkend && end >=start)
                {
                    lt_Hours = (end - start).TotalSeconds;
                }
            }
            return lt_Hours / 3600;
        }

        public void loadEvents()
        {
            this.TaskEvents = new List<FreeMeasurements_Tasks_Event>();
            if(this.MeasurementId != -1 && this.TaskId !=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, user, eventtype, eventdate, notes FROM freemeasurements_tasks_events WHERE freemeasurementid=@freemeasurementid AND taskid=@taskid "
                    + " ORDER BY eventdate";
                cmd.Parameters.AddWithValue("@freemeasurementid", this.MeasurementId);
                cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    FreeMeasurements_Tasks_Event fmev = new FreeMeasurements_Tasks_Event();
                    fmev.freemeasurementid = this.MeasurementId;
                    fmev.taskid = this.TaskId;
                    fmev.id = rdr.GetInt32(0);
                    fmev.user = rdr.GetString(1);
                    fmev.eventtype = rdr.GetChar(2);
                    fmev.eventdate = rdr.GetDateTime(3);
                    fmev.notes = rdr.GetString(4);
                    this.TaskEvents.Add(fmev);
                }
                rdr.Close();
                conn.Close();
            }
        }
    }

    public class FreeMeasurements_Tasks_Event
    {
        public String log;

        public int id;
        public int freemeasurementid;
        public int taskid;
        public String user;
        public Char eventtype;
        public DateTime eventdate;
        public String notes;

        public FreeMeasurements_Tasks_Event()
        {
            this.id = -1;
            this.freemeasurementid = -1;
            this.taskid = -1;
        }

        public FreeMeasurements_Tasks_Event(int eventid)
        {
            this.id = -1;
            this.freemeasurementid = -1;
            this.taskid = -1;
            if (eventid !=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, freemeasurementid, taskid, user, eventtype, eventdate, notes FROM freemeasurements_tasks_events WHERE id=@evid";
                cmd.Parameters.AddWithValue("@evid", eventid);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    this.id = rdr.GetInt32(0);
                    this.freemeasurementid = rdr.GetInt32(1);
                    this.taskid = rdr.GetInt32(2);
                    this.user = rdr.GetString(3);
                    this.eventtype = rdr.GetChar(4);
                    this.eventdate = rdr.GetDateTime(5);
                    this.notes = rdr.GetString(6);
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if note saved successfully
         * 2 if task event not found
         */
        public int SaveNote(String note)
        {
            int ret = 0;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE freemeasurements_tasks_events SET notes=@note WHERE id=@id";
            cmd.Parameters.AddWithValue("@note", note);
            cmd.Parameters.AddWithValue("@id", id);
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                this.notes = note;
                ret = 1;
            }
            catch(Exception ex)
            {
                this.log = ex.Message;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }
    }

    public class FreeMeasurements_Tasks_Events_Timespan
    {
        public int id;
        public int freemeasurementid;
        public int taskid;
        public String user;
        public int starteventid;
        public Char starteventtype;
        public DateTime starteventdate;
        public String starteventnotes;
        public int endeventid;
        public Char endeventtype;
        public DateTime endeventdate;
        public String endeventnotes;
    }

    public class FreeMeasurentsTasksJsonStruct
    {
        public int MeasurementId;
        public DateTime Creationdate;
        public String CreatedBy;
        public DateTime PlannedStartDate;
        public DateTime PlannedEndDate;
        public int DepartmentId;
        public String DepartmentName;
        public String MeasurementName;
        public String MeasurementDescription;
        public int ProcessId;
        public int ProcessRev;
        public int VariantId;
        public String ProductName;
        public char Status;
        public String SerialNumber;
        public Double Quantity;
        public int MeasurementUnitId;
        public String MeasurementUnitType;
        public int TaskId;
        public int OrigTaskId;
        public int OrigTaskRev;
        public int NoProductiveTaskId;
        public String TaskName;
        public String TaskDescription;
        public int Sequence;
        public int WorkstationId;
        public String WorkstationName;
        public Double TaskQuantity;
        public Char TaskStatus;
        public String ProcessName;
        public String VariantName;
        public Boolean AllowCustomTasks;
        public Boolean ExecuteFinishedTasks;
        public int LastTaskEventId;
        public double step;
        public Boolean isAcyclic;
        public double Acyclic_CycleTime;
        public double Acyclic_QuantityUsed;
        public double Acyclic_QuantityForEachProduct;
        public char ValueOrWaste;
        public Char Ergonomy;
        public String Operator;
        public Double RealLeadTime_Hour;
        public Double RealWorkingTime_Hour;
        public String Notes;
        public Double AdjustedTime; // Adjusted time considering the step: Acyclic_CycleTime * step / 60
    }
}