/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2021 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using Dapper;

namespace KIS.App_Code
{
    public class Cliente
    {
        public String Tenant;

        public String log;

        public String ID
        {
            get
            {
                return this._CodiceCliente;
            }
        }

        private String _CodiceCliente;
        public String CodiceCliente
        {
            get
            {
                return this._CodiceCliente;
            }
        }

        private String _RagioneSociale;
        public String RagioneSociale
        {
            get
            {
                return this._RagioneSociale;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "UPDATE anagraficaclienti SET ragsociale = @ragsoc WHERE codice = @codice";
                try
                {
                    conn.Execute(sql, new { ragsoc = value, codice = this.CodiceCliente }, tr);
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _PartitaIVA;
        public String PartitaIVA
        {
            get
            {
                return this._PartitaIVA;
            }
            set
            {
                if (value.Length == 11 || value.Length == 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE anagraficaclienti SET partitaiva = @piva WHERE codice = @codice";
                    try
                    {
                        conn.Execute(sql, new { piva = value, codice = this.CodiceCliente }, tr);
                        tr.Commit();
                        this._PartitaIVA = value;
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

        private String _CodiceFiscale;
        public String CodiceFiscale
        {
            get
            {
                return this._CodiceFiscale;
            }
            set
            {
                if (value.Length == 16 || value.Length==0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE anagraficaclienti SET codfiscale = @codfiscale WHERE codice = @codice";
                    try
                    {
                        conn.Execute(sql, new { codfiscale = value, codice = this.CodiceCliente }, tr);
                        tr.Commit();
                        this._CodiceFiscale = value;
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

        private String _Indirizzo;
        public String Indirizzo
        {
            get
            {
                return this._Indirizzo;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "UPDATE anagraficaclienti SET indirizzo = @indirizzo WHERE codice = @codice";
                try
                {
                    conn.Execute(sql, new { indirizzo = value, codice = this.CodiceCliente }, tr);
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Citta;
        public String Citta
        {
            get
            {
                return this._Citta;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "UPDATE anagraficaclienti SET citta = @citta WHERE codice = @codice";
                try
                {
                    conn.Execute(sql, new { citta = value, codice = this.CodiceCliente }, tr);
                    tr.Commit();
                    this._Citta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Provincia;
        public String Provincia
        {
            get
            {
                return this._Provincia;
            }
            set
            {
                if (value.Length <= 2)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE anagraficaclienti SET provincia = @provincia WHERE codice = @codice";
                    try
                    {
                        conn.Execute(sql, new { provincia = value, codice = this.CodiceCliente }, tr);
                        tr.Commit();
                        this._Provincia = value;
                    }
                    catch (Exception ex)
                    {
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
                else
                {
                    log = "String too long.";
                }
            }
        }

        private String _CAP;
        public String CAP
        {
            get
            {
                return this._CAP;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "UPDATE anagraficaclienti SET CAP = @cap WHERE codice = @codice";
                try
                {
                    conn.Execute(sql, new { cap = value, codice = this.CodiceCliente }, tr);
                    tr.Commit();
                    this._CAP = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Stato;
        public String Stato
        {
            get
            {
                return this._Stato;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "UPDATE anagraficaclienti SET stato = @stato WHERE codice = @codice";
                try
                {
                    conn.Execute(sql, new { stato = value, codice = this.CodiceCliente }, tr);
                    tr.Commit();
                    this._Stato = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private String _Telefono;
        public String Telefono
        {
            get
            {
                return this._Telefono;
            }
            set
            {
                if (value.Length <= 45)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE anagraficaclienti SET telefono = @telefono WHERE codice = @codice";
                    try
                    {
                        conn.Execute(sql, new { telefono = value, codice = this.CodiceCliente }, tr);
                        tr.Commit();
                        this._Telefono = value;
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

        private String _Email;
        public String Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "UPDATE anagraficaclienti SET email = @email WHERE codice = @codice";
                try
                {
                    conn.Execute(sql, new { email = value, codice = this.CodiceCliente }, tr);
                    tr.Commit();
                    this._Email = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public List<Contatto> ElencoContatti;
        public void loadContatti()
        {
            ElencoContatti = new List<Contatto>();
            if (this.CodiceCliente.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT idContatto FROM contatticlienti WHERE cliente = @codice"
                    + " ORDER BY lastname, firstname";
                var rows = conn.Query<int>(sql, new { codice = this.CodiceCliente });
                foreach (var id in rows)
                {
                    this.ElencoContatti.Add(new Contatto(this.Tenant, id));
                }
                conn.Close();
            }
        }

        private Boolean _KanbanManaged;
        public Boolean KanbanManaged
        {
            get 
            {
                return this._KanbanManaged;
            }
            set 
            {
                // Se kanbanbox by sintesia è abilitato e ho già caricato il cliente
                KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                if (this.CodiceCliente != "" && this.CodiceCliente.Length > 0 && kboxCfg.KanbanBoxEnabled == true)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE anagraficaclienti SET kanbanManaged = @kanban"
                        + " WHERE codice = @codice";
                    try
                    {
                        conn.Execute(sql, new { kanban = value, codice = this.CodiceCliente });
                        this._KanbanManaged = value;
                    }
                    catch
                    {
                    }
                    conn.Close();
                }
            }
        }

        public Cliente(String tenant, String cod)
        {
            this.Tenant = tenant;
            this._listCommesse = new List<Commessa>();
            this._IntervalliDiLavoro = new List<IntervalliDiLavoroEffettivi>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT codice, ragsociale, partitaiva, codfiscale, indirizzo, citta, provincia, CAP, "
            + "stato, telefono, email, kanbanManaged FROM anagraficaclienti WHERE codice = @cod";
            var rows = conn.Query<(string codice, string ragsociale, string partitaiva, string codfiscale, string indirizzo, string citta, string provincia, string CAP, string stato, string telefono, string email, bool kanbanManaged)>(sql, new { cod = cod }).ToList();
            if (rows.Count > 0 && rows[0].codice != null)
            {
                this._CodiceCliente = cod;
                this._RagioneSociale = rows[0].ragsociale != null ? rows[0].ragsociale : "";
                this._PartitaIVA = rows[0].partitaiva != null ? rows[0].partitaiva : "";
                this._CodiceFiscale = rows[0].codfiscale != null ? rows[0].codfiscale : "";

                this._Indirizzo = "";
                if (rows[0].indirizzo != null)
                {
                    this._Indirizzo = rows[0].indirizzo;
                }

                this._Citta = "";
                if (rows[0].citta != null)
                {
                    this._Citta = rows[0].citta;
                }

                this._Provincia = "";
                if (rows[0].provincia != null)
                {
                    this._Provincia = rows[0].provincia;
                }

                this._CAP = "";
                if (rows[0].CAP != null)
                {
                    this._CAP = rows[0].CAP;
                }

                this._Stato = "";
                if (rows[0].stato != null)
                {
                    this._Stato = rows[0].stato;
                }

                this._Telefono = "";
                if (rows[0].telefono != null)
                {
                    this._Telefono = rows[0].telefono;
                }

                this._Email = "";
                if (rows[0].email != null)
                {
                    this._Email = rows[0].email;
                }

                this._KanbanManaged = rows[0].kanbanManaged;

                this.loadContatti();

            }
            else
            {
                this._CodiceCliente = "";
                this._RagioneSociale = "";
                this._PartitaIVA = "";
                this._RagioneSociale = "";
            }
            conn.Close();
        }

        /* returns:
         * -1 if some error
         * Contact ID if ok
         */
        public int AddContatto(String firstname, String lastname, String role)
        {
            int rt = -1;
            if (this.CodiceCliente.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sqlMax = "SELECT MAX(idContatto) FROM contatticlienti";
                int maxID = 0;
                int? maxVal = conn.QueryFirstOrDefault<int?>(sqlMax);
                if (maxVal.HasValue)
                {
                    maxID = maxVal.Value + 1;
                }
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO contatticlienti(idContatto, cliente, firstname, lastname, ruolo) VALUES("
                    + "@maxID, "
                    + "@codice, "
                    + "@firstname, "
                    + "@lastname, "
                    + "@role"
                    + ")";
                try
                {
                    conn.Execute(sql, new { maxID = maxID, codice = this.CodiceCliente, firstname = firstname, lastname = lastname, role = role }, tr);
                    tr.Commit();
                    rt = maxID;
                }
                catch(Exception ex)
                {
                    rt = -1;
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return rt;
        }

        private List<Commessa> _listCommesse;
        public List<Commessa> listCommesse
        {
            get
            {
                return this._listCommesse;
            }
        }

        public void loadCommesse(DateTime inizio, DateTime fine)
        {
            this._listCommesse = new List<Commessa>();
            if (this.CodiceCliente.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT commesse.idcommesse, commesse.anno FROM commesse "
                    + " INNER JOIN tasksproduzione ON (commesse.idcommesse = tasksproduzione.idcommessa AND commesse.anno = tasksproduzione.annocommessa) "
                    + " WHERE commesse.cliente = @codice"
                    + " AND tasksproduzione.lateFinish >= @inizio"
                    + " AND earlyStart <= @fine "
                    + " GROUP by commesse.idcommesse, commesse.anno "
                    + "ORDER BY commesse.anno, commesse.idcommesse";
                var rows = conn.Query<(int, int)>(sql, new { codice = this.CodiceCliente, inizio = inizio.ToString("yyyy/MM/dd"), fine = fine.ToString("yyyy/MM/dd") });
                foreach (var r in rows)
                {
                    this._listCommesse.Add(new Commessa(this.Tenant, r.Item1, r.Item2));
                }
                conn.Close();
            }
        }

        private TimeSpan _TempoDiLavoro;
        public TimeSpan TempoDiLavoro
        {
            get
            {
                return this._TempoDiLavoro;
            }
        }
        public Double TempoDiLavoroDbl
        {
            get
            {
                return Math.Round(this._TempoDiLavoro.TotalHours, 2);
            }
        }

        public void loadTempoDiLavoro(DateTime inizio, DateTime fine)
        {
            this._TempoDiLavoro = new TimeSpan(0, 0, 0);
            this.loadCommesse(inizio, fine);
            for (int i = 0; i < this.listCommesse.Count; i++)
            {
                this.listCommesse[i].loadArticoli();
                for (int j = 0; j < this.listCommesse[i].Articoli.Count; j++)
                {
                    this.listCommesse[i].Articoli[j].loadTempoDiLavoroTotale(inizio, fine);
                    this._TempoDiLavoro += this.listCommesse[i].Articoli[j].TempoDiLavoroTotale;
                }
            }
        }

        private List<IntervalliDiLavoroEffettivi> _IntervalliDiLavoro;
        public List<IntervalliDiLavoroEffettivi> IntervalliDiLavoro
        {
            get
            {
                return this._IntervalliDiLavoro;
            }
        }

        public void loadIntervalliDiLavoro(DateTime inizio, DateTime fine)
        {
            this._IntervalliDiLavoro = new List<IntervalliDiLavoroEffettivi>();
            this._listCommesse = new List<Commessa>();
            this.loadCommesse(inizio, fine);
            for (int i = 0; i < this.listCommesse.Count; i++)
            {
                this.listCommesse[i].loadArticoli();
                for (int j = 0; j < this.listCommesse[i].Articoli.Count; j++)
                {
                    this.listCommesse[i].Articoli[j].loadTasksProduzione();
                    for (int k = 0; k < this.listCommesse[i].Articoli[j].Tasks.Count; k++)
                    {
                        TaskProduzione tsk = this.listCommesse[i].Articoli[j].Tasks[k];
                        tsk.loadIntervalliDiLavoroEffettivi();
                        for (int l = 0; l < tsk.Intervalli.Count; l++)
                        {
                            if (tsk.Intervalli[l].Inizio >= inizio && tsk.Intervalli[l].Inizio <= fine)
                            {
                                this._IntervalliDiLavoro.Add(tsk.Intervalli[l]);
                            }
                        }
                    }
                }
            }
        }

        public Boolean Delete()
        {
            Boolean ret = false;
            this.loadCommesse(new DateTime(1970, 1, 1), DateTime.Now.AddYears(10));
            if (this.listCommesse.Count == 0)
            {
                this.loadContatti();
                for (int i = 0; i < this.ElencoContatti.Count; i++)
                {
                    this.ElencoContatti[i].Delete();
                }
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "DELETE FROM anagraficaclienti WHERE codice = @codice";
                try
                {
                    conn.Execute(sql, new { codice = this.CodiceCliente }, tr);
                    ret = true;
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        private List<Articolo> _listArticoli;
        public List<Articolo> listArticoli
        {
            get
            {
                return this._listArticoli;
            }
        }

        public Boolean loadArticoli(char artStatus)
        {
            Boolean ret = false;
            this._listArticoli = new List<Articolo>();
            if (this.CodiceCliente.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT productionplan.id, productionplan.anno FROM productionplan INNER JOIN commesse ON "
                    + "(productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa=commesse.anno) "
                    + " WHERE commesse.cliente = @codice AND productionplan.status = @artStatus";
                var rows = conn.Query<(int, int)>(sql, new { codice = this.CodiceCliente, artStatus = artStatus.ToString() });
                ret = true;
                foreach (var r in rows)
                {
                    Articolo art = new Articolo(this.Tenant, r.Item1, r.Item2);
                    if (art.ID != -1)
                    {
                        this._listArticoli.Add(art);
                    }
                }
                conn.Close();
            }
            return ret;
        }

        public Boolean loadArticoli(ProcessoVariante origProc, char prodStatus, DateTime start, DateTime end)
        {
            Boolean ret = false;
            if (this.CodiceCliente.Length > 0)
            {
                this._listArticoli = new List<Articolo>();
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT productionplan.id, productionplan.anno FROM "
                    + " productionplan INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa = commesse.anno) "
                    + " WHERE productionplan.status = @prodStatus AND ";
                DynamicParameters pars = new DynamicParameters();
                pars.Add("prodStatus", prodStatus.ToString());
                String condOrigProc = "", condCustomer = "", condTime = "";
                if (origProc != null && origProc.process != null && origProc.variant != null && origProc.process.processID != -1 && origProc.variant.idVariante != -1)
                {
                    condOrigProc = " productionplan.processo = @processID"
                        + " AND productionplan.revisione = @revisione"
                        + " AND productionplan.variante = @idVariante";
                }

                condCustomer = " commesse.cliente = @codice";

                if (start != null && end != null && start < end)
                {
                    condTime = " productionplan.dataPrevistaFineProduzione >= @start "
                        + " AND productionplan.dataPrevistaFineProduzione <= @end";
                }

                if (condOrigProc.Length > 0)
                {
                    sql += condOrigProc;
                    pars.Add("processID", origProc.process.processID);
                    pars.Add("revisione", origProc.process.revisione);
                    pars.Add("idVariante", origProc.variant.idVariante);
                }

                if (condCustomer.Length > 0)
                {
                    if (condOrigProc.Length > 0)
                    {
                        sql += " AND ";
                    }
                    sql += condCustomer;
                    pars.Add("codice", this.CodiceCliente);
                }
                if (condTime.Length > 0)
                {
                    if (condCustomer.Length > 0 || condOrigProc.Length > 0)
                    {
                        sql += " AND ";
                    }
                    sql += condTime;
                    pars.Add("start", start.ToString("yyyy/MM/dd"));
                    pars.Add("end", end.ToString("yyyy/MM/dd"));
                }

                sql += " ORDER BY dataPrevistaFineProduzione";

                log = sql;

                var rows = conn.Query<(int, int)>(sql, pars);
                foreach (var r in rows)
                {
                    this._listArticoli.Add(new Articolo(this.Tenant, r.Item1, r.Item2));
                }
                conn.Close();
                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }
    }

    public class PortafoglioClienti
    {
        public String Tenant;

        public String log;

        public List<Cliente> Elenco;
        public PortafoglioClienti(String Tenant)
        {
            this.Tenant = Tenant;

            this.Elenco = new List<Cliente>();
            this._TempoDiLavoroTotale = new TimeSpan(0, 0, 0);
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT codice FROM anagraficaclienti WHERE customer IS TRUE ORDER BY ragsociale";
            var rows = conn.Query<string>(sql);
            foreach (var cod in rows)
            {
                this.Elenco.Add(new Cliente(this.Tenant, cod));
            }
            conn.Close();
        }

        public PortafoglioClienti(DateTime inizio, DateTime fine)
        {
            this.Elenco = new List<Cliente>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "SELECT DISTINCT(anagraficaclienti.codice) FROM anagraficaclienti "
                + "INNER JOIN commesse ON (commesse.cliente = anagraficaclienti.codice) "
                + " INNER JOIN tasksproduzione ON (commesse.idcommesse = tasksproduzione.idcommessa AND commesse.anno = tasksproduzione.annocommessa) "
                + " INNER JOIN registroeventitaskproduzione ON (tasksproduzione.taskID = registroeventitaskproduzione.task)"
                //+ " WHERE tasksproduzione.lateFinish >= '"+ inizio.ToString("yyyy/MM/dd") +"'"
                + " WHERE registroeventitaskproduzione.data >= @inizio "
                //+ " AND earlyStart <= '" + fine.ToString("yyyy/MM/dd") + "' "
                + " AND registroeventitaskproduzione.data <= @fine"
                + "  AND customer IS TRUE "
                + "ORDER BY ragsociale";
            var rows = conn.Query<string>(sql, new { inizio = inizio.ToString("yyyy/MM/dd"), fine = fine.ToString("yyyy/MM/dd") });
            foreach (var cod in rows)
            {
                this.Elenco.Add(new Cliente(this.Tenant, cod));
            }
            conn.Close();
        }

        public bool Add(String codice, String ragSoc, String pIva, String codFiscale, String indirizzo, String citta, String provincia, String CAP, String stato, String telefono, String email, Boolean kanban, Boolean provider, Boolean customer)
        {
            bool rt = false;
            bool validatePIvacFisc = false;
            validatePIvacFisc = (codFiscale.Length < 255 && pIva.Length < 255) ? true : false;
            bool validateCodice = (codice.Length > 0 && codice.Length < 255) ? true : false;
            bool validateRagSociale = (ragSoc.Length > 0 && ragSoc.Length < 255) ? true : false;
            bool validateEmail = false;
            MailAddress mail;
            String strMail = "";
            if (email.Length > 0)
            {
                try
                {
                    mail = new MailAddress(email);
                    strMail = mail.Address;
                    validateEmail = true;
                }
                catch
                {
                    strMail = "";
                    validateEmail = false;
                }
            }
            else
            {
                validateEmail = true;
                strMail = "";
            }

            if (validateCodice == true && validatePIvacFisc == true && validateRagSociale == true && validateEmail == true)
            {
                String strProvider = "false";
                String strCustomer = "false";
                strProvider = provider ? "true" : "false";
                strCustomer = customer ? "true" : "false";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO anagraficaclienti(codice, ragsociale, partitaiva, codfiscale, indirizzo, "
                + "citta, provincia, CAP, stato, telefono, email, kanbanManaged, customer, provider) VALUES("
                    + "@codice, "
                    + "@ragSoc, ";
                sql += "@pIva, ";
                sql += "@codFiscale, ";
                sql += "@indirizzo, "
                    + "@citta, "
                    + "@provincia, "
                    + "@CAP, "
                    + "@stato, "
                    + "@telefono, ";
                sql += "@email, ";
                sql += "@kanban, "
                    + "@customer, "
                    + "@provider"
                    + ")";
                try
                {
                    conn.Execute(sql, new { codice = codice, ragSoc = ragSoc, pIva = pIva.Length > 0 ? pIva : (object)DBNull.Value, codFiscale = codFiscale.Length > 0 ? codFiscale : (object)DBNull.Value, indirizzo = indirizzo, citta = citta, provincia = provincia, CAP = CAP, stato = stato, telefono = telefono, email = strMail.Length > 0 ? email : (object)DBNull.Value, kanban = kanban.ToString(), customer = strCustomer, provider = strProvider }, tr);
                    rt = true;
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message + sql;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                rt = false;
            }
            return rt;
        }
    
        private TimeSpan _TempoDiLavoroTotale;
        public TimeSpan TempoDiLavoroTotale
        {
            get
            {
                return this._TempoDiLavoroTotale;
            }
        }
        public Double TempoDiLavoroTotaleDbl
        {
            get
            {
                return this._TempoDiLavoroTotale.TotalHours;
            }
        }

        public void loadTempoDiLavoroTotale(DateTime inizio, DateTime fine)
        {
            for (int i = 0; i < this.Elenco.Count; i++)
            {
                this.Elenco[i].loadTempoDiLavoro(inizio, fine);
                this._TempoDiLavoroTotale += this.Elenco[i].TempoDiLavoro;
            }
        }
    }

    public class Contatto
    {
        protected String Tenant;

        public String log;

        private String _Cliente;
        public String Cliente
        {
            get
            {
                return this._Cliente;
            }
        }

        private int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        private String _FirstName;
        public String FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if (value.Length > 0 && this.ID!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE contatticlienti SET firstname = @firstname WHERE idContatto = @idContatto";
                    try
                    {
                        conn.Execute(sql, new { firstname = value, idContatto = this.ID }, tr);
                        tr.Commit();
                        this._FirstName = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        private String _LastName;
        public String LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if (value.Length > 0 && this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE contatticlienti SET lastname = @lastname WHERE idContatto = @idContatto";
                    try
                    {
                        conn.Execute(sql, new { lastname = value, idContatto = this.ID }, tr);
                        tr.Commit();
                        this._LastName = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        private String _Ruolo;
        public String Ruolo
        {
            get
            {
                return this._Ruolo;
            }
            set
            {
                if (value.Length > 0 && this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE contatticlienti SET ruolo = @ruolo WHERE idContatto = @idContatto";
                    try
                    {
                        conn.Execute(sql, new { ruolo = value, idContatto = this.ID }, tr);
                        tr.Commit();
                        this._Ruolo = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        public void loadEmails()
        {
            this.Emails = new List<ContattoEmail>();
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT email FROM contatticlienti_email WHERE idContatto = @idContatto";
                var rows = conn.Query<string>(sql, new { idContatto = this.ID });
                foreach (var em in rows)
                {
                    if (em != null)
                    {
                        this.Emails.Add(new ContattoEmail(this.Tenant, this.ID, new MailAddress(em)));
                    }
                }
                conn.Close();
            }
        }
        public List<ContattoEmail> Emails;

        public void loadPhones()
        {
            this.Phones = new List<ContattoTelefono>();
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT phone FROM contatticlienti_phone WHERE idContatto = @idContatto";
                var rows = conn.Query<string>(sql, new { idContatto = this.ID });
                foreach (var ph in rows)
                {
                    if (ph != null)
                    {
                        this.Phones.Add(new ContattoTelefono(this.Tenant, this.ID, ph));
                    }
                }
                conn.Close();
            }
        }
        public List<ContattoTelefono> Phones;

        private User _user;
        public User user
        {
            get {
                return this._user;
            }
            set {
                if(this.ID!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    string sql = "UPDATE contatticlienti SET user = @user WHERE "
                        + " idcontatto = @idContatto";
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(sql, new { user = value.username, idContatto = this.ID }, tr);
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

        public Contatto(String Tenant, int idC)
        {
            this.Tenant = Tenant;

            this._ID = -1;
            this.Emails = new List<ContattoEmail>();
            this.Phones = new List<ContattoTelefono>();
            if (idC != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT idContatto, cliente, firstname, lastname, ruolo, user "
                    +" FROM contatticlienti WHERE idContatto = @idContatto";
                var rows = conn.Query<(int, string, string, string, string, string)>(sql, new { idContatto = idC }).ToList();
                if (rows.Count > 0)
                {
                    var r = rows[0];
                    this._ID = r.Item1;
                    this._Cliente = r.Item2;
                    this._FirstName = r.Item3;
                    if(r.Item4 != null)
                    {
                        this._LastName = r.Item4;
                    }
                    this._Ruolo = r.Item5;
                    if(r.Item6 != null)
                    { 
                        User usr = new User(r.Item6);
                        if(usr!=null && usr.username.Length > 0)
                        { 
                            this._user = usr;
                        }
                    }
                    loadEmails();
                    loadPhones();
                }
                conn.Close();
            }
        }

        public bool addPhone(String number, String notes)
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO contatticlienti_phone(idContatto, phone, note) VALUES("
                    + "@idContatto, "
                    + "@phone, "
                    + "@note"
                    +")";
                try
                {
                    conn.Execute(sql, new { idContatto = this.ID, phone = number, note = notes }, tr);
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

        public bool addEmail(MailAddress mail, String notes)
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO contatticlienti_email(idContatto, email, note) VALUES("
                    + "@idContatto, "
                    + "@email, "
                    + "@note"
                    + ")";
                try
                {
                    conn.Execute(sql, new { idContatto = this.ID, email = mail.Address, note = notes }, tr);
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

        public bool Delete()
        {
            bool ret = false;
            if (this.ID != -1)
            {
                this.loadEmails();
                for (int i = 0; i < this.Emails.Count; i++)
                {
                    this.Emails[i].Delete();
                } 
                this.loadPhones();
                for (int i = 0; i < this.Phones.Count; i++)
                {
                    this.Phones[i].Delete();
                }

                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "DELETE FROM contatticlienti WHERE idContatto = @idContatto";
                try
                {
                    conn.Execute(sql, new { idContatto = this.ID }, tr);
                    ret = true;
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                ret = false;

            }
            return ret;
        }

        public void loadUser()
        {
            this.user = this._user;
        }
    }

    public class ContattoEmail
    {
        public String Tenant;

        public String log;

        private int _IdContatto;
        public int idContatto
        {
            get
            {
                return this._IdContatto;
            }
        }

        private MailAddress _Email;
        public MailAddress Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                if (this.idContatto!=-1 && value.Address.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE contatticlienti_email SET email = @email WHERE idContatto = @idContatto"
                        + " AND email = @oldEmail";
                    try
                    {
                        conn.Execute(sql, new { email = value.Address, idContatto = this.idContatto, oldEmail = this.Email.Address.ToString() }, tr);
                        tr.Commit();
                        this._Email = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        private String _Note;
        public String Note
        {
            get
            {
                return this._Note;
            }
            set
            {
                if (this.idContatto!=-1 && value.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE contatticlienti_email SET note = @note WHERE idContatto = @idContatto"
                        + " AND email = @email";
                    try
                    {
                        conn.Execute(sql, new { note = value, idContatto = this.idContatto, email = this.Email.Address.ToString() }, tr);
                        tr.Commit();
                        this._Note = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }
    
        public ContattoEmail(String Tenant, int idCont, MailAddress mail)
        {
            this.Tenant = Tenant;

            this._Email = null;
            if(idCont!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT email, note FROM contatticlienti_email WHERE idContatto = @idContatto"
                    + " AND email LIKE @email";
                var rows = conn.Query<(string, string)>(sql, new { idContatto = idCont, email = mail.Address }).ToList();
                if (rows.Count > 0)
                {
                    var r = rows[0];
                    this._IdContatto = idCont;

                    if (r.Item1 != null)
                    {
                        try
                        {
                            this._Email = new MailAddress(r.Item1);
                        }
                        catch
                        {
                            this._Email = null;
                        }
                    }

                    if (r.Item2 != null)
                    {
                        this._Note = r.Item2;
                    }
                }
                conn.Close();
            }
        }

        public bool Delete()
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            string sql = "DELETE FROM contatticlienti_email WHERE idContatto = @idContatto"
                + " AND email LIKE @email";
            try
            {
                conn.Execute(sql, new { idContatto = this.idContatto, email = this.Email.Address }, tr);
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
            return rt;
        }
    }

    public class ContattoTelefono
    {
        public String Tenant;

        public String log;

        private int _IdContatto;
        public int idContatto
        {
            get
            {
                return this._IdContatto;
            }
        }

        private String _Phone;
        public String Phone
        {
            get
            {
                return this._Phone;
            }
            set
            {
                if (this.idContatto != -1 && value.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE contatticlienti_phone SET phone = @phone WHERE idContatto = @idContatto"
                        + " AND phone = @oldPhone";
                    try
                    {
                        conn.Execute(sql, new { phone = value, idContatto = this.idContatto, oldPhone = this.Phone }, tr);
                        tr.Commit();
                        this._Phone = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        private String _Note;
        public String Note
        {
            get
            {
                return this._Note;
            }
            set
            {
                if (this.idContatto != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE contatticlienti_phone SET note = @note WHERE idContatto = @idContatto" +
                        " AND phone = @phone";
                    try
                    {
                        conn.Execute(sql, new { note = value, idContatto = this.idContatto, phone = this.Phone }, tr);
                        tr.Commit();
                        this._Note = value;
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }
                    conn.Close();
                }
            }
        }

        public ContattoTelefono(String Tenant, int idCont, String tel)
        {
            this.Tenant = Tenant;
            this._Phone = null;
            if (idCont != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sql = "SELECT phone, note FROM contatticlienti_phone WHERE idContatto = @idContatto"
                    + " AND phone LIKE @phone";
                var rows = conn.Query<(string, string)>(sql, new { idContatto = idCont, phone = tel }).ToList();
                if (rows.Count > 0)
                {
                    var r = rows[0];
                    this._IdContatto = idCont;

                    if (r.Item1 != null)
                    {
                        try
                        {
                            this._Phone = r.Item1;
                        }
                        catch
                        {
                            this._Phone = "";
                        }
                    }

                    if (r.Item2 != null)
                    {
                        this._Note = r.Item2;
                    }
                }
                conn.Close();
            }
        }

        public bool Delete()
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            string sql = "DELETE FROM contatticlienti_phone WHERE idContatto = @idContatto"
                + " AND phone LIKE @phone";
            try
            {
                conn.Execute(sql, new { idContatto = this.idContatto, phone = this.Phone }, tr);
                tr.Commit();
                rt = true;
            }
            catch(Exception ex)
            {
                rt = false;
                log = ex.Message;
                tr.Rollback();
            }
            conn.Close();
            return rt;
        }
    }

    public class Providers
    {
        protected String Tenant;

        public String log;

        public List<Cliente> List;
        public Providers(String Tenant)
        {
            this.List = new List<Cliente>();
            MySqlConnection conn = (new Dati.Dati()).mycon(Tenant);
            conn.Open();
            string sql = "SELECT codice FROM anagraficaclienti WHERE provider IS TRUE ORDER BY ragsociale";
            var rows = conn.Query<string>(sql);
            foreach (var cod in rows)
            {
                this.List.Add(new Cliente(Tenant, cod));
            }
            conn.Close();
        }

        public bool Add(String codice, String ragSoc, String pIva, String codFiscale, String indirizzo, String citta, String provincia, String CAP, String stato, String telefono, String email, Boolean kanban)
        {
            bool rt = false;
            bool validatePIvacFisc = false;
            validatePIvacFisc = (codFiscale.Length < 255 && pIva.Length < 255) ? true : false;
            bool validateCodice = (codice.Length > 0 && codice.Length < 255) ? true : false;
            bool validateRagSociale = (ragSoc.Length > 0 && ragSoc.Length < 255) ? true : false;
            bool validateEmail = false;
            MailAddress mail;
            String strMail = "";
            if (email.Length > 0)
            {
                try
                {
                    mail = new MailAddress(email);
                    strMail = mail.Address;
                    validateEmail = true;
                }
                catch
                {
                    strMail = "";
                    validateEmail = false;
                }
            }
            else
            {
                validateEmail = true;
                strMail = "";
            }

            if (validateCodice == true && validatePIvacFisc == true && validateRagSociale == true && validateEmail == true)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                string sql = "INSERT INTO anagraficaclienti(codice, ragsociale, partitaiva, codfiscale, indirizzo, "
                + "citta, provincia, CAP, stato, telefono, email, kanbanManaged) VALUES("
                    + "@codice, "
                    + "@ragSoc, ";
                sql += "@pIva, ";
                sql += "@codFiscale, ";
                sql += "@indirizzo, "
                    + "@citta, "
                    + "@provincia, "
                    + "@CAP, "
                    + "@stato, "
                    + "@telefono, ";
                sql += "@email, ";
                sql += "@kanban"
                    + ")";
                try
                {
                    conn.Execute(sql, new { codice = codice, ragSoc = ragSoc, pIva = pIva.Length > 0 ? pIva : (object)DBNull.Value, codFiscale = codFiscale.Length > 0 ? codFiscale : (object)DBNull.Value, indirizzo = indirizzo, citta = citta, provincia = provincia, CAP = CAP, stato = stato, telefono = telefono, email = strMail.Length > 0 ? email : (object)DBNull.Value, kanban = kanban.ToString() }, tr);
                    rt = true;
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message + sql;
                    rt = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            else
            {
                rt = false;
            }
            return rt;
        }
    }

}