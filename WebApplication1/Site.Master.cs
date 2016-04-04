using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using KIS.App_Code;

namespace KIS
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Logo lg = new Logo();
            kisLogo.ImageUrl = lg.filePath;
            kisLogo.Height = 50;
        }
    }
}
