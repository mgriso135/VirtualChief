using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Analysis
{
    public partial class ReportCustomerProdProgress_chooseFProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmChooseProductF.Visible = false;
            lnkProdReportINP.Visible = false;
            lnkProdReportF.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["customerID"]))
            {
                Cliente customer = new Cliente(Request.QueryString["customerID"]);
                if (customer.CodiceCliente.Length > 0)
                {
                    lnkProdReportF.Visible = true;
                    lnkProdReportINP.Visible = true;
                    frmChooseProductF.Visible = true;
                    frmChooseProductF.customerID = customer.CodiceCliente;
                    if (!Page.IsPostBack)
                    {
                        lnkProdReportF.NavigateUrl += "?customerID=" + customer.CodiceCliente.ToString();
                        lnkProdReportINP.NavigateUrl += "?customerID=" + customer.CodiceCliente.ToString();
                    }
                }
                else
                {
                    frmChooseProductF.Visible = false;
                    lnkProdReportF.Visible = false;
                    lnkProdReportINP.Visible = false;
                    lbl1.Text = "Cliente non trovato.";
                }
            }
            else
            {
                lbl1.Text = "Errore: dati di input non validi.";
                frmChooseProductF.Visible = false;
                lnkProdReportF.Visible = false;
                lnkProdReportINP.Visible = false;
            }
        }
    }
}