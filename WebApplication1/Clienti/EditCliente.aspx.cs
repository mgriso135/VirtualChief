using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Clienti
{
    public partial class EditCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmEditCliente.Visible = false;
            frmContattiClienti.Visible = false;
            frmAddContatto.Visible = false;

            if(!String.IsNullOrEmpty(Request.QueryString["idCliente"]))
            {
                String codCliente = Request.QueryString["idCliente"];
                if (codCliente.Length > 0)
                {
                    frmEditCliente.codCliente = codCliente;
                    frmEditCliente.Visible = true;
                    frmContattiClienti.Visible = true;
                    frmContattiClienti.idCliente = codCliente;
                    frmAddContatto.Visible = true;
                    frmAddContatto.idCliente = codCliente;
                }
            }
            
        }
    }
}