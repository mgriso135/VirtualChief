﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using KIS.App_Code;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using KIS.Commesse;

namespace KIS.KanbanBox
{
    /// <summary>
    /// Descrizione di riepilogo per KanbanBoxReader
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class KanbanBoxReader : System.Web.Services.WebService
    {
        public String log;
        public Boolean KanbanBoxEnabled;
        public String x_api_key;
        public String Version;
        public String kBoxURL;
        public KanbanCardList KanbanList;

        public void LoadConfiguration(String tenant)
        {
            try
            {
                KIS.App_Code.KanbanBoxConfig kboxCfg = (KIS.App_Code.KanbanBoxConfig)System.Configuration.ConfigurationManager.GetSection("kanbanBox");
                KanbanBoxEnabled = kboxCfg.KanbanBoxEnabled;
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

        public String ReadCards(String tenant)
        {
            KanbanList = new KanbanCardList(tenant);
            String ret = "";
            LoadConfiguration(tenant);
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
                catch(Exception ex)
                {
                    ret = ex.Message;
                    receivedCards = false;
                }
                
                if (response!=null && response.IsSuccessStatusCode && receivedCards == true)
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

                            Cliente customer = new Cliente(tenant, kbCard.customer_name);
                            Reparto rp = new Reparto(kbCard.supplier_name);

                            if (customer.KanbanManaged == true || rp.KanbanManaged == true)
                            {
                                KanbanList.cards.Add(kbCard);

                                ret += kbCard.part_number + "-"
                                        + kbCard.ekanban_string.ToString() + "-"
                                        + kbCard.kanban_status_name + "-"
                                        + kbCard.customer_name + "-"
                                        + kbCard.DataConsegna.ToString("dd/MM/yyyy HH:mm:ss")
                                        + "\n";
                            }
                        }
                    
                }
                else
                {
                    ret += "KO";
                }
            }
            return ret;
        }

