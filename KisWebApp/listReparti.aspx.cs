using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Reparti
{
    public partial class listReparti1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void showAddReparti_Click(object sender, ImageClickEventArgs e)
        {
            if (addReparti.Visible == false)
            {
                addReparti.Visible = true;
            }
            else
            {
                addReparti.Visible = false;
            }
        }
    }
}