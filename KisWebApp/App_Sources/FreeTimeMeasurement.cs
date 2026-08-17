using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Dapper;
using KIS.App_Code;
using KIS.App_Sources;


namespace KIS.App_Sources
{
    public class FreeTimeMeasurement
    {
        public String Tenant;

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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements SET name=@name WHERE id=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { name = value, id = this.id }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements SET description=@name description id=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { description = value, id = this.id }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements SET status=@status WHERE id=@id";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { status = value.ToString(), id = this.id }, tr);
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

        public FreeTimeMeasurement(String Tenant, int id)
        {
            this._id = -1;
            this.Tenant = Tenant;
            this.Tasks = new List<FreeMeasurement_Task>();
            MySqlConnection conn = (new Dati.Dati()).mycon(Tenant);
            conn.Open();
            string sql = "SELECT "
                         + " freemeasurements.id,  "
                         + " freemeasurements.creationdate, "
                         + " freemeasurements.createdby, "
                         + " freemeasurements.plannedstartdate,  "
                         + " freemeasurements.plannedenddate,  "
                         + " freemeasurements.departmentid,   "
                         + " reparti.nome,  "
                         + " reparti.timezone,  "
                         + " freemeasurements.name,  "
                         + " freemeasurements.description,  "
                         + " freemeasurements.processid,   "
                         + " freemeasurements.processrev,  "
                         + " freemeasurements.variantid,  "
                         + " varproc.name,  "
                         + " varproc.description,  "
                         + " varproc.nomevariante,   "
                         + " freemeasurements.status,  "
                         + " freemeasurements.serialnumber,  "
                         + " freemeasurements.quantity,  "
                         + " freemeasurements.measurementunit,  "
                         + " measurementunits.type,   "
                         + " freemeasurements.realenddate,  "
                         + " freemeasurements.realworkingtime_hours, "
                         + " freemeasurements.realleadtime_hours,  "
                         + " freemeasurements.AllowCustomTasks,  "
                         + " freemeasurements.ExecuteFinishedTasks "
                         + "  FROM freemeasurements "
                         + "  LEFT JOIN "
                         + " (SELECT * FROM variantiprocessi "
                         + " INNER JOIN processo ON(variantiprocessi.processo = processo.processId and variantiprocessi.revProc = processo.revisione) "
                         + " INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante)) varproc "
                         + " ON(varproc.variante = freemeasurements.variantid "
                         + " AND varproc.processo = freemeasurements.processid "
                         + " AND varproc.revProc = freemeasurements.processrev) "
                         + " LEFT JOIN reparti ON(freemeasurements.departmentid = reparti.idreparto) "
                         + " INNER JOIN measurementunits ON(measurementunits.id = freemeasurements.measurementUnit) "
                         + " WHERE freemeasurements.id = @id";

            var rows = conn.Query<(int id, DateTime creationdate, String createdby, DateTime plannedstartdate, DateTime plannedenddate, int? departmentid, String nome, String timezone, String fname, String fdesc, int? processid, int? processrev, int? variantid, String vname, String vdesc, String nomevariante, String status, String serialnumber, double quantity, int measurementunit, String unittype, DateTime? realenddate, double? realworkingtime_hours, double? realleadtime_hours, Boolean AllowCustomTasks, Boolean ExecuteFinishedTasks)>(sql, new { id = id }).ToList();
            if (rows.Count > 0)
            {
                var row = rows[0];
                this._id = row.id;
                this._CreationDate = row.creationdate;
                this._CreatedBy = row.createdby;
                this._PlannedStartDate = row.plannedstartdate;
                this._PlannedEndDate = row.plannedenddate;
                this._DepartmentId = row.departmentid.HasValue ? row.departmentid.Value : -1;
                this._DepartmentName = row.nome != null ? row.nome : "";
                this._DepartmentTimeZone = row.timezone != null ? row.timezone : "";
                this._Name = row.fname != null ? row.fname : "";
                this._Description = row.fdesc != null ? row.fdesc : "";
                this._ProcessId = row.processid.HasValue ? row.processid.Value : -1;
                this._ProcessRev = row.processrev.HasValue ? row.processrev.Value : -1;
                this._VariantId = row.variantid.HasValue ? row.variantid.Value : -1;
                this._ProcessName = row.vname != null ? row.vname : "";
                this._ProcessDescription = row.vdesc != null ? row.vdesc : "";
                this._VariantName = row.nomevariante != null ? row.nomevariante : "";
                this._Status = row.status != null ? row.status[0] : '\0';
                this._SerialNumber = row.serialnumber != null ? row.serialnumber : "";
                this._Quantity = row.quantity;
                this._MeasurementUnitId = row.measurementunit;
                this._MeasurementUnitType = row.unittype;
                this._RealEndDate = row.realenddate.HasValue ? row.realenddate.Value : new DateTime(1970, 1, 1);
                this._RealWorkingTime_Hours = row.realworkingtime_hours.HasValue ? row.realworkingtime_hours.Value : 0;
                this._RealLeadTime_Hours = row.realleadtime_hours.HasValue ? row.realleadtime_hours.Value : 0;
                this._AllowCustomTasks = row.AllowCustomTasks;
                this._AllowExecuteFinishedTasks = row.ExecuteFinishedTasks;
            }
            conn.Close();
           
        }

