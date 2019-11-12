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
            this._FieldList.Add("EndProductionWeek", 0);
            this._FieldList.Add("DeliveryWeek", 0);

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

        public List<DepartmentAndonProductsStruct> WIP;

        public AndonReparto(int idRep):base()
        {
            this.WIP = new List<DepartmentAndonProductsStruct>();
            this.Warnings = new List<DepartmentWarningStruct>();
            Reparto rp = new Reparto(idRep);
            this._RepartoID = idRep;
            this._DepartmentName = rp.name;
            this._WIP = new List<Articolo>();
            this._DepartmentTimezone = rp.tzFusoOrario;
            this.UserPanel = new List<UserPanelStruct>();


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
            this._FieldList.Add("EndProductionWeek", 0);
            this._FieldList.Add("DeliveryWeek", 0);

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
            this._FieldListTasks.Add("TaskAssignedUsers", 0);
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

        public List<UserPanelStruct> UserPanel;

        public List<DepartmentWarningStruct> Warnings;

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

        // OBSOLETE
        /*public void loadWIP()
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
        */
        public void loadWIP2()
        {
            this.WIP = new List<DepartmentAndonProductsStruct>();
            if (this.RepartoID!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT "
+ "commesse.idcommesse AS SalesOrderID,"                        //0
+ "commesse.anno AS SalesOrderYear,"
+ "commesse.ExternalID AS OrderExternalID,"                     //2
+ "anagraficaclienti.codice AS CustomerID, "
+ "anagraficaclienti.ragsociale AS CustomerName,"               //4
+ "commesse.dataInserimento AS SalesOrderDate,"
+ "commesse.note AS SalesOrderNotes,"                           //6
+ "variantiprocessi.ExternalID AS ProductExternalID,"              
+ "productionplan.id AS ProductionOrderID,"                     //8
+ "productionplan.anno AS ProductionOrderYear,"
+ "productionplan.processo AS ProductionOrderProductTypeID,"    //10
+ "productionplan.revisione AS ProductionOrderProductTypeReview,"
+ "productionplan.variante AS ProductionOrderProductID,"        //12
+ "productionplan.matricola AS ProductionOrderSerialNumber,"
+ "productionplan.status AS ProductionOrderStatus,"             //14
+ "productionplan.reparto AS ProductionOrderDepartmentID,"
+ "productionplan.startTime AS ProductionOrderStartTime,"       //16
+ "productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,"
+ "productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,"  //18
+ "productionplan.planner AS ProductionOrderPlanner,"
+ "productionplan.quantita AS ProductionOrderQuantityOrdered,"  //20
+ "productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,"
+ "productionplan.kanbanCard AS ProductionOrderKanbanCardID,"   //22
+ "productionplan.measurementunit AS MeasurementUnit, "
+ "processo.processID AS ProductTypeID,"                        //24
+ "processo.revisione AS ProductTypeReview,"
+ "processo.dataRevisione AS ProductTypeReviewDate,"            //26
+ "processo.Name AS ProductTypeName,"
+ "processo.description AS ProductTypeDescription,"             //28
+ "processo.attivo AS ProductTypeEnabled,"
+ "varianti.idvariante AS ProductID,"                           //30
+ "varianti.nomeVariante AS ProductName,"                       //31
+ " varianti.descVariante AS ProductDescription,"               //32
+ " reparti.idreparto AS DepartmentID,"
+ " reparti.nome AS DepartmentName,"    
+ " reparti.descrizione AS DepartmentDescription,"
+ " reparti.cadenza AS DepartmentTaktTime,"
+ " reparti.timezone AS DepartmentTimeZone,"
+ " productionplan.LeadTime AS ProductRealLeadTime,"
+ " productionplan.WorkingTime AS ProductRealWorkingTime, "
+ " productionplan.Delay AS ProductRealDelay,"
+ " productionplan.EndProductionDateReal AS ProductRealEndProductionDate,"
+ " tasksproduzione.TaskiD AS TaskID,"
+ " tasksproduzione.name AS TaskName,"
+ " tasksproduzione.description AS TaskDescription,"
+ " tasksproduzione.earlyStart As TaskEarlyStart,"
+ " tasksproduzione.lateStart AS TaskLateStart,"
+ " tasksproduzione.earlyFinish AS TaskEarlyFinish,"
+ " tasksproduzione.lateFinish AS TaskLateFinish,"
+ "tasksproduzione.status AS TaskStatus,"
+ "tasksproduzione.nOperatori AS TaskNumOperators,"
+ "tasksproduzione.qtaPrevista AS TaskQuantityOrdered,"
+ "tasksproduzione.qtaProdotta AS TaskQuantityProduced,"
+ "tempiciclo.setup AS TaskSetupTimePlanned,"
+ "tempiciclo.tempo AS TaskCycleTimePlanned,"
+ "tempiciclo.tunload AS TaskUnloadTimePlanned,"
+ "postazioni.idpostazioni AS WorkstationID,"
+ "postazioni.name AS WorkstationName,"
+ "postazioni.description AS WorkstationDescription,"
+ "tasksproduzione.endDateReal as TaskEndDateReal,"
+ "tasksproduzione.LeadTime AS TaskLeadTime,"
+ "tasksproduzione.WorkingTime AS TaskWorkingTime,"
+ " tasksproduzione.Delay AS TaskDelay, "
+ " tasksproduzione.OrigTask AS TaskOriginalTaskID, "
+ " tasksproduzione.RevOrigTask AS TaskOriginalTaskRev, "
 + " tasksproduzione.variante AS TaskOriginalTaskVar, "
 + " taskuser.user AS TaskUser "
 + " FROM anagraficaclienti INNER JOIN commesse ON(anagraficaclienti.codice = commesse.cliente) INNER JOIN "
 + " productionplan ON(commesse.anno = productionplan.annoCommessa AND commesse.idcommesse = productionplan.commessa)"
 + " INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto)"
 + " INNER JOIN variantiprocessi ON(productionplan.variante = variantiprocessi.variante AND productionplan.processo = variantiprocessi.processo AND productionplan.revisione= variantiprocessi.revProc)"
 + " INNER JOIN varianti ON(varianti.idvariante = variantiprocessi.variante)"
 + " INNER JOIN processo ON(processo.ProcessID = variantiprocessi.processo AND processo.revisione = variantiprocessi.revProc)"
 + " INNER JOIN tasksproduzione ON(tasksproduzione.idArticolo = productionplan.id AND tasksproduzione.annoArticolo = productionplan.anno)"
 + " inner join processo AS TaskProcess ON(TaskProcess.processID = tasksproduzione.origTask AND TaskProcess.revisione = TasksProduzione.revOrigTask)"
 + " INNER JOIN varianti AS TaskVariant ON(taskvariant.idvariante = tasksproduzione.variante)"
 + " INNER JOIN postazioni ON(postazioni.idpostazioni = tasksproduzione.postazione)"
 + " INNER JOIN tempiciclo ON(tempiciclo.processo = tasksproduzione.origTask AND tempiciclo.revisione= tasksproduzione.revOrigTask AND tasksproduzione.variante = tempiciclo.variante)"
 + " LEFT JOIN taskuser on (tasksproduzione.taskID = taskuser.taskid) "
 + " WHERE productionplan.status <> 'F' AND productionplan.status <> 'N' AND reparti.idreparto = "+this.RepartoID.ToString()
 + " order by productionplan.dataPrevistaFineProduzione";

                MySqlDataReader rdr = cmd.ExecuteReader();
                List<DepartmentAndonFullStruct> fullList = new List<DepartmentAndonFullStruct>();

                while(rdr.Read())
                {
                    DepartmentAndonFullStruct curr = new DepartmentAndonFullStruct();
                    curr.CommessaID = rdr.GetInt32(0);
                    curr.SalesOrderYear = rdr.GetInt32(1);
                    curr.OrderExternalID = rdr.GetString(2);
                    curr.CommessaCodiceCliente = rdr.GetString(3);
                    curr.CommessaRagioneSocialeCliente = rdr.GetString(4);
                    curr.CommessaDataInserimento = rdr.GetDateTime(5);
                    if (!rdr.IsDBNull(6))
                    { 
                        curr.CommessaNote = rdr.GetString(6);
                    }
                    if (!rdr.IsDBNull(7))
                    { 
                        curr.ProductExternalID = rdr.GetString(7);
                    }
                    curr.ProdottoID = rdr.GetInt32(8);
                    curr.ProdottoYear = rdr.GetInt32(9);
                    if(!rdr.IsDBNull(13))
                    { 
                    curr.ProdottoMatricola = rdr.GetString(13);
                    }
                    curr.ProdottoStatus = rdr.GetChar(14);
                    if(!rdr.IsDBNull(15))
                    { 
                        curr.DepartmentID = rdr.GetInt32(15);
                    }
                    curr.Reparto = rdr.GetString(34);
                    curr.DataPrevistaConsegna = rdr.GetDateTime(17);
                    curr.DataPrevistaFineProduzione = rdr.GetDateTime(18);
                    curr.ProdottoQuantita = rdr.GetDouble(20);
                    curr.ProdottoQuantitaRealizzata = rdr.GetDouble(21);
                    curr.MeasurementUnit = rdr.GetString(23);
                    curr.ProdottoLineaProdotto = rdr.GetString(27);
                    curr.ProdottoNomeProdotto = rdr.GetString(31);
                    curr.ProdottoRitardo = rdr.GetTimeSpan(40);
                    curr.ProdottoTempodiLavoroTotale = rdr.GetTimeSpan(39);

                    curr.DepartmentID = rdr.GetInt32(33);
                    curr.Reparto = rdr.GetString(34);
                    curr.TaskID = rdr.GetInt32(42);
                    curr.TaskNome = rdr.GetString(43);
                    curr.TaskDescrizione = rdr.GetString(44);
                    curr.TaskEarlyStart = rdr.GetDateTime(45);
                    curr.TaskLateStart = rdr.GetDateTime(46);
                    curr.TaskEarlyFinish = rdr.GetDateTime(47);
                    curr.TaskLateFinish = rdr.GetDateTime(48);
                    curr.TaskStatus = rdr.GetChar(49);
                    curr.TaskNumeroOperatori = rdr.GetInt32(50);
                    curr.TaskQuantitaPrevista = rdr.GetDouble(51);
                    curr.TaskQuantitaProdotta = rdr.GetDouble(52);
                    curr.TaskTempoCiclo = rdr.GetTimeSpan(54);
                    curr.TaskPostazione = rdr.GetString(57);
                    if (!rdr.IsDBNull(66))
                    { 
                    curr.AssignedUser = rdr.GetString(66);
                    }

                    /*
        public Double ProdottoIndicatoreCompletamentoTasks;
        public Double ProdottoIndicatoreCompletamentoTempoPrevisto;
                     * */
                    fullList.Add(curr);
                }

                var prodList = fullList.Select(s=> new { s.CommessaID, s.SalesOrderYear,
                    s.OrderExternalID, s.CommessaCodiceCliente, s.CommessaRagioneSocialeCliente, 
                    s.CommessaDataInserimento, s.CommessaNote, s.ProductExternalID, s.ProdottoID,
                    s.ProdottoYear, s.ProdottoLineaProdotto, s.ProdottoNomeProdotto,
                    s.ProdottoMatricola, s.ProdottoStatus, s.DepartmentID, s.Reparto,
                    s.DataPrevistaConsegna, s.DataPrevistaFineProduzione, s.EarlyStart,
                    s.LateStart, s.EarlyFinish, s.LateFinish, s.ProdottoQuantita,
                    s.ProdottoQuantitaRealizzata, s.MeasurementUnit, s.ProdottoRitardo, s.ProdottoTempodiLavoroTotale
        })
        .Distinct();

                this.WIP = new List<DepartmentAndonProductsStruct>();

                foreach (var m in prodList)
                {
                    DepartmentAndonProductsStruct currProd = new DepartmentAndonProductsStruct();
                    currProd.CommessaID = m.CommessaID;
                    currProd.SalesOrderYear = m.SalesOrderYear;
                    currProd.OrderExternalID = m.OrderExternalID;

                    currProd.CommessaCodiceCliente = m.CommessaCodiceCliente;
                    currProd.CommessaRagioneSocialeCliente=m.CommessaRagioneSocialeCliente;
                    currProd.CommessaDataInserimento = m.CommessaDataInserimento;
                    currProd.CommessaNote = m.CommessaNote;
                    currProd.ProductExternalID = m.ProductExternalID;
                    currProd.ProdottoID = m.ProdottoID;
                    currProd.ProdottoYear = m.ProdottoYear;
                    currProd.ProdottoLineaProdotto = m.ProdottoLineaProdotto;
                    currProd.ProdottoNomeProdotto = m.ProdottoNomeProdotto;
                    currProd.ProdottoMatricola = m.ProdottoMatricola;
                    currProd.ProdottoStatus = m.ProdottoStatus;
                    currProd.DepartmentID = m.DepartmentID;
                    currProd.Reparto = m.Reparto;
                    currProd.DataPrevistaConsegna = m.DataPrevistaConsegna;
                    currProd.DataPrevistaFineProduzione = m.DataPrevistaFineProduzione;
                    currProd.EarlyStart = m.EarlyStart;
                    currProd.LateStart = m.LateStart;
                    currProd.EarlyFinish = m.EarlyFinish;
                    currProd.LateFinish = m.LateFinish;
                    currProd.ProdottoQuantita = m.ProdottoQuantita;
                    currProd.ProdottoQuantitaRealizzata = m.ProdottoQuantitaRealizzata;
                    currProd.MeasurementUnit = m.MeasurementUnit;
                    currProd.ProdottoRitardo= m.ProdottoRitardo;
                    currProd.ProdottoTempodiLavoroTotale = m.ProdottoTempodiLavoroTotale;
                    int deliveryYear = currProd.DataPrevistaConsegna.Year;
                    int deliveryWeek = Dati.Utilities.GetWeekOfTheYear(currProd.DataPrevistaConsegna);
                    currProd.DeliveryWeek = deliveryWeek + "/" + deliveryYear;
                    int endProdYear = currProd.DataPrevistaFineProduzione.Year;
                    int endProdWeek = Dati.Utilities.GetWeekOfTheYear(currProd.DataPrevistaFineProduzione);
                    currProd.EndProductionWeek = endProdWeek + "/" + endProdYear;

                    currProd.Tasks = new List<DepartmentAndonTasksStruct>();

                    var tasksList = fullList
                        .Where(s => s.ProdottoID == currProd.ProdottoID && s.ProdottoYear == currProd.ProdottoYear)
                        .GroupBy(g => new { g.TaskID, g.TaskNome, g.TaskDescrizione, g.TaskPostazione,
                            g.TaskEarlyStart, g.TaskLateStart, g.TaskEarlyFinish, g.TaskLateFinish,
                            g.TaskNumeroOperatori, g.TaskTempoCiclo, g.TaskTempoDiLavoroPrevisto, g.TaskTempoDiLavoroEffettivo,
                            g.TaskStatus, g.TaskQuantitaPrevista, g.TaskQuantitaProdotta, g.TaskRitardo, g.TaskInizioEffettivo,
                            g.TaskFineEffettiva})
                        .Select(s => new
                        {
                            ID = s.Key,
                            AssignedUsers = String.Join(", ", s.Select(ss=>ss.AssignedUser))
 
                        })
                        .OrderBy(x=>x.ID.TaskLateStart);

                    foreach(var t in tasksList)
                    {
                        DepartmentAndonTasksStruct currTask = new DepartmentAndonTasksStruct();
                        currTask.TaskID = t.ID.TaskID;
                        currTask.TaskNome = t.ID.TaskNome;
                        currTask.TaskDescrizione=t.ID.TaskDescrizione;
                        currTask.TaskPostazione=t.ID.TaskPostazione;
                        currTask.TaskEarlyStart=t.ID.TaskEarlyStart;
                        currTask.TaskLateStart=t.ID.TaskLateStart;
                        currTask.TaskEarlyFinish=t.ID.TaskEarlyFinish;
                        currTask.TaskLateFinish=t.ID.TaskLateFinish;
                        currTask.TaskNumeroOperatori=t.ID.TaskNumeroOperatori;
                        currTask.TaskTempoCiclo=t.ID.TaskTempoCiclo;
                        currTask.TaskTempoDiLavoroPrevisto=t.ID.TaskTempoDiLavoroPrevisto;
                        currTask.TaskTempoDiLavoroEffettivo=t.ID.TaskTempoDiLavoroEffettivo;
                        currTask.TaskStatus=t.ID.TaskStatus;
                        currTask.TaskQuantitaPrevista=t.ID.TaskQuantitaPrevista;
                        currTask.TaskQuantitaProdotta=t.ID.TaskQuantitaProdotta;
                        currTask.TaskRitardo=t.ID.TaskRitardo;
                        currTask.TaskInizioEffettivo=t.ID.TaskInizioEffettivo;
                        currTask.TaskFineEffettiva = t.ID.TaskFineEffettiva;
                        currTask.AssignedUser = t.AssignedUsers;

                        currProd.Tasks.Add(currTask);
                    }

                    this.WIP.Add(currProd);
                }

                conn.Close();
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

        public void loadUserPanelData()
        {
            this.UserPanel = new List<UserPanelStruct>();
            if(this.RepartoID!=-1)
            { 
                UserList usrList = new UserList(new Permesso("Task Produzione"));
                foreach(var m in usrList.listUsers)
                {
                    m.loadTaskAvviati();
                    UserPanelStruct curr = new UserPanelStruct();
                    curr.firstName = m.name;
                    curr.lastName = m.cognome;
                    curr.username = m.username;
                    curr.isActive = m.TaskAvviati.Count == 0 ? false : true;
                    this.UserPanel.Add(curr);
                }
            }
        }

        public int OrdersToBeCompletedByTheEndOfTheShift(DateTime endDate)
        {
            int ret = 0;
            if(this.RepartoID!=-1)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(id) FROM productionplan WHERE (status = 'I' OR status='P') AND reparto = "+this.RepartoID.ToString()
                    +" AND dataPrevistaFineProduzione <= '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = rdr.GetInt32(0);
                }
                conn.Close();
            }
            return ret;
        }

        public void loadOpenWarnings()
        {
            this.Warnings = new List<DepartmentWarningStruct>();
            if(this.RepartoID!=-1)
            { 
                WarningAperti Elenco = new WarningAperti(new Reparto(this.RepartoID));
                if(Elenco!=null && Elenco.Elenco!=null)
                {
                    for(int i = 0; i < Elenco.Elenco.Count; i++)
                    {
                        DepartmentWarningStruct curr = new DepartmentWarningStruct();
                        curr.WarningID = Elenco.Elenco[i].ID;
                        curr.TaskID = Elenco.Elenco[i].TaskID;
                        curr.OpeningDate = Elenco.Elenco[i].DataChiamata;
                        curr.WorkstationName = Elenco.Elenco[i].NomePostazione;
                        curr.User = Elenco.Elenco[i].User;

                        this.Warnings.Add(curr);
                    }
                }
            }
        }
    }

    public struct DepartmentAndonProductsStruct
    {
        public int CommessaID;
        public int SalesOrderYear;
        public String OrderExternalID;
        public String CommessaCodiceCliente;
        public String CommessaRagioneSocialeCliente;
        public DateTime CommessaDataInserimento;
        public String CommessaNote;
        public String ProductExternalID;
        public int ProdottoID;
        public int ProdottoYear;
        public String ProdottoLineaProdotto;
        public String ProdottoNomeProdotto;
        public String ProdottoMatricola;
        public Char ProdottoStatus;
        public int DepartmentID;
        public String Reparto;
        public DateTime DataPrevistaConsegna;
        public String DeliveryWeek;
        public DateTime DataPrevistaFineProduzione;
        public String EndProductionWeek;
        public DateTime EarlyStart;
        public DateTime LateStart;
        public DateTime EarlyFinish;
        public DateTime LateFinish;
        public double ProdottoQuantita;
        public double ProdottoQuantitaRealizzata;
        public String MeasurementUnit;
        public TimeSpan ProdottoRitardo;
        public TimeSpan ProdottoTempodiLavoroTotale;
        public Double ProdottoIndicatoreCompletamentoTasks;
        public Double ProdottoIndicatoreCompletamentoTempoPrevisto;

        public List<DepartmentAndonTasksStruct> Tasks;
    }
    public struct DepartmentAndonTasksStruct
    {
        public int TaskID;
        public String TaskNome;
        public String TaskDescrizione;
        public String TaskPostazione;
        public DateTime TaskEarlyStart;
        public DateTime TaskLateStart;
        public DateTime TaskEarlyFinish;
        public DateTime TaskLateFinish;
        public int TaskNumeroOperatori;
        public TimeSpan TaskTempoCiclo;
        public TimeSpan TaskTempoDiLavoroPrevisto;
        public TimeSpan TaskTempoDiLavoroEffettivo;
        public Char TaskStatus;
        public Double TaskQuantitaPrevista;
        public Double TaskQuantitaProdotta;
        public TimeSpan TaskRitardo;
        public DateTime TaskInizioEffettivo;
        public DateTime TaskFineEffettiva;
        public String AssignedUser;
    }

    public struct DepartmentAndonFullStruct
    {
        public int CommessaID;
        public int SalesOrderYear;
        public String OrderExternalID;
        public String CommessaCodiceCliente;
        public String CommessaRagioneSocialeCliente;
        public DateTime CommessaDataInserimento;
        public String CommessaNote;
        public String ProductExternalID;
        public int ProdottoID;
        public int ProdottoYear;
        public String ProdottoLineaProdotto;
        public String ProdottoNomeProdotto;
        public String ProdottoMatricola;
        public Char ProdottoStatus;
        public int DepartmentID;
        public String Reparto;
        public DateTime DataPrevistaConsegna;
        public DateTime DataPrevistaFineProduzione;
        public DateTime EarlyStart;
        public DateTime LateStart;
        public DateTime EarlyFinish;
        public DateTime LateFinish;
        public double ProdottoQuantita;
        public double ProdottoQuantitaRealizzata;
        public String MeasurementUnit;
        public TimeSpan ProdottoRitardo;
        public TimeSpan ProdottoTempodiLavoroTotale;
        public Double ProdottoIndicatoreCompletamentoTasks;
        public Double ProdottoIndicatoreCompletamentoTempoPrevisto;

        public int TaskID;
        public String TaskNome;
        public String TaskDescrizione;
        public String TaskPostazione;
        public DateTime TaskEarlyStart;
        public DateTime TaskLateStart;
        public DateTime TaskEarlyFinish;
        public DateTime TaskLateFinish;
        public int TaskNumeroOperatori;
        public TimeSpan TaskTempoCiclo;
        public TimeSpan TaskTempoDiLavoroPrevisto;
        public TimeSpan TaskTempoDiLavoroEffettivo;
        public Char TaskStatus;
        public Double TaskQuantitaPrevista;
        public Double TaskQuantitaProdotta;
        public TimeSpan TaskRitardo;
        public DateTime TaskInizioEffettivo;
        public DateTime TaskFineEffettiva;
        public String AssignedUser;
    }

    public struct UserPanelStruct
    {
        public String username;
        public String firstName;
        public String lastName;
        public Boolean isActive;
    }

    public struct ProductivityIndicatorsStruct
    {
        public int OrdersToBeCompletedByTheEndOfTheShift;
    }

    public struct DepartmentWarningStruct
    {
        public int WarningID;
        public int TaskID;
        public DateTime OpeningDate;
        public String WorkstationName;
        public String User;
    }
}