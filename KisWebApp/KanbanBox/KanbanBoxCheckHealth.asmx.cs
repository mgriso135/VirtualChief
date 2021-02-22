using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using KIS.App_Code;
using System.Net.Mail;
using KIS.Commesse;

namespace KIS.KanbanBox
{
    /// <summary>
    /// Descrizione di riepilogo per KanbanBoxCheckHealth
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class KanbanBoxCheckHealth : System.Web.Services.WebService
    {
        public String log;

        [WebMethod]
        public string Main()
        {
            String ret = "";
            KanbanBoxDataSet kbSet = new KanbanBoxDataSet(Session["ActiveWorkspace"].ToString());
            List<String> nonExPN2 = kbSet.checkNonExistentPartNumbers();
            var nonExPN = nonExPN2.Distinct().ToList();
            if (nonExPN.Count > 0)
            {
                String nonExPNText = "Ciao, sono il tuo sistema automatico di controllo congruenza dati tra KIS e KanbanBox.<br />"
                    + "Su KanbanBox sono presenti i seguenti CODICI PRODOTTO che su KIS non esistono:<br/><br/>";
                for (int i = 0; i < nonExPN.Count; i++)
                {
                    nonExPNText += nonExPN[i] + "<br />";
                }
                nonExPNText += "<br/>I cartellini associati a questi prodotto non potranno essere prodotti.<br />"
                + "Per favore crea i prodotti richiesti anche su KIS.<br/><br/><br/>Ciao";

                sendEmail(kbSet.kBoxManagersMails, nonExPNText);

                ret = nonExPNText;
            }


            List<Articolo> ghostProd = new List<Articolo>();
            ghostProd = kbSet.checkGhostProducts();
            if (ghostProd.Count > 0)
            {
                String ghostProdsTxt = "Ciao,<br />sono il tuo sistema automatico di controllo congruenza dati tra KIS e KanbanBox.<br />"
                    + "Il sistema non è riuscito a pianificare in produzione i seguenti cartellini kanban:<br/><br/>";
                for (int i = 0; i < ghostProd.Count; i++)
                {
                    ghostProdsTxt += ghostProd[i].Proc.process.processName + " - " + ghostProd[i].KanbanCardID + "<br />";
                }
                ghostProdsTxt += "<br/>"
                + "Verifica che il tempo di produzione disponibile sia sufficiente a produrre questi articoli, "
                +"in caso contrario:<br/>"
                + "- Verifica il lead time impostato su KanbanBox<br/>"
                +"- Varia manualmente la data di consegna del prodotto, la data fine produzione su KIS e lancia il prodotto in produzione."
                +"<br/><br/><br/>Ciao";

                sendEmail(kbSet.kBoxManagersMails, ghostProdsTxt);

                ret += ghostProdsTxt;
            }

            List<Articolo> nonUpdateProds = new List<Articolo>();
            nonUpdateProds = kbSet.checkNonUpdatedCards();
            if (nonUpdateProds.Count > 0)
            {
                String nonUpdateProdsTxt = "Ciao,<br />sono il tuo sistema automatico di controllo congruenza dati tra KIS e KanbanBox.<br />"
                    + "KIS ha in produzione i seguenti cartellini, che però non sono stati avanzati su KanbanBox:<br/><br/>";
                for (int i = 0; i < nonUpdateProds.Count; i++)
                {
                    nonUpdateProdsTxt += nonUpdateProds[i].Proc.process.processName + " - " + nonUpdateProds[i].KanbanCardID + "<br />";
                }
                nonUpdateProdsTxt += "<br/>"
                + "Ho provato io ad aggiornarli automaticamente, per favore verifica che ci sia riuscito."
                + "<br/><br/><br/>Ciao";

                sendEmail(kbSet.kBoxManagersMails, nonUpdateProdsTxt);
                ret += nonUpdateProdsTxt;
            }

            List<String> nExCusts = new List<String>();
            nExCusts = kbSet.checkNonExistentCustomers();
            ret = kbSet.log;
            if (nExCusts.Count > 0)
            {
                String nExCustsTxt = "Ciao,<br />sono il tuo sistema automatico di controllo congruenza dati tra KIS e KanbanBox.<br />"
                    + "I seguenti Clienti non esistono su KIS:<br/><br/>";
                for (int i = 0; i < nExCusts.Count; i++)
                {
                    nExCustsTxt += nExCusts[i] + "<br />";
                }
                nExCustsTxt += "<br/>"
                + "Per favore aggiungili."
                + "<br/><br/>Ciao";

                sendEmail(kbSet.kBoxManagersMails, nExCustsTxt);
                ret =  kbSet.log + "\n"+nExCustsTxt;
            }

            List<String> nonConsistentCards = new List<String>();
            nonConsistentCards = kbSet.checkCardsConsistency();
            ret = kbSet.log;
            if (nonConsistentCards.Count > 0)
            {
                String nonConsistentCardsTxt = "Ciao,<br />sono il tuo sistema automatico di controllo congruenza dati tra KIS e KanbanBox.<br />"
                    + "Su KIS sono state trovate le seguenti incongruenze da risolvere:<br/><br/>";
                for (int i = 0; i < nonConsistentCards.Count; i++)
                {
                    nonConsistentCardsTxt += nonConsistentCards[i];
                }
                nonConsistentCardsTxt += "<br/>"
                + "Questi devono essere risolti per un corretto utilizzo di KIS e KanbanBox."
                + "<br/><br/>Ciao";

                sendEmail(kbSet.kBoxManagersMails, nonConsistentCardsTxt);
                ret = kbSet.log + "\n" + nonConsistentCardsTxt;
            }

            return ret;
        }

        public Boolean sendEmail(List<MailAddress> MailList, String corpoMail)
        {
            Boolean ret = false;
            System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
            mMessage.From = new MailAddress("tools@kaizenpeople.it", "robot@kis");
            mMessage.To.Add(new MailAddress("tools@kaizenpeople.it", "robot@kis"));
            for (int q = 0; q < MailList.Count; q++)
            {
                mMessage.CC.Add(MailList[q]);
            }
            mMessage.Subject = "KIS - KanbanBox warning";
            mMessage.IsBodyHtml = true;



            mMessage.Body = corpoMail;

            SmtpClient smtpcli = new SmtpClient();
            smtpcli.Send(mMessage);
            ret = true;
            return ret;
        }

        // SEGNALO ERRORI AI GRUPPI CHE HANNO IL PERMESSO DI KanbanBox GetWarnings
    }
}
