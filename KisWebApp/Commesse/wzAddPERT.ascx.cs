using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using System.Web.Services;
using KIS.App_Sources;

namespace KIS.Commesse
{
    public partial class wzAddPERT1 : System.Web.UI.UserControl
    {
        public int idCommessa;
        public int annoCommessa;
        protected void Page_Load(object sender, EventArgs e)
        {
            tblAddPERT.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (idCommessa != -1 && annoCommessa != -1)
                {
                    Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), idCommessa, annoCommessa);
                    if (cm.ID != -1 && cm.Year != -1)
                    {                       

                        tblAddPERT.Visible = true;
                        if (!Page.IsPostBack)
                        {
                            ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                            var sorted = el.elencoFigli.OrderBy(x => x.NomeCombinato);

                            ddlCopiaPERT.DataSource = sorted;
                            
                            ddlCopiaPERT.DataValueField = "IDCombinato";
                            ddlCopiaPERT.DataTextField = "NomeCombinato";
                            ddlCopiaPERT.DataBind();

                            lblTitle.Text = GetLocalResourceObject("lblCliente").ToString()
                                +": " + cm.Cliente;

                            PortafoglioClienti portCli = new PortafoglioClienti(Session["ActiveWorkspace_Name"].ToString());
                            ddlCopiaPERTClienti.DataSource = portCli.Elenco;
                            ddlCopiaPERTClienti.DataValueField = "CodiceCliente";
                            ddlCopiaPERTClienti.DataTextField = "RagioneSociale";
                            ddlCopiaPERTClienti.DataBind();

                            ddlStdFiltroCliente.DataSource = portCli.Elenco;
                            ddlStdFiltroCliente.DataValueField = "CodiceCliente";
                            ddlStdFiltroCliente.DataTextField = "RagioneSociale";
                            ddlStdFiltroCliente.DataBind();

                            var sortedStd = el.elencoFigli.OrderBy(x => x.NomeCombinato);
                            //ElencoProcessiVarianti elProdStd = new ElencoProcessiVarianti(true);
                            ddlAddProdStandard.DataSource = sortedStd;
                            ddlAddProdStandard.DataValueField = "IDCombinato2";
                            ddlAddProdStandard.DataTextField = "NomeCombinato";
                            ddlAddProdStandard.DataBind();
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void btnCopiaPERT_Click(object sender, EventArgs e)
        {
            elencoVarianti elVar = new elencoVarianti(Session["ActiveWorkspace_Name"].ToString());
            bool result = elVar.elenco.Any(v => v.nomeVariante == txtNomeCopiaProd.Text);
            if (result)
            {
                trCopyConfirm.Visible = true;
                txtNomeCopiaProd.Enabled = false;
                txtDescCopiaProd.Enabled = false;
                txtQtyCopiaProd.Enabled = false;
                ddlCopiaPERT.Enabled = false;
                ddlCopiaPERTClienti.Enabled = false;
                btnCopiaPERT.Visible = false;
                imgCopyConfirmOK.Focus();
            }
            else
            {
                addCopyProduct();
            }
        }

        protected void chkCopiaReparti_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopiaPostazioni.Checked == true)
            {
                chkCopiaReparti.Checked = true;
            }
        }

