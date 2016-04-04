﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Admin
{
    public partial class configOrderStatusBase1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmConfigReport.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Configurazione Report Stato Ordini Clienti";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                frmConfigReport.Visible = true;
                if (!Page.IsPostBack)
                {
                    configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
                    ddlIDCommessa.SelectedValue = cfgCust.IDCommessa ? "true" : "false";
                    ddlCliente.SelectedValue = cfgCust.Cliente ? "true" : "false";
                    ddlDataInserimentoOrdine.SelectedValue = cfgCust.DataInserimentoOrdine ? "true" : "false";
                    ddlNoteOrdine.SelectedValue = cfgCust.NoteOrdine ? "true" : "false";
                    ddlIDProdotto.SelectedValue = cfgCust.IDProdotto ? "true" : "false";
                    ddlNomeProdotto.SelectedValue = cfgCust.NomeProdotto ? "true" : "false";
                    ddlNomeVariante.SelectedValue = cfgCust.NomeVariante ? "true" : "false";
                    ddlMatricola.SelectedValue = cfgCust.Matricola ? "true" : "false";
                    ddlStatus.SelectedValue = cfgCust.Status ? "true" : "false";
                    ddlReparto.SelectedValue = cfgCust.Reparto ? "true" : "false";
                    ddlDataPrevistaConsegna.SelectedValue = cfgCust.DataPrevistaConsegna ? "true" : "false";
                    ddlDataPrevistaFineProduzione.SelectedValue = cfgCust.DataPrevistaFineProduzione ? "true" : "false";
                    ddlEarlyStart.SelectedValue = cfgCust.EarlyStart ? "true" : "false";
                    ddlEarlyFinish.SelectedValue = cfgCust.EarlyFinish ? "true" : "false";
                    ddlLateStart.SelectedValue = cfgCust.LateStart ? "true" : "false";
                    ddlLateFinish.SelectedValue = cfgCust.LateFinish ? "true" : "false";
                    ddlQuantita.SelectedValue = cfgCust.Quantita ? "true" : "false";
                    ddlQuantitaProdotta.SelectedValue = cfgCust.QuantitaProdotta ? "true" : "false";
                    ddlRitardo.SelectedValue = cfgCust.Ritardo ? "true" : "false";
                    ddlTempoDiLavoroTotale.SelectedValue = cfgCust.TempoDiLavoroTotale ? "true" : "false";
                    ddlLeadTime.SelectedValue = cfgCust.LeadTime ? "true" : "false";
                    ddlTempoDiLavoroPrevisto.SelectedValue = cfgCust.TempoDiLavoroPrevisto ? "true" : "false";
                    ddlIndicatoreCompletamentoTasks.SelectedValue = cfgCust.IndicatoreCompletamentoTasks ? "true" : "false";
                    ddlIndicatoreCompletamentoTempoPrevisto.SelectedValue = cfgCust.IndicatoreCompletamentoTempoPrevisto ? "true" : "false";
                    ddlViewGanttTasks.SelectedValue = cfgCust.ViewGanttTasks ? "true" : "false";
                    ddlViewElencoTasks.SelectedValue = cfgCust.ViewElencoTasks ? "true" : "false";
                    ddlTask_ID.SelectedValue = cfgCust.Task_ID ? "true" : "false";
                    ddlTask_Nome.SelectedValue = cfgCust.Task_Nome ? "true" : "false";
                    ddlTask_Descrizione.SelectedValue = cfgCust.Task_Descrizione ? "true" : "false";
                    ddlTask_Postazione.SelectedValue = cfgCust.Task_Postazione ? "true" : "false";
                    ddlTask_EarlyStart.SelectedValue = cfgCust.Task_EarlyStart ? "true" : "false";
                    ddlTask_EarlyFinish.SelectedValue = cfgCust.Task_EarlyFinish ? "true" : "false";
                    ddlTask_LateStart.SelectedValue = cfgCust.Task_LateStart ? "true" : "false";
                    ddlTask_LateFinish.SelectedValue = cfgCust.Task_LateFinish ? "true" : "false";
                    ddlTask_NOperatori.SelectedValue = cfgCust.Task_NOperatori ? "true" : "false";
                    ddlTask_TempoCiclo.SelectedValue = cfgCust.Task_TempoCiclo ? "true" : "false";
                    ddlTask_TempoDiLavoroPrevisto.SelectedValue = cfgCust.Task_TempoDiLavoroPrevisto ? "true" : "false";
                    ddlTask_TempoDiLavoroEffettivo.SelectedValue = cfgCust.Task_TempoDiLavoroEffettivo ? "true" : "false";
                    ddlTask_Status.SelectedValue = cfgCust.Task_Status ? "true" : "false";
                    ddlTask_QuantitaProdotta.SelectedValue = cfgCust.Task_QuantitaProdotta ? "true" : "false";
                }
            }
        }

        protected void ddlIDCommessa_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.IDCommessa;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlIDCommessa.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.IDCommessa = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione di IDCommessa avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Cliente;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlCliente.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Cliente = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione di Cliente avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlDataInserimentoOrdine_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.DataInserimentoOrdine;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlDataInserimentoOrdine.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.DataInserimentoOrdine = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Data Inserimento Ordine avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlNoteOrdine_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.NoteOrdine;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlNoteOrdine.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.NoteOrdine = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione delle Note della Commessa avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }
        
        protected void ddlIDProdotto_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.IDProdotto;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlIDProdotto.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.IDProdotto = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dell'ID Prodotto avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlNomeProdotto_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.NomeProdotto;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlNomeProdotto.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.NomeProdotto = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Nome Prodotto avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlNomeVariante_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.NomeVariante;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlNomeVariante.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.NomeVariante = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Nome Variante avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlMatricola_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Matricola;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlMatricola.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Matricola = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Matricola avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Status;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlStatus.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Status = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dello Status avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlReparto_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Reparto;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlReparto.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Reparto = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Reparto avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlDataPrevistaConsegna_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.DataPrevistaConsegna;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlDataPrevistaConsegna.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.DataPrevistaConsegna = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Data Prevista Consegna avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlDataPrevistaFineProduzione_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.DataPrevistaFineProduzione;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlDataPrevistaFineProduzione.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.DataPrevistaFineProduzione = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Data Prevista Fine Produzione avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlEarlyStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.EarlyStart;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlEarlyStart.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.EarlyStart = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione di EarlyStart avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlEarlyFinish_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.EarlyFinish;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlEarlyFinish.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.EarlyFinish = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione di EarlyFinish avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlLateStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.LateStart;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlLateStart.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.LateStart = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione di LateStart avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlLateFinish_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.LateFinish;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlLateFinish.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.LateFinish = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione di LateStart avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlQuantita_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Quantita;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlQuantita.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Quantita = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Quantità Prevista avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlQuantitaProdotta_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.QuantitaProdotta;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlQuantitaProdotta.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.QuantitaProdotta = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Quantità Prodotta avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlRitardo_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Ritardo;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlRitardo.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Ritardo = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Ritardo avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTempoDiLavoroTotale_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.TempoDiLavoroTotale;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTempoDiLavoroTotale.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.TempoDiLavoroTotale = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Tempo di Lavoro Totale avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlLeadTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.LeadTime;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlLeadTime.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.LeadTime = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Lead Time avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTempoDiLavoroPrevisto_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.TempoDiLavoroPrevisto;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTempoDiLavoroPrevisto.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.TempoDiLavoroPrevisto = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Tempo di Lavoro Previsto avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlIndicatoreCompletamentoTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.IndicatoreCompletamentoTasks;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlIndicatoreCompletamentoTasks.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.IndicatoreCompletamentoTasks = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dell'Indicatore di Completamento dei Tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlIndicatoreCompletamentoTempoPrevisto_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.IndicatoreCompletamentoTempoPrevisto;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlIndicatoreCompletamentoTempoPrevisto.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.IndicatoreCompletamentoTempoPrevisto = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dell'Indicatore di Completamento dei Tasks sul Tempo Previsto avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlViewGanttTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.ViewGanttTasks;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlViewGanttTasks.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.ViewGanttTasks = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del diagramma di GANTT di avanzamento dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlViewElencoTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.ViewElencoTasks;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlViewElencoTasks.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.ViewElencoTasks = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dell'elenco dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_ID;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_ID.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_ID = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dell'ID dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_Nome_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_Nome;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_Nome.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_Nome = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Nome dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_Descrizione_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_Descrizione;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_Descrizione.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_Descrizione = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Descrizione dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_Postazione_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_Postazione;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_Postazione.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_Postazione = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Postazione dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_EarlyStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_EarlyStart;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_EarlyStart.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_EarlyStart = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dell'EarlyStart dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_LateStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_LateStart;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_LateStart.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_LateStart = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del LateStart dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_EarlyFinish_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_EarlyFinish;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_EarlyFinish.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_EarlyFinish = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dell'EarlyFinish dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_LateFinish_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_LateFinish;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_LateFinish.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_LateFinish = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del LateFinish dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_NOperatori_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_NOperatori;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_NOperatori.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_NOperatori = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Numero di Operatori dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_TempoCiclo_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_TempoCiclo;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_TempoCiclo.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_TempoCiclo = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Tempo Ciclo dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_TempoDiLavoroPrevisto_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_TempoDiLavoroPrevisto;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_TempoDiLavoroPrevisto.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_TempoDiLavoroPrevisto = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Tempo di Lavoro Previsto dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_TempoDiLavoroEffettivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_TempoDiLavoroEffettivo;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_TempoDiLavoroEffettivo.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_TempoDiLavoroEffettivo = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione del Tempo di Lavoro Effettivo dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_Status;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_Status.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_Status = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione dello Status dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }

        protected void ddlTask_QuantitaProdotta_SelectedIndexChanged(object sender, EventArgs e)
        {
            configBaseOrderStatusReport cfgCust = new configBaseOrderStatusReport();
            Boolean newValue = cfgCust.Task_QuantitaProdotta;
            Boolean check = false;
            try
            {
                newValue = Convert.ToBoolean(ddlTask_QuantitaProdotta.SelectedValue);
                check = true;
            }
            catch
            {
                check = false;
            }
            if (check)
            {
                cfgCust.Task_QuantitaProdotta = newValue;
                lbl1.Text = "Aggiornamento della visualizzazione della Quantità Prodotta dei tasks avvenuto correttamente.";
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.<br/ >";
            }
        }
    }
}