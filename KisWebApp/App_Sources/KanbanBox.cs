/* Copyright © 2013 Matteo Griso -  Tutti i diritti riservati */
/* Copyright © 2017 Matteo Griso -  Tutti i diritti riservati */

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Net.Mail;
using KIS.App_Sources;
//using KIS.Commesse;

namespace KIS.App_Code
{
    public class KanbanBoxDataSet
    {
        protected String Tenant;

        public String log;
        public Boolean KanbanBoxEnabled;
        public String x_api_key;
        public String Version;
        public String kBoxURL;

        public List<Reparto> Reparti;

        public List<Cliente> Clienti;

        public List<User> KanbanBoxManagers;

        public KanbanBoxDataSet(String Tenant)
        {
            this.Tenant = Tenant;

            this.Reparti = new List<Reparto>();
            this.Clienti = new List<Cliente>();
            this.KanbanBoxManagers = new List<User>();
        }

        public void loadKanbanBoxManagers()
        {
            this.KanbanBoxManagers = new List<User>();
            Permission prm = new Permission("KanbanBox GetWarnings");
            if (prm.ID != -1)
            {
                MySqlConnection conn = (new Dati.Dati()).VCMainConn();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT groupid FROM groupspermissions WHERE permissionid = " + prm.ID.ToString();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Group grp = new Group(rdr.GetInt32(0));
                    if (grp.ID != -1)
                    {
                        Workspace ws = new Workspace(this.Tenant);
                        grp.loadUtenti(ws.id);
                        for (int i = 0; i < grp.Utenti.Count; i++)
                        {
                            this.KanbanBoxManagers.Add(new User(grp.Utenti[i]));
                        }
                    }
                }
                rdr.Close();
                conn.Close();
            }
        }

        public List<MailAddress> kBoxManagersMails
        {
            get
            {
                List<MailAddress> lstMailAddr = new List<MailAddress>();
                for (int i = 0; i < KanbanBoxManagers.Count; i++)
                {
                    KanbanBoxManagers[i].loadEmails();
                    for (int j = 0; j < KanbanBoxManagers[i].Email.Count; j++)
                    {
                        Boolean valid = false;
                        MailAddress ml = null;
                        try
                        {
                            ml = new MailAddress(KanbanBoxManagers[i].Email[j].Email);
                            valid = true;
                        }
                        catch
                        {
                            valid = false;
                        }

                        if (valid == true)
                        {
                            lstMailAddr.Add(ml);
                        }
                    }
                }
                return lstMailAddr;
            }
        }

