using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Commesse
{
    public partial class wzAddCliente1 : System.Web.UI.UserControl
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
            }
            else
            {
                frmAddcliente.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lbl1.Text = "";
            if (Page.IsValid)
            {
                PortafoglioClienti elenco = new PortafoglioClienti(Session["ActiveWorkspace"].ToString());
                bool checkCod = false;
                bool checkRagSoc = false;
                bool checkPIva = false;
                bool checkCodFiscale = false;

                var codCustomer = txtCodiceCliente.Text.Replace(" ", String.Empty);
                for (int i = 0; i < elenco.Elenco.Count; i++)
                {
                    if (Server.HtmlEncode(codCustomer) == elenco.Elenco[i].CodiceCliente)
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
                    
                    bool rt = elenco.Add(Server.HtmlEncode(codCustomer), Server.HtmlEncode(txtRagSoc.Text),
                        Server.HtmlEncode(txtPartitaIVA.Text), Server.HtmlEncode(txtCodFiscale.Text),
                        Server.HtmlEncode(txtIndirizzo.Text), Server.HtmlEncode(txtCitta.Text),
                        Server.HtmlEncode(txtProvincia.Text), Server.HtmlEncode(txtCAP.Text),
                        Server.HtmlEncode(txtStato.Text), Server.HtmlEncode(txtTelefono.Text),
                        Server.HtmlEncode(txtEmail.Text), kanban);
                    if (rt == true)
                    {
                        Response.Redirect("wzAddCommessa.aspx");
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorInput").ToString();
                    }
                }
                else
                {
                    if (checkCod == true)
                    {
                        lbl1.Text += lbl1.Text = GetLocalResourceObject("lblErrorCodCliente").ToString()+" " + Server.HtmlEncode(txtCodiceCliente.Text) + "<br />";
                    }
                    if (checkRagSoc == true)
                    {
                        lbl1.Text += GetLocalResourceObject("lblErrorRagSociale").ToString() + " " + Server.HtmlEncode(txtRagSoc.Text) + "<br />";
                    }
                    if (checkPIva == true)
                    {
                        lbl1.Text += GetLocalResourceObject("lblErrorPIVA").ToString() + " " + Server.HtmlEncode(txtPartitaIVA.Text) + "<br />";
                    }
                    if (checkCodFiscale == true)
                    {
                        lbl1.Text += GetLocalResourceObject("lblErrorCodFiscale").ToString() + " " + Server.HtmlEncode(txtCodFiscale.Text) + "<br />";
                    }
                }
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
        }*/
    }
}