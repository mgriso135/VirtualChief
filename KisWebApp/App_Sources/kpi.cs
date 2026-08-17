/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */

using System;
using MySql.Data.MySqlClient;
using Dapper;

namespace KIS.App_Code
{
    public class Kpi
    {
        protected String Tenant;

        private int _id;
        public int id 
        {
            get { return _id; }
        }

        private String _name;
        public String name
        {
            get { return _name; }
            set
            {
                // Code to change name of kpi
                String strSQL = "UPDATE kpi_description SET name = @pName WHERE id = @pId";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { pName = value, pId = this.id });
                conn.Close();
            }
        }

        private String _description;
        public String description
        {
            get { return _description; }
            set
            {
                // Code to change description of kpi
                String strSQL = "UPDATE kpi_description SET description = @pDescription WHERE id = @pId";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { pDescription = value, pId = this.id });
                conn.Close();
            }
        }

        private int _procID;
        public int procID
        {
            get { return _procID; }
        }

        private int _revProc;
        public int revProc
        {
            get { return this._revProc; }
        }

        private int _numData;
        public int numData
        {
            get { return _numData; }
        }

        private float _baseVal;
        public float baseVal
        {
            get { return this._baseVal; }
            set
            {
                if(this._id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    String strSQL = "UPDATE kpi_description SET baseval = @pBaseVal WHERE id = @pId";
                    conn.Execute(strSQL, new { pBaseVal = value, pId = this.id });
                    conn.Close();
                    _baseVal = value;
                }
            }
        }

        private KpiRecord[] _data;
        public KpiRecord[] data
        {
            get { return _data; }
        }

        private class KpiRow
        {
            public int id { get; set; }
            public String name { get; set; }
            public String description { get; set; }
            public int idprocesso { get; set; }
            public int revisione { get; set; }
            public int attivo { get; set; }
            public float baseVal { get; set; }
        }

        private class KpiRecordRow
        {
            public int kpiID { get; set; }
            public DateTime data { get; set; }
            public float valore { get; set; }
            public int task { get; set; }
        }

        public Kpi(String Tenant)
        {
            this.Tenant = Tenant;

            this._id = -1;
            this._name = "";
            this._description = "";
        }

        public Kpi(String Tenant, int kpiID)
        {
            this.Tenant = Tenant;

            string strSQL = "SELECT id, name, description, idprocesso, revisione, attivo, baseVal FROM kpi_description WHERE id = @pId";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<KpiRow>(strSQL, new { pId = kpiID });
            if (row != null)
            {
                this._id = row.id;
                this._name = row.name;
                this._description = row.description;
                this._procID = row.idprocesso;
                this._revProc = row.revisione;
                this._baseVal = row.baseVal;
                //loadRecords();
            }
            else
            {
                this._id = -1;
                this._name = "";
                this._description = "";
            }
            conn.Close();
        }

        public bool loadRecords()
        {
            bool rt;
            this._numData = 0;
            if(this._id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                String strSQL = "SELECT COUNT(kpiID) FROM kpi_record WHERE kpiID = @pId";
                this._numData = conn.QueryFirstOrDefault<int>(strSQL, new { pId = this.id });

                if (this._numData > 0)
                {
                    this._data = new KpiRecord[this.numData];
                    strSQL = "SELECT * FROM kpi_record WHERE kpiID = @pId ORDER BY data";
                    var rows = conn.Query<KpiRecordRow>(strSQL, new { pId = this.id });
                    int i = 0;
                    foreach (var r in rows)
                    {
                        if (i >= this._numData) break;
                        this._data[i] = new KpiRecord(this.Tenant);
                        this._data[i].valore = r.valore;
                        this._data[i].date = r.data;
                        this._data[i].Task = r.task;
                        i++;
                    }
                }
                else
                {
                    this._data = null;
                }
                conn.Close();
                rt = true;
            }
            else
            {
                this._numData = 0;
                this._data = null;
                rt = false;
            }
            return rt;
        }

        public double mean
        {
            get
            {
                if (this.numData > 0)
                {
                    double sum = 0;
                    for (int i = 0; i < this._numData; i++)
                    {
                        sum += this.data[i].valore;
                    }
                    return (sum / this._numData);
                }
                else
                {
                    return 0.0;
                }
            }
        }

        public double devStd
        {
            get
            {
                double squareSums = 0.0;
                for (int i = 0; i < this.numData; i++)
                {
                    squareSums += Math.Pow((this.data[i].valore - this.mean), 2);
                }
                return Math.Sqrt(squareSums / this.numData);
            }
        }

        public double UCL
        {
            get
            {
                return (this.mean + 3 * this.devStd);
            }
        }

        public double LCL
        {
            get
            {
                return (this.mean - 3 * this.devStd);
            }
        }

        public bool add(String name, String Description, processo proc, float baseVal)
        {
            if (name.Length > 0 && Description.Length > 0 && proc != null && proc.processID != -1)
            {
                string strSQL = "SELECT MAX(id) FROM kpi_description";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? max = conn.QueryFirstOrDefault<int?>(strSQL);
                if (max.HasValue)
                {
                    try
                    {
                        this._id = max.Value + 1;
                    }
                    catch
                    {
                        this._id = 0;
                    }
                }
                else
                {
                    this._id = 0;
                }
                strSQL = "INSERT INTO kpi_description(id, name, description, idprocesso, revisione, attivo, baseval) VALUES("
                    + "@pId, @pName, @pDescription, @pProcID, @pRevProc, 1, @pBaseVal)";
                conn.Execute(strSQL, new { pId = this._id, pName = name, pDescription = Description, pProcID = proc.processID, pRevProc = proc.revisione, pBaseVal = baseVal });
                conn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool add(String name, String Description, int procID, int revProc, float baseVal)
        {
            if (name.Length > 0 && Description.Length > 0 && procID != -1 && revProc != -1)
            {
                string strSQL = "SELECT MAX(id) FROM kpi_description";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? max = conn.QueryFirstOrDefault<int?>(strSQL);
                if (max.HasValue)
                {
                    try
                    {
                        this._id = max.Value + 1;
                    }
                    catch
                    {
                        this._id = 0;
                    }
                }
                else
                {
                    this._id = 0;
                }
                strSQL = "INSERT INTO kpi_description(id, name, description, idprocesso, revisione, attivo, baseval) VALUES("
                    + "@pId, @pName, @pDescription, @pProcID, @pRevProc, 1, @pBaseVal)";
                conn.Execute(strSQL, new { pId = this._id, pName = name, pDescription = Description, pProcID = procID, pRevProc = revProc, pBaseVal = baseVal });
                conn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool resumeTrashedKPI()
        {
            if (this.id != -1)
            {
                String strSQL = "UPDATE kpi_description SET attivo=1 WHERE id=@pId";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { pId = this.id });
                conn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool moveToTrash()
        {
            if (this.id != -1)
            {
                String strSQL = "UPDATE kpi_description SET attivo=0 WHERE id=@pId";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { pId = this.id });
                conn.Close();
                return true;
            }
            else
            {
                return false;
            }                
        }

        public bool delete()
        {
            bool ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            String strSQL = "DELETE FROM kpi_record WHERE kpiID = @pId";
            String strSQL2 = "DELETE FROM kpi_description WHERE id = @pId";
            try
            {
                conn.Execute(strSQL, new { pId = this.id });
                conn.Execute(strSQL2, new { pId = this.id });
                ret = true;
            }
            catch
            {
                ret = false;
            }
            conn.Close();
            return ret;
        }

        public bool recordValueNow(double valore, int task)
        { 
            bool rt = false;
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string val = valore.ToString().Replace(",", ".");
                String strSQL = "INSERT INTO kpi_record(kpiID, data, valore, task) VALUES(@pId, @pData, @pValore, @pTask)";
                conn.Execute(strSQL, new { pId = this.id, pData = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), pValore = val, pTask = task });
                conn.Close();
                rt = true;
            }
            return rt;
        }

        public bool loadLimitedRecords(DateTime start, DateTime end)
        {
            bool rt;
            this._numData = 0;
            this._data = null;

            if (this._id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                String strSQL = "SELECT COUNT(kpiID) FROM kpi_record WHERE kpiID = @pId AND data >= @pStart AND data <= @pEnd";
                this._numData = conn.QueryFirstOrDefault<int>(strSQL, new { pId = this.id, pStart = start.ToString("yyyy-MM-dd"), pEnd = end.ToString("yyyy-MM-dd") });

                if (this._numData > 0)
                {
                    this._data = new KpiRecord[this.numData];
                    strSQL = "SELECT * FROM kpi_record WHERE kpiID = @pId AND data >= @pStart AND data <= @pEnd ORDER BY data";
                    var rows = conn.Query<KpiRecordRow>(strSQL, new { pId = this.id, pStart = start.ToString("yyyy-MM-dd"), pEnd = end.ToString("yyyy-MM-dd") });
                    int i = 0;
                    foreach (var r in rows)
                    {
                        if (i >= this._numData) break;
                        this._data[i] = new KpiRecord(this.Tenant);
                        this._data[i].valore = r.valore;
                        this._data[i].date = r.data;
                        this._data[i].Task = r.kpiID;
                        i++;
                    }
                }
                else
                {
                    this._data = null;
                }
                conn.Close();
                rt = true;
            }
            else
            {
                this._numData = 0;
                this._data = null;
                rt = false;
            }
            return rt;
        }
    }

    public class KpiRecord
    {
        protected String Tenant;

        private float _valore;
        public float valore
        {
            get { return this._valore; }
            set { this._valore = value; }
        }

        private DateTime _date;
        public DateTime date
        {
            get {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._date, fuso.tzFusoOrario);
            }
            set { FusoOrario fuso = new FusoOrario(this.Tenant);
                this._date = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario); }
        }

        private int _task;
        public int Task
        {
            get { return _task; }
            set { _task = value; }
        }

        public KpiRecord(String Tenant)
        {
            this.Tenant = Tenant;

            this._valore = 0;
            this._date = DateTime.UtcNow;
        }

    }
}