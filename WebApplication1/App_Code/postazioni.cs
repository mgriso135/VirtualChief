using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using WebApplication1;
using Dati;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace KIS
{
    public class Postazione
    {
        public String log;
        public String err;
        private int _id;
        public int id
        {
            get { return this._id; }
        }

        private String _name;
        public String name
        {
            get { return this._name; }
            set
            {
                if (value.Length > 0 && this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE postazioni SET name='" + value + "' WHERE idpostazioni = " + this.id.ToString();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    this._name = value;
                }
            }
        }

        private String _desc;
        public String desc
        {
            get { return this._desc; }
            set
            {
                if (value.Length > 0 && this.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE postazioni SET description='" + value + "' WHERE idpostazioni = " + this.id.ToString();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    this._desc = value;
                }
            }
        }

        private List<processo> _tasks;
        public List<processo> tasks
        {
            get { return this._tasks; }
        }

        private CalendarioPostazione _calendario;
        public CalendarioPostazione Calendario
        {
            get { return this._calendario; }
        }

        public Postazione()
        {
            this._id = -1;
            this._name = "";
            this._desc = "";
            this._ElencoIDReparti = new List<int>();
        }

        public Postazione(int postID)
        {
            this._ElencoIDReparti = new List<int>();
            if (postID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT idpostazioni, name, description FROM postazioni WHERE idPostazioni = " + postID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._id = rdr.GetInt32(0);
                    this._name = rdr.GetString(1);
                    this._desc = rdr.GetString(2);
                }
                else
                {
                    this._id = -1;
                    this._name = "";
                    this._desc = "";
                }
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._id = -1;
                this._name = "";
                this._desc = "";
            }
        }

        public bool loadTasks()
        {
            bool rt = false;
            if (this.id != -1)
            {
                this._tasks = new List<processo>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT processo, revProc FROM repartipostazioniattivita WHERE postazione = " + this.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._tasks.Add(new processo(rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                conn.Close();
            }
            return rt;
        }

        // Rende disponibile una postazione ai figli del processo
        public bool add(String nome, String desc)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(idpostazioni) FROM postazioni";
            MySqlDataReader rdr = cmd.ExecuteReader();
            int newID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                newID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();
            cmd.CommandText = "INSERT INTO postazioni(idpostazioni, name, description) VALUES "
                + "(" + newID.ToString() + ", '" + nome + "', '" + desc + "')";
            try
            {
                cmd.ExecuteNonQuery();
                rt = true;
            }
            catch
            {
                rt = false;
            }
            conn.Close();
            return rt;
        }

        public bool delete()
        {
            bool rt = false;
            if (this.id != -1)
            {
                this.loadTasks();
                if (this.tasks.Count == 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = trans;
                    cmd.CommandText = "DELETE FROM postazioni WHERE idPostazioni = " + this.id.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        trans.Commit();
                        rt = true;
                    }
                    catch(Exception ex)
                    {
                        log = ex.Message;
                        rt = false;
                        trans.Rollback();
                    }
                    conn.Close();
                }
                else
                {
                    rt = false;
                }
            }
            return rt;
        }

        public TimeSpan calculateWorkLoad(ProcessoVariante prc)
        {
            TimeSpan carico = new TimeSpan(0);
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                for (int i = 0; i < prc.process.subProcessi.Count; i++)
                {
                    /*cmd.CommandText = "SELECT * FROM repartipostazioniattivita INNER JOIN processo ON "
                    + "(repartipostazioniattivita.processo = processo.processID AND repartipostazioniattivita.revProc = processo.revisione) "
                    + "WHERE postazione = " + this.id.ToString()
                        + " AND processo = " + prc.process.subProcessi[i].processID.ToString() + " AND revProc = "
                        + prc.process.subProcessi[i].revisione.ToString() + " AND variante = " + prc.variant.idVariante
                        + " AND processo.attivo = 1";*/

                    cmd.CommandText = "SELECT tempo FROM tempiciclo INNER JOIN repartipostazioniattivita ON (tempiciclo.processo = repartipostazioniattivita.processo"
                    + " AND tempiciclo.revisione = repartipostazioniattivita.revProc AND tempiciclo.variante = repartipostazioniattivita.variante)"
                    + " WHERE postazione = " + this.id.ToString() + " AND repartipostazioniattivita.processo = " + prc.process.subProcessi[i].processID.ToString()
                    + " AND repartipostazioniattivita.revProc = " + prc.process.subProcessi[i].revisione.ToString()
                    + " AND repartipostazioniattivita.variante = " + prc.variant.idVariante.ToString()
                    + " AND tempiciclo.def = true";

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        carico += rdr.GetTimeSpan(0);
                    }
                    rdr.Close();
                }
                conn.Close();
            }
            return carico;
        }

        private List<ProcessoVariante> _MainProc;
        public List<ProcessoVariante> MainProc
        {
            get { return this._MainProc; }
        }

        public bool LoadMainProc()
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processipadrifigli.padre, processipadrifigli.revPadre, repartipostazioniattivita.variante FROM "
                + "repartipostazioniattivita INNER JOIN processipadrifigli ON (repartipostazioniattivita.processo = processipadrifigli.task AND "
                + "repartipostazioniattivita.revProc = processipadrifigli.revTask) WHERE repartipostazioniattivita.postazione = " + this.id.ToString();
                //+ " AND processo.attivo = 1";
            MySqlDataReader rdr = cmd.ExecuteReader();
            _MainProc = new List<ProcessoVariante>();
            log = cmd.CommandText;
            try
            { 
            while (rdr.Read())
            {
                    log += rdr.GetInt32(0) + " " + rdr.GetInt32(1) + " " + rdr.GetInt32(2) + "<br />";
                ProcessoVariante daAggiungere = new ProcessoVariante(new processo(rdr.GetInt32(0), rdr.GetInt32(1)), new variante(rdr.GetInt32(2)));
                if (daAggiungere.process != null && daAggiungere.variant != null)
                {
                    // inoltre controllo di non averlo già aggiunto.
                    bool found = false;
                    for (int j = 0; j < _MainProc.Count; j++)
                    {
                        if (this.MainProc[j].process.processID == daAggiungere.process.processID && this.MainProc[j].process.revisione == daAggiungere.process.revisione && this.MainProc[j].variant.idVariante == daAggiungere.variant.idVariante)
                        {
                            found = true;
                        }
                    }

                    if (found == false)
                    {
                        _MainProc.Add(daAggiungere);
                    }
                }
            }
            }
            catch(Exception ex)
            {
                log += "<br />" + ex.Message;
            }
            rdr.Close();
            conn.Close();
            return rt;
        }

        private List<int> _ElencoIDReparti;
        public List<int> ElencoIDReparti
        {
            get { return this._ElencoIDReparti; }
        }

        public bool loadReparti()
        {
            bool rt = false;
            this._ElencoIDReparti = new List<int>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(reparto) FROM repartipostazioniattivita WHERE postazione = " + this.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._ElencoIDReparti.Add(rdr.GetInt32(0));
                }
                rdr.Close();
                conn.Close();
                rt = true;
            }
            return rt;
        }

        public void loadCalendario(DateTime Inizio, DateTime Fine)
        {
            if (this.id != -1)
            {
                this._calendario = new CalendarioPostazione(this.id, Inizio, Fine);
            }
        }

        private List<String> _UtentiLoggati;
        public List<String> UtentiLoggati
        {
            get { return this._UtentiLoggati; }
        }

        public void loadUtentiLoggati()
        {
            this._UtentiLoggati = new List<String>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT username FROM registrooperatoripostazioni WHERE logout IS NULL AND postazione = " + this.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._UtentiLoggati.Add(rdr.GetString(0));
                }
                rdr.Close();
                conn.Close();
            }
        }

        private List<int> _IdTaskProduzioneAvviabili;
        public List<int> IdTaskProduzioneAvviabili
        {
            get { return this._IdTaskProduzioneAvviabili; }
        }

        public void loadTaskProduzioneAvviabili()
        {
            this._IdTaskProduzioneAvviabili = new List<int>();
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE (status = 'N' OR status = 'I' OR status = 'P') "
                 + "AND postazione = " + this.id.ToString() + " ORDER BY lateStart, earlyStart, idArticolo";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    // Verifico che tutti i precedenti siano terminati
                    TaskProduzione tsk = new TaskProduzione(rdr.GetInt32(0));
                    
                    if (tsk.TaskProduzioneID != -1)
                    {
                        
                        tsk.loadPrecedenti();
                        bool controllo = true;
                        for (int i = 0; i < tsk.IdPrecedenti.Count; i++)
                        {
                            TaskProduzione prec = new TaskProduzione(tsk.IdPrecedenti[i]);
                            if (prec.Status != 'F')
                            {
                                controllo = false;
                            }
                            else
                            {
                            }
                        }
                        if (controllo == true)
                        {
                            this._IdTaskProduzioneAvviabili.Add(rdr.GetInt32(0));
                        }
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        private List<int> _TaskAvviatiUtente;
        public List<int> TaskAvviatiUtente
        {
            get { return this._TaskAvviatiUtente; }
        }

        public void loadTaskAvviati(User usr)
        {
            this._TaskAvviatiUtente = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT tasksproduzione.taskID, evento FROM tasksproduzione INNER JOIN registroeventitaskproduzione ON("
                + "tasksproduzione.taskID = registroeventitaskproduzione.task) WHERE tasksproduzione.status = 'I' AND "
                + "tasksproduzione.postazione = " + this.id.ToString()
                + " AND registroeventitaskproduzione.user = '" + usr.username + "' ORDER BY registroeventitaskproduzione.data DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<int> DaNonInserire = new List<int>();
            while (rdr.Read())
            {
                log += "Task: " + rdr.GetInt32(0).ToString() + " " + rdr.GetChar(1);
                if (rdr.GetChar(1) == 'P' || rdr.GetChar(1) == 'F')
                {
                    log += " da non inserire<br/>";
                    DaNonInserire.Add(rdr.GetInt32(0));
                }
                else if (rdr.GetChar(1) == 'I')
                {
                    log += " da verificare --> ";
                    // Verifico che non sia nella lista di quelli da non inserire, e nemmeno in quella dei già inseriti!
                    bool checkN = false;
                    bool checkI = false;
                    for (int q = 0; q < DaNonInserire.Count; q++)
                    {
                        if (DaNonInserire[q] == rdr.GetInt32(0))
                        {
                            log += " da non inserire";
                            checkN = true;
                        }
                    }
                    for (int q = 0; q < this._TaskAvviatiUtente.Count; q++)
                    {
                        if (this._TaskAvviatiUtente[q] == rdr.GetInt32(0))
                        {
                            log += " già inserito";
                            checkI = true;
                        }
                    }
                    if (checkN == false && checkI == false)
                    {
                        log += "aggiunto.<br/>";
                        this._TaskAvviatiUtente.Add(rdr.GetInt32(0));
                    }
                    else
                    {
                        log += "<br/>";
                    }
                }
            }
            rdr.Close();
            conn.Close();
        }

        private List<Warning> _WarningAperti;
        public List<Warning> WarningAperti
        {
            get { return this._WarningAperti; }
        }

        public void loadWarningAperti()
        {
            this._WarningAperti = new List<Warning>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT warningproduzione.id FROM warningproduzione INNER JOIN tasksproduzione ON "
                + "(warningproduzione.task = tasksproduzione.taskID) WHERE warningproduzione.dataRisoluzione IS NULL "
                + " AND tasksproduzione.postazione = " + this.id.ToString() + " ORDER BY warningproduzione.dataChiamata";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._WarningAperti.Add(new Warning(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public TimeSpan getCaricoDiLavoroProgrammato(int reparto, DateTime inizio, DateTime fine)
        {
            TimeSpan ret = new TimeSpan(0, 0, 0);
            if (this.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE postazione = " + this.id.ToString()
                    + " AND reparto = " + reparto
                    + " AND ("
                    + "(lateStart > '" + inizio.AddDays(-1).ToString("yyyy/MM/dd 00:00:00") + "' AND lateFinish <= '" + fine.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "')"
                    + " OR (lateStart <= '" + inizio.AddDays(-1).ToString("yyyy/MM/dd 00:00:00") + "' AND (lateFinish >= '" + inizio.AddDays(-1).ToString("yyyy/MM/dd 00:00:00") + "' AND lateFinish <= '" + fine.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "'))"
                    + " OR ((lateStart >= '" + inizio.AddDays(-1).ToString("yyyy/MM/dd 00:00:00") + "' AND lateStart <= '" + fine.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "') AND lateFinish <= '" + fine.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "')"
                    + " OR (lateStart <= '" + inizio.AddDays(-1).ToString("yyyy/MM/dd 00:00:00") + "' AND lateFinish >= '" + fine.AddDays(1).ToString("yyyy/MM/dd 00:00:00") + "')"
                    + ") ORDER BY lateFinish ASC";
                MySqlDataReader rdr = cmd.ExecuteReader();
                log = cmd.CommandText + "<br />";
                Reparto rp = new Reparto(reparto);
                rp.loadCalendario(inizio.AddDays(-1), fine.AddDays(1));
                while (rdr.Read())
                {
                    TaskProduzione tsk = new TaskProduzione(rdr.GetInt32(0));
                    log += "Task " + tsk.TaskProduzioneID.ToString() + ", postazione " + tsk.PostazioneID.ToString() + " " + tsk.LateStart.ToString("dd/MM/yyyy HH:mm:ss") + " - " + tsk.LateFinish.ToString("dd/MM/yyyy HH:mm:ss") + " - " + tsk.TempoC.TotalHours.ToString() + "<br />";

                    if (tsk.LateStart >= inizio && tsk.LateFinish <= fine)
                    {
                        ret += tsk.TempoC;
                    }
                    else if(tsk.LateStart <= inizio && tsk.LateFinish >= inizio && tsk.LateFinish <= fine)
                    {
                        // Provvisorio
                        //ret += tsk.TempoC;

                        for (int h = 0; h < rp.CalendarioRep.Intervalli.Count; h++)
                        {
                            if (rp.CalendarioRep.Intervalli[h].Fine <= inizio)
                            {
                                // Non aggiunto niente
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio > tsk.LateFinish)
                            {
                                // Non aggiungo niente
                            }
                            else if (inizio >= rp.CalendarioRep.Intervalli[h].Inizio && rp.CalendarioRep.Intervalli[h].Fine >= inizio && rp.CalendarioRep.Intervalli[h].Fine <= tsk.LateFinish)
                            {
                                ret += rp.CalendarioRep.Intervalli[h].Fine - inizio;
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Fine <= tsk.LateFinish)
                            {
                                ret += rp.CalendarioRep.Intervalli[h].Fine - rp.CalendarioRep.Intervalli[h].Inizio;
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Inizio <= tsk.LateFinish && rp.CalendarioRep.Intervalli[h].Fine > tsk.LateFinish)
                            {
                                ret += tsk.LateFinish - rp.CalendarioRep.Intervalli[h].Inizio;
                            }
                        }


                    }
                    else if (tsk.LateStart >= inizio && tsk.LateStart <= fine && tsk.LateFinish >= fine)
                    {
                        // Provvisorio
                        //ret += tsk.TempoC;
                        for (int h = 0; h < rp.CalendarioRep.Intervalli.Count; h++)
                        {
                            if (rp.CalendarioRep.Intervalli[h].Fine < tsk.LateStart)
                            {
                                // Non aggiungo niente
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio >= fine)
                            {
                                // Non aggiungo niente
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio <= tsk.LateStart && tsk.LateStart <= rp.CalendarioRep.Intervalli[h].Fine && rp.CalendarioRep.Intervalli[h].Fine <= fine)
                            {
                                ret += rp.CalendarioRep.Intervalli[h].Fine - tsk.LateStart;
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio >= tsk.LateStart && rp.CalendarioRep.Intervalli[h].Fine <= fine)
                            {
                                ret += rp.CalendarioRep.Intervalli[h].Fine - rp.CalendarioRep.Intervalli[h].Inizio;
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio >= tsk.LateStart && fine <= rp.CalendarioRep.Intervalli[h].Fine && rp.CalendarioRep.Intervalli[h].Inizio <= fine)
                            {
                                ret += fine - rp.CalendarioRep.Intervalli[h].Inizio;
                            }
                            else if (rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Fine >= fine && tsk.LateStart >= rp.CalendarioRep.Intervalli[h].Inizio && tsk.LateFinish >= fine)
                            {
                                ret += fine - tsk.LateStart;
                            }
                        }

                    }
                    else if (tsk.LateStart <= inizio && tsk.LateFinish >= fine)
                    {
                        // Provvisorio
                        ret += tsk.TempoC;
                    }
                }
                conn.Close();
            }
            return ret;
        }

        private RisorsePostazione _Risorse;
        public RisorsePostazione Risorse
        {
            get
            {
                return this._Risorse;
            }
        }

        public void loadRisorse()
        {
            if (this.id != -1)
            {
                this._Risorse = new RisorsePostazione(this);
            }
        }
    }

    public class ElencoPostazioni
    {
        public List<Postazione> elenco;

        public ElencoPostazioni()
        {
            elenco = new List<Postazione>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idpostazioni FROM postazioni ORDER BY name";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                elenco.Add(new Postazione(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public ElencoPostazioni(Reparto rp)
        {
            elenco = new List<Postazione>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT(postazione) FROM repartipostazioniattivita WHERE reparto = " + rp.id.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                elenco.Add(new Postazione(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

    }

    public class IntervalloLavoroPostazione
    {
        private int _idPostazione;
        public int idPostazione
        {
            get { return this._idPostazione; }
        }
        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        private DateTime _Inizio;
        public DateTime Inizio
        {
            get {
                Reparto rp = new Reparto(idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, rp.tzFusoOrario); }
        }

        private DateTime _Fine;
        public DateTime Fine
        {
            get {
                Reparto rp = new Reparto(idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, rp.tzFusoOrario); }
        }

        private char _Status;
        public char Status
        {
            get { return this._Status; }
        }

        private int _idTurno;
        public int idTurno
        {
            get { return this._idTurno; }
        }

        public IntervalloLavoroPostazione(int idPost, int repID, IntervalloCalendarioReparto intCalRep)
        {
            Reparto rp = new Reparto(idReparto);
            this._idPostazione = idPost;
            this._Inizio = TimeZoneInfo.ConvertTimeToUtc(intCalRep.Inizio, rp.tzFusoOrario);
            this._Fine = TimeZoneInfo.ConvertTimeToUtc(intCalRep.Fine, rp.tzFusoOrario);
            this._idReparto = repID;
            this._Status = intCalRep.Status;
            this._idTurno = intCalRep.idTurno;
        }
    }

    public class IntervalloTaskPostazione
    {
        private int _idPostazione;
        public int idPostazione
        {
            get { return this._idPostazione; }
        }
        private int _idReparto;
        public int idReparto
        {
            get { return this._idReparto; }
        }

        private int _TaskProduzioneID;
        public int TaskProduzioneID
        {
            get { return this._TaskProduzioneID; }
        }

        private DateTime _Inizio;
        public DateTime Inizio
        {
            get { Reparto rp = new Reparto(idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc( this._Inizio, rp.tzFusoOrario); }
        }

        private DateTime _Fine;
        public DateTime Fine
        {
            get {
                Reparto rp = new Reparto(idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, rp.tzFusoOrario);
            }
        }

        private char _Status;
        public char Status
        {
            get { return this._Status; }
        }

        public IntervalloTaskPostazione(int tskProd, DateTime start, DateTime end)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID, postazione, reparto, defStart, defFinish, status FROM tasksproduzione WHERE taskID = " + tskProd.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._TaskProduzioneID = rdr.GetInt32(0);
                this._idPostazione = rdr.GetInt32(1);
                this._idReparto = rdr.GetInt32(2);
                this._Inizio = start;
                this._Fine = end;
                this._Status = rdr.GetChar(5);
            }
            else
            {
                this._TaskProduzioneID = -1;
                this._idPostazione = -1;
                this._idReparto = -1;
                this._Inizio = DateTime.UtcNow;
                this._Fine = DateTime.UtcNow;
                this._Status = '\0';
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class IntervalloPostazione
    {
        private char _Status;
        public char Status
        {
            get { return this._Status; }
            set { this._Status = value; }
        }
        private DateTime _Inizio;
        public DateTime Inizio
        {
            get {
                KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, fuso.tzFusoOrario); }
            set {
                KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario();
                this._Inizio = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario); }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc( this._Fine, fuso.tzFusoOrario); }
            set { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario();
                this._Fine = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario); }
        }
        public IntervalloPostazione(DateTime start, DateTime end, char stat)
        {
            this.Inizio = start;
            this.Fine = end;
            this.Status = stat;
        }
    }

    public class CalendarioPostazione
    {
        public String log;

        private int _pstID;
        public int idPostazione
        {
            get { return this._pstID; }
        }

        public List<IntervalloLavoroPostazione> Intervalli;

        public List<IntervalloTaskPostazione> IntervalliTaskProduzione;

        public List<IntervalloPostazione> IntervalliLiberi;

        private DateTime _Inizio;
        public DateTime Inizio
        {
            get { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, fuso.tzFusoOrario); }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, fuso.tzFusoOrario); }
        }

        public CalendarioPostazione(int idPost, DateTime InizioCal, DateTime FineCal)
        {
            Intervalli = new List<IntervalloLavoroPostazione>();
            InizioCal = new DateTime(InizioCal.Ticks, DateTimeKind.Utc);
            FineCal = new DateTime(FineCal.Ticks, DateTimeKind.Utc);
            if (InizioCal < FineCal)
            {
                this._pstID = idPost;
                // Trovo tutti i reparti che utilizzano la tal postazione
                KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario();
                //this._Inizio = TimeZoneInfo.ConvertTimeToUtc(InizioCal, fuso.tzFusoOrario);
                //this._Fine = TimeZoneInfo.ConvertTimeToUtc(FineCal, fuso.tzFusoOrario);
                this._Inizio = InizioCal;
                this._Fine = FineCal;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DISTINCT(reparto) FROM repartipostazioniattivita WHERE postazione = " + idPost.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                List<Reparto> rpList = new List<Reparto>();
                while (rdr.Read())
                {
                    rpList.Add(new Reparto(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
                for (int i = 0; i < rpList.Count; i++)
                {
                    rpList[i].loadCalendario(InizioCal, FineCal);
                    for (int j = 0; j < rpList[i].CalendarioRep.Intervalli.Count; j++)
                    {
                        Intervalli.Add(new IntervalloLavoroPostazione(this.idPostazione, rpList[i].id, rpList[i].CalendarioRep.Intervalli[j]));
                    }
                }
                Intervalli.Sort(delegate(IntervalloLavoroPostazione p1, IntervalloLavoroPostazione p2)
                {
                    return p1.Inizio.CompareTo(p2.Inizio);
                });
            }
            else
            {
                this._pstID = -1;
            }
        }

        public void loadTasksProduzione()
        {
            IntervalliTaskProduzione = new List<IntervalloTaskPostazione>();
            if (this.idPostazione != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                
                cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE postazione = " + this.idPostazione.ToString()
                    + " AND status <> 'F' AND earlyStart > '" + this.Inizio.ToString("yyyy/MM/dd 00:00:00") + "' AND lateFinish <= '"+
                    this.Fine.ToString("yyyy/MM/dd 00:00:00") + "' ORDER BY lateFinish ASC";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    TaskProduzione tsk = new TaskProduzione(rdr.GetInt32(0));
                    // Ricerco l'intervallo di lavoro corretto
                    int intervalloIniziale = -1;
                    for (int i = 0; i < this.Intervalli.Count && intervalloIniziale == -1; i++)
                    {
                        /*log += "Intervallo: " + this.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - "
                        + this.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " " + this.Intervalli[i].idReparto.ToString();
                         * */
                        if (this.Intervalli[i].Inizio <= tsk.StartEffettivo && tsk.StartEffettivo <= this.Intervalli[i].Fine && this.Intervalli[i].idReparto == tsk.RepartoID)
                        {
                            intervalloIniziale = i;
                            //log += " TROVATO!";
                        }
                        //log += "<br/>";
                    }
                    

                    // Se ho trovato l'intervallo, inizio a creare il primo intervalloTask
                    if (intervalloIniziale != -1)
                    {
                        //log += "Entro nell'if!<br/>";
                        DateTime minore;
                        TimeSpan residuo;
                        if (this.Intervalli[intervalloIniziale].Fine >= tsk.FinishEffettivo)
                        {
                            minore = tsk.FinishEffettivo;
                            residuo = new TimeSpan(0, 0, 0);
                        }
                        else
                        {
                            minore = this.Intervalli[intervalloIniziale].Fine;
                            residuo = tsk.FinishEffettivo - this.Intervalli[intervalloIniziale].Fine;
                        }
                        //log += "Minore: " + minore.ToString() + " Residuo: " + residuo.ToString();

                        this.IntervalliTaskProduzione.Add(new IntervalloTaskPostazione(tsk.TaskProduzioneID, tsk.StartEffettivo, minore));
                       // log += "Prima stringa: " + this.IntervalliTaskProduzione[0].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - "
                        //    + this.IntervalliTaskProduzione[0].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                        int nextInterv = intervalloIniziale;
                        while (residuo.TotalSeconds > 0)
                        {
                            bool found = false;
                            //log += "Ricerco il prossimo intervallo utile<br/>";
                            // Ricerco il prossimo intervallo utile!
                            for (int i = nextInterv + 1; i < this.Intervalli.Count && found == false; i++)
                            {
                                //log += "nextInterv: " + nextInterv.ToString() + "<br/>";
                                log += tsk.RepartoID.ToString() + " " + this.Intervalli[i].idReparto.ToString() + " "
                                    + this.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - "
                                    + this.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                                if (this.Intervalli[i].idReparto == tsk.RepartoID)
                                {
                                    found = true;
                                  //  log += " FOUND";
                                    nextInterv = i;
                                }
                                //log += "<br/>";
                            }

                            // Trovo il minore tra fine task e fine intervallo e calcolo il residuo
                            if (this.Intervalli[nextInterv].Fine >= tsk.FinishEffettivo)
                            {
                                minore = tsk.FinishEffettivo;
                                residuo = new TimeSpan(0, 0, 0);
                            }
                            else
                            {
                                minore = this.Intervalli[nextInterv].Fine;
                                residuo = tsk.FinishEffettivo - this.Intervalli[nextInterv].Fine;
                            }
                            //log += "Minore: " + minore.ToString() + " Residuo: " + residuo.ToString() + "<br/>";
                            // Aggiungo l'intervallo alla lista
                            this.IntervalliTaskProduzione.Add(new IntervalloTaskPostazione(tsk.TaskProduzioneID, this.Intervalli[nextInterv].Inizio, minore));

                        }
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

    }

    public class RisorsePostazioneTurno
    {
        public String log;
        private Postazione _postazione;
        public Postazione postazione { get { return this._postazione; } }

        private Turno _turno;
        public Turno turno { get { return this._turno; } }

        private int _NumRisorse;
        public int NumRisorse
        {
            get { return this._NumRisorse; }
            set
            {
                if (this.postazione!=null && this.turno!=null && this.postazione.id != -1 && this.turno.id != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT risorse FROM risorseturnopostazione WHERE "
                        + " idTurno = " + this.turno.id.ToString()
                        + " AND idPostazione = " + this.postazione.id.ToString();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    Boolean check = false;
                    
                    check = (rdr.Read() && !rdr.IsDBNull(0)) ? true : false;
                    rdr.Close();
                    if (check)
                    {
                        cmd.CommandText = "UPDATE risorseturnopostazione SET risorse = " + value.ToString()
                            + " WHERE idTurno = " + this.turno.id.ToString()
                            + " AND idPostazione = " + this.postazione.id.ToString();
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO risorseturnopostazione(idTurno, idPostazione, risorse) "
                            + " VALUES("+this.turno.id.ToString() + ", "
                            + this.postazione.id.ToString() + ", "
                            + value.ToString()
                            +")";
                    }
                    log = cmd.CommandText;
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._NumRisorse = value;
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }

                    conn.Close();
                }
            }
        }

        public RisorsePostazioneTurno(Postazione pst, Turno trn)
        {
            this._postazione = null;
            this._turno = null;
            this._NumRisorse = 0;
            if (pst.id != -1 && trn.id != -1)
            {
                this._postazione = new Postazione(pst.id);
                this._turno = new Turno(trn.id);

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT risorse FROM risorseturnopostazione WHERE "
                    + "idTurno = " + trn.id.ToString()
                    + " AND idPostazione = " + pst.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._NumRisorse = rdr.GetInt32(0);
                }
                rdr.Close();
                conn.Close();
            }
        }


        // Cancello la configurazione delle risorse!
        public Boolean delete()
        {
            Boolean ret = false;
            if (this.turno != null && this.postazione != null)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM risorseturnopostazione WHERE "
                        + "idTurno = " + this.turno.id.ToString()
                        + " AND idPostazione = " + this.postazione.id.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._turno = null;
                    this._postazione = null;
                    this._NumRisorse = -1;
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
    }

    public class RisorsePostazione
    {
        private List<RisorsePostazioneTurno> _Turni;
        public List<RisorsePostazioneTurno> Turni
        { get { return this._Turni; } }

        public RisorsePostazione(Postazione pst)
        {
            this._Turni = new List<RisorsePostazioneTurno>();

            if (pst.id != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idTurno FROM risorseturnopostazione WHERE idPostazione = " + pst.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this._Turni.Add(new RisorsePostazioneTurno(pst, new Turno(rdr.GetInt32(0))));
                }
                rdr.Close();
                conn.Close();
            }
        }
    }
}