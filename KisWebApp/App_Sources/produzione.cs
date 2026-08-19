/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2018 Matteo Griso -  Tutti i diritti riservati */

/* Developed by MATTEO GRISO
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.App_Code
{
    
    public class prodotto
    {
        public String Tenant;

        public String err;
        private String _matricola;
        public String matricola
        {
            get { return this._matricola; }
        }

        public ProcessoVariante modello;

        public int _RepartoID;
        public int RepartoID
        {
            get { return this._RepartoID; }
        }

        private char _status;
        public char status
        {
            get { return _status; }
            set
            {
                if (this.matricola.Length > 0)
                {
                    string strSQL = "UPDATE productionPlan SET status = @status WHERE matricola = @matricola";
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        conn.Execute(strSQL, new { status = value.ToString(), matricola = this.matricola });
                        this._status = value;
                    }
                }
            }
        }
 
        public prodotto(String Tenant)
        {
            this.Tenant = Tenant;
            this._matricola = "";
            this._RepartoID = -1;
            this.modello = null;
        }

        public prodotto(String Tenant, String prodID, ProcessoVariante mdl)
        {
            this.Tenant = Tenant;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT matricola, processo, revisione, variante, status, reparto, startTime FROM productionPlan "
                + "WHERE matricola LIKE @matricola AND processo = @processo"
                + " AND revisione = @revisione"
                + " AND variante = @variante";
            ProdottoRow row = conn.QueryFirstOrDefault<ProdottoRow>(sql, new { matricola = prodID, processo = mdl.process.processID, revisione = mdl.process.revisione, variante = mdl.variant.idVariante });
            if (row != null)
            {
                
                processo prc = new processo(this.Tenant, row.Processo, row.Revisione);
                variante vr = new variante(this.Tenant, row.Variante);
                if (prc.processID != -1 && vr.idVariante != -1)
                {
                    modello = new ProcessoVariante(this.Tenant, prc, vr);
                    modello.loadReparto();
                    modello.process.loadFigli(modello.variant);
                    if (modello.process != null && modello.variant != null)
                    {
                        this._matricola = row.Matricola;
                        this._status = row.Status[0];
                        this._RepartoID = row.Reparto;
                        this._dataIniziale = row.StartTime;
                        Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                        rp.loadTurni();
                    }
                    else
                    {
                        modello = null;
                        this._matricola = "";
                        this._RepartoID = -1;
                    }
                }
                else
                {
                    modello = null;
                    this._matricola = "";
                    this._RepartoID = -1;
                }
            }
            else
            {
                modello = null;
                this._matricola = "";
                this._RepartoID = -1;
            }
            }
        }

        private class ProdottoRow
        {
            public String Matricola { get; set; }
            public int Processo { get; set; }
            public int Revisione { get; set; }
            public int Variante { get; set; }
            public String Status { get; set; }
            public int Reparto { get; set; }
            public DateTime StartTime { get; set; }
        }

        private DateTime _dataIniziale;
        public DateTime dataIniziale
        {
            get
            {
                TimeZoneInfo tz = null;
                if (this.RepartoID != -1)
                { Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                    tz = rp.tzFusoOrario;
                }
                else
                {
                    FusoOrario fuso = new FusoOrario(this.Tenant);
                    tz = fuso.tzFusoOrario;
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._dataIniziale, tz);
            }
        }
    }

    public class TaskProduzione
    {
        public String Tenant;

        public String log;
        public String logP { get { return this.log; } }

        public Reparto rp;

        private int _TaskProduzioneID;
        public int TaskProduzioneID
        {
            get { return this._TaskProduzioneID; }
        }

        private String _Name;
        public String Name
        {
            get { return this._Name; }
        }

        private String _Description;
        public String Description
        {
            get { return this._Description; }
        }

        private DateTime _EarlyStart;
        public DateTime EarlyStart
        {
            get
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyStart, rp.tzFusoOrario);
            }
            set
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        string sql = "UPDATE tasksproduzione SET earlyStart = @earlyStart"
                            + " WHERE taskid = @taskid";
                        try
                        {
                            conn.Execute(sql, new { earlyStart = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss"), taskid = this.TaskProduzioneID }, tr);
                            tr.Commit();
                            this._EarlyStart = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            log = ex.Message;
                        }
                    }
                }
            }
        }

        private DateTime _LateStart;
        public DateTime LateStart
        {
            get
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateStart, rp.tzFusoOrario);
            }
            set
            {
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        string sql = "UPDATE tasksproduzione SET lateStart = @lateStart"
                            + " WHERE taskid = @taskid";
                        try
                        {
                            conn.Execute(sql, new { lateStart = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss"), taskid = this.TaskProduzioneID }, tr);
                            tr.Commit();
                            this._LateStart = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            log = ex.Message;
                        }
                    }
                }
            }
        }
        private DateTime _EarlyFinish;
        public DateTime EarlyFinish
        {
            get
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyFinish, rp.tzFusoOrario);
            }
            set
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        string sql = "UPDATE tasksproduzione SET earlyFinish = @earlyFinish"
                            + " WHERE taskid = @taskid";
                        try
                        {
                            conn.Execute(sql, new { earlyFinish = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss"), taskid = this.TaskProduzioneID }, tr);
                            tr.Commit();
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            log = ex.Message;
                        }
                    }
                }
            }
        }
        private DateTime _LateFinish;
        public DateTime LateFinish
        {
            get
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateFinish, rp.tzFusoOrario);
            }
            set
            {
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        string sql = "UPDATE tasksproduzione SET lateFinish = @lateFinish"
                            + " WHERE taskid = @taskid";
                        try
                        {
                            conn.Execute(sql, new { lateFinish = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss"), taskid = this.TaskProduzioneID }, tr);
                            tr.Commit();
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            log = ex.Message;
                        }
                    }
                }
            }
        }
        private DateTime _StartEffettivo;
        public DateTime StartEffettivo
        {
            get
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._StartEffettivo, rp.tzFusoOrario);
            }
        }
        private DateTime _FinishEffettivo;
        public DateTime FinishEffettivo
        {
            get
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._FinishEffettivo, rp.tzFusoOrario);
            }
        }

        private int _OriginalTask;
        public int OriginalTask
        {
            get { return this._OriginalTask; }
        }
        private int _OriginalTaskRevisione;
        public int OriginalTaskRevisione
        {
            get { return this._OriginalTaskRevisione; }
        }
        public TaskVariante OriginalTaskVariante
        {
            get
            {
                TaskVariante modello = new TaskVariante(this.Tenant, new processo(this.Tenant, this.OriginalTask, this.OriginalTaskRevisione), new variante(this.Tenant, this.VarianteID));
                return modello;
            }
        }

        private int _VarianteID;
        public int VarianteID
        {
            get { return this._VarianteID; }
        }

        private int _RepartoID;
        public int RepartoID
        {
            get { return this._RepartoID; }
        }

        private int _PostazioneID;
        public int PostazioneID
        {
            get { return this._PostazioneID; }
        }

        public String PostazioneName
        {
            get
            {
                Postazione p = new Postazione(this.Tenant, PostazioneID);
                return p.name;
            }
        }

        /*
         * N se non iniziato
         * I se task in corso
         * P se in pausa
         * F se terminato
         */
        private char _Status;
        public char Status
        {
            get { return this._Status; }
        }

        private int _ArticoloID;
        public int ArticoloID
        {
            get { return this._ArticoloID; }
        }
        private int _ArticoloAnno;
        public int ArticoloAnno
        {
            get { return this._ArticoloAnno; }
        }

        private int _NumOperatori;
        public int NumOperatori
        {
            get { return this._NumOperatori; }
        }

        /* Calculated as: Setup time + (Quantity*CycleTime) + UnloadTime
         */
        private TimeSpan _TempoCiclo;
        public TimeSpan TempoC
        {
            get { return this._TempoCiclo; }
        }

        private bool _IsCritical;
        public bool IsCritical
        {
            get { return this._IsCritical; }
        }

        private int _QuantitaPrevista;
        public int QuantitaPrevista
        {
            get { return this._QuantitaPrevista; }
        }

        private int _QuantitaProdotta;
        public int QuantitaProdotta
        {
            get { return this._QuantitaProdotta; }
            set
            {
                if (this.TaskProduzioneID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            string sql = "UPDATE tasksproduzione SET qtaProdotta = @qtaProdotta"
                                + " WHERE taskID = @taskid";
                            try
                            {
                                conn.Execute(sql, new { qtaProdotta = value, taskid = this.TaskProduzioneID }, tr);
                                tr.Commit();
                                this._QuantitaProdotta = value;
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

        public KIS.App_Sources.WorkInstructions.WorkInstruction WorkInstructionActive;

        public List<TaskOperatorNote> TaskOperatorNotes;

        /* This indicator is the productivity on the task
         * Productivity is calculated as the relation between planned working time and real working time 
         * if < 1 performance is poor
         * if > 1 performance is good
         */
        public Double Productivity
        {
            get
            {
                if(this.TaskProduzioneID!=-1 && this.Status=='F' && this.WorkingTime.Ticks > 0)
                {
                    return (this.TempoC.TotalSeconds / this.WorkingTime.TotalSeconds);
                }
                else
                {
                    return 0.0;
                }
            }
        }

        public TaskProduzione(String tenant, int tskProdID)
        {
            this.Tenant = tenant;
            this.WorkInstructionActive = null;
            this.TaskOperatorNotes = null;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT taskID, name, description, earlyStart, lateStart, earlyFinish, lateFinish, "
                    + " origTask, revOrigTask, variante, reparto, postazione, status, idArticolo, annoArticolo, "
                    + " nOperatori, tempoCiclo, qtaPrevista, qtaProdotta, endDateReal, LeadTime, WorkingTime, Delay FROM tasksproduzione WHERE taskID = @taskid";
                TaskProduzioneRow row = conn.QueryFirstOrDefault<TaskProduzioneRow>(sql, new { taskid = tskProdID });
            if (row != null)
            {
                this._TaskProduzioneID = row.TaskID;
                this._Name = row.Name;
                this._Description = row.Description;
                this._EarlyStart = row.EarlyStart;
                this._LateStart = row.LateStart;
                this._EarlyFinish = row.EarlyFinish;
                this._LateFinish = row.LateFinish;
                this._OriginalTask = row.OrigTask;
                this._OriginalTaskRevisione = row.RevOrigTask;
                this._VarianteID = row.Variante;
                this._RepartoID = row.Reparto;
                this._PostazioneID = row.Postazione;
                this._Status = row.Status[0];
                this._ArticoloID = row.IdArticolo;
                this._ArticoloAnno = row.AnnoArticolo;
                this._NumOperatori = row.NOperatori;
                this._TempoCiclo = row.TempoCiclo;
                this._QuantitaPrevista = row.QtaPrevista;
                this._QuantitaProdotta = row.QtaProdotta;
                this._RealEndDate = new DateTime(1970, 1, 1);
                if (row.EndDateReal.HasValue)
                {
                    this._RealEndDate = row.EndDateReal.Value;
                }
                this._LeadTime = new TimeSpan(1970, 1, 1);
                if (row.LeadTime.HasValue)
                {
                    this._LeadTime = row.LeadTime.Value;
                }
                this._WorkingTime = new TimeSpan(0, 0, 0);
                if (row.WorkingTime.HasValue)
                {
                    this._WorkingTime = row.WorkingTime.Value;
                }
                this._Delay = new TimeSpan(0, 0, 0);
                if (row.Delay.HasValue)
                {
                    this._Delay = row.Delay.Value;
                }
            }
            else
            {
                this._TaskProduzioneID = -1;
                this._Name = "";
                this._Description = "";
                this._EarlyStart = DateTime.UtcNow;
                this._LateStart = DateTime.UtcNow;
                this._EarlyFinish = DateTime.UtcNow;
                this._LateFinish = DateTime.UtcNow;
                this._OriginalTask = -1;
                this._OriginalTaskRevisione = -1;
                this._VarianteID = -1;
                this._RepartoID = -1;
                this._PostazioneID = -1;
                this._Status = '\0';
                this._ArticoloID = -1;
                this._ArticoloAnno = 1900;
                this._NumOperatori = 0;
                this._TempoCiclo = new TimeSpan(0, 0, 0);
                this._IsCritical = false;
                this._StartEffettivo = DateTime.UtcNow;
                this._FinishEffettivo = DateTime.UtcNow;
                this._QuantitaPrevista = -1;
                this._QuantitaProdotta = -1;
                this._WorkingTime = new TimeSpan(0, 0, 0);
                this._RealEndDate = new DateTime(1970, 1, 1);
                this._Delay = new TimeSpan(0, 0, 0);
            }
            }
        }

        private class TaskProduzioneRow
        {
            public int TaskID { get; set; }
            public String Name { get; set; }
            public String Description { get; set; }
            public DateTime EarlyStart { get; set; }
            public DateTime LateStart { get; set; }
            public DateTime EarlyFinish { get; set; }
            public DateTime LateFinish { get; set; }
            public int OrigTask { get; set; }
            public int RevOrigTask { get; set; }
            public int Variante { get; set; }
            public int Reparto { get; set; }
            public int Postazione { get; set; }
            public String Status { get; set; }
            public int IdArticolo { get; set; }
            public int AnnoArticolo { get; set; }
            public int NOperatori { get; set; }
            public TimeSpan TempoCiclo { get; set; }
            public int QtaPrevista { get; set; }
            public int QtaProdotta { get; set; }
            public DateTime? EndDateReal { get; set; }
            public TimeSpan? LeadTime { get; set; }
            public TimeSpan? WorkingTime { get; set; }
            public TimeSpan? Delay { get; set; }
        }

        // Precedenti
        private List<int> _IdPrecedenti;
        public List<int> IdPrecedenti
        {
            get { return this._IdPrecedenti; }
        }

        public List<NearTask> PreviousTasks;
        public List<NearTask> FollowingTasks;

        public void loadPrecedenti()
        {
            this._IdPrecedenti = new List<int>();
            this.PreviousTasks = new List<NearTask>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT prec, ConstraintType FROM prectasksproduzione WHERE succ = @succ";
                var rows = conn.Query<(int Prec, int ConstraintType)>(sql, new { succ = this.TaskProduzioneID });
                foreach (var row in rows)
                {
                    this._IdPrecedenti.Add(row.Prec);
                    NearTask curr = new NearTask(this.Tenant);
                    curr.NearTaskID = row.Prec;
                    curr.ConstraintType = row.ConstraintType;
                    this.PreviousTasks.Add(curr);
                }
            }
        }

        // Eventi del task
        private List<EventoTaskProduzione> _Eventi;
        public List<EventoTaskProduzione> Eventi
        {
            get { return this._Eventi; }
        }

        public void loadEventi()
        {
            this._Eventi = new List<EventoTaskProduzione>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id FROM registroeventitaskproduzione WHERE task = @task"
                    + " ORDER BY data";
                var rows = conn.Query<int>(sql, new { task = this.TaskProduzioneID });
                foreach (var id in rows)
                {
                    this._Eventi.Add(new EventoTaskProduzione(this.Tenant, id));
                }
            }
        }

        // Utenti attivi
        private List<int> _UtentiAttivi;
        public List<int> UtentiAttivi
        {
            get { return this._UtentiAttivi; }
        }

        public List<int> ActiveInputPoints
        {
            get { return this.UtentiAttivi; }
        }

        public void loadUtentiAttivi()
        {
            this._UtentiAttivi = new List<int>();
            List<int> utentiNonAttivi = new List<int>();
            this.loadEventi();

            for (int i = this.Eventi.Count - 1; i >= 0; i--)
            {

                if (this.Eventi[i].Evento == 'P')
                {
                    utentiNonAttivi.Add(this.Eventi[i].InputPoint);
                }
                else if (this.Eventi[i].Evento == 'I')
                {
                    bool found = false;
                    // Ricerco tra la lista degli inattivi
                    for (int j = 0; j < utentiNonAttivi.Count; j++)
                    {
                        if (this.Eventi[i].InputPoint == utentiNonAttivi[j])
                        {
                            found = true;
                        }
                    }
                    // Se non l'ho trovato, aggiungo a lista utenti attivi
                    if (found == false)
                    {
                        this._UtentiAttivi.Add(this.Eventi[i].InputPoint);
                    }
                }
            }
        }

        public String CustomerName
        {
            get
            {
                String ret = "";
                if (this.TaskProduzioneID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT anagraficaclienti.ragsociale FROM anagraficaclienti INNER JOIN commesse ON (anagraficaclienti.codice = commesse.cliente) "
                            + " INNER JOIN productionplan ON (productionplan.commessa = commesse.idcommesse AND productionplan.anno = commesse.anno) INNER JOIN "
                            + " tasksproduzione ON (tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
                            + " WHERE taskID = @taskID";
                        try
                        {
                            String r = conn.QueryFirstOrDefault<String>(sql, new { taskID = this.TaskProduzioneID });
                            if (r != null)
                            {
                                ret = r;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                        }
                    }
                }
                return ret;
            }
        }

        public String CustomerCode
        {
            get
            {
                String ret = "";
                if (this.TaskProduzioneID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT anagraficaclienti.codice FROM anagraficaclienti INNER JOIN commesse ON (anagraficaclienti.codice = commesse.cliente) "
                            + " INNER JOIN productionplan ON (productionplan.commessa = commesse.idcommesse AND productionplan.anno = commesse.anno) INNER JOIN "
                            + " tasksproduzione ON (tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
                            + " WHERE taskID = @taskID";
                        try
                        {
                            String r = conn.QueryFirstOrDefault<String>(sql, new { taskID = this.TaskProduzioneID });
                            if (r != null)
                            {
                                ret = r;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                        }
                    }
                }
                return ret;
            }
        }

        public String ExternalID
        {
            get
            {
                String ret = "";
                if (this.TaskProduzioneID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT commesse.ExternalID FROM commesse "
                            + " INNER JOIN productionplan ON (productionplan.commessa = commesse.idcommesse AND productionplan.anno = commesse.anno) INNER JOIN "
                            + " tasksproduzione ON (tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno) "
                            + " WHERE taskID = @taskID";
                        try
                        {
                            String r = conn.QueryFirstOrDefault<String>(sql, new { taskID = this.TaskProduzioneID });
                            if (r != null)
                            {
                                ret = r;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                        }
                    }
                }
                return ret;
            }
        }

        // Utenti che hanno lavorato o stanno lavorato su questo task
        private List<int> _InputPoints;
        public List<int> InputPoints
        {
            get { return this._InputPoints; }
        }

        public List<TaskParameter> Parameters;

        public List<TaskParameter> CompleteParameters;

        private List<String> _AssignedOperators;
        public List<String> AssignedOperators
        {
            get { return this._AssignedOperators; }
        }

        public void loadInputPoints()
        {
            this._InputPoints = new List<int>();
            List<int> utentiNonAttivi = new List<int>();
            this.loadEventi();

            for (int i = 0; i < this.Eventi.Count; i++)
            {
                utentiNonAttivi.Add(this.Eventi[i].InputPoint);
            }
            this._InputPoints = new List<int>(utentiNonAttivi.Distinct());
        }

        public bool Start(InputPoint inputpoint)
        {
            bool rt = false;
            if (this.TaskProduzioneID != -1 && this.Status != 'F' && inputpoint!=null && inputpoint.id>=0)
            {
                bool controlloUltimaAzione = false;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT evento FROM registroeventitaskproduzione WHERE task=@taskid "
                    + " AND inputpoint=@ipid ORDER BY data desc";
                String evento = conn.QueryFirstOrDefault<String>(sql, new { taskid = this.TaskProduzioneID.ToString(), ipid = inputpoint.id });
                if (evento != null)
                {
                    if (evento[0] != 'I')
                    {
                        controlloUltimaAzione = true;
                    }
                    else
                    {
                        controlloUltimaAzione = false;
                    }
                }
                else
                {
                    controlloUltimaAzione = true;
                }

                // Controllo che tutti i precedenti siano terminati
                bool checkPrecedenti = true;
                this.loadPrecedenti();
                for (int i = 0; i < this.PreviousTasks.Count; i++)
                {
                    TaskProduzione precedente = new TaskProduzione(this.Tenant, this.PreviousTasks[i].NearTaskID);
                    if (this.PreviousTasks[i].ConstraintType == 0)
                    {
                        if (precedente.Status == 'N')
                        {
                            checkPrecedenti = false;
                        }
                    }
                    else
                    {
                        if (precedente.Status != 'F')
                        {
                            checkPrecedenti = false;
                        }
                    }

                }

                bool controlloTasksAvviatiUtente = true;
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                if (rp.TasksAvviabiliContemporaneamenteDaOperatore == 0)
                {
                    controlloTasksAvviatiUtente = true;
                }
                else
                {
                    inputpoint.loadTaskAvviati();
                    if (inputpoint.RunningTasks.Count < rp.TasksAvviabiliContemporaneamenteDaOperatore)
                    {
                        controlloTasksAvviatiUtente = true;
                    }
                    else
                    {
                        controlloTasksAvviatiUtente = false;
                    }
                }

                log += controlloUltimaAzione.ToString() + " " + checkPrecedenti.ToString() + " " + controlloTasksAvviatiUtente.ToString();

                if (controlloUltimaAzione == true && checkPrecedenti == true && controlloTasksAvviatiUtente == true)
                {
                    using (var tr = conn.BeginTransaction())
                    {
                        int maxID = 0;
                        int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(id) FROM registroeventitaskproduzione", null, tr);
                        if (maxRow.HasValue)
                        {
                            maxID = maxRow.Value + 1;
                        }
                        try
                        {
                            conn.Execute("INSERT INTO registroeventitaskproduzione(id, inputpoint, task, data, evento, note) VALUES(@id, @ipid, @taskid, @date, @statusI, @note)",
                                new { id = maxID.ToString(), ipid = inputpoint.id, taskid = this.TaskProduzioneID.ToString(), date = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), statusI = 'I', note = "" }, tr);
                            conn.Execute("UPDATE tasksproduzione SET status =@statusI WHERE taskID = @taskid",
                                new { statusI = 'I', taskid = this.TaskProduzioneID.ToString() }, tr);
                            Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                            if (art.Status == 'P')
                            {
                                conn.Execute("UPDATE productionplan SET status=@statusI WHERE id=@prodid AND anno=@prodYear",
                                    new { statusI = 'I', prodid = art.ID, prodYear = art.Year }, tr);
                            }

                            tr.Commit();
                            rt = true;
                        }
                        catch (Exception ex)
                        {
                            rt = false;
                            log += ex.Message;
                            tr.Rollback();
                        }
                    }
                }
                }
            }
            return rt;
        }

        // Starts the task in a specific DateTime. It used for manual registration
        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if task not found or status == 'F'
         * 3 if error during the sql insert into commands
         * 4 if all previous tasks are not ended
         * 5 if user is already working on the max number of tasks
         * 6 if user is currently working on this task
         * 7 if user is not logged in the workstation
         */
        public int Start(User usr, DateTime regDate)
        {
            int rt = 0;
            if (this.TaskProduzioneID != -1 && this.Status != 'F')
            {
                // Controllo che l'utente sia in postazione...
                Postazione p = new Postazione(this.Tenant, this.PostazioneID);
                p.loadUtentiLoggati();
                bool controlloUtente = false;
                for (int i = 0; i < p.UtentiLoggati.Count; i++)
                {
                    if (p.UtentiLoggati[i] == usr.username)
                    {
                        controlloUtente = true;
                    }
                }

                if (!controlloUtente)
                {
                    rt = 7;
                }

                bool controlloUltimaAzione = false;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT evento FROM registroeventitaskproduzione WHERE task = @task"
                + " AND user = @user ORDER BY data desc";
                String evento = conn.QueryFirstOrDefault<String>(sql, new { task = this.TaskProduzioneID, user = usr.username });
                if (evento != null)
                {
                    if (evento[0] != 'I')
                    {
                        controlloUltimaAzione = true;
                    }
                    else
                    {
                        controlloUltimaAzione = false;
                    }
                }
                else
                {
                    controlloUltimaAzione = true;
                }

                if (!controlloUltimaAzione)
                {
                    rt = 6;
                }

                // Controllo che tutti i precedenti siano terminati
                bool checkPrecedenti = true;
                this.loadPrecedenti();
                for (int i = 0; i < this.PreviousTasks.Count; i++)
                {
                    TaskProduzione precedente = new TaskProduzione(this.Tenant, this.PreviousTasks[i].NearTaskID);
                    if (this.PreviousTasks[i].ConstraintType == 0)
                    {
                        if (precedente.Status == 'N')
                        {
                            checkPrecedenti = false;
                        }
                    }
                    else
                    {
                        if (precedente.Status != 'F')
                        {
                            checkPrecedenti = false;
                        }
                    }

                }

                if (!checkPrecedenti)
                {
                    rt = 4;
                }

                bool controlloTasksAvviatiUtente = true;
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                if (rp.TasksAvviabiliContemporaneamenteDaOperatore == 0)
                {
                    controlloTasksAvviatiUtente = true;
                }
                else
                {
                    usr.loadTaskAvviati();
                    if (usr.TaskAvviati.Count < rp.TasksAvviabiliContemporaneamenteDaOperatore)
                    {
                        controlloTasksAvviatiUtente = true;
                    }
                    else
                    {
                        controlloTasksAvviatiUtente = false;
                    }
                }

                if (!controlloTasksAvviatiUtente)
                {
                    rt = 5;
                }

                log += controlloUtente.ToString() + " " + controlloUltimaAzione.ToString() + " " + checkPrecedenti.ToString() + " " + controlloTasksAvviatiUtente.ToString();

                if (controlloUtente == true && controlloUltimaAzione == true && checkPrecedenti == true && controlloTasksAvviatiUtente == true)
                {
                    using (var tr = conn.BeginTransaction())
                    {
                        int maxID = 0;
                        int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(id) FROM registroeventitaskproduzione", null, tr);
                        if (maxRow.HasValue)
                        {
                            maxID = maxRow.Value + 1;
                        }
                        try
                        {
                            conn.Execute("INSERT INTO registroeventitaskproduzione(id, user, task, data, evento, note) VALUES(@id, @user, @task, @date, 'I', '')",
                                new { id = maxID, user = usr.username, task = this.TaskProduzioneID, date = TimeZoneInfo.ConvertTimeToUtc(regDate, rp.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") }, tr);
                            conn.Execute("UPDATE tasksproduzione SET status = 'I' WHERE taskID = @task",
                                new { task = this.TaskProduzioneID }, tr);
                            Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                            if (art.Status == 'P')
                            {
                                conn.Execute("UPDATE productionplan SET status='I' WHERE id = @artID" +
                                    " AND anno = @artYear",
                                    new { artID = art.ID, artYear = art.Year }, tr);
                            }

                            tr.Commit();
                            rt = 1;
                        }
                        catch (Exception ex)
                        {
                            rt = 3;
                            this.log += ex.Message;
                            tr.Rollback();
                        }
                    }
                }
                }
            }
            else
            {
                rt = 2;
            }
            return rt;
        }

        public bool Pause(InputPoint usr)
        {
            bool rt = false;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    // Verifico che l'ultima azione per questo utente sia di Inizio del task
                    bool check = false;
                    String evento = conn.QueryFirstOrDefault<String>("SELECT evento FROM registroeventitaskproduzione WHERE task=@taskid"
                        + " AND inputpoint=@ipid ORDER BY data desc", new { taskid = this.TaskProduzioneID, ipid = usr.id }, tr);

                    if (evento != null)
                    {
                        if (evento[0] == 'I')
                        {
                            check = true;
                        }
                    }
                    if (check == true)
                    {
                        int maxID = 0;
                        int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(id) FROM registroeventitaskproduzione", null, tr);
                        if (maxRow.HasValue)
                        {
                            maxID = maxRow.Value + 1;
                        }
                        try
                        {
                            conn.Execute("INSERT INTO registroeventitaskproduzione(id, inputpoint, task, data, evento, note) VALUES(@maxid, @ipid, @taskid, @date, @event, @notes)",
                                new { maxid = maxID, ipid = usr.id, taskid = this.TaskProduzioneID, date = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), @event = 'P', notes = "" }, tr);
                            this.loadUtentiAttivi();
                            if (this.UtentiAttivi.Count == 0 || (this.UtentiAttivi.Count == 1 && this.UtentiAttivi[0] == usr.id))
                            {
                                conn.Execute("UPDATE tasksproduzione SET status = 'P' WHERE taskID = @taskid",
                                    new { taskid = this.TaskProduzioneID }, tr);
                            }
                            tr.Commit();
                            rt = true;
                        }
                        catch (Exception ex)
                        {
                            rt = false;
                            log += ex.Message;
                            tr.Rollback();
                        }
                    }
                }
            }
            return rt;
        }

        // Starts the task in a specific DateTime. It used for manual registration
        /*Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if the user not currently working on the task
         * 4 if error while insert into query
         */
        public int Pause(InputPoint usr, DateTime regDate)
        {
            int rt = 0;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    // Verifico che l'ultima azione per questo utente sia di Inizio del task
                    bool check = true;
                    /*cmd.CommandText = "SELECT evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                        + " AND user = '" + usr.username + "' ORDER BY data desc";
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        if (rdr.GetChar(0) == 'I')
                        {
                            check = true;
                        }
                    }
                    rdr.Close();*/
                    if (check == true)
                    {
                        int maxID = 0;
                        int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(id) FROM registroeventitaskproduzione", null, tr);
                        if (maxRow.HasValue)
                        {
                            maxID = maxRow.Value + 1;
                        }
                        try
                        {
                            Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                            conn.Execute("INSERT INTO registroeventitaskproduzione(id, inputpoint, task, data, evento, note) VALUES(@maxid, @ipid, @taskid, @date, @event, @notes)",
                                new { maxid = maxID, ipid = usr.id, taskid = this.TaskProduzioneID, date = TimeZoneInfo.ConvertTimeToUtc(regDate, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss"), @event = 'P', notes = "" }, tr);
                            this.loadUtentiAttivi();
                            if (this.UtentiAttivi.Count == 0 || (this.UtentiAttivi.Count == 1 && this.UtentiAttivi[0] == usr.id))
                            {
                                conn.Execute("UPDATE tasksproduzione SET status='P' WHERE taskID = @taskid",
                                    new { taskid = this.TaskProduzioneID }, tr);
                            }
                            tr.Commit();
                            rt = 1;
                        }
                        catch (Exception ex)
                        {
                            rt = 4;
                            log += ex.Message;
                            tr.Rollback();
                        }
                    }
                    else
                    {
                        rt = 2;
                    }
                }
            }
            return rt;
        }

        public Boolean Complete(InputPoint usr)
        {
            this.log = "";
            Boolean rt = false;
            if (this.Status != 'F')
            {
                // Controllo che l'utente sia loggato
                this.loadUtentiAttivi();
                bool checkUtenteAttivo = false;
                for (int i = 0; i < this.UtentiAttivi.Count; i++)
                {
                    if (usr.id == this.UtentiAttivi[i])
                    {
                        checkUtenteAttivo = true;
                    }
                }
                bool controlloUltimaAzione = false;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT evento FROM registroeventitaskproduzione WHERE task = @task"
                    + " AND inputpoint = @inputpoint ORDER BY data desc";
                    String evento = conn.QueryFirstOrDefault<String>(sql, new { task = this.TaskProduzioneID, inputpoint = usr.id });
                if (evento != null)
                {
                    if (evento[0] == 'I')
                    {
                        controlloUltimaAzione = true;
                    }
                    else
                    {
                        controlloUltimaAzione = false;
                    }
                }
                else
                {
                    controlloUltimaAzione = true;
                }

                bool checkIfPreviousCompleted = true;
                this.loadPrecedenti();
                for (int i = 0; i < this.PreviousTasks.Count; i++)
                {
                    TaskProduzione prec = new TaskProduzione(this.Tenant, this.PreviousTasks[i].NearTaskID);
                    if (prec.Status != 'F')
                    {
                        checkIfPreviousCompleted = false;
                    }
                }

                // Copies all fixed parameters from the model to the current task
                this.CopyFixedParameters();
                // Check that all mandatory parameters have been set
                Boolean checkParameters = this.CheckParametersComplete();

                this.log = "checkUtenteAttivo: " + checkUtenteAttivo.ToString() + " " + "controlloUltimaAzione: " + controlloUltimaAzione.ToString()
                    + " checkParameters: " + checkParameters.ToString()
                    + "<br/>";
                if (checkUtenteAttivo == true && controlloUltimaAzione == true && checkParameters && checkIfPreviousCompleted)
                {
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            // Termino tutti gli utenti attivi

                            DateTime endDate = DateTime.UtcNow;
                            this.loadUtentiAttivi();
                            for (int i = 0; i < this.UtentiAttivi.Count; i++)
                            {
                                int idEv = 0;
                                int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(id) FROM registroeventitaskproduzione", null, tr);
                                if (maxRow.HasValue)
                                {
                                    idEv = maxRow.Value + 1;
                                }
                                conn.Execute("INSERT INTO registroeventitaskproduzione(id, inputpoint, task, data, evento, note) VALUES(@id, @inputpoint, @task, @date, 'F', '')",
                                    new { id = idEv, inputpoint = this.ActiveInputPoints[i], task = this.TaskProduzioneID, date = endDate.ToString("yyyy/MM/dd HH:mm:ss") }, tr);
                            }
                            // Imposto lo stato del task a terminato
                            conn.Execute("UPDATE tasksproduzione SET status = 'F', endDateReal=@endDateReal WHERE taskID = @task",
                                new { endDateReal = endDate.ToString("yyyy-MM-dd HH:mm:ss"), task = this.TaskProduzioneID }, tr);
                            this._Status = 'F';
                            this._RealEndDate = endDate;

                            // Se tutti i task sono conclusi, allora imposto l'articolo come terminato!
                            bool controlloFineTasks = true;
                            Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                            art.loadTasksProduzione();
                            for (int j = 0; j < art.Tasks.Count; j++)
                            {
                                if (art.Tasks[j].TaskProduzioneID != this.TaskProduzioneID && art.Tasks[j].Status != 'F')
                                {
                                    controlloFineTasks = false;
                                }
                            }
                            if (controlloFineTasks == true)
                            {

                                conn.Execute("UPDATE productionplan SET status = 'F', quantitaProdotta=@quantitaProdotta"
                                    + ", EndProductionDateReal=@endProductionDateReal"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { quantitaProdotta = this.QuantitaProdotta, endProductionDateReal = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                this._Status = 'F';

                                // Segnalo a kanbanbox che ho finito il mio mestiere!
                                KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                                Cliente cust = new Cliente(this.Tenant, art.Cliente);
                                if (rp.id != -1 && cust.CodiceCliente.Length > 0)
                                {
                                    if (kboxCfg != null && art != null && kboxCfg.KanbanBoxEnabled && art.KanbanCardID.Length > 0 && (rp.KanbanManaged || cust.KanbanManaged))
                                    {
                                        art.changeKanbanBoxCardStatus("full");
                                        //log +=  "<br />" + art.log;
                                    }
                                }
                            }


                            tr.Commit();
                            rt = true;
                        }
                        catch (Exception ex)
                        {
                            log += ex.Message;
                            rt = false;
                            tr.Rollback();
                        }
                    }

                    // Sets the working time, delay and leadtime
                    if (rt)
                    {
                        TimeSpan dela = this.ritardo;
                        dela = dela < new TimeSpan(838, 59, 59) ? dela : new TimeSpan(838, 59, 58);
                        String delaStr = Math.Floor(dela.TotalHours).ToString() + ":" + dela.Minutes.ToString() + ":" + dela.Seconds.ToString();
                        TimeSpan workingtime = this.TempoDiLavoroEffettivo;
                        workingtime = workingtime < new TimeSpan(838, 59, 59) ? this.TempoDiLavoroEffettivo : new TimeSpan(838, 59, 58);
                        String workingtimeStr = Math.Floor(workingtime.TotalHours).ToString() + ":" + workingtime.Minutes.ToString() + ":" + workingtime.Seconds.ToString();
                        TimeSpan lt = this.getLeadTime();
                        lt = lt < new TimeSpan(838, 59, 59) ? this.getLeadTime() : new TimeSpan(838, 59, 58);
                        String ltStr = Math.Floor(lt.TotalHours).ToString() + ":" + lt.Minutes.ToString() + ":" + lt.Seconds.ToString();
                        string sql2 = "UPDATE tasksproduzione SET WorkingTime = @workingTime, Delay=@delay, LeadTime=@leadTime WHERE taskid = @task";
                        conn.Execute(sql2, new { workingTime = workingtimeStr, delay = delaStr, leadTime = ltStr, task = this.TaskProduzioneID });

                        this.log = sql2;
                    }

                    if (this.Status == 'F')
                    {
                        // Aggiorno il leadtime
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                art.loadLeadTimes();
                                TimeSpan finalLt = art.CalculateLeadTime();
                                String leadtime = Math.Floor(finalLt.TotalHours).ToString()
                                    + ":" + finalLt.Minutes.ToString() + ":" + finalLt.Seconds.ToString();
                                conn.Execute("UPDATE productionplan SET leadtime=@leadtimeProd"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { leadtimeProd = leadtime, articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                //this.log = "Lead time fail: " + ex.Message;
                                tr.Rollback();
                            }
                        }

                        // Aggiorno il WorkingTime
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                art.loadTempoDiLavoroTotale();
                                TimeSpan tLavTot = art.TempoDiLavoroTotale;
                                String tLavTotStr = Math.Floor(tLavTot.TotalHours).ToString()
                                    + ":" + tLavTot.Minutes.ToString() + ":" + tLavTot.Seconds.ToString();
                                conn.Execute("UPDATE productionplan SET WorkingTime=@workingTimeProd"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { workingTimeProd = tLavTotStr, articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                //this.log = "Working time fail: " + ex.Message;
                                tr.Rollback();
                            }
                        }
                        // Updates delay
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                TimeSpan tRitTot = art.Ritardo;
                                String tRitTotStr = Math.Floor(tRitTot.TotalHours).ToString()
                                    + ":" + tRitTot.Minutes.ToString() + ":" + tRitTot.Seconds.ToString();
                                conn.Execute("UPDATE productionplan SET Delay=@delayProd"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { delayProd = tRitTotStr, articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                //   this.log = "Delay fail: " + ex.Message;
                                tr.Rollback();
                            }
                        }

                    }
                }
                }
            }
            else
            {
                rt = false;
            }
            return rt;
        }

        // Starts the task in a specific DateTime. It used for manual registration
        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if task is already in status F
         * 3 if user is not currently working on the task
         * 4 if all previous tasks are not finished
         * 5 if there are some parameters that needs to be defined
         * 6 if there are problems during the insert into queries
         */
        public int Complete(InputPoint usr, DateTime regDate)
        {
            this.log = regDate.ToString();
            int rt = 0;
            if (this.Status != 'F')
            {
                // Controllo che l'utente sia loggato
                this.loadUtentiAttivi();
                bool checkUtenteAttivo = false;
                for (int i = 0; i < this.UtentiAttivi.Count; i++)
                {
                    if (usr.id == this.ActiveInputPoints[i])
                    {
                        checkUtenteAttivo = true;
                    }
                }
                bool controlloUltimaAzione = false;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT evento FROM registroeventitaskproduzione WHERE task = @task"
                    + " AND inputpoint = @inputpoint ORDER BY data desc";
                    String evento = conn.QueryFirstOrDefault<String>(sql, new { task = this.TaskProduzioneID, inputpoint = usr.id });
                if (evento != null)
                {
                    if (evento[0] == 'I')
                    {
                        controlloUltimaAzione = true;
                    }
                    else
                    {
                        controlloUltimaAzione = false;
                    }
                }
                else
                {
                    controlloUltimaAzione = true;
                }

                if (!controlloUltimaAzione)
                {
                    rt = 3;
                }

                bool checkIfPreviousCompleted = true;
                this.loadPrecedenti();
                for (int i = 0; i < this.PreviousTasks.Count; i++)
                {
                    TaskProduzione prec = new TaskProduzione(this.Tenant, this.PreviousTasks[i].NearTaskID);
                    if (prec.Status != 'F')
                    {
                        checkIfPreviousCompleted = false;
                    }
                }

                if (!checkIfPreviousCompleted)
                {
                    rt = 4;
                }

                // Copies all fixed parameters from the model to the current task
                this.CopyFixedParameters();
                // Check that all mandatory parameters have been set
                Boolean checkParameters = this.CheckParametersComplete();

                if (!checkParameters)
                {
                    rt = 5;
                }

                this.log = "checkUtenteAttivo: " + checkUtenteAttivo.ToString() + " " + "controlloUltimaAzione: " + controlloUltimaAzione.ToString()
                    + " checkParameters: " + checkParameters.ToString() + " checkIfPreviousCompleted: " + checkIfPreviousCompleted
                    + "<br/>";
                if (checkUtenteAttivo == true && controlloUltimaAzione == true && checkParameters && checkIfPreviousCompleted)
                {

                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            // Termino tutti gli utenti attivi
                            Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                            DateTime endDate = TimeZoneInfo.ConvertTimeToUtc(regDate, rp.tzFusoOrario);
                            this.loadUtentiAttivi();
                            for (int i = 0; i < this.UtentiAttivi.Count; i++)
                            {
                                int idEv = 0;
                                int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(id) FROM registroeventitaskproduzione", null, tr);
                                if (maxRow.HasValue)
                                {
                                    idEv = maxRow.Value + 1;
                                }
                                conn.Execute("INSERT INTO registroeventitaskproduzione(id, user, task, data, evento, note) VALUES(@id, @user, @task, @date, 'F', '')",
                                    new { id = idEv, user = this.UtentiAttivi[i].ToString(), task = this.TaskProduzioneID, date = endDate.ToString("yyyy-MM-dd HH:mm:ss") }, tr);
                            }
                            // Imposto lo stato del task a terminato
                            conn.Execute("UPDATE tasksproduzione SET status = 'F', endDateReal=@endDateReal WHERE taskID = @task",
                                new { endDateReal = endDate.ToString("yyyy-MM-dd HH:mm:ss"), task = this.TaskProduzioneID }, tr);
                            this._Status = 'F';
                            this._RealEndDate = endDate;

                            // Se tutti i task sono conclusi, allora imposto l'articolo come terminato!
                            bool controlloFineTasks = true;
                            Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                            art.loadTasksProduzione();
                            for (int j = 0; j < art.Tasks.Count; j++)
                            {
                                if (art.Tasks[j].TaskProduzioneID != this.TaskProduzioneID && art.Tasks[j].Status != 'F')
                                {
                                    controlloFineTasks = false;
                                }
                            }
                            if (controlloFineTasks == true)
                            {

                                conn.Execute("UPDATE productionplan SET status = 'F', quantitaProdotta=@quantitaProdotta"
                                    + ", EndProductionDateReal=@endProductionDateReal"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { quantitaProdotta = this.QuantitaProdotta, endProductionDateReal = endDate.ToString("yyyy-MM-dd HH:mm:ss"), articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                this._Status = 'F';

                                // Segnalo a kanbanbox che ho finito il mio mestiere!
                                KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                                //Reparto rp = new Reparto(this.RepartoID);
                                Cliente cust = new Cliente(this.Tenant, art.Cliente);
                                if (rp.id != -1 && cust.CodiceCliente.Length > 0)
                                {
                                    if (kboxCfg != null && art != null && kboxCfg.KanbanBoxEnabled && art.KanbanCardID.Length > 0 && (rp.KanbanManaged || cust.KanbanManaged))
                                    {
                                        art.changeKanbanBoxCardStatus("full");
                                    }
                                }
                            }


                            tr.Commit();
                            rt = 1;
                        }
                        catch (Exception ex)
                        {
                            log += ex.Message;
                            rt = 6;
                            tr.Rollback();
                        }
                    }

                    // Sets the working time, delay and leadtime
                    if (rt == 1)
                    {
                        TimeSpan dela = this.getDelay(regDate);
                        String delaStr = Math.Floor(dela.TotalHours).ToString() + ":" + dela.Minutes.ToString() + ":" + dela.Seconds.ToString();
                        TimeSpan workingtime = this.TempoDiLavoroEffettivo;
                        String workingtimeStr = Math.Floor(workingtime.TotalHours).ToString() + ":" + workingtime.Minutes.ToString() + ":" + workingtime.Seconds.ToString();
                        TimeSpan lt = this.getLeadTime();
                        String ltStr = Math.Floor(lt.TotalHours).ToString() + ":" + lt.Minutes.ToString() + ":" + lt.Seconds.ToString();
                        string sql2 = "UPDATE tasksproduzione SET WorkingTime = @workingTime, Delay=@delay, LeadTime=@leadTime WHERE taskid = @task";
                        conn.Execute(sql2, new { workingTime = workingtimeStr, delay = delaStr, leadTime = ltStr, task = this.TaskProduzioneID });

                        this.log = sql2;
                    }

                    if (this.Status == 'F')
                    {
                        // Aggiorno il leadtime
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                art.loadLeadTimes();
                                TimeSpan finalLt = art.CalculateLeadTime();
                                String leadtime = Math.Floor(finalLt.TotalHours).ToString()
                                    + ":" + finalLt.Minutes.ToString() + ":" + finalLt.Seconds.ToString();
                                conn.Execute("UPDATE productionplan SET leadtime=@leadtimeProd"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { leadtimeProd = leadtime, articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                            }
                        }

                        // Aggiorno il WorkingTime
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                art.loadTempoDiLavoroTotale();
                                TimeSpan tLavTot = art.TempoDiLavoroTotale;
                                String tLavTotStr = Math.Floor(tLavTot.TotalHours).ToString()
                                    + ":" + tLavTot.Minutes.ToString() + ":" + tLavTot.Seconds.ToString();
                                conn.Execute("UPDATE productionplan SET WorkingTime=@workingTimeProd"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { workingTimeProd = tLavTotStr, articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                            }
                        }
                        // Updates delay
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                TimeSpan tRitTot = art.Ritardo;
                                String tRitTotStr = Math.Floor(tRitTot.TotalHours).ToString()
                                    + ":" + tRitTot.Minutes.ToString() + ":" + tRitTot.Seconds.ToString();
                                conn.Execute("UPDATE productionplan SET Delay=@delayProd"
                                    + " WHERE id = @articoloID"
                                    + " AND anno = @articoloAnno",
                                    new { delayProd = tRitTotStr, articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                            }
                        }

                    }
                }
                }
            }
            return rt;
        }

        public TimeSpan TempoDiLavoroEffettivo
        {
            get
            {
                TimeSpan tc = new TimeSpan(0, 0, 0);
                if (this.TaskProduzioneID != -1 && this.RepartoID != -1)
                {
                    if (this.rp == null || rp.id == -1)
                    {
                        this.rp = new Reparto(this.Tenant, this.RepartoID);
                    }
                    log = "<br/><B>" + rp.id.ToString() + "</b> " + rp.ModoCalcoloTC.ToString() + "<br />";
                    if (rp.ModoCalcoloTC == false)
                    {
                        // Calcolo il tempo di lavoro NON tenendo conto degli intervalli produttivi
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                            string sql = "SELECT inputpoint, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                            + " ORDER BY inputpoint, data";
                        var rows = conn.Query<(String User, DateTime Data, String Evento)>(sql, new { task = this.TaskProduzioneID }).ToList();
                        int idx = 0;
                        while (idx < rows.Count)
                        {
                            log += "1-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                            DateTime inizio = rows[idx].Data;
                            String usrI = rows[idx].User;
                            Char EventoI = rows[idx].Evento[0];
                            idx++;
                            if (EventoI == 'I')
                            {
                                if (idx < rows.Count)
                                {
                                    log += "2-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                                    String usrF = rows[idx].User;
                                    Char EventoF = rows[idx].Evento[0];
                                    DateTime fine = rows[idx].Data;
                                    idx++;
                                    if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                    {

                                        tc += (fine - inizio);
                                        log += (fine - inizio).ToString();
                                        log += tc.ToString() + "--> OK<br/><br/>";
                                    }
                                    else // RAMO AGGIUNTO PER EVITARE CHE SE CI SONO FASI IN STATO "I", QUESTE PORTINO IL CONTO A 0
                                    {
                                        idx++;
                                    }
                                }
                            }
                        }
                        }
                    }
                    else
                    {
                        // Tengo conto degli intervalli produttivi
                        using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                        {
                            conn.Open();
                        List<DateTime[]> elenco = new List<DateTime[]>();
                        string sql = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                            + " ORDER BY user, data";
                        var rows = conn.Query<(String User, DateTime Data, String Evento)>(sql, new { task = this.TaskProduzioneID }).ToList();
                        log += "TENGO CONTO DEGLI INTERVALLI DI LAVORO<BR/>";
                        int idx = 0;
                        while (idx < rows.Count)
                        {
                            //log += "1-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                            DateTime inizio = rows[idx].Data;
                            String usrI = rows[idx].User;
                            Char EventoI = rows[idx].Evento[0];
                            idx++;
                            if (idx < rows.Count)
                            {
                                //log += "2-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                                String usrF = rows[idx].User;
                                Char EventoF = rows[idx].Evento[0];
                                DateTime fine = rows[idx].Data;
                                idx++;
                                if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                {
                                    DateTime[] interv = new DateTime[2];
                                    interv[0] = inizio;
                                    interv[1] = fine;
                                    elenco.Add(interv);
                                    //tc += (fine - inizio);
                                    log += (fine - inizio).ToString();
                                    log += tc.ToString() + "--> OK<br/><br/>";
                                }
                                else // RAMO AGGIUNTO PER EVITARE CHE SE CI SONO FASI IN STATO "I", QUESTE PORTINO IL CONTO A 0
                                {
                                    idx++;
                                }
                            }
                        }

                        for (int i = 0; i < elenco.Count; i++)
                        {
                            log += elenco[i][0].ToString("dd/MM/yyyy HH:mm:ss") + " - " + elenco[i][1].ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                            rp.loadCalendario(elenco[i][0].AddDays(-7), elenco[i][1].AddDays(7));
                            for (int j = 0; j < rp.CalendarioRep.Intervalli.Count; j++)
                            {
                                log += rp.CalendarioRep.Intervalli[j].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " +
                                    rp.CalendarioRep.Intervalli[j].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " ";
                                if (rp.CalendarioRep.Intervalli[j].Inizio > elenco[i][1])
                                {
                                    log += "if1<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Fine < elenco[i][0])
                                {
                                    log += "if2<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i][0] && rp.CalendarioRep.Intervalli[j].Fine <= elenco[i][1])
                                {
                                    tc += (rp.CalendarioRep.Intervalli[j].Fine - rp.CalendarioRep.Intervalli[j].Inizio);
                                    log += "if3 " + tc.TotalHours.ToString() + "<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i][0] && elenco[i][0] <= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i][1])
                                {
                                    tc += (elenco[i][1] - rp.CalendarioRep.Intervalli[j].Inizio);
                                    log += "if4 " + tc.TotalHours.ToString() + "<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i][0] && elenco[i][1] >= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i][0])
                                {
                                    tc += (rp.CalendarioRep.Intervalli[j].Fine - elenco[i][0]);
                                    log += "if5 " + tc.TotalHours.ToString() + "<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i][0] && elenco[i][1] <= rp.CalendarioRep.Intervalli[j].Fine)
                                {
                                    tc += (elenco[i][1] - elenco[i][0]);
                                    log += "if6 " + tc.TotalHours.ToString() + "<br />";
                                }
                            }
                        }
                        }
                    }
                }
                return tc;
            }
        }

        public Double TempoDiLavoroEffettivoDbl
        {
            get
            {
                return Math.Round(this.TempoDiLavoroEffettivo.TotalHours, 2);
            }
        }

        public TimeSpan TempoDiLavoroEffettivoUnitario
        {
            get
            {
                return new TimeSpan(this.TempoDiLavoroEffettivo.Ticks / this.QuantitaProdotta);
            }
        }

        public Double TempoDiLavoroEffettivoUnitarioDbl
        {
            get
            {
                return Math.Round(this.TempoDiLavoroEffettivoUnitario.TotalHours, 2);
            }
        }

        public TimeSpan getTempoDiLavoroEffettivo(DateTime startDate, DateTime endDate)
        {
            TimeSpan tc = new TimeSpan(0, 0, 0);
            if (this.TaskProduzioneID != -1 && this.RepartoID != -1)
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                log = "<br/><B>" + rp.id.ToString() + "</b> " + rp.ModoCalcoloTC.ToString() + "<br />";
                if (rp.ModoCalcoloTC == false)
                {
                    // Calcolo il tempo di lavoro NON tenendo conto degli intervalli produttivi
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                            + " AND data >= @startDate "
                            + "AND data <= @endDate"
                            + " ORDER BY user, data";
                    var rows = conn.Query<(String User, DateTime Data, String Evento)>(sql, new { task = this.TaskProduzioneID, startDate = startDate.ToString("yyyy/MM/dd HH:mm:ss"), endDate = endDate.ToString("yyyy/MM/dd HH:mm:ss") }).ToList();
                    int idx = 0;
                    while (idx < rows.Count)
                    {
                        log += "1-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                        DateTime inizio = rows[idx].Data;
                        String usrI = rows[idx].User;
                        Char EventoI = rows[idx].Evento[0];
                        idx++;
                        if (idx < rows.Count)
                        {
                            log += "2-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                            String usrF = rows[idx].User;
                            Char EventoF = rows[idx].Evento[0];
                            DateTime fine = rows[idx].Data;
                            idx++;
                            if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                            {

                                tc += (fine - inizio);
                                log += (fine - inizio).ToString();
                                log += tc.ToString() + "--> OK<br/><br/>";
                            }
                        }
                    }
                    }
                }
                else
                {
                    // Tengo conto degli intervalli produttivi
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        List<DateTime[]> elenco = new List<DateTime[]>();
                    string sql = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                        + " ORDER BY user, data";
                    var rows = conn.Query<(String User, DateTime Data, String Evento)>(sql, new { task = this.TaskProduzioneID }).ToList();
                    log += "TENGO CONTO DEGLI INTERVALLI DI LAVORO<BR/>";
                    int idx = 0;
                    while (idx < rows.Count)
                    {
                        //log += "1-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                        DateTime inizio = rows[idx].Data;
                        String usrI = rows[idx].User;
                        Char EventoI = rows[idx].Evento[0];
                        idx++;
                        if (idx < rows.Count)
                        {
                            //log += "2-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                            String usrF = rows[idx].User;
                            Char EventoF = rows[idx].Evento[0];
                            DateTime fine = rows[idx].Data;
                            idx++;
                            if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                            {
                                DateTime[] interv = new DateTime[2];
                                interv[0] = inizio;
                                interv[1] = fine;
                                elenco.Add(interv);
                                //tc += (fine - inizio);
                                log += (fine - inizio).ToString();
                                log += tc.ToString() + "--> OK<br/><br/>";
                            }
                        }
                    }

                    for (int i = 0; i < elenco.Count; i++)
                    {
                        log += elenco[i][0].ToString("dd/MM/yyyy HH:mm:ss") + " - " + elenco[i][1].ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                        rp.loadCalendario(elenco[i][0].AddDays(-7), elenco[i][1].AddDays(7));
                        for (int j = 0; j < rp.CalendarioRep.Intervalli.Count; j++)
                        {
                            log += rp.CalendarioRep.Intervalli[j].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " +
                                rp.CalendarioRep.Intervalli[j].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " ";
                            if (rp.CalendarioRep.Intervalli[j].Inizio > elenco[i][1])
                            {
                                log += "if1<br />";
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Fine < elenco[i][0])
                            {
                                log += "if2<br />";
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i][0] && rp.CalendarioRep.Intervalli[j].Fine <= elenco[i][1])
                            {
                                tc += (rp.CalendarioRep.Intervalli[j].Fine - rp.CalendarioRep.Intervalli[j].Inizio);
                                log += "if3 " + tc.TotalHours.ToString() + "<br />";
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i][0] && elenco[i][0] <= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i][1])
                            {
                                tc += (elenco[i][1] - rp.CalendarioRep.Intervalli[j].Inizio);
                                log += "if4 " + tc.TotalHours.ToString() + "<br />";
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i][0] && elenco[i][1] >= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i][0])
                            {
                                tc += (rp.CalendarioRep.Intervalli[j].Fine - elenco[i][0]);
                                log += "if5 " + tc.TotalHours.ToString() + "<br />";
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i][0] && elenco[i][1] <= rp.CalendarioRep.Intervalli[j].Fine)
                            {
                                tc += (elenco[i][1] - elenco[i][0]);
                                log += "if6 " + tc.TotalHours.ToString() + "<br />";
                            }
                        }
                }
                }
                }
            }
            return tc;
        }

        public TimeSpan TempoDiLavoroPrevisto
        {
            get
            {
                TimeSpan ret = TimeSpan.FromTicks(this.NumOperatori * this.TempoC.Ticks);
                return ret;
            }
        }

        /* Data effettiva di inizio task */
        public DateTime DataInizioTask
        {
            get
            {
                DateTime min = DateTime.UtcNow.AddDays(30);
                if (this.Status == 'F' || this.Status == 'I')
                {
                    if (this.Eventi == null || this.Eventi.Count == 0)
                    {
                        this.loadEventi();
                    }
                    for (int i = 0; i < this.Eventi.Count; i++)
                    {
                        if (this.Eventi[i].Evento == 'I' && this.Eventi[i].Data < min)
                        {
                            min = this.Eventi[i].Data;
                        }
                    }
                }
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                //return TimeZoneInfo.ConvertTimeFromUtc(min, rp.tzFusoOrario);
                return min;
            }
        }

        /* Data effettiva di fine task */
        private DateTime _RealEndDate;
        public DateTime RealEndDate
        {
            get
            {
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._RealEndDate, rp.tzFusoOrario);
            }
        }

        /* 20181127: DataFineTask is DEPRECATED */
        public DateTime DataFineTask
        {
            get
            {
                if (this._RealEndDate.Year > 2000)
                {
                    return this.RealEndDate;
                }
                else
                {
                    DateTime max = new DateTime(1970, 1, 1);
                    if (this.Status == 'F')
                    {
                        if (this.Eventi == null || this.Eventi.Count == 0)
                        {
                            this.loadEventi();
                        }
                        for (int i = 0; i < this.Eventi.Count; i++)
                        {
                            if (this.Eventi[i].Evento == 'F')
                            {
                                max = this.Eventi[i].Data;
                            }
                        }
                    }
                    if (this.rp == null || rp.id == -1)
                    {
                        this.rp = new Reparto(this.Tenant, this.RepartoID);
                    }
                    //return TimeZoneInfo.ConvertTimeFromUtc(max, rp.tzFusoOrario);
                    return max;
                }
            }
        }

        // Gestione degli warning
        private List<Warning> _Warnings;
        public List<Warning> Warnings
        {
            get { return this._Warnings; }
        }

        public void loadWarnings()
        {
            this._Warnings = new List<Warning>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id FROM warningproduzione WHERE task = @task" +
                    " ORDER BY dataChiamata";
                var rows = conn.Query<int>(sql, new { task = this.TaskProduzioneID });
                foreach (var id in rows)
                {
                    this._Warnings.Add(new Warning(this.Tenant, id));
                }
            }
        }

        public bool generateWarning(InputPoint usr)
        {
            bool rt = false;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                using (var tr = conn.BeginTransaction())
                {
                    try
                    {
                        int maxID = 0;
                        int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT max(id) FROM warningproduzione", null, tr);
                        if (maxRow.HasValue)
                        {
                            maxID = maxRow.Value + 1;
                        }
                        conn.Execute("INSERT INTO warningproduzione(id, dataChiamata, task, inputpoint) VALUES("
                            + "@maxID, @dataChiamata, @task, @inputpoint)",
                            new { maxID = maxID, dataChiamata = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), task = this.TaskProduzioneID, inputpoint = usr.id }, tr);

                        conn.Execute("DELETE FROM registroeventiproduzione WHERE TipoEvento='Warning' AND taskID = @taskID",
                            new { taskID = this.TaskProduzioneID }, tr);

                        conn.Execute("INSERT INTO registroeventiproduzione(TipoEvento, taskID, segnalato) VALUES('Warning', "
                            + "@taskID, false)",
                            new { taskID = this.TaskProduzioneID }, tr);

                        rt = true;
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        rt = false;
                        tr.Rollback();
                    }
                }
            }
            return rt;
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
                conn.Open();
                string sql = "SELECT warningproduzione.id FROM warningproduzione WHERE warningproduzione.dataRisoluzione IS NULL "
                + " AND task = @task ORDER BY warningproduzione.dataChiamata";
                var rows = conn.Query<int>(sql, new { task = this.TaskProduzioneID });
                foreach (var id in rows)
                {
                    this._WarningAperti.Add(new Warning(this.Tenant, id));
                }
            }
        }

        /* 
         * Questo algoritmo si comporta in tale modo:
         * il ritardo per i task finiti, in corso di esecuzione o in pausa è calcolato sulla base del 
         * LateFinish e del tempo di fine effettivo
         * 
         * per i task da iniziare è calcolato in base al LateStart
         * 
         * Il funzionamento dell'algoritmo è il seguente:
         * Calcolo il ritardo "grezzo", cioè sulla base del calendario umano
         * Dopodiché vado a depurare il ritardo dal monte ore non lavorato che sta tra 
         * data prevista (inizio/fine) e data effettiva (inizio/fine)
         * se data prevista > data effettiva, allora il ritardo è ZERO!
         */
        public TimeSpan ritardo
        {
            get
            {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                log = "";
                TimeSpan rit = new TimeSpan(0, 0, 0);
                if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) <= this.LateStart || (this.Status == 'F' && this.DataInizioTask <= this.LateStart))
                {
                    rit = new TimeSpan(0, 0, 0);
                }
                else
                {
                    DateTime lateStart = this.LateStart;
                    DateTime lateFinish = this.LateFinish;

                    if (Status == 'N')
                    {
                        DateTime df = new DateTime();
                        df = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario);
                        rit = df - this.LateStart;
                        rp.loadCalendario(this.LateStart.AddDays(-7), df.AddDays(7));
                        int indInizio = -1;
                        int indFine = -1;
                        bool foundFine = false;
                        for (int i = 0; i < rp.CalendarioRep.Intervalli.Count && (indInizio == -1 || indFine == -1); i++)
                        {
                            //log += i.ToString() + "<br />";
                            if (rp.CalendarioRep.Intervalli[i].Inizio <= lateStart && lateStart <= rp.CalendarioRep.Intervalli[i].Fine)
                            {
                                //log += "indInizio: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                                indInizio = i;
                            }
                            else if ((i + 1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < lateStart && lateStart < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                            {
                                //log += "indInizio Esterno: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + df.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + rp.CalendarioRep.Intervalli[i + 1].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                                indInizio = i;
                                foundFine = true;
                            }
                            if (rp.CalendarioRep.Intervalli[i].Inizio <= df && df <= rp.CalendarioRep.Intervalli[i].Fine)
                            {
                                //log += "indFine: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + df.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                                indFine = i;
                                foundFine = true;
                            }
                            else if (rp.CalendarioRep.Intervalli[i].Fine < df && df < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                            {
                                //log += "indFine Esterna: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + df.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + rp.CalendarioRep.Intervalli[i + 1].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                                indFine = i;
                                foundFine = true;
                            }
                        }

                        if (foundFine == true)
                        {
                            //log += "Calcolo il ritardo: <br/>";
                            while (indInizio < indFine)
                            {
                                if ((indInizio + 1) < rp.CalendarioRep.Intervalli.Count && indInizio > -1)
                                {
                                    // Questo tiene conto del fatto che il LateFinish potrebbe essere esterno ad un intervalli di lavoro se elimino straordinari o ferie...
                                    //DateTime prevFine = this.LateFinish > rp.CalendarioRep.Intervalli[indInizio].Fine ? this.LateFinish : rp.CalendarioRep.Intervalli[indInizio].Fine;
                                    DateTime prevFine = lateStart > rp.CalendarioRep.Intervalli[indInizio].Fine ? lateStart : rp.CalendarioRep.Intervalli[indInizio].Fine;
                                    log += (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - rp.CalendarioRep.Intervalli[indInizio].Fine).Hours.ToString() + ":" + (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - rp.CalendarioRep.Intervalli[indInizio].Fine).Minutes.ToString() + ":" + (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - rp.CalendarioRep.Intervalli[indInizio].Fine).Seconds.ToString() + " ";
                                    rit -= (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - prevFine);
                                }
                                indInizio++;
                            }

                            if (rp.CalendarioRep.Intervalli[indFine].Fine < df)
                            {
                                rit -= (df - rp.CalendarioRep.Intervalli[indFine].Fine);
                            }
                        }
                        else
                        {
                            rit = new TimeSpan(0, 0, 0);
                        }
                    }
                    else if (this.Status == 'I' || this.Status == 'P' || this.Status == 'F')
                    {
                        DateTime df = new DateTime();
                        if (this.Status == 'F')
                        {
                            df = this.DataFineTask;
                        }
                        else
                        {
                            df = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario);
                        }
                        rit = df - this.LateFinish;
                        DateTime uno = df < lateFinish ? df : lateFinish;
                        DateTime due = df < lateFinish ? lateFinish : df;
                        //rp.loadCalendario(this.LateStart.AddDays(-1), df.AddDays(1));
                        rp.loadCalendario(uno.AddDays(-7), due.AddDays(7));
                        int indInizio = -1;
                        int indFine = -1;
                        bool foundFine = false;
                        //try
                        //{
                        /*log = rit.TotalHours.ToString() + "<br />"
                            + uno.ToString("dd/MM/yyyy HH:mm:ss")
                            + due.ToString("dd/MM/yyyy HH:mm:ss")
                            + this.TaskProduzioneID.ToString() 
                            + " " + this.LateFinish.ToString("dd/MM/yyyy HH:mm:ss")
                            + " " + df.ToString("dd/MM/yyyy HH:mm:ss")
                            + "<br />";*/
                        for (int i = 0; i < rp.CalendarioRep.Intervalli.Count && (indInizio == -1 || indFine == -1); i++)
                        {
                            //log+=rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " "
                            //+ rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                            if (rp.CalendarioRep.Intervalli[i].Inizio <= lateFinish && lateFinish <= rp.CalendarioRep.Intervalli[i].Fine)
                            {
                                //log += " A";
                                indInizio = i;
                            }
                            else if ((i + 1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < lateFinish && lateFinish < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                            {
                                //log += " B";
                                indInizio = i;
                                foundFine = true;
                            }
                            if (rp.CalendarioRep.Intervalli[i].Inizio <= df && df <= rp.CalendarioRep.Intervalli[i].Fine)
                            {
                                //log += " C";
                                indFine = i;
                                foundFine = true;
                            }
                            else if ((i + 1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < df && df < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                            {
                                //log += " D";
                                indFine = i;
                                foundFine = true;
                            }
                            //log += "<br/>";
                        }


                        //log += foundFine.ToString() + " " + indInizio.ToString() + " " + indFine.ToString() + "<br />";
                        if (foundFine == true)
                        {
                            while (indInizio < indFine)
                            {
                                if ((indInizio + 1) < rp.CalendarioRep.Intervalli.Count && indInizio > -1)
                                {
                                    DateTime prevFine = this.LateFinish > rp.CalendarioRep.Intervalli[indInizio].Fine ? this.LateFinish : rp.CalendarioRep.Intervalli[indInizio].Fine;
                                    rit -= (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - prevFine);
                                }
                                indInizio++;
                            }

                            if (rp.CalendarioRep.Intervalli[indFine].Fine < df)
                            {
                                rit -= (df - rp.CalendarioRep.Intervalli[indFine].Fine);
                            }
                        }
                        else
                        {
                            rit = new TimeSpan(0, 0, 0);
                        }

                    }

                }

                if (rit.TotalHours < 0)
                {
                    rit = new TimeSpan(0, 0, 0);
                }
                return rit;
            }
        }

        public TimeSpan getDelay(DateTime endDate)
        {
            if (this.rp == null || rp.id == -1)
            {
                this.rp = new Reparto(this.Tenant, this.RepartoID);
            }
            log = "";
            TimeSpan rit = new TimeSpan(0, 0, 0);
            if (TimeZoneInfo.ConvertTimeFromUtc(endDate, rp.tzFusoOrario) <= this.LateStart || (this.Status == 'F' && this.DataInizioTask <= this.LateStart))
            {
                rit = new TimeSpan(0, 0, 0);
            }
            else
            {
                if (Status == 'N')
                {
                    DateTime df = new DateTime();
                    df = TimeZoneInfo.ConvertTimeFromUtc(endDate, rp.tzFusoOrario);
                    rit = df - this.LateStart;
                    rp.loadCalendario(this.LateStart.AddDays(-7), df.AddDays(7));
                    int indInizio = -1;
                    int indFine = -1;
                    bool foundFine = false;
                    for (int i = 0; i < rp.CalendarioRep.Intervalli.Count && (indInizio == -1 || indFine == -1); i++)
                    {
                        //log += i.ToString() + "<br />";
                        if (rp.CalendarioRep.Intervalli[i].Inizio <= this.LateStart && this.LateStart <= rp.CalendarioRep.Intervalli[i].Fine)
                        {
                            //log += "indInizio: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                            indInizio = i;
                        }
                        else if ((i + 1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < this.LateStart && this.LateStart < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                        {
                            //log += "indInizio Esterno: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + df.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + rp.CalendarioRep.Intervalli[i + 1].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                            indInizio = i;
                            foundFine = true;
                        }
                        if (rp.CalendarioRep.Intervalli[i].Inizio <= df && df <= rp.CalendarioRep.Intervalli[i].Fine)
                        {
                            //log += "indFine: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + df.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                            indFine = i;
                            foundFine = true;
                        }
                        else if (rp.CalendarioRep.Intervalli[i].Fine < df && df < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                        {
                            //log += "indFine Esterna: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + df.ToString("dd/MM/yyyy HH:mm:ss") + " <= " + rp.CalendarioRep.Intervalli[i + 1].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                            indFine = i;
                            foundFine = true;
                        }
                    }

                    if (foundFine == true)
                    {
                        //log += "Calcolo il ritardo: <br/>";
                        while (indInizio < indFine)
                        {
                            if ((indInizio + 1) < rp.CalendarioRep.Intervalli.Count && indInizio > -1)
                            {
                                // Questo tiene conto del fatto che il LateFinish potrebbe essere esterno ad un intervalli di lavoro se elimino straordinari o ferie...
                                //DateTime prevFine = this.LateFinish > rp.CalendarioRep.Intervalli[indInizio].Fine ? this.LateFinish : rp.CalendarioRep.Intervalli[indInizio].Fine;
                                DateTime prevFine = this.LateStart > rp.CalendarioRep.Intervalli[indInizio].Fine ? this.LateStart : rp.CalendarioRep.Intervalli[indInizio].Fine;
                                log += (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - rp.CalendarioRep.Intervalli[indInizio].Fine).Hours.ToString() + ":" + (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - rp.CalendarioRep.Intervalli[indInizio].Fine).Minutes.ToString() + ":" + (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - rp.CalendarioRep.Intervalli[indInizio].Fine).Seconds.ToString() + " ";
                                rit -= (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - prevFine);
                            }
                            indInizio++;
                        }

                        if (rp.CalendarioRep.Intervalli[indFine].Fine < df)
                        {
                            rit -= (df - rp.CalendarioRep.Intervalli[indFine].Fine);
                        }
                    }
                    else
                    {
                        rit = new TimeSpan(0, 0, 0);
                    }
                }
                else if (this.Status == 'I' || this.Status == 'P' || this.Status == 'F')
                {
                    DateTime df = new DateTime();
                    if (this.Status == 'F')
                    {
                        df = this.DataFineTask;
                    }
                    else
                    {
                        df = TimeZoneInfo.ConvertTimeFromUtc(endDate, rp.tzFusoOrario);
                    }
                    rit = df - this.LateFinish;
                    DateTime uno = df < this.LateFinish ? df : this.LateFinish;
                    DateTime due = df < this.LateFinish ? this.LateFinish : df;
                    //rp.loadCalendario(this.LateStart.AddDays(-1), df.AddDays(1));
                    rp.loadCalendario(uno.AddDays(-7), due.AddDays(7));
                    int indInizio = -1;
                    int indFine = -1;
                    bool foundFine = false;
                    //try
                    //{
                    /*log = rit.TotalHours.ToString() + "<br />"
                        + uno.ToString("dd/MM/yyyy HH:mm:ss")
                        + due.ToString("dd/MM/yyyy HH:mm:ss")
                        + this.TaskProduzioneID.ToString() 
                        + " " + this.LateFinish.ToString("dd/MM/yyyy HH:mm:ss")
                        + " " + df.ToString("dd/MM/yyyy HH:mm:ss")
                        + "<br />";*/
                    for (int i = 0; i < rp.CalendarioRep.Intervalli.Count && (indInizio == -1 || indFine == -1); i++)
                    {
                        //log+=rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " "
                        //+ rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                        if (rp.CalendarioRep.Intervalli[i].Inizio <= this.LateFinish && this.LateFinish <= rp.CalendarioRep.Intervalli[i].Fine)
                        {
                            //log += " A";
                            indInizio = i;
                        }
                        else if ((i + 1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < this.LateFinish && this.LateFinish < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                        {
                            //log += " B";
                            indInizio = i;
                            foundFine = true;
                        }
                        if (rp.CalendarioRep.Intervalli[i].Inizio <= df && df <= rp.CalendarioRep.Intervalli[i].Fine)
                        {
                            //log += " C";
                            indFine = i;
                            foundFine = true;
                        }
                        else if ((i + 1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < df && df < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                        {
                            //log += " D";
                            indFine = i;
                            foundFine = true;
                        }
                        //log += "<br/>";
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //   this.log += ex.Message;
                    //    rit = new TimeSpan(0, 0, 0);
                    //}

                    //log += foundFine.ToString() + " " + indInizio.ToString() + " " + indFine.ToString() + "<br />";
                    if (foundFine == true)
                    {
                        while (indInizio < indFine)
                        {
                            if ((indInizio + 1) < rp.CalendarioRep.Intervalli.Count && indInizio > -1)
                            {
                                DateTime prevFine = this.LateFinish > rp.CalendarioRep.Intervalli[indInizio].Fine ? this.LateFinish : rp.CalendarioRep.Intervalli[indInizio].Fine;
                                rit -= (rp.CalendarioRep.Intervalli[indInizio + 1].Inizio - prevFine);
                            }
                            indInizio++;
                        }

                        if (rp.CalendarioRep.Intervalli[indFine].Fine < df)
                        {
                            rit -= (df - rp.CalendarioRep.Intervalli[indFine].Fine);
                        }
                    }
                    else
                    {
                        rit = new TimeSpan(0, 0, 0);
                    }

                }

            }

            if (rit.TotalHours < 0)
            {
                rit = new TimeSpan(0, 0, 0);
            }
            return rit;
        }

        private TimeSpan _Delay;
        public TimeSpan Delay
        {
            get
            {
                return this._Delay;
            }
        }

        private TimeSpan _WorkingTime;
        public TimeSpan WorkingTime
        {
            get
            {
                return this._WorkingTime;
            }
        }

        private TimeSpan _LeadTime;
        public TimeSpan LeadTime
        {
            get { return this._LeadTime; }
        }

        protected TimeSpan getLeadTime()
        {
            TimeSpan sommaLTs = new TimeSpan(0, 0, 0);
            DateTime min = new DateTime(2999, 1, 1);
            DateTime max = new DateTime(1970, 1, 1);
            TimeSpan ret = new TimeSpan(0, 0, 0);
            min = this.DataInizioTask;
            max = this.RealEndDate;
            if (this.Status == 'F' && min <= max)
            {
                // Find earliest started task and latest finished task
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                rp.loadCalendario(min.AddDays(-30), max.AddDays(7));
                for (int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
                {
                    if (min >= rp.CalendarioRep.Intervalli[i].Inizio && rp.CalendarioRep.Intervalli[i].Fine <= max && min <= rp.CalendarioRep.Intervalli[i].Fine)
                    {
                        TimeSpan curr = (rp.CalendarioRep.Intervalli[i].Fine - min);
                        ret = ret.Add(curr);
                    }
                    else if (min <= rp.CalendarioRep.Intervalli[i].Inizio && max >= rp.CalendarioRep.Intervalli[i].Fine)
                    {
                        TimeSpan curr = (rp.CalendarioRep.Intervalli[i].Fine - rp.CalendarioRep.Intervalli[i].Inizio);
                        ret = ret.Add(curr);
                    }
                    else if (rp.CalendarioRep.Intervalli[i].Inizio >= min && max <= rp.CalendarioRep.Intervalli[i].Fine && max >= rp.CalendarioRep.Intervalli[i].Inizio)
                    {
                        TimeSpan curr = (max - rp.CalendarioRep.Intervalli[i].Inizio);
                        ret = ret.Add(curr);
                    }
                    else if (min >= rp.CalendarioRep.Intervalli[i].Inizio && max <= rp.CalendarioRep.Intervalli[i].Fine)
                    {
                        TimeSpan curr = max - min;
                        ret = ret.Add(curr);
                    }
                }

                this._LeadTime = ret;
            }
            return ret;
        }

        private List<IntervalliDiLavoroEffettivi> _Intervalli;
        public List<IntervalliDiLavoroEffettivi> Intervalli
        {
            get
            {
                return this._Intervalli;
            }
        }
        public void loadIntervalliDiLavoroEffettivi()
        {
            this._Intervalli = new List<IntervalliDiLavoroEffettivi>();
            if (this.Status == 'F')
            {
                // Carica gli intervalli di lavoro NON tenendo conto degli intervalli produttivi
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                if (rp.ModoCalcoloTC == false)
                {
                    log = "ENTRO IN LOADINTERVALLI<br />";
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT inputpoint, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                             + " ORDER BY inputpoint, data";
                    var rows = conn.Query<(int User, DateTime Data, String Evento)>(sql, new { task = this.TaskProduzioneID }).ToList();
                    int idx = 0;
                    while (idx < rows.Count)
                    {
                        log += "1-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                        DateTime inizio = rows[idx].Data;
                        int usrI = rows[idx].User;
                        Char EventoI = rows[idx].Evento[0];
                        idx++;
                        if (idx < rows.Count)
                        {
                            log += "2-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                            int usrF = rows[idx].User;
                            Char EventoF = rows[idx].Evento[0];
                            DateTime fine = rows[idx].Data;
                            idx++;
                            if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                            {
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = usrI;
                                curr.Inizio = inizio;
                                curr.Fine = fine;
                                curr.Intervallo = fine - inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                curr.idReparto = rp.id;
                                Postazione pst = new Postazione(this.Tenant, this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                curr.idReparto = rp.id;
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                        }
                    }
                    }
                }
                else
                {
                    // Carica gli intervalli di lavoro tenendo conto degli intervalli produttivi
                    // TO-DO!!!

                    List<IntervalliDiLavoroEffettivi> elenco = new List<IntervalliDiLavoroEffettivi>();

                    log = "ENTRO IN LOADINTERVALLI<br />";
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT inputpoint, data, evento FROM registroeventitaskproduzione WHERE task = @task"
                             + " ORDER BY inputpoint, data";
                    var rows = conn.Query<(int User, DateTime Data, String Evento)>(sql, new { task = this.TaskProduzioneID }).ToList();
                    int idx = 0;
                    while (idx < rows.Count)
                    {
                        log += "1-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                        DateTime inizio = rows[idx].Data;
                        int usrI = rows[idx].User;
                        Char EventoI = rows[idx].Evento[0];
                        idx++;
                        if (idx < rows.Count)
                        {
                            log += "2-Evento: " + rows[idx].Evento[0] + " " + rows[idx].Data + "<br />";
                            int usrF = rows[idx].User;
                            Char EventoF = rows[idx].Evento[0];
                            DateTime fine = rows[idx].Data;
                            idx++;
                            if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                            {
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = usrI;
                                curr.Inizio = inizio;
                                curr.Fine = fine;
                                curr.Intervallo = fine - inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                curr.idReparto = rp.id;
                                Postazione pst = new Postazione(this.Tenant, this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                elenco.Add(curr);
                            }
                        }

                    }
                    }
                    // piazzo qui la divisione!
                    // devo aggiungere le fasi a this.Intervalli
                    for (int i = 0; i < elenco.Count; i++)
                    {
                        log += elenco[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + elenco[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                        rp.loadCalendario(elenco[i].Inizio.AddDays(-7), elenco[i].Fine.AddDays(7));
                        for (int j = 0; j < rp.CalendarioRep.Intervalli.Count; j++)
                        {
                            log += rp.CalendarioRep.Intervalli[j].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " +
                                rp.CalendarioRep.Intervalli[j].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " ";
                            if (rp.CalendarioRep.Intervalli[j].Inizio > elenco[i].Fine)
                            {
                                log += "if1<br />";
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Fine < elenco[i].Inizio)
                            {
                                log += "if2<br />";
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i].Inizio && rp.CalendarioRep.Intervalli[j].Fine <= elenco[i].Fine)
                            {
                                //   tc += (rp.CalendarioRep.Intervalli[j].Fine - rp.CalendarioRep.Intervalli[j].Inizio);
                                //   log += "if3 " + tc.TotalHours.ToString() + "<br />";
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = elenco[i].user;
                                curr.Inizio = rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.Fine = rp.CalendarioRep.Intervalli[j].Fine;
                                curr.Intervallo = rp.CalendarioRep.Intervalli[j].Fine - rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                curr.idReparto = rp.id;
                                Postazione pst = new Postazione(this.Tenant, this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i].Inizio && elenco[i].Inizio <= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i].Fine)
                            {
                                //tc += (elenco[i].Fine - rp.CalendarioRep.Intervalli[j].Inizio);
                                //log += "if4 " + tc.TotalHours.ToString() + "<br />";
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = elenco[i].user;
                                curr.Inizio = rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.Fine = elenco[i].Fine;
                                curr.Intervallo = elenco[i].Fine - rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                curr.idReparto = rp.id;
                                Postazione pst = new Postazione(this.Tenant, this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i].Inizio && elenco[i].Fine >= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i].Inizio)
                            {
                                //tc += (rp.CalendarioRep.Intervalli[j].Fine - elenco[i].Inizio);
                                //log += "if5 " + tc.TotalHours.ToString() + "<br />";
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = elenco[i].user;
                                curr.Inizio = elenco[i].Inizio;
                                curr.Fine = rp.CalendarioRep.Intervalli[j].Fine;
                                curr.Intervallo = rp.CalendarioRep.Intervalli[j].Fine - elenco[i].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                curr.idReparto = rp.id;
                                Postazione pst = new Postazione(this.Tenant, this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i].Inizio && elenco[i].Fine <= rp.CalendarioRep.Intervalli[j].Fine)
                            {
                                //tc += (elenco[i].Fine - elenco[i].Inizio);
                                //log += "if6 " + tc.TotalHours.ToString() + "<br />";
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = elenco[i].user;
                                curr.Inizio = elenco[i].Inizio;
                                curr.Fine = elenco[i].Fine;
                                curr.Intervallo = elenco[i].Fine - elenco[i].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                curr.idReparto = rp.id;
                                Postazione pst = new Postazione(this.Tenant, this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                        }
                    }
                }
            }
        }

        public TimeSpan TempoCicloEffettivo
        {
            get
            {
                TimeSpan tc = new TimeSpan(0, 0, 0);
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                if (rp.ModoCalcoloTC == false)
                {
                    this.loadIntervalliDiLavoroEffettivi();
                    log = "this.Intervalli.Count: " + this.Intervalli.Count + "<br />";
                    if (this.Intervalli.Count > 0 && this.Status == 'F')
                    {
                        List<IntervalliDiLavoroEffettivi> conteggiati = new List<IntervalliDiLavoroEffettivi>();
                        this._Intervalli = this._Intervalli.OrderBy(x => x.Inizio).ToList();
                        for (int i = 0; i < this.Intervalli.Count; i++)
                        {
                            log += this.Intervalli[i].TaskID.ToString() + " " + this.Intervalli[i].user + " " +
                                this.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " "
                                + this.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " "
                                + this.Intervalli[i].Intervallo.TotalHours.ToString()
                                + "<br />";
                            // Verifico come si relaziona l'intervallo corrente con il precedente vagliato
                            if (conteggiati.Count > 0)
                            {
                                // Caso 1: contenuto in / sovrapposto
                                if (conteggiati[conteggiati.Count - 1].Inizio <= this.Intervalli[i].Inizio && conteggiati[conteggiati.Count - 1].Fine >= this.Intervalli[i].Fine)
                                {
                                    log += "if1: +0<br />";
                                    // Tralascio
                                }
                                // Caso 2: parzialmente compreso, "sborda a destra". Prendo la coda a destra!
                                else if (conteggiati[conteggiati.Count - 1].Inizio <= this.Intervalli[i].Inizio && this.Intervalli[i].Inizio <= conteggiati[conteggiati.Count - 1].Fine && this.Intervalli[i].Fine >= conteggiati[conteggiati.Count - 1].Fine)
                                {
                                    tc += (this.Intervalli[i].Fine - conteggiati[conteggiati.Count - 1].Fine);
                                    conteggiati.Add(this.Intervalli[i]);
                                    log += "if2 " + (this.Intervalli[i].Fine - conteggiati[conteggiati.Count - 1].Fine).TotalHours.ToString() + "<br />";
                                }
                                // Caso 3: fuori dai precedenti, tutto a destra
                                else if (this.Intervalli[i].Inizio >= conteggiati[conteggiati.Count - 1].Fine)
                                {
                                    tc += (this.Intervalli[i].Fine - this.Intervalli[i].Inizio);
                                    conteggiati.Add(this.Intervalli[i]);
                                    log += "if3 " + (this.Intervalli[i].Fine - this.Intervalli[i].Inizio).TotalHours.ToString() + "<br />";
                                }
                            }
                            else
                            {
                                // E' il primo, lo aggiungo diretto!
                                tc += (this.Intervalli[i].Fine - this.Intervalli[i].Inizio);
                                conteggiati.Add(this.Intervalli[i]);
                                log += "else " + (this.Intervalli[i].Fine - this.Intervalli[i].Inizio).TotalHours.ToString() + "<br />";
                            }
                        }
                    }
                }
                else
                {
                    // TO-DO! FUNZIONE DA TERMINARE!!!
                    // Considero i turni!

                    this.loadIntervalliDiLavoroEffettivi();
                    if (this.Intervalli.Count > 0 && this.Status == 'F')
                    {
                        log = "Considero i turni e ne rimuovo le pause dal conteggio<br />";
                        List<IntervalliDiLavoroEffettivi> conteggiati = new List<IntervalliDiLavoroEffettivi>();
                        List<DateTime[]> elenco = new List<DateTime[]>();
                        this._Intervalli = this._Intervalli.OrderBy(x => x.Inizio).ToList();
                        for (int i = 0; i < this.Intervalli.Count; i++)
                        {
                            log += this.Intervalli[i].TaskID.ToString() + " " + this.Intervalli[i].user + " " +
                                this.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " "
                                + this.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " "
                                + this.Intervalli[i].Intervallo.TotalHours.ToString()
                                + "<br />";
                            // Verifico come si relaziona l'intervallo corrente con il precedente vagliato
                            if (conteggiati.Count > 0)
                            {
                                DateTime[] interv = new DateTime[2];
                                // Caso 1: contenuto in / sovrapposto
                                if (conteggiati[conteggiati.Count - 1].Inizio <= this.Intervalli[i].Inizio && conteggiati[conteggiati.Count - 1].Fine >= this.Intervalli[i].Fine)
                                {
                                    log += "if1: +0<br />";
                                    // Tralascio
                                }
                                // Caso 2: parzialmente compreso, "sborda a destra". Prendo la coda a destra!
                                else if (conteggiati[conteggiati.Count - 1].Inizio <= this.Intervalli[i].Inizio && this.Intervalli[i].Inizio <= conteggiati[conteggiati.Count - 1].Fine && this.Intervalli[i].Fine >= conteggiati[conteggiati.Count - 1].Fine)
                                {
                                    tc += (this.Intervalli[i].Fine - conteggiati[conteggiati.Count - 1].Fine);
                                    conteggiati.Add(this.Intervalli[i]);
                                    log += "if2 " + (this.Intervalli[i].Fine - conteggiati[conteggiati.Count - 1].Fine).TotalHours.ToString() + "<br />";
                                    interv[0] = conteggiati[conteggiati.Count - 1].Fine;
                                    interv[1] = this.Intervalli[i].Fine;
                                    elenco.Add(interv);
                                }
                                // Caso 3: fuori dai precedenti, tutto a destra
                                else if (this.Intervalli[i].Inizio >= conteggiati[conteggiati.Count - 1].Fine)
                                {
                                    tc += (this.Intervalli[i].Fine - this.Intervalli[i].Inizio);
                                    conteggiati.Add(this.Intervalli[i]);
                                    log += "if3 " + (this.Intervalli[i].Fine - this.Intervalli[i].Inizio).TotalHours.ToString() + "<br />";
                                    interv[0] = this.Intervalli[i].Inizio;
                                    interv[1] = this.Intervalli[i].Fine;
                                    elenco.Add(interv);
                                }
                            }
                            else
                            {
                                // E' il primo, lo aggiungo diretto!
                                tc += (this.Intervalli[i].Fine - this.Intervalli[i].Inizio);
                                conteggiati.Add(this.Intervalli[i]);
                                log += "else " + (this.Intervalli[i].Fine - this.Intervalli[i].Inizio).TotalHours.ToString() + "<br />";
                                DateTime[] interv = new DateTime[2];
                                interv[0] = this.Intervalli[i].Inizio;
                                interv[1] = this.Intervalli[i].Fine;
                                elenco.Add(interv);

                            }
                        }

                        tc = new TimeSpan(0, 0, 0);

                        for (int i = 0; i < elenco.Count; i++)
                        {
                            log += elenco[i][0].ToString("dd/MM/yyyy HH:mm:ss") + " - " + elenco[i][1].ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                            rp.loadCalendario(elenco[i][0].AddDays(-7), elenco[i][1].AddDays(7));
                            for (int j = 0; j < rp.CalendarioRep.Intervalli.Count; j++)
                            {
                                log += rp.CalendarioRep.Intervalli[j].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " +
                                    rp.CalendarioRep.Intervalli[j].Fine.ToString("dd/MM/yyyy HH:mm:ss") + " ";
                                if (rp.CalendarioRep.Intervalli[j].Inizio > elenco[i][1])
                                {
                                    log += "if1<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Fine < elenco[i][0])
                                {
                                    log += "if2<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i][0] && rp.CalendarioRep.Intervalli[j].Fine <= elenco[i][1])
                                {
                                    tc += (rp.CalendarioRep.Intervalli[j].Fine - rp.CalendarioRep.Intervalli[j].Inizio);
                                    log += "if3 " + tc.TotalHours.ToString() + "<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i][0] && elenco[i][0] <= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i][1])
                                {
                                    tc += (elenco[i][1] - rp.CalendarioRep.Intervalli[j].Inizio);
                                    log += "if4 " + tc.TotalHours.ToString() + "<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i][0] && elenco[i][1] >= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i][0])
                                {
                                    tc += (rp.CalendarioRep.Intervalli[j].Fine - elenco[i][0]);
                                    log += "if5 " + tc.TotalHours.ToString() + "<br />";
                                }
                                else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i][0] && elenco[i][1] <= rp.CalendarioRep.Intervalli[j].Fine)
                                {
                                    tc += (elenco[i][1] - elenco[i][0]);
                                    log += "if6 " + tc.TotalHours.ToString() + "<br />";
                                }
                            }
                        }
                    }
                }
                return tc;
            }
        }

        public Double TempoCicloEffettivoDbl
        {
            get
            {
                return Math.Round(this.TempoCicloEffettivo.TotalHours, 2);
            }
        }

        public TimeSpan TempoCicloEffettivoUnitario
        {
            get
            {
                return new TimeSpan(this.TempoCicloEffettivo.Ticks / this.QuantitaProdotta);
            }
        }

        public Double TempoCicloEffettivoUnitarioDbl
        {
            get
            {
                return Math.Round(this.TempoCicloEffettivoUnitario.TotalHours, 2);
            }
        }

        public Boolean deleteSegnalazioneRitardo()
        {
            Boolean ret = false;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "DELETE FROM registroeventiproduzione WHERE taskID=@taskID"
                    + " AND TipoEvento LIKE 'Ritardo'";
                using (var tr = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(sql, new { taskID = this.TaskProduzioneID }, tr);
                        ret = true;
                        log = sql;
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        ret = false;
                        tr.Rollback();
                    }
                }
            }
            return ret;
        }

        public Boolean Riesuma()
        {
            Boolean res = false;
            if (this.TaskProduzioneID != -1 && this.Status == 'F')
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute("UPDATE registroeventitaskproduzione SET evento = 'P' WHERE "
                        + "task = @task"
                        + " AND evento = 'F'",
                        new { task = this.TaskProduzioneID }, tr);

                        conn.Execute("UPDATE tasksproduzione SET status='P' WHERE taskID = @taskID",
                            new { taskID = this.TaskProduzioneID }, tr);

                        Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                        if (art.Status == 'F')
                        {
                            conn.Execute("UPDATE productionplan SET status='I' WHERE id = @articoloID AND anno = @articoloAnno",
                                new { articoloID = this.ArticoloID, articoloAnno = this.ArticoloAnno }, tr);
                        }

                        tr.Commit();
                        res = true;
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        res = false;
                        tr.Rollback();
                    }
                }
                }
            }

            return res;
        }

        public void loadParameters()
        {
            this.Parameters = new List<TaskParameter>();
            if (this.TaskProduzioneID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT paramID FROM taskparameters WHERE taskID = @taskID";
                    var rows = conn.Query<int>(sql, new { taskID = this.TaskProduzioneID });
                    foreach (var id in rows)
                    {
                        this.Parameters.Add(new TaskParameter(this.Tenant, this.TaskProduzioneID, id));
                    }
                }
            }
        }

        public Boolean addParameter(String name, String description, ProductParametersCategory category, Boolean isFixed,
            Boolean isRequired, int inputpoint)
        {
            Boolean ret = false;
            if (this.TaskProduzioneID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    int maxID = 0;
                    int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(paramID) FROM taskparameters WHERE TaskID = @taskID", new { taskID = this.TaskProduzioneID });
                if (maxRow.HasValue)
                {
                    maxID = maxRow.Value + 1;
                }

                int maxSequence = maxID;

                string sql = "INSERT INTO taskparameters(TaskID, paramID, paramCategory, "
                    + " paramName, paramDescription, isFixed, isRequired, sequence, CreatedBy, CreatedDate) "
                    + "VALUES("
                    + "@taskID, "
                    + "@maxID, "
                    + "@categoryID, "
                    + "@name, @description, @isFixed, "
                    + "@isRequired, "
                    + "@maxSequence, ";
                if (inputpoint >= 0)
                {
                    sql += "@inputpoint, ";
                }
                else
                {
                    sql += "null, ";
                }
                sql += "@createdDate"
                    + ")";
                var prms = new Dictionary<string, object>();
                prms["taskID"] = this.TaskProduzioneID;
                prms["maxID"] = maxID;
                prms["categoryID"] = category.ID;
                prms["name"] = (object)(name ?? "");
                prms["description"] = (object)(description ?? "");
                prms["isFixed"] = isFixed;
                prms["isRequired"] = isRequired;
                prms["maxSequence"] = maxSequence;
                if (inputpoint >= 0)
                {
                    prms["inputpoint"] = inputpoint;
                }
                prms["createdDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                using (var tr = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(sql, prms, tr);
                        tr.Commit();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message + " " + sql;
                        ret = false;
                        tr.Rollback();
                    }
                }
                }
            }
            return ret;
        }

        public Boolean deleteParameter(int paramID, int CategoryID)
        {
            Boolean ret = false;
            if (this.TaskProduzioneID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "DELETE FROM taskparameters WHERE TaskID = @taskID"
                        + " AND paramID = @paramID AND paramCategory = @categoryID";
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute(sql, new { taskID = this.TaskProduzioneID, paramID = paramID, categoryID = CategoryID }, tr);
                            tr.Commit();
                            ret = true;
                        }
                        catch (Exception ex)
                        {
                            log = ex.Message + " " + sql;
                            ret = false;
                            tr.Rollback();
                        }
                    }
                }
            }
            return ret;
        }

        /*Returns:
         * true if all mandatory parameters have been set
         * false otherwise
         */
        public Boolean CheckParametersComplete()
        {
            this.log = "";
            Boolean ret = true;
            this.loadParameters();
            TaskVariante prcVar = this.OriginalTaskVariante;
            prcVar.loadParameters();
            this.log = "Param count: " + prcVar.Parameters.Count + "<br/>";
            if (prcVar.Parameters.Count > 0)
            {
                for (int i = 0; i < prcVar.Parameters.Count && ret; i++)
                {
                    this.log +=
                        prcVar.Parameters[i].Name + " "
                        + prcVar.Parameters[i].isRequired.ToString() + " ";
                    if (prcVar.Parameters[i].isRequired)
                    {
                        try
                        {
                            var lst = this.Parameters.First(x => x.Name == prcVar.Parameters[i].Name &&
                            x.ParameterCategory.ID == prcVar.Parameters[i].ParameterCategory.ID);
                            this.log += "Found";
                        }
                        catch
                        {
                            ret = false;
                            this.log += "Catch";
                        }
                        this.log += "<br />";
                    }
                }
            }
            this.log += ret.ToString();
            return ret;
        }

        /*Returns:
         * true if all non-fixed mandatory parameters have been set
         * false otherwise
         */
        public Boolean CheckNonFixedParametersComplete()
        {
            this.log = "";
            Boolean ret = true;
            this.loadParameters();
            TaskVariante prcVar = this.OriginalTaskVariante;
            prcVar.loadParameters();
            this.log = "Param count: " + prcVar.Parameters.Count + "<br/>";
            if (prcVar.Parameters.Count > 0)
            {
                for (int i = 0; i < prcVar.Parameters.Count && ret; i++)
                {
                    this.log +=
                        prcVar.Parameters[i].Name + " "
                        + prcVar.Parameters[i].isRequired.ToString() + " ";
                    if (prcVar.Parameters[i].isRequired && !prcVar.Parameters[i].isFixed)
                    {
                        try
                        {
                            var lst = this.Parameters.First(x => x.Name == prcVar.Parameters[i].Name &&
                            x.ParameterCategory.ID == prcVar.Parameters[i].ParameterCategory.ID);
                            this.log += "Found";
                        }
                        catch
                        {
                            ret = false;
                            this.log += "Catch";
                        }
                        this.log += "<br />";
                    }
                }
            }
            this.log += ret.ToString();
            return ret;
        }

        public String CopyFixedParameters()
        {
            this.log = "";
            this.loadParameters();
            TaskVariante prcVar = this.OriginalTaskVariante;
            prcVar.loadParameters();
            this.log = "Param count: " + prcVar.Parameters.Count + "<br/>";
            for (int i = 0; i < prcVar.Parameters.Count; i++)
            {
                this.log +=
                    prcVar.Parameters[i].Name + " "
                    + prcVar.Parameters[i].isFixed.ToString() + " ";
                if (prcVar.Parameters[i].isFixed)
                {
                    Boolean exists = false;
                    try
                    {
                        this.Parameters.First(x => x.Name == prcVar.Parameters[i].Name
                        && x.ParameterCategory.ID == prcVar.Parameters[i].ParameterCategory.ID);
                        exists = true;
                        this.log += "Found ";
                    }
                    catch
                    {
                        exists = false;
                        this.log += "Not found ";
                    }
                    if (!exists)
                    {
                        this.addParameter(prcVar.Parameters[i].Name,
                            prcVar.Parameters[i].Description,
                            prcVar.Parameters[i].ParameterCategory,
                            prcVar.Parameters[i].isFixed,
                            prcVar.Parameters[i].isRequired,
                            -1);
                    }
                    this.log += "<br />";
                }
            }
            return this.log;
        }


        // Checks if a delay event has been detected for the task, and it has been booked to send an e-mail to responsible person, or e-mail has just been sent
        public Boolean DelayDetected
        {
            get
            {
                Boolean ret = false;
                if (this.TaskProduzioneID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        string sql = "SELECT * FROM registroeventiproduzione WHERE TipoEvento LIKE 'Ritardo' and TaskID = @taskID";
                        var row = conn.QueryFirstOrDefault<object>(sql, new { taskID = this.TaskProduzioneID });
                        if (row != null)
                        {
                            ret = true;
                        }
                    }
                }
                return ret;
            }
        }

        public void loadWorkInstructionActive()
        {
            this.WorkInstructionActive = null;
            if (this.TaskProduzioneID != -1 && this.OriginalTask != -1 && this.OriginalTaskRevisione != -1 && this.VarianteID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT ManualID, ManualVersion FROM tasksmanuals WHERE taskid=@TaskID AND taskRev=@TaskVersion AND taskVarianti=@TaskVariant "
                        + " AND isActive=true AND expiryDate >= @expiryDate AND validityInitialDate <= @validityDate";
                    var row = conn.QueryFirstOrDefault<(int? ManualID, int? ManualVersion)>(sql, new { expiryDate = DateTime.UtcNow.ToString("yyyy-MM-dd"), validityDate = DateTime.UtcNow.ToString("yyyy-MM-dd"), TaskID = this.OriginalTask, TaskVersion = this.OriginalTaskRevisione, TaskVariant = this.VarianteID });
                    if (row.ManualID.HasValue && row.ManualVersion.HasValue)
                    {
                        this.WorkInstructionActive = new App_Sources.WorkInstructions.WorkInstruction(this.Tenant, row.ManualID.Value, row.ManualVersion.Value);
                    }
                }
            }
        }

        public void loadAssignedOperators()
        {
            this._AssignedOperators = new List<string>();
            if (this.TaskProduzioneID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT user FROM taskuser WHERE taskid=@TaskID";
                    var rows = conn.Query<String>(sql, new { TaskID = this.TaskProduzioneID });
                    foreach (var usr in rows)
                    {
                        this._AssignedOperators.Add(usr);
                    }
                }
            }
        }

        /*Returns:
         * 0 if generic error
         * 1 if ok
         */
        public int deleteAssignedOperator(String defOp)
        {
            int ret = 0;
            if (this.TaskProduzioneID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "DELETE FROM taskuser WHERE taskid=@TaskID AND user=@User";
                    conn.Execute(sql, new { TaskID = this.TaskProduzioneID, User = defOp });
                    ret = 1;
                }
            }
            return ret;
        }

        /*Returns:
         * 0 if generic error
         * 1 if ok
         */
        public int addAssignedOperator(String defOp)
        {
            int ret = 0;
            if (this.TaskProduzioneID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "INSERT INTO taskuser(TaskID, user, exclusive) VALUES(@TaskID, @User, @Exclusive)";
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute(sql, new { TaskID = this.TaskProduzioneID, User = defOp, Exclusive = false }, tr);
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                    ret = 1;
                }
            }
            return ret;
        }

        /* This function returns the foreseen end date by using start date (as input) and CycleTime (TempoC)
         * Returns:
         * 1970-1-1 if it is not possible to calculate the end date
         * Foreseen finish date at UTC time otherwise
         */
        public DateTime getEndDate(DateTime start)
        {
            DateTime ret = new DateTime(1970, 1, 1);
            if (this.TaskProduzioneID != -1)
            {
                Reparto rp = new Reparto(this.Tenant, this.RepartoID);
                if (rp != null && rp.id != -1 && TimeZoneInfo.ConvertTimeToUtc(start, rp.tzFusoOrario) > DateTime.UtcNow)
                {
                    TimeSpan minsevendays = new TimeSpan(7, 0, 0, 0);
                    rp.loadCalendario(start.AddDays(-7), TimeZoneInfo.ConvertTimeToUtc(start + this.TempoC + minsevendays).AddDays(this.TempoC.TotalDays * 5));
                    // Check if start is inside a shift
                    int insideshift = -1;
                    int closestshift = -1;
                    for (int i = 0; i < rp.CalendarioRep.Intervalli.Count && insideshift == -1; i++)

                    {
                        if (rp.CalendarioRep.Intervalli[i].Inizio <= start && start <= rp.CalendarioRep.Intervalli[i].Fine)
                        {
                            insideshift = i;
                        }
                        if (closestshift == -1 && start <= rp.CalendarioRep.Intervalli[i].Inizio)
                        {
                            closestshift = i;
                        }
                    }

                    if (insideshift == -1 && closestshift != -1)
                    {
                        start = rp.CalendarioRep.Intervalli[closestshift].Inizio;
                        insideshift = closestshift;
                    }

                    if (insideshift != -1)
                    {
                        TimeSpan progress = new TimeSpan(0, 0, 0);

                        //1st round
                        progress = rp.CalendarioRep.Intervalli[insideshift].Fine - start;
                        if (progress > this.TempoC)
                        {
                            ret = start + this.TempoC;
                        }
                        else
                        {
                            Boolean endloop = false;
                            for (int i = insideshift + 1; i < rp.CalendarioRep.Intervalli.Count && !endloop; i++)
                            {
                                TimeSpan oldProg = progress;
                                progress += rp.CalendarioRep.Intervalli[i].Fine - rp.CalendarioRep.Intervalli[i].Inizio;
                                if (progress > this.TempoC)
                                {
                                    //ret = TimeZoneInfo.ConvertTimeToUtc(rp.CalendarioRep.Intervalli[i].Inizio + (this.TempoC - oldProg), rp.tzFusoOrario);
                                    ret = rp.CalendarioRep.Intervalli[i].Inizio + (this.TempoC - oldProg);
                                    endloop = true;
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        public void logRescheduledTasks(int taskID, DateTime oldstart, DateTime oldend, TimeSpan oldworkingtime, String userid)
        {
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "INSERT INTO taskreschedulelog(task, rescheduledate, oldstart, oldend, oldworkingtime, userid) VALUES(@TaskID, @rescheduledate, "
                    + "@oldstart, @oldend, @oldworkingtime, @userid)";
                using (var tr = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(sql, new
                        {
                            TaskID = taskID,
                            rescheduledate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            oldstart = oldstart.ToString("yyyy-MM-dd HH:mm:ss"),
                            oldend = oldend.ToString("yyyy-MM-dd HH:mm:ss"),
                            oldworkingtime = (Math.Truncate(oldworkingtime.TotalHours)).ToString() + ":"
                                + oldworkingtime.Minutes.ToString() + ":" + oldworkingtime.Seconds.ToString(),
                            userid = userid
                        }, tr);
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        this.log = ex.Message;
                    }
                }
            }
        }

        /* Returns:
  * 0 if generic error
  * 1 if parameter added correctly
  * 2 if user not found
  * 3 if parameter already set
  * 4 if original parameter in product model does not exists
  * 5 if Error while adding parameter
  * 6 if task not found
  */
        public int CompileParameter(InputPoint usr, /*int ParamID,*/ int ParamCategory, String ParamName, String ParamValue)
        {

            int ret = 0;
            if (usr != null && usr.id >= 0)
            {
                if (this.TaskProduzioneID != -1)
                {
                    this.loadParameters();
                    Boolean exists = false;
                    try
                    {
                        this.Parameters.First(x => x.Name == ParamName
                        && x.ParameterCategory.ID == ParamCategory);
                        exists = true;
                        ret = 3;
                    }
                    catch
                    {
                        exists = false;
                    }

                    if (!exists)
                    {
                        Boolean existsOriginal = false;
                        TaskVariante origTsk = new TaskVariante(this.Tenant, new App_Code.processo(this.Tenant, this.OriginalTask,
                            this.OriginalTaskRevisione), new variante(this.Tenant, this.VarianteID));
                        origTsk.loadParameters();
                        //ret += "Parameters: " + origTsk.Parameters.Count + "<br />";
                        ModelTaskParameter origParam = null;
                        //ret += ParamName + "<br />";
                        try
                        {
                            origParam = origTsk.Parameters
                                    .First(x => x.Name == ParamName && x.ParameterCategory.ID == ParamCategory);
                            existsOriginal = true;
                        }
                        catch
                        {
                            existsOriginal = false;
                        }
                        if (existsOriginal && origParam != null && origParam.ParameterID != -1)
                        {
                            ret = 1;
                            Boolean checkAdd = this.addParameter(ParamName,
                                ParamValue,
                                new ProductParametersCategory(this.Tenant, ParamCategory),
                                origParam.isFixed,
                                origParam.isRequired,
                                usr.id);
                            ret = checkAdd ? 1 : 5;
                        }
                        else
                        {
                            ret = 4;
                        }
                    }
                    else
                    {
                        ret = 1;
                    }
                }
                else
                {
                    ret = 6;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }
        
        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         * 3 if task not found
         */
        public int RegisterTaskOperatorNote(String user, String note)
        {
            int ret = 0;
            if(this!=null && this.TaskProduzioneID!=-1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    // Check if a comment of the same user exists
                    Boolean exists = false;
                    var existing = conn.QueryFirstOrDefault<object>("SELECT * FROM tasksproduzioneoperatornotes WHERE taskid = @taskid"
                        + " AND user LIKE @user", new { taskid = this.TaskProduzioneID, user = user });
                    if (existing != null)
                    {
                        exists = true;
                    }

                    using (var tr = conn.BeginTransaction())
                    {
                        string sql;
                        if (exists)
                        {
                            sql = "UPDATE tasksproduzioneoperatornotes SET notes = @note WHERE "
                                + " taskid = @taskid AND user LIKE @user";
                            conn.Execute(sql, new { note = (object)(note ?? ""), taskid = this.TaskProduzioneID, user = user }, tr);
                        }
                        else
                        {
                            int maxID = 0;
                            int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(CommentID) FROM tasksproduzioneoperatornotes WHERE taskid=@taskid", new { taskid = this.TaskProduzioneID }, tr);
                            if (maxRow.HasValue)
                            {
                                maxID = maxRow.Value + 1;
                            }
                            sql = "INSERT INTO tasksproduzioneoperatornotes(taskid, commentid, date, user, notes) VALUES(@TaskID, @CommentID, @Date, @User, @Notes)";
                            conn.Execute(sql, new
                            {
                                TaskID = this.TaskProduzioneID,
                                CommentID = maxID,
                                Date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                                User = user,
                                Notes = (object)(note ?? "")
                            }, tr);
                        }
                        try
                        {
                            tr.Commit();
                            ret = 1;
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                            tr.Rollback();
                            ret = 4;
                        }
                    }
                }
            }
            else
            {
                ret = 3;
            }
            return ret;
        }

        public void loadTaskOperatorNotes()
        {
            this.TaskOperatorNotes = new List<TaskOperatorNote>();
            if(this.TaskProduzioneID!=-1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT CommentID FROM tasksproduzioneoperatornotes WHERE TaskID=@TaskID ORDER BY date DESC";
                    var rows = conn.Query<int>(sql, new { TaskID = this.TaskProduzioneID });
                    foreach (var id in rows)
                    {
                        this.TaskOperatorNotes.Add(new TaskOperatorNote(this.Tenant, this.TaskProduzioneID, id));
                    }
                }
            }
        }
    }
    

    public class ProductionSchedule
    {
        public String Tenant;

        public String log;

        public List<ProductionOrderStruct> ScheduledProducts;

        private int _RepartoID;
        public int RepartoID
        {
            get { return this._RepartoID; }
        }

        private List<prodotto> _ElencoCommesse;
        public List<prodotto> ElencoCommesse
        {
            get { return this._ElencoCommesse; }
        }

        public ProductionSchedule(String Tenant, Reparto rp)
        {
            this.Tenant = Tenant;

            if (rp.id != -1)
            {
                this._RepartoID = rp.id;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT matricola, processo, revisione, variante FROM ProductionPlan WHERE reparto = @reparto";
                    var rows = conn.Query<(String Matricola, int Processo, int Revisione, int Variante)>(sql, new { reparto = rp.id });
                    _ElencoCommesse = new List<prodotto>();
                    foreach (var row in rows)
                    {
                        processo prc = new processo(this.Tenant, row.Processo, row.Revisione);
                        variante vr = new variante(this.Tenant, row.Variante);
                        ProcessoVariante prvr = new ProcessoVariante(this.Tenant, prc, vr);
                        prvr.loadReparto();
                        prvr.process.loadFigli(prvr.variant);
                        if (prvr.process != null && prvr.variant != null)
                        {
                            this._ElencoCommesse.Add(new prodotto(this.Tenant, row.Matricola, prvr));
                        }
                    }
                }

            }
        }

        public bool addProduct(String matricola, ProcessoVariante vr)
        {
            bool rt = false;
            if (this.RepartoID != -1 && matricola.Length > 0 && vr.process != null && vr.variant != null)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                
                        // Ricerco la disponibilità della postazione per il primo processo del percorso critico per capire quando far partire il prodotto
                        vr.process.calculateCriticalPath(vr.variant);

                        RepartoProcessoPostazione p1 = new RepartoProcessoPostazione(this.Tenant, this.RepartoID, new TaskVariante(this.Tenant, vr.process.CriticalPath[0], vr.variant));
                        Postazione pst = p1.Pst;
                        /*DateTime inizio = pst.trovaDisponibilita(new Reparto(RepartoID));
                        err += pst.err;
                        cmd.CommandText = "INSERT INTO productionplan(processo, revisione, variante, matricola, status, reparto, startTime) VALUES("
                            + vr.process.processID.ToString() + ", " + vr.process.revisione.ToString() + ", "
                            + vr.variant.idVariante.ToString() + ", '" + matricola + "', "
                        + "'N', " + this.RepartoID.ToString() + ", '" + inizio.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        try
                        {
                            cmd.ExecuteNonQuery();
                            rt = true;
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            rt = false;
                        }*/
                        if (rt == true)
                        {
                        }
                    }
                }
            }
            return rt;
        }

        public ProductionSchedule(String Tenant)
        {
            this.Tenant = Tenant;
            this.ScheduledProducts = new List<ProductionOrderStruct>();
        }

        public void loadProductionSchedule()
        {
            this.ScheduledProducts = new List<ProductionOrderStruct>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT anagraficaclienti.codice AS CustomerID, " //1
                + " anagraficaclienti.ragsociale AS CustomerName,"//2
                + " anagraficaclienti.partitaiva AS CustomerVATNumber,"//3
                + " anagraficaclienti.codfiscale AS CustomerCodiceFiscale,"//4
+ " anagraficaclienti.indirizzo AS CustomerAddress,"                //5
+ " anagraficaclienti.citta AS CustomerCity,"
+ " anagraficaclienti.provincia AS CustomerProvince,"
+ " anagraficaclienti.CAP AS CustomerZipCode,"
+ " anagraficaclienti.stato AS CustomerCountry,"
+ " anagraficaclienti.telefono AS CustomerPhoneNumber,"
+ " anagraficaclienti.email AS CustomerEMail,"
+ " anagraficaclienti.kanbanmanaged AS CustomerKanbanManaged,"
+ " commesse.idcommesse AS SalesOrderID,"
+ " commesse.anno AS SalesOrderYear,"
+ " commesse.cliente AS SalesOrderCustomer,"
+ " commesse.dataInserimento AS SalesOrderDate,"
+ " commesse.note AS SalesOrderNotes,"
+ " productionplan.id AS ProductionOrderID,"
+ " productionplan.anno AS ProductionOrderYear,"
+ " productionplan.processo AS ProductionOrderProductTypeID,"
+ " productionplan.revisione AS ProductionOrderProductTypeReview,"
+ " productionplan.variante AS ProductionOrderProductID,"
+ " productionplan.matricola AS ProductionOrderSerialNumber,"
+ " productionplan.status AS ProductionOrderStatus,"
+ " productionplan.reparto AS ProductionOrderDepartmentID,"
+ " productionplan.startTime AS ProductionOrderStartTime,"
+ " productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,"
+ " productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,"
+ " productionplan.planner AS ProductionOrderPlanner,"
+ " productionplan.quantita AS ProductionOrderQuantityOrdered,"         // 30
+ " productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,"
+ " productionplan.kanbanCard AS ProductionOrderKanbanCardID,"
+ " processo.processID AS ProductTypeID,"
+ " processo.revisione AS ProductTypeReview,"
+ " processo.dataRevisione AS ProductTypeReviewDate,"
+ " processo.Name AS ProductTypeName,"
+ " processo.description AS ProductTypeDescription,"
+ " processo.attivo AS ProductTypeEnabled,"
+ " varianti.idvariante AS ProductID,"
+ " varianti.nomeVariante AS ProductName,"
+ " varianti.descVariante AS ProductDescription,"
+ " reparti.idreparto AS DepartmentID,"
+ " reparti.nome AS DepartmentName,"
+ " reparti.descrizione AS DepartmentDescription,"
+ " reparti.cadenza AS DepartmentTaktTime,"
+ " reparti.timezone AS DepartmentTimeZone, "
+ " productionplan.leadtime AS RealLeadTime, "
+ " productionplan.WorkingTime AS RealWorkingTime, "
+ " productionplan.Delay AS RealDelay, "
+ " productionplan.EndProductionDateReal AS RealEndProductionDate, "
+ " commesse.ExternalID AS SalesOrderExternalID, "
+ " variantiprocessi.ExternalID AS ProductExternalID, "
 + " measurementunits.type AS MeasurementUnit"
+ " FROM anagraficaclienti INNER JOIN commesse ON (anagraficaclienti.codice = commesse.cliente) INNER JOIN"
 + " productionplan ON(commesse.anno =productionplan.annoCommessa AND commesse.idcommesse = productionplan.commessa)"
 + "INNER JOIN variantiprocessi ON (productionplan.variante = variantiprocessi.variante AND productionplan.processo = variantiprocessi.processo AND productionplan.revisione = variantiprocessi.revProc)"
 + " INNER JOIN varianti ON (varianti.idvariante = variantiprocessi.variante)"
 + " INNER JOIN processo ON (processo.ProcessID = variantiprocessi.processo AND processo.revisione = variantiprocessi.revProc) "
+ " INNER JOIN measurementunits ON(variantiprocessi.measurementUnit = measurementunits.id)"
+ " LEFT JOIN reparti ON(reparti.idreparto = productionplan.reparto)"
 + " WHERE productionplan.status <> 'F'"
 + " order by productionplan.dataprevistafineproduzione";
            var rows = conn.Query<ProductionOrderRow>(sql).ToList();
            FusoOrario fuso = new FusoOrario(this.Tenant);
            foreach (var row in rows)
            {
                ProductionOrderStruct curr = new ProductionOrderStruct();
                KIS.App_Code.Reparto rp = null;
                if (row.DepartmentID.HasValue)
                { 
                    curr.DepartmentID = row.DepartmentID.Value;
                    rp = new App_Code.Reparto(this.Tenant, curr.DepartmentID);
                }
                else
                {
                    curr.DepartmentID = -1;
                }
                if (row.CustomerID != null)
                {
                    curr.CustomerID = row.CustomerID;
                }
                if (row.CustomerName != null)
                {
                    curr.CustomerName = row.CustomerName;
                }
                if (row.CustomerVATNumber != null)
                {
                    curr.CustomerVATNumber = row.CustomerVATNumber;
                }
                if (row.CustomerCodiceFiscale != null)
                {
                    curr.CustomerCodiceFiscale = row.CustomerCodiceFiscale;
                }
                if (row.CustomerAddress != null)
                {
                    curr.CustomerAddress = row.CustomerAddress;
                }
                if (row.CustomerCity != null)
                {
                    curr.CustomerCity = row.CustomerCity;
                }
                if (row.CustomerProvince != null)
                {
                    curr.CustomerProvince = row.CustomerProvince;
                }
                if (row.CustomerZipCode != null)
                {
                    curr.CustomerZipCode = row.CustomerZipCode;
                }
                if (row.CustomerCountry != null)
                {
                    curr.CustomerCountry = row.CustomerCountry;
                }
                if (row.CustomerPhoneNumber != null)
                {
                    curr.CustomerPhoneNumber = row.CustomerPhoneNumber;
                }
                if (row.CustomerEMail != null)
                {
                    curr.CustomerEMail = row.CustomerEMail;
                }
                if (row.CustomerKanbanManaged.HasValue)
                {
                    curr.CustomerKanbanManaged = row.CustomerKanbanManaged.Value;
                }
                if (row.SalesOrderID.HasValue)
                {
                    curr.SalesOrderID = row.SalesOrderID.Value;
                }
                if (row.SalesOrderYear.HasValue)
                {
                    curr.SalesOrderYear = row.SalesOrderYear.Value;
                }
                if (row.SalesOrderCustomer != null)
                {
                    curr.SalesOrderCustomer = row.SalesOrderCustomer;
                }
                if (row.SalesOrderDate.HasValue)
                {
                    if(rp!=null)
                    { 
                    curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(row.SalesOrderDate.Value, rp.tzFusoOrario);
                    }
                    else
                    {
                        curr.SalesOrderDate = TimeZoneInfo.ConvertTimeFromUtc(row.SalesOrderDate.Value, fuso.tzFusoOrario);
                    }
                }
                if (row.SalesOrderNotes != null)
                {
                    curr.SalesOrderNotes = row.SalesOrderNotes;
                }
                if (row.ProductionOrderID.HasValue)
                {
                    curr.ProductionOrderID = row.ProductionOrderID.Value;
                }
                if (row.ProductionOrderYear.HasValue)
                {
                    curr.ProductionOrderYear = row.ProductionOrderYear.Value;
                }
                if (row.ProductionOrderProductTypeID.HasValue)
                {
                    curr.ProductionOrderProductTypeID = row.ProductionOrderProductTypeID.Value;
                }
                if (row.ProductionOrderProductTypeReview.HasValue)
                {
                    curr.ProductionOrderProductTypeReview = row.ProductionOrderProductTypeReview.Value;
                }
                if (row.ProductionOrderProductID.HasValue)
                {
                    curr.ProductionOrderProductID = row.ProductionOrderProductID.Value;
                }
                if (row.ProductionOrderSerialNumber != null)
                {
                    curr.ProductionOrderSerialNumber = row.ProductionOrderSerialNumber;
                }
                if (row.ProductionOrderStatus != null)
                {
                    curr.ProductionOrderStatus = row.ProductionOrderStatus[0];
                }
                if (row.ProductionOrderDepartmentID.HasValue)
                {
                    curr.ProductionOrderDepartmentID = row.ProductionOrderDepartmentID.Value;
                }
                if (row.ProductionOrderStartTime.HasValue)
                {
                    if(rp!=null)
                    { 
                    curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderStartTime.Value, rp.tzFusoOrario);
                    }
                    else
                    {
                        curr.ProductionOrderStartTime = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderStartTime.Value, fuso.tzFusoOrario);
                    }
                }
                if (row.ProductionOrderDeliveryDate.HasValue)
                {
                    if (rp != null)
                    {
                        curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderDeliveryDate.Value, rp.tzFusoOrario);
                    }
                    else
                    {
                        curr.ProductionOrderDeliveryDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderDeliveryDate.Value, fuso.tzFusoOrario);
                    }
                }
                if (row.ProductionOrderEndProductionDate.HasValue)
                {
                    if(rp!=null)
                    { 
                        curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderEndProductionDate.Value, rp.tzFusoOrario);
                    }
                    else
                    {
                        curr.ProductionOrderEndProductionDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductionOrderEndProductionDate.Value, fuso.tzFusoOrario);
                    }
                }
                if (row.ProductionOrderPlanner != null)
                {
                    curr.ProductionOrderPlanner = row.ProductionOrderPlanner;
                }
                if (row.ProductionOrderQuantityOrdered.HasValue)
                {
                    curr.ProductionOrderQuantityOrdered = row.ProductionOrderQuantityOrdered.Value;
                }
                if (row.ProductionOrderQuantityProduced.HasValue)
                {
                    curr.ProductionOrderQuantityProduced = row.ProductionOrderQuantityProduced.Value;
                }
                if (row.ProductionOrderKanbanCardID != null)
                {
                    curr.ProductionOrderKanbanCardID = row.ProductionOrderKanbanCardID;
                }
                if (row.ProductTypeID.HasValue)
                {
                    curr.ProductTypeID = row.ProductTypeID.Value;
                }
                if (row.ProductTypeReview.HasValue)
                {
                    curr.ProductTypeReview = row.ProductTypeReview.Value;
                }
                if (row.ProductTypeReviewDate.HasValue)
                {
                    if(rp!=null)
                    { 
                    curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductTypeReviewDate.Value, rp.tzFusoOrario);
                    }
                    else
                    {
                        curr.ProductTypeReviewDate = TimeZoneInfo.ConvertTimeFromUtc(row.ProductTypeReviewDate.Value, fuso.tzFusoOrario);
                    }
                }
                if (row.ProductTypeName != null)
                {
                    curr.ProductTypeName = row.ProductTypeName;
                }
                if (row.ProductTypeDescription != null)
                {
                    curr.ProductTypeDescription = row.ProductTypeDescription;
                }
                if (row.ProductTypeEnabled.HasValue)
                {
                    curr.ProductTypeEnabled = row.ProductTypeEnabled.Value;
                }
                if (row.ProductID.HasValue)
                {
                    curr.ProductID = row.ProductID.Value;
                }
                if (row.ProductName != null)
                {
                    curr.ProductName = row.ProductName;
                }
                if (row.ProductDescription != null)
                {
                    curr.ProductDescription = row.ProductDescription;
                }
                if (row.DepartmentID.HasValue)
                {
                    curr.DepartmentID = row.DepartmentID.Value;
                }
                if (row.DepartmentName != null)
                {
                    curr.DepartmentName = row.DepartmentName;
                }
                if (row.DepartmentDescription != null)
                {
                    curr.DepartmentDescription = row.DepartmentDescription;
                }
                if (row.DepartmentTaktTime.HasValue)
                {
                    curr.DepartmentTaktTime = row.DepartmentTaktTime.Value;
                }
                if (row.DepartmentTimeZone != null)
                {
                    curr.DepartmentTimeZone = row.DepartmentTimeZone;
                }
                if (row.RealLeadTime.HasValue)
                {
                    curr.RealLeadTime = row.RealLeadTime.Value;
                }
                if (row.RealWorkingTime.HasValue)
                {
                    curr.RealWorkingTime = row.RealWorkingTime.Value;
                }
                if (row.RealDelay.HasValue)
                {
                    curr.RealDelay = row.RealDelay.Value;
                }
                if (row.ProductionOrderEndProductionDateReal.HasValue)
                {
                    curr.ProductionOrderEndProductionDateReal = row.ProductionOrderEndProductionDateReal.Value;
                }
                if (row.SalesOrderExternalID != null)
                {
                    curr.SalesOrderExternalID = row.SalesOrderExternalID;
                }
                if (row.ProductExternalID != null)
                {
                    curr.ProductExternalID = row.ProductExternalID;
                }
                if (row.MeasurementUnit != null)
                {
                    curr.MeasurementUnit = row.MeasurementUnit;
                }
                this.ScheduledProducts.Add(curr);
            }
            }
        }
    }
    
    public class TaskConfigurato
    {
        public String Tenant;

        public String log;

        private TaskVariante _Task;
        public TaskVariante Task
        {
            get { return this._Task; }
        }

        private int _Quantita;
        public int Quantita
        {
            get { return this._Quantita; }
        }

        private TempoCiclo _Tempo;
        public TempoCiclo Tempo
        {
            get { return this._Tempo; }
            set { this._Tempo = value; }
        }

        private TimeSpan _TempoTotale;
        public TimeSpan TempoTotale
        {
            get
            {
                return this.Tempo.TempoSetup + TimeSpan.FromTicks(this.Quantita * this.Tempo.Tempo.Ticks) + this.Tempo.TempoUnload;
            }
        }

        public List<int> Precedenti;
        public List<int> Successivi;
        public List<TimeSpan> PrecedentiPausa;
        public List<TimeSpan> SuccessiviPausa;
        public List<int> PrecedentiConstraintType;
        public List<int> SuccessiviConstraintType;

        private Postazione _PostazioneDiLavoro;
        public Postazione PostazioneDiLavoro
        {
            get { return this._PostazioneDiLavoro; }
        }

        private TimeSpan _earlyStartTime;
        public TimeSpan EarlyStartTime
        {
            get { return this._earlyStartTime; }
            set { this._earlyStartTime = value; }
        }

        private TimeSpan _earlyFinishTime;
        public TimeSpan EarlyFinishTime
        {
            get { return this._earlyFinishTime; }
            set { this._earlyFinishTime = value; }
        }

        private TimeSpan _lateStartTime;
        public TimeSpan LateStartTime
        {
            get
            {
                return this._lateStartTime;
            }
            set { this._lateStartTime = value; }
        }

        private TimeSpan _lateFinishTime;
        public TimeSpan LateFinishTime
        {
            get { return this._lateFinishTime; }
            set { this._lateFinishTime = value; }
        }

        private int _RepartoID;
        public int RepartoID
        {
            get { return this._RepartoID; }
        }

        public Reparto rp;

        /* Date per EarlyStart, LateStart, EarlyFinish, LateFinish */
        private DateTime _EarlyStartDate;
        public DateTime EarlyStartDate
        {
            get {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyStartDate, rp.tzFusoOrario); }
            set {
                if (this.rp == null || rp.id == -1)
                {
                    this.rp = new Reparto(this.Tenant, this.RepartoID);
                }
                this._EarlyStartDate = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario); }
        }
        private DateTime _LateStartDate;
        public DateTime LateStartDate
        {
            get {
                if (this.rp == null || rp.id == -1)
                {
                    rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateStartDate, rp.tzFusoOrario);
            }
            set {
                if (this.rp == null || rp.id == -1)
                {
                    rp = new Reparto(this.Tenant, this.RepartoID);
                }
                this._LateStartDate = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
            }
        }
        private DateTime _EarlyFinishDate;
        public DateTime EarlyFinishDate
        {
            get {
                if (this.rp == null || rp.id == -1)
                {
                    rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyFinishDate, rp.tzFusoOrario);
            }
            set {
                if (this.rp == null || rp.id == -1)
                {
                    rp = new Reparto(this.Tenant, this.RepartoID);
                }
                this._EarlyFinishDate = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
            }
        }
        private DateTime _LateFinishDate;
        public DateTime LateFinishDate
        {
            get {
                if (this.rp == null || rp.id == -1)
                {
                    rp = new Reparto(this.Tenant, this.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateFinishDate, rp.tzFusoOrario); }
            set {
                if (this.rp == null || rp.id == -1)
                {
                    rp = new Reparto(this.Tenant, this.RepartoID);
                }
                this._LateFinishDate = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
            }
        }
        private bool _InseribileInCalendario;
        public bool InseribileInCalendario
        {
            get { return this._InseribileInCalendario; }
            set { this._InseribileInCalendario = value; }
        }

        public TaskConfigurato(String Tenant, TaskVariante tsk, TempoCiclo tc, int repID, int qty)
        {
            this.Tenant = Tenant;
            Precedenti = new List<int>();
            Successivi = new List<int>();
            PrecedentiConstraintType = new List<int>();
            SuccessiviConstraintType = new List<int>();
            this.PrecedentiPausa = new List<TimeSpan>();
            this.SuccessiviPausa = new List<TimeSpan>();
            tsk.loadTempiCiclo();
            Reparto rp = new Reparto(this.Tenant, repID);
            if (tc.IdProcesso == tsk.Task.processID && tc.RevisioneProcesso == tsk.Task.revisione && tsk.variant.idVariante == tc.Variante && rp.id != -1)
            {
                this._RepartoID = repID;
                this._Task = tsk;
                this.Tempo = tc;
                this._Quantita = qty;

                tsk.Task.loadPrecedenti(tsk.variant);                
                for (int i = 0; i < tsk.Task.processiPrec.Count; i++)
                {
                    Precedenti.Add(tsk.Task.processiPrec[i]);
                    this.PrecedentiPausa.Add(tsk.Task.pausePrec[i]);
                    this.PrecedentiConstraintType.Add(tsk.Task.ConstraintType[i]);
                }
                tsk.Task.loadSuccessivi(tsk.variant);
                for (int i = 0; i < tsk.Task.processiSucc.Count; i++)
                {
                    Successivi.Add(tsk.Task.processiSucc[i]);
                    this.SuccessiviPausa.Add(tsk.Task.pauseSucc[i]);
                    this.SuccessiviConstraintType.Add(tsk.Task.ConstraintType[i]);
                }

                //Carico la postazione di lavoro
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT postazione FROM repartipostazioniattivita WHERE processo = @processo"
                     + " AND revProc = @revProc AND variante = @variante"
                     + " AND reparto = @reparto";
                    int? postazione = conn.QueryFirstOrDefault<int?>(sql, new
                    {
                        processo = this.Task.Task.processID,
                        revProc = this.Task.Task.revisione,
                        variante = this.Task.variant.idVariante,
                        reparto = repID
                    });
                    if (postazione.HasValue)
                    {
                        this._PostazioneDiLavoro = new Postazione(this.Tenant, postazione.Value);
                    }
                    else
                    {
                        this._PostazioneDiLavoro = null;
                    }
                }
            }
            else
            {
                this._Task = null;
                this.Tempo = null;
            }
        }

    }

    public class ConfigurazioneProcesso
    {
        public String Tenant;

        public String log;

        public List<TaskConfigurato> Processi;

        private int _IDArticolo;
        public int ArticoloID
        {
            get { return this._IDArticolo; }
        }
        private int _ArticoloAnno;
        public int ArticoloAnno
        {
            get { return this._ArticoloAnno; }
        }

        public ProcessoVariante MainProcess;

        private Reparto _RepartoProduttivo;
        public Reparto RepartoProduttivo
        {
            get { return this._RepartoProduttivo; }
        }

        private int _Quantita;
        public int Quantita
        {
            get { return this._Quantita; }
        }

        public ConfigurazioneProcesso(String Tenant, Articolo art, List<TaskConfigurato> lst, Reparto rp, int qty)
        {
            this.Tenant = Tenant;
            this._IDArticolo = art.ID;
            this._ArticoloAnno = art.Year;
            this._RepartoProduttivo = rp;
            this._Quantita = qty;
            // Carico i figli
            // Verifico che tutti i task in lst appartengano ai figli del processo padre
            bool check = true;
            for (int i = 0; i < lst.Count && check == true; i++)
            {
                check = this.checkAppartenenzaFigli(art.Proc, lst[i]);
            }

            Processi = new List<TaskConfigurato>();
            if (check == true)
            {
                MainProcess = art.Proc;
                this._RepartoProduttivo = rp;
                for (int i = 0; i < lst.Count; i++)
                {
                    Processi.Add(new TaskConfigurato(this.Tenant, lst[i].Task, lst[i].Tempo, rp.id, this._Quantita));
                }
            }
        }

        protected bool checkAppartenenzaFigli(ProcessoVariante prc, TaskConfigurato tsk)
        {
            bool rt = false;
            prc.process.loadFigli(prc.variant);
            for (int j = 0; j < prc.process.subProcessi.Count && rt == false; j++)
            {
                if (tsk!=null && tsk.Task!=null && tsk.Task.Task!=null && tsk.Task.Task.processID == prc.process.subProcessi[j].processID && tsk.Task.Task.revisione == prc.process.subProcessi[j].revisione)
                {
                    rt = true;
                }
            }
            return rt;
        }

        public List<TaskConfigurato> CriticalPath;

        /* Returns:
         * 0 if generic error
         * 1 if all is fine
         * 2 if there is some task without link between other tasks
         * 3 if diagram type is NOT Pert
         * 5 if some subtask is missing Kpi called "Tempo ciclo"
         * 6 if some subtask is missing la postazione
         */
        public int checkConsistency()
        {
            int rt = 1;

            if (this.MainProcess.process.processID != -1)
            {
                // TO-DO: controllare che tutti i task abbiano almeno un precedente o un successivo!
                if (this.Processi.Count != 1)
                {
                    for (int i = 0; i < this.Processi.Count; i++)
                    {
                        if (this.Processi[i].Precedenti.Count == 0 && this.Processi[i].Successivi.Count == 0)
                        {
                            rt = 2;
                        }
                    }
                }

                // Controllo che il task sia effettivamente un PERT
                if (this.MainProcess.process.isVSM == true)
                {
                    rt = 3;
                }
                // Controllo che tutti i subtasks abbiano impostato il tempo ciclo
                if (rt == 1)
                {
                    for (int i = 0; i < this.Processi.Count && rt == 1; i++)
                    {
                        if (this.Processi[i].Tempo == null)
                        {
                            rt = 5;
                        }
                    }
                }

                // Controllo che tutti i subtasks siano assegnati ad almeno una postazione
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    if (this.Processi[i].PostazioneDiLavoro == null)
                    {
                        rt = 6;
                    }
                }

            }
            else
            {
                rt = 0;
            }
            return rt;
        }

        public void calculateCriticalPath()
        {
            if (this.checkConsistency() == 1)
            {
                // Ricerco i task capostipite e calcolo il loro earlyStartTime ed earlyFinishTime
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    if (this.Processi[i].Precedenti.Count == 0)
                    {
                        //this.subProcessi[i].loadKPIs();
                        this.Processi[i].EarlyStartTime = new TimeSpan(0, 0, 0);
                        this.Processi[i].EarlyFinishTime = this.Processi[i].TempoTotale;
                        // Ora calcolo earlyStartTime e earlyFinishTime per i loro successivi, fino alla fine!!!
                        this.calculateEarlyTimesforSucc(this.Processi[i].Task.Task.processID, this.Processi[i].EarlyFinishTime);
                    }
                }

                // Ricerco i task finali e calcolo il loro lateStartTime e lateFinishTime
                // Per farlo devo trovare il task finale con il massimo earlyFinishTime e impostare lateFinishTime = max(earlyFinishTime)

                TimeSpan maxEarlyFinishTime = new TimeSpan(0, 0, 0);
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    if (this.Processi[i].Successivi.Count == 0)
                    {
                        if (this.Processi[i].EarlyFinishTime > maxEarlyFinishTime)
                        {
                            maxEarlyFinishTime = this.Processi[i].EarlyFinishTime;
                        }
                    }
                }

                // Inoltre inizializzo tutti quanti i task a maxEarlyFinishTime + 1
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    this.Processi[i].LateFinishTime = maxEarlyFinishTime.Add(new TimeSpan(0, 0, 1));
                }

                // Ora calcolo il lateFinishTime e il lateStartTime per i processi finali!
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    if (this.Processi[i].Successivi.Count == 0)
                    {
                        this.Processi[i].LateFinishTime = maxEarlyFinishTime;
                        this.Processi[i].LateStartTime = this.Processi[i].LateFinishTime - this.Processi[i].TempoTotale;
                        calculateLateTimesforPrec(this.Processi[i].Task.Task.processID, this.Processi[i].LateStartTime);
                    }
                }

                // Ora posso raccogliere le attività critiche
                // Condizione: dove data minima inizio == data massima inizio && data minima fine == data massima fine
                int numProcCritical = 0;
                CriticalPath = new List<TaskConfigurato>();
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    if (this.Processi[i].EarlyStartTime == this.Processi[i].LateStartTime && this.Processi[i].EarlyFinishTime == this.Processi[i].LateFinishTime)
                    {
                        CriticalPath.Add(this.Processi[i]);
                        numProcCritical++;
                    }
                }

                // Ordino l'array per "LateStartTime"
                for (int j = 0; j < CriticalPath.Count - 1; j++)
                {
                    TaskConfigurato temp;
                    int pos_min = j;
                    for (int i = j + 1; i < CriticalPath.Count; i++)
                    {
                        if (CriticalPath[pos_min].LateStartTime > CriticalPath[i].LateStartTime)
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

        private void calculateEarlyTimesforSucc(int prcId, TimeSpan finish)
        {
            int procIndex = -1;
            for (int i = 0; i < this.Processi.Count; i++)
            {
                if (this.Processi[i].Task.Task.processID == prcId)
                {
                    procIndex = i;
                }
            }


            for (int i = 0; i < this.Processi[procIndex].Successivi.Count; i++)
            {
                // Cerco l'indice
                int index = -1;
                for (int j = 0; j < this.Processi.Count; j++)
                {
                    if (this.Processi[procIndex].Successivi[i] == this.Processi[j].Task.Task.processID)
                    {
                        index = j;
                    }
                }

                // Ora andiamo a calcolare earlyStart e earlyFinish
                //this.subProcessi[index].loadKPIs();
                if (this.Processi[index].EarlyStartTime < finish + this.Processi[procIndex].SuccessiviPausa[i])
                {
                    //this.Processi[index].EarlyStartTime = finish;
                    this.Processi[index].EarlyStartTime = finish+this.Processi[procIndex].SuccessiviPausa[i];
                }
                if (this.Processi[index].EarlyFinishTime < finish + this.Processi[index].TempoTotale + this.Processi[procIndex].SuccessiviPausa[i])
                {
                    this.Processi[index].EarlyFinishTime = finish + this.Processi[index].TempoTotale +this.Processi[procIndex].SuccessiviPausa[i];
                }
                calculateEarlyTimesforSucc(this.Processi[index].Task.Task.processID, this.Processi[index].EarlyFinishTime);

            }
        }

        private void calculateLateTimesforPrec(int prcId, TimeSpan start)
        {
            int procIndex = -1;
            for (int i = 0; i < this.Processi.Count; i++)
            {
                if (this.Processi[i].Task.Task.processID == prcId)
                {
                    procIndex = i;
                }
            }

            //this.Processi[procIndex].loadPrecedenti();
            for (int i = 0; i < this.Processi[procIndex].Precedenti.Count; i++)
            {
                // Trovo l'indice del precedente
                int index = -1;
                for (int j = 0; j < this.Processi.Count; j++)
                {
                    if (this.Processi[j].Task.Task.processID == this.Processi[procIndex].Precedenti[i])
                    {
                        index = j;
                    }
                }

                // Aggiunte queste 2 righe per le pause tra tasks
                //this.Processi[procIndex].Task.Task.loadPrecedenti();
                //int indPrec = this.Processi[procIndex].Task.Task.processiPrec.FindIndex(c => c == this.Processi[procIndex].Precedenti[i]);

                if (index != -1)
                {
                    //this.Processi[index].loadKPIs();
                    if (this.Processi[index].LateFinishTime > start)
                    {
                        //this.Processi[index].LateFinishTime = start; 
                        this.Processi[index].LateFinishTime = start - this.Processi[procIndex].PrecedentiPausa[i];
                    }
                    this.Processi[index].LateStartTime = this.Processi[index].LateFinishTime - this.Processi[index].TempoTotale;// - this.Processi[procIndex].PrecedentiPausa[i];
                    calculateLateTimesforPrec(this.Processi[index].Task.Task.processID, this.Processi[index].LateStartTime);
                }

            }
        }

        protected struct IntervalloTempi
        {
            public int IntervalloIndex;
            public TimeSpan Start, End;
        }

        /*
         * Returns:
         * 1 se tutto ok
         * 2 se il tempo a disposizione è < del CriticalPath, quindi va ritardata la consegna
         * 3 se non sono riuscito a dare un EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate a tutti i task.
         * 4 se la data prevista di fine produzione non è reale
         */
        public int SimulaIntroduzioneInProduzione()
        {
            int rt = 1;
            Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
            if (art.ID != -1 && TimeZoneInfo.ConvertTimeToUtc(art.DataPrevistaFineProduzione, this.RepartoProduttivo.tzFusoOrario) > DateTime.UtcNow)
            {
                // Per prima cosa, verifico quale è la data di consegna, ed il primo turno utile in cui inserire il task
                Reparto rp = new Reparto(this.Tenant, art.Reparto);
                rp.loadCalendario(DateTime.UtcNow, TimeZoneInfo.ConvertTimeToUtc(art.DataPrevistaConsegna, this.RepartoProduttivo.tzFusoOrario).AddDays(7));
                // Ricerco l'ultimo turno papabile
                TimeSpan tempoADisposizione = new TimeSpan(0, 0, 0);
                int indInterv = -1;
                log = "Reparto: " + rp.id.ToString() + rp.CalendarioRep.Intervalli.Count.ToString();
                bool FPCompresa = false;
                for (int i = rp.CalendarioRep.Intervalli.Count - 1; i >= 0 && indInterv == -1; i--)
                {
                    if (rp.CalendarioRep.Intervalli[i].Fine >= art.DataPrevistaFineProduzione && rp.CalendarioRep.Intervalli[i].Inizio <= art.DataPrevistaFineProduzione)
                    {
                        FPCompresa = true;
                        indInterv = i;
                    }
                    else if (rp.CalendarioRep.Intervalli[i].Fine <= art.DataPrevistaFineProduzione && rp.CalendarioRep.Intervalli[i].Inizio <= art.DataPrevistaFineProduzione)
                    {
                        indInterv = i;
                    }
                }

                // Ricerco quanto lungo è il CriticalPath
                this.calculateCriticalPath();
                TimeSpan maxTime = new TimeSpan(0, 0, 0);
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    if (this.Processi[i].LateFinishTime > maxTime)
                    {
                        maxTime = this.Processi[i].LateFinishTime;
                    }
                }

                // Calcolo il tempo a disposizione e vedo se può essere congruente con il CriticalPath
                for (int i = indInterv; i >= 0 && tempoADisposizione <= maxTime; i--)
                {
                    log += "Tempo a disposizione...";
                    if (i == indInterv && FPCompresa == true)
                    {
                        tempoADisposizione += (art.DataPrevistaFineProduzione - rp.CalendarioRep.Intervalli[i].Inizio);
                    }
                    else
                    {
                        tempoADisposizione += (rp.CalendarioRep.Intervalli[i].Fine - rp.CalendarioRep.Intervalli[i].Inizio);
                    }
                }

                if (tempoADisposizione < maxTime)
                {
                    log += "NON HO TEMPO A DISPOSIZIONE!"+tempoADisposizione.ToString() + "vs"+maxTime.ToString() + ";";
                    rt = 2;
                }
                else
                {
                    log = "HO TEMPO!";
                }

                // Se ho tempo a disposizione, adesso devo convertire gli earlyStart, lateStart, earlyFinish, lateFinish di ogni task in date da mettere a calendario
                if (rt == 1)
                {
                    // Nomino i vari intervalli, a partire dall'ultimo, in riferimento agli Start e Finish Time.
                    int cont = indInterv;
                    TimeSpan residuo = maxTime;
                    List<IntervalloTempi> ElIntervalli = new List<IntervalloTempi>();
                    int c = 0;
                    if (FPCompresa == true)
                    {
                        ElIntervalli.Add(new IntervalloTempi());
                        IntervalloTempi tm = ElIntervalli[c];
                        tm.IntervalloIndex = cont;
                        tm.End = residuo - (art.DataPrevistaFineProduzione - rp.CalendarioRep.Intervalli[cont].Fine);
                        tm.Start = residuo - (art.DataPrevistaFineProduzione - rp.CalendarioRep.Intervalli[cont].Inizio);
                        residuo -= (art.DataPrevistaFineProduzione - rp.CalendarioRep.Intervalli[cont].Inizio);
                        ElIntervalli[c] = tm;
                        c++;
                        cont--;
                    }
                    while (cont >= 0 && rp.CalendarioRep.Intervalli[cont].Inizio > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.RepartoProduttivo.tzFusoOrario))
                    {
                        ElIntervalli.Add(new IntervalloTempi());
                        IntervalloTempi tm = ElIntervalli[c];
                        tm.IntervalloIndex = cont;
                        tm.End = residuo;
                        tm.Start = residuo - (rp.CalendarioRep.Intervalli[cont].Fine - rp.CalendarioRep.Intervalli[cont].Inizio);
                        residuo -= (rp.CalendarioRep.Intervalli[cont].Fine - rp.CalendarioRep.Intervalli[cont].Inizio);
                        ElIntervalli[c] = tm;
                        c++;
                        cont--;
                    }

                    // Controllo la configurazione del reparto: se posso splittare il task procedo in un modo, se lo devo tenere unito nell'altro.
                    bool split = this.RepartoProduttivo.splitTasks;
                    if (split == false)
                    {
                        //log += "<br/><br/>";

                        // Cerco di inserire i processi negli intervalli --> trovo StartDate e FinishDate
                        // Per ogni task, trovo l'intervallo in cui risiede, e in base a data e ora di inizio e fine ci inserisco il task

                        for (int i = 0; i < this.Processi.Count; i++)
                        {
                            // Ricerco l'intervallo di mio gradimento
                            int indiceInt = -1;

                            for (int j = 0; j < ElIntervalli.Count; j++)
                            {
                                //log += "--->" + this.Processi[i].EarlyStartTime.ToString() + " - " + this.Processi[i].LateFinishTime.ToString() + "; "
                                //    + ElIntervalli[j].Start.ToString() + " - " + ElIntervalli[j].End.ToString();
                                if (ElIntervalli[j].Start <= this.Processi[i].LateStartTime && ElIntervalli[j].End >= this.Processi[i].EarlyFinishTime && (ElIntervalli[j].End - ElIntervalli[j].Start) >= this.Processi[i].Tempo.Tempo)
                                {
                                    indiceInt = j;
                                    //log += " <----- FOUND!";
                                }
                                //log += "<br/>";    
                            }


                            if (indiceInt != -1)
                            {
                                int indiceIntervallo = ElIntervalli[indiceInt].IntervalloIndex;
                                this.Processi[i].EarlyStartDate = rp.CalendarioRep.Intervalli[indiceIntervallo].Inizio.AddSeconds((this.Processi[i].EarlyStartTime - ElIntervalli[indiceInt].Start).TotalSeconds);
                                this.Processi[i].LateStartDate = rp.CalendarioRep.Intervalli[indiceIntervallo].Inizio.AddSeconds((this.Processi[i].LateStartTime - ElIntervalli[indiceInt].Start).TotalSeconds);
                                this.Processi[i].EarlyFinishDate = rp.CalendarioRep.Intervalli[indiceIntervallo].Fine.AddSeconds((this.Processi[i].EarlyFinishTime - ElIntervalli[indiceInt].End).TotalSeconds);
                                this.Processi[i].LateFinishDate = rp.CalendarioRep.Intervalli[indiceIntervallo].Fine.AddSeconds((this.Processi[i].LateFinishTime - ElIntervalli[indiceInt].End).TotalSeconds);
                            }
                            else
                            {
                                rt = 3;
                            }

                        }

                    }
                    else
                    {
                        // POSSO DIVIDERE I TASK TRA PIU' TURNI
                        for (int i = 0; i < this.Processi.Count; i++)
                        {
                            // Trovo gli intervalli in cui si posizionano i miei task, per early e late start e finish
                            int indEarlyStart, indLateStart, indEarlyFinish, indLateFinish;
                            indEarlyFinish = indEarlyStart = indLateFinish = indLateStart = -1;

                            for (int j = 0; j < ElIntervalli.Count; j++)
                            {
                                if (ElIntervalli[j].Start <= this.Processi[i].EarlyStartTime && ElIntervalli[j].End >= this.Processi[i].EarlyStartTime)
                                {
                                    indEarlyStart = j;
                                }
                                if (ElIntervalli[j].Start <= this.Processi[i].LateStartTime && ElIntervalli[j].End >= this.Processi[i].LateStartTime)
                                {
                                    indLateStart = j;
                                }
                                if (ElIntervalli[j].Start <= this.Processi[i].EarlyFinishTime && ElIntervalli[j].End >= this.Processi[i].EarlyFinishTime)
                                {
                                    indEarlyFinish = j;
                                }
                                if (ElIntervalli[j].Start <= this.Processi[i].LateFinishTime && ElIntervalli[j].End >= this.Processi[i].LateFinishTime)
                                {
                                    indLateFinish = j;
                                }
                            }

                            if (indEarlyStart != -1 && indLateStart != -1 && indEarlyFinish != -1 && indLateFinish != -1)
                            {
                                int indES = ElIntervalli[indEarlyStart].IntervalloIndex;
                                int indLS = ElIntervalli[indLateStart].IntervalloIndex;
                                int indEF = ElIntervalli[indEarlyFinish].IntervalloIndex;
                                int indLF = ElIntervalli[indLateFinish].IntervalloIndex;
                                this.Processi[i].EarlyStartDate = rp.CalendarioRep.Intervalli[indES].Inizio.AddSeconds((this.Processi[i].EarlyStartTime - ElIntervalli[indEarlyStart].Start).TotalSeconds);
                                this.Processi[i].LateStartDate = rp.CalendarioRep.Intervalli[indLS].Inizio.AddSeconds((this.Processi[i].LateStartTime - ElIntervalli[indLateStart].Start).TotalSeconds);
                                this.Processi[i].EarlyFinishDate = rp.CalendarioRep.Intervalli[indEF].Fine.AddSeconds((this.Processi[i].EarlyFinishTime - ElIntervalli[indEarlyFinish].End).TotalSeconds);
                                this.Processi[i].LateFinishDate = rp.CalendarioRep.Intervalli[indLF].Fine.AddSeconds((this.Processi[i].LateFinishTime - ElIntervalli[indLateFinish].End).TotalSeconds);
                            }
                            else
                            {
                                rt = 3;
                            }
                        }



                    }
                }
            }
            else
            {
                // Se la data di prevista fine produzione < OGGI!
                rt = 4;
            }
            return rt;
        }

        /*
         * Lancia il processo in produzione
         * Returns:
         * 0 se errore generico
         * 1 se tutto ok
         * 2 se non sono state impostate correttamente EarlyStartDate, LateStartDate, EarlyFinishDate, LateFinishDate
         * 3 se l'articolo è già stato lanciato in produzione
         */
        public int LanciaInProduzione()
        {
            int rt = 1;
            if (this.Processi != null && this.MainProcess != null && this.RepartoProduttivo != null && this.RepartoProduttivo.id != -1 && this.Quantita > 0)
            {
                // Ora controllo che tutti i task abbiano Early e Late Start e Finish Date impostato e maggiore di DateTime.Now
                for (int i = 0; i < this.Processi.Count; i++)
                {
                    if (!(this.Processi[i].EarlyStartDate != null && this.Processi[i].EarlyStartDate >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.RepartoProduttivo.tzFusoOrario) && this.Processi[i].LateStartDate != null && this.Processi[i].LateStartDate >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.RepartoProduttivo.tzFusoOrario) && this.Processi[i].EarlyFinishDate != null && this.Processi[i].EarlyFinishDate >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.RepartoProduttivo.tzFusoOrario) && this.Processi[i].LateFinishDate != null && this.Processi[i].LateFinishDate >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.RepartoProduttivo.tzFusoOrario)))
                    {
                        log += this.Processi[i].EarlyStartDate.ToString() + " " + this.Processi[i].LateStartDate.ToString() + " " + this.Processi[i].EarlyFinishDate.ToString() + " " + this.Processi[i].LateFinishDate.ToString() + "<br/>";
                        rt = 2;
                    }
                }
                Articolo art = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                if (art.Status != 'N')
                {
                    rt = 3;
                }
            }
            else
            {
                rt = 0;
            }

            // Se i controlli preliminari sono ok, allora tento di inserire i task in produzione
            if (rt == 1)
            {
                List<int[]> lstTaskIDOLDREVNEW = new List<int[]>();
                log += "Inserisco i task in produzione<br/>";
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        int maxTaskID = 0;
                        int? maxRow = conn.QueryFirstOrDefault<int?>("SELECT MAX(taskID) FROM tasksproduzione", null, tr);
                if (maxRow.HasValue)
                {
                    maxTaskID = maxRow.Value + 1;
                }
                try
                {
                    TimeSpan ProductPlannedLabor = new TimeSpan(0, 0, 0);
                    Articolo artcl = new Articolo(this.Tenant, this.ArticoloID, this.ArticoloAnno);
                    
                    // Inserisco i task nel piano di produzione
                    for (int i = 0; i < this.Processi.Count; i++)
                    {
                        TimeSpan tcTotale = this.Processi[i].Tempo.TempoSetup + TimeSpan.FromTicks(this.Quantita * this.Processi[i].Tempo.Tempo.Ticks)
                            + this.Processi[i].Tempo.TempoUnload;
                        // Inserisco il task
                        String strSQL = "INSERT INTO tasksproduzione(taskID, name, description, earlyStart, LateStart, "
                            + "earlyFinish, LateFinish, origTask, revOrigTask, variante, reparto, postazione, status, idcommessa, "
                            + " annoCommessa, idArticolo, annoArticolo, nOperatori, tempoCiclo, qtaPrevista, qtaProdotta) VALUES("
                            + "@maxTaskID, "
                            + "@processName, "
                            + "@processDescription, "
                            + "@earlyStart, "
                            + "@lateStart, "
                            + "@earlyFinish, "
                            + "@lateFinish, "
                            + "@processID, "
                            + "@revisione, "
                            + "@idVariante, "
                            + "@reparto, "
                            + "@postazione, "
                            + "'N', "
                            + "@commessa, "
                            + "@annoCommessa, "
                            + "@articoloID, "
                            + "@annoArticolo, "
                            + "@nOperatori, "
                            + "@tempoCicloStr"
                            + ", @qtaPrevista, "
                            + "@qtaProdotta"
                            + ")";

                        conn.Execute(strSQL, new
                        {
                            maxTaskID = maxTaskID,
                            processName = (object)(this.Processi[i].Task.Task.processName ?? ""),
                            processDescription = (object)(this.Processi[i].Task.Task.processDescription ?? ""),
                            earlyStart = TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].EarlyStartDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss"),
                            lateStart = TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].LateStartDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss"),
                            earlyFinish = TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].EarlyFinishDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss"),
                            lateFinish = TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].LateFinishDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss"),
                            processID = this.Processi[i].Task.Task.processID,
                            revisione = this.Processi[i].Task.Task.revisione,
                            idVariante = this.MainProcess.variant.idVariante,
                            reparto = this.RepartoProduttivo.id,
                            postazione = this.Processi[i].PostazioneDiLavoro.id,
                            commessa = artcl.Commessa,
                            annoCommessa = artcl.AnnoCommessa,
                            articoloID = artcl.ID,
                            annoArticolo = artcl.Year,
                            nOperatori = this.Processi[i].Tempo.NumeroOperatori,
                            tempoCicloStr = Math.Floor(tcTotale.TotalHours).ToString() //Math.Floor(this.Processi[i].Tempo.Tempo.TotalHours).ToString()
                                + ":" + tcTotale.Minutes.ToString()//this.Processi[i].Tempo.Tempo.Minutes.ToString()
                                + ":" + tcTotale.Seconds.ToString(),//this.Processi[i].Tempo.Tempo.Seconds.ToString()
                            qtaPrevista = this.Quantita,
                            qtaProdotta = this.Quantita
                        }, tr);

                        ProductPlannedLabor = ProductPlannedLabor.Add(tcTotale);

                        // Array di corrispondenza id+revisione processo e taskproduzione
                        int[] idOLDNEW = new int[3];
                        idOLDNEW[0] = this.Processi[i].Task.Task.processID;
                        idOLDNEW[1] = this.Processi[i].Task.Task.revisione;
                        idOLDNEW[2] = maxTaskID;
                        lstTaskIDOLDREVNEW.Add(idOLDNEW);
                        maxTaskID++;
                        log += strSQL + "<br />";
                    }

                    

                    String sqlUpdate = "UPDATE productionplan SET status = 'P', WorkingTimePlanned = @workingTimePlanned"                        
                         + " WHERE id = @articoloID AND anno = @annoArticolo";
                    conn.Execute(sqlUpdate, new
                    {
                        workingTimePlanned = Math.Floor(ProductPlannedLabor.TotalHours).ToString() //Math.Floor(this.Processi[i].Tempo.Tempo.TotalHours).ToString()
                                + ":" + ProductPlannedLabor.Minutes.ToString()//this.Processi[i].Tempo.Tempo.Minutes.ToString()
                                + ":" + ProductPlannedLabor.Seconds.ToString(),//this.Processi[i].Tempo.Tempo.Seconds.ToString()
                        articoloID = artcl.ID,
                        annoArticolo = artcl.Year
                    }, tr);

                    log += sqlUpdate;

                    tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        log += ex.Message;
                        tr.Rollback();
                        rt = 0;
                    }
                    }
                    // Copy Default Operators
                    if(rt!=0)
                { 
                for(int i = 0; i < this.Processi.Count; i++)
                {
                        int taskid = -1;
                        foreach(var itm in lstTaskIDOLDREVNEW)
                        {
                            if(itm[0] == this.Processi[i].Task.Task.processID && itm[1] == this.Processi[i].Task.Task.revisione)
                            {
                                taskid = itm[2];
                            }
                        }
                        if(taskid!=-1)
                        { 
                    this.Processi[i].Task.loadDefaultOperators();
                    foreach (var defOp in this.Processi[i].Task.DefaultOperators)
                    {
                            conn.Execute("INSERT INTO taskuser(taskID, user, exclusive) VALUES(@TaskID, @User, @Exclusive)",
                                new { TaskID = taskid, User = defOp.username, Exclusive = false });
                    }
                        }
                    }
                }

                // Ora inserisco i vincoli di precedenza

                try
                {
                    // Inserisco i precedenti
                    for (int i = 0; i < this.Processi.Count; i++)
                    {
                        int newCurrID = -1;

                        for (int q = 0; q < this.Processi[i].Precedenti.Count; q++)
                        {
                            int newPrecID = -1;
                            
                            // Ricerco il nuovo id!
                            for (int j = 0; j < lstTaskIDOLDREVNEW.Count; j++)
                            {
                                if (lstTaskIDOLDREVNEW[j][0] == this.Processi[i].Precedenti[q])
                                {
                                    newPrecID = lstTaskIDOLDREVNEW[j][2];
                                }
                                if (lstTaskIDOLDREVNEW[j][0] == this.Processi[i].Task.Task.processID)
                                {
                                    newCurrID = lstTaskIDOLDREVNEW[j][2];
                                }
                            }

                            if (newPrecID != -1)
                            {
                                conn.Execute("INSERT INTO prectasksproduzione(prec, succ, relazione, pausa, ConstraintType) VALUES("
                                    + "@prec, @succ, 0, '00:00:00', @constraintType)",
                                    new { prec = newPrecID, succ = newCurrID, constraintType = this.Processi[i].PrecedentiConstraintType[q] });
                            }

                        }
                    }
                }
                catch(Exception ex)
                {
                    log += ex.Message;
                }


                }
            }

            return rt;
        }
    }

    public class ElencoTaskProduzione
    {
        public String Tenant;

        public String log;

        private List<TaskProduzione> _TaskAvviabili;
        public List<TaskProduzione> TaskAvviabili
        {
            get { return this._TaskAvviabili; }
        }

        public ElencoTaskProduzione()
        {
            this._TaskAvviabili = new List<TaskProduzione>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT taskID FROM tasksproduzione WHERE status = 'N' OR status = 'I' OR status = 'P' ORDER BY lateStart";
                var rows = conn.Query<int>(sql);
                foreach (var id in rows)
                {
                    TaskProduzione tsk = new TaskProduzione(this.Tenant, id);

                    this._TaskAvviabili.Add(tsk);
                }
            }
        }

        private List<TaskProduzione> _Tasks;
        public List<TaskProduzione> Tasks
        {
            get
            {
                return this._Tasks;
            }
        }

        public ElencoTaskProduzione(String Tenant, processo origProc)
        {
            this.Tenant = Tenant;
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT taskID FROM tasksproduzione WHERE status LIKE 'F' "
                        + " AND origTask = @origTask"
                        + " AND revOrigTask = @revOrigTask"
                        + " ORDER BY lateStart";
                    var rows = conn.Query<int>(sql, new { origTask = origProc.processID, revOrigTask = origProc.revisione });
                    foreach (var id in rows)
                    {
                        this._Tasks.Add(new TaskProduzione(this.Tenant, id));
                    }
                }
            }
        }

        public ElencoTaskProduzione(String Tenant, processo origProc, DateTime start, DateTime end)
        {
            this.Tenant = Tenant;
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT taskID FROM tasksproduzione WHERE status LIKE 'F' "
                        + " AND origTask = @origTask"
                        + " AND revOrigTask = @revOrigTask"
                        + " AND earlyStart > @startDate"
                        + " AND earlyStart < @endDate"
                        + " ORDER BY lateStart";
                    var rows = conn.Query<int>(sql, new { origTask = origProc.processID, revOrigTask = origProc.revisione, startDate = start.Year.ToString() + "/" + start.Month.ToString() + "/" + start.Day.ToString(), endDate = end.Year.ToString() + "/" + end.Month.ToString() + "/" + end.Day.ToString() });
                    foreach (var id in rows)
                    {
                        this._Tasks.Add(new TaskProduzione(this.Tenant, id));
                    }
                }
            }
        }

        public ElencoTaskProduzione(String Tenant, processo origProc, ProcessoVariante ProdottoPadre)
        {
            this.Tenant = Tenant;
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT taskID FROM tasksproduzione INNER JOIN productionplan ON"
                        + "(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
                        + " WHERE tasksproduzione.status LIKE 'F' "
                        + " AND tasksproduzione.origTask = @origTask"
                        + " AND tasksproduzione.revOrigTask = @revOrigTask"
                        + " AND productionplan.processo = @processo"
                        + " AND productionplan.revisione = @revisione"
                        + " AND productionplan.variante = @variante"
                        + " ORDER BY lateStart";
                    var rows = conn.Query<int>(sql, new { origTask = origProc.processID, revOrigTask = origProc.revisione, processo = ProdottoPadre.process.processID, revisione = ProdottoPadre.process.revisione, variante = ProdottoPadre.variant.idVariante });
                    foreach (var id in rows)
                    {
                        this._Tasks.Add(new TaskProduzione(this.Tenant, id));
                    }
                }
            }
        }

        public ElencoTaskProduzione(String Tenant, processo origProc, ProcessoVariante ProdottoPadre, DateTime start, DateTime end)
        {
            this.Tenant = Tenant;
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT taskID FROM tasksproduzione INNER JOIN productionplan ON"
                        + "(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
                        + " WHERE tasksproduzione.status LIKE 'F' "
                        + " AND tasksproduzione.origTask = @origTask"
                        + " AND tasksproduzione.revOrigTask = @revOrigTask"
                        + " AND earlyStart > @startDate"
                        + " AND earlyStart < @endDate"
                        + " AND productionplan.processo = @processo"
                        + " AND productionplan.revisione = @revisione"
                        + " AND productionplan.variante = @variante"
                        + " ORDER BY lateStart";
                    var rows = conn.Query<int>(sql, new { origTask = origProc.processID, revOrigTask = origProc.revisione, startDate = start.Year.ToString() + "/" + start.Month.ToString() + "/" + start.Day.ToString(), endDate = end.Year.ToString() + "/" + end.Month.ToString() + "/" + end.Day.ToString(), processo = ProdottoPadre.process.processID, revisione = ProdottoPadre.process.revisione, variante = ProdottoPadre.variant.idVariante });
                    foreach (var id in rows)
                    {
                        this._Tasks.Add(new TaskProduzione(this.Tenant, id));
                    }
                }
            }
        }

        public ElencoTaskProduzione(String Tenant, DateTime start, DateTime end, char status)
        {
            this.Tenant = Tenant;
            this._Tasks = new List<TaskProduzione>();
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT taskID FROM tasksproduzione WHERE status LIKE @status "
                        + " AND ("
                        + "(earlyStart < @startFull AND lateFinish > @endFull)"
                        + "OR (earlyStart > @startFull AND lateFinish > @endFull AND @endDouble > lateStart)"
                        + "OR (earlyStart < @startFull AND lateFinish < @endFull AND @startDouble < lateFinish)"
                        + "OR (@startFull < earlyStart AND @endFull > lateFinish)"
                        + ")";
                    var rows = conn.Query<int>(sql, new
                    {
                        status = status.ToString(),
                        startFull = start.ToString("yyyy-MM-dd HH:mm:ss"),
                        endFull = end.ToString("yyyy-MM-dd HH:mm:ss"),
                        startDouble = start.ToString("yyyy-MM-dd  HH:mm:ss"),
                        endDouble = end.ToString("yyyy-MM-dd  HH:mm:ss")
                    });
                    foreach (var id in rows)
                    {
                        this._Tasks.Add(new TaskProduzione(this.Tenant, id));
                    }
                }
        }

        public ElencoTaskProduzione(String Tenant, char status)
        {
            this.Tenant = Tenant;
            this._Tasks = new List<TaskProduzione>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT taskID FROM tasksproduzione WHERE status LIKE @status ";
                var rows = conn.Query<int>(sql, new { status = status.ToString() });
                foreach (var id in rows)
                {
                    this._Tasks.Add(new TaskProduzione(this.Tenant, id));
                }
            }
        }

        public ElencoTaskProduzione(String Tenant, Reparto dept, char status)
        {
            this.Tenant = Tenant;
            this._Tasks = new List<TaskProduzione>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT taskID FROM tasksproduzione WHERE reparto = @reparto AND status LIKE @status ";
                var rows = conn.Query<int>(sql, new { reparto = dept.id, status = status.ToString() });
                foreach (var id in rows)
                {
                    this._Tasks.Add(new TaskProduzione(this.Tenant, id));
                }
            }
        }

        public TimeSpan MediaTempoLavoro
        {
            get
            {
                TimeSpan media = new TimeSpan(0, 0, 0);
                if (this.Tasks.Count > 0)
                {
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        media = media.Add(this.Tasks[i].TempoDiLavoroEffettivo);
                    }
                    long mean = media.Ticks / this.Tasks.Count;
                    media = new TimeSpan(mean);
                }
                return media;
            }
        }

        public TimeSpan MediaTempoCiclo
        {
            get
            {
                TimeSpan mean = new TimeSpan(0, 0, 0);
                if (this.Tasks.Count > 0)
                {
                    long somma = this.Tasks.Sum(x => x.TempoCicloEffettivo.Ticks);
                    somma = somma / this.Tasks.Count;
                    mean = new TimeSpan(somma);
                }
                return mean;
            }
        }
    }

    public class EventoTaskProduzione
    {
        public String Tenant;

        public Reparto rp;
        private int _InputPoint;
        public int InputPoint
        {
            get { return this._InputPoint; }
        }
        private DateTime _Data;
        public DateTime Data
        {
            get {
                if(rp==null || rp.id==-1)
                {
                    TaskProduzione tskProd = new TaskProduzione(this.Tenant, this.TaskProduzioneID);
                    rp = new Reparto(this.Tenant, tskProd.RepartoID);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._Data, rp.tzFusoOrario); }
        }

        private Char _Evento;
        public Char Evento
        {
            get { return this._Evento; }
        }

        private int _IdEvento;
        public int IdEvento
        {
            get { return this._IdEvento; }
        }

        private String _Note;
        public String Note
        {
            get { return this._Note; }
        }

        private int _TaskProduzioneID;
        public int TaskProduzioneID
        {
            get { return this._TaskProduzioneID; }
        }

        public EventoTaskProduzione(String Tenant, int evID)
        {
            this.Tenant = Tenant;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT inputpoint, task, data, evento, note FROM registroeventitaskproduzione WHERE id = @id";
                var row = conn.QueryFirstOrDefault<EventoTaskProduzioneRow>(sql, new { id = evID });
                if (row != null)
                {
                    this._IdEvento = evID;
                    this._InputPoint = row.Inputpoint;
                    this._TaskProduzioneID = row.Task;
                    this._Data = row.Data;
                    this._Evento = row.Evento[0];
                    this._Note = "";
                    if (row.Note != null)
                    {
                        this._Note = row.Note;
                    }
                }
                else
                {
                    this._IdEvento = -1;
                }
            }
        }

        private class EventoTaskProduzioneRow
        {
            public int Inputpoint { get; set; }
            public int Task { get; set; }
            public DateTime Data { get; set; }
            public String Evento { get; set; }
            public String Note { get; set; }
        }

    }

    public class Warning
    {
        public String Tenant;

        public String log;
        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private DateTime _DataChiamata;
        public DateTime DataChiamata
        {
            get {
                TaskProduzione tsk = new TaskProduzione(this.Tenant, this.TaskID);
                Reparto rp = new Reparto(this.Tenant, tsk.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._DataChiamata, rp.tzFusoOrario); }
        }

        private int _TaskID;
        public int TaskID
        {
            get { return this._TaskID; }
        }

        private String _User;
        public String User
        {
            get { return this._User; }
        }

        private DateTime _DataRisoluzione;
        public DateTime DataRisoluzione
        {
            get {
                TaskProduzione tsk = new TaskProduzione(this.Tenant, this.TaskID);
                Reparto rp = new Reparto(this.Tenant, tsk.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._DataRisoluzione, rp.tzFusoOrario);
            }
            set
            {
                TaskProduzione tsk = new TaskProduzione(this.Tenant, this.TaskID);
                Reparto rp = new Reparto(this.Tenant, tsk.RepartoID);
                this._DataRisoluzione = DateTime.UtcNow;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("UPDATE warningproduzione SET dataRisoluzione = @dataRisoluzione "
                                + " WHERE id = @id",
                                new { dataRisoluzione = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), id = this.ID }, tr);
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }
            }
        }

        private String _MotivoChiamata;
        public String MotivoChiamata
        {
            get { return this._MotivoChiamata; }
            set
            {
                this._MotivoChiamata = value;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("UPDATE warningproduzione SET motivo = @motivo WHERE id = @id",
                                new { motivo = (object)(value ?? ""), id = this.ID }, tr);
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }
            }
        }

        private String _Risoluzione;
        public String Risoluzione
        {
            get { return this._Risoluzione; }
            set 
            {
                this._Risoluzione = value;
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("UPDATE warningproduzione SET risoluzione = @risoluzione WHERE id = @id",
                                new { risoluzione = (object)(value ?? ""), id = this.ID }, tr);
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }
            }
        }

        public bool isOpen
        {
            get
            {
                if (this._DataRisoluzione == new DateTime(1970,1,1) || this._MotivoChiamata == null || this._Risoluzione == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Warning(String Tenant, int wrnID)
        {
            this.Tenant = Tenant;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id, dataChiamata, task, user, dataRisoluzione, motivo, risoluzione FROM warningproduzione "
                    + "WHERE id = @id";
                var row = conn.QueryFirstOrDefault<WarningRow>(sql, new { id = wrnID });
                if (row != null)
                {
                    this._ID = row.ID;
                    this._DataChiamata = row.DataChiamata;
                    this._TaskID = row.Task;
                    this._User = row.User;
                    if (row.DataRisoluzione.HasValue)
                    {
                        this._DataRisoluzione = row.DataRisoluzione.Value;
                    }
                    else
                    {
                        this._DataRisoluzione = new DateTime(1970, 1, 1);
                    }
                    this._MotivoChiamata = row.Motivo;
                    this._Risoluzione = row.Risoluzione;
                }
            }
        }

        private class WarningRow
        {
            public int ID { get; set; }
            public DateTime DataChiamata { get; set; }
            public int Task { get; set; }
            public String User { get; set; }
            public DateTime? DataRisoluzione { get; set; }
            public String Motivo { get; set; }
            public String Risoluzione { get; set; }
        }

        public String NomePostazione
        {
            get 
            {
                TaskProduzione tsk = new TaskProduzione(this.Tenant, this.TaskID);
                return tsk.PostazioneName;
            }
        }

        public String NomeReparto
        {
            get
            {
                TaskProduzione tsk = new TaskProduzione(this.Tenant, this.TaskID);
                Reparto rp = new Reparto(this.Tenant, tsk.RepartoID);
                return rp.name;
            }
        }
    }

    public class WarningAperti
    {
        public String Tenant;

        private List<Warning> _Elenco;
        public List<Warning> Elenco
        {
            get { return this._Elenco; }
        }

        public WarningAperti(String Tenant)
        {
            this.Tenant = Tenant;
            this._Elenco = new List<Warning>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id FROM warningproduzione WHERE dataRisoluzione IS NULL ORDER BY dataChiamata";
                var rows = conn.Query<int>(sql);
                foreach (var id in rows)
                {
                    this._Elenco.Add(new Warning(this.Tenant, id));
                }
            }
        }

        public WarningAperti(String Tenant, Reparto rp)
        {
            this.Tenant = Tenant;
            this._Elenco = new List<Warning>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id FROM warningproduzione INNER JOIN tasksproduzione ON(warningproduzione.task = tasksproduzione.taskID) "
                    + " WHERE dataRisoluzione IS NULL "
                    + " AND reparto = @reparto"
                    + " ORDER BY dataChiamata";
                var rows = conn.Query<int>(sql, new { reparto = rp.id });
                foreach (var id in rows)
                {
                    this._Elenco.Add(new Warning(this.Tenant, id));
                }
            }
        }

        
    }

    public class Warnings
    {
        public String Tenant;

        private List<Warning> _List;
        public List<Warning> List
        {
            get { return this._List; }
        }

        public Warnings(String Tenant)
        {
            this.Tenant = Tenant;
            this._List = new List<Warning>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id FROM warningproduzione ORDER BY dataChiamata";
                var rows = conn.Query<int>(sql);
                foreach (var id in rows)
                {
                    this._List.Add(new Warning(this.Tenant, id));
                }
            }
        }

        public Warnings(String Tenant, Reparto rp)
        {
            this.Tenant = Tenant;
            this._List = new List<Warning>();
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id FROM warningproduzione INNER JOIN tasksproduzione ON(warningproduzione.task = tasksproduzione.taskID) "
                    + " WHERE reparto = @reparto"
                    + " ORDER BY dataChiamata";
                var rows = conn.Query<int>(sql, new { reparto = rp.id });
                foreach (var id in rows)
                {
                    this._List.Add(new Warning(this.Tenant, id));
                }
            }
        }
    }

    public class ElencoArticoliInProduzione
    {
        public String Tenant;

        private List<Articolo> _ElencoArticoli;
        public List<Articolo> ElencoArticoli
        {
            get { return this._ElencoArticoli; }
        }

        public ElencoArticoliInProduzione(String Tenant)
        {
            this.Tenant = Tenant;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT id, anno FROM productionplan WHERE status = 'I' OR status = 'P' "
                    + " ORDER BY status DESC, anno DESC, id DESC";
                var rows = conn.Query<(int ID, int Anno)>(sql);
                this._ElencoArticoli = new List<Articolo>();
                foreach (var row in rows)
                {
                    this._ElencoArticoli.Add(new Articolo(this.Tenant, row.ID, row.Anno));
                }
            }
        }
    }

    /* Struttura usata da TaskProduzione e User */
    public struct IntervalliDiLavoroEffettivi
    {
        public String Tenant;

        public int user;
        public TimeSpan Intervallo;
        public DateTime Inizio;
        public DateTime Fine;
        public int TaskID;
        public String nomeTask;
        public int idPostazione;
        public String nomePostazione;
        public int idProdotto;
        public int annoProdotto;
        public String nomeProdotto;
        public String ragioneSocialeCliente;
        public int idReparto;

        public int TaskProduzioneID
        {
            get
            {
                return this.TaskID;
            }
            set
            {
                this.TaskID = value;
            }
        }
        public DateTime DataInizio
        {
            get
            {
                //FusoOrario fuso = new FusoOrario();
                //return TimeZoneInfo.ConvertTimeFromUtc(this.Inizio, fuso.tzFusoOrario);
                Reparto rp = new Reparto(this.Tenant, this.idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this.Inizio, rp.tzFusoOrario);
            }
        }
        public DateTime DataFine
        {
            get
            {
                //FusoOrario fuso = new FusoOrario();
                //return TimeZoneInfo.ConvertTimeFromUtc(this.Fine, fuso.tzFusoOrario);
                Reparto rp = new Reparto(this.Tenant, this.idReparto);
                return TimeZoneInfo.ConvertTimeFromUtc(this.Fine, rp.tzFusoOrario);
            }
        }
        public TimeSpan DurataIntervallo
        {
            get
            {
                return this.Intervallo;
            }
        }
        /*public String Username
        {
            get
            {
                return this.user;
            }
        }*/

        public int InputPointId
        {
            get
            {
                return this.user;
            }
        }

        private String _InputPointName;

        public String InputPointName { get { return this._InputPointName; } }
        public int IDPostazione
        {
            get
            { return this.idPostazione; }
        }
        public String NomePostazione
        {
            get
            {
                return this.nomePostazione;
            }
        }
        public String NomeTask
        { get { return this.nomeTask; } }
        public int IDProdotto { get { return this.idProdotto; } }
        public int AnnoProdotto { get { return this.annoProdotto; } }
        public String NomeProdotto { get { return this.nomeProdotto; }}
        public String RagioneSocialeCliente { get { return this.ragioneSocialeCliente; } }
        public char EndEventStatus;
        public char ProductStatus;
        public char TaskStatus;
        public int StartEventID;
        public int EndEventID;
        public TimeSpan PlannedWorkingTime;
    }

    /* Usata da checkWorkload */
    public struct caricoDiLavoro
    {
        public String Tenant;

        public Postazione postazione;
        public int articolo;
        public int articoloAnno;
        public TimeSpan carico;
        public double caricoOre
        {
            get
            {
                return carico.TotalHours;
            }
        }

        public double CaricoOre;
        public bool DaProgrammare;

        public System.Drawing.Color colore
        {
            get
            {
                if (DaProgrammare == false)
                {
                    return System.Drawing.Color.Yellow;
                }
                else
                {
                    return System.Drawing.Color.FromArgb((articolo * articoloAnno) % 255, (Math.Abs(articoloAnno - articolo)) % 255, (articoloAnno / (articolo + 1)) % 255);
                }
            }
        }
    }

    public struct FlatWarning
    {
        public String Tenant;

        public int WarningID;
        public DateTime WarningDate;
        public String User;
        public String Reason;
        public String Resolution;
        public int TaskID;
        public int TaskName;
        public int Workstation;
        public String WorkstationName;
        public int ProductID;
        public int ProductYear;
        public String processName;
        public String varianteName;
        public String SerialNumber;
        public int Quantity;
        public String CustomerID;
        public String CustomerName;
    }

    public class TaskParameter
    {
        public String Tenant;

        public String log;

        private int _TaskID;
        public int TaskID
        {
            get
            {
                return this._TaskID;
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
                if (this.TaskID != -1 && value != null && value.ID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("UPDATE modelTaskparameters SET paramCategory = @paramCategory WHERE "
                                    + " paramID = @paramID"
                                    + " AND TaskID = @taskID",
                                    new { paramCategory = value.ID, paramID = this.ParameterID, taskID = this.TaskID }, tr);
                                tr.Commit();
                                this._ParameterCategory = value;
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                        }
                    }
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
                if (this.TaskID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("UPDATE modelTaskparameters SET paramName = @paramName WHERE "
                                    + " paramID = @paramID"
                                    + " AND TaskID = @taskID",
                                    new { paramName = (object)(value ?? ""), paramID = this.ParameterID, taskID = this.TaskID }, tr);
                                tr.Commit();
                                this._Name = value;
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                        }
                    }
                }
            }
        }

        private String _Description;
        public String Description
        {
            get { return this._Description; }
            set
            {
                if (this.TaskID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("UPDATE taskparameters SET paramDescription = @paramDescription WHERE "
                                    + " paramID = @paramID"
                                    + " AND TaskID = @taskID",
                                    new { paramDescription = (object)(value ?? ""), paramID = this.ParameterID, taskID = this.TaskID }, tr);
                                tr.Commit();
                                this._Description = value;
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                        }
                    }
                }
            }
        }

        private Boolean _isFixed;
        public Boolean isFixed
        {
            get { return this._isFixed; }
            set
            {
                if (this.TaskID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("UPDATE taskparameters SET isFixed = @isFixed WHERE "
                                    + " paramID = @paramID"
                                    + " AND TaskID = @taskID",
                                    new { isFixed = value, paramID = this.ParameterID, taskID = this.TaskID }, tr);
                                tr.Commit();
                                this._isFixed = value;
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                        }
                    }
                }
            }
        }

        private Boolean _isRequired;
        public Boolean isRequired
        {
            get { return this._isRequired; }
            set
            {
                if (this.TaskID != -1)
                {
                    using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            try
                            {
                                conn.Execute("UPDATE taskparameters SET isRequired = @isRequired WHERE "
                                    + " paramID = @paramID"
                                    + " AND TaskID = @taskID",
                                    new { isRequired = value, paramID = this.ParameterID, taskID = this.TaskID }, tr);
                                tr.Commit();
                                this._isRequired = value;
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                        }
                    }
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

        private String _CreatedBy;
        public String CreatedBy
        {
            get { return this._CreatedBy; }
        }

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return this._CreatedDate; }
        }

        public TaskParameter(String Tenant, int TaskID, int parameterID)
        {
            this.Tenant = Tenant;
            this._ParameterID = -1;
            this._TaskID = -1;
            using (var conn = (new Dati.Dati()).mycon(this.Tenant))
            {
                conn.Open();
                string sql = "SELECT TaskID, paramID, paramCategory, paramName, "
                    + " paramDescription, isFixed, isRequired, sequence, CreatedBy, CreatedDate "
                    + " FROM taskparameters WHERE "
                    + " TaskID = @taskID"
                    + " AND paramID = @paramID";
                var row = conn.QueryFirstOrDefault<TaskParameterRow>(sql, new { taskID = TaskID, paramID = parameterID });
                if (row != null && row.ParamName != null)
                {
                    this._TaskID = row.TaskID;
                    this._ParameterID = row.ParamID;
                    this.ParameterCategory = new ProductParametersCategory(this.Tenant, row.ParamCategory);
                    this._Name = row.ParamName;
                    this._Description = row.ParamDescription;
                    if (row.IsFixed.HasValue)
                    {
                        this._isFixed = row.IsFixed.Value;
                    }
                    if (row.IsRequired.HasValue)
                    {
                        this._isRequired = row.IsRequired.Value;
                    }
                    if (row.Sequence.HasValue)
                    {
                        this._Sequence = row.Sequence.Value;
                    }
                    this._CreatedBy = row.CreatedBy;
                    if (row.CreatedDate.HasValue)
                    {
                        this._CreatedDate = row.CreatedDate.Value;
                    }
                }
            }
        }

        private class TaskParameterRow
        {
            public int TaskID { get; set; }
            public int ParamID { get; set; }
            public int ParamCategory { get; set; }
            public String ParamName { get; set; }
            public String ParamDescription { get; set; }
            public Boolean? IsFixed { get; set; }
            public Boolean? IsRequired { get; set; }
            public int? Sequence { get; set; }
            public String CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

        public Boolean Delete()
        {
            Boolean ret = false;
            if (this.ParameterID != -1 && this.TaskID != -1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            conn.Execute("DELETE FROM taskparameters WHERE taskID = @taskID"
                                + " AND paramID = @paramID",
                                new { taskID = this.TaskID, paramID = this.ParameterID }, tr);
                            tr.Commit();
                            ret = true;
                        }
                        catch (Exception ex)
                        {
                            this.log = ex.Message;
                            ret = false;
                            tr.Rollback();
                        }
                    }
                }
            }
            return ret;
        }
    }

    public class TaskOperatorNote
    {
        public String Tenant;

        private int _TaskID;
        public int TaskID
        {
            get
            {
                return this._TaskID;
            }
        }
        private int _CommentID;
        public int CommentID
        {
            get
            {
                return this._CommentID;
            }
        }
        private String _User;
        public String User
        {
            get
            {
                return this._User;
            }
        }
        private DateTime _Date;
        public DateTime Date
        {
            get
            {
                return this._Date;
            }
        }
        private String _Note;
        public String Note
        {
            get
            {
                return this._Note;
            }
        }

        public TaskOperatorNote(String Tenant)
        {
            this.Tenant = Tenant;
            this._TaskID = -1;
            this._CommentID = -1;
            this._User = "";
            this._Date = new DateTime(1970, 1, 1);
            this._Note = "";
        }

        public TaskOperatorNote(String Tenant, int TaskID, int CommentID)
        {
            this.Tenant = Tenant;
            this._TaskID = -1;
            this._CommentID = -1;
            this._User = "";
            this._Date = new DateTime(1970, 1, 1);
            this._Note = "";
            if(TaskID!=-1 && CommentID!=-1)
            {
                using (var conn = (new Dati.Dati()).mycon(this.Tenant))
                {
                    conn.Open();
                    string sql = "SELECT date, user, notes FROM tasksproduzioneoperatornotes WHERE "
                        + " TaskID = @taskID"
                        + " AND CommentID = @commentID";
                    var row = conn.QueryFirstOrDefault<TaskOperatorNoteRow>(sql, new { taskID = TaskID, commentID = CommentID });
                    if (row != null)
                    {
                        this._TaskID = TaskID;
                        this._CommentID = CommentID;
                        this._Date = row.Date;
                        this._User = row.User;
                        this._Note = row.Notes;
                    }
                }
            }
        }

        private class TaskOperatorNoteRow
        {
            public DateTime Date { get; set; }
            public String User { get; set; }
            public String Notes { get; set; }
        }
    }

    public class ProductionOrderRow
    {
        public String CustomerID;
        public String CustomerName;
        public String CustomerVATNumber;
        public String CustomerCodiceFiscale;
        public String CustomerAddress;
        public String CustomerCity;
        public String CustomerProvince;
        public String CustomerZipCode;
        public String CustomerCountry;
        public String CustomerPhoneNumber;
        public String CustomerEMail;
        public Boolean? CustomerKanbanManaged;
        public int? SalesOrderID;
        public int? SalesOrderYear;
        public String SalesOrderCustomer;
        public DateTime? SalesOrderDate;
        public String SalesOrderNotes;
        public int? ProductionOrderID;
        public int? ProductionOrderYear;
        public int? ProductionOrderProductTypeID;
        public int? ProductionOrderProductTypeReview;
        public int? ProductionOrderProductID;
        public String ProductionOrderSerialNumber;
        public String ProductionOrderStatus;
        public int? ProductionOrderDepartmentID;
        public DateTime? ProductionOrderStartTime;
        public DateTime? ProductionOrderDeliveryDate;
        public DateTime? ProductionOrderEndProductionDate;
        public String ProductionOrderPlanner;
        public int? ProductionOrderQuantityOrdered;
        public int? ProductionOrderQuantityProduced;
        public String ProductionOrderKanbanCardID;
        public int? ProductTypeID;
        public int? ProductTypeReview;
        public DateTime? ProductTypeReviewDate;
        public String ProductTypeName;
        public String ProductTypeDescription;
        public Boolean? ProductTypeEnabled;
        public int? ProductID;
        public String ProductName;
        public String ProductDescription;
        public int? DepartmentID;
        public String DepartmentName;
        public String DepartmentDescription;
        public Double? DepartmentTaktTime;
        public String DepartmentTimeZone;
        public TimeSpan? RealLeadTime;
        public TimeSpan? RealWorkingTime;
        public TimeSpan? RealDelay;
        public DateTime? ProductionOrderEndProductionDateReal;
        public String SalesOrderExternalID;
        public String ProductExternalID;
        public String MeasurementUnit;
    }

    public struct ProductionOrderStruct
    {
        public String Tenant;

        public String CustomerID;
        public String CustomerName;
        public String CustomerVATNumber;
        public String CustomerCodiceFiscale;
        public String CustomerAddress;
        public String CustomerCity;
        public String CustomerProvince;
        public String CustomerZipCode;
        public String CustomerCountry;
        public String CustomerPhoneNumber;
        public String CustomerEMail;
        public Boolean CustomerKanbanManaged;
        public int SalesOrderID;
        public int SalesOrderYear;
        public String SalesOrderCustomer;
        public DateTime SalesOrderDate;
        public String SalesOrderNotes;
        public String SalesOrderExternalID;
        public int ProductionOrderID;
        public int ProductionOrderYear;
        public int ProductionOrderProductTypeID;
        public int ProductionOrderProductTypeReview;
        public int ProductionOrderProductID;
        public String ProductionOrderSerialNumber;
        public Char ProductionOrderStatus;
        public int ProductionOrderDepartmentID;
        public DateTime ProductionOrderStartTime;
        public DateTime ProductionOrderDeliveryDate;
        public DateTime ProductionOrderEndProductionDate;
        public String ProductionOrderPlanner;
        public int ProductionOrderQuantityOrdered;
        public int ProductionOrderQuantityProduced;
        public String ProductionOrderKanbanCardID;
        public int ProductTypeID;
        public int ProductTypeReview;
        public DateTime ProductTypeReviewDate;
        public String ProductTypeName;
        public String ProductTypeDescription;
        public Boolean ProductTypeEnabled;
        public int ProductID;
        public String ProductName;
        public String ProductDescription;
        public int DepartmentID;
        public String DepartmentName;
        public String DepartmentDescription;
        public Double DepartmentTaktTime;
        public String DepartmentTimeZone;
        public TimeSpan RealWorkingTime;
        public TimeSpan RealDelay;
        public TimeSpan RealLeadTime;
        public DateTime ProductionOrderEndProductionDateReal;
        public String ProductExternalID;
        public String MeasurementUnit;
    }
}