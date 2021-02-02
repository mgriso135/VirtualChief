/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

/*
 * CHANGELOG
 * 
 * 20171202 Matteo Griso
 * * Added class ProductParametersCategory
 * * Added class ProductParametersCategories
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace KIS.App_Code
{
    public class macroProcessi
    {
        public List<processo> Elenco;
        
        public macroProcessi()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processo.processID, processo.revisione FROM processo LEFT JOIN processipadrifigli "
                + " ON (processo.processID = processipadrifigli.task AND processo.revisione = processipadrifigli.revTask) "
                + " WHERE processipadrifigli.padre IS NULL AND processo.attivo = 1 ORDER BY processo.name";

            MySqlDataReader mysqlReader = cmd.ExecuteReader();
            this.Elenco = new List<processo>();
            while (mysqlReader.Read())
            {
                this.Elenco.Add(new processo(mysqlReader.GetInt32(0), mysqlReader.GetInt32(1)));
            }
            mysqlReader.Close();
            conn.Close();            
        }
        
        public bool Add(String nome, String Descr, bool isVSM)
        {
            bool res = false;
            if(nome.Length > 0 && Descr.Length > 0)
            {                
                string strSQL = "SELECT MAX(processID) from processo";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd1.ExecuteReader();
                int maxCod;
                rdr.Read();
                if (!rdr.IsDBNull(0))
                {
                    maxCod = rdr.GetInt32(0) + 1;
                }
                else
                {
                    maxCod = 0;
                }
                strSQL = "INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, isVSM, posx, posy, attivo) " +
                    "VALUES(" + maxCod + ", 0, '" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', '" + nome + "', '" + Descr + 
                    "', " + isVSM.ToString() + ", 0, 0, 1)";
                MySqlCommand cmd2 = new MySqlCommand(strSQL, conn);
                rdr.Close();
                cmd2.ExecuteNonQuery();
                res = true;
                conn.Close();
            }
            else
            {
                res = false;
            }
            return res;
        }

        public int deleteMacroProcess(int macroProcID)
        {
            int res = 0;
            int found = -1;
            // Check if it is a macroprocess
            for (int i = 0; i < Elenco.Count; i++)
            {
                if (Elenco[i].processID == macroProcID)
                {
                    found = i;
                }
            }
            if (found != -1)
            {
                // Check if it has subprocesses
                if (Elenco[found].subProcessi.Count == 0)
                {
                    string strSQL = "DELETE FROM processo WHERE processID = " + macroProcID.ToString();
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    res = 1;
                }
                else
                {
                    res = 2;
                }
            }
            return res;
        }

        public List<int[]> FindByName(String nome)
        {
            List<int[]> elencoFindings = new List<int[]>();
            for (int i = 0; i < this.Elenco.Count; i++)
            {
                if (this.Elenco[i].processName == nome)
                {
                    int[] procFound = new int[2];
                    procFound[0] = this.Elenco[i].processID;
                    procFound[1] = this.Elenco[i].revisione;
                    elencoFindings.Add(procFound);
                }
            }
            return elencoFindings;
        }
    }

    public class processo
    {
        public String log;

        private int _processID;
        private int _revisione;
        public int revisione
        {
            get { return this._revisione; }
        }
        private DateTime _dataRevisione;
        public DateTime dataRevisione
        {
            get {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._dataRevisione, fuso.tzFusoOrario); }
        }

        private List<variante> _variantiProcesso;
        public List<variante> variantiProcesso
        {
            get { return this._variantiProcesso; }
        }

        private List<variante> _variantiFigli;
        public List<variante> variantiFigli
        {
            get { return this._variantiFigli; }
        }
        
        private String _processName;
        private String _processDescription;

        private List<int> _processiSucc;
        public List<int> processiSucc
        {
            get { return _processiSucc; }
        }

        private List<int> _revisioneSucc;
        public List<int> revisioneSucc
        {
            get { return _revisioneSucc; }
        }

        private List<relazione> _relazioneSucc;
        public List<relazione> relazioneSucc
        {
            get { return this._relazioneSucc; }
        }

        private List<int> _processiPrec;
        public List<int> processiPrec
        {
            get { return _processiPrec; }
        }

        private List<int> _revisionePrec;
        public List<int> revisionePrec
        {
            get { return _revisionePrec; }
        }

        private List<relazione> _relazionePrec;
        public List<relazione> relazionePrec
        {
            get { return this._relazionePrec; }
        }

        private List<TimeSpan> _pausePrec;
        public List<TimeSpan> pausePrec
        { get { return this._pausePrec; } }

        private List<TimeSpan> _pauseSucc;
        public List<TimeSpan> pauseSucc
        { get { return this._pauseSucc; } }

        public Boolean setPausaPrec(TaskVariante tskPrec, TimeSpan pausa)
        {
            Boolean ret = false;
            this.loadPrecedenti(tskPrec.variant);
            bool found = false;
            int index = -1;
            try
            {
                var controllo = this.processiPrec.Where(x => x == tskPrec.Task.processID).First();
                index = this.processiPrec.IndexOf(tskPrec.Task.processID);
                found = true;
            }
            catch
            {
                found = false;
            }

            if(found && index!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                cmd.CommandText = "UPDATE precedenzeprocessi SET pausa = '"
                    + Math.Truncate(pausa.TotalHours).ToString() + ":"
                    + pausa.Minutes.ToString() + ":"
                    + pausa.Seconds.ToString()
                    + "' WHERE "
                    + "prec = " + tskPrec.Task.processID.ToString()
                    + " AND revPrec = " + tskPrec.Task.revisione.ToString()
                    + " AND succ = " + this.processID.ToString()
                    + " AND revSucc=" + this.revisione.ToString()
                    + " AND variante = " + tskPrec.variant.idVariante.ToString();

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }

            return ret;
        }

        /* 0: previous task can be started if preceeding is opened
         * 1: following task can only be started if previous task is closed
         * 
         * in both cases, following task only can be closed if the previous task is closed.
         */
        private List<int> _ConstraintType;
        public List<int> ConstraintType
        {
            get
            {
                return this._ConstraintType;
            }
        }

        public Boolean setConstraintType(TaskVariante tskPrec, int cstrType)
        {
            Boolean ret = false;
            this.loadPrecedenti(tskPrec.variant);
            bool found = false;
            int index = -1;
            try
            {
                var controllo = this.processiPrec.Where(x => x == tskPrec.Task.processID).First();
                index = this.processiPrec.IndexOf(tskPrec.Task.processID);
                found = true;
            }
            catch
            {
                found = false;
            }

            if (found && index != -1 && (cstrType == 0 || cstrType == 1))
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                cmd.CommandText = "UPDATE precedenzeprocessi SET ConstraintType = " + cstrType.ToString()
                    + " WHERE "
                    + "prec = " + tskPrec.Task.processID.ToString()
                    + " AND revPrec = " + tskPrec.Task.revisione.ToString()
                    + " AND succ = " + this.processID.ToString()
                    + " AND revSucc=" + this.revisione.ToString()
                    + " AND variante = " + tskPrec.variant.idVariante.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }

            return ret;
        }

        public List<processo> subProcessi;

        private int _processoPadre;
        public int processoPadre { get { return _processoPadre; } }

        private int _revPadre;
        public int revPadre
        {
            get { return this._revPadre; }
        }

        /* Ritorna un array di 2 elementi: il primo è l'ID del padre, il secondo la revisione*/
        public int[] getPadre(variante vr)
        {
            int[] ret = new int[2];
            ret[0] = -1;
            ret[1] = -1;
            if(this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT padre, revPadre FROM processipadrifigli WHERE task = " + this.processID.ToString()
                    + " AND revTask = " + this.revisione.ToString() + " AND variante = " + vr.idVariante.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() &&!rdr.IsDBNull(0))
                {
                    ret[0] = rdr.GetInt32(0);
                    ret[1] = rdr.GetInt32(1);
                }
                rdr.Close();
                conn.Close();
            }
            return ret;
        }

        public int processID { get { return _processID; } }
        public String processName
        {
            get { return _processName; }
            set
            {
                string strSQL = "UPDATE processo SET Name = '" + value + "' WHERE processID = " + _processID.ToString();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                _processName = value;
                conn.Close();
            }
        }
        public String processDescription {
            get { return _processDescription; }
            set
            {
                string strSQL = "UPDATE processo SET Description = '" + value + "' WHERE processID = " + _processID.ToString();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                _processDescription = value;
                conn.Close();
            }
        }

        private int _posX;
        public int posX
        {
            get { return this._posX; }
            set
            {
                string strSQL = "UPDATE processo SET posx = " + value + " WHERE processID = " + _processID.ToString();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                this._posX = value;
                conn.Close();
            }
        }

        public bool setPosX(int psx, variante vr)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE processipadrifigli SET posx = " + psx.ToString() + " WHERE variante = " + vr.idVariante.ToString()
                + " AND task = " + this.processID.ToString() + " AND revTask = " + this.revisione.ToString();
            cmd.ExecuteNonQuery();
            this._posX = psx;
            rt = true;
            conn.Close();
            return rt;
        }

        public bool setPosY(int psy, variante vr)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE processipadrifigli SET posy = " + psy.ToString() + " WHERE variante = " + vr.idVariante.ToString()
                + " AND task = " + this.processID.ToString() + " AND revTask = " + this.revisione.ToString();
            cmd.ExecuteNonQuery();
            this._posY = psy;
            rt = true;
            conn.Close();
            return rt;
        }

        private int _posY;
        public int posY
        {
            get { return this._posY; }
            set
            {
                string strSQL = "UPDATE processo SET posy = " + value + " WHERE processID = " + _processID.ToString();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                this._posY = value;
                conn.Close();
            }
        }

        public Kpi[] KPIs;
        private int _numKPIs;
        public int numKPIs
        {
            get { return _numKPIs; }
        }

        private bool _isVSM;
        public bool isVSM
        {
            get { return this._isVSM; }
            set
            {
                string strSQL = "UPDATE processo SET isVSM = " + value + " WHERE processID = " + _processID.ToString();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                this._isVSM = value;
                conn.Close();
            }
        }

        private bool _attivo;
        public bool attivo
        {
            get { return this._attivo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE processo SET attivo = "+value+" WHERE processID = " + this.processID.ToString()
                    + " AND revisione = " + this.revisione.ToString();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private User[] _processOwners;
        public User[] processOwners
        {
            get { return this._processOwners; }
        }

        private int _numProcessOwners;
        public int numProcessOwners
        {
            get { return this._numProcessOwners; }
        }

        private variante _varianteSelezionata;
        public variante varianteSelezionata
        {
            get { return this._varianteSelezionata; }
        }

        public String err;

        public processo()
        {
            this._processID = -1;
            this._dataRevisione = DateTime.UtcNow;
            this._revisione = 0;
            this._processName = "NULL";
            this._processDescription = "NULL";
            this.subProcessi = null;
            this._processoPadre = -1;
            this.KPIs = null;
            this._isVSM = false;
            this._numKPIs = 0;
            this._posX = 0;
            this._posY = 0;
            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
        }

        public processo(int procID)
        {
            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;

            // Ricerco l'ultima revisione del processo
            String strSQL = "SELECT MAX(revisione) FROM processo WHERE processID = " + procID.ToString() + " AND processo.attivo = 1";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader mysqlReader = cmd.ExecuteReader();
            if (mysqlReader.Read() && !mysqlReader.IsDBNull(0))
            {
                this._revisione = mysqlReader.GetInt32(0);
            }
            mysqlReader.Close();

            // Carico le informazioni di base del processo.
            strSQL = "SELECT processo.processID, revisione, dataRevisione, processo.Name, processo.Description, "
            + "isVSM, posx, posy, attivo FROM processo WHERE processID = " + procID.ToString() + " AND revisione = " + this.revisione.ToString() + " AND processo.attivo = 1";
            
            cmd = new MySqlCommand(strSQL, conn);
            mysqlReader = cmd.ExecuteReader();
            if (mysqlReader.Read() && !mysqlReader.IsDBNull(0))
            {
                this._processID = procID;
                this._revisione = mysqlReader.GetInt32(1);
                this._dataRevisione = mysqlReader.GetDateTime(2);
                this._processName = mysqlReader.GetString(3);
                this._processDescription = mysqlReader.GetString(4);
                if (mysqlReader.GetBoolean(5) == true)
                {
                    this._isVSM = true;
                }
                else
                {
                    this._isVSM = false;
                }
                this._posX = mysqlReader.GetInt32(6);
                this._posY = mysqlReader.GetInt32(7);
                this._attivo = mysqlReader.GetBoolean(8);
            }
            else
            {
                this._processID = -1;
                this._processName = "";
                this._processDescription = "";
            }

            mysqlReader.Close();

             conn.Close();
        }
        
        public processo(int procID, int rev)
        {
            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            // Carico le informazioni di base del processo.
            String strSQL = "SELECT processo.processID, revisione, dataRevisione, processo.Name, processo.Description, "
            + "isVSM, posx, posy, attivo FROM processo WHERE processID = " + procID.ToString() + " AND revisione = " + rev.ToString();// +" AND processo.attivo = 1";

            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader mysqlReader = cmd.ExecuteReader();
            if (mysqlReader.Read() && !mysqlReader.IsDBNull(0))
            {
                this._processID = procID;
                this._revisione = mysqlReader.GetInt32(1);
                this._dataRevisione = mysqlReader.GetDateTime(2);
                this._processName = mysqlReader.GetString(3);
                this._processDescription = mysqlReader.IsDBNull(4) ? "" : mysqlReader.GetString(4);
                if (mysqlReader.GetBoolean(5) == true)
                {
                    this._isVSM = true;
                }
                else
                {
                    this._isVSM = false;
                }
                this._posX = mysqlReader.GetInt32(6);
                this._posY = mysqlReader.GetInt32(7);
                this._attivo = mysqlReader.GetBoolean(8);
            }
            else
            {
                this._processID = -1;
                this._processName = "";
                this._processDescription = "";
            }

            mysqlReader.Close();

            conn.Close();
        }

        public processo(String procName)
        {
            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;

            // Ricerco l'ultima revisione del processo
            String strSQL = "SELECT processID FROM processo WHERE Name LIKE '" + procName.ToString() + "' AND processo.attivo = 1";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader mysqlReader = cmd.ExecuteReader();
            if (mysqlReader.Read() && !mysqlReader.IsDBNull(0))
            {
                int idProc = mysqlReader.GetInt32(0);
                this.setupInt(idProc);
            }
            else
            {
                this._processID = -1;
                this._revisione = -1;
                this._processName = "";
            }
            mysqlReader.Close();
            conn.Close();
        }

        public Boolean setupInt(int procID)
        {
            Boolean ret = false;
            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;

            // Ricerco l'ultima revisione del processo
            String strSQL = "SELECT MAX(revisione) FROM processo WHERE processID = " + procID.ToString() + " AND processo.attivo = 1";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader mysqlReader = cmd.ExecuteReader();
            if (mysqlReader.Read() && !mysqlReader.IsDBNull(0))
            {
                this._revisione = mysqlReader.GetInt32(0);
            }
            mysqlReader.Close();

            // Carico le informazioni di base del processo.
            strSQL = "SELECT processo.processID, revisione, dataRevisione, processo.Name, processo.Description, "
            + "isVSM, posx, posy, attivo FROM processo WHERE processID = " + procID.ToString() + " AND revisione = " + this.revisione.ToString() + " AND processo.attivo = 1";

            cmd = new MySqlCommand(strSQL, conn);
            mysqlReader = cmd.ExecuteReader();
            if (mysqlReader.Read() && !mysqlReader.IsDBNull(0))
            {
                this._processID = procID;
                this._revisione = mysqlReader.GetInt32(1);
                this._dataRevisione = mysqlReader.GetDateTime(2);
                this._processName = mysqlReader.GetString(3);
                this._processDescription = mysqlReader.GetString(4);
                if (mysqlReader.GetBoolean(5) == true)
                {
                    this._isVSM = true;
                }
                else
                {
                    this._isVSM = false;
                }
                this._posX = mysqlReader.GetInt32(6);
                this._posY = mysqlReader.GetInt32(7);
                this._attivo = mysqlReader.GetBoolean(8);
                ret = true;
            }
            else
            {
                this._processID = -1;
                this._processName = "";
                this._processDescription = "";
                ret = false;
            }

            mysqlReader.Close();

            conn.Close();
            return ret;
        }
        

        // Carico i dati del processo padre.
        public void loadPadre()
        {
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT padre, revPadre FROM processipadrifigli WHERE task = " + this.processID.ToString()
                    + " AND revTask = " + this.revisione.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._processoPadre = rdr.GetInt32(0);
                    this._revPadre = rdr.GetInt32(1);
                }
                else
                {
                    this._processoPadre = -1;
                    this._revPadre = -1;
                }
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._processoPadre = -1;
                this._revPadre = -1;
            }
        }

        // Carico i dati del processo padre.
        public void loadPadre(variante vr)
        {
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT padre, revPadre FROM processipadrifigli WHERE task = " + this.processID.ToString()
                    + " AND revTask = " + this.revisione.ToString() + " AND variante = " + vr.idVariante.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._processoPadre = rdr.GetInt32(0);
                    this._revPadre = rdr.GetInt32(1);
                }
                else
                {
                    this._processoPadre = -1;
                    this._revPadre = -1;
                }
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._processoPadre = -1;
                this._revPadre = -1;
            }
        }

        // Carico le varianti cui appartiene il processo this nell'array variantiprocessi
        public bool loadVarianti()
        {
            this._variantiProcesso = new List<variante>();
            bool rt = false;
            if (this.processID >= 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                
                
                cmd.CommandText = "SELECT variante FROM processo INNER JOIN variantiprocessi ON "
                    + "(variantiprocessi.processo = processo.processID AND variantiprocessi.revProc = processo.revisione) "
                    + " WHERE processo.processID = " + this.processID.ToString() + " AND processo.revisione = " + this.revisione.ToString();
                    //+ " AND processo.attivo = 1";
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    this._variantiProcesso.Add(new variante(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();

            }
            return rt;
        }

        // Carica l'elenco delle varianti cui appartengono i figli di questo processo
        public bool loadVariantiFigli()
        {
            bool ret = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();


                this._variantiFigli = new List<variante>();
                cmd.CommandText = "SELECT DISTINCT(variante) FROM processipadrifigli INNER JOIN processo ON (processipadrifigli.task = processo.processID "
                    + " AND processipadrifigli.revTask = processo.revisione)"
                    + " WHERE processipadrifigli.padre = " + this.processID.ToString() + " AND processipadrifigli.revPadre = " + this.revisione.ToString()
                    + " AND processo.attivo = 1";

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    this._variantiFigli.Add(new variante(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
            return ret;
        }

        // Carica l'array completo dei figli in subProcessi
        public bool loadFigli()
        {
            bool rt;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT task, revTask FROM processo INNER JOIN variantiprocessi ON "
                + "(variantiprocessi.processo = processo.processID AND variantiprocessi.revProc = processo.revisione) "
                + "INNER JOIN processipadrifigli ON(processipadrifigli.padre = variantiprocessi.processo AND processipadrifigli.revPadre = variantiprocessi.revProc "
                + " AND processipadrifigli.variante = variantiprocessi.variante) WHERE "
                + " processipadrifigli.padre = " + this.processID.ToString()
                + " AND processipadrifigli.revPadre = " + this.revisione.ToString() + " AND processo.attivo = 1";
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                this.subProcessi = new List<processo>();
                int i = 0;
                while (mysqlReader.Read())
                {
                    this.subProcessi.Add(new processo(mysqlReader.GetInt32(0)));
                    this.subProcessi[i].loadPrecedenti();
                    this.subProcessi[i].loadSuccessivi();
                    i++;
                }
                mysqlReader.Close();
                conn.Close();
                sortSons();
                this._varianteSelezionata = null;
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        // Carica in subProcessi l'array dei figli che appartengono però alla sola variante var
        public bool loadFigli(variante var)
        {
            bool ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            this.subProcessi = new List<processo>();

            cmd.CommandText = "SELECT task, revTask, processipadrifigli.posx, processipadrifigli.posy FROM processo INNER JOIN variantiprocessi ON "
                + "(variantiprocessi.processo = processo.processID AND variantiprocessi.revProc = processo.revisione) "
                + "INNER JOIN processipadrifigli ON(processipadrifigli.padre = variantiprocessi.processo AND processipadrifigli.revPadre = variantiprocessi.revProc "
                + " AND processipadrifigli.variante = variantiprocessi.variante) INNER JOIN processo AS figlio ON(figlio.processID = task AND figlio.revisione=revtask) WHERE "
                + " variantiprocessi.variante = " + var.idVariante.ToString() + " AND processipadrifigli.padre = " + this.processID.ToString()
                + " AND processipadrifigli.revPadre = " + this.revisione.ToString() + " AND processo.attivo = 1 AND figlio.attivo=1 "
                + " ORDER by processipadrifigli.posx";

            MySqlDataReader rdr = cmd.ExecuteReader();
            int i = 0;
            while (rdr.Read())
            {
                this.subProcessi.Add(new processo(rdr.GetInt32(0), rdr.GetInt32(1)));
                this.subProcessi[i]._posX = rdr.GetInt32(2);
                this.subProcessi[i]._posY = rdr.GetInt32(3);
                this.subProcessi[i]._varianteSelezionata = var;
                i++;
            }
            conn.Close();
            sortSons();
            this._varianteSelezionata = var;

            return ret;
        }

        // Ordina l'array subProcessi. Funziona nel caso di VSM o SIPOC
        public void sortSons()
        {
            // Utile in caso di Value - Stream. Ricercare il primo e accodare gli altri
            if (isVSM)
            {
                int firstIndex = -1;
                for (int i = 0; i < this.subProcessi.Count; i++)
                {
                    this.subProcessi[i].loadSuccessivi();
                    this.subProcessi[i].loadPrecedenti();
                    if (this.subProcessi[i].processiPrec.Count == 0)
                    {
                        // Questo è il primo.
                        firstIndex = i;
                    }
                }
                // scambio processo di indice 0 con quelli di indice firstIndex.
                if (firstIndex != -1)
                {
                    processo swapProc = this.subProcessi[firstIndex];
                    this.subProcessi[firstIndex] = this.subProcessi[0];
                    this.subProcessi[0] = swapProc;
                    for (int i = 1; i < this.subProcessi.Count; i++)
                    {
                        // Trovo dove si trova il successivo dell'i-1 e lo salvo su found
                        int found = -1;
                        for (int j = i; j < this.subProcessi.Count; j++)
                        {
                            if (this.subProcessi[i - 1].processiSucc.Count > 0)
                            {
                                if (this.subProcessi[i - 1].processiSucc[0] == this.subProcessi[j].processID)
                                {
                                    found = j;
                                }
                            }

                        }
                        if (found != -1)
                        {
                            swapProc = this.subProcessi[i];
                            this.subProcessi[i] = this.subProcessi[found];
                            this.subProcessi[found] = swapProc;
                        }
                    }
                }
            }
        }

        public bool loadKPIs()
        {
            bool rt;
            if (this.processID != -1)
            {
                String strSQL = "SELECT COUNT(id) FROM kpi_description WHERE idprocesso = " + this.processID.ToString() + " AND revisione = " + this.revisione.ToString() + " AND attivo = 1";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                mysqlReader.Read();
                this._numKPIs = mysqlReader.GetInt32(0);
                this.KPIs = new Kpi[this.numKPIs];
                mysqlReader.Close();
                strSQL = "SELECT id FROM kpi_description WHERE idprocesso = " + this.processID.ToString() + " AND revisione = " + this.revisione.ToString() + " AND attivo = 1";
                cmd = new MySqlCommand(strSQL, conn);
                mysqlReader = cmd.ExecuteReader();
                int c = 0;
                while (c < this.numKPIs && mysqlReader.Read())
                {
                    this.KPIs[c] = new Kpi(mysqlReader.GetInt32(0));
                    c++;
                }
                mysqlReader.Close();
                conn.Close();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public double getKPIBaseValByName(String kpiName)
        {
            double baseValKPI = 0;
            for (int i = 0; i < this.numKPIs; i++)
            {
                if(kpiName == this.KPIs[i].name)
                {
                    baseValKPI = this.KPIs[i].baseVal;
                }
            }
            return baseValKPI;
        }

        public List<NearTask> PreviousTasks;

        // Carica l'array dei processi precedenti
        public bool loadPrecedenti()
        {
            this.PreviousTasks = new List<NearTask>();
           bool res = false;
            this._ConstraintType = new List<int>();
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                this._processiPrec = new List<int>();
                this._revisionePrec = new List<int>();
                this._relazionePrec = new List<relazione>();
                this._pausePrec = new List<TimeSpan>();

                // Carico i processi precedenti
                cmd.CommandText = "SELECT precedenzeprocessi.prec, precedenzeprocessi.revPrec, precedenzeprocessi.relazione, "
                    + "precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType"
                    + " FROM precedenzeprocessi"
                    + " WHERE precedenzeprocessi.succ = " + this.processID.ToString() + " AND precedenzeprocessi.revsucc = " + this.revisione.ToString();
                MySqlDataReader mysqlReader = cmd.ExecuteReader();

                while(mysqlReader.Read())
                {
                    this._processiPrec.Add(mysqlReader.GetInt32(0));
                    this._revisionePrec.Add(mysqlReader.GetInt32(1));
                    this._relazionePrec.Add(new relazione(mysqlReader.GetInt32(2)));
                    this._pausePrec.Add(mysqlReader.GetTimeSpan(3));
                    this._ConstraintType.Add(mysqlReader.GetInt32(4));

                    NearTask curr = new NearTask();
                    curr.NearTaskID = mysqlReader.GetInt32(0);
                    curr.ConstraintType = mysqlReader.GetInt32(4);
                    processo currprc = new processo(curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.PreviousTasks.Add(curr);
                }

                mysqlReader.Close();
                conn.Close();
                res = true;
            }
            return res;
        }

        // Carica l'array dei processi precedenti
        public bool loadPrecedenti(variante var)
        {
            bool res = false;
            this._ConstraintType = new List<int>();
            this.PreviousTasks = new List<NearTask>();
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                this._processiPrec = new List<int>();
                this._relazionePrec = new List<relazione>();
                this._revisionePrec = new List<int>();
                this._pausePrec = new List<TimeSpan>();
        
                // Carico i processi precedenti
                cmd.CommandText = "SELECT precedenzeprocessi.prec, precedenzeprocessi.revPrec, precedenzeprocessi.relazione, precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType FROM precedenzeprocessi"
                    + " WHERE precedenzeprocessi.succ = " + this.processID.ToString() + " AND precedenzeprocessi.revsucc = " + this.revisione.ToString()
                    + " AND precedenzeprocessi.variante = " + var.idVariante.ToString();
                
                MySqlDataReader mysqlReader = cmd.ExecuteReader();


                while(mysqlReader.Read())
                {
                    this._processiPrec.Add(mysqlReader.GetInt32(0));
                    this._relazionePrec.Add(new relazione(mysqlReader.GetInt32(2)));
                    this._revisionePrec.Add(mysqlReader.GetInt32(1));
                    this._pausePrec.Add(mysqlReader.GetTimeSpan(3));
                    this._ConstraintType.Add(mysqlReader.GetInt32(4));

                    NearTask curr = new NearTask();
                    curr.NearTaskID = mysqlReader.GetInt32(0);
                    curr.ConstraintType = mysqlReader.GetInt32(4);
                    processo currprc = new processo(curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.PreviousTasks.Add(curr);
                }

                mysqlReader.Close();
                conn.Close();
                res = true;
            }
            return res;
        }

        public List<NearTask> FollowingTasks;

        // Carica l'array dei processi successivi
        public bool loadSuccessivi()
        {
            this._pauseSucc = new List<TimeSpan>();
            this._ConstraintType = new List<int>();
            this.FollowingTasks = new List<NearTask>();
            bool res = false;
            if (this._processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                this._processiSucc = new List<int>();
                this._revisioneSucc = new List<int>();
                this._relazioneSucc = new List<relazione>();

                // Carico i processi successivi
                cmd.CommandText = "SELECT precedenzeprocessi.succ, precedenzeprocessi.revSucc, precedenzeprocessi.relazione, "
                    + " precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType"
                    + " FROM precedenzeprocessi "
                    + " WHERE precedenzeprocessi.prec = " + this.processID.ToString() + " AND precedenzeprocessi.revPrec = "
                    + this.revisione.ToString();
                MySqlDataReader mysqlReader = cmd.ExecuteReader();
                while(mysqlReader.Read())
                {
                    this._processiSucc.Add(mysqlReader.GetInt32(0));
                    this._revisioneSucc.Add(mysqlReader.GetInt32(1));
                    this._relazioneSucc.Add(new relazione(mysqlReader.GetInt32(2)));
                    this._pauseSucc.Add(mysqlReader.GetTimeSpan(3));
                    if(!mysqlReader.IsDBNull(4))
                    { 
                    this._ConstraintType.Add(mysqlReader.GetInt32(4));
                    }
                    else
                    {
                        this._ConstraintType.Add(0);
                    }

                    NearTask curr = new NearTask();
                    curr.NearTaskID = mysqlReader.GetInt32(0);
                    curr.ConstraintType = mysqlReader.GetInt32(4);
                    processo currprc = new processo(curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.FollowingTasks.Add(curr);
                }

                mysqlReader.Close();
                conn.Close();
                res = true;
            }
            return res;
        }

        // Carica l'array dei processi successivi, limitati ad una variante
        public bool loadSuccessivi(variante var)
        {
            bool res = false;
            this.FollowingTasks = new List<NearTask>();
            this._pauseSucc = new List<TimeSpan>();
            this._revisioneSucc = new List<int>();
            this._ConstraintType = new List<int>();
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                
                MySqlCommand cmd = conn.CreateCommand();

                this._processiSucc = new List<int>();
                this._relazioneSucc = new List<relazione>();


                // Carico i processi successivi
                cmd.CommandText = "SELECT precedenzeprocessi.succ, precedenzeprocessi.revSucc, precedenzeprocessi.relazione, "
                    + "precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType "
                    + " FROM precedenzeprocessi "
                    + " WHERE precedenzeprocessi.prec = " + this.processID.ToString() + " AND precedenzeprocessi.revPrec = "
                    + this.revisione.ToString() + " AND precedenzeprocessi.variante = " + var.idVariante.ToString();

                MySqlDataReader mysqlReader = cmd.ExecuteReader();

                while (mysqlReader.Read())
                {
                    this._processiSucc.Add(mysqlReader.GetInt32(0));
                    this._revisioneSucc.Add(mysqlReader.GetInt32(1));
                    this._relazioneSucc.Add(new relazione(mysqlReader.GetInt32(2)));
                    this._pauseSucc.Add(mysqlReader.GetTimeSpan(3));
                    this._ConstraintType.Add(mysqlReader.GetInt32(4));

                    NearTask curr = new NearTask();
                    curr.NearTaskID = mysqlReader.GetInt32(0);
                    curr.ConstraintType = mysqlReader.GetInt32(4);
                    processo currprc = new processo(curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.FollowingTasks.Add(curr);
                }

                mysqlReader.Close();
                conn.Close();
                res = true;
            }
            return res;
        }

        /*
         * Returns:
         * -1 if some error
         * processID if all is ok
         */
        public int createDefaultSubProcess(variante var)
        {
            int res = -1;
            if (this.processID != -1)
            {                
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = trans;

                    // Trovo l'ultimo processo
                    int procID = 0;
                    cmd.CommandText = "SELECT MAX(ProcessID) FROM processo";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    if (!rdr.IsDBNull(0))
                    {
                        procID = rdr.GetInt32(0) + 1;
                    }
                    rdr.Close();
                    try
                    {
                        // Aggiungo il processo
                        cmd.CommandText = "INSERT INTO processo(ProcessID, revisione, dataRevisione, Name, Description, "
                        + " isVSM, posx, posy, attivo) VALUES(" + procID + ", 0, '" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                        + "', 'New Default Process', 'New Default Process Notes', 0, 100, 100, 1)";
                        cmd.ExecuteNonQuery();

                        // Inserisco il task come figlio di this e se incontro rogne faccio un rollback di tutto!
                        cmd.CommandText = "INSERT INTO processipadrifigli(task, revTask, padre, revPadre, variante) VALUES("
                            + procID.ToString() + ", 0, " + this.processID.ToString() + ", " + this.revisione.ToString() + ", "+var.idVariante.ToString()+")";
                        cmd.ExecuteNonQuery();
                        res = procID;
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        res = -1;
                        trans.Rollback();
                    }
                    
                    // CASO DI VALUE STREAM MAP: VERIFICO SE HO CREATO IL PROCESSO, E LO COLLEGO ALL'ULTIMO DELLO STREAM!
                    if (res != -1 && this.isVSM)
                    {
                        // Crea un collegamento con l'ultimo subprocesso, se già presenti
                        this.loadFigli(var);
                        if (this.subProcessi.Count > 1)
                        {
                            int indLast = -1;
                            for (int i = 0; i < this.subProcessi.Count; i++)
                            {
                                this.subProcessi[i].loadSuccessivi();
                                if (this.subProcessi[i].processiSucc.Count == 0 && this.subProcessi[i].processID != procID)
                                {
                                    indLast = i;
                                }
                            }
                            if (indLast != -1)
                            {
                                processo corrente = new processo(procID);
                                this.subProcessi[indLast].addProcessoSuccessivo(corrente, var, 0);
                            }
                        }
                    }

                // AGGIUNGO I KPI DI DEFAULT
                /*Kpi def = new Kpi();
                def.add("Warning", HttpUtility.HtmlEncode("Segnala problemi all'interno dei processi"), new processo(procID), 0);*/
                conn.Close();
                
            }
            return res;
        }

        public bool addVariante(variante var)
        {
            bool ret = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO variantiprocessi(variante, processo, revProc, ExternalID, measurementUnit) VALUES(" 
                    + var.idVariante.ToString() + ", " 
                    + this.processID.ToString() + ", " 
                    + this.revisione.ToString() +", "
                    +"NULL, "
                    + "0"
                    + ")";
                try
                {
                    cmd.ExecuteNonQuery();
                    ret = true;
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    ret = false;
                }
                conn.Close();
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if process deleted
         * 2 if process not deleted because of sub-processes or because of kpis
         * 3 if process is not deleted because of multi-precedent processes
         * 4 se il processo è attivo in qualche reparto.
         */
        // To-do: cancellare i PERMESSI associati al processo
       public int delete()
       {
           int res = 1;
           if (this.processID != -1)
           {
               this.loadFigli();
               this.loadKPIs();
               bool foundReparto = false;
               ElencoReparti elRep = new ElencoReparti();
               for (int i = 0; i < elRep.elenco.Count; i++)
               {
                   elRep.elenco[i].loadProcessiVarianti();
                   for(int j = 0; j < elRep.elenco[i].processiVarianti.Count; j++)
                   {
                       if (this.processID == elRep.elenco[i].processiVarianti[j].process.processID)
                       {
                           foundReparto = true;
                       }
                   }
                   
               }
               //foundReparto = true;
               if (this.subProcessi.Count == 0 && this.numKPIs == 0 && foundReparto == false)
               {
                   MySqlConnection conn = (new Dati.Dati()).mycon();
                   conn.Open();
                   MySqlCommand cmd = conn.CreateCommand();
                   MySqlTransaction trans = conn.BeginTransaction();
                   cmd.Transaction = trans;
                   cmd.Connection = conn;

                   try
                   {
                       // Rimuovo l'associazione col processo padre
                       String strSQL = "DELETE FROM processipadrifigli WHERE task = " + this.processID.ToString()
                           + " AND revTask = " + this.revisione.ToString();
                       cmd.CommandText = strSQL;
                       cmd.ExecuteNonQuery();

                       // Cancello il process owner
                       strSQL = "DELETE FROM processOwners WHERE process = " + this.processID.ToString();
                       cmd.CommandText = strSQL;
                       cmd.ExecuteNonQuery();

                       // Cancello l'associazione con le varianti
                       this.loadVarianti();
                       cmd.CommandText = "DELETE FROM variantiprocessi WHERE processo = " + this.processID.ToString();
                       cmd.ExecuteNonQuery();

                       // E' il primo processo. Cancello il precedente dei successivi ed elimino il processo
                       strSQL = "DELETE FROM precedenzeprocessi WHERE prec = " + this.processID.ToString() + " OR succ = "
                           + this.processID.ToString() + " AND revPrec = " + this.revisione.ToString()
                           + " AND revSucc = " + this.revisione.ToString();
                       cmd.CommandText = strSQL;
                       cmd.ExecuteNonQuery();

                        this.attivo = false;
                       // Elimino il processo.
                       strSQL = "DELETE FROM processo WHERE processID = " + this.processID.ToString();
                       cmd.CommandText = strSQL;
                       cmd.ExecuteNonQuery();
                       trans.Commit();
                       res = 1;
                   }
                   catch(Exception ex)
                   {
                       log = ex.Message;
                       trans.Rollback();
                       res = 0;
                   }
                   conn.Close();
               }
               else if (foundReparto == true)
               {
                   res = 4;
               }
               else
               {
                   res = 2;
               }

           }
           else
           {
               res = 0;
           }
           return res;
       }

        /* Returns:
         * 0 if this is not loaded or if this has no precedent processes
         * 1 if relation has been correctly changed
         */
        public bool changeRelationPrec(processo precedente, relazione rel)
        {
            bool res = false;
            if (this.processID != -1 && this.processiPrec.Count >= 1)
            {
                string strSQL = "UPDATE precedenzeprocessi SET relazione = " + rel.relationID + " WHERE succ = " + this.processID.ToString() + " AND prec = " + precedente.processID.ToString();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(strSQL, conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
                res = true;
            }
            return res;
        }

        public bool addKPI(String nomeKPI, String descrKPI)
        {
            Kpi tmp = new Kpi();
            return tmp.add(nomeKPI, descrKPI, new processo(this.processID), 0);            
        }

        /* Returns:
         * 1 if next process correctly added
         * 0 if error
         */
        public bool addProcessoSuccessivo(processo next, variante var, int cstrType)
        {
            bool res = false;
            if(this.processID != -1)
            {
                String strSQL = "INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione, pausa, ConstraintType) VALUES(" + 
                    this.processID.ToString() + ", " + this.revisione.ToString() + ", " + 
                    next.processID.ToString() + ", " + next.revisione.ToString() + ", " + var.idVariante + ", 0, '00:00:00', "+cstrType.ToString()+")";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    res = true;
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    res = false;
                }
                loadSuccessivi();
                conn.Close();
            }
            return res;
        }

        /* Returns:
         * 1 if precedent process correctly added
         * 0 if error
         */
        public bool addProcessoPrecedente(processo preced, variante var, int cstrType)
        {
            bool res = false;
            if (this.processID != -1)
            {
                String strSQL = "INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione, pausa, ConstraintType) VALUES(" 
                    + preced.processID.ToString() + ", " + preced.revisione + ", " + this.processID.ToString() + ", " + this.revisione.ToString()
                    + ", " + var.idVariante.ToString() + ", 0, '00:00:00', " + cstrType.ToString() + ")";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                res = true;
                conn.Close();
                loadPrecedenti();
            }
            return res;
        }

        /* Returns:
         * 0 if generic error
         * 1 if link between processes is deleted successfully
         * 2 if "next" process remains orphan (nothing will be deleted)
         */
        public int deleteProcessoSuccessivo(processo next, variante var)
        {
            int res = 0;
            if (this.processID != -1)
            {
                // Look if next è un processo successivo rispetto a this.
                bool found = false;
                this.loadSuccessivi();
                this.loadPrecedenti();
                next.loadPrecedenti();
                next.loadSuccessivi();
                for (int i = 0; i < this.processiSucc.Count; i++)
                {
                    if (this.processiSucc[i] == next.processID)
                    {
                        found = true;
                    }
                }

                if (found == true)
                {
                    if (next.processiSucc.Count > 0 || next.processiPrec.Count > 0)
                    {
                        // Se il processo successivo non rimane orfano cancello il legame con this.
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        String strSQL = "DELETE FROM precedenzeprocessi WHERE prec = " + this.processID.ToString() 
                            + " AND revPrec = " + this.revisione.ToString() + " AND succ = " + next.processID.ToString()
                            + " AND revSucc = " + next.revisione.ToString() + " AND variante = " + var.idVariante.ToString();
                        MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        res = 1;
                        loadSuccessivi();
                    }
                    else
                    {
                        // Se il processo successivo rimane orfano non cancello nulla e lo segnalo
                        res = 2;
                    }
                }
                else
                {
                    res = 0;
                }
            }
            return res;
        }

        /* Returns:
         * 0 if generic error
         * 1 if link between processes is deleted successfully
         * 2 if "next" process remains orphan (nothing will be deleted)
         */
        public int deleteProcessoPrecedente(processo preced, variante var)
        {
            int res = 0;
            if (this.processID != -1)
            {
                this.loadPrecedenti();
                this.loadSuccessivi();
                preced.loadPrecedenti();
                preced.loadSuccessivi();
                // Look if next è un processo successivo rispetto a this.
                bool found = false;
                for (int i = 0; i < this.processiPrec.Count; i++)
                {
                    if (this.processiPrec[i] == preced.processID)
                    {
                        found = true;
                    }
                }

                if (found == true)
                {
                    if (preced.processiPrec.Count > 0 || preced.processiSucc.Count > 0)
                    {
                        // Se il processo successivo non rimane orfano cancello il legame con this.
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        String strSQL = "DELETE FROM precedenzeprocessi WHERE succ = " + this.processID.ToString() + " AND revSucc = " + this.revisione.ToString() + " AND prec = " + preced.processID.ToString() + " AND revPrec = " + preced.revisione.ToString() + " AND variante = " + var.idVariante;
                        MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        res = 1;
                        loadPrecedenti();
                    }
                    else
                    {
                        // Se il processo successivo rimane orfano non cancello nulla e lo segnalo
                        res = 2;
                    }
                }
                else
                {
                    res = 0;
                }
            }
            return res;
        }

        public bool isPrecedente(processo curr, variante var)
        {
            bool found = false;
            this.loadPrecedenti(var);
            for (int i = 0; i < this.processiPrec.Count && found == false; i++)
            {
                if (this.processiPrec[i] == curr.processID)
                {
                    found = true;
                }
                else
                {
                    processo prec = new processo(this.processiPrec[i]);
                    found = prec.isPrecedente(curr, var);
                }
            }
            return found;
            
        }

        public bool isSuccessivo(processo curr, variante var)
        {
            bool found = false;
            this.loadSuccessivi(var);
            for (int i = 0; i < this.processiSucc.Count && found == false; i++)
            {
                if (this.processiSucc[i] == curr.processID)
                {
                    found = true;
                }
                else
                {
                    processo succ = new processo(this.processiSucc[i]);
                    found = succ.isSuccessivo(curr, var);
                }
            }
            return found;

        }

        public bool loadProcessOwners()
        {
            bool rt;
            if (this.processID != -1)
            {
                this._numProcessOwners = 0;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();

                // trovo il numero di processowners
                String strSQL = "SELECT COUNT(users.userID) FROM processo INNER JOIN processOwners "
                 + "ON(processo.ProcessID = processOwners.process) INNER JOIN users ON(users.userID = processOwners.user) "
                 + "WHERE processo.ProcessID = " + this.processID.ToString();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                this._numProcessOwners = rdr.GetInt32(0);
                this._processOwners = new User[this._numProcessOwners];
                rdr.Close();

                // creo l'istanza degli utenti
                strSQL = "SELECT processo.ProcessID, users.userID FROM processo INNER JOIN processOwners "
                 + "ON(processo.ProcessID = processOwners.process) INNER JOIN users ON(users.userID = processOwners.user) "
                 + "WHERE processo.ProcessID = " + this.processID.ToString();
                cmd = new MySqlCommand(strSQL, conn);
                rdr = cmd.ExecuteReader();
                for (int i = 0; i < this.numProcessOwners && rdr.Read(); i++)
                {
                    this._processOwners[i] = new User(rdr.GetString(1));
                }
                conn.Close();
                rt = true;
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public bool deleteProcessOwner(User currProcOwner)
        {
            bool rt = true;
            if (this.processID != -1)
            {
                String strSQL = "DELETE FROM processOwners WHERE process = " + this.processID.ToString() + " AND user = '" + currProcOwner.username + "'";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public bool addProcessOwner(User newProcOwner)
        {
            bool rt = true;
            if (this.processID != -1)
            {
                // Controllo se l'utente da aggiungere è già presente o meno
                this.loadProcessOwners();
                for (int i = 0; i < this.numProcessOwners; i++)
                {
                    if (newProcOwner.username == this.processOwners[i].username)
                    {
                        rt = false;
                    }
                }

                // Se l'utente da aggiungere non è già presente lo aggiungo
                if (rt == true)
                {
                    String strSQL = "INSERT INTO processOwners(process, user) VALUES(" + this.processID + ", '" + newProcOwner.username + "')";
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    this._processOwners = null;
                    this._numProcessOwners = 0;
                    this.loadProcessOwners();
                }
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        /*Returns:
         * 0 if generic error
         * 1 if all ok
         * 2 if some tasks have not following tasks or precedences.
         */
        public int checkConsistency(variante var)
        {
            int rt = 1;

            if (this.processID != -1)
            {
                // Controllo che tutti i processi figli del task abbiano almeno un precedente oppure un successivo
                this.loadFigli(var);
                if (this.subProcessi.Count == 1)
                {
                    rt = 1;
                }
                else
                {
                    for (int i = 0; i < this.subProcessi.Count; i++)
                    {
                        this.subProcessi[i].loadPrecedenti(var);
                        this.subProcessi[i].loadSuccessivi(var);
                        if (this.subProcessi[i].processiPrec.Count == 0 && this.subProcessi[i].processiSucc.Count == 0)
                        {
                            rt = 2;
                        }
                    }
                }
            }
            else
            {
                rt = 0;
            }
            return rt;
        }

        /* Returns:
         * 0 if generic error
         * 1 if all is fine
         * 2 if there is some task without link between other tasks
         * 3 if diagram type is NOT Pert
         * 5 if some subtask is missing Kpi called "Tempo ciclo"
         * 6 if some subtask is missing la postazione
         * 7 if there are no subtasks
         */
        public int checkConsistencyPERT(variante var)
        {
            int rt;
            rt = this.checkConsistency(var);
            if (this.processID != -1 && rt == 1)
            {
                // Controllo che il task sia effettivamente un PERT
                if (this.isVSM == true)
                {
                    rt = 3;
                }
                // Controllo che ci sia almeno 1 subtask
                this.loadFigli(var);
                if (rt == 1)
                {
                    if (this.subProcessi.Count == 0)
                    {
                        rt = 7;
                    }
                }
                // Controllo che tutti i subtasks abbiano un KPI chiamato "Tempo ciclo"
                if (rt == 1)
                {
                    for (int i = 0; i < this.subProcessi.Count && rt == 1; i++)
                    {
                        /*this.loadVarianti();
                        for (int j = 0; j < this.variantiProcesso.Count; j++)
                        {*/
                            TaskVariante tsk = new TaskVariante(this.subProcessi[i], var);
                            tsk.loadTempiCiclo();
                            if (tsk.Tempi.Tempi.Count == 0)
                            {
                                rt = 5;
                            }
                        //}
                    }
                }

                // Controllo che tutti i subtasks siano assegnati ad almeno una postazione
                if (rt == 1)
                {
                    for (int i = 0; i < this.subProcessi.Count; i++)
                    {
                        this.subProcessi[i].loadPostazioniTask();
                        if (this.subProcessi[i].elencoPostazioniTask.Count == 0)
                        {
                            rt = 6;
                        }
                    }
                }
            }
            return rt;
        }

        private double _earlyStartTime;
        public double earlyStartTime
        {
            get { return this._earlyStartTime; }
        }

        private double _earlyFinishTime;
        public double earlyFinishTime
        {
            get { return this._earlyFinishTime; }
        }

        private double _lateStartTime;
        public double lateStartTime
        {
            get
            {
                return this._lateStartTime;
            }
        }

        private double _lateFinishTime;
        public double lateFinishTime
        {
            get { return this._lateFinishTime; }
        }

        public List<processo> CriticalPath;

        public void calculateCriticalPath(variante vr)
        {
            if (this.checkConsistencyPERT(vr) == 1)
            {
                // Carico i processi figli e i loro legami di precedente / successivo
                this.loadFigli(vr);
                for (int i = 0; i < this.subProcessi.Count; i++)
                {
                    this.subProcessi[i].loadPrecedenti();
                    this.subProcessi[i].loadSuccessivi();
                }

                // Ricerco i task capostipite e calcolo il loro earlyStartTime ed earlyFinishTime
                for (int i = 0; i < this.subProcessi.Count; i++)
                {
                    if (this.subProcessi[i].processiPrec.Count == 0)
                    {
                        this.subProcessi[i].loadKPIs();
                        this.subProcessi[i]._earlyStartTime = 0.0;
                        this.subProcessi[i]._earlyFinishTime = this.subProcessi[i].getKPIBaseValByName("Tempo ciclo");
                        // Ora calcolo earlyStartTime e earlyFinishTime per i loro successivi, fino alla fine!!!
                        this.calculateEarlyTimesforSucc(this.subProcessi[i].processID, this.subProcessi[i]._earlyFinishTime);
                    }
                }

                // Ricerco i task finali e calcolo il loro lateStartTime e lateFinishTime
                // Per farlo devo trovare il task finale con il massimo earlyFinishTime e impostare lateFinishTime = max(earlyFinishTime)
                
                double maxEarlyFinishTime = 0.0;
                for (int i = 0; i < this.subProcessi.Count; i++)
                {
                    if (this.subProcessi[i].processiSucc.Count == 0)
                    {
                        if (this.subProcessi[i].earlyFinishTime > maxEarlyFinishTime)
                        {
                            maxEarlyFinishTime = this.subProcessi[i].earlyFinishTime;
                        }
                    }
                }

                // Inoltre inizializzo tutti quanti i task a maxEarlyFinishTime + 1
                for (int i = 0; i < this.subProcessi.Count; i++)
                {
                    this.subProcessi[i]._lateFinishTime = maxEarlyFinishTime + 1;
                }

                // Ora calcolo il lateFinishTime e il lateStartTime per i processi finali!
                for (int i = 0; i < this.subProcessi.Count; i++)
                {
                    if (this.subProcessi[i].processiSucc.Count == 0)
                    {
                        this.subProcessi[i]._lateFinishTime = maxEarlyFinishTime;
                        this.subProcessi[i]._lateStartTime = this.subProcessi[i]._lateFinishTime - this.subProcessi[i].getKPIBaseValByName("Tempo ciclo");
                        calculateLateTimesforPrec(this.subProcessi[i].processID, this.subProcessi[i].lateStartTime);
                    }
                }

                // Ora posso raccogliere le attività critiche
                // Condizione: dove data minima inizio == data massima inizio && data minima fine == data massima fine
                int numProcCritical = 0;
                CriticalPath = new List<processo>();
                for (int i = 0; i < this.subProcessi.Count; i++)
                {
                    if (this.subProcessi[i].earlyStartTime == this.subProcessi[i].lateStartTime && this.subProcessi[i].earlyFinishTime == this.subProcessi[i].lateFinishTime)
                    {
                        CriticalPath.Add(this.subProcessi[i]);
                        numProcCritical++;
                    }
                }

                // Ordino l'array per "LateStartTime"
                for (int j = 0; j < CriticalPath.Count - 1; j++)
                {
                    processo temp;
                    int pos_min = j;
                    for (int i = j + 1; i < CriticalPath.Count; i++)
                    {
                        if (CriticalPath[pos_min].lateStartTime > CriticalPath[i].lateStartTime)
                        {
                            pos_min = i;
                        }
                    }
                    if (pos_min != j)
                    {
                        temp = CriticalPath[j];
                        CriticalPath[j] = CriticalPath[pos_min];
                        CriticalPath[pos_min] = temp;
                    }
                }
            }
        }

        private void calculateEarlyTimesforSucc(int prcId, double finish)
        {
            int procIndex = -1;
            for (int i = 0; i < this.subProcessi.Count; i++)
            {
                if (this.subProcessi[i].processID == prcId)
                {
                    procIndex = i;
                }
            }

            this.subProcessi[procIndex].loadSuccessivi();
            for (int i = 0; i < this.subProcessi[procIndex].processiSucc.Count; i++)
            {
                // Cerco l'indice cazzo
                int index = -1;
                for (int j = 0; j < this.subProcessi.Count; j++)
                {
                    if (this.subProcessi[procIndex].processiSucc[i] == this.subProcessi[j].processID)
                    {
                        index = j;
                    }
                }

                // Ora andiamo a calcolare earlyStart e earlyFinish
                this.subProcessi[index].loadKPIs();
                if (this.subProcessi[index]._earlyStartTime < finish)
                {
                    this.subProcessi[index]._earlyStartTime = finish;
                }
                if (this.subProcessi[index]._earlyFinishTime < finish + this.subProcessi[index].getKPIBaseValByName("Tempo ciclo"))
                {
                    this.subProcessi[index]._earlyFinishTime = finish + this.subProcessi[index].getKPIBaseValByName("Tempo ciclo");
                }
                calculateEarlyTimesforSucc(this.subProcessi[index].processID, this.subProcessi[index]._earlyFinishTime);

            }
        }

        private void calculateLateTimesforPrec(int prcId, double start)
        {
            int procIndex = -1;
            for (int i = 0; i < this.subProcessi.Count; i++)
            {
                if (this.subProcessi[i].processID == prcId)
                {
                    procIndex = i;
                }
            }

            this.subProcessi[procIndex].loadPrecedenti();
            for (int i = 0; i < this.subProcessi[procIndex].processiPrec.Count; i++)
            {
                // Trovo l'indice del precedente
                int index = -1;
                for(int j = 0; j < this.subProcessi.Count; j++)
                {
                    if (this.subProcessi[j].processID == this.subProcessi[procIndex].processiPrec[i])
                    {
                        index = j;
                    }
                }
                if (index != -1)
                {
                    this.subProcessi[index].loadKPIs();
                    if (this.subProcessi[index]._lateFinishTime > start)
                    {
                        this.subProcessi[index]._lateFinishTime = start;
                    }
                    this.subProcessi[index]._lateStartTime = this.subProcessi[index]._lateFinishTime - this.subProcessi[index].getKPIBaseValByName("Tempo ciclo");
                    calculateLateTimesforPrec(this.subProcessi[index].processID, this.subProcessi[index]._lateStartTime);
                }
                
            }
        }
    
        // Gestione postazioni di lavoro del processo

        private int _numPostazioni;
        public int numPostazioni
        {
            get { return this._numPostazioni; }
        }
        private Postazione[] _elencoPostazioni;
        public Postazione[] elencoPostazioni
        {
            get { return this._elencoPostazioni; }
        }

        // Carica la postazioni disponibili per associare i figli come task!
        public bool loadPostazioniFigli()
        {
            bool rt = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(idpostazioni) FROM postazioni WHERE mainProc = " + this.processID.ToString() + " AND revProc = " + this.revisione.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                this._numPostazioni = 0;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._numPostazioni = rdr.GetInt32(0);
                }
                rdr.Close();
                this._elencoPostazioni = new Postazione[this.numPostazioni];
                cmd.CommandText = "SELECT idpostazioni FROM postazioni WHERE mainProc = " + this.processID.ToString() + " AND revProc = " + this.revisione.ToString();
                rdr = cmd.ExecuteReader();
                for (int i = 0; i < this.numPostazioni && rdr.Read(); i++)
                {
                    this._elencoPostazioni[i] = new Postazione(rdr.GetInt32(0));
                }
                conn.Close();
            }
            return rt;
        }


        // Associa il processo inteso come task ad una postazione
        public bool addTaskToPostazione(Postazione postID)
        {
            bool rt = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = trans;
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO taskspostazioni(postazione, idTask, revTask) VALUES(" + postID.id.ToString()
                    + ", " + this.processID.ToString() + ", " + this.revisione.ToString() + ")";
                try
                {
                    cmd.ExecuteNonQuery();
                    rt = true;
                    trans.Commit();
                }
                catch
                {
                    rt = false;
                    trans.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        public bool deleteTaskFromPostazioni()
        {
            bool rt = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = trans;
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM taskspostazioni WHERE idTask = " + this.processID.ToString() + " AND revTask = " + this.revisione.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    rt = true;
                    trans.Commit();
                }
                catch
                {
                    rt = false;
                    trans.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        private List<Postazione> _elencoPostazioniTask;
        public List<Postazione> elencoPostazioniTask
        {
            get { return this._elencoPostazioniTask; }
        }
        // Carica le postazioni cui questo processo è associato come task
        public bool loadPostazioniTask()
        {
            bool rt = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                this._elencoPostazioniTask = new List<Postazione>();
                cmd.CommandText = "SELECT postazione FROM repartipostazioniattivita WHERE processo = " +
                    this.processID.ToString() + " AND revProc = " + this.revisione.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this._elencoPostazioniTask.Add(new Postazione(rdr.GetInt32(0)));
                }
                conn.Close();
                rt = true;
            }
            return rt;
        }
   
        // Cambia l'assegnazione di un task ad una postazione. Caso associazione 1 - 1
        public bool changeTaskFromPostazione(Postazione postID)
        {
            bool rt = false;
            if (this.processID != -1 && postID.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = trans;
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE taskspostazioni SET postazione = " + postID.id.ToString() + " WHERE idTask = " + this.processID.ToString() + " AND revTask = " + this.revisione.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    rt = true;
                    trans.Commit();
                }
                catch
                {
                    rt = false;
                    trans.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        public bool buildNewBlankRevision()
        {
            bool rt = false;
            if (this.processID != -1)
            {
                    
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                
                try
                {
                    cmd.CommandText = "INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, isVSM, posx, posy, attivo) " +
                            "VALUES(" + this.processID + ", " + (this.revisione + 1).ToString() + ", '" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', '" + this.processName + "', '" + this.processDescription +
                            "', " + isVSM.ToString() + ", " + this.posX.ToString() + ", " + this.posY.ToString() + ", 1)";
                    cmd.ExecuteNonQuery();

                    // disattivo la revisione precedente
                    cmd.CommandText = "UPDATE processo SET attivo = 0 WHERE processID = " + this.processID.ToString() + " AND revisione = " + this.revisione.ToString();
                    cmd.ExecuteNonQuery();

                    // Copio i padri
                    List<int[]> elencoPadri = new List<int[]>();
                    cmd.CommandText = "SELECT padre, revPadre, variante, posx, posy FROM processipadrifigli WHERE task = " + this.processID.ToString()
                        + " AND revTask = " + this.revisione.ToString();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        int[] arrPadre = new int[5];
                        arrPadre[0] = rdr.GetInt32(0);
                        arrPadre[1] = rdr.GetInt32(1);
                        arrPadre[2] = rdr.GetInt32(2);
                        arrPadre[3] = rdr.GetInt32(3);
                        arrPadre[4] = rdr.GetInt32(4);
                        elencoPadri.Add(arrPadre);
                    }
                    rdr.Close();
                    for (int i = 0; i < elencoPadri.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO processipadrifigli(task, revTask, padre, revPadre, variante, posx, posy) VALUES("
                            + this.processID.ToString() + ", " + (this.revisione + 1).ToString() + ", " + elencoPadri[i][0].ToString()
                            + ", " + elencoPadri[i][1].ToString() + ", " + elencoPadri[i][2].ToString() + ", " + elencoPadri[i][3].ToString()
                                + ", " + elencoPadri[i][4].ToString() + ")";
                        cmd.ExecuteNonQuery();

                    }

                    List<int[]> elencoPrecedenti = new List<int[]>();
                    cmd.CommandText = "SELECT prec, revPrec, variante, relazione FROM precedenzeprocessi WHERE succ = "
                        + this.processID.ToString() + " AND revSucc = " + this.revisione.ToString();
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        int[] arrPrec = new int[4];
                        arrPrec[0] = rdr.GetInt32(0);
                        arrPrec[1] = rdr.GetInt32(1);
                        arrPrec[2] = rdr.GetInt32(2);
                        arrPrec[3] = rdr.GetInt32(3);
                        elencoPrecedenti.Add(arrPrec);
                    }
                    rdr.Close();
                    for (int i = 0; i < elencoPrecedenti.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES("
                            + elencoPrecedenti[i][0].ToString() + ", "
                            + elencoPrecedenti[i][1].ToString() + ", "
                            + this.processID.ToString() + ", "
                            + (this.revisione+1).ToString() + ", "
                            + elencoPrecedenti[i][2].ToString() + ", "
                            + elencoPrecedenti[i][3].ToString()
                            + ")";
                        cmd.ExecuteNonQuery();
                    }

                    List<int[]> elencoSuccessivi = new List<int[]>();
                    cmd.CommandText = "SELECT succ, revSucc, variante, relazione FROM precedenzeprocessi WHERE prec = "
                        + this.processID.ToString() + " AND revPrec = " + this.revisione.ToString();
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        int[] arrSucc = new int[4];
                        arrSucc[0] = rdr.GetInt32(0);
                        arrSucc[1] = rdr.GetInt32(1);
                        arrSucc[2] = rdr.GetInt32(2);
                        arrSucc[3] = rdr.GetInt32(3);
                        elencoSuccessivi.Add(arrSucc);
                    }
                    rdr.Close();
                    for (int i = 0; i < elencoSuccessivi.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES("
                            + this.processID.ToString() + ", "
                            + (this.revisione+1).ToString() + ", "
                            + elencoSuccessivi[i][0].ToString() + ", "
                            + elencoSuccessivi[i][1].ToString() + ", "
                            + elencoSuccessivi[i][2].ToString() + ", "
                            + elencoSuccessivi[i][3].ToString()
                            + ")";
                        cmd.ExecuteNonQuery();
                    }


                    trans.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    trans.Rollback();
                    rt = false;
                }
                conn.Close();
            }
            else
            {
                  rt = false;
            }
            return rt;
        }

        public bool buildNewRevisionCopy()
        {
            bool rt = false;
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmdRdr = conn.CreateCommand();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction trans = conn.BeginTransaction();
                MySqlDataReader rdr;
                cmd.Transaction = trans;

                try
                {
                    // Copio il processo originale
                    cmd.CommandText = "INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, ProcessoPadre, revPadre, isVSM, posx, posy, attivo) " +
                            "VALUES(" + this.processID + ", " + (this.revisione + 1).ToString() + ", '" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', '" + this.processName + "', '" + this.processDescription +
                            "', " + this.processoPadre.ToString() + ", " + this.revPadre.ToString() + " , " + isVSM.ToString() + ", " + this.posX.ToString() + ", " + this.posY.ToString() + ", 1)";
                    cmd.ExecuteNonQuery();

                    // Disattivo la revisione precedente
                    cmd.CommandText = "UPDATE processo SET attivo = 0 WHERE processID = " + this.processID.ToString() + " AND revisione = " + this.revisione.ToString();
                    cmd.ExecuteNonQuery();
                    
                    // Copio i KPIs

                    // Trovo l'id massimo
                    cmdRdr.CommandText = "SELECT MAX(id) FROM kpi_description";
                    int maxKPI;
                    rdr = cmdRdr.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        maxKPI = rdr.GetInt32(0) + 1;
                    }
                    else
                    {
                        maxKPI = 0;
                    }
                    rdr.Close();
                    cmdRdr.CommandText = "SELECT id FROM kpi_description "
                        + " WHERE idprocesso = " + this.processID.ToString() + " AND revisione = " + this.revisione.ToString();
                    rdr = cmdRdr.ExecuteReader();
                    List<Kpi> KPIS = new List<Kpi>();
                    while (rdr.Read())
                    {
                        KPIS.Add(new Kpi(rdr.GetInt32(0)));
                    }
                    rdr.Close();
                    for(int j = 0; j < KPIS.Count; j++)
                    {
                        cmd.CommandText = "INSERT INTO kpi_description(id, name, description, idprocesso, revisione, attivo, baseval) "
                            + "VALUES(" + maxKPI.ToString() + ", '" + KPIS[j].name + "', '" + KPIS[j].description + "',"
                            + this.processID.ToString() + ", " + (this.revisione + 1).ToString() + ", 1, " + KPIS[j].baseVal.ToString() + ")";
                        cmd.ExecuteNonQuery();
                        maxKPI++;
                    }
                    
                    // COPIO LE VARIANTI
                    // Associo questo processo alle varianti cui appartiene il padre.
                    cmdRdr.CommandText = "SELECT variante FROM variantiprocessi WHERE processo = " + this.processID.ToString()
                        + " AND revProc = " + this.revisione.ToString();
                    rdr = cmdRdr.ExecuteReader();
                    List<int> variantiElenco = new List<int>();
                    while (rdr.Read())
                    {
                        variantiElenco.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                    for (int i = 0; i < variantiElenco.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO variantiprocessi(variante, processo, revProc) VALUES("
                            + variantiElenco[i].ToString() + ", " + this.processID.ToString() + ", " + (this.revisione + 1).ToString() + ")";
                        cmd.ExecuteNonQuery();
                    }

                    // Copio le relazioni di precedenza del processo per variante
                    cmdRdr.CommandText = "SELECT prec, revPrec, variante, relazione FROM precedenzeprocessi "
                        + " WHERE succ = " + this.processID.ToString() + " AND revSucc = " + this.revisione.ToString();
                    rdr = cmdRdr.ExecuteReader();
                    List<int>[] elencoPrecedenti = new List<int>[4];
                    elencoPrecedenti[0] = new List<int>();
                    elencoPrecedenti[1] = new List<int>();
                    elencoPrecedenti[2] = new List<int>();
                    elencoPrecedenti[3] = new List<int>();
                    while (rdr.Read())
                    {
                        elencoPrecedenti[0].Add(rdr.GetInt32(0));
                        elencoPrecedenti[1].Add(rdr.GetInt32(1));
                        elencoPrecedenti[2].Add(rdr.GetInt32(2));
                        elencoPrecedenti[3].Add(rdr.GetInt32(3));
                    }
                    rdr.Close();
                    for (int j = 0; j < elencoPrecedenti[0].Count; j++)
                    {
                        cmd.CommandText = "INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES "
                        + "(" + elencoPrecedenti[0][j].ToString() + ", " + elencoPrecedenti[1][j].ToString()
                        + ", " + this.processID.ToString() + ", " + (this.revisione+1).ToString()
                        + ", " + elencoPrecedenti[2][j].ToString()
                        + ", " + elencoPrecedenti[3][j].ToString() + ")";
                        cmd.ExecuteNonQuery();
                    }

                    // Copio le relazioni con i successivi per variante
                    cmdRdr.CommandText = "SELECT succ, revSucc, variante, relazione FROM precedenzeprocessi "
                        + "WHERE prec = " + this.processID.ToString() + " AND revPrec = " + this.revisione.ToString();
                    rdr = cmdRdr.ExecuteReader();
                    List<int>[] elencoSuccessivi = new List<int>[4];
                    elencoSuccessivi[0] = new List<int>();
                    elencoSuccessivi[1] = new List<int>();
                    elencoSuccessivi[2] = new List<int>();
                    elencoSuccessivi[3] = new List<int>();
                    while (rdr.Read())
                    {
                        elencoSuccessivi[0].Add(rdr.GetInt32(0));
                        elencoSuccessivi[1].Add(rdr.GetInt32(1));
                        elencoSuccessivi[2].Add(rdr.GetInt32(2));
                        elencoSuccessivi[3].Add(rdr.GetInt32(3));
                    }
                    rdr.Close();
                    for (int j = 0; j < elencoSuccessivi[0].Count; j++)
                    {
                        cmd.CommandText = "INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES "
                            + "(" + this.processID.ToString() + ", " + (this.revisione+1).ToString()
                            + ", " + elencoSuccessivi[0][j].ToString() + ", " + elencoSuccessivi[1][j].ToString()
                            + ", " + elencoSuccessivi[2][j].ToString()
                            + ", " + elencoSuccessivi[3][j].ToString() + ")";
                        cmd.ExecuteNonQuery();
                    }


                    trans.Commit();
                    rt = true;

                }
                catch(Exception er)
                {
                    err = "Creazione nuovo e disattivazione vecchio processo: " + er.Message + "<br/>";
                    trans.Rollback();
                    rt = false;
                }
                conn.Close();

                if (rt == true)
                {
                    // Copio i figli di questo processo, con i loro KPI e le loro varianti
                    rt = copiaFigli(new processo(this.processID, (this.revisione + 1)));
                }
            }
            return rt;
        }

        // Copia i figli del processo corrente, con i loro KPIs e varianti sotto al processo newPrc
        protected bool copiaFigli(processo newPrc)
        {
            bool rt = false;
            // Se i processi sono consistenti, e se this è diverso da newPrc
            if (this.processID != -1 && newPrc.processID != -1 && (this.processID != newPrc.processID || this.revisione != newPrc.revisione))
            {
                this.loadFigli();
                List<int[]> idOldNew = new List<int[]>();

                // Copio i processi e ne tengo le associazioni con il vecchio id
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlCommand cmdRdr = conn.CreateCommand();
                MySqlDataReader rdr;
                cmd.Transaction = trans;
                bool resProc = false;
                try
                {
                    List<int[]> elencoFigli = new List<int[]>();
                    cmdRdr.CommandText = "SELECT processID, revisione FROM processo WHERE processoPadre = " + this.processID.ToString()
                        + " AND revPadre = " + this.revisione.ToString();
                    rdr = cmdRdr.ExecuteReader();
                    while (rdr.Read())
                    {
                        int[] procFigli = new int[2];
                        procFigli[0] = rdr.GetInt32(0);
                        procFigli[1] = rdr.GetInt32(1);
                        elencoFigli.Add(procFigli);
                    }
                    rdr.Close();

                    for (int i = 0; i < elencoFigli.Count; i++)
                    {
                        MySqlCommand findMax = conn.CreateCommand();
                        cmd.CommandText = "SELECT MAX(processID) FROM processo";
                        rdr = cmd.ExecuteReader();
                        int newID = 0;
                        if (rdr.Read() && !rdr.IsDBNull(0))
                        {
                            newID = rdr.GetInt32(0) + 1;
                        }
                        rdr.Close();
                        processo figlio = new processo(elencoFigli[i][0], elencoFigli[i][1]);
                        cmd.CommandText = "INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, processoPadre, "
                        + "revPadre, isVSM, posx, posy, attivo) VALUES(" + newID + ", 0, '" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                        + "', '" + figlio.processName + "', '" + figlio.processDescription + "', "
                        + newPrc.processID.ToString() + ", " + newPrc.revisione.ToString()
                        + ", " + figlio.isVSM.ToString() + ", " + figlio.posX.ToString() + ", "
                        + figlio.posY.ToString() + ", " + figlio.ToString() + ")";
                        cmd.ExecuteNonQuery();

                        int[] idS = new int[2];
                        idS[0] = figlio.processID;
                        idS[1] = newID;
                        idOldNew.Add(idS);


                        // Copio i KPIs
                        this.subProcessi[i].loadKPIs();
                        for (int h = 0; h < this.subProcessi[i].numKPIs; h++)
                        {
                            cmd.CommandText = "SELECT MAX(id) FROM kpi_description";
                            int maxKPI = 0;
                            rdr = cmd.ExecuteReader();
                            if (rdr.Read() && !rdr.IsDBNull(0))
                            {
                                maxKPI = rdr.GetInt32(0) + 1;
                            }
                            rdr.Close();
                            cmd.CommandText = "INSERT INTO kpi_description(id, name, description, idprocesso, revisione, attivo, baseval)"
                                + " VALUES(" + maxKPI.ToString() + ", '" + this.subProcessi[i].KPIs[h].name + "', '"
                                + this.subProcessi[i].KPIs[h].description + "', " + newID.ToString() + ", 0, 1, "
                                + this.subProcessi[i].KPIs[h].baseVal.ToString() + ")";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    resProc = true;
                    trans.Commit();
                }
                catch(Exception er)
                {
                    err = er.Message;
                    resProc = false;
                    trans.Rollback();
                }

                if (resProc == true)
                {
                    // Se ho copiato correttamente i processi, ora parto a copiare dalle varianti
                    err += "Ok, ora verifico le varianti<br/>";
                    List<variante> varFigli = new List<variante>();
                    cmd.CommandText = "SELECT DISTINCT(variantiprocessi.variante) FROM processo AS padre INNER JOIN processo AS figlio ON "
                    + "(padre.processID = figlio.processoPadre AND padre.revisione = figlio.revpadre) INNER JOIN variantiprocessi "
                    + " ON(variantiprocessi.processo = figlio.processID AND variantiprocessi.revProc = figlio.revisione) "
                    + " WHERE padre.processID = " + this.processID.ToString() + " AND padre.revisione = " + this.revisione.ToString();
                    MySqlDataReader rdrVars = cmd.ExecuteReader();
                    while (rdrVars.Read())
                    {
                        varFigli.Add(new variante(rdrVars.GetInt32(0)));
                    }
                    rdrVars.Close();

                    trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                    try
                    {
                        for (int i = 0; i < varFigli.Count; i++)
                        {
                            List<int[]> idPrecSuccOld = new List<int[]>();
                            variante var = new variante(varFigli[i].idVariante);
                            var.loadProcessi();
                            for (int z = 0; z < var.processi.Count; z++)
                            {
                                // Aggiungo l'associazione nuovo processo - variante
                                // Prima cosa: ricerco l'ID del nuovo processo creato, corrispondente al vecchio
                                int newID = -1;
                                for (int w = 0; w < idOldNew.Count; w++)
                                {
                                    if (var.processi[z].processID == idOldNew[w][0])
                                    {
                                        newID = idOldNew[w][1];
                                    }
                                }
                                if (newID != -1)
                                {
                                    cmd.CommandText = "INSERT INTO variantiprocessi(variante, processo, revProc) VALUES ("
                                        + var.idVariante + ", " + newID + ", 0)";
                                    cmd.ExecuteNonQuery();
                                }

                                // Carico i vincoli di precedenza in un array
                                var.processi[z].loadPrecedenti(var);

                                for (int w = 0; w < var.processi[z].processiPrec.Count; w++)
                                {
                                    idPrecSuccOld.Add(new int[3] { var.processi[z].processiPrec[w], var.processi[z].processID, var.processi[z].relazionePrec[w].relationID });
                                }
                                var.processi[z].loadSuccessivi(var);
                                for (int w = 0; w < var.processi[z].processiSucc.Count; w++)
                                {
                                    idPrecSuccOld.Add(new int[3] { var.processi[z].processID, var.processi[z].processiSucc[w], var.processi[z].relazioneSucc[w].relationID });
                                }
                            }

                            // Scremo la lista idPrecSuccOld dai duplicati
                            List<int[]> idPrecSuccOK = new List<int[]>();
                            for (int z = 0; z < idPrecSuccOld.Count; z++)
                            {
                                bool found = false;
                                for (int w = 0; w < idPrecSuccOK.Count; w++)
                                {
                                    if (idPrecSuccOld[z][0] == idPrecSuccOK[w][0] && idPrecSuccOld[z][1] == idPrecSuccOK[w][1])
                                    {
                                        found = true;
                                    }
                                }
                                if (found == false)
                                {
                                    int[] idS = new int[3];
                                    idS[0] = idPrecSuccOld[z][0];
                                    idS[1] = idPrecSuccOld[z][1];
                                    idS[2] = idPrecSuccOld[z][2];
                                    idPrecSuccOK.Add(idS);
                                }
                            }

                            // Inserisco le precedenze per i nuovi processi creati
                            for (int z = 0; z < idPrecSuccOK.Count; z++)
                            {
                                int newPrec = -1;
                                int newSucc = -1;
                                for (int w = 0; w < idOldNew.Count; w++)
                                {
                                    if (idPrecSuccOK[z][0] == idOldNew[w][0])
                                    {
                                        newPrec = idOldNew[w][1];
                                    }
                                    if (idPrecSuccOK[z][1] == idOldNew[w][0])
                                    {
                                        newSucc = idOldNew[w][1];
                                    }
                                }

                                // Se li ho trovati, aggiungo la relazione di precedenza per la variante
                                if (newPrec != -1 && newSucc != -1)
                                {
                                    cmd.CommandText = "INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES ("
                                        + newPrec + ", 0, " + newSucc + ", 0, " + var.idVariante + ", " + idPrecSuccOK[z][2] + ")";
                                    cmd.ExecuteNonQuery();
                                }

                            }
                        }

                        trans.Commit();
                        rt = true;
                    }
                    catch
                    {
                        rt = false;
                        trans.Rollback();
                    }
                    
                }
                conn.Close();
                for (int i = 0; i < idOldNew.Count; i++)
                {
                    this.subProcessi[i].copiaFigli(new processo(idOldNew[i][1]));
                }
            }
            return rt;
        }

        public bool linkProcessoVariante(TaskVariante prc)
        {
            bool rt = false;
            if (prc.Task != null && prc.variant != null && this.processID!=-1)
            {
                if (prc.Task.processID != -1 && prc.variant.idVariante != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO processipadrifigli(task, revTask, padre, revPadre, variante) VALUES("
                        + prc.Task.processID.ToString() + ", " + prc.Task.revisione.ToString() + ", "
                        + this.processID.ToString() + ", " + this.revisione.ToString() + ", " + prc.variant.idVariante.ToString() + ")";
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        rt = true;
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        rt = false;
                        log = ex.Message;
                        tr.Rollback();
                    }

                    conn.Close();
                }
            }
            return rt;
        }
    
        // Trova i prodotti in cui viene eseguito questo task
        public List<ProcessoVariante> ImplosioneProdotti;
        public void loadImplosioneProdotti()
        {
            this.ImplosioneProdotti = new List<ProcessoVariante>();
            if (this.processID != -1 && this.revisione != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT productionplan.processo, productionplan.revisione, productionplan.variante FROM "
                    + "productionplan INNER JOIN tasksproduzione ON (productionplan.id = tasksproduzione.idArticolo AND "
                    + " productionplan.anno = tasksproduzione.annoArticolo) "
                    + " WHERE tasksproduzione.origTask = " + this.processID.ToString()
                    + " AND tasksproduzione.revOrigTask = " + this.revisione.ToString();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProcessoVariante prcv = new ProcessoVariante(new processo(rdr.GetInt32(0), rdr.GetInt32(1)), new variante(rdr.GetInt32(2)));
                    prcv.loadReparto();
                    prcv.process.loadFigli(prcv.variant);
                    this.ImplosioneProdotti.Add(prcv);
                }
                rdr.Close();

                conn.Close();
            }
        }
    
    }

 public class NearTask
    {
        public int NearTaskID { get; set; }
        public String NearTaskName { get; set; }
        public int ConstraintType { get; set; }
        public String ConstraintTypeDesc { get; set; }

        public NearTask()
        {
            this.NearTaskID = -1;
            this.NearTaskName = "";
            this.ConstraintType = -1;
            this.ConstraintTypeDesc = "";
        }
    }

    public class ElencoProcessi
    {
        private List<processo> _Elenco;
        public List<processo> Elenco
        {
            get
            {
                return this._Elenco;
            }
        }

        public ElencoProcessi()
        {
            this._Elenco = new List<processo>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processo.processID, processo.revisione FROM processo "
                + " WHERE processo.attivo = 1 ORDER BY processo.name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Elenco.Add(new processo(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public List<int[]> FindByName(String nome)
        {
            List<int[]> elencoFindings = new List<int[]>();
            for (int i = 0; i < this.Elenco.Count; i++)
            {
                if (this.Elenco[i].processName == nome)
                {
                    int[] procFound = new int[2];
                    procFound[0] = this.Elenco[i].processID;
                    procFound[1] = this.Elenco[i].revisione;
                    elencoFindings.Add(procFound);
                }
            }
            return elencoFindings;
        }
    }

    public class elencoVarianti
    {
        private int _numVarianti;
        public int numVarianti
        {
            get { return this._numVarianti; }
        }
        private variante[] _elenco;
        public variante[] elenco
        {
            get { return this._elenco; }
        }

        public elencoVarianti()
        {
            string strSQL = "SELECT COUNT(*) FROM varianti";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(strSQL, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._numVarianti = rdr.GetInt32(0);
            }
            else
            {
                this._numVarianti = 0;
            }
            rdr.Close();
            this._elenco = new variante[this._numVarianti];
            strSQL = "SELECT idVariante FROM varianti ORDER BY idVariante";
            cmd = new MySqlCommand(strSQL, conn);
            rdr = cmd.ExecuteReader();
            int cont = 0;
            while (rdr.Read() && cont < this._numVarianti)
            {
                this._elenco[cont] = new variante(rdr.GetInt32(0));
                cont++;
            }
            conn.Close();
        }
    }

    public class variante
    {
        public String log;
        private int _idVariante;
        public int idVariante
        {
            get { return this._idVariante; }
        }

        private String _nomeVariante;
        public String nomeVariante
        {
            get { return this._nomeVariante; }
            set
            {
                if (this.idVariante != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                    cmd.Connection = conn;
                    try
                    {
                        cmd.CommandText = "UPDATE varianti SET nomeVariante = '" + value + "' WHERE idVariante = " + this.idVariante;
                        cmd.ExecuteNonQuery();
                        trans.Commit();
                        this._nomeVariante = value;
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                    conn.Close();
                }
            }
        }
        private String _descrizioneVariante;
        public String descrizioneVariante
        {
            get { return this._descrizioneVariante; }
            set
            {
                if (this.idVariante != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                    cmd.Connection = conn;
                    try
                    {
                        cmd.CommandText = "UPDATE varianti SET descVariante = '" + value + "' WHERE idVariante = " + this.idVariante;
                        cmd.ExecuteNonQuery();
                        trans.Commit();
                        this._descrizioneVariante = value;
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public variante()
        {
            this._idVariante = -1;
            this._nomeVariante = "";
            this._descrizioneVariante = "";
        }

        public variante(int varID)
        {
            if (varID >= 0)
            {
                String strSQL = "SELECT idvariante, nomeVariante, descVariante FROM varianti WHERE idvariante = " + varID.ToString();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._idVariante = rdr.GetInt32(0);
                    this._nomeVariante = rdr.GetString(1);
                    this._descrizioneVariante = rdr.GetString(2);
                }
                else
                {
                    this._idVariante = -1;
                    this._nomeVariante = "";
                    this._descrizioneVariante = "";
                }
                conn.Close();
            }
            else
            {
                this._idVariante = -1;
                this._nomeVariante = "";
                this._descrizioneVariante = "";
            }
        }

        private int _numProcessi;
        public int numProcessi
        {
            get { return this._numProcessi; }
        }

        private List<processo> _processi;
        public List<processo> processi
        {
            get { return this._processi; }
        }

        public bool loadProcessi()
        {
            bool rt = false;
            this._processi = new List<processo>();
            if (this.idVariante >= 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                string strSQL = "SELECT COUNT(processo) FROM variantiprocessi INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante)"
                    + " WHERE varianti.idvariante = " + this.idVariante.ToString();
                MySqlCommand cmd = new MySqlCommand (strSQL, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._numProcessi = rdr.GetInt32(0);
                }
                else
                {
                    this._numProcessi = 0;
                }
                //this._processi = new int[this.numProcessi];
                rdr.Close();
                strSQL = "SELECT processo, revProc FROM variantiprocessi INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante)"
                    + " WHERE varianti.idvariante = " + this.idVariante.ToString();
                cmd = new MySqlCommand(strSQL, conn);
                rdr = cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read() && !rdr.IsDBNull(0) && !rdr.IsDBNull(1))
                {
                    this._processi.Add(new processo(rdr.GetInt32(0), rdr.GetInt32(1)));
                    i++;
                }
                conn.Close();
            }
            return rt;
        }

        public bool DeleteLinkToProcesso(processo prc)
        {
            bool ret = false;
            this.loadProcessi();
            bool found = false;
            for (int i = 0; i < this.numProcessi; i++)
            {
                if (this.processi[i].processID == prc.processID)
                {
                    found = true;
                }
            }

            ProcessoVariante prcVar = new ProcessoVariante(prc, new variante(this.idVariante));
            prcVar.loadReparto();
            prcVar.process.loadFigli(prcVar.variant);
            
            if (prcVar.process.subProcessi.Count == 0)
            {
                found = true;
            }
            else
            {
                found = false;
            }

            if (found == true)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                try
                {
                    cmd.CommandText = "DELETE FROM variantiprocessi WHERE variante = " + this.idVariante.ToString()
                        + " AND processo = " + prc.processID.ToString()
                        + " AND revProc = " + prc.revisione.ToString();
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                    ret = false;
                }

                //this.delete();
                conn.Close();
            }
            return ret;
        }

        public bool delete()
        {
            bool rt = false;
            if (this.idVariante >= 0)
            {
                this.loadProcessi();
                if (this.numProcessi == 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.CommandText = "DELETE FROM varianti WHERE idvariante = " + this.idVariante.ToString();
                        cmd.ExecuteNonQuery();
                        rt = true;
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        rt = false;
                        tr.Rollback();
                    }
                    conn.Close();
                    rt = true;
                }
                else
                {
                    //log += "Numero di sub-processi > 0<br/>";
                    rt = false;
                }
            }
            return rt;
        }

        /* Returns:
         * -1 if some error
         * idVariante if succesfully created
         */
        public int add(String nome, String desc)
        {
            int ret = -1;
            
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(idvariante) FROM varianti";
            cmd.Connection = conn;
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                ret = rdr.GetInt32(0) + 1;
            }
            else
            {
                ret = 0;
            }
            rdr.Close();

            
            MySqlTransaction trans = conn.BeginTransaction();
            cmd.CommandText = "INSERT INTO varianti(idVariante, nomeVariante, descVariante) VALUES(" + ret.ToString() + ", '" + nome + "', '" + desc + "')";
            cmd.Transaction = trans;
            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                ret = -1;
            }
            conn.Close();
            return ret;
        }
    }

    public class ProcessoVariante
    {
        public String log;

        //private String _IDCombinato;
        public String IDCombinato
        {
            get { return this.process.processID.ToString() + "," + this.variant.idVariante.ToString(); }
        }

        public String IDCombinato2
        {
            get { return this.process.processID.ToString() + "/" + this.process.revisione.ToString() + "/" + this.variant.idVariante.ToString(); }
        }

        public String NomeCombinato
        {
            get { return this.process.processName + " - " + this.variant.nomeVariante; }
        }

        private processo _process;
        public processo process
        {
            get { return this._process; }
        }

        private variante _variant;
        public variante variant
        {
            get { return this._variant; }
        }

        private int _MeasurementUnitID;
        public int MeasurementUnitID
        {
            get
            {
                return this._MeasurementUnitID;
            }
            set
            {
                if(this.process!=null && this.process.processID!=-1 && this.variant!=null && this.variant.idVariante!=-1)
                {
                    MeasurementUnit mu = new MeasurementUnit(value);
                    if(mu.ID!=-1)
                    { 
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE variantiprocessi SET measurementUnit = " + value.ToString()
                            + " WHERE variante = " + this.variant.idVariante.ToString()
                            + " AND processo = " + this.process.processID.ToString();
                        MySqlTransaction tr = conn.BeginTransaction();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tr.Commit();
                            this._MeasurementUnitID = value;
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
        }

        public MeasurementUnit measurementUnit;

        private Reparto _RepartoProduttivo;
        public Reparto RepartoProduttivo
        {
            get { return this._RepartoProduttivo; }
        }

        private List<Reparto> _RepartiProduttivi;
        public List<Reparto> RepartiProduttivi
        {
            get { return this._RepartiProduttivi; }
        }

        // Restituisce l'ultimo reparto utilizzato per produrre il prodotto corrente
        public Reparto UltimoRepartoUtilizzato
        {
            get
            {
                Reparto rp = new Reparto();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT reparto FROM productionplan WHERE processo = "
                    + this.process.processID.ToString()
                    + " AND revisione = " + this.process.revisione.ToString()
                    + " AND variante = " + this.variant.idVariante.ToString()
                    + " ORDER BY anno DESC, id DESC";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    rp = new Reparto(rdr.GetInt32(0));
                }
                rdr.Close();
                conn.Close();
                return rp;
            }
        }

        public List<ModelParameter> Parameters;

        public ProcessoVariante(processo prc, variante vr)
        {
            // Controllo se la variante appartiene al processo (esiste almeno un figlio che ha questa variante)
            bool found = false;
            this.Parameters = new List<ModelParameter>();
            this._ExternalID = "";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processo.processID, variantiprocessi.ExternalID, variantiprocessi.measurementUnit FROM processo INNER JOIN variantiprocessi ON(variantiprocessi.processo = processo.processID "
                + " AND processo.revisione = variantiprocessi.revProc) WHERE variantiprocessi.variante = " + vr.idVariante.ToString() +
                " AND processo.processID = " + prc.processID.ToString() + " AND processo.revisione = " + prc.revisione.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                found = true;
            }
            

            if (found == true)
            {
                this._process = prc;
                this._variant = vr;
                if(!rdr.IsDBNull(1))
                { 
                this._ExternalID = rdr.GetString(1);
                }
                this._MeasurementUnitID = rdr.GetInt32(2);
                //this.loadReparto();
                //prc.loadFigli(vr);
            }
            else
            {
                this._process = null;
                this._variant = null;
                this._ExternalID = "";
                this._MeasurementUnitID = -1;
            }
            rdr.Close();
            conn.Close();
        }

        public ProcessoVariante(String ExternalID)
        {
            // Controllo se la variante appartiene al processo (esiste almeno un figlio che ha questa variante)
            bool found = false;
            this.Parameters = new List<ModelParameter>();
            this._ExternalID = "";
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processo.processID, processo.revisione, variantiprocessi.variante, variantiprocessi.measurementUnit FROM processo INNER JOIN variantiprocessi ON(variantiprocessi.processo = processo.processID "
                + " AND processo.revisione = variantiprocessi.revProc) WHERE variantiprocessi.ExternalID = '" + ExternalID + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                found = true;
            }


            if (found == true)
            {
                this._process = new processo(rdr.GetInt32(0), rdr.GetInt32(1));
                this._variant = new variante(rdr.GetInt32(2));
                    this._ExternalID = ExternalID;
                this._MeasurementUnitID = rdr.GetInt32(3);
            }
            else
            {
                this._process = null;
                this._variant = null;
                this._ExternalID = "";
                this._MeasurementUnitID = -1;
            }
            rdr.Close();
            conn.Close();
        }

        public void loadReparto()
        {
            this._RepartiProduttivi = new List<Reparto>();
            if (this.process != null && this.variant != null && this.process.processID != -1 && this.variant.idVariante != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idReparto FROM repartiprocessi WHERE processID = " + this.process.processID.ToString()
                    + " AND revisione = " + this.process.revisione.ToString() + " AND variante = " + this.variant.idVariante.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this._RepartoProduttivo = new Reparto(rdr.GetInt32(0));
                    this._RepartiProduttivi.Add(new Reparto(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._RepartoProduttivo = null;
            }
        }

        public bool AddReparto(Reparto rp)
        {
            bool rt = false;
            if (this.process != null && this.variant != null)
            {
                this.loadReparto();
                bool trovato = false;
                for (int i = 0; i < this.RepartiProduttivi.Count; i++)
                {
                    if (this.RepartiProduttivi[i].id == rp.id)
                    {
                        trovato = true;
                    }
                }
                if (trovato == false)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO repartiprocessi(idReparto, processID, revisione, variante) VALUES("
                        + rp.id.ToString() + ", "
                        + this.process.processID.ToString() + ", "
                        + this.process.revisione.ToString() + ", "
                        + this.variant.idVariante.ToString() + ")";
                    try
                    {
                        cmd.ExecuteNonQuery();
                        rt = true;
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        log += ex.Message;
                        rt = false;
                        tr.Rollback();
                    }

                    conn.Close();
                }
                else
                {
                    rt = true;
                }
            }
            return rt;
        }

        public bool DeleteReparto(Reparto rp)
        {
            bool rt = false;
            if (this.process != null && this.variant != null)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM repartiprocessi WHERE idReparto = " + rp.id.ToString()
                    + " AND processID = " + this.process.processID.ToString()
                    + " AND revisione = " + this.process.revisione.ToString()
                    + " AND variante = " + this.variant.idVariante.ToString();
                try
                {
                    rt = true;
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        /*Funzione di copia del PERT */
        public bool CopyTo(processo dest, bool copiaTasks, bool copiaTempiCiclo, bool copiaReparti, bool copiaPostazioni, bool copyParameters, bool copyWorkInstructions)
        {
            bool rt = true;
            if (!(copiaPostazioni == true && copiaReparti == false))
            {
                if (this.process != null && this.variant != null)
                {
                    // Creo una nuova variante sotto il processo!
                    variante var = new variante();
                    int newVarID = var.add("New version - copia da " + this.process.processName + " - " + this.variant.nomeVariante, "New version - copia da " + this.process.processName + " - " + this.variant.nomeVariante);
                    if (newVarID != -1)
                    {
                        var = new variante(newVarID);
                        bool ckAddVar = dest.addVariante(var);
                        if (ckAddVar == true)
                        {
                            ProcessoVariante nuovoProcVar = new ProcessoVariante(dest, var);
                            nuovoProcVar.MeasurementUnitID = this.MeasurementUnitID;
                            nuovoProcVar.loadReparto();
                            nuovoProcVar.process.loadFigli(nuovoProcVar.variant);
                            if (copiaTasks == true)
                            {
                                // Se copio i tasks creandone di nuovi
                                /*
                                 * TO-DO
                                 */
                            }
                            else
                            {
                                // Se mantengo i tasks esistenti
                                this.process.loadFigli(this.variant);
                                bool checkCopiaProcessi = true;
                                for (int i = 0; i < this.process.subProcessi.Count && checkCopiaProcessi ==true; i++)
                                {
                                    log += this.process.subProcessi[i].processID.ToString() + " " + this.process.subProcessi[i].processName + "<br />";
                                    checkCopiaProcessi = nuovoProcVar.process.linkProcessoVariante(new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant));
                                    // Assegno la posizione
                                    this.process.subProcessi[i].setPosX(this.process.subProcessi[i].posX, var);
                                    this.process.subProcessi[i].setPosY(this.process.subProcessi[i].posY, var);
                                }

                                // Copio i vincoli di precedenza

                                bool checkCopiaPrecedenze = true;
                                if (checkCopiaProcessi == true)
                                {
                                    nuovoProcVar.process.loadFigli(var);
                                    for (int i = 0; i < this.process.subProcessi.Count && checkCopiaPrecedenze == true; i++)
                                    {
                                        this.process.subProcessi[i].loadSuccessivi(this.variant);
                                        for (int j = 0; j < this.process.subProcessi[i].processiSucc.Count && checkCopiaPrecedenze ==true; j++)
                                        {
                                            checkCopiaPrecedenze = nuovoProcVar.process.subProcessi[i].addProcessoSuccessivo(new processo(this.process.subProcessi[i].processiSucc[j]), var, this.process.subProcessi[i].ConstraintType[j]);
                                        }
                                    }
                                }

                                // Se copio anche i tempi ciclo (ed il resto è andato a buon fine)
                                if (copiaTempiCiclo == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadTempiCiclo();
                                        for (int j = 0; j < orig.Tempi.Tempi.Count; j++)
                                        {
                                            nuovo.loadTempiCiclo();
                                            log += this.process.subProcessi[i].processID.ToString() + " " 
                                                + nuovoProcVar.variant.idVariante.ToString() + " " 
                                                + orig.Task.processID.ToString() + " "
                                                + orig.Task.processName + " "
                                                + orig.Tempi.Tempi[j].Tempo.TotalMinutes.ToString()
                                                + "<br />";
                                            nuovo.Tempi.Add(orig.Tempi.Tempi[j].NumeroOperatori, orig.Tempi.Tempi[j].Tempo, orig.Tempi.Tempi[j].TempoSetup, orig.Tempi.Tempi[j].Default);
                                        }
                                    }
                                }

                                // Se copio i reparti
                                if (copiaReparti == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    log += "Entro in copiaReparti<br/>";
                                    this.loadReparto();
                                    nuovoProcVar.loadReparto();
                                    for (int i = 0; i < this.RepartiProduttivi.Count; i++)
                                    {
                                        log += "Reparto trovato: " + this.RepartiProduttivi[i].name + "<br />";
                                        if (this.RepartiProduttivi[i] != null && this.RepartiProduttivi[i].id != -1)
                                        {
                                            log += " Aggiungo.<br />";
                                            nuovoProcVar.AddReparto(this.RepartiProduttivi[i]);
                                        }
                                    }
                                }
                                // Se copio anche le postazioni per ogni reparto
                                if (copiaReparti == true && copiaPostazioni == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    log += "<br/>Copio le postazioni.<br/>";
                                    for (int i = 0; i < this.RepartiProduttivi.Count; i++)
                                    {
                                        log += "Reparto: " + this.RepartiProduttivi[i].name + "<br />";
                                        for (int j = 0; j < this.process.subProcessi.Count; j++)
                                        {
                                            log += "   Task " + this.process.subProcessi[j].processID.ToString() + " "
                                                + this.process.subProcessi[j].processName + "<br />";
                                            TaskVariante corrente =new TaskVariante(this.process.subProcessi[j], this.variant);
                                            corrente.loadPostazioni();
                                            TaskVariante eccolo = new TaskVariante(this.process.subProcessi[j], var);
                                            Postazione pst = corrente.CercaPostazione(this.RepartiProduttivi[i]);
                                            if(pst!=null && pst.id!=-1)
                                            {
                                                log += "      Trovata la postazione " + pst.name + "<br />";
                                                this.RepartiProduttivi[i].LinkTaskToPostazione(eccolo, pst);
                                            }
                                        }
                                    }
                                }

                                if(copyParameters && checkCopiaProcessi && checkCopiaPrecedenze)
                                {
                                    this.loadParameters();
                                    nuovoProcVar.loadParameters();
                                    for (int i = 0; i < this.Parameters.Count; i++)
                                    {
                                        nuovoProcVar.addParameter(this.Parameters[i].Name,
                                            this.Parameters[i].Description,
                                            this.Parameters[i].ParameterCategory,
                                            this.Parameters[i].isFixed,
                                            this.Parameters[i].isRequired);
                                    }

                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadParameters();
                                        for (int j = 0; j < orig.Parameters.Count; j++)
                                        {
                                            nuovo.loadParameters();
                                            nuovo.addParameter(orig.Parameters[j].Name,
                                                orig.Parameters[j].Description,
                                                orig.Parameters[j].ParameterCategory,
                                                orig.Parameters[j].isFixed,
                                                orig.Parameters[j].isRequired);
                                        }
                                    }
                                }


                                // CopyWorkInstructions
                                if (copyWorkInstructions && checkCopiaProcessi && checkCopiaPrecedenze)
                                {
                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadWorkInstructions();
                                        for (int j = 0; j < orig.WorkInstructions.Count; j++)
                                        {
                                            //nuovo.loadWorkInstructions();
                                            //nuovo.WorkInstructions.Add(nuovo.WorkInstructions[i]);
                                            KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(orig.WorkInstructions[j].WI.ID, orig.WorkInstructions[j].WI.Version);
                                            int lnkRet = currWI.linkManualToTask(nuovo.Task.processID, nuovo.Task.revisione, nuovo.variant.idVariante,
                                                orig.WorkInstructions[j].InitialDate,
                                                orig.WorkInstructions[j].ExpiryDate,
                                                orig.WorkInstructions[j].Sequence,
                                                orig.WorkInstructions[j].IsActive
                                                );
                                        }
                                    }
                                }

                                if (checkCopiaProcessi == false || checkCopiaPrecedenze == false)
                                {
                                    rt = false;
                                }
                            }
                        }
                        else
                        {
                            rt = false;
                            log += "Errore nell'associare la variante al processo.<br />";
                        }
                    }
                    else
                    {
                        rt = false;
                        log += "Errore durante la creazione della nuova variante.<br />";
                    }
                }
                else
                {
                    log = "Errore: processo o variante è null!<br />";
                    rt = false;
                }
            }
            else
            {
                this.log = "Opzioni illogica per copiaPostazioni e copiaReparti.<br/>";
                    rt = false;
            }
            return rt;
        }

        /*Funzione di copia del PERT
         * overload con inserimento nome e descrizione
         * Restituisce l'id della variante creata
         */
        public int CopyTo(processo dest, String nomeVariante, String descVariante, bool copiaTasks, bool copiaTempiCiclo, bool copiaReparti, bool copiaPostazioni, bool copyParameters, bool copyWorkInstructions)
        {
            bool rt = true;
            int retVarID = -1;
            if (!(copiaPostazioni == true && copiaReparti == false))
            {
                if (this.process != null && this.variant != null)
                {
                    // Creo una nuova variante sotto il processo!
                    variante var = new variante();
                    int newVarID = var.add(nomeVariante, descVariante);
                    retVarID = newVarID;
                    if (newVarID != -1)
                    {
                        var = new variante(newVarID);
                        bool ckAddVar = dest.addVariante(var);
                        if (ckAddVar == true)
                        {
                            ProcessoVariante nuovoProcVar = new ProcessoVariante(dest, var);
                            nuovoProcVar.MeasurementUnitID = this.MeasurementUnitID;
                            nuovoProcVar.loadReparto();
                            nuovoProcVar.process.loadFigli(nuovoProcVar.variant);
                            if (copiaTasks == true)
                            {
                                // Se copio i tasks creandone di nuovi
                                /*
                                 * TO-DO
                                 */
                            }
                            else
                            {
                                // Se mantengo i tasks esistenti
                                this.process.loadFigli(this.variant);
                                bool checkCopiaProcessi = true;
                                for (int i = 0; i < this.process.subProcessi.Count && checkCopiaProcessi == true; i++)
                                {
                                    log += this.process.subProcessi[i].processID.ToString() + " " + this.process.subProcessi[i].processName + "<br />";
                                    checkCopiaProcessi = nuovoProcVar.process.linkProcessoVariante(new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant));
                                    // Assegno la posizione
                                    this.process.subProcessi[i].setPosX(this.process.subProcessi[i].posX, var);
                                    this.process.subProcessi[i].setPosY(this.process.subProcessi[i].posY, var);
                                }

                                // Copio i vincoli di precedenza

                                bool checkCopiaPrecedenze = true;
                                if (checkCopiaProcessi == true)
                                {
                                    nuovoProcVar.process.loadFigli(var);
                                    for (int i = 0; i < this.process.subProcessi.Count && checkCopiaPrecedenze == true; i++)
                                    {
                                        this.process.subProcessi[i].loadSuccessivi(this.variant);
                                        for (int j = 0; j < this.process.subProcessi[i].processiSucc.Count && checkCopiaPrecedenze == true; j++)
                                        {
                                            checkCopiaPrecedenze = nuovoProcVar.process.subProcessi[i].addProcessoSuccessivo(new processo(this.process.subProcessi[i].processiSucc[j]), var, this.process.subProcessi[i].ConstraintType[j]);
                                        }
                                    }
                                }

                                // Se copio anche i tempi ciclo (ed il resto è andato a buon fine)
                                if (copiaTempiCiclo == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadTempiCiclo();
                                        for (int j = 0; j < orig.Tempi.Tempi.Count; j++)
                                        {
                                            nuovo.loadTempiCiclo();
                                            log += this.process.subProcessi[i].processID.ToString() + " "
                                                + nuovoProcVar.variant.idVariante.ToString() + " "
                                                + orig.Task.processID.ToString() + " "
                                                + orig.Task.processName + " "
                                                + orig.Tempi.Tempi[j].Tempo.TotalMinutes.ToString()
                                                + "<br />";
                                            nuovo.Tempi.Add(orig.Tempi.Tempi[j].NumeroOperatori, orig.Tempi.Tempi[j].Tempo, orig.Tempi.Tempi[j].TempoSetup, orig.Tempi.Tempi[j].Default);
                                        }
                                    }
                                }

                                // Se copio i reparti
                                if (copiaReparti == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    log += "Entro in copiaReparti<br/>";
                                    this.loadReparto();
                                    nuovoProcVar.loadReparto();
                                    for (int i = 0; i < this.RepartiProduttivi.Count; i++)
                                    {
                                        log += "Reparto trovato: " + this.RepartiProduttivi[i].name + "<br />";
                                        if (this.RepartiProduttivi[i] != null && this.RepartiProduttivi[i].id != -1)
                                        {
                                            log += " Aggiungo.<br />";
                                            nuovoProcVar.AddReparto(this.RepartiProduttivi[i]);
                                        }
                                    }
                                }
                                // Se copio anche le postazioni per ogni reparto
                                if (copiaReparti == true && copiaPostazioni == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    log += "<br/>Copio le postazioni.<br/>";
                                    for (int i = 0; i < this.RepartiProduttivi.Count; i++)
                                    {
                                        log += "Reparto: " + this.RepartiProduttivi[i].name + "<br />";
                                        for (int j = 0; j < this.process.subProcessi.Count; j++)
                                        {
                                            log += "   Task " + this.process.subProcessi[j].processID.ToString() + " "
                                                + this.process.subProcessi[j].processName + "<br />";
                                            TaskVariante corrente = new TaskVariante(this.process.subProcessi[j], this.variant);
                                            corrente.loadPostazioni();
                                            TaskVariante eccolo = new TaskVariante(this.process.subProcessi[j], var);
                                            Postazione pst = corrente.CercaPostazione(this.RepartiProduttivi[i]);
                                            if (pst != null && pst.id != -1)
                                            {
                                                log += "      Trovata la postazione " + pst.name + "<br />";
                                                this.RepartiProduttivi[i].LinkTaskToPostazione(eccolo, pst);
                                            }
                                        }
                                    }
                                }


                                if (copyParameters && checkCopiaProcessi && checkCopiaPrecedenze)
                                {
                                    this.loadParameters();
                                    nuovoProcVar.loadParameters();
                                    for (int i = 0; i < this.Parameters.Count; i++)
                                    {
                                        nuovoProcVar.addParameter(this.Parameters[i].Name,
                                            this.Parameters[i].Description,
                                            this.Parameters[i].ParameterCategory,
                                            this.Parameters[i].isFixed,
                                            this.Parameters[i].isRequired);
                                    }

                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadParameters();
                                        for (int j = 0; j < orig.Parameters.Count; j++)
                                        {
                                            nuovo.loadParameters();
                                            nuovo.addParameter(orig.Parameters[j].Name,
                                                orig.Parameters[j].Description,
                                                orig.Parameters[j].ParameterCategory,
                                                orig.Parameters[j].isFixed,
                                                orig.Parameters[j].isRequired);
                                        }
                                    }

                                }

                                // CopyWorkInstructions
                                if (copyWorkInstructions && checkCopiaProcessi && checkCopiaPrecedenze)
                                {
                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadWorkInstructions();
                                        for (int j = 0; j < orig.WorkInstructions.Count; j++)
                                        {
                                            KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(orig.WorkInstructions[j].WI.ID, orig.WorkInstructions[j].WI.Version);
                                            int lnkRet = currWI.linkManualToTask(nuovo.Task.processID, nuovo.Task.revisione, nuovo.variant.idVariante,
                                                orig.WorkInstructions[j].InitialDate,
                                                orig.WorkInstructions[j].ExpiryDate,
                                                orig.WorkInstructions[j].Sequence,
                                                orig.WorkInstructions[j].IsActive
                                                );
                                        }
                                    }
                                }

                                if (checkCopiaProcessi == false || checkCopiaPrecedenze == false)
                                {
                                    rt = false;
                                }
                            }
                        }
                        else
                        {
                            rt = false;
                            log += "Errore nell'associare la variante al processo.<br />";
                        }
                    }
                    else
                    {
                        retVarID = -1;
                        rt = false;
                        log += "Errore durante la creazione della nuova variante.<br />";
                    }
                }
                else
                {
                    log = "Errore: processo o variante è null!<br />";
                    rt = false;
                    retVarID = -1;
                }
            }
            else
            {
                this.log = "Opzioni illogica per copiaPostazioni e copiaReparti.<br/>";
                rt = false;
                retVarID = -1;
            }
            return retVarID;
        }

        public void loadParameters()
        {
            this.Parameters = new List<ModelParameter>();
            if(this.process!=null && this.variant!=null && this.process.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT paramID FROM modelparameters WHERE processID = " +
                    this.process.processID.ToString()
                    + " AND processRev = " + this.process.revisione.ToString()
                    + " AND varianteID = " + this.variant.idVariante.ToString()
                    + " ORDER BY sequence, paramname";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.Parameters.Add(new ModelParameter(this.process.processID,
                        this.process.revisione, this.variant.idVariante, rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public Boolean addParameter(String name, String description, ProductParametersCategory category, Boolean isFixed,
            Boolean isRequired)
        {
            Boolean ret = false;
            if (this.process != null && this.variant != null && this.process.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(paramID) FROM ModelParameters WHERE processID = " + this.process.processID.ToString()
                    + " AND processRev = " + this.process.revisione.ToString()
                    + " AND varianteID = " + this.variant.idVariante.ToString();
                int maxID = 0;
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();

                int maxSequence = maxID;

                cmd.CommandText = "INSERT INTO modelparameters(processid, processrev, varianteID, paramID, paramCategory, "
                    + " paramName, paramDescription, isFixed, isRequired, sequence) VALUES("
                    + this.process.processID.ToString() + ", "
                    + this.process.revisione.ToString() + ", "
                    + this.variant.idVariante.ToString() + ", "
                    + maxID.ToString() + ", "
                    + category.ID.ToString() + ", "
                    + "'" + name + "', '" + description + "', " + isFixed.ToString() + ", "
                    + isRequired.ToString() + ", " + maxSequence.ToString() + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
                    ret = false;
                    tr.Rollback();
                }
                rdr.Close();
                conn.Close();
            }
            return ret;
        }

        public Boolean deleteParameter(int paramID)
        {
            Boolean ret = false;
            if (this.process != null && this.variant != null && this.process.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Modelparameters WHERE processID = " + this.process.processID.ToString()
                    + " AND processRev = " + this.process.revisione.ToString()
                    + " AND varianteID = " + this.variant.idVariante.ToString()
                    + " AND paramID = " + paramID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        private String _ExternalID;
        public String ExternalID
        {
            get {return this._ExternalID;
            }
            set {
                if (this.process != null && this.process.processID != -1 && this.variant != null && this.variant.idVariante != -1)
                { 
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                
                    cmd.CommandText = "UPDATE variantiprocessi SET ExternalID ='" + value + "' WHERE variante=" + this.variant.idVariante + " AND processo = " + this.process.processID
                        + " AND revProc = " + this.process.revisione.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._ExternalID = value;
                    }
                    catch
                    {
                    }
                    conn.Close();
                }
            }
        }

        public void loadMeasurementUnit()
        {
            if(this.MeasurementUnitID!=-1)
            {
                this.measurementUnit = new MeasurementUnit(this.MeasurementUnitID);
            }
            else
            {
                this.measurementUnit = null;
            }
        }
    }

    public class ElencoMacroProcessiVarianti
    {
        public String log;

        public List<ProcessoVariante> elenco;

        public ElencoMacroProcessiVarianti()
        {
            macroProcessi macroEl = new macroProcessi();
            elenco = new List<ProcessoVariante>();
            for (int i = 0; i < macroEl.Elenco.Count; i++)
            {
                //log += "MACROPROCESSO: "+ macroEl.Elenco[i].processID.ToString() + "<br/>";
                macroEl.Elenco[i].loadVariantiFigli();
                for (int j = 0; j < macroEl.Elenco[i].variantiFigli.Count; j++)
                {
                    //log += "Variante: " + macroEl.Elenco[i].variantiFigli[j].idVariante.ToString() + "<br/>";
                    elenco.Add(new ProcessoVariante(macroEl.Elenco[i], macroEl.Elenco[i].variantiFigli[j]));
                }
            }
        }

    }

    public class ElencoProcessiVarianti
    {
        // Crea l'elenco di processi e varianti per i figli del processo passato al costruttore

        public List<ProcessoVariante> elencoFigli;

        public ElencoProcessiVarianti(ProcessoVariante proc)
        {
            if (proc.process.processID != -1 && proc.variant.idVariante != -1)
            {
                elencoFigli = new List<ProcessoVariante>();
                for (int i = 0; i < proc.process.subProcessi.Count; i++)
                {
                    proc.process.subProcessi[i].loadVariantiFigli();
                    for (int j = 0; j < proc.process.subProcessi[i].variantiFigli.Count; j++)
                    {
                        elencoFigli.Add(new ProcessoVariante(proc.process.subProcessi[i], proc.process.subProcessi[i].variantiFigli[j]));
                    }
                }
            }
        }

        public ElencoProcessiVarianti()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT variante, processo, revProc FROM variantiprocessi INNER JOIN processo ON (variantiprocessi.processo = processo.processID "
                + "AND processo.revisione = variantiprocessi.revProc) WHERE processo.attivo = true ORDER BY variante, processo.name";
            elencoFigli = new List<ProcessoVariante>();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                elencoFigli.Add(new ProcessoVariante(new processo(rdr.GetInt32(1), rdr.GetInt32(2)), new variante(rdr.GetInt32(0))));
            }
            conn.Close();
        }

        /* Ritorna processivarianti solo per un tipo di processo */
        public ElencoProcessiVarianti(bool isPert)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT variante, processo, revProc FROM variantiprocessi INNER JOIN processo ON (variantiprocessi.processo = processo.processID "
                + "AND processo.revisione = variantiprocessi.revProc) WHERE processo.attivo = true" 
                + " AND processo.isVSM = " + (!isPert).ToString()
                + " ORDER BY variante, processo.name";
            elencoFigli = new List<ProcessoVariante>();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                elencoFigli.Add(new ProcessoVariante(new processo(rdr.GetInt32(1), rdr.GetInt32(2)), new variante(rdr.GetInt32(0))));
            }
            conn.Close();
        }

        /* Ritorna processivarianti solo per un tipo di processo, realizzato per un certo cliente */
        public ElencoProcessiVarianti(bool isPert, Cliente customer)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT variantiprocessi.variante, processo.processID, processo.revisione FROM variantiprocessi INNER JOIN processo ON (variantiprocessi.processo = processo.processID "
                + "AND processo.revisione = variantiprocessi.revProc) INNER JOIN productionplan ON ("
                + "productionplan.processo = processo.processID AND productionplan.revisione = processo.revisione "
                + " AND productionplan.variante = variantiprocessi.variante)"
                + " INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa = commesse.anno) "
                + " WHERE "
                + " commesse.cliente = '" + customer.CodiceCliente + "'"
                +" AND processo.attivo = true"
                + " AND processo.isVSM = " + (!isPert).ToString()
                + " GROUP BY variantiprocessi.variante, processo.processID, processo.revisione"
                + " ORDER BY variante, processo.name";
            elencoFigli = new List<ProcessoVariante>();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                elencoFigli.Add(new ProcessoVariante(new processo(rdr.GetInt32(1), rdr.GetInt32(2)), new variante(rdr.GetInt32(0))));
            }
            conn.Close();
        }
    }

    public class TaskVariante
    {
        public String log;
        private processo _Task;
        public processo Task
        {
            get { return this._Task; }
        }

        private variante _variant;
        public variante variant
        {
            get { return this._variant; }
        }

        public List<TaskWorkInstruction> WorkInstructions;
        public List<TaskWorkInstruction> WorkInstructionsArchive;

        private TempiCiclo _Tempi;
        public TempiCiclo Tempi
        {
            get { return this._Tempi; }
        }

        public List<ModelTaskParameter> Parameters;

        public List<TaskMicrostep> microsteps;

        public TaskVariante(processo prc, variante vr)
        {
            this.Parameters = new List<ModelTaskParameter>();
            this.WorkInstructions = new List<TaskWorkInstruction>();
            this.WorkInstructionsArchive = new List<TaskWorkInstruction>();
            this._DefaultOperators = new List<User>();
            this._Task = prc;
            this.microsteps = new List<TaskMicrostep>();
                this._variant = vr;
                prc.loadFigli(vr);
                this.loadPostazioni();
        }
      

        public bool loadTempiCiclo()
        {
            bool rt = false;
            if (this.Task != null && this.variant != null)
            {
                this._Tempi = new TempiCiclo(this.Task.processID, this.Task.revisione, this.variant.idVariante);
            }
            else
            {
                this._Tempi = null;
                rt = false;
            }
            return rt;
        }

        public bool Delete()
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            try
            {

                cmd.CommandText = "DELETE FROM processipadrifigli WHERE variante = " + this.variant.idVariante.ToString()
                    + " AND task = " + this.Task.processID.ToString() + " AND revTask = " + this.Task.revisione.ToString();
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM precedenzeprocessi WHERE variante = " + this.variant.idVariante.ToString()
                    + " AND ((prec = "+this.Task.processID.ToString()+" AND revPrec = "+this.Task.revisione.ToString()
                    +") OR (succ = "+this.Task.processID.ToString()+" AND revSucc = "+ this.Task.revisione.ToString() + "))";
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch(Exception ex)
            {
                rt = false;
                log = ex.Message;
                tr.Rollback();
            }
            return rt;
        }

        public TimeSpan getDefaultTempoCiclo()
        {
            TimeSpan ret = new TimeSpan(0,0,0);
            if (this.Tempi == null)
            {
                this.loadTempiCiclo();
            }
            for (int i = 0; i < this.Tempi.Tempi.Count; i++)
            {
                if (this.Tempi.Tempi[i].Default)
                {
                    ret = this.Tempi.Tempi[i].Tempo;
                }
            }
            return ret;
        }

        public int getDefaultOperatori()
        {
            int ret = 0;
            if (this.Tempi == null)
            {
                this.loadTempiCiclo();
            }
            for (int i = 0; i < this.Tempi.Tempi.Count; i++)
            {
                if (this.Tempi.Tempi[i].Default)
                {
                    ret = this.Tempi.Tempi[i].NumeroOperatori;
                }
            }
            return ret;
        }

        public TempoCiclo getDefaultTempo()
        {
            TempoCiclo ret = null;
            if (this.Tempi == null)
            {
                this.loadTempiCiclo();
            }
            for (int i = 0; i < this.Tempi.Tempi.Count; i++)
            {
                if (this.Tempi.Tempi[i].Default)
                {
                    ret = this.Tempi.Tempi[i];
                }
            }
            return ret;
        }

        private Postazione _PostazioneDiLavoro;
        public Postazione PostazioneDiLavoro
        {
            get { return this._PostazioneDiLavoro; }
        }

        private List<Postazione> _PostazioniDiLavoro;
        public List<Postazione> PostazioniDiLavoro
        {
            get { return this._PostazioniDiLavoro; }
        }

        private List<User> _DefaultOperators;
        public List<User> DefaultOperators {
            get
            {
                return this._DefaultOperators;
            }
        }

        public void loadPostazioni()
        {
            this._PostazioniDiLavoro = new List<Postazione>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT postazione FROM repartipostazioniattivita WHERE processo = " + this.Task.processID.ToString()
                 + " AND revProc = " + this.Task.revisione.ToString() + " AND variante = " + this.variant.idVariante.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this._PostazioneDiLavoro = new Postazione(rdr.GetInt32(0));
                this._PostazioniDiLavoro.Add(new Postazione(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean deleteLinkPostazione(Postazione p)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM repartipostazioniattivita WHERE "
                 + " processo = " + this.Task.processID.ToString()
                 + " AND revProc = " + this.Task.revisione.ToString() 
                 + " AND variante = " + this.variant.idVariante.ToString()
                 //+ " AND reparto = " + rp.id.ToString()
                 + " AND postazione = " + p.id.ToString();
            try
            {
                cmd.ExecuteNonQuery();
                ret = true;
                tr.Commit();
            }
            catch(Exception ex)
            {
                ret = false;
                log = ex.Message;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public Postazione CercaPostazione(Reparto rp)
        {
            Postazione rt = new Postazione();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT postazione FROM repartipostazioniattivita WHERE processo = " + this.Task.processID.ToString()
                 + " AND revProc = " + this.Task.revisione.ToString() 
                 + " AND variante = " + this.variant.idVariante.ToString()
                 + " AND reparto = " + rp.id.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                rt = new Postazione(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
            return rt;
        }

        public void loadParameters()
        {
            this.Parameters = new List<ModelTaskParameter>();
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT paramID FROM modelTaskparameters WHERE TaskID = " +
                    this.Task.processID.ToString()
                    + " AND TaskRev = " + this.Task.revisione.ToString()
                    + " AND varianteID = " + this.variant.idVariante.ToString()
                    + " ORDER BY sequence, paramname";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.Parameters.Add(new ModelTaskParameter(this.Task.processID,
                        this.Task.revisione, this.variant.idVariante, rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public Boolean addParameter(String name, String description, ProductParametersCategory category, Boolean isFixed,
            Boolean isRequired)
        {
            Boolean ret = false;
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(paramID) FROM ModelTaskParameters WHERE TaskID = " + this.Task.processID.ToString()
                    + " AND TaskRev = " + this.Task.revisione.ToString()
                    + " AND varianteID = " + this.variant.idVariante.ToString();
                int maxID = 0;
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();

                int maxSequence = maxID;

                cmd.CommandText = "INSERT INTO modelTaskparameters(TaskID, TaskRev, varianteID, paramID, paramCategory, "
                    + " paramName, paramDescription, isFixed, isRequired, sequence) VALUES("
                    + this.Task.processID.ToString() + ", "
                    + this.Task.revisione.ToString() + ", "
                    + this.variant.idVariante.ToString() + ", "
                    + maxID.ToString() + ", "
                    + category.ID.ToString() + ", "
                    + "'" + name + "', '" + description + "', " + isFixed.ToString() + ", "
                    + isRequired.ToString() + ", " + maxSequence.ToString() + ")";
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
                    ret = false;
                    tr.Rollback();
                }
                rdr.Close();
                conn.Close();
            }
            return ret;
        }

        public Boolean deleteParameter(int paramID)
        {
            Boolean ret = false;
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM ModelTaskparameters WHERE TaskID = " + this.Task.processID.ToString()
                    + " AND TaskRev = " + this.Task.revisione.ToString()
                    + " AND varianteID = " + this.variant.idVariante.ToString()
                    + " AND paramID = " + paramID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public void loadWorkInstructions()
        {
            this.WorkInstructions = new List<TaskWorkInstruction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT manualID, manualVersion, validityInitialDate, expiryDate, sequence, isActive FROM tasksmanuals WHERE taskID = @TaskId AND taskRev = @TaskRev AND taskVarianti = @variante "
                +" AND isActive = true AND ExpiryDate >= @ExpiryDate";
            cmd.Parameters.AddWithValue("@TaskId", this.Task.processID);
            cmd.Parameters.AddWithValue("@TaskRev", this.Task.revisione);
            cmd.Parameters.AddWithValue("@variante", this.variant.idVariante);
            cmd.Parameters.AddWithValue("@ExpiryDate", DateTime.UtcNow.ToString("yyyy-MM-dd"));

            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                TaskWorkInstruction curr = new TaskWorkInstruction(this.Task.processID, this.Task.revisione, this.variant.idVariante,
                    rdr.GetInt32(0), rdr.GetInt32(1));

                this.WorkInstructions.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }

        public void loadWorkInstructionsArchive()
        {
            this.WorkInstructionsArchive = new List<TaskWorkInstruction>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT manualID, manualVersion FROM tasksmanuals WHERE taskID = @TaskId AND taskRev = @TaskRev AND taskVarianti = @variante "
                + " AND ExpiryDate < @ExpiryDate";
            cmd.Parameters.AddWithValue("@TaskId", this.Task.processID);
            cmd.Parameters.AddWithValue("@TaskRev", this.Task.revisione);
            cmd.Parameters.AddWithValue("@variante", this.variant.idVariante);
            cmd.Parameters.AddWithValue("@ExpiryDate", DateTime.UtcNow.ToString("yyyy-MM-dd"));

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                TaskWorkInstruction curr = new TaskWorkInstruction(this.Task.processID, this.Task.revisione, this.variant.idVariante,
                    rdr.GetInt32(0), rdr.GetInt32(1));
                this.WorkInstructionsArchive.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }

        public void loadDefaultOperators()
        {
            this._DefaultOperators = new List<User>();
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT user FROM taskusermodel WHERE taskid = " + this.Task.processID
                    + " AND taskrev=" + this.Task.revisione
                    + " AND variantid = " + this.variant.idVariante.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this._DefaultOperators.Add(new User(rdr.GetString(0)));
                }
                rdr.Close();
            conn.Close();
            }
        }

        public Boolean addDefaultOperator(String usr)
        {
            Boolean ret = false;
            
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                User curr = new User(usr);
                if(usr!=null && curr.username.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO taskusermodel(taskid, taskrev, variantid, user, exclusive) VALUES("
                        +"@TaskID, @TaskRev, @VariantID, @User, @Exclusive)";
                    cmd.Parameters.AddWithValue("@TaskID", this.Task.processID);
                    cmd.Parameters.AddWithValue("@TaskRev", this.Task.revisione);
                    cmd.Parameters.AddWithValue("@VariantID", this.variant.idVariante);
                    cmd.Parameters.AddWithValue("@User", usr);
                    cmd.Parameters.AddWithValue("@Exclusive", false);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret =true;
                    }
                    catch
                    {
                        tr.Rollback();
                        ret = false;
                    }

                    conn.Close();
                }
            }
            return ret;
            }

        public Boolean deleteDefaultOperator(String usr)
        {
            Boolean ret = false;

            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "DELETE FROM taskusermodel WHERE taskid=@TaskID AND taskrev=@TaskRev AND variantid=@VariantID AND user=@User";
                    cmd.Parameters.AddWithValue("@TaskID", this.Task.processID);
                    cmd.Parameters.AddWithValue("@TaskRev", this.Task.revisione);
                    cmd.Parameters.AddWithValue("@VariantID", this.variant.idVariante);
                    cmd.Parameters.AddWithValue("@User", usr);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        ret = true;
                    }
                    catch
                    {
                        tr.Rollback();
                        ret = false;
                    }

                    conn.Close();
                }
            return ret;
        }

        public void loadTaskMicrosteps()
        {
            this.microsteps = new List<TaskMicrostep>();
            if (this.Task != null && this.variant != null && this.Task.processID!=-1 && this.Task.revisione!=-1 && this.variant.idVariante!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT microstep_id, microstep_rev FROM task_microsteps WHERE "
                    + "taskid=@taskid AND taskrev=@taskrev AND variantid=@variantid "
                    + " ORDER BY sequence";
                cmd.Parameters.AddWithValue("@taskid", this.Task.processID);
                cmd.Parameters.AddWithValue("@taskrev", this.Task.revisione);
                cmd.Parameters.AddWithValue("@variantid", this.variant.idVariante);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.microsteps.Add(new TaskMicrostep(this.Task.processID, this.Task.revisione, this.variant.idVariante, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if microstep correctly added
         * 2 if input is not valid
         * 3 if Task does not exist
         * 4 if error while adding data to database
         */
        public int addMicrostep(String name, String description, int sequence, int cycletime /* seconds */, Char value_or_waste)
        {
            int ret = 0;
            if (this.Task != null && this.variant != null && this.Task.processID != -1 && this.Task.revisione != -1 && this.variant.idVariante != -1)
            {
                if (name.Length < 255 &&
                    (value_or_waste == 'V' || value_or_waste == 'W' || value_or_waste == 'H'))
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {

                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Transaction = tr;
                        // Add microstep
                        cmd.CommandText = "INSERT INTO microsteps(name, description) VALUES(@name, @desc)";
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@desc", description);
                        cmd.ExecuteNonQuery();
                        // Retrieve the ID with LAST_INSERT_ID()
                        cmd.CommandText = "SELECT LAST_INSERT_ID()";
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        rdr.Read();
                        int microstepid = rdr.GetInt32(0);
                        rdr.Close();

                        // Link microstep to the Task
                        cmd.CommandText = "INSERT INTO task_microsteps(taskid, taskrev, variantid, microstep_id, microstep_rev, sequence, cycletime, value_or_waste) "
                            + " VALUES(@taskid, @taskrev, @variantid, @microstep_id, @microstep_rev, @sequence, @cycletime, @value_or_waste)";
                        cmd.Parameters.AddWithValue("@taskid", this.Task.processID);
                        cmd.Parameters.AddWithValue("@taskrev", this.Task.revisione);
                        cmd.Parameters.AddWithValue("@variantid", this.variant.idVariante);
                        cmd.Parameters.AddWithValue("@microstep_id", microstepid);
                        cmd.Parameters.AddWithValue("@microstep_rev", 0);
                        cmd.Parameters.AddWithValue("@sequence", sequence);
                        cmd.Parameters.AddWithValue("@cycletime", cycletime);
                        cmd.Parameters.AddWithValue("@value_or_waste", value_or_waste);
                        cmd.ExecuteNonQuery();

                        ret = 1;

                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        ret = 4;
                        this.log = ex.Message;
                        tr.Rollback();
                    }
                }
                else
                {
                    ret = 2;
                }
            }
            else
            {
                ret = 3;
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if microstep delete successfully
         * 2 if TaskVariante not set
         */
        public int deleteMicrostep(int MicrostepId, int MicrostepReview)
        {
            int ret = 0;
            if (this.Task != null && this.variant != null && this.Task.processID != -1 && this.Task.revisione != -1 && this.variant.idVariante != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "DELETE FROM task_microsteps WHERE taskid=@taskid AND taskrev=@taskrev AND variantid=@variantid "
                        + " AND microstep_id=@microstepid AND microstep_rev=@microsteprev";
                    cmd.Parameters.AddWithValue("@taskid", this.Task.processID);
                    cmd.Parameters.AddWithValue("@taskrev", this.Task.revisione);
                    cmd.Parameters.AddWithValue("@variantid", this.variant.idVariante);
                    cmd.Parameters.AddWithValue("@microstepid", MicrostepId);
                    cmd.Parameters.AddWithValue("@microsteprev", MicrostepReview);

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM microsteps WHERE id=@microstepid AND review=@microsteprev";
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
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
                ret = 2; 
            }
            return ret;
        }
    }

    public class TempoCiclo
    {
        public String log;

        private int _IdProcesso;
        public int IdProcesso
        {
            get { return this._IdProcesso; }
        }

        private int _RevisioneProcesso;
        public int RevisioneProcesso
        {
            get { return this._RevisioneProcesso; }
        }

        private int _Variante;
        public int Variante
        {
            get { return this._Variante; }
        }

        private int _NumeroOperatori;
        public int NumeroOperatori
        {
            get { return this._NumeroOperatori; }
        }

        private TimeSpan _TempoSetup;
        public TimeSpan TempoSetup
        {
            get { return this._TempoSetup; }
        }

        private TimeSpan _Tempo;
        public TimeSpan Tempo
        {
            get { return this._Tempo; }
        }

        private TimeSpan _TempoUnload;
        public TimeSpan TempoUnload
        {
            get { return this._TempoUnload; }
        }

        private bool _Default;
        public bool Default
        {
            get { return this._Default; }
            set
            {
                if (this.NumeroOperatori != -1 && this.IdProcesso != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE tempiciclo SET def = " + value
                        + " WHERE processo = " + this.IdProcesso.ToString()
                        + " AND revisione = " + this.RevisioneProcesso.ToString()
                        + " AND variante = " + this.Variante.ToString()
                        + " AND num_op = " + this.NumeroOperatori.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._Default = value;
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

        public TempoCiclo(int idProc, int revProc, int var, int num_op)
        {
            if (idProc >= 0 && revProc >= 0 && num_op >= 1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT setup, tempo, tunload, def FROM tempiciclo WHERE processo = " + idProc.ToString()
                    + " AND revisione = " + revProc.ToString() + " AND num_op = " + num_op.ToString() + " AND variante = " + var.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._IdProcesso = idProc;
                    this._RevisioneProcesso = revProc;
                    this._Variante = var;
                    this._NumeroOperatori = num_op;
                    this._TempoSetup = rdr.GetTimeSpan(0);
                    this._Tempo = rdr.GetTimeSpan(1);
                    this._TempoUnload = rdr.GetTimeSpan(2);
                    this._Default = rdr.GetBoolean(3);
                }
                else
                {
                    this._IdProcesso = -1;
                    this._RevisioneProcesso = -1;
                    this._Variante = -1;
                    this._NumeroOperatori = 0;
                    this._Tempo = new TimeSpan(0, 0, 0);
                    this._TempoSetup = new TimeSpan(0, 0, 0);
                    this._TempoUnload = new TimeSpan(0, 0, 0);
                    this._Default = false;
                }
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._Default = false;
                this._IdProcesso = -1;
                this._Variante = -1;
                this._RevisioneProcesso = -1;
                this._NumeroOperatori = 0;
                this._Tempo = new TimeSpan(0, 0, 0);
                this._TempoSetup = new TimeSpan(0, 0, 0);
                this._TempoUnload = new TimeSpan(0, 0, 0);
            }
        }

        public bool Delete()
        {
            bool rt = false;
            if (IdProcesso != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM tempiciclo WHERE processo = " + this.IdProcesso.ToString() + " AND revisione = " 
                    + this.RevisioneProcesso.ToString() + " AND variante = " + this.Variante.ToString() + " AND num_op = " + this.NumeroOperatori.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    rt = true;
                }
                catch (Exception ex)
                {
                    rt = false;
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }
    }

    public class TempiCiclo
    {
        public String log;

        private int _IdProcesso;
        public int IdProcesso
        {
            get { return this._IdProcesso; }
        }

        private int _RevisioneProcesso;
        public int RevisioneProcesso
        {
            get { return this._RevisioneProcesso; }
        }

        private int _Variante;
        public int Variante
        {
            get { return this._Variante; }
        }

        public List<TempoCiclo> Tempi;

        public TempiCiclo(int idProc, int revProc, int var)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT num_op FROM tempiciclo WHERE processo = " + idProc.ToString() + " AND revisione = " + revProc.ToString()
                + " AND variante = " + var.ToString() + " ORDER BY num_op";
            MySqlDataReader rdr = cmd.ExecuteReader();
            Tempi = new List<TempoCiclo>();
            while (rdr.Read())
            {
                Tempi.Add(new TempoCiclo(idProc, revProc, var, rdr.GetInt32(0)));
            }

            this._IdProcesso = idProc;
            this._RevisioneProcesso = revProc;
            this._Variante = var;

            rdr.Close();
            conn.Close();
        }

        public bool Add(int n_ops, TimeSpan tc, TimeSpan tSetup, bool def)
        {
            bool rt = false;
            if (this.IdProcesso!=-1 && n_ops > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                log += "<br/>"+tc.ToString();
                try
                {
                    if(def == true)
                    {
                        cmd.CommandText = "UPDATE tempiciclo SET def = false WHERE processo = " + this.IdProcesso.ToString()
                            + " AND revisione = " + this.RevisioneProcesso.ToString() + " AND variante = " + this.Variante.ToString();
                        cmd.ExecuteNonQuery();
                    }
                    
                    cmd.CommandText = "INSERT INTO tempiciclo(processo, revisione, variante, num_op, setup, tempo, tunload, def) VALUES("
                        + this.IdProcesso.ToString() + ", " 
                        + this.RevisioneProcesso.ToString() + ", " 
                        + this.Variante.ToString() + ", "
                        + n_ops.ToString() + ", "
                        + "'" + Math.Floor(tSetup.TotalHours).ToString() + ":" + tSetup.Minutes.ToString() + ":" + tSetup.Seconds.ToString() + "', " 
                        + "'" + Math.Floor(tc.TotalHours).ToString() + ":" + tc.Minutes.ToString() + ":" + tc.Seconds.ToString() + "', " 
                        + "'00:00:00', " 
                        + def.ToString() + ")";
                    
                    cmd.ExecuteNonQuery();

                    rt = true;
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    log += ex.Message;
                    tr.Rollback();
                    rt = false;
                }
                conn.Close();
            }
            return rt;
        }
    }

    public class ElencoTasks
    {
        public List<processo> Elenco;

        public ElencoTasks()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processID, revisione FROM processo WHERE attivo = 1 ORDER BY Name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            Elenco = new List<processo>();
            while (rdr.Read())
            {
                Elenco.Add(new processo(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        /* eseguibili = true Ritorna solo i task eseguibili in produzione, cioè senza figli! */
        public ElencoTasks(bool eseguibili)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            if (eseguibili == true)
            {
                cmd.CommandText = "SELECT DISTINCT(processID), revisione FROM processo "
                    + " INNER JOIN processipadrifigli ON (processo.processID = processipadrifigli.task AND "
                    + "processo.revisione = processipadrifigli.revTask) "
                +"WHERE attivo = 1 AND padre IS NOT NULL ORDER BY Name";
            }
            else
            {
                cmd.CommandText = "SELECT processID, revisione FROM processo WHERE attivo = 1 ORDER BY Name";
            }
            MySqlDataReader rdr = cmd.ExecuteReader();
            Elenco = new List<processo>();
            while (rdr.Read())
            {
                processo tsk = new processo(rdr.GetInt32(0), rdr.GetInt32(1));
                if (eseguibili == true)
                {
                    tsk.loadFigli();
                    if (tsk.subProcessi.Count == 0)
                    {
                        Elenco.Add(tsk);
                    }
                }
                else
                {
                    Elenco.Add(tsk);
                }
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class ProductParametersCategory
    {
        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private String _Name;
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE productparameterscategories SET paramCatName = '" + value + "' WHERE "
                    + " paramCatID = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Name = value;
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Description;
        public String Description
        {
            get { return this._Description; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE productparameterscategories SET paramCatDescription = '" + value + "' WHERE "
                    + " paramCatID = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Description = value;
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public ProductParametersCategory()
        {
            this._ID = -1;
            this.Name = "";
            this.Description = "";
        }

        public ProductParametersCategory(int CategoryID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT paramCatName, paramCatDescription FROM productparameterscategories "
                + " WHERE paramCatID = " + CategoryID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._ID = -1;
            this._Name = "";
            this._Description = "";

            if(rdr.Read())
            {
                this._ID = CategoryID;
                this._Name = rdr.GetString(0);
                this._Description = rdr.GetString(1);
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class ProductParametersCategories
    {
        public String log;

        public List<ProductParametersCategory> Categories;

        public ProductParametersCategories()
        {
            this.Categories = new List<ProductParametersCategory>();
        }

        public void loadCategories()
        {
            this.Categories = new List<ProductParametersCategory>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT paramCatID FROM productparameterscategories ORDER BY paramCatName";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.Categories.Add(new ProductParametersCategory(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean Add(String name, String description)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(paramCatID) FROM productparameterscategories";
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxID = 0;
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();

            cmd.CommandText = "INSERT INTO productparameterscategories(paramCatID, paramCatName, paramCatDescription) "
                + " VALUES(" + maxID.ToString() + ", '" + name + "', '" + description + "')";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch(Exception ex)
            {
                log = ex.Message;
                ret = false;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public Boolean Delete(int id)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM productparameterscategories WHERE paramCatID = " + id.ToString();
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                ret = false;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }
    }

    public class ModelParameter
    {
        private int _ProcessID;
        public int ProcessID
        {
            get
            {
                return this._ProcessID;
            }
        }

        private int _ProcessRev;
        public int ProcessRev
        {
            get
            {
                return this._ProcessRev;
            }
        }

        private int _VarianteID;
        public int VarianteID
        {
            get
            {
                return this._VarianteID;
            }
        }

        private ProductParametersCategory _ParameterCategory;
        public ProductParametersCategory ParameterCategory
        {
            get
            {
                return this._ParameterCategory;
            }
            set
            {
                if (this.ProcessID != -1 && this.VarianteID != -1 && value!= null && value.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelparameters SET paramCategory = " + value.ID + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND processID = " + this.ProcessID.ToString()
                        + " AND processRev = " + this.ProcessRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._ParameterCategory = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private int _ParameterID;
        public int ParameterID
        {
            get
            {
                return this._ParameterID;
            }
        }

        private String _Name;
        public String Name
        {
            get { return this._Name; }
            set
            {
                if(this.ProcessID!=-1 && this.VarianteID!=-1)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE modelparameters SET paramName = '" + value + "' WHERE "
                    + " paramID = " + this.ParameterID.ToString() 
                    + " AND processID = " + this.ProcessID.ToString()
                    + " AND processRev = " + this.ProcessRev.ToString()
                    + " AND varianteID = " + this.VarianteID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Name = value;
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
                }
            }
        }

        private String _Description;
        public String Description
        {
            get { return this._Description; }
            set
            {
                if (this.ProcessID != -1 && this.VarianteID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelparameters SET paramDescription = '" + value + "' WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND processID = " + this.ProcessID.ToString()
                        + " AND processRev = " + this.ProcessRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Description = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private Boolean _isFixed;
        public Boolean isFixed
        {
            get { return this._isFixed; }
            set
            {
                if (this.ProcessID != -1 && this.VarianteID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelparameters SET isFixed = " + value + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND processID = " + this.ProcessID.ToString()
                        + " AND processRev = " + this.ProcessRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._isFixed = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private Boolean _isRequired;
        public Boolean isRequired
        {
            get { return this._isRequired; }
            set
            {
                if (this.ProcessID != -1 && this.VarianteID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelparameters SET isRequired = " + value + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND processID = " + this.ProcessID.ToString()
                        + " AND processRev = " + this.ProcessRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._isRequired = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private int _Sequence;
        public int Sequence
        {
            get
            {
                return this._Sequence;
            }
        }

        public ModelParameter(int processID, int processRev, int variantID, int parameterID)
        {
            this._ParameterID = -1;
            this._ProcessID = -1;
            this._ProcessRev = -1;
            this._VarianteID = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processID, processRev, varianteID, paramID, paramCategory, paramName, "
                + " paramDescription, isFixed, isRequired, sequence FROM modelparameters WHERE "
                + " processID = " + processID.ToString()
                + " AND processRev = " + processRev.ToString()
                + " AND varianteID = " + variantID.ToString()
                + " AND paramID = " + parameterID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(3))
            {
                this._ProcessID = rdr.GetInt32(0);
                this._ProcessRev = rdr.GetInt32(1);
                this._VarianteID = rdr.GetInt32(2);
                this._ParameterID = rdr.GetInt32(3);
                this.ParameterCategory = new ProductParametersCategory(rdr.GetInt32(4));
                if(!rdr.IsDBNull(5))
                { 
                    this._Name = rdr.GetString(5);
                }
                if(!rdr.IsDBNull(6))
                {
                    this._Description = rdr.GetString(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    this._isFixed = rdr.GetBoolean(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    this._isRequired = rdr.GetBoolean(8);
                }
                if (!rdr.IsDBNull(9))
                {
                    this._Sequence = rdr.GetInt32(9);
                }
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class ModelTaskParameter
    {
        private int _TaskID;
        public int TaskID
        {
            get
            {
                return this._TaskID;
            }
        }

        private int _TaskRev;
        public int TaskRev
        {
            get
            {
                return this._TaskRev;
            }
        }

        private int _VarianteID;
        public int VarianteID
        {
            get
            {
                return this._VarianteID;
            }
        }

        private ProductParametersCategory _ParameterCategory;
        public ProductParametersCategory ParameterCategory
        {
            get
            {
                return this._ParameterCategory;
            }
            set
            {
                if (this.TaskID != -1 && this.VarianteID != -1 && value != null && value.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelTaskparameters SET paramCategory = " + value.ID + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND TaskID = " + this.TaskID.ToString()
                        + " AND TaskRev = " + this.TaskRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._ParameterCategory = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private int _ParameterID;
        public int ParameterID
        {
            get
            {
                return this._ParameterID;
            }
        }

        private String _Name;
        public String Name
        {
            get { return this._Name; }
            set
            {
                if (this.TaskID != -1 && this.VarianteID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelTaskparameters SET paramName = '" + value + "' WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND TaskID = " + this.TaskID.ToString()
                        + " AND TaskRev = " + this.TaskRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Name = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private String _Description;
        public String Description
        {
            get { return this._Description; }
            set
            {
                if (this.TaskID != -1 && this.VarianteID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelTaskparameters SET paramDescription = '" + value + "' WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND TaskID = " + this.TaskID.ToString()
                        + " AND TaskRev = " + this.TaskRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Description = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private Boolean _isFixed;
        public Boolean isFixed
        {
            get { return this._isFixed; }
            set
            {
                if (this.TaskID != -1 && this.VarianteID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelTaskparameters SET isFixed = " + value + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND TaskID = " + this.TaskID.ToString()
                        + " AND TaskRev = " + this.TaskRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._isFixed = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private Boolean _isRequired;
        public Boolean isRequired
        {
            get { return this._isRequired; }
            set
            {
                if (this.TaskID != -1 && this.VarianteID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelTaskparameters SET isRequired = " + value + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND TaskID = " + this.TaskID.ToString()
                        + " AND TaskRev = " + this.TaskRev.ToString()
                        + " AND varianteID = " + this.VarianteID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._isRequired = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private int _Sequence;
        public int Sequence
        {
            get
            {
                return this._Sequence;
            }
        }

        public ModelTaskParameter(int TaskID, int TaskRev, int variantID, int parameterID)
        {
            this._ParameterID = -1;
            this._TaskID = -1;
            this._TaskRev = -1;
            this._VarianteID = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TaskID, TaskRev, varianteID, paramID, paramCategory, paramName, "
                + " paramDescription, isFixed, isRequired, sequence FROM modelTaskparameters WHERE "
                + " TaskID = " + TaskID.ToString()
                + " AND TaskRev = " + TaskRev.ToString()
                + " AND varianteID = " + variantID.ToString()
                + " AND paramID = " + parameterID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(3))
            {
                this._TaskID = rdr.GetInt32(0);
                this._TaskRev = rdr.GetInt32(1);
                this._VarianteID = rdr.GetInt32(2);
                this._ParameterID = rdr.GetInt32(3);
                this.ParameterCategory = new ProductParametersCategory(rdr.GetInt32(4));
                if (!rdr.IsDBNull(5))
                {
                    this._Name = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    this._Description = rdr.GetString(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    this._isFixed = rdr.GetBoolean(7);
                }
                if (!rdr.IsDBNull(8))
                {
                    this._isRequired = rdr.GetBoolean(8);
                }
                if (!rdr.IsDBNull(9))
                {
                    this._Sequence = rdr.GetInt32(9);
                }
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class TaskWorkInstruction
    {
        public KIS.App_Sources.WorkInstructions.WorkInstruction WI;

        private int _TaskID;
        public int TaskID
        {
            get
            {
                return this._TaskID;
            }
        }
        private int _TaskRev;
        public int TaskRev
        {
            get
            {
                return this._TaskRev;
            }
        }
        private int _VariantID;
        public int VariantID
        {
            get
            {
                return this._VariantID;
            }
        }

        private DateTime _InitialDate;
        public DateTime InitialDate { 
            get{return this._InitialDate;}
        }

        private DateTime _ExpiryDate;
        public DateTime ExpiryDate
        {
            get { return this._ExpiryDate; }
        }

        private int _Sequence;
        public int Sequence { get { return this._Sequence; } }

        private Boolean _IsActive;
        public Boolean IsActive { get { return this._IsActive; } }

        public TaskWorkInstruction(int TaskID, int TaskRev, int VariantID, int ManualID, int ManualVersion)
        {
            this._TaskID = -1;
            this._TaskRev = -1;
            this._VariantID = -1;
            this.WI = null;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT manualID, manualVersion, validityInitialDate, expiryDate, sequence, isActive FROM tasksmanuals WHERE "
                + " taskID = @TaskId AND taskRev = @TaskRev AND taskVarianti = @variante "
                + " AND manualID = @ManualID AND manualVersion=@ManualVersion";
            cmd.Parameters.AddWithValue("@TaskId", TaskID);
            cmd.Parameters.AddWithValue("@TaskRev", TaskRev);
            cmd.Parameters.AddWithValue("@variante", VariantID);
            cmd.Parameters.AddWithValue("@ManualID", ManualID);
            cmd.Parameters.AddWithValue("@ManualVersion", ManualVersion);

            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                this.WI = new App_Sources.WorkInstructions.WorkInstruction(rdr.GetInt32(0), rdr.GetInt32(1));
                this._InitialDate = rdr.GetDateTime(2);
                this._ExpiryDate = rdr.GetDateTime(3);
                this._Sequence = rdr.GetInt32(4);
                this._IsActive = rdr.GetBoolean(5);
                this._TaskID = TaskID;
                this._TaskRev = TaskRev;
                this._VariantID = VariantID;
            }
            rdr.Close();

            conn.Close();
        }

        /*Returns:
         */
         public int Delete()
        {
            int ret = 0;
            if(this.WI!=null && this.WI.ID!=-1 && this.WI.Version!=-1 && this.TaskID!=-1 && this.VariantID!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM tasksmanuals WHERE taskId=@TaskID AND TaskRev=@TaskRev AND taskVarianti=@VariantID "
                    + " AND manualID=@ManualID AND manualVersion=@ManualVersion";
                cmd.Parameters.AddWithValue("@TaskID", TaskID);
                cmd.Parameters.AddWithValue("@TaskRev", TaskRev);
                cmd.Parameters.AddWithValue("@VariantID", VariantID);
                cmd.Parameters.AddWithValue("@ManualID", this.WI.ID);
                cmd.Parameters.AddWithValue("@ManualVersion", this.WI.Version);

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = 1;
                }
                catch
                {
                    ret = 2;
                    tr.Rollback();
                }
               conn.Close();
            }
            return ret;
        }
    }

    public class Microstep
    {
        private int _id;
        public int id
        {
            get { return this._id;  }
        }

        private int _review;
        public int review
        { get { return this._review; } }

        private String _name;
        public String name
        {
            get { return this._name; }
        }

        private String _description;
        public String description
        {
            get { return this._description; }
        }

        private DateTime _CreationDate;
        public DateTime CreationDate
        {
            get { return this._CreationDate; }
        }

        public Microstep(int id, int rev)
        {
            this._CreationDate = new DateTime(1970, 1, 1);
            this._id = -1;
            this._review = -1;
            this._name = "";
            this._description = "";

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, review, name, description, creation_date FROM microsteps WHERE "
                + "id=@id AND review=@review";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@review", rev);

            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                this._id = rdr.GetInt32(0);
                this._review = rdr.GetInt32(1);
                this._name = rdr.GetString(2);
                this._description = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                this._CreationDate = rdr.GetDateTime(4);
            }
            rdr.Close();

            conn.Close();
        }
    }

    public class TaskMicrostep
    {
        private int _TaskId;
        public int TaskId
        {
            get { return this._TaskId; }
        }

        private int _TaskRev;
        public int TaskRev
        {
            get { return this._TaskRev; }
        }

        private int _VariantId;
        public int VariantId
        {
            get { return this._VariantId; }
        }

        private int _MicrostepId;
        public int MicrostepId
        {
            get { return this._MicrostepId; }
        }

        private int _MicrostepReview;
        public int MicrostepReview
        { get { return this._MicrostepReview; } }

        private String _MicrostepName;
        public String MicrostepName
        {
            get { return this._MicrostepName; }
        }

        private String _MicrostepDescription;
        public String MicrostepDescription
        {
            get { return this._MicrostepDescription; }
        }

        private DateTime _CreationDate;
        public DateTime CreationDate
        {
            get { return this._CreationDate; }
        }

        // Hours
        private int _CycleTime;
        public int CycleTime { get { return this._CycleTime; } }

        /* Returns:
         * V Value
         * W evident Waste
         * H Hidden waste
         */
        private Char _ValueOrWaste;
        public Char ValueOrWaste {  get { return this._ValueOrWaste; } }

        private int _Sequence;
        public int Sequence { 
            get { return this._Sequence; } 
            set
            {
                if(value >= 0 && this.TaskId != 1 && this.TaskRev != -1 && this.MicrostepId !=-1 && this.MicrostepReview != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE task_microsteps SET sequence=@sequence WHERE taskid=@taskid AND taskrev=@taskrev "
                        + " AND variantid=@variantid AND microstep_id=@microstepid AND microstep_rev=@microsteprev";
                    cmd.Parameters.AddWithValue("@sequence", value);
                    cmd.Parameters.AddWithValue("@taskid", this.TaskId);
                    cmd.Parameters.AddWithValue("@taskrev", this.TaskRev);
                    cmd.Parameters.AddWithValue("@variantid", this.VariantId);
                    cmd.Parameters.AddWithValue("@microstepid", this.MicrostepId);
                    cmd.Parameters.AddWithValue("@microsteprev", this.MicrostepReview);
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public TaskMicrostep(int taskID, int taskRev, int variantID, int microstepID, int microstepRev)
        {
            this._CreationDate = new DateTime(1970, 1, 1);
            this._TaskId = -1;
            this._TaskRev = -1;
            this._VariantId = -1;
            this._MicrostepId = -1;
            this._MicrostepReview = -1;
            this._MicrostepName = "";
            this._MicrostepDescription = "";
            this._Sequence = -1;
            this._CycleTime = 0;
            this._ValueOrWaste = '\0';

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT microsteps.id, microsteps.review, microsteps.name, microsteps.description, microsteps.creation_date, "
                + "task_microsteps.taskid, task_microsteps.taskrev, task_microsteps.variantid, task_microsteps.sequence, task_microsteps.cycletime, task_microsteps.value_or_waste "
                + " FROM microsteps "
                + " INNER JOIN task_microsteps ON (microsteps.id = task_microsteps.microstep_id AND microsteps.review = task_microsteps.microstep_rev)"
                + " WHERE "
                + "microsteps.id=@id AND microsteps.review=@review"
                + " AND task_microsteps.taskid=@taskid AND task_microsteps.taskrev=@taskrev AND task_microsteps.variantid=@variantid";
            cmd.Parameters.AddWithValue("@id", microstepID);
            cmd.Parameters.AddWithValue("@review", microstepRev);
            cmd.Parameters.AddWithValue("@taskid", taskID);
            cmd.Parameters.AddWithValue("@taskrev", taskRev);
            cmd.Parameters.AddWithValue("@variantid", variantID);

            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                this._MicrostepId = rdr.GetInt32(0);
                this._MicrostepReview = rdr.GetInt32(1);
                this._MicrostepName = rdr.GetString(2);
                this._MicrostepDescription = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                this._CreationDate = rdr.GetDateTime(4);
                this._TaskId = rdr.GetInt32(5);
                this._TaskRev = rdr.GetInt32(6);
                this._VariantId = rdr.GetInt32(7);
                this._Sequence = rdr.GetInt32(8);
                this._CycleTime = rdr.GetInt32(9);
                this._ValueOrWaste = rdr.GetChar(10);
            }
            else
            {
                
            }
            rdr.Close();

            conn.Close();
        }
    }
}