using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Admin
{
    public partial class addUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tblInputNewUser.Visible = false;
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
            User utn = new User();
            int rt = utn.add(inputUsername.Text, inputPassword.Text, inputNome.Text, inputCognome.Text, tipoUtente.SelectedValue);
            if (rt == 2)
            {
                lblEsito.Text = "Error: username già presente";
            }
            else if (rt == 0)
            {
                lblEsito.Text = "Error: generic";
            }
            else
            {
                lblEsito.Text = "Utente aggiunto correttamente";
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}