        public Boolean ChangeStatus(String tenant, KanbanCard card, String status)
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
                catch(Exception ex)
                {
                    ret = false;
                    log = ex.Message;
                }
                
            }
            return ret;
        }

        /* Returns:
         * 0 if generic error
         * 1 if ok
         * 2 if customer does not exist in Kaizen Indicator System
         * 3 if failed to create Commessa
         * 4 if ambiguous process: numero di varianti > 1
         * 5 if required date is earlier than now
         * 6 if processoVariante is not consistent
         * 7 if not possible to add the articolo to commessa
         * 8 if Reparto supplier does not exist!
         * 9 if Articolo where already launch and it is a 'N' status inside KIS
         * 10 if KanbanCard is not consistent
         */
        public int AddToProductionQue(String tenant, KanbanCard card)
        {
            int ret = 1;
            int procID = -1;
            int qty = -1;
            
            ElencoCommesse elComm = new ElencoCommesse(Session["ActiveWorkspace_Name"].ToString());
            elComm.loadCommesse();
            Cliente customer = new Cliente(tenant, card.customer_name);
            processo prc = new processo(card.part_number);
            prc.loadVarianti();
            variante var = null;
            ProcessoVariante procVar = null;
            Reparto rp = new Reparto(card.supplier_name);

            if (prc.variantiProcesso.Count != 1)
            {
                ret = 4;
            }
            else
            {
                var = new variante(Session["ActiveWorkspace_Name"].ToString(), prc.variantiProcesso[0].idVariante);
                procVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), prc, var);
                procVar.loadReparto();
                procVar.process.loadFigli(procVar.variant);
            }

            if (!(customer.CodiceCliente.Length > 0))
            {
                ret = 2;
            }

            if (card.DataConsegna <= DateTime.Now)
            { 
                ret = 5;
            }

            if (procVar == null || procVar.process == null || procVar.process.processID == -1
                || procVar.variant == null || procVar.variant.idVariante == -1)
            {
                ret = 6;
            }

            if (rp.id == -1)
            {
                ret = 8;
            }

            Articolo chechIfAlreadyProcessed = new Articolo(tenant, card);
            if (chechIfAlreadyProcessed.ID != -1)
            {
                ret = 9;
                log = "Articolo is already inside KIS but in a 'N' status";
            }

            int cardConsist = card.checkCardsConsistency();
            if (cardConsist != 1)
            {
                ret = 10;
            }

            if (ret == 1)
            {
                int idCommessa = elComm.Add(card.customer_name, card.ekanban_string, "");
                int annoCommessa = DateTime.UtcNow.Year;
                Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), idCommessa, annoCommessa);

                    cm.Confirmed = true;
                    cm.ConfirmationDate = DateTime.UtcNow;


                    if (cm.ID != -1 && cm.Year != 1)
                {
                    int[]prod = cm.AddArticolo(procVar, card.DataConsegna, card.Quantita, card.ekanban_string);
                    log += "Processo: " + procVar.process.processID.ToString()
                        + " - " + procVar.variant.idVariante.ToString() + "\n";
                    if (prod[0] != -1 && prod[1] != -1)
                    {
                        Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), prod[0], prod[1]);
                        art.DataPrevistaFineProduzione = card.DataConsegna;
                        art.Reparto = rp.id;
                        List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();

                        art.Proc.process.loadFigli(art.Proc.variant);
                        for (int i = 0; i < art.Proc.process.subProcessi.Count; i++)
                        {
                            TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), art.Proc.process.subProcessi[i].processID, art.Proc.process.subProcessi[i].revisione), art.Proc.variant);
                            tskVar.loadTempiCiclo();
                            TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), tskVar.Task.processID, tskVar.Task.revisione, art.Proc.variant.idVariante, tskVar.getDefaultOperatori());
                            if (tc.Tempo != null)
                            {
                                lstTasks.Add(new TaskConfigurato(Session["ActiveWorkspace_Name"].ToString(), tskVar, tc, rp.id, art.Quantita));
                            }
                        }
 
                        ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(Session["ActiveWorkspace_Name"].ToString(), art, lstTasks, rp, art.Quantita);
                        int consistenza = prcCfg.checkConsistency();
                        int rt1 = prcCfg.SimulaIntroduzioneInProduzione();

                        if (rt1 == 1 && consistenza ==1)
                        {
                            //art.Planner = null;//(User)Session["user"];
                            int rt = prcCfg.LanciaInProduzione();
                            if (rt == 1)
                            {
                                log += "Articolo" + card.ekanban_string + " pianificato correttamente in produzione\n";
                                Boolean cambioStato = ChangeStatus(Session["ActiveWorkspace_Name"].ToString(), card, "process");
                                if (cambioStato == true)
                                {
                                    log += "Il cartellino ha cambiato stato.";
                                }
                                else
                                {
                                    // Fallito il segnalo di cambio di stato del cartellino kanban
                                    log += "Error while changing kanban card status. Removing the product";
                                    ret = 11;
                                    art.Depianifica();
                                    art.Delete();
                                    cm.Delete();
                                }
                            }
                            else
                            {
                                ret = 10;
                                //Fallita la pianificazione
                                log += "Production launch failed.";
                                art.Depianifica();
                                art.Delete();
                                cm.Delete();
                            }
                        }
                        else
                        {
                            // Fallita la simulazione della pianificazione
                            ret = 9;
                            ret = rt1;
                            log += "Production launch simulation failed or inconsistent process.";
                            //art.Depianifica();
                            //art.Delete();
                            //cm.Delete();
                        }

                    }
                    else
                    {
                        //Fallita la creazione dell'articolo
                        ret = 7;
                        cm.Delete();
                    }
                }
                else
                {
                    ret = 3;
                }
            }
            return ret;
        }


        [WebMethod]
        public String Main(String tenant)
        {
            LoadConfiguration(tenant);
            if (KanbanBoxEnabled == true)
            {
                ReadCards(tenant);
                for (int i = 0; i < KanbanList.cards.Count; i++)
                {
                    AddToProductionQue(tenant, KanbanList.cards[i]);
                }
            }
            return this.log;
        }
    }
}
