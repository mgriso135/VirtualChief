using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.eventi;

namespace KIS.Eventi
{
    public partial class CommessaRitardo : System.Web.UI.UserControl
    {
        public int commessaID, commessaAnno;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Commessa EventoRitardo";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                KIS.Commesse.Commessa curr = new KIS.Commesse.Commessa(commessaID, commessaAnno);
                for (int i = 0; i < 24; i++)
                {
                    ddlOre.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                for (int i = 0; i < 60; i++)
                {
                    ddlMinuti.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    ddlSecondi.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                if (!Page.IsPostBack)
                {
                    ConfigurazioneRitardoCommessa cfgRp = new ConfigurazioneRitardoCommessa(curr);
                    if (cfgRp.RitardoMinimoDaSegnalare != null)
                    {
                        ddlOre.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Hours.ToString();
                        ddlMinuti.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Minutes.ToString();
                        ddlSecondi.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Seconds.ToString();
                    }
                    List<Group> grpList = new List<Group>();
                    for (int i = 0; i < cfgRp.ListGroupsID.Count; i++)
                    {
                        grpList.Add(new Group(cfgRp.ListGroupsID[i]));
                    }
                    rptListGruppi.DataSource = grpList;
                    rptListGruppi.DataBind();

                    List<User> usrList = new List<User>();
                    cfgRp.loadUsers();
                    for (int i = 0; i < cfgRp.ListUsers.Count; i++)
                    {
                        usrList.Add(new User(cfgRp.ListUsers[i]));
                    }
                    rptListUtenti.DataSource = usrList;
                    rptListUtenti.DataBind();

                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di gestire gli allarmi di ritardo della commessa.";
                frmAddRitardoGruppo.Visible = false;
                btnShowAddRitardoGruppo.Visible = false;
                btnShowAddRitardoUtente.Visible = false;
                ddlOre.Visible = false;
                ddlMinuti.Visible = false;
                ddlSecondi.Visible = false;
            }
        }


        protected void btnShowAddRitardoGruppo_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddRitardoGruppo.Visible == false)
            {
                frmAddRitardoGruppo.Visible = true;
                GroupList elencoGruppi = new GroupList();
                ddlAddRitardoGruppo.Items.Clear();
                ddlAddRitardoGruppo.Items.Add(new ListItem("Seleziona un gruppo", ""));
                ddlAddRitardoGruppo.DataSource = elencoGruppi.Elenco;
                ddlAddRitardoGruppo.DataTextField = "Nome";
                ddlAddRitardoGruppo.DataValueField = "ID";
                ddlAddRitardoGruppo.DataBind();
                btnSaveRitardoGruppo.Focus();
            }
            else
            {
                frmAddRitardoGruppo.Visible = false;
            }
        }

