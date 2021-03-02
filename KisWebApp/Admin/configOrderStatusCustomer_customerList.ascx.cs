using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Admin
{
    public partial class configOrderStatusCustomer_customerList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptCustomerList.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Configurazione Report Stato Ordini Clienti";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                rptCustomerList.Visible = true;
                if (!Page.IsPostBack)
                {
                    PortafoglioClienti customers = new PortafoglioClienti(Session["ActiveWorkspace"].ToString());
                    var customersOrdered = customers.Elenco.OrderBy(x => x.RagioneSociale).ToList();
                    rptCustomerList.DataSource = customersOrdered;
                    rptCustomerList.DataBind();
                }
            }
            else
            {
            }
        }
    }
}