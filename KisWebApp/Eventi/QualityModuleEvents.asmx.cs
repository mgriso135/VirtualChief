using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using KIS.App_Sources;
using System.Net.Mail;
using KIS.App_Code;

namespace KIS.Eventi
{
    /// <summary>
    /// Descrizione di riepilogo per QualityModuleEvents
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class QualityModuleEvents : System.Web.Services.WebService
    {

        [WebMethod]
        public String NotifyImprovementActionsDelays()
        {
            KISConfig cfg = new KISConfig(Session["ActiveWorkspace_Name"].ToString());
            String baseURL = cfg.baseUrl + cfg.basePath;
            String ret = "";
            ImprovementActionsEvents LateIActs = new ImprovementActionsEvents(Session["ActiveWorkspace_Name"].ToString());
            LateIActs.loadLateImprovementActions();
            for (int i = 0; i < LateIActs.LateImprovementActions.Count; i++)
            {
                System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();

                mMessage.From = new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief");

                LateIActs.LateImprovementActions[i].loadTeamMembers();
                for (int j = 0; j < LateIActs.LateImprovementActions[i].TeamMembers.Count; j++)
                {
                    User curr = new User(LateIActs.LateImprovementActions[i].TeamMembers[j].User);
                    curr.loadEmails();
                    for (int k = 0; k < curr.Email.Count; k++)
                    {
                        if (curr.Email[k].ForAlarm)
                        {
                            mMessage.To.Add(new MailAddress(curr.Email[k].Email, curr.name + " " + curr.cognome));
                        }
                    }
                }
                mMessage.Subject = "[Virtual Chief] " + ResQualityEvents.QualityModuleEvents.lblTitleIADelay;
                mMessage.IsBodyHtml = true;

                mMessage.Body = "<html><body>"
                    + ResQualityEvents.QualityModuleEvents.lblIAShoulEnd + ": "
                    + LateIActs.LateImprovementActions[i].EndDateExpected.ToString("dd/MM/yyyy") + "<br />"
                    + "<b>" + ResQualityEvents.QualityModuleEvents.lblCurrentSituation + ":</b> "
                    + LateIActs.LateImprovementActions[i].CurrentSituation
                    + "<br /><br />"
                    + "<b>" + ResQualityEvents.QualityModuleEvents.lblExpectedResult + ":</b> "
                    + "<br /><br />"
                    + ResQualityEvents.QualityModuleEvents.lblFurtherDetails
                    + " <a href=\"" + baseURL + "/Quality/ImprovementActions/Update?ID="
                    + LateIActs.LateImprovementActions[i].ID.ToString()
                    + "&Year=" + LateIActs.LateImprovementActions[i].Year.ToString()
                    + "\" target=\"_blank\">"
                    + ResQualityEvents.QualityModuleEvents.lblClickHere
                    + "</a>"
                    + "</body></html>";

                if (mMessage.To.Count > 0)
                {
                    try
                    {
                        SmtpClient smtpcli = new SmtpClient();
                        smtpcli.Send(mMessage);
                    }
                    catch { }
                }

                ret += LateIActs.LateImprovementActions[i].ID
                    + "/" + LateIActs.LateImprovementActions[i].Year + " " + LateIActs.LateImprovementActions[i].EndDateExpected.ToString("dd/MM/yyyy") + "<br />";
            }
            return ret;
        }

        [WebMethod]
        public String NotifyCorrectiveActionsDelays(String tenant)
        {
            FusoOrario fuso = new FusoOrario(tenant);
            String tzOffset = "";
            tzOffset = fuso.tzFusoOrario.BaseUtcOffset.Ticks >= 0 ? "+" : "";
            tzOffset += fuso.tzFusoOrario.BaseUtcOffset.Hours + ":" + fuso.tzFusoOrario.BaseUtcOffset.Minutes;
            String ret = "Offset: " + tzOffset + "<br />Not finished:<br />";
            CorrectiveActionsEvents LateCActs = new CorrectiveActionsEvents(tenant);

            KISConfig cfg = new KISConfig(tenant);
            String baseURL = cfg.baseUrl + cfg.basePath;

            LateCActs.loadNotFinishedCorrectiveActions();
            ret += LateCActs.NotFinishedCorrectiveActions.Count + "<br />";
            for (int i = 0; i < LateCActs.NotFinishedCorrectiveActions.Count; i++)
            {
                System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
                mMessage.From = new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief");

                LateCActs.NotFinishedCorrectiveActions[i].loadTeamMembers();
                for (int j = 0; j < LateCActs.NotFinishedCorrectiveActions[i].TeamMembers.Count; j++)
                {
                    User curr = new User(LateCActs.NotFinishedCorrectiveActions[i].TeamMembers[j].User);
                    curr.loadEmails();
                    for (int k = 0; k < curr.Email.Count; k++)
                    {
                        if (curr.Email[k].ForAlarm)
                        {
                            mMessage.To.Add(new MailAddress(curr.Email[k].Email, curr.name + " " + curr.cognome));
                        }
                    }
                }

                ImprovementAction currIAct = new ImprovementAction(tenant, LateCActs.NotFinishedCorrectiveActions[i].ImprovementActionID, LateCActs.NotFinishedCorrectiveActions[i].ImprovementActionYear);
                currIAct.loadTeamMembers();
                for (int j = 0; j < currIAct.TeamMembers.Count; j++)
                {
                    if (currIAct.TeamMembers[j].Role == 'M')
                    {
                        User currManager = new App_Code.User(currIAct.TeamMembers[j].User);
                        currManager.loadEmails();
                        for (int k = 0; k < currManager.Email.Count; k++)
                        {
                            if (currManager.Email[k].ForAlarm)
                            {
                                mMessage.CC.Add(new MailAddress(currManager.Email[k].Email, currManager.name + " " + currManager.cognome));
                            }
                        }
                    }
                }

                mMessage.Subject = "[Virtual Chief] " + ResQualityEvents.QualityModuleEvents.lblTitleCADelay;
                mMessage.IsBodyHtml = true;

                mMessage.Body = "<html><body><div>"
                    + ResQualityEvents.QualityModuleEvents.lblCAShoulEnd + ": "
                    + LateCActs.NotFinishedCorrectiveActions[i].LateFinish.ToString("dd/MM/yyyy") + "<br />"
                    + LateCActs.NotFinishedCorrectiveActions[i].Description
                    + "<br /><br />"
                    + ResQualityEvents.QualityModuleEvents.lblFurtherDetails
                    + " <a href=\"" + baseURL + "/Quality/ImprovementActions/CorrectiveActionEdit?ImprovementActionID="
                    + LateCActs.NotFinishedCorrectiveActions[i].ImprovementActionID.ToString()
                    + "&ImprovementActionYear="
                    + LateCActs.NotFinishedCorrectiveActions[i].ImprovementActionYear.ToString()
                    + "&CorrectiveActionID="
                    + LateCActs.NotFinishedCorrectiveActions[i].CorrectiveActionID.ToString()
                    + "\" target=\"_blank\">"
                    + ResQualityEvents.QualityModuleEvents.lblClickHere
                    + "</a>"
                    + "</div></body></html>";


                if (mMessage.To.Count > 0)
                {
                    try
                    {
                        SmtpClient smtpcli = new SmtpClient();
                        smtpcli.Send(mMessage);
                    }
                    catch { }
                }

                ret += LateCActs.NotFinishedCorrectiveActions[i].ImprovementActionID
                    + "/" + LateCActs.NotFinishedCorrectiveActions[i].ImprovementActionYear
                    + " - " + LateCActs.NotFinishedCorrectiveActions[i].CorrectiveActionID + " "
                    + LateCActs.NotFinishedCorrectiveActions[i].LateFinish.ToString("dd/MM/yyyy") + "<br />";
            }

            ret += "Not started:<br />";

            LateCActs.loadNotStartedCorrectiveActions();
            ret += LateCActs.NotStartedCorrectiveActions.Count + "<br />";
            for (int i = 0; i < LateCActs.NotStartedCorrectiveActions.Count; i++)
            {
                System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
                mMessage.From = new MailAddress("info@virtualchief.net", "Scheduler@VirtualChief");

                LateCActs.NotStartedCorrectiveActions[i].loadTeamMembers();
                for (int j = 0; j < LateCActs.NotStartedCorrectiveActions[i].TeamMembers.Count; j++)
                {
                    User curr = new User(LateCActs.NotStartedCorrectiveActions[i].TeamMembers[j].User);
                    curr.loadEmails();
                    for (int k = 0; k < curr.Email.Count; k++)
                    {
                        if (curr.Email[k].ForAlarm)
                        {
                            mMessage.To.Add(new MailAddress(curr.Email[k].Email, curr.name + " " + curr.cognome));
                        }
                    }
                }

                ImprovementAction currIAct = new ImprovementAction(tenant, LateCActs.NotStartedCorrectiveActions[i].ImprovementActionID, LateCActs.NotStartedCorrectiveActions[i].ImprovementActionYear);
                currIAct.loadTeamMembers();
                for (int j = 0; j < currIAct.TeamMembers.Count; j++)
                {
                    if (currIAct.TeamMembers[j].Role == 'M')
                    {
                        User currManager = new App_Code.User(currIAct.TeamMembers[j].User);
                        currManager.loadEmails();
                        for (int k = 0; k < currManager.Email.Count; k++)
                        {
                            if (currManager.Email[k].ForAlarm)
                            {
                                mMessage.CC.Add(new MailAddress(currManager.Email[k].Email, currManager.name + " " + currManager.cognome));
                            }
                        }
                    }
                }
                mMessage.Subject = "[Virtual Chief] " + ResQualityEvents.QualityModuleEvents.lblTitleCADelay;
                mMessage.IsBodyHtml = true;

                mMessage.Body = "<html><body><div>"
                    + ResQualityEvents.QualityModuleEvents.lblCAShouldStart + ": "
                    + LateCActs.NotStartedCorrectiveActions[i].LateStart.ToString("dd/MM/yyyy") + "<br />"
                    + LateCActs.NotStartedCorrectiveActions[i].Description
                    + "<br /><br />"
                    + ResQualityEvents.QualityModuleEvents.lblFurtherDetails
                    + " <a href=\"" + baseURL + "/Quality/ImprovementActions/CorrectiveActionEdit?ImprovementActionID="
                    + LateCActs.NotStartedCorrectiveActions[i].ImprovementActionID.ToString()
                    + "&ImprovementActionYear="
                    + LateCActs.NotStartedCorrectiveActions[i].ImprovementActionYear.ToString()
                    + "&CorrectiveActionID="
                    + LateCActs.NotStartedCorrectiveActions[i].CorrectiveActionID.ToString()
                    + "\" target=\"_blank\">"
                    + ResQualityEvents.QualityModuleEvents.lblClickHere
                    + "</a>"
                    + "</div></body></html>";


                if (mMessage.To.Count > 0)
                {
                    try
                    {
                        SmtpClient smtpcli = new SmtpClient();
                        smtpcli.Send(mMessage);
                    }
                    catch { }
                }

                ret += LateCActs.NotStartedCorrectiveActions[i].ImprovementActionID
                    + "/" + LateCActs.NotStartedCorrectiveActions[i].ImprovementActionYear
                    + " - " + LateCActs.NotStartedCorrectiveActions[i].CorrectiveActionID + " "
                    + LateCActs.NotStartedCorrectiveActions[i].LateStart.ToString("dd/MM/yyyy") + "<br />";
            }
            return ret;
        }
    }
}
