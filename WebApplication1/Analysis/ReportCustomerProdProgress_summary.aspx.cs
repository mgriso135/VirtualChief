using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Analysis
{
    public partial class ReportCustomerProdProgress_summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkProdReportINP.Visible = false;
            lnkProdReportF.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["customerID"]))
            {
                Cliente customer = new Cliente(Request.QueryString["customerID"]);
                if (customer.CodiceCliente.Length > 0)
                {
                    lnkProdReportF.Visible = true;
                    lnkProdReportINP.Visible = true;
                    frmSummary.customerID = customer.CodiceCliente;
                    if (!Page.IsPostBack)
                    {
                        lnkProdReportF.NavigateUrl += "?customerID=" + customer.CodiceCliente.ToString();
                        lnkProdReportINP.NavigateUrl += "?customerID=" + customer.CodiceCliente.ToString();
                    }
                }
                else
                {
                    lnkProdReportF.Visible = false;
                    lnkProdReportINP.Visible = false;
                    lbl1.Text = "Cliente non trovato.";
                }
            }
            else
            {
                lbl1.Text = "Errore: dati di input non validi.";
                lnkProdReportF.Visible = false;
                lnkProdReportINP.Visible = false;
            }
        }
    }
}