using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Configuration
{
    public partial class wizCustomerReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmConfigCustomerReport.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Configurazione Report Stato Ordini Clienti";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                frmConfigCustomerReport.Visible = true;
            }
            else
            {
                frmConfigCustomerReport.Visible = false;
                lbl1.Text = "<a href=\"/Login/login.aspx"
                    + "?red=/Configuration/wizCustomerReport.aspx\">"+GetLocalResourceObject("lblLnkLogin").ToString()
                    +".</a>";
            }
        }
    }
}