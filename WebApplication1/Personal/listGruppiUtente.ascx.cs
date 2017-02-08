using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Personal
{
    public partial class listGruppiUtente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                curr.loadGruppi();
                rptGruppi.DataSource = curr.Gruppi;
                rptGruppi.DataBind();
            }
            else
            {
                upd1.Visible = false;
            }
        }
    }
}