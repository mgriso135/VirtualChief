using System;
using System.Web.UI;
using KIS.App_Code;
using System.Web.Mvc;
using KIS.App_Sources;

namespace KIS.Login
{
    public partial class welcomeBoxMVC : ViewUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (((UserAccount)Session["user"]) != null && ((UserAccount)Session["user"]).id!=-1)
                {
                    lblInfoLogin.Text = "<small>"
                        + GetLocalResourceObject("lblWelcome1").ToString()
                        +"&nbsp;" 
                        + ((UserAccount)Session["user"]).firstname + " " + ((UserAccount)Session["user"]).lastname + "<br/>"
                        + GetLocalResourceObject("lblWelcome2").ToString()
                    +": " + ((UserAccount)Session["user"]).LastLogin.ToString("dd/MM/yyyy HH:mm:ss") + "</small>";
                }
            }
        }
    }
}