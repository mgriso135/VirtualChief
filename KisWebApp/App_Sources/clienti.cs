/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Net.Mail;

namespace KIS.App_Code
{
    public class Cliente
    {
        public String Tenant;

        public String log;

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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE anagraficaclienti SET ragsociale = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE anagraficaclienti SET partitaiva = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE anagraficaclienti SET codfiscale = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE anagraficaclienti SET indirizzo = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE anagraficaclienti SET citta = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                try
                {
                    cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE anagraficaclienti SET provincia = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE anagraficaclienti SET CAP = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                try
                {
                    cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE anagraficaclienti SET stato = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                try
                {
                    cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE anagraficaclienti SET telefono = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE anagraficaclienti SET email = '" + value + "' WHERE codice = '" + this.CodiceCliente + "'";
                try
                {
                    cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idContatto FROM contatticlienti WHERE cliente = '" + this.CodiceCliente +
                    "' ORDER BY lastname, firstname";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this.ElencoContatti.Add(new Contatto(this.Tenant, rdr.GetInt32(0)));
                }
                rdr.Close();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE anagraficaclienti SET kanbanManaged = " + value +
                        " WHERE codice ='"+this.CodiceCliente+"'";
                    try
                    {
                        cmd.ExecuteNonQuery();
                        this._KanbanManaged = value;
                    }
                    catch
                    {
                    }
                    conn.Close();
                }
            }
        }

