using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Postazioni
{
    public partial class listPostazioniOperatoriLoggatiReparto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
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
                    if (!Page.IsPostBack)
                    {
                        if (rp.id != -1)
                        {
                            ElencoPostazioni elPost = new ElencoPostazioni(rp);
                            rptPostazioniUtenti.DataSource = elPost.elenco;
                            rptPostazioniUtenti.DataBind();
                        }
                    }
                }
        }

        protected void rptPostazioniUtenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hID = (HiddenField)e.Item.FindControl("hID");
            Label lblUsers = (Label)e.Item.FindControl("lblUsers");

            int pID = -1;
            try
            {
                pID = Int32.Parse(hID.Value);
            }
            catch
            {
                pID = -1;
                //lblUsers.Text = "Error while converting.";
            }

            // Ricerco i parametri di configurazione del reparto
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

            String configShowNomi = "0";
            if (repID != -1)
            {
                Reparto rp = new Reparto(repID);
                if(rp.id!=-1)
                {
                    configShowNomi = rp.AndonPostazioniFormatoUsername.ToString();
                }
            }
        

            if (pID != -1)
            {
                Postazione p = new Postazione(pID);
                p.loadUtentiLoggati();
                for (int i = 0; i < p.UtentiLoggati.Count; i++)
                {
                    if (configShowNomi == "0")
                    {
                        lblUsers.Text += p.UtentiLoggati[i] + "<br />";
                    }
                    else if (configShowNomi == "1")
                    {
                        User usr = new User(p.UtentiLoggati[i]);
                        lblUsers.Text += usr.name + "<br />";
                    }
                    else if (configShowNomi == "2")
                    {
                        User usr = new User(p.UtentiLoggati[i]);
                        lblUsers.Text += usr.name + " " + usr.cognome.Substring(0, 1) + "<br />";
                    }
                    else if (configShowNomi == "3")
                    {
                        User usr = new User(p.UtentiLoggati[i]);
                        lblUsers.Text += usr.name + " " + usr.cognome + "<br />";
                    }
                }
            }

            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void updTimer1_Tick(object sender, EventArgs e)
        {
            lblData.Text = "Last update: " + DateTime.Now.ToString();
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
                    rptPostazioniUtenti.Visible = true;
                    Reparto rp = new Reparto(repID);
                    ElencoPostazioni elPost = new ElencoPostazioni(rp);
                    rptPostazioniUtenti.DataSource = elPost.elenco;
                    rptPostazioniUtenti.DataBind();
                }
                else
                {
                    rptPostazioniUtenti.Visible = false;
                }

            
        }
    }
}