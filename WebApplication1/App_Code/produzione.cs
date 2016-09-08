﻿/* CHANGELOG
 * 20130714
 * Aggiunta proprietà Ritardo nella classe TaskProduzione
 * Modificato metodo generateWarning nella classe taskproduzione --> Scrive una riga anche nella tabella registroeventiproduzione
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using KIS;
using KIS.Commesse;
using KIS.App_Code;

namespace KIS
{
    
    public class prodotto
    {
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
                    string strSQL = "UPDATE productionPlan SET status = '" + value + "' WHERE matricola = '" + this.matricola + "'";
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    this._status = value;
                }
            }
        }
 
        public prodotto()
        {
            this._matricola = "";
            this._RepartoID = -1;
            this.modello = null;
        }

        public prodotto(String prodID, ProcessoVariante mdl)
        {
            
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT matricola, processo, revisione, variante, status, reparto, startTime FROM productionPlan "
                + "WHERE matricola LIKE '" + prodID + "' AND processo = " + mdl.process.processID.ToString()
                + " AND revisione = " + mdl.process.revisione.ToString()
                + " AND variante = " + mdl.variant.idVariante.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                
                processo prc = new processo(rdr.GetInt32(1), rdr.GetInt32(2));
                variante vr = new variante(rdr.GetInt32(3));
                if (prc.processID != -1 && vr.idVariante != -1)
                {
                    modello = new ProcessoVariante(prc, vr);
                    if (modello.process != null && modello.variant != null)
                    {
                        this._matricola = rdr.GetString(0);
                        this._status = rdr.GetChar(4);
                        this._RepartoID = rdr.GetInt32(5);
                        this._dataIniziale = rdr.GetDateTime(6);
                        Reparto rp = new Reparto(this.RepartoID);
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
            rdr.Close();
            conn.Close();
        }

        private DateTime _dataIniziale;
        public DateTime dataIniziale
        {
            get
            {
                TimeZoneInfo tz = null;
                if (this.RepartoID != -1)
                { Reparto rp = new Reparto(this.RepartoID);
                    tz = rp.tzFusoOrario;
                }
                else
                {
                    FusoOrario fuso = new FusoOrario();
                    tz = fuso.tzFusoOrario;
                }
                return TimeZoneInfo.ConvertTimeFromUtc(this._dataIniziale, tz);
            }
        }
    }
    
    public class TaskProduzione
    {
        public String log;
        public String logP {get { return this.log; } }

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
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyStart, rp.tzFusoOrario); }
            set
            {
                Reparto rp = new Reparto(this.RepartoID);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE tasksproduzione SET earlyStart = '" 
                    + TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss")
                    + "' WHERE taskid = " + this.TaskProduzioneID.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EarlyStart = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                    log = ex.Message;
                }
                conn.Close();
            }
        }

        private DateTime _LateStart;
        public DateTime LateStart
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateStart, rp.tzFusoOrario);
            }
            set
            {
                Reparto rp = new Reparto(this.RepartoID);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE tasksproduzione SET lateStart = '" + TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss")
                    + "' WHERE taskid = " + this.TaskProduzioneID.ToString();
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
        private DateTime _EarlyFinish;
        public DateTime EarlyFinish
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyFinish, rp.tzFusoOrario); }
            set
            {
                Reparto rp = new Reparto(this.RepartoID);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE tasksproduzione SET earlyFinish = '" 
                    + TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss")
                    + "' WHERE taskid = " + this.TaskProduzioneID.ToString();
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
        private DateTime _LateFinish;
        public DateTime LateFinish
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateFinish, rp.tzFusoOrario); }
            set
            {
                Reparto rp = new Reparto(this.RepartoID);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE tasksproduzione SET lateFinish = '" 
                    + TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss")
                    + "' WHERE taskid = " + this.TaskProduzioneID.ToString();
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
        private DateTime _StartEffettivo;
        public DateTime StartEffettivo
        {
            get
            {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._StartEffettivo, rp.tzFusoOrario);
            }
        }
        private DateTime _FinishEffettivo;
        public DateTime FinishEffettivo
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._FinishEffettivo, rp.tzFusoOrario); }
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
        public ProcessoVariante OriginalProcessoVariante
        {
            get
            {
                return new ProcessoVariante(new processo(this.OriginalTask, this.OriginalTaskRevisione), new variante(this.VarianteID));
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
                Postazione p = new Postazione(PostazioneID);
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE tasksproduzione SET qtaProdotta = " + value.ToString() +
                        " WHERE taskID= " + this.TaskProduzioneID.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._QuantitaProdotta = value;
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

        public TaskProduzione(int tskProdID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID, name, description, earlyStart, lateStart, earlyFinish, lateFinish, "
                + " origTask, revOrigTask, variante, reparto, postazione, status, idArticolo, annoArticolo, "
                + " nOperatori, tempoCiclo, qtaPrevista, qtaProdotta FROM tasksproduzione WHERE taskID = " + tskProdID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._TaskProduzioneID = rdr.GetInt32(0);
                this._Name = rdr.GetString(1);
                this._Description = rdr.GetString(2);
                this._EarlyStart = rdr.GetDateTime(3);
                this._LateStart = rdr.GetDateTime(4);
                this._EarlyFinish = rdr.GetDateTime(5);
                this._LateFinish = rdr.GetDateTime(6);
                this._OriginalTask = rdr.GetInt32(7);
                this._OriginalTaskRevisione = rdr.GetInt32(8);
                this._VarianteID = rdr.GetInt32(9);
                this._RepartoID = rdr.GetInt32(10);
                this._PostazioneID = rdr.GetInt32(11);
                this._Status = rdr.GetChar(12);
                this._ArticoloID = rdr.GetInt32(13);
                this._ArticoloAnno = rdr.GetInt32(14);
                this._NumOperatori = rdr.GetInt32(15);
                this._TempoCiclo = rdr.GetTimeSpan(16);
                this._QuantitaPrevista = rdr.GetInt32(17);
                this._QuantitaProdotta = rdr.GetInt32(18);
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
            }
            rdr.Close();
            conn.Close();
        }
    
        // Precedenti
        private List<int> _IdPrecedenti;
        public List<int> IdPrecedenti
        {
            get { return this._IdPrecedenti; }
        }

        public void loadPrecedenti()
        {
            this._IdPrecedenti = new List<int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT prec FROM prectasksproduzione WHERE succ = " + this.TaskProduzioneID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._IdPrecedenti.Add(rdr.GetInt32(0));
            }
            rdr.Close();
            conn.Close();
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                + " ORDER BY data";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Eventi.Add(new EventoTaskProduzione(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        // Utenti attivi
        private List<String> _UtentiAttivi;
        public List<String> UtentiAttivi
        {
            get { return this._UtentiAttivi; }
        }

        public void loadUtentiAttivi()
        {
            this._UtentiAttivi = new List<String>();
            List<String> utentiNonAttivi = new List<String>();
            this.loadEventi();
            
            for (int i = this.Eventi.Count - 1; i >= 0; i--)
            {
                
                if (this.Eventi[i].Evento == 'P')
                {
                    utentiNonAttivi.Add(this.Eventi[i].User);
                }
                else if (this.Eventi[i].Evento == 'I')
                {
                    bool found = false;
                    // Ricerco tra la lista degli inattivi
                    for (int j = 0; j < utentiNonAttivi.Count; j++)
                    {
                        if (this.Eventi[i].User == utentiNonAttivi[j])
                        {
                            found = true;
                        }
                    }
                    // Se non l'ho trovato, aggiungo a lista utenti inattivi
                    if (found == false)
                    {
                        this._UtentiAttivi.Add(this.Eventi[i].User);
                    }
                }
            }
        }

        // Utenti che hanno lavorato o stanno lavorato su questo task
        private List<String> _Operatori;
        public List<String> Operatori
        {
            get { return this._Operatori; }
        }

        public void loadOperatori()
        {
            this._Operatori = new List<String>();
            List<String> utentiNonAttivi = new List<String>();
            this.loadEventi();

            for (int i =0; i < this.Eventi.Count; i++)
            {
                utentiNonAttivi.Add(this.Eventi[i].User);
            }
            this._Operatori = new List<String>(utentiNonAttivi.Distinct());
        }

        public bool Start(User usr)
        {
            bool rt = false;
            if (this.TaskProduzioneID != -1 && this.Status != 'F')
            {
                // Controllo che l'utente sia in postazione...
                Postazione p = new Postazione(this.PostazioneID);
                p.loadUtentiLoggati();
                bool controlloUtente = false;
                for (int i = 0; i < p.UtentiLoggati.Count; i++)
                {
                    if (p.UtentiLoggati[i] == usr.username)
                    {
                        controlloUtente = true;
                    }
                }

                bool controlloUltimaAzione = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                + " AND user = '" + usr.username + "' ORDER BY data desc";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    if (rdr.GetChar(0) != 'I')
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
                rdr.Close();
                log += "Controllo utente: " + controlloUtente.ToString() + " Controllo ultima azione: " + controlloUltimaAzione.ToString() + "<br />";

                // Controllo che tutti i precedenti siano terminati
                bool checkPrecedenti = true;
                this.loadPrecedenti();
                for (int i = 0; i < this.IdPrecedenti.Count; i++)
                {
                    TaskProduzione precedente = new TaskProduzione(this.IdPrecedenti[i]);
                    if (precedente.Status != 'F')
                    {
                        checkPrecedenti = false;
                    }
                }

                bool controlloTasksAvviatiUtente = true;
                Reparto rp = new Reparto(this.RepartoID);
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

                log += controlloUtente.ToString() + " " + controlloUltimaAzione.ToString() + " " + checkPrecedenti.ToString() + " " + controlloTasksAvviatiUtente.ToString();

                if (controlloUtente == true && controlloUltimaAzione == true && checkPrecedenti == true && controlloTasksAvviatiUtente == true)
                {
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "SELECT MAX(id) FROM registroeventitaskproduzione";
                    int maxID = 0;
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        maxID = rdr.GetInt32(0) + 1;
                    }
                    rdr.Close();
                    try
                    {
                        cmd.CommandText = "INSERT INTO registroeventitaskproduzione(id, user, task, data, evento, note) VALUES("
                            + maxID.ToString() + ", '" + usr.username + "', " + this.TaskProduzioneID.ToString() + ", '"
                            + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', 'I', '')";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "UPDATE tasksproduzione SET status = 'I' WHERE taskID = " + this.TaskProduzioneID.ToString();
                        cmd.ExecuteNonQuery();
                        Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
                        if (art.Status == 'P')
                        {
                            cmd.CommandText = "UPDATE productionplan SET status='I' WHERE id = " + art.ID.ToString() +
                                " AND anno = " + art.Year.ToString();
                            cmd.ExecuteNonQuery();
                        }

                        tr.Commit();
                        rt = true;
                    }
                    catch (Exception ex)
                    {
                        rt = false;
                        //log += ex.Message;
                        tr.Rollback();
                    }
                }
                conn.Close();
            }
            return rt;
        }

        public bool Pause(User usr)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            // Verifico che l'ultima azione per questo utente sia di Inizio del task
            bool check = false;
            cmd.CommandText = "SELECT evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                + " AND user = '" + usr.username + "' ORDER BY data desc";
            MySqlDataReader rdr = cmd.ExecuteReader();
            
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                if (rdr.GetChar(0) == 'I')
                {
                    check = true;
                }
            }
            rdr.Close();
            if (check == true)
            {
                cmd.CommandText = "SELECT MAX(id) FROM registroeventitaskproduzione";
                int maxID = 0;
                rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
                try
                {
                    cmd.CommandText = "INSERT INTO registroeventitaskproduzione(id, user, task, data, evento, note) VALUES("
                            + maxID.ToString() + ", '" + usr.username + "', " + this.TaskProduzioneID.ToString() + ", '"
                            + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', 'P', '')";
                    cmd.ExecuteNonQuery();
                    this.loadUtentiAttivi();
                    if (this.UtentiAttivi.Count == 0 || (this.UtentiAttivi.Count == 1 && this.UtentiAttivi[0] == usr.username))
                    {
                        cmd.CommandText = "UPDATE tasksproduzione SET status = 'P' WHERE taskID = " + this.TaskProduzioneID.ToString();
                        cmd.ExecuteNonQuery();
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
            return rt;
        }

        public bool Complete(User usr)
        {
            bool rt = false;
            if (this.Status != 'F')
            {
                // Controllo che l'utente sia loggato
                this.loadUtentiAttivi();
                bool checkUtenteAttivo = false;
                for (int i = 0; i < this.UtentiAttivi.Count; i++)
                {
                    if (usr.username == this.UtentiAttivi[i])
                    {
                        checkUtenteAttivo = true;
                    }
                }
                bool controlloUltimaAzione = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                + " AND user = '" + usr.username + "' ORDER BY data desc";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    if (rdr.GetChar(0) == 'I')
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
                rdr.Close();

                this.log = "checkUtenteAttivo: " + checkUtenteAttivo.ToString() + " " + "controlloUltimaAzione: " + controlloUltimaAzione.ToString() + "<br/>";
                if (checkUtenteAttivo == true && controlloUltimaAzione == true)
                {
                    
                    MySqlTransaction tr = conn.BeginTransaction();
                    
                    cmd.Transaction = tr;
                    try
                    {
                        // Termino tutti gli utenti attivi
                        this.loadUtentiAttivi();
                        for (int i = 0; i < this.UtentiAttivi.Count; i++)
                        {
                            int idEv = 0;
                            cmd.CommandText = "SELECT MAX(id) FROM registroeventitaskproduzione";
                            rdr = cmd.ExecuteReader();
                            if (rdr.Read() && !rdr.IsDBNull(0))
                            {
                                idEv = rdr.GetInt32(0) + 1;
                            }
                            rdr.Close();
                            cmd.CommandText = "INSERT INTO registroeventitaskproduzione(id, user, task, data, evento, note) VALUES("
                                + idEv.ToString() + ", '" + this.UtentiAttivi[i] + "', " + this.TaskProduzioneID.ToString()
                                + ", '" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', 'F', '')";
                            cmd.ExecuteNonQuery();
                        }
                        // Imposto lo stato del task a terminato
                        cmd.CommandText = "UPDATE tasksproduzione SET status = 'F' WHERE taskID = " + this.TaskProduzioneID.ToString();
                        cmd.ExecuteNonQuery();

                        // Se tutti i task sono conclusi, allora imposto l'articolo come terminato!
                        bool controlloFineTasks = true;
                        Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
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
                            cmd.CommandText = "UPDATE productionplan SET status = 'F', quantitaProdotta="+this.QuantitaProdotta.ToString()+" WHERE id = " + this.ArticoloID.ToString()
                                + " AND anno = " + this.ArticoloAnno.ToString();
                            cmd.ExecuteNonQuery();

                            // Segnalo a kanbanbox che ho finito il mio mestiere!
                            KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                            Reparto rp = new Reparto(this.RepartoID);
                            Cliente cust = new Cliente(art.Cliente);
                            if(rp.id!=-1 && cust.CodiceCliente.Length > 0)
                            {
                            if (kboxCfg!=null && art!=null && kboxCfg.KanbanBoxEnabled && art.KanbanCardID.Length > 0 && (rp.KanbanManaged || cust.KanbanManaged))
                            {
                                art.changeKanbanBoxCardStatus("full");
                                log +=  "<br />" + art.log;
                            }
                            }
                        }


                        tr.Commit();
                        rt = true;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        rt = false;
                        tr.Rollback();
                    }
                    
                }
                conn.Close();
            }
            return rt;
        }

        public TimeSpan TempoDiLavoroEffettivo
        {
            get
            {
                TimeSpan tc = new TimeSpan(0, 0, 0);
                if (this.TaskProduzioneID != -1 && this.RepartoID!=-1)
                {
                    Reparto rp = new Reparto(this.RepartoID);
                    log = "<br/><B>" + rp.id.ToString() + "</b> " + rp.ModoCalcoloTC.ToString() + "<br />";
                    if (rp.ModoCalcoloTC == false)
                    {
                        // Calcolo il tempo di lavoro NON tenendo conto degli intervalli produttivi
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                            + " ORDER BY user, data";
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            DateTime inizio = rdr.GetDateTime(1);
                            String usrI = rdr.GetString(0);
                            Char EventoI = rdr.GetChar(2);
                            if (rdr.Read())
                            {
                                log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                                String usrF = rdr.GetString(0);
                                Char EventoF = rdr.GetChar(2);
                                DateTime fine = rdr.GetDateTime(1);
                                if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                                {

                                    tc += (fine - inizio);
                                    log += (fine - inizio).ToString();
                                    log += tc.ToString() + "--> OK<br/><br/>";
                                }
                                else // RAMO AGGIUNTO PER EVITARE CHE SE CI SONO FASI IN STATO "I", QUESTE PORTINO IL CONTO A 0
                                {
                                    rdr.Read();
                                }
                            }
                        }
                        conn.Close();
                    }
                    else
                    {
                        // Tengo conto degli intervalli produttivi
                        MySqlConnection conn = (new Dati.Dati()).mycon();
                        conn.Open();
                        List<DateTime[]> elenco = new List<DateTime[]>();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                            + " ORDER BY user, data";
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        log += "TENGO CONTO DEGLI INTERVALLI DI LAVORO<BR/>";
                        while (rdr.Read())
                        {
                            //log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            DateTime inizio = rdr.GetDateTime(1);
                            String usrI = rdr.GetString(0);
                            Char EventoI = rdr.GetChar(2);
                            if (rdr.Read())
                            {
                                //log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                                String usrF = rdr.GetString(0);
                                Char EventoF = rdr.GetChar(2);
                                DateTime fine = rdr.GetDateTime(1);
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
                                    rdr.Read();
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
                        conn.Close();
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
                Reparto rp = new Reparto(this.RepartoID);
                log = "<br/><B>" + rp.id.ToString() + "</b> " + rp.ModoCalcoloTC.ToString() + "<br />";
                if (rp.ModoCalcoloTC == false)
                {
                    // Calcolo il tempo di lavoro NON tenendo conto degli intervalli produttivi
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                        + " AND data >= '" + startDate.ToString("yyyy/MM/dd HH:mm:ss") + "' "
                        + "AND data <='" + endDate.ToString("yyyy/MM/dd HH:mm:ss") + "'"
                        + " ORDER BY user, data";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                        DateTime inizio = rdr.GetDateTime(1);
                        String usrI = rdr.GetString(0);
                        Char EventoI = rdr.GetChar(2);
                        if (rdr.Read())
                        {
                            log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            String usrF = rdr.GetString(0);
                            Char EventoF = rdr.GetChar(2);
                            DateTime fine = rdr.GetDateTime(1);
                            if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                            {

                                tc += (fine - inizio);
                                log += (fine - inizio).ToString();
                                log += tc.ToString() + "--> OK<br/><br/>";
                            }
                        }
                    }
                    conn.Close();
                }
                else
                {
                    // Tengo conto degli intervalli produttivi
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    List<DateTime[]> elenco = new List<DateTime[]>();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                        + " ORDER BY user, data";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    log += "TENGO CONTO DEGLI INTERVALLI DI LAVORO<BR/>";
                    while (rdr.Read())
                    {
                        //log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                        DateTime inizio = rdr.GetDateTime(1);
                        String usrI = rdr.GetString(0);
                        Char EventoI = rdr.GetChar(2);
                        if (rdr.Read())
                        {
                            //log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            String usrF = rdr.GetString(0);
                            Char EventoF = rdr.GetChar(2);
                            DateTime fine = rdr.GetDateTime(1);
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
                    conn.Close();
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
                    this.loadEventi();
                    for (int i = 0; i < this.Eventi.Count; i++)
                    {
                        if (this.Eventi[i].Evento == 'I' && this.Eventi[i].Data < min)
                        {
                            min = this.Eventi[i].Data;
                        }
                    }
                }
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(min, rp.tzFusoOrario);
            }
        }

        /* Data effettiva di fine task */
        public DateTime DataFineTask
        {
            get
            {
                DateTime max = new DateTime(1970, 1, 1);
                if (this.Status == 'F')
                {
                    this.loadEventi();
                    for (int i = 0; i < this.Eventi.Count; i++)
                    {
                        if (this.Eventi[i].Evento == 'F')
                        {
                            max = this.Eventi[i].Data;
                        }
                    }
                }
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(max, rp.tzFusoOrario);
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd= conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM warningproduzione WHERE task = " + this.TaskProduzioneID.ToString() +
                " ORDER BY dataChiamata";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Warnings.Add(new Warning(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public bool generateWarning(User usr)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            try
            {
                int maxID = 0;
                cmd.CommandText = "SELECT max(id) FROM warningproduzione";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
                cmd.CommandText = "INSERT INTO warningproduzione(id, dataChiamata, task, user) VALUES("
                    + maxID.ToString() + ", '" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', " + this.TaskProduzioneID.ToString()
                    + ", '" + usr.username + "')";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO registroeventiproduzione(TipoEvento, taskID, segnalato) VALUES('Warning', "
                    + this.TaskProduzioneID.ToString() + ", false)";
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT warningproduzione.id FROM warningproduzione WHERE warningproduzione.dataRisoluzione IS NULL "
                + " AND task = " + this.TaskProduzioneID.ToString() + " ORDER BY warningproduzione.dataChiamata";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._WarningAperti.Add(new Warning(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        /* Questo algoritmo si comporta in tale modo:
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
                Reparto rp = new Reparto(this.RepartoID);
                log = "";
                TimeSpan rit = new TimeSpan(0, 0, 0);
                if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) <= this.LateStart || (this.Status == 'F' && this.DataInizioTask <= this.LateStart))
                {
                    rit = new TimeSpan(0, 0, 0);
                }
                else
                {                    
                    this.loadIntervalliDiLavoroEffettivi();
                    if (Status == 'N')
                    {
                        DateTime df = new DateTime();
                            df = DateTime.UtcNow;
                            rit = df - this.LateStart;
                        rp.loadCalendario(this.LateStart.AddDays(-5), df.AddDays(5));
                        int indInizio = -1;
                        int indFine = -1;
                        bool foundFine = false;
                        for (int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
                        {
                            //log += i.ToString() + "<br />";
                            if (rp.CalendarioRep.Intervalli[i].Inizio <= this.LateStart && this.LateStart <= rp.CalendarioRep.Intervalli[i].Fine)
                            {
                                //log += "indInizio: " + i.ToString() + " " + rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>";
                                indInizio = i;
                            }
                            else if ((i+1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < this.LateStart && this.LateStart < rp.CalendarioRep.Intervalli[i + 1].Inizio)
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
                                    DateTime prevFine = this.LateFinish > rp.CalendarioRep.Intervalli[indInizio].Fine ? this.LateFinish : rp.CalendarioRep.Intervalli[indInizio].Fine;
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
                        this.loadIntervalliDiLavoroEffettivi();
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
                        DateTime uno = df < this.LateFinish ? df : this.LateFinish;
                        DateTime due = df < this.LateFinish ? this.LateFinish : df;
                        //rp.loadCalendario(this.LateStart.AddDays(-1), df.AddDays(1));
                        rp.loadCalendario(uno.AddDays(-5),due.AddDays(5));
                        int indInizio = -1;
                        int indFine = -1;
                        bool foundFine = false;
                        //try
                        //{
                        log = rit.TotalHours.ToString() + "<br />"
                            + uno.ToString("dd/MM/yyyy HH:mm:ss")
                            + due.ToString("dd/MM/yyyy HH:mm:ss")
                            + this.TaskProduzioneID.ToString() 
                            + " " + this.LateFinish.ToString("dd/MM/yyyy HH:mm:ss")
                            + " " + df.ToString("dd/MM/yyyy HH:mm:ss")
                            + "<br />";
                            for (int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
                        {
                                log+=rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " "
                                + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                                if (rp.CalendarioRep.Intervalli[i].Inizio <= this.LateFinish && this.LateFinish <= rp.CalendarioRep.Intervalli[i].Fine)
                                {
                                    log += " A";
                                    indInizio = i;
                                }
                                else if((i+1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < this.LateFinish && this.LateFinish < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                                {
                                    log += " B";
                                    indInizio = i;
                                    foundFine = true;
                                }
                                if (rp.CalendarioRep.Intervalli[i].Inizio <= df && df <= rp.CalendarioRep.Intervalli[i].Fine)
                                {
                                    log += " C";
                                    indFine = i;
                                    foundFine = true;
                                }
                                else if ((i + 1) < rp.CalendarioRep.Intervalli.Count && rp.CalendarioRep.Intervalli[i].Fine < df && df < rp.CalendarioRep.Intervalli[i + 1].Inizio)
                                {
                                    log += " D";
                                    indFine = i;
                                    foundFine = true;
                                }
                                log += "<br/>";
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //   this.log += ex.Message;
                        //    rit = new TimeSpan(0, 0, 0);
                        //}

                        log += foundFine.ToString() + " " + indInizio.ToString() + " " + indFine.ToString() + "<br />";
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
                Reparto rp = new Reparto(this.RepartoID);
                if (rp.ModoCalcoloTC == false)
                {
                    log = "ENTRO IN LOADINTERVALLI<br />";
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                         + " ORDER BY user, data";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                        DateTime inizio = rdr.GetDateTime(1);
                        String usrI = rdr.GetString(0);
                        Char EventoI = rdr.GetChar(2);
                        if (rdr.Read())
                        {
                            log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            String usrF = rdr.GetString(0);
                            Char EventoF = rdr.GetChar(2);
                            DateTime fine = rdr.GetDateTime(1);
                            if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                            {
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = usrI;
                                curr.Inizio = inizio;
                                curr.Fine = fine;
                                curr.Intervallo = fine - inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                Postazione pst = new Postazione(this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                        }

                    }
                    conn.Close();
                }
                else
                {
                    // Carica gli intervalli di lavoro tenendo conto degli intervalli produttivi
                    // TO-DO!!!

                    List<IntervalliDiLavoroEffettivi> elenco = new List<IntervalliDiLavoroEffettivi>();

                    log = "ENTRO IN LOADINTERVALLI<br />";
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT user, data, evento FROM registroeventitaskproduzione WHERE task = " + this.TaskProduzioneID.ToString()
                         + " ORDER BY user, data";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        log += "1-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                        DateTime inizio = rdr.GetDateTime(1);
                        String usrI = rdr.GetString(0);
                        Char EventoI = rdr.GetChar(2);
                        if (rdr.Read())
                        {
                            log += "2-Evento: " + rdr.GetChar(2) + " " + rdr.GetDateTime(1) + "<br />";
                            String usrF = rdr.GetString(0);
                            Char EventoF = rdr.GetChar(2);
                            DateTime fine = rdr.GetDateTime(1);
                            if (fine >= inizio && EventoI == 'I' && (EventoF == 'P' || EventoF == 'F') && usrI == usrF)
                            {
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = usrI;
                                curr.Inizio = inizio;
                                curr.Fine = fine;
                                curr.Intervallo = fine - inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                Postazione pst = new Postazione(this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                elenco.Add(curr);
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
                                curr.user = elenco[i].Username;
                                curr.Inizio = rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.Fine = rp.CalendarioRep.Intervalli[j].Fine;
                                curr.Intervallo = rp.CalendarioRep.Intervalli[j].Fine - rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                Postazione pst = new Postazione(this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio >= elenco[i].Inizio && elenco[i].Inizio <= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i].Fine)
                            {
                                //tc += (elenco[i].Fine - rp.CalendarioRep.Intervalli[j].Inizio);
                                //log += "if4 " + tc.TotalHours.ToString() + "<br />";
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = elenco[i].Username;
                                curr.Inizio = rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.Fine = elenco[i].Fine;
                                curr.Intervallo = elenco[i].Fine - rp.CalendarioRep.Intervalli[j].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                Postazione pst = new Postazione(this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i].Inizio && elenco[i].Fine >= rp.CalendarioRep.Intervalli[j].Fine && rp.CalendarioRep.Intervalli[j].Fine >= elenco[i].Inizio)
                            {
                                //tc += (rp.CalendarioRep.Intervalli[j].Fine - elenco[i].Inizio);
                                //log += "if5 " + tc.TotalHours.ToString() + "<br />";
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = elenco[i].Username;
                                curr.Inizio = elenco[i].Inizio;
                                curr.Fine = rp.CalendarioRep.Intervalli[j].Fine;
                                curr.Intervallo = rp.CalendarioRep.Intervalli[j].Fine - elenco[i].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                Postazione pst = new Postazione(this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                            else if (rp.CalendarioRep.Intervalli[j].Inizio <= elenco[i].Inizio && elenco[i].Fine <= rp.CalendarioRep.Intervalli[j].Fine)
                            {
                                //tc += (elenco[i].Fine - elenco[i].Inizio);
                                //log += "if6 " + tc.TotalHours.ToString() + "<br />";
                                IntervalliDiLavoroEffettivi curr = new IntervalliDiLavoroEffettivi();
                                curr.user = elenco[i].Username;
                                curr.Inizio = elenco[i].Inizio;
                                curr.Fine = elenco[i].Fine;
                                curr.Intervallo = elenco[i].Fine - elenco[i].Inizio;
                                curr.TaskID = this.TaskProduzioneID;
                                curr.idPostazione = this.PostazioneID;
                                Postazione pst = new Postazione(this.PostazioneID);
                                curr.nomePostazione = pst.name;
                                curr.nomePostazione = this.Name;
                                curr.idProdotto = this.ArticoloID;
                                curr.annoProdotto = this.ArticoloAnno;
                                Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
                                curr.nomeProdotto = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                                this.Intervalli.Add(curr);
                            }
                        }
                    }


                    conn.Close();
                }
            }
        }

        //private TimeSpan _TempoCicloEffettivo;
        public TimeSpan TempoCicloEffettivo
        {
            get
            {
                TimeSpan tc = new TimeSpan(0, 0, 0);
                Reparto rp = new Reparto(this.RepartoID);
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.CommandText = "DELETE FROM registroeventiproduzione WHERE taskID=" + this.TaskProduzioneID.ToString()
                + " AND TipoEvento LIKE 'Ritardo'";
            try
            {
                cmd.ExecuteNonQuery();
                ret = true;
                log = cmd.CommandText;
                tr.Commit();
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

        public Boolean Riesuma()
        {
            Boolean res = false;
            if (this.TaskProduzioneID != -1 && this.Status == 'F')
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                        cmd.CommandText = "UPDATE registroeventitaskproduzione SET evento = 'P' WHERE "
                        + "task = " + this.TaskProduzioneID.ToString()
                        + " AND evento = 'F'";
                        cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE tasksproduzione SET status='P' WHERE taskID = " + this.TaskProduzioneID.ToString();
                    cmd.ExecuteNonQuery();

                    tr.Commit();
                    res = true;
                }
                catch (Exception ex)
                {
                    this.log = ex.Message;
                    res = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return res;
        }
    }

    public class ProductionPlan
    {
        public String err;

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

        public ProductionPlan(Reparto rp)
        {
            if (rp.id != -1)
            {
                this._RepartoID = rp.id;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT matricola, processo, revisione, variante FROM ProductionPlan WHERE reparto = " + rp.id.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                _ElencoCommesse = new List<prodotto>();
                while (rdr.Read())
                {
                    processo prc = new processo(rdr.GetInt32(1), rdr.GetInt32(2));
                    variante vr = new variante(rdr.GetInt32(3));
                    ProcessoVariante prvr = new ProcessoVariante(prc, vr);
                    if (prvr.process != null && prvr.variant != null)
                    {
                        this._ElencoCommesse.Add(new prodotto(rdr.GetString(0), prvr));
                    }
                }
                rdr.Close();
                conn.Close();

            }
        }

        public bool addProduct(String matricola, ProcessoVariante vr)
        {
            bool rt = false;
            if (this.RepartoID != -1 && matricola.Length > 0 && vr.process != null && vr.variant != null)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                
                // Ricerco la disponibilità della postazione per il primo processo del percorso critico per capire quando far partire il prodotto
                vr.process.calculateCriticalPath(vr.variant);

                RepartoProcessoPostazione p1 = new RepartoProcessoPostazione(this.RepartoID, new TaskVariante(vr.process.CriticalPath[0], vr.variant));
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
                conn.Close();
            }
            return rt;
        }
    }
    
    public class TaskConfigurato
    {
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

        /* Date per EarlyStart, LateStart, EarlyFinish, LateFinish */
        private DateTime _EarlyStartDate;
        public DateTime EarlyStartDate
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyStartDate, rp.tzFusoOrario); }
            set {
                Reparto rp = new Reparto(this.RepartoID);
                this._EarlyStartDate = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario); }
        }
        private DateTime _LateStartDate;
        public DateTime LateStartDate
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateStartDate, rp.tzFusoOrario);
            }
            set {
                Reparto rp = new Reparto(this.RepartoID);
                this._LateStartDate = TimeZoneInfo.ConvertTimeToUtc(value);
            }
        }
        private DateTime _EarlyFinishDate;
        public DateTime EarlyFinishDate
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._EarlyFinishDate, rp.tzFusoOrario);
            }
            set {
                Reparto rp = new Reparto(this.RepartoID);
                this._EarlyFinishDate = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
            }
        }
        private DateTime _LateFinishDate;
        public DateTime LateFinishDate
        {
            get {
                Reparto rp = new Reparto(this.RepartoID);
                return TimeZoneInfo.ConvertTimeFromUtc(this._LateFinishDate, rp.tzFusoOrario); }
            set {
                Reparto rp = new Reparto(this.RepartoID);
                this._LateFinishDate = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
            }
        }
        private bool _InseribileInCalendario;
        public bool InseribileInCalendario
        {
            get { return this._InseribileInCalendario; }
            set { this._InseribileInCalendario = value; }
        }

        public TaskConfigurato(TaskVariante tsk, TempoCiclo tc, int repID, int qty)
        {
            Precedenti = new List<int>();
            Successivi = new List<int>();
            this.PrecedentiPausa = new List<TimeSpan>();
            this.SuccessiviPausa = new List<TimeSpan>();
            tsk.loadTempiCiclo();
            Reparto rp = new Reparto(repID);
            if (tc.IdProcesso == tsk.Task.processID && tc.RevisioneProcesso == tsk.Task.revisione && tsk.variant.idVariante == tc.Variante && rp.id != -1)
            {
                this._RepartoID = repID;
                this._Task = tsk;
                this.Tempo = tc;
                this._Quantita = qty;
                
                tsk.Task.loadPrecedenti(tsk.variant);
                tsk.Task.loadSuccessivi(tsk.variant);
                for (int i = 0; i < tsk.Task.processiPrec.Count; i++)
                {
                    Precedenti.Add(tsk.Task.processiPrec[i]);
                    this.PrecedentiPausa.Add(tsk.Task.pausePrec[i]);
                }
                for (int i = 0; i < tsk.Task.processiSucc.Count; i++)
                {
                    Successivi.Add(tsk.Task.processiSucc[i]);
                    this.SuccessiviPausa.Add(tsk.Task.pauseSucc[i]);
                }

                //Carico la postazione di lavoro
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT postazione FROM repartipostazioniattivita WHERE processo = " + this.Task.Task.processID.ToString()
                 + " AND revProc = " + this.Task.Task.revisione.ToString() + " AND variante = " + this.Task.variant.idVariante.ToString()
                 + " AND reparto = " + repID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._PostazioneDiLavoro = new Postazione(rdr.GetInt32(0));
                }
                else
                {
                    this._PostazioneDiLavoro = null;
                }
                rdr.Close();
                conn.Close();
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

        public ConfigurazioneProcesso(Articolo art, List<TaskConfigurato> lst, Reparto rp, int qty)
        {
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
                    Processi.Add(new TaskConfigurato(lst[i].Task, lst[i].Tempo, rp.id, this._Quantita));
                }
            }
        }

        protected bool checkAppartenenzaFigli(ProcessoVariante prc, TaskConfigurato tsk)
        {
            bool rt = false;
            prc.process.loadFigli(prc.variant);
            for (int j = 0; j < prc.process.subProcessi.Count && rt == false; j++)
            {
                if (tsk.Task.Task.processID == prc.process.subProcessi[j].processID && tsk.Task.Task.revisione == prc.process.subProcessi[j].revisione)
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
            Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
            if (art.ID != -1 && TimeZoneInfo.ConvertTimeToUtc(art.DataPrevistaFineProduzione, this.RepartoProduttivo.tzFusoOrario) > DateTime.UtcNow)
            {
                // Per prima cosa, verifico quale è la data di consegna, ed il primo turno utile in cui inserire il task
                Reparto rp = new Reparto(art.Reparto);
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
                                    indEarlyStart = j;//ElIntervalli[j].IntervalloIndex;
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
                Articolo art = new Articolo(this.ArticoloID, this.ArticoloAnno);
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                int maxTaskID = 0;
                cmd.CommandText = "SELECT MAX(taskID) FROM tasksproduzione";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxTaskID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
                try
                {
                    
                    Articolo artcl = new Articolo(this.ArticoloID, this.ArticoloAnno);
                    
                    // Inserisco i task nel piano di produzione
                    for (int i = 0; i < this.Processi.Count; i++)
                    {
                        TimeSpan tcTotale = this.Processi[i].Tempo.TempoSetup + TimeSpan.FromTicks(this.Quantita * this.Processi[i].Tempo.Tempo.Ticks);
                        // Inserisco il task
                        String strSQL = "INSERT INTO tasksproduzione(taskID, name, description, earlyStart, LateStart, "
                            + "earlyFinish, LateFinish, origTask, revOrigTask, variante, reparto, postazione, status, idcommessa, "
                            + " annoCommessa, idArticolo, annoArticolo, nOperatori, tempoCiclo, qtaPrevista, qtaProdotta) VALUES("
                            + maxTaskID.ToString() + ", "
                            + "'" + this.Processi[i].Task.Task.processName + "', "
                            + "'" + this.Processi[i].Task.Task.processDescription + "', "
                            + "'" + TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].EarlyStartDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") + "', "
                            + "'" + TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].LateStartDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") + "', "
                            + "'" + TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].EarlyFinishDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") + "', "
                            + "'" + TimeZoneInfo.ConvertTimeToUtc(this.Processi[i].LateFinishDate, this.RepartoProduttivo.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") + "', "
                            + this.Processi[i].Task.Task.processID.ToString() + ", "
                            + this.Processi[i].Task.Task.revisione.ToString() + ", "
                            + this.MainProcess.variant.idVariante.ToString() + ", "
                            + this.RepartoProduttivo.id.ToString() + ", "
                            + this.Processi[i].PostazioneDiLavoro.id.ToString() + ", "
                            + "'N', "
                            + artcl.Commessa.ToString() + ", "
                            + artcl.AnnoCommessa.ToString() + ", "
                            + artcl.ID.ToString() + ", "
                            + artcl.Year.ToString() + ", "
                            + this.Processi[i].Tempo.NumeroOperatori.ToString() + ", "
                            + "'" + Math.Floor(tcTotale.TotalHours).ToString() //Math.Floor(this.Processi[i].Tempo.Tempo.TotalHours).ToString()
                            + ":" + tcTotale.Minutes.ToString()//this.Processi[i].Tempo.Tempo.Minutes.ToString()
                            + ":" + tcTotale.Seconds.ToString()//this.Processi[i].Tempo.Tempo.Seconds.ToString()
                            + "'"
                            + ", " + this.Quantita.ToString() + ", "
                            + this.Quantita.ToString()
                            + ")";

                        cmd.CommandText = strSQL;
                        cmd.ExecuteNonQuery();

                        // Array di corrispondenza id+revisione processo e taskproduzione
                        int[] idOLDNEW = new int[3];
                        idOLDNEW[0] = this.Processi[i].Task.Task.processID;
                        idOLDNEW[1] = this.Processi[i].Task.Task.revisione;
                        idOLDNEW[2] = maxTaskID;
                        lstTaskIDOLDREVNEW.Add(idOLDNEW);
                        maxTaskID++;
                        log += cmd.CommandText + "<br />";
                    }

                    

                    cmd.CommandText = "UPDATE productionplan SET status = 'P' WHERE id = " + artcl.ID.ToString() + " AND anno = " + artcl.Year.ToString();
                    cmd.ExecuteNonQuery();

                    log += cmd.CommandText;

                    tr.Commit();
                }
                catch(Exception ex)
                {
                    log += ex.Message;
                    tr.Rollback();
                    rt = 0;
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
                                //log += "&nbsp;&nbsp;&nbsp;&nbsp;Precedente inserito: " + newPrecID.ToString() + " - " + newCurrID + "<br/>";
                                cmd.CommandText = "INSERT INTO prectasksproduzione(prec, succ, relazione) VALUES("
                                    + newPrecID.ToString() + ", " + newCurrID.ToString() + ", 0)";
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }
                }
                catch(Exception ex)
                {
                    log += ex.Message;
                }


                conn.Close();
            }

            return rt;
        }
    }

    public class ElencoTaskProduzione
    {
        private List<TaskProduzione> _TaskAvviabili;
        public List<TaskProduzione> TaskAvviabili
        {
            get { return this._TaskAvviabili; }
        }

        public ElencoTaskProduzione()
        {
            this._TaskAvviabili = new List<TaskProduzione>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE status = 'N' OR status = 'I' OR status = 'P' ORDER BY lateStart";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                TaskProduzione tsk = new TaskProduzione(rdr.GetInt32(0));
                
                this._TaskAvviabili.Add(tsk);
            }
            rdr.Close();
            conn.Close();
        }

        private List<TaskProduzione> _Tasks;
        public List<TaskProduzione> Tasks
        {
            get
            {
                return this._Tasks;
            }
        }

        public ElencoTaskProduzione(processo origProc)
        {
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE status LIKE 'F' "
                    + " AND origTask = " + origProc.processID.ToString()
                    + " AND revOrigTask = " + origProc.revisione.ToString()
                    + " ORDER BY lateStart";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Tasks.Add(new TaskProduzione(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public ElencoTaskProduzione(processo origProc, DateTime start, DateTime end)
        {
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE status LIKE 'F' "
                    + " AND origTask = " + origProc.processID.ToString()
                    + " AND revOrigTask = " + origProc.revisione.ToString()
                    + " AND earlyStart > '"+ start.Year.ToString() + "/" + start.Month.ToString() + "/" + start.Day.ToString() +"'"
                    + " AND earlyStart < '" + end.Year.ToString() + "/" + end.Month.ToString() + "/" + end.Day.ToString() + "'"
                    + " ORDER BY lateStart";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Tasks.Add(new TaskProduzione(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public ElencoTaskProduzione(processo origProc, ProcessoVariante ProdottoPadre)
        {
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione INNER JOIN productionplan ON"
                    + "(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
                    + " WHERE tasksproduzione.status LIKE 'F' "
                    + " AND tasksproduzione.origTask = " + origProc.processID.ToString()
                    + " AND tasksproduzione.revOrigTask = " + origProc.revisione.ToString()
                    + " AND productionplan.processo = " + ProdottoPadre.process.processID.ToString()
                    + " AND productionplan.revisione = " + ProdottoPadre.process.revisione.ToString()
                    + " AND productionplan.variante = " + ProdottoPadre.variant.idVariante.ToString()
                    + " ORDER BY lateStart";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Tasks.Add(new TaskProduzione(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public ElencoTaskProduzione(processo origProc, ProcessoVariante ProdottoPadre, DateTime start, DateTime end)
        {
            this._Tasks = new List<TaskProduzione>();
            if (origProc.processID != -1 && origProc.revisione != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione INNER JOIN productionplan ON"
                    + "(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
                    + " WHERE tasksproduzione.status LIKE 'F' "
                    + " AND tasksproduzione.origTask = " + origProc.processID.ToString()
                    + " AND tasksproduzione.revOrigTask = " + origProc.revisione.ToString()
                    + " AND earlyStart > '" + start.Year.ToString() + "/" + start.Month.ToString() + "/" + start.Day.ToString() + "'"
                    + " AND earlyStart < '" + end.Year.ToString() + "/" + end.Month.ToString() + "/" + end.Day.ToString() + "'"
                    + " AND productionplan.processo = " + ProdottoPadre.process.processID.ToString()
                    + " AND productionplan.revisione = " + ProdottoPadre.process.revisione.ToString()
                    + " AND productionplan.variante = " + ProdottoPadre.variant.idVariante.ToString()
                    + " ORDER BY lateStart";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Tasks.Add(new TaskProduzione(rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
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
        private String _User;
        public String User
        {
            get { return this._User; }
        }
        private DateTime _Data;
        public DateTime Data
        {
            get {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._Data, fuso.tzFusoOrario); }
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

        public EventoTaskProduzione(int evID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT user, task, data, evento, note FROM registroeventitaskproduzione WHERE id = " + evID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._IdEvento = evID;
                this._User = rdr.GetString(0);
                this._TaskProduzioneID = rdr.GetInt32(1);
                this._Data = rdr.GetDateTime(2);
                this._Evento = rdr.GetChar(3);
                this._Note = rdr.GetString(4);
            }
            else
            {
                this._IdEvento = -1;
            }
            rdr.Close();
            conn.Close();
        }

    }

    public class Warning
    {
        public String log;
        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private DateTime _DataChiamata;
        public DateTime DataChiamata
        {
            get { FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._DataChiamata, fuso.tzFusoOrario); }
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
            get { FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this._DataRisoluzione, fuso.tzFusoOrario);
            }
            set
            {
                FusoOrario fuso = new FusoOrario();
                this._DataRisoluzione = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE warningproduzione SET dataRisoluzione = '" 
                    + TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") 
                    + "' WHERE id = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _MotivoChiamata;
        public String MotivoChiamata
        {
            get { return this._MotivoChiamata; }
            set
            {
                this._MotivoChiamata = value;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE warningproduzione SET motivo = '" + value + "' WHERE id = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Risoluzione;
        public String Risoluzione
        {
            get { return this._Risoluzione; }
            set 
            {
                this._Risoluzione = value;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE warningproduzione SET risoluzione = '" + value + "' WHERE id = " + this.ID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public bool isOpen
        {
            get
            {
                if (this._DataRisoluzione == (new DateTime(1970,1,1)) || this._MotivoChiamata == null || this._Risoluzione == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Warning(int wrnID)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, dataChiamata, task, user, dataRisoluzione, motivo, risoluzione FROM warningproduzione "
                + "WHERE id = " + wrnID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = rdr.GetInt32(0);
                this._DataChiamata = rdr.GetDateTime(1);
                this._TaskID = rdr.GetInt32(2);
                this._User = rdr.GetString(3);
                if (!rdr.IsDBNull(4))
                {
                    this._DataRisoluzione = rdr.GetDateTime(4);
                }
                else
                {
                    this._DataRisoluzione = new DateTime(1970, 1, 1);
                }
                if (!rdr.IsDBNull(5))
                {
                    this._MotivoChiamata = rdr.GetString(5);
                }
                else
                {
                    this._MotivoChiamata = null;
                }
                if (!rdr.IsDBNull(6))
                {
                    this._Risoluzione = rdr.GetString(6);
                }
                else
                {
                    this._Risoluzione = null;
                }
            }
            rdr.Close();
            conn.Close();
        }

        public String NomePostazione
        {
            get 
            {
                TaskProduzione tsk = new TaskProduzione(this.TaskID);
                return tsk.PostazioneName;
            }
        }

        public String NomeReparto
        {
            get
            {
                TaskProduzione tsk = new TaskProduzione(this.TaskID);
                Reparto rp = new Reparto(tsk.RepartoID);
                return rp.name;
            }
        }
    }

    public class WarningAperti
    {
        private List<Warning> _Elenco;
        public List<Warning> Elenco
        {
            get { return this._Elenco; }
        }

        public WarningAperti()
        {
            this._Elenco = new List<Warning>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM warningproduzione WHERE dataRisoluzione IS NULL ORDER BY dataChiamata";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Elenco.Add(new Warning(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public WarningAperti(Reparto rp)
        {
            this._Elenco = new List<Warning>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id FROM warningproduzione INNER JOIN tasksproduzione ON(warningproduzione.task = tasksproduzione.taskID) "
                + " WHERE dataRisoluzione IS NULL "
                + " AND reparto = " + rp.id.ToString()
                + " ORDER BY dataChiamata";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Elenco.Add(new Warning(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }

        
    }

    public class ElencoArticoliInProduzione
    {
        private List<Articolo> _ElencoArticoli;
        public List<Articolo> ElencoArticoli
        {
            get { return this._ElencoArticoli; }
        }

        public ElencoArticoliInProduzione()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, anno FROM productionplan WHERE status = 'I' OR status = 'P' "
                +" ORDER BY status DESC, anno DESC, id DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._ElencoArticoli = new List<Articolo>();
            while (rdr.Read())
            {
                this._ElencoArticoli.Add(new Articolo(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }
    }

    /* Struttura usata da TaskProduzione */
    public struct IntervalliDiLavoroEffettivi
    {
        public String user;
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
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this.Inizio, fuso.tzFusoOrario);
            }
        }
        public DateTime DataFine
        {
            get
            {
                FusoOrario fuso = new FusoOrario();
                return TimeZoneInfo.ConvertTimeFromUtc(this.Fine, fuso.tzFusoOrario);
            }
        }
        public TimeSpan DurataIntervallo
        {
            get
            {
                return this.Intervallo;
            }
        }
        public String Username
        {
            get
            {
                return this.user;
            }
        }
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
    }

    /* Usata da checkWorkload */
    public struct caricoDiLavoro
    {
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
}