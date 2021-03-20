using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Configuration
{
    public partial class MainWizConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.section = "";
            if (Session["ActiveWorkspace_Id"] != null && Session["IsWorkspaceAdmin"] != null && Session["IsWorkspaceAdmin"].ToString() == "1")
            {
                frmConfigStatus.Visible = true;
            }
            else
            {
                frmConfigStatus.Visible = false;
            }
        }
    }
}