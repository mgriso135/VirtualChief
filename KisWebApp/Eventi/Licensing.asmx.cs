using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using KIS.App_Code;
using System.Net.Mail;

namespace KIS.Eventi
{
    /// <summary>
    /// Questo webservice implementa le funzioni di segnalazione della scadenza della licenza di Kaizen Indicator System via e-mail
    /// a tutti i membri del gruppo "Admin"
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class Licensing : System.Web.Services.WebService
    {
        [WebMethod]
        public string CheckLicense(String tenant)
        {
            // Verifico se devo segnalare qualcosa

            KISConfig kisCfg = new KISConfig(tenant);
            FusoOrario fuso = new FusoOrario(tenant);
            String ret = kisCfg.ExpiryDate.ToString("dd/MM/yyyy") + "<br />";
            String installationName = kisCfg.basePath;
            if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) >= kisCfg.ExpiryDate.AddDays(-30) &&
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) <= kisCfg.ExpiryDate.AddDays(+30))
            {
                List<MailAddress> MailList = new List<MailAddress>();
                int AdmGrpID = -1;
                GroupList grpList = new GroupList(tenant);
                for (int i = 0; i < grpList.Elenco.Count; i++)
                {
                    if (grpList.Elenco[i].Nome == "Admin")
                    {
                        AdmGrpID = grpList.Elenco[i].ID;
                    }
                }

                Group admGroup = new Group(tenant, AdmGrpID);
                admGroup.loadUtenti();
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
                    // Invio l'e-mail
                    System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();

                    mMessage.From = new MailAddress("info@virtualchief.net", "Scheduler@KaizenIndicatorSystem");
                    mMessage.To.Add(new MailAddress("info@virtualchief.net", "Scheduler@KaizenIndicatorSystem"));
                    for (int q = 0; q < MailList.Count; q++)
                    {
                        mMessage.CC.Add(MailList[q]);
                    }

                    mMessage.Subject = "Virtual Chief"+installationName+": license renewal needed";
                    mMessage.IsBodyHtml = true;

                    if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) >= kisCfg.ExpiryDate)
                    {
                        TimeSpan t = (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario) - kisCfg.ExpiryDate);
                        mMessage.Body = "Please pay attention: your Virtual Chief license is expired from "
                            + Math.Round(t.TotalDays,0) + " days."
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



                    SmtpClient smtpcli = new SmtpClient();
                    smtpcli.Send(mMessage);
                    ret += "Segnalato.";
                }
            }
            return ret;
        }
    }
}
