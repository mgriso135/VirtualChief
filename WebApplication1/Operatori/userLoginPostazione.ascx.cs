using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Operatori
{
    public partial class userLoginPostazione : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                if (!Page.IsPostBack && !Page.IsCallback)
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
                        lblNome.Text = curr.name + " " + curr.cognome + " (" + curr.username + ")";
                        ElencoPostazioni elPostazioni = new ElencoPostazioni();
                        rptPostazioni.DataSource = elPostazioni.elenco;
                        rptPostazioni.DataBind();
                    }
                    else
                    {
                        rptPostazioni.Visible = false;
                        lblNome.Text = "Errore: non hai i permessi necessari per eseguire il check-in in una postazione di lavoro.<br />";
                    }
                }
            }

            else
            {
                rptPostazioni.Visible = false;
            }
        }

        protected void rptPostazioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                    curr.loadPostazioniAttive();
                    bool found = false;
                    for (int i = 0; i < curr.PostazioniAttive.Count; i++)
                    {
                        if (curr.PostazioniAttive[i].id == pstID)
                        {
                            found = true;
                        }
                    }

                    if (found == true)
                    {
                        // Trovata la postazione tra quelle attive!
                        System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                        tRow.Visible = false;
                    }
                    else
                    {
                        
                    }

                    // Inserisco la lista degli utenti già loggati
                    Label lblUserLoggati = (Label)e.Item.FindControl("lblUserLogged");
                    Postazione p = new Postazione(pstID);
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
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptPostazioni_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "checkIn")
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
                    Postazione p = new Postazione(pst);
                    if (p.id != -1)
                    {
                        bool rt = curr.DoCheckIn(p);
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = curr.log;
                        }
                    }
                }
            }
        }

        protected void timer_Tick(object sender, EventArgs e)
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
                lblNome.Text = curr.name + " " + curr.cognome + " (" + curr.username + ")";
                ElencoPostazioni elPostazioni = new ElencoPostazioni();
                rptPostazioni.DataSource = elPostazioni.elenco;
                rptPostazioni.DataBind();
            }
            else
            {
                rptPostazioni.Visible = false;
                lblNome.Text = "Errore: non hai i permessi necessari per eseguire il check-in in una postazione di lavoro.<br />";
            }
        }
    }
}