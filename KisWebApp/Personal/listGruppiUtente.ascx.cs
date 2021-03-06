using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Personal
{
    public partial class listGruppiUtente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null && Session["ActiveWorkspace_Name"]!=null)
            {
                Workspace ws = new Workspace(Session["ActiveWorkspace_Name"].ToString());
                UserAccount curr = (UserAccount)Session["user"];
                curr.loadGroups(ws.id);
                rptGruppi.DataSource = curr.groups;
                rptGruppi.DataBind();
            }
            else
            {
                upd1.Visible = false;
            }
        }
    }
}