        public Cliente(String cod)
        {
            this._listCommesse = new List<Commessa>();
            this._IntervalliDiLavoro = new List<IntervalliDiLavoroEffettivi>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT codice, ragsociale, partitaiva, codfiscale, indirizzo, citta, provincia, CAP, "
            + "stato, telefono, email, kanbanManaged FROM anagraficaclienti WHERE codice = '" + cod + "'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._CodiceCliente = cod;
                if (!rdr.IsDBNull(1))
                {
                    this._RagioneSociale = rdr.GetString(1);
                }
                else
                {
                    this._RagioneSociale = "";
                }
                if (!rdr.IsDBNull(2))
                {
                    this._PartitaIVA = rdr.GetString(2);
                }
                else
                {
                    this._PartitaIVA = "";
                }
                if (!rdr.IsDBNull(3))
                {
                    this._CodiceFiscale = rdr.GetString(3);
                }
                else
                {
                    this._CodiceFiscale = "";
                }

                this._Indirizzo = "";
                if (!rdr.IsDBNull(4))
                {
                    this._Indirizzo = rdr.GetString(4);
                }

                this._Citta = "";
                if (!rdr.IsDBNull(5))
                {
                    this._Citta = rdr.GetString(5);
                }

                this._Provincia = "";
                if (!rdr.IsDBNull(6))
                {
                    this._Provincia = rdr.GetString(6);
                }

                this._CAP = "";
                if (!rdr.IsDBNull(7))
                {
                    this._CAP = rdr.GetString(7);
                }

                this._Stato = "";
                if (!rdr.IsDBNull(8))
                {
                    this._Stato = rdr.GetString(8);
                }

                this._Telefono = "";
                if (!rdr.IsDBNull(9))
                {
                    this._Telefono = rdr.GetString(9);
                }

                this._Email = "";
                if (!rdr.IsDBNull(10))
                {
                    this._Email = rdr.GetString(10);
                }

                this._KanbanManaged = rdr.GetBoolean(11);

                this.loadContatti();

            }
            else
            {
                this._CodiceCliente = "";
                this._RagioneSociale = "";
                this._PartitaIVA = "";
                this._RagioneSociale = "";
            }
            rdr.Close();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(idContatto) FROM contatticlienti";
                MySqlDataReader rdr = cmd.ExecuteReader();
                int maxID = 0;
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxID = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO contatticlienti(idContatto, cliente, firstname, lastname, ruolo) VALUES("
                    + maxID.ToString() + ", "
                    + "'" + this.CodiceCliente + "', "
                    + "'" + firstname + "', "
                    + "'" + lastname + "', "
                    + "'" + role + "'"
                    + ")";

                try
                {
                    cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT commesse.idcommesse, commesse.anno FROM commesse "
                    + " INNER JOIN tasksproduzione ON (commesse.idcommesse = tasksproduzione.idcommessa AND commesse.anno = tasksproduzione.annocommessa) "
                    + " WHERE commesse.cliente = '" + this.CodiceCliente + "'"
                    + " AND tasksproduzione.lateFinish >= '" + inizio.ToString("yyyy/MM/dd") + "'"
                    + " AND earlyStart <= '" + fine.ToString("yyyy/MM/dd") + "' "
                    + " GROUP by commesse.idcommesse, commesse.anno "
                    + "ORDER BY commesse.anno, commesse.idcommesse";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._listCommesse.Add(new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
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
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM anagraficaclienti WHERE codice = '" + this.CodiceCliente + "'";
                try
                {
                    cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT productionplan.id, productionplan.anno FROM productionplan INNER JOIN commesse ON "
                    + "(productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa=commesse.anno) "
                    + " WHERE commesse.cliente = '" + this.CodiceCliente + "' AND productionplan.status = '" + artStatus + "'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                ret = true;
                while (rdr.Read())
                {
                    Articolo art = new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1));
                    if (art.ID != -1)
                    {
                        this._listArticoli.Add(art);
                    }
                }
                rdr.Close();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT productionplan.id, productionplan.anno FROM "
                    + " productionplan INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa = commesse.anno) "
                    + " WHERE productionplan.status = '" + prodStatus + "' AND ";
                String condOrigProc = "", condCustomer = "", condTime = "";
                if (origProc != null && origProc.process != null && origProc.variant != null && origProc.process.processID != -1 && origProc.variant.idVariante != -1)
                {
                    condOrigProc = " productionplan.processo = " + origProc.process.processID.ToString()
                        + " AND productionplan.revisione = " + origProc.process.revisione.ToString()
                        + " AND productionplan.variante = " + origProc.variant.idVariante.ToString();
                }

                condCustomer = " commesse.cliente = '" + this.CodiceCliente + "'";

                if (start != null && end != null && start < end)
                {
                    condTime = " productionplan.dataPrevistaFineProduzione >= '" + start.ToString("yyyy/MM/dd") + "' "
                        + " AND productionplan.dataPrevistaFineProduzione <= '" + end.ToString("yyyy/MM/dd") + "'";
                }

                if (condOrigProc.Length > 0)
                {
                    cmd.CommandText += condOrigProc;
                }

                if (condCustomer.Length > 0)
                {
                    if (condOrigProc.Length > 0)
                    {
                        cmd.CommandText += " AND ";
                    }
                    cmd.CommandText += condCustomer;
                }
                if (condTime.Length > 0)
                {
                    if (condCustomer.Length > 0 || condOrigProc.Length > 0)
                    {
                        cmd.CommandText += " AND ";
                    }
                    cmd.CommandText += condTime;
                }

                cmd.CommandText += " ORDER BY dataPrevistaFineProduzione";

                log = cmd.CommandText;

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._listArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
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
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT codice FROM anagraficaclienti ORDER BY ragsociale";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.Elenco.Add(new Cliente(rdr.GetString(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public PortafoglioClienti(DateTime inizio, DateTime fine)
        {
            this.Elenco = new List<Cliente>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT(anagraficaclienti.codice) FROM anagraficaclienti "
                + "INNER JOIN commesse ON (commesse.cliente = anagraficaclienti.codice) "
                + " INNER JOIN tasksproduzione ON (commesse.idcommesse = tasksproduzione.idcommessa AND commesse.anno = tasksproduzione.annocommessa) "
                + " INNER JOIN registroeventitaskproduzione ON (tasksproduzione.taskID = registroeventitaskproduzione.task)"
                //+ " WHERE tasksproduzione.lateFinish >= '"+ inizio.ToString("yyyy/MM/dd") +"'"
                + " WHERE registroeventitaskproduzione.data >= '" + inizio.ToString("yyyy/MM/dd") + "' "
                //+ " AND earlyStart <= '" + fine.ToString("yyyy/MM/dd") + "' "
                + " AND registroeventitaskproduzione.data <= '" + fine.ToString("yyyy/MM/dd") + "'"
                + "ORDER BY ragsociale";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.Elenco.Add(new Cliente(rdr.GetString(0)));
            }
            rdr.Close();
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
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO anagraficaclienti(codice, ragsociale, partitaiva, codfiscale, indirizzo, "
                + "citta, provincia, CAP, stato, telefono, email, kanbanManaged) VALUES("
                    + "'" + codice + "', "
                    + "'" + ragSoc + "', ";
                cmd.CommandText += (pIva.Length > 0 ? "'" + pIva + "', " : "null, ");
                cmd.CommandText += (codFiscale.Length > 0 ? "'" + codFiscale + "', " : "null, ");
                cmd.CommandText += "'" + indirizzo + "', "
                    + "'" + citta + "', "
                    + "'" + provincia + "', "
                    + "'" + CAP + "', "
                    + "'" + stato + "', "
                    + "'" + telefono + "', ";
                cmd.CommandText += (strMail.Length > 0 ? "'" + email + "', " : "null, ");
                cmd.CommandText += kanban.ToString()
                    + ")";
                try
                {
                    cmd.ExecuteNonQuery();
                    rt = true;
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = ex.Message + cmd.CommandText;
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE contatticlienti SET firstname = '" + value + "' WHERE idContatto = " + this.ID.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE contatticlienti SET lastname = '" + value + "' WHERE idContatto = " + this.ID.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE contatticlienti SET ruolo = '" + value + "' WHERE idContatto = " + this.ID.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT email FROM contatticlienti_email WHERE idContatto = " + this.ID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        this.Emails.Add(new ContattoEmail(this.Tenant, this.ID, new MailAddress(rdr.GetString(0))));
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT phone FROM contatticlienti_phone WHERE idContatto = " + this.ID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        this.Phones.Add(new ContattoTelefono(this.Tenant, this.ID, rdr.GetString(0)));
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
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE contatticlienti SET user = '" + value.username + "' WHERE "
                        + " idcontatto = " + this.ID.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idContatto, cliente, firstname, lastname, ruolo, user "
                    +" FROM contatticlienti WHERE idContatto = " + idC.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    this._ID = rdr.GetInt32(0);
                    this._Cliente = rdr.GetString(1);
                    this._FirstName = rdr.GetString(2);
                    if(!rdr.IsDBNull(3))
                    {
                        this._LastName = rdr.GetString(3);
                    }
                    this._Ruolo = rdr.GetString(4);
                    if(!rdr.IsDBNull(5))
                    { 
                        User usr = new User(rdr.GetString(5));
                        if(usr!=null && usr.username.Length > 0)
                        { 
                            this._user = usr;
                        }
                    }
                    loadEmails();
                    loadPhones();
                }
                rdr.Close();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO contatticlienti_phone(idContatto, phone, note) VALUES("
                    + this.ID.ToString() + ", "
                    + "'" + number + "', "
                    + "'" + notes + "'"
                    +")";
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

        public bool addEmail(MailAddress mail, String notes)
        {
            bool rt = false;
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO contatticlienti_email(idContatto, email, note) VALUES("
                    + this.ID.ToString() + ", "
                    + "'" + mail.Address + "', "
                    + "'" + notes + "'"
                    + ")";
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
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM contatticlienti WHERE idContatto = " + this.ID.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE contatticlienti_email SET email = '" + value.Address + "' WHERE idContatto = " + this.idContatto.ToString()
                        + " AND email = '" + this.Email.Address.ToString() + "'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE contatticlienti_email SET note = '" + value + "' WHERE idContatto = " + this.idContatto.ToString()
                        + " AND email ='"+this.Email.Address.ToString()+"'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT email, note FROM contatticlienti_email WHERE idContatto = " + idCont.ToString()
                    + " AND email LIKE '" + mail.Address + "'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    this._IdContatto = idCont;

                    if(!rdr.IsDBNull(0))
                    {
                        try
                        {
                            this._Email = new MailAddress(rdr.GetString(0));
                        }
                        catch
                        {
                            this._Email = null;
                        }
                    }

                    if (!rdr.IsDBNull(1))
                    {
                        this._Note = rdr.GetString(1);
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        public bool Delete()
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM contatticlienti_email WHERE idContatto = " + this.idContatto.ToString()
                + " AND email LIKE '" + this.Email.Address + "'";
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE contatticlienti_phone SET phone = '" + value + "' WHERE idContatto = " + this.idContatto.ToString()
                        + " AND phone = '"+this.Phone+"'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE contatticlienti_phone SET note = '" + value + "' WHERE idContatto = " + this.idContatto.ToString() +
                        " AND phone = '"+this.Phone+"'";
                    try
                    {
                        cmd.ExecuteNonQuery();
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
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT phone, note FROM contatticlienti_phone WHERE idContatto = " + idCont.ToString()
                    + " AND phone LIKE '" + tel + "'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    this._IdContatto = idCont;

                    if (!rdr.IsDBNull(0))
                    {
                        try
                        {
                            this._Phone = rdr.GetString(0);
                        }
                        catch
                        {
                            this._Phone = "";
                        }
                    }

                    if (!rdr.IsDBNull(1))
                    {
                        this._Note = rdr.GetString(1);
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        public bool Delete()
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM contatticlienti_phone WHERE idContatto = " + this.idContatto.ToString()
                + " AND phone LIKE '" + this.Phone + "'";
            try
            {
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
            conn.Close();
            return rt;
        }
    }

}