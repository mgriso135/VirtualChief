using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Admin
{
    public partial class addUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String permessoRichiesto = "Utenti";
            bool checkUser = false;
            if (Session["User"] != null && Session["ActiveWorkspace_Name"]!=null)
            {
                Workspace ws = new Workspace(Session["ActiveWorkspace_Name"].ToString());
                UserAccount curr = (UserAccount)Session["user"];
                curr.loadGroups(ws.id);
                for (int i = 0; i < curr.groups.Count; i++)
                {
                    for (int j = 0; j < curr.groups[i].Permissions.Elenco.Count; j++)
                    {
                        if (curr.groups[i].Permissions.Elenco[j].NomePermesso == permessoRichiesto && curr.groups[i].Permissions.Elenco[j].W == true)
                        {
                            checkUser = true;
                        }
                    }
                }
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    tblInputNewUser.Visible = false;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                tblInputNewUser.Visible = false;
                btnUserAdd.Visible = false;
                btnUserAdd.Enabled = false;
            }

        }

        protected void btnUserAdd_Click(object sender, EventArgs e)
        {
            btnUserAdd.Visible = false;
            tblInputNewUser.Visible = true;
        }

        protected void btnUndo_Click(object sender, EventArgs e)
        {
            btnUserAdd.Visible = true;
            tblInputNewUser.Visible = false;
        }

        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            User utn = new User(Session["ActiveWorkspace_Name"].ToString());
            String rt = utn.add(inputUsername.Text, inputPassword.Text, inputNome.Text, inputCognome.Text, 
                tipoUtente.SelectedValue, ddlLanguages.SelectedValue,
                true,
                null);
            if (rt == "2")
            {
                lbl1.Text = GetLocalResourceObject("lblUserYaexistente").ToString();
            }
            else if (rt == "0")
            {
                lblEsito.Text = GetLocalResourceObject("lblErrorGen").ToString()+ "<br/>" + utn.log;
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }
            
        }
    }
}