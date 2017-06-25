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
    public partial class AndonReparto : System.Web.UI.Page
    {
        protected List<Articolo> artNP = new List<Articolo>();
        //private int idRep;
        public static KIS.App_Code.AndonReparto andonCfg;
        public static Reparto rp;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool check = false;
            int repID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    repID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    repID = -1;
                }
            }

            frmShowStatusUtenti.repID = repID;

            if (repID != -1)
            {
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    rp = new Reparto(repID);
                    if (rp.id != -1)
                    {
                        frmShowStatusUtenti.repID = rp.id;
                        frmArticoliAvviati.repID = rp.id;
                            andonCfg = new KIS.App_Code.AndonReparto(rp.id);
                            andonCfg.loadCampiVisualizzati();
                            lblReparto.Text = rp.name;
                            loadCommesse(rp);
                            /*artNP.Sort(delegate (Articolo p1, Articolo p2)
                            {
                                return p1.LateStart.CompareTo(p2.LateStart);
                            });*/
                            if (artNP.Count == 0)
                            {
                                rptElencoArticoliNP.Visible = false;
                            lbl1.Text = GetLocalResourceObject("lblNoMoreProducts").ToString();
                            }
                            else
                            {
                                rptElencoArticoliNP.Visible = true;
                                rptElencoArticoliNP.DataSource = artNP;
                                rptElencoArticoliNP.DataBind();
                            }
                        }
                        else
                        {
                            frmArticoliAvviati.repID = -1;
                            frmShowStatusUtenti.repID = -1;
                        }
                    }
                    else
                    {
                        //frmArticoliAvviati.repID = -1;
                        //frmShowStatusUtenti.repID = -1;
                    }
                }
            }

        protected void rptElencoArticoliNP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField HartID = (HiddenField)e.Item.FindControl("lblIDArticolo");
                HiddenField HartYear = (HiddenField)e.Item.FindControl("lblAnnoArticolo");
                
                HyperLink lnkShowStatusArticolo = (HyperLink)e.Item.FindControl("lnkStatoArticolo");
                
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
                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    ///*
                    if (art.ID != -1 &&tRow!=null)
                    {
                        //Cliente customer = new Cliente(art.Cliente);

                        if (art.Status == 'N')
                        {
                            lnkShowStatusArticolo.Enabled = false;
                            lnkShowStatusArticolo.Visible = false;
                        }
                        for (int i = 0; i < andonCfg.CampiVisualizzati.Count; i++)
                        {
                            HtmlTableCell td = new HtmlTableCell();
                            Commessa comm;
                            switch (andonCfg.CampiVisualizzati.Keys.ElementAt(i))
                            {
                                case "CommessaID":
                                    td.InnerHtml = art.Commessa + "/" + art.AnnoCommessa;
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
                                default:
                                    break;
                            }
                            tRow.Controls.Add(td);
                        }
                    }
                    else
                    {
                        tRow.Visible = false;
                    }
                    //*/
                    if (tRow != null)
                    {
                        if (art.Status == 'N')
                        {
                            tRow.BgColor = "#FFFFFF";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                        }
                        else if (art.Status == 'P')
                        {
                            ///*
                            art.loadTasksProduzione();
                            bool checkYellow = false;
                            bool checkRed = false;
                            for (int i = 0; i < art.Tasks.Count; i++)
                            {
                                if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) >= art.Tasks[i].EarlyStart && TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) <= art.Tasks[i].LateStart)
                                {
                                    checkYellow = true;
                                }
                                else if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) >= art.Tasks[i].LateStart)
                                {
                                    checkRed = true;
                                }
                            }
                            if (checkYellow == false && checkRed == false)
                            {
                                tRow.BgColor = "#ADFF2F";
                                tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#ADFF2F'");
                            }
                            else if (checkRed == true)
                            {
                                tRow.BgColor = "#FF0000";
                                tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF0000'");
                            }
                            else if (checkYellow == true)
                            {
                                tRow.BgColor = "#00FFFF";
                                tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FFFF'");
                            }
                            //*/
                        }
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                System.Web.UI.HtmlControls.HtmlTableRow tHead = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trHead");
                for (int i = 0; i < andonCfg.CampiVisualizzati.Count; i++)
                {
                    HtmlTableCell td = new HtmlTableCell("th");
                    switch (andonCfg.CampiVisualizzati.Keys.ElementAt(i))
                    {
                        case "CommessaID":
                            td.InnerHtml = GetLocalResourceObject("CommessaID").ToString();
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

        protected void loadCommesse(Reparto rp)
        {
            //rp = new Reparto(idRep);
            ElencoArticoli elArtN =new ElencoArticoli('N', rp);
            ElencoArticoli elArtP = new ElencoArticoli('P', rp);

            artNP = elArtP.ListArticoli.Concat(elArtN.ListArticoli).ToList();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.LateStart.CompareTo(p2.LateStart);
            });
            
        }

        protected void TimeCheck_Tick(object sender, EventArgs e)
        {
            lbl1.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
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
                frmShowStatusUtenti.repID = repID;
                //Reparto 
                rp = new Reparto(repID);
                if (rp.id != -1)
                {

                    loadCommesse(rp);
                    artNP.Sort(delegate(Articolo p1, Articolo p2)
                    {
                        return p1.LateStart.CompareTo(p2.LateStart);
                    });

                    if (artNP.Count == 0)
                    {
                        rptElencoArticoliNP.Visible = false;
                        lbl1.Text = GetLocalResourceObject("lblNoMoreProducts").ToString();
                    }
                    else
                    {
                        rptElencoArticoliNP.Visible = true;
                        rptElencoArticoliNP.DataSource = artNP;
                    }
                    rptElencoArticoliNP.DataBind();
                }
            }
        }

    }
}