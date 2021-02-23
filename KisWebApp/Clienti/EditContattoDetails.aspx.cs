using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Clienti
{
    public partial class EditContattoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkModCustomer.Visible = false;
            lnkContattoDetail.Visible = false;
            frmEditContatto.Visible = false;
            String strIDContatto = Request.QueryString["idContatto"];
            if (!String.IsNullOrEmpty(strIDContatto))
            {
                int idCont = -1;
                try
                {
                    idCont = Int32.Parse(strIDContatto);
                }
                catch
                {
                    idCont = -1;
                }

                if (idCont != -1)
                {
                    Contatto contCln = new Contatto(Session["ActiveWorkspace"].ToString(), idCont);
                    if (contCln.ID != -1)
                    {
                        frmEditContatto.Visible = true;
                        frmEditContatto.idContatto = contCln.ID;
                        lnkModCustomer.Visible = true;
                        lnkModCustomer.NavigateUrl += "?idCliente="+contCln.Cliente;
                        lnkContattoDetail.Visible = true;
                        lnkContattoDetail.NavigateUrl += "?idContatto=" + contCln.ID.ToString();
                    }
                }
            }
        }
    }
}