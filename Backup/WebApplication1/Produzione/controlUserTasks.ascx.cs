using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Produzione
{
    public partial class controlUserTasks : System.Web.UI.UserControl
    {
        public string user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(user) && user.Length > 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Utente", "var utente = " + user + ";", true);
            }
        }
    }
}