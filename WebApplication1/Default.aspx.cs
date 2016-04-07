﻿using System;
/*using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using KIS;*/

namespace KIS
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                lbl1.Text = ((User)Session["user"]).name + " " + ((User)Session["user"]).cognome +
                    "<br/>Last login: " + ((User)Session["user"]).lastLogin.ToString();
            }
            else
            {
                lbl1.Text = "You're not logged in. Please <a href=\"/Login/login.aspx\">log in</a>.";
            }
        }
    }

}
