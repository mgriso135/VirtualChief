using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Login
{
    public partial class welcomeBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (((UserAccount)Session["user"]) != null)
                {
                    lblInfoLogin.Text = "<small>"
                        + GetLocalResourceObject("lblWelcome1").ToString()
                        + "&nbsp;"
                        + ((UserAccount)Session["user"]).firstname + " " + ((UserAccount)Session["user"]).lastname + "<br/>"
                        + GetLocalResourceObject("lblWelcome2").ToString()
                    + ": ";// + ((UserAccount)Session["user"]).lastLogin.ToString() + "</small>";
                }
            }
        }
    }
}