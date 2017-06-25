using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.Commesse;

namespace KIS.Analysis
{
    public partial class CostificazioneProdottiTerminatiRicerca1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptArticoliTerminati.Visible = false;
            tblRicerca.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Analisi Articolo Costo";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ckUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (ckUser == true)
            {
                tblRicerca.Visible = true;
                if (!Page.IsPostBack)
                {
                    PortafoglioClienti portClienti = new PortafoglioClienti();
                    var lstClienti = portClienti.Elenco.OrderBy(x => x.RagioneSociale);
                    ddlCliente.DataValueField = "CodiceCliente";
                    ddlCliente.DataTextField = "RagioneSociale";
                    ddlCliente.DataSource = lstClienti;
                    ddlCliente.DataBind();

                    ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                    var sorted = el.elencoFigli.OrderBy(x => x.process.processName).ThenBy(y => y.variant.idVariante);
                    ddlTipoProdotto.DataSource = sorted;
                    ddlTipoProdotto.DataValueField = "IDCombinato2";
                    ddlTipoProdotto.DataTextField = "NomeCombinato";
                    ddlTipoProdotto.DataBind();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (uno.Checked == true)
            {
                int idArticolo=-1, annoArticolo=-1;
                try
                {
                    idArticolo = Int32.Parse(txtIDArticolo.Text.ToString());
                    annoArticolo = Int32.Parse(txtAnnoArticolo.Text.ToString());
                }
                catch
                {
                    idArticolo = -1;
                    annoArticolo = -1;
                }
                Articolo art = new Articolo(idArticolo, annoArticolo);
                if (art.ID != -1 && art.Year != -1 && art.Status == 'F')
                {
                    rptArticoliTerminati.Visible = true;
                    ElencoArticoli elArt = new ElencoArticoli(art);
                    rptArticoliTerminati.DataSource = elArt.ListArticoli;
                    rptArticoliTerminati.DataBind();
                }
                else
                {
                    rptArticoliTerminati.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblErroreCampi").ToString();
                }
            }
            else if (due.Checked == true)
            {
                String codCliente = ddlCliente.SelectedValue.ToString();
                String codProcVar = ddlTipoProdotto.SelectedValue.ToString();
                String dataInizio = txtProductDateStart.Text.ToString();
                String dataFine = txtProductDateEnd.Text.ToString();

                Cliente customer = null;
                if (codCliente != "-1")
                {
                    customer = new Cliente(codCliente);
                }

                ProcessoVariante origProc = null;
                if (codProcVar != "-1/-1/-1")
                {
                    String[] prcVar = codProcVar.Split('/');
                    int codProc = -1, revProc = -1, idVar = -1;
                    try
                    {
                        codProc = Int32.Parse(prcVar[0]);
                        revProc = Int32.Parse(prcVar[1]);
                        idVar = Int32.Parse(prcVar[2]);
                    }
                    catch
                    {
                        codProc = -1;
                        revProc = -1;
                        idVar = -1;
                    }

                    if (codProc != -1 && revProc != -1 && idVar != -1)
                    {
                        origProc = new ProcessoVariante(new processo(codProc, revProc), new variante(idVar));
                    }
                }

                    DateTime inPrd = new DateTime(1970,1,1), finPrd = new DateTime(1970,1,1);
                    if (chkConsideraDate.Checked)
                    {
                        int ggI, ggF, mmI, mmF, yyI, yyF;
                        String[] dataI = new String[3];
                        String[] dataF = new String[3];
                        dataI = (txtProductDateStart.Text).Split('/');
                        dataF = (txtProductDateEnd.Text).Split('/');
                        
                        try
                        {
                            ggI = Int32.Parse(dataI[0]);
                            mmI = Int32.Parse(dataI[1]);
                            yyI = Int32.Parse(dataI[2]);

                            ggF = Int32.Parse(dataF[0]);
                            mmF = Int32.Parse(dataF[1]);
                            yyF = Int32.Parse(dataF[2]);

                            inPrd = new DateTime(yyI, mmI, ggI);
                            finPrd = new DateTime(yyF, mmF, ggF);

                            if (inPrd > finPrd)
                            {
                                inPrd = new DateTime(1970, 1, 1);
                                finPrd = new DateTime(1970, 1, 1);
                            }
                        }
                        catch
                        {
                            inPrd = new DateTime(1970, 1, 1);
                            finPrd = new DateTime(1970, 1, 1);
                        }
                    }

                    if ((origProc != null && origProc.process != null && origProc.process.processID != -1 && origProc.variant != null && origProc.variant.idVariante != -1) || (customer.CodiceCliente.Length > 0) || (inPrd < finPrd && inPrd > new DateTime(1970, 1, 1)))
                    {
                        ElencoArticoli elArt = new ElencoArticoli(origProc, customer, inPrd, finPrd);
                        for (int i = 0; i < elArt.ListArticoli.Count; i++)
                        {
                            elArt.ListArticoli[i].loadTempoDiLavoroTotale();
                        }
                        rptArticoliTerminati.Visible = true;
                        rptArticoliTerminati.DataSource = elArt.ListArticoli;
                        rptArticoliTerminati.DataBind();
                    }
                    else
                    {
                        rptArticoliTerminati.Visible = false;
                        lbl1.Text = GetLocalResourceObject("lblErroreNoCriteria").ToString();
                    }
                
            }
        }
    }
}