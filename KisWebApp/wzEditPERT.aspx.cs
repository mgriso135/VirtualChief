using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Commesse
{
    public partial class wzEditPERT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                //frmEditPERT.Visible = false;
            String sIDCommessa = Request.QueryString["idCommessa"];
            String sAnnoCommessa = Request.QueryString["annoCommessa"];
            String sIDProc = Request.QueryString["idProc"];
            String sRevProc = Request.QueryString["revProc"];
            String sIDVariante = Request.QueryString["idVariante"];
            String sIDProdotto = Request.QueryString["idProdotto"];
            String sAnnoProdotto = Request.QueryString["annoProdotto"];
            String sQuantita = Request.QueryString["quantita"];
            String matricola = Request.QueryString["matricola"];
            lnkGoBack.Visible = false;
            lnkGoFwd.Visible = false;

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
                lnkGoBack.Visible = true;
                lnkGoFwd.Visible = true;

                ProcessID.Value = sIDProc;
            ProcessRev.Value = sRevProc;
            VariantID.Value = sIDVariante;

            if (!String.IsNullOrEmpty(sIDCommessa) && !String.IsNullOrEmpty(sAnnoCommessa) && !String.IsNullOrEmpty(sIDProc) && !String.IsNullOrEmpty(sRevProc) && !String.IsNullOrEmpty(sIDVariante) && !String.IsNullOrEmpty(sIDProdotto) && !String.IsNullOrEmpty(sAnnoProdotto) && !String.IsNullOrEmpty(sQuantita))
            {
                int idCommessa = -1;
                int annoCommessa = -1;
                int idProc = -1;
                int revProc = -1;
                int idVariante = -1;
                int idProdotto = -1;
                int annoProdotto = -1;
                int quantita = -1;
                try
                {
                    idCommessa = Int32.Parse(sIDCommessa);
                    annoCommessa = Int32.Parse(sAnnoCommessa);
                    idProc = Int32.Parse(sIDProc);
                    revProc = Int32.Parse(sRevProc);
                    idVariante = Int32.Parse(sIDVariante);
                    idProdotto = Int32.Parse(sIDProdotto);
                    annoProdotto = Int32.Parse(sAnnoProdotto);
                    quantita = Int32.Parse(sQuantita);

                }
                catch
                {
                    idCommessa = -1;
                    annoCommessa = -1;
                    idProc = -1;
                    revProc = -1;
                    idVariante = -1;
                    idProdotto = -1;
                    annoProdotto = -1;
                    quantita = -1;
                }

                if (idCommessa != -1 && annoCommessa != -1 && idProc != -1 && revProc != -1 && idVariante != -1)
                {
                    Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), idCommessa, annoCommessa);
                    cm.loadArticoli();
                    processo prc = new processo(Session["ActiveWorkspace_Name"].ToString(), idProc, revProc);
                    variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), idVariante);
                    if (cm != null && cm.ID != -1 && prc != null && prc.processID != -1 && var != null && var.idVariante != -1)
                    {
                        lnkAddPert.NavigateUrl = "~/Commesse/wzAddPERT.aspx?idCommessa=" + cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString();
                        lnkEditPert.NavigateUrl = "~/Commesse/wzEditPERT.aspx?idCommessa=" + cm.ID.ToString()
                        + "&annoCommessa=" + cm.Year.ToString()
                        + "&idProc="+ prc.processID.ToString()
                        + "&revProc=" + prc.revisione.ToString()
                        + "&idVariante=" + var.idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&quantita=" + quantita.ToString()
                            +"&matricola="+matricola.ToString();

                        lnkSwitchToGrid.NavigateUrl = "~/Commesse/wzEditPERT_updtable.aspx?idCommessa=" + cm.ID.ToString()
                        + "&annoCommessa=" + cm.Year.ToString()
                        + "&idProc=" + prc.processID.ToString()
                        + "&revProc=" + prc.revisione.ToString()
                        + "&idVariante=" + var.idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&quantita=" + quantita.ToString()
                            + "&matricola=" + matricola.ToString();

                        bool checkIntegrity = true;
                if (idProdotto != -1 && annoProdotto != -1)
                {
                    checkIntegrity = false;
                    Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), idProdotto, annoProdotto);
                    if (art.ID != -1 && art.Year != -1)
                    {
                        if (art.Commessa == idCommessa && art.AnnoCommessa == annoCommessa && art.Proc.process.processID == idProc && art.Proc.process.revisione == revProc && art.Proc.variant.idVariante == idVariante)
                        {
                            checkIntegrity = true;
                        }
                    }
                }


                        ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace_Name"].ToString(), prc, var);
                        prcVar.loadReparto();
                        prcVar.process.loadFigli(prcVar.variant);
                        if (prcVar != null && prcVar.process != null && prcVar.variant != null && checkIntegrity)
                        {
                            frmEditPERT.Visible = true;
                            frmEditPERT.procID = prcVar.process.processID;
                            frmEditPERT.procRev = prcVar.process.revisione;
                            frmEditPERT.varID = prcVar.variant.idVariante;
                            frmEditDatiVariante.idProcesso = prcVar.process.processID;
                            frmEditDatiVariante.revProcesso = prcVar.process.revisione;
                            frmEditDatiVariante.idVariante = prcVar.variant.idVariante;

                            prcVar.process.loadFigli(prcVar.variant);

                            LinkBCK.NavigateUrl = "~/Commesse/wzAddPERT.aspx?idCommessa="
                             + cm.ID.ToString()
                             + "&annoCommessa=" + cm.Year.ToString();

                                LinkFWD.Visible = true;
                                LinkFWD.NavigateUrl = "~/Commesse/wzAssociaPERTReparto.aspx?idCommessa="
                                    + cm.ID.ToString()
                                + "&annoCommessa=" + cm.Year.ToString()
                                + "&idProc=" + prcVar.process.processID.ToString()
                                + "&revProc=" + prcVar.process.revisione.ToString()
                                + "&idVariante=" + prcVar.variant.idVariante.ToString()
                                + "&idProdotto=" + idProdotto.ToString()
                                + "&annoProdotto=" + annoProdotto.ToString()
                                + "&quantita=" + quantita.ToString()
                                + "&matricola=" + matricola.ToString();
                        }
                        else
                        {
                            frmEditPERT.Visible = false;
                            frmEditDatiVariante.Visible = false;
                            lnkGoBack.Visible = false;
                            lnkGoFwd.Visible = false;
                            LinkBCK.Visible = false;
                            lbl1.Text = GetLocalResourceObject("lblError").ToString();

                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblError").ToString();
                    }
                }
                else
                {
                    frmEditPERT.Visible = false;
                    frmEditDatiVariante.Visible = false;
                    lnkGoBack.Visible = false;
                    lnkGoFwd.Visible = false;
                    LinkBCK.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblError").ToString();

                    lnkSwitchToGrid.Enabled = false;
                    lnkSwitchToGrid.Visible = false;
                    imgSwitchToGrid.Visible = false;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorQueryString").ToString();
                frmEditPERT.Visible = false;
                frmEditDatiVariante.Visible = false;
                lnkGoBack.Visible = false;
                lnkGoFwd.Visible = false;
                LinkBCK.Visible = false;

                lnkSwitchToGrid.Enabled = false;
                lnkSwitchToGrid.Visible = false;
                imgSwitchToGrid.Visible = false;
            }
        }
    }
    }
}