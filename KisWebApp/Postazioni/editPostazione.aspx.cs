using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Postazioni
{
    public partial class editPostazione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int pstID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["pstID"]))
            {
                try
                {
                    pstID = Int32.Parse(Request.QueryString["pstID"]);
                }
                catch
                {
                    pstID = -1;
                }
                if (pstID != -1)
                {
                    editPst.pstID = pstID;
                }
            }
        }
    }
}