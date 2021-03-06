using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Admin
{
    public partial class configOrderStatusCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmConfigOrderCustomer.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                String sCustomer = Request.QueryString["id"];
                Cliente customer = new Cliente(Session["ActiveWorkspace_Name"].ToString(), sCustomer);
                if (customer.CodiceCliente.Length > 0)
                {
                    frmConfigOrderCustomer.Visible = true;
                    frmConfigOrderCustomer.codCliente = customer.CodiceCliente;
                    lnkCurrentPage.Visible = true;
                    lnkCurrentPage.NavigateUrl = "~/Admin/configOrderStatusCustomer.aspx?id="+customer.CodiceCliente.ToString();
                }
                else
                {
                    lnkCurrentPage.Visible = false;
                }
            }
            else
            {
                lnkCurrentPage.Visible = false;
            }
        }
    }
}