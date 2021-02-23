using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Clienti
{
    public partial class addContattoCliente : System.Web.UI.UserControl
    {
        public String idCliente;
        protected void Page_Load(object sender, EventArgs e)
        {
            customerID.Value = idCliente;
            imgViewTableAdd.Visible = false;

            lblErrCustomer.Value = GetLocalResourceObject("lblErrCustomer").ToString();
            lblErrLanguage.Value = GetLocalResourceObject("lblErrLanguage").ToString();
            lblErrUsername.Value = GetLocalResourceObject("lblErrUsername").ToString();
            lblErrPassword.Value = GetLocalResourceObject("lblErrPassword").ToString();
            lblErrUniqueUsername.Value = GetLocalResourceObject("lblErrUniqueUsername").ToString();
            lblGroupNotFound.Value = GetLocalResourceObject("lblGroupNotFound").ToString();
            lblGenericError.Value = GetLocalResourceObject("lblGenericError").ToString();
            lblErrEmail.Value = GetLocalResourceObject("lblErrEmail").ToString();
            lblErrFirstName.Value = GetLocalResourceObject("lblErrFirstName").ToString();
            lblErrLastName.Value = GetLocalResourceObject("lblErrLastName").ToString();


            List <String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Anagrafica Clienti Contatti";
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
                KISConfig cfg = new KISConfig(Session["ActiveWorkspace"].ToString());
                language.Value = cfg.Language;
                imgViewTableAdd.Visible = true;

                Cliente cln = new Cliente(Session["ActiveWorkspace"].ToString(), idCliente);
                if (cln.CodiceCliente.Length > 0)
                {
                    /*if (!Page.IsPostBack)
                    {
                        tblAddContatto.Visible = false;
                    }*/
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                tblAddContatto.Visible = false;
            }
        }

        /*protected void addContattoCliente_Click(object sender, ImageClickEventArgs e)
        {
            if (tblAddContatto.Visible == true)
            {
                tblAddContatto.Visible = false;
            }
            else
            {
                tblAddContatto.Visible = true;
            }
        }*/

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (idCliente.Length > 0)
            {
                Cliente cln = new Cliente(Session["ActiveWorkspace"].ToString(), idCliente);
                if (cln.CodiceCliente.Length > 0)
                {
                    int rt = cln.AddContatto(Server.HtmlEncode(txtFirstName.Text), Server.HtmlEncode(txtLastName.Text), 
                        Server.HtmlEncode(txtRuolo.Text));
                    if (rt >= 0)
                    {
                        lbl1.Text = GetLocalResourceObject("lblAddOk").ToString();
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblError").ToString() + cln.log;
                    }
                }
            }
        }

    }
}