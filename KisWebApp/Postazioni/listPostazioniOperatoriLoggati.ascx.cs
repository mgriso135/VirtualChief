using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Postazioni
{
    public partial class listPostazioniOperatoriLoggati : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                if (!Page.IsPostBack)
                {
                    ElencoPostazioni elPost = new ElencoPostazioni(Session["ActiveWorkspace"].ToString());
                    rptPostazioniUtenti.DataSource = elPost.elenco;
                    rptPostazioniUtenti.DataBind();
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

            if (pID != -1)
            {
                Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), pID);
                p.loadUtentiLoggati();
                AndonCompleto andone = new AndonCompleto(Session["ActiveWorkspace"].ToString());
                String configShowNomi = andone.PostazioniFormatoUsername.ToString();
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
                    //lblUsers.Text += p.UtentiLoggati[i] + "<br />";
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
            ElencoPostazioni elPost = new ElencoPostazioni(Session["ActiveWorkspace"].ToString());
            rptPostazioniUtenti.DataSource = elPost.elenco;
            rptPostazioniUtenti.DataBind();
        }
    }
}