        protected void btnSaveRitardoGruppo_Click(object sender, ImageClickEventArgs e)
        {

            ConfigurazioneRitardoCommessa cfgRp = new ConfigurazioneRitardoCommessa(new KIS.Commesse.Commessa(commessaID, commessaAnno));
            int idGrp = -1;
            try
            {
                idGrp = Int32.Parse(ddlAddRitardoGruppo.SelectedValue);
            }
            catch
            {
                idGrp = -1;
            }
            if (idGrp != -1)
            {
                Group grp = new Group(idGrp);
                if (grp.ID != -1)
                {
                    bool rt = cfgRp.addGruppo(grp);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text += "Errore." + cfgRp.log;
                    }
                }
            }
            btnSaveRitardoGruppo.Focus();
        }

        protected void btnUndoRitardoGruppo_Click(object sender, ImageClickEventArgs e)
        {
            ddlAddRitardoGruppo.SelectedValue = "";
            btnSaveRitardoGruppo.Focus();
        }

        protected void btnSaveRitMin_Click(object sender, ImageClickEventArgs e)
        {
            ConfigurazioneRitardoCommessa cfgRp = new ConfigurazioneRitardoCommessa(new KIS.Commesse.Commessa(commessaID, commessaAnno));
            int ore, min, sec;
            try
            {
                ore = Int32.Parse(ddlOre.SelectedValue);
                min = Int32.Parse(ddlMinuti.SelectedValue);
                sec = Int32.Parse(ddlSecondi.SelectedValue);
            }
            catch
            {
                ore = -1;
                min = -1;
                sec = -1;
            }
            if (ore != -1 && min != -1 && sec != -1)
            {
                TimeSpan rit = new TimeSpan(ore, min, sec);
                cfgRp.RitardoMinimoDaSegnalare = rit;
            }
            else
            {
                lbl1.Text = "E' avvenuto un errore.";
            }
            btnSaveRitMin.Focus();
        }

        protected void btnUndoRitMin_Click(object sender, ImageClickEventArgs e)
        {
            ConfigurazioneRitardoCommessa cfgRp = new ConfigurazioneRitardoCommessa(new KIS.Commesse.Commessa(commessaID, commessaAnno));
            if (cfgRp.RitardoMinimoDaSegnalare != null)
            {
                ddlOre.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Hours.ToString();
                ddlMinuti.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Minutes.ToString();
                ddlSecondi.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Seconds.ToString();
            }
            else
            {
                ddlOre.SelectedValue = "0";
                ddlMinuti.SelectedValue = "0";
                ddlSecondi.SelectedValue = "0";
            }
            btnSaveRitMin.Focus();
        }

        protected void rptListGruppi_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                   /* tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptListGruppi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int groupID = -1;
            try
            {
                groupID = Int32.Parse(e.CommandArgument.ToString());
            }
            catch
            {
                groupID = -1;
            }

            if (e.CommandName == "deleteGruppo")
            {
                if (groupID != -1)
                {
                    ConfigurazioneRitardoCommessa cfgRp = new ConfigurazioneRitardoCommessa(new KIS.Commesse.Commessa(commessaID, commessaAnno));
                    // Ricerco il gruppo
                    cfgRp.loadGruppi();
                    bool rt = cfgRp.deleteGruppo(new Group(groupID));
                    if (rt == true)
                    {
                        lbl1.Text = "OK!";
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "E' avvenuto un errore.";
                    }
                }
            }
        }

        protected void btnShowAddRitardoUtente_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddRitardoUtente.Visible == false)
            {
                frmAddRitardoUtente.Visible = true;
                UserList elencoUtenti = new UserList();
                ddlAddRitardoUtente.Items.Clear();
                ddlAddRitardoUtente.Items.Add(new ListItem("Seleziona un utente", ""));
                ddlAddRitardoUtente.DataSource = elencoUtenti.elencoUtenti;
                ddlAddRitardoUtente.DataTextField = "FullName";
                ddlAddRitardoUtente.DataValueField = "username";
                ddlAddRitardoUtente.DataBind();
                btnSaveRitardoUtente.Focus();
            }
            else
            {
                frmAddRitardoGruppo.Visible = false;
            }
        }

        protected void btnSaveRitardoUtente_Click(object sender, ImageClickEventArgs e)
        {
            ConfigurazioneRitardoCommessa cfgRp = new ConfigurazioneRitardoCommessa(new KIS.Commesse.Commessa(commessaID, commessaAnno));

            if (ddlAddRitardoUtente.SelectedValue != "")
            {
                User usr = new User(ddlAddRitardoUtente.SelectedValue);
                if (usr.username != "")
                {
                    bool rt = cfgRp.addUser(usr);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text += "Errore." + cfgRp.log;
                    }
                }
            }
            btnSaveRitardoUtente.Focus();
        }

        protected void btnUndoRitardoUtente_Click(object sender, ImageClickEventArgs e)
        {
            ddlAddRitardoUtente.SelectedValue = "";
            btnSaveRitardoUtente.Focus();
        }

        protected void rptListUtenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr2");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr2");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptListUtenti_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "deleteUtente")
            {
                ConfigurazioneRitardoCommessa cfgRp = new ConfigurazioneRitardoCommessa(new KIS.Commesse.Commessa(commessaID, commessaAnno));
                // Ricerco il gruppo

                bool rt = cfgRp.deleteUtente(new User(e.CommandArgument.ToString()));
                if (rt == true)
                {
                    lbl1.Text = "OK!";
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl1.Text = "E' avvenuto un errore.";
                }
            }
        }
   
    
    }
}