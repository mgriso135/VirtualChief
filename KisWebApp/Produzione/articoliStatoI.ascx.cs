using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS.App_Code;
using System.Web.UI.HtmlControls;

namespace KIS.Produzione
{
    public partial class articoliStatoI : System.Web.UI.UserControl
    {
        List<Articolo> artI;
        public static AndonCompleto andonCfg;
        public TimeZoneInfo tzFuso;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            /*List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {*/
            FusoOrario fuso = new FusoOrario(Session["ActiveWorkspace"].ToString());
            tzFuso = fuso.tzFusoOrario;
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                
                    andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                    andonCfg.loadCampiVisualizzati();
                    andonCfg.loadCampiVisualizzatiTasks();

                artI = new List<Articolo>();
                loadArticoli();
                lblData.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                if (artI.Count == 0)
                    {
                        rptArticoliAvviati.Visible = false;
                        lblTitle.Visible = false;
                        lblData.Visible = false;
                    }
                    else
                    {
                    rptArticoliAvviati.Visible = true;
                        lblTitle.Visible = true;
                        lblData.Visible = true;
                        rptArticoliAvviati.DataSource = artI;
                        rptArticoliAvviati.DataBind();
                    
                    }
                }
            /*}
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare gli articoli avviati.<br/>";
                rptArticoliAvviati.Visible = false;
            }*/
        }

        protected void loadArticoli()
        {
            //ElencoArticoliInProduzione artAperti = new ElencoArticoliInProduzione();
            //artI = artAperti.ElencoArticoli;
            ElencoCommesse elComm = new ElencoCommesse(Session["ActiveWorkspace"].ToString());
            elComm.loadCommesse();
            artI = new List<Articolo>();
            
            for (int i = 0; i < elComm.Commesse.Count; i++)
            {
                elComm.Commesse[i].loadArticoli();
                for (int j = 0; j < elComm.Commesse[i].Articoli.Count; j++)
                {
                    if (elComm.Commesse[i].Articoli[j].Status == 'I')
                    {
                        artI.Add(elComm.Commesse[i].Articoli[j]);
                    }
                }
            }
            artI.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.DataPrevistaFineProduzione.CompareTo(p2.DataPrevistaFineProduzione);
            });
        }

        protected void rptArticoliAvviati_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (andonCfg == null || andonCfg.CampiVisualizzati == null)
            {
                andonCfg = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                andonCfg.loadCampiVisualizzati();
                andonCfg.loadCampiVisualizzatiTasks();
            }

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField HartID = (HiddenField)e.Item.FindControl("lblIDArticolo");
                HiddenField HartYear = (HiddenField)e.Item.FindControl("lblAnnoArticolo");
                Label lblCliente = (Label)e.Item.FindControl("lblCliente");
                Label lblIDCommessa = (Label)e.Item.FindControl("lblIDCommessa");
                Label lblYearCommessa = (Label)e.Item.FindControl("lblAnnoCommessa");
                HyperLink lnkShowStatusArticolo = (HyperLink)e.Item.FindControl("lnkStatoArticolo");
                StatoTasksArticolo frmStatusTasks = (StatoTasksArticolo)e.Item.FindControl("frmStatoTasks");
                int artID = -1;
                int artYear = -1;
                try
                {
                    artID = Int32.Parse(HartID.Value.ToString());
                    artYear = Int32.Parse(HartYear.Value.ToString());
                }
                catch
                {
                    artID = -1;
                    artYear = -1;
                }
                if (artID != -1 && artYear != -1)
                {
                    Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), artID, artYear);
                    if (art.ID != -1)
                    {
                        if (art.Status == 'N')
                        {
                            lnkShowStatusArticolo.Enabled = false;
                            lnkShowStatusArticolo.Visible = false;
                        }

                        Repeater rptStatoTasks = (Repeater)e.Item.FindControl("rptStatoTasks");
                        if(rptStatoTasks!=null)
                        { 
                            art.loadTasksProduzione();
                            if(art.Tasks!=null && art.Tasks.Count>0)
                            { 
                            rptStatoTasks.DataSource = art.Tasks;
                            rptStatoTasks.DataBind();
                            }
                        }
                    }
                    else
                    {
                        lblCliente.Text = "#ERROR";
                        lblIDCommessa.Text = "#ERROR";
                        lblYearCommessa.Text = "#ERROR";
                    }


                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        tRow.Align = "Center";
                        if (art.Status == 'I')
                        {
                            if (andonCfg == null || andonCfg.CampiVisualizzati == null)
                            {
                                andonCfg.loadCampiVisualizzati();
                                andonCfg.loadCampiVisualizzatiTasks();
                            }
                            for (int i = 0; i < andonCfg.CampiVisualizzati.Count; i++)
                            {
                                HtmlTableCell td = new HtmlTableCell();
                                //td.InnerHtml = andonCfg.CampiVisualizzati.Keys.ElementAt(i);
                                Commessa comm;
                                switch (andonCfg.CampiVisualizzati.Keys.ElementAt(i))
                                {
                                    case "CommessaID":
                                        td.InnerHtml = art.Commessa + "/" + art.AnnoCommessa;
                                        break;
                                    case "OrderExternalID":
                                        comm = new Commessa(art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.ExternalID;
                                        break;
                                    case "CommessaCodiceCliente":
                                        td.InnerHtml = art.Cliente;
                                        break;
                                    case "CommessaRagioneSocialeCliente":
                                        td.InnerHtml = art.RagioneSocialeCliente;
                                        break;
                                    case "CommessaDataInserimento":
                                        comm = new Commessa(art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.DataInserimento.ToString("dd/MM/yyyy");
                                        break;
                                    case "CommessaNote":
                                        comm = new Commessa(art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.Note;
                                        break;
                                    case "ProdottoID":
                                        comm = new Commessa(art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = art.ID.ToString() + "/" + art.Year.ToString();
                                        break;
                                    case "ProdottoLineaProdotto":
                                        td.InnerHtml = art.Proc.process.processName;
                                        break;
                                    case "ProdottoNomeProdotto":
                                        td.InnerHtml = art.Proc.variant.nomeVariante;
                                        break;
                                    case "ProdottoMatricola":
                                        td.InnerHtml = art.Matricola;
                                        break;
                                    case "ProdottoStatus":
                                        td.InnerHtml = art.Status.ToString();
                                        break;
                                    case "Reparto":
                                        td.InnerHtml = art.RepartoNome;
                                        break;
                                    case "DataPrevistaConsegna":
                                        td.InnerHtml = art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
                                        break;
                                    case "DataPrevistaFineProduzione":
                                        td.InnerHtml = art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy");
                                        break;
                                    case "EarlyStart":
                                        td.InnerHtml = art.EarlyStart.ToString("dd/MM/yyyy HH:mm:ss");
                                        break;
                                    case "LateStart":
                                        td.InnerHtml = art.LateStart.ToString("dd/MM/yyyy HH:mm:ss");
                                        break;
                                    case "EarlyFinish":
                                        td.InnerHtml = art.EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss");
                                        break;
                                    case "LateFinish":
                                        td.InnerHtml = art.LateFinish.ToString("dd/MM/yyyy HH:mm:ss");
                                        break;
                                    case "ProdottoQuantita":
                                        td.InnerHtml = art.Quantita.ToString();
                                        break;
                                    case "ProdottoQuantitaRealizzata":
                                        td.InnerHtml = art.QuantitaProdotta.ToString();
                                        break;
                                    case "ProdottoRitardo":
                                        td.InnerHtml = Math.Round(art.Ritardo.TotalHours,2).ToString();
                                        break;
                                    case "ProdottoTempodiLavoroTotale":
                                        art.loadTempoDiLavoroTotale();
                                        td.InnerHtml = Math.Round(art.TempoDiLavoroTotale.TotalHours,2).ToString();
                                        break;
                                    case "ProdottoIndicatoreCompletamentoTasks":
                                        td.InnerHtml = Math.Round(art.IndicatoreCompletamentoTasks,1).ToString() + "%";
                                        break;
                                    case "ProdottoIndicatoreCompletamentoTempoPrevisto":
                                        td.InnerHtml = Math.Round(art.IndicatoreCompletamentoTempoPrevisto,1).ToString() + "%";
                                        break;
                                    case "ProductExternalID":
                                        td.InnerHtml = art.Proc.ExternalID.ToString();
                                        break;
                                    case "MeasurementUnit":
                                        art.Proc.loadMeasurementUnit();
                                        td.InnerHtml = art.Proc.measurementUnit.Type.ToString();
                                        break;
                                    default:
                                        break;
                                }
                                tRow.Controls.Add(td);
                            }
                            //tRow.BgColor = "#00FF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#E0E0E0'");
                        }
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    }

                    System.Web.UI.HtmlControls.HtmlTableCell tRowDetail = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tr2");
                    if (tRowDetail != null)
                    {
                        tRowDetail.ColSpan = andonCfg.CampiVisualizzati.Count+1;
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                System.Web.UI.HtmlControls.HtmlTableRow tHead = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trHead_Prod");
                for (int i = 0; i < andonCfg.CampiVisualizzati.Count; i++)
                {
                    HtmlTableCell td = new HtmlTableCell("th");
                    //td.InnerHtml = andonCfg.CampiVisualizzati.Keys.ElementAt(i);

                    switch (andonCfg.CampiVisualizzati.Keys.ElementAt(i))
                    {
                        case "CommessaID":
                            td.InnerHtml = GetLocalResourceObject("CommessaID").ToString();
                            break;
                        case "OrderExternalID":
                            td.InnerHtml = GetLocalResourceObject("OrderExternalID").ToString();
                            break;
                        case "CommessaCodiceCliente":
                            td.InnerHtml = GetLocalResourceObject("CommessaCodiceCliente").ToString();
                            break;
                        case "CommessaRagioneSocialeCliente":
                            td.InnerHtml = GetLocalResourceObject("CommessaRagioneSocialeCliente").ToString();
                            break;
                        case "CommessaDataInserimento":
                            td.InnerHtml = GetLocalResourceObject("CommessaDataInserimento").ToString();
                            break;
                        case "CommessaNote":
                            td.InnerHtml = GetLocalResourceObject("CommessaNote").ToString();
                            break;
                        case "ProdottoID":
                            td.InnerHtml = GetLocalResourceObject("ProdottoID").ToString();
                            break;
                        case "ProdottoLineaProdotto":
                            td.InnerHtml = GetLocalResourceObject("ProdottoLineaProdotto").ToString();
                            break;
                        case "ProdottoNomeProdotto":
                            td.InnerHtml = GetLocalResourceObject("ProdottoNomeProdotto").ToString();
                            break;
                        case "ProdottoMatricola":
                            td.InnerHtml = GetLocalResourceObject("ProdottoMatricola").ToString();
                            break;
                        case "ProdottoStatus":
                            td.InnerHtml = GetLocalResourceObject("ProdottoStatus").ToString();
                            break;
                        case "Reparto":
                            td.InnerHtml = GetLocalResourceObject("Reparto").ToString();
                            break;
                        case "DataPrevistaConsegna":
                            td.InnerHtml = GetLocalResourceObject("DataPrevistaConsegna").ToString();
                            break;
                        case "DataPrevistaFineProduzione":
                            td.InnerHtml = GetLocalResourceObject("DataPrevistaFineProduzione").ToString();
                            break;
                        case "EarlyStart":
                            td.InnerHtml = GetLocalResourceObject("EarlyStart").ToString();
                            break;
                        case "LateStart":
                            td.InnerHtml = GetLocalResourceObject("LateStart").ToString();
                            break;
                        case "EarlyFinish":
                            td.InnerHtml = GetLocalResourceObject("EarlyFinish").ToString();
                            break;
                        case "LateFinish":
                            td.InnerHtml = GetLocalResourceObject("LateFinish").ToString();
                            break;
                        case "ProdottoQuantita":
                            td.InnerHtml = GetLocalResourceObject("ProdottoQuantita").ToString();
                            break;
                        case "ProdottoQuantitaRealizzata":
                            td.InnerHtml = GetLocalResourceObject("ProdottoQuantitaRealizzata").ToString();
                            break;
                        case "ProdottoRitardo":
                            td.InnerHtml = GetLocalResourceObject("ProdottoRitardo").ToString();
                            break;
                        case "ProdottoTempodiLavoroTotale":
                            td.InnerHtml = GetLocalResourceObject("ProdottoTempodiLavoroTotale").ToString();
                            break;
                        case "ProdottoIndicatoreCompletamentoTasks":
                            td.InnerHtml = GetLocalResourceObject("ProdottoIndicatoreCompletamentoTasks").ToString();
                            break;
                        case "ProdottoIndicatoreCompletamentoTempoPrevisto":
                            td.InnerHtml = GetLocalResourceObject("ProdottoIndicatoreCompletamentoTempoPrevisto").ToString();
                            break;
                        case "ProductExternalID":
                            td.InnerHtml = GetLocalResourceObject("ProdottoProductExternalID").ToString();
                            break;
                        case "MeasurementUnit":
                            td.InnerHtml = GetLocalResourceObject("ProdottoMeasurementUnit").ToString();
                            break;
                        default:
                            break;


                            /*case "Commessa ID":
                                td.InnerHtml = "ID Commessa";
                                break;
                            case "Commessa Codice Cliente":
                                td.InnerHtml = "Codice cliente";
                                break;
                            case "Commessa RagioneSociale Cliente":
                                td.InnerHtml = "Ragione sociale cliente";
                                break;
                            case "Commessa Data Inserimento":
                                td.InnerHtml = "Data inserimento commessa";
                                break;
                            case "Commessa Note":
                                td.InnerHtml = "Note commessa";
                                break;
                            case "Prodotto ID":
                                td.InnerHtml = "ID Prodotto";
                                break;
                            case "Prodotto LineaProdotto":
                                td.InnerHtml = "Linea prodotto";
                                break;
                            case "Prodotto NomeProdotto":
                                td.InnerHtml = "Nome prodotto";
                                break;
                            case "Prodotto Matricola":
                                td.InnerHtml = "Matricola";
                                break;
                            case "Prodotto Status":
                                td.InnerHtml = "Status";
                                break;
                            case "Reparto":
                                td.InnerHtml = "Reparto";
                                break;
                            case "Data Prevista Consegna":
                                td.InnerHtml = "Data prevista consegna";
                                break;
                            case "Data Prevista Fine Produzione":
                                td.InnerHtml = "Data prevista fine produzione";
                                break;
                            case "EarlyStart":
                                td.InnerHtml = "Early start";
                                break;
                            case "LateStart":
                                td.InnerHtml = "Late Start";
                                break;
                            case "EarlyFinish":
                                td.InnerHtml = "Early Finish";
                                break;
                            case "LateFinish":
                                td.InnerHtml = "Late Finish";
                                break;
                            case "Prodotto Quantita":
                                td.InnerHtml = "Quantità prevista";
                                break;
                            case "Prodotto Quantita Realizzata":
                                td.InnerHtml = "Quantità realizzata";
                                break;
                            case "Prodotto Ritardo":
                                td.InnerHtml = "Ritardo";
                                break;
                            case "Prodotto Tempo di Lavoro Totale":
                                td.InnerHtml = "Tempo di lavoro totale";
                                break;
                            case "Prodotto Indicatore Completamento Tasks":
                                td.InnerHtml = "Indicatore Completamento Tasks";
                                break;
                            case "Prodotto Indicatore Completamento Tempo Previsto":
                                td.InnerHtml = "Indicatore Completamento Tempo Previsto";
                                break;
                            default:
                                break;*/
                    }
                    tHead.Controls.Add(td);
                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            lblData.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            loadArticoli();
            if (artI.Count == 0)
            {
                rptArticoliAvviati.Visible = false;
                lblTitle.Visible = false;
                lblData.Visible = false;
            }
            else
            {
                rptArticoliAvviati.Visible = true;
                lblTitle.Visible = true;
                lblData.Visible = true;
                rptArticoliAvviati.DataSource = artI;
                rptArticoliAvviati.DataBind();
            }
        }

        protected void rptStatoTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField hTaskID = (HiddenField)e.Item.FindControl("hTaskID");
                Label lblTaskFields = (Label)e.Item.FindControl("lblTaskFields");
                int taskID = -1;
                try
                {
                    taskID = Int32.Parse(hTaskID.Value.ToString());
                }
                catch
                {
                    taskID = -1;
                }

                TaskProduzione tsk = new TaskProduzione(taskID);
                lblTaskFields.Text = "";
                if (tsk.TaskProduzioneID != -1)
                {
                    for (int i = 0; i < andonCfg.CampiVisualizzatiTasks.Count; i++)
                    {
                        lblTaskFields.Text +=i>0? "<br />":"";
                        switch (andonCfg.CampiVisualizzatiTasks.Keys.ElementAt(i))
                        {
                            case "TaskID":
                                lblTaskFields.Text += tsk.TaskProduzioneID.ToString();
                                break;
                            case "TaskNome":
                                lblTaskFields.Text += tsk.Name.ToString();
                                break;
                            case "TaskDescrizione":
                                lblTaskFields.Text += tsk.Description.ToString();
                                break;
                            case "TaskPostazione":
                                Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), tsk.PostazioneID);
                                lblTaskFields.Text += pst.name.ToString();
                                break;
                            case "TaskEarlyStart":
                                lblTaskFields.Text += tsk.EarlyStart.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "TaskLateStart":
                                lblTaskFields.Text += tsk.LateStart.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "TaskEarlyFinish":
                                lblTaskFields.Text += tsk.EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "TaskLateFinish":
                                lblTaskFields.Text += tsk.LateFinish.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "TaskNumeroOperatori":
                                lblTaskFields.Text += tsk.NumOperatori.ToString();
                                break;
                            case "TaskTempoCiclo":
                                lblTaskFields.Text += Math.Round(tsk.TempoC.TotalHours,1).ToString() + "h";
                                break;
                            case "TaskTempoDiLavoroPrevisto":
                                lblTaskFields.Text += Math.Round(tsk.TempoDiLavoroPrevisto.TotalHours,1).ToString() + "h";
                                break;
                            case "TaskTempoDiLavoroEffettivo":
                                lblTaskFields.Text += Math.Round(tsk.TempoDiLavoroEffettivo.TotalHours,1).ToString() + "h";
                                break;
                            case "TaskStatus":
                                lblTaskFields.Text += tsk.Status.ToString();
                                break;
                            case "TaskQuantitaPrevista":
                                lblTaskFields.Text += tsk.QuantitaPrevista.ToString();
                                break;
                            case "TaskQuantitaProdotta":
                                lblTaskFields.Text += tsk.QuantitaProdotta.ToString();
                                break;
                            case "TaskRitardo":
                                lblTaskFields.Text += Math.Round(tsk.ritardo.TotalHours,1).ToString() + "h";
                                break;
                            case "TaskInizioEffettivo":
                                lblTaskFields.Text += tsk.DataInizioTask.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "TaskFineEffettiva":
                                lblTaskFields.Text += tsk.DataFineTask.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            default:
                            break;
                        }
                    }
                    
                    
                }
                else
                {
                }

                // COLORE!!!
                System.Web.UI.HtmlControls.HtmlTableCell tRow = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    if (tsk.Status == 'F')
                    {
                        tRow.BgColor = "#00FF00";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                    }
                    else if (tsk.Status == 'N')
                    {
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        if (tsk.EarlyStart >= DateTime.Now)
                        {
                            tRow.BgColor = "#FFFFFF";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                        }
                        else if (tsk.EarlyStart <= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzFuso) && tsk.LateStart >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzFuso))
                        {
                            // Giallo
                            tRow.BgColor = "#FFFF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFF00'");
                        }
                        else
                        {
                            // Rosso
                            tRow.BgColor = "#FF0000";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF0000'");
                        }
                    }
                    else if (tsk.Status == 'I')
                    {
                        tRow.BgColor = "#2196F3";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#2196F3'");
                        tRow.Style.Add("color", "#FFFFFF");
                    }
                    else
                    {
                        tRow.BgColor = "#FF9800";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF9800'");
                    }
                }
            }
        }
    }
}