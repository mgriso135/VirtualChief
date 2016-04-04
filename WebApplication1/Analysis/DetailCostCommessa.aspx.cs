using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;

namespace KIS.Analysis
{
    public partial class DetailCostCommessa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int annoComm = -1, idComm = -1;
            frmCostDetail.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]) && !String.IsNullOrEmpty(Request.QueryString["Year"]))
            {
                try
                {
                    idComm = Int32.Parse(Request.QueryString["ID"]);
                    annoComm = Int32.Parse(Request.QueryString["Year"]);
                }
                catch
                {
                    idComm = -1;
                    annoComm = -1;
                }

                if (idComm != -1 && annoComm != -1)
                {
                    Commessa cm = new Commessa(idComm, annoComm);
                    if (cm.ID != -1 && cm.Year != -1)
                    {
                        frmCostDetail.commID = cm.ID;
                        frmCostDetail.commYear = cm.Year;
                        frmCostDetail.Visible = true;
                        lnkMenu.HRef += "ID=" + cm.ID.ToString() + "&Year=" + cm.Year.ToString();
                    }
                }
            }
            else
            {
                
            }
        }
    }
}