using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Commesse
{
    public partial class wzAssociaPERTReparto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InfoPanel.Visible = false;
            frmAssociaPERTReparto.Visible = false;
            int idCommessa = -1;
            int annoCommessa = -1;
            int idProc=-1;
            int revProc=-1;
            int idVariante = -1;
            int idProdotto = -1;
            int annoProdotto = -1;
            int qty = -1;

            String sIDCommessa, sAnnoCommessa, sIDProc, sRevProc, sIDVariante, sIDProdotto, sAnnoProdotto, sQuantita;
            sIDCommessa = Request.QueryString["idCommessa"];
            sAnnoCommessa = Request.QueryString["annoCommessa"];
            sIDProc = Request.QueryString["idProc"];
            sRevProc = Request.QueryString["revProc"];
            sIDVariante = Request.QueryString["idVariante"];
            sIDProdotto = Request.QueryString["idProdotto"];
            sAnnoProdotto = Request.QueryString["annoProdotto"];
            sQuantita = Request.QueryString["quantita"];

            if (!String.IsNullOrEmpty(sIDCommessa) && !String.IsNullOrEmpty(sAnnoCommessa) && !String.IsNullOrEmpty(sIDProc) && !String.IsNullOrEmpty(sRevProc) && !String.IsNullOrEmpty(sIDVariante) && !String.IsNullOrEmpty(sIDProdotto) && !String.IsNullOrEmpty(sAnnoProdotto) && !String.IsNullOrEmpty(sQuantita))
            {
                try
                {
                    idCommessa = Int32.Parse(sIDCommessa);
                    annoCommessa = Int32.Parse(sAnnoCommessa);
                    idProc = Int32.Parse(sIDProc);
                    revProc = Int32.Parse(sRevProc);
                    idVariante = Int32.Parse(sIDVariante);
                    idProdotto = Int32.Parse(sIDProdotto);
                    annoProdotto = Int32.Parse(sAnnoProdotto);
                    qty = Int32.Parse(sQuantita);
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
                    qty = -1;
                }

                if (idCommessa != -1 && annoCommessa != -1 && idProc != -1 && revProc != -1 && idVariante != -1 && qty>0)
                {
                    InfoPanel.Visible = true;
                    InfoPanel.idCommessa = idCommessa;
                    InfoPanel.annoCommessa = annoCommessa;
                    InfoPanel.idProc = idProc;
                    InfoPanel.revProc = revProc;
                    InfoPanel.idVariante = idVariante;
                    InfoPanel.idReparto = -1;
                    InfoPanel.quantita = qty;

                    frmAssociaPERTReparto.Visible = true;
                    frmAssociaPERTReparto.idProc = idProc;
                    frmAssociaPERTReparto.revProc = revProc;
                    frmAssociaPERTReparto.idVar = idVariante;
                    frmAssociaPERTReparto.idCommessa = idCommessa;
                    frmAssociaPERTReparto.annoCommessa = annoCommessa;
                    frmAssociaPERTReparto.idProdotto = idProdotto;
                    frmAssociaPERTReparto.annoProdotto = annoProdotto;
                    frmAssociaPERTReparto.quantita = qty;

                    lnkAddPert.NavigateUrl = "wzAddPERT.aspx?idCommessa=" + idCommessa.ToString()
                            + "&annoCommessa=" + annoCommessa.ToString();
                    lnkEditPert.NavigateUrl = "wzEditPERT.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                    + "&annoProdotto=" + annoProdotto.ToString()
                    +"&quantita="+qty.ToString();

                    lnkAssociaReparto.NavigateUrl = "wzAssociaPERTReparto.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                    + "&annoProdotto=" + annoProdotto.ToString()
                    + "&quantita=" + qty.ToString(); ;

                }
                else
                {
                }
            }
            else
            {
            }
        }
    }
}