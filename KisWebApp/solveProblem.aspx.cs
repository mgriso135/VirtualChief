using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Produzione
{
    public partial class solveProblem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int wID = -1;
                try
                {
                    wID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    wID = -1;
                }
                if (wID != -1)
                {
                    frmSolveProblem.idWarning = wID;
                }
                else
                {
                    frmSolveProblem.Visible = false;
                }
            }
            else
            {
                frmSolveProblem.Visible = false;
            }
        }
    }
}