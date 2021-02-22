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
    public partial class articoliStatoIAndonReparto : System.Web.UI.UserControl
    {
        List<Articolo> artI;
        public int repID;
        public static KIS.App_Code.AndonReparto andonCfg;
        public static TimeZoneInfo tzFuso;

        protected void Page_Load(object sender, EventArgs e)
        {
                if (repID != -1)
                {
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                tzFuso = rp.tzFusoOrario;
                if (!Page.IsPostBack && !Page.IsCallback)
                    {
                    loadArticoli(repID);                    
                    andonCfg = new KIS.App_Code.AndonReparto(Session["ActiveWorkspace"].ToString(), repID);
                    lblData.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");


                    andonCfg.loadCampiVisualizzati();
                        andonCfg.loadCampiVisualizzatiTasks();
                        if (artI.Count == 0)
                        {
                            rptArticoliAvviati.Visible = false;
                            lblData.Visible = false;
                        }
                        else
                        {
                            rptArticoliAvviati.Visible = true;
                            lblData.Visible = true;
                            rptArticoliAvviati.DataSource = artI;
                            rptArticoliAvviati.DataBind();
                        }
                    }
                }
        }

        protected void loadArticoli(int rp)
        {

            artI = new List<Articolo>();
            Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), rp);
            artI = (new ElencoArticoli(Session["ActiveWorkspace"].ToString(), 'I', rep)).ListArticoli;


            artI = artI.OrderBy(x => x.DataPrevistaFineProduzione).ThenBy(y => y.EarlyStart).ToList();
        }

        protected void rptArticoliAvviati_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField HartID = (HiddenField)e.Item.FindControl("lblIDArticolo");
                HiddenField HartYear = (HiddenField)e.Item.FindControl("lblAnnoArticolo");
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
                        Cliente cln = new Cliente(Session["ActiveWorkspace"].ToString(), art.Cliente);
                        Commessa cm = new Commessa(Session["ActiveWorkspace"].ToString(), art.Commessa, art.AnnoCommessa);

                        if (art.Status == 'N')
                        {
                            lnkShowStatusArticolo.Enabled = false;
                            lnkShowStatusArticolo.Visible = false;
                        }

                        Repeater rptStatoTasks = (Repeater)e.Item.FindControl("rptStatoTasks");
                        art.loadTasksProduzione();

                        rptStatoTasks.DataSource = art.Tasks;
                        rptStatoTasks.DataBind();
                    }
                    else
                    {

                    }


                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        if (art.Status == 'I')
                        {
                            //tRow.BgColor = "#00FF00";
                            //tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor=''");
                        }
                        //tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    }

                    //tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        if (art.Status == 'I')
                        {
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
                                        comm = new Commessa(Session["ActiveWorkspace"].ToString(), art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.ExternalID;
                                        break;
                                    case "CommessaCodiceCliente":
                                        td.InnerHtml = art.Cliente;
                                        break;
                                    case "CommessaRagioneSocialeCliente":
                                        td.InnerHtml = art.RagioneSocialeCliente;
                                        break;
                                    case "CommessaDataInserimento":
                                        comm = new Commessa(Session["ActiveWorkspace"].ToString(), art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.DataInserimento.ToString("dd/MM/yyyy");
                                        break;
                                    case "CommessaNote":
                                        comm = new Commessa(Session["ActiveWorkspace"].ToString(), art.Commessa, art.AnnoCommessa);
                                        td.InnerHtml = comm.Note;
                                        break;
                                    case "ProdottoID":
                                        comm = new Commessa(Session["ActiveWorkspace"].ToString(), art.Commessa, art.AnnoCommessa);
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
                                        td.InnerHtml = Math.Round(art.Ritardo.TotalHours, 2).ToString();
                                        break;
                                    case "ProdottoTempodiLavoroTotale":
                                        art.loadTempoDiLavoroTotale();
                                        td.InnerHtml = Math.Round(art.TempoDiLavoroTotale.TotalHours, 2).ToString();
                                        break;
                                    case "ProdottoIndicatoreCompletamentoTasks":
                                        td.InnerHtml = Math.Round(art.IndicatoreCompletamentoTasks, 1).ToString() + "%";
                                        break;
                                    case "ProdottoIndicatoreCompletamentoTempoPrevisto":
                                        td.InnerHtml = Math.Round(art.IndicatoreCompletamentoTempoPrevisto, 1).ToString() + "%";
                                        break;
                                    case "ProductExternalID":
                                        if (art.Proc != null && art.Proc.ExternalID != null && art.Proc.ExternalID.Length > 0)
                                        {
                                            td.InnerHtml = art.Proc.ExternalID.ToString();
                                        }
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
                            //tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#E0E0E0'");
                        }
                        //tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    }

                    System.Web.UI.HtmlControls.HtmlTableCell tRowDetail = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tr2");
                    if (tRowDetail != null)
                    {
                        tRowDetail.ColSpan = andonCfg.CampiVisualizzati.Count + 1;
                    }

                }
            }
            else if (e.Item.ItemType == ListItemType.Header && andonCfg!= null && andonCfg.CampiVisualizzati!=null)
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

                            
                    }
                    tHead.Controls.Add(td);
                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            lblData.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            bool check = false;
                int repID = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        repID = Int32.Parse(Request.QueryString["id"]);
                        check = true;
                    }
                    catch
                    {
                        repID = -1;
                        check = false;
                    }
                }
                if (repID != -1 && check == true)
                {
                    loadArticoli(repID);
                    if (artI.Count == 0)
                    {
                        rptArticoliAvviati.Visible = false;
                        //lblTitle.Visible = false;
                    }
                    else
                    {
                        rptArticoliAvviati.Visible = true;
                        //lblTitle.Visible = true;
                        lblData.Visible = true;
                        rptArticoliAvviati.DataSource = artI;
                        rptArticoliAvviati.DataBind();
                    }
                }
        }

        protected void rptStatoTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (andonCfg == null || andonCfg.CampiVisualizzati == null)
            {
                andonCfg = new KIS.App_Code.AndonReparto(Session["ActiveWorkspace"].ToString(), repID);
                andonCfg.loadCampiVisualizzati();
                andonCfg.loadCampiVisualizzatiTasks();
            }
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

                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), taskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    
                    for (int i = 0; i < andonCfg.CampiVisualizzatiTasks.Count; i++)
                    {
                        lblTaskFields.Text += i > 0 ? "<br />" : "";
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
                                lblTaskFields.Text += Math.Round(tsk.TempoC.TotalHours, 1).ToString() + "h";
                                break;
                            case "TaskTempoDiLavoroPrevisto":
                                lblTaskFields.Text += Math.Round(tsk.TempoDiLavoroPrevisto.TotalHours, 1).ToString() + "h";
                                break;
                            case "TaskTempoDiLavoroEffettivo":
                                lblTaskFields.Text += Math.Round(tsk.TempoDiLavoroEffettivo.TotalHours, 1).ToString() + "h";
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
                                lblTaskFields.Text += Math.Round(tsk.ritardo.TotalHours, 1).ToString() + "h";
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
                        if (tsk.EarlyStart >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzFuso))
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