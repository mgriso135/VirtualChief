using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Analysis
{
    public partial class DetailAnalysisCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmDetailCustomer.Visible = false;
            lblThisLink.Visible = false;
            lblTitle.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Clienti";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                
                String customerID = Request.QueryString["customerID"];
                Cliente customer = new Cliente(customerID);
                if (customer.CodiceCliente.Length > 0)
                {
                    lblThisLink.Visible = true;                    
                    frmDetailCustomer.Visible = true;
                    //frmDetailCustomer.customer = customer;
                    frmDetailCustomer.Visible = true;
                    lblTitle.Visible = true;
                    if (!Page.IsPostBack)
                    {
                        lblTitle.InnerHtml = "Analisi cliente " + customer.RagioneSociale;
                        lblThisLink.NavigateUrl = "DetailAnalysisCustomer.aspx?customerID=" + customer.CodiceCliente;
                    }
                }
                else
                {
                    lblThisLink.Visible = false;
                    lblThisLink.NavigateUrl = "";
                    frmDetailCustomer.Visible = false;
                }
            }
            else
            {
                lblThisLink.Visible = false;
                frmDetailCustomer.Visible = false;
            }
        }
    }
}