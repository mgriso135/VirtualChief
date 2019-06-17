using System;
using System.Web.UI;
using KIS.App_Code;
using System.Web.Mvc;

namespace KIS.Login
{
    public partial class welcomeBoxMVC : ViewUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (((User)Session["user"]) != null && ((User)Session["user"]).authenticated == true)
                {
                    lblInfoLogin.Text = "<small>"
                        + GetLocalResourceObject("lblWelcome1").ToString()
                        +"&nbsp;" 
                        + ((User)Session["user"]).name + " " + ((User)Session["user"]).cognome + "<br/>"
                        + GetLocalResourceObject("lblWelcome2").ToString()
                    +": " + ((User)Session["user"]).lastLogin.ToString() + "</small>";
                }
            }
        }
    }
}