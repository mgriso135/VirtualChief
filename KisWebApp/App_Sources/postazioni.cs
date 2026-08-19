/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
//using WebApplication1;
using MySql.Data.MySqlClient;
using Dapper;

namespace KIS.App_Code
{
    public class Postazione
    {
        protected String Tenant;

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
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Execute("UPDATE postazioni SET name=@p0 WHERE idpostazioni = @p1",
                            new { @p0 = value, @p1 = this.id });
                        this._name = value;
                    }
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
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Execute("UPDATE postazioni SET description=@p0 WHERE idpostazioni = @p1",
                            new { @p0 = value, @p1 = this.id });
                        this._desc = value;
                    }
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


        /* When this flag is TRUE it is not necessary to explicitly CHECK-IN or
         * CHECK-OUT from workstation when using BARCODE SYSTEM
         * as it is done automatically.
         * ONLY works with BARCODE GEMBA, NOT with WEB GEMBA
        */ 
        private Boolean _barcodeAutoCheckIn;
        public Boolean barcodeAutoCheckIn
        {
            get
            {
                return this._barcodeAutoCheckIn;
            }
            set
            {
                if(this.id!=-1)
                { 
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            conn.Execute("UPDATE postazioni SET barcodeAutoCheckIn = @p0 WHERE idpostazioni = @p1",
                                new { @p0 = value, @p1 = this.id }, tr);
                            tr.Commit();
                            this._barcodeAutoCheckIn = value;
                        }
                    }
                }
            }

        }

        public Postazione(String Tenant)
        {
            this.Tenant = Tenant;

            this._id = -1;
            this._name = "";
            this._desc = "";
            this._ElencoIDReparti = new List<int>();
            this._barcodeAutoCheckIn = false;
        }

        public Postazione(String Tenant, int postID)
        {
            this.Tenant = Tenant;
            this._ElencoIDReparti = new List<int>();
            if (postID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var row = conn.QueryFirstOrDefault<PostazioneRow>(
                        "SELECT idpostazioni, name, description, barcodeAutoCheckIn FROM postazioni WHERE idPostazioni = @p0",
                        new { @p0 = postID });
                    if (row != null)
                    {
                        this._id = row.idpostazioni;
                        this._name = row.name;
                        this._desc = row.description;
                        this._barcodeAutoCheckIn = row.barcodeAutoCheckIn;
                    }
                    else
                    {
                        this._id = -1;
                        this._name = "";
                        this._desc = "";
                        this._barcodeAutoCheckIn = false;
                    }
                }
            }
            else
            {
                this._id = -1;
                this._name = "";
                this._desc = "";
                this._barcodeAutoCheckIn = false;
            }
        }

        private class PostazioneRow
        {
            public int idpostazioni { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public bool barcodeAutoCheckIn { get; set; }
        }

        public bool loadTasks()
        {
            bool rt = false;
            if (this.id != -1)
            {
                this._tasks = new List<processo>();
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var rows = conn.Query<(int processo, int revProc)>(
                        "SELECT processo, revProc FROM repartipostazioniattivita WHERE postazione = @p0",
                        new { @p0 = this.id });
                    foreach (var r in rows)
                    {
                        this._tasks.Add(new processo(this.Tenant, r.processo, r.revProc));
                    }
                }
            }
            return rt;
        }

        // Rende disponibile una postazione ai figli del processo
        public bool add(String nome, String desc, Boolean barcodeAutoCheckIn)
        {
            bool rt = false;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                int? maxId = conn.QueryFirstOrDefault<int?>("SELECT MAX(idpostazioni) FROM postazioni");
                int newID = (maxId ?? 19) + 1;
                try
                {
                    conn.Execute("INSERT INTO postazioni(idpostazioni, name, description, barcodeAutoCheckIn) VALUES "
                        + "(@p0, @p1, @p2, @p3)",
                        new { @p0 = newID, @p1 = nome, @p2 = desc, @p3 = barcodeAutoCheckIn });
                    rt = true;
                }
                catch
                {
                    rt = false;
                }
            }
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
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("DELETE FROM postazioni WHERE idPostazioni = @p0",
                                    new { @p0 = this.id }, trans);
                                trans.Commit();
                                rt = true;
                            }
                            catch(Exception ex)
                            {
                                log = ex.Message;
                                rt = false;
                                trans.Rollback();
                            }
                        }
                    }
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
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    for (int i = 0; i < prc.process.subProcessi.Count; i++)
                    {
                        TimeSpan? tempo = conn.QueryFirstOrDefault<TimeSpan?>(
                            "SELECT tempo FROM tempiciclo INNER JOIN repartipostazioniattivita ON (tempiciclo.processo = repartipostazioniattivita.processo"
                            + " AND tempiciclo.revisione = repartipostazioniattivita.revProc AND tempiciclo.variante = repartipostazioniattivita.variante)"
                            + " WHERE postazione = @p0 AND repartipostazioniattivita.processo = @p1"
                            + " AND repartipostazioniattivita.revProc = @p2"
                            + " AND repartipostazioniattivita.variante = @p3"
                            + " AND tempiciclo.def = true",
                            new { @p0 = this.id, @p1 = prc.process.subProcessi[i].processID, @p2 = prc.process.subProcessi[i].revisione, @p3 = prc.variant.idVariante });
                        if (tempo.HasValue)
                        {
                            carico += tempo.Value;
                        }
                    }
                }
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
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                string sql = "SELECT processipadrifigli.padre, processipadrifigli.revPadre, repartipostazioniattivita.variante FROM "
                    + "repartipostazioniattivita INNER JOIN processipadrifigli ON (repartipostazioniattivita.processo = processipadrifigli.task AND "
                    + "repartipostazioniattivita.revProc = processipadrifigli.revTask) WHERE repartipostazioniattivita.postazione = @p0";
                var rows = conn.Query<(int padre, int revPadre, int variante)>(sql, new { @p0 = this.id });
                _MainProc = new List<ProcessoVariante>();
                log = sql;
                try
                { 
                    foreach (var r in rows)
                    {
                        log += r.padre + " " + r.revPadre + " " + r.variante + "<br />";
                        ProcessoVariante daAggiungere = new ProcessoVariante(this.Tenant, new processo(this.Tenant, r.padre, r.revPadre), new variante(this.Tenant, r.variante));
                        daAggiungere.loadReparto();
                        daAggiungere.process.loadFigli(daAggiungere.variant);
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
            }
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
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var ids = conn.Query<int>("SELECT DISTINCT(reparto) FROM repartipostazioniattivita WHERE postazione = @p0",
                        new { @p0 = this.id });
                    foreach (int idReparto in ids)
                    {
                        this._ElencoIDReparti.Add(idReparto);
                    }
                }
                rt = true;
            }
            return rt;
        }

        public void loadCalendario(DateTime Inizio, DateTime Fine)
        {
            if (this.id != -1)
            {
                this._calendario = new CalendarioPostazione(this.Tenant, this.id, Inizio, Fine);
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
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var usernames = conn.Query<string>("SELECT username FROM registrooperatoripostazioni WHERE logout IS NULL AND postazione = @p0",
                        new { @p0 = this.id });
                    foreach (string username in usernames)
                    {
                        this._UtentiLoggati.Add(username);
                    }
                }
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
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var ids = conn.Query<int>("SELECT taskID FROM tasksproduzione WHERE (status = 'N' OR status = 'I' OR status = 'P') "
                        + "AND postazione = @p0 ORDER BY lateStart, earlyStart, idArticolo",
                        new { @p0 = this.id });
                    foreach (int taskId in ids)
                    {

                        // Verifico che tutti i precedenti siano terminati (se ConstraintType=1) oppure se siano avviati (se ConstraintType=0)
                        TaskProduzione tsk = new TaskProduzione(this.Tenant, taskId);
                        
                        if (tsk.TaskProduzioneID != -1)
                        {
                            
                            tsk.loadPrecedenti();
                            bool controllo = true;
                            /*for (int i = 0; i < tsk.IdPrecedenti.Count; i++)
                            {
                                TaskProduzione prec = new TaskProduzione(tsk.IdPrecedenti[i]);
                                if (prec.Status != 'F')
                                {
                                    controllo = false;
                                }
                                else
                                {
                                }
                            }*/
                            for(int i = 0; i < tsk.PreviousTasks.Count; i++)
                            {
                                TaskProduzione prec = new TaskProduzione(this.Tenant, tsk.PreviousTasks[i].NearTaskID);
                                if(tsk.PreviousTasks[i].ConstraintType == 0)
                                {
                                    if (prec.Status == 'N')
                                    {
                                        controllo = false;
                                    }
                                }
                                else
                                { 
                                    if (prec.Status != 'F')
                                    {
                                        controllo = false;
                                    }
                                }
                            }
                            if (controllo == true)
                            {
                                this._IdTaskProduzioneAvviabili.Add(taskId);
                            }
                        }
                    }
                }
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
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                var rows = conn.Query<(int taskID, char evento)>(
                    "SELECT tasksproduzione.taskID, evento FROM tasksproduzione INNER JOIN registroeventitaskproduzione ON("
                    + "tasksproduzione.taskID = registroeventitaskproduzione.task) WHERE tasksproduzione.status = 'I' AND "
                    + "tasksproduzione.postazione = @p0"
                    + " AND registroeventitaskproduzione.user = @p1 ORDER BY registroeventitaskproduzione.data DESC",
                    new { @p0 = this.id, @p1 = usr.username });
                List<int> DaNonInserire = new List<int>();
                foreach (var r in rows)
                {
                    log += "Task: " + r.taskID.ToString() + " " + r.evento;
                    if (r.evento == 'P' || r.evento == 'F')
                    {
                        log += " da non inserire<br/>";
                        DaNonInserire.Add(r.taskID);
                    }
                    else if (r.evento == 'I')
                    {
                        log += " da verificare --> ";
                        // Verifico che non sia nella lista di quelli da non inserire, e nemmeno in quella dei già inseriti!
                        bool checkN = false;
                        bool checkI = false;
                        for (int q = 0; q < DaNonInserire.Count; q++)
                        {
                            if (DaNonInserire[q] == r.taskID)
                            {
                                log += " da non inserire";
                                checkN = true;
                            }
                        }
                        for (int q = 0; q < this._TaskAvviatiUtente.Count; q++)
                        {
                            if (this._TaskAvviatiUtente[q] == r.taskID)
                            {
                                log += " già inserito";
                                checkI = true;
                            }
                        }
                        if (checkN == false && checkI == false)
                        {
                            log += "aggiunto.<br/>";
                            this._TaskAvviatiUtente.Add(r.taskID);
                        }
                        else
                        {
                            log += "<br/>";
                        }
                    }
                }
            }
        }

        private List<Warning> _WarningAperti;
        public List<Warning> WarningAperti
        {
            get { return this._WarningAperti; }
        }

        public void loadWarningAperti()
        {
            this._WarningAperti = new List<Warning>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                var ids = conn.Query<int>("SELECT warningproduzione.id FROM warningproduzione INNER JOIN tasksproduzione ON "
                    + "(warningproduzione.task = tasksproduzione.taskID) WHERE warningproduzione.dataRisoluzione IS NULL "
                    + " AND tasksproduzione.postazione = @p0 ORDER BY warningproduzione.dataChiamata",
                    new { @p0 = this.id });
                foreach (int id in ids)
                {
                    this._WarningAperti.Add(new Warning(this.Tenant, id));
                }
            }
        }

        public TimeSpan getCaricoDiLavoroProgrammato(int reparto, DateTime inizio, DateTime fine)
        {
            TimeSpan ret = new TimeSpan(0, 0, 0);
            if (this.id != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    string sql = "SELECT taskID FROM tasksproduzione WHERE postazione = @p0"
                        + " AND reparto = @p1"
                        + " AND ("
                        + "(lateStart > @p2 AND lateFinish <= @p3)"
                        + " OR (lateStart <= @p2 AND (lateFinish >= @p2 AND lateFinish <= @p3))"
                        + " OR ((lateStart >= @p2 AND lateStart <= @p3) AND lateFinish <= @p3)"
                        + " OR (lateStart <= @p2 AND lateFinish >= @p3)"
                        + ") ORDER BY lateFinish ASC";
                    var ids = conn.Query<int>(sql, new { @p0 = this.id, @p1 = reparto, @p2 = inizio.AddDays(-1).ToString("yyyy/MM/dd 00:00:00"), @p3 = fine.AddDays(1).ToString("yyyy/MM/dd 00:00:00") });
                    log = sql + "<br />";
                    Reparto rp = new Reparto(this.Tenant, reparto);
                    rp.loadCalendario(inizio.AddDays(-1), fine.AddDays(1));
                    foreach (int taskId in ids)
                    {
                        TaskProduzione tsk = new TaskProduzione(this.Tenant, taskId);
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
                }
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
                this._Risorse = new RisorsePostazione(this.Tenant, this);
            }
        }
    }

    public class ElencoPostazioni
    {
        protected String Tenant;

        public List<Postazione> elenco;

        public ElencoPostazioni(String Tenant)
        {
            this.Tenant = Tenant;

            elenco = new List<Postazione>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                var ids = conn.Query<int>("SELECT idpostazioni FROM postazioni ORDER BY name");
                foreach (int idPostazione in ids)
                {
                    elenco.Add(new Postazione(this.Tenant, idPostazione));
                }
            }
        }

        public ElencoPostazioni(String Tenant, Reparto rp)
        {
            this.Tenant = Tenant;

            elenco = new List<Postazione>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                var ids = conn.Query<int>("SELECT DISTINCT(postazione) FROM repartipostazioniattivita WHERE reparto = @p0",
                    new { @p0 = rp.id });
                foreach (int idPostazione in ids)
                {
                    elenco.Add(new Postazione(this.Tenant, idPostazione));
                }
            }
        }

    }

    public class IntervalloLavoroPostazione
    {
        protected String Tenant;

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
                Reparto rp = new Reparto(this.Tenant, idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, rp.tzFusoOrario); }
        }

        private DateTime _Fine;
        public DateTime Fine
        {
            get {
                Reparto rp = new Reparto(this.Tenant, idReparto);
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

        public IntervalloLavoroPostazione(String Tenant, int idPost, int repID, IntervalloCalendarioReparto intCalRep)
        {
            this.Tenant = Tenant;

            Reparto rp = new Reparto(this.Tenant, idReparto);
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
        protected String Tenant;

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
            get { Reparto rp = new Reparto(this.Tenant, idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc( this._Inizio, rp.tzFusoOrario); }
        }

        private DateTime _Fine;
        public DateTime Fine
        {
            get {
                Reparto rp = new Reparto(this.Tenant, idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, rp.tzFusoOrario);
            }
        }

        private char _Status;
        public char Status
        {
            get { return this._Status; }
        }

        public IntervalloTaskPostazione(String Tenant, int tskProd, DateTime start, DateTime end)
        {
            this.Tenant = Tenant;

            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                var row = conn.QueryFirstOrDefault<IntervalloTaskPostazioneRow>(
                    "SELECT taskID, postazione, reparto, defStart, defFinish, status FROM tasksproduzione WHERE taskID = @p0",
                    new { @p0 = tskProd });
                if (row != null)
                {
                    this._TaskProduzioneID = row.taskID;
                    this._idPostazione = row.postazione;
                    this._idReparto = row.reparto;
                    this._Inizio = start;
                    this._Fine = end;
                    this._Status = row.status;
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
            }
        }
    }

    internal class IntervalloTaskPostazioneRow
    {
        public int taskID { get; set; }
        public int postazione { get; set; }
        public int reparto { get; set; }
        public char status { get; set; }
    }

    public class IntervalloPostazione
    {
        protected String Tenant;

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
                KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, fuso.tzFusoOrario); }
            set {
                KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario(this.Tenant);
                this._Inizio = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario); }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc( this._Fine, fuso.tzFusoOrario); }
            set { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario(this.Tenant);
                this._Fine = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario); }
        }
        public IntervalloPostazione(String Tenant, DateTime start, DateTime end, char stat)
        {
            this.Tenant = Tenant;
            this.Inizio = start;
            this.Fine = end;
            this.Status = stat;
        }
    }

    public class CalendarioPostazione
    {
        protected String Tenant;

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
            get { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Inizio, fuso.tzFusoOrario); }
        }
        private DateTime _Fine;
        public DateTime Fine
        {
            get { KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._Fine, fuso.tzFusoOrario); }
        }

        public CalendarioPostazione(String Tenant, int idPost, DateTime InizioCal, DateTime FineCal)
        {
            this.Tenant = Tenant;

            Intervalli = new List<IntervalloLavoroPostazione>();
            InizioCal = new DateTime(InizioCal.Ticks, DateTimeKind.Utc);
            FineCal = new DateTime(FineCal.Ticks, DateTimeKind.Utc);
            if (InizioCal < FineCal)
            {
                this._pstID = idPost;
                // Trovo tutti i reparti che utilizzano la tal postazione
                KIS.App_Code.FusoOrario fuso = new App_Code.FusoOrario(this.Tenant);
                //this._Inizio = TimeZoneInfo.ConvertTimeToUtc(InizioCal, fuso.tzFusoOrario);
                //this._Fine = TimeZoneInfo.ConvertTimeToUtc(FineCal, fuso.tzFusoOrario);
                this._Inizio = InizioCal;
                this._Fine = FineCal;
                List<Reparto> rpList = new List<Reparto>();
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var ids = conn.Query<int>("SELECT DISTINCT(reparto) FROM repartipostazioniattivita WHERE postazione = @p0",
                        new { @p0 = idPost });
                    foreach (int idReparto in ids)
                    {
                        rpList.Add(new Reparto(this.Tenant, idReparto));
                    }
                }
                for (int i = 0; i < rpList.Count; i++)
                {
                    rpList[i].loadCalendario(InizioCal, FineCal);
                    for (int j = 0; j < rpList[i].CalendarioRep.Intervalli.Count; j++)
                    {
                        Intervalli.Add(new IntervalloLavoroPostazione(this.Tenant, this.idPostazione, rpList[i].id, rpList[i].CalendarioRep.Intervalli[j]));
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
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var ids = conn.Query<int>(
                        "SELECT taskID FROM tasksproduzione WHERE postazione = @p0"
                        + " AND status <> 'F' AND earlyStart > @p1 AND lateFinish <= @p2 ORDER BY lateFinish ASC",
                        new { @p0 = this.idPostazione, @p1 = this.Inizio.ToString("yyyy/MM/dd 00:00:00"), @p2 = this.Fine.ToString("yyyy/MM/dd 00:00:00") });
                    foreach (int taskId in ids)
                    {
                        TaskProduzione tsk = new TaskProduzione(this.Tenant, taskId);
                        // Ricerco l'intervallo di lavoro corretto
                        int intervalloIniziale = -1;
                        for (int i = 0; i < this.Intervalli.Count && intervalloIniziale == -1; i++)
                        {
                            if (this.Intervalli[i].Inizio <= tsk.StartEffettivo && tsk.StartEffettivo <= this.Intervalli[i].Fine && this.Intervalli[i].idReparto == tsk.RepartoID)
                            {
                                intervalloIniziale = i;
                            }
                        }
                        

                        // Se ho trovato l'intervallo, inizio a creare il primo intervalloTask
                        if (intervalloIniziale != -1)
                        {
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

                            this.IntervalliTaskProduzione.Add(new IntervalloTaskPostazione(this.Tenant, tsk.TaskProduzioneID, tsk.StartEffettivo, minore));
                            int nextInterv = intervalloIniziale;
                            while (residuo.TotalSeconds > 0)
                            {
                                bool found = false;
                                for (int i = nextInterv + 1; i < this.Intervalli.Count && found == false; i++)
                                {
                                    log += tsk.RepartoID.ToString() + " " + this.Intervalli[i].idReparto.ToString() + " "
                                        + this.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - "
                                        + this.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                                    if (this.Intervalli[i].idReparto == tsk.RepartoID)
                                    {
                                        found = true;
                                        nextInterv = i;
                                    }
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
                                // Aggiungo l'intervallo alla lista
                                this.IntervalliTaskProduzione.Add(new IntervalloTaskPostazione(this.Tenant, tsk.TaskProduzioneID, this.Intervalli[nextInterv].Inizio, minore));

                            }
                        }
                    }
                }
            }
        }

    }

    public class RisorsePostazioneTurno
    {
        protected String Tenant;

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
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        int? risorse = conn.QueryFirstOrDefault<int?>(
                            "SELECT risorse FROM risorseturnopostazione WHERE "
                            + " idTurno = @p0"
                            + " AND idPostazione = @p1",
                            new { @p0 = this.turno.id, @p1 = this.postazione.id });
                        Boolean check = risorse.HasValue;
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            string sql;
                            if (check)
                            {
                                sql = "UPDATE risorseturnopostazione SET risorse = @p2"
                                    + " WHERE idTurno = @p0"
                                    + " AND idPostazione = @p1";
                            }
                            else
                            {
                                sql = "INSERT INTO risorseturnopostazione(idTurno, idPostazione, risorse) "
                                    + " VALUES(@p0, @p1, @p2)";
                            }
                            log = sql;
                            try
                            {
                                conn.Execute(sql, new { @p0 = this.turno.id, @p1 = this.postazione.id, @p2 = value }, tr);
                                this._NumRisorse = value;
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
            }
        }

        public RisorsePostazioneTurno(String Tenant, Postazione pst, Turno trn)
        {
            this.Tenant = Tenant;

            this._postazione = null;
            this._turno = null;
            this._NumRisorse = 0;
            if (pst.id != -1 && trn.id != -1)
            {
                this._postazione = new Postazione(this.Tenant, pst.id);
                this._turno = new Turno(this.Tenant, trn.id);

                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    int? risorse = conn.QueryFirstOrDefault<int?>(
                        "SELECT risorse FROM risorseturnopostazione WHERE "
                        + "idTurno = @p0"
                        + " AND idPostazione = @p1",
                        new { @p0 = trn.id, @p1 = pst.id });
                    if (risorse.HasValue)
                    {
                        this._NumRisorse = risorse.Value;
                    }
                }
            }
        }


        // Cancello la configurazione delle risorse!
        public Boolean delete()
        {
            Boolean ret = false;
            if (this.turno != null && this.postazione != null)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("DELETE FROM risorseturnopostazione WHERE "
                                    + "idTurno = @p0"
                                    + " AND idPostazione = @p1",
                                new { @p0 = this.turno.id, @p1 = this.postazione.id }, tr);
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
                    }
                }
            }
            return ret;
        }
    }

    public class RisorsePostazione
    {
        protected String Tenant;

        private List<RisorsePostazioneTurno> _Turni;
        public List<RisorsePostazioneTurno> Turni
        { get { return this._Turni; } }

        public RisorsePostazione(String Tenant, Postazione pst)
        {
            this.Tenant = Tenant;

            this._Turni = new List<RisorsePostazioneTurno>();

            if (pst.id != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    var ids = conn.Query<int>("SELECT idTurno FROM risorseturnopostazione WHERE idPostazione = @p0",
                        new { @p0 = pst.id });
                    foreach (int idTurno in ids)
                    {
                        this._Turni.Add(new RisorsePostazioneTurno(this.Tenant, pst, new Turno(this.Tenant, idTurno)));
                    }
                }
            }
        }
    }
}