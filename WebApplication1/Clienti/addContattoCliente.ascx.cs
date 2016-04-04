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
            imgViewTableAdd.Visible = false;
            imgViewTableAdd.Enabled = false;

            List<String[]> elencoPermessi = new List<String[]>();
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
                imgViewTableAdd.Enabled = true;
                imgViewTableAdd.Visible = true;

                Cliente cln = new Cliente(idCliente);
                if (cln.CodiceCliente.Length > 0)
                {
                    imgViewTableAdd.Enabled = true;
                    if (!Page.IsPostBack)
                    {
                        tblAddContatto.Visible = false;
                    }
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di aggiungere contatti.";
            }
        }

        protected void addContattoCliente_Click(object sender, ImageClickEventArgs e)
        {
            if (tblAddContatto.Visible == true)
            {
                tblAddContatto.Visible = false;
            }
            else
            {
                tblAddContatto.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (idCliente.Length > 0)
            {
                Cliente cln = new Cliente(idCliente);
                if (cln.CodiceCliente.Length > 0)
                {
                    int rt = cln.AddContatto(Server.HtmlEncode(txtNominativo.Text), Server.HtmlEncode(txtRuolo.Text));
                    if (rt >= 0)
                    {
                        lbl1.Text = "Contatto aggiunto.";
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "E' avvenuto un errore. " + cln.log;
                    }
                }
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtNominativo.Text = "";
            txtRuolo.Text = "";
        }
    }
}