        public void loadTasks()
        {
            this.Tasks = new List<FreeMeasurement_Task>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT freemeasurements_tasks.MeasurementId, freemeasurements_tasks.TaskId FROM freemeasurements_tasks "
                    + " INNER JOIN freemeasurements ON (freemeasurements.id = freemeasurements_tasks.MeasurementId)"
                    + " WHERE freemeasurements_tasks.MeasurementId=@measurementid"
                    + " ORDER BY sequence";
                var rows = conn.Query<(int MeasurementId, int TaskId)>(sql, new { measurementid = this.id });
                foreach (var r in rows)
                {
                    FreeMeasurement_Task curr = new FreeMeasurement_Task(this.Tenant, this.id, r.TaskId);
                    this.Tasks.Add(curr);
                }
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

                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    /*SELECT MAX(TaskId) FROM freemeasurements_tasks WHERE measurementid=@measurementid;*/
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, NoProductiveTaskId, name, "
                        + " description, sequence, workstationid, quantity_planned, status) "
                        + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @NoProductiveTaskId, @name, "
                        + " @description, @sequence, @workstationid, @quantity_planned, @status)";

                    try
                    {
                        conn.Execute(sql, new { measurementid = this.id, taskid = tID, OrigTaskId = (int?)null, OrigTaskRev = (int?)null, VariantId = (int?)null, NoProductiveTaskId = npTask.ID, name = npTask.Name, description = npTask.Description, sequence = seq, workstationid = (int?)null, quantity_planned = this.Quantity, status = 'N'.ToString() }, tr);
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

                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, NoProductiveTaskId, name, "
                        + " description, sequence, workstationid, quantity_planned, status) "
                        + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @NoProductiveTaskId, @name, "
                        + " @description, @sequence, @workstationid, @quantity_planned, @status)";

                    try
                    {
                        conn.Execute(sql, new { measurementid = this.id, taskid = tID, OrigTaskId = (int?)null, OrigTaskRev = (int?)null, VariantId = (int?)null, NoProductiveTaskId = (int?)null, name = TaskName, description = "", sequence = seq, workstationid = (int?)null, quantity_planned = this.Quantity, status = 'N'.ToString() }, tr);
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

                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, NoProductiveTaskId, name, "
                        + " description, sequence, workstationid, quantity_planned, status, quantity_produced, task_startdatereal, task_enddatereal, "
                        + " realleadtime_hours, realworkingtime_hours, step, isAcyclic, Acyclic_CycleTime, Acyclic_QtyUsed, Acyclic_QtyForEachProduct, ValueOrWaste, Ergonomy"
                        + ") "
                        + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @NoProductiveTaskId, @name, "
                        + " @description, @sequence, @workstationid, @quantity_planned, @status, @quantityproduced, @task_startdatereal, @task_enddatereal, "
                        + " @realleadtime_hours, @realworkingtime_hours, @step, @isAcyclic, @Acyclic_CycleTime, @Acyclic_QtyUsed, @Acyclic_QtyForEachProduct, @ValueOrWaste, @Ergonomy)";

                    try
                    {
                        conn.Execute(sql, new { measurementid = this.id, taskid = tID, OrigTaskId = (int?)null, OrigTaskRev = (int?)null, VariantId = (int?)null, NoProductiveTaskId = (int?)null, name = TaskName, description = "", sequence = seq, workstationid = (int?)null, quantity_planned = this.Quantity, status = 'F'.ToString(), quantityproduced = this.Quantity, task_startdatereal = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), task_enddatereal = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), realleadtime_hours = 0, realworkingtime_hours = workingtime, step = step, isAcyclic = isAcyclic, Acyclic_CycleTime = Acyclic_CycleTime, Acyclic_QtyUsed = Acyclic_QtyUsed, Acyclic_QtyForEachProduct = Acyclic_QtyForEachProduct, ValueOrWaste = ValueOrWaste.ToString(), Ergonomy = Ergonomy.ToString() }, tr);
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

                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        string sql = "UPDATE freemeasurements SET status='F', realenddate=@enddate, "
                            + " realworkingtime_hours=@workingtime, realleadtime_hours=@leadtime "
                            + " WHERE id=@measurementid";
                        conn.Execute(sql, new { enddate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), workingtime = workingtime, leadtime = leadtime, measurementid = this.id }, tr);

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

                    FreeTimeMeasurements fms = new FreeTimeMeasurements(this.Tenant);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                DateTime start = new DateTime(1970, 1, 1);
                DateTime end = new DateTime(1970, 1, 1);
                Boolean checkstart = false;
                Boolean checkend = false;
                // Gets the first event
                string sql = "SELECT id, freemeasurements_tasks_events.freemeasurementid, freemeasurements_tasks_events.taskid, inputpoint, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    + " WHERE freemeasurementid=@measurementid AND eventtype='I' "
                    + " AND freemeasurements_tasks.NoProductiveTaskId=-1 "
                    + " ORDER BY eventdate";
                var rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { measurementid = this.id }).ToList();
                if (rows.Count > 0)
                {
                    start = rows[0].eventdate;
                    checkstart = true;
                }

                sql = "SELECT id, freemeasurements_tasks_events.freemeasurementid, freemeasurements_tasks_events.taskid, inputpoint, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    +" WHERE freemeasurementid=@measurementid AND (eventtype='F' OR eventtype='P') "
                    + " AND freemeasurements_tasks.NoProductiveTaskId=-1 "
                    + " ORDER BY eventdate DESC";
                rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { measurementid = this.id }).ToList();
                if (rows.Count > 0)
                {
                    end = rows[0].eventdate;
                    checkend = true;
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                // Get all events of finished tasks there are not in the timespans table
                string sql = "SELECT freemeasurements_tasks_events.id, "
                                + " freemeasurements_tasks_events.freemeasurementid, "
                                + " freemeasurements_tasks_events.taskid, "
                                + " freemeasurements_tasks_events.inputpoint, "
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
                                + " freemeasurements_tasks_events.inputpoint, "
                                + " freemeasurements_tasks_events.eventdate";
                var rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { measurementid = this.id });
                foreach (var r in rows)
                {
                    FreeMeasurements_Tasks_Event currEv = new FreeMeasurements_Tasks_Event(this.Tenant);
                    currEv.id = r.id;
                    currEv.freemeasurementid = r.freemeasurementid;
                    currEv.taskid = r.taskid;
                    currEv.inputpoint = r.inputpoint;
                    currEv.eventtype = r.eventtype[0];
                    currEv.eventdate = r.eventdate;
                    currEv.notes = r.notes;
                    events.Add(currEv);
                }

