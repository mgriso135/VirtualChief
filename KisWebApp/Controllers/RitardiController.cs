using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Http;
using Dapper;
using KIS.App_Code;
using MySql.Data.MySqlClient;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Eventi/Ritardi.asmx – finds late tasks, emails delay notifications, marks as segnalato.
    /// Contract kept identical for scheduler clients.
    /// </summary>
    public class RitardiController : ApiController
    {
        // GET: api/Ritardi?tenant=...
        [HttpGet]
        public IHttpActionResult Get(String tenant)
        {
            TrovaRitardi(tenant);
            SegnalaRitardi(tenant);
            return Ok("Ritardi check executed.");
        }

        // POST: api/Ritardi/Trigger
        [HttpPost]
        public IHttpActionResult Trigger(String tenant)
        {
            TrovaRitardi(tenant);
            bool rt = SegnalaRitardi(tenant);
            return Ok(rt ? "Ritardi trigger executed." : "Ritardi trigger failed.");
        }

        private List<String[]> TrovaRitardi(String tenant)
        {
            List<String[]> ritardi = new List<string[]>();
            MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
            conn.Open();
            FusoOrario fuso = new FusoOrario(tenant);
            string sql = "SELECT taskID FROM tasksproduzione WHERE status <> 'F'  AND earlystart <= '" + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario).ToString("yyyy-MM-dd HH:mm:ss") + "'" + " ORDER BY lateStart";
            var taskIDs = conn.Query<int>(sql);
            List<int> tskRitardo = new List<int>();
            foreach (var taskID in taskIDs)
            {
                TaskProduzione tsk = new TaskProduzione(tenant, taskID);

                // Check if delay is already in registroeventiproduzione table
                if (!tsk.DelayDetected)
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

            for (int i = 0; i < tskRitardo.Count; i++)
            {
                sql = "INSERT INTO registroeventiproduzione(TipoEvento, taskID, segnalato) VALUES('Ritardo', "
                    + tskRitardo[i].ToString() + ", false)";
                try
                {
                    conn.Execute(sql);
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
            String wsName = WebEnv.ActiveWorkspaceName;
            if (wsName.Length == 0)
            {
                wsName = tenant;
            }
            KISConfig cfg = new KISConfig(tenant);
            String baseURL = cfg.baseUrl + cfg.basePath;
            MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
            conn.Open();
            List<TaskProduzione> tskList = new List<TaskProduzione>();
            var taskIDs = conn.Query<int>("SELECT taskID FROM registroeventiproduzione WHERE TipoEvento LIKE 'Ritardo' AND segnalato = false");
            foreach (var taskID in taskIDs)
            {
                tskList.Add(new TaskProduzione(tenant, taskID));
            }
            for (int i = 0; i < tskList.Count; i++)
            {
                TimeSpan ritardo = tskList[i].ritardo;
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
                Commessa cm = new Commessa(wsName, art.Commessa, art.AnnoCommessa);
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
                        }
                    }
                }

                Postazione pst = new Postazione(tenant, tskList[i].PostazioneID);

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

                    mMessage.Body = "<html><body><div>" + ResEventsDelay.Ritardi.lblDelaySentence + ":<br/>"
                        + ResEventsDelay.Ritardi.lblDepartment + ": " + rp.name + "<br />"
                        + ResEventsDelay.Ritardi.lblWorkstations + ": " + pst.name + "<br />"
                        + ResEventsDelay.Ritardi.lblOrder + ": " + cm.ID.ToString() + "/" + cm.Year.ToString() + " " + "(" + cm.ExternalID + ") " + ResEventsDelay.Ritardi.lblForCustomer + " " + cm.RagioneSocialeCliente + "<br />"
                        + ResEventsDelay.Ritardi.lblProduct + ": " + art.ID.ToString() + "/" + art.Year.ToString() + " " + art.Proc.ExternalID + " " + art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante + "<br />"
                        + ResEventsDelay.Ritardi.lblTask + ": " + tskList[i].Name + " (" + tskList[i].TaskProduzioneID.ToString() + ")"
                        + "</div>"
                        + "<div>" + ResEventsDelay.Ritardi.lblGoToWorkstation + " <a href=\"" + baseURL
                             + "/Workplace/WebGemba/Index" + "\">" +
                             ResEventsDelay.Ritardi.lblClickHere
                             + "</a></div>"
                             + "<div>" + ResEventsDelay.Ritardi.lblGoToAndon + " <a href=\"" + baseURL
                        + "/Andon/DepartmentAndon/Index?DepartmentID=" + rp.id.ToString() + "\">" +
                        ResEventsDelay.Ritardi.lblClickHere
                        + "</a></div>"
                        + "</body></html>";

                    SmtpClient smtpcli = KIS.App_Code.Secrets.ConfigureSmtp(new SmtpClient());
                    smtpcli.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpcli.EnableSsl = true;
                    smtpcli.Send(mMessage);
                }

                // Metto il ritardo come già segnalato
                string sql = "UPDATE registroeventiproduzione SET segnalato = true WHERE TipoEvento LIKE 'Ritardo' AND taskID = " + tskList[i].TaskProduzioneID;
                try
                {
                    conn.Execute(sql);
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