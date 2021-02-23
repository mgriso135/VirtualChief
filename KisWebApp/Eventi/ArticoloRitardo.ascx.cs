using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.eventi;
using KIS.App_Code;
using KIS.Commesse;

namespace KIS.Eventi
{
    public partial class ArticoloRitardo : System.Web.UI.UserControl
    {
        public int articoloID, articoloAnno;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articolo EventoRitardo";
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
                Articolo curr = new Articolo(Session["ActiveWorkspace"].ToString(), articoloID, articoloAnno);
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
                    ConfigurazioneRitardoArticolo cfgRp = new ConfigurazioneRitardoArticolo(Session["ActiveWorkspace"].ToString(), curr);
                    if (cfgRp.RitardoMinimoDaSegnalare != null)
                    {
                        ddlOre.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Hours.ToString();
                        ddlMinuti.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Minutes.ToString();
                        ddlSecondi.SelectedValue = cfgRp.RitardoMinimoDaSegnalare.Seconds.ToString();
                    }
                    List<Group> grpList = new List<Group>();
                    for (int i = 0; i < cfgRp.ListGroupsID.Count; i++)
                    {
                        grpList.Add(new Group(Session["ActiveWorkspace"].ToString(), cfgRp.ListGroupsID[i]));
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
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmAddRitardoGruppo.Visible = false;
                btnShowAddRitardoGruppo.Visible = false;
                btnShowAddRitardoUtente.Visible = false;
                ddlOre.Visible = false;
                ddlMinuti.Visible = false;
                ddlSecondi.Visible = false;
                btnSaveRitardoGruppo.Visible = false;
                btnSaveRitardoUtente.Visible = false;
                btnSaveRitMin.Visible = false;
                btnUndoRitMin.Visible = false;
            }
        }


        protected void btnShowAddRitardoGruppo_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddRitardoGruppo.Visible == false)
            {
                frmAddRitardoGruppo.Visible = true;
                GroupList elencoGruppi = new GroupList(Session["ActiveWorkspace"].ToString());
                ddlAddRitardoGruppo.Items.Clear();
                ddlAddRitardoGruppo.Items.Add(new ListItem(GetLocalResourceObject("lblGruppiSel").ToString(), ""));
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

            ConfigurazioneRitardoArticolo cfgRp = new ConfigurazioneRitardoArticolo(Session["ActiveWorkspace"].ToString(), new Articolo(Session["ActiveWorkspace"].ToString(), articoloID, articoloAnno));
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
                Group grp = new Group(Session["ActiveWorkspace"].ToString(), idGrp);
                if (grp.ID != -1)
                {
                    bool rt = cfgRp.addGruppo(grp);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text += "Error. " + cfgRp.log;
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
            ConfigurazioneRitardoArticolo cfgRp = new ConfigurazioneRitardoArticolo(Session["ActiveWorkspace"].ToString(), new Articolo(Session["ActiveWorkspace"].ToString(), articoloID, articoloAnno));
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
                lbl1.Text = GetLocalResourceObject("lblGenericError").ToString();
            }
            btnSaveRitMin.Focus();
        }

        protected void btnUndoRitMin_Click(object sender, ImageClickEventArgs e)
        {
            ConfigurazioneRitardoArticolo cfgRp = new ConfigurazioneRitardoArticolo(Session["ActiveWorkspace"].ToString(), new Articolo(Session["ActiveWorkspace"].ToString(), articoloID, articoloAnno));
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
                    /*tRow.BgColor = "#C0C0C0";
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
                    ConfigurazioneRitardoArticolo cfgRp = new ConfigurazioneRitardoArticolo(Session["ActiveWorkspace"].ToString(), new Articolo(Session["ActiveWorkspace"].ToString(), articoloID, articoloAnno));
                    // Ricerco il gruppo
                    cfgRp.loadGruppi();
                    bool rt = cfgRp.deleteGruppo(new Group(Session["ActiveWorkspace"].ToString(), groupID));
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                         lbl1.Text = GetLocalResourceObject("lblGenericError").ToString();
                    }
                }
            }
        }

        protected void btnShowAddRitardoUtente_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddRitardoUtente.Visible == false)
            {
                frmAddRitardoUtente.Visible = true;
                UserList elencoUtenti = new UserList(Session["ActiveWorkspace"].ToString());
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
            ConfigurazioneRitardoArticolo cfgRp = new ConfigurazioneRitardoArticolo(Session["ActiveWorkspace"].ToString(), new Articolo(Session["ActiveWorkspace"].ToString(), articoloID, articoloAnno));

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
                        lbl1.Text += "Error. " + cfgRp.log;
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
                ConfigurazioneRitardoArticolo cfgRp = new ConfigurazioneRitardoArticolo(Session["ActiveWorkspace"].ToString(), new Articolo(Session["ActiveWorkspace"].ToString(), articoloID, articoloAnno));
                // Ricerco il gruppo

                bool rt = cfgRp.deleteUtente(new User(e.CommandArgument.ToString()));
                if (rt == true)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblGenericError").ToString();
                }
            }
        }
   
    
    }
}