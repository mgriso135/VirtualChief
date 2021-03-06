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
    public partial class avanzamentoProduzione : System.Web.UI.Page
    {
        protected List<Articolo> artNP = new List<Articolo>();
        public static AndonCompleto andonCfg;
        public int ScrollType;
        public Double ScrollTypeContinuousGoSpeed;
        public Double ScrollTypeContinuousBackSpeed;

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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {*/
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                lbl1.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                ScrollType = 0;
                ScrollTypeContinuousGoSpeed = 0.0;
                ScrollTypeContinuousBackSpeed = 0.0;

                andonCfg = new AndonCompleto(Session["ActiveWorkspace_Name"].ToString());
                    andonCfg.loadCampiVisualizzati();
                    loadCommesse();
                    //try
                    //{
                        artNP.Sort(delegate(Articolo p1, Articolo p2)
                        {
                            return p1.LateStart.CompareTo(p2.LateStart);
                        });

                //}
                /*catch (Exception ex)
                {
                    lbl1.Text += ex.Message;
                }*/

                andonCfg.loadScrollType();
                ScrollType = andonCfg.ScrollType;
                txtScrollType.Value = andonCfg.ScrollType.ToString();
                if (ScrollType == 1)
                {
                    ScrollTypeContinuousGoSpeed = andonCfg.ContinuousScrollGoSpeed;
                    ScrollTypeContinuousBackSpeed = andonCfg.ContinuousScrollBackSpeed;

                    txtScrollTypeContinuousGoSpeed.Value = andonCfg.ContinuousScrollGoSpeed.ToString();
                    txtScrollTypeContinuousBackSpeed.Value = andonCfg.ContinuousScrollBackSpeed.ToString();
                }

                if (artNP.Count > 0)
                    {
                        rptElencoArticoliNP.DataSource = artNP;
                        rptElencoArticoliNP.DataBind();
                        rptElencoArticoliNP.Visible = true;
                    }
                    else
                    {
                        rptElencoArticoliNP.Visible = false;
                        lbl1.Text = GetLocalResourceObject("lblNoMoreProducts").ToString();
                    }
                }
            //}
            /*else
            {
                rptElencoArticoliNP.Visible = false;
                lbl1.Text = "Non hai il permesso di visualizzare gli articoli da avviare.";
            }*/
        }

        protected void rptElencoArticoliNP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if(andonCfg==null||andonCfg.CampiVisualizzati==null)
            {
                andonCfg = new AndonCompleto(Session["ActiveWorkspace_Name"].ToString());
                andonCfg.loadCampiVisualizzati();
                andonCfg.loadScrollType();
                ScrollType = andonCfg.ScrollType;
                txtScrollType.Value = andonCfg.ScrollType.ToString();
                if (ScrollType == 1)
                {
                    ScrollTypeContinuousGoSpeed = andonCfg.ContinuousScrollGoSpeed;
                    ScrollTypeContinuousBackSpeed = andonCfg.ContinuousScrollBackSpeed;

                    txtScrollTypeContinuousGoSpeed.Value = andonCfg.ContinuousScrollGoSpeed.ToString();
                    txtScrollTypeContinuousBackSpeed.Value = andonCfg.ContinuousScrollBackSpeed.ToString();
                }
            }

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
                    Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), artID, artYear);
                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (art.ID != -1 && tRow!=null)
                    {

                        if (art.Status == 'N')
                        {
                            lnkShowStatusArticolo.Enabled = false;
                            lnkShowStatusArticolo.Visible = false;
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
                                    comm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
                                    td.InnerHtml = comm.ExternalID;
                                    break;
                                case "CommessaCodiceCliente":
                                    td.InnerHtml = art.Cliente;
                                    break;
                                case "CommessaRagioneSocialeCliente":
                                    td.InnerHtml = art.RagioneSocialeCliente;
                                    break;
                                case "CommessaDataInserimento":
                                    comm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
                                    td.InnerHtml = comm.DataInserimento.ToString("dd/MM/yyyy");
                                    break;
                                case "CommessaNote":
                                    comm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
                                    td.InnerHtml = comm.Note;
                                    break;
                                case "ProdottoID":
                                    comm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
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
                                    if(art.Proc!=null && art.Proc.ExternalID!=null && art.Proc.ExternalID.Length>0)
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
                    }
                    else
                    {
                        tRow.Visible = false;
                    }

                    
                    //System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        if (art.Status == 'N')
                        {
                            tRow.BgColor = "#FFFFFF";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                        }
                        else if (art.Status == 'P')
                        {
                            art.loadTasksProduzione();
                            bool checkYellow = false;
                            bool checkRed = false;
                            for (int i = 0; i < art.Tasks.Count; i++)
                            {
                                if (DateTime.Now >= art.Tasks[i].EarlyStart && DateTime.Now <= art.Tasks[i].LateStart)
                                {
                                    checkYellow = true;
                                }
                                else if (DateTime.Now >= art.Tasks[i].LateStart)
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
                                tRow.BgColor = "#FFFF00";
                                tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFF00'");
                            }
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
                    //td.InnerHtml = andonCfg.CampiVisualizzati.Keys.ElementAt(i);
                    Commessa comm;
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

        protected void loadCommesse()
        {
            ElencoCommesse elComm = new ElencoCommesse(Session["ActiveWorkspace_Name"].ToString());
            elComm.loadCommesse();
            for (int i = 0; i < elComm.Commesse.Count; i++)
            {
                elComm.Commesse[i].loadArticoli();
                for (int j = 0; j < elComm.Commesse[i].Articoli.Count; j++)
                {
                    if (elComm.Commesse[i].Articoli[j].Status == 'N' || elComm.Commesse[i].Articoli[j].Status == 'P')
                    {
                        artNP.Add(elComm.Commesse[i].Articoli[j]);
                        /*lbl1.Text += "Articolo: " + elComm.Commesse[i].Articoli[j].ID.ToString() + "/" + elComm.Commesse[i].Articoli[j].Year.ToString() 
                            + elComm.Commesse[i].Articoli[j].LateStart.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";*/
                    }
                }
            }
        }

        protected void btnSortID_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.ID.CompareTo(p2.ID);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortYear_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.Year.CompareTo(p2.Year);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortCliente_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.Cliente.CompareTo(p2.Cliente);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortCommessa_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.Commessa.CompareTo(p2.Commessa);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortAnnoCommessa_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.AnnoCommessa.CompareTo(p2.AnnoCommessa);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortMatricola_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.Matricola.CompareTo(p2.Matricola);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortStatus_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.Status.CompareTo(p2.Status);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortDataFP_Click(object sender, EventArgs e)
        {
            loadCommesse();
            try
            {
                artNP.Sort(delegate(Articolo p1, Articolo p2)
                {
                    return p1.LateStart.CompareTo(p2.LateStart);
                });

            }
            catch (Exception ex)
            {
                lbl1.Text = ex.Message;
            }
            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void btnSortDataPC_Click(object sender, EventArgs e)
        {
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.DataPrevistaConsegna.CompareTo(p2.DataPrevistaConsegna);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();
        }

        protected void TimeCheck_Tick(object sender, EventArgs e)
        {
            lbl1.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            loadCommesse();
            artNP.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.LateStart.CompareTo(p2.LateStart);
            });

            rptElencoArticoliNP.DataSource = artNP;
            rptElencoArticoliNP.DataBind();

            rptElencoArticoliNP.Visible = artNP.Count > 0 ? true : false;
        }
    }
}