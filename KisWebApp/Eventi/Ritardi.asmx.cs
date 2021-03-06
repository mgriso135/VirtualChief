using System;
using System.Collections.Generic;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using KIS.App_Code;
namespace KIS.Eventi
{
    /// <summary>
    /// Questo webservice implementa le funzioni di ricerca dei ritardi e di segnalazione dei ritardi a chi di dovere
    /// Inoltre esegue la pulizia della tabella eventi da quelli segnalati
    /// </summary>
    [WebService(Namespace = "http://kis.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class Ritardi : System.Web.Services.WebService
    {

        [WebMethod]
        public void Main(String tenant)
        {
            //bool rt = false;
            List<String[]> rt = TrovaRitardi(tenant);
            SegnalaRitardi(tenant);
            //rt = true;
            //return rt;
        }

        private List<String[]> TrovaRitardi(String tenant)
        {
            List<String[]> ritardi = new List<string[]>();
            MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            FusoOrario fuso = new FusoOrario(tenant);
            cmd.CommandText = "SELECT taskID FROM tasksproduzione WHERE status <> 'F'  AND earlystart <= '"+TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss") + "'" + " ORDER BY lateStart";
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<int> tskRitardo = new List<int>();
            while (rdr.Read())
            {
                TaskProduzione tsk = new TaskProduzione(tenant, rdr.GetInt32(0));

                // Check if delay is already in registroeventiproduzione table
                if(!tsk.DelayDetected)
                { 
                    Reparto rp = new Reparto(tenant, tsk.RepartoID);

                    String[] ritardo = new String[4];
                    TimeSpan rit = tsk.ritardo;
                    if (rit > new TimeSpan(0, 0, 0))
                    {
                        tskRitardo.Add(tsk.TaskProduzioneID);
                
                
                    ritardo[0] = tsk.TaskProduzioneID.ToString();
                
                    ritardo[1] = rit.Hours + ":" + rit.Minutes + ":" + rit.Seconds;
                    ritardo[2] = "00:00:00";
                    ritardi.Add(ritardo);
                    }
                }
            }
            rdr.Close();

            for (int i = 0; i < tskRitardo.Count; i++)
            {
                cmd.CommandText = "INSERT INTO registroeventiproduzione(TipoEvento, taskID, segnalato) VALUES('Ritardo', "
                    + tskRitardo[i].ToString() + ", false)";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
            conn.Close();
            return ritardi;
        }
        
        private bool SegnalaRitardi(String tenant)
        {
            bool rt = false;
            KISConfig cfg = new KISConfig(tenant);
            String baseURL = cfg.baseUrl + cfg.basePath;
            MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
            conn.Open();
            List<TaskProduzione> tskList = new List<TaskProduzione>();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT taskID FROM registroeventiproduzione WHERE TipoEvento LIKE 'Ritardo' AND segnalato = false";
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //retu += "DB: " + rdr.GetInt32(0).ToString() + "<br />";
                tskList.Add(new TaskProduzione(tenant, rdr.GetInt32(0)));
            }
            rdr.Close();
            for (int i = 0; i < tskList.Count; i++)
            {
                TimeSpan ritardo = tskList[i].ritardo;
                //retu += "Main cycle: " + tskList[i].TaskProduzioneID.ToString() + "<br />";
                // Ricerco tutti gli indirizzi cui inviare la mail
                List<System.Net.Mail.MailAddress> MailList = new List<System.Net.Mail.MailAddress>();
                // Ricerca per reparto
                Reparto rp = new Reparto(tenant, tskList[i].RepartoID);
                rp.loadEventoRitardo();
                if (rp.EventoRitardo.RitardoMinimoDaSegnalare != null && ritardo >= rp.EventoRitardo.RitardoMinimoDaSegnalare)
                {
                    for (int j = 0; j < rp.EventoRitardo.MailingList.Count; j++)
                    {
                        bool found = false;
                        for (int k = 0; k < MailList.Count; k++)
                        {
                            if (MailList[k].Address == rp.EventoRitardo.MailingList[j].Address)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            MailList.Add(rp.EventoRitardo.MailingList[j]);
                            //retu += "Reparto " + rp.EventoRitardo.MailingList[j].Address + "<br />";
                        }
                    }
                }

                // Ricerco per articolo
                Articolo art = new Articolo(tenant, tskList[i].ArticoloID, tskList[i].ArticoloAnno);
                art.loadEventoRitardo();
                if (art.EventoRitardo.RitardoMinimoDaSegnalare != null && ritardo >= art.EventoRitardo.RitardoMinimoDaSegnalare)
                {
                    for (int j = 0; j < art.EventoRitardo.MailingList.Count; j++)
                    {
                        bool found = false;
                        for (int k = 0; k < MailList.Count; k++)
                        {
                            if (MailList[k].Address == art.EventoRitardo.MailingList[j].Address)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            MailList.Add(art.EventoRitardo.MailingList[j]);
                        }
                    }
                }



                // Ricerco per commessa
                Commessa cm = new Commessa(Session["ActiveWorkspace_Name"].ToString(), art.Commessa, art.AnnoCommessa);
                cm.loadEventoRitardo();

                if (cm.EventoRitardo.RitardoMinimoDaSegnalare != null && ritardo >= cm.EventoRitardo.RitardoMinimoDaSegnalare)
                {
                    for (int j = 0; j < cm.EventoRitardo.MailingList.Count; j++)
                    {
                        bool found = false;
                        for (int k = 0; k < MailList.Count; k++)
                        {
                            if (MailList[k].Address == cm.EventoRitardo.MailingList[j].Address)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            MailList.Add(cm.EventoRitardo.MailingList[j]);
                            //retu += "Commessa " + cm.EventoRitardo.MailingList[j].Address + "<br />";
                        }
                    }
                }

                Postazione pst = new Postazione(tenant, tskList[i].PostazioneID);
                // Ricerca per articolo
                // Ricerca per specifico task
                //.................


                if (MailList.Count > 0)
                {
                    // Invio l'e-mail
                    System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();

                    mMessage.From = new MailAddress("matteo.griso@virtualchief.net", "Scheduler@VirtualChief");
                    mMessage.To.Add(new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief"));
                    for (int q = 0; q < MailList.Count; q++)
                    {
                        mMessage.CC.Add(MailList[q]);
                    }
                    mMessage.Subject = "[Virtual Chief] " + ResEventsDelay.Ritardi.lblDelayNotification;
                    mMessage.IsBodyHtml = true;



                    mMessage.Body = "<html><body><div>" + ResEventsDelay.Ritardi.lblDelaySentence +":<br/>"
                        + ResEventsDelay.Ritardi.lblDepartment + ": " + rp.name + "<br />"
                        + ResEventsDelay.Ritardi.lblWorkstations +": " + pst.name + "<br />"
                        + ResEventsDelay.Ritardi.lblOrder + ": " + cm.ID.ToString() + "/" + cm.Year.ToString() + " " + "("+cm.ExternalID+") " + ResEventsDelay.Ritardi.lblForCustomer +" " + cm.RagioneSocialeCliente + "<br />"
                        + ResEventsDelay.Ritardi.lblProduct + ": " + art.ID.ToString() + "/" + art.Year.ToString() + " "+art.Proc.ExternalID+ " " + art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante + "<br />"
                        + ResEventsDelay.Ritardi.lblTask + ": " + tskList[i].Name + " (" + tskList[i].TaskProduzioneID.ToString()+")"
                        + "</div>"
                        + "<div>" + ResEventsDelay.Ritardi.lblGoToWorkstation + " <a href=\"" + baseURL
                             + "/Workplace/WebGemba/Index" + "\">" +
                             ResEventsDelay.Ritardi.lblClickHere
                             + "</a></div>"
                             + "<div>"+ResEventsDelay.Ritardi.lblGoToAndon+" <a href=\"" + baseURL
                        + "/Andon/DepartmentAndon/Index?DepartmentID=" + rp.id.ToString() + "\">" +
                        ResEventsDelay.Ritardi.lblClickHere
                        +"</a></div>"
                        +"</body></html>";

                    SmtpClient smtpcli = new SmtpClient();
                    smtpcli.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpcli.EnableSsl = true;
                    smtpcli.Send(mMessage);
                }


                //TimeSpan rit = tskList[i].ritardo;

                /*if (ritardo >= rp.EventoRitardo.RitardoMinimoDaSegnalare 
                    && ritardo >= cm.EventoRitardo.RitardoMinimoDaSegnalare && ritardo >= art.EventoRitardo.RitardoMinimoDaSegnalare)
                {*/
                    // Metto il ritardo come già segnalato
                    cmd.CommandText = "UPDATE registroeventiproduzione SET segnalato = true WHERE TipoEvento LIKE 'Ritardo' AND taskID = " + tskList[i].TaskProduzioneID;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        rt = true;
                    }
                    catch
                    {
                        rt = false;
                    }
                //}
            }
            conn.Close();
            //return retu;
            return rt;
        }
    }
}
