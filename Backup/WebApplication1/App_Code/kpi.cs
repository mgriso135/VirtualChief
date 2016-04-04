using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dati;
using MySql.Data;
using MySql.Data.MySqlClient;
using WebApplication1;

namespace WebApplication1
{
    public class Kpi
    {
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
                String strSQL = "UPDATE kpi_description SET name = '" + value + "' WHERE id = " + this.id.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
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
                String strSQL = "UPDATE kpi_description SET description = '" + value + "' WHERE id = " + this.id.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private int _procID;
        public int procID
        {
            get { return _procID; }
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
                    MySqlConnection conn = Dati.Dati.mycon();
                    conn.Open();
                    String strSQL = "UPDATE kpi_description SET baseval = " + value.ToString() + " WHERE id = " + this.id;
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    _baseVal = value;
                }
            }
        }

        private KpiRecord[] _data;
        public KpiRecord[] data
        {
            get { return _data; }
        }

        public Kpi()
        {
            this._id = -1;
            this._name = "";
            this._description = "";
        }

        public Kpi(int kpiID)
        {
            string strSQL = "SELECT * FROM kpi_description WHERE id = " + kpiID.ToString();
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr1 = cmd1.ExecuteReader();
            if (rdr1.Read())
            {
                this._id = rdr1.GetInt32(0);
                this._name = rdr1.GetString(1);
                this._description = rdr1.GetString(2);
                this._procID = rdr1.GetInt32(3);
                this._baseVal = rdr1.GetFloat(5);
                //loadRecords();
            }
            else
            {
                this._id = -1;
                this._name = "";
                this._description = "";
            }
            rdr1.Close();
            conn.Close();
        }

        public bool loadRecords()
        {
            bool rt;
            this._numData = 0;
            if(this._id != -1)
            {
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                String strSQL = "SELECT COUNT(kpiID) FROM kpi_record WHERE kpiID = " + this.id;
                MySqlCommand cmdCount = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdCount = cmdCount.ExecuteReader();
                rdCount.Read();
                this._numData = rdCount.GetInt32(0);
                rdCount.Close();

                if (this._numData > 0)
                {
                    this._data = new KpiRecord[this.numData];
                    strSQL = "SELECT * FROM kpi_record WHERE kpiID = " + this.id + " ORDER BY data";
                    MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                    MySqlDataReader rdr = cmd1.ExecuteReader();
                    int i = 0;
                    while (rdr.Read() && i < this._numData)
                    {
                        this._data[i] = new KpiRecord();
                        this._data[i].valore = rdr.GetFloat(2);
                        this._data[i].date = rdr.GetDateTime(1);
                        i++;
                    }
                    rdr.Close();
                    conn.Close();
                }
                else
                {
                    this._data = null;
                }
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
                double sum = 0;
                for (int i = 0; i < this._numData; i++)
                {
                    sum += this.data[i].valore;
                }
                return (sum / this._numData);
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
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    try
                    {
                        this._id = rdr1.GetInt32(0) + 1;
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
                rdr1.Close();
                strSQL = "INSERT INTO kpi_description(id, name, description, idprocesso, attivo, baseval) VALUES(" + this._id.ToString() + ", '" + name + "', '" + Description + "', " + proc.processID + ", 1, " + baseVal.ToString() + ")";
                MySqlCommand cmd2 = new MySqlCommand(strSQL, conn);
                cmd2.ExecuteNonQuery();
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
                String strSQL = "UPDATE kpi_description SET attivo=1 WHERE id=" + this.id.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
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
                String strSQL = "UPDATE kpi_description SET attivo=0 WHERE id=" + this.id.ToString();
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
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
            MySqlConnection conn = Dati.Dati.mycon();
            conn.Open();
            String strSQL = "DELETE FROM kpi_record WHERE kpiID = " + this.id.ToString();
            MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
            String strSQL2 = "DELETE FROM kpi_description WHERE id = " + this.id.ToString();
            MySqlCommand cmd = new MySqlCommand(strSQL2, conn);
            try
            {
                cmd1.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                ret = true;
            }
            catch
            {
                ret = false;
            }
            conn.Close();
            return ret;
        }

        public bool recordValueNow(double valore)
        { 
            bool rt = false;
            if (this.id != -1)
            {
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                string val = valore.ToString().Replace(",", ".");
                String strSQL = "INSERT INTO kpi_record(kpiID, data, valore) VALUES(" + this.id + ", '" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "', " + val +")";
                MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                cmd1.ExecuteNonQuery();
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
                MySqlConnection conn = Dati.Dati.mycon();
                conn.Open();
                String strSQL = "SELECT COUNT(kpiID) FROM kpi_record WHERE kpiID = " + this.id + " AND data >= '" + start.ToString("yyyy-MM-dd") + "' AND data <= '" + end.ToString("yyyy-MM-dd") + "'";
                MySqlCommand cmdCount = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdCount = cmdCount.ExecuteReader();
                rdCount.Read();
                this._numData = rdCount.GetInt32(0);
                rdCount.Close();

                if (this._numData > 0)
                {
                    this._data = new KpiRecord[this.numData];
                    strSQL = "SELECT * FROM kpi_record WHERE kpiID = " + this.id + " AND data >= '" + start.ToString("yyyy-MM-dd") + "' AND data <= '" + end.ToString("yyyy-MM-dd") + "' ORDER BY data";
                    MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                    MySqlDataReader rdr = cmd1.ExecuteReader();
                    int i = 0;
                    while (rdr.Read() && i < this._numData)
                    {
                        this._data[i] = new KpiRecord();
                        this._data[i].valore = rdr.GetFloat(2);
                        this._data[i].date = rdr.GetDateTime(1);
                        i++;
                    }
                    rdr.Close();
                    conn.Close();
                }
                else
                {
                    this._data = null;
                }
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

        private float _valore;
        public float valore
        {
            get { return this._valore; }
            set { this._valore = value; }
        }

        private DateTime _date;
        public DateTime date
        {
            get { return this._date; }
            set { this._date = value; }
        }

        public KpiRecord()
        {
            this._valore = 0;
            this._date = System.DateTime.Now;
        }

    }
}