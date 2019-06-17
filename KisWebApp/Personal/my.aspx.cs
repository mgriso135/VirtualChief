using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Personal
{
    public partial class my : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                lblLogin.Visible = false;
                accordion1.Visible = true;
                lblUsername.Text = ((User)Session["user"]).username;
                lblDatiPersonali.Visible = true;
            }
            else
            {
                lblDatiPersonali.Visible = false;
                lblLogin.Visible = true;
                accordion1.Visible = false;
            }
        }
    }
}