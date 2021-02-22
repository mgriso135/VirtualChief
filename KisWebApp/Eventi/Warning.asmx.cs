using System.Collections.Generic;
using System.Web.Services;
using MySql.Data.MySqlClient;
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
        public void Main(string tenant)
        {
            SegnalaWarning(tenant);
        }

        private bool SegnalaWarning(string tenant)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
            conn.Open();
            List<TaskProduzione> tskList = new List<TaskProduzione>();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID FROM registroeventiproduzione WHERE TipoEvento LIKE 'Warning' AND segnalato = false";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                tskList.Add(new TaskProduzione(tenant, rdr.GetInt32(0)));
            }
            rdr.Close();
            for (int i = 0; i < tskList.Count; i++)
            {
                // Ricerco tutti gli indirizzi cui inviare la mail
                List<System.Net.Mail.MailAddress> MailList = new List<System.Net.Mail.MailAddress>();
                // Ricerca per reparto
                Reparto rp = new Reparto(tenant, tskList[i].RepartoID);
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
                Articolo art = new Articolo(tenant, tskList[i].ArticoloID, tskList[i].ArticoloAnno);
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
                Commessa cm = new Commessa(tenant, art.Commessa, art.AnnoCommessa);
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


                Postazione pst = new Postazione(tenant, tskList[i].PostazioneID);
                
                // Ricerca per specifico task
                //.................



                // Invio l'e-mail
                if (MailList.Count > 0)
                {
                    System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
                    mMessage.From = new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief");
                    mMessage.To.Add(new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief"));
                    for (int q = 0; q < MailList.Count; q++)
                    {
                        mMessage.CC.Add(MailList[q]);
                    }
                    mMessage.Subject = "[Virtual Chief] " + ResEvents.Warning.lblWarningNotification;
                    mMessage.IsBodyHtml = true;



                    mMessage.Body = "<html><body><div>" + ResEvents.Warning.lblWarningSentence + ":<br/>"
                        + ResEvents.Warning.lblDepartment + ": " + rp.name + "<br />"
                        + ResEvents.Warning.lblWorkstation + ": " + pst.name + "<br />"
                        + ResEvents.Warning.lblOrder + ": " + cm.ID.ToString() + "/" + cm.Year.ToString() + " " + ResEvents.Warning.lblForCustomer + " " + cm.Cliente + "<br />"
                        + ResEvents.Warning.lblProduct + ": " + art.ID.ToString() + "/" + art.Year.ToString() + " " + art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante + "<br />"
                        + ResEvents.Warning.lblTask + ": " + tskList[i].Name + " (" + tskList[i].TaskProduzioneID.ToString() + ")"
                        + "</div></body></html>";

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
