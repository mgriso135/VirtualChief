using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Web.UI.HtmlControls;
using KIS;

namespace KIS.Admin
{
    public partial class managePermessi : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            lnkLogin.Visible = false;


        }
    }
}