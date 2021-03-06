using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Reparti
{
    public partial class listRepartoUtenti : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto Operatori";
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
                    frmAddOperator.Visible = false;
                    if (idReparto != -1)
                    {
                        Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                        if (rp.id != -1)
                        {
                            rp.loadOperatori();
                            rptRepUsers.DataSource = rp.Operatori.Elenco;
                            rptRepUsers.DataBind();
                            UserList elUsr = new UserList(Session["ActiveWorkspace_Name"].ToString());
                            ddlUser.DataSource = elUsr.elencoUtenti;
                            ddlUser.DataTextField = "FullName";
                            ddlUser.DataValueField = "username";
                            ddlUser.DataBind();
                        }
                        else
                        {
                            lbl1.Text = "Err2<br/>";
                        }
                    }
                    else
                    {
                        lbl1.Text = "Error<br/>";
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                ddlUser.Visible = false;
                rptRepUsers.Visible = false;
                imgShowAddFrm.Visible = false;
                imgAddUser.Visible = false;
                frmAddOperator.Visible = false;
            }
        }

        protected void imgShowAddFrm_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddOperator.Visible == false)
            {
                frmAddOperator.Visible = true;
                rptRepUsers.Visible = false;
            }
            else
            {
                frmAddOperator.Visible = false;
                rptRepUsers.Visible = true;
            }
        }

        protected void imgAddUser_Click(object sender, ImageClickEventArgs e)
        {
            Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
            //lbl1.Text = rp.id + " " + ddlUser.SelectedItem.Value;
            rp.loadOperatori();
            bool rt = rp.Operatori.Add(new User(ddlUser.SelectedItem.Value));
            if (rt == true)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl1.Text = rp.Operatori.log;
            }
        }

        protected void rptRepUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "deleteOP")
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                if (rp.id != -1)
                {
                    rp.loadOperatori();
                    bool rt = rp.Operatori.Delete(new User(e.CommandArgument.ToString()));
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorDelete").ToString();
                    }
                }
            }
        }
    }
}