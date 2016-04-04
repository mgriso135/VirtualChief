using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using KIS;

namespace KIS.Admin
{
    public partial class listUsers : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String permessoRichiesto = "Utenti";
            bool checkUser = false;
            if (Session["User"] != null)
            {
                User curr = (User)Session["user"];
                curr.loadGruppi();
                for (int i = 0; i < curr.Gruppi.Count; i++)
                {
                    for (int j = 0; j < curr.Gruppi[i].Permessi.Elenco.Count; j++)
                    {
                        if (curr.Gruppi[i].Permessi.Elenco[j].NomePermesso == permessoRichiesto && curr.Gruppi[i].Permessi.Elenco[j].R == true)
                        {
                            checkUser = true;
                        }
                    }
                }
            }

            UserList lst = new UserList();
            if (lst.numUsers > 0)
            {
                if (checkUser == true)
                {
                    rptUsers.DataSource = lst.elencoUtenti;
                    rptUsers.DataBind();
                }
                else
                {
                    lblLstUsers.Text = "Non hai il permesso di visualizzare l'elenco degli utenti.<br/>";
                }
            }
            else
            {
                rptUsers.Visible = false;
                lblLstUsers.Text = "No user defined yet.<br/>";
            }
        }


        public void rptUsers_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
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

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

    }
}