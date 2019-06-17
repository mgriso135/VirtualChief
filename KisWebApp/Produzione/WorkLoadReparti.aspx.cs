using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Produzione
{
    public partial class WorkLoadReparti : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idRep = -1;
            try
            {
                idRep = Int32.Parse(Request.QueryString["id"]);
            }
            catch
            {
                idRep = -1;
            }

            if (idRep != -1)
            {
                frmListReparto.idReparto = idRep;
            }
            else
            {
                frmListReparto.Visible = false;
            }
        }
    }
}