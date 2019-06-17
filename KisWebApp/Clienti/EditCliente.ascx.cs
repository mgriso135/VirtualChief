using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;


namespace KIS.Clienti
{
    public partial class EditCliente1 : System.Web.UI.UserControl
    {
        public String codCliente;
        protected void Page_Load(object sender, EventArgs e)
        {
            tblData.Visible = false;
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
                if (!String.IsNullOrEmpty(codCliente) && codCliente.Length > 0)
                {
                    Cliente cli = new Cliente(codCliente);
                    if (cli.CodiceCliente.Length > 0)
                    {
                        tblData.Visible = true;
                        if (!Page.IsPostBack)
                        {
                            txtCodCli.Text = Server.HtmlDecode(cli.CodiceCliente);
                            txtRagSoc.Text = Server.HtmlDecode(cli.RagioneSociale);
                            txtCodFiscale.Text = Server.HtmlDecode(cli.CodiceFiscale);
                            txtPIva.Text = Server.HtmlDecode(cli.PartitaIVA);
                            txtIndirizzo.Text = Server.HtmlDecode(cli.Indirizzo);
                            txtCitta.Text = Server.HtmlDecode(cli.Citta);
                            txtProvincia.Text = Server.HtmlDecode(cli.Provincia);
                            txtCAP.Text = Server.HtmlDecode(cli.CAP);
                            txtStato.Text = Server.HtmlDecode(cli.Stato);
                            txtTelefono.Text = Server.HtmlDecode(cli.Telefono);
                            txtEmail.Text = Server.HtmlDecode(cli.Email);

                            if (cli.KanbanManaged == true)
                            {
                                ddlKanban.SelectedValue = "1";
                            }
                            else
                            {
                                ddlKanban.SelectedValue = "0";
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                if (codCliente.Length > 0)
                {
                    Cliente cli = new Cliente(codCliente);
                    if (cli.CodiceCliente.Length > 0)
                    {
                        cli.RagioneSociale = Server.HtmlDecode(txtRagSoc.Text);
                        cli.CodiceFiscale = Server.HtmlEncode(txtCodFiscale.Text);
                        cli.PartitaIVA = Server.HtmlEncode(txtPIva.Text);
                        cli.Indirizzo = Server.HtmlEncode(txtIndirizzo.Text);
                        cli.Citta = Server.HtmlEncode(txtCitta.Text);
                        cli.Provincia = Server.HtmlEncode(txtProvincia.Text);
                        cli.CAP = Server.HtmlEncode(txtCAP.Text);
                        cli.Stato = Server.HtmlEncode(txtStato.Text);
                        cli.Telefono = Server.HtmlEncode(txtTelefono.Text);
                        cli.Email = Server.HtmlEncode(txtEmail.Text);
                        if (ddlKanban.SelectedValue == "1")
                        {
                            cli.KanbanManaged = true;
                        }
                        else
                        {
                            cli.KanbanManaged = false;
                        }

                        lbl1.Text = GetLocalResourceObject("lblUpdateOk").ToString();
                    }
                }
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            tblData.Visible = false;
            if (codCliente.Length > 0)
            {
                Cliente cli = new Cliente(codCliente);
                if (cli.CodiceCliente.Length > 0)
                {
                        tblData.Visible = true;
                        txtCodCli.Text = Server.HtmlDecode(cli.CodiceCliente);
                        txtRagSoc.Text = Server.HtmlDecode(cli.RagioneSociale);
                        txtCodFiscale.Text = Server.HtmlDecode(cli.CodiceFiscale);
                        txtPIva.Text = Server.HtmlDecode(cli.PartitaIVA);
                        txtIndirizzo.Text = Server.HtmlDecode(cli.Indirizzo);
                        txtCitta.Text = Server.HtmlDecode(cli.Citta);
                        txtProvincia.Text = Server.HtmlDecode(cli.Provincia);
                        txtCAP.Text = Server.HtmlDecode(cli.CAP);
                        txtStato.Text = Server.HtmlDecode(cli.Stato);
                        txtTelefono.Text = Server.HtmlDecode(cli.Telefono);
                        txtEmail.Text = Server.HtmlDecode(cli.Email);
                    }
                
            }
        }

        /*protected void valCodFiscale_ServerValidate(object source, ServerValidateEventArgs args)
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

        protected void valPIva_ServerValidate(object source, ServerValidateEventArgs args)
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

        protected void valProvincia_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Length == 2)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }*/
    }
}