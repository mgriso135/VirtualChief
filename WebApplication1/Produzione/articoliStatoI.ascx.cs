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
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    andonCfg = new AndonCompleto();
                    andonCfg.loadCampiVisualizzati();
                    andonCfg.loadCampiVisualizzatiTasks();
                artI = new List<Articolo>();
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
            ElencoCommesse elComm = new ElencoCommesse();
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
                    Articolo art = new Articolo(artID, artYear);
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
                        if (art.Status == 'I')
                        {
                            for(int i = 0; i < andonCfg.CampiVisualizzati.Count; i++)
                            {
                                HtmlTableCell td = new HtmlTableCell();
                                //td.InnerHtml = andonCfg.CampiVisualizzati.Keys.ElementAt(i);
                                Commessa comm;
                                switch (andonCfg.CampiVisualizzati.Keys.ElementAt(i))
                                {
                                    case "Commessa ID":
                                        td.InnerHtml = art.Commessa + "/" + art.AnnoCommessa;
                                        break;
                                    case "Commessa Codice Cliente":
                                        td.InnerHtml = art.Cliente;
                                        break;
                                    case "Commessa RagioneSociale Cliente":
                                        td.InnerHtml = art.RagioneSocialeCliente;
                                        break;
                                    case "Commessa Data Inserimento":
                                        comm = new Commessa(art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.DataInserimento.ToString("dd/MM/yyyy");
                                        break;
                                    case "Commessa Note":
                                        comm = new Commessa(art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.Note;
                                        break;
                                    case "Prodotto ID":
                                        comm = new Commessa(art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = art.ID.ToString() + "/" + art.Year.ToString();
                                        break;
                                    case "Prodotto LineaProdotto":
                                        td.InnerHtml = art.Proc.process.processName;
                                        break;
                                    case "Prodotto NomeProdotto":
                                        td.InnerHtml = art.Proc.variant.nomeVariante;
                                        break;
                                    case "Prodotto Matricola":
                                        td.InnerHtml = art.Matricola;
                                        break;
                                    case "Prodotto Status":
                                        td.InnerHtml = art.Status.ToString();
                                        break;
                                    case "Reparto":
                                        td.InnerHtml = art.RepartoNome;
                                        break;
                                    case "Data Prevista Consegna":
                                        td.InnerHtml = art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
                                        break;
                                    case "Data Prevista Fine Produzione":
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
                                    case "Prodotto Quantita":
                                        td.InnerHtml = art.Quantita.ToString();
                                        break;
                                    case "Prodotto Quantita Realizzata":
                                        td.InnerHtml = art.QuantitaProdotta.ToString();
                                        break;
                                    case "Prodotto Ritardo":
                                        td.InnerHtml = Math.Round(art.Ritardo.TotalHours,2).ToString();
                                        break;
                                    case "Prodotto Tempo di Lavoro Totale":
                                        art.loadTempoDiLavoroTotale();
                                        td.InnerHtml = Math.Round(art.TempoDiLavoroTotale.TotalHours,2).ToString();
                                        break;
                                    case "Prodotto Indicatore Completamento Tasks":
                                        td.InnerHtml = Math.Round(art.IndicatoreCompletamentoTasks,1).ToString() + "%";
                                        break;
                                    case "Prodotto Indicatore Completamento Tempo Previsto":
                                        td.InnerHtml = Math.Round(art.IndicatoreCompletamentoTempoPrevisto,1).ToString() + "%";
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
                        case "Commessa ID":
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
                            break;
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
                            case "Task ID":
                                lblTaskFields.Text += tsk.TaskProduzioneID.ToString();
                                break;
                            case "Task Nome":
                                lblTaskFields.Text += tsk.Name.ToString();
                                break;
                            case "Task Descrizione":
                                lblTaskFields.Text += tsk.Description.ToString();
                                break;
                            case "Task Postazione":
                                Postazione pst = new Postazione(tsk.PostazioneID);
                                lblTaskFields.Text += pst.name.ToString();
                                break;
                            case "Task EarlyStart":
                                lblTaskFields.Text += tsk.EarlyStart.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "Task LateStart":
                                lblTaskFields.Text += tsk.LateStart.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "Task EarlyFinish":
                                lblTaskFields.Text += tsk.EarlyFinish.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "Task LateFinish":
                                lblTaskFields.Text += tsk.LateFinish.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "Task NumeroOperatori":
                                lblTaskFields.Text += tsk.NumOperatori.ToString();
                                break;
                            case "Task TempoCiclo":
                                lblTaskFields.Text += Math.Round(tsk.TempoC.TotalHours,1).ToString() + "h";
                                break;
                            case "Task TempoDiLavoroPrevisto":
                                lblTaskFields.Text += Math.Round(tsk.TempoDiLavoroPrevisto.TotalHours,1).ToString() + "h";
                                break;
                            case "Task TempoDiLavoroEffettivo":
                                lblTaskFields.Text += Math.Round(tsk.TempoDiLavoroEffettivo.TotalHours,1).ToString() + "h";
                                break;
                            case "Task Status":
                                lblTaskFields.Text += tsk.Status.ToString();
                                break;
                            case "Task QuantitaPrevista":
                                lblTaskFields.Text += tsk.QuantitaPrevista.ToString();
                                break;
                            case "Task QuantitaProdotta":
                                lblTaskFields.Text += tsk.QuantitaProdotta.ToString();
                                break;
                            case "Task Ritardo":
                                lblTaskFields.Text += Math.Round(tsk.ritardo.TotalHours,1).ToString() + "h";
                                break;
                            case "Task InizioEffettivo":
                                lblTaskFields.Text += tsk.DataInizioTask.ToString("dd/MM/yyyy HH:mm:ss");
                                break;
                            case "Task FineEffettiva":
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
                        else if (tsk.EarlyStart <= DateTime.Now && tsk.LateStart >= DateTime.Now)
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
                        tRow.BgColor = "#0000FF";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#0000FF'");
                        tRow.Style.Add("color", "#FFFFFF");
                    }
                    else
                    {
                        tRow.BgColor = "#ffa500";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#ffa500'");
                    }
                }
            }
        }
    }
}