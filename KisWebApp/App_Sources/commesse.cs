/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2018 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2019 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Net.Http;
using KIS.App_Sources;

namespace KIS.App_Code
{
    public class Commessa
    {
        public String Tenant;

        public String log;

        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private int _Year;
        public int Year
        {
            get { return _DataInserimento.Year; }
        }

        private String _ExternalID;
        public String ExternalID
        {
            get { return this._ExternalID; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                String extID = value;
                if(value.Length >= 255)
                {
                    extID = value.Substring(0, 250);
                }
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE commesse SET ExternalID = '"+extID+"' WHERE idcommesse=" + this.ID.ToString() + " AND anno=" + this.Year.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    tr.Rollback();
                    this.log = ex.Message;
                }
                conn.Clone();
            }
        }
        private String _Cliente;
        public String Cliente
        {
            get { return this._Cliente; }
        }

        public String RagioneSocialeCliente
        {
            get
            {
                String ret = "";
                if (this._Cliente != null && this._Cliente.Length > 0)
                {
                    Cliente cln = new App_Code.Cliente(this.Tenant, this.Cliente);
                    if (cln.RagioneSociale.Length > 0)
                    {
                        ret = cln.RagioneSociale;
                    }
                }
                return ret;
            }
        }

        private DateTime _DataInserimento;
        public DateTime DataInserimento
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._DataInserimento, fuso.tzFusoOrario);
            }
        }

        private List<Articolo> _Articoli;
        public List<Articolo> Articoli
        {
            get { return this._Articoli; }
        }

        private String _Note;
        public String Note
        {
            get { return this._Note; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                String notes = value;
                if (value.Length >= 255)
                {
                    notes = value.Substring(0, 250);
                }
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE commesse SET note = '" + notes + "' WHERE idcommesse=" + this.ID.ToString() + " AND anno=" + this.Year.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    this.log = ex.Message;
                }
                conn.Clone();
            }
        }

        public char Status
        {
            get
            {
                /* Si basa sugli articoli
                 * 'N' se nessuno è iniziato
                 * 'P' se i task sono stati pianificati in produzione
                 * 'I' se almeno uno è in corso di esecuzione
                 * 'F' se tutti sono finiti
                */
                int numN = 0;
                int numI = 0;
                int numF = 0;
                int numP = 0;
                this.loadArticoli();
                for (int i = 0; i < this.Articoli.Count; i++)
                {
                    if (this.Articoli[i].Status == 'N')
                    {
                        numN++;
                    }
                    else if (this.Articoli[i].Status == 'I')
                    {
                        numI++;
                    }
                    else if (this.Articoli[i].Status == 'F')
                    {
                        numF++;
                    }
                    else if (this.Articoli[i].Status == 'P')
                    {
                        numP++;
                    }
                }
                    char rt = '\0';
                    if (this.Articoli.Count == 0 || numN == this.Articoli.Count)
                    {
                        rt = 'N';
                    }
                    else if (numF == this.Articoli.Count)
                    {
                        rt = 'F';
                    }
                    else if (numP == this.Articoli.Count)
                    {
                        rt = 'P';
                    }
                    /*else if (numN > 0)
                    {
                        rt = 'N';
                    }*/
                    else if (numI > 0 || numF>0)
                    {
                        rt = 'I';
                    }
                    else
                    {
                        rt = '\0';
                    }
                    return rt;
                
            }
        }

        private Boolean _Confirmed;
        public Boolean Confirmed
        {
            get
            {
                return this._Confirmed;
            }
            set
            {
                // value.ToString("yyyy/MM/dd HH:mm:ss")
                if (this.ID != -1 && this.Year > 2000)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE commesse set Confirmed = "
                            + value.ToString()
                            + " WHERE idcommesse = " + this.ID.ToString()
                            + " AND anno = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Confirmed = value;
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private DateTime _ConfirmationDate;
        public DateTime ConfirmationDate
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                return TimeZoneInfo.ConvertTimeFromUtc(this._ConfirmationDate, fuso.tzFusoOrario);
            }
            set
            {
                // value.ToString("yyyy/MM/dd HH:mm:ss")
                if (this.ID != -1 && this.Year > 2000)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE commesse set ConfirmedDate = '"
                            + value.ToString("yyyy/MM/dd HH:mm:ss")
                            + "' WHERE idcommesse = " + this.ID.ToString()
                            + " AND anno = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._ConfirmationDate = value;
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        private UserAccount _ConfirmedBy;
        public UserAccount ConfirmedBy
        {
            get
            {
                return this._ConfirmedBy;
            }
            set
            {
                // value.ToString("yyyy/MM/dd HH:mm:ss")
                if (this.ID != -1 && this.Year > 2000)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE commesse set ConfirmedBy = '"
                            + value.userId
                            + "' WHERE idcommesse = " + this.ID.ToString()
                            + " AND anno = " + this.Year.ToString();
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._ConfirmedBy = value;
                    }
                    catch (Exception ex)
                    {
                        this.log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public Commessa(String Tenant, int idCommessa, int AnnoComm)
        {
            this.Tenant = Tenant;

            this._ExternalID = "";
            this._Confirmed = false;
            this._ConfirmedBy = null;
            this._ConfirmationDate = new DateTime(1970, 1, 1);
            this._Note = "";
            this._Articoli = new List<Articolo>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT cliente, dataInserimento, note, confirmed, confirmedBy, confirmedDate, ExternalID"
                + " FROM commesse"
                + " WHERE idcommesse = " + idCommessa.ToString() + " AND "
                + " anno = " + AnnoComm.ToString() + " ORDER BY anno DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = idCommessa;
                this._Cliente = rdr.GetString(0);
                this._DataInserimento = DateTime.SpecifyKind(rdr.GetDateTime(1), DateTimeKind.Utc);
                if(!rdr.IsDBNull(2))
                { 
                this._Note = rdr.GetString(2);
                }
                if (!rdr.IsDBNull(3))
                {
                    this._Confirmed = rdr.GetBoolean(3);
                }
                if (!rdr.IsDBNull(4))
                {
                    this._ConfirmedBy = new UserAccount(rdr.GetString(4));
                }
                if (!rdr.IsDBNull(5))
                {
                    this._ConfirmationDate = rdr.GetDateTime(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    this._ExternalID = rdr.GetString(6);
                }
            }
            else
            {
                this._ID = -1;
                this._DataInserimento = new DateTime(1900, 1, 1);
                this._Articoli = new List<Articolo>();
                this._Confirmed = false;
                this._ConfirmedBy = null;
                this._ConfirmationDate = new DateTime(1970, 1, 1);
                this._ExternalID = "";
            }

            rdr.Close();
            conn.Close();
        }

        public void loadArticoli()
        {
            if(this.ID!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, anno FROM productionplan WHERE commessa = " + this.ID.ToString() + " AND annoCommessa = " + this.Year.ToString()
                    + " ORDER BY status, dataConsegnaPrevista";
                MySqlDataReader rdr = cmd.ExecuteReader();
                this._Articoli = new List<Articolo>();
                while (rdr.Read())
                {
                    this._Articoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void loadArticoli(Reparto rp)
        {
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, anno FROM productionplan WHERE commessa = " + this.ID.ToString() 
                    + " AND annoCommessa = " + this.Year.ToString()
                    + " AND reparto = " + rp.id.ToString()
                    + " ORDER BY status, dataConsegnaPrevista";
                MySqlDataReader rdr = cmd.ExecuteReader();
                this._Articoli = new List<Articolo>();
                while (rdr.Read())
                {
                    this._Articoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public bool AddArticolo(ProcessoVariante prc, DateTime consPrev, int qty)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(id) FROM productionPlan WHERE anno = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int artID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                artID = rdr.GetInt32(0)+1;
            }
            else
            {
                artID = 0;
            }
            FusoOrario fuso = new FusoOrario(this.Tenant);
            rdr.Close();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO productionplan(id, anno, processo, revisione, variante, matricola, status, reparto, startTime, "
                + "commessa, annoCommessa, dataConsegnaPrevista, planner, quantita, quantitaProdotta, MeasurementUnit) VALUES(" + artID.ToString() + ", " + DateTime.UtcNow.Year.ToString()
                + ", " + prc.process.processID.ToString() + ", " + prc.process.revisione.ToString() + ", " + prc.variant.idVariante.ToString()
                + ", null, 'N', null, null, " + this.ID.ToString() + ", " 
                + this.Year.ToString() + ", " 
                + "'" + TimeZoneInfo.ConvertTimeToUtc(consPrev, fuso.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") + "', " 
                + "null, " 
                + qty.ToString() + ", "
                + qty.ToString() + ", "
                + prc.MeasurementUnitID.ToString()
                + ")";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = true;
            }
            catch(Exception ex)
            {
                log = ex.Message;
                tr.Rollback();
                rt = false;
            }
            conn.Close();
            return rt;
        }

        // Uguale alla precedente ma ritorna le coordinate dell'articolo inserito.
        public int[] AddArticoloInt(ProcessoVariante prc, DateTime consPrev, int qty)
        {
            int[] rt = new int[2];
            rt[0] = -1;
            rt[1] = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(id) FROM productionPlan WHERE anno = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int artID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                artID = rdr.GetInt32(0) + 1;
            }
            else
            {
                artID = 0;
            }
            rdr.Close();
            cmd.Transaction = tr;
            FusoOrario fuso = new FusoOrario(this.Tenant);
            cmd.CommandText = "INSERT INTO productionplan(id, anno, processo, revisione, variante, matricola, status, reparto, startTime, "
                + "commessa, annoCommessa, dataConsegnaPrevista, planner, quantita, quantitaProdotta, MeasurementUnit) VALUES(" + artID.ToString() + ", " + DateTime.UtcNow.Year.ToString()
                + ", " + prc.process.processID.ToString() + ", " + prc.process.revisione.ToString() + ", " + prc.variant.idVariante.ToString()
                + ", null, 'N', null, null, " + this.ID.ToString() + ", "
                + this.Year.ToString() + ", "
                + "'" + TimeZoneInfo.ConvertTimeToUtc(consPrev, fuso.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") + "', "
                + "null, "
                + qty.ToString() + ", "
                + qty.ToString() + ", "
                + prc.MeasurementUnitID
                + ")";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt[0] = artID;
                rt[1] = DateTime.UtcNow.Year;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                tr.Rollback();
                rt[0] = -1;
                rt[1] = -1;
            }
            conn.Close();
            return rt;
        }

        // Uguale alla precedente ma ritorna le coordinate dell'articolo inserito.
        public int[] AddArticolo(ProcessoVariante prc, DateTime consPrev, int qty, String kanbanCard)
        {
            int[] rt = new int[2];
            rt[0] = -1;
            rt[1] = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(id) FROM productionPlan WHERE anno = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int artID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                artID = rdr.GetInt32(0) + 1;
            }
            else
            {
                artID = 0;
            }
            rdr.Close();
            cmd.Transaction = tr;
            FusoOrario fuso = new FusoOrario(this.Tenant);
            cmd.CommandText = "INSERT INTO productionplan(id, anno, processo, revisione, variante, matricola, status, reparto, startTime, "
                + "commessa, annoCommessa, dataConsegnaPrevista, planner, quantita, quantitaProdotta, kanbanCard, measurementunit) VALUES(" 
                + artID.ToString() + ", " 
                + DateTime.UtcNow.Year.ToString()
                + ", " + prc.process.processID.ToString() + ", " + prc.process.revisione.ToString() + ", " + prc.variant.idVariante.ToString()
                + ", null, 'N', null, null, " + this.ID.ToString() + ", "
                + this.Year.ToString() + ", "
                + "'" + TimeZoneInfo.ConvertTimeToUtc(consPrev, fuso.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss") + "', "
                + "null, "
                + qty.ToString() + ", "
                + qty.ToString() + ", "
                + "'" + kanbanCard + "', "
                + prc.MeasurementUnitID.ToString()
                + ")";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt[0] = artID;
                rt[1] = DateTime.UtcNow.Year;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                tr.Rollback();
                rt[0] = -1;
                rt[1] = -1;
            }
            conn.Close();
            return rt;
        }


        public bool Delete()
        {
            bool rt = false;
            if (this.ID != -1)
            {
                if (this.Status == 'N')
                {
                    this.loadArticoli();
                    if (this.Articoli.Count == 0)
                    {
                        MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                        conn.Open();
                        MySqlTransaction tr = conn.BeginTransaction();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Transaction = tr;
                        try
                        {
                            cmd.CommandText = "DELETE FROM commesse WHERE idcommesse = " + this.ID.ToString()
                                + " AND anno = " + this.Year.ToString();
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
                }
            }
            return rt;
        }

        public ConfigurazioneRitardoCommessa EventoRitardo;
        public void loadEventoRitardo()
        {
            if (this.ID != -1 && this.Year != -1)
            {
                this.EventoRitardo = new ConfigurazioneRitardoCommessa(this.Tenant, this);
            }
        }

        public ConfigurazioneWarningCommessa EventoWarning;
        public void loadEventoWarning()
        {
            if (this.ID != -1 && this.Year != -1)
            {
                this.EventoWarning = new ConfigurazioneWarningCommessa(this.Tenant, this);
            }
        }
    
    }

    public class ElencoCommesse
    {
        public String Tenant;

        public String log;

        private List<Commessa> _Commesse;
        public List<Commessa> Commesse
        {
            get { return this._Commesse; }
        }

        public ElencoCommesse(String Tenant)
        {
            this.Tenant = Tenant;
            this._Commesse = new List<Commessa>();
        }

        public void loadCommesse()
        {
            this._Commesse = new List<Commessa>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idcommesse, anno FROM commesse ORDER BY dataInserimento DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Commesse.Add(new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        /* Returns
         * OrderID if all is ok
         * -1 if there is an error
         */
        public int Add(String Cliente, String notes, String externalID)
        {
            int rt = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            // Ricerco l'id corretto
            cmd.CommandText = "SELECT MAX(idcommesse) FROM commesse WHERE anno = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxCommID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                maxCommID = rdr.GetInt32(0) + 1;
            }
            if (externalID.Length >= 255)
            {
                externalID = externalID.Substring(0, 250);
            }
                rdr.Close();
            cmd.CommandText = "INSERT INTO commesse(idcommesse, anno, cliente, dataInserimento, note, "
                + "confirmed, confirmedBy, confirmedDate, ExternalID"
                + ") VALUES(" + maxCommID.ToString()
                + ", " + DateTime.UtcNow.Year.ToString() + ", '" + Cliente + "', '"
                + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', '"
                + notes + "', "
                + "false, "
                + "'', "
                + "null,"
                + "'" + externalID + "'"
                + ")";
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = maxCommID;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                rt = -1;
                tr.Rollback();
            }
            conn.Close();
            return rt;
        }

        public void loadCommesse(Cliente customer)
        {
            this._Commesse = new List<Commessa>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idcommesse, anno FROM commesse WHERE cliente ='" 
                + customer.CodiceCliente + "' ORDER BY dataInserimento DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Commesse.Add(new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();

            this._TempoDiLavoroTotale = new TimeSpan(0, 0, 0);
            for (int i = 0; i < this.Commesse.Count; i++)
            {
                this.Commesse[i].loadArticoli();
                for (int j = 0; j < this.Commesse[i].Articoli.Count; j++)
                {
                    this.Commesse[i].Articoli[j].loadTempoDiLavoroTotale();
                    this._TempoDiLavoroTotale += this.Commesse[i].Articoli[j].TempoDiLavoroTotale;
                }
            }
        }

        public void loadCommesse(Cliente customer, DateTime inizio, DateTime fine)
        {
            this._Commesse = new List<Commessa>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT commesse.idcommesse, commesse.anno FROM commesse "
                + " INNER JOIN tasksproduzione ON (commesse.idcommesse = tasksproduzione.idcommessa AND commesse.anno = tasksproduzione.annocommessa) "
                + " WHERE commesse.cliente = '" + customer.CodiceCliente + "'"
                +" AND tasksproduzione.lateFinish >= '" + inizio.ToString("dd/MM/yyyy HH:mm:ss") + "'"
                + " AND earlyStart <= '" + fine.ToString("dd/MM/yyyy HH:mm:ss") + "' "
                + " GROUP BY commesse.idcommesse, commesse.anno"
                + " ORDER BY commesse.anno, commesse.idcommesse";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Commesse.Add(new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();

            this._TempoDiLavoroTotale = new TimeSpan(0, 0, 0);
            for (int i = 0; i < this.Commesse.Count; i++)
            {
                this.Commesse[i].loadArticoli();
                for (int j = 0; j < this.Commesse[i].Articoli.Count; j++)
                {
                    this.Commesse[i].Articoli[j].loadTempoDiLavoroTotale();
                    this._TempoDiLavoroTotale += this.Commesse[i].Articoli[j].TempoDiLavoroTotale;
                }
            }
        }

        private TimeSpan _TempoDiLavoroTotale;
        public TimeSpan TempoDiLavoroTotale
        {
            get { return this._TempoDiLavoroTotale; }
        }
    }

    public class ElencoCommesseAperte
    {
        public String Tenant;

        public String log;

        private List<Commessa> _Commesse;
        public List<Commessa> Commesse
        {
            get { return this._Commesse; }
        }

        public ElencoCommesseAperte(String Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idcommesse, commesse.anno FROM commesse LEFT JOIN productionplan "
                + " ON (commesse.idcommesse = productionplan.commessa AND commesse.anno = productionplan.annoCommessa) "
                + " WHERE productionplan.status <> 'F' OR productionplan.status IS NULL "
                + " GROUP BY idcommesse, commesse.anno "
                + " ORDER BY commesse.anno, commesse.idcommesse";
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._Commesse = new List<Commessa>();
            while (rdr.Read())
            {
                this._Commesse.Add(new Commessa(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public int Add(String Cliente, String notes)
        {
            int rt = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            // Ricerco l'id corretto
            cmd.CommandText = "SELECT MAX(idcommesse) FROM commesse WHERE anno = " + DateTime.UtcNow.Year.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            int maxCommID = 0;
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                maxCommID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();
            cmd.CommandText = "INSERT INTO commesse(idcommesse, anno, cliente, dataInserimento, note) VALUES(" + maxCommID.ToString()
                + ", " + DateTime.UtcNow.Year.ToString() + ", '" + Cliente + "', '" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "', '" + notes + "')";
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                rt = maxCommID;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                rt = -1;
                tr.Rollback();
            }
            conn.Close();
            return rt;
        }
    }

    public class Articolo
    {
        public String Tenant;

        public String log;

        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        public String IDCombinato
        {
            get { return this.ID.ToString() + "/" + this.Year.ToString(); }
        }

        private int _Year;
        public int Year
        {
            get { return this._Year; }
        }

        private ProcessoVariante _Proc;
        public ProcessoVariante Proc
        {
            get { return this._Proc; }
        }

        /*
         * N se non pianificato
         * P se pianificato ma non iniziato
         * I se in corso d'opera
         * F se terminato
         */
        private char _Status;
        public char Status
        {
            get { return this._Status; }
        }

        private Reparto reparto;
        private int _Reparto;
        public int Reparto
        {
            get { return this._Reparto; }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    if (value == -1)
                    {
                        cmd.CommandText = "UPDATE productionplan SET reparto = null WHERE id = " + this.ID.ToString() + " AND anno = " + this.Year.ToString();
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE productionplan SET reparto = " + value + " WHERE id = " + this.ID.ToString() + " AND anno = " + this.Year.ToString();
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._Reparto = value;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                    conn.Close();
                }
                       
            }
        }
        public String RepartoNome
        {
            get
            {
                Reparto rp = new Reparto(this.Tenant, this.Reparto);
                return rp.name;
            }
        }
        private int _Commessa;
        public int Commessa
        {
            get { return this._Commessa; }
        }

        private int _AnnoCommessa;
        public int AnnoCommessa
        {
            get { return this._AnnoCommessa; }
        }

        private String _CommessaExternalID;
        public String CommessaExternalID
        {
            get { return this._CommessaExternalID; }
        }

        private String _Cliente;
        public String Cliente
        {
            get { return this._Cliente; }
        }

        public String RagioneSocialeCliente
        {
            get
            {
                Cliente customer = new Cliente(this.Tenant, this.Cliente);
                return customer.RagioneSociale;
            }
        }

        private String _Matricola;
        public String Matricola
        {
            get { return this._Matricola; }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.CommandText = "UPDATE productionPlan SET matricola='" + value + "' WHERE ID = " + this.ID.ToString();
                    cmd.Transaction = tr;
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
        }

        private DateTime _DataPrevistaConsegna;
        public DateTime DataPrevistaConsegna
        {
            get
            {
                DateTime consegna = this._DataPrevistaConsegna;
                if (this.reparto == null || this.reparto.id == -1)
                {
                    this.reparto = new Reparto(this.Tenant, this.Reparto);
                }

                consegna = TimeZoneInfo.ConvertTimeFromUtc(this._DataPrevistaConsegna, this.reparto.tzFusoOrario);
                return consegna;
            }
            set
            {
                Reparto rp = new Reparto(this.Tenant, this.Reparto);
                if (this.ID != -1 && TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario) >= DateTime.UtcNow)// && this.Status == 'N')
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE productionplan SET dataconsegnaprevista = '" + TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario).ToString("yyyy/MM/dd HH:mm:ss")
                        + "' WHERE id = " + this.ID.ToString() + " AND anno = " + this.Year.ToString();
                    try
                    {
                        this._DataPrevistaConsegna = TimeZoneInfo.ConvertTimeToUtc(value, rp.tzFusoOrario);
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        this._DataPrevistaConsegna = new DateTime(1970, 1, 1);
                        log = ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
            }
        }

        public DateTime _DataPrevistaFineProduzione;
        public DateTime DataPrevistaFineProduzione
        {
            get 
            {
                DateTime dataFine = this._DataPrevistaFineProduzione;
                if(this.reparto==null || this.reparto.id==-1)
                {
                    this.reparto = new Reparto(this.Tenant, this.Reparto);
                    dataFine = TimeZoneInfo.ConvertTimeFromUtc(this._DataPrevistaFineProduzione, this.reparto.tzFusoOrario);
                }
                else
                {
                    FusoOrario fuso = new FusoOrario(this.Tenant);
                    dataFine = TimeZoneInfo.ConvertTimeFromUtc(this._DataPrevistaFineProduzione, fuso.tzFusoOrario);
                }

                
 
                return dataFine;
            }
            set
            {
                DateTime fineProd = value;
                if (this.reparto == null || this.reparto.id == -1)
                {
                    this.reparto = new Reparto(this.Tenant, this.Reparto);
                    fineProd = TimeZoneInfo.ConvertTimeToUtc(value, this.reparto.tzFusoOrario);
                }
                else
                {
                    FusoOrario fuso = new FusoOrario(this.Tenant);
                    fineProd = TimeZoneInfo.ConvertTimeToUtc(value, fuso.tzFusoOrario);
                }
                
                if (this.ID != -1 && fineProd <= this.DataPrevistaConsegna && fineProd >= DateTime.UtcNow)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE productionplan SET dataPrevistaFineProduzione = '" + fineProd.ToString("yyyy/MM/dd HH:mm:ss")
                        + "' WHERE id = " + this.ID.ToString() + " AND anno = " + this.Year.ToString();
                    log = cmd.CommandText;
                    try
                    {
                        this._DataPrevistaFineProduzione = fineProd;
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        this._DataPrevistaFineProduzione = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
                        log += ex.Message;
                        tr.Rollback();
                    }
                    conn.Close();
                }
                else
                {
                    log = "Data non consistente." + value.ToString() + " vs " + this._DataPrevistaConsegna.ToString();
                }
            }
        }

        public DateTime DataFineUltimoTask
        {
            get
            {
                DateTime rt = new DateTime(1970, 1, 1);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(lateFinish) as dataFineProd, idArticolo, annoArticolo FROM tasksproduzione "
                    + " INNER JOIN productionplan ON (idArticolo=id AND annoarticolo=productionplan.anno) "
                    + " WHERE productionplan.id = " + this.ID.ToString()
                    + " AND productionplan.anno = " + this.Year.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    rt = rdr.GetDateTime(0);
                }
                rdr.Close();
                conn.Close();
                if(this.reparto==null || this.reparto.id==-1)
                {
                    reparto = new Reparto(this.Tenant, this.Reparto);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(rt, reparto.tzFusoOrario);
            }
        }

        public DateTime EarlyStart
        {
            get
            {
                DateTime ret = DateTime.UtcNow.AddDays(365);
                if (this.ID != -1)
                {
                    if(this.Tasks==null || this.Tasks.Count ==0)
                    { 
                        this.loadTasksProduzione();
                    }
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        if(ret > this.Tasks[i].EarlyStart)
                        {
                            ret = this.Tasks[i].EarlyStart;
                        }
                    }
                }
                if (this.reparto == null || this.reparto.id == -1)
                {
                    reparto = new Reparto(this.Tenant, this.Reparto);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(ret, reparto.tzFusoOrario);
            }
        }

        public DateTime LateStart
        {
            get
            {
                DateTime ret = this.DataPrevistaConsegna;
                if (this.ID != -1)
                {
                    if (this.Tasks == null || this.Tasks.Count == 0)
                    {
                        this.loadTasksProduzione();
                    }
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        if(ret > this.Tasks[i].LateStart)
                        {
                            ret = this.Tasks[i].LateStart;
                        }
                    }
                }
                if (this.reparto == null || this.reparto.id == -1)
                {
                    reparto = new Reparto(this.Tenant, this.Reparto);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(ret, reparto.tzFusoOrario);
            }
        }

        public DateTime EarlyFinish
        {
            get
            {
                DateTime ret = DateTime.UtcNow.AddDays(-365);
                if (this.ID != -1)
                {
                    if (this.Tasks == null || this.Tasks.Count == 0)
                    {
                        this.loadTasksProduzione();
                    }
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        if (ret < this.Tasks[i].EarlyFinish)
                        {
                            ret = this.Tasks[i].EarlyFinish;
                        }
                    }
                }
                if(this.reparto==null || this.reparto.id==-1)
                { 
                    reparto = new Reparto(this.Tenant, this.Reparto);
                }
                return TimeZoneInfo.ConvertTimeFromUtc(ret, reparto.tzFusoOrario);
            }
        }

        public DateTime LateFinish
        {
            get
            {
                return this.DataFineUltimoTask;
            }
        }

        private UserAccount _Planner;
        public UserAccount Planner
        {
            get
            {
                return this._Planner;
            }
            set
            {
                if (this.ID != -1 && this.Year != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE productionplan SET planner = '" + value.id.ToString() 
                        + "' WHERE id = " + this.ID.ToString()
                        + " AND anno = " + this.Year.ToString();

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
        }

        private int _Quantita;
        public int Quantita
        {
            get
            {
                return this._Quantita;
            }
            set
            {
                if (this.ID != -1 && this.Year != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE productionplan SET quantita = '" + value
                        + "' WHERE id = " + this.ID.ToString()
                        + " AND anno = " + this.Year.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
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
        
        private int _QuantitaProdotta;
        public int QuantitaProdotta
        {
            get
            {
                return this._QuantitaProdotta;
            }
            set
            {
                if (this.ID != -1 && this.Year != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE productionplan SET quantitaProdotta = '" + value
                        + "' WHERE id = " + this.ID.ToString()
                        + " AND anno = " + this.Year.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
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

        private int _MeasurementUnitID;
        public int MeasurementUnitID
        {
            get { return this._MeasurementUnitID; }
        }

        public MeasurementUnit MeasurementUnit;

        private String _KanbanCardID;
        public String KanbanCardID
        {
            get
            {
                return this._KanbanCardID;
            }
            set
            {
                if (this.ID != -1 && this.Year!=-1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE productionplan SET kanbancard = '" + value + "' WHERE id = "
                        + this.ID.ToString()
                        + " AND anno = " + this.Year.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        this._KanbanCardID = value;
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
        
        public DateTime DataInserimento
        {
            get
            {
                Commessa cm = new Commessa(this.Tenant, this.Commessa, this.AnnoCommessa);
                return cm.DataInserimento;
            }
        }

        private String _ProductExternalID;
        public String ProductExternalID
        {
            get { return this._ProductExternalID; }
        }
            
        public List<ProductParameter> Parameters;

        public List<TaskParameter> TaskParameters;

        public TimeSpan PlannedWorkingTime
        {
            get
            {
                TimeSpan ret = new TimeSpan(0, 0, 0);
                if(this.ID!=-1 && this.Year!=-1)
                { 
                this.loadTasksProduzione();
                    foreach(var t in this.Tasks)
                    {
                        ret = ret.Add(t.TempoC);
                    }
                }
                return ret;
            }
        }

        /* This indicator is the productivity on the task
         * Productivity is calculated as the relation between planned working time and real working time 
         * if < 1 performance is poor
         * if > 1 performance is good
         */
        public Double Productivity
        {
            get
            {
                if (this.ID != -1 && this.Year!=-1 && this.Status == 'F')
                {
                    return (this.PlannedWorkingTime.TotalSeconds / this.WorkingTime.TotalSeconds);
                }
                else
                {
                    return 0.0;
                }
            }
        }

        private DateTime _EndProductionDateReal;
        public DateTime EndProductionDateReal
        {
            get
            {
                return this._EndProductionDateReal;
            }
        }

        private TimeSpan _WorkingTimePlanned;
            public TimeSpan WorkingTimePlanned
        {
            get
            {
                return this._WorkingTimePlanned;
            }
        }

        public Articolo(String Tenant, int idArticolo, int AnnoArticolo)
        {
            this.Tenant = Tenant;

            this._TempoDiLavoroTotale = new TimeSpan(0, 0, 0);
            this._LeadTimes = new List<TimeSpan>();
            this._ProductExternalID = "";
            this._EndProductionDateReal = new DateTime(1970, 1, 1);
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT processo, revisione, variante, matricola, status, reparto, startTime, commessa, annoCommessa, "
            + " dataConsegnaPrevista, commesse.cliente, dataPrevistaFineProduzione, planner, productionplan.quantita, "
            + "productionplan.quantitaProdotta, productionplan.kanbanCard, productionplan.leadtime, productionplan.workingtime, productionplan.delay, productionplan.measurementunit, "
            + " commesse.ExternalID, endproductiondatereal, WorkingTimePlanned "
            + " FROM productionplan INNER JOIN commesse ON (productionplan.commessa = commesse.idCommesse AND commesse.anno = productionplan.annoCommessa) WHERE id = " + idArticolo.ToString()
            + " AND productionplan.anno = " + AnnoArticolo.ToString()
            + " ORDER BY productionplan.anno DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = idArticolo;
                this._Proc = new ProcessoVariante(this.Tenant, new processo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)), new variante(this.Tenant, rdr.GetInt32(2)));
                this._Proc.loadReparto();
                this._Proc.process.loadFigli(this._Proc.variant);
                this._ProductExternalID = this._Proc.ExternalID;
                if (!rdr.IsDBNull(3))
                {
                    this._Matricola = rdr.GetString(3);
                }
                this._Status = rdr.GetChar(4);
                if (!rdr.IsDBNull(5))
                {
                    this._Reparto = rdr.GetInt32(5);
                }
                else
                {
                    this._Reparto = -1;
                }
                this._Commessa = rdr.GetInt32(7);
                this._AnnoCommessa = rdr.GetInt32(8);
                this._DataPrevistaConsegna = rdr.GetDateTime(9);
                this._Year = AnnoArticolo;
                this._Cliente = rdr.GetString(10);
                if (!rdr.IsDBNull(11))
                {
                    this._DataPrevistaFineProduzione = rdr.GetDateTime(11);
                }
                else
                {
                    Reparto rp = new Reparto(this.Tenant, this.Reparto);
                    this._DataPrevistaFineProduzione = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1), rp.tzFusoOrario);
                }
                if (!rdr.IsDBNull(12))
                {
                    this._Planner = new UserAccount(rdr.GetString(12));
                }
                else
                {
                    this._Planner = null;
                }
                this._Quantita = rdr.GetInt32(13);
                if (!rdr.IsDBNull(14))
                {
                    this._QuantitaProdotta = rdr.GetInt32(14);
                }
                else
                {
                    this._QuantitaProdotta = 0; 
                }
                if (!rdr.IsDBNull(15))
                {
                    this._KanbanCardID = rdr.GetString(15);
                }
                else
                {
                    this._KanbanCardID = "";
                }
                if(!rdr.IsDBNull(16))
                {
                    this._LeadTime = rdr.GetTimeSpan(16);
                }
                else
                {
                    this._LeadTime = new TimeSpan(0, 0, 0);
                }
                if (!rdr.IsDBNull(17))
                {
                    this._WorkingTime = rdr.GetTimeSpan(17);
                }
                else
                {
                    this._WorkingTime = new TimeSpan(0, 0, 0);
                }
                if (!rdr.IsDBNull(18))
                {
                    this._Delay = rdr.GetTimeSpan(18);
                }
                else
                {
                    this._Delay = new TimeSpan(0, 0, 0);
                }

                this._MeasurementUnitID = rdr.GetInt32(19);
                this._CommessaExternalID = rdr.GetString(20);
                this._EndProductionDateReal = new DateTime(1970, 1, 1);
                if(!rdr.IsDBNull(21))
                {
                    this._EndProductionDateReal=rdr.GetDateTime(21);
                }
                if(!rdr.IsDBNull(22))
                {
                    this._WorkingTimePlanned = rdr.GetTimeSpan(22);
                }
                else
                {
                    this._WorkingTimePlanned = new TimeSpan(0, 0, 0);
                }
            }
            else
            {
                this._ID = -1;
                this._AnnoCommessa = 1900;
                this._Commessa = -1;
                this._DataPrevistaConsegna = new DateTime(1900, 1, 1);
                this._ID = -1;
                this._Proc = null;
                this._Reparto = -1;
                this._Status = '\0';
                this._Year = 1900;
                this._Planner = null;
                this._KanbanCardID = "";
                this._LeadTime = new TimeSpan(0, 0, 0);
                this._WorkingTime = new TimeSpan(0, 0, 0);
                this._Delay = new TimeSpan(0, 0, 0);
                this._MeasurementUnitID = -1;
                this._CommessaExternalID = "";
                this._EndProductionDateReal = new DateTime(1970, 1, 1);
            }
            rdr.Close();
            conn.Close();
        }

        public Articolo(String Tenant, KanbanCard card)
        {
            this.Tenant = Tenant;

            this._LeadTimes = new List<TimeSpan>();
            KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
            if (kboxCfg.KanbanBoxEnabled && card.ekanban_string.Length > 0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT processo, revisione, variante, matricola, status, reparto, startTime, commessa, annoCommessa, "
                + " dataConsegnaPrevista, commesse.cliente, dataPrevistaFineProduzione, planner, productionplan.quantita, "
                + "productionplan.quantitaProdotta, productionplan.kanbanCard, productionplan.id, productionplan.anno, productionplan.leadtime, productionplan.workingtime, productionplan.delay, productionplan.measurementunit, "
                + "commesse.ExternalID, productionplan.WorkingTimePlanned "
                + " FROM productionplan INNER JOIN commesse ON (productionplan.commessa = commesse.idCommesse AND commesse.anno = productionplan.annoCommessa) "
                + " WHERE productionplan.kanbanCard LIKE '" + card.ekanban_string.ToString() + "' AND "
                + " productionplan.kanbanCard IS NOT NULL";

                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    this._ID = rdr.GetInt32(16);
                    this._Year = rdr.GetInt32(17);
                    this._Proc = new ProcessoVariante(this.Tenant, new processo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)), new variante(this.Tenant, rdr.GetInt32(2)));
                    this._Proc.loadReparto();
                    this._Proc.process.loadFigli(this._Proc.variant);
                    if (!rdr.IsDBNull(3))
                    {
                        this._Matricola = rdr.GetString(3);
                    }
                    this._Status = rdr.GetChar(4);
                    if (!rdr.IsDBNull(5))
                    {
                        this._Reparto = rdr.GetInt32(5);
                    }
                    else
                    {
                        this._Reparto = -1;
                    }
                    this._Commessa = rdr.GetInt32(7);
                    this._AnnoCommessa = rdr.GetInt32(8);
                    this._DataPrevistaConsegna = rdr.GetDateTime(9);

                    this._Cliente = rdr.GetString(10);
                    if (!rdr.IsDBNull(11))
                    {
                        this._DataPrevistaFineProduzione = rdr.GetDateTime(11);
                    }
                    else
                    {
                        this._DataPrevistaFineProduzione = new DateTime(1970, 1, 1);
                    }
                    if (!rdr.IsDBNull(12))
                    {
                        this._Planner = new UserAccount(rdr.GetString(12));
                    }
                    else
                    {
                        this._Planner = null;
                    }
                    this._Quantita = rdr.GetInt32(13);
                    if (!rdr.IsDBNull(14))
                    {
                        this._QuantitaProdotta = rdr.GetInt32(14);
                    }
                    else
                    {
                        this._QuantitaProdotta = 0;
                    }
                    if (!rdr.IsDBNull(15))
                    {
                        this._KanbanCardID = rdr.GetString(15);
                    }
                    else
                    {
                        this._KanbanCardID = "";
                    }
                    this._TempoDiLavoroTotale = new TimeSpan(0, 0, 0);

                    if (!rdr.IsDBNull(18))
                    {
                        this._LeadTime = rdr.GetTimeSpan(18);
                    }
                    else
                    {
                        this._LeadTime = new TimeSpan(0, 0, 0);
                    }
                    if (!rdr.IsDBNull(19))
                    {
                        this._WorkingTime = rdr.GetTimeSpan(19);
                    }
                    else
                    {
                        this._WorkingTime = new TimeSpan(0, 0, 0);
                    }
                    if (!rdr.IsDBNull(20))
                    {
                        this._Delay = rdr.GetTimeSpan(20);
                    }
                    else
                    {
                        this._Delay = new TimeSpan(0, 0, 0);
                    }
                    if (!rdr.IsDBNull(21))
                    {
                        this._WorkingTimePlanned = rdr.GetTimeSpan(21);
                    }
                    else
                    {
                        this._WorkingTimePlanned = new TimeSpan(0, 0, 0);
                    }
                    this._MeasurementUnitID = rdr.GetInt32(21);
                    this._CommessaExternalID = rdr.GetString(22);
                }
                else
                {
                    this._TempoDiLavoroTotale = new TimeSpan(0, 0, 0);
                    this._ID = -1;
                    this._AnnoCommessa = 1900;
                    this._Commessa = -1;
                    this._DataPrevistaConsegna = new DateTime(1900, 1, 1);
                    this._ID = -1;
                    this._Proc = null;
                    this._Reparto = -1;
                    this._Status = '\0';
                    this._Year = 1900;
                    this._Planner = null;
                    this._KanbanCardID = "";
                    this._LeadTime = new TimeSpan(0, 0, 0);
                    this._WorkingTime = new TimeSpan(0, 0, 0);
                    this._Delay = new TimeSpan(0, 0, 0);
                    this._MeasurementUnitID = -1;
                    this._CommessaExternalID = "";
                }
                rdr.Close();
                conn.Close();
            }
            else
            {
                this._ID = -1;
                this._AnnoCommessa = 1900;
                this._Commessa = -1;
                this._DataPrevistaConsegna = new DateTime(1900, 1, 1);
                this._ID = -1;
                this._Proc = null;
                this._Reparto = -1;
                this._Status = '\0';
                this._Year = 1900;
                this._Planner = null;
                this._KanbanCardID = "";
            }
        }

        public bool Delete()
        {
            bool rt = false;
            if (this.ID != -1)
            {
                if (this.Status == 'N')
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "DELETE FROM productionPlan WHERE ID = "+ this.ID.ToString() + " AND anno = " + this.Year.ToString();
                    try
                    {
                        this.loadTaskParameters();
                        for(int i = 0; i < this.TaskParameters.Count; i++)
                        {
                            this.TaskParameters[i].Delete();
                        }

                        this.loadParameters();
                        for(int i = 0; i < this.Parameters.Count; i++)
                        {
                            this.Parameters[i].Delete();
                        }
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
                else
                {
                    rt = false;

                }
            }
            return rt;
        }

        private List<TaskProduzione> _Tasks;
        public List<TaskProduzione> Tasks
        {
            get { return this._Tasks; }
        }

        public void loadTasksProduzione()
        {
            this._Tasks = new List<TaskProduzione>();
            if (this.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE idArticolo = " + this.ID.ToString() +
                    " AND annoArticolo = " + this.Year.ToString() + " ORDER BY lateStart, earlyStart";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Tasks.Add(new TaskProduzione(this.Tenant, rdr.GetInt32(0)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public DateTime DataFineAttivita
        {
            get
            {
                DateTime max = new DateTime(1970, 1, 1);
                if (this.Status == 'F')
                {
                    this.loadTasksProduzione();

                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        if (this.Tasks[i].DataFineTask > max)
                        {
                            max = this.Tasks[i].DataFineTask;
                        }
                    }
                }
                if (this.reparto == null || this.reparto.id == -1)
                {
                    reparto = new Reparto(this.Tenant, this.Reparto);
                }
                //return TimeZoneInfo.ConvertTimeFromUtc(max, reparto.tzFusoOrario);
                return max;
            }
        }

        public ConfigurazioneRitardoArticolo EventoRitardo;
        public void loadEventoRitardo()
        {
            if (this.ID != -1 && this.Year != -1)
            {
                this.EventoRitardo = new ConfigurazioneRitardoArticolo(this.Tenant, this);
            }
        }

        public ConfigurazioneWarningArticolo EventoWarning;
        public void loadEventoWarning()
        {
            if (this.ID != -1 && this.Year != -1)
            {
                this.EventoWarning = new ConfigurazioneWarningArticolo(this.Tenant, this);
            }
        }

        // Calcolato come il ritardo massimo fra tutti i task del prodotto.
        public TimeSpan Ritardo
        {
            get
            {
                TimeSpan rit = new TimeSpan(0, 0, 0);
                if(this.Tasks==null || this.Tasks.Count ==0)
                { 
                    this.loadTasksProduzione();
                }
                if (this.Status == 'F')
                {
                    DateTime lastTask = new DateTime(1970, 1, 1);
                    int indLastTask=-1;
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        if (lastTask < this.Tasks[i].DataFineTask)
                        {
                            lastTask = this.Tasks[i].DataFineTask;
                            indLastTask = i;
                        }
                    }

                    if (indLastTask > -1 && indLastTask < this.Tasks.Count)
                    {
                        rit = this.Tasks[indLastTask].ritardo;
                    }
                }
                else
                {
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        TimeSpan rit2 = this.Tasks[i].ritardo;
                        if (rit < rit2 && this.Tasks[i].Status!='F')
                        {
                            rit = rit2;
                        }
                    }
                }
                return rit;
            }
        }
    
        // Metodo che consente di rimuovere un prodotto non iniziato dal piano di produzione. Lo riporta allo stato 'N'
        public bool Depianifica()
        {
            bool ret = false;
            if (this.ID != -1 && this.Year != -1 && this.Status == 'P')
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                
              try
                {
                    this.loadTasksProduzione();

                    // Delete assigned operators
                    foreach(var t in this.Tasks)
                    {
                        t.loadAssignedOperators();
                        foreach(var op in t.AssignedOperators)
                        {
                            t.deleteAssignedOperator(op);
                        }
                    }


                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        cmd.CommandText = "DELETE FROM taskreschedulelog WHERE task = " + this.Tasks[i].TaskProduzioneID.ToString();
                        cmd.ExecuteNonQuery();
                    }

                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        cmd.CommandText = "DELETE FROM prectasksproduzione WHERE prec = "
                            + this.Tasks[i].TaskProduzioneID.ToString()
                            + " OR succ = " + this.Tasks[i].TaskProduzioneID.ToString();
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DELETE FROM registroeventiproduzione WHERE taskID = " + this.Tasks[i].TaskProduzioneID.ToString();
                        cmd.ExecuteNonQuery();
                    }

                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        cmd.CommandText = "DELETE FROM tasksproduzione WHERE taskID = " + this.Tasks[i].TaskProduzioneID.ToString();
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = "UPDATE productionplan SET status = 'N' WHERE id = "
                        + this.ID.ToString()
                        + " AND anno = " + this.Year.ToString();
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

        private TimeSpan _TempoDiLavoroTotale;        
        public TimeSpan TempoDiLavoroTotale
        {
            get
            {
                return this._TempoDiLavoroTotale;
            }
        }
        public double TempoDiLavoroTotaleDbl
        {
            get
            {
                return Math.Round(this.TempoDiLavoroTotale.TotalHours, 2);
            }
        }

        public void loadTempoDiLavoroTotale()
        {
            this._TempoDiLavoroTotale = new TimeSpan(0, 0, 0);
            TimeSpan totTempo = new TimeSpan(0, 0, 0);
            if (this.Tasks == null || this.Tasks.Count == 0)
            {
                this.loadTasksProduzione();
            }
            for (int i = 0; i < this.Tasks.Count; i++)
            {
                this._TempoDiLavoroTotale = this._TempoDiLavoroTotale.Add(this.Tasks[i].TempoDiLavoroEffettivo);
            }
        }
        
        public void loadTempoDiLavoroTotale(DateTime startDate, DateTime endDate)
        {
            TimeSpan _TempoDiLavoroTotale = new TimeSpan(0, 0, 0);
            if (this.Tasks == null || this.Tasks.Count == 0)
            {
                this.loadTasksProduzione();
            }
            for (int i = 0; i < this.Tasks.Count; i++)
            {
                this._TempoDiLavoroTotale = this._TempoDiLavoroTotale.Add(this.Tasks[i].getTempoDiLavoroEffettivo(startDate, endDate));
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

        public TimeSpan WorkingTimeUnitary
        {
            get
            {
                return new TimeSpan(this.WorkingTime.Ticks / this.QuantitaProdotta);
            }
        }

        public TimeSpan TempoDiLavoroUnitario
        {
            get
            {
                return new TimeSpan(this.TempoDiLavoroTotale.Ticks / this.QuantitaProdotta);
            }
        }

        public Double TempoDiLavoroUnitarioHoursDbl
        {
            get { return this.TempoDiLavoroUnitario.TotalHours; }
        }

        private TimeSpan _Delay;
        public TimeSpan Delay
        {
            get
            {
                return this._Delay;
            }
        }

        private List<TimeSpan> _LeadTimes;
        public List<TimeSpan> LeadTimes
        {
            get
            {
                return this._LeadTimes; 
            }
        }

        public void loadLeadTimes()
        {
            this._LeadTimes = new List<TimeSpan>();
            if (this.ID != -1 && this.Year != -1 && this.Status == 'F')
            {
                DateTime inizio = DateTime.UtcNow.AddDays(5);
                DateTime fine = new DateTime(1970, 1, 1);
                if (this.Tasks == null || this.Tasks.Count == 0)
                {
                    this.loadTasksProduzione();
                }
                inizio = (from d in this.Tasks select d.DataInizioTask).Min();
                fine = (from d in this.Tasks select d.DataFineTask).Max();


                if (this.reparto == null || this.reparto.id == -1)
                {
                    reparto = new Reparto(this.Tenant, this.Reparto);
                }
                reparto.loadCalendario(inizio.AddDays(-30), DateTime.UtcNow.AddDays(1));
                this.log = inizio.ToString("dd/MM/yyyy HH:mm") + " - " + fine.ToString("dd/MM/yyyy HH:mm");

                for (int i = 0; i < reparto.CalendarioRep.Intervalli.Count; i++)
                {
                    if (inizio >= reparto.CalendarioRep.Intervalli[i].Inizio && reparto.CalendarioRep.Intervalli[i].Fine <= fine && inizio <= reparto.CalendarioRep.Intervalli[i].Fine)
                    {
                        TimeSpan curr = (reparto.CalendarioRep.Intervalli[i].Fine - inizio);
                        this._LeadTimes.Add(curr);
                    }
                    else if (inizio <= reparto.CalendarioRep.Intervalli[i].Inizio && fine >= reparto.CalendarioRep.Intervalli[i].Fine)
                    {
                        TimeSpan curr = (reparto.CalendarioRep.Intervalli[i].Fine - reparto.CalendarioRep.Intervalli[i].Inizio);
                        this._LeadTimes.Add(curr);
                    }
                    else if (reparto.CalendarioRep.Intervalli[i].Inizio >= inizio && fine <= reparto.CalendarioRep.Intervalli[i].Fine && fine >= reparto.CalendarioRep.Intervalli[i].Inizio)
                    {
                        TimeSpan curr = (fine - reparto.CalendarioRep.Intervalli[i].Inizio);
                        this._LeadTimes.Add(curr);
                    }
                }
            }
        }

        public TimeSpan CalculateLeadTime()
        {
                TimeSpan sommaLTs = new TimeSpan(0, 0, 0);
            this.loadTasksProduzione();
            DateTime min = new DateTime(2999, 1, 1);
            DateTime max = new DateTime(1970, 1, 1);
            TimeSpan ret = new TimeSpan(0, 0, 0);
            if(this.Status == 'F')
            {
                // Find earliest started task and latest finished task
                for (int i = 0; i < this.Tasks.Count; i++)
                {
                    if(min > this.Tasks[i].DataInizioTask)
                    {
                        min = this.Tasks[i].DataInizioTask;
                    }
                    if(max < this.Tasks[i].DataFineTask)
                    {
                        max = this.Tasks[i].DataFineTask;
                    }
                }
                /*if(min < max && min < new DateTime(2999, 1, 1) && max > new DateTime(1970, 1, 1))
                {
                    ret = max - min;
                }*/

                Reparto rp = new Reparto(this.Tenant, this.Reparto);
                rp.loadCalendario(min.AddDays(-30), max.AddDays(7));
                for(int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
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
                }
            }
            return ret;
        }

        private TimeSpan _LeadTime;
        public TimeSpan LeadTime
        {
            get
            {
                return this._LeadTime;
            }
        }

        public double LeadTimeDbl
        {
            get
            {
                this.loadLeadTimes();
                double sommaLTs = this.LeadTimes.Select(x=>x.TotalHours).Sum();
                return Math.Round(sommaLTs, 2);
            }
        }

        public TimeSpan TempoDiLavoroPrevisto
        {
            get
            {
                TimeSpan tempoTot = new TimeSpan(0, 0, 0);
                if (this.Tasks == null || this.Tasks.Count == 0)
                {
                    this.loadTasksProduzione();
                }
                for (int i = 0; i < this.Tasks.Count; i++)
                {
                    tempoTot = tempoTot.Add(this.Tasks[i].TempoDiLavoroPrevisto);
                }
                return tempoTot;
            }
        }

        /* Somma di tasks completati * tempo di lavoro teorico */
        public TimeSpan TempoDiLavoroCompletatoPrevisto
        {
            get
            {
                TimeSpan tempoTot = new TimeSpan(0, 0, 0);
                if (this.Tasks == null || this.Tasks.Count == 0)
                {
                    this.loadTasksProduzione();
                }
                for (int i = 0; i < this.Tasks.Count; i++)
                {
                    if (this.Tasks[i].Status == 'F')
                    {
                        tempoTot = tempoTot.Add(this.Tasks[i].TempoDiLavoroPrevisto);
                    }
                }
                return tempoTot;
            }
        }

        /* numero tasks completati / numero tasks totali */
        public double IndicatoreCompletamentoTasks
        {
            get
            {
                double ret = 0;
                int numTasksTerminati=0;
                if (this.Tasks == null || this.Tasks.Count == 0)
                {
                    this.loadTasksProduzione();
                }
                if (this.Tasks.Count > 0)
                {
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        if (this.Tasks[i].Status == 'F')
                        {
                            numTasksTerminati++;
                        }
                    }

                    ret = 100 * numTasksTerminati / this.Tasks.Count;
                }
                return ret;

            }
        }
    
        /* tempo di lavoro previsto completato / tempo di lavoro previsto totale*/
        public double IndicatoreCompletamentoTempoPrevisto
        {
            get
            {
                double ret = 0;
                double tasksTerminati = 0;
                if (this.Tasks == null || this.Tasks.Count == 0)
                {
                    this.loadTasksProduzione();
                }
                if (this.Tasks.Count > 0)
                {
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        if (this.Tasks[i].Status == 'F')
                        {
                            tasksTerminati += this.Tasks[i].TempoDiLavoroPrevisto.TotalSeconds;
                        }
                    }

                    ret = tasksTerminati / this.TempoDiLavoroPrevisto.TotalSeconds * 100;
                }
                return ret;

            }
        }

        public Boolean changeKanbanBoxCardStatus(String newStatus)
        {
            Boolean ret = false;
            KanbanBoxConfig kboxCfg = (KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
            if (this.KanbanCardID.Length > 0 && kboxCfg.KanbanBoxEnabled)
            {
                if (this.KanbanCardID.Length > 0)
                {
                    String url = kboxCfg.Url + this.KanbanCardID;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("X-API-KEY", kboxCfg.X_API_KEY);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    client.BaseAddress = new Uri(url);

                    // List data response.
                    HttpResponseMessage response = null;
                    HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("new_status",newStatus)
                });
                    try
                    {
                        response = client.PutAsync(url, content).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            log = "Card: " + this.KanbanCardID + " " + response.StatusCode + " " + response.RequestMessage + " \n";
                            ret = true;
                        }
                        else
                        {
                            ret = false;
                            throw new Exception(response.IsSuccessStatusCode.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        ret = false;
                        log = ex.Message;
                    }

                }
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if all is ok
         * 2 if status not ok or Articolo non esistente
         * 3 se le date fine produzione e date consegna impostate non sono corrette
         * 4 se è avvenuto un errore in fase di simulazione del lancio in produzione
         */
        public int SpostaPianificazione(DateTime dFineProd, DateTime dConsegna)
        {
            int ret = 1;
            if (this.ID != -1 && this.Year != -1 && (this.Status=='I' || this.Status=='P'))
            {
                if (dFineProd >= DateTime.UtcNow && dFineProd <= dConsegna)
                {
                    this.DataPrevistaConsegna = dConsegna;
                    this.DataPrevistaFineProduzione = dFineProd;

                    List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
                    Reparto rp = new Reparto(this.Tenant, this.Reparto);

                    this.Proc.process.loadFigli(this.Proc.variant);
                    for (int i = 0; i < this.Proc.process.subProcessi.Count; i++)
                    {
                        TaskVariante tskVar = new TaskVariante(this.Tenant, new processo(this.Tenant, this.Proc.process.subProcessi[i].processID, this.Proc.process.subProcessi[i].revisione), this.Proc.variant);
                        tskVar.loadTempiCiclo();
                        TempoCiclo tc = new TempoCiclo(this.Tenant, tskVar.Task.processID, tskVar.Task.revisione, this.Proc.variant.idVariante, tskVar.getDefaultOperatori());
                        if (tc.Tempo != null)
                        {
                            lstTasks.Add(new TaskConfigurato(this.Tenant, tskVar, tc, rp.id, this.Quantita));
                        }
                    }

                    ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(this.Tenant, this, lstTasks, rp, this.Quantita);
                    int rt1 = prcCfg.SimulaIntroduzioneInProduzione();
                    if (rt1 == 1)
                    {
                        if (this.Tasks == null || this.Tasks.Count == 0)
                        {
                            this.loadTasksProduzione();
                        }
                        for (int i = 0; i < this.Tasks.Count; i++)
                        {
                            if (this.Tasks[i].Status == 'N')
                            {
                                // Cerco il task processo giusto
                                for (int j = 0; j < prcCfg.Processi.Count; j++)
                                {
                                    if (this.Tasks[i].OriginalTask == prcCfg.Processi[j].Task.Task.processID && this.Tasks[i].OriginalTaskRevisione == prcCfg.Processi[j].Task.Task.revisione && this.Tasks[i].VarianteID == prcCfg.Processi[j].Task.variant.idVariante)
                                    {
                                        this.Tasks[i].EarlyStart = prcCfg.Processi[j].EarlyStartDate;
                                        this.Tasks[i].EarlyFinish = prcCfg.Processi[j].EarlyFinishDate;
                                        this.Tasks[i].LateStart = prcCfg.Processi[j].LateStartDate;
                                        this.Tasks[i].LateFinish = prcCfg.Processi[j].LateFinishDate;
                                        this.Tasks[i].deleteSegnalazioneRitardo();
                                        log += this.Tasks[i].log + "<br />";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ret = 4;
                    }
                }
                else
                {
                    ret = 3;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        public Boolean Riesuma()
        {
            Boolean res = false;
            if (this.ID != -1 && this.Year != -1 && this.Status == 'F')
            {
                // this.Status = I
                // tutti i tasks --> P
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    this.loadTasksProduzione();
                    for (int i = 0; i < this.Tasks.Count; i++)
                    {
                        cmd.CommandText = "UPDATE registroeventitaskproduzione SET evento = 'P' WHERE "
                        + "task = " + this.Tasks[i].TaskProduzioneID.ToString()
                        + " AND evento = 'F'";
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "UPDATE tasksproduzione SET status='P' WHERE idArticolo = " + this.ID.ToString()
                        + " AND annoArticolo = " + this.Year.ToString()
                        + " AND status = 'F'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "UPDATE productionplan SET status='I' WHERE id = " + this.ID.ToString()
                        + " AND anno = " + this.Year.ToString();
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

        public Boolean addParameter(int ParamID, String name, String description, Boolean isFixed, int sequence)
        {
            Boolean ret = false;
            if (this.ID != -1 && this.Year > 2000)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "INSERT INTO productparameters(ProductID, ProductYear, paramID, "
                    + " paramName, paramDescription, isFixed, sequence) VALUES("
                    + this.ID.ToString() + ", "
                    + this.Year.ToString() + ", "
                    + ParamID.ToString() + ", "
                    + "'" + name + "', "
                    + "'" + description + "', "
                    + isFixed.ToString() + ", "
                    + sequence.ToString()
                    + ")";
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

        public void loadParameters()
        {
            this.Parameters = new List<ProductParameter>();
            if(this.ID!=-1 && this.Year>2000)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ProductID, ProductYear, paramID, paramCategory "
                    + " FROM productparameters WHERE ProductID = " + this.ID.ToString()
                    + " AND ProductYear = " + this.Year.ToString()
                    + " ORDER BY sequence";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    this.Parameters.Add(new ProductParameter(rdr.GetInt32(0), rdr.GetInt32(1),
                        new ProductParametersCategory(this.Tenant, rdr.GetInt32(3)),
                        rdr.GetInt32(2)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void loadTaskParameters()
        {
            this.TaskParameters = new List<TaskParameter>();
            if(this.ID!=-1 && this.Year > 2010)
            {
                this.loadTasksProduzione();
                for(int i = 0; i < this.Tasks.Count; i++)
                {
                    this.Tasks[i].loadParameters();
                    for(int j=0; j < this.Tasks[i].Parameters.Count; j++)
                    {
                        this.TaskParameters.Add(this.Tasks[i].Parameters[j]);
                    }
                }
            }
        }

        public void loadMeasurementUnit()
        {
            if (this.MeasurementUnitID != -1)
            {
                this.MeasurementUnit = new MeasurementUnit(this.Tenant, this.MeasurementUnitID);
            }
            else
            {
                this.MeasurementUnit = null;
            }
        }

        /* Returns:
         * 0 if generic error
         * 1 if product correctly ended
         * 2 if user not found
         * 3 if product not found
         */
         public int CompleteProductBruteForce(User usr)
        {
            int ret = 0;
            if(this.ID!=-1 && this.Year > 2010)
            { 
            if(usr!=null && usr.username.Length>0)
            {
                    this.loadTasksProduzione();
                    var lstTasks = this.Tasks.Where(z=>z.Status != 'F').OrderBy(x => x.LateFinish);
                    foreach(var tsk in lstTasks)
                    {
                        Postazione currWS = new Postazione(this.Tenant, tsk.PostazioneID);
                        if(currWS!=null)
                        {
                            Boolean checkCheckIn = usr.DoCheckIn(currWS);
                            if(checkCheckIn)
                            { 
                                Boolean checkStart = tsk.Start(usr);
                                if(checkStart)
                                {
                                    TaskVariante origTsk = new TaskVariante(this.Tenant, new App_Code.processo(this.Tenant, tsk.OriginalTask,
                            tsk.OriginalTaskRevisione), new variante(this.Tenant, tsk.VarianteID));
                                    origTsk.loadParameters();
                                    foreach(var prm in origTsk.Parameters)
                                    {
                                        if(prm.isRequired)
                                        { 
                                        int paramret = tsk.CompileParameter(usr, 
                                            prm.ParameterCategory.ID, 
                                            prm.Name, "Brute force completed by " + usr.username + " on " + DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")
                                            + " (UTC)");
                                        }
                                    }
                                }

                                Boolean checkComplete = tsk.Complete(usr);
                            }
                            Boolean checkOut = usr.DoCheckOut(currWS);
                        }
                    }
                        ret = 1;

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
    }

    public class ElencoArticoliAperti
    {
        public String Tenant;

        public String log;
        private List<Articolo> _ArticoliAperti;
        public List<Articolo> ArticoliAperti
    {
        get
        {
            return this._ArticoliAperti;
        }
    }

        public ElencoArticoliAperti(String Tenant)
        {
            this.Tenant = Tenant;

            this._ArticoliAperti = new List<Articolo>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, anno FROM productionplan WHERE status <> 'F' ORDER BY dataConsegnaPrevista";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._ArticoliAperti.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public ElencoArticoliAperti(int idReparto)
        {
            this._ArticoliAperti = new List<Articolo>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, anno FROM productionplan WHERE status <> 'F' AND reparto = "
            + idReparto.ToString() + " ORDER BY dataConsegnaPrevista";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._ArticoliAperti.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class ElencoArticoli
    {
        public String Tenant;

        public String log;
        private List<Articolo> _ListArticoli;
        public List<Articolo> ListArticoli
        {
            get
            {
                return this._ListArticoli;
            }
        }

        public List<FlatProduct> ProductList;

        // Carica tutti i prodotti
        public ElencoArticoli(String Tenant)
        {
            this.Tenant = Tenant;
            this._ListArticoli = new List<Articolo>();
            this.ProductList = new List<FlatProduct>();
        }

        public void loadArticoli()
        {
            this._ListArticoli = new List<Articolo>();
            this.ProductList = new List<FlatProduct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, anno FROM productionplan ORDER BY anno DESC, id DESC";
            
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public void loadProductList()
        {
            this.ProductList = new List<FlatProduct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            //                cmd.CommandText = "SELECT id, anno FROM productionplan ORDER BY anno DESC, id DESC";
            cmd.CommandText = "SELECT productionplan.id, productionplan.anno, processo.name, varianti.nomevariante,"
                                            + " productionplan.matricola, productionplan.quantita, commesse.cliente, "
                                            + " anagraficaclienti.ragsociale, processo.processID, processo.revisione, varianti.idVariante, "
                                            + " productionplan.status "
                                            + " FROM productionplan INNER JOIN variantiprocessi "
                                            + " ON(productionplan.processo = variantiprocessi.processo AND productionplan.revisione = variantiprocessi.revproc"
                                            + " AND productionplan.variante = variantiprocessi.variante) INNER JOIN processo "
                                            + " ON(processo.processID = variantiprocessi.processo AND processo.revisione = variantiprocessi.revProc)"
                                            + " INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante) INNER JOIN commesse "
                                            + " ON(productionplan.commessa = commesse.idcommesse AND productionplan.annocommessa = commesse.anno)"
                                            + " INNER JOIN anagraficaclienti ON(commesse.cliente = anagraficaclienti.codice)"
                                            + " ORDER BY productionplan.anno DESC, productionplan.id DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                FlatProduct curr = new FlatProduct();
                curr.ProductID = rdr.GetInt32(0);
                curr.ProductYear = rdr.GetInt32(1);
                curr.processName = rdr.GetString(2);
                curr.varianteName = rdr.GetString(3);
                if (!rdr.IsDBNull(4))
                {
                    curr.SerialNumber = rdr.GetString(4);
                }
                curr.Quantity = rdr.GetInt32(5);
                curr.CustomerID = rdr.GetString(6);
                curr.CustomerName = rdr.GetString(7);
                curr.processID = rdr.GetInt32(8);
                curr.revID = rdr.GetInt32(9);
                curr.varID = rdr.GetInt32(10);
                curr.Status = rdr.GetChar(11);

                this.ProductList.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }

        /* prcVar: articoli generati da questo processo-varianti
         * closed: solo articoli il cui processo produttivo è già terminato
         */
        public ElencoArticoli(String Tenant, ProcessoVariante prcVar, bool closed)
        {
            this.Tenant = Tenant;

            this._ListArticoli = new List<Articolo>();
            if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.process.revisione != -1 && prcVar.variant.idVariante != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, anno FROM productionplan WHERE "
                    + " processo = " + prcVar.process.processID.ToString()
                    + " AND revisione = " + prcVar.process.revisione.ToString()
                    + " AND variante = " + prcVar.variant.idVariante.ToString();
                if (closed == true)
                {
                    cmd.CommandText += " AND status = 'F'";
                }
                cmd.CommandText += " ORDER BY dataConsegnaPrevista";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }
                rdr.Close();
                conn.Close();
            }
        }

        /* prcVar: articoli generati da questo processo-varianti
            * stato: status del prodotto
            */
        public ElencoArticoli(String Tenant, ProcessoVariante prcVar, char stato, DateTime start, DateTime end)
        {
            this.Tenant = Tenant;

            this._ListArticoli = new List<Articolo>();
            if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.process.revisione != -1 && prcVar.variant.idVariante != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT id, anno FROM productionplan WHERE "
                    + " processo = " + prcVar.process.processID.ToString()
                    + " AND revisione = " + prcVar.process.revisione.ToString()
                    + " AND variante = " + prcVar.variant.idVariante.ToString()
                    + " AND status = '" + stato + "'";
                if (start != null && end != null && start < end)
                {
                    cmd.CommandText += " AND productionplan.dataPrevistaFineProduzione >= '" + start.ToString("yyyy/MM/dd") + "' "
                        + " AND productionplan.dataPrevistaFineProduzione <= '" + end.ToString("yyyy/MM/dd") + "'";
                }    

                cmd.CommandText+= " ORDER BY dataConsegnaPrevista";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
                }

                rdr.Close();
                conn.Close();
            }
        }

        public ElencoArticoli(String Tenant, Articolo art)
        {
            this.Tenant = Tenant;

            this._ListArticoli = new List<Articolo>();
            if (art.ID != -1 && art.Year != -1)
            {
                this._ListArticoli.Add(art);
            }
        }

        /* Carica i prodotti di un determinato cliente
         */
        public ElencoArticoli(String Tenant, Cliente customer)
        {
            this.Tenant = Tenant;

            this._ListArticoli = new List<Articolo>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT productionplan.id, productionplan.anno FROM "
                + " productionplan INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa = commesse.anno) "
                + " WHERE ";
            String condCustomer="";
            if (customer != null && customer.CodiceCliente.Length > 0)
            {
                condCustomer = " commesse.cliente = '" + customer.CodiceCliente + "'";
            }
            
            if (condCustomer.Length > 0)
            {
                cmd.CommandText += condCustomer;
            }
            
            cmd.CommandText += " ORDER BY productionplan.anno DESC, productionplan.id DESC";

            log = cmd.CommandText;

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public ElencoArticoli(String Tenant, ProcessoVariante origProc, Cliente customer, DateTime start, DateTime end)
        {
            this.Tenant = Tenant;

            this._ListArticoli = new List<Articolo>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT productionplan.id, productionplan.anno FROM "
                + " productionplan INNER JOIN commesse ON (productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa = commesse.anno) "
                + " WHERE productionplan.status = 'F' AND ";
            String condOrigProc = "", condCustomer = "", condTime = "";
            if (origProc != null && origProc.process != null && origProc.variant != null && origProc.process.processID != -1 && origProc.variant.idVariante != -1)
            {
                condOrigProc = " productionplan.processo = " + origProc.process.processID.ToString()
                    + " AND productionplan.revisione = " + origProc.process.revisione.ToString()
                    + " AND productionplan.variante = " + origProc.variant.idVariante.ToString();
            }

            if (customer!=null && customer.CodiceCliente.Length > 0)
            {
                condCustomer = " commesse.cliente = '" + customer.CodiceCliente + "'";
            }

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
                this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public ElencoArticoli(String Tenant, char stato)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, anno FROM productionplan WHERE status = '" + stato + "'ORDER BY DataPrevistaFineProduzione";
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._ListArticoli = new List<Articolo>();
            while (rdr.Read())
            {
                this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public ElencoArticoli(String Tenant, char stato, Reparto rep)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, anno FROM productionplan WHERE status = '" + stato + "' AND reparto = " + rep.id.ToString() + " ORDER BY DataPrevistaFineProduzione";
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._ListArticoli = new List<Articolo>();
            while (rdr.Read())
            {
                this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public ElencoArticoli(String Tenant, char stato, DateTime start, DateTime finish)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT max(lateFinish) as dataFineProd, idArticolo, annoArticolo FROM "
            + " tasksproduzione INNER JOIN productionplan ON (idArticolo = productionplan.id AND annoarticolo = productionplan.anno) "
            + " WHERE productionplan.status = '" + stato + "'"
            + " AND lateFinish >= '"+start.ToString("yyyy-MM-dd") +"'"
            + " AND lateFinish <= '"+finish.ToString("yyyy-MM-dd")+"'"
            + " GROUP BY idArticolo, annoArticolo"
            + " ORDER BY datafineprod DESC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._ListArticoli = new List<Articolo>();
            while (rdr.Read())
            {
                this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(1), rdr.GetInt32(2)));
            }
            rdr.Close();
            conn.Close();
        }

        public ElencoArticoli(String Tenant, DateTime start, DateTime finish)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select productionplan.id, productionplan.anno from productionplan "
                + "inner join commesse on "
                + "(productionplan.commessa = commesse.idcommesse AND productionplan.annoCommessa = commesse.anno)"
                + " WHERE commesse.dataInserimento >= '" + start.ToString("yyyy-MM-dd") + "'"
                +" AND commesse.dataInserimento <= '" + finish.ToString("yyyy-MM-dd") + "'"
                + " ORDER BY commesse.dataInserimento";
            MySqlDataReader rdr = cmd.ExecuteReader();
            this._ListArticoli = new List<Articolo>();
            while (rdr.Read())
            {
                this._ListArticoli.Add(new Articolo(this.Tenant, rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            conn.Close();
        }

        public TimeSpan TempoDiLavoroMedio
        {
            get
            {
                TimeSpan sum = new TimeSpan(0, 0, 0);
                int qta = 0;
                foreach (Articolo art in ListArticoli)
                {
                    //art.loadTempoDiLavoroTotale();
                    sum = sum.Add(art.TempoDiLavoroTotale);
                    qta += art.QuantitaProdotta;
                }

                long media = 0;
                if (qta > 0)
                {
                    media = sum.Ticks / qta;
                }
                TimeSpan Mean = new TimeSpan(media);
                return Mean;
            }
        }

        public TimeSpan LeadTimeMedio
        {
            get
            {
                TimeSpan sum = new TimeSpan(0, 0, 0);
                foreach (Articolo art in ListArticoli)
                {
                    sum = sum.Add(art.LeadTime);
                }

                long media = 0;
                if (this.ListArticoli.Count > 0)
                {
                    media = sum.Ticks / this.ListArticoli.Count;
                }
                TimeSpan Mean = new TimeSpan(media);
                return Mean;
            }
        }
        
    }

    public struct FlatProduct
    {
        public int ProductID;
        public int ProductYear;
        public String processName;
        public String varianteName;
        public String SerialNumber;
        public int Quantity;
        public String CustomerID;
        public String CustomerName;
        public int processID;
        public int revID;
        public int varID;
        public char Status;
    }

    public class ProductsList
    {
        public String Tenant;

        public List<FlatProduct> ProductList;

        public ProductsList()
        {
            this.ProductList = new List<FlatProduct>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT productionplan.id, productionplan.anno, processo.name, varianti.nomevariante,"
                                + "productionplan.matricola, productionplan.quantita, commesse.cliente, "
                                + "anagraficaclienti.ragsociale"
                                + " FROM productionplan INNER JOIN variantiprocessi "
                                + "ON(productionplan.processo = variantiprocessi.processo AND productionplan.revisione = variantiprocessi.revproc"
                                + "AND productionplan.variante = variantiprocessi.variante) INNER JOIN processo "
                                + "ON(processo.processID = variantiprocessi.processo AND processo.revisione = variantiprocessi.revProc)"
                                + " INNER JOIN varianti ON(variantiprocessi.variante = varianti.idvariante) INNER JOIN commesse "
                                + "ON(productionplan.commessa = commesse.idcommesse AND productionplan.annocommessa = commesse.anno)"
                                + "INNER JOIN anagraficaclienti ON(commesse.cliente = anagraficaclienti.codice)"
                                + "ORDER BY productionplan.anno, productionplan.id";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                FlatProduct curr = new FlatProduct();
                curr.ProductID = rdr.GetInt32(0);
                curr.ProductYear = rdr.GetInt32(1);
                curr.processName = rdr.GetString(2);
                curr.varianteName = rdr.GetString(3);
                curr.SerialNumber = rdr.GetString(4);
                curr.Quantity = rdr.GetInt32(5);
                curr.CustomerID = rdr.GetString(6);
                curr.CustomerName = rdr.GetString(7);

                this.ProductList.Add(curr);
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class ProductParameter
    {
        public String Tenant;

        public String log;

        private int _ProductID;
        public int ProductID
        {
            get
            {
                return this._ProductID;
            }
        }

        private int _ProductYear;
        public int ProductYear
        {
            get
            {
                return this._ProductYear;
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
                if (this.ProductID != -1 && this.ProductYear != -1 && value != null && value.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE productparameters SET paramCategory = " + value.ID + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND productID = " + this.ProductID.ToString()
                        + " AND productYear = " + this.ProductYear.ToString();
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
                if (this.ProductID != -1 && this.ProductYear>2000)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE productparameters SET paramName = '" + value + "' WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND productID = " + this.ProductID.ToString()
                        + " AND productYear = " + this.ProductYear.ToString();
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
                if (this.ProductID != -1 && this.ProductYear != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelparameters SET paramDescription = '" + value + "' WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND ProductID = " + this.ProductID.ToString()
                        + " AND ProductYear = " + this.ProductYear.ToString();
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
                if (this.ProductID != -1 && this.ProductYear != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelparameters SET isFixed = " + value + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND ProductID = " + this.ProductID.ToString()
                        + " AND ProductYear = " + this.ProductYear.ToString();
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
                if (this.ProductID != -1 && this.ProductYear != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE modelparameters SET isRequired = " + value + " WHERE "
                        + " paramID = " + this.ParameterID.ToString()
                        + " AND ProductID = " + this.ProductID.ToString()
                        + " AND ProductYear = " + this.ProductYear.ToString();
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

        public ProductParameter(int productID, int productYear, ProductParametersCategory category, int parameterID)
        {
            this._ParameterID = -1;
            this._ProductID = -1;
            this._ProductYear = -1;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT productID, productYear, paramID, paramCategory, paramName, "
                + " paramDescription, isFixed, sequence FROM productparameters WHERE "
                + " productID = " + productID.ToString()
                + " AND productYear = " + productYear.ToString()
                + " AND paramID = " + parameterID.ToString()
                + " AND paramCategory = " + category.ID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(3))
            {
                this._ProductID = rdr.GetInt32(0);
                this._ProductYear = rdr.GetInt32(1);
                this._ParameterID = rdr.GetInt32(2);
                this.ParameterCategory = new ProductParametersCategory(this.Tenant, rdr.GetInt32(3));
                if (!rdr.IsDBNull(4))
                {
                    this._Name = rdr.GetString(4);
                }
                if (!rdr.IsDBNull(5))
                {
                    this._Description = rdr.GetString(5);
                }
                if (!rdr.IsDBNull(6))
                {
                    this._isFixed = rdr.GetBoolean(6);
                }
                if (!rdr.IsDBNull(7))
                {
                    this._Sequence = rdr.GetInt32(7);
                }
            }
            rdr.Close();
            conn.Close();
        }

        public Boolean Delete()
        {
            Boolean ret = false;
            if(this.ParameterID != -1 && this.ProductID != -1 && this.ProductYear > 2010)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM productparameters WHERE productID = " + this.ProductID.ToString()
                    + " AND productYear = " + this.ProductYear.ToString()
                    + " AND paramID = " + this.ParameterID.ToString();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch(Exception ex)
                {
                    this.log = ex.Message;
                    ret = false;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }
    }
}