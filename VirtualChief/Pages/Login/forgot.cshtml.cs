using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KIS.App_Code;
using KIS.App_Sources;
using System.Collections.Generic;

namespace VirtualChief.Pages.Login
{
    /// <summary>
    /// Migrated from WebForms Login/forgot.aspx + forgotUsername.ascx +
    /// forgotPassword.ascx: recovery of username or password.
    /// The tenant is unknown before signing in, so every enabled workspace is
    /// scanned — the same discovery rule used by the login page. E-mails are
    /// sent through the legacy Secrets SMTP seam; when SMTP is not configured
    /// the legacy failure messages are shown.
    /// </summary>
    [AllowAnonymous]
    public class forgotModel : PageModel
    {
        private readonly ILogger<forgotModel> _logger;

        public forgotModel(ILogger<forgotModel> logger)
        {
            _logger = logger;
        }

        // Legacy resx: forgotPassword.ascx / forgotUsername.ascx
        public const string Disclaimer =
            "Attenzione: per motivi di sicurezza la password verrà resettata e spedita all'indirizzo e-mail predefinito dell'utente indicato.";
        public const string MailSubjPassword = "KIS - Recupero password";
        public const string MailBodyPassword =
            "Tu, o qualcuno per conto tuo, ha richiesto l'invio della tua password.<br/>Per motivi di sicurezza l'invio della vecchia password non è possibile e quindi è stata resettata. Inoltre questa viene inviata esclusivamente ai tuoi indirizzi e-mail.<br />";
        public const string MsgMailSent = "Abbiamo inviato l'informazione richiesta al tuo indirizzo e-mail.";
        public const string MsgMailKO = "E' avvenuto un errore imprevisto. Verificare lo username o gli indirizzi e-mail inseriti e riprovare.";
        public const string MsgNoMailAddress =
            "Attenzione: non hai impostato nessun indirizzo e-mail per questo utente, quindi non è possibile resettare la password. Contatta il tuo amministratore di sistema.";
        public const string MsgGenericError = "Errore generico.";
        public const string MailSubjUsername = "Virtual Chief - Recupero username";
        public const string MsgUsernameNotFound =
            "Non abbiamo trovato nessun utente con l'indirizzo e-mail da te segnalato. Per favore verifica che sia quello corretto.";
        public const string MsgEmailNotValid = "L'indirizzo e-mail inserito non è valido.";

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        /// <summary>forgotUsername.ascx.btnUsername_Click: send the username to the matching e-mail.</summary>
        public IActionResult OnPostUsername()
        {
            MailAddress mailAddr;
            try
            {
                mailAddr = new MailAddress(WebUtility.HtmlEncode(Email));
            }
            catch
            {
                mailAddr = null;
            }

            if (mailAddr == null)
            {
                Message = MsgEmailNotValid;
                return Page();
            }

            bool found = false;
            foreach (var ws in LoadWorkspaces())
            {
                try
                {
                    var wsObj = new Workspace(ws.Name);
                    wsObj.loadUserAccounts();
                    foreach (var ua in wsObj.UserAccounts)
                    {
                        if (string.Equals(ua.email?.Address, mailAddr.Address, StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            SendMail(
                                from: new MailAddress("info@virtualchief.net", "Matteo@VirtualChief"),
                                to: mailAddr,
                                bcc: "info@virtualchief.net",
                                subject: MailSubjUsername,
                                body: "Tu, o qualcuno per conto tuo, ha richiesto l'invio del tuo username.<br/>"
                                    + "Per motivi di sicurezza questo viene inviato esclusivamente alla tua e-mail.<br />"
                                    + "Lo username richiesto è: " + ua.userId + "<br/><br/><br/>KIS Robot");
                            Message = MsgMailSent;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Login/forgot.cshtml.cs: ricerca username su {Workspace}", ws.Name);
                }
            }

            if (!found)
            {
                Message = MsgUsernameNotFound;
            }
            Email = "";
            return Page();
        }

        /// <summary>forgotPassword.ascx.btnUsername_Click: reset the password and mail it.</summary>
        public IActionResult OnPostPassword()
        {
            var usrID = WebUtility.HtmlEncode(Username);
            bool handled = false;

            foreach (var ws in LoadWorkspaces())
            {
                User curr;
                try
                {
                    curr = new User(ws.Name, usrID);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Login/forgot.cshtml.cs: ricerca utente su {Workspace}", ws.Name);
                    continue;
                }

                if (string.IsNullOrEmpty(curr.username))
                {
                    continue;
                }

                curr.loadEmails();
                if (curr.Email == null || curr.Email.Count == 0)
                {
                    Message = MsgNoMailAddress;
                    return Page();
                }

                var newPass = curr.ResetPassword();
                if (string.IsNullOrEmpty(newPass))
                {
                    Message = MsgGenericError;
                    return Page();
                }

                var msg = SendMail(
                    from: new MailAddress("tools@kaizenpeople.it", "robot@kis"),
                    to: null,
                    bcc: "tools@kaizenpeople.it",
                    subject: MailSubjPassword,
                    body: MailBodyPassword + "La nuova password è: " + newPass + "<br/><br/><br/>KIS Robot",
                    recipients: curr.Email.Select(e => new MailAddress(e.Email)).ToList());
                Message = msg ? MsgMailSent : MsgMailKO;
                handled = true;
                break;
            }

            if (!handled && string.IsNullOrEmpty(Message))
            {
                Message = MsgGenericError;
            }
            Username = "";
            return Page();
        }

        private List<Workspace> LoadWorkspaces()
        {
            try
            {
                var elenco = new Workspaces();
                elenco.loadWorkspaces();
                return elenco.workspaces ?? new List<Workspace>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login/forgot.cshtml.cs: caricamento workspaces");
                return new List<Workspace>();
            }
        }

        private bool SendMail(MailAddress from, MailAddress to, string bcc,
            string subject, string body, List<MailAddress> recipients = null)
        {
            try
            {
                using var mMessage = new MailMessage();
                mMessage.From = from;
                if (to != null)
                {
                    mMessage.To.Add(to);
                }
                foreach (var r in recipients ?? new List<MailAddress>())
                {
                    mMessage.To.Add(r);
                }
                mMessage.Bcc.Add(bcc);
                mMessage.Subject = subject;
                mMessage.IsBodyHtml = true;
                mMessage.Body = body;

                using SmtpClient smtpcli = KIS.App_Code.Secrets.ConfigureSmtp(new SmtpClient());
                smtpcli.Send(mMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login/forgot.cshtml.cs: invio e-mail fallito");
                return false;
            }
        }
    }
}
