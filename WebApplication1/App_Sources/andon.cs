using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient; 
//using KIS.Commesse;

namespace KIS.App_Code
{
    public class AndonCompleto
    {
        public String log;

        public AndonCompleto()
        {
            this._FieldList = new Dictionary<string, int>();
            this._FieldList.Add("Commessa ID", 0);
            this._FieldList.Add("Commessa Codice Cliente", 0);
            this._FieldList.Add("Commessa RagioneSociale Cliente", 0);
            this._FieldList.Add("Commessa Data Inserimento", 0);
            this._FieldList.Add("Commessa Note", 0);
            this._FieldList.Add("Prodotto ID", 0);
            this._FieldList.Add("Prodotto LineaProdotto", 0);
            this._FieldList.Add("Prodotto NomeProdotto", 0);
            this._FieldList.Add("Prodotto Matricola", 0);
            this._FieldList.Add("Prodotto Status", 0);
            this._FieldList.Add("Reparto", 0);
            this._FieldList.Add("Data Prevista Consegna", 0);
            this._FieldList.Add("Data Prevista Fine Produzione", 0);
            this._FieldList.Add("EarlyStart", 0);
            this._FieldList.Add("LateStart", 0);
            this._FieldList.Add("EarlyFinish", 0);
            this._FieldList.Add("LateFinish", 0);
            this._FieldList.Add("Prodotto Quantita", 0);
            this._FieldList.Add("Prodotto Quantita Realizzata", 0);
            this._FieldList.Add("Prodotto Ritardo", 0);
            this._FieldList.Add("Prodotto Tempo di Lavoro Totale", 0);
            this._FieldList.Add("Prodotto Indicatore Completamento Tasks", 0);
            this._FieldList.Add("Prodotto Indicatore Completamento Tempo Previsto", 0);

            this._FieldListTasks = new Dictionary<string, int>();
            this._FieldListTasks.Add("Task ID", 0);
            this._FieldListTasks.Add("Task Nome", 0);
            this._FieldListTasks.Add("Task Descrizione", 0);
            this._FieldListTasks.Add("Task Postazione", 0);
            this._FieldListTasks.Add("Task EarlyStart", 0);
            this._FieldListTasks.Add("Task LateStart", 0);
            this._FieldListTasks.Add("Task EarlyFinish", 0);
            this._FieldListTasks.Add("Task LateFinish", 0);
            this._FieldListTasks.Add("Task NumeroOperatori", 0);
            this._FieldListTasks.Add("Task TempoCiclo", 0);
            this._FieldListTasks.Add("Task TempoDiLavoroPrevisto", 0);
            this._FieldListTasks.Add("Task TempoDiLavoroEffettivo", 0);
            this._FieldListTasks.Add("Task Status", 0);
            this._FieldListTasks.Add("Task QuantitaPrevista", 0);
            this._FieldListTasks.Add("Task QuantitaProdotta", 0);
            this._FieldListTasks.Add("Task Ritardo", 0);
            this._FieldListTasks.Add("Task InizioEffettivo", 0);
            this._FieldListTasks.Add("Task FineEffettiva", 0);
        }

        /* Configurazione visualizzazione nomi utente su Andon:
         * 0 --> vedo username
         * 1 --> vedo il nome
         * 2 --> nome e iniziale del cognome
         * 3 --> nome e cognome
         */
        public char PostazioniFormatoUsername
        {
            get
            {
                char ret = '0';
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Andon Completo' "
                    + "AND ID = -1 AND parametro LIKE 'FormatoUsername'";
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
                    // Verifico che sia presente la configurazione
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Andon Completo' "
                        + "AND ID = -1 AND parametro LIKE 'FormatoUsername'";
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
                        cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Andon Completo', "
                            + "-1, 'FormatoUsername', '" + value.ToString() + "')";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE configurazione SET valore = '" + value + "' WHERE "
                        + " Sezione = 'Andon Completo' AND ID = -1" +
                        " AND parametro LIKE 'FormatoUsername'";
                    }

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

                    rdr.Close();
            }
        }

        /* String è il nome del campo
         * int è l'ordinamento
         */
        protected Dictionary<String, int> _FieldList;
        public Dictionary<String, int> FieldList
        {
            get
            {
                return this._FieldList;
            }
        }

