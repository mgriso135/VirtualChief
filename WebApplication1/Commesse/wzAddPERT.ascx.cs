using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

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
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (idCommessa != -1 && annoCommessa != -1)
                {
                    Commessa cm = new Commessa(idCommessa, annoCommessa);
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

                            lblTitle.Text = "Cliente: " + cm.Cliente;

                            PortafoglioClienti portCli = new PortafoglioClienti();
                            ddlCopiaPERTClienti.DataSource = portCli.Elenco;
                            ddlCopiaPERTClienti.DataValueField = "CodiceCliente";
                            ddlCopiaPERTClienti.DataTextField = "RagioneSociale";
                            ddlCopiaPERTClienti.DataBind();

                            ddlStdFiltroCliente.DataSource = portCli.Elenco;
                            ddlStdFiltroCliente.DataValueField = "CodiceCliente";
                            ddlStdFiltroCliente.DataTextField = "RagioneSociale";
                            ddlStdFiltroCliente.DataBind();

                            var sortedStd = el.elencoFigli.OrderBy(x => x.NomeCombinato);
                            ElencoProcessiVarianti elProdStd = new ElencoProcessiVarianti(true);
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
                lbl1.Text = "Non hai i permessi di aggiungere diagrammi di PERT.<br />";
            }
        }

        protected void addVariante_Click(object sender, EventArgs e)
        {
            // Cerco se esiste un processo di tipo PERT con il nome del cliente
            Commessa cm = new Commessa(idCommessa, annoCommessa);
            macroProcessi elProc = new macroProcessi();
            List<int[]> lstProc = elProc.FindByName(cm.Cliente);
            int prcID = -1;
            int prcRev = -1;
            for (int i = 0; i < lstProc.Count; i++)
            {
                lbl1.Text += lstProc[i][0] + " " + lstProc[i][1] + "<br />";
                processo prc = new processo(lstProc[i][0], lstProc[i][1]);
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
                    macroProcessi elMacroProc = new macroProcessi();
                    bool check = elMacroProc.Add(cm.Cliente, cm.Cliente, false);
                    if (check == false)
                    {
                        lbl1.Text = "Si è verificato un errore.<br />";
                    }
                    else
                    {
                        elMacroProc = new macroProcessi();
                        List<int[]> res = elMacroProc.FindByName(cm.Cliente);
                        prcID = res[0][0];
                        prcRev = res[0][1];
                    }
                }

                // Aggiungo la variante al processo
                if (prcID != -1 && prcRev != -1)
                {
                    processo proc = new processo(prcID, prcRev);
                    variante var = new variante();
                    int varID = var.add(Server.HtmlEncode(txtNomeBlankProd.Text), Server.HtmlEncode(txtDescBlankProd.Text));
                    if (varID == -1)
                    {
                        lbl1.Text = "Si è verificato un errore durante l'aggiunta del nuovo prodotto.";
                    }
                    else
                    {
                        bool retAddProcVar = proc.addVariante(new variante(varID));
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
                                + qty.ToString());
                        }
                        else
                        {
                            lbl1.Text = "Si è verificato un errore durante l'aggiunta del nuovo prodotto.";
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = "Errore nella quantità. Verificare che sia in formato numerico.";
            }
        }

        protected void btnCopiaPERT_Click(object sender, EventArgs e)
        {
            Commessa cm = new Commessa(idCommessa, annoCommessa);
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

                macroProcessi elProc = new macroProcessi();
                List<int[]> lstProc = elProc.FindByName(cm.Cliente);
                int prcID = -1;
                int prcRev = -1;
                for (int i = 0; i < lstProc.Count; i++)
                {
                    lbl1.Text += lstProc[i][0] + " " + lstProc[i][1] + "<br />";
                    processo prc = new processo(lstProc[i][0], lstProc[i][1]);
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
                    macroProcessi elMacroProc = new macroProcessi();
                    bool check = elMacroProc.Add(cm.Cliente, cm.Cliente, false);
                    if (check == false)
                    {
                        lbl1.Text = "Si è verificato un errore.<br />";
                    }
                    else
                    {
                        elMacroProc = new macroProcessi();
                        List<int[]> res = elMacroProc.FindByName(cm.Cliente);
                        prcID = res[0][0];
                        prcRev = res[0][1];
                    }
                }

                if (prcID != -1 && prcRev != -1 && varID != -1)
                {
                    ProcessoVariante daCopiare = new ProcessoVariante(new processo(procID), new variante(varID));
                    processo curr = new processo(prcID, prcRev);
                    if (curr != null && daCopiare != null && curr.processID != -1 && daCopiare.process != null && daCopiare.variant != null)
                    {
                        lbl1.Text = "curr: " + curr.processID.ToString() + "<br />"
                            + "daCopiare: " + daCopiare.process.processID.ToString() + " - " + daCopiare.variant.idVariante.ToString() + "<br />";
                        bool flagCopia = !chkCopia.Checked;
                        bool flagCopiaTC = chkCopiaTC.Checked;
                        bool flagCopiaReparto = chkCopiaReparti.Checked;
                        bool flagCopiaPostazioni = chkCopiaPostazioni.Checked;
                        if (chkCopiaPostazioni.Checked && !chkCopiaReparti.Checked)
                        {
                            lbl1.Text = "Errore: se copio le postazioni devo anche copiare i reparti";
                        }
                        else
                        {
                            int newVarID = daCopiare.CopyTo(curr, Server.HtmlEncode(txtNomeCopiaProd.Text), Server.HtmlEncode(txtDescCopiaProd.Text), flagCopia, flagCopiaTC, flagCopiaReparto, flagCopiaPostazioni);
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
                                + qty.ToString());
                            }
                            else
                            {
                                lblCopiaPERTLog.Text = "Attenzione: si è verificato un errore. Il prodotto potrebbe essere stato copiato parzialmente. Ricaricare la pagina e verificare completamente il prodotto creato.<br />";
                            }
                        }
                    }
                    else
                    {
                        lbl1.Text = "Errore creando i ProcessoVariante<br/>";
                    }
                }
            }
            else
            {
                lbl1.Text = "Errore nel formato della quantità";
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
                Cliente customer = new Cliente(codCliente);
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
                    Commessa cm = new Commessa(idCommessa, annoCommessa);
                    if (cm.ID != -1 && cm.Year != -1)
                    {
                        ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, rev), new variante(idVar));
                        if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.variant.idVariante != -1)
                        {
                            Response.Redirect("wzAssociaPERTReparto.aspx?idCommessa=" + cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString()
                            + "&idProc=" + prcVar.process.processID.ToString()
                            + "&revProc=" + prcVar.process.revisione.ToString()
                            + "&idVariante=" + prcVar.variant.idVariante.ToString()
                            + "&idProdotto=-1&annoProdotto=-1&quantita="
                            + qty.ToString());
                        }
                        else
                        {
                            lbl1.Text = "Attenzione: il prodotto richiesto è inesistente.";
                        }
                    }
                    else
                    {
                        lbl1.Text = "Attenzione: commessa non trovata.";
                    }
                }
                else
                {
                    lbl1.Text = "Attenzione: verificare che la quantità inserita sia corretta.";
                }
            }
            else
            {
                lbl1.Text = "Errore: devi prima selezionare un prodotto.";
            }
        }

        protected void ddlStdFiltroCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (ddlStdFiltroCliente.SelectedValue.Length > 0)
            {
                String codCliente = Server.HtmlEncode(ddlStdFiltroCliente.SelectedValue);
                Cliente customer = new Cliente(codCliente);
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
                
                ddlAddProdStandard.Items.Add(new ListItem("", "Nessuna selezione"));
                ddlAddProdStandard.DataSource = sorted;
                ddlAddProdStandard.DataValueField = "IDCombinato2";
                ddlAddProdStandard.DataTextField = "NomeCombinato";
                ddlAddProdStandard.DataBind();
                ddlAddProdStandard.Focus();
            }
        }
    }
}