        public void loadReparti()
        {
            this.Reparti = new List<Reparto>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM configurazione WHERE "
                + " Sezione LIKE 'Reparto'"
                + " AND parametro LIKE 'KanbanManaged'"
                + " AND valore = 1";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Reparto rp = new Reparto(this.Tenant, rdr.GetInt32(0));
                if (rp!=null && rp.id != -1)
                {
                    this.Reparti.Add(rp);
                }
            }
            rdr.Close();
            conn.Close();
        }

        public void loadClienti()
        {
            this.Clienti = new List<Cliente>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT codice FROM anagraficaclienti WHERE kanbanManaged = true";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                this.Clienti.Add(new Cliente(this.Tenant, rdr.GetString(0)));
            }
            rdr.Close();
            conn.Close();
        }

        public List<String> checkNonExistentPartNumbers()
        {
            List<String> PNlist = new List<string>();
            KanbanCardList elenco = this.ReadCards();
            for (int i = 0; i < elenco.cards.Count; i++)
            {
                processo prc = new processo(elenco.cards[i].part_number);
                if (prc.processID == -1)
                {
                    PNlist.Add(elenco.cards[i].part_number);
                }
            }
            return PNlist;
        }

        public void LoadConfiguration()
        {
            this.KanbanBoxEnabled = false;
            try
            {
                KIS.App_Code.KanbanBoxConfig kboxCfg = (KIS.App_Code.KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                KanbanBoxEnabled = true;
                Version = kboxCfg.Version;
                x_api_key = kboxCfg.X_API_KEY;
                kBoxURL = kboxCfg.Url;
            }
            catch (Exception ex)
            {
                KanbanBoxEnabled = false;
                Version = "";
                x_api_key = "";
                kBoxURL = "";
            }

        }

        public KanbanCardList ReadCards()
        {
            log = "";
            KanbanCardList KanbanList = new KanbanCardList(this.Tenant);
            LoadConfiguration();
            if (KanbanBoxEnabled == true)
            {
                String urlParameters = "?status=released&limit=1000&fields=ekanban_string,kanban_quantity,part_number,"
                + "kanban_status_name,supplier_name,required_date,lead_time,customer_name,initial_empty_date";

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-API-KEY", x_api_key);
                client.BaseAddress = new Uri(kBoxURL);
                // List data response.                
                bool receivedCards = false;
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(urlParameters).Result;
                    receivedCards = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    receivedCards = false;
                }

                if (response != null && response.IsSuccessStatusCode && receivedCards == true)
                {
                    // Parse the response body. Blocking!
                    var cardList = response.Content.ReadAsAsync<KanbanCardList>().Result;

                    List<KanbanCard> kbList = cardList.cards.OrderBy(g => g.initial_empty_date).ToList();
                    for (int i = 0; i < kbList.Count; i++)
                    {
                        KanbanCard kbCard = new KanbanCard();
                        kbCard.ekanban_string = kbList[i].ekanban_string;
                        kbCard.part_number = kbList[i].part_number;
                        kbCard.kanban_quantity = kbList[i].kanban_quantity;
                        kbCard.kanban_status_name = kbList[i].kanban_status_name;
                        kbCard.supplier_name = kbList[i].supplier_name;
                        kbCard.lead_time = kbList[i].lead_time;
                        kbCard.customer_name = kbList[i].customer_name;
                        kbCard.initial_empty_date = kbList[i].initial_empty_date;
                        kbCard.required_date = kbList[i].required_date;

                        Cliente customer = new Cliente(this.Tenant, kbCard.customer_name);
                        Reparto rp = new Reparto(kbCard.supplier_name);

                        log += "Trovato cartellino " + kbCard.ekanban_string + "-" + kbCard.supplier_name +"-" + rp.id.ToString() + "-" + rp.KanbanManaged.ToString() + "\n";

                        if (customer.KanbanManaged == true || rp.KanbanManaged == true)
                        {
                            log += "ENTRO!";
                            KanbanList.cards.Add(kbCard);
                        }
                    }

                }
                else
                {
                    
                }
            }
            return KanbanList;
        }

        public KanbanCardList ReadCards(String status)
        {
            log = "";
            KanbanCardList KanbanList = new KanbanCardList(this.Tenant);
            LoadConfiguration();
            if (KanbanBoxEnabled == true)
            {
                String urlParameters = "?status="+status+"&limit=1000&fields=ekanban_string,kanban_quantity,part_number,"
                + "kanban_status_name,supplier_name,required_date,lead_time,customer_name,initial_empty_date";

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-API-KEY", x_api_key);
                client.BaseAddress = new Uri(kBoxURL);
                // List data response.                
                bool receivedCards = false;
                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(urlParameters).Result;
                    receivedCards = true;
                }
                catch (Exception ex)
                {
                    log = ex.Message;
                    receivedCards = false;
                }

                if (response != null && response.IsSuccessStatusCode && receivedCards == true)
                {
                    // Parse the response body. Blocking!
                    var cardList = response.Content.ReadAsAsync<KanbanCardList>().Result;

                    List<KanbanCard> kbList = cardList.cards.OrderBy(g => g.initial_empty_date).ToList();
                    for (int i = 0; i < kbList.Count; i++)
                    {
                        KanbanCard kbCard = new KanbanCard();
                        kbCard.ekanban_string = kbList[i].ekanban_string;
                        kbCard.part_number = kbList[i].part_number;
                        kbCard.kanban_quantity = kbList[i].kanban_quantity;
                        kbCard.kanban_status_name = kbList[i].kanban_status_name;
                        kbCard.supplier_name = kbList[i].supplier_name;
                        kbCard.lead_time = kbList[i].lead_time;
                        kbCard.customer_name = kbList[i].customer_name;
                        kbCard.initial_empty_date = kbList[i].initial_empty_date;
                        kbCard.required_date = kbList[i].required_date;

                        Cliente customer = new Cliente(this.Tenant, kbCard.customer_name);
                        Reparto rp = new Reparto(kbCard.supplier_name);

                        log += "Trovato cartellino " + kbCard.ekanban_string + "-" + kbCard.supplier_name + "-" + rp.id.ToString() + "-" + rp.KanbanManaged.ToString() + "\n";

                        if (customer.KanbanManaged == true || rp.KanbanManaged == true)
                        {
                            log += "ENTRO!";
                            KanbanList.cards.Add(kbCard);
                        }
                    }

                }
                else
                {

                }
            }
            return KanbanList;
        }

        public List<Articolo> checkGhostProducts()
        {
            List<Articolo> ghostProds = new List<Articolo>();
            KanbanCardList elenco = this.ReadCards();
            for (int i = 0; i < elenco.cards.Count; i++)
            {
                Articolo curr = new Articolo(this.Tenant, elenco.cards[i]);
                if (curr.ID!=-1 && curr.Status == 'N')
                {
                    ghostProds.Add(curr);
                }
            }
            return ghostProds;
        }

        public List<Articolo> checkNonUpdatedCards()
        {
            List<Articolo> nonUpdatedProds = new List<Articolo>();
            KanbanCardList elenco = this.ReadCards();
            for (int i = 0; i < elenco.cards.Count; i++)
            {
                Articolo curr = new Articolo(this.Tenant, elenco.cards[i]);
                if (curr.ID!=-1 && curr.KanbanCardID.Length > 0 && curr.Status != 'N')
                {
                    nonUpdatedProds.Add(curr);
                    if (curr.Status == 'F')
                    {
                        curr.changeKanbanBoxCardStatus("full");
                    }
                    else
                    {
                        curr.changeKanbanBoxCardStatus("process");
                    }
                }
            }

            elenco = new KanbanCardList(this.Tenant);
            elenco = this.ReadCards("process");
            for (int i = 0; i < elenco.cards.Count; i++)
            {
                Articolo curr = new Articolo(this.Tenant, elenco.cards[i]);
                if (curr.ID != -1 && curr.KanbanCardID.Length > 0 && curr.Status == 'F')
                {
                    nonUpdatedProds.Add(curr);
                    if (curr.Status == 'F')
                    {
                        curr.changeKanbanBoxCardStatus("full");
                    }
                    else
                    {
                        curr.changeKanbanBoxCardStatus("process");
                    }
                }
            }
            return nonUpdatedProds;
        }

        public Boolean ChangeStatus(KanbanCard card, String status)
        {
            Boolean ret = false;
            if (card.ekanban_string != "")
            {
                String url = kBoxURL + card.ekanban_string;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-API-KEY", x_api_key);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.BaseAddress = new Uri(url);

                // List data response.
                HttpResponseMessage response = null;
                HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("new_status",status)
                });


                try
                {
                    response = client.PutAsync(url, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        log = "Card: " + card.ekanban_string + " " + response.StatusCode + " " + response.RequestMessage + " \n";
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
            return ret;
        }

        public List<String> checkNonExistentCustomers()
        {
            List<String> nExCustomers = new List<String>();
            KanbanCardList elenco = this.ReadCards();
            for (int i = 0; i < elenco.cards.Count; i++)
            {
                Cliente curr = new Cliente(this.Tenant, elenco.cards[i].customer_name);
                if (curr.CodiceCliente.Length == 0)
                {
                    nExCustomers.Add(elenco.cards[i].customer_name);
                }
            }
            return nExCustomers;
        }

        public List<String> checkCardsConsistency()
        {
            log = "";
            List<String> ret = new List<String>() ;
            
            KanbanCardList elenco = this.ReadCards();
            for (int i = 0; i < elenco.cards.Count; i++)
            {
                log += "Card " + elenco.cards[i].ekanban_string + "\n";
                if (elenco.cards[i].part_number.Length > 0)
                {
                    processo proc = new processo(elenco.cards[i].part_number);
                    Reparto rp = new Reparto(elenco.cards[i].supplier_name);
                    if(proc.processID !=-1)
                    {
                        if(rp.id !=-1)
                        {
                            proc.loadVarianti();
                            if(proc.variantiProcesso.Count == 1)
                            {
                                int ckPERT = proc.checkConsistencyPERT(proc.variantiProcesso[0]);
                                if (ckPERT == 1)
                                {
                                    variante var = new variante(this.Tenant, proc.variantiProcesso[0].idVariante);
                                    ProcessoVariante procVar = new ProcessoVariante(this.Tenant, proc, var);
                                    procVar.loadReparto();
                                    procVar.process.loadFigli(procVar.variant);
                                    if (procVar != null && procVar.process != null && procVar.process.processID != -1
                                        && procVar.variant != null &&procVar.variant.idVariante != -1)
                                    {
                                        
                                        bool repFound = false;
                                        for (int j = 0; j < procVar.RepartiProduttivi.Count; j++)
                                        {
                                            if (procVar.RepartiProduttivi[j].id == rp.id)
                                            {
                                                repFound = true;
                                            }
                                        }

                                        if (repFound == true)
                                        {
                                            proc.loadFigli(var);
                                            for(int k = 0; k < proc.subProcessi.Count; k++)
                                            {
                                            TaskVariante tskVar = new TaskVariante(this.Tenant, proc.subProcessi[k], var);
                                            if (tskVar != null)
                                            {
                                                Postazione pstCheck = tskVar.CercaPostazione(rp);
                                                if(pstCheck.id==-1)
                                                {
                                                    ret.Add("Nel processo " + proc.processName + ", il task " + proc.subProcessi[k].processName + " non è assegnato a nessuna postazione del reparto " + rp.name + "<br />");
                                                }
                                            }
                                            else
                                            {
                                                ret.Add("Processo " + proc.processName + " ha problemi con il task " + proc.subProcessi[k].processName + "<br />");
                                            }
                                            }
                                        }
                                        else
                                        {
                                            ret.Add("Il prodotto " + proc.processName + " non è assegnato al reparto " + rp.name + "<br />");
                                        }
                                    }
                                    else
                                    {
                                        ret.Add("Processo " + proc.processName + " ha problemi con la variante " + proc.variantiProcesso[0].nomeVariante + "<br />");
                                    }
                                }
                                else
                                {
                                    if (ckPERT == 2)
                                    {
                                        ret.Add("Nel prodotto " + proc.processName + " esiste qualche task senza vincoli di precedenza.<br />");
                                    }
                                    else if(ckPERT == 3)
                                    {
                                        ret.Add("Il prodotto " + proc.processName + " non ha un PERT.<br />");
                                    }
                                    else if(ckPERT == 5)
                                    {
                                        ret.Add("A qualche task del prodotto " + proc.processName + " manca il tempo ciclo.<br />");
                                    }
                                    else if (ckPERT == 6)
                                    {
                                        ret.Add("A qualche task del prodotto " + proc.processName + " non è stata assegnata una postazione di lavoro.<br />");
                                    }
                                    else
                                    {
                                        ret.Add("Generic error " + ckPERT.ToString() + "<br />");
                                    }
                                }
                            }
                            else
                            {
                                ret.Add("Il prodotto " + proc.processName + " deve avere una ed una sola variante.<br />");
                            }
                        }
                        else
                        {
                            ret.Add("Il reparto " + elenco.cards[i].supplier_name + " non esiste su KIS.<br />");
                        }
                    }
                    else
                    {
                        ret.Add("Il prodotto " + elenco.cards[i].part_number + " non esiste su KIS.<br />");
                    }
                }
                else
                {
                    ret.Add("Nessun codice prodotto definito per il cartellino " + elenco.cards[i].ekanban_string + "<br />");
                }
            }
            return ret;
        }
    }

    public class KanbanCard
    {
        protected String Tenant;
        public String ekanban_string { get; set; }
        public String kanban_quantity { get; set; }
        public String part_number { get; set; }
        public String kanban_status_name { get; set; }
        public String supplier_name { get; set; }
        public String required_date { get; set; }
        public String lead_time { get; set; }
        public String customer_name { get; set; }
        public String initial_empty_date { get; set; }

        public DateTime DataConsegna
        {
            get
            {
                FusoOrario fuso = new FusoOrario(this.Tenant);
                DateTime data_consegna = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                Double consegna_timestamp = Double.Parse(this.required_date);
                data_consegna = ((TimeZoneInfo.ConvertTimeFromUtc(data_consegna, fuso.tzFusoOrario))).AddSeconds(consegna_timestamp);
                return data_consegna;
            }
        }
        public int Quantita
        {
            get
            {
                int qty = 0;
                try
                {
                    qty = Int32.Parse(kanban_quantity);
                }
                catch
                {
                    qty = 0;
                }
                return qty;
            }
        }

        public String log;

        /*Returns:
         * 0 if ekanban_string is ""
         * 2 if no part_number is defined
         * 3 if no process found
         * 4 if no Reparto found
         * 5 if there are no varianti or there is more than 1 variante
         * 6 if PERT is not consistent
         * 7 if troubles with ProcessoVariante
         * 8 se il processo non è associato al reparto che richiede KanbanBox
         * 9 se generici problemi di un task figlio
         * 10 se il task figlio non è associato a nessuna postazione, per il reparto richiesto
         */
        public int checkCardsConsistency()
        {
            log = "";
            int ret = 1;

            if (this.ekanban_string.Length > 0)
            {
                log += "Card " + ekanban_string + "\n";
                if (part_number.Length > 0)
                {
                    processo proc = new processo(part_number);
                    Reparto rp = new Reparto(supplier_name);
                    if (proc.processID != -1)
                    {
                        if (rp.id != -1)
                        {
                            proc.loadVarianti();
                            if (proc.variantiProcesso.Count == 1)
                            {
                                int ckPERT = proc.checkConsistencyPERT(proc.variantiProcesso[0]);
                                if (ckPERT == 1)
                                {
                                    variante var = new variante(this.Tenant, proc.variantiProcesso[0].idVariante);
                                    ProcessoVariante procVar = new ProcessoVariante(this.Tenant, proc, var);
                                    procVar.loadReparto();
                                    procVar.process.loadFigli(procVar.variant);
                                    if (procVar != null && procVar.process != null && procVar.process.processID != -1
                                        && procVar.variant != null && procVar.variant.idVariante != -1)
                                    {
                                        bool repFound = false;
                                        for (int j = 0; j < procVar.RepartiProduttivi.Count; j++)
                                        {
                                            if (procVar.RepartiProduttivi[j].id == rp.id)
                                            {
                                                repFound = true;
                                            }
                                        }

                                        if (repFound == true)
                                        {
                                            proc.loadFigli(var);
                                            for (int k = 0; k < proc.subProcessi.Count; k++)
                                            {
                                                TaskVariante tskVar = new TaskVariante(this.Tenant, proc.subProcessi[k], var);
                                                if (tskVar != null)
                                                {
                                                    Postazione pstCheck = tskVar.CercaPostazione(rp);
                                                    if (pstCheck.id == -1)
                                                    {
                                                        ret=10;
                                                    }
                                                }
                                                else
                                                {
                                                    ret=9;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ret=8;
                                        }
                                    }
                                    else
                                    {
                                        ret=7;
                                    }
                                }
                                else
                                {
                                        ret = 6;
                                }
                            }
                            else
                            {
                                ret=5;
                            }
                        }
                        else
                        {
                            ret=4;
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
            }
            else
            {
                ret = 0;
            }
            return ret;
        }
    }

    public class KanbanCardList
    {
        protected String Tenant;
        public List<KanbanCard> cards { get; set; }
        public KanbanCardList(String Tenant)
        {
            this.Tenant = Tenant;

            this.cards = new List<KanbanCard>();
        }
    }

    public class RepartiKanban
    {
        protected String Tenant;

        private List<Reparto> _ElencoReparti;
        public List<Reparto> ElencoReparti
        {
            get
            {
                return this._ElencoReparti;
            }
        }

        public RepartiKanban(String Tenant)
        {
            this.Tenant = Tenant;

            this._ElencoReparti = new List<Reparto>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID FROM configurazione WHERE Sezione LIKE 'Reparto' AND paramentro LIKE 'KanbanManaged' "
                + " AND valore = 1";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (!rdr.Read())
            {
                if (!rdr.IsDBNull(0))
                {
                    this._ElencoReparti.Add(new Reparto(this.Tenant, rdr.GetInt32(0)));
                }
            }
            rdr.Close();
            conn.Close();
        }
    }

    public class ClientiKanban
    {
        protected String Tenant;

        private List<Cliente> _ElencoClienti;
        public List<Cliente> ElencoClienti
        {
            get
            {
                return this._ElencoClienti;
            }
        }

        public ClientiKanban(String Tenant)
        {
            this.Tenant = Tenant;

            this._ElencoClienti = new List<Cliente>();
            MySqlConnection conn = (new Dati.Dati()).mycon(this.Tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT codice FROM anagraficaclienti WHERE kanbanManaged = true ORDER BY ragsociale";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (!rdr.IsDBNull(0))
                {
                    this._ElencoClienti.Add(new Cliente(this.Tenant, rdr.GetString(0)));
                }
            }
            rdr.Close();
            conn.Close();
        }
    }
}