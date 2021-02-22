using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Analysis
{
    public partial class ReportCustomerProdProgress_chooseCustomer1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Report Stato Ordini Clienti";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    lnkGoFwd.Visible = false;
                    imgGoFwd.Visible = false;
                    PortafoglioClienti pClienti = new PortafoglioClienti(Session["ActiveWorkspace"].ToString());
                    var clientiSorted = pClienti.Elenco.OrderBy(x => x.RagioneSociale);
                    rblClienti.DataSource = clientiSorted;
                    rblClienti.DataValueField = "CodiceCliente";
                    rblClienti.DataTextField = "RagioneSociale";
                    rblClienti.DataBind();
                }
            }
        }

        protected void rblClienti_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cliente customer = new Cliente(rblClienti.SelectedValue);
            if (customer.CodiceCliente.Length > 0)
            {
                lnkGoFwd.Visible = true;
                imgGoFwd.Visible = true;
                lnkGoFwd.NavigateUrl = "ReportCustomerProdProgress_chooseINPProducts.aspx?customerID=" + customer.CodiceCliente;
                lbl1.Text = GetLocalResourceObject("lblClienteSelezionato") + ": " + customer.RagioneSociale;
                lnkGoFwd.Focus();
            }
            else
            {
                lnkGoFwd.Visible = false;
                imgGoFwd.Visible = false;
            }
        }
    }
}