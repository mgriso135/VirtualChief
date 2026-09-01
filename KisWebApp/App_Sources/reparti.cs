/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;

namespace KIS.App_Code
{
    public class Reparto
    {
        protected String Tenant;

        private class RepartoRow
        {
            public int idreparto { get; set; }
            public String nome { get; set; }
            public String descrizione { get; set; }
            public float? cadenza { get; set; }
            public bool splitTasks { get; set; }
            public int anticipoTasks { get; set; }
            public bool ModoCalcoloTC { get; set; }
            public String timezone { get; set; }
        }

        public String err;
        public String log;

        private int _id;
        public int id
        {
            get { return this._id; }
        }

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

        private TimeSpan _Cadenza;
        public TimeSpan Cadenza
        {
            get { return this._Cadenza; }
            set
            {
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    int newCadenza = value.Seconds + value.Minutes * 60 + value.Hours * 3600;
                    string sql = "UPDATE reparti SET cadenza = @cadenza WHERE idReparto = @id";
                    try
                    {
                        conn.Execute(sql, new { cadenza = newCadenza, id = this.id }, trans);
                        this._Cadenza = value;
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private List<Turno> _Turni;
        public List<Turno> Turni
        {
            get { return this._Turni; }
        }
        
        public CalendarioReparto CalendarioRep;

        private List<ProcessoVariante> _processiVarianti;
        public List<ProcessoVariante> processiVarianti
        {
            get { return this._processiVarianti; }
        }

        private bool _splitTasks;
        public bool splitTasks
        {
            get { return this._splitTasks; }
            set
            {
                if (this.id != -1)
                {
                    bool oldSplitTasks = this.splitTasks;
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    string sql = "UPDATE reparti SET splitTasks = @splitTasks WHERE idreparto = @id";
                    try
                    {
                        conn.Execute(sql, new { splitTasks = value, id = this.id }, trans);
                        this._splitTasks = value;
                        trans.Commit();
                    }
                    catch
                    {
                        this._splitTasks = oldSplitTasks;
                        trans.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        // Valore in secondi dell'anticipo minimo da dare ai tasks non appartenenti al critical path rispetto al task successivo
        private TimeSpan _anticipoMinimoTasks;
        public TimeSpan anticipoMinimoTasks
        {
            get { return this._anticipoMinimoTasks; }
            set
            {
                if (this.id != -1)
                {
                    TimeSpan oldAnticipo = this.anticipoMinimoTasks;
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    string sql = "UPDATE reparti SET anticipoTasks = @anticipoTasks WHERE idreparto = @id";
                    try
                    {
                        conn.Execute(sql, new { anticipoTasks = value.TotalSeconds, id = this.id });
                        this._anticipoMinimoTasks = value;
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        err = ex.Message;
                        this._anticipoMinimoTasks = oldAnticipo;
                        trans.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private UtentiReparto _Operatori;
        public UtentiReparto Operatori
        {
            get { return this._Operatori; }
        }

        private Boolean _KanbanManaged;
        public Boolean KanbanManaged
        {
            get { return this._KanbanManaged; }
            set
            {
                // Funziona solo se KanbanBox by Sintesia è abilitato
                KanbanBoxConfig kboxCfg = (KIS.App_Code.KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                // Se KanbanBox è abilitato
                if (kboxCfg.KanbanBoxEnabled == true)
                {
                    // Se ho caricato il reparto
                    if (this.id != -1)
                    {
                        int param = 0;
                        if (value == true)
                        {
                            param = 1;
                        }
                        else
                        {
                            param = 0;
                        }
                        MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                        conn.Open();
                        string strSQL = "SELECT valore FROM configurazione WHERE Sezione Like 'Reparto' AND "
                            + " ID = @id"
                            + " AND parametro LIKE 'KanbanManaged'";
                        String valore = conn.QueryFirstOrDefault<string>(strSQL, new { id = this.id });
                        if (valore != null)
                        {
                            strSQL = "UPDATE configurazione SET valore = @param"
                                + " WHERE Sezione LIKE 'Reparto' "
                                + " AND ID = @id"
                                + " AND parametro LIKE 'KanbanManaged'";
                        }
                        else
                        {
                            strSQL = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                                + "'Reparto', "
                                + "@id" + ", "
                                + "'KanbanManaged'" + ", "
                                + "@param)";
                        }
                        conn.Execute(strSQL, new { id = this.id, param = param });
                        conn.Close();

                        this._KanbanManaged = value;
                    }
                }
                
                this._KanbanManaged = value;
            }
        }

        private String _fusoOrario;
        public String fusoOrario
        {
            get {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return !string.IsNullOrEmpty(this._fusoOrario) ? this._fusoOrario : fuso.tzFusoOrario.Id; }
            set
            {
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    string sql = "UPDATE reparti SET timezone = @timezone WHERE idreparto = @id";
                    try
                    {
                        conn.Execute(sql, new { timezone = value.ToString(), id = this.id });
                        this._fusoOrario = value;
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;
                        trans.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public TimeZoneInfo tzFusoOrario
        {
            get
            {
                TimeZoneInfo ret;
                try
                {
                    ret=TimeZoneInfo.FindSystemTimeZoneById(this._fusoOrario);
                }
                catch
                {
                    ret = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                }
                return ret;
            }
        }

        public Boolean FullyConfigured
        {
            get
            {
                Boolean ret = false;
                this.loadTurni();
                Boolean checkTurni = this.Turni.Count > 0 ? true : false;
                Boolean checkIntervalli = false;
                for (int i = 0; i < this.Turni.Count; i++)
                {
                    checkIntervalli = this.Turni[i].OrariDiLavoro.Count > 0 ? true : false;
                }
                Boolean checkTimezone = this.fusoOrario.Length > 0 ? true : false;

/*                AndonReparto aRep = new AndonReparto(this.Tenant, this.id);
                aRep.loadCampiVisualizzati();
                Boolean checkAndon = aRep.CampiVisualizzati.Count>0?true:false;*/

                ret = checkTurni && checkIntervalli && checkTimezone; // && checkAndon;
                return ret;
            }
        }

        /*Returns
         * 0 if date
         * 1 if week
         */
        private int _EndProductionDateFormat;
        public int EndProductionDateFormat
        {
            get
            {
                return this._EndProductionDateFormat;
            }
            set
            {
                    if (this.id != -1)
                    {
                        // Verifico che sia presente la configurazione
                        MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                        conn.Open();
                        MySqlTransaction tr = conn.BeginTransaction();
                        bool add = false;
                        String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                            + "AND ID = @id AND parametro LIKE 'EndProductionDate Format'", new { id = this.id });
                        if (valore != null)
                        {
                            add = false;
                        }
                        else
                        {
                            add = true;
                        }

                        string sql;
                        if (add == true)
                        {
                            sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                                + "@id, 'EndProductionDate Format', @value)";
                        }
                        else
                        {
                            sql = "UPDATE configurazione SET valore = @value WHERE "
                            + " Sezione = 'Reparto' AND ID = @id" +
                            " AND parametro LIKE 'EndProductionDate Format'";
                        }

                        try
                        {
                            conn.Execute(sql, new { id = this.id, value = value.ToString() }, tr);
                            tr.Commit();
                        }
                        catch (Exception ex)
                        {
                            log = ex.Message;
                            tr.Rollback();
                        }
                    }
                }
        }

        /* Returns
  * 0 if date
  * 1 if week
  */
        private int _DeliveryDateFormat;
        public int DeliveryDateFormat
        {
            get
            {
                return this._DeliveryDateFormat;
            }
            set
            {
                if (this.id != -1)
                {
                    // Verifico che sia presente la configurazione
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    bool add = false;
                    String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                            + "AND ID = @id AND parametro LIKE 'DeliveryDate Format'", new { id = this.id });
                    if (valore != null)
                    {
                        add = false;
                    }
                    else
                    {
                        add = true;
                    }

                    string sql;
                    if (add == true)
                    {
                        sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                            + "@id, 'DeliveryDate Format', @value)";
                    }
                    else
                    {
                        sql = "UPDATE configurazione SET valore = @value WHERE "
                            + " Sezione = 'Reparto' AND ID = @id" +
                            " AND parametro LIKE 'DeliveryDate Format'";
                    }

                    try
                    {
                        conn.Execute(sql, new { id = this.id, value = value.ToString() }, tr);
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                }
            }
        }

        /* Returns:
         * TRUE if Operators comments are allowed
         * FALSE if Operators comments are NOT allowed
         */
        private Boolean _AllowTaskOperatorsComments;
        public Boolean AllowTaskOperatorsComments
        {
            get
            {
                return this._AllowTaskOperatorsComments;
            }
            set
            {
                if (this.id != -1)
                {
                    // Verifico che sia presente la configurazione
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    bool add = false;
                    String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                            + "AND ID = @id AND parametro LIKE 'Allow Tasks Operators Comments'", new { id = this.id });
                    if (valore != null)
                    {
                        add = false;
                    }
                    else
                    {
                        add = true;
                    }

                    string sql;
                    if (add == true)
                    {
                        sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                            + "@id, 'Allow Tasks Operators Comments', @value)";
                    }
                    else
                    {
                        sql = "UPDATE configurazione SET valore = @value WHERE "
                            + " Sezione = 'Reparto' AND ID = @id" +
                            " AND parametro LIKE 'Allow Tasks Operators Comments'";
                    }

                    try
                    {
                        conn.Execute(sql, new { id = this.id, value = value.ToString() }, tr);
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                }
            }
        }

        /* This parameter returns
         * true if tasks will be automatically auto-paused outside work shifts (Enables AutoPauseTasks Controller)
         * false otherwise
         */
        public Boolean AutoPauseTasksOutsideWorkShifts
        {
            get
            {
                Boolean ret = true;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                    + "AND ID = @id AND parametro LIKE 'AutoPauseTasks'", new { id = this.id });
                if (valore != null)
                {
                    if (valore =="False")
                    {
                        ret = false;
                    }
                }

                conn.Close();
                return ret;
            }

            set
            {
                if (this.id != -1)
                {
                    // Verifico che sia presente la configurazione
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    bool add = false;
                    String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                        + "AND ID = @id AND parametro LIKE 'AutoPauseTasks'", new { id = this.id });
                    if (valore != null)
                    {
                        add = false;
                    }
                    else
                    {
                        add = true;
                    }

                    string sql;
                    if (add == true)
                    {
                        sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                            + "@id, 'AutoPauseTasks', @value)";
                    }
                    else
                    {
                        sql = "UPDATE configurazione SET valore = @value WHERE "
                        + " Sezione = 'Reparto' AND ID = @id" +
                        " AND parametro LIKE 'AutoPauseTasks'";
                    }

                    try
                    {
                        conn.Execute(sql, new { id = this.id, value = value.ToString() }, tr);
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                }
            }
        }

        public List<KIS.App_Sources.InputPointDepartment> inputpoints;

        public Reparto(String tenant)
        {
            this.inputpoints = new List<App_Sources.InputPointDepartment>();
            this.Tenant = tenant;
            this._id = -1;
        }

        public Reparto(String tenant, int repID)
        {
            this.inputpoints = new List<App_Sources.InputPointDepartment>();
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT idreparto, nome, descrizione, cadenza, splitTasks, anticipoTasks, ModoCalcoloTC, timezone FROM reparti WHERE idReparto = @repID";
            try
            {
                var row = conn.QueryFirstOrDefault<RepartoRow>(sql, new { repID = repID });
                if (row != null)
                {
                    this._id = row.idreparto;
                    this._name = row.nome;
                    this._description = row.descrizione;
                    if (row.cadenza.HasValue)
                    {
                        this._Cadenza = TimeSpan.FromSeconds(row.cadenza.Value);
                    }
                    else
                    {
                        this._Cadenza = new TimeSpan(0, 0, 0);
                    }
                    this._splitTasks = row.splitTasks;
                    long anticipo = row.anticipoTasks;
                    this._anticipoMinimoTasks = new TimeSpan(anticipo * 10000000);
                    this._ModoCalcoloTC = row.ModoCalcoloTC;
                    this._fusoOrario = "";
                    if (row.timezone != null)
                    {
                        this._fusoOrario = row.timezone;
                    }
                    else
                    {
                        FusoOrario fuso = new FusoOrario(this.Tenant);
                        this._fusoOrario = fuso.fusoOrario;
                    }
                }
                else
                {
                    this._id = -1;
                    this._name = "";
                    this._description = "";
                    this._splitTasks = false;
                    this._Cadenza = new TimeSpan(0, 0, 0);
                    this._KanbanManaged = false;
                    this._fusoOrario = "";
                }
                //this.loadConfigurazioneKanban();
            }
            catch
            {
                this._id = -1;
                this._name = "";
                this._description = "";
                this._splitTasks = false;
                this._Cadenza = new TimeSpan(0, 0, 0);
                this._KanbanManaged = false;
                this._fusoOrario = "";
            }
            conn.Close();
        }

        public Reparto(String tenant, String name)
        {
            this.inputpoints = new List<App_Sources.InputPointDepartment>();
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT idreparto, nome, descrizione, cadenza, splitTasks, anticipoTasks, ModoCalcoloTC, timezone FROM reparti WHERE nome LIKE @name";
            try
            {
                var row = conn.QueryFirstOrDefault<RepartoRow>(sql, new { name = name });
                if (row != null)
                {
                    this._id = row.idreparto;
                    this._name = row.nome;
                    this._description = row.descrizione;
                    if (row.cadenza.HasValue)
                    {
                        this._Cadenza = TimeSpan.FromSeconds(row.cadenza.Value);
                    }
                    else
                    {
                        this._Cadenza = new TimeSpan(0, 0, 0);
                    }
                    this._splitTasks = row.splitTasks;
                    long anticipo = row.anticipoTasks;
                    this._anticipoMinimoTasks = new TimeSpan(anticipo * 10000000);
                    this._ModoCalcoloTC = row.ModoCalcoloTC;
                    this._fusoOrario = "";
                    if (row.timezone != null)
                    {
                        this._fusoOrario = row.timezone;
                    }
                    this.loadConfigurazioneKanban();
                }
                else
                {
                    this._id = -1;
                    this._name = "";
                    this._description = "";
                    this._splitTasks = false;
                    this._Cadenza = new TimeSpan(0, 0, 0);
                    this._KanbanManaged = false;
                    this._fusoOrario = "";
                }
            }
            catch
            {
                this._id = -1;
                this._name = "";
                this._description = "";
                this._splitTasks = false;
                this._Cadenza = new TimeSpan(0, 0, 0);
                this._KanbanManaged = false;
                this._fusoOrario = "";
            }
            conn.Close();
        }

        /*Returns
         * IDReparto if correctly added
         * -1 in case of error
         */
        public int Add(String nome, String descrizione, String timeZone)
        {
            int rt=-1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? maxId = conn.ExecuteScalar<int?>("SELECT MAX(idReparto) FROM reparti");
            int repID = 0;
            if (maxId.HasValue)
            {
                repID = maxId.Value + 1;
            }

            MySqlTransaction trans = conn.BeginTransaction();
            string sql = "INSERT INTO reparti(idReparto, nome, descrizione, cadenza, splitTasks, anticipoTasks, ModoCalcoloTC, timezone) VALUES(" 
                + "@repID, @nome, @descrizione, 0, 1, 0, false, @timeZone)";
            try
            {
                conn.Execute(sql, new { repID = repID, nome = nome, descrizione = descrizione, timeZone = timeZone }, trans);
                trans.Commit();
                rt = repID;
            }
            catch(Exception ex)
            {
                err = ex.Message;
                rt = -1;
                trans.Rollback();
            }
            conn.Close();
            return rt;
        }

        public bool loadTurni()
        {
            this._Turni = new List<Turno>();
            bool rt = false;;
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT id FROM turniproduzione WHERE reparto = @id ORDER BY nome", new { id = this.id });
                foreach (var id in rows)
                {
                    this._Turni.Add(new Turno(this.Tenant, id));
                }
                conn.Close();
                rt = true;
            }
            return rt;
        }

        public bool addTurno(String nome, String colore)
        {
            bool rt = false;
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? maxId = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM turniproduzione");
                int turnoID = 0;
                if (maxId.HasValue)
                {
                    turnoID = maxId.Value + 1;
                }
                MySqlTransaction trans = conn.BeginTransaction();
                string sql = "INSERT INTO turniproduzione(id, reparto, nome, colore) VALUES(@id"
                    + ", @reparto, @nome, @colore)";
                try
                {
                    rt = true;
                    conn.Execute(sql, new { id = turnoID, reparto = this.id, nome = nome, colore = colore });
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    err = ex.Message;
                    rt = false;
                    trans.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        public bool deleteTurno(Turno idTurno)
        {
            bool rt = false;
            this.log = this.id.ToString() + " " + idTurno.id.ToString() + " <br />";
            if (this.id != -1 && idTurno.id != -1)
            {
                for (int i = 0; i < idTurno.OrariDiLavoro.Count; i++)
                {
                    idTurno.OrariDiLavoro[i].Delete();
                }

                // cancello le risorse
                idTurno.loadRisorse();
                for (int i = 0; i < idTurno.Risorse.Postazioni.Count; i++)
                {
                    idTurno.Risorse.Postazioni[i].delete();
                }
                

                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    conn.Execute("DELETE FROM straordinarifestivita WHERE turno = @turno", new { turno = idTurno.id }, trans);
                    conn.Execute("DELETE FROM turniproduzione WHERE id = @id AND reparto = @reparto", new { id = idTurno.id, reparto = this.id }, trans);
                    rt = true;
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    rt = false;
                }
                conn.Close();
            }
            return rt;
        }

        public void loadCalendario(DateTime I, DateTime F)
        {
            //this.CalendarioRep = new CalendarioReparto(this.id, I, F);
            if(this.Turni==null || this.Turni.Count == 0)
            {
                this.loadTurni();
            }
            this.CalendarioRep = new CalendarioReparto(this.Tenant, this, I, F);
        }

        public Turno trovaProssimoTurno()
        {
            Turno prossimo;
            this.loadTurni();
            double[] distanze = new double[this.Turni.Count];

            // Ora trovo la distanza minima
            double min = 1000000000;
            int indexMin = -1;
            for (int i = 0; i < this.Turni.Count; i++ )
            {
                if (distanze[i] < min)
                {
                    min = distanze[i];
                    indexMin = i;
                }
            }
            prossimo = this.Turni[indexMin];
            return prossimo;
        }

        public bool addProcesso(processo prc, variante vr)
        {
            bool rt = false;
            if (this.id != -1 && prc.processID != -1 && vr.idVariante != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                string sql = "INSERT INTO repartiprocessi(idReparto, processID, revisione, variante) VALUES("
                    + "@idReparto, @processID, @revisione"
                    + ", @variante)";

                try
                {
                    conn.Execute(sql, new { idReparto = this.id, processID = prc.processID, revisione = prc.revisione, variante = vr.idVariante });
                    rt = true;
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    err = ex.Message;
                    trans.Rollback();
                    rt = false;
                }
                conn.Close();
            }
            return rt;
        }

        public void loadProcessiVarianti()
        {
            this._processiVarianti = new List<ProcessoVariante>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<(int processID, int revisione, int variante)>("SELECT repartiprocessi.processID, repartiprocessi.revisione, repartiprocessi.variante FROM repartiprocessi WHERE "
                    + " idReparto = @idReparto", new { idReparto = this.id });
                
                foreach (var r in rows)
                {
                    ProcessoVariante vr = new ProcessoVariante(this.Tenant, new processo(this.Tenant, r.processID, r.revisione),  new variante(this.Tenant, r.variante));
                    vr.loadReparto();
                    vr.process.loadFigli(vr.variant);
                    if (vr != null && vr.process != null && vr.variant != null)
                    {
                        this._processiVarianti.Add(vr);
                    }
                }
                conn.Close();
            }
        }

        public bool deleteProcesso(processo prc, variante vr)
        {
            bool rt = false;
            if (this.id != -1 && prc.processID != -1 && vr.idVariante != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                string sql = "DELETE FROM repartiprocessi WHERE idReparto = @idReparto AND "
                    +" processID = @processID AND revisione = @revisione"
                    + " AND variante = @variante";

                try
                {
                    conn.Execute(sql, new { idReparto = this.id, processID = prc.processID, revisione = prc.revisione, variante = vr.idVariante });
                    rt = true;
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    err = ex.Message;
                    trans.Rollback();
                    rt = false;
                }
                conn.Close();
            }
            return rt;
        }

        public List<RepartoProcessoPostazione> PostazioniTask;

        public bool loadPostazioniTask(ProcessoVariante prc)
        {
            bool rt = false;
            if (this.id != -1 && prc.variant != null & prc.process != null)
            {
                PostazioniTask = new List<RepartoProcessoPostazione>();
                prc.process.loadFigli(prc.variant);
                for (int i = 0; i < prc.process.subProcessi.Count; i++)
                {
                    TaskVariante figlio = new TaskVariante(this.Tenant, prc.process.subProcessi[i], prc.variant);
                    RepartoProcessoPostazione implementazione = new RepartoProcessoPostazione(this.Tenant, this.id, figlio);
                    if (implementazione.proc != null && implementazione.Pst != null)
                    {
                        PostazioniTask.Add(implementazione);
                    }
                }
                rt = true;
            }
            return rt;
        }

        public bool LinkTaskToPostazione(TaskVariante prc, Postazione post)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            string sql = "INSERT INTO repartipostazioniattivita(reparto, postazione, processo, revProc, variante) VALUES("
                + "@reparto, @postazione, @processo"
                + ", @revProc, @variante)";
            try
            {
                conn.Execute(sql, new { reparto = this.id, postazione = post.id, processo = prc.Task.processID, revProc = prc.Task.revisione, variante = prc.variant.idVariante });
                trans.Commit();
                rt = true;
            }
            catch
            {
                trans.Rollback();
                rt = false;
            }
            conn.Close();
            return rt;
        }

        public bool DeleteLinkTaskFromPostazione(TaskVariante prc, Postazione post)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            string sql = "DELETE FROM repartipostazioniattivita WHERE reparto = @reparto"
            + " AND postazione = @postazione AND processo = @processo"
            + " AND revProc = @revProc AND variante = @variante";
            try
            {
                conn.Execute(sql, new { reparto = this.id, postazione = post.id, processo = prc.Task.processID, revProc = prc.Task.revisione, variante = prc.variant.idVariante });
                trans.Commit();
                rt = true;
            }
            catch
            {
                trans.Rollback();
                rt = false;
            }
            conn.Close();
            return rt;
        }

        public bool DeleteLinkTaskFromPostazione(TaskVariante prc)
        {
            bool rt = false;
            if (this.id != -1 && prc.Task != null && prc.variant != null && prc.variant.idVariante != -1 && prc.Task.processID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                string sql = "DELETE FROM repartipostazioniattivita WHERE reparto = @reparto"
                + " AND processo = @processo"
                + " AND revProc = @revProc AND variante = @variante";
                try
                {
                    conn.Execute(sql, new { reparto = this.id, processo = prc.Task.processID, revProc = prc.Task.revisione, variante = prc.variant.idVariante });
                    trans.Commit();
                    rt = true;
                }
                catch
                {
                    trans.Rollback();
                    rt = false;
                }
                conn.Close();
            }
            return rt;
        }

   /*     private ProductionPlan _PianoProduzione;
        public ProductionPlan PianoProduzione
        {
            get { return this._PianoProduzione; }
        }
        
        public void loadProductionPlan()
        {
            this._PianoProduzione = new ProductionPlan(this);
        }*/

        public void loadOperatori()
        {
            this._Operatori = new UtentiReparto(this.Tenant, this.id);
        }

        private List<Postazione> _Postazioni;
        public List<Postazione> Postazioni
        {
            get { return this._Postazioni; }
        }

        public void loadPostazioni()
        {
            this._Postazioni = new List<Postazione>();
            if(this.id!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT DISTINCT(postazione) FROM repartipostazioniattivita WHERE reparto = @id"
                    + " ORDER BY postazione", new { id = this.id });
                foreach (var id in rows)
                {
                    this._Postazioni.Add(new Postazione(this.Tenant, id));
                }
                conn.Close();
            }
        
        }

        public ConfigurazioneRitardoReparto EventoRitardo;
        public void loadEventoRitardo()
        {
            if (this.id != -1)
            {
                this.EventoRitardo = new ConfigurazioneRitardoReparto(this.Tenant, this.id);
            }
        }

        public ConfigurazioneWarningReparto EventoWarning;
        public void loadEventoWarning()
        {
            if (this.id != -1)
            {
                this.EventoWarning = new ConfigurazioneWarningReparto(this.Tenant, this.id);
            }
        }

        /*
         * se FALSE --> Calcola il tempo ciclo non tenendo conto degli intervalli di lavoro;
         * se TRUE  --> tiene conto degli intervalli di lavoro e "SCREMA" le parti in eccesso
         */
        private bool _ModoCalcoloTC;
        public bool ModoCalcoloTC
        {
            get
            {
                return this._ModoCalcoloTC;
            }
            set
            {
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE reparti SET ModoCalcoloTC = @modoCalcoloTC WHERE idreparto = @id";
                    try
                    {
                        conn.Execute(sql, new { modoCalcoloTC = value, id = this.id });
                        tr.Commit();
                        this._ModoCalcoloTC = value;
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }
    
        /* Configurazione visualizzazione nomi utente su Andon:
         * 0 --> vedo username
         * 1 --> vedo il nome
         * 2 --> nome e iniziale del cognome
         * 3 --> nome e cognome
         */
        public char AndonPostazioniFormatoUsername
        {
            get
            {
                char ret = '0';
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                    + "AND ID = @id AND parametro LIKE 'Andon FormatoUsername'", new { id = this.id });
                if (valore != null)
                {
                    if (valore.Length > 0)
                    {
                        ret = valore[0];
                    }
                }

                conn.Close();
                return ret;
            }

            set
            {
                if (this.id != -1)
                {
                    // Verifico che sia presente la configurazione
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    bool add = false;
                    String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                        + "AND ID = @id AND parametro LIKE 'Andon FormatoUsername'", new { id = this.id });
                    if (valore != null)
                    {
                        add = false;
                    }
                    else
                    {
                        add = true;
                    }

                    string sql;
                    if (add == true)
                    {
                        sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                            + "@id, 'Andon FormatoUsername', @value)";
                    }
                    else
                    {
                        sql = "UPDATE configurazione SET valore = @value WHERE "
                        + " Sezione = 'Reparto' AND ID = @id" +
                        " AND parametro LIKE 'Andon FormatoUsername'";
                    }

                    try
                    {
                        conn.Execute(sql, new { id = this.id, value = value.ToString() }, tr);
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                }
            }
        }
    
        /* Configurazione del numero massimo di tasks avviabili contemporaneamente
         * 0 --> infiniti
         */
        public int TasksAvviabiliContemporaneamenteDaOperatore
        {
            get
            {
                int ret = 0;
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    String strRet = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' AND ID = "
                        + "@id AND parametro = 'AvvioTasks'", new { id = this.id });
                    if (strRet != null)
                    {
                        try
                        {
                            ret = Int32.Parse(strRet);
                        }
                        catch(Exception ex)
                        {
                            ret = 0;
                            log = ex.Message;
                        }
                    }
                    else
                    {
                        ret = 0;
                    }
                    conn.Close();
                }
                return ret;
            }

            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool found = false;
                String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' AND ID = "
                    + "@id AND parametro = 'AvvioTasks'", new { id = this.id });
                if (valore != null)
                {
                    found = true;
                }
                else
                {
                    found = false;
                }

                MySqlTransaction tr = conn.BeginTransaction();

                string sql;
                if (found == true)
                {
                    sql = "UPDATE configurazione SET valore = @value"
                        + " WHERE Sezione = 'Reparto' AND ID = @id AND parametro = 'AvvioTasks'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                        + "@id, 'AvvioTasks', @value)";
                }

                try
                {
                    conn.Execute(sql, new { id = this.id, value = value.ToString() }, tr);
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    log = ex.Message;
                }

                conn.Close();
            }
        }

        /*
         * Carica il parametro di configurazione _KanbanManaged
         * Funzione solo se KanbanBox by Sintesia è abilitato
         */

        public Boolean loadConfigurazioneKanban()
        {
            Boolean ret = false;
            this._KanbanManaged = false;
            KanbanBoxConfig kboxCfg = (KIS.App_Code.KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
            // Se KanbanBox è abilitato
            if (kboxCfg.KanbanBoxEnabled == true)
            {
                // Se ho caricato il reparto
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    int? param = conn.QueryFirstOrDefault<int?>("SELECT valore FROM configurazione WHERE Sezione Like 'Reparto' AND "
                        + " ID = @id"
                        + " AND parametro LIKE 'KanbanManaged'", new { id = this.id });
                    if (param.HasValue)
                    {
                        if (param.Value == 1)
                        {
                            this._KanbanManaged = true;
                            ret = true;
                        }
                        else
                        {
                            this._KanbanManaged = false;
                            ret = false;
                        }
                    }
                    else
                    {
                        this._KanbanManaged = false;
                        ret = false;
                    }
                    conn.Close();
                }
            }
            else
            {
                this.KanbanManaged = false;
                ret = false;
            }
            return ret;
        }

        public void loadEndProductionDateFormat()
        {
            this._EndProductionDateFormat = 0;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                + "AND ID = @id AND parametro LIKE 'EndProductionDate Format'", new { id = this.id });
            if (valore != null)
            {
                try
                {
                    this._EndProductionDateFormat = Int32.Parse(valore);
                }
                catch
                {
                    this._EndProductionDateFormat = 0;
                }
            }

            conn.Close();
        }

        public void loadDeliveryDateFormat()
        {
            this._EndProductionDateFormat = 0;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            String valore = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                + "AND ID = @id AND parametro LIKE 'DeliveryDate Format'", new { id = this.id });
            if (valore != null)
            {
                try
                {
                    this._DeliveryDateFormat = Int32.Parse(valore);
                }
                catch
                {
                    this._DeliveryDateFormat = 0;
                }
            }

            conn.Close();
        }

        public void loadAllowTaskOperatorsCommentFlag()
        {
            this._AllowTaskOperatorsComments = true;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            String flag = conn.QueryFirstOrDefault<string>("SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                + "AND ID = @id AND parametro LIKE 'Allow Tasks Operators Comments'", new { id = this.id });
            if (flag != null)
            {
                try
                {
                    if(flag == "False")
                    {
                        this._AllowTaskOperatorsComments = false;
                    }
                    else
                    {
                        this._AllowTaskOperatorsComments = true;
                    }
                    
                }
                catch
                {
                    this._AllowTaskOperatorsComments = true;
                }
            }
            conn.Close();
        }

        public void loadInputPoints()
        {
            this.inputpoints = new List<App_Sources.InputPointDepartment>();
            if(this.id> -1 && this.Tenant.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT inputpointid FROM inputpoints_departments WHERE departmentid=@deptid AND enabled is true", new { deptid = this.id });
                foreach (var id in rows)
                {
                    this.inputpoints.Add(new App_Sources.InputPointDepartment(this.Tenant, id, this.id));
                }
                conn.Close();
            }
        }
    }

    public class ElencoReparti
    {
        protected String Tenant;

        public List<Reparto> elenco;

        public ElencoReparti(String tenant)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<int>("SELECT idReparto FROM reparti ORDER BY nome");
            elenco = new List<Reparto>();
            foreach (var id in rows)
            {
                elenco.Add(new Reparto(this.Tenant, id));
            }
            conn.Close();
        }
    }

    public class Turno
    {
        protected String Tenant;

        private class TurnoRow
        {
            public int id { get; set; }
            public int reparto { get; set; }
            public String nome { get; set; }
            public String colore { get; set; }
        }

        public String err;

        private int _id;
        public int id
        {
            get { return this._id; }
        }

        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        private String _Nome;
        public String Nome
        {
            get { return this._Nome; }
            set
            {
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    string sql = "UPDATE turniproduzione SET nome = @nome WHERE id = @id AND reparto = @idReparto";
                    try
                    {
                        conn.Execute(sql, new { nome = value, id = this.id, idReparto = this.idReparto }, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;
                        trans.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private List<IntervalloLavorativoTurno> _Orari;
        public List<IntervalloLavorativoTurno> OrariDiLavoro
        {
            get
            {
                return this._Orari;
            }
        }

        private System.Drawing.Color _Colore;
        public System.Drawing.Color Colore
        {
            get { return this._Colore; }
            set
            {
                if (this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();
                    String strCol = "#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
                    string sql = "UPDATE turniproduzione SET colore = @colore WHERE id = @id";
                    try
                    {
                        conn.Execute(sql, new { colore = strCol, id = this.id }, trn);
                        this._Colore = value;
                        trn.Commit();
                    }
                    catch (Exception ex)
                    {
                        trn.Rollback();
                        err = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        public CalendarioTurno CalendarioTrn;

        public Turno(String tenant)
        {
            this.Tenant = tenant;
            this._id = -1;
            this._idReparto = -1;
        }

        public Turno(String tenant, int turnoID)
        {
            this.Tenant = tenant;
            this._id = -1;
            this._idReparto = -1;
            this._straordinari = new List<Straordinario>();
            this._festivita = new List<Festivita>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<TurnoRow>("SELECT turniproduzione.id, turniproduzione.reparto, turniproduzione.nome, turniproduzione.colore "
            + " FROM turniproduzione WHERE id = @id", new { id = turnoID });
            if (row != null)
            {
                this._id = row.id;
                this._idReparto = row.reparto;
                this._Nome = row.nome;
                this._Colore = new System.Drawing.Color();
                this._Colore = System.Drawing.ColorTranslator.FromHtml(row.colore);
            }
            else
            {
                this._id = -1;
                this._idReparto = -1;
            }

            // Carico gli orari di lavoro
            this._Orari = new List<IntervalloLavorativoTurno>();
            if (this.id!=-1)
            { 
            var ids = conn.Query<int>("SELECT id FROM orarilavoroturni WHERE idTurno = @idTurno"
                + " ORDER BY giornoInizio, oraInizio", new { idTurno = this.id });
            
            foreach (var id in ids)
            {
                this._Orari.Add(new IntervalloLavorativoTurno(this.Tenant, id));
            }
            }
            conn.Close();
        }

        public bool AddOrario(DayOfWeek dInizio, TimeSpan oInizio, DayOfWeek dFine, TimeSpan oFine)
        {
            bool rt;
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? maxId = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM orarilavoroturni");
                int newID;
                if (maxId.HasValue)
                {
                    newID = maxId.Value + 1;
                }
                else
                {
                    newID = 0;
                }
                MySqlTransaction trans = conn.BeginTransaction();
                String strOraInizio = oInizio.Hours.ToString() + ":" + oInizio.Minutes.ToString() + ":" + oInizio.Seconds.ToString();
                String strOraFine = oFine.Hours.ToString() + ":" + oFine.Minutes.ToString() + ":" + oFine.Seconds.ToString();
                string sql = "INSERT INTO orarilavoroturni(id, idTurno, giornoInizio, oraInizio, giornoFine, oraFine) VALUES"
                    + "(@id, @idTurno, @giornoInizio, @oraInizio, @giornoFine, @oraFine)";
                try
                {
                    rt = true;
                    conn.Execute(sql, new { id = newID, idTurno = this.id, giornoInizio = ((Int32)dInizio), oraInizio = strOraInizio, giornoFine = ((Int32)dFine), oraFine = strOraFine }, trans);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    rt = false;
                    trans.Rollback();
                }
                conn.Close();
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        public void loadCalendario(DateTime I, DateTime F)
        {
            this.CalendarioTrn = new CalendarioTurno(this.Tenant, this.id, I, F);
        }

        private List<Straordinario> _straordinari;
        public List<Straordinario> straordinari
        {
            get { return this._straordinari; }
        }
        private List<Festivita> _festivita;
        public List<Festivita> festivita
        {
            get { return this._festivita; }
        }

        public void loadStraordinari()
        {
            this._straordinari = new List<Straordinario>();
            if (this.id != -1)
            {
                Reparto rp = new Reparto(this.Tenant,idReparto);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT straordinarifestivita.id FROM straordinarifestivita "
                + "WHERE azione = 'S' AND straordinarifestivita.turno = @idTurno"
                    + " AND datafine > @today"
                    + " ORDER BY datainizio", new { idTurno = this.id, today = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") });

                foreach (var id in rows)
                {
                    this._straordinari.Add(new Straordinario(this.Tenant, id));
                }
                conn.Close();
            }
            else
            {
                this._idReparto = -1;
            }
        }

        public void loadFestivita()
        {
            this._festivita = new List<Festivita>();
            if (this.id != -1)
            {
                Reparto rp = new Reparto(this.Tenant, idReparto);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT straordinarifestivita.id FROM straordinarifestivita "
                    + "WHERE azione = 'F' AND straordinarifestivita.turno = @idTurno"
                    + " AND datafine > @today"
                    + " ORDER BY datainizio", new { idTurno = this.id, today = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") });
                
                foreach (var id in rows)
                {
                    festivita.Add(new Festivita(this.Tenant, id));
                }
                conn.Close();
            }
            else
            {
                this._idReparto = -1;
            }
        }

        private RisorseTurno _Risorse;
        public RisorseTurno Risorse { get { return this._Risorse; } }

        public void loadRisorse()
        {
            if (this.id != -1)
            {
                this._Risorse = new RisorseTurno(this.Tenant, this);
            }
        }
    }
    public class IntervalloLavorativoTurno
    {
        protected String Tenant;

        private class IntervalloRow
        {
            public int idTurno { get; set; }
            public int giornoInizio { get; set; }
            public TimeSpan oraInizio { get; set; }
            public int giornoFine { get; set; }
            public TimeSpan oraFine { get; set; }
        }

        public String err;

        private int _idIntervallo;
        public int idIntervallo
        {
            get { return this._idIntervallo; }
        }
        private int _idTurno;
        public int idTurno
        {
            get { return this._idTurno; }
        }

        private DayOfWeek _GiornoInizio;
        public DayOfWeek GiornoInizio
        {
            get { return this._GiornoInizio; }
        }

        private TimeSpan _OraInizio;
        public TimeSpan OraInizio
        {
            get { return this._OraInizio; }
        }

        private DayOfWeek _GiornoFine;
        public DayOfWeek GiornoFine
        {
            get { return this._GiornoFine; }
        }

        private TimeSpan _OraFine;
        public TimeSpan OraFine
        {
            get { return this._OraFine; }
        }

        public TimeSpan Inizio
        {
            get
            {
                return new TimeSpan((Int32)GiornoInizio, OraInizio.Hours, OraInizio.Minutes, OraInizio.Seconds);
            }
        }
        public TimeSpan Fine
        {
            get
            {
                return new TimeSpan((Int32)GiornoFine, OraFine.Hours, OraFine.Minutes, OraFine.Seconds);
            }
        }

        public TimeSpan Durata
        {
            get
            {
                DateTime primo;
                primo = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, OraInizio.Hours, OraInizio.Minutes, OraInizio.Seconds);

                DateTime secondo;
                secondo = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, OraFine.Hours, OraFine.Minutes, OraFine.Seconds);

                // Ricerco la prossima data per cui DayOfWeek corrisponde a GiornoInizio
                while (primo.DayOfWeek != GiornoInizio)
                {
                    primo = primo.AddDays(1);
                }

                // Inizializzo il timespan finale al giorno del primo ma all'ora finale
                secondo = new DateTime(primo.Year, primo.Month, primo.Day, OraFine.Hours, OraFine.Minutes, OraFine.Seconds);
                // Ricerco la prossima data per cui DayOfWeek corrisponde a GiornoFine
                    while (secondo.DayOfWeek != GiornoFine)
                    {

                        secondo = secondo.AddDays(1);

                    }

                return (secondo - primo);
            }
        }

        public IntervalloLavorativoTurno(String tenant, int intervallo)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<IntervalloRow>("SELECT idTurno, giornoInizio, oraInizio, giornoFine, oraFine FROM orarilavoroturni WHERE "
                + " id = @id", new { id = intervallo });
            if (row != null)
            {
                this._idIntervallo = intervallo;
                this._idTurno = row.idTurno;
                this._GiornoInizio = new DayOfWeek();
                this._GiornoInizio = (DayOfWeek)row.giornoInizio;
                this._OraInizio = row.oraInizio;
                this._GiornoFine = new DayOfWeek();
                this._GiornoFine = (DayOfWeek)row.giornoFine;
                this._OraFine = row.oraFine;
            }
            else
            {
                this._idIntervallo = -1;
                this._idTurno = -1;
            }
            conn.Close();
        }

        public bool Delete()
        {
            bool rt = false;
            if (this.idIntervallo != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                string sql = "DELETE FROM orarilavoroturni WHERE id = @id" +
                    " AND idTurno = @idTurno";
                try
                {
                    conn.Execute(sql, new { id = this.idIntervallo, idTurno = this.idTurno }, trans);
                    rt = true;
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    err = ex.Message;
                    rt = false;
                    trans.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

    }

    public class RepartoProcessoPostazione
    {
        protected String Tenant;

        public String err;
        private int _repID;
        public int repID
        {
            get { return this._repID; }
        }

        private TaskVariante _proc;
        public TaskVariante proc
        {
            get { return this._proc; }
        }

        private Postazione _pst;
        public Postazione Pst
        {
            get { return this._pst;  }
        }

        public RepartoProcessoPostazione(String tenant, int rp, TaskVariante prc, Postazione ps)
        {
            this.Tenant = tenant;
            this._repID = rp;
            this._proc = prc;
            this._pst = ps;
        }

        public RepartoProcessoPostazione(String tenant, int rp, TaskVariante prc)
        {
            this.Tenant = tenant;
            if (prc.Task != null && prc.variant != null)
            {
                this._repID = rp;
                this._proc = prc;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                int? postazione = conn.QueryFirstOrDefault<int?>("SELECT postazione FROM repartipostazioniattivita WHERE reparto = @reparto"
                    + " AND processo = @processo AND revProc = @revProc"
                    + " AND variante = @variante", new { reparto = this.repID, processo = prc.Task.processID, revProc = prc.Task.revisione, variante = prc.variant.idVariante });
                if (postazione.HasValue)
                {
                    this._pst = new Postazione(this.Tenant, postazione.Value);
                    err = "Entro nell'if";
                }
                else
                {
                    err = "NON entro nell'if";
                    this._repID = -1;
                    this._pst = null;
                    this._proc = null;
                }
                conn.Close();
            }
        }

    }

    public class ElencoFestivita
    {
        protected String Tenant;

        public String log;

        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        public List<Festivita> feste;

        public ElencoFestivita(String tenant, int rep)
        {
            this.Tenant = tenant;
            Reparto rp = new Reparto(this.Tenant, rep);
            feste = new List<Festivita>();
            if (rp.id != -1)
            {
                this._idReparto = rp.id;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT straordinarifestivita.id FROM straordinarifestivita "
                    + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                    + "WHERE azione = 'F' AND turniproduzione.reparto = @reparto"
                    + " AND datafine > @today ORDER BY datainizio", new { reparto = rp.id, today = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") });
                
                foreach (var id in rows)
                {
                    feste.Add(new Festivita(this.Tenant, id));
                }
                conn.Close();
            }
            else
            {
                this._idReparto = -1;
            }
        }

        public bool Add(int idTurno, DateTime i, DateTime f)
        {
            bool rt = false;
            if (this.idReparto != -1)// && idTurno > 0)
            {
                if (i < f)
                {
                    Reparto rp = new Reparto(this.Tenant, this.idReparto);
                    i = TimeZoneInfo.ConvertTimeToUtc(i, rp.tzFusoOrario);
                    f = TimeZoneInfo.ConvertTimeToUtc(f, rp.tzFusoOrario);

                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    // Controllo che non ci sia sovrapposizione con altri intervalli

                    var overlap = conn.Query<int>("SELECT straordinarifestivita.id FROM straordinarifestivita INNER JOIN turniproduzione ON ("
                        + "turniproduzione.id = straordinarifestivita.turno)  "
                        +" WHERE turniproduzione.reparto = @idReparto AND "
                        + " turno = @idTurno AND "
                        + " ((datainizio < @i AND @i < dataFine) "
                        + " OR (datainizio < @f AND @f < dataFine) "
                        + " OR (@i < datainizio AND datafine < @f))", new { idReparto = this.idReparto, idTurno = idTurno, i = i.ToString("yyyy-MM-dd HH:mm:ss"), f = f.ToString("yyyy-MM-dd HH:mm:ss") });
                    
                    bool check1 = true;

                    if (overlap.Any())
                    {
                        check1 = false;
                    }
                    else
                    {
                        check1 = true;
                    }

                    // Controllo che non ci sia sovrapposizione con le pause dai turni di reparto
                    bool check2 = false;
                    CalendarioReparto crp = new CalendarioReparto(this.Tenant, this.idReparto, i.AddDays(-3), f.AddDays(3));

                    for (int q = 0; q < crp.Turni.Count && check2 == false; q++)
                    {
                        if ((TimeZoneInfo.ConvertTimeToUtc(crp.Turni[q].Inizio, rp.tzFusoOrario) <= i && i <= TimeZoneInfo.ConvertTimeToUtc(crp.Turni[q].Fine, rp.tzFusoOrario) && TimeZoneInfo.ConvertTimeToUtc(crp.Turni[q].Inizio, rp.tzFusoOrario) <= f && f <= TimeZoneInfo.ConvertTimeToUtc(crp.Turni[q].Fine, rp.tzFusoOrario)))
                        {
                            check2 = true;
                        }
                    }

                    if (check1 == true && check2)
                    {
                        int? max = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM straordinarifestivita");
                        int maxId = 0;
                        if (max.HasValue)
                        {
                            maxId = max.Value + 1;
                        }
                        else
                        {
                            maxId = 0;
                        }
                        MySqlTransaction trn = conn.BeginTransaction();
                        string sql = "INSERT INTO straordinarifestivita(id, azione, datainizio, datafine, turno) VALUES("
                            + "@id, 'F', @i, @f"
                            + ", @idTurno)";
                        try
                        {
                            conn.Execute(sql, new { id = maxId, i = i.ToString("yyyy-MM-dd HH:mm:ss"), f = f.ToString("yyyy-MM-dd HH:mm:ss"), idTurno = idTurno }, trn);
                            rt = true;
                            trn.Commit();
                        }
                        catch (Exception ex)
                        {
                            rt = false;
                            log = ex.Message;
                            trn.Rollback();
                        }
                    }
                    else
                    {
                        rt = false;
                    }
                    conn.Close();
                    return rt;
                }
                else
                {
                    return rt;
                }
            }
            else
            {
                return rt;
            }
        }

    }

    public class Festivita
    {
        protected String Tenant;

        public String log;

        private class FestivitaRow
        {
            public DateTime dataInizio { get; set; }
            public DateTime dataFine { get; set; }
            public int turno { get; set; }
            public int reparto { get; set; }
        }

        private int _idFestivita;
        public int idFestivita
        {
            get { return this._idFestivita; }
        }
        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        private int _idTurno;
        public int idTurno
        {
            get
            { return this._idTurno; }
        }

        private DateTime _Inizio;
        public DateTime Inizio
        {
            get {
                Reparto rp = new Reparto(this.Tenant, this.idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, rp.tzFusoOrario);
            }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get { Reparto rp = new Reparto(this.Tenant, this.idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, rp.tzFusoOrario);
            }
        }

        public Festivita(String tenant, int idFest)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<FestivitaRow>("SELECT dataInizio, dataFine, turno, turniproduzione.reparto FROM straordinarifestivita "
                + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                + "WHERE azione = 'F' AND straordinarifestivita.id = @id", new { id = idFest });
            if (row != null)
            {
                this._idFestivita = idFest;
                this._Inizio = new DateTime();
                this._Fine = new DateTime();
                this._Inizio = row.dataInizio;
                this._Fine = row.dataFine;
                this._idTurno = row.turno;
                this._idReparto = row.reparto;
            }
            else
            {
                this._idTurno = -1;
                this._idFestivita = -1;
                this._idReparto = -1;
                this._Inizio = DateTime.UtcNow;
                this._Fine = DateTime.UtcNow;
            }
            conn.Close();
        }

        public bool delete()
        {
            bool rt = false;
            if (this.idFestivita != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "DELETE FROM straordinarifestivita WHERE id = @id";
                MySqlTransaction trn = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { id = this.idFestivita });
                    rt = true;
                    trn.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    trn.Rollback();
                }
                conn.Close();
            }
            return rt;
        }
    }

    public class Straordinario
    {
        protected String Tenant;

        public String log;

        private class StraordinarioRow
        {
            public DateTime dataInizio { get; set; }
            public DateTime dataFine { get; set; }
            public int turno { get; set; }
            public int reparto { get; set; }
        }

        private int _idStraordinario;
        public int idStraordinario
        {
            get { return this._idStraordinario; }
        }
        private int _idTurno;
        public int idTurno
        {
            get
            {
                return this._idTurno;
            }
        }

        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        private DateTime _Inizio;
        public DateTime Inizio
        {
            get
            {
                Reparto rp = new Reparto(this.Tenant, this.idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, rp.tzFusoOrario);
            }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get
            {
                Reparto rp = new Reparto(this.Tenant, this.idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, rp.tzFusoOrario);
            }
        }

        public Straordinario(String tenant, int idStraord)
        {
            this.Tenant = tenant;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<StraordinarioRow>("SELECT dataInizio, dataFine, turno, turniproduzione.reparto FROM straordinarifestivita "
                + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                + "WHERE azione = 'S' AND straordinarifestivita.id = @id", new { id = idStraord });
            if (row != null)
            {
                this._idStraordinario = idStraord;
                this._idTurno = row.turno;
                this._idReparto = row.reparto;
                this._Inizio = new DateTime();
                this._Fine = new DateTime();
                this._Inizio = row.dataInizio;
                this._Fine = row.dataFine;
            }
            else
            {
                this._idTurno = -1;
                this._idStraordinario = -1;
                this._idReparto = -1;
                this._Inizio = DateTime.UtcNow;
                this._Fine = DateTime.UtcNow;
            }
            conn.Close();
        }

        public bool delete()
        {
            bool rt = false;
            if (this.idStraordinario != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "DELETE FROM straordinarifestivita WHERE id = @id";
                MySqlTransaction trn = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { id = this.idStraordinario });
                    rt = true;
                    trn.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    trn.Rollback();
                }
                conn.Close();
            }
            return rt;
        }
    }

    public class ElencoStraordinari
    {
        protected String Tenant;

        public String log;

        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        public List<Straordinario> Straordinari;

        public ElencoStraordinari(String tenant, int rep)
        {
            this.Tenant = tenant;
            Reparto rp = new Reparto(this.Tenant, rep);
            Straordinari = new List<Straordinario>();
            if (rp.id != -1)
            {
                this._idReparto = rp.id;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var rows = conn.Query<int>("SELECT straordinarifestivita.id FROM straordinarifestivita "
                + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                + "WHERE azione = 'S' AND turniproduzione.reparto = @reparto"
                    + " AND datafine > @today ORDER BY datainizio", new { reparto = rp.id, today = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") });
                
                foreach (var id in rows)
                {
                    Straordinari.Add(new Straordinario(this.Tenant, id));
                }
                conn.Close();
            }
            else
            {
                this._idReparto = -1;
            }
        }

        public bool Add(int idTurno, DateTime i, DateTime f)
        {
            bool rt = false;
            if (this.idReparto != -1)
            {
                if (i < f)
                {
                    Reparto rp = new Reparto(this.Tenant, this.idReparto);
                    //i = TimeZoneInfo.ConvertTimeToUtc(i, rp.tzFusoOrario);
                    //f = TimeZoneInfo.ConvertTimeToUtc(f, rp.tzFusoOrario);
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();

                    // Controllo che non ci sia sovrapposizione con altri intervalli
                    var overlap = conn.Query<int>("SELECT id FROM straordinarifestivita WHERE turno = @idTurno AND "
                        + " ((datainizio < @i AND @i < dataFine) "
                        + " OR (datainizio < @f AND @f < dataFine) "
                        + " OR (@i <= datainizio AND datafine <= @f))", new { idTurno = idTurno, i = i.ToString("yyyy-MM-dd HH:mm:ss"), f = f.ToString("yyyy-MM-dd HH:mm:ss") });
                    bool check1 = true;

                    if (overlap.Any())
                    {
                        check1 = false;
                    }
                    else
                    {
                        check1 = true;
                    }

                    // Controllo che non ci sia sovrapposizione con i turni di reparto
                    bool check2 = true;
                    CalendarioReparto crp = new CalendarioReparto(this.Tenant, this.idReparto, i.AddMonths(-1), f.AddMonths(1));
                    
                    for (int q = 0; q < crp.Turni.Count && check2 == true; q++)
                    {
                        //log += i.ToString("yyyy-MM-dd HH:mm:ss") + " " + f.ToString("yyyy-MM-dd HH:mm:ss") + " " + crp.Turni[q].Inizio.ToString("yyyy-MM-dd HH:mm:ss") + " " + crp.Turni[q].Fine.ToString("yyyy-MM-dd HH:mm:ss");
                        if((crp.Turni[q].Inizio < i && i < crp.Turni[q].Fine) || (crp.Turni[q].Inizio < f && f < crp.Turni[q].Fine) || (i < crp.Turni[q].Inizio && crp.Turni[q].Fine < f))
                        {
                            //log += " ENTRO!";
                            check2 = false;
                        }
                        else
                        {
                            check2 = true;
                        }
                        //log += "<br/>";
                    }
                    log += "Check1: " +check1.ToString() + " Check2: " + check2.ToString() + "<br/>";
                    if (check1 == true && check2 == true)
                    {


                        int? max = conn.ExecuteScalar<int?>("SELECT MAX(id) FROM straordinarifestivita");
                        int maxId = 0;
                        if (max.HasValue)
                        {
                            maxId = max.Value + 1;
                        }
                        else
                        {
                            maxId = 0;
                        }
                        MySqlTransaction trn = conn.BeginTransaction();
                        i = TimeZoneInfo.ConvertTimeToUtc(i, rp.tzFusoOrario);
                        f = TimeZoneInfo.ConvertTimeToUtc(f, rp.tzFusoOrario);
                        string sql = "INSERT INTO straordinarifestivita(id, azione, datainizio, datafine, turno) VALUES("
                            + "@id, 'S', @iUtc, @fUtc"
                            + ", @idTurno)";
                        try
                        {
                            conn.Execute(sql, new { id = maxId, iUtc = i.ToString("yyyy-MM-dd HH:mm:ss"), fUtc = f.ToString("yyyy-MM-dd HH:mm:ss"), idTurno = idTurno }, trn);
                            rt = true;
                            trn.Commit();
                        }
                        catch (Exception ex)
                        {
                            rt = false;
                            log = ex.Message;
                            trn.Rollback();
                        }
                    }
                    else
                    {
                        rt = false;
                    }
                        conn.Close();
                        return rt;
                    
                }
                else
                {
                    return rt;
                }
            }
            else
            {
                return rt;
            }
        }
    }

    public class IntervalloCalendarioReparto
    {
        protected String Tenant;

        public String log;

        /* Stati possibili
         * LAVORO NORMALE 'L'
         * LAVORO STRAORDINARIO 'S'
         * FESTIVITA 'F'
         * NON LAVORATIVO 'N'
         */

        private Char _status;
        public Char Status
        {
            get { return this._status; }
        }

        private int _idOrarioTurno;
        public int idOrarioTurno
        {
            get
            {
                if (this._status == 'L')
                {
                    return this._idOrarioTurno;
                }
                else
                {
                    return -1;
                }
            }
        }

        private int _idFestivita;
        public int idFestivita
        {
            get
            {
                if (this._status == 'F')
                {
                    return this._idFestivita;
                }
                else
                {
                    return -1;
                }
            }
        }

        private int _idStraordinario;
        public int idStraordinario
        {
            get
            {
                if (this._status == 'S')
                {
                    return this._idStraordinario;
                }
                else
                {
                    return -1;
                }
            }
        }

        private Reparto reparto;

        private DateTime _Inizio;
        public DateTime Inizio
        {
            get {
                if (reparto == null || reparto.id == -1)
                {
                Turno trn = new Turno(this.Tenant, idTurno);
                reparto = new Reparto(this.Tenant, trn.idReparto);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, this.reparto.tzFusoOrario); 
                //return this._Inizio;
            }
        }

        private DateTime _Fine;
        public DateTime Fine
        {
            get {
                if (reparto == null || reparto.id == -1)
                {
                    if(reparto==null||reparto.id==-1)
                    { 
                        Turno trn = new Turno(this.Tenant, idTurno);
                        reparto = new Reparto(this.Tenant, trn.idReparto);
                    }
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, this.reparto.tzFusoOrario); 
                //return this._Fine;
            }
        }

        private int _idTurno;
        public int idTurno
        {
            get
            {
                return this._idTurno;
            }
        }

        public IntervalloCalendarioReparto(String tenant, Char stat, int idIntervallo, DateTime I, DateTime F)
        {
            this.Tenant = tenant;
            bool controlli = true;
            // Controllo che il carattere di status sia ok
            if (stat == 'L' || stat == 'F' || stat == 'N' || stat == 'S')
            {
                controlli = true;
            }
            else
            {
                controlli = false;
            }
            // Controllo che idIntervallo sia conforme a quanto richiamato dal carattere stat
            if (controlli == true)
            {
                controlli = false;
                if (stat == 'L')
                {
                    IntervalloLavorativoTurno intTrn = new IntervalloLavorativoTurno(this.Tenant, idIntervallo);
                    if (intTrn.idIntervallo != -1)
                    {
                        controlli = true;
                        this._idTurno = intTrn.idTurno;
                    }
                }
                else if (stat == 'N')
                {
                    controlli = true;
                    this._idTurno = -1;
                }
                else if (stat == 'S')
                {
                    Straordinario str = new Straordinario(this.Tenant, idIntervallo);
                    if (str.idStraordinario != -1)
                    {
                        controlli = true;
                        this._idTurno = str.idTurno;
                        F = str.Fine;
                        //Turno trn = new Turno(idTurno);
                        //reparto = new Reparto(trn.idReparto);
                        //F = TimeZoneInfo.ConvertTimeToUtc(str.Fine, reparto.tzFusoOrario);
                    }
                }
                else if (stat == 'F')
                {
                    Festivita fst = new Festivita(this.Tenant, idIntervallo);
                    if (fst.idFestivita != -1)
                    {
                        controlli = true;
                        this._idTurno = fst.idTurno;
                    }
                }
            }

            // Controllo che Inizio < Fine
            if (controlli == true && I <= F)
            {
                controlli = true;
            }
            else
            {
                controlli = false;
            }
            

            // Se tutti i controlli sono andati a buon fine allora creo la classe altrimenti no
            if (controlli == true)
            {
                if(reparto==null || reparto.id==-1)
                { 
                    Turno trn = new Turno(this.Tenant, idTurno);
                    reparto= new Reparto(this.Tenant, trn.idReparto);
                }
                this._Inizio = TimeZoneInfo.ConvertTimeToUtc(I, reparto.tzFusoOrario);
                this._Fine = TimeZoneInfo.ConvertTimeToUtc(F, reparto.tzFusoOrario);
                this._status = stat;
                this._idOrarioTurno = -1;
                this._idFestivita = -1;
                this._idStraordinario = -1;

                if (this._status == 'L')
                {
                    this._idOrarioTurno = idIntervallo;
                }
                else if (this._status == 'S')
                {
                    this._idStraordinario = idIntervallo;
                }
                else if (this._status == 'F')
                {
                    this._idFestivita = idIntervallo;
                }
            }
            else
            {
                this._Inizio = DateTime.UtcNow;
                this._Fine = DateTime.UtcNow;
                this._status = '\0';
                this._idOrarioTurno = -1;
                this._idFestivita = -1;
                this._idStraordinario = -1;
                this._idTurno = -1;
            }
        }

        private int _NumArticoliDaTerminare;
        public int NumArticoliDaTerminare
        {
            get
            {
                return this._NumArticoliDaTerminare;
            }
        }

        private List<Articolo> _ArticoliTerminati;
        public List<Articolo> ArticoliTerminati
        {
            get
            {
                return this._ArticoliTerminati;
            }
        }

        public void loadArticoliDaTerminare(Reparto rp)
        {
            log = " ";
            this._NumArticoliDaTerminare = 0;
            List<Articolo> ElencoArticoliDaTerminareReparto = new List<Articolo>();

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(DateTime dataFineProd, int idArticolo, int annoArticolo)>("SELECT MAX(lateFinish) as dataFineProd, idArticolo, annoArticolo FROM tasksproduzione "
                + " INNER JOIN productionplan ON (idArticolo=productionplan.id AND annoarticolo=productionplan.anno) "
                + " WHERE productionplan.reparto = @reparto"
                + " GROUP BY idArticolo, annoArticolo "
                + " ORDER BY dataFineProd DESC", new { reparto = rp.id });
            foreach (var r in rows)
            {
                //Articolo art = new Articolo(rdr.GetInt32(1), rdr.GetInt32(2));
                DateTime fineTask = r.dataFineProd;
                //DateTime fineTask = art.DataFineUltimoTask;
                if (this.Inizio <= fineTask && fineTask <= this.Fine)
                {
                    Articolo art = new Articolo(this.Tenant, r.idArticolo, r.annoArticolo);
                    log += art.ID.ToString() + "/" + art.Year.ToString() + " ";
                    ElencoArticoliDaTerminareReparto.Add(art);
                    this._NumArticoliDaTerminare++;
                }
            }
            conn.Close();
            
        }

        public void loadArticoliTerminati(Reparto rp)
        {
            this._ArticoliTerminati = new List<Articolo>();
            List<Articolo> elencoTerminati = new List<Articolo>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var rows = conn.Query<(int id, int anno)>("SELECT id, anno FROM productionplan WHERE status = 'F' AND reparto = @reparto", new { reparto = rp.id });
            foreach (var r in rows)
            {
                elencoTerminati.Add(new Articolo(this.Tenant, r.id, r.anno));
            }
            for (int i = 0; i < elencoTerminati.Count; i++)
            {
                elencoTerminati[i].loadTasksProduzione();
                DateTime lastTask = new DateTime(1900, 1, 1);
                for (int j = 0; j < elencoTerminati[i].Tasks.Count; j++)
                {
                    if (lastTask < elencoTerminati[i].Tasks[j].DataFineTask)
                    {
                        lastTask = elencoTerminati[i].Tasks[j].DataFineTask;
                    }
                }

                if (this.Inizio <= lastTask && lastTask <= this.Fine)
                {
                    this._ArticoliTerminati.Add(elencoTerminati[i]);
                }
            }
            conn.Close();
        }
    }

    public class CalendarioReparto
    {
        protected String Tenant;

        public String log;
        public DateTime InizioCal;
        public DateTime FineCal;
        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        public Reparto rep;

        public List<IntervalloCalendarioReparto> Turni;

        public List<IntervalloCalendarioReparto> Intervalli;

        public CalendarioReparto(String tenant, int rp, DateTime InizioCal, DateTime FineCal)
        {
            this.Tenant = tenant;
            Intervalli = new List<IntervalloCalendarioReparto>();
            Turni = new List<IntervalloCalendarioReparto>();
            if (InizioCal <= FineCal)
            {                
                // Comincio con il trovare tutti i turni di lavoro del reparto
                Reparto rep = new Reparto(this.Tenant, rp);
                rep.loadTurni();
                this._idReparto = rep.id;
                List<IntervalloLavorativoTurno> sommaTurni = new List<IntervalloLavorativoTurno>();
                //Turni = new List<IntervalloLavorativoTurno>();
                for (int i = 0; i < rep.Turni.Count; i++)
                {
                    for (int j = 0; j < rep.Turni[i].OrariDiLavoro.Count; j++)
                    {
                        sommaTurni.Add(new IntervalloLavorativoTurno(this.Tenant, rep.Turni[i].OrariDiLavoro[j].idIntervallo));
                    }
                }
                
                // Ri-ordino i turni di lavoro
                /*IntervalloLavorativoTurno swap;
                for (int i = 0; i < sommaTurni.Count; i++)
                {
                    for (int j = i; j < sommaTurni.Count; j++)
                    {
                        if (sommaTurni[j].Inizio < sommaTurni[i].Inizio)
                        {
                            // Li scambio
                            swap = new IntervalloLavorativoTurno(sommaTurni[i].idIntervallo);
                            sommaTurni[i] = sommaTurni[j];
                            sommaTurni[j] = swap;
                        }
                    }
                }*/

                sommaTurni = sommaTurni.OrderBy(x => x.Inizio).ToList();
                
                // Ora collego i turni al calendario per capire da quale partire
                List<DateTime> sommaTurniData = new List<DateTime>();
                List<TimeSpan> DistI = new List<TimeSpan>();
                TimeSpan minDist = new TimeSpan(1000, 23, 59, 59, 0);
                int minIndex = -1;
                for (int i = 0; i < sommaTurni.Count; i++)
                {
                    DateTime nxt = this.findNextOccurrence(sommaTurni[i].GiornoInizio, sommaTurni[i].OraInizio, InizioCal);
                    sommaTurniData.Add(nxt);
                    DistI.Add(sommaTurniData[i] - InizioCal);
                    if(DistI[i] < minDist)
                    {
                        minDist = DistI[i];
                        minIndex = i;
                    }
                }

                if (minIndex > -1)
                {
                    DateTime attuale = InizioCal;
                    int cont = minIndex;
                    int contIntervalli = 0;
                    while (attuale <= FineCal)
                    {
                        DateTime dtI = this.findNextOccurrence(sommaTurni[cont].GiornoInizio, sommaTurni[cont].OraInizio, attuale);
                        DateTime dtF = this.findNextOccurrence(sommaTurni[cont].GiornoFine, sommaTurni[cont].OraFine, dtI);
                        IntervalloCalendarioReparto intCalRep = new IntervalloCalendarioReparto(this.Tenant, 'L', sommaTurni[cont].idIntervallo, dtI, dtF);
                        Intervalli.Add(intCalRep);
                        Turni.Add(intCalRep);

                        attuale = dtF;
                        //log += Intervalli[contIntervalli].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + Intervalli[contIntervalli].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                        contIntervalli++;
                        if (cont == sommaTurni.Count - 1)
                        {
                            cont = 0;
                        }
                        else
                        {
                            cont++;
                        }

                    }
                }

                // ORA CARICO GLI STRAORDINARI
                ElencoStraordinari straord = new ElencoStraordinari(this.Tenant, this.idReparto);
                // Ricerco il primo straordinario da inserire
                int k = 0;
                while(k < straord.Straordinari.Count && straord.Straordinari[k].Inizio < InizioCal && straord.Straordinari[k].Fine < FineCal)
                {
                    k++;
                }
                while (k < straord.Straordinari.Count && straord.Straordinari[k].Inizio < FineCal)
                {
                        DateTime st = new DateTime();
                        DateTime en = new DateTime();
                        if (straord.Straordinari[k].Inizio <= InizioCal && straord.Straordinari[k].Fine >= InizioCal)
                        {
                            st = InizioCal;
                        }
                        else
                        {
                            st = straord.Straordinari[k].Inizio;
                        }
                        if (straord.Straordinari[k].Fine >= FineCal)
                        {
                            en = FineCal;
                        }
                        else
                        {
                            en = straord.Straordinari[k].Fine;
                        }
                        Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'S', straord.Straordinari[k].idStraordinario, st, en));
                        k++;
                }

                // Ora carico le festività
                k = 0;
                ElencoFestivita elFest = new ElencoFestivita(this.Tenant, this.idReparto);
                while (k < elFest.feste.Count && elFest.feste[k].Inizio < InizioCal)
                {
                    log += elFest.feste[k].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " " + elFest.feste[k].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                    k++;
                }

                log += "<br/>" + k.ToString() + "<br/>";

                while (k < elFest.feste.Count)
                {
                    int indInterv = 0;
                    while (indInterv < this.Intervalli.Count && !(elFest.feste[k].Inizio >= Intervalli[indInterv].Inizio && elFest.feste[k].Fine <= Intervalli[indInterv].Fine))
                    {
                        indInterv++;
                    }
                    log += "indInterv " + indInterv.ToString() +"<br/>";

                    // Se ho trovato l'intervallo corretto...
                    if (indInterv < this.Intervalli.Count)
                    {
                        
                        log += elFest.feste[k].Inizio.ToString("yyyy-MM-dd HH:mm:ss") + " == " + this.Intervalli[indInterv].Inizio.ToString("yyyy-MM-dd HH:mm:ss") + " " + elFest.feste[k].Fine.ToString("yyyy-MM-dd HH:mm:ss") + " == " + this.Intervalli[indInterv].Fine.ToString("yyyy-MM-dd HH:mm:ss") + "<br/>";
                        //Primo caso: festivita copre interamente il turno
                        if (elFest.feste[k].Inizio == this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 1<br/>";
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Secondo caso: Iniziofest = Inizio e Finefest < FineInterv
                        else if (elFest.feste[k].Inizio == this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine < this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 2<br/>";
                            DateTime i = elFest.feste[k].Fine;
                            DateTime f = this.Intervalli[indInterv].Fine;
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Terzo caso: Iniziofest > Inizio e Finefest = FineInterv
                        else if (elFest.feste[k].Inizio > this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 3<br/>";
                            DateTime i = this.Intervalli[indInterv].Inizio;
                            DateTime f = elFest.feste[k].Inizio;
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Quarto caso: Iniziofest > Inizio e Finefest < FineInterv
                        else if (elFest.feste[k].Inizio > this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine < this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 4<br/>";
                            DateTime i1 = this.Intervalli[indInterv].Inizio;
                            DateTime f1 = elFest.feste[k].Inizio;
                            DateTime i2 = elFest.feste[k].Fine;
                            DateTime f2 = this.Intervalli[indInterv].Fine;
                            int idIntervallo = this.Intervalli[indInterv].idOrarioTurno;
                            this.Intervalli.RemoveAt(indInterv);
                            //Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i1, f1));
                            //Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i2, f2));
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', idIntervallo, i1, f1));
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', idIntervallo, i2, f2));
                        }
                    }
                    k++;
                }
                // Riordino l'array Intervalli
                sortIntervalli();
            }
            else
            {
                InizioCal = DateTime.UtcNow;
                FineCal = DateTime.UtcNow;
                this._idReparto = rp;
            }
        }

        public CalendarioReparto(String tenant, Reparto rp, DateTime InizioCal, DateTime FineCal)
        {
            this.Tenant = tenant;
            Intervalli = new List<IntervalloCalendarioReparto>();
            Turni = new List<IntervalloCalendarioReparto>();
            if (InizioCal <= FineCal)
            {
                // Comincio con il trovare tutti i turni di lavoro del reparto
                this.rep = rp;
                if(rep.Turni==null || rep.Turni.Count ==0)
                { 
                    rep.loadTurni();
                }
                this._idReparto = rep.id;
                List<IntervalloLavorativoTurno> sommaTurni = new List<IntervalloLavorativoTurno>();
                //Turni = new List<IntervalloLavorativoTurno>();
                for (int i = 0; i < rep.Turni.Count; i++)
                {
                    for (int j = 0; j < rep.Turni[i].OrariDiLavoro.Count; j++)
                    {
                        //sommaTurni.Add(new IntervalloLavorativoTurno(rep.Turni[i].OrariDiLavoro[j].idIntervallo));
                        sommaTurni.Add(rep.Turni[i].OrariDiLavoro[j]);
                    }
                }

                // Ri-ordino i turni di lavoro
                sommaTurni = sommaTurni.OrderBy(x => x.Inizio).ToList();

                // Ora collego i turni al calendario per capire da quale partire
                List<DateTime> sommaTurniData = new List<DateTime>();
                List<TimeSpan> DistI = new List<TimeSpan>();
                TimeSpan minDist = new TimeSpan(1000, 23, 59, 59, 0);
                int minIndex = -1;
                for (int i = 0; i < sommaTurni.Count; i++)
                {
                    DateTime nxt = this.findNextOccurrence(sommaTurni[i].GiornoInizio, sommaTurni[i].OraInizio, InizioCal);
                    sommaTurniData.Add(nxt);
                    DistI.Add(sommaTurniData[i] - InizioCal);
                    if (DistI[i] < minDist)
                    {
                        minDist = DistI[i];
                        minIndex = i;
                    }
                }

                if (minIndex > -1)
                {
                    DateTime attuale = InizioCal;
                    int cont = minIndex;
                    int contIntervalli = 0;
                    while (attuale <= FineCal)
                    {
                        DateTime dtI = this.findNextOccurrence(sommaTurni[cont].GiornoInizio, sommaTurni[cont].OraInizio, attuale);
                        DateTime dtF = this.findNextOccurrence(sommaTurni[cont].GiornoFine, sommaTurni[cont].OraFine, dtI);
                        IntervalloCalendarioReparto intCalRep = new IntervalloCalendarioReparto(this.Tenant, 'L', sommaTurni[cont].idIntervallo, dtI, dtF);
                        Intervalli.Add(intCalRep);
                        Turni.Add(intCalRep);

                        attuale = dtF;
                        //log += Intervalli[contIntervalli].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + Intervalli[contIntervalli].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                        contIntervalli++;
                        if (cont == sommaTurni.Count - 1)
                        {
                            cont = 0;
                        }
                        else
                        {
                            cont++;
                        }

                    }
                }

                // ORA CARICO GLI STRAORDINARI
                ElencoStraordinari straord = new ElencoStraordinari(this.Tenant, this.idReparto);
                // Ricerco il primo straordinario da inserire
                int k = 0;
                while (k < straord.Straordinari.Count && straord.Straordinari[k].Inizio < InizioCal && straord.Straordinari[k].Fine < FineCal)
                {
                    k++;
                }
                while (k < straord.Straordinari.Count && straord.Straordinari[k].Inizio < FineCal)
                {
                    DateTime st = new DateTime();
                    DateTime en = new DateTime();
                    if (straord.Straordinari[k].Inizio <= InizioCal && straord.Straordinari[k].Fine >= InizioCal)
                    {
                        st = InizioCal;
                    }
                    else
                    {
                        st = straord.Straordinari[k].Inizio;
                    }
                    if (straord.Straordinari[k].Fine >= FineCal)
                    {
                        en = FineCal;
                    }
                    else
                    {
                        en = straord.Straordinari[k].Fine;
                    }
                    Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant,'S', straord.Straordinari[k].idStraordinario, st, en));
                    k++;
                }

                // Ora carico le festività
                k = 0;
                ElencoFestivita elFest = new ElencoFestivita(this.Tenant, this.idReparto);
                while (k < elFest.feste.Count && elFest.feste[k].Inizio < InizioCal)
                {
                    log += elFest.feste[k].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " " + elFest.feste[k].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                    k++;
                }

                log += "<br/>" + k.ToString() + "<br/>";

                while (k < elFest.feste.Count)
                {
                    int indInterv = 0;
                    while (indInterv < this.Intervalli.Count && !(elFest.feste[k].Inizio >= Intervalli[indInterv].Inizio && elFest.feste[k].Fine <= Intervalli[indInterv].Fine))
                    {
                        indInterv++;
                    }
                    log += "indInterv " + indInterv.ToString() + "<br/>";

                    // Se ho trovato l'intervallo corretto...
                    if (indInterv < this.Intervalli.Count)
                    {

                        log += elFest.feste[k].Inizio.ToString("yyyy-MM-dd HH:mm:ss") + " == " + this.Intervalli[indInterv].Inizio.ToString("yyyy-MM-dd HH:mm:ss") + " " + elFest.feste[k].Fine.ToString("yyyy-MM-dd HH:mm:ss") + " == " + this.Intervalli[indInterv].Fine.ToString("yyyy-MM-dd HH:mm:ss") + "<br/>";
                        //Primo caso: festivita copre interamente il turno
                        if (elFest.feste[k].Inizio == this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 1<br/>";
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Secondo caso: Iniziofest = Inizio e Finefest < FineInterv
                        else if (elFest.feste[k].Inizio == this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine < this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 2<br/>";
                            DateTime i = elFest.feste[k].Fine;
                            DateTime f = this.Intervalli[indInterv].Fine;
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Terzo caso: Iniziofest > Inizio e Finefest = FineInterv
                        else if (elFest.feste[k].Inizio > this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 3<br/>";
                            DateTime i = this.Intervalli[indInterv].Inizio;
                            DateTime f = elFest.feste[k].Inizio;
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Quarto caso: Iniziofest > Inizio e Finefest < FineInterv
                        else if (elFest.feste[k].Inizio > this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine < this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 4<br/>";
                            DateTime i1 = this.Intervalli[indInterv].Inizio;
                            DateTime f1 = elFest.feste[k].Inizio;
                            DateTime i2 = elFest.feste[k].Fine;
                            DateTime f2 = this.Intervalli[indInterv].Fine;
                            int idIntervallo = this.Intervalli[indInterv].idOrarioTurno;
                            this.Intervalli.RemoveAt(indInterv);
                            //Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i1, f1));
                            //Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i2, f2));
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', idIntervallo, i1, f1));
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', idIntervallo, i2, f2));
                        }
                    }
                    k++;
                }
                // Riordino l'array Intervalli
                sortIntervalli();
            }
            else
            {
                InizioCal = DateTime.UtcNow;
                FineCal = DateTime.UtcNow;
                this._idReparto = rp.id;
            }
        }

        protected DateTime findNextOccurrence(DayOfWeek giorno, TimeSpan orario, DateTime inizio)
        {
            DateTime ret = new DateTime(inizio.Year, inizio.Month, inizio.Day, orario.Hours, orario.Minutes, orario.Seconds);
            if (ret < inizio && ret.DayOfWeek == inizio.DayOfWeek)
            {
                ret = ret.AddDays(1);
            }
            while (ret.DayOfWeek != giorno)
            {
                ret = ret.AddDays(1);
            }
            return ret;
        }

        protected void sortIntervalli()
        {
            if (this.idReparto != -1)
            {
                /* IntervalloCalendarioReparto swap;
                 for (int i = 0; i < this.Intervalli.Count; i++)
                 {
                     for (int j = i; j < this.Intervalli.Count; j++)
                     {
                         if (this.Intervalli[j].Inizio < this.Intervalli[i].Inizio)
                         {
                             // Li scambio
                             swap = this.Intervalli[i];
                             this.Intervalli[i] = this.Intervalli[j];
                             this.Intervalli[j] = swap;
                         }
                     }
                 }*/

                this.Intervalli = this.Intervalli.OrderBy(x => x.Inizio).ToList();
            }
        }
    }

    public class UtentiReparto
    {
        protected String Tenant;

        public String log;

        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        private List<User> _Elenco;
        public List<User> Elenco
        {
            get { return this._Elenco; }
        }

        public UtentiReparto(String tenant, int repID)
        {
            this.Tenant = tenant;
            Reparto rp = new Reparto(this.Tenant, repID);
            if (rp.id != -1)
            {
                this._idReparto = rp.id;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var usernames = conn.Query<string>("SELECT operatore FROM operatorireparto WHERE reparto = @reparto", new { reparto = repID });
                this._Elenco = new List<User>();
                foreach (var username in usernames)
                {
                    this._Elenco.Add(new User(username));
                }
                conn.Close();
            }
            else
            {
                this._idReparto = -1;
            }
        }

        public bool Add(User usr)
        {
            bool rt = false;
            if (this.idReparto != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO operatorireparto(operatore, reparto) VALUES(@operatore, @reparto)";
                try
                {
                    conn.Execute(sql, new { operatore = usr.username, reparto = this.idReparto }, tr);
                    tr.Commit();
                    rt = true;
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        public bool Delete(User usr)
        {
            bool rt = false;
            if (this.idReparto != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "DELETE FROM operatorireparto WHERE operatore = @operatore AND reparto = @reparto";
                try
                {
                    conn.Execute(sql, new { operatore = usr.username, reparto = this.idReparto }, tr);
                    tr.Commit();
                    rt = true;
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
    }

    public class CalendarioTurno
    {
        protected String Tenant;

        public String log;
        public DateTime InizioCal;
        public DateTime FineCal;
        private int _idTurno;
        public int idTurno
        {
            get { return this._idTurno; }
        }

        public List<IntervalloCalendarioReparto> Turni;

        public List<IntervalloCalendarioReparto> Intervalli;

        public CalendarioTurno(String tenant, int turno, DateTime InizioCal, DateTime FineCal)
        {
            this.Tenant = tenant;
            Intervalli = new List<IntervalloCalendarioReparto>();
            Turni = new List<IntervalloCalendarioReparto>();
            if (InizioCal <= FineCal)
            {

                // Comincio con il trovare tutti i turni di lavoro del reparto
                Turno trn = new Turno(this.Tenant, turno);
                this._idTurno = trn.id;

                // Ora collego i turni al calendario per capire da quale partire
                List<DateTime> sommaTurniData = new List<DateTime>();
                List<TimeSpan> DistI = new List<TimeSpan>();
                TimeSpan minDist = new TimeSpan(1000, 23, 59, 59, 0);
                int minIndex = -1;
                for (int i = 0; i < trn.OrariDiLavoro.Count; i++)
                {
                    DateTime nxt = this.findNextOccurrence(trn.OrariDiLavoro[i].GiornoInizio, trn.OrariDiLavoro[i].OraInizio, InizioCal);
                    //log += nxt.ToString("dd/MM/yyyy HH:mm:ss") + " - ";
                    sommaTurniData.Add(nxt);
                    DistI.Add(sommaTurniData[i] - InizioCal);
                    if (DistI[i] < minDist)
                    {
                        minDist = DistI[i];
                        minIndex = i;
                    }
                    //log += DistI[i] + " minDist: " + minDist.Ticks.ToString() + " minIndex: " + minIndex + "<br/>";
                }

                if (minIndex > -1)
                {
                    DateTime attuale = InizioCal;
                    int cont = minIndex;
                    int contIntervalli = 0;
                    while (attuale <= FineCal)
                    {
                        DateTime dtI = this.findNextOccurrence(trn.OrariDiLavoro[cont].GiornoInizio, trn.OrariDiLavoro[cont].OraInizio, attuale);
                        DateTime dtF = this.findNextOccurrence(trn.OrariDiLavoro[cont].GiornoFine, trn.OrariDiLavoro[cont].OraFine, dtI);
                        IntervalloCalendarioReparto intCalRep = new IntervalloCalendarioReparto(this.Tenant, 'L', trn.OrariDiLavoro[cont].idIntervallo, dtI, dtF);
                        Intervalli.Add(intCalRep);
                        Turni.Add(intCalRep);

                        attuale = dtF;
                        //log += Intervalli[contIntervalli].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + Intervalli[contIntervalli].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                        contIntervalli++;
                        if (cont == trn.OrariDiLavoro.Count - 1)
                        {
                            cont = 0;
                        }
                        else
                        {
                            cont++;
                        }

                    }
                }

                // ORA CARICO GLI STRAORDINARI
                trn.loadStraordinari();
                // Ricerco il primo straordinario da inserire
                int k = 0;
                while (k < trn.straordinari.Count && trn.straordinari[k].Inizio < InizioCal && trn.straordinari[k].Fine < FineCal)
                {
                    k++;
                }
                while (k < trn.straordinari.Count && trn.straordinari[k].Inizio < FineCal)
                {
                    DateTime st = new DateTime();
                    DateTime en = new DateTime();
                    if (trn.straordinari[k].Inizio <= InizioCal && trn.straordinari[k].Fine >= InizioCal)
                    {
                        st = InizioCal;
                    }
                    else
                    {
                        st = trn.straordinari[k].Inizio;
                    }
                    if (trn.straordinari[k].Fine >= FineCal)
                    {
                        en = FineCal;
                    }
                    else
                    {
                        en = trn.straordinari[k].Fine;
                    }
                    Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'S', trn.straordinari[k].idStraordinario, st, en));
                    k++;
                }

                // Ora carico le festività
                k = 0;
                trn.loadFestivita();
                while (k < trn.festivita.Count && trn.festivita[k].Inizio < InizioCal)
                {
                    log += trn.festivita[k].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " " + trn.festivita[k].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                    k++;
                }

                log += "<br/>" + k.ToString() + "<br/>";

                while (k < trn.festivita.Count)
                {
                    int indInterv = 0;
                    while (indInterv < this.Intervalli.Count && !(trn.festivita[k].Inizio >= Intervalli[indInterv].Inizio && trn.festivita[k].Fine <= Intervalli[indInterv].Fine))
                    {
                        indInterv++;
                    }
                    log += "indInterv " + indInterv.ToString() + "<br/>";

                    // Se ho trovato l'intervallo corretto...
                    if (indInterv < this.Intervalli.Count)
                    {

                        log += trn.festivita[k].Inizio.ToString("yyyy-MM-dd HH:mm:ss") + " == " + this.Intervalli[indInterv].Inizio.ToString("yyyy-MM-dd HH:mm:ss") + " " + trn.festivita[k].Fine.ToString("yyyy-MM-dd HH:mm:ss") + " == " + this.Intervalli[indInterv].Fine.ToString("yyyy-MM-dd HH:mm:ss") + "<br/>";
                        //Primo caso: festivita copre interamente il turno
                        if (trn.festivita[k].Inizio == this.Intervalli[indInterv].Inizio && trn.festivita[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 1<br/>";
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Secondo caso: Iniziofest = Inizio e Finefest < FineInterv
                        else if (trn.festivita[k].Inizio == this.Intervalli[indInterv].Inizio && trn.festivita[k].Fine < this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 2<br/>";
                            DateTime i = trn.festivita[k].Fine;
                            DateTime f = this.Intervalli[indInterv].Fine;
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Terzo caso: Iniziofest > Inizio e Finefest = FineInterv
                        else if (trn.festivita[k].Inizio > this.Intervalli[indInterv].Inizio && trn.festivita[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 3<br/>";
                            DateTime i = this.Intervalli[indInterv].Inizio;
                            DateTime f = trn.festivita[k].Inizio;
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Quarto caso: Iniziofest > Inizio e Finefest < FineInterv
                        else if (trn.festivita[k].Inizio > this.Intervalli[indInterv].Inizio && trn.festivita[k].Fine < this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 4<br/>";
                            DateTime i1 = this.Intervalli[indInterv].Inizio;
                            DateTime f1 = trn.festivita[k].Inizio;
                            DateTime i2 = trn.festivita[k].Fine;
                            DateTime f2 = this.Intervalli[indInterv].Fine;
                            int idIntervallo = this.Intervalli[indInterv].idOrarioTurno;
                            this.Intervalli.RemoveAt(indInterv);
                            //Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i1, f1));
                            //Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i2, f2));
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', idIntervallo, i1, f1));
                            Intervalli.Add(new IntervalloCalendarioReparto(this.Tenant, 'L', idIntervallo, i2, f2));
                        }
                    }
                    k++;
                }
                // Riordino l'array Intervalli
                sortIntervalli();
            }
            else
            {
                InizioCal = DateTime.UtcNow;
                FineCal = DateTime.UtcNow;
                this._idTurno = idTurno;
            }
        }

        protected DateTime findNextOccurrence(DayOfWeek giorno, TimeSpan orario, DateTime inizio)
        {
            DateTime ret = new DateTime(inizio.Year, inizio.Month, inizio.Day, orario.Hours, orario.Minutes, orario.Seconds);
            if (ret < inizio && ret.DayOfWeek == inizio.DayOfWeek)
            {
                ret = ret.AddDays(1);
            }
            while (ret.DayOfWeek != giorno)
            {
                ret = ret.AddDays(1);
            }
            return ret;
        }

        protected void sortIntervalli()
        {
                IntervalloCalendarioReparto swap;
                for (int i = 0; i < this.Intervalli.Count; i++)
                {
                    for (int j = i; j < this.Intervalli.Count; j++)
                    {
                        if (this.Intervalli[j].Inizio < this.Intervalli[i].Inizio)
                        {
                            // Li scambio
                            swap = this.Intervalli[i];
                            this.Intervalli[i] = this.Intervalli[j];
                            this.Intervalli[j] = swap;
                        }
                    
                }
            }
        }
    }

    public class RisorseTurno
    {
        protected String Tenant;

        private List<RisorsePostazioneTurno> _Postazioni;
        public List<RisorsePostazioneTurno> Postazioni
        { get { return this._Postazioni; } }

        public RisorseTurno(String tenant, Turno trn)
        {
            this.Tenant = tenant;
            this._Postazioni = new List<RisorsePostazioneTurno>();

            if (trn.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                var ids = conn.Query<int>("SELECT idPostazione FROM risorseturnopostazione WHERE idTurno = @idTurno", new { idTurno = trn.id });
                foreach (var id in ids)
                {
                    this._Postazioni.Add(new RisorsePostazioneTurno(this.Tenant, new Postazione(this.Tenant, id), trn));
                }
                conn.Close();
            }
        }
    }
}