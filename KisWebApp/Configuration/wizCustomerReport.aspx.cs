using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
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
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
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