using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Eventi/Licensing.asmx – License expiry e‑mail alerts to Admin group.
    /// Contract kept identical for scheduler clients.
    /// </summary>
    public class LicensingController : ApiController
    {
        /// <summary>
        /// GET api/Licensing/CheckLicense?tenant=...
        /// Mirrors Licensing.asmx CheckLicense(tenant): warns the Admin group by e‑mail
        /// when the license is within ±30 days of expiry, and returns a summary string.
        /// </summary>
        [HttpGet]
        public string CheckLicense(String tenant)
        {
            KISConfig kisCfg = new KISConfig(tenant);
            FusoOrario fuso = new FusoOrario(tenant);
            String ret = kisCfg.ExpiryDate.ToString("dd/MM/yyyy") + "<br />";
            String installationName = kisCfg.basePath;
            if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) >= kisCfg.ExpiryDate.AddDays(-30) &&
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) <= kisCfg.ExpiryDate.AddDays(+30))
            {
                List<MailAddress> MailList = new List<MailAddress>();
                int AdmGrpID = -1;
                GroupList grpList = new GroupList();
                for (int i = 0; i < grpList.Elenco.Count; i++)
                {
                    if (grpList.Elenco[i].Nome == "Admin")
                    {
                        AdmGrpID = grpList.Elenco[i].ID;
                    }
                }

                Workspace ws = new Workspace(tenant);
                Group admGroup = new Group(AdmGrpID);
                admGroup.loadUtenti(ws.id);
                for (int i = 0; i < admGroup.Utenti.Count; i++)
                {
                    User usr = new User(admGroup.Utenti[i]);
                    usr.loadEmails();
                    for (int j = 0; j < usr.Email.Count; j++)
                    {
                        if (usr.Email[j].ForAlarm)
                        {
                            MailList.Add(new MailAddress(usr.Email[j].Email, usr.FullName));
                            ret += usr.Email[j].Email + "<br />";
                        }
                    }
                }
                if (MailList.Count > 0)
                {
                    System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();

                    mMessage.From = new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief");
                    mMessage.To.Add(new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief"));
                    for (int q = 0; q < MailList.Count; q++)
                    {
                        mMessage.CC.Add(MailList[q]);
                    }

                    mMessage.Subject = "Virtual Chief" + installationName + ": license renewal needed";
                    mMessage.IsBodyHtml = true;

                    if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) >= kisCfg.ExpiryDate)
                    {
                        TimeSpan t = (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) - kisCfg.ExpiryDate);
                        mMessage.Body = "Please pay attention: your Virtual Chief license is expired from "
                            + Math.Round(t.TotalDays, 0) + " days."
                            + "<hr />"
                            + "Atención: su licencia de Virtual Chief expiró a partir de " + Math.Round(t.TotalDays, 0) + " días."
                            + "<hr />"
                            + "Attenzione: la licenza di Virtual Chief è scaduta da " + Math.Round(t.TotalDays, 0) + " giorni."
                            ;
                    }
                    else
                    {
                        TimeSpan t = (kisCfg.ExpiryDate - TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario));
                        mMessage.Body = "Please pay attention: your Virtual Chief license will expire in "
                            + Math.Round(t.TotalDays, 0) + " days."
                            + "<hr />"
                            + "Atención: su licencia de Virtual Chief caducará en " + Math.Round(t.TotalDays, 0) + " días."
                            + "<hr />"
                            + "Attenzione: la licenza di Virtual Chief scadrà fra " + Math.Round(t.TotalDays, 0) + " giorni.";
                    }

                    SmtpClient smtpcli = KIS.App_Code.Secrets.ConfigureSmtp(new SmtpClient());
                    smtpcli.Send(mMessage);
                    ret += "Segnalato.";
                }
            }
            return ret;
        }

        // GET: api/Licensing
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(CheckLicense(WebEnv.ActiveWorkspaceName));
        }

        // POST: api/Licensing/Refresh
        [HttpPost]
        public IHttpActionResult Refresh()
        {
            return Ok(CheckLicense(WebEnv.ActiveWorkspaceName));
        }
    }
}