                // Transform all events to timespans
                for (int i = 0; i < events.Count; i += 2)
                {
                    if (events[i].eventtype == 'I' && (events[i + 1].eventtype == 'F' || events[i + 1].eventtype == 'P')
                        && events[i].inputpoint == events[i + 1].inputpoint
                        && events[i].freemeasurementid == events[i + 1].freemeasurementid
                        && events[i].taskid == events[i + 1].taskid)
                    {
                        FreeMeasurements_Tasks_Events_Timespan currTs = new FreeMeasurements_Tasks_Events_Timespan();
                        currTs.freemeasurementid = events[i].freemeasurementid;
                        currTs.taskid = events[i].taskid;
                        currTs.inputpoint = events[i].inputpoint;
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
                    string sqlTs = "INSERT INTO freemeasurements_tasks_events_timespans (measurementid,taskid,inputpoint,starteventid,starteventtype,starteventdate,starteventnotes, "
                        + "endeventid, endeventtype, endeventdate, endeventnotes) "
                        + " VALUES (@measurementid, @taskid, @inputpoint, @starteventid, @starteventtype, @starteventdate, @starteventnotes, "
                        + " @endeventid, @endeventtype, @endeventdate, @endeventnotes)";
                    try
                    {
                        conn.Execute(sqlTs, new { measurementid = ts.freemeasurementid, taskid = ts.taskid, inputpoint = ts.inputpoint, starteventid = ts.starteventid, starteventtype = ts.starteventtype.ToString(), starteventdate = ts.starteventdate.ToString("yyyy-MM-dd HH:mm:ss"), starteventnotes = ts.starteventnotes, endeventid = ts.endeventid, endeventtype = ts.endeventtype.ToString(), endeventdate = ts.endeventdate.ToString("yyyy-MM-dd HH:mm:ss"), endeventnotes = ts.endeventnotes }, tr);
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

        protected String Tenant;

        public List<FreeTimeMeasurement> MeasurementsList;

        public FreeTimeMeasurements(String Tenant)
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
            this.Tenant = Tenant;
        }

        public void loadAllMeasurements()
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT id FROM freemeasurements ORDER BY plannedstartdate, plannedenddate");
            foreach (var id in rows)
            {
                this.MeasurementsList.Add(new FreeTimeMeasurement(this.Tenant, id));
            }
            conn.Close();
        }

        /* Loads a single status
         * if input = 'O' then loads all NOT finished Measurements
         * if input = 'A' then loads all
         */
        public void loadMeasurements(char Status)
        {
            this.MeasurementsList = new List<FreeTimeMeasurement>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            String strWhere = "";
            if(Status != 'A')
            { 
                if (Status == 'O')
                {
                    strWhere = " WHERE status <> 'F'";
                }
                else
                {
                    strWhere = " WHERE status = @status";
                }
            }
            string sql = "SELECT id FROM freemeasurements  " + strWhere + " ORDER BY plannedstartdate, plannedenddate";
            IEnumerable<int> rows;
            if (Status != 'A' && Status != 'O')
            {
                rows = conn.Query<int>(sql, new { status = Status.ToString() });
            }
            else
            {
                rows = conn.Query<int>(sql);
            }
            foreach (var measurementid in rows)
            {
                FreeTimeMeasurement curr = new FreeTimeMeasurement(this.Tenant, measurementid);
                this.MeasurementsList.Add(curr);
            }
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
                processo prc = new processo(this.Tenant, processid, processrev);
                variante vr = new variante(this.Tenant, variantid);
                ProcessoVariante prcVar = new ProcessoVariante(this.Tenant, prc, vr);
                if (prcVar != null && prcVar.process != null && prcVar.process.processID != -1
                    && prcVar.variant != null && prcVar.variant.idVariante != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "INSERT INTO freemeasurements(createdby, plannedstartdate, plannedenddate, departmentid, name, description,"
                        + " processid, processrev, variantid, status, serialnumber, quantity, measurementunit, "
                        + " AllowCustomTasks, ExecuteFinishedTasks) "
                        + " VALUES(@createdby, @plannedstartdate, @plannedenddate, @departmentid, @name, @description,"
                        + " @processid, @processrev, @variantid, @status, @serialnumber, @quantity, @measurementunit, "
                        + " @AllowCustomTasks, @ExecuteFinishedTasks)";

                    try
                    {
                        conn.Execute(sql, new { createdby = createdby, plannedstartdate = plannedstartdate, plannedenddate = plannedenddate, departmentid = DepartmentId, name = name, description = description, processid = processid, processrev = processrev, variantid = variantid, status = 'N'.ToString(), serialnumber = serialnumber, quantity = quantity, measurementunit = measurementUnitId, AllowCustomTasks = AllowCustomTasks, ExecuteFinishedTasks = AllowExecuteFinishedTasks }, tr);
                        int? lid = conn.QueryFirstOrDefault<int?>("SELECT LAST_INSERT_ID()", transaction: tr);
                        if (lid.HasValue)
                        {
                            ret = lid.Value;
                        }

                        // LoadSon
                        prc.loadFigli(vr);
                        for (int i = 0; i < prc.subProcessi.Count; i++)
                        {
                            TaskVariante tskvar = new TaskVariante(this.Tenant, prc.subProcessi[i], vr);
                            tskvar.loadPostazioni();
                            int wsId = -1;
                            if (tskvar.PostazioniDiLavoro.Count > 0)
                            {
                                wsId = tskvar.PostazioniDiLavoro[0].id;
                            }
                            string sqlTasks = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, name, "
                                + " description, sequence, workstationid, quantity_planned, status) "
                                + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @name, "
                                + " @description, @sequence, @workstationid, @quantity_planned, @status)";
                            if (wsId != -1)
                            {
                                conn.Execute(sqlTasks, new { measurementid = ret, taskid = i, OrigTaskId = prc.subProcessi[i].processID, OrigTaskRev = prc.subProcessi[i].revisione, VariantId = vr.idVariante, name = prc.subProcessi[i].processName, description = prc.subProcessi[i].processDescription, sequence = (i + 1), workstationid = wsId, quantity_planned = quantity, status = 'N'.ToString() }, tr);
                            }
                            else
                            {
                                conn.Execute(sqlTasks, new { measurementid = ret, taskid = i, OrigTaskId = prc.subProcessi[i].processID, OrigTaskRev = prc.subProcessi[i].revisione, VariantId = vr.idVariante, name = prc.subProcessi[i].processName, description = prc.subProcessi[i].processDescription, sequence = (i + 1), workstationid = (int?)null, quantity_planned = quantity, status = 'N'.ToString() }, tr);
                            }
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "INSERT INTO freemeasurements(createdby, plannedstartdate, plannedenddate, departmentid, name, description,"
                        + " processid, processrev, variantid, status, serialnumber, quantity, measurementunit, "
                        + " AllowCustomTasks, ExecuteFinishedTasks, MeasurementType, realenddate, realleadtime_hours, realworkingtime_hours) "
                        + " VALUES(@createdby, @plannedstartdate, @plannedenddate, @departmentid, @name, @description,"
                        + " @processid, @processrev, @variantid, @status, @serialnumber, @quantity, @measurementunit, "
                        + " @AllowCustomTasks, @ExecuteFinishedTasks, @MeasurementType, @realenddate, @realleadtime, @realworkingtime)";

                try
                    {
                        conn.Execute(sql, new { createdby = createdby, plannedstartdate = plannedstartdate, plannedenddate = plannedenddate, departmentid = (int?)null, name = name, description = description, processid = (int?)null, processrev = (int?)null, variantid = (int?)null, status = 'F'.ToString(), serialnumber = serialnumber, quantity = quantity, measurementunit = measurementUnitId, AllowCustomTasks = AllowCustomTasks, ExecuteFinishedTasks = AllowExecuteFinishedTasks, MeasurementType = "B", realenddate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), realleadtime = realleadtime, realworkingtime = realworkingtime }, tr);
                        int? lid = conn.QueryFirstOrDefault<int?>("SELECT LAST_INSERT_ID()", transaction: tr);
                        if (lid.HasValue)
                        {
                            ret = lid.Value;
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
            return ret;
        }

        public List<FreeMeasurentsTasksJsonStruct> GetFreeMeasurentsTasksJson(int departmentId)
        {
            List<FreeMeasurentsTasksJsonStruct> fmStruct = new List<FreeMeasurentsTasksJsonStruct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT "
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
            var rows = conn.Query<(int MeasurementId, DateTime Creationdate, String CreatedBy, DateTime PlannedStartDate, DateTime PlannedEndDate, int DepartmentId, String MeasurementName, String MeasurementDescription, int ProcessId, int ProcessRev, int VariantId, String Status, String SerialNumber, double Quantity, int MeasurementUnitId, String MeasurementUnitType, int TaskId, int? OrigTaskId, int? OrigTaskRev, int? TaskVariantId, int? NoProductiveTaskId, String TaskName, String TaskDescription, int Sequence, int? WorkstationId, String WorkstationName, double TaskQuantity, String TaskStatus, String ProcessName, String VariantName, Boolean AllowCustomTasks, Boolean ExecuteFinishedTasks)>(sql, new { departmentid = departmentId });
            foreach (var r in rows)
            {
                FreeMeasurentsTasksJsonStruct curr = new FreeMeasurentsTasksJsonStruct();
                curr.MeasurementId = r.MeasurementId;
                curr.Creationdate = r.Creationdate;
                curr.CreatedBy = r.CreatedBy;
                curr.PlannedStartDate = r.PlannedStartDate;
                curr.PlannedEndDate = r.PlannedEndDate;
                curr.DepartmentId = r.DepartmentId;
                curr.MeasurementName = r.MeasurementName;
                curr.MeasurementDescription = r.MeasurementDescription;
                curr.ProcessId = r.ProcessId;
                curr.ProcessRev = r.ProcessRev;
                curr.VariantId = r.VariantId;
                curr.Status = r.Status[0];
                curr.SerialNumber = r.SerialNumber;
                curr.Quantity = r.Quantity;
                curr.MeasurementUnitId = r.MeasurementUnitId;
                curr.MeasurementUnitType = r.MeasurementUnitType;
                curr.TaskId = r.TaskId;
                curr.OrigTaskId = r.OrigTaskId.HasValue ? r.OrigTaskId.Value : -1;
                curr.OrigTaskRev = r.OrigTaskRev.HasValue ? r.OrigTaskRev.Value : -1;
                curr.VariantId = r.TaskVariantId.HasValue ? r.TaskVariantId.Value : -1;
                curr.NoProductiveTaskId = r.NoProductiveTaskId.HasValue ? r.NoProductiveTaskId.Value : -1;
                curr.TaskName = r.TaskName;
                curr.TaskDescription = r.TaskDescription;
                curr.Sequence = r.Sequence;
                curr.WorkstationId = r.WorkstationId.HasValue ? r.WorkstationId.Value : -1;
                curr.WorkstationName = r.WorkstationName != null ? r.WorkstationName : "";
                curr.TaskQuantity = r.TaskQuantity;
                curr.TaskStatus = r.TaskStatus[0];
                curr.ProcessName = r.ProcessName;
                curr.VariantName = r.VariantName;
                curr.AllowCustomTasks = r.AllowCustomTasks;
                curr.ExecuteFinishedTasks = r.ExecuteFinishedTasks;
                fmStruct.Add(curr);
            }
            conn.Close();

            return fmStruct;
        }

        public List<FreeMeasurentsTasksJsonStruct> GetRunningTasks(Reparto dept, InputPoint ip)
        {
            List<FreeMeasurentsTasksJsonStruct> ret = new List<FreeMeasurentsTasksJsonStruct>();
            if(this.Tenant.Length > 0 && dept != null && dept.id>=0 && ip!=null && ip.id>=0)
            { 
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT "
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
                  + "                AND freemeasurements_tasks_events.inputpoint = @inputpoint "
                  + "                AND (freemeasurements.departmentid = @departmentid OR freemeasurements_tasks.NoProductiveTaskId IS NOT NULL) "
                  + "               ORDER BY freemeasurements_tasks_events.eventdate DESC) AS runningtasks "
                  + "               GROUP BY runningtasks.taskid) AS runningtasks2 "
                  + "  INNER JOIN freemeasurements_tasks_events AS freemeasurements_tasks_events2 ON(freemeasurements_tasks_events2.id = runningtasks2.runningtasksid) "
                  + "  INNER JOIN freemeasurements_tasks ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events2.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events2.taskid) "
                  + "  inner join freemeasurements ON(freemeasurements.id = freemeasurements_tasks.MeasurementId) "
                  + "  INNER JOIN measurementunits ON(measurementunits.id = freemeasurements.measurementUnit) "
                  + "  LEFT JOIN postazioni ON(freemeasurements_tasks.workstationid = postazioni.idpostazioni) "
                    + " WHERE eventtype = 'I'";

                var rows = conn.Query<(int MeasurementId, int TaskId, String TaskName, String WorkstationName, double TaskQuantity, String MeasurementUnitType, String MeasurementName, int LastTaskEventId, int? NoProductiveTaskId)>(sql, new { inputpoint = ip.id, departmentid = dept.id });
                foreach (var r in rows)
                {
                    FreeMeasurentsTasksJsonStruct curr = new FreeMeasurentsTasksJsonStruct();
                    curr.MeasurementId = r.MeasurementId;
                    curr.TaskId = r.TaskId;
                    curr.TaskName = r.TaskName;
                    curr.WorkstationName = r.WorkstationName != null ? r.WorkstationName : "";
                    curr.TaskQuantity = r.TaskQuantity;
                    curr.MeasurementUnitType = r.MeasurementUnitType;
                    curr.MeasurementName = r.MeasurementName;
                    curr.LastTaskEventId = r.LastTaskEventId;
                    curr.NoProductiveTaskId = r.NoProductiveTaskId.HasValue ? r.NoProductiveTaskId.Value : -1;
                    ret.Add(curr);
                }
                conn.Close();
            }
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();

            // Get all events of finished tasks there are not in the timespans table
            string sql = "SELECT freemeasurements_tasks_events.id, "
                            + " freemeasurements_tasks_events.freemeasurementid, "
                            + " freemeasurements_tasks_events.taskid, "
                            + " freemeasurements_tasks_events.inputpoint, "
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
                            + " freemeasurements_tasks_events.inputpoint, "
                            + " freemeasurements_tasks_events.taskid, "
                            + " freemeasurements_tasks_events.eventdate";
            var rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql);
            foreach (var r in rows)
            {
                FreeMeasurements_Tasks_Event currEv = new FreeMeasurements_Tasks_Event(this.Tenant);
                currEv.id = r.id;
                currEv.freemeasurementid = r.freemeasurementid;
                currEv.taskid = r.taskid;
                currEv.inputpoint = r.inputpoint;
                currEv.eventtype = r.eventtype[0];
                currEv.eventdate = r.eventdate;
                currEv.notes = r.notes;
                events.Add(currEv);
            }

            // Transform all events to timespans
            for(int i = 0; i < events.Count - 1; i+=2)
            {
                if(events[i].eventtype == 'I' && (events[i+1].eventtype == 'F' || events[i+1].eventtype == 'P')
                    && events[i].inputpoint == events[i + 1].inputpoint
                    && events[i].freemeasurementid == events[i+1].freemeasurementid
                    && events[i].taskid == events[i + 1].taskid)
                {
                    FreeMeasurements_Tasks_Events_Timespan currTs = new FreeMeasurements_Tasks_Events_Timespan();
                    currTs.freemeasurementid = events[i].freemeasurementid;
                    currTs.taskid = events[i].taskid;
                    currTs.inputpoint = events[i].inputpoint;
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
                string sqlTs = "INSERT INTO freemeasurements_tasks_events_timespans (measurementid,taskid,inputpoint,starteventid,starteventtype,starteventdate,starteventnotes, " 
                    + "endeventid, endeventtype, endeventdate, endeventnotes) "
                    + " VALUES (@measurementid, @taskid, @inputpoint, @starteventid, @starteventtype, @starteventdate, @starteventnotes, "
                    + " @endeventid, @endeventtype, @endeventdate, @endeventnotes)";
                try
                {
                    conn.Execute(sqlTs, new { measurementid = ts.freemeasurementid, taskid = ts.taskid, inputpoint = ts.inputpoint, starteventid = ts.starteventid, starteventtype = ts.starteventtype.ToString(), starteventdate = ts.starteventdate.ToString("yyyy-MM-dd HH:mm:ss"), starteventnotes = ts.starteventnotes, endeventid = ts.endeventid, endeventtype = ts.endeventtype.ToString(), endeventdate = ts.endeventdate.ToString("yyyy-MM-dd HH:mm:ss"), endeventnotes = ts.endeventnotes }, tr);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT freemeasurements.id FROM freemeasurements LEFT JOIN "
                + " (SELECT DISTINCT(measurementid) FROM freemeasurements_tasks WHERE status <> 'F' AND noproductivetaskid IS NULL) AS openmeasurements "
                + " ON(freemeasurements.id = openmeasurements.measurementid) WHERE status <> 'F' AND measurementid IS NULL");
            foreach (var id in rows)
            {
                this.MeasurementsList.Add(new FreeTimeMeasurement(this.Tenant, id));
            }
            conn.Close();
        }

    }

    public class FreeMeasurement_Task
    {
        protected String Tenant;

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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements_tasks SET quantity_produced=@quantity_produced WHERE measurementid=@measurementid AND taskid=@taskid";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { quantity_produced = value, measurementid = this.MeasurementId, taskid = this.TaskId }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements_tasks SET status=@status WHERE measurementid=@measurementid AND taskid=@taskid";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { status = value.ToString(), measurementid = this.MeasurementId, taskid = this.TaskId }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements_tasks SET task_startdatereal=@startdatereal WHERE measurementid=@measurementid AND taskid=@taskid";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { startdatereal = value.ToString("yyyy-MM-dd HH:mm:ss"), measurementid = this.MeasurementId, taskid = this.TaskId }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements_tasks SET task_enddatereal=@enddatereal WHERE measurementid=@measurementid AND taskid=@taskid";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { enddatereal = value.ToString("yyyy-MM-dd HH:mm:ss"), measurementid = this.MeasurementId, taskid = this.TaskId }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements_tasks SET RealLeadTime_Hours=@leadtime WHERE measurementid=@measurementid AND taskid=@taskid";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { leadtime = value, measurementid = this.MeasurementId, taskid = this.TaskId }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE freemeasurements_tasks SET RealWorkingTime_Hours=@workingtime WHERE measurementid=@measurementid AND taskid=@taskid";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { workingtime = value, measurementid = this.MeasurementId, taskid = this.TaskId }, tr);
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

        public FreeMeasurement_Task(String tenant, int measurementID, int taskID)
        {
            this.Tenant = tenant;
            this._MeasurementId = -1;
            this._TaskId = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT "
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
            var rows = conn.Query<(int MeasurementId, int TaskId, int? OrigTaskId, int? OrigTaskRev, int? VariantId, int? NoProductiveTaskId, String Name, String Description, int Sequence, int? WorkstationId, String WorkstationName, double PlannedQuantity, double? ProducedQuantity, String Status, DateTime? StartDateReal, DateTime? EndDateReal, double? RealLeadTime_Hours, double? RealWorkingTime_Hours, Boolean AllowCustomTasks, Boolean ExecuteFinishedTasks, int? DepartmentId, int? Step, Boolean? isAcyclic, double? Acyclic_CycleTime, double? Acyclic_QuantityUsed, double? Acyclic_QuantityForEachProduct, String ValueOrWaste, int? Ergonomy)>(sql, new { taskid = taskID, measurementid = measurementID }).ToList();
            if (rows.Count > 0)
            {
                var row = rows[0];
                this._MeasurementId = row.MeasurementId;
                this._TaskId = row.TaskId;
                this._OrigTaskId = row.OrigTaskId.HasValue ? row.OrigTaskId.Value : -1;
                this._OrigTaskRev = row.OrigTaskRev.HasValue ? row.OrigTaskRev.Value : -1;
                this._VariantId = row.VariantId.HasValue ? row.VariantId.Value : -1;
                this._NoProductiveTaskId = row.NoProductiveTaskId.HasValue ? row.NoProductiveTaskId.Value : -1;
                this._Name = row.Name;
                this._Description = row.Description;
                this._Sequence = row.Sequence;
                this._WorkstationId = row.WorkstationId.HasValue ? row.WorkstationId.Value : -1;
                this._WorkstationName = row.WorkstationName != null ? row.WorkstationName : "";
                this._PlannedQuantity = row.PlannedQuantity;
                this._ProducedQuantity = row.ProducedQuantity.HasValue ? row.ProducedQuantity.Value : 0;
                this._Status = row.Status[0];
                this._StartDateReal = row.StartDateReal.HasValue ? row.StartDateReal.Value : new DateTime(1970, 1, 1);
                this._EndDateReal = row.EndDateReal.HasValue ? row.EndDateReal.Value : new DateTime(1970, 1, 1);
                this._RealLeadTime_Hours = row.RealLeadTime_Hours.HasValue ? row.RealLeadTime_Hours.Value : 0;
                this._RealWorkingTime_Hours = row.RealWorkingTime_Hours.HasValue ? row.RealWorkingTime_Hours.Value : 0;
                this._AllowCustomTasks = row.AllowCustomTasks;
                this._AllowExecuteFinishedTasks = row.ExecuteFinishedTasks;
                this._DepartmentId = row.DepartmentId.HasValue ? row.DepartmentId.Value : -1;
                this._Step = row.Step.HasValue ? row.Step.Value : 60;                this._isAcyclic = row.isAcyclic.HasValue ? row.isAcyclic.Value : false;
                this._Acyclic_CycleTime = row.Acyclic_CycleTime.HasValue ? row.Acyclic_CycleTime.Value : 0;                this._Acyclic_QuantityUsed = row.Acyclic_QuantityUsed.HasValue ? row.Acyclic_QuantityUsed.Value : 0;
                this._Acyclic_QuantityForEachProduct = row.Acyclic_QuantityForEachProduct.HasValue ? row.Acyclic_QuantityForEachProduct.Value : 0;
                this._ValueOrWaste = row.ValueOrWaste != null ? row.ValueOrWaste[0] : '\0';
                this._Ergonomy = row.Ergonomy.HasValue ? row.Ergonomy.Value : -1;
            }
            conn.Close();
        }

        /* Returns:
         * 0 if generic error
         * 1 if task started successfully
         * 2 if task already started
         * 3 if operator not found
         * 4 if operator exceeds max number of running tasks
         * 6 if inputpoint is already running the task
         */
        public int Start(InputPoint ip)
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

                Reparto dept = new Reparto(this.Tenant, this.DepartmentId);
                int maxTasksInExecution = dept.TasksAvviabiliContemporaneamenteDaOperatore;
                FreeTimeMeasurement fmMeas = new FreeTimeMeasurement(this.Tenant, this.MeasurementId);
                FreeTimeMeasurement prevMeas = null;
                ip.loadFreeMeasurementRunningTasks(dept);
                int tasksInExecution = ip.FreeMeasurementTasks.Count;
                Boolean checkMaxTasks = false;
                Boolean PauseDefaultNoProductiveTask = false;
                int npTask = -1;
                int npTaskMeasurentId = -1;
                if ((maxTasksInExecution > 0 && tasksInExecution < maxTasksInExecution) || maxTasksInExecution == 0)
                { 
                    checkMaxTasks = true; 
                }

                if(ip.FreeMeasurementTasks.Count == 1 && ip.FreeMeasurementTasks[0].NoProductiveTaskId != -1 
                     && (ip.FreeMeasurementTasks[0].TaskId != this.TaskId || ip.FreeMeasurementTasks[0].MeasurementId != this.MeasurementId))
                {
                    checkMaxTasks = true;
                    PauseDefaultNoProductiveTask = true;
                    npTask = ip.FreeMeasurementTasks[0].TaskId;
                    npTaskMeasurentId = ip.FreeMeasurementTasks[0].MeasurementId;
                    prevMeas = new KIS.App_Sources.FreeTimeMeasurement(this.Tenant, ip.FreeMeasurementTasks[0].MeasurementId);
                }

                // Check that inputpoint is not already running this task
                Boolean checkAlreadyInExecution = false;
                try
                {
                    var found = ip.FreeMeasurementTasks.First(y => y.MeasurementId == this.MeasurementId && y.TaskId == this.TaskId); 
                    checkAlreadyInExecution = true;
                    ret = 6;
                }
                catch
                {
                    checkAlreadyInExecution = false;
                }

                if (checkMaxTasks && !checkAlreadyInExecution)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        if (PauseDefaultNoProductiveTask)
                        {
                            char npStatus = 'P';
                            if(prevMeas!= null && prevMeas.Status == 'F')
                            {
                                npStatus = 'F';
                            }
                            string sqlDef = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, inputpoint, eventtype, eventdate, notes) "
                                + " VALUES(@freemeasurementid, @taskid, @inputpoint, @eventtype, @eventdate, @notes) ";
                            conn.Execute(sqlDef, new { freemeasurementid = npTaskMeasurentId, taskid = npTask, inputpoint = ip.id.ToString(), eventtype = npStatus.ToString(), eventdate = eventtime.ToString("yyyy-MM-dd HH:mm:ss"), notes = "" }, tr);
                        }


                        string sql = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, inputpoint, eventtype, eventdate, notes) "
                                + " VALUES(@freemeasurementid, @taskid, @inputpoint, @eventtype, @eventdate, @notes) ";
                        conn.Execute(sql, new { freemeasurementid = this.MeasurementId, taskid = this.TaskId, inputpoint = ip.id.ToString(), eventtype = 'I'.ToString(), eventdate = eventtime.ToString("yyyy-MM-dd HH:mm:ss"), notes = "" }, tr);
                        tr.Commit();
                        this.Status = 'I';
                        fmMeas.Status = 'I';


                        // Eventually, change the status of the default no productive task
                        if (PauseDefaultNoProductiveTask)
                        {
                            FreeMeasurement_Task defTask = new FreeMeasurement_Task(this.Tenant, npTaskMeasurentId, npTask);
                            defTask.loadActiveInputPoints();
                            if (prevMeas.Status == 'F')
                            {
                                defTask.Status = 'F';
                                defTask.EndDateReal = eventtime;
                                Double pWt = defTask.calculateWorkingTime();
                                defTask.RealWorkingTime_Hours = pWt;
                                Double pLt = defTask.calculateLeadTime();
                                defTask.RealLeadTime_Hours = pLt;
                            }
                            else if(defTask.InputPoints.Count == 0)
                            {
                                FreeTimeMeasurement fm = new FreeTimeMeasurement(this.Tenant, defTask.MeasurementId);
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

                        FreeTimeMeasurements fms = new FreeTimeMeasurements(this.Tenant);
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
        public int Pause(InputPoint ip)
        {
            int ret = 0;
            ip.loadFreeMeasurementRunningTasks();
            bool running = false;
            try
            {
                ip.FreeMeasurementTasks.First(y => y.MeasurementId == this.MeasurementId && y.TaskId == this.TaskId);
                running = true;
            }
            catch
            {
                running = false;
            }

            if (this.Status == 'I' && ip.id!=-1 && running)
            {
                FreeTimeMeasurement fms = new FreeTimeMeasurement(this.Tenant, this.MeasurementId);
                char eventtype = 'P';
                eventtype = fms.Status == 'F' ? 'F' : 'P';

                DateTime eventtime = DateTime.UtcNow;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, inputpoint, eventtype, eventdate, notes) "
                   + " VALUES(@freemeasurementid, @taskid, @user, @eventtype, @eventdate, @notes) ";
                
                try
                { 
                    conn.Execute(sql, new { freemeasurementid = this.MeasurementId, taskid = this.TaskId, user = ip.id.ToString(), eventtype = eventtype.ToString(), eventdate = eventtime.ToString("yyyy-MM-dd HH:mm:ss"), notes = "" }, tr);
                    tr.Commit();
                    ret = 1;
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                }

                this.loadActiveInputPoints();
                if(this.InputPoints.Count == 0)
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

                ip.loadFreeMeasurementRunningTasks();
                if (ip.FreeMeasurementTasks.Count == 0)
                {
                    NoProductiveTasks npts = new NoProductiveTasks(this.Tenant);
                    var defTask = npts.TaskList.FirstOrDefault(x => x.IsDefault == true);
                    if(defTask!=null && defTask.ID != -1)
                    {
                        MySqlTransaction tr2 = conn.BeginTransaction();

                        // ADD NO PRODUCTIVE TASK IN TASK_LIST
                        int nptask = -1;
                        int npmeasurement = -1;
                        FreeTimeMeasurement fm = new FreeTimeMeasurement(this.Tenant, this.MeasurementId);
                        if(fm.Status!='F')
                        { 
                            nptask = fm.addTask(defTask);
                            npmeasurement = this.MeasurementId;
                        }
                        else
                        {
                            FreeTimeMeasurements fmss = new FreeTimeMeasurements(this.Tenant);
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
                                string sqlEv = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, inputpoint, eventtype, eventdate, notes) "
                                   + " VALUES(@freemeasurementid, @taskid, @inputpoint, @eventtype, @eventdate, @notes) ";
                                conn.Execute(sqlEv, new { freemeasurementid = npmeasurement, taskid = nptask, inputpoint = ip.id.ToString(), eventtype = 'I'.ToString(), eventdate = eventtime.ToString("yyyy-MM-dd HH:mm:ss"), notes = "" }, tr2);

                                string sqlUpd = "UPDATE freemeasurements_tasks SET status='I', task_startdatereal=@eventdate WHERE measurementid=@freemeasurementid AND TaskId=@taskid";
                                conn.Execute(sqlUpd, new { eventdate = eventtime.ToString("yyyy-MM-dd HH:mm:ss"), freemeasurementid = npmeasurement, taskid = nptask }, tr2);

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
          * 1 if Finished succesfully
          * 2 if task is not running or operator not found
          */
        public int Finish(InputPoint ip)
        {
            int ret = 0;
            if (this.Status == 'I' && ip.id >= 0 && this.NoProductiveTaskId == -1)
            {
                DateTime eventtime = DateTime.UtcNow;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    // Close task for all the users
                    this.loadActiveInputPoints();

                    foreach(var usr in this.InputPoints)
                    {
                        string sql = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, inputpoint, eventtype, eventdate, notes) "
                           + " VALUES(@freemeasurementid, @taskid, @inputpoint, @eventtype, @eventdate, @notes) ";
                        conn.Execute(sql, new { freemeasurementid = this.MeasurementId, taskid = this.TaskId, inputpoint = ip.id, eventtype = 'F'.ToString(), eventdate = eventtime.ToString("yyyy-MM-dd HH:mm:ss"), notes = "Task finished by " + ip.id.ToString() }, tr);
                    }
                    tr.Commit();

                    this.EndDateReal = eventtime;
                    this.Status = 'F';
                    ret = 1;
                }
                catch (Exception ex)
                {
                    this.log = ex.Message;
                    tr.Rollback();
                }

                Double leadtime = this.calculateLeadTime();
                Double workingtime = this.calculateWorkingTime();
                // Calculates leadtime and workingtime and writes in the database
                string sql2 = "UPDATE freemeasurements_tasks SET realleadtime_hours=@leadtime, realworkingtime_hours=@workingtime WHERE "
                   + " MeasurementId=@measurementid AND taskid=@taskid";
                tr = conn.BeginTransaction();
                try
                { 
                    conn.Execute(sql2, new { measurementid = this.MeasurementId, taskid = this.TaskId, leadtime = leadtime, workingtime = workingtime }, tr);
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    tr.Rollback();
                }

                NoProductiveTasks npts = new NoProductiveTasks(this.Tenant);
                var defTask = npts.TaskList.FirstOrDefault(x => x.IsDefault == true);
                if (defTask != null && defTask.ID != -1)
                {
                    // ADD NO PRODUCTIVE TASK IN TASK_LIST
                    FreeTimeMeasurement fm = new FreeTimeMeasurement(this.Tenant, this.MeasurementId);
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
                        FreeTimeMeasurements fmss = new FreeTimeMeasurements(this.Tenant);
                        fmss.loadMeasurements('I');
                        if (fmss.MeasurementsList.Count > 0)
                        {
                            nptask = fmss.MeasurementsList[0].addTask(defTask);
                            npmeasurement = fmss.MeasurementsList[0].id;
                        }
                    }
                    if (nptask != -1)
                    {
                        FreeMeasurement_Task npTsk = new FreeMeasurement_Task(this.Tenant, this.MeasurementId, nptask);

                        foreach (var usr in this.InputPoints)
                        {
                            ip.loadFreeMeasurementRunningTasks();
                            if (ip.FreeMeasurementTasks.Count == 0)
                            {
                                MySqlTransaction tr2 = conn.BeginTransaction();

                                try
                                {
                                    string sqlDef = "INSERT INTO freemeasurements_tasks_events(freemeasurementid, taskid, inputpoint, eventtype, eventdate, notes) "
                                       + " VALUES(@freemeasurementid, @taskid, @inputpoint, @eventtype, @eventdate, @notes) ";
                                    conn.Execute(sqlDef, new { freemeasurementid = npmeasurement, taskid = nptask, inputpoint = ip.id.ToString(), eventtype = 'I'.ToString(), eventdate = eventtime.ToString("yyyy-MM-dd HH:mm:ss"), notes = "" }, tr2);

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

        public List<int> InputPoints;
        public void loadActiveInputPoints()
        {
            this.InputPoints = new List<int>();
            if (this.MeasurementId !=-1 && this.TaskId != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT  "
                    + " freemeasurements.id,  "
                + " freemeasurements_tasks.taskid, "
                   + " freemeasurements_tasks.name AS TaskName, "
                    + " postazioni.name AS WorkstationName, "
                   + "  freemeasurements_tasks.quantity_planned, "
                   + "  measurementunits.type,"
                   + "  freemeasurements_tasks_events2.inputpoint "
                   + "      FROM"
                    + " (SELECT MAX(runningtasks.id) AS runningtasksid "
                    + "  FROM "
                + " (SELECT freemeasurements_tasks_events.id, freemeasurements_tasks_events.inputpoint, freemeasurements_tasks_events.eventtype, "
                + " freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, freemeasurements_tasks_events.eventdate "
                     + "        FROM freemeasurements_tasks "
                     + "         INNER JOIN freemeasurements_tasks_events "
                     + "        ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                     + "        INNER JOIN freemeasurements ON(freemeasurements.id = freemeasurements_tasks.measurementid) "
                     + "        WHERE freemeasurements_tasks.status = 'I' "
                    + " AND freemeasurements_tasks.taskid = @taskid "
                + " AND freemeasurements.id = @measurementid "
                + "             ORDER BY freemeasurements_tasks_events.eventdate DESC) AS runningtasks "
                + "             GROUP BY runningtasks.taskid, runningtasks.inputpoint) AS runningtasks2 "
               + " INNER JOIN freemeasurements_tasks_events AS freemeasurements_tasks_events2 ON(freemeasurements_tasks_events2.id = runningtasks2.runningtasksid)"
               + " INNER JOIN freemeasurements_tasks ON(freemeasurements_tasks.measurementid = freemeasurements_tasks_events2.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events2.taskid)"
               + " inner join freemeasurements ON(freemeasurements.id = freemeasurements_tasks.MeasurementId)"
               + " INNER JOIN measurementunits ON(measurementunits.id = freemeasurements.measurementUnit)"
               + " LEFT JOIN postazioni ON(postazioni.idpostazioni = freemeasurements_tasks.workstationid)"
                + " WHERE eventtype = 'I'; ";
                var rows = conn.Query<(int id, int taskid, String TaskName, String WorkstationName, double QuantityPlanned, String MeasurementUnitType, int InputPoint)>(sql, new { measurementid = this.MeasurementId, taskid = this.TaskId });
                foreach (var r in rows)
                {
                    this.InputPoints.Add(r.InputPoint);
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT id, freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, inputpoint, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    + " WHERE freemeasurements_tasks.measurementid=@measurementid AND freemeasurements_tasks.taskid=@taskid "
                   // + " AND freemeasurements_tasks.NoProductiveTaskId IS NULL "
                    + " ORDER BY inputpoint, eventdate";
                var rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { measurementid = this.MeasurementId, taskid = this.TaskId });
                foreach (var r in rows)
                {
                    FreeMeasurements_Tasks_Event curr = new FreeMeasurements_Tasks_Event(this.Tenant);
                    curr.id = r.id;
                    curr.freemeasurementid = r.freemeasurementid;
                    curr.taskid = r.taskid;
                    curr.inputpoint = r.inputpoint;
                    curr.eventtype = r.eventtype[0];
                    curr.eventdate = r.eventdate;
                    curr.notes = r.notes;
                    eventsLst.Add(curr);
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                DateTime start = new DateTime(1970, 1, 1);
                DateTime end = new DateTime(1970, 1, 1);
                Boolean checkstart = false;
                Boolean checkend = false;
                // Gets the first event
                string sql = "SELECT id, freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, inputpoint, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) "
                    + " WHERE freemeasurements_tasks.measurementid=@measurementid AND freemeasurements_tasks.taskid=@taskid AND eventtype='I' "
                   // + " AND freemeasurements_tasks.NoProductiveTaskId IS NULL "
                    + " ORDER BY eventdate";
                var rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { measurementid = this.MeasurementId, taskid = this.TaskId }).ToList();
                if (rows.Count > 0)
                {
                    start = rows[0].eventdate;
                    checkstart = true;
                }

                sql = "SELECT id, freemeasurements_tasks.measurementid, freemeasurements_tasks.taskid, inputpoint, eventtype, eventdate, notes FROM "
                    + " freemeasurements_tasks_events INNER JOIN freemeasurements_tasks ON "
                    + "(freemeasurements_tasks.measurementid = freemeasurements_tasks_events.freemeasurementid AND freemeasurements_tasks.taskid = freemeasurements_tasks_events.taskid) " 
                    + " WHERE freemeasurements_tasks.measurementid=@measurementid AND freemeasurements_tasks.taskid=@taskid AND (eventtype='F' OR eventtype='P') "
                    // + " AND freemeasurements_tasks.NoProductiveTaskId IS NULL "
                    + " ORDER BY eventdate DESC";
                rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { measurementid = this.MeasurementId, taskid = this.TaskId }).ToList();
                if (rows.Count > 0)
                {
                    end = rows[0].eventdate;
                    checkend = true;
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT id, inputpoint, eventtype, eventdate, notes FROM freemeasurements_tasks_events WHERE freemeasurementid=@freemeasurementid AND taskid=@taskid "
                    + " ORDER BY eventdate";
                var rows = conn.Query<(int id, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { freemeasurementid = this.MeasurementId, taskid = this.TaskId });
                foreach (var r in rows)
                {
                    FreeMeasurements_Tasks_Event fmev = new FreeMeasurements_Tasks_Event(this.Tenant);
                    fmev.freemeasurementid = this.MeasurementId;
                    fmev.taskid = this.TaskId;
                    fmev.id = r.id;
                    fmev.inputpoint = r.inputpoint;
                    fmev.eventtype = r.eventtype[0];
                    fmev.eventdate = r.eventdate;
                    fmev.notes = r.notes;
                    this.TaskEvents.Add(fmev);
                }
                conn.Close();
            }
        }
    }

    public class FreeMeasurements_Tasks_Event
    {
        public String log;

        protected String Tenant;

        public int id;
        public int freemeasurementid;
        public int taskid;
        public int inputpoint;
        public Char eventtype;
        public DateTime eventdate;
        public String notes;

        public FreeMeasurements_Tasks_Event(String tenant)
        {
            this.id = -1;
            this.freemeasurementid = -1;
            this.taskid = -1;
            this.Tenant = tenant;
        }

        public FreeMeasurements_Tasks_Event(String tenant, int eventid)
        {
            this.Tenant = tenant;
            this.id = -1;
            this.freemeasurementid = -1;
            this.taskid = -1;
            if (eventid !=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT id, freemeasurementid, taskid, inputpoint, eventtype, eventdate, notes FROM freemeasurements_tasks_events WHERE id=@evid";
                var rows = conn.Query<(int id, int freemeasurementid, int taskid, int inputpoint, String eventtype, DateTime eventdate, String notes)>(sql, new { evid = eventid }).ToList();
                if (rows.Count > 0)
                {
                    var row = rows[0];
                    this.id = row.id;
                    this.freemeasurementid = row.freemeasurementid;
                    this.taskid = row.taskid;
                    this.inputpoint = row.inputpoint;
                    this.eventtype = row.eventtype[0];
                    this.eventdate = row.eventdate;
                    this.notes = row.notes;
                }
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "UPDATE freemeasurements_tasks_events SET notes=@note WHERE id=@id";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { note = note, id = id }, tr);
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
        public int inputpoint;
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
        public int step;
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