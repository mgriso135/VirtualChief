using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Produzione
{
    public partial class addNewItemToProductionPlan : System.Web.UI.UserControl
    {
        public int origProcID;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void imgSaveProduct_Click(object sender, EventArgs e)
        {
            if (matricola.Text.Length > 0)
            {
                produzione curr = new produzione(origProcID);
                curr.addProduct(matricola.Text);
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}