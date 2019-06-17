using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Postazioni
{
    public partial class managePostazioni : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                addPostazioni.Visible = false;
            }
        }

        protected void imgShowAddPostazioni_Click(object sender, EventArgs e)
        {
            if (addPostazioni.Visible == false)
            {
                addPostazioni.Visible = true;
            }
            else
            {
                addPostazioni.Visible = false;
            }
        }
    }
}