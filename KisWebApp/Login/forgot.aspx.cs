using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Login
{
    public partial class forgot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Response.Redirect("login.aspx");
            }
            else if (!Page.IsPostBack)
            {
                frmUsername.Visible = false;
                frmPassword.Visible = false;
            }
        }

        protected void btnScelta_Click(object sender, EventArgs e)
        {
            if (rbMain.SelectedValue == "username")
            {
                frmUsername.Visible = true;
                frmPassword.Visible = false;
            }
            else if (rbMain.SelectedValue == "password")
            {
                frmUsername.Visible = false; 
                frmPassword.Visible = true;
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }
    }
}