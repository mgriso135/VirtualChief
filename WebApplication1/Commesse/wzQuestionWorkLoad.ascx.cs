using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzQuestionWorkLoad1 : System.Web.UI.UserControl
    {
        public int idCommessa, annoCommessa, idProc, revProc, idVariante, idReparto, idProdotto, annoProdotto;
        protected void Page_Load(object sender, EventArgs e)
        {
            tblShowQuestion.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {

                    Commessa cm = new Commessa(idCommessa, annoCommessa);
                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
                    Reparto rp = new Reparto(idReparto);
                    Articolo art = new Articolo(idProdotto, annoProdotto);

                    if ((art.Status=='N'||art.Status =='I' || art.Status=='P') && cm.ID != -1 && cm.Year != -1 && prcVar.process != null && prcVar.variant != null && rp.id != -1 && art.ID != -1 && art.Year != -1)
                    {
                        if (art.Commessa == cm.ID && art.AnnoCommessa == cm.Year && art.Proc.process.processID == prcVar.process.processID && art.Proc.process.revisione == prcVar.process.revisione && art.Proc.variant.idVariante == prcVar.variant.idVariante)
                        {
                            tblShowQuestion.Visible = true;
                            lnkGoBack.NavigateUrl += "?idCommessa="+cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString()
                            + "&idProc=" + prcVar.process.processID.ToString()
                            + "&revProc=" + prcVar.process.revisione.ToString()
                            + "&idVariante="+ prcVar.variant.idVariante.ToString()
                            + "&idReparto=" + rp.id.ToString()
                            + "&idProdotto=" + art.ID.ToString()
                            + "&annoProdotto=" + art.Year.ToString()
                            + "&quantita=" + art.Quantita.ToString();

                            lnkFwdCheckWorkLoad.NavigateUrl += "?idCommessa=" + cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString()
                            + "&idProc=" + prcVar.process.processID.ToString()
                            + "&revProc=" + prcVar.process.revisione.ToString()
                            + "&idVariante=" + prcVar.variant.idVariante.ToString()
                            + "&idReparto=" + rp.id.ToString()
                            + "&idProdotto=" + art.ID.ToString()
                            + "&annoProdotto=" + art.Year.ToString()
                            + "&quantita=" + art.Quantita.ToString();

                            lnkFwdDeliveryDate.NavigateUrl += "?idCommessa=" + cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString()
                            + "&idProc=" + prcVar.process.processID.ToString()
                            + "&revProc=" + prcVar.process.revisione.ToString()
                            + "&idVariante=" + prcVar.variant.idVariante.ToString()
                            + "&idReparto=" + rp.id.ToString()
                            + "&idProdotto=" + art.ID.ToString()
                            + "&annoProdotto=" + art.Year.ToString() 
                            + "&quantita=" + art.Quantita.ToString();
                        }
                        else
                        {
                            lbl1.Text = "C'è un'incongruenza nei dati fornitimi. Non è possibile continuare.";
                        }
                    }
                    else
                    {
                        lbl1.Text = "Si è verificato un errore.";
                    }

            }
            else
            {
            }
        }
    }
}