        protected void chkCopiaPostazioni_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopiaReparti.Checked == false)
            {
                chkCopiaPostazioni.Checked = false;
            }
        }

        protected void ddlCopiaPERTClienti_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCopiaPERTClienti.SelectedValue.Length > 0)
            {
                String codCliente = Server.HtmlEncode(ddlCopiaPERTClienti.SelectedValue);
                Cliente customer = new Cliente(Session["ActiveWorkspace_Name"].ToString(), codCliente);
                ElencoProcessiVarianti el = null;
                if (customer.CodiceCliente.Length > 0)
                {
                    el = new ElencoProcessiVarianti(true, customer);
                }
                else
                {
                     el = new ElencoProcessiVarianti(true);
                }
                var sorted = el.elencoFigli.OrderBy(x => x.process.processName).ThenBy(y => y.variant.nomeVariante);

                ddlCopiaPERT.DataSource = sorted;

                ddlCopiaPERT.DataValueField = "IDCombinato";
                ddlCopiaPERT.DataTextField = "NomeCombinato";
                ddlCopiaPERT.DataBind();
                ddlCopiaPERT.Focus();
            }
            else
            {
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                var sorted = el.elencoFigli.OrderBy(x => x.process.processName).ThenBy(y => y.variant.nomeVariante);

                ddlCopiaPERT.DataSource = sorted;

                ddlCopiaPERT.DataValueField = "IDCombinato";
                ddlCopiaPERT.DataTextField = "NomeCombinato";
                ddlCopiaPERT.DataBind();
                ddlCopiaPERT.Focus();
            }
        }

        protected void addProdStd_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlAddProdStandard.SelectedValue.Length > 0)
            {
                lbl1.Text = "Aggiungo un prodotto standard.<br/>" + ddlAddProdStandard.SelectedValue.ToString();
                lbl1.Text += ddlAddProdStandard.SelectedValue.ToString() + "<br/>";
                int idProc = -1, rev = -1, idVar = -1, qty=-1;
                try
                {
                    String[] splitted = ddlAddProdStandard.SelectedValue.ToString().Split('/');
                    idProc = Int32.Parse(splitted[0]);
                    rev = Int32.Parse(splitted[1]);
                    idVar = Int32.Parse(splitted[2]);
                    qty = Int32.Parse(txtStdQty.Text);
                }
                catch
                {
                    idProc = -1;
                    rev = -1;
                    idVar = -1;
                    qty = -1;
                }

                if (idProc != -1 && rev != -1 && idVar != -1 && qty > 0)
                {
                    Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), idCommessa, annoCommessa);
                    if (cm.ID != -1 && cm.Year != -1)
                    {
                        ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), idProc, rev), new variante(Session["ActiveWorkspace_Name"].ToString(), idVar));
                        prcVar.loadReparto();
                        prcVar.process.loadFigli(prcVar.variant);
                        if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.variant.idVariante != -1)
                        {
                            Response.Redirect("wzAssociaPERTReparto.aspx?idCommessa=" + cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString()
                            + "&idProc=" + prcVar.process.processID.ToString()
                            + "&revProc=" + prcVar.process.revisione.ToString()
                            + "&idVariante=" + prcVar.variant.idVariante.ToString()
                            + "&idProdotto=-1&annoProdotto=-1&quantita="
                            + qty.ToString()
                            +"&matricola=" + Server.HtmlEncode(txtStdMatricola.Text.ToString()));
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrProdStdInesistente").ToString();
                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrOrdineNotFound").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrQta").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrNoProdSelected").ToString();
            }
        }

        protected void ddlStdFiltroCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (ddlStdFiltroCliente.SelectedValue.Length > 0)
            {
                String codCliente = Server.HtmlEncode(ddlStdFiltroCliente.SelectedValue);
                Cliente customer = new Cliente(Session["ActiveWorkspace_Name"].ToString(), codCliente);
                ElencoProcessiVarianti el = null;
                if (customer.CodiceCliente.Length > 0)
                {
                    el = new ElencoProcessiVarianti(true, customer);
                }
                else
                {
                     el = new ElencoProcessiVarianti(true);
                }

                var sorted = el.elencoFigli.OrderBy(x => x.process.processName).ThenBy(y => y.variant.nomeVariante);

                ddlAddProdStandard.DataSource = el.elencoFigli;
                ddlAddProdStandard.DataValueField = "IDCombinato2";
                ddlAddProdStandard.DataTextField = "NomeCombinato";
                ddlAddProdStandard.DataBind();
                ddlAddProdStandard.Focus();
            }
            else
            {
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                var sorted = el.elencoFigli.OrderBy(x => x.process.processName).ThenBy(y => y.variant.nomeVariante);

                ddlAddProdStandard.Items.Clear();
                
                ddlAddProdStandard.Items.Add(new ListItem("", GetLocalResourceObject("lblNoSelection").ToString()));
                ddlAddProdStandard.DataSource = sorted;
                ddlAddProdStandard.DataValueField = "IDCombinato2";
                ddlAddProdStandard.DataTextField = "NomeCombinato";
                ddlAddProdStandard.DataBind();
                ddlAddProdStandard.Focus();
            }
        }

        protected void addVariante_Click(object sender, EventArgs e)
        {
            addBlankProduct();
        }

        protected void addVariante_Click1(object sender, ImageClickEventArgs e)
        {
            elencoVarianti elVar = new elencoVarianti(Session["ActiveWorkspace_Name"].ToString());
            bool result = elVar.elenco.Any(v => v.nomeVariante == txtNomeBlankProd.Text);
            if (result)
            {
                trBlankConfirm.Visible = true;
                txtNomeBlankProd.Enabled = false;
                txtDescBlankProd.Enabled = false;
                txtQtyBlank.Enabled = false;
                addVariante.Visible = false;
                imgBlankConfirmOK.Focus();
            }
            else
            {
                addBlankProduct();
            }
        }

        protected void addBlankProduct()
        {
            trBlankConfirm.Visible = false;
            txtNomeBlankProd.Enabled = true;
            txtDescBlankProd.Enabled = true;
            txtQtyBlank.Enabled = true;
            addVariante.Visible = true;

            // Cerco se esiste un processo di tipo PERT con il nome del cliente
            Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), idCommessa, annoCommessa);
            macroProcessi elProc = new macroProcessi(Session["ActiveWorkspace_Name"].ToString());
            List<int[]> lstProc = elProc.FindByName(cm.Cliente);
            int prcID = -1;
            int prcRev = -1;
            for (int i = 0; i < lstProc.Count; i++)
            {
                lbl1.Text += lstProc[i][0] + " " + lstProc[i][1] + "<br />";
                processo prc = new processo(Session["ActiveWorkspace_Name"].ToString(), lstProc[i][0], lstProc[i][1]);
                if (prc.isVSM == false)
                {
                    prcID = prc.processID;
                    prcRev = prc.revisione;
                    break;
                }
            }

            int qty = -1;
            try
            {
                qty = Int32.Parse(txtQtyBlank.Text);
            }
            catch
            {
                qty = -1;
            }

            if (qty > 0)
            {

                // Se non ho trovato un macroprocesso corrispondente, creo un nuovo macroprocesso
                if (prcID == -1 && prcRev == -1)
                {
                    // Creo un nuovo processo
                    macroProcessi elMacroProc = new macroProcessi(Session["ActiveWorkspace_Name"].ToString());
                    bool check = elMacroProc.Add(cm.Cliente, cm.Cliente, false);
                    if (check == false)
                    {
                        lbl1.Text = GetLocalResourceObject("lblErr").ToString() + "<br />";
                    }
                    else
                    {
                        elMacroProc = new macroProcessi(Session["ActiveWorkspace_Name"].ToString());
                        List<int[]> res = elMacroProc.FindByName(cm.Cliente);
                        prcID = res[0][0];
                        prcRev = res[0][1];
                    }
                }

                // Aggiungo la variante al processo
                if (prcID != -1 && prcRev != -1)
                {
                    processo proc = new processo(Session["ActiveWorkspace_Name"].ToString(), prcID, prcRev);
                    variante var = new variante(Session["ActiveWorkspace_Name"].ToString());

                    int varID = var.add(Server.HtmlEncode(txtNomeBlankProd.Text), Server.HtmlEncode(txtDescBlankProd.Text));
                    if (varID == -1)
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrAddingProduct").ToString();
                    }
                    else
                    {
                        bool retAddProcVar = proc.addVariante(new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                        if (retAddProcVar == true)
                        {
                            String page = "wzEditPERT.aspx";
                            WizardConfig wizCfg = new WizardConfig();
                            if (wizCfg.interfacciaPERT == "Table")
                            {
                                page = "wzEditPERT_updtable.aspx";
                            }
                            else
                            {
                                page = "wzEditPERT.aspx";
                            }
                            Response.Redirect(page + "?idCommessa=" + cm.ID.ToString() + "&annoCommessa="
                                + cm.Year.ToString() + "&idProc=" + prcID.ToString()
                                + "&revProc=" + prcRev.ToString() + "&idVariante=" + varID.ToString()
                                + "&idProdotto=-1"
                                + "&annoProdotto=-1&quantita="
                                + qty.ToString()
                                + "&matricola=" + Server.HtmlEncode(txtMatricolaBlank.Text.ToString()));
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrAddingProduct").ToString();
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrQta").ToString();
            }
        }

        protected void imgBlankConfirmKO_Click(object sender, ImageClickEventArgs e)
        {
            trBlankConfirm.Visible = false;
            txtNomeBlankProd.Enabled = true;
            txtDescBlankProd.Enabled = true;
            txtQtyBlank.Enabled = true;
            addVariante.Visible = true;
        }

        protected void imgCopyConfirmOK_Click(object sender, ImageClickEventArgs e)
        {
            addCopyProduct();
        }

        protected void imgCopyConfirmKO_Click(object sender, ImageClickEventArgs e)
        {
            trCopyConfirm.Visible = false;
            txtNomeCopiaProd.Enabled = true;
            txtDescCopiaProd.Enabled = true;
            txtQtyCopiaProd.Enabled = true;
            ddlCopiaPERT.Enabled = true;
            ddlCopiaPERTClienti.Enabled = true;
            btnCopiaPERT.Visible = true;
        }

        protected void addCopyProduct()
        {
            Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), idCommessa, annoCommessa);
            int procID = -1;
            int varID = -1;
            int currProc = -1;
            int qty = -1;


            String[] splitted = ddlCopiaPERT.SelectedValue.Split(',');
            try
            {
                procID = Int32.Parse(splitted[0]);
                varID = Int32.Parse(splitted[1]);
                qty = Int32.Parse(txtQtyCopiaProd.Text);
            }
            catch
            {
                procID = -1;
                varID = -1;
                qty = -1;
            }

            if (qty > 0)
            {

                macroProcessi elProc = new macroProcessi(Session["ActiveWorkspace_Name"].ToString());
                List<int[]> lstProc = elProc.FindByName(cm.Cliente);
                int prcID = -1;
                int prcRev = -1;
                for (int i = 0; i < lstProc.Count; i++)
                {
                    lbl1.Text += lstProc[i][0] + " " + lstProc[i][1] + "<br />";
                    processo prc = new processo(Session["ActiveWorkspace_Name"].ToString(), lstProc[i][0], lstProc[i][1]);
                    if (prc.isVSM == false)
                    {
                        prcID = prc.processID;
                        prcRev = prc.revisione;
                        break;
                    }
                }

                // Se non ho trovato un macroprocesso corrispondente, creo un nuovo macroprocesso
                if (prcID == -1 && prcRev == -1)
                {
                    // Creo un nuovo processo
                    macroProcessi elMacroProc = new macroProcessi(Session["ActiveWorkspace_Name"].ToString());
                    bool check = elMacroProc.Add(cm.Cliente, cm.Cliente, false);
                    if (check == false)
                    {
                        lbl1.Text = GetLocalResourceObject("lblErr").ToString() + "<br />";
                    }
                    else
                    {
                        elMacroProc = new macroProcessi(Session["ActiveWorkspace_Name"].ToString());
                        List<int[]> res = elMacroProc.FindByName(cm.Cliente);
                        prcID = res[0][0];
                        prcRev = res[0][1];
                    }
                }

                if (prcID != -1 && prcRev != -1 && varID != -1)
                {
                    ProcessoVariante daCopiare = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), procID), new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                    daCopiare.loadReparto();
                    daCopiare.process.loadFigli(daCopiare.variant);
                    processo curr = new processo(Session["ActiveWorkspace_Name"].ToString(), prcID, prcRev);
                    if (curr != null && daCopiare != null && curr.processID != -1 && daCopiare.process != null && daCopiare.variant != null)
                    {
                        lbl1.Text = "curr: " + curr.processID.ToString() + "<br />"
                            + "daCopiare: " + daCopiare.process.processID.ToString() + " - " + daCopiare.variant.idVariante.ToString() + "<br />";
                        bool flagCopia = !chkCopia.Checked;
                        bool flagCopiaTC = chkCopiaTC.Checked;
                        bool flagCopiaReparto = chkCopiaReparti.Checked;
                        bool flagCopiaPostazioni = chkCopiaPostazioni.Checked;
                        bool flagCopyParameters = chkCopyParameters.Checked;
                        bool flagCopyWorkInstructions = chkCopyWorkInstructions.Checked;
                        if (chkCopiaPostazioni.Checked && !chkCopiaReparti.Checked)
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrPostRep").ToString();
                        }
                        else
                        {
                            int newVarID = daCopiare.CopyTo(curr, Server.HtmlEncode(txtNomeCopiaProd.Text), Server.HtmlEncode(txtDescCopiaProd.Text), flagCopia, flagCopiaTC, flagCopiaReparto, flagCopiaPostazioni, flagCopyParameters, flagCopyWorkInstructions);
                            if (newVarID != -1)
                            {
                                String page = "wzEditPERT.aspx";
                                WizardConfig wizCfg = new WizardConfig();
                                if (wizCfg.interfacciaPERT == "Table")
                                {
                                    page = "wzEditPERT_updtable.aspx";
                                }
                                else
                                {
                                    page = "wzEditPERT.aspx";
                                }
                                Response.Redirect(page + "?idCommessa=" + cm.ID.ToString() + "&annoCommessa="
                                + cm.Year.ToString() + "&idProc=" + prcID.ToString()
                                + "&revProc=" + prcRev.ToString() + "&idVariante=" + newVarID.ToString()
                                + "&idProdotto=-1"
                                + "&annoProdotto=-1&quantita="
                                + qty.ToString()
                                +"&matricola="+ Server.HtmlEncode(txtMatricolaCopiaProd.Text.ToString()));
                            }
                            else
                            {
                                lbl1.Text = GetLocalResourceObject("lblErrPartialCopy").ToString() + "<br />";
                            }
                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErr").ToString() + "<br />";
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrQta").ToString() + "<br />";
            }
        }
    }
}