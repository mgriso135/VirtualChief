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
using Dapper;
using MySql.Data.MySqlClient;

namespace KIS.App_Code
{
    public class macroProcessi
    {
        protected String Tenant;

        public List<processo> Elenco;

        private class ProcessoIdRow
        {
            public int processID { get; set; }
            public int revisione { get; set; }
        }
        
        public macroProcessi(String Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            this.Elenco = new List<processo>();
            foreach (var row in conn.Query<ProcessoIdRow>("SELECT processo.processID, processo.revisione FROM processo LEFT JOIN processipadrifigli "
                + " ON (processo.processID = processipadrifigli.task AND processo.revisione = processipadrifigli.revTask) "
                + " WHERE processipadrifigli.padre IS NULL AND processo.attivo = 1 ORDER BY processo.name"))
            {
                this.Elenco.Add(new processo(this.Tenant, row.processID, row.revisione));
            }
            conn.Close();            
        }
        
        public bool Add(String nome, String Descr, bool isVSM)
        {
            bool res = false;
            if(nome.Length > 0 && Descr.Length > 0)
            {                
                string strSQL = "SELECT MAX(processID) from processo";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? maxVal = conn.ExecuteScalar<int?>(strSQL);
                int maxCod;
                if (maxVal.HasValue)
                {
                    maxCod = maxVal.Value + 1;
                }
                else
                {
                    maxCod = 0;
                }
                strSQL = "INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, isVSM, posx, posy, attivo) " +
                    "VALUES(@p0, 0, @p1, @p2, @p3, @p4, 0, 0, 1)";
                conn.Execute(strSQL, new { p0 = maxCod, p1 = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), p2 = nome, p3 = Descr, p4 = isVSM });
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
                    string strSQL = "DELETE FROM processo WHERE processID = @p0";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    conn.Execute(strSQL, new { p0 = macroProcID });
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
        protected String Tenant;

        public String log;

        private class PadreRow
        {
            public int padre { get; set; }
            public int revPadre { get; set; }
        }

        private class FiglioRow
        {
            public int task { get; set; }
            public int revTask { get; set; }
        }

        private class FiglioPosRow
        {
            public int task { get; set; }
            public int revTask { get; set; }
            public int posx { get; set; }
            public int posy { get; set; }
        }

        private class FiglioCopyRow
        {
            public int processID { get; set; }
            public int revisione { get; set; }
        }

        private class VarianteFigliaRow
        {
            public int variante { get; set; }
        }

        private class PrecedenzaRow
        {
            public int prec { get; set; }
            public int revPrec { get; set; }
            public int relazione { get; set; }
            public TimeSpan pausa { get; set; }
            public int? ConstraintType { get; set; }
        }

        private class SuccessivoRow
        {
            public int succ { get; set; }
            public int revSucc { get; set; }
            public int relazione { get; set; }
            public TimeSpan pausa { get; set; }
            public int? ConstraintType { get; set; }
        }

        private class ProcessoRow
        {
            public int processID { get; set; }
            public int revisione { get; set; }
            public DateTime dataRevisione { get; set; }
            public String Name { get; set; }
            public String Description { get; set; }
            public bool isVSM { get; set; }
            public int posx { get; set; }
            public int posy { get; set; }
            public bool attivo { get; set; }
        }

        private class ProcessOwnerRow
        {
            public String userID { get; set; }
        }

        private class ImplosioneRow
        {
            public int processo { get; set; }
            public int revisione { get; set; }
            public int variante { get; set; }
        }

        private class PadreCopyRow
        {
            public int padre { get; set; }
            public int revPadre { get; set; }
            public int variante { get; set; }
            public int posx { get; set; }
            public int posy { get; set; }
        }

        private class PrecSuccCopyRow
        {
            public int prec { get; set; }
            public int revPrec { get; set; }
            public int succ { get; set; }
            public int revSucc { get; set; }
            public int variante { get; set; }
            public int relazione { get; set; }
        }

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
                FusoOrario fuso = new FusoOrario(this.Tenant);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    conn.Execute("UPDATE precedenzeprocessi SET pausa = @p0 WHERE "
                        + "prec = @p1"
                        + " AND revPrec = @p2"
                        + " AND succ = @p3"
                        + " AND revSucc=@p4"
                        + " AND variante = @p5", new { p0 = Math.Truncate(pausa.TotalHours).ToString() + ":"
                        + pausa.Minutes.ToString() + ":"
                        + pausa.Seconds.ToString(), p1 = tskPrec.Task.processID, p2 = tskPrec.Task.revisione, p3 = this.processID, p4 = this.revisione, p5 = tskPrec.variant.idVariante }, tr);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();

                try
                {
                    conn.Execute("UPDATE precedenzeprocessi SET ConstraintType = @p0"
                        + " WHERE "
                        + "prec = @p1"
                        + " AND revPrec = @p2"
                        + " AND succ = @p3"
                        + " AND revSucc=@p4"
                        + " AND variante = @p5", new { p0 = cstrType, p1 = tskPrec.Task.processID, p2 = tskPrec.Task.revisione, p3 = this.processID, p4 = this.revisione, p5 = tskPrec.variant.idVariante }, tr);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var row = conn.QueryFirstOrDefault<PadreRow>("SELECT padre, revPadre FROM processipadrifigli WHERE task = @p0"
                    + " AND revTask = @p1 AND variante = @p2", new { p0 = this.processID, p1 = this.revisione, p2 = vr.idVariante });
                if(row != null)
                {
                    ret[0] = row.padre;
                    ret[1] = row.revPadre;
                }
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
                string strSQL = "UPDATE processo SET Name = @p0 WHERE processID = @p1";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = value, p1 = _processID });
                _processName = value;
                conn.Close();
            }
        }
        public String processDescription {
            get { return _processDescription; }
            set
            {
                string strSQL = "UPDATE processo SET Description = @p0 WHERE processID = @p1";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = value, p1 = _processID });
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
                string strSQL = "UPDATE processo SET posx = @p0 WHERE processID = @p1";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = value, p1 = _processID });
                this._posX = value;
                conn.Close();
            }
        }

        public bool setPosX(int psx, variante vr)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            conn.Execute("UPDATE processipadrifigli SET posx = @p0 WHERE variante = @p1"
                + " AND task = @p2 AND revTask = @p3", new { p0 = psx, p1 = vr.idVariante, p2 = this.processID, p3 = this.revisione });
            this._posX = psx;
            rt = true;
            conn.Close();
            return rt;
        }

        public bool setPosY(int psy, variante vr)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            conn.Execute("UPDATE processipadrifigli SET posy = @p0 WHERE variante = @p1"
                + " AND task = @p2 AND revTask = @p3", new { p0 = psy, p1 = vr.idVariante, p2 = this.processID, p3 = this.revisione });
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
                string strSQL = "UPDATE processo SET posy = @p0 WHERE processID = @p1";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = value, p1 = _processID });
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
                string strSQL = "UPDATE processo SET isVSM = @p0 WHERE processID = @p1";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = value, p1 = _processID });
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute("UPDATE processo SET attivo = @p0 WHERE processID = @p1"
                    + " AND revisione = @p2", new { p0 = value, p1 = this.processID, p2 = this.revisione });
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

        public processo(String Tenant)
        {
            this.Tenant = Tenant;

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
            : this(KIS.App_Code.WebEnv.ActiveWorkspaceName, procID)
        {
        }

        public processo(int procID, int rev)
            : this(KIS.App_Code.WebEnv.ActiveWorkspaceName, procID, rev)
        {
        }

        public processo(String Tenant, int procID)
        {
            this.Tenant = Tenant;

            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;

            // Ricerco l'ultima revisione del processo
            String strSQL = "SELECT MAX(revisione) FROM processo WHERE processID = @p0 AND processo.attivo = 1";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? maxRev = conn.ExecuteScalar<int?>(strSQL, new { p0 = procID });
            if (maxRev.HasValue)
            {
                this._revisione = maxRev.Value;
            }

            // Carico le informazioni di base del processo.
            strSQL = "SELECT processo.processID, revisione, dataRevisione, processo.Name, processo.Description, "
            + "isVSM, posx, posy, attivo FROM processo WHERE processID = @p0 AND revisione = @p1 AND processo.attivo = 1";
            
            var row = conn.QueryFirstOrDefault<ProcessoRow>(strSQL, new { p0 = procID, p1 = this.revisione });
            if (row != null)
            {
                this._processID = procID;
                this._revisione = row.revisione;
                this._dataRevisione = row.dataRevisione;
                this._processName = row.Name;
                this._processDescription = row.Description;
                this._isVSM = row.isVSM;
                this._posX = row.posx;
                this._posY = row.posy;
                this._attivo = row.attivo;
            }
            else
            {
                this._processID = -1;
                this._processName = "";
                this._processDescription = "";
            }

            conn.Close();
        }
        
        public processo(String Tenant, int procID, int rev)
        {
            this.Tenant = Tenant;

            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            // Carico le informazioni di base del processo.
            String strSQL = "SELECT processo.processID, revisione, dataRevisione, processo.Name, processo.Description, "
            + "isVSM, posx, posy, attivo FROM processo WHERE processID = @p0 AND revisione = @p1";// +" AND processo.attivo = 1";

            var row = conn.QueryFirstOrDefault<ProcessoRow>(strSQL, new { p0 = procID, p1 = rev });
            if (row != null)
            {
                this._processID = procID;
                this._revisione = row.revisione;
                this._dataRevisione = row.dataRevisione;
                this._processName = row.Name;
                this._processDescription = row.Description;
                this._isVSM = row.isVSM;
                this._posX = row.posx;
                this._posY = row.posy;
                this._attivo = row.attivo;
            }
            else
            {
                this._processID = -1;
                this._processName = "";
                this._processDescription = "";
            }

            conn.Close();
        }

        public processo(String Tenant, String procName)
        {
            this.Tenant = Tenant;

            this._processiPrec = new List<int>();
            this._processiSucc = new List<int>();
            this._processID = -1;
            this._processName = "";
            this._processDescription = "";
            this.subProcessi = null;

            // Ricerco l'ultima revisione del processo
            String strSQL = "SELECT processID FROM processo WHERE Name LIKE @p0 AND processo.attivo = 1";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? idProc = conn.ExecuteScalar<int?>(strSQL, new { p0 = procName });
            if (idProc.HasValue)
            {
                this.setupInt(idProc.Value);
            }
            else
            {
                this._processID = -1;
                this._revisione = -1;
                this._processName = "";
            }
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
            String strSQL = "SELECT MAX(revisione) FROM processo WHERE processID = @p0 AND processo.attivo = 1";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? maxRev = conn.ExecuteScalar<int?>(strSQL, new { p0 = procID });
            if (maxRev.HasValue)
            {
                this._revisione = maxRev.Value;
            }

            // Carico le informazioni di base del processo.
            strSQL = "SELECT processo.processID, revisione, dataRevisione, processo.Name, processo.Description, "
            + "isVSM, posx, posy, attivo FROM processo WHERE processID = @p0 AND revisione = @p1 AND processo.attivo = 1";

            var row = conn.QueryFirstOrDefault<ProcessoRow>(strSQL, new { p0 = procID, p1 = this.revisione });
            if (row != null)
            {
                this._processID = procID;
                this._revisione = row.revisione;
                this._dataRevisione = row.dataRevisione;
                this._processName = row.Name;
                this._processDescription = row.Description;
                this._isVSM = row.isVSM;
                this._posX = row.posx;
                this._posY = row.posy;
                this._attivo = row.attivo;
                ret = true;
            }
            else
            {
                this._processID = -1;
                this._processName = "";
                this._processDescription = "";
                ret = false;
            }

            conn.Close();
            return ret;
        }
        

        // Carico i dati del processo padre.
        public void loadPadre()
        {
            if (this.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var row = conn.QueryFirstOrDefault<PadreRow>("SELECT padre, revPadre FROM processipadrifigli WHERE task = @p0"
                    + " AND revTask = @p1", new { p0 = this.processID, p1 = this.revisione });
                if (row != null)
                {
                    this._processoPadre = row.padre;
                    this._revPadre = row.revPadre;
                }
                else
                {
                    this._processoPadre = -1;
                    this._revPadre = -1;
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var row = conn.QueryFirstOrDefault<PadreRow>("SELECT padre, revPadre FROM processipadrifigli WHERE task = @p0"
                    + " AND revTask = @p1 AND variante = @p2", new { p0 = this.processID, p1 = this.revisione, p2 = vr.idVariante });
                if (row != null)
                {
                    this._processoPadre = row.padre;
                    this._revPadre = row.revPadre;
                }
                else
                {
                    this._processoPadre = -1;
                    this._revPadre = -1;
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                
                
                foreach (var variante in conn.Query<int>("SELECT variante FROM processo INNER JOIN variantiprocessi ON "
                    + "(variantiprocessi.processo = processo.processID AND variantiprocessi.revProc = processo.revisione) "
                    + " WHERE processo.processID = @p0 AND processo.revisione = @p1", new { p0 = this.processID, p1 = this.revisione }))
                {
                    this._variantiProcesso.Add(new variante(this.Tenant, variante));
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                this._variantiFigli = new List<variante>();
                foreach (var variante in conn.Query<int>("SELECT DISTINCT(variante) FROM processipadrifigli INNER JOIN processo ON (processipadrifigli.task = processo.processID "
                    + " AND processipadrifigli.revTask = processo.revisione)"
                    + " WHERE processipadrifigli.padre = @p0 AND processipadrifigli.revPadre = @p1"
                    + " AND processo.attivo = 1", new { p0 = this.processID, p1 = this.revisione }))
                {
                    this._variantiFigli.Add(new variante(this.Tenant, variante));
                }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                this.subProcessi = new List<processo>();
                int i = 0;
                foreach (var row in conn.Query<FiglioRow>("SELECT task, revTask FROM processo INNER JOIN variantiprocessi ON "
                + "(variantiprocessi.processo = processo.processID AND variantiprocessi.revProc = processo.revisione) "
                + "INNER JOIN processipadrifigli ON(processipadrifigli.padre = variantiprocessi.processo AND processipadrifigli.revPadre = variantiprocessi.revProc "
                + " AND processipadrifigli.variante = variantiprocessi.variante) WHERE "
                + " processipadrifigli.padre = @p0"
                + " AND processipadrifigli.revPadre = @p1 AND processo.attivo = 1", new { p0 = this.processID, p1 = this.revisione }))
                {
                    this.subProcessi.Add(new processo(this.Tenant, row.task));
                    this.subProcessi[i].loadPrecedenti();
                    this.subProcessi[i].loadSuccessivi();
                    i++;
                }
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            this.subProcessi = new List<processo>();

            int i = 0;
            foreach (var row in conn.Query<FiglioPosRow>("SELECT task, revTask, processipadrifigli.posx, processipadrifigli.posy FROM processo INNER JOIN variantiprocessi ON "
                + "(variantiprocessi.processo = processo.processID AND variantiprocessi.revProc = processo.revisione) "
                + "INNER JOIN processipadrifigli ON(processipadrifigli.padre = variantiprocessi.processo AND processipadrifigli.revPadre = variantiprocessi.revProc "
                + " AND processipadrifigli.variante = variantiprocessi.variante) INNER JOIN processo AS figlio ON(figlio.processID = task AND figlio.revisione=revtask) WHERE "
                + "variantiprocessi.variante = @p0 AND processipadrifigli.padre = @p1"
                + " AND processipadrifigli.revPadre = @p2 AND processo.attivo = 1 AND figlio.attivo=1", new { p0 = var.idVariante, p1 = this.processID, p2 = this.revisione }))
            {
                this.subProcessi.Add(new processo(this.Tenant, row.task, row.revTask));
                this.subProcessi[i]._posX = row.posx;
                this.subProcessi[i]._posY = row.posy;
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
                String strSQL = "SELECT COUNT(id) FROM kpi_description WHERE idprocesso = @p0 AND revisione = @p1 AND attivo = 1";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                this._numKPIs = conn.ExecuteScalar<int>(strSQL, new { p0 = this.processID, p1 = this.revisione });
                this.KPIs = new Kpi[this.numKPIs];
                strSQL = "SELECT id FROM kpi_description WHERE idprocesso = @p0 AND revisione = @p1 AND attivo = 1";
                int c = 0;
                foreach (var id in conn.Query<int>(strSQL, new { p0 = this.processID, p1 = this.revisione }))
                {
                    if (c < this.numKPIs)
                    {
                        this.KPIs[c] = new Kpi(this.Tenant, id);
                        c++;
                    }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                this._processiPrec = new List<int>();
                this._revisionePrec = new List<int>();
                this._relazionePrec = new List<relazione>();
                this._pausePrec = new List<TimeSpan>();

                // Carico i processi precedenti
                foreach (var row in conn.Query<PrecedenzaRow>("SELECT precedenzeprocessi.prec, precedenzeprocessi.revPrec, precedenzeprocessi.relazione, "
                    + "precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType"
                    + " FROM precedenzeprocessi"
                    + " WHERE precedenzeprocessi.succ = @p0 AND precedenzeprocessi.revsucc = @p1", new { p0 = this.processID, p1 = this.revisione }))
                {
                    this._processiPrec.Add(row.prec);
                    this._revisionePrec.Add(row.revPrec);
                    this._relazionePrec.Add(new relazione(this.Tenant, row.relazione));
                    this._pausePrec.Add(row.pausa);
                    this._ConstraintType.Add(row.ConstraintType.Value);

                    NearTask curr = new NearTask(this.Tenant);
                    curr.NearTaskID = row.prec;
                    curr.ConstraintType = row.ConstraintType.Value;
                    processo currprc = new processo(this.Tenant, curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.PreviousTasks.Add(curr);
                }

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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                this._processiPrec = new List<int>();
                this._relazionePrec = new List<relazione>();
                this._revisionePrec = new List<int>();
                this._pausePrec = new List<TimeSpan>();
        
                // Carico i processi precedenti
                foreach (var row in conn.Query<PrecedenzaRow>("SELECT precedenzeprocessi.prec, precedenzeprocessi.revPrec, precedenzeprocessi.relazione, precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType FROM precedenzeprocessi"
                    + " WHERE precedenzeprocessi.succ = @p0 AND precedenzeprocessi.revsucc = @p1"
                    + " AND precedenzeprocessi.variante = @p2", new { p0 = this.processID, p1 = this.revisione, p2 = var.idVariante }))
                {
                    this._processiPrec.Add(row.prec);
                    this._relazionePrec.Add(new relazione(this.Tenant, row.relazione));
                    this._revisionePrec.Add(row.revPrec);
                    this._pausePrec.Add(row.pausa);
                    this._ConstraintType.Add(row.ConstraintType.Value);

                    NearTask curr = new NearTask(this.Tenant);
                    curr.NearTaskID = row.prec;
                    curr.ConstraintType = row.ConstraintType.Value;
                    processo currprc = new processo(this.Tenant, curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.PreviousTasks.Add(curr);
                }

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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                this._processiSucc = new List<int>();
                this._revisioneSucc = new List<int>();
                this._relazioneSucc = new List<relazione>();

                // Carico i processi successivi
                foreach (var row in conn.Query<SuccessivoRow>("SELECT precedenzeprocessi.succ, precedenzeprocessi.revSucc, precedenzeprocessi.relazione, "
                    + " precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType"
                    + " FROM precedenzeprocessi "
                    + " WHERE precedenzeprocessi.prec = @p0 AND precedenzeprocessi.revPrec = @p1", new { p0 = this.processID, p1 = this.revisione }))
                {
                    this._processiSucc.Add(row.succ);
                    this._revisioneSucc.Add(row.revSucc);
                    this._relazioneSucc.Add(new relazione(this.Tenant, row.relazione));
                    this._pauseSucc.Add(row.pausa);
                    if (row.ConstraintType.HasValue)
                    {
                        this._ConstraintType.Add(row.ConstraintType.Value);
                    }
                    else
                    {
                        this._ConstraintType.Add(0);
                    }

                    NearTask curr = new NearTask(this.Tenant);
                    curr.NearTaskID = row.succ;
                    curr.ConstraintType = row.ConstraintType.GetValueOrDefault();
                    processo currprc = new processo(this.Tenant, curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.FollowingTasks.Add(curr);
                }

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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                
                this._processiSucc = new List<int>();
                this._relazioneSucc = new List<relazione>();


                // Carico i processi successivi
                foreach (var row in conn.Query<SuccessivoRow>("SELECT precedenzeprocessi.succ, precedenzeprocessi.revSucc, precedenzeprocessi.relazione, "
                    + "precedenzeprocessi.pausa, precedenzeprocessi.ConstraintType "
                    + " FROM precedenzeprocessi "
                    + " WHERE precedenzeprocessi.prec = @p0 AND precedenzeprocessi.revPrec = @p1"
                    + " AND precedenzeprocessi.variante = @p2", new { p0 = this.processID, p1 = this.revisione, p2 = var.idVariante }))
                {
                    this._processiSucc.Add(row.succ);
                    this._revisioneSucc.Add(row.revSucc);
                    this._relazioneSucc.Add(new relazione(this.Tenant, row.relazione));
                    this._pauseSucc.Add(row.pausa);
                    this._ConstraintType.Add(row.ConstraintType.Value);

                    NearTask curr = new NearTask(this.Tenant);
                    curr.NearTaskID = row.succ;
                    curr.ConstraintType = row.ConstraintType.Value;
                    processo currprc = new processo(this.Tenant, curr.NearTaskID);
                    curr.NearTaskName = currprc.processName;
                    this.FollowingTasks.Add(curr);
                }

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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();

                    // Trovo l'ultimo processo
                    int procID = 0;
                    int? maxID = conn.ExecuteScalar<int?>("SELECT MAX(ProcessID) FROM processo", transaction: trans);
                    if (maxID.HasValue)
                    {
                        procID = maxID.Value + 1;
                    }
                    try
                    {
                        // Aggiungo il processo
                        conn.Execute("INSERT INTO processo(ProcessID, revisione, dataRevisione, Name, Description, "
                        + " isVSM, posx, posy, attivo) VALUES(@p0, 0, @p1"
                        + ", 'New Default Process', 'New Default Process Notes', 0, 100, 100, 1)",
                        new { p0 = procID, p1 = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") }, trans);

                        // Inserisco il task come figlio di this e se incontro rogne faccio un rollback di tutto!
                        conn.Execute("INSERT INTO processipadrifigli(task, revTask, padre, revPadre, variante) VALUES("
                            + "@p0, 0, @p2, @p3, @p4)",
                        new { p0 = procID, p2 = this.processID, p3 = this.revisione, p4 = var.idVariante }, trans);
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
                                processo corrente = new processo(this.Tenant, procID);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    conn.Execute("INSERT INTO variantiprocessi(variante, processo, revProc, ExternalID, measurementUnit) VALUES(" 
                        + "@p0, @p1, @p2"
                        +", "
                        +"NULL, "
                        + "0"
                        + ")",
                        new { p0 = var.idVariante, p1 = this.processID, p2 = this.revisione }, trans);
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
               ElencoReparti elRep = new ElencoReparti(this.Tenant);
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
MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();

                    try
                    {
                        // Rimuovo l'associazione col processo padre
                        String strSQL = "DELETE FROM processipadrifigli WHERE task = @p0"
                            + " AND revTask = @p1";
                        conn.Execute(strSQL, new { p0 = this.processID, p1 = this.revisione }, trans);

                        // Cancello il process owner
                        strSQL = "DELETE FROM processOwners WHERE process = @p0";
                        conn.Execute(strSQL, new { p0 = this.processID }, trans);

                        // Cancello l'associazione con le varianti
                        this.loadVarianti();
                        conn.Execute("DELETE FROM variantiprocessi WHERE processo = @p0", new { p0 = this.processID }, trans);

                        // E' il primo processo. Cancello il precedente dei successivi ed elimino il processo
                        strSQL = "DELETE FROM precedenzeprocessi WHERE prec = @p0 OR succ = "
                            + "@p0 AND revPrec = @p1"
                            + " AND revSucc = @p1";
                        conn.Execute(strSQL, new { p0 = this.processID, p1 = this.revisione }, trans);

                        this.attivo = false;
                        // Elimino il processo.
                        strSQL = "DELETE FROM processo WHERE processID = @p0";
                        conn.Execute(strSQL, new { p0 = this.processID }, trans);
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
                string strSQL = "UPDATE precedenzeprocessi SET relazione = @p0 WHERE succ = @p1 AND prec = @p2";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = rel.relationID, p1 = this.processID, p2 = precedente.processID });
                conn.Close();
                res = true;
            }
            return res;
        }

        public bool addKPI(String nomeKPI, String descrKPI)
        {
            Kpi tmp = new Kpi(this.Tenant);
            return tmp.add(nomeKPI, descrKPI, new processo(this.Tenant, this.processID), 0);            
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
                    "@p0, @p1, @p2, @p3, @p4, 0, '00:00:00', @p5)";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(strSQL, new { p0 = this.processID, p1 = this.revisione, p2 = next.processID, p3 = next.revisione, p4 = var.idVariante, p5 = cstrType }, tr);
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
                    + "@p0, @p1, @p2, @p3"
                    + ", @p4, 0, '00:00:00', @p5)";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = preced.processID, p1 = preced.revisione, p2 = this.processID, p3 = this.revisione, p4 = var.idVariante, p5 = cstrType });
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
                        MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                        conn.Open();
                        String strSQL = "DELETE FROM precedenzeprocessi WHERE prec = @p0" 
                            + " AND revPrec = @p1 AND succ = @p2"
                            + " AND revSucc = @p3 AND variante = @p4";
                        conn.Execute(strSQL, new { p0 = this.processID, p1 = this.revisione, p2 = next.processID, p3 = next.revisione, p4 = var.idVariante });
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
                        MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                        conn.Open();
                        String strSQL = "DELETE FROM precedenzeprocessi WHERE succ = @p0 AND revSucc = @p1 AND prec = @p2 AND revPrec = @p3 AND variante = @p4";
                        conn.Execute(strSQL, new { p0 = this.processID, p1 = this.revisione, p2 = preced.processID, p3 = preced.revisione, p4 = var.idVariante });
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
                    processo prec = new processo(this.Tenant, this.processiPrec[i]);
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
                    processo succ = new processo(this.Tenant, this.processiSucc[i]);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                // trovo il numero di processowners
                String strSQL = "SELECT COUNT(users.userID) FROM processo INNER JOIN processOwners "
                 + "ON(processo.ProcessID = processOwners.process) INNER JOIN users ON(users.userID = processOwners.user) "
                 + "WHERE processo.ProcessID = @p0";
                this._numProcessOwners = conn.ExecuteScalar<int>(strSQL, new { p0 = this.processID });
                this._processOwners = new User[this._numProcessOwners];

                // creo l'istanza degli utenti
                strSQL = "SELECT processo.ProcessID, users.userID FROM processo INNER JOIN processOwners "
                 + "ON(processo.ProcessID = processOwners.process) INNER JOIN users ON(users.userID = processOwners.user) "
                 + "WHERE processo.ProcessID = @p0";
                int i = 0;
                foreach (var row in conn.Query<ProcessOwnerRow>(strSQL, new { p0 = this.processID }))
                {
                    this._processOwners[i] = new User(row.userID);
                    i++;
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
                String strSQL = "DELETE FROM processOwners WHERE process = @p0 AND user = @p1";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                conn.Execute(strSQL, new { p0 = this.processID, p1 = currProcOwner.username });
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
                    String strSQL = "INSERT INTO processOwners(process, user) VALUES(@p0, @p1)";
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    conn.Execute(strSQL, new { p0 = this.processID, p1 = newProcOwner.username });
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
                            TaskVariante tsk = new TaskVariante(this.Tenant, this.subProcessi[i], var);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                this._numPostazioni = 0;
                int? numPost = conn.ExecuteScalar<int?>("SELECT COUNT(idpostazioni) FROM postazioni WHERE mainProc = @p0 AND revProc = @p1", new { p0 = this.processID, p1 = this.revisione });
                if (numPost.HasValue)
                {
                    this._numPostazioni = numPost.Value;
                }
                this._elencoPostazioni = new Postazione[this.numPostazioni];
                int i = 0;
                foreach (var idPost in conn.Query<int>("SELECT idpostazioni FROM postazioni WHERE mainProc = @p0 AND revProc = @p1", new { p0 = this.processID, p1 = this.revisione }))
                {
                    if (i < this.numPostazioni)
                    {
                        this._elencoPostazioni[i] = new Postazione(this.Tenant, idPost);
                        i++;
                    }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    conn.Execute("INSERT INTO taskspostazioni(postazione, idTask, revTask) VALUES(@p0, @p1, @p2)",
                        new { p0 = postID.id, p1 = this.processID, p2 = this.revisione }, trans);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM taskspostazioni WHERE idTask = @p0 AND revTask = @p1",
                        new { p0 = this.processID, p1 = this.revisione }, trans);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                this._elencoPostazioniTask = new List<Postazione>();
                foreach (var idPost in conn.Query<int>("SELECT postazione FROM repartipostazioniattivita WHERE processo = @p0 AND revProc = @p1", new { p0 = this.processID, p1 = this.revisione }))
                {
                    this._elencoPostazioniTask.Add(new Postazione(this.Tenant, idPost));
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    conn.Execute("UPDATE taskspostazioni SET postazione = @p0 WHERE idTask = @p1 AND revTask = @p2",
                        new { p0 = postID.id, p1 = this.processID, p2 = this.revisione }, trans);
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
                    
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                
                try
                {
                    conn.Execute("INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, isVSM, posx, posy, attivo) " +
                            "VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, 1)",
                            new { p0 = this.processID, p1 = (this.revisione + 1), p2 = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"),
                                  p3 = this.processName, p4 = this.processDescription, p5 = isVSM, p6 = this.posX, p7 = this.posY }, trans);

                    // disattivo la revisione precedente
                    conn.Execute("UPDATE processo SET attivo = 0 WHERE processID = @p0 AND revisione = @p8",
                        new { p0 = this.processID, p8 = this.revisione }, trans);

                    // Copio i padri
                    List<int[]> elencoPadri = new List<int[]>();
                    foreach (var row in conn.Query<PadreCopyRow>("SELECT padre, revPadre, variante, posx, posy FROM processipadrifigli WHERE task = @p0"
                        + " AND revTask = @p8", new { p0 = this.processID, p8 = this.revisione }, trans))
                    {
                        int[] arrPadre = new int[5];
                        arrPadre[0] = row.padre;
                        arrPadre[1] = row.revPadre;
                        arrPadre[2] = row.variante;
                        arrPadre[3] = row.posx;
                        arrPadre[4] = row.posy;
                        elencoPadri.Add(arrPadre);
                    }
                    for (int i = 0; i < elencoPadri.Count; i++)
                    {
                        conn.Execute("INSERT INTO processipadrifigli(task, revTask, padre, revPadre, variante, posx, posy) VALUES("
                            + "@p0, @p1, @p9"
                            + ", @p10, @p11, @p12"
                                + ", @p13)",
                            new { p0 = this.processID, p1 = (this.revisione + 1), p9 = elencoPadri[i][0], p10 = elencoPadri[i][1],
                                  p11 = elencoPadri[i][2], p12 = elencoPadri[i][3], p13 = elencoPadri[i][4] }, trans);

                    }

                    List<int[]> elencoPrecedenti = new List<int[]>();
                    foreach (var row in conn.Query<PrecSuccCopyRow>("SELECT prec, revPrec, variante, relazione FROM precedenzeprocessi WHERE succ = "
                        + "@p0 AND revSucc = @p8", new { p0 = this.processID, p8 = this.revisione }, trans))
                    {
                        int[] arrPrec = new int[4];
                        arrPrec[0] = row.prec;
                        arrPrec[1] = row.revPrec;
                        arrPrec[2] = row.variante;
                        arrPrec[3] = row.relazione;
                        elencoPrecedenti.Add(arrPrec);
                    }
                    for (int i = 0; i < elencoPrecedenti.Count; i++)
                    {
                        conn.Execute("INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES("
                            + "@p14, "
                            + "@p15, "
                            + "@p0, "
                            + "@p1, "
                            + "@p16, "
                            + "@p17"
                            + ")",
                            new { p14 = elencoPrecedenti[i][0], p15 = elencoPrecedenti[i][1], p0 = this.processID,
                                  p1 = (this.revisione + 1), p16 = elencoPrecedenti[i][2], p17 = elencoPrecedenti[i][3] }, trans);
                    }

                    List<int[]> elencoSuccessivi = new List<int[]>();
                    foreach (var row in conn.Query<PrecSuccCopyRow>("SELECT succ, revSucc, variante, relazione FROM precedenzeprocessi WHERE prec = "
                        + "@p0 AND revPrec = @p8", new { p0 = this.processID, p8 = this.revisione }, trans))
                    {
                        int[] arrSucc = new int[4];
                        arrSucc[0] = row.succ;
                        arrSucc[1] = row.revSucc;
                        arrSucc[2] = row.variante;
                        arrSucc[3] = row.relazione;
                        elencoSuccessivi.Add(arrSucc);
                    }
                    for (int i = 0; i < elencoSuccessivi.Count; i++)
                    {
                        conn.Execute("INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES("
                            + "@p0, "
                            + "@p1, "
                            + "@p18, "
                            + "@p19, "
                            + "@p20, "
                            + "@p21"
                            + ")",
                            new { p0 = this.processID, p1 = (this.revisione + 1), p18 = elencoSuccessivi[i][0], p19 = elencoSuccessivi[i][1],
                                  p20 = elencoSuccessivi[i][2], p21 = elencoSuccessivi[i][3] }, trans);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // Copio il processo originale
                    conn.Execute("INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, ProcessoPadre, revPadre, isVSM, posx, posy, attivo) " +
                            "VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, 1)",
                            new { p0 = this.processID, p1 = (this.revisione + 1), p2 = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"),
                                  p3 = this.processName, p4 = this.processDescription, p5 = this.processoPadre, p6 = this.revPadre,
                                  p7 = isVSM, p8 = this.posX, p9 = this.posY }, trans);

                    // Disattivo la revisione precedente
                    conn.Execute("UPDATE processo SET attivo = 0 WHERE processID = @p0 AND revisione = @p10",
                        new { p0 = this.processID, p10 = this.revisione }, trans);
                    
                    // Copio i KPIs

                    // Trovo l'id massimo
                    int maxKPI = 0;
                    int? maxKpiId = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM kpi_description", transaction: trans);
                    if (maxKpiId.HasValue)
                    {
                        maxKPI = maxKpiId.Value + 1;
                    }
                    List<Kpi> KPIS = new List<Kpi>();
                    foreach (var id in conn.Query<int>("SELECT id FROM kpi_description "
                        + " WHERE idprocesso = @p0 AND revisione = @p1", new { p0 = this.processID, p1 = this.revisione }, trans))
                    {
                        KPIS.Add(new Kpi(this.Tenant, id));
                    }
                    for(int j = 0; j < KPIS.Count; j++)
                    {
                        conn.Execute("INSERT INTO kpi_description(id, name, description, idprocesso, revisione, attivo, baseval) "
                            + "VALUES(@pMaxKPI, @pName, @pDesc,"
                            + "@p0, @p1, 1, @pBaseVal)",
                            new { pMaxKPI = maxKPI, pName = KPIS[j].name, pDesc = KPIS[j].description, p0 = this.processID, p1 = this.revisione, pBaseVal = KPIS[j].baseVal }, trans);
                        maxKPI++;
                    }
                    
                    // COPIO LE VARIANTI
                    // Associo questo processo alle varianti cui appartiene il padre.
                    List<int> variantiElenco = new List<int>();
                    foreach (var v in conn.Query<int>("SELECT variante FROM variantiprocessi WHERE processo = @p0"
                        + " AND revProc = @p1", new { p0 = this.processID, p1 = this.revisione }, trans))
                    {
                        variantiElenco.Add(v);
                    }
                    for (int i = 0; i < variantiElenco.Count; i++)
                    {
                        conn.Execute("INSERT INTO variantiprocessi(variante, processo, revProc) VALUES("
                            + "@pVariante, @p0, @p1)",
                            new { pVariante = variantiElenco[i], p0 = this.processID, p1 = this.revisione }, trans);
                    }

                    // Copio le relazioni di precedenza del processo per variante
                    List<int>[] elencoPrecedenti = new List<int>[4];
                    elencoPrecedenti[0] = new List<int>();
                    elencoPrecedenti[1] = new List<int>();
                    elencoPrecedenti[2] = new List<int>();
                    elencoPrecedenti[3] = new List<int>();
                    foreach (var row in conn.Query<PrecSuccCopyRow>("SELECT prec, revPrec, variante, relazione FROM precedenzeprocessi "
                        + " WHERE succ = @p0 AND revSucc = @p1", new { p0 = this.processID, p1 = this.revisione }, trans))
                    {
                        elencoPrecedenti[0].Add(row.prec);
                        elencoPrecedenti[1].Add(row.revPrec);
                        elencoPrecedenti[2].Add(row.variante);
                        elencoPrecedenti[3].Add(row.relazione);
                    }
                    for (int j = 0; j < elencoPrecedenti[0].Count; j++)
                    {
                        conn.Execute("INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES "
                        + "(@pPrec0, @pPrec1"
                        + ", @p0, @p1"
                        + ", @pPrec2"
                        + ", @pPrec3)",
                        new { pPrec0 = elencoPrecedenti[0][j], pPrec1 = elencoPrecedenti[1][j], p0 = this.processID, p1 = this.revisione,
                              pPrec2 = elencoPrecedenti[2][j], pPrec3 = elencoPrecedenti[3][j] }, trans);
                    }

                    // Copio le relazioni con i successivi per variante
                    List<int>[] elencoSuccessivi = new List<int>[4];
                    elencoSuccessivi[0] = new List<int>();
                    elencoSuccessivi[1] = new List<int>();
                    elencoSuccessivi[2] = new List<int>();
                    elencoSuccessivi[3] = new List<int>();
                    foreach (var row in conn.Query<PrecSuccCopyRow>("SELECT succ, revSucc, variante, relazione FROM precedenzeprocessi "
                        + "WHERE prec = @p0 AND revPrec = @p1", new { p0 = this.processID, p1 = this.revisione }, trans))
                    {
                        elencoSuccessivi[0].Add(row.succ);
                        elencoSuccessivi[1].Add(row.revSucc);
                        elencoSuccessivi[2].Add(row.variante);
                        elencoSuccessivi[3].Add(row.relazione);
                    }
                    for (int j = 0; j < elencoSuccessivi[0].Count; j++)
                    {
                        conn.Execute("INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES "
                            + "(@p0, @p1"
                            + ", @pSucc0, @pSucc1"
                            + ", @pSucc2"
                            + ", @pSucc3)",
                            new { p0 = this.processID, p1 = this.revisione, pSucc0 = elencoSuccessivi[0][j], pSucc1 = elencoSuccessivi[1][j],
                                  pSucc2 = elencoSuccessivi[2][j], pSucc3 = elencoSuccessivi[3][j] }, trans);
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
                    rt = copiaFigli(new processo(this.Tenant, this.processID, (this.revisione + 1)));
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                bool resProc = false;
                try
                {
                    List<int[]> elencoFigli = new List<int[]>();
                    foreach (var row in conn.Query<FiglioCopyRow>("SELECT processID, revisione FROM processo WHERE processoPadre = @p0 AND revPadre = @p1",
                        new { p0 = this.processID, p1 = this.revisione }, trans))
                    {
                        int[] procFigli = new int[2];
                        procFigli[0] = row.processID;
                        procFigli[1] = row.revisione;
                        elencoFigli.Add(procFigli);
                    }

                    for (int i = 0; i < elencoFigli.Count; i++)
                    {
                        int newID = 0;
                        int? maxId = conn.ExecuteScalar<int?>("SELECT MAX(processID) FROM processo", transaction: trans);
                        if (maxId.HasValue)
                        {
                            newID = maxId.Value + 1;
                        }
                        processo figlio = new processo(this.Tenant, elencoFigli[i][0], elencoFigli[i][1]);
                        conn.Execute("INSERT INTO processo(processID, revisione, dataRevisione, Name, Description, processoPadre, "
                        + "revPadre, isVSM, posx, posy, attivo) VALUES(@p0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)",
                        new { p0 = newID, p1 = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), p2 = figlio.processName, p3 = figlio.processDescription,
                              p4 = newPrc.processID, p5 = newPrc.revisione, p6 = figlio.isVSM, p7 = figlio.posX, p8 = figlio.posY, p9 = figlio.ToString() }, trans);

                        int[] idS = new int[2];
                        idS[0] = figlio.processID;
                        idS[1] = newID;
                        idOldNew.Add(idS);


                        // Copio i KPIs
                        this.subProcessi[i].loadKPIs();
                        for (int h = 0; h < this.subProcessi[i].numKPIs; h++)
                        {
                            int maxKPI = 0;
                            int? maxKpiId = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM kpi_description", transaction: trans);
                            if (maxKpiId.HasValue)
                            {
                                maxKPI = maxKpiId.Value + 1;
                            }
                            conn.Execute("INSERT INTO kpi_description(id, name, description, idprocesso, revisione, attivo, baseval)"
                                + " VALUES(@p0, @p1, @p2, @p3, 0, @p4, @p5)",
                                new { p0 = maxKPI, p1 = this.subProcessi[i].KPIs[h].name, p2 = this.subProcessi[i].KPIs[h].description,
                                      p3 = newID, p4 = 1, p5 = this.subProcessi[i].KPIs[h].baseVal }, trans);
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
                    foreach (var v in conn.Query<int>("SELECT DISTINCT(variantiprocessi.variante) FROM processo AS padre INNER JOIN processo AS figlio ON "
                    + "(padre.processID = figlio.processoPadre AND padre.revisione = figlio.revpadre) INNER JOIN variantiprocessi "
                    + " ON(variantiprocessi.processo = figlio.processID AND variantiprocessi.revProc = figlio.revisione) "
                    + " WHERE padre.processID = @p0 AND padre.revisione = @p1", new { p0 = this.processID, p1 = this.revisione }))
                    {
                        varFigli.Add(new variante(this.Tenant, v));
                    }

                    trans = conn.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < varFigli.Count; i++)
                        {
                            List<int[]> idPrecSuccOld = new List<int[]>();
                            variante var = new variante(this.Tenant, varFigli[i].idVariante);
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
                                    conn.Execute("INSERT INTO variantiprocessi(variante, processo, revProc) VALUES (@p0, @p1, 0)",
                                        new { p0 = var.idVariante, p1 = newID }, trans);
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
                                    conn.Execute("INSERT INTO precedenzeprocessi(prec, revPrec, succ, revSucc, variante, relazione) VALUES (@p0, 0, @p1, 0, @p2, @p3)",
                                        new { p0 = newPrec, p1 = newSucc, p2 = var.idVariante, p3 = idPrecSuccOK[z][2] }, trans);
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
                    this.subProcessi[i].copiaFigli(new processo(this.Tenant, idOldNew[i][1]));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        rt = true;
                        conn.Execute("INSERT INTO processipadrifigli(task, revTask, padre, revPadre, variante) VALUES("
                            + "@p0, @p1, @p2, @p3, @p4)",
                            new { p0 = prc.Task.processID, p1 = prc.Task.revisione, p2 = this.processID, p3 = this.revisione, p4 = prc.variant.idVariante }, tr);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                foreach (var row in conn.Query<ImplosioneRow>("SELECT DISTINCT productionplan.processo, productionplan.revisione, productionplan.variante FROM "
                    + "productionplan INNER JOIN tasksproduzione ON (productionplan.id = tasksproduzione.idArticolo AND "
                    + " productionplan.anno = tasksproduzione.annoArticolo) "
                    + " WHERE tasksproduzione.origTask = @p0"
                    + " AND tasksproduzione.revOrigTask = @p1",
                    new { p0 = this.processID, p1 = this.revisione }))
                {
                    ProcessoVariante prcv = new ProcessoVariante(this.Tenant, new processo(this.Tenant, row.processo, row.revisione), new variante(this.Tenant, row.variante));
                    prcv.loadReparto();
                    prcv.process.loadFigli(prcv.variant);
                    this.ImplosioneProdotti.Add(prcv);
                }

                conn.Close();
            }
        }
    
    }

 public class NearTask
    {
        protected String Tenant;
        public int NearTaskID { get; set; }
        public String NearTaskName { get; set; }
        public int ConstraintType { get; set; }
        public String ConstraintTypeDesc { get; set; }

        public NearTask(String Tenant)
        {
            this.Tenant = Tenant;

            this.NearTaskID = -1;
            this.NearTaskName = "";
            this.ConstraintType = -1;
            this.ConstraintTypeDesc = "";
        }
    }

    public class ElencoProcessi
    {
        protected String Tenant;

        private List<processo> _Elenco;
        public List<processo> Elenco
        {
            get
            {
                return this._Elenco;
            }
        }

        private class ProcessoIdRow
        {
            public int processID { get; set; }
            public int revisione { get; set; }
        }

        public ElencoProcessi(String Tenant)
        {
            this.Tenant = Tenant;

            this._Elenco = new List<processo>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var row in conn.Query<ProcessoIdRow>("SELECT processo.processID, processo.revisione FROM processo "
                + " WHERE processo.attivo = 1 ORDER BY processo.name"))
            {
                this._Elenco.Add(new processo(this.Tenant, row.processID, row.revisione));
            }
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
        protected String Tenant;

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

        public elencoVarianti(String Tenant)
        {
            this.Tenant = Tenant;

            string strSQL = "SELECT COUNT(*) FROM varianti";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            this._numVarianti = conn.ExecuteScalar<int>(strSQL);
            this._elenco = new variante[this._numVarianti];
            strSQL = "SELECT idVariante FROM varianti ORDER BY idVariante";
            int cont = 0;
            foreach (var id in conn.Query<int>(strSQL))
            {
                if (cont >= this._numVarianti) break;
                this._elenco[cont] = new variante(this.Tenant, id);
                cont++;
            }
            conn.Close();
        }
    }

    public class variante
    {
        protected String Tenant;

        public String log;
        private int _idVariante;
        public int idVariante
        {
            get { return this._idVariante; }
        }

        private class VarianteRow
        {
            public int idvariante { get; set; }
            public String nomeVariante { get; set; }
            public String descVariante { get; set; }
        }

        private class VarianteProcessoRow
        {
            public int processo { get; set; }
            public int revProc { get; set; }
        }

        private String _nomeVariante;
        public String nomeVariante
        {
            get { return this._nomeVariante; }
            set
            {
                if (this.idVariante != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE varianti SET nomeVariante = @p0 WHERE idVariante = @p1", new { p0 = value, p1 = this.idVariante }, trans);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE varianti SET descVariante = @p0 WHERE idVariante = @p1", new { p0 = value, p1 = this.idVariante }, trans);
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

        public variante(String Tenant)
        {
            this.Tenant = Tenant;

            this._idVariante = -1;
            this._nomeVariante = "";
            this._descrizioneVariante = "";
        }

        public variante(String Tenant, int varID)
        {
            this.Tenant = Tenant;

            if (varID >= 0)
            {
                String strSQL = "SELECT idvariante, nomeVariante, descVariante FROM varianti WHERE idvariante = @p0";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var row = conn.QueryFirstOrDefault<VarianteRow>(strSQL, new { p0 = varID });
                if (row != null)
                {
                    this._idVariante = row.idvariante;
                    this._nomeVariante = row.nomeVariante;
                    this._descrizioneVariante = row.descVariante;
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string strSQL = "SELECT COUNT(processo) FROM variantiprocessi INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante)"
                    + " WHERE varianti.idvariante = @p0";
                this._numProcessi = conn.ExecuteScalar<int>(strSQL, new { p0 = this.idVariante });
                strSQL = "SELECT processo, revProc FROM variantiprocessi INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante)"
                    + " WHERE varianti.idvariante = @p0";
                foreach (var row in conn.Query<VarianteProcessoRow>(strSQL, new { p0 = this.idVariante }))
                {
                    this._processi.Add(new processo(this.Tenant, row.processo, row.revProc));
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

            ProcessoVariante prcVar = new ProcessoVariante(this.Tenant, prc, new variante(this.Tenant, this.idVariante));
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM variantiprocessi WHERE variante = @p0"
                        + " AND processo = @p1"
                        + " AND revProc = @p2", new { p0 = this.idVariante, p1 = prc.processID, p2 = prc.revisione }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("DELETE FROM varianti WHERE idvariante = @p0", new { p0 = this.idVariante }, tr);
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
            
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? maxVal = conn.ExecuteScalar<int?>("SELECT MAX(idvariante) FROM varianti");
            if (maxVal.HasValue)
            {
                ret = maxVal.Value + 1;
            }
            else
            {
                ret = 0;
            }

            MySqlTransaction trans = conn.BeginTransaction();
            try
            {
                conn.Execute("INSERT INTO varianti(idVariante, nomeVariante, descVariante) VALUES(@p0, @p1, @p2)", new { p0 = ret, p1 = nome, p2 = desc }, trans);
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
        protected String Tenant;
        public String log;

        private class ProcessoVarianteRow
        {
            public int processID { get; set; }
            public String ExternalID { get; set; }
            public int measurementUnit { get; set; }
        }

        private class ProcessoVarianteByExternalRow
        {
            public int processID { get; set; }
            public int revisione { get; set; }
            public int variante { get; set; }
            public int measurementUnit { get; set; }
        }

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
                    MeasurementUnit mu = new MeasurementUnit(this.Tenant, value);
                    if(mu.ID!=-1)
                    { 
                        MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                        conn.Open();
                        MySqlTransaction tr = conn.BeginTransaction();
                        try
                        {
                            conn.Execute("UPDATE variantiprocessi SET measurementUnit = @p0"
                                + " WHERE variante = @p1"
                                + " AND processo = @p2", new { p0 = value, p1 = this.variant.idVariante, p2 = this.process.processID }, tr);
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
                Reparto rp = new Reparto(this.Tenant);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? reparto = conn.ExecuteScalar<int?>("SELECT reparto FROM productionplan WHERE processo = @p0"
                    + " AND revisione = @p1"
                    + " AND variante = @p2"
                    + " ORDER BY anno DESC, id DESC", new { p0 = this.process.processID, p1 = this.process.revisione, p2 = this.variant.idVariante });
                if (reparto.HasValue)
                {
                    rp = new Reparto(this.Tenant, reparto.Value);
                }
                conn.Close();
                return rp;
            }
        }

        public List<ModelParameter> Parameters;

        public ProcessoVariante(String Tenant, processo prc, variante vr)
        {
            this.Tenant = Tenant;

            // Controllo se la variante appartiene al processo (esiste almeno un figlio che ha questa variante)
            bool found = false;
            this.Parameters = new List<ModelParameter>();
            this._ExternalID = "";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<ProcessoVarianteRow>("SELECT processo.processID, variantiprocessi.ExternalID, variantiprocessi.measurementUnit FROM processo INNER JOIN variantiprocessi ON(variantiprocessi.processo = processo.processID "
                + " AND processo.revisione = variantiprocessi.revProc) WHERE variantiprocessi.variante = @p0" +
                " AND processo.processID = @p1 AND processo.revisione = @p2", new { p0 = vr.idVariante, p1 = prc.processID, p2 = prc.revisione });
            if (row != null)
            {
                found = true;
            }
            

            if (found == true)
            {
                this._process = prc;
                this._variant = vr;
                if(row.ExternalID != null)
                { 
                this._ExternalID = row.ExternalID;
                }
                this._MeasurementUnitID = row.measurementUnit;
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
            conn.Close();
        }

        public ProcessoVariante(String Tenant, String ExternalID)
        {
            this.Tenant = Tenant;

            // Controllo se la variante appartiene al processo (esiste almeno un figlio che ha questa variante)
            bool found = false;
            this.Parameters = new List<ModelParameter>();
            this._ExternalID = "";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<ProcessoVarianteByExternalRow>("SELECT processo.processID, processo.revisione, variantiprocessi.variante, variantiprocessi.measurementUnit FROM processo INNER JOIN variantiprocessi ON(variantiprocessi.processo = processo.processID "
                + " AND processo.revisione = variantiprocessi.revProc) WHERE variantiprocessi.ExternalID = @p0", new { p0 = ExternalID });
            if (row != null)
            {
                found = true;
            }


            if (found == true)
            {
                this._process = new processo(this.Tenant, row.processID, row.revisione);
                this._variant = new variante(this.Tenant, row.variante);
                    this._ExternalID = ExternalID;
                this._MeasurementUnitID = row.measurementUnit;
            }
            else
            {
                this._process = null;
                this._variant = null;
                this._ExternalID = "";
                this._MeasurementUnitID = -1;
            }
            conn.Close();
        }

        public void loadReparto()
        {
            this._RepartiProduttivi = new List<Reparto>();
            if (this.process != null && this.variant != null && this.process.processID != -1 && this.variant.idVariante != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                foreach (var idReparto in conn.Query<int>("SELECT idReparto FROM repartiprocessi WHERE processID = @p0"
                    + " AND revisione = @p1 AND variante = @p2", new { p0 = this.process.processID, p1 = this.process.revisione, p2 = this.variant.idVariante }))
                {
                    this._RepartoProduttivo = new Reparto(this.Tenant, idReparto);
                    this._RepartiProduttivi.Add(new Reparto(this.Tenant, idReparto));
                }
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("INSERT INTO repartiprocessi(idReparto, processID, revisione, variante) VALUES(@p0, "
                            + "@p1, "
                            + "@p2, "
                            + "@p3)", new { p0 = rp.id, p1 = this.process.processID, p2 = this.process.revisione, p3 = this.variant.idVariante }, tr);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM repartiprocessi WHERE idReparto = @p0"
                        + " AND processID = @p1"
                        + " AND revisione = @p2"
                        + " AND variante = @p3", new { p0 = rp.id, p1 = this.process.processID, p2 = this.process.revisione, p3 = this.variant.idVariante }, tr);
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
                    variante var = new variante(this.Tenant);
                    int newVarID = var.add("New version - copia da " + this.process.processName + " - " + this.variant.nomeVariante, "New version - copia da " + this.process.processName + " - " + this.variant.nomeVariante);
                    if (newVarID != -1)
                    {
                        var = new variante(this.Tenant, newVarID);
                        bool ckAddVar = dest.addVariante(var);
                        if (ckAddVar == true)
                        {
                            ProcessoVariante nuovoProcVar = new ProcessoVariante(this.Tenant, dest, var);
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
                                    checkCopiaProcessi = nuovoProcVar.process.linkProcessoVariante(new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant));
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
                                            checkCopiaPrecedenze = nuovoProcVar.process.subProcessi[i].addProcessoSuccessivo(new processo(this.Tenant, this.process.subProcessi[i].processiSucc[j]), var, this.process.subProcessi[i].ConstraintType[j]);
                                        }
                                    }
                                }

                                // Se copio anche i tempi ciclo (ed il resto è andato a buon fine)
                                if (copiaTempiCiclo == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.Tenant, this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant);
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
                                            TaskVariante corrente =new TaskVariante(this.Tenant, this.process.subProcessi[j], this.variant);
                                            corrente.loadPostazioni();
                                            TaskVariante eccolo = new TaskVariante(this.Tenant, this.process.subProcessi[j], var);
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
                                        TaskVariante orig = new TaskVariante(this.Tenant, this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant);
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
                                        TaskVariante orig = new TaskVariante(this.Tenant, this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadWorkInstructions();
                                        for (int j = 0; j < orig.WorkInstructions.Count; j++)
                                        {
                                            //nuovo.loadWorkInstructions();
                                            //nuovo.WorkInstructions.Add(nuovo.WorkInstructions[i]);
                                            KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(this.Tenant, orig.WorkInstructions[j].WI.ID, orig.WorkInstructions[j].WI.Version);
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
                    variante var = new variante(this.Tenant);
                    int newVarID = var.add(nomeVariante, descVariante);
                    retVarID = newVarID;
                    if (newVarID != -1)
                    {
                        var = new variante(this.Tenant, newVarID);
                        bool ckAddVar = dest.addVariante(var);
                        if (ckAddVar == true)
                        {
                            ProcessoVariante nuovoProcVar = new ProcessoVariante(this.Tenant, dest, var);
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
                                    checkCopiaProcessi = nuovoProcVar.process.linkProcessoVariante(new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant));
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
                                            checkCopiaPrecedenze = nuovoProcVar.process.subProcessi[i].addProcessoSuccessivo(new processo(this.Tenant, this.process.subProcessi[i].processiSucc[j]), var, this.process.subProcessi[i].ConstraintType[j]);
                                        }
                                    }
                                }

                                // Se copio anche i tempi ciclo (ed il resto è andato a buon fine)
                                if (copiaTempiCiclo == true && checkCopiaProcessi == true && checkCopiaPrecedenze == true)
                                {
                                    for (int i = 0; i < this.process.subProcessi.Count; i++)
                                    {
                                        TaskVariante orig = new TaskVariante(this.Tenant, this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant);
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
                                            TaskVariante corrente = new TaskVariante(this.Tenant, this.process.subProcessi[j], this.variant);
                                            corrente.loadPostazioni();
                                            TaskVariante eccolo = new TaskVariante(this.Tenant, this.process.subProcessi[j], var);
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
                                        TaskVariante orig = new TaskVariante(this.Tenant, this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant);
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
                                        TaskVariante orig = new TaskVariante(this.Tenant, this.process.subProcessi[i], this.variant);
                                        TaskVariante nuovo = new TaskVariante(this.Tenant, this.process.subProcessi[i], nuovoProcVar.variant);
                                        orig.loadWorkInstructions();
                                        for (int j = 0; j < orig.WorkInstructions.Count; j++)
                                        {
                                            KIS.App_Sources.WorkInstructions.WorkInstruction currWI = new App_Sources.WorkInstructions.WorkInstruction(this.Tenant, orig.WorkInstructions[j].WI.ID, orig.WorkInstructions[j].WI.Version);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                foreach (var paramID in conn.Query<int>("SELECT paramID FROM modelparameters WHERE processID = @p0"
                    + " AND processRev = @p1"
                    + " AND varianteID = @p2"
                    + " ORDER BY sequence, paramname", new { p0 = this.process.processID, p1 = this.process.revisione, p2 = this.variant.idVariante }))
                {
                    this.Parameters.Add(new ModelParameter(this.Tenant, this.process.processID,
                        this.process.revisione, this.variant.idVariante, paramID));
                }
                conn.Close();
            }
        }

        public Boolean addParameter(String name, String description, ProductParametersCategory category, Boolean isFixed,
            Boolean isRequired)
        {
            Boolean ret = false;
            if (this.process != null && this.variant != null && this.process.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? maxVal = conn.ExecuteScalar<int?>("SELECT MAX(paramID) FROM ModelParameters WHERE processID = @p0"
                    + " AND processRev = @p1"
                    + " AND varianteID = @p2", new { p0 = this.process.processID, p1 = this.process.revisione, p2 = this.variant.idVariante });
                int maxID = 0;
                if (maxVal.HasValue)
                {
                    maxID = maxVal.Value + 1;
                }

                int maxSequence = maxID;

                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("INSERT INTO modelparameters(processid, processrev, varianteID, paramID, paramCategory, "
                        + " paramName, paramDescription, isFixed, isRequired, sequence) VALUES(@p0, "
                        + "@p1, "
                        + "@p2, "
                        + "@p3, "
                        + "@p4, "
                        + "@p5, @p6, @p7, "
                        + "@p8, @p9)", new { p0 = this.process.processID, p1 = this.process.revisione, p2 = this.variant.idVariante, p3 = maxID, p4 = category.ID, p5 = name, p6 = description, p7 = isFixed, p8 = isRequired, p9 = maxSequence }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message + " INSERT INTO modelparameters(processid, processrev, varianteID, paramID, paramCategory, paramName, paramDescription, isFixed, isRequired, sequence) VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean deleteParameter(int paramID)
        {
            Boolean ret = false;
            if (this.process != null && this.variant != null && this.process.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM Modelparameters WHERE processID = @p0"
                        + " AND processRev = @p1"
                        + " AND varianteID = @p2"
                        + " AND paramID = @p3", new { p0 = this.process.processID, p1 = this.process.revisione, p2 = this.variant.idVariante, p3 = paramID }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message + " DELETE FROM Modelparameters WHERE processID = @p0 AND processRev = @p1 AND varianteID = @p2 AND paramID = @p3";
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    try
                    {
                        conn.Execute("UPDATE variantiprocessi SET ExternalID = @p0 WHERE variante = @p1 AND processo = @p2"
                            + " AND revProc = @p3", new { p0 = value, p1 = this.variant.idVariante, p2 = this.process.processID, p3 = this.process.revisione });
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
                this.measurementUnit = new MeasurementUnit(this.Tenant, this.MeasurementUnitID);
            }
            else
            {
                this.measurementUnit = null;
            }
        }
    }

    public class ElencoMacroProcessiVarianti
    {
        protected String Tenant;
        public String log;

        public List<ProcessoVariante> elenco;

        public ElencoMacroProcessiVarianti(String Tenant)
        {
            this.Tenant = Tenant;

            macroProcessi macroEl = new macroProcessi(this.Tenant);
            elenco = new List<ProcessoVariante>();
            for (int i = 0; i < macroEl.Elenco.Count; i++)
            {
                //log += "MACROPROCESSO: "+ macroEl.Elenco[i].processID.ToString() + "<br/>";
                macroEl.Elenco[i].loadVariantiFigli();
                for (int j = 0; j < macroEl.Elenco[i].variantiFigli.Count; j++)
                {
                    //log += "Variante: " + macroEl.Elenco[i].variantiFigli[j].idVariante.ToString() + "<br/>";
                    elenco.Add(new ProcessoVariante(this.Tenant, macroEl.Elenco[i], macroEl.Elenco[i].variantiFigli[j]));
                }
            }
        }

    }

    public class ElencoProcessiVarianti
    {
        protected String Tenant;
        // Crea l'elenco di processi e varianti per i figli del processo passato al costruttore

        public List<ProcessoVariante> elencoFigli;

        private class ElencoProcessiVariantiRow
        {
            public int variante { get; set; }
            public int processo { get; set; }
            public int revProc { get; set; }
        }

        public ElencoProcessiVarianti(String Tenant, ProcessoVariante proc)
        {
            this.Tenant = Tenant;

            if (proc.process.processID != -1 && proc.variant.idVariante != -1)
            {
                elencoFigli = new List<ProcessoVariante>();
                for (int i = 0; i < proc.process.subProcessi.Count; i++)
                {
                    proc.process.subProcessi[i].loadVariantiFigli();
                    for (int j = 0; j < proc.process.subProcessi[i].variantiFigli.Count; j++)
                    {
                        elencoFigli.Add(new ProcessoVariante(this.Tenant, proc.process.subProcessi[i], proc.process.subProcessi[i].variantiFigli[j]));
                    }
                }
            }
        }

        public ElencoProcessiVarianti(String Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            elencoFigli = new List<ProcessoVariante>();
            foreach (var row in conn.Query<ElencoProcessiVariantiRow>("SELECT variante, processo, revProc FROM variantiprocessi INNER JOIN processo ON (variantiprocessi.processo = processo.processID "
                + "AND processo.revisione = variantiprocessi.revProc) WHERE processo.attivo = true ORDER BY variante, processo.name"))
            {
                elencoFigli.Add(new ProcessoVariante(this.Tenant, new processo(this.Tenant, row.processo, row.revProc), new variante(this.Tenant, row.variante)));
            }
            conn.Close();
        }

        /* Ritorna processivarianti solo per un tipo di processo */
        public ElencoProcessiVarianti(String tenant, bool isPert)
        {
            this.Tenant = tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            elencoFigli = new List<ProcessoVariante>();
            foreach (var row in conn.Query<ElencoProcessiVariantiRow>("SELECT variante, processo, revProc FROM variantiprocessi INNER JOIN processo ON (variantiprocessi.processo = processo.processID "
                + "AND processo.revisione = variantiprocessi.revProc) WHERE processo.attivo = true" 
                + " AND processo.isVSM = @p0"
                + " ORDER BY variante, processo.name", new { p0 = !isPert }))
            {
                elencoFigli.Add(new ProcessoVariante(this.Tenant, new processo(this.Tenant, row.processo, row.revProc), new variante(this.Tenant, row.variante)));
            }
            conn.Close();
        }

        /* Ritorna processivarianti solo per un tipo di processo, realizzato per un certo cliente */
        public ElencoProcessiVarianti(String tenant, bool isPert, Cliente customer)
        {
            this.Tenant = tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            elencoFigli = new List<ProcessoVariante>();
            foreach (var row in conn.Query<ElencoProcessiVariantiRow>("SELECT variantiprocessi.variante, processo.processID, processo.revisione FROM variantiprocessi INNER JOIN processo ON (variantiprocessi.processo = processo.processID "
                + "AND processo.revisione = variantiprocessi.revProc) INNER JOIN productionplan ON ("
                + "productionplan.processo = processo.processID AND productionplan.revisione = processo.revisione "
                + " AND productionplan.variante = variantiprocessi.variante)"
                + " INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa = commesse.anno) "
                + " WHERE "
                + " commesse.cliente = @p0"
                +" AND processo.attivo = true"
                + " AND processo.isVSM = @p1"
                + " GROUP BY variantiprocessi.variante, processo.processID, processo.revisione"
                + " ORDER BY variante, processo.name", new { p0 = customer.CodiceCliente, p1 = !isPert }))
            {
                elencoFigli.Add(new ProcessoVariante(this.Tenant, new processo(this.Tenant, row.processo, row.revProc), new variante(this.Tenant, row.variante)));
            }
            conn.Close();
        }
    }

    public class TaskVariante
    {
        protected String Tenant;
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

        private class TaskWorkInstructionRow
        {
            public int manualID { get; set; }
            public int manualVersion { get; set; }
        }

        public TaskVariante(String Tenant, processo prc, variante vr)
        {
            this.Tenant = Tenant;

            this.Parameters = new List<ModelTaskParameter>();
            this.WorkInstructions = new List<TaskWorkInstruction>();
            this.WorkInstructionsArchive = new List<TaskWorkInstruction>();
            this._DefaultOperators = new List<User>();
            this._Task = prc;
                this._variant = vr;
                prc.loadFigli(vr);
                this.loadPostazioni();
        }

        private TempiCiclo _Tempi;
        public TempiCiclo Tempi
        {
            get { return this._Tempi; }
        }

        public List<ModelTaskParameter> Parameters;

        public bool loadTempiCiclo()
        {
            bool rt = false;
            if (this.Task != null && this.variant != null)
            {
                this._Tempi = new TempiCiclo(this.Tenant, this.Task.processID, this.Task.revisione, this.variant.idVariante);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute("DELETE FROM processipadrifigli WHERE variante = @p0"
                    + " AND task = @p1 AND revTask = @p2", new { p0 = this.variant.idVariante, p1 = this.Task.processID, p2 = this.Task.revisione }, tr);
                conn.Execute("DELETE FROM precedenzeprocessi WHERE variante = @p0"
                    + " AND ((prec = @p1 AND revPrec = @p2"
                    + ") OR (succ = @p1 AND revSucc = @p2))", new { p0 = this.variant.idVariante, p1 = this.Task.processID, p2 = this.Task.revisione }, tr);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var postazione in conn.Query<int>("SELECT postazione FROM repartipostazioniattivita WHERE processo = @p0"
                 + " AND revProc = @p1 AND variante = @p2", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante }))
            {
                this._PostazioneDiLavoro = new Postazione(this.Tenant, postazione);
                this._PostazioniDiLavoro.Add(new Postazione(this.Tenant, postazione));
            }
            conn.Close();
        }

        public Boolean deleteLinkPostazione(Postazione p)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute("DELETE FROM repartipostazioniattivita WHERE "
                     + " processo = @p0"
                     + " AND revProc = @p1" 
                     + " AND variante = @p2"
                     //+ " AND reparto = " + rp.id.ToString()
                     + " AND postazione = @p3", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante, p3 = p.id }, tr);
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
            Postazione rt = new Postazione(this.Tenant);
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? postazione = conn.ExecuteScalar<int?>("SELECT postazione FROM repartipostazioniattivita WHERE processo = @p0"
                 + " AND revProc = @p1" 
                 + " AND variante = @p2"
                 + " AND reparto = @p3", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante, p3 = rp.id });
            if(postazione.HasValue)
            {
                rt = new Postazione(this.Tenant, postazione.Value);
            }
            conn.Close();
            return rt;
        }

        public void loadParameters()
        {
            this.Parameters = new List<ModelTaskParameter>();
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                foreach (var paramID in conn.Query<int>("SELECT paramID FROM modelTaskparameters WHERE TaskID = @p0"
                    + " AND TaskRev = @p1"
                    + " AND varianteID = @p2"
                    + " ORDER BY sequence, paramname", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante }))
                {
                    this.Parameters.Add(new ModelTaskParameter(this.Tenant, this.Task.processID,
                        this.Task.revisione, this.variant.idVariante, paramID));
                }
                conn.Close();
            }
        }

        public Boolean addParameter(String name, String description, ProductParametersCategory category, Boolean isFixed,
            Boolean isRequired)
        {
            Boolean ret = false;
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? maxVal = conn.ExecuteScalar<int?>("SELECT MAX(paramID) FROM ModelTaskParameters WHERE TaskID = @p0"
                    + " AND TaskRev = @p1"
                    + " AND varianteID = @p2", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante });
                int maxID = 0;
                if (maxVal.HasValue)
                {
                    maxID = maxVal.Value + 1;
                }

                int maxSequence = maxID;

                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("INSERT INTO modelTaskparameters(TaskID, TaskRev, varianteID, paramID, paramCategory, "
                        + " paramName, paramDescription, isFixed, isRequired, sequence) VALUES(@p0, "
                        + "@p1, "
                        + "@p2, "
                        + "@p3, "
                        + "@p4, "
                        + "@p5, @p6, @p7, "
                        + "@p8, @p9)", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante, p3 = maxID, p4 = category.ID, p5 = name, p6 = description, p7 = isFixed, p8 = isRequired, p9 = maxSequence }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " INSERT INTO modelTaskparameters(TaskID, TaskRev, varianteID, paramID, paramCategory, paramName, paramDescription, isFixed, isRequired, sequence) VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean deleteParameter(int paramID)
        {
            Boolean ret = false;
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM ModelTaskparameters WHERE TaskID = @p0"
                        + " AND TaskRev = @p1"
                        + " AND varianteID = @p2"
                        + " AND paramID = @p3", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante, p3 = paramID }, tr);
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " DELETE FROM ModelTaskparameters WHERE TaskID = @p0 AND TaskRev = @p1 AND varianteID = @p2 AND paramID = @p3";
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var row in conn.Query<TaskWorkInstructionRow>("SELECT manualID, manualVersion, validityInitialDate, expiryDate, sequence, isActive FROM tasksmanuals WHERE taskID = @TaskId AND taskRev = @TaskRev AND taskVarianti = @variante "
                +" AND isActive = true AND ExpiryDate >= @ExpiryDate", new { TaskId = this.Task.processID, TaskRev = this.Task.revisione, variante = this.variant.idVariante, ExpiryDate = DateTime.UtcNow.ToString("yyyy-MM-dd") }))
            {
                TaskWorkInstruction curr = new TaskWorkInstruction(this.Tenant, this.Task.processID, this.Task.revisione, this.variant.idVariante,
                    row.manualID, row.manualVersion);

                this.WorkInstructions.Add(curr);
            }
            conn.Close();
        }

        public void loadWorkInstructionsArchive()
        {
            this.WorkInstructionsArchive = new List<TaskWorkInstruction>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var row in conn.Query<TaskWorkInstructionRow>("SELECT manualID, manualVersion FROM tasksmanuals WHERE taskID = @TaskId AND taskRev = @TaskRev AND taskVarianti = @variante "
                + " AND ExpiryDate < @ExpiryDate", new { TaskId = this.Task.processID, TaskRev = this.Task.revisione, variante = this.variant.idVariante, ExpiryDate = DateTime.UtcNow.ToString("yyyy-MM-dd") }))
            {
                TaskWorkInstruction curr = new TaskWorkInstruction(this.Tenant, this.Task.processID, this.Task.revisione, this.variant.idVariante,
                    row.manualID, row.manualVersion);
                this.WorkInstructionsArchive.Add(curr);
            }
            conn.Close();
        }

        public void loadDefaultOperators()
        {
            this._DefaultOperators = new List<User>();
            if (this.Task != null && this.variant != null && this.Task.processID > -1 && this.variant.idVariante > -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                foreach (var usr in conn.Query<string>("SELECT user FROM taskusermodel WHERE taskid = @p0"
                    + " AND taskrev = @p1"
                    + " AND variantid = @p2", new { p0 = this.Task.processID, p1 = this.Task.revisione, p2 = this.variant.idVariante }))
                {
                    this._DefaultOperators.Add(new User(usr));
                }
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("INSERT INTO taskusermodel(taskid, taskrev, variantid, user, exclusive) VALUES("
                            +"@TaskID, @TaskRev, @VariantID, @User, @Exclusive)", new { TaskID = this.Task.processID, TaskRev = this.Task.revisione, VariantID = this.variant.idVariante, User = usr, Exclusive = false }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("DELETE FROM taskusermodel WHERE taskid=@TaskID AND taskrev=@TaskRev AND variantid=@VariantID AND user=@User", new { TaskID = this.Task.processID, TaskRev = this.Task.revisione, VariantID = this.variant.idVariante, User = usr }, tr);
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
    }

    public class TempoCiclo
    {
        protected String Tenant;
        public String log;

        private class TempoCicloRow
        {
            public TimeSpan setup { get; set; }
            public TimeSpan tempo { get; set; }
            public TimeSpan tunload { get; set; }
            public bool def { get; set; }
        }

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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE tempiciclo SET def = @p0"
                            + " WHERE processo = @p1"
                            + " AND revisione = @p2"
                            + " AND variante = @p3"
                            + " AND num_op = @p4", new { p0 = value, p1 = this.IdProcesso, p2 = this.RevisioneProcesso, p3 = this.Variante, p4 = this.NumeroOperatori }, tr);
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

        public TempoCiclo(String Tenant, int idProc, int revProc, int var, int num_op)
        {
            this.Tenant = Tenant;

            if (idProc >= 0 && revProc >= 0 && num_op >= 1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var row = conn.QueryFirstOrDefault<TempoCicloRow>("SELECT setup, tempo, tunload, def FROM tempiciclo WHERE processo = @p0"
                    + " AND revisione = @p1 AND num_op = @p2 AND variante = @p3", new { p0 = idProc, p1 = revProc, p2 = num_op, p3 = var });
                if (row != null)
                {
                    this._IdProcesso = idProc;
                    this._RevisioneProcesso = revProc;
                    this._Variante = var;
                    this._NumeroOperatori = num_op;
                    this._TempoSetup = row.setup;
                    this._Tempo = row.tempo;
                    this._TempoUnload = row.tunload;
                    this._Default = row.def;
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM tempiciclo WHERE processo = @p0 AND revisione = @p1"
                        + " AND variante = @p2 AND num_op = @p3", new { p0 = this.IdProcesso, p1 = this.RevisioneProcesso, p2 = this.Variante, p3 = this.NumeroOperatori }, tr);
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
        protected String Tenant;
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

        public TempiCiclo(String Tenant, int idProc, int revProc, int var)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            Tempi = new List<TempoCiclo>();
            foreach (var num_op in conn.Query<int>("SELECT num_op FROM tempiciclo WHERE processo = @p0 AND revisione = @p1"
                + " AND variante = @p2 ORDER BY num_op", new { p0 = idProc, p1 = revProc, p2 = var }))
            {
                Tempi.Add(new TempoCiclo(this.Tenant, idProc, revProc, var, num_op));
            }

            this._IdProcesso = idProc;
            this._RevisioneProcesso = revProc;
            this._Variante = var;

            conn.Close();
        }

        public bool Add(int n_ops, TimeSpan tc, TimeSpan tSetup, bool def)
        {
            bool rt = false;
            if (this.IdProcesso!=-1 && n_ops > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                log += "<br/>"+tc.ToString();
                string strSetup = Math.Floor(tSetup.TotalHours).ToString() + ":" + tSetup.Minutes.ToString() + ":" + tSetup.Seconds.ToString();
                string strTempo = Math.Floor(tc.TotalHours).ToString() + ":" + tc.Minutes.ToString() + ":" + tc.Seconds.ToString();
                try
                {
                    if(def == true)
                    {
                        conn.Execute("UPDATE tempiciclo SET def = false WHERE processo = @p0"
                            + " AND revisione = @p1 AND variante = @p2", new { p0 = this.IdProcesso, p1 = this.RevisioneProcesso, p2 = this.Variante }, tr);
                    }
                    
                    conn.Execute("INSERT INTO tempiciclo(processo, revisione, variante, num_op, setup, tempo, tunload, def) VALUES("
                        + "@p0, "
                        + "@p1, "
                        + "@p2, "
                        + "@p3, "
                        + "@p4, "
                        + "@p5, "
                        + "@p6, "
                        + "@p7)", new { p0 = this.IdProcesso, p1 = this.RevisioneProcesso, p2 = this.Variante, p3 = n_ops, p4 = strSetup, p5 = strTempo, p6 = "00:00:00", p7 = def }, tr);

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
        protected String Tenant;
        public List<processo> Elenco;

        private class ProcessoIdRow
        {
            public int processID { get; set; }
            public int revisione { get; set; }
        }

        public ElencoTasks(String tenant)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            Elenco = new List<processo>();
            foreach (var row in conn.Query<ProcessoIdRow>("SELECT processID, revisione FROM processo WHERE attivo = 1 ORDER BY Name"))
            {
                Elenco.Add(new processo(this.Tenant, row.processID, row.revisione));
            }
            conn.Close();
        }

        /* eseguibili = true Ritorna solo i task eseguibili in produzione, cioè senza figli! */
        public ElencoTasks(String tenant, bool eseguibili)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql;
            if (eseguibili == true)
            {
                sql = "SELECT DISTINCT(processID), revisione FROM processo "
                    + " INNER JOIN processipadrifigli ON (processo.processID = processipadrifigli.task AND "
                    + "processo.revisione = processipadrifigli.revTask) "
                +"WHERE attivo = 1 AND padre IS NOT NULL ORDER BY Name";
            }
            else
            {
                sql = "SELECT processID, revisione FROM processo WHERE attivo = 1 ORDER BY Name";
            }
            Elenco = new List<processo>();
            foreach (var row in conn.Query<ProcessoIdRow>(sql))
            {
                processo tsk = new processo(this.Tenant, row.processID, row.revisione);
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
            conn.Close();
        }
    }

    public class ProductParametersCategory
    {
        protected String Tenant;
        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private class ProductParametersCategoryRow
        {
            public String paramCatName { get; set; }
            public String paramCatDescription { get; set; }
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("UPDATE productparameterscategories SET paramCatName = @p0 WHERE "
                        + " paramCatID = @p1", new { p0 = value, p1 = this.ID }, tr);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("UPDATE productparameterscategories SET paramCatDescription = @p0 WHERE "
                        + " paramCatID = @p1", new { p0 = value, p1 = this.ID }, tr);
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

        public ProductParametersCategory(String Tenant)
        {
            this.Tenant = Tenant;
            this._ID = -1;
            this.Name = "";
            this.Description = "";
        }

        public ProductParametersCategory(String Tenant, int CategoryID)
        {
            this.Tenant = Tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<ProductParametersCategoryRow>("SELECT paramCatName, paramCatDescription FROM productparameterscategories "
                + " WHERE paramCatID = @p0", new { p0 = CategoryID });
            this._ID = -1;
            this._Name = "";
            this._Description = "";

            if(row != null)
            {
                this._ID = CategoryID;
                this._Name = row.paramCatName;
                this._Description = row.paramCatDescription;
            }
            conn.Close();
        }
    }

    public class ProductParametersCategories
    {
        protected String Tenant;
        public String log;

        public List<ProductParametersCategory> Categories;

        public ProductParametersCategories(String Tenant)
        {
            this.Tenant = Tenant;
            this.Categories = new List<ProductParametersCategory>();
        }

        public void loadCategories()
        {
            this.Categories = new List<ProductParametersCategory>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var paramCatID in conn.Query<int>("SELECT paramCatID FROM productparameterscategories ORDER BY paramCatName"))
            {
                this.Categories.Add(new ProductParametersCategory(this.Tenant, paramCatID));
            }
            conn.Close();
        }

        public Boolean Add(String name, String description)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? maxVal = conn.ExecuteScalar<int?>("SELECT MAX(paramCatID) FROM productparameterscategories");
            int maxID = 0;
            if (maxVal.HasValue)
            {
                maxID = maxVal.Value + 1;
            }

            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute("INSERT INTO productparameterscategories(paramCatID, paramCatName, paramCatDescription) "
                    + " VALUES(@p0, @p1, @p2)", new { p0 = maxID, p1 = name, p2 = description }, tr);
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
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();

            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute("DELETE FROM productparameterscategories WHERE paramCatID = @p0", new { p0 = id }, tr);
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
        protected String Tenant;

        private class ModelParameterRow
        {
            public int processID { get; set; }
            public int processRev { get; set; }
            public int varianteID { get; set; }
            public int paramID { get; set; }
            public int paramCategory { get; set; }
            public String paramName { get; set; }
            public String paramDescription { get; set; }
            public bool? isFixed { get; set; }
            public bool? isRequired { get; set; }
            public int? sequence { get; set; }
        }

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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelparameters SET paramCategory = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND processID = @p2"
                            + " AND processRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value.ID, p1 = this.ParameterID, p2 = this.ProcessID, p3 = this.ProcessRev, p4 = this.VarianteID }, tr);
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("UPDATE modelparameters SET paramName = @p0 WHERE "
                        + " paramID = @p1" 
                        + " AND processID = @p2"
                        + " AND processRev = @p3"
                        + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.ProcessID, p3 = this.ProcessRev, p4 = this.VarianteID }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelparameters SET paramDescription = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND processID = @p2"
                            + " AND processRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.ProcessID, p3 = this.ProcessRev, p4 = this.VarianteID }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelparameters SET isFixed = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND processID = @p2"
                            + " AND processRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.ProcessID, p3 = this.ProcessRev, p4 = this.VarianteID }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelparameters SET isRequired = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND processID = @p2"
                            + " AND processRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.ProcessID, p3 = this.ProcessRev, p4 = this.VarianteID }, tr);
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

        public ModelParameter(String Tenant, int processID, int processRev, int variantID, int parameterID)
        {
            this.Tenant = Tenant;
            this._ParameterID = -1;
            this._ProcessID = -1;
            this._ProcessRev = -1;
            this._VarianteID = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<ModelParameterRow>("SELECT processID, processRev, varianteID, paramID, paramCategory, paramName, "
                + " paramDescription, isFixed, isRequired, sequence FROM modelparameters WHERE "
                + " processID = @p0"
                + " AND processRev = @p1"
                + " AND varianteID = @p2"
                + " AND paramID = @p3", new { p0 = processID, p1 = processRev, p2 = variantID, p3 = parameterID });
            if(row != null)
            {
                this._ProcessID = row.processID;
                this._ProcessRev = row.processRev;
                this._VarianteID = row.varianteID;
                this._ParameterID = row.paramID;
                this.ParameterCategory = new ProductParametersCategory(this.Tenant, row.paramCategory);
                if(row.paramName != null)
                { 
                    this._Name = row.paramName;
                }
                if(row.paramDescription != null)
                {
                    this._Description = row.paramDescription;
                }
                if (row.isFixed.HasValue)
                {
                    this._isFixed = row.isFixed.Value;
                }
                if (row.isRequired.HasValue)
                {
                    this._isRequired = row.isRequired.Value;
                }
                if (row.sequence.HasValue)
                {
                    this._Sequence = row.sequence.Value;
                }
            }
            conn.Close();
        }
    }

    public class ModelTaskParameter
    {
        protected String Tenant;

        private class ModelTaskParameterRow
        {
            public int TaskID { get; set; }
            public int TaskRev { get; set; }
            public int varianteID { get; set; }
            public int paramID { get; set; }
            public int paramCategory { get; set; }
            public String paramName { get; set; }
            public String paramDescription { get; set; }
            public bool? isFixed { get; set; }
            public bool? isRequired { get; set; }
            public int? sequence { get; set; }
        }

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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelTaskparameters SET paramCategory = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND TaskID = @p2"
                            + " AND TaskRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value.ID, p1 = this.ParameterID, p2 = this.TaskID, p3 = this.TaskRev, p4 = this.VarianteID }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelTaskparameters SET paramName = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND TaskID = @p2"
                            + " AND TaskRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.TaskID, p3 = this.TaskRev, p4 = this.VarianteID }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelTaskparameters SET paramDescription = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND TaskID = @p2"
                            + " AND TaskRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.TaskID, p3 = this.TaskRev, p4 = this.VarianteID }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelTaskparameters SET isFixed = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND TaskID = @p2"
                            + " AND TaskRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.TaskID, p3 = this.TaskRev, p4 = this.VarianteID }, tr);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE modelTaskparameters SET isRequired = @p0 WHERE "
                            + " paramID = @p1"
                            + " AND TaskID = @p2"
                            + " AND TaskRev = @p3"
                            + " AND varianteID = @p4", new { p0 = value, p1 = this.ParameterID, p2 = this.TaskID, p3 = this.TaskRev, p4 = this.VarianteID }, tr);
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

        public ModelTaskParameter(String Tenant, int TaskID, int TaskRev, int variantID, int parameterID)
        {
            this.Tenant = Tenant;

            this._ParameterID = -1;
            this._TaskID = -1;
            this._TaskRev = -1;
            this._VarianteID = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<ModelTaskParameterRow>("SELECT TaskID, TaskRev, varianteID, paramID, paramCategory, paramName, "
                + " paramDescription, isFixed, isRequired, sequence FROM modelTaskparameters WHERE "
                + " TaskID = @p0"
                + " AND TaskRev = @p1"
                + " AND varianteID = @p2"
                + " AND paramID = @p3", new { p0 = TaskID, p1 = TaskRev, p2 = variantID, p3 = parameterID });
            if (row != null)
            {
                this._TaskID = row.TaskID;
                this._TaskRev = row.TaskRev;
                this._VarianteID = row.varianteID;
                this._ParameterID = row.paramID;
                this.ParameterCategory = new ProductParametersCategory(this.Tenant, row.paramCategory);
                if (row.paramName != null)
                {
                    this._Name = row.paramName;
                }
                if (row.paramDescription != null)
                {
                    this._Description = row.paramDescription;
                }
                if (row.isFixed.HasValue)
                {
                    this._isFixed = row.isFixed.Value;
                }
                if (row.isRequired.HasValue)
                {
                    this._isRequired = row.isRequired.Value;
                }
                if (row.sequence.HasValue)
                {
                    this._Sequence = row.sequence.Value;
                }
            }
            conn.Close();
        }
    }

    public class TaskWorkInstruction
    {
        protected String Tenant;

        public KIS.App_Sources.WorkInstructions.WorkInstruction WI;

        private class TaskWorkInstructionFullRow
        {
            public int manualID { get; set; }
            public int manualVersion { get; set; }
            public DateTime validityInitialDate { get; set; }
            public DateTime expiryDate { get; set; }
            public int sequence { get; set; }
            public bool isActive { get; set; }
        }

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

        public TaskWorkInstruction(String Tenant, int TaskID, int TaskRev, int VariantID, int ManualID, int ManualVersion)
        {
            this.Tenant = Tenant;

            this._TaskID = -1;
            this._TaskRev = -1;
            this._VariantID = -1;
            this.WI = null;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<TaskWorkInstructionFullRow>("SELECT manualID, manualVersion, validityInitialDate, expiryDate, sequence, isActive FROM tasksmanuals WHERE "
                + " taskID = @TaskId AND taskRev = @TaskRev AND taskVarianti = @variante "
                + " AND manualID = @ManualID AND manualVersion=@ManualVersion", new { TaskId = TaskID, TaskRev = TaskRev, variante = VariantID, ManualID = ManualID, ManualVersion = ManualVersion });

            if (row != null)
            {
                this.WI = new App_Sources.WorkInstructions.WorkInstruction(this.Tenant, row.manualID, row.manualVersion);
                this._InitialDate = row.validityInitialDate;
                this._ExpiryDate = row.expiryDate;
                this._Sequence = row.sequence;
                this._IsActive = row.isActive;
                this._TaskID = TaskID;
                this._TaskRev = TaskRev;
                this._VariantID = VariantID;
            }

            conn.Close();
        }

        /*Returns:
         */
         public int Delete()
        {
            int ret = 0;
            if(this.WI!=null && this.WI.ID!=-1 && this.WI.Version!=-1 && this.TaskID!=-1 && this.VariantID!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute("DELETE FROM tasksmanuals WHERE taskId=@TaskID AND TaskRev=@TaskRev AND taskVarianti=@VariantID "
                        + " AND manualID=@ManualID AND manualVersion=@ManualVersion", new { TaskID = TaskID, TaskRev = TaskRev, VariantID = VariantID, ManualID = this.WI.ID, ManualVersion = this.WI.Version }, tr);
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
}