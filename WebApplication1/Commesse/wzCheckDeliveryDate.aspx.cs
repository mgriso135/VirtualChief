using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Commesse
{
    public partial class wzCheckDeliveryDate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmSetFineProduzione.Visible = false;
            InfoPanel.Visible = false;
            int idCommessa = -1, annoCommessa = -1, idProc = -1, revProc = -1, idVariante = -1, idReparto=-1, idProdotto=-1, annoProdotto=-1;
            String sIDCommessa = Request.QueryString["idCommessa"];
            String sAnnoCommessa = Request.QueryString["annoCommessa"];
            String sIDProc = Request.QueryString["idProc"];
            String sRevProc = Request.QueryString["revProc"];
            String sIDVariante = Request.QueryString["idVariante"];
            String sIDReparto = Request.QueryString["idReparto"];
            String sIDProdotto = Request.QueryString["idProdotto"];
            String sAnnoProdotto = Request.QueryString["annoProdotto"];

            if (!String.IsNullOrEmpty(sIDCommessa) && !String.IsNullOrEmpty(sAnnoCommessa) && !String.IsNullOrEmpty(sIDProc) && !String.IsNullOrEmpty(sRevProc) && !String.IsNullOrEmpty(sIDVariante) && !String.IsNullOrEmpty(sIDReparto))
            {
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
                if (idCommessa != -1 && annoCommessa != -1 && idProc != -1 && revProc != -1 && idVariante != -1 && idReparto != -1)
                {
                    Articolo art = new Articolo(idProdotto, annoProdotto);
                    frmSetFineProduzione.Visible = true;

                    frmSetFineProduzione.idCommessa = idCommessa;
                    frmSetFineProduzione.annoCommessa = annoCommessa;
                    frmSetFineProduzione.idProc = idProc;
                    frmSetFineProduzione.revProc = revProc;
                    frmSetFineProduzione.idVariante = idVariante;
                    frmSetFineProduzione.idReparto = idReparto;
                    frmSetFineProduzione.idProdotto = idProdotto;
                    frmSetFineProduzione.annoProdotto = annoProdotto;

                    lnkAddPert.NavigateUrl = "wzAddPERT.aspx?idCommessa=" + idCommessa.ToString()
                            + "&annoCommessa=" + annoCommessa.ToString();
                    lnkEditPert.NavigateUrl = "wzEditPERT.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&quantita=" + art.Quantita.ToString();

                    lnkAssociaReparto.NavigateUrl = "wzAssociaPERTReparto.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                         + "&quantita=" + art.Quantita.ToString();

                    lnkAssociaTasks.NavigateUrl = "wzAssociaTaskPostazioni.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                         + "&quantita=" + art.Quantita.ToString();

                    lnkDataConsegna.NavigateUrl = "wzInserisciDataConsegna.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                         + "&quantita=" + art.Quantita.ToString();

                    lnkLancio.NavigateUrl = "wzQuestionWorkLoad.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                         + "&quantita=" + art.Quantita.ToString();

                    lnkDataFineProd.NavigateUrl = "wzQuestionWorkLoad.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                         + "&quantita=" + art.Quantita.ToString();

                    InfoPanel.Visible = true;
                    InfoPanel.idCommessa = idCommessa;
                    InfoPanel.annoCommessa = annoCommessa;
                    InfoPanel.idProc = idProc;
                    InfoPanel.revProc = revProc;
                    InfoPanel.idVariante = idVariante;
                    InfoPanel.idProdotto = idProdotto;
                    InfoPanel.annoProdotto = annoProdotto;
                    InfoPanel.idReparto = idReparto;
                }
            }
        }
    }
}