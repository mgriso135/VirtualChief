using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Commesse
{
    public partial class wzCheckWorkLoadReparto : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            frmCheckWorkLoad.Visible = false;

            int idCommessa = -1, annoCommessa = -1, idProc = -1, revProc = -1, idVariante = -1, idReparto=-1, idProdotto=-1, annoProdotto=-1;
            String sIDCommessa = Request.QueryString["idCommessa"];
            String sAnnoCommessa = Request.QueryString["annoCommessa"];
            String sIDProc = Request.QueryString["idProc"];
            String sRevProc = Request.QueryString["revProc"];
            String sIDVariante = Request.QueryString["idVariante"];
            String sIDReparto = Request.QueryString["idReparto"];
            String sIDProdotto = Request.QueryString["idProdotto"];
            String sAnnoProdotto = Request.QueryString["annoProdotto"];
            String matricola = Request.QueryString["matricola"];

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
                    idReparto=-1;
                    idProdotto = -1;
                    annoProdotto = -1;
                }
                if (idCommessa != -1 && annoCommessa != -1 && idProc != -1 && revProc != -1 && idVariante != -1 && idReparto != -1)
                {
                    frmCheckWorkLoad.Visible = true;
                    frmCheckWorkLoad.idCommessa = idCommessa;
                    frmCheckWorkLoad.annoCommessa = annoCommessa;
                    frmCheckWorkLoad.idProc = idProc;
                    frmCheckWorkLoad.revProc = revProc;
                    frmCheckWorkLoad.idVariante = idVariante;
                    frmCheckWorkLoad.idReparto = idReparto;
                    frmCheckWorkLoad.idProdotto = idProdotto;
                    frmCheckWorkLoad.annoProdotto = annoProdotto;
                    frmCheckWorkLoad.matricola = matricola;

                    lnkAddPert.NavigateUrl = "wzAddPERT.aspx?idCommessa=" + idCommessa.ToString()
                            + "&annoCommessa=" + annoCommessa.ToString();
                    lnkEditPert.NavigateUrl = "wzEditPERT.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&matricola="+matricola.ToString();

                    lnkAssociaReparto.NavigateUrl = "wzAssociaPERTReparto.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&matricola=" + matricola.ToString(); ;

                    lnkAssociaTasks.NavigateUrl = "wzAssociaTaskPostazioni.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                        + "&matricola=" + matricola.ToString(); ;

                    lnkDataConsegna.NavigateUrl = "wzInserisciDataConsegna.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                        + "&matricola=" + matricola.ToString(); ;

                    lnkLancio.NavigateUrl = "wzQuestionWorkLoad.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                        + "&matricola=" + matricola.ToString(); ;

                    lnkWorkLoad.NavigateUrl = "wzCheckWorkLoadReparto.aspx?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                        + "&matricola=" + matricola.ToString(); ;

                }
            }
            else
            {
                lbl1.Text = "Error.";
            }
        }
    }
}