using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Commesse
{
    public partial class wzInserisciDataConsegna : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmAssegnaDataConsegna.Visible = false;
            InfoPanel.Visible = false;
            String sIDCommessa = Request.QueryString["idCommessa"];
            String sAnnoCommessa = Request.QueryString["annoCommessa"];
            String sIDProc = Request.QueryString["idProc"];
            String sRevProc = Request.QueryString["revProc"];
            String sIDVariante = Request.QueryString["idVariante"];
            String sIDReparto = Request.QueryString["idReparto"];
            String sIDProdotto = Request.QueryString["idProdotto"];
            String sAnnoProdotto = Request.QueryString["annoProdotto"];
            String sQuantita = Request.QueryString["quantita"];

            int idCommessa=-1, annoCommessa=-1, idProc=-1, revProc=-1, idVariante=-1, idReparto=-1, idProdotto=-1, annoProdotto=-1, quantita=-1;

            if (!String.IsNullOrEmpty(sIDCommessa) && !String.IsNullOrEmpty(sAnnoCommessa) && !String.IsNullOrEmpty(sIDProc) && !String.IsNullOrEmpty(sRevProc) && !String.IsNullOrEmpty(sIDVariante) && !String.IsNullOrEmpty(sIDReparto) && !String.IsNullOrEmpty(sIDProdotto) && !String.IsNullOrEmpty(sAnnoProdotto) && !String.IsNullOrEmpty(sQuantita))
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
                    quantita = Int32.Parse(sQuantita);
                }
                catch
                {
                    idCommessa=-1; 
                    annoCommessa=-1;
                    idProc=-1;
                    revProc = -1;
                    idVariante = -1;
                    idReparto=-1;
                    idProdotto = -1;
                    annoProdotto = -1;
                    quantita = -1;
                }

                if (idCommessa != -1 && annoCommessa != -1 && idProc != -1 && revProc != -1 && idVariante != -1 && idReparto != -1 && quantita >0)
                {
                    Commessa cm = new Commessa(idCommessa, annoCommessa);
                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
                    Reparto rp = new Reparto(idReparto);
                    if (cm.ID != -1 && prcVar.process != null && prcVar.variant != null && rp.id != -1)
                    {
                        frmAssegnaDataConsegna.Visible = true;

                        frmAssegnaDataConsegna.idReparto = idReparto;
                        frmAssegnaDataConsegna.idCommessa = cm.ID;
                        frmAssegnaDataConsegna.annoCommessa = cm.Year;
                        frmAssegnaDataConsegna.idProc = prcVar.process.processID;
                        frmAssegnaDataConsegna.revProc = prcVar.process.revisione;
                        frmAssegnaDataConsegna.idVariante = prcVar.variant.idVariante;
                        frmAssegnaDataConsegna.idProdotto = idProdotto;
                        frmAssegnaDataConsegna.annoProdotto = annoProdotto;
                        frmAssegnaDataConsegna.quantita = quantita;

                        lnkAddPert.NavigateUrl = "wzAddPERT.aspx?idCommessa=" + idCommessa.ToString()
                            + "&annoCommessa=" + annoCommessa.ToString();
                        lnkEditPert.NavigateUrl = "wzEditPERT.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            +"&quantita="+quantita.ToString();

                        lnkAssociaReparto.NavigateUrl = "wzAssociaPERTReparto.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&quantita=" + quantita.ToString();

                        lnkAssociaTasks.NavigateUrl = "wzAssociaTaskPostazioni.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&idReparto=" + idReparto.ToString()
                            + "&quantita=" + quantita.ToString();

                        lnkDataConsegna.NavigateUrl = "wzInserisciDataConsegna.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&idReparto=" + idReparto.ToString()
                            + "&quantita=" + quantita.ToString();

                        InfoPanel.Visible = true;
                        InfoPanel.idCommessa = cm.ID;
                        InfoPanel.annoCommessa = cm.Year;
                        InfoPanel.idProc = prcVar.process.processID;
                        InfoPanel.revProc = prcVar.process.revisione;
                        InfoPanel.idVariante = prcVar.variant.idVariante;
                        InfoPanel.idProdotto = idProdotto;
                        InfoPanel.annoProdotto = annoProdotto;
                        InfoPanel.idReparto = rp.id;
                        InfoPanel.quantita = quantita;
                    }
                }
            }

        }
    }
}