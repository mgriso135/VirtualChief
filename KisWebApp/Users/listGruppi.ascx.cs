using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Users
{
    public partial class listGruppi : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Gruppi";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {

                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    GroupList grl = new GroupList();
                    rptGruppi.DataSource = grl.Elenco;
                    rptGruppi.DataBind();
                }
            }
            else
            {
                rptGruppi.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }


        public void rptListGruppi_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void rptGruppi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Label lbNome = (Label)e.Item.FindControl("lbNome");
            Label lbDesc = (Label)e.Item.FindControl("lbDesc");
            TextBox tbNome = (TextBox)e.Item.FindControl("tbNome");
            TextBox tbDesc = (TextBox)e.Item.FindControl("tbDesc");
            ImageButton btnSave = (ImageButton)e.Item.FindControl("btnSave");
            ImageButton btnDel = (ImageButton)e.Item.FindControl("btnDel");
            listPermessiGruppi lstGruppiPermessi = (listPermessiGruppi)e.Item.FindControl("gruppiPermessi");

            if (e.CommandName == "edit")
            {

                if (lbNome.Visible == true)
                {
                    lbNome.Visible = false;
                    lbDesc.Visible = false;
                    tbNome.Visible = true;
                    tbDesc.Visible = true;
                    btnSave.Visible = true;
                    btnDel.Visible = true;
                }
                else
                {
                    lbNome.Visible = true;
                    lbDesc.Visible = true;
                    tbNome.Visible = false;
                    tbDesc.Visible = false;
                    btnSave.Visible = false;
                    btnDel.Visible = false;
                }
            }
            else if(e.CommandName == "save")
            {
                int idGrp = -1;
                try
                {
                    idGrp = Int32.Parse(e.CommandArgument.ToString());
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
                        grp.Nome = Server.HtmlEncode(tbNome.Text);
                        grp.Descrizione = Server.HtmlEncode(tbDesc.Text);
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                }
            }
            else if (e.CommandName == "delete")
            {
                int idGrp = -1;
                try
                {
                    idGrp = Int32.Parse(e.CommandArgument.ToString());
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
                        grp.Delete();
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                }
            }
            else if(e.CommandName == "mngPermessi")
            {
                int idGrp = -1;
                try
                {
                    idGrp = Int32.Parse(e.CommandArgument.ToString());
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
                        string url = "PermessiGruppi.aspx?ID=" + grp.ID.ToString();
                        string fullURL = "window.open('" + url + "', '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);  
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                }
            }
            else if (e.CommandName == "mngMenu")
            {
                int idGrp = -1;
                try
                {
                    idGrp = Int32.Parse(e.CommandArgument.ToString());
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
                        string url = "../Admin/MenuGruppi.aspx?ID=" + grp.ID.ToString();
                        string fullURL = "window.open('" + url + "', '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblGroupNotFound").ToString();
                }
            }

        }
    }
}