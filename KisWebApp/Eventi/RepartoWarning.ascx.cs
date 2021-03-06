using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using KIS.eventi;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Eventi
{
    public partial class RepartoWarning : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto EventoWarning";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    ConfigurazioneWarningReparto cfgRp = new ConfigurazioneWarningReparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
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
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmAddWarningGruppo.Visible = false;
                btnShowAddWarningGruppo.Visible = false;
                btnShowAddWarningUtente.Visible = false;
            }
        }

        protected void btnShowAddWarningGruppo_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddWarningGruppo.Visible == false)
            {
                frmAddWarningGruppo.Visible = true;
                GroupList elencoGruppi = new GroupList();
                ddlAddWarningGruppo.Items.Clear();
                ddlAddWarningGruppo.Items.Add(new ListItem("Seleziona un gruppo", ""));
                ddlAddWarningGruppo.DataSource = elencoGruppi.Elenco;
                ddlAddWarningGruppo.DataTextField = "Nome";
                ddlAddWarningGruppo.DataValueField = "ID";
                ddlAddWarningGruppo.DataBind();
                btnSaveWarningGruppo.Focus();
            }
            else
            {
                frmAddWarningGruppo.Visible = false;
            }
        }

        protected void btnSaveWarningGruppo_Click(object sender, ImageClickEventArgs e)
        {
            ConfigurazioneWarningReparto cfgRp = new ConfigurazioneWarningReparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
            int idGrp = -1;
            try
            {
                idGrp = Int32.Parse(ddlAddWarningGruppo.SelectedValue);
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
                        lbl1.Text += "Error." + cfgRp.log;
                    }
                }
            }
            btnSaveWarningGruppo.Focus();
        }

        protected void btnUndoWarningGruppo_Click(object sender, ImageClickEventArgs e)
        {
            ddlAddWarningGruppo.SelectedValue = "";
            btnSaveWarningGruppo.Focus();
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
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
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
                    ConfigurazioneWarningReparto cfgRp = new ConfigurazioneWarningReparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                    // Ricerco il gruppo
                    //cfgRp.loadGruppi();
                    bool rt = cfgRp.deleteGruppo(new Group(groupID));
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "Error.";
                    }
                }
            }
        }

        protected void btnShowAddWarningUtente_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddWarningUtente.Visible == false)
            {
                frmAddWarningUtente.Visible = true;
                UserList elencoUtenti = new UserList(Session["ActiveWorkspace_Name"].ToString());
                ddlAddWarningUtente.Items.Clear();
                ddlAddWarningUtente.Items.Add(new ListItem(GetLocalResourceObject("lblTitleCfgWarningUserSel").ToString(), ""));
                ddlAddWarningUtente.DataSource = elencoUtenti.elencoUtenti;
                ddlAddWarningUtente.DataTextField = "FullName";
                ddlAddWarningUtente.DataValueField = "username";
                ddlAddWarningUtente.DataBind();
                btnSaveWarningUtente.Focus();
            }
            else
            {
                frmAddWarningUtente.Visible = false;
            }
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
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr2");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }
        }

        protected void rptListUtenti_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "deleteUtente")
            {
                ConfigurazioneWarningReparto cfgRp = new ConfigurazioneWarningReparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                // Ricerco il gruppo

                bool rt = cfgRp.deleteUtente(new User(e.CommandArgument.ToString()));
                if (rt == true)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl1.Text = "Error.";
                }
            }
        }

        protected void btnSaveWarningUtente_Click(object sender, ImageClickEventArgs e)
        {
            ConfigurazioneWarningReparto cfgRp = new ConfigurazioneWarningReparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);

            if (ddlAddWarningUtente.SelectedValue != "")
            {
                User usr = new User(ddlAddWarningUtente.SelectedValue);
                if (usr.username != "")
                {
                    bool rt = cfgRp.addUser(usr);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text += "Error." + cfgRp.log;
                    }
                }
            }
            btnSaveWarningUtente.Focus();
        }

        protected void btnUndoWarningUtente_Click(object sender, ImageClickEventArgs e)
        {
            ddlAddWarningUtente.SelectedValue = "";
            btnSaveWarningUtente.Focus();
        }
    }
}