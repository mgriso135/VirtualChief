using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;

namespace KIS.Produzione
{
    public partial class AdonRepartoGANTT : System.Web.UI.Page
    {
        protected List<Articolo> artNP = new List<Articolo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack && !Page.IsCallback)
            {
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
                    Reparto rp = new Reparto(repID);
                    if (rp.id != -1)
                    {
                        lblReparto.Text = rp.name;
                        loadCommesse(rp.id);
                        artNP.Sort(delegate(Articolo p1, Articolo p2)
                        {
                            return p1.LateStart.CompareTo(p2.LateStart);
                        });
                        if (artNP.Count == 0)
                        {
                            rptElencoArticoliNP.Visible = false;
                            lbl1.Text = "Nessun articolo in stato N o P<br/>";
                        }
                        else
                        {
                            rptElencoArticoliNP.Visible = true;
                            rptElencoArticoliNP.DataSource = artNP;
                            rptElencoArticoliNP.DataBind();
                        }

                        frmGANTT.idReparto = rp.id;
                    }
                    else
                    {
                        frmGANTT.idReparto = -1;
                        frmGANTT.Visible = false;
                    }
                }
                else
                {
                    frmGANTT.idReparto = -1;
                    frmGANTT.Visible = false;
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
                Label lblCliente = (Label)e.Item.FindControl("lblCliente");
                Label lblIDCommessa = (Label)e.Item.FindControl("lblIDCommessa");
                Label lblYearCommessa = (Label)e.Item.FindControl("lblAnnoCommessa");
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
                    if (art.ID != -1)
                    {
                        lblCliente.Text = art.Cliente;
                        lblIDCommessa.Text = art.Commessa.ToString();
                        lblYearCommessa.Text = art.AnnoCommessa.ToString();

                        if (art.Status == 'N')
                        {
                            lnkShowStatusArticolo.Enabled = false;
                            lnkShowStatusArticolo.Visible = false;
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
                                tRow.BgColor = "#00FFFF";
                                tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FFFF'");
                            }
                        }
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    }


                }
            }
        }

        protected void loadCommesse(int idRep)
        {
            ElencoCommesse elComm = new ElencoCommesse();
            elComm.loadCommesse();
            Reparto rp = new Reparto(idRep);
            for (int i = 0; i < elComm.Commesse.Count; i++)
            {
                elComm.Commesse[i].loadArticoli(rp);
                for (int j = 0; j < elComm.Commesse[i].Articoli.Count; j++)
                {
                    if (elComm.Commesse[i].Articoli[j].Status == 'N' || elComm.Commesse[i].Articoli[j].Status == 'P' && elComm.Commesse[i].Articoli[j].Reparto == idRep)
                    {
                        artNP.Add(elComm.Commesse[i].Articoli[j]);
                    }
                }
            }
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
                Reparto rp = new Reparto(repID);
                if (rp.id != -1)
                {

                    loadCommesse(repID);
                    artNP.Sort(delegate(Articolo p1, Articolo p2)
                    {
                        return p1.LateStart.CompareTo(p2.LateStart);
                    });

                    if (artNP.Count == 0)
                    {
                        rptElencoArticoliNP.Visible = false;
                        lbl1.Text += "<br />Nessun articolo in stato N o P<br />";
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