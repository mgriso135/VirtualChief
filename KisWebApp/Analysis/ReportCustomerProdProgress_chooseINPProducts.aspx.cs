using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Analysis
{
    public partial class ReportCustomerProdProgress_chooseINPProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmChooseProductsINP.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["customerID"]))
            {
                Cliente cli = new Cliente(Session["ActiveWorkspace"].ToString(), Request.QueryString["customerID"]);
                if (cli.CodiceCliente.Length > 0)
                {
                    frmChooseProductsINP.Visible = true;
                    frmChooseProductsINP.customerID = cli.CodiceCliente;
                    if (!Page.IsPostBack)
                    {
                        
                    }
                }
                else
                {
                    lbl1.Text = "Cliente non trovato.";
                }
            }
            else
            {
                lbl1.Text = "Errore nei dati di input.";
            }
        }
    }
}