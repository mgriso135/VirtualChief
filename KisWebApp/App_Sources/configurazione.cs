/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2020 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using Dapper;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace KIS.App_Code
{
    public class KISConfig
    {
        protected String Tenant;

        public int workspace_id;

        public KISConfig(String Tenant)
        {
            this.Tenant = Tenant;
        }
        public KISConfig(String Tenant, int workspace)
        {
            this.Tenant = Tenant;
            workspace_id = workspace;
        }

        public Boolean WizLogoCompleted
        {
            get
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'");
                ret = valore != null;
                conn.Close();
                return ret;
            }
        }

        public Boolean WizRepartiCompleted
        {
            get
            {
                ElencoReparti elRep = new ElencoReparti(this.Tenant);
                Boolean ret = true;
                if(elRep.elenco.Count == 0)
                {
                    ret = false;
                }
                else
                {
                    ret = true;
                    for (int i = 0; i < elRep.elenco.Count; i++)
                    {
                        if (!elRep.elenco[i].FullyConfigured)
                        {
                            ret = false;
                        }
                    }
                }
                return ret;
            }
        }
        
        public Boolean WizPostazioniCompleted
        {
            get
            {
                ElencoPostazioni elPost = new ElencoPostazioni(this.Tenant);
                return elPost.elenco.Count > 0;
            }
        }

        public Boolean WizUsersCompleted
        {
            get
            {
                UserList usrList = new UserList(this.Tenant);
                Boolean ret = false;
                if(usrList.listUsers.Count >0)
                {
                    ret = true;
                    for(int i =0; i < usrList.listUsers.Count; i++)
                    {
                        if(!usrList.listUsers[i].FullyConfigured)
                        {
                            ret = false;
                        }
                    }
                }
                else
                {
                    ret = false;
                }
                return ret;
            }
        }
        
        public Boolean WizTimezoneCompleted
        {
            get
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT * FROM configurazione WHERE parametro LIKE 'TimeZone'");
                ret = valore != null;
                conn.Close();
                return ret;
            }
        }

        public Boolean WizAndonCompleted {
            get
            {
                bool ret = false;
                AndonCompleto andon = new AndonCompleto(this.Tenant);
                andon.loadCampiVisualizzati();
                andon.loadCampiVisualizzatiTasks();
                ret = (andon.CampiVisualizzati.Count > 0 && andon.CampiVisualizzatiTasks.Count > 0);
                return ret;   
            }
        }

        public Boolean WizCustomerReportCompleted {
            get {
                Boolean ret = false;
                configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport(this.Tenant);
                ret = cfgCust.IDCommessa ||
                cfgCust.Cliente ||
                cfgCust.DataInserimentoOrdine ||
                cfgCust.NoteOrdine ||
                cfgCust.IDProdotto ||
                cfgCust.NomeProdotto ||
                cfgCust.NomeVariante ||
                cfgCust.Matricola ||
                cfgCust.Status ||
                cfgCust.Reparto ||
                cfgCust.DataPrevistaConsegna ||
                cfgCust.DataPrevistaFineProduzione ||
                cfgCust.EarlyStart ||
                cfgCust.EarlyFinish ||
                cfgCust.LateStart ||
                cfgCust.LateFinish ||
                cfgCust.Quantita ||
                cfgCust.QuantitaProdotta ||
                cfgCust.Ritardo ||
                cfgCust.TempoDiLavoroTotale ||
                cfgCust.LeadTime ||
                cfgCust.TempoDiLavoroPrevisto ||
                cfgCust.IndicatoreCompletamentoTasks ||
                cfgCust.IndicatoreCompletamentoTempoPrevisto ||
                cfgCust.ViewGanttTasks ||
                cfgCust.ViewElencoTasks ||
                cfgCust.Task_ID ||
                cfgCust.Task_Nome ||
                cfgCust.Task_Descrizione ||
                cfgCust.Task_Postazione ||
                cfgCust.Task_EarlyStart ||
                cfgCust.Task_LateStart ||
                cfgCust.Task_EarlyFinish ||
                cfgCust.Task_LateFinish ||
                cfgCust.Task_NOperatori ||
                cfgCust.Task_TempoCiclo ||
                cfgCust.Task_TempoDiLavoroPrevisto ||
                cfgCust.Task_TempoDiLavoroEffettivo ||
                cfgCust.Task_Status ||
                cfgCust.Task_QuantitaProdotta;

                return ret;
            }
        }

        public DateTime ExpiryDate
        {
            get
            {
                DateTime exp = new DateTime(1970, 1, 1);
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                ConfigRow row = conn.QueryFirstOrDefault<ConfigRow>("SELECT sezione, ID, parametro, valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'ExpiryDate'");
                if (row != null)
                {
                    try
                    {
                        String[] aExp = row.valore.Split('/');
                        int anno = Int32.Parse(aExp[2]);
                        int mese = Int32.Parse(aExp[1]);
                        int giorno = Int32.Parse(aExp[0]);
                        FusoOrario fuso = new FusoOrario(this.Tenant);
                        exp = new DateTime(anno, mese, giorno);
                        exp = TimeZoneInfo.ConvertTimeFromUtc(exp, fuso.tzFusoOrario);
                    }
                    catch
                    {
                        exp = new DateTime(1970, 1, 1);
                    }
                    conn.Close();
                }
                return exp;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                String expDate = value.Day.ToString() + "/"
                    + value.Month.ToString() + "/"
                    + value.Year.ToString();

                // Controllo se esiste già il parametro
                bool exists = conn.ExecuteScalar<string>("SELECT sezione, ID, parametro, valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'ExpiryDate'") != null;

                string sql;
                if (exists)
                {
                    sql = "UPDATE configurazione SET parametro = @p WHERE "
                        + "Sezione = 'Main' AND parametro = 'ExpiryDate'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Main', -1, 'ExpiryDate', @p)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { p = expDate }, tr);
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
             }
        }

        public String Language
        {
            get
            {
                String ret = "en";
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'Language'");
                if (valore != null)
                {
                    ret = valore;
                }
                conn.Close();
                return ret;
            }
        }

        public String baseUrl
        {
            get
            {
                String ret = "";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'BaseUrl'");
                if (valore != null)
                {
                    ret = valore;
                }
                conn.Close();
                return ret;
            }
        }

        public String basePath
        {
            get
            {
                String ret = "";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'BasePath'");
                if (valore != null)
                {
                    ret = valore;
                }
                conn.Close();
                return ret;
            }
        }

        public String ConfigController_X_API_KEY
        {
            get
            {
                String ret = "";
                MySqlConnection conn = (new Dati.Dati().mycon(this.Tenant));
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione LIKE 'ConfigController' AND parametro"
                    + " LIKE 'X-API-KEY'");
                if (valore != null)
                {
                    ret = valore;
                }
                conn.Close();
                return ret;
         }
            }

        public Boolean SalesOrderImportFrom3PartySystem
        {
            get
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'SalesOrderImportFrom3PartySystemEnabled'");
                if (valore != null)
                {
                    ret = (valore.Trim() == "1" || valore.Trim().ToLowerInvariant() == "true");
                }
                conn.Close();
                return ret;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();

                // Controllo se esiste già il parametro
                bool exists = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'SalesOrderImportFrom3PartySystemEnabled'") != null;
                string sql;
                if (exists)
                {
                    sql = "UPDATE configurazione SET `valore` = @valore WHERE(`Sezione` = 'Main') and(`ID` = '-1') and(`parametro` = 'SalesOrderImportFrom3PartySystemEnabled')";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Main', -1, 'SalesOrderImportFrom3PartySystemEnabled', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        private class ConfigRow
        {
            public string sezione { get; set; }
            public int ID { get; set; }
            public string parametro { get; set; }
            public string valore { get; set; }
        }

    }

    public class Logo
    {
        protected String Tenant;

        public String log;
        public String filePath
        {
            get
            {
                String percorsoLogo = "~/Data/Logo/LogoMG.png";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'");
                if (valore != null)
                {
                    percorsoLogo = "~/Data/Logo/" + valore;
                }
                conn.Close();
                return percorsoLogo;
            }
            set
            {
                log = "Carico... ";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool found = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'") != null;

                MySqlTransaction tr = conn.BeginTransaction();

                string sql;
                if (found)
                {
                    sql = "UPDATE configurazione SET valore = @valore"
                        + " WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Main', 0, 'Logo', @valore)";
                }
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
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

        public String filePathAbs
        {
            get
            {
                String percorsoLogo = "/Data/Logo/LogoMG.png";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'");
                if (valore != null)
                {
                    percorsoLogo = "/Data/Logo/" + valore;
                }
                conn.Close();
                return percorsoLogo;
            }
            set
            {
                log = "Carico... ";
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool found = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'") != null;

                MySqlTransaction tr = conn.BeginTransaction();

                string sql;
                if (found)
                {
                    sql = "UPDATE configurazione SET valore = @valore"
                        + " WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Main', 0, 'Logo', @valore)";
                }
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
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

        public Logo(String Tenant)
        {
            this.Tenant = Tenant;
        }
    }

    public class FusoOrario
    {
        protected String Tenant;

        public String log;
        private String _fusoOrario;
        public String fusoOrario
        {
            get { return this._fusoOrario; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                Boolean res = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione ='Main' AND ID=-1 AND parametro='TimeZone'") != null;
                string sql;
                if (res)
                {
                    sql = "UPDATE configurazione SET valore = @valore"
                        + " WHERE Sezione ='Main' AND ID =-1 AND parametro = 'TimeZone'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Main', "
                        + "-1, "
                        + "'TimeZone', "
                        + "@valore"
                        +")";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = sql + " <br/>" + ex.Message;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public TimeZoneInfo tzFusoOrario
        {
            get
            {
                return TimeZoneInfo.FindSystemTimeZoneById(this._fusoOrario);
            }
        }

        public FusoOrario(String Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione ='Main' AND ID=-1 AND parametro='TimeZone'");
            if (valore != null)
            {
                this._fusoOrario = valore;
            }
            else
            {
                this._fusoOrario = "W. Europe Standard Time";
            }
            conn.Close();
        }
    }

    public class KanbanBoxConfig : ConfigurationSection
    {
        protected String Tenant;

        [ConfigurationProperty("kanbanBoxEnabled", DefaultValue=false, IsRequired = false)]
        public Boolean KanbanBoxEnabled
        {
            get
            {
                return (Boolean)this["kanbanBoxEnabled"];
            }
            set
            {
                this["kanbanBoxEnabled"] = value;
            }
        }

        [ConfigurationProperty("x-api-key", IsRequired = false)]
        public String X_API_KEY
        {
            get
            {
                string v = System.Environment.GetEnvironmentVariable("VC_KANBANBOX_API_KEY");
                if (!string.IsNullOrEmpty(v))
                {
                    return v;
                }
                return (String)this["x-api-key"];
            }
            set
            {
                this["kanbanBoxEnabled"] = value;
            }
        }

        [ConfigurationProperty("version", IsRequired = false)]
        public String Version
        {
            get
            {
                return (String)this["version"];
            }
            set
            {
                this["version"] = value;
            }
        }

        public String Url
        {
            get
            {
                return "https://api.kanbanbox.com/rest/v" + this.Version + "/cards/";
            }
        }
    }

    public class WizardConfig
    {
        protected String Tenant;

        public String log;

        public String interfacciaPERT
        {
            get
            {
                String ret;
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione LIKE 'Wizard' and parametro LIKE 'TipoPERT'");
                if (valore != null)
                {
                    ret = valore;
                }
                else
                {
                    ret = "Graph";
                }
                conn.Close();
                return ret;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool exists = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE Sezione LIKE 'Wizard' and parametro LIKE 'TipoPERT'") != null;
                String sqlString;
                if (exists)
                {
                    sqlString = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'Wizard' and parametro LIKE 'TipoPERT'";
                }
                else
                {
                    sqlString = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Wizard', -1, 'TipoPERT', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sqlString, new { valore = value }, tr);
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

        public WizardConfig(String tenant)
        {
            this.Tenant = tenant;
        }
    }

    public class CustomersControllerConfig : ConfigurationSection
    {
        protected String Tenant;

        public String x_api_key;

        [ConfigurationProperty("baseUrl", IsRequired = true)]
        public String BaseUrl
        {
            get
            {
                return (this["baseUrl"]).ToString();
            }
            set
            {
                this["baseUrl"] = value;
            }
        }

        public CustomersControllerConfig()
        {
            this.x_api_key = "";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE "
                + "Sezione LIKE 'CustomersController'"
                + " AND parametro LIKE 'X-API-KEY'");
            if (valore != null)
            {
                this.x_api_key = valore;
            }
            else
            {
                this.x_api_key = "";
            }
            conn.Close();
        }
    }

    public class EventsExportControllerConfig : ConfigurationSection
    {
        protected String Tenant;

        public String x_api_key;

        [ConfigurationProperty("baseUrl", IsRequired = true)]
        public String BaseUrl
        {
            get
            {
                return (this["baseUrl"]).ToString();
            }
            set
            {
                this["baseUrl"] = value;
            }
        }

        public EventsExportControllerConfig()
        {
            this.x_api_key = "";
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string valore = conn.ExecuteScalar<string>("SELECT valore FROM configurazione WHERE "
                + "Sezione LIKE 'EventsExportController'"
                + " AND parametro LIKE 'EVENTSEXPORT-API-KEY'");
            if (valore != null)
            {
                this.x_api_key = valore;
            }
            else
            {
                this.x_api_key = "";
            }
            conn.Close();
        }
    }

    public class configBaseOrderStatusReport
    {
        protected String Tenant;

        public String log;

        protected class CfgRow
        {
            public string parametro { get; set; }
            public string valore { get; set; }
        }

        protected Boolean _IDCommessa;
        public Boolean IDCommessa
        {
            get { return this._IDCommessa; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_IDCommessa'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_IDCommessa'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        +"'OrderStatusCustomerReport base', -1, 'Commessa_IDCommessa', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IDCommessa = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message +" " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Cliente;
        public Boolean Cliente
        {
            get
            { return this._Cliente; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_Cliente'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_Cliente'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Commessa_Cliente', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Cliente = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _DataInserimentoOrdine;
        public Boolean DataInserimentoOrdine
        {
            get { return this._DataInserimentoOrdine; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_DataInserimento'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_DataInserimento'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Commessa_DataInserimento', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._DataInserimentoOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _NoteOrdine;
        public Boolean NoteOrdine
        {
            get { return this._NoteOrdine; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_Note'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_Note'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Commessa_Note', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NoteOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _IDProdotto;
        public Boolean IDProdotto
        {
            get { return this._IDProdotto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_IDProdotto'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_IDProdotto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_IDProdotto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IDProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _NomeProdotto;
        public Boolean NomeProdotto
        {
            get { return this._NomeProdotto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_NomeProdotto'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_NomeProdotto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_NomeProdotto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NomeProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _NomeVariante;
        public Boolean NomeVariante
        {
            get { return this._NomeVariante; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_NomeVariante'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_NomeVariante'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_NomeVariante', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Matricola;
        public Boolean Matricola
        {
            get { return this._Matricola; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Matricola'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Matricola'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Matricola', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Status;
        public Boolean Status
        {
            get { return this._Status; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Status'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Status'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Status', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Reparto;
        public Boolean Reparto
        {
            get { return this._Reparto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Reparto'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Reparto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Reparto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Reparto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _DataPrevistaConsegna;
        public Boolean DataPrevistaConsegna
        {
            get { return this._DataPrevistaConsegna; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_DataPrevistaConsegna', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._DataPrevistaConsegna = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _DataPrevistaFineProduzione;
        public Boolean DataPrevistaFineProduzione
        {
            get { return this._DataPrevistaFineProduzione; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_DataPrevistaFineProduzione', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._DataPrevistaFineProduzione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _EarlyStart;
        public Boolean EarlyStart
        {
            get { return this._EarlyStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_EarlyStart'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_EarlyStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_EarlyStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _EarlyFinish;
        public Boolean EarlyFinish
        {
            get { return this._EarlyFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_EarlyFinish'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_EarlyFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_EarlyFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _LateStart;
        public Boolean LateStart
        {
            get { return this._LateStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_LateStart'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_LateStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_LateStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _LateFinish;
        public Boolean LateFinish
        {
            get { return this._LateFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_LateFinish'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_LateFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_LateFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Quantita;
        public Boolean Quantita
        {
            get { return this._Quantita; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Quantita'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Quantita'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Quantita', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Quantita = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _QuantitaProdotta;
        public Boolean QuantitaProdotta
        {
            get { return this._QuantitaProdotta; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_QuantitaProdotta'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_QuantitaProdotta'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_QuantitaProdotta', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Ritardo;
        public Boolean Ritardo
        {
            get { return this._Ritardo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Ritardo'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Ritardo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Ritardo', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Ritardo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _TempoDiLavoroTotale;
        public Boolean TempoDiLavoroTotale
        {
            get { return this._TempoDiLavoroTotale; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_TempoDiLavoroTotale', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._TempoDiLavoroTotale = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _LeadTime;
        public Boolean LeadTime
        {
            get { return this._LeadTime; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_LeadTime'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_LeadTime'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_LeadTime', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._LeadTime = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _TempoDiLavoroPrevisto;
        public Boolean TempoDiLavoroPrevisto
        {
            get { return this._TempoDiLavoroPrevisto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_TempoDiLavoroPrevisto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _IndicatoreCompletamentoTasks;
        public Boolean IndicatoreCompletamentoTasks
        {
            get { return this._IndicatoreCompletamentoTasks; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_IndicatoreCompletamentoTasks', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IndicatoreCompletamentoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _IndicatoreCompletamentoTempoPrevisto;
        public Boolean IndicatoreCompletamentoTempoPrevisto
        {
            get { return this._IndicatoreCompletamentoTempoPrevisto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_IndicatoreCompletamentoTempoPrevisto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IndicatoreCompletamentoTempoPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _ViewGanttTasks;
        public Boolean ViewGanttTasks
        {
            get { return this._ViewGanttTasks; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_ViewGanttTasks'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_ViewGanttTasks'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_ViewGanttTasks', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._ViewGanttTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _ViewElencoTasks;
        public Boolean ViewElencoTasks
        {
            get { return this._ViewElencoTasks; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_ViewElencoTasks'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_ViewElencoTasks'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_ViewElencoTasks', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._ViewElencoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_ID;
        public Boolean Task_ID
        {
            get { return this._Task_ID; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_ID'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_ID'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_ID', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_ID = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_Nome;
        public Boolean Task_Nome
        {
            get { return this._Task_Nome; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Nome'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Nome'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Nome', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Nome = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_Descrizione;
        public Boolean Task_Descrizione
        {
            get { return this._Task_Descrizione; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Descrizione'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Descrizione'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Descrizione', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Descrizione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_Postazione;
        public Boolean Task_Postazione
        {
            get { return this._Task_Postazione; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Postazione'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Postazione'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Postazione', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Postazione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_EarlyStart;
        public Boolean Task_EarlyStart
        {
            get { return this._Task_EarlyStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_EarlyStart'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_EarlyStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_EarlyStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_LateStart;
        public Boolean Task_LateStart
        {
            get { return this._Task_LateStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_LateStart'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_LateStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_LateStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_EarlyFinish;
        public Boolean Task_EarlyFinish
        {
            get { return this._Task_EarlyFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_EarlyFinish'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_EarlyFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_EarlyFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_LateFinish;
        public Boolean Task_LateFinish
        {
            get { return this._Task_LateFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_LateFinish'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_LateFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_LateFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_NOperatori;
        public Boolean Task_NOperatori
        {
            get { return this._Task_NOperatori; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_NOperatori'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_NOperatori'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_NOperatori', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_NOperatori = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_TempoCiclo;
        public Boolean Task_TempoCiclo
        {
            get { return this._Task_TempoCiclo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_TempoCiclo'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_TempoCiclo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_TempoCiclo', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_TempoCiclo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_TempoDiLavoroPrevisto;
        public Boolean Task_TempoDiLavoroPrevisto
        {
            get { return this._Task_TempoDiLavoroPrevisto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_TempoDiLavoroPrevisto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_TempoDiLavoroEffettivo;
        public Boolean Task_TempoDiLavoroEffettivo
        {
            get { return this._Task_TempoDiLavoroEffettivo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_TempoDiLavoroEffettivo', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_TempoDiLavoroEffettivo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_Status;
        public Boolean Task_Status
        {
            get { return this._Task_Status; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Status'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Status'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Status', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        protected Boolean _Task_QuantitaProdotta;
        public Boolean Task_QuantitaProdotta
        {
            get { return this._Task_QuantitaProdotta; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_QuantitaProdotta'") != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_QuantitaProdotta'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_QuantitaProdotta', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public configBaseOrderStatusReport(String Tenant)
        {
            this.Tenant = Tenant;

            this._IDCommessa = true;
            this._Cliente = true;
            this._DataInserimentoOrdine = true;
            this._NoteOrdine = true;
            this._IDProdotto = true;
            this._NomeProdotto = true;
            this._NomeVariante = true;
            this._Matricola = true;
            this._Status = true;
            this._Reparto = true;
            this._DataPrevistaConsegna = true;
            this._DataPrevistaFineProduzione = true;
            this._EarlyStart=true;
            this._EarlyFinish = true;
            this._LateStart = true;
            this._LateFinish = true;
            this._Quantita = true;
            this._QuantitaProdotta = true;
            this._Ritardo = true;
            this._TempoDiLavoroTotale = true;
            this._LeadTime = true;
            this._TempoDiLavoroPrevisto = true;
            this._IndicatoreCompletamentoTasks = true;
            this._IndicatoreCompletamentoTempoPrevisto = true;
            this._ViewGanttTasks = true;
            this._ViewElencoTasks = true;
            this._Task_ID = true;
            this._Task_Nome = true;
            this._Task_Descrizione = true;
            this._Task_Postazione = true;
            this._Task_EarlyStart = true;
            this._Task_LateStart = true;
            this._Task_EarlyFinish = true;
            this._Task_LateFinish = true;
            this._Task_NOperatori = true;
            this._Task_TempoCiclo = true;
            this._Task_TempoDiLavoroPrevisto = true;
            this._Task_TempoDiLavoroEffettivo = true;
            this._Task_Status = true;
            this._Task_QuantitaProdotta = true;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var r in conn.Query<CfgRow>("SELECT parametro, valore FROM configurazione WHERE Sezione LIKE 'OrderStatusCustomerReport base'"))
            {
                String param = r.parametro;
                String val = r.valore;
                switch (param)
                {
                    case "Commessa_IDCommessa":
                        try
                        {
                            this._IDCommessa = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IDCommessa = true;
                        }
                        break;
                    case "Commessa_Cliente":
                        try
                        {
                            this._Cliente = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Cliente = true;
                        }
                        break;
                    case "Commessa_DataInserimento":
                        try
                        {
                            this._DataInserimentoOrdine = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._DataInserimentoOrdine = true;
                        }
                        break;
                    case "Commessa_Note":
                        try
                        {
                            this._NoteOrdine = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._NoteOrdine = true;
                        }
                        break;
                    case "Prodotto_IDProdotto":
                        try
                        {
                            this._IDProdotto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IDProdotto = true;
                        }
                        break;
                    case "Prodotto_NomeProdotto":
                        try
                        {
                            this._NomeProdotto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._NomeProdotto = true;
                        }
                        break;
                    case "Prodotto_NomeVariante":
                        try
                        {
                            this._NomeVariante = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._NomeVariante = true;
                        }
                        break;
                    case "Prodotto_Matricola":
                        try
                        {
                            this._Matricola = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Matricola = true;
                        }
                        break;
                    case "Prodotto_Status":
                        try
                        {
                            this._Status = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Status = true;
                        }
                        break;
                    case "Prodotto_Reparto":
                        try
                        {
                            this._Reparto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Reparto = true;
                        }
                        break;
                    case "Prodotto_DataPrevistaConsegna":
                        try
                        {
                            this._DataPrevistaConsegna = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._DataPrevistaConsegna = true;
                        }
                        break;
                    case "Prodotto_DataPrevistaFineProduzione":
                        try
                        {
                            this._DataPrevistaFineProduzione = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._DataPrevistaFineProduzione = true;
                        }
                        break;
                    case "Prodotto_EarlyStart":
                        try
                        {
                            this._EarlyStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._EarlyStart = true;
                        }
                        break;
                    case "Prodotto_EarlyFinish":
                        try
                        {
                            this._EarlyFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._EarlyFinish = true;
                        }
                        break;
                    case "Prodotto_LateStart":
                        try
                        {
                            this._LateStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._LateStart = true;
                        }
                        break;
                    case "Prodotto_LateFinish":
                        try
                        {
                            this._LateFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._LateFinish = true;
                        }
                        break;
                    case "Prodotto_Quantita":
                        try
                        {
                            this._Quantita = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Quantita = true;
                        }
                        break;
                    case "Prodotto_QuantitaProdotta":
                        try
                        {
                            this._QuantitaProdotta = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._QuantitaProdotta = true;
                        }
                        break;
                    case "Prodotto_Ritardo":
                        try
                        {
                            this._Ritardo = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Ritardo = true;
                        }
                        break;
                    case "Prodotto_TempoDiLavoroTotale":
                        try
                        {
                            this._TempoDiLavoroTotale = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._TempoDiLavoroTotale = true;
                        }
                        break;
                    case "Prodotto_LeadTime":
                        try
                        {
                            this._LeadTime = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._LeadTime = true;
                        }
                        break;
                    case "Prodotto_TempoDiLavoroPrevisto":
                        try
                        {
                            this._TempoDiLavoroPrevisto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._TempoDiLavoroPrevisto = true;
                        }
                        break;
                    case "Prodotto_IndicatoreCompletamentoTasks":
                        try
                        {
                            this._IndicatoreCompletamentoTasks = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IndicatoreCompletamentoTasks = true;
                        }
                        break;
                    case "Prodotto_IndicatoreCompletamentoTempoPrevisto":
                        try
                        {
                            this._IndicatoreCompletamentoTempoPrevisto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IndicatoreCompletamentoTempoPrevisto = true;
                        }
                        break;
                    case "Prodotto_ViewGanttTasks":
                        try
                        {
                            this._ViewGanttTasks = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._ViewGanttTasks = true;
                        }
                        break;
                    case "Prodotto_ViewElencoTasks":
                        try
                        {
                            this._ViewElencoTasks = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._ViewElencoTasks = true;
                        }
                        break;
                    case "Task_ID":
                        try
                        {
                            this._Task_ID = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_ID = true;
                        }
                        break;
                    case "Task_Nome":
                        try
                        {
                            this._Task_Nome = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Nome = true;
                        }
                        break;
                    case "Task_Descrizione":
                        try
                        {
                            this._Task_Descrizione = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Descrizione = true;
                        }
                        break;
                    case "Task_Postazione":
                        try
                        {
                            this._Task_Postazione = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Postazione = true;
                        }
                        break;
                    case "Task_EarlyStart":
                        try
                        {
                            this._Task_EarlyStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_EarlyStart = true;
                        }
                        break;
                    case "Task_LateStart":
                        try
                        {
                            this._Task_LateStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_LateStart = true;
                        }
                        break;
                    case "Task_EarlyFinish":
                        try
                        {
                            this._Task_EarlyFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_EarlyFinish = true;
                        }
                        break;
                    case "Task_LateFinish":
                        try
                        {
                            this._Task_LateFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_LateFinish = true;
                        }
                        break;
                    case "Task_NOperatori":
                        try
                        {
                            this._Task_NOperatori = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_NOperatori = true;
                        }
                        break;
                    case "Task_TempoCiclo":
                        try
                        {
                            this._Task_TempoCiclo = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_TempoCiclo = true;
                        }
                        break;
                    case "Task_TempoDiLavoroPrevisto":
                        try
                        {
                            this._Task_TempoDiLavoroPrevisto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_TempoDiLavoroPrevisto = true;
                        }
                        break;
                    case "Task_TempoDiLavoroEffettivo":
                        try
                        {
                            this._Task_TempoDiLavoroEffettivo = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_TempoDiLavoroEffettivo = true;
                        }
                        break;
                    case "Task_Status":
                        try
                        {
                            this._Task_Status = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Status = true;
                        }
                        break;
                    case "Task_QuantitaProdotta":
                        try
                        {
                            this._Task_QuantitaProdotta = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_QuantitaProdotta = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            conn.Close();
        }
    }

    public class configCustomerOrderStatusReport : configBaseOrderStatusReport
    {
        protected String Tenant;

        public String codCliente;

        public new Boolean IDCommessa
        {
            get { return this._IDCommessa; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Commessa_IDCommessa'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Commessa_IDCommessa'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Commessa_IDCommessa', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IDCommessa = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Cliente
        {
            get
            { return this._Cliente; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Commessa_Cliente'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Commessa_Cliente'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Commessa_Cliente', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Cliente = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean DataInserimentoOrdine
        {
            get { return this._DataInserimentoOrdine; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Commessa_DataInserimento'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Commessa_DataInserimento'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Commessa_DataInserimento', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._DataInserimentoOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean NoteOrdine
        {
            get { return this._NoteOrdine; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Commessa_Note'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Commessa_Note'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Commessa_Note', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NoteOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean IDProdotto
        {
            get { return this._IDProdotto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_IDProdotto'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_IDProdotto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_IDProdotto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IDProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean NomeProdotto
        {
            get { return this._NomeProdotto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_NomeProdotto'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_NomeProdotto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_NomeProdotto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NomeProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean NomeVariante
        {
            get { return this._NomeVariante; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_NomeVariante'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_NomeVariante'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_NomeVariante', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Matricola
        {
            get { return this._Matricola; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_Matricola'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_Matricola'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_Matricola', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Status
        {
            get { return this._Status; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_Status'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_Status'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_Status', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Reparto
        {
            get { return this._Reparto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_Reparto'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_Reparto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_Reparto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Reparto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean DataPrevistaConsegna
        {
            get { return this._DataPrevistaConsegna; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_DataPrevistaConsegna', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._DataPrevistaConsegna = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean DataPrevistaFineProduzione
        {
            get { return this._DataPrevistaFineProduzione; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_DataPrevistaFineProduzione', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._DataPrevistaFineProduzione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean EarlyStart
        {
            get { return this._EarlyStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_EarlyStart'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_EarlyStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_EarlyStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean EarlyFinish
        {
            get { return this._EarlyFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_EarlyFinish'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_EarlyFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_EarlyFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean LateStart
        {
            get { return this._LateStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_LateStart'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_LateStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_LateStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean LateFinish
        {
            get { return this._LateFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_LateFinish'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_LateFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_LateFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Quantita
        {
            get { return this._Quantita; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_Quantita'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_Quantita'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_Quantita', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Quantita = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean QuantitaProdotta
        {
            get { return this._QuantitaProdotta; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_QuantitaProdotta'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_QuantitaProdotta'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_QuantitaProdotta', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Ritardo
        {
            get { return this._Ritardo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_Ritardo'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_Ritardo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_Ritardo', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Ritardo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean TempoDiLavoroTotale
        {
            get { return this._TempoDiLavoroTotale; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_TempoDiLavoroTotale', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._TempoDiLavoroTotale = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean LeadTime
        {
            get { return this._LeadTime; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_LeadTime'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_LeadTime'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_LeadTime', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._LeadTime = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean TempoDiLavoroPrevisto
        {
            get { return this._TempoDiLavoroPrevisto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_TempoDiLavoroPrevisto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean IndicatoreCompletamentoTasks
        {
            get { return this._IndicatoreCompletamentoTasks; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_IndicatoreCompletamentoTasks', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IndicatoreCompletamentoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean IndicatoreCompletamentoTempoPrevisto
        {
            get { return this._IndicatoreCompletamentoTempoPrevisto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_IndicatoreCompletamentoTempoPrevisto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._IndicatoreCompletamentoTempoPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean ViewGanttTasks
        {
            get { return this._ViewGanttTasks; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_ViewGanttTasks'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_ViewGanttTasks'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_ViewGanttTasks', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._ViewGanttTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean ViewElencoTasks
        {
            get { return this._ViewElencoTasks; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Prodotto_ViewElencoTasks'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Prodotto_ViewElencoTasks'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Prodotto_ViewElencoTasks', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._ViewElencoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_ID
        {
            get { return this._Task_ID; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_ID'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_ID'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_ID', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_ID = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_Nome
        {
            get { return this._Task_Nome; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_Nome'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_Nome'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_Nome', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Nome = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_Descrizione
        {
            get { return this._Task_Descrizione; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_Descrizione'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_Descrizione'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_Descrizione', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Descrizione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_Postazione
        {
            get { return this._Task_Postazione; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_Postazione'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_Postazione'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_Postazione', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Postazione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_EarlyStart
        {
            get { return this._Task_EarlyStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_EarlyStart'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_EarlyStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_EarlyStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_LateStart
        {
            get { return this._Task_LateStart; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_LateStart'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_LateStart'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_LateStart', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_EarlyFinish
        {
            get { return this._Task_EarlyFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_EarlyFinish'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_EarlyFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_EarlyFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_LateFinish
        {
            get { return this._Task_LateFinish; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_LateFinish'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_LateFinish'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_LateFinish', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_NOperatori
        {
            get { return this._Task_NOperatori; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_NOperatori'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_NOperatori'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_NOperatori', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_NOperatori = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_TempoCiclo
        {
            get { return this._Task_TempoCiclo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_TempoCiclo'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_TempoCiclo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_TempoCiclo', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_TempoCiclo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_TempoDiLavoroPrevisto
        {
            get { return this._Task_TempoDiLavoroPrevisto; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_TempoDiLavoroPrevisto', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_TempoDiLavoroEffettivo
        {
            get { return this._Task_TempoDiLavoroEffettivo; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_TempoDiLavoroEffettivo', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_TempoDiLavoroEffettivo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_Status
        {
            get { return this._Task_Status; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_Status'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_Status'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_Status', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }
        public new Boolean Task_QuantitaProdotta
        {
            get { return this._Task_QuantitaProdotta; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                string sezione = "OrderStatusCustomerReport " + this.codCliente;
                bool check = conn.ExecuteScalar<string>("SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE @sezione "
                + "AND parametro LIKE 'Task_QuantitaProdotta'", new { sezione }) != null;
                string sql;
                if (check)
                {
                    sql = "UPDATE configurazione SET valore = @valore WHERE "
                        + "Sezione LIKE @sezione "
                        + "AND parametro LIKE 'Task_QuantitaProdotta'";
                }
                else
                {
                    sql = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "@sezione, -1, 'Task_QuantitaProdotta', @valore)";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { sezione, valore = value.ToString() }, tr);
                    tr.Commit();
                    this._Task_QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + sql;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public configCustomerOrderStatusReport(String Tenant, String idCliente) : base(Tenant)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            this.codCliente = idCliente;
            foreach (var r in conn.Query<CfgRow>("SELECT parametro, valore FROM configurazione WHERE Sezione LIKE @p", new { p = "OrderStatusCustomerReport " + idCliente }))
            {
                String param = r.parametro;
                String val = r.valore;
                switch (param)
                {
                    case "Commessa_IDCommessa":
                        try
                        {
                            this._IDCommessa = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IDCommessa = true;
                        }
                        break;
                    case "Commessa_Cliente":
                        try
                        {
                            this._Cliente = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Cliente = true;
                        }
                        break;
                    case "Commessa_DataInserimento":
                        try
                        {
                            this._DataInserimentoOrdine = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._DataInserimentoOrdine = true;
                        }
                        break;
                    case "Commessa_Note":
                        try
                        {
                            this._NoteOrdine = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._NoteOrdine = true;
                        }
                        break;
                    case "Prodotto_IDProdotto":
                        try
                        {
                            this._IDProdotto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IDProdotto = true;
                        }
                        break;
                    case "Prodotto_NomeProdotto":
                        try
                        {
                            this._NomeProdotto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._NomeProdotto = true;
                        }
                        break;
                    case "Prodotto_NomeVariante":
                        try
                        {
                            this._NomeVariante = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._NomeVariante = true;
                        }
                        break;
                    case "Prodotto_Matricola":
                        try
                        {
                            this._Matricola = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Matricola = true;
                        }
                        break;
                    case "Prodotto_Status":
                        try
                        {
                            this._Status = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Status = true;
                        }
                        break;
                    case "Prodotto_Reparto":
                        try
                        {
                            this._Reparto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Reparto = true;
                        }
                        break;
                    case "Prodotto_DataPrevistaConsegna":
                        try
                        {
                            this._DataPrevistaConsegna = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._DataPrevistaConsegna = true;
                        }
                        break;
                    case "Prodotto_DataPrevistaFineProduzione":
                        try
                        {
                            this._DataPrevistaFineProduzione = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._DataPrevistaFineProduzione = true;
                        }
                        break;
                    case "Prodotto_EarlyStart":
                        try
                        {
                            this._EarlyStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._EarlyStart = true;
                        }
                        break;
                    case "Prodotto_EarlyFinish":
                        try
                        {
                            this._EarlyFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._EarlyFinish = true;
                        }
                        break;
                    case "Prodotto_LateStart":
                        try
                        {
                            this._LateStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._LateStart = true;
                        }
                        break;
                    case "Prodotto_LateFinish":
                        try
                        {
                            this._LateFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._LateFinish = true;
                        }
                        break;
                    case "Prodotto_Quantita":
                        try
                        {
                            this._Quantita = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Quantita = true;
                        }
                        break;
                    case "Prodotto_QuantitaProdotta":
                        try
                        {
                            this._QuantitaProdotta = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._QuantitaProdotta = true;
                        }
                        break;
                    case "Prodotto_Ritardo":
                        try
                        {
                            this._Ritardo = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Ritardo = true;
                        }
                        break;
                    case "Prodotto_TempoDiLavoroTotale":
                        try
                        {
                            this._TempoDiLavoroTotale = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._TempoDiLavoroTotale = true;
                        }
                        break;
                    case "Prodotto_LeadTime":
                        try
                        {
                            this._LeadTime = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._LeadTime = true;
                        }
                        break;
                    case "Prodotto_TempoDiLavoroPrevisto":
                        try
                        {
                            this._TempoDiLavoroPrevisto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._TempoDiLavoroPrevisto = true;
                        }
                        break;
                    case "Prodotto_IndicatoreCompletamentoTasks":
                        try
                        {
                            this._IndicatoreCompletamentoTasks = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IndicatoreCompletamentoTasks = true;
                        }
                        break;
                    case "Prodotto_IndicatoreCompletamentoTempoPrevisto":
                        try
                        {
                            this._IndicatoreCompletamentoTempoPrevisto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._IndicatoreCompletamentoTempoPrevisto = true;
                        }
                        break;
                    case "Prodotto_ViewGanttTasks":
                        try
                        {
                            this._ViewGanttTasks = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._ViewGanttTasks = true;
                        }
                        break;
                    case "Prodotto_ViewElencoTasks":
                        try
                        {
                            this._ViewElencoTasks = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._ViewElencoTasks = true;
                        }
                        break;
                    case "Task_ID":
                        try
                        {
                            this._Task_ID = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_ID = true;
                        }
                        break;
                    case "Task_Nome":
                        try
                        {
                            this._Task_Nome = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Nome = true;
                        }
                        break;
                    case "Task_Descrizione":
                        try
                        {
                            this._Task_Descrizione = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Descrizione = true;
                        }
                        break;
                    case "Task_Postazione":
                        try
                        {
                            this._Task_Postazione = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Postazione = true;
                        }
                        break;
                    case "Task_EarlyStart":
                        try
                        {
                            this._Task_EarlyStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_EarlyStart = true;
                        }
                        break;
                    case "Task_LateStart":
                        try
                        {
                            this._Task_LateStart = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_LateStart = true;
                        }
                        break;
                    case "Task_EarlyFinish":
                        try
                        {
                            this._Task_EarlyFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_EarlyFinish = true;
                        }
                        break;
                    case "Task_LateFinish":
                        try
                        {
                            this._Task_LateFinish = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_LateFinish = true;
                        }
                        break;
                    case "Task_NOperatori":
                        try
                        {
                            this._Task_NOperatori = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_NOperatori = true;
                        }
                        break;
                    case "Task_TempoCiclo":
                        try
                        {
                            this._Task_TempoCiclo = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_TempoCiclo = true;
                        }
                        break;
                    case "Task_TempoDiLavoroPrevisto":
                        try
                        {
                            this._Task_TempoDiLavoroPrevisto = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_TempoDiLavoroPrevisto = true;
                        }
                        break;
                    case "Task_TempoDiLavoroEffettivo":
                        try
                        {
                            this._Task_TempoDiLavoroEffettivo = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_TempoDiLavoroEffettivo = true;
                        }
                        break;
                    case "Task_Status":
                        try
                        {
                            this._Task_Status = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_Status = true;
                        }
                        break;
                    case "Task_QuantitaProdotta":
                        try
                        {
                            this._Task_QuantitaProdotta = Convert.ToBoolean(val);
                        }
                        catch
                        {
                            this._Task_QuantitaProdotta = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            conn.Close();
        }

        public Boolean DeleteConfiguration()
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            string sql = "DELETE FROM configurazione WHERE Sezione LIKE @sezione";
            try
            {
                conn.Execute(sql, new { sezione = "OrderStatusCustomerReport " + this.codCliente }, tr);
                tr.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                log = ex.Message;
                ret = false;
                tr.Rollback();
            }
            return ret;
        }
    }

    public class HomeBox
    {
        protected String Tenant;

        private class HomeBoxRow
        {
            public int idHomeBox { get; set; }
            public string nome { get; set; }
            public string descrizione { get; set; }
            public string path { get; set; }
        }

        private int _ID;
        public int ID
        {
            get
            { return this._ID; }
        }

        private String _Nome;
        public String Nome
        {
            get { return this._Nome; }
        }

        private String _Descrizione;
        public String Descrizione
        {
            get
            {
                return this._Descrizione;
            }
        }

        private String _Path;
        public String Path
        {
            get { return this._Path; }
        }

        public HomeBox(String Tenant, int boxId)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<HomeBoxRow>("SELECT idHomeBox, nome, descrizione, path FROM homeboxesregistro WHERE "
                + " idHomeBox = @p0", new { p0 = boxId });
            if (row != null)
            {
                this._ID = row.idHomeBox;
                this._Nome = row.nome;
                this._Descrizione = row.descrizione;
                this._Path = row.path;
            }
            else
            {
                this._ID = -1;
                this._Nome = "";
                this._Descrizione = "";
                this._Path = "";
            }
            conn.Close();
        }
    }

    public class HomeBoxesList
    {
        protected String Tenant;

        private List<HomeBox> _Elenco;
        public List<HomeBox> Elenco
        {
            get
            {
                return this._Elenco;
            }
        }

        public HomeBoxesList(String Tenant)
        {
            this.Tenant = Tenant;

            this._Elenco = new List<HomeBox>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var id in conn.Query<int>("SELECT idHomeBox FROM homeboxesregistro ORDER BY nome"))
            {
                this._Elenco.Add(new HomeBox(this.Tenant, id));
            }
            conn.Close();
        }
    }

    public class HomeBoxUser
    {
        protected String Tenant;

        public String log;

        private class HomeBoxUserRow
        {
            public int idHomeBox { get; set; }
            public string user { get; set; }
            public int ordine { get; set; }
        }

        private HomeBox _homeBox;
        public HomeBox homeBox
        {
            get
            { return this._homeBox; }
        }

        private User _user;
        public User user { get { return this._user; } }

        private int _ordine;
        public int ordine
        {
            get { return this._ordine; }
            set
            {
                if (this.homeBox != null && this.homeBox.ID != -1 && user!=null && user.username.Length>0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE homeboxesuser SET ordine = @p0 WHERE "
                        + " idHomeBox = @p1"
                        + " AND user = @p2";
                    try
                    {
                        conn.Execute(sql, new { p0 = value, p1 = this.homeBox.ID, p2 = user.username }, tr);
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

        public HomeBoxUser(String Tenant, User usr, HomeBox hBox)
        {
            this.Tenant = Tenant;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<HomeBoxUserRow>("SELECT idHomeBox, user, ordine FROM homeboxesuser WHERE "
                + " idHomeBox = @p0 AND user = @p1", new { p0 = hBox.ID, p1 = usr.username });
            if (row != null)
            {
                this._homeBox = new HomeBox(this.Tenant, row.idHomeBox);
                this._user = new User(row.user);
                this._ordine = row.ordine;
            }
            else
            {
                this._homeBox = null;
                this._user = null;
                this._ordine = -1;
            }
            conn.Close();
        }
    }

    public class HomeBoxesListUser
    {
        protected String Tenant;

        private List<HomeBoxUser> _Elenco;
        public List<HomeBoxUser> Elenco
        {
            get
            {
                return this._Elenco;
            }
        }

        public HomeBoxesListUser(String Tenant, User usr)
        {
            this.Tenant = Tenant;

            this._Elenco = new List<HomeBoxUser>();
            if(usr.username.Length>0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                conn.Open();
                foreach (var id in conn.Query<int>("SELECT idHomeBox FROM homeboxesuser WHERE user = @p0 ORDER BY ordine", new { p0 = usr.username }))
                {
                    this._Elenco.Add(new HomeBoxUser(this.Tenant, usr, new HomeBox(this.Tenant, id)));
                }
                conn.Close();
            }
        }
    }

    public class MeasurementUnit
    {
        protected String Tenant;

        private class MeasurementUnitRow
        {
            public int ID { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public bool IsDefault { get; set; }
        }

        private int _ID;
        public int ID
        {
            get { return this._ID; }
        }

        private String _Type;
        public String Type
        {
            get { return this._Type; ; }
            set
            {
                if(this.ID!=-1 && value.Length > 0)
                { 
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE measurementunits SET Type = @p0 WHERE ID = @p1";
                    try
                    {
                        conn.Execute(sql, new { p0 = value, p1 = this.ID }, tr);
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

        private String _Description;
        public String Description
        {
            get { return this._Description; ; }
            set
            {
                if (this.ID != -1 && value.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    string sql = "UPDATE measurementunits SET Description = @p0 WHERE ID = @p1";
                    try
                    {
                        conn.Execute(sql, new { p0 = value, p1 = this.ID }, tr);
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


        // There must be at least one default measurement unit!
        private Boolean _IsDefault;
        public Boolean IsDefault
        {
            get
            {
                return this._IsDefault;
            }
            set
            {
                if (this.ID != -1)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
                    conn.Open();
                    
                    int defUnit = 0;
                    if(value)
                    {
                        defUnit = this.ID;
                    }
                    else
                    {
                        defUnit = conn.ExecuteScalar<int>("SELECT ID FROM measurementunits ORDER BY ID");
                    }
                    MySqlTransaction tr = conn.BeginTransaction();
                    try
                    {
                        conn.Execute("UPDATE measurementunits SET isDefault = FALSE WHERE ID <> @p0", new { p0 = defUnit }, tr);
                        conn.Execute("UPDATE measurementunits SET isDefault = TRUE WHERE ID = @p0", new { p0 = defUnit }, tr);
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

        public MeasurementUnit(String Tenant)
        {
            this.Tenant = Tenant;

            this._ID = -1;
            this._Type = "";
            this._Description = "";
            this._IsDefault = false;
        }

        public MeasurementUnit(String Tenant, int uID)
        {
            this.Tenant = Tenant;

            this._ID = -1;
            this._Type = "";
            this._Description = "";
            this._IsDefault = false;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            var row = conn.QueryFirstOrDefault<MeasurementUnitRow>("SELECT ID, Type, Description, isdefault FROM measurementunits WHERE ID = @p0", new { p0 = uID });
            if (row != null)
            {
                this._ID = row.ID;
                this._Type = row.Type;
                this._Description = row.Description;
                this._IsDefault = row.IsDefault;
            }
            conn.Close();
        }
    }

    public class MeasurementUnits
    {
        protected String Tenant;

        public List<MeasurementUnit> UnitsList;
        public MeasurementUnits(String Tenant)
        {
            this.Tenant = Tenant;
            this.UnitsList = new List<MeasurementUnit>();
        }

        public void loadMeasurementUnits()
        {
            this.UnitsList = new List<MeasurementUnit>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            foreach (var id in conn.Query<int>("SELECT ID FROM measurementunits ORDER BY type ASC"))
            {
                this.UnitsList.Add(new MeasurementUnit(this.Tenant, id));
            }
            conn.Close();
        }

        public Boolean Add(String uType, String uDescription, Boolean uDefault)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            int? maxVal = conn.ExecuteScalar<int?>("SELECT MAX(ID) FROM measurementunits");
            int maxID = maxVal.HasValue ? maxVal.Value + 1 : 0;
            string sql = "INSERT INTO measurementunits(ID, Type, Description, isDefault) VALUES("
                + "@p0, "
                + "@p1, "
                + "@p2, "
                + " FALSE"
                +")";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { p0 = maxID, p1 = uType, p2 = uDescription }, tr);
                tr.Commit();
                ret = true;
            }
            catch
            {
                tr.Rollback();
                ret = false;
            }
            conn.Close();

            // if measurement unit was added correctly and it is the default measurement unit, set the default flag = true
            if(ret && uDefault)
            {
                    MeasurementUnit curr = new MeasurementUnit(this.Tenant, maxID);
                    curr.IsDefault = true;
            }
            return ret;
        }

        public Boolean Delete(int uID)
        {
            Boolean ret = false;
            MeasurementUnit currUnit = new MeasurementUnit(this.Tenant, uID);
            if(currUnit.ID!=-1)
            { 
            Boolean wasDefault = currUnit.IsDefault;

            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            string sql = "DELETE FROM measurementunits WHERE ID = @p0";
            MySqlTransaction tr = conn.BeginTransaction();
            try
            {
                conn.Execute(sql, new { p0 = uID }, tr);
                tr.Commit();
                ret = true;
            }
            catch
            {
                ret = false;
                tr.Rollback();
            }

            // Sets a new default value, if the deleted one was the default value
            if(ret && wasDefault)
            {
                this.loadMeasurementUnits();
                if(this.UnitsList.Count > 0)
                {
                    this.UnitsList[0].IsDefault = true;
                }
            }
            conn.Close();
            }
            return ret;
        }
    }
}