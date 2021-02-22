using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Operatori
{
    public partial class operatorePostazioniAttive : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {

                User curr = (User)Session["user"];
                // Verifico se l'utente appartiene al gruppo "Operatori"
                curr.loadGruppi();
                bool checkOperatori = false;
                for (int i = 0; i < curr.Gruppi.Count; i++)
                {
                    for (int j = 0; j < curr.Gruppi[i].Permessi.Elenco.Count; j++)
                    {

                        if (curr.Gruppi[i].Permessi.Elenco[j].NomePermesso == "Postazione check-in" && curr.Gruppi[i].Permessi.Elenco[j].X == true)
                        {
                            checkOperatori = true;
                        }
                    }
                }

                if (checkOperatori == true)
                {
                    if (!Page.IsPostBack)
                    {
                        curr.loadPostazioniAttive();
                        if (curr.PostazioniAttive.Count > 0)
                        {
                            rptPostazioniAttive.DataSource = curr.PostazioniAttive;
                            rptPostazioniAttive.DataBind();
                        }
                        else
                        {
                            rptPostazioniAttive.Visible = false;
                            lbl1.Text = GetLocalResourceObject("lblNoCheckIn").ToString();
                        }
                    }
                }
            
            }
            else
            {
                rptPostazioniAttive.Visible = false;
            }
        }

        protected void rptPostazioniAttive_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField HID = (HiddenField)e.Item.FindControl("id");
                int pstID = -1;
                try
                {
                    pstID = Int32.Parse(HID.Value.ToString());
                }
                catch
                {
                    pstID = -1;
                }

                if (pstID != -1)
                {
                    User curr = (User)Session["user"];

                    // Inserisco la lista degli utenti già loggati
                    Label lblUserLoggati = (Label)e.Item.FindControl("lblUserLogged");
                    Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), pstID);
                    p.loadUtentiLoggati();
                    for (int i = 0; i < p.UtentiLoggati.Count; i++)
                    {
                        if (p.UtentiLoggati[i] != curr.username)
                        {
                            lblUserLoggati.Text += p.UtentiLoggati[i];
                            if (i < p.UtentiLoggati.Count - 1)
                            {
                                lblUserLoggati.Text += "<br/>";
                            }
                        }
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
                    /* tRow.BgColor = "#C0C0C0";
                     tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                     tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptPostazioniAttive_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "checkOut")
            {
                User curr = (User)Session["user"];
                int pst = -1;
                try
                {
                    pst = Int32.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    pst = -1;
                }

                if (pst != -1)
                {
                    Postazione p = new Postazione(Session["ActiveWorkspace"].ToString(), pst);
                    if (p.id != -1)
                    {
                        bool rt = curr.DoCheckOut(p);
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblCheckOpenTasks").ToString();
                        }
                    }
                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                // Verifico se l'utente appartiene al gruppo "Operatori"
                curr.loadGruppi();
                bool checkOperatori = false;
                for (int i = 0; i < curr.Gruppi.Count; i++)
                {
                    for (int j = 0; j < curr.Gruppi[i].Permessi.Elenco.Count; j++)
                    {

                        if (curr.Gruppi[i].Permessi.Elenco[j].NomePermesso == "Postazione check-in" && curr.Gruppi[i].Permessi.Elenco[j].X == true)
                        {
                            checkOperatori = true;
                        }
                    }
                }

                if (checkOperatori == true)
                {
                    curr.loadPostazioniAttive();
                    if (curr.PostazioniAttive.Count > 0)
                    {
                        rptPostazioniAttive.DataSource = curr.PostazioniAttive;
                        rptPostazioniAttive.DataBind();
                    }
                    else
                    {
                        rptPostazioniAttive.Visible = false;
                        lbl1.Text = GetLocalResourceObject("lblNoCheckIn").ToString();
                    }
                }
            }
        }
    }
}