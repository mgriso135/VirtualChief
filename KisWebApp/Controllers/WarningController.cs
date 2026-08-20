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
    /// Replaces Eventi/Warning.asmx – finds open production warnings, emails configured recipients, marks as segnalato.
    /// Contract kept identical for scheduler clients.
    /// </summary>
    public class WarningController : ApiController
    {
        // GET: api/Warning?tenant=...
        [HttpGet]
        public IHttpActionResult Get(String tenant)
        {
            SegnalaWarning(tenant);
            return Ok("Warning check executed.");
        }

        // POST: api/Warning/Trigger
        [HttpPost]
        public IHttpActionResult Trigger(String tenant)
        {
            bool rt = SegnalaWarning(tenant);
            return Ok(rt ? "Warning trigger executed." : "Warning trigger failed.");
        }

        private bool SegnalaWarning(string tenant)
        {
            bool rt = false;
            MySqlConnection conn = (new Dati.Dati()).mycon(tenant);
            conn.Open();
            List<TaskProduzione> tskList = new List<TaskProduzione>();
            var taskIDs = conn.Query<int>("SELECT taskID FROM registroeventiproduzione WHERE TipoEvento LIKE 'Warning' AND segnalato = false");
            foreach (var taskID in taskIDs)
            {
                tskList.Add(new TaskProduzione(tenant, taskID));
            }
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

                    SmtpClient smtpcli = KIS.App_Code.Secrets.ConfigureSmtp(new SmtpClient());
                    smtpcli.Send(mMessage);
                }
                // Metto lo warning come già segnalato
                string sql = "UPDATE registroeventiproduzione SET segnalato = true WHERE TipoEvento LIKE 'Warning' AND taskID = " + tskList[i].TaskProduzioneID;
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