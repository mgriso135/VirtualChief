/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace KIS.App_Code
{
    public class KISConfig
    {
        public Boolean WizLogoCompleted
        {
            get
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
        }

        public Boolean WizRepartiCompleted
        {
            get
            {
                ElencoReparti elRep = new ElencoReparti();
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
                ElencoPostazioni elPost = new ElencoPostazioni();
                return elPost.elenco.Count > 0;
            }
        }

        public Boolean WizUsersCompleted
        {
            get
            {
                UserList usrList = new UserList();
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
        
        public Boolean WizAdminUserCompleted
        {
            get
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT groupss.id, groupusers.user FROM groupss INNER JOIN groupusers ON("
                    + "groupss.id = groupusers.groupid) WHERE nomegruppo LIKE 'Admin'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
        }

        public Boolean WizTimezoneCompleted
        {
            get
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM configurazione WHERE parametro LIKE 'TimeZone'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
        }

        public Boolean WizAndonCompleted {
            get
            {
                bool ret = false;
                AndonCompleto andon = new AndonCompleto();
                andon.loadCampiVisualizzati();
                andon.loadCampiVisualizzatiTasks();
                ret = (andon.CampiVisualizzati.Count > 0 && andon.CampiVisualizzatiTasks.Count > 0);
                return ret;   
            }
        }

        public Boolean WizCustomerReportCompleted {
            get {
                Boolean ret = false;
                configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT sezione, ID, parametro, valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'ExpiryDate'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    try
                    {
                        String[] aExp = rdr.GetString(3).Split('/');
                        int anno = Int32.Parse(aExp[2]);
                        int mese = Int32.Parse(aExp[1]);
                        int giorno = Int32.Parse(aExp[0]);
                        FusoOrario fuso = new FusoOrario();
                        exp = new DateTime(anno, mese, giorno);
                        exp = TimeZoneInfo.ConvertTimeFromUtc(exp, fuso.tzFusoOrario);
                    }
                    catch
                    {
                        exp = new DateTime(1970, 1, 1);
                    }
                    rdr.Close();
                    conn.Close();
                }
                return exp;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                String expDate = value.Day.ToString() + "/"
                    + value.Month.ToString() + "/"
                    + value.Year.ToString();

                // Controllo se esiste già il parametro
                cmd.CommandText = "SELECT sezione, ID, parametro, valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'ExpiryDate'";
                MySqlDataReader rdr = cmd.ExecuteReader();

                bool exists = false;

                exists = (rdr.Read() && !rdr.IsDBNull(0));
                    rdr.Close();
                if(exists)
                {
                    cmd.CommandText = "UPDATE configurazione SET parametro = '"+expDate+"' WHERE "
                        + "Sezione = 'Main' AND parametro = 'ExpiryDate'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Main', -1, 'ExpiryDate', '"+ expDate + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
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

        public String Language
        {
            get
            {
                String ret = "en";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'Language'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    try
                    {
                        ret = rdr.GetString(0);
                    }
                    catch
                    {
                        ret = "en";
                    }
                    rdr.Close();
                    conn.Close();
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
        }

        public String baseUrl
        {
            get
            {
                String ret = "";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'BaseUrl'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    try
                    {
                        ret = rdr.GetString(0);
                    }
                    catch
                    {
                        ret = "";
                    }
                    rdr.Close();
                    conn.Close();
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
        }

        public String basePath
        {
            get
            {
                String ret = "";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'BasePath'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    try
                    {
                        ret = rdr.GetString(0);
                    }
                    catch
                    {
                        ret = "";
                    }
                    rdr.Close();
                    conn.Close();
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
        }

        public String ConfigController_X_API_KEY
        {
            get
            {
                String ret = "";
                MySqlConnection conn = (new Dati.Dati().mycon());
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'ConfigController' AND parametro"
                    + " LIKE 'X-API-KEY'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = rdr.GetString(0);
                }
                rdr.Close();
                conn.Close();
                return ret;
         }
            }

        public Boolean SalesOrderImportFrom3PartySystem
        {
            get
            {
                Boolean ret = false;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'SalesOrderImportFrom3PartySystemEnabled'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = rdr.GetBoolean(0);
                }
                else
                {
                    ret = false;
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                // Controllo se esiste già il parametro
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione='Main' AND "
                    + "parametro = 'SalesOrderImportFrom3PartySystemEnabled'";
                MySqlDataReader rdr = cmd.ExecuteReader();

                bool exists = false;
                exists = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (exists)
                {
                    cmd.CommandText = "UPDATE configurazione SET `valore` = '"+value.ToString()+"' WHERE(`Sezione` = 'Main') and(`ID` = '-1') and(`parametro` = 'SalesOrderImportFrom3PartySystemEnabled')";
                    ;
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Main', -1, 'SalesOrderImportFrom3PartySystemEnabled', '" + value + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
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

    public class Logo
    {
        public String log;
        public String filePath
        {
            get
            {
                String percorsoLogo = "~/Data/Logo/LogoMG.png";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        percorsoLogo = "~/Data/Logo/" + rdr.GetString(0);
                    }
                rdr.Close();
                conn.Close();
                return percorsoLogo;
            }
            set
            {
                log = "Carico... ";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                bool found = false;
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
                rdr.Close();

                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                if (found == true)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString()
                        + "' WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Main', 0, 'Logo', '"
                        + value + "')";
                }

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

        public String filePathAbs
        {
            get
            {
                String percorsoLogo = "/Data/Logo/LogoMG.png";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    percorsoLogo = "/Data/Logo/" + rdr.GetString(0);
                }
                rdr.Close();
                conn.Close();
                return percorsoLogo;
            }
            set
            {
                log = "Carico... ";
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                bool found = false;
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
                rdr.Close();

                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;

                if (found == true)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString()
                        + "' WHERE Sezione = 'Main' AND ID = 0 AND parametro = 'Logo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Main', 0, 'Logo', '"
                        + value + "')";
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

                conn.Close();
            }
        }

        public Logo()
        {
        }
    }

    public class FusoOrario
    {
        public String log;
        private String _fusoOrario;
        public String fusoOrario
        {
            get { return this._fusoOrario; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                Boolean res = false;
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione ='Main' AND ID=-1 AND parametro='TimeZone'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
                rdr.Close();
                if (res)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '"
                        + value + "' WHERE Sezione ='Main' AND ID =-1 AND parametro = 'TimeZone'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Main', "
                        + "-1, "
                        + "'TimeZone', "
                        + "'" + value.ToString() + "'"
                        +")";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    log = cmd.CommandText + " <br/>" + ex.Message;
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

        public FusoOrario()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione ='Main' AND ID=-1 AND parametro='TimeZone'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._fusoOrario = rdr.GetString(0);
            }
            else
            {
                this._fusoOrario = "W. Europe Standard Time";
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class KanbanBoxConfig : ConfigurationSection
    {
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
        public String log;

        public String interfacciaPERT
        {
            get
            {
                String ret;
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Wizard' and parametro LIKE 'TipoPERT'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    ret = rdr.GetString(0);
                }
                else
                {
                    ret = "Graph";
                }
                rdr.Close();
                conn.Close();
                return ret;
            }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT valore FROM configurazione WHERE Sezione LIKE 'Wizard' and parametro LIKE 'TipoPERT'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                String sqlString = "";
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    sqlString = "UPDATE configurazione SET valore = '" + value + "' WHERE "
                        + "Sezione LIKE 'Wizard' and parametro LIKE 'TipoPERT'";
                }
                else
                {
                    sqlString = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'Wizard', -1, 'TipoPERT', '" + value + "')";
                }
                rdr.Close();
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                cmd.CommandText = sqlString;
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
    }

    public class CustomersControllerConfig : ConfigurationSection
    {
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valore FROM configurazione WHERE "
                + "Sezione LIKE 'CustomersController'"
                + " AND parametro LIKE 'X-API-KEY'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this.x_api_key = rdr.GetString(0);
            }
            else
            {
                this.x_api_key = "";
            }
            rdr.Close();
            conn.Close();
        }
    }


    public class EventsExportControllerConfig : ConfigurationSection
    {
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valore FROM configurazione WHERE "
                + "Sezione LIKE 'CustomersController'"
                + " AND parametro LIKE 'EVENTSEXPORT-API-KEY'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this.x_api_key = rdr.GetString(0);
            }
            else
            {
                this.x_api_key = "";
            }
            rdr.Close();
            conn.Close();
        }
    }


    public class configBaseOrderStatusReport
    {
        public String log;

        protected Boolean _IDCommessa;
        public Boolean IDCommessa
        {
            get { return this._IDCommessa; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_IDCommessa'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '"+value.ToString()+"' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_IDCommessa'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        +"'OrderStatusCustomerReport base', -1, 'Commessa_IDCommessa', '"+value.ToString()+"')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IDCommessa = value;
                }
                catch(Exception ex)
                {
                    log = ex.Message +" " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_Cliente'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_Cliente'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Commessa_Cliente', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Cliente = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_DataInserimento'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_DataInserimento'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Commessa_DataInserimento', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._DataInserimentoOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Commessa_Note'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Commessa_Note'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Commessa_Note', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NoteOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_IDProdotto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_IDProdotto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_IDProdotto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IDProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_NomeProdotto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_NomeProdotto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_NomeProdotto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NomeProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_NomeVariante'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_NomeVariante'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_NomeVariante', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Matricola'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Matricola'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Matricola', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Status'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Status'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Status', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Reparto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Reparto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Reparto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Reparto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_DataPrevistaConsegna', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._DataPrevistaConsegna = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_DataPrevistaFineProduzione', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._DataPrevistaFineProduzione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_EarlyStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_EarlyStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_EarlyStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_EarlyFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_EarlyFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_EarlyFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_LateStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_LateStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_LateStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_LateFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_LateFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_LateFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Quantita'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Quantita'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Quantita', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Quantita = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_QuantitaProdotta'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_QuantitaProdotta'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_QuantitaProdotta', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_Ritardo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_Ritardo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_Ritardo', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Ritardo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_TempoDiLavoroTotale', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._TempoDiLavoroTotale = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_LeadTime'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_LeadTime'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_LeadTime', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LeadTime = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_TempoDiLavoroPrevisto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_IndicatoreCompletamentoTasks', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IndicatoreCompletamentoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_IndicatoreCompletamentoTempoPrevisto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IndicatoreCompletamentoTempoPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_ViewGanttTasks'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_ViewGanttTasks'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_ViewGanttTasks', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ViewGanttTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Prodotto_ViewElencoTasks'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Prodotto_ViewElencoTasks'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Prodotto_ViewElencoTasks', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ViewElencoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_ID'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_ID'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_ID', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_ID = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Nome'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Nome'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Nome', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Nome = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Descrizione'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Descrizione'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Descrizione', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Descrizione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Postazione'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Postazione'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Postazione', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Postazione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_EarlyStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_EarlyStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_EarlyStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_LateStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_LateStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_LateStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_EarlyFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_EarlyFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_EarlyFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_LateFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_LateFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_LateFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_NOperatori'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_NOperatori'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_NOperatori', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_NOperatori = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_TempoCiclo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_TempoCiclo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_TempoCiclo', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_TempoCiclo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_TempoDiLavoroPrevisto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_TempoDiLavoroEffettivo', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_TempoDiLavoroEffettivo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_Status'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_Status'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_Status', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport base' "
                + "AND parametro LIKE 'Task_QuantitaProdotta'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport base' "
                        + "AND parametro LIKE 'Task_QuantitaProdotta'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport base', -1, 'Task_QuantitaProdotta', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public configBaseOrderStatusReport()
        {
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

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE Sezione LIKE 'OrderStatusCustomerReport base'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                String param = rdr.GetString(0);
                String val = rdr.GetString(1);
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
        public String codCliente;

        public new Boolean IDCommessa
        {
            get { return this._IDCommessa; }
            set
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Commessa_IDCommessa'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Commessa_IDCommessa'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Commessa_IDCommessa', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IDCommessa = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Commessa_Cliente'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Commessa_Cliente'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Commessa_Cliente', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Cliente = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Commessa_DataInserimento'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Commessa_DataInserimento'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Commessa_DataInserimento', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._DataInserimentoOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Commessa_Note'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Commessa_Note'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Commessa_Note', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NoteOrdine = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_IDProdotto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_IDProdotto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_IDProdotto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IDProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_NomeProdotto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_NomeProdotto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_NomeProdotto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NomeProdotto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_NomeVariante'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_NomeVariante'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_NomeVariante', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_Matricola'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_Matricola'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_Matricola', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._NomeVariante = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_Status'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_Status'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_Status', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_Reparto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_Reparto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_Reparto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Reparto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaConsegna'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_DataPrevistaConsegna', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._DataPrevistaConsegna = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_DataPrevistaFineProduzione'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_DataPrevistaFineProduzione', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._DataPrevistaFineProduzione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_EarlyStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_EarlyStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_EarlyStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_EarlyFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_EarlyFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_EarlyFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_LateStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_LateStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_LateStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_LateFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_LateFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_LateFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_Quantita'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_Quantita'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_Quantita', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Quantita = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_QuantitaProdotta'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_QuantitaProdotta'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_QuantitaProdotta', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_Ritardo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_Ritardo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_Ritardo', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Ritardo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroTotale'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_TempoDiLavoroTotale', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._TempoDiLavoroTotale = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_LeadTime'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_LeadTime'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_LeadTime', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._LeadTime = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_TempoDiLavoroPrevisto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_TempoDiLavoroPrevisto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTasks'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_IndicatoreCompletamentoTasks', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IndicatoreCompletamentoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_IndicatoreCompletamentoTempoPrevisto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_IndicatoreCompletamentoTempoPrevisto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._IndicatoreCompletamentoTempoPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_ViewGanttTasks'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_ViewGanttTasks'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_ViewGanttTasks', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ViewGanttTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Prodotto_ViewElencoTasks'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Prodotto_ViewElencoTasks'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Prodotto_ViewElencoTasks', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._ViewElencoTasks = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_ID'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_ID'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_ID', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_ID = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_Nome'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_Nome'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_Nome', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Nome = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_Descrizione'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_Descrizione'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_Descrizione', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Descrizione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_Postazione'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_Postazione'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_Postazione', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Postazione = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_EarlyStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_EarlyStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_EarlyStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_EarlyStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_LateStart'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_LateStart'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_LateStart', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_LateStart = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_EarlyFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_EarlyFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_EarlyFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_EarlyFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_LateFinish'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_LateFinish'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_LateFinish', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_LateFinish = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_NOperatori'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_NOperatori'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_NOperatori', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_NOperatori = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_TempoCiclo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_TempoCiclo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_TempoCiclo', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_TempoCiclo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_TempoDiLavoroPrevisto'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_TempoDiLavoroPrevisto', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_TempoDiLavoroPrevisto = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_TempoDiLavoroEffettivo'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_TempoDiLavoroEffettivo', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_TempoDiLavoroEffettivo = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_Status'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_Status'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_Status', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_Status = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
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
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE "
                + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                + "AND parametro LIKE 'Task_QuantitaProdotta'";
                MySqlDataReader rdr = cmd.ExecuteReader();
                bool check = false;
                check = (rdr.Read() && !rdr.IsDBNull(0));
                rdr.Close();
                if (check)
                {
                    cmd.CommandText = "UPDATE configurazione SET valore = '" + value.ToString() + "' WHERE "
                        + "Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "' "
                        + "AND parametro LIKE 'Task_QuantitaProdotta'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES("
                        + "'OrderStatusCustomerReport " + this.codCliente + "', -1, 'Task_QuantitaProdotta', '" + value.ToString() + "')";
                }
                MySqlTransaction tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                try
                {
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    this._Task_QuantitaProdotta = value;
                }
                catch (Exception ex)
                {
                    log = ex.Message + " " + cmd.CommandText;
                    tr.Rollback();
                }
                conn.Close();
            }
        }

        public configCustomerOrderStatusReport(String idCliente) : base()
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT parametro, valore FROM configurazione WHERE Sezione LIKE 'OrderStatusCustomerReport "+idCliente+"'";
            this.codCliente = idCliente;
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                String param = rdr.GetString(0);
                String val = rdr.GetString(1);
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
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlTransaction tr = conn.BeginTransaction();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = tr;
            cmd.CommandText = "DELETE FROM configurazione WHERE Sezione LIKE 'OrderStatusCustomerReport " + this.codCliente + "'";
            try
            {
                cmd.ExecuteNonQuery();
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

        public HomeBox(int boxId)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idHomeBox, nome, descrizione, path FROM homeboxesregistro WHERE "
                + " idHomeBox = " + boxId.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = rdr.GetInt32(0);
                this._Nome = rdr.GetString(1);
                this._Descrizione = rdr.GetString(2);
                this._Path = rdr.GetString(3);
            }
            else
            {
                this._ID = -1;
                this._Nome = "";
                this._Descrizione = "";
                this._Path = "";
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class HomeBoxesList
    {
        private List<HomeBox> _Elenco;
        public List<HomeBox> Elenco
        {
            get
            {
                return this._Elenco;
            }
        }

        public HomeBoxesList()
        {
            this._Elenco = new List<HomeBox>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idHomeBox FROM homeboxesregistro ORDER BY nome";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this._Elenco.Add(new HomeBox(rdr.GetInt32(0)));
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class HomeBoxUser
    {
        public String log;

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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE homeboxesuser SET ordine = " + value + " WHERE "
                        + " idHomeBox = " + this.homeBox.ID.ToString()
                        + " AND user = '" + user.username + "'";
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

        public HomeBoxUser(User usr, HomeBox hBox)
        {
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT idHomeBox, user, ordine FROM homeboxesuser WHERE "
                + " idHomeBox = " + hBox.ID.ToString() + " AND user = '"+usr.username+"'";
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read() && !rdr.IsDBNull(0))
            {
                this._homeBox = new HomeBox(rdr.GetInt32(0));
                this._user = new User(rdr.GetString(1));
                this._ordine = rdr.GetInt32(2);
            }
            else
            {
                this._homeBox = null;
                this._user = null;
                this._ordine = -1;
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class HomeBoxesListUser
    {
        private List<HomeBoxUser> _Elenco;
        public List<HomeBoxUser> Elenco
        {
            get
            {
                return this._Elenco;
            }
        }

        public HomeBoxesListUser(User usr)
        {
            this._Elenco = new List<HomeBoxUser>();
            if(usr.username.Length>0)
            {
                MySqlConnection conn = (new Dati.Dati()).mycon();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT idHomeBox FROM homeboxesuser WHERE user = '"+usr.username+"' ORDER BY ordine";
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    this._Elenco.Add(new HomeBoxUser(usr, new HomeBox(rdr.GetInt32(0))));
                }
                rdr.Close();
                conn.Close();
            }
        }
    }

    public class MeasurementUnit
    {
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE measurementunits SET Type = '" + value + "' WHERE ID = " + this.ID.ToString();
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

        private String _Description;
        public String Description
        {
            get { return this._Description; ; }
            set
            {
                if (this.ID != -1 && value.Length > 0)
                {
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    cmd.CommandText = "UPDATE measurementunits SET Description = '" + value + "' WHERE ID = " + this.ID.ToString();
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
                    MySqlConnection conn = (new Dati.Dati()).mycon();
                    conn.Open();
                    
                    int defUnit = 0;
                    if(value)
                    {
                        defUnit = this.ID;
                    }
                    else
                    {
                        MySqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandText = "SELECT ID FROM measurementunits ORDER BY ID";
                        MySqlDataReader rdr = cmd2.ExecuteReader();
                        if(rdr.Read() && !rdr.IsDBNull(0))
                        {
                            defUnit = rdr.GetInt32(0);
                        }
                        rdr.Close();
                    }
                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlTransaction tr = conn.BeginTransaction();
                    cmd.Transaction = tr;
                    try
                    {
                            cmd.CommandText = "UPDATE measurementunits SET isDefault = FALSE WHERE ID <> " + defUnit.ToString();
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "UPDATE measurementunits SET isDefault = TRUE WHERE ID = " + defUnit.ToString();
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

        public MeasurementUnit()
        {
            this._ID = -1;
            this._Type = "";
            this._Description = "";
            this._IsDefault = false;
        }

        public MeasurementUnit(int uID)
        {
            this._ID = -1;
            this._Type = "";
            this._Description = "";
            this._IsDefault = false;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Type, Description, isdefault FROM measurementunits WHERE ID = " + uID.ToString();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                this._ID = rdr.GetInt32(0);
                this._Type = rdr.GetString(1);
                this._Description = rdr.GetString(2);
                this._IsDefault = rdr.GetBoolean(3);
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class MeasurementUnits
    {
        public List<MeasurementUnit> UnitsList;
        public MeasurementUnits()
        {
            this.UnitsList = new List<MeasurementUnit>();
        }

        public void loadMeasurementUnits()
        {
            this.UnitsList = new List<MeasurementUnit>();
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM measurementunits ORDER BY type ASC";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.UnitsList.Add(new MeasurementUnit(rdr.GetInt32(0)));
            }
            conn.Close();
        }

        public Boolean Add(String uType, String uDescription, Boolean uDefault)
        {
            Boolean ret = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(ID) FROM measurementunits";
            int maxID = 0;
            MySqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read() && !rdr.IsDBNull(0))
            {
                maxID = rdr.GetInt32(0) + 1;
            }
            rdr.Close();
            cmd.CommandText = "INSERT INTO measurementunits(ID, Type, Description, isDefault) VALUES("
                + maxID.ToString() +", "
                + "'" + uType + "', "
                +"'" + uDescription + "', "
                + " FALSE"
                +")";
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            try
            {
                cmd.ExecuteNonQuery();
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
                    MeasurementUnit curr = new MeasurementUnit(maxID);
                    curr.IsDefault = true;
            }
            return ret;
        }

        public Boolean Delete(int uID)
        {
            Boolean ret = false;
            MeasurementUnit currUnit = new MeasurementUnit(uID);
            if(currUnit.ID!=-1)
            { 
            Boolean wasDefault = currUnit.IsDefault;

            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM measurementunits WHERE ID = " + uID.ToString();
            MySqlTransaction tr = conn.BeginTransaction();
            cmd.Transaction = tr;
            try
            {
                cmd.ExecuteNonQuery();
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