using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using KIS.Commesse;
using System.Net.Mail;
using KIS.App_Code;
namespace KIS.Eventi
{
    /// <summary>
    /// Descrizione di riepilogo per Warning
    /// </summary>
    [WebService(Namespace = "http://kis.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class Warning : System.Web.Services.WebService
    {

        [WebMethod]
        public void Main()
        {
            SegnalaWarning();
        }

        private bool SegnalaWarning()
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon();
            conn.Open();
            List<TaskProduzione> tskList = new List<TaskProduzione>();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID FROM registroeventiproduzione WHERE TipoEvento LIKE 'Warning' AND segnalato = false";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                tskList.Add(new TaskProduzione(rdr.GetInt32(0)));
            }
            rdr.Close();
            for (int i = 0; i < tskList.Count; i++)
            {
                // Ricerco tutti gli indirizzi cui inviare la mail
                List<System.Net.Mail.MailAddress> MailList = new List<System.Net.Mail.MailAddress>();
                // Ricerca per reparto
                Reparto rp = new Reparto(tskList[i].RepartoID);
                rp.loadEventoWarning();
                for (int j = 0; j < rp.EventoWarning.MailingList.Count; j++)
                {
                    bool found = false;
                    for (int k = 0; k < MailList.Count; k++)
                    {
                        if (MailList[k].Address == rp.EventoWarning.MailingList[j].Address)
                        {
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        MailList.Add(rp.EventoWarning.MailingList[j]);
                    }
                }
                // Ricerca per articolo
                Articolo art = new Articolo(tskList[i].ArticoloID, tskList[i].ArticoloAnno);
                art.loadEventoWarning();
                for (int j = 0; j < art.EventoWarning.MailingList.Count; j++)
                {
                    bool found = false;
                    for (int k = 0; k < MailList.Count; k++)
                    {
                        if (MailList[k].Address == art.EventoWarning.MailingList[j].Address)
                        {
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        MailList.Add(art.EventoWarning.MailingList[j]);
                    }
                }


                // Ricerco per commessa
                Commessa cm = new Commessa(art.Commessa, art.AnnoCommessa);
                cm.loadEventoWarning();
                for (int j = 0; j < cm.EventoWarning.MailingList.Count; j++)
                {
                    bool found = false;
                    for (int k = 0; k < MailList.Count; k++)
                    {
                        if (MailList[k].Address == cm.EventoWarning.MailingList[j].Address)
                        {
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        MailList.Add(cm.EventoWarning.MailingList[j]);
                    }
                }


                Postazione pst = new Postazione(tskList[i].PostazioneID);
                
                // Ricerca per specifico task
                //.................



                // Invio l'e-mail
                if (MailList.Count > 0)
                {
                    System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
                    mMessage.From = new MailAddress("kaizenindicatorsystem@yahoo.com", "Scheduler@KaizenIndicatorSystem");
                    mMessage.To.Add(new MailAddress("kaizenindicatorsystem@yahoo.com", "Scheduler@KaizenIndicatorSystem"));
                    for (int q = 0; q < MailList.Count; q++)
                    {
                        mMessage.CC.Add(MailList[q]);
                    }
                    mMessage.Subject = "KIS - Segnalazione warning";
                    mMessage.IsBodyHtml = true;



                    mMessage.Body = "E' stato segnalato uno warning in produzione, con riferimento a:<br/>"
                        + "Reparto: " + rp.name + "<br />"
                        + "Postazione: " + pst.name + "<br />"
                        + "Commessa " + cm.ID.ToString() + "/" + cm.Year.ToString() + " per cliente " + cm.Cliente + "<br />"
                        + "Articolo " + art.ID.ToString() + "/" + art.Year.ToString() + " " + art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante + "<br />"
                        + "Task " + tskList[i].TaskProduzioneID.ToString() + " " + tskList[i].Name;

                    SmtpClient smtpcli = new SmtpClient();
                    smtpcli.Send(mMessage);
                }
                // Metto lo warning come già segnalato
                cmd.CommandText = "UPDATE registroeventiproduzione SET segnalato = true WHERE TipoEvento LIKE 'Warning' AND taskID = " + tskList[i].TaskProduzioneID;
                try
                {
                    cmd.ExecuteNonQuery();
                    rt = true;
                }
                catch
                {
                    rt = false;
                }
            }
            conn.Close();
            return rt;
        }


    }
}
