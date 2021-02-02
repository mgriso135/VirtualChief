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
        public int id {  get { return this._id; } }
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
        public String MeasurementUnitType {  get { return this._MeasurementUnitType; } }

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

        public FreeTimeMeasurement(int id)
        {
            this._id = -1;

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
            if(rdr.Read())
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
                this._RealEndDate = rdr.GetDateTime(21);
                this._RealWorkingTime_Hours = rdr.GetDouble(22);
                this._RealLeadTime_Hours = rdr.GetDouble(23);
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
            while(rdr.Read())
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
            if(Status == 'O')
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
         */
        public int Add(String createdby, DateTime plannedstartdate, DateTime plannedenddate, int DepartmentId, String name, String description, int processid, int processrev, int variantid,
            Char Status, String serialnumber, Double quantity, int measurementUnitId = 0, Boolean AllowCustomTasks = true, Boolean AllowExecuteFinishedTasks = true)
        {
            int ret = -1;
            if(name.Length < 255)
            {
                processo prc = new processo(processid, processrev);
                variante vr = new variante(variantid);
                ProcessoVariante prcVar = new ProcessoVariante(prc, vr);
                if(prcVar!=null && prcVar.process!=null && prcVar.process.processID != -1 
                    && prcVar.variant!=null && prcVar.variant.idVariante!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO freemeasurement(createdby, plannedstartdate, plannedenddate, departmentid, name, description,"
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
                    cmd.Parameters.AddWithValue("@status", Status);
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
                        if(rdr.Read())
                        {
                            ret = rdr.GetInt32(0);
                        }
                        rdr.Close();

                        // LoadSon
                        prc.loadFigli(vr);
                        for(int i = 0; i < prc.subProcessi.Count; i++)
                        {
                            TaskVariante tskvar = new TaskVariante(prc.subProcessi[i], vr);
                            tskvar.loadPostazioni();
                            int wsId = -1;
                            if(tskvar.PostazioniDiLavoro.Count > 0)
                            {
                                wsId = tskvar.PostazioniDiLavoro[0].id;
                            }
                            MySqlCommand cmdTasks = conn.CreateCommand();
                            cmdTasks.CommandText = "INSERT INTO freemeasurements_tasks(MeasurementId, TaskId, OrigTaskId, OrigTaskRev, VariantId, name, "
                                + " description, sequence, workstationid, quantity_planned, status) "
                                + " VALUES (@measurementid, @taskid, @OrigTaskId, @OrigTaskRev, @VariantId, @name, "
                                + " @description, @sequence, @workstationid, @quantity_planned, @status)";
                            cmd.Parameters.AddWithValue("@measurementid", ret);
                            cmd.Parameters.AddWithValue("@taskid", i);
                            cmd.Parameters.AddWithValue("@OrigTaskId", prc.subProcessi[i].processID);
                            cmd.Parameters.AddWithValue("@OrigTaskRev", prc.subProcessi[i].revisione);
                            cmd.Parameters.AddWithValue("@VariantId", vr.idVariante);
                            cmd.Parameters.AddWithValue("@name", prc.subProcessi[i].processName);
                            cmd.Parameters.AddWithValue("@description", prc.subProcessi[i].processDescription);
                            cmd.Parameters.AddWithValue("@sequence", (i+1));
                            if(wsId != -1)
                            { 
                                cmd.Parameters.AddWithValue("@workstationid", wsId);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@workstationid", null);
                            }
                            cmd.Parameters.AddWithValue("@quantity_planned", ret);
                            cmd.Parameters.AddWithValue("@status", ret);
                            cmd.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        this.log = ex.Message;
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
}