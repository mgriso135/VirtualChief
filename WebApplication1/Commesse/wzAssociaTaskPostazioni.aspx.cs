using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Commesse
{
    public partial class wzAssociaTaskPostazioni : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InfoPanel.Visible = false;
            frmAddPostazione.Visible = false;
            imgManagePostazioni.Visible = false;
            imgShowAddPostazioni.Visible = false;
            frmAssociaTasksPostazioni.Visible = false;
            frmWorkLoad.Visible = false;
            lnkGoBack.Visible = false;
            lnkGoFwd.Visible = false;
            imgGoFwd.Visible = false;

            String sIDCommessa= Request.QueryString["idCommessa"];
            String sAnnoCommessa= Request.QueryString["annoCommessa"];
            String sIDProc = Request.QueryString["idProc"];
            String sRevProc= Request.QueryString["revProc"];
            String sIDVariante = Request.QueryString["idVariante"];
            String sIDReparto = Request.QueryString["idReparto"];
            String sIDProdotto = Request.QueryString["idProdotto"];
            String sAnnoProdotto = Request.QueryString["annoProdotto"];
            String sQuantita = Request.QueryString["quantita"];

            if (!String.IsNullOrEmpty(sIDCommessa) && !String.IsNullOrEmpty(sAnnoCommessa) && !String.IsNullOrEmpty(sIDProc) && !String.IsNullOrEmpty(sRevProc) && !String.IsNullOrEmpty(sIDVariante) && !String.IsNullOrEmpty(sIDReparto) && !String.IsNullOrEmpty(sQuantita))
            {
                int idCommessa = -1;
                int annoCommessa = -1;
                int idProc = -1;
                int revProc = -1;
                int idVariante = -1;
                int idReparto = -1;
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
                    idReparto = Int32.Parse(sIDReparto);
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
                    idReparto = -1;
                    idProdotto = -1;
                    annoProdotto = -1;
                    quantita = -1;
                }

                if (idCommessa != -1 && annoCommessa != -1 && idProc != -1 && revProc != -1 && idVariante != -1 && idReparto != -1)
                {
                    Commessa cm = new Commessa(idCommessa, annoCommessa);
                    cm.loadArticoli();
                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
                    Reparto rp = new Reparto(idReparto);
                    if (cm.ID != -1 && prcVar.process != null && prcVar.variant != null && rp.id != -1)
                    {
                        frmAssociaTasksPostazioni.idCommessa = cm.ID;
                        frmAssociaTasksPostazioni.annoCommessa = cm.Year;
                        frmAssociaTasksPostazioni.idProcesso = prcVar.process.processID;
                        frmAssociaTasksPostazioni.revProcesso = prcVar.process.revisione;
                        frmAssociaTasksPostazioni.idVariante = prcVar.variant.idVariante;
                        frmAssociaTasksPostazioni.idReparto = rp.id;
                        frmAssociaTasksPostazioni.idProdotto = idProdotto;
                        frmAssociaTasksPostazioni.annoProdotto = annoProdotto;
                        frmAssociaTasksPostazioni.Visible = true;

                        imgShowAddPostazioni.Visible = true;
                        imgManagePostazioni.Visible = true;

                        frmWorkLoad.procID = prcVar.process.processID;
                        frmWorkLoad.var = prcVar.variant.idVariante;
                        frmWorkLoad.repID = rp.id;
                        frmWorkLoad.Visible = true;

                        lnkGoBack.Visible = true;

                        lnkGoBack.NavigateUrl = "~/Commesse/wzAssociaPERTReparto.aspx?idCommessa=" + cm.ID.ToString()
                        + "&annoCommessa=" + cm.Year.ToString()
                        + "&idProc=" + prcVar.process.processID.ToString()
                        + "&revProc=" + prcVar.process.revisione.ToString()
                        + "&idVariante=" + prcVar.variant.idVariante.ToString()
                        + "&idReparto=" + rp.id.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        +"&quantita=" + quantita.ToString();

                        rp.loadPostazioniTask(prcVar);
                        prcVar.process.loadFigli(prcVar.variant);
                        if (rp.PostazioniTask.Count == prcVar.process.subProcessi.Count)
                        {
                            lnkGoFwd.Visible = true;
                            imgGoFwd.Visible = true;
                        }

                        lnkGoFwd.NavigateUrl = "~/Commesse/wzInserisciDataConsegna.aspx?idCommessa=" + cm.ID.ToString()
                        + "&annoCommessa=" + cm.Year.ToString()
                        + "&idProc=" + prcVar.process.processID.ToString()
                        + "&revProc=" + prcVar.process.revisione.ToString()
                        + "&idVariante=" + prcVar.variant.idVariante.ToString()
                        + "&idReparto=" + rp.id.ToString() 
                        + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&quantita=" + quantita.ToString(); ;

                        lnkAddPert.NavigateUrl = "wzAddPERT.aspx?idCommessa=" + idCommessa.ToString()
                            + "&annoCommessa=" + annoCommessa.ToString();
                        lnkEditPert.NavigateUrl = "wzEditPERT.aspx?idCommessa=" + idCommessa.ToString()
                        + "&annoCommessa=" + annoCommessa.ToString()
                        + "&idProc=" + idProc.ToString()
                        + "&revProc=" + revProc.ToString()
                        + "&idVariante=" + idVariante.ToString()
                        + "&idProdotto=" + idProdotto.ToString()
                            + "&annoProdotto=" + annoProdotto.ToString()
                            + "&quantita=" + quantita.ToString();

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

                        InfoPanel.Visible = true;
                        InfoPanel.idCommessa = cm.ID;
                        InfoPanel.annoCommessa = cm.Year;
                        InfoPanel.idProdotto = idProdotto;
                        InfoPanel.annoProdotto = annoProdotto;
                        InfoPanel.idProc = prcVar.process.processID;
                        InfoPanel.revProc = prcVar.process.revisione;
                        InfoPanel.idVariante = prcVar.variant.idVariante;
                        InfoPanel.idReparto = rp.id;
                        InfoPanel.quantita = quantita;
                    }
                }
            }

        }

        protected void imgShowAddPostazioni_Click(object sender, EventArgs e)
        {
            if (frmAddPostazione.Visible == false)
            {
                frmAddPostazione.Visible = true;
            }
            else
            {
                frmAddPostazione.Visible = true;
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            lnkGoFwd.Visible = false;
            imgGoFwd.Visible = false;
            String sIDProc = Request.QueryString["idProc"];
            String sRevProc = Request.QueryString["revProc"];
            String sIDVariante = Request.QueryString["idVariante"];
            String sIDReparto = Request.QueryString["idReparto"];
            if (!String.IsNullOrEmpty(sIDProc) && !String.IsNullOrEmpty(sRevProc) && !String.IsNullOrEmpty(sIDVariante) && !String.IsNullOrEmpty(sIDReparto))
            {
                int idProc = -1;
                int revProc = -1;
                int idVariante = -1;
                int idReparto = -1;
                try
                {
                    idProc = Int32.Parse(sIDProc);
                    revProc = Int32.Parse(sRevProc);
                    idVariante = Int32.Parse(sIDVariante);
                    idReparto = Int32.Parse(sIDReparto);
                }
                catch
                {
                    idProc = -1;
                    revProc = -1;
                    idVariante = -1;
                    idReparto = -1;
                }

                if (idProc != -1 && revProc != -1 && idVariante != -1 && idReparto != -1)
                {

                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
                    Reparto rp = new Reparto(idReparto);
                    rp.loadPostazioniTask(prcVar);
                    prcVar.process.loadFigli(prcVar.variant);
                    if (rp.PostazioniTask.Count == prcVar.process.subProcessi.Count)
                    {
                        lnkGoFwd.Visible = true;
                        imgGoFwd.Visible = true;
                    }
                    else
                    {
                        lnkGoFwd.Visible = false;
                        imgGoFwd.Visible = false;
                    }
                }
            }
        }
    }
}