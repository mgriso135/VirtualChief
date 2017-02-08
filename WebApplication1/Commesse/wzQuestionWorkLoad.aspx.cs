using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzQuestionWorkLoad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmQuestionWorkLoad.Visible = false;
            InfoPanel.Visible = false;

            String sIDCommessa = Request.QueryString["idCommessa"];
            String sAnnoCommessa = Request.QueryString["annoCommessa"];
            String sIDProc = Request.QueryString["idProc"];
            String sRevProc = Request.QueryString["revProc"]; 
            String sIDVariante = Request.QueryString["idVariante"];
            String sIDReparto = Request.QueryString["idReparto"];
            String sIDProdotto = Request.QueryString["idProdotto"];
            String sAnnoProdotto = Request.QueryString["annoProdotto"];
            int idCommessa, annoCommessa, idProc, revProc, idVariante, idReparto, idProdotto, annoProdotto;

                try
                {
                    idCommessa = Int32.Parse(sIDCommessa);
                    annoCommessa = Int32.Parse(sAnnoCommessa);
                    idProc = Int32.Parse(sIDProc);
                    revProc = Int32.Parse(sRevProc);
                    idVariante = Int32.Parse(sIDVariante);
                    idReparto = Int32.Parse(sIDReparto);
                    idProdotto = Int32.Parse(sIDProdotto);
                    annoProdotto = Int32.Parse(sAnnoProdotto);
                }
                catch
                {
                    idCommessa = -1;
                    annoCommessa = -1;
                    idProc = -1;
                    revProc = -1;
                    idVariante = -1;
                    idReparto = -1;
                    idProdotto = -1;
                    annoProdotto = -1;
                }
            if (idCommessa != -1 && annoCommessa != -1 && idProc != -1 && revProc != -1 && idVariante != -1 && idReparto != -1 && idProdotto != -1 && annoProdotto != -1)
            {
                Commessa cm = new Commessa(idCommessa, annoCommessa);
                ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
                Reparto rp = new Reparto(idReparto);
                Articolo art = new Articolo(idProdotto, annoProdotto);

                if (cm.ID != -1 && cm.Year != -1 && prcVar.process != null && prcVar.variant != null && rp.id != -1 && art.ID != -1 && art.Year != -1)
                {
                    if (art.Commessa == cm.ID && art.AnnoCommessa == cm.Year && art.Proc.process.processID == prcVar.process.processID && art.Proc.process.revisione == prcVar.process.revisione && art.Proc.variant.idVariante == prcVar.variant.idVariante)
                    {
                        frmQuestionWorkLoad.Visible = true;

                        frmQuestionWorkLoad.idCommessa = cm.ID;
                        frmQuestionWorkLoad.annoCommessa = cm.Year;
                        frmQuestionWorkLoad.idProc = prcVar.process.processID;
                        frmQuestionWorkLoad.revProc = prcVar.process.revisione;
                        frmQuestionWorkLoad.idVariante = prcVar.variant.idVariante;
                        frmQuestionWorkLoad.idReparto = rp.id;
                        frmQuestionWorkLoad.idProdotto = art.ID;
                        frmQuestionWorkLoad.annoProdotto = art.Year;

                        lnkAddPert.NavigateUrl = "wzAddPERT.aspx?idCommessa=" + idCommessa.ToString()
                            + "&annoCommessa=" + annoCommessa.ToString();
                        lnkEditPert.NavigateUrl = "wzEditPERT.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString();

                        lnkAssociaReparto.NavigateUrl = "wzAssociaPERTReparto.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString();

                        lnkAssociaTasks.NavigateUrl = "wzAssociaTaskPostazioni.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&idReparto=" + idReparto.ToString();

                        lnkDataConsegna.NavigateUrl = "wzInserisciDataConsegna.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&idReparto=" + idReparto.ToString();

                        lnkLancio.NavigateUrl = "wzQuestionWorkLoad.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&idReparto=" + idReparto.ToString();

                        InfoPanel.Visible = true;
                        InfoPanel.idCommessa = cm.ID;
                        InfoPanel.annoCommessa = cm.Year;
                        InfoPanel.idProdotto = art.ID;
                        InfoPanel.annoProdotto = art.Year;
                        InfoPanel.idProc = prcVar.process.processID;
                        InfoPanel.revProc = prcVar.process.revisione;
                        InfoPanel.idVariante = prcVar.variant.idVariante;
                        InfoPanel.idReparto = rp.id;
                    }
                    else
                    {
                        lbl1.Text = art.Commessa.ToString() + " "
                            + cm.ID.ToString() + " " 
                            + art.AnnoCommessa.ToString() + " "
                            + cm.Year.ToString() + " "
                            + art.Reparto.ToString() + " "
                            + rp.id.ToString() + " "
                            + art.Proc.process.processID.ToString() + " "
                            + prcVar.process.processID.ToString() + " "
                            + art.Proc.process.revisione.ToString() + " "
                            + prcVar.process.revisione.ToString() + " "
                            + art.Proc.variant.idVariante.ToString() + " "
                            + prcVar.variant.idVariante.ToString();
                        lbl1.Text += "C'è un'incongruenza nei dati fornitimi. Non è possibile continuare.";
                    }
                }
                else
                {
                    lbl1.Text = "Si è verificato un errore.";
                }
            }
        }
    }
}