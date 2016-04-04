using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using KIS.Commesse;
using KIS.eventi;
using KIS.App_Code;

namespace KIS
{
    public class Reparto
    {
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                    int newCadenza = value.Seconds + value.Minutes * 60 + value.Hours * 3600;
                    cmd.CommandText = "UPDATE reparti SET cadenza = " + newCadenza.ToString() + " WHERE idReparto = " + this.id.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = trans;
                    cmd.CommandText = "UPDATE reparti SET splitTasks = " + value.ToString() + " WHERE idreparto = " + this.id.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction trans = conn.BeginTransaction();
                    cmd.CommandText = "UPDATE reparti SET anticipoTasks = " + value.TotalSeconds.ToString() + " WHERE idreparto = " + this.id.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione Like 'Reparto' AND "
                            + " ID = " + this.id.ToString()
                            + " AND parametro LIKE 'KanbanManaged'";
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        String strSQL = "";
                        if (rdr.Read() && !rdr.IsDBNull(0))
                        {
                            strSQL = "UPDATE configurazione SET valore = " + param
                                + " WHERE Sezione LIKE 'Reparto' "
                                + " AND ID = " + this.id.ToString()
                                + " AND parametro LIKE 'KanbanManaged'";
                        }
                        else
                        {
                            strSQL = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                                + "'Reparto', "
                                + this.id.ToString() + ", "
                                + "'KanbanManaged'" + ", "
                                + param.ToString() + ")";
                        }
                        rdr.Close();
                        cmd.CommandText = strSQL;
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        this._KanbanManaged = value;
                    }
                }
                
                this._KanbanManaged = value;
            }
        }

        public Reparto()
        {
            this._id = -1;
        }

        public Reparto(int repID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idreparto, nome, descrizione, cadenza, splitTasks, anticipoTasks, ModoCalcoloTC FROM reparti WHERE idReparto = " + repID.ToString();
            try
            {
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                this._id = rdr.GetInt32(0);
                this._name = rdr.GetString(1);
                this._description = rdr.GetString(2);
                if (!rdr.IsDBNull(3))
                {
                    this._Cadenza = TimeSpan.FromSeconds(rdr.GetFloat(3));
                }
                else
                {
                    this._Cadenza = new TimeSpan(0, 0, 0);
                }
                this._splitTasks = rdr.GetBoolean(4);
                long anticipo = rdr.GetInt32(5);
                this._anticipoMinimoTasks = new TimeSpan(anticipo * 10000000);
                this._ModoCalcoloTC = rdr.GetBoolean(6);
                this.loadConfigurazioneKanban();
            }
            catch
            {
                this._id = -1;
                this._name = "";
                this._description = "";
                this._splitTasks = false;
                this._Cadenza = new TimeSpan(0, 0, 0);
                this._KanbanManaged = false;
            }
            conn.Close();
        }

        public Reparto(String name)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idreparto, nome, descrizione, cadenza, splitTasks, anticipoTasks, ModoCalcoloTC FROM reparti WHERE nome LIKE '" + name + "'";
            try
            {
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                this._id = rdr.GetInt32(0);
                this._name = rdr.GetString(1);
                this._description = rdr.GetString(2);
                if (!rdr.IsDBNull(3))
                {
                    this._Cadenza = TimeSpan.FromSeconds(rdr.GetFloat(3));
                }
                else
                {
                    this._Cadenza = new TimeSpan(0, 0, 0);
                }
                this._splitTasks = rdr.GetBoolean(4);
                long anticipo = rdr.GetInt32(5);
                this._anticipoMinimoTasks = new TimeSpan(anticipo * 10000000);
                this._ModoCalcoloTC = rdr.GetBoolean(6);
                this.loadConfigurazioneKanban();
            }
            catch
            {
                this._id = -1;
                this._name = "";
                this._description = "";
                this._splitTasks = false;
                this._Cadenza = new TimeSpan(0, 0, 0);
                this._KanbanManaged = false;
            }
            conn.Close();
        }

        public bool Add(String nome, String descrizione)
        {
            bool rt;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(idReparto) FROM reparti";
            MySqlDataReader rdr = cmd.ExecuteReader();
            int repID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                repID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();

            MySqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            cmd.CommandText = "INSERT INTO reparti(idReparto, nome, descrizione, cadenza, splitTasks, anticipoTasks, ModoCalcoloTC) VALUES(" 
                + repID.ToString() + ", '" + nome + "', '" + descrizione + "', 0, 0, 0, false)";
            try
            {
                cmd.ExecuteNonQuery();
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
            return rt;
        }

        public bool loadTurni()
        {
            this._Turni = new List<Turno>();
            bool rt = false;;
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id FROM turniproduzione WHERE reparto = " + this.id.ToString() + " ORDER BY nome";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._Turni.Add(new Turno(rdr.GetInt32(0)));
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(id) FROM turniproduzione";
                MySqlDataReader rdr = cmd.ExecuteReader();
                int turnoID = 0;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    turnoID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.CommandText = "INSERT INTO turniproduzione(id, reparto, nome, colore) VALUES(" + turnoID.ToString()
                    + ", " + this.id.ToString() + ", '" + nome + "', '" + colore + "')";
                try
                {
                    rt = true;
                    cmd.ExecuteNonQuery();
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
                

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;

                try
                {
                    cmd.CommandText = "DELETE FROM straordinarifestivita WHERE turno = " + idTurno.id.ToString();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM turniproduzione WHERE id = " + idTurno.id.ToString() + " AND reparto = " + this.id.ToString();
                    cmd.ExecuteNonQuery();
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
            this.CalendarioRep = new CalendarioReparto(this.id, I, F);
        }

        public Turno trovaProssimoTurno()
        {
            Turno prossimo;
            this.loadTurni();
            double[] distanze = new double[this.Turni.Count];

            // Calcolo la "distanza" in secondi di ogni turno
            for(int i = 0; i < this.Turni.Count; i++)
            {
/*                DateTime prossimaData = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Turni[i].oraInizio.Hours, Turni[i].oraInizio.Minutes, Turni[i].oraInizio.Seconds);
                // casi: giorno della settimana ancora da venire, giorno in corso, giorno già passato
                if (prossimaData < DateTime.Now)
                {
                    prossimaData = prossimaData.AddDays(1);
                }
                
                while (prossimaData.DayOfWeek != this.Turni[i].GiornoInizio)
                {
                    prossimaData = prossimaData.AddDays(1);
                }
                distanze[i] = (prossimaData - DateTime.Now).TotalSeconds;
                */
            }

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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.CommandText = "INSERT INTO repartiprocessi(idReparto, processID, revisione, variante) VALUES("
                    + this.id.ToString() + ", " + prc.processID.ToString() + ", " + prc.revisione.ToString()
                    + ", " + vr.idVariante.ToString() + ")";

                try
                {
                    cmd.ExecuteNonQuery();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT repartiprocessi.processID, repartiprocessi.revisione, repartiprocessi.variante FROM repartiprocessi WHERE "
                    + " idReparto = " + this.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    ProcessoVariante vr = new ProcessoVariante(new processo(rdr.GetInt32(0), rdr.GetInt32(1)),  new variante(rdr.GetInt32(2)));
                    if(vr != null && vr.process != null && vr.variant != null)
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.CommandText = "DELETE FROM repartiprocessi WHERE idReparto = " + this.id.ToString() + " AND "
                    +" processID = " + prc.processID.ToString() + " AND revisione = " + prc.revisione.ToString()
                    + " AND variante = " + vr.idVariante.ToString();

                try
                {
                    cmd.ExecuteNonQuery();
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
                for (int i = 0; i < prc.process.subProcessi.Count; i++)
                {
                    TaskVariante figlio = new TaskVariante(prc.process.subProcessi[i], prc.variant);
                    RepartoProcessoPostazione implementazione = new RepartoProcessoPostazione(this.id, figlio);
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO repartipostazioniattivita(reparto, postazione, processo, revProc, variante) VALUES("
                + this.id.ToString() + ", " + post.id.ToString() + ", " + prc.Task.processID.ToString()
                + ", " + prc.Task.revisione.ToString() + ", " + prc.variant.idVariante + ")";
            try
            {
                cmd.ExecuteNonQuery();
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM repartipostazioniattivita WHERE reparto = " + this.id.ToString()
            + " AND postazione = " + post.id.ToString() + " AND processo = " + prc.Task.processID.ToString()
            + " AND revProc = " + prc.Task.revisione.ToString() + " AND variante = " + prc.variant.idVariante.ToString();
            try
            {
                cmd.ExecuteNonQuery();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM repartipostazioniattivita WHERE reparto = " + this.id.ToString()
                + " AND processo = " + prc.Task.processID.ToString()
                + " AND revProc = " + prc.Task.revisione.ToString() + " AND variante = " + prc.variant.idVariante.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
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

        private ProductionPlan _PianoProduzione;
        public ProductionPlan PianoProduzione
        {
            get { return this._PianoProduzione; }
        }

        public void loadProductionPlan()
        {
            this._PianoProduzione = new ProductionPlan(this);
        }

        public void loadOperatori()
        {
            this._Operatori = new UtentiReparto(this.id);
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(postazione) FROM repartipostazioniattivita WHERE reparto = " + this.id.ToString()
                    + " ORDER BY postazione";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this._Postazioni.Add(new Postazione(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        
        }

        public ConfigurazioneRitardoReparto EventoRitardo;
        public void loadEventoRitardo()
        {
            if (this.id != -1)
            {
                this.EventoRitardo = new ConfigurazioneRitardoReparto(this.id);
            }
        }

        public ConfigurazioneWarningReparto EventoWarning;
        public void loadEventoWarning()
        {
            if (this.id != -1)
            {
                this.EventoWarning = new ConfigurazioneWarningReparto(this.id);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.CommandText = "UPDATE reparti SET ModoCalcoloTC = " + value + " WHERE idreparto = " + this.id.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                    + "AND ID = " + this.id.ToString() + " AND parametro LIKE 'Andon FormatoUsername'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    String valore = rdr.GetString(0);
                    if (valore.Length > 0)
                    {
                        ret = valore[0];
                    }
                }
                rdr.Close();

                conn.Close();
                return ret;
            }

            set
            {
                if (this.id != -1)
                {
                    // Verifico che sia presente la configurazione
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                        + "AND ID = " + this.id.ToString() + " AND parametro LIKE 'Andon FormatoUsername'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    bool add = false;
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        add = false;
                    }
                    else
                    {
                        add = true;
                    }
                    rdr.Close();

                    cmd.Transaction = tr;
                    if (add == true)
                    {
                        cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                            + this.id.ToString() + ", 'Andon FormatoUsername', '" + value.ToString() + "')";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE configurazione SET valore = '" + value + "' WHERE "
                        + " Sezione = 'Reparto' AND ID = " + this.id.ToString() +
                        " AND parametro LIKE 'Andon FormatoUsername'";
                    }

                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }

                    rdr.Close();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Reparto' AND ID = "
                        + this.id.ToString() + " AND parametro = 'AvvioTasks'";

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        String strRet = rdr.GetString(0);
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
                    rdr.Close();
                    conn.Close();
                }
                return ret;
            }

            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                bool found = false;
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Reparto' AND ID = "
                    + this.id.ToString() + " AND parametro = 'AvvioTasks'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
                rdr.Close();

                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                if (found == true)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString()
                        + "' WHERE Sezione = 'Reparto' AND ID = " + this.id.ToString() + " AND parametro = 'AvvioTasks'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Reparto', "
                        + this.id.ToString() + ", 'AvvioTasks', '" + value + "')";
                }

                try
                {
                    cmd.ExecuteNonQuery();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione Like 'Reparto' AND "
                        + " ID = " + this.id.ToString()
                        + " AND parametro LIKE 'KanbanManaged'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        int param = rdr.GetInt32(0);
                        if (param == 1)
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
                    rdr.Close();
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
    }

    public class ElencoReparti
    {
        public List<Reparto> elenco;

        public ElencoReparti()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idReparto FROM reparti ORDER BY nome";
            MySqlDataReader rdr = cmd.ExecuteReader();
            elenco = new List<Reparto>();
            while (rdr.Read())
            {
                elenco.Add(new Reparto(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class Turno
    {
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE turniproduzione SET nome = '" + value + "' WHERE id = " + this.id + " AND reparto = " + this.idReparto;
                    MySqlTransaction trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction trn = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = trn;
                    String strCol = "#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
                    cmd.CommandText = "UPDATE turniproduzione SET colore = '" + strCol + "' WHERE id = " + this.id.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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

        public Turno()
        {
            this._id = -1;
            this._idReparto = -1;
        }

        public Turno(int turnoID)
        {
            this._straordinari = new List<Straordinario>();
            this._festivita = new List<Festivita>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT turniproduzione.id, turniproduzione.reparto, turniproduzione.nome, turniproduzione.colore "
            + " FROM turniproduzione WHERE id = " + turnoID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._id = rdr.GetInt32(0);
                this._idReparto = rdr.GetInt32(1);
                this._Nome = rdr.GetString(2);
                this._Colore = new System.Drawing.Color();
                this._Colore = System.Drawing.ColorTranslator.FromHtml(rdr.GetString(3));
            }
            rdr.Close();
            // Carico gli orari di lavoro
            cmd.CommandText = "SELECT id FROM orarilavoroturni WHERE idTurno = " + this.id.ToString()
                + " ORDER BY giornoInizio, oraInizio";
            rdr = cmd.ExecuteReader();
            this._Orari = new List<IntervalloLavorativoTurno>();
            while (rdr.Read())
            {
                this._Orari.Add(new IntervalloLavorativoTurno(rdr.GetInt32(0)));
            }
            rdr.Close();

            conn.Close();
        }

        public bool AddOrario(DayOfWeek dInizio, TimeSpan oInizio, DayOfWeek dFine, TimeSpan oFine)
        {
            bool rt;
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(id) FROM orarilavoroturni";
                MySqlDataReader rdr = cmd.ExecuteReader();
                int newID;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    newID = rdr.GetInt32(0) + 1;
                }
                else
                {
                    newID = 0;
                }
                rdr.Close();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                cmd.CommandText = "INSERT INTO orarilavoroturni(id, idTurno, giornoInizio, oraInizio, giornoFine, oraFine) VALUES"
                    + "(" + newID.ToString() + ", " + this.id.ToString() + ", " + ((Int32)dInizio).ToString() + ", '"
                    + oInizio.Hours.ToString() + ":" + oInizio.Minutes.ToString() + ":" + oInizio.Seconds.ToString() + "', "
                    + ((Int32)dFine).ToString() + ", '" + oFine.Hours.ToString() + ":" + oFine.Minutes.ToString() + ":"
                    + oFine.Seconds.ToString() + "')";
                try
                {
                    rt = true;
                    cmd.ExecuteNonQuery();
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
            this.CalendarioTrn = new CalendarioTurno(this.id, I, F);
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT straordinarifestivita.id FROM straordinarifestivita "
                + "WHERE azione = 'S' AND straordinarifestivita.turno = " + this.id.ToString()
                    + " AND datafine > '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY datainizio";

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    this._straordinari.Add(new Straordinario(rdr.GetInt32(0)));
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT straordinarifestivita.id FROM straordinarifestivita "
                    + "WHERE azione = 'F' AND straordinarifestivita.turno = " + this.id.ToString()
                    + " AND datafine > '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' ORDER BY datainizio";
                
                MySqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    festivita.Add(new Festivita(rdr.GetInt32(0)));
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
                this._Risorse = new RisorseTurno(this);
            }
        }
    }
    public class IntervalloLavorativoTurno
    {
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
                primo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, OraInizio.Hours, OraInizio.Minutes, OraInizio.Seconds);

                DateTime secondo;
                secondo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, OraFine.Hours, OraFine.Minutes, OraFine.Seconds);

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

        public IntervalloLavorativoTurno(int intervallo)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idTurno, giornoInizio, oraInizio, giornoFine, oraFine FROM orarilavoroturni WHERE "
                + " id = " + intervallo.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._idIntervallo = intervallo;
                this._idTurno = rdr.GetInt32(0);
                this._GiornoInizio = new DayOfWeek();
                this._GiornoInizio = (DayOfWeek)rdr.GetInt32(1);
                this._OraInizio = rdr.GetTimeSpan(2);
                this._GiornoFine = new DayOfWeek();
                this._GiornoFine = (DayOfWeek)rdr.GetInt32(3);
                this._OraFine = rdr.GetTimeSpan(4);
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                cmd.CommandText = "DELETE FROM orarilavoroturni WHERE id = " + this.idIntervallo.ToString() +
                    " AND idTurno = " + this.idTurno.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
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

        public RepartoProcessoPostazione(int rp, TaskVariante prc, Postazione ps)
        {
            this._repID = rp;
            this._proc = prc;
            this._pst = ps;
        }

        public RepartoProcessoPostazione(int rp, TaskVariante prc)
        {
            if (prc.Task != null && prc.variant != null)
            {
                this._repID = rp;
                this._proc = prc;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT postazione FROM repartipostazioniattivita WHERE reparto = " + this.repID.ToString()
                    + " AND processo = " + prc.Task.processID.ToString() + " AND revProc = " + prc.Task.revisione.ToString()
                    + " AND variante = " + prc.variant.idVariante.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._pst = new Postazione(rdr.GetInt32(0));
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
        public String log;

        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        public List<Festivita> feste;

        public ElencoFestivita(int rep)
        {
            Reparto rp = new Reparto(rep);
            feste = new List<Festivita>();
            if (rp.id != -1)
            {
                this._idReparto = rp.id;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT straordinarifestivita.id FROM straordinarifestivita "
                    + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                    + "WHERE azione = 'F' AND turniproduzione.reparto = " + rp.id.ToString()
                    + " AND datafine > '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' ORDER BY datainizio";
                
                MySqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    feste.Add(new Festivita(rdr.GetInt32(0)));
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
            if (this.idReparto != -1 && idTurno > 0)
            {
                if (i < f)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    // Controllo che non ci sia sovrapposizione con altri intervalli

                    cmd.CommandText = "SELECT straordinarifestivita.id FROM straordinarifestivita INNER JOIN turniproduzione ON ("
                        + "turniproduzione.id = straordinarifestivita.turno)  "
                        +" WHERE turniproduzione.reparto = " + this.idReparto.ToString() + " AND "
                        + " turno = " + idTurno.ToString() + " AND "
                        + " ((datainizio < '" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' < dataFine) " +
                        " OR (datainizio < '" + f.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + f.ToString("yyyy-MM-dd HH:mm:ss") + "' < dataFine) " +
                        " OR ('" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' < datainizio AND datafine < '" + f.ToString("yyyy-MM-dd HH:mm:ss") + "'))";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    
                    bool check1 = true;

                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        check1 = false;
                    }
                    else
                    {
                        check1 = true;
                    }
                    rdr.Close();

                    // Controllo che non ci sia sovrapposizione con le pause dai turni di reparto
                    bool check2 = false;
                    CalendarioReparto crp = new CalendarioReparto(this.idReparto, i.AddMonths(-2), f.AddMonths(1));

                    for (int q = 0; q < crp.Turni.Count && check2 == false; q++)
                    {
                        if ((crp.Turni[q].Inizio <= i && i <= crp.Turni[q].Fine && crp.Turni[q].Inizio <= f && f <= crp.Turni[q].Fine))
                        {
                            check2 = true;
                        }
                    }

                    if (check1 == true && check2)
                    {
                        cmd.CommandText = "SELECT MAX(id) FROM straordinarifestivita";
                        rdr = cmd.ExecuteReader();
                        int maxId = 0;
                        if (rdr.Read() && !rdr.IsDBNull(0))
                        {
                            maxId = rdr.GetInt32(0) + 1;
                        }
                        else
                        {
                            maxId = 0;
                        }
                        rdr.Close();
                        MySqlTransaction trn = conn.BeginTransaction();
                        cmd.Transaction = trn;
                        cmd.CommandText = "INSERT INTO straordinarifestivita(id, azione, datainizio, datafine, turno) VALUES("
                            + maxId.ToString() + ", 'F', '" + i.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + f.ToString("yyyy-MM-dd HH:mm:ss")
                            + "', " + idTurno.ToString() + ")";
                        try
                        {
                            cmd.ExecuteNonQuery();
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
        public String log;

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
            get { return this._Inizio; }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get { return this._Fine; }
        }

        public Festivita(int idFest)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT dataInizio, dataFine, turno, turniproduzione.reparto FROM straordinarifestivita "
                + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                + "WHERE azione = 'F' AND straordinarifestivita.id = " + idFest.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._idFestivita = idFest;
                this._Inizio = new DateTime();
                this._Fine = new DateTime();
                this._Inizio = rdr.GetDateTime(0);
                this._Fine = rdr.GetDateTime(1);
                this._idTurno = rdr.GetInt32(2);
                this._idReparto = rdr.GetInt32(3);
            }
            else
            {
                this._idTurno = -1;
                this._idFestivita = -1;
                this._idReparto = -1;
                this._Inizio = DateTime.Now;
                this._Fine = DateTime.Now;
            }
            conn.Close();
        }

        public bool delete()
        {
            bool rt = false;
            if (this.idFestivita != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM straordinarifestivita WHERE id = " + this.idFestivita;
                MySqlTransaction trn = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
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
        public String log;
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
            get { return this._Inizio; }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get { return this._Fine; }
        }

        public Straordinario(int idStraord)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT dataInizio, dataFine, turno, turniproduzione.id FROM straordinarifestivita "
                + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                + "WHERE azione = 'S' AND straordinarifestivita.id = " + idStraord.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._idStraordinario = idStraord;
                this._idTurno = rdr.GetInt32(2);
                this._idReparto = rdr.GetInt32(3);
                this._Inizio = new DateTime();
                this._Fine = new DateTime();
                this._Inizio = rdr.GetDateTime(0);
                this._Fine = rdr.GetDateTime(1);
            }
            else
            {
                this._idTurno = -1;
                this._idStraordinario = -1;
                this._idReparto = -1;
                this._Inizio = DateTime.Now;
                this._Fine = DateTime.Now;
            }
            conn.Close();
        }

        public bool delete()
        {
            bool rt = false;
            if (this.idStraordinario != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM straordinarifestivita WHERE id = " + this.idStraordinario;
                MySqlTransaction trn = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
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
        public String log;

        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        public List<Straordinario> Straordinari;

        public ElencoStraordinari(int rep)
        {
            Reparto rp = new Reparto(rep);
            Straordinari = new List<Straordinario>();
            if (rp.id != -1)
            {
                this._idReparto = rp.id;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT straordinarifestivita.id FROM straordinarifestivita "
                + "INNER JOIN turniproduzione ON (turniproduzione.id = straordinarifestivita.turno) "
                + "WHERE azione = 'S' AND turniproduzione.reparto = " + rp.id.ToString()
                    + " AND datafine > '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY datainizio";
                
                MySqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    Straordinari.Add(new Straordinario(rdr.GetInt32(0)));
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();

                    // Controllo che non ci sia sovrapposizione con altri intervalli
                    cmd.CommandText = "SELECT id FROM straordinarifestivita WHERE turno = " + idTurno.ToString() + " AND "
                        + " ((datainizio < '" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' < dataFine) " +
                        " OR (datainizio < '" + f.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + f.ToString("yyyy-MM-dd HH:mm:ss") + "' < dataFine) " +
                        " OR ('" + i.ToString("yyyy-MM-dd HH:mm:ss") + "' <= datainizio AND datafine <= '" + f.ToString("yyyy-MM-dd HH:mm:ss") + "'))";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    bool check1 = true;

                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        check1 = false;
                    }
                    else
                    {
                        check1 = true;
                    }
                    rdr.Close();

                    // Controllo che non ci sia sovrapposizione con i turni di reparto
                    bool check2 = true;
                    CalendarioReparto crp = new CalendarioReparto(this.idReparto, i.AddMonths(-1), f.AddMonths(1));
                    
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


                        cmd.CommandText = "SELECT MAX(id) FROM straordinarifestivita";
                        rdr = cmd.ExecuteReader();
                        int maxId = 0;
                        if (rdr.Read() && !rdr.IsDBNull(0))
                        {
                            maxId = rdr.GetInt32(0) + 1;
                        }
                        else
                        {
                            maxId = 0;
                        }
                        rdr.Close();
                        MySqlTransaction trn = conn.BeginTransaction();
                        cmd.Transaction = trn;
                        cmd.CommandText = "INSERT INTO straordinarifestivita(id, azione, datainizio, datafine, turno) VALUES("
                            + maxId.ToString() + ", 'S', '" + i.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + f.ToString("yyyy-MM-dd HH:mm:ss")
                            + "', " + idTurno.ToString() + ")";
                        try
                        {
                            cmd.ExecuteNonQuery();
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

        private DateTime _Inizio;
        public DateTime Inizio
        {
            get { return this._Inizio; }
        }

        private DateTime _Fine;
        public DateTime Fine
        {
            get { return this._Fine; }
        }

        private int _idTurno;
        public int idTurno
        {
            get
            {
                return this._idTurno;
            }
        }

        public IntervalloCalendarioReparto(Char stat, int idIntervallo, DateTime I, DateTime F)
        {
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
                    IntervalloLavorativoTurno intTrn = new IntervalloLavorativoTurno(idIntervallo);
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
                    Straordinario str = new Straordinario(idIntervallo);
                    if (str.idStraordinario != -1)
                    {
                        controlli = true;
                        this._idTurno = str.idTurno;
                    }
                }
                else if (stat == 'F')
                {
                    Festivita fst = new Festivita(idIntervallo);
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
                this._Inizio = I;
                this._Fine = F;
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
                this._Inizio = DateTime.Now;
                this._Fine = DateTime.Now;
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

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(lateFinish) as dataFineProd, idArticolo, annoArticolo FROM tasksproduzione "
                + " INNER JOIN productionplan ON (idArticolo=productionplan.id AND annoarticolo=productionplan.anno) "
                + " WHERE productionplan.reparto = " + rp.id.ToString()
                + " GROUP BY idArticolo, annoArticolo "
                + " ORDER BY dataFineProd DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //Articolo art = new Articolo(rdr.GetInt32(1), rdr.GetInt32(2));
                DateTime fineTask = rdr.GetDateTime(0);
                //DateTime fineTask = art.DataFineUltimoTask;
                if (this.Inizio <= fineTask && fineTask <= this.Fine)
                {
                    Articolo art = new Articolo(rdr.GetInt32(1), rdr.GetInt32(2));
                    log += art.ID.ToString() + "/" + art.Year.ToString() + " ";
                    ElencoArticoliDaTerminareReparto.Add(art);
                    this._NumArticoliDaTerminare++;
                }
            }
            rdr.Close();
            conn.Close();
            
        }

        public void loadArticoliTerminati(Reparto rp)
        {
            this._ArticoliTerminati = new List<Articolo>();
            List<Articolo> elencoTerminati = new List<Articolo>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, anno FROM productionplan WHERE status = 'F' AND reparto = " + rp.id.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                elencoTerminati.Add(new Articolo(rdr.GetInt32(0), rdr.GetInt32(1)));
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
        public String log;
        public DateTime InizioCal;
        public DateTime FineCal;
        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        public List<IntervalloCalendarioReparto> Turni;

        public List<IntervalloCalendarioReparto> Intervalli;

        public CalendarioReparto(int rp, DateTime InizioCal, DateTime FineCal)
        {
            Intervalli = new List<IntervalloCalendarioReparto>();
            Turni = new List<IntervalloCalendarioReparto>();
            if (InizioCal <= FineCal)
            {
                
                // Comincio con il trovare tutti i turni di lavoro del reparto
                Reparto rep = new Reparto(rp);
                rep.loadTurni();
                this._idReparto = rep.id;
                List<IntervalloLavorativoTurno> sommaTurni = new List<IntervalloLavorativoTurno>();
                //Turni = new List<IntervalloLavorativoTurno>();
                for (int i = 0; i < rep.Turni.Count; i++)
                {
                    for (int j = 0; j < rep.Turni[i].OrariDiLavoro.Count; j++)
                    {
                        sommaTurni.Add(new IntervalloLavorativoTurno(rep.Turni[i].OrariDiLavoro[j].idIntervallo));
                    }
                }
                
                // Ri-ordino i turni di lavoro
                IntervalloLavorativoTurno swap;
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
                }
                // Scrivo i turni sul log
                /*for (int i = 0; i < sommaTurni.Count; i++)
                {
                    log += sommaTurni[i].GiornoInizio.ToString() + " " + sommaTurni[i].OraInizio.Hours + ":" + sommaTurni[i].OraInizio.Minutes
                        + ":" + sommaTurni[i].OraInizio.Seconds + " - " + sommaTurni[i].GiornoFine.ToString() + " "
                        + sommaTurni[i].OraFine.Hours + ":" + sommaTurni[i].OraFine.Minutes + ":" + sommaTurni[i].OraFine.Seconds + "<br/>";
                }*/
                
                // Ora collego i turni al calendario per capire da quale partire
                List<DateTime> sommaTurniData = new List<DateTime>();
                List<TimeSpan> DistI = new List<TimeSpan>();
                TimeSpan minDist = new TimeSpan(1000, 23, 59, 59, 0);
                int minIndex = -1;
                for (int i = 0; i < sommaTurni.Count; i++)
                {
                    DateTime nxt = this.findNextOccurrence(sommaTurni[i].GiornoInizio, sommaTurni[i].OraInizio, InizioCal);
                    //log += nxt.ToString("dd/MM/yyyy HH:mm:ss") + " - ";
                    sommaTurniData.Add(nxt);
                    DistI.Add(sommaTurniData[i] - InizioCal);
                    if(DistI[i] < minDist)
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
                        DateTime dtI = this.findNextOccurrence(sommaTurni[cont].GiornoInizio, sommaTurni[cont].OraInizio, attuale);
                        DateTime dtF = this.findNextOccurrence(sommaTurni[cont].GiornoFine, sommaTurni[cont].OraFine, dtI);
                        IntervalloCalendarioReparto intCalRep = new IntervalloCalendarioReparto('L', sommaTurni[cont].idIntervallo, dtI, dtF);
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
                ElencoStraordinari straord = new ElencoStraordinari(this.idReparto);
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
                        Intervalli.Add(new IntervalloCalendarioReparto('S', straord.Straordinari[k].idStraordinario, st, en));
                        k++;
                }

                // Ora carico le festività
                k = 0;
                ElencoFestivita elFest = new ElencoFestivita(this.idReparto);
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
                            Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Terzo caso: Iniziofest > Inizio e Finefest = FineInterv
                        else if (elFest.feste[k].Inizio > this.Intervalli[indInterv].Inizio && elFest.feste[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 3<br/>";
                            DateTime i = this.Intervalli[indInterv].Inizio;
                            DateTime f = elFest.feste[k].Inizio;
                            Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i, f));
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
                            Intervalli.Add(new IntervalloCalendarioReparto('L', idIntervallo, i1, f1));
                            Intervalli.Add(new IntervalloCalendarioReparto('L', idIntervallo, i2, f2));
                        }
                    }
                    k++;
                }
                // Riordino l'array Intervalli
                sortIntervalli();
            }
            else
            {
                InizioCal = DateTime.Now;
                FineCal = DateTime.Now;
                this._idReparto = rp;
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
    }

    public class UtentiReparto
    {
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

        public UtentiReparto(int repID)
        {
            Reparto rp = new Reparto(repID);
            if (rp.id != -1)
            {
                this._idReparto = rp.id;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT operatore FROM operatorireparto WHERE reparto = " + repID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                this._Elenco = new List<User>();
                while (rdr.Read())
                {
                    this._Elenco.Add(new User(rdr.GetString(0)));
                }
                rdr.Close();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO operatorireparto(operatore, reparto) VALUES('" + usr.username + "'," + this.idReparto.ToString() + ")";
                try
                {
                    cmd.ExecuteNonQuery();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM operatorireparto WHERE operatore = '" + usr.username + "' AND reparto = " + this.idReparto.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
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

        public CalendarioTurno(int turno, DateTime InizioCal, DateTime FineCal)
        {
            Intervalli = new List<IntervalloCalendarioReparto>();
            Turni = new List<IntervalloCalendarioReparto>();
            if (InizioCal <= FineCal)
            {

                // Comincio con il trovare tutti i turni di lavoro del reparto
                Turno trn = new Turno(turno);
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
                        IntervalloCalendarioReparto intCalRep = new IntervalloCalendarioReparto('L', trn.OrariDiLavoro[cont].idIntervallo, dtI, dtF);
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
                    Intervalli.Add(new IntervalloCalendarioReparto('S', trn.straordinari[k].idStraordinario, st, en));
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
                            Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i, f));
                            this.Intervalli.RemoveAt(indInterv);
                        }
                        // Terzo caso: Iniziofest > Inizio e Finefest = FineInterv
                        else if (trn.festivita[k].Inizio > this.Intervalli[indInterv].Inizio && trn.festivita[k].Fine == this.Intervalli[indInterv].Fine)
                        {
                            log += "Entro nell'if 3<br/>";
                            DateTime i = this.Intervalli[indInterv].Inizio;
                            DateTime f = trn.festivita[k].Inizio;
                            Intervalli.Add(new IntervalloCalendarioReparto('L', this.Intervalli[indInterv].idOrarioTurno, i, f));
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
                            Intervalli.Add(new IntervalloCalendarioReparto('L', idIntervallo, i1, f1));
                            Intervalli.Add(new IntervalloCalendarioReparto('L', idIntervallo, i2, f2));
                        }
                    }
                    k++;
                }
                // Riordino l'array Intervalli
                sortIntervalli();
            }
            else
            {
                InizioCal = DateTime.Now;
                FineCal = DateTime.Now;
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
        private List<RisorsePostazioneTurno> _Postazioni;
        public List<RisorsePostazioneTurno> Postazioni
        { get { return this._Postazioni; } }

        public RisorseTurno(Turno trn)
        {
            this._Postazioni = new List<RisorsePostazioneTurno>();

            if (trn.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idPostazione FROM risorseturnopostazione WHERE idTurno = " + trn.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Postazioni.Add(new RisorsePostazioneTurno(new Postazione(rdr.GetInt32(0)), trn));
                }
                rdr.Close();
                conn.Close();
            }
        }
    }
}