        /* String è il nome del campo
         * int è l'ordinamento
         */
        protected Dictionary<String, int> _CampiVisualizzati;
        public Dictionary<String, int> CampiVisualizzati
        {
            get
            {
                return this._CampiVisualizzati;
            }
        }

        public virtual void loadCampiVisualizzati()
        {
            this._CampiVisualizzati = new Dictionary<String, int>();
            Dictionary<String, int> swap = new Dictionary<string, int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE Sezione LIKE 'Andon ViewFields' "
                + " AND ID = -1 "
                + " ORDER BY valore";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read() && !rdr.IsDBNull(0))
            {
                String field = rdr.GetString(0);
                String sOrdine = rdr.GetString(1);
                int ord = -1;
                try
                {
                    ord = Int32.Parse(sOrdine);
                }
                catch(Exception ex)
                {
                    ord = 0;
                    log = ex.Message;
                }
                swap.Add(field, ord);
            }

            this._CampiVisualizzati = swap.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            rdr.Close();
            conn.Close();
        }

        public virtual Boolean addCampoVisualizzato(String field)
        {
            Boolean ret = false;
            this.loadCampiVisualizzati();
            int prog = this.CampiVisualizzati.Count;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                + "'Andon ViewFields',"
                + "-1, "
                + "'" + field + "', "
                + "'" + prog.ToString() + "'"
                +")";

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public virtual Boolean deleteCampoVisualizzato(String field)
        {
            Boolean ret = false;
            this.loadCampiVisualizzati();
            int prog = this.CampiVisualizzati.Count;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM configurazione WHERE "
            + "Sezione='Andon ViewFields'"
            + " AND ID=-1"
            + " AND parametro = '"+field+"'";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message;
                tr.Rollback();
            }
            if (ret == true)
            {
                this.loadCampiVisualizzati();
                int i = 0; 
                foreach (KeyValuePair<string, int> pair in this.CampiVisualizzati)
	            {
                    this.setOrdineCampoVisualizzato(pair.Key, i);
                    i++;
                }
            }
            conn.Close();
            return ret;
        }

        public virtual Boolean setOrdineCampoVisualizzato(String field, int ordine)
        {
            Boolean ret = false;
            this.loadCampiVisualizzati();
            int prog = this.CampiVisualizzati.Count;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "UPDATE configurazione SET valore = " + ordine.ToString()
                + " WHERE Sezione='Andon ViewFields' AND ID=-1 AND parametro='"+field+"'";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        /* String è il nome del campo
      * int è l'ordinamento
      */
        protected Dictionary<String, int> _FieldListTasks;
        public Dictionary<String, int> FieldListTasks
        {
            get
            {
                return this._FieldListTasks;
            }
        }

        /* String è il nome del campo
         * int è l'ordinamento
         */
        protected Dictionary<String, int> _CampiVisualizzatiTasks;
        public Dictionary<String, int> CampiVisualizzatiTasks
        {
            get
            {
                return this._CampiVisualizzatiTasks;
            }
        }

        public virtual void loadCampiVisualizzatiTasks()
        {
            this._CampiVisualizzatiTasks = new Dictionary<String, int>();
            Dictionary<String, int> swap = new Dictionary<string, int>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE Sezione LIKE 'Andon ViewFieldsTasks' "
                + " AND ID = -1 "
                + " ORDER BY valore";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read() && !rdr.IsDBNull(0))
            {
                String field = rdr.GetString(0);
                String sOrdine = rdr.GetString(1);
                int ord = -1;
                try
                {
                    ord = Int32.Parse(sOrdine);
                }
                catch (Exception ex)
                {
                    ord = 0;
                    log = ex.Message;
                }
                swap.Add(field, ord);
            }

            this._CampiVisualizzatiTasks = swap.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            rdr.Close();
            conn.Close();
        }

        public virtual Boolean addCampoVisualizzatoTasks(String field)
        {
            Boolean ret = false;
            this.loadCampiVisualizzatiTasks();
            int prog = this.CampiVisualizzatiTasks.Count;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                + "'Andon ViewFieldsTasks',"
                + "-1, "
                + "'" + field + "', "
                + "'" + prog.ToString() + "'"
                + ")";

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }

        public virtual Boolean deleteCampoVisualizzatoTasks(String field)
        {
            Boolean ret = false;
            this.loadCampiVisualizzatiTasks();
            int prog = this.CampiVisualizzatiTasks.Count;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM configurazione WHERE "
            + "Sezione='Andon ViewFieldsTasks'"
            + " AND ID=-1"
            + " AND parametro = '" + field + "'";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message;
                tr.Rollback();
            }
            if (ret == true)
            {
                this.loadCampiVisualizzatiTasks();
                int i = 0;
                foreach (KeyValuePair<string, int> pair in this.CampiVisualizzatiTasks)
                {
                    this.setOrdineCampoVisualizzatoTasks(pair.Key, i);
                    i++;
                }
            }
            conn.Close();
            return ret;
        }

        public virtual Boolean setOrdineCampoVisualizzatoTasks(String field, int ordine)
        {
            Boolean ret = false;
            this.loadCampiVisualizzatiTasks();
            int prog = this.CampiVisualizzatiTasks.Count;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "UPDATE configurazione SET valore = " + ordine.ToString()
                + " WHERE Sezione='Andon ViewFieldsTasks' AND ID=-1 AND parametro='" + field + "'";
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                log = ex.Message;
                tr.Rollback();
            }
            conn.Close();
            return ret;
        }
    }

    public class AndonReparto : AndonCompleto
    {
        //public String log;
        private int _RepartoID;
        public int RepartoID
        {
            get
            {
                return this._RepartoID;
            }
        }

        private List<Articolo> _WIP;

        public AndonReparto(int idRep):base()
        {
            Reparto rp = new Reparto(idRep);
            this._RepartoID = idRep;
            this._WIP = new List<Articolo>();
            if (rp.id != -1)
            {
                this._RepartoID = rp.id;
                ElencoArticoliAperti elOpenArt = new ElencoArticoliAperti(rp.id);
                for (int j = 0; j < elOpenArt.ArticoliAperti.Count; j++)
                {
                    this._WIP.Add(elOpenArt.ArticoliAperti[j]);
                }
            }
            else
            {
                this._RepartoID = -1;
            }

            // Campi Andon Reparto
            this._FieldList = new Dictionary<string, int>();
            this._FieldList.Add("Commessa ID", 0);
            this._FieldList.Add("Commessa Codice Cliente", 0);
            this._FieldList.Add("Commessa RagioneSociale Cliente", 0);
            this._FieldList.Add("Commessa Data Inserimento", 0);
            this._FieldList.Add("Commessa Note", 0);
            this._FieldList.Add("Prodotto ID", 0);
            this._FieldList.Add("Prodotto LineaProdotto", 0);
            this._FieldList.Add("Prodotto NomeProdotto", 0);
            this._FieldList.Add("Prodotto Matricola", 0);
            this._FieldList.Add("Prodotto Status", 0);
            this._FieldList.Add("Reparto", 0);
            this._FieldList.Add("Data Prevista Consegna", 0);
            this._FieldList.Add("Data Prevista Fine Produzione", 0);
            this._FieldList.Add("EarlyStart", 0);
            this._FieldList.Add("LateStart", 0);
            this._FieldList.Add("EarlyFinish", 0);
            this._FieldList.Add("LateFinish", 0);
            this._FieldList.Add("Prodotto Quantita", 0);
            this._FieldList.Add("Prodotto Quantita Realizzata", 0);
            this._FieldList.Add("Prodotto Ritardo", 0);
            this._FieldList.Add("Prodotto Tempo di Lavoro Totale", 0);
            this._FieldList.Add("Prodotto Indicatore Completamento Tasks", 0);
            this._FieldList.Add("Prodotto Indicatore Completamento Tempo Previsto", 0);

            this._FieldListTasks = new Dictionary<string, int>();
            this._FieldListTasks.Add("Task ID", 0);
            this._FieldListTasks.Add("Task Nome", 0);
            this._FieldListTasks.Add("Task Descrizione", 0);
            this._FieldListTasks.Add("Task Postazione", 0);
            this._FieldListTasks.Add("Task EarlyStart", 0);
            this._FieldListTasks.Add("Task LateStart", 0);
            this._FieldListTasks.Add("Task EarlyFinish", 0);
            this._FieldListTasks.Add("Task LateFinish", 0);
            this._FieldListTasks.Add("Task NumeroOperatori", 0);
            this._FieldListTasks.Add("Task TempoCiclo", 0);
            this._FieldListTasks.Add("Task TempoDiLavoroPrevisto", 0);
            this._FieldListTasks.Add("Task TempoDiLavoroEffettivo", 0);
            this._FieldListTasks.Add("Task Status", 0);
            this._FieldListTasks.Add("Task QuantitaPrevista", 0);
            this._FieldListTasks.Add("Task QuantitaProdotta", 0);
            this._FieldListTasks.Add("Task Ritardo", 0);
            this._FieldListTasks.Add("Task InizioEffettivo", 0);
            this._FieldListTasks.Add("Task FineEffettiva", 0);
        }

        public List<TaskProduzione> ElencoTasks
        {
            get
            {
                List<TaskProduzione> elenco = new List<TaskProduzione>();
                DateTime maxDate = DateTime.UtcNow.AddDays(this.MaxViewDays);
                for (int i = 0; i < this._WIP.Count; i++)
                {
                    this._WIP[i].loadTasksProduzione();
                    for (int j = 0; j < this._WIP[i].Tasks.Count; j++)
                    {
                        if (this._WIP[i].Tasks[j].Status != 'F' && maxDate > this._WIP[i].Tasks[j].LateStart)
                        {
                            elenco.Add(this._WIP[i].Tasks[j]);
                        }
                    }
                }
                return elenco;
            }
            
        }

        public int MaxViewDays
        {
            get
            {
                int ret = 1;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' "
                    + " AND ID = " + this.RepartoID.ToString()
                    + " AND parametro LIKE 'MaxViewDays'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    String strDays = rdr.GetString(0);
                    try
                    {
                        ret = Int32.Parse(strDays);
                    }
                    catch
                    {
                        ret = 1;
                    }
                }
                else
                {
                    ret = 1;
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
            set
            {
                if (this.RepartoID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' "
                        + " AND ID = " + this.RepartoID.ToString()
                        + " AND parametro LIKE 'MaxViewDays'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    bool found = false;
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                    }
                    rdr.Close();

                    if (found == true)
                    {
                        cmd.CommandText = "UPDATE configurazione SET valore = '"+value.ToString()
                            +"' WHERE Sezione LIKE 'Andon Reparto' "
                        + " AND ID = " + this.RepartoID.ToString()
                        + " AND parametro LIKE 'MaxViewDays'";
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('"
                        + "Andon Reparto', " 
                        + this.RepartoID.ToString()
                        + ", 'MaxViewDays', '" + value.ToString() + "')";
                    }

                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {
                        tr.Rollback();
                        log = ex.Message;
                    }

                    conn.Close();
                }
            }
        }

        public override void loadCampiVisualizzati()
        {
            this._CampiVisualizzati = new Dictionary<String, int>();
            if (this.RepartoID != -1)
            {
                Dictionary<String, int> swap = new Dictionary<string, int>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE Sezione LIKE 'Andon ViewFields' "
                    + " AND ID = " + this.RepartoID.ToString()
                    + " ORDER BY valore";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    String field = rdr.GetString(0);
                    String sOrdine = rdr.GetString(1);
                    int ord = -1;
                    try
                    {
                        ord = Int32.Parse(sOrdine);
                    }
                    catch (Exception ex)
                    {
                        ord = 0;
                        log = ex.Message;
                    }
                    swap.Add(field, ord);
                }

                this._CampiVisualizzati = swap.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                rdr.Close();
                conn.Close();
            }
        }

        public override Boolean addCampoVisualizzato(String field)
        {
            Boolean ret = false;
            if (this.RepartoID != -1)
            {
                this.loadCampiVisualizzati();
                int prog = this.CampiVisualizzati.Count;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                    + "'Andon ViewFields',"
                    + this.RepartoID.ToString() + ", "
                    + "'" + field + "', "
                    + "'" + prog.ToString() + "'"
                    + ")";

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public override Boolean deleteCampoVisualizzato(String field)
        {
            Boolean ret = false;
            if (this.RepartoID != -1)
            {
                this.loadCampiVisualizzati();
                int prog = this.CampiVisualizzati.Count;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM configurazione WHERE "
                + "Sezione='Andon ViewFields'"
                + " AND ID=" + this.RepartoID.ToString()
                + " AND parametro = '" + field + "'";
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                    tr.Rollback();
                }
                if (ret == true)
                {
                    this.loadCampiVisualizzati();
                    int i = 0;
                    foreach (KeyValuePair<string, int> pair in this.CampiVisualizzati)
                    {
                        this.setOrdineCampoVisualizzato(pair.Key, i);
                        i++;
                    }
                }
                conn.Close();
            }
            return ret;
        }

        public override Boolean setOrdineCampoVisualizzato(String field, int ordine)
        {
            Boolean ret = false;
            if (this.RepartoID != -1)
            {
                this.loadCampiVisualizzati();
                int prog = this.CampiVisualizzati.Count;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE configurazione SET valore = " + ordine.ToString()
                    + " WHERE Sezione='Andon ViewFields' AND ID=" + this.RepartoID.ToString()
                    + " AND parametro='" + field + "'";
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public override void loadCampiVisualizzatiTasks()
        {
            this._CampiVisualizzatiTasks = new Dictionary<String, int>();
            if (this.RepartoID != -1)
            {
                Dictionary<String, int> swap = new Dictionary<string, int>();
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE Sezione LIKE 'Andon ViewFieldsTasks' "
                    + " AND ID = " + this.RepartoID.ToString()
                    + " ORDER BY valore";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    String field = rdr.GetString(0);
                    String sOrdine = rdr.GetString(1);
                    int ord = -1;
                    try
                    {
                        ord = Int32.Parse(sOrdine);
                    }
                    catch (Exception ex)
                    {
                        ord = 0;
                        log = ex.Message;
                    }
                    swap.Add(field, ord);
                }

                this._CampiVisualizzatiTasks = swap.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                rdr.Close();
                conn.Close();
            }
        }

        public override Boolean addCampoVisualizzatoTasks(String field)
        {
            Boolean ret = false;
            if (this.RepartoID != -1)
            {
                this.loadCampiVisualizzatiTasks();
                int prog = this.CampiVisualizzatiTasks.Count;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                    + "'Andon ViewFieldsTasks',"
                    + this.RepartoID.ToString() + ", "
                    + "'" + field + "', "
                    + "'" + prog.ToString() + "'"
                    + ")";

                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }

        public override Boolean deleteCampoVisualizzatoTasks(String field)
        {
            Boolean ret = false;
            if (this.RepartoID != -1)
            {
                this.loadCampiVisualizzatiTasks();
                int prog = this.CampiVisualizzatiTasks.Count;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "DELETE FROM configurazione WHERE "
                + "Sezione='Andon ViewFieldsTasks'"
                + " AND ID=" + this.RepartoID.ToString()
                + " AND parametro = '" + field + "'";
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                    tr.Rollback();
                }
                if (ret == true)
                {
                    this.loadCampiVisualizzatiTasks();
                    int i = 0;
                    foreach (KeyValuePair<string, int> pair in this.CampiVisualizzatiTasks)
                    {
                        this.setOrdineCampoVisualizzatoTasks(pair.Key, i);
                        i++;
                    }
                }
                conn.Close();
            }
            return ret;
        }

        public override Boolean setOrdineCampoVisualizzatoTasks(String field, int ordine)
        {
            Boolean ret = false;
            if (this.RepartoID != -1)
            {
                this.loadCampiVisualizzatiTasks();
                int prog = this.CampiVisualizzatiTasks.Count;

                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlTransaction tr = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = tr;
                cmd.CommandText = "UPDATE configurazione SET valore = " + ordine.ToString()
                    + " WHERE Sezione='Andon ViewFieldsTasks' AND ID=" + this.RepartoID.ToString() + " AND parametro='" + field + "'";
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
            return ret;
        }
    }

}