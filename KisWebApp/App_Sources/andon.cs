/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

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
            this._FieldList.Add("CommessaID", 0);
            this._FieldList.Add("OrderExternalID", 0);
            this._FieldList.Add("CommessaCodiceCliente", 0);
            this._FieldList.Add("CommessaRagioneSocialeCliente", 0);
            this._FieldList.Add("CommessaDataInserimento", 0);
            this._FieldList.Add("CommessaNote", 0);
            this._FieldList.Add("ProductExternalID", 0);
            this._FieldList.Add("ProdottoID", 0);
            this._FieldList.Add("ProdottoLineaProdotto", 0);
            this._FieldList.Add("ProdottoNomeProdotto", 0);
            this._FieldList.Add("ProdottoMatricola", 0);
            this._FieldList.Add("ProdottoStatus", 0);
            this._FieldList.Add("Reparto", 0);
            this._FieldList.Add("DataPrevistaConsegna", 0);
            this._FieldList.Add("DataPrevistaFineProduzione", 0);
            this._FieldList.Add("EarlyStart", 0);
            this._FieldList.Add("LateStart", 0);
            this._FieldList.Add("EarlyFinish", 0);
            this._FieldList.Add("LateFinish", 0);
            this._FieldList.Add("ProdottoQuantita", 0);
            this._FieldList.Add("ProdottoQuantitaRealizzata", 0);
            this._FieldList.Add("MeasurementUnit", 0);
            this._FieldList.Add("ProdottoRitardo", 0);
            this._FieldList.Add("ProdottoTempodiLavoroTotale", 0);
            this._FieldList.Add("ProdottoIndicatoreCompletamentoTasks", 0);
            this._FieldList.Add("ProdottoIndicatoreCompletamentoTempoPrevisto", 0);

            this._FieldListTasks = new Dictionary<string, int>();
            this._FieldListTasks.Add("TaskID", 0);
            this._FieldListTasks.Add("TaskNome", 0);
            this._FieldListTasks.Add("TaskDescrizione", 0);
            this._FieldListTasks.Add("TaskPostazione", 0);
            this._FieldListTasks.Add("TaskEarlyStart", 0);
            this._FieldListTasks.Add("TaskLateStart", 0);
            this._FieldListTasks.Add("TaskEarlyFinish", 0);
            this._FieldListTasks.Add("TaskLateFinish", 0);
            this._FieldListTasks.Add("TaskNumeroOperatori", 0);
            this._FieldListTasks.Add("TaskTempoCiclo", 0);
            this._FieldListTasks.Add("TaskTempoDiLavoroPrevisto", 0);
            this._FieldListTasks.Add("TaskTempoDiLavoroEffettivo", 0);
            this._FieldListTasks.Add("TaskStatus", 0);
            this._FieldListTasks.Add("TaskQuantitaPrevista", 0);
            this._FieldListTasks.Add("TaskQuantitaProdotta", 0);
            this._FieldListTasks.Add("TaskRitardo", 0);
            this._FieldListTasks.Add("TaskInizioEffettivo", 0);
            this._FieldListTasks.Add("TaskFineEffettiva", 0);
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

        /*
         * 0 if Scroll not enabled
         * 1 if Continuous Scroll
         */
        public int ScrollType;
        public double ContinuousScrollGoSpeed;
        public double ContinuousScrollBackSpeed;
            
        /* String format in database: ScrollType;Param1;Param2
         if ContinuousScroll: ScrollType = 1, Param1 = Speed1 (from top to bottom), Param2 = Speed2 (from bottom to top)
             */
        public void loadScrollType()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Completo' AND parametro LIKE 'ScrollType'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this.log = rdr.GetString(0);
                String val = rdr.GetString(0);
                if(val[0] == '0')
                {
                    this.ScrollType = 0;
                    this.ContinuousScrollGoSpeed = 0.0;
                    this.ContinuousScrollBackSpeed = 0.0;
                }
                else if(val[0] == '1')
                {
                    var valArr = val.Split(';');
                    try
                    {                    
                    this.ScrollType = 1;
                    this.ContinuousScrollGoSpeed = Double.Parse(valArr[1]);
                    this.ContinuousScrollBackSpeed = Double.Parse(valArr[2]);
                    }
                    catch
                    {
                        this.ScrollType = 0;
                        this.ContinuousScrollGoSpeed = 0.0;
                        this.ContinuousScrollBackSpeed = 0.0;
                    }
                }
            }
            else
            {
                this.ScrollType = 0;
                this.ContinuousScrollGoSpeed = 0.0;
                this.ContinuousScrollBackSpeed = 0.0;
            }
            rdr.Close();
            conn.Clone();
        }

        /* 0 if generic error
         * 1 if all is ok
         */
        public int setScrollType(int ScrollType, String Params)
        {
            int ret = 0;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Completo' AND parametro LIKE 'ScrollType'";
            bool CreateOrUpdate = false;
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                CreateOrUpdate = true;
            }
            else
            {
                CreateOrUpdate = false;
            }
            rdr.Close();

            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            String val = "0";
            if(ScrollType >0)
            { 
                val = ScrollType.ToString() + ";" + Params;
            }
            if (CreateOrUpdate)
            {
                // Update
                cmd.CommandText = "UPDATE configurazione SET valore ='" + val + "' WHERE Sezione = 'Andon Completo' AND parametro = 'ScrollType'";
            }
            else
            {
                // Create
                cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Andon Completo', -1, 'ScrollType', '" + val + "')";
            }

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = 1;
            }
            catch
            {
                tr.Rollback();
                ret = 0;
            }

            conn.Close();
            return ret;
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

        private String _DepartmentName;
        public String DepartmentName { get { return this._DepartmentName; } }

        private List<Articolo> _WIP;

        private TimeZoneInfo _DepartmentTimezone;
        public TimeZoneInfo DepartmentTimezone
        {
            get { return this._DepartmentTimezone; }
        }

        public AndonReparto(int idRep):base()
        {
            Reparto rp = new Reparto(idRep);
            this._RepartoID = idRep;
            this._DepartmentName = rp.name;
            this._WIP = new List<Articolo>();
            this._DepartmentTimezone = rp.tzFusoOrario;


            // Campi Andon Reparto
            this._FieldList = new Dictionary<string, int>();
            this._FieldList.Add("CommessaID", 0);
            this._FieldList.Add("OrderExternalID", 0);
            this._FieldList.Add("CommessaCodiceCliente", 0);
            this._FieldList.Add("CommessaRagioneSocialeCliente", 0);
            this._FieldList.Add("CommessaDataInserimento", 0);
            this._FieldList.Add("CommessaNote", 0);
            this._FieldList.Add("ProductExternalID", 0);
            this._FieldList.Add("ProdottoID", 0);
            this._FieldList.Add("ProdottoLineaProdotto", 0);
            this._FieldList.Add("ProdottoNomeProdotto", 0);
            this._FieldList.Add("ProdottoMatricola", 0);
            this._FieldList.Add("ProdottoStatus", 0);
            this._FieldList.Add("Reparto", 0);
            this._FieldList.Add("DataPrevistaConsegna", 0);
            this._FieldList.Add("DataPrevistaFineProduzione", 0);
            this._FieldList.Add("EarlyStart", 0);
            this._FieldList.Add("LateStart", 0);
            this._FieldList.Add("EarlyFinish", 0);
            this._FieldList.Add("LateFinish", 0);
            this._FieldList.Add("ProdottoQuantita", 0);
            this._FieldList.Add("ProdottoQuantitaRealizzata", 0);
            this._FieldList.Add("MeasurementUnit", 0);
            this._FieldList.Add("ProdottoRitardo", 0);
            this._FieldList.Add("ProdottoTempodiLavoroTotale", 0);
            this._FieldList.Add("ProdottoIndicatoreCompletamentoTasks", 0);
            this._FieldList.Add("ProdottoIndicatoreCompletamentoTempoPrevisto", 0);

            this._FieldListTasks = new Dictionary<string, int>();
            this._FieldListTasks.Add("TaskID", 0);
            this._FieldListTasks.Add("TaskNome", 0);
            this._FieldListTasks.Add("TaskDescrizione", 0);
            this._FieldListTasks.Add("TaskPostazione", 0);
            this._FieldListTasks.Add("TaskEarlyStart", 0);
            this._FieldListTasks.Add("TaskLateStart", 0);
            this._FieldListTasks.Add("TaskEarlyFinish", 0);
            this._FieldListTasks.Add("TaskLateFinish", 0);
            this._FieldListTasks.Add("TaskNumeroOperatori", 0);
            this._FieldListTasks.Add("TaskTempoCiclo", 0);
            this._FieldListTasks.Add("TaskTempoDiLavoroPrevisto", 0);
            this._FieldListTasks.Add("TaskTempoDiLavoroEffettivo", 0);
            this._FieldListTasks.Add("TaskStatus", 0);
            this._FieldListTasks.Add("TaskQuantitaPrevista", 0);
            this._FieldListTasks.Add("TaskQuantitaProdotta", 0);
            this._FieldListTasks.Add("TaskRitardo", 0);
            this._FieldListTasks.Add("TaskInizioEffettivo", 0);
            this._FieldListTasks.Add("TaskFineEffettiva", 0);
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

        /*
         * 0 if Scroll not enabled
         * 1 if Continuous Scroll
         */
        public new int ScrollType;
        public new double ContinuousScrollGoSpeed;
        public new double ContinuousScrollBackSpeed;

        private Boolean _ShowActiveUsers;
        public Boolean ShowActiveUsers
        {
            get { return this._ShowActiveUsers; }
            set
            {
                if (this.RepartoID != -1)
                {
                    Char cValue = value ? '1' : '0';
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' AND parametro LIKE 'ShowActiveUsers' AND ID = " + this.RepartoID;
                    bool CreateOrUpdate = false;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        CreateOrUpdate = true;
                    }
                    else
                    {
                        CreateOrUpdate = false;
                    }
                    rdr.Close();

                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    if (CreateOrUpdate)
                    {
                        // Update
                        cmd.CommandText = "UPDATE configurazione SET valore ='" + cValue + "' WHERE Sezione = 'Andon Reparto' AND parametro = 'ShowActiveUsers' AND ID = " + this.RepartoID.ToString();
                    }
                    else
                    {
                        // Create
                        cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Andon Reparto', " + this.RepartoID + ", 'ShowActiveUsers', '" + cValue + "')";
                    }

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
        }

        private Boolean _ShowProductionIndicator;
        public Boolean ShowProductionIndicator
        {
            get { return this._ShowProductionIndicator; }
            set
            {
                if (this.RepartoID != -1)
                {
                    Char cValue = value ? '1' : '0';
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' AND parametro LIKE 'ShowProductionIndicator' AND ID = " + this.RepartoID;
                    bool CreateOrUpdate = false;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        CreateOrUpdate = true;
                    }
                    else
                    {
                        CreateOrUpdate = false;
                    }
                    rdr.Close();

                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    if (CreateOrUpdate)
                    {
                        // Update
                        cmd.CommandText = "UPDATE configurazione SET valore ='" + cValue + "' WHERE Sezione = 'Andon Reparto' AND parametro = 'ShowProductionIndicator' AND ID = " + this.RepartoID.ToString();
                    }
                    else
                    {
                        // Create
                        cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Andon Reparto', " + this.RepartoID + ", 'ShowProductionIndicator', '" + cValue + "')";
                    }

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
        }

        

        /* String format in database: ScrollType;Param1;Param2
         if ContinuousScroll: ScrollType = 1, Param1 = Speed1 (from top to bottom), Param2 = Speed2 (from bottom to top)
             */
        public new void loadScrollType()
        {
            if(this.RepartoID!=-1)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' AND parametro LIKE 'ScrollType' AND ID = " + this.RepartoID;
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this.log = rdr.GetString(0);
                String val = rdr.GetString(0);
                if (val[0] == '0')
                {
                    this.ScrollType = 0;
                    this.ContinuousScrollGoSpeed = 0.0;
                    this.ContinuousScrollBackSpeed = 0.0;
                }
                else if (val[0] == '1')
                {
                    var valArr = val.Split(';');
                    try
                    {
                        this.ScrollType = 1;
                        this.ContinuousScrollGoSpeed = Double.Parse(valArr[1]);
                        this.ContinuousScrollBackSpeed = Double.Parse(valArr[2]);
                    }
                    catch
                    {
                        this.ScrollType = 0;
                        this.ContinuousScrollGoSpeed = 0.0;
                        this.ContinuousScrollBackSpeed = 0.0;
                    }
                }
            }
            else
            {
                this.ScrollType = 0;
                this.ContinuousScrollGoSpeed = 0.0;
                this.ContinuousScrollBackSpeed = 0.0;
            }
            rdr.Close();
            conn.Close();
            }
        }

        /* 0 if generic error
         * 1 if all is ok
         */
        public new int setScrollType(int ScrollType, String Params)
        {
            int ret = 0;
            if(this.RepartoID!=-1)
            { 
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' AND parametro LIKE 'ScrollType' AND ID = " + this.RepartoID;
            bool CreateOrUpdate = false;
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                CreateOrUpdate = true;
            }
            else
            {
                CreateOrUpdate = false;
            }
            rdr.Close();

            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            String val = "0";
            if (ScrollType > 0)
            {
                val = ScrollType.ToString() + ";" + Params;
            }
            if (CreateOrUpdate)
            {
                // Update
                cmd.CommandText = "UPDATE configurazione SET valore ='" + val + "' WHERE Sezione = 'Andon Reparto' AND parametro = 'ScrollType' AND ID = " + this.RepartoID.ToString();
            }
            else
            {
                // Create
                cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Andon Reparto', " + this.RepartoID+", 'ScrollType', '" + val + "')";
            }

            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                ret = 1;
            }
            catch
            {
                tr.Rollback();
                ret = 0;
            }

            conn.Close();
            }
            return ret;
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

        public void loadShowActiveUsers()
        {
            this._ShowActiveUsers = false;
            if (this.RepartoID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' AND parametro LIKE 'ShowActiveUsers' AND ID = " + this.RepartoID;
                MySqlDataReader rdr = cmd.ExecuteReader();
                
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    String sVal = rdr.GetString(0);
                    if (sVal == "1")
                    {
                        this._ShowActiveUsers = true;
                    }
                    else
                    {
                        this._ShowActiveUsers = false;
                    }
                }

                else
                {
                    this._ShowActiveUsers = false;
                }
                rdr.Close();
                conn.Close();
            }
        }

        public void loadShowProductionIndicator()
        {
            this._ShowActiveUsers = false;
            if (this.RepartoID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Andon Reparto' AND parametro LIKE 'ShowProductionIndicator' AND ID = " + this.RepartoID;
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    String sVal = rdr.GetString(0);
                    if (sVal == "1")
                    {
                        this._ShowProductionIndicator = true;
                    }
                    else
                    {
                        this._ShowProductionIndicator = false;
                    }
                }

                else
                {
                    this._ShowProductionIndicator = false;
                }
                rdr.Close();
                conn.Close();
            }
        }

        // Completely re-write this function
        public void loadWIP()
        {
            this._WIP = new List<Articolo>();
            if (this.RepartoID != -1)
            {
                ElencoArticoliAperti elOpenArt = new ElencoArticoliAperti(this.RepartoID);
                for (int j = 0; j < elOpenArt.ArticoliAperti.Count; j++)
                {
                    this._WIP.Add(elOpenArt.ArticoliAperti[j]);
                }
            }
        }


        /* Configurazione visualizzazione nomi utente su Andon:
         * 0 --> vedo username
         * 1 --> vedo il nome
         * 2 --> nome e iniziale del cognome
         * 3 --> nome e cognome
         */
        public char UsernameFormat
        {
            get
            {
                char ret = '0';
                if (this.RepartoID!=-1)
                { 
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                    + "AND ID = " + this.RepartoID.ToString() + " AND parametro LIKE 'Andon FormatoUsername'";
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
                }
                return ret;
            }
            set
            {
                if (this.RepartoID != -1)
                {
                    // Verifico che sia presente la configurazione
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Reparto' "
                        + "AND ID = " + this.RepartoID.ToString() + " AND parametro LIKE 'Andon FormatoUsername'";
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
                            + this.RepartoID.ToString() + ", 'Andon FormatoUsername', '" + value.ToString() + "')";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE configurazione SET valore = '" + value + "' WHERE "
                        + " Sezione = 'Reparto' AND ID = " + this.RepartoID.ToString() +
                        " AND parametro LIKE 'Andon FormatoUsername'";
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
        }
    }

}