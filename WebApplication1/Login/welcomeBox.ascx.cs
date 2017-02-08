using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Login
{
    public partial class welcomeBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (((User)Session["user"]) != null && ((User)Session["user"]).authenticated == true)
                {
                    lblInfoLogin.Text = "<small>Benvenuto&nbsp;" + ((User)Session["user"]).name + " " + ((User)Session["user"]).cognome + "<br/>"
                    + "Last login at: " + ((User)Session["user"]).lastLogin.ToString() + "</small>";
                }
            }
        }
    }
}