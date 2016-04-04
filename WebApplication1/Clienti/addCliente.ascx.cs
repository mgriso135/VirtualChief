using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Clienti
{
    public partial class addCliente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Anagrafica Clienti";
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
                if (!Page.IsPostBack)
                {
                }
            }
            else
            {
                frmAddcliente.Visible = false;
                lbl1.Text = "Non hai il permesso di aggiungere un utente.";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lbl1.Text = "";
            if (Page.IsValid)
            {
                PortafoglioClienti elenco = new PortafoglioClienti();
                bool checkCod = false;
                bool checkRagSoc = false;
                bool checkPIva = false;
                bool checkCodFiscale = false;
                for (int i = 0; i < elenco.Elenco.Count; i++)
                {
                    if (Server.HtmlEncode(txtCodiceCliente.Text) == elenco.Elenco[i].CodiceCliente)
                    {
                        checkCod = true;
                    }
                    if (Server.HtmlEncode(txtRagSoc.Text) == elenco.Elenco[i].RagioneSociale)
                    {
                        checkRagSoc = true;
                    }
                    if (Server.HtmlEncode(txtPartitaIVA.Text) == elenco.Elenco[i].PartitaIVA && elenco.Elenco[i].PartitaIVA.Length > 1)
                    {
                        checkPIva = true;
                    }
                    if (Server.HtmlEncode(txtCodFiscale.Text) == elenco.Elenco[i].CodiceFiscale && elenco.Elenco[i].CodiceFiscale.Length > 1)
                    {
                        checkCodFiscale = true;
                    }
                }


                if (checkCod == false && checkRagSoc == false && checkRagSoc == false && checkPIva == false && checkCodFiscale == false)
                {
                    Boolean kanban = false;
                    if (ddlKanban.SelectedValue == "1")
                    {
                        kanban = true;
                    }
                    else
                    {
                        kanban = false;
                    }
                    bool rt = elenco.Add(Server.HtmlEncode(txtCodiceCliente.Text), Server.HtmlEncode(txtRagSoc.Text),
                        Server.HtmlEncode(txtPartitaIVA.Text), Server.HtmlEncode(txtCodFiscale.Text),
                        Server.HtmlEncode(txtIndirizzo.Text), Server.HtmlEncode(txtCitta.Text),
                        Server.HtmlEncode(txtProvincia.Text), Server.HtmlEncode(txtCAP.Text),
                        Server.HtmlEncode(txtStato.Text), Server.HtmlEncode(txtTelefono.Text),
                        Server.HtmlEncode(txtEmail.Text), kanban);
                    if (rt == true)
                    {
                        Response.Redirect("Clienti.aspx");
                    }
                    else
                    {
                        lbl1.Text = "E' avvenuto un errore. Verifica i dati inseriti e riprova.";
                    }
                }
                else
                {
                    if (checkCod == true)
                    {
                        lbl1.Text += "Attenzione: esiste già un cliente con codice " + Server.HtmlEncode(txtCodiceCliente.Text) + "<br />";
                    }
                    if (checkRagSoc == true)
                    {
                        lbl1.Text += "Attenzione: esiste già un cliente con ragione sociale " + Server.HtmlEncode(txtRagSoc.Text) + "<br />";
                    }
                    if (checkPIva == true)
                    {
                        lbl1.Text += "Attenzione: esiste già un cliente con partita iva " + Server.HtmlEncode(txtPartitaIVA.Text) + "<br />";
                    }
                    if (checkCodFiscale == true)
                    {
                        lbl1.Text += "Attenzione: esiste già un cliente con codice fiscale " + Server.HtmlEncode(txtCodFiscale.Text) + "<br />";
                    }
                }
            }
        }

        protected void valCodFisc_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Length == 16 || args.Value.Length == 0)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void valPIv_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Length == 11 || args.Value.Length == 0)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void valProvinc_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Length == 2)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtCAP.Text = "";
            txtCitta.Text = "";
            txtCodFiscale.Text = "";
            txtCodiceCliente.Text = "";
            txtEmail.Text = "";
            txtIndirizzo.Text = "";
            txtPartitaIVA.Text = "";
            txtProvincia.Text = "";
            txtRagSoc.Text = "";
            txtStato.Text = "";
            txtTelefono.Text = "";
        }
   
    
    }
}