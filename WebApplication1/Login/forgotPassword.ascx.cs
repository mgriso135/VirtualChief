using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using KIS.App_Code;
namespace KIS.Login
{
    public partial class forgotPassword : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUsername_Click(object sender, EventArgs e)
        {
            String usrID = Server.HtmlEncode(txtUsername.Text);
            User curr = new User(usrID);
            if (curr.username.Length > 0 && curr.username != "")
            {
                curr.loadEmails();
                String newPass = curr.ResetPassword();
                if (curr.Email.Count > 0)
                {
                    if (newPass != "" && newPass.Length > 0)
                    {
                        // Invio l'e-mail
                        System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
                        mMessage.From = new MailAddress("tools@kaizenpeople.it", "robot@kis");
                        for (int q = 0; q < curr.Email.Count; q++)
                        {
                            mMessage.To.Add(new MailAddress(curr.Email[q].Email));
                        }
                        mMessage.Bcc.Add("tools@kaizenpeople.it");
                        mMessage.Subject = "KIS - Recupero password";
                        mMessage.IsBodyHtml = true;

                        mMessage.Body = "Tu, o qualcuno per conto tuo, ha richiesto l'invio della tua password.<br/>"
                            + "Per motivi di sicurezza l'invio della vecchia password non è possibile e quindi è stata resettata."
                            + " Inoltre questa viene inviata esclusivamente ai tuoi indirizzi e-mail.<br />"
                            + "La tua nuova password è: " + newPass + "<br/><br/><br/>KIS Robot";
                        SmtpClient smtpcli = new SmtpClient();
                        try
                        {
                            smtpcli.Send(mMessage);
                            lbl1.Text = "Abbiamo inviato l'informazione richiesta al tuo indirizzo e-mail.<br />";
                        }
                        catch
                        {
                            lbl1.Text = "E' avvenuto un errore imprevisto. Verificare lo username o gli indirizzi e-mail inseriti e riprovare.<br />";
                        }


                    }
                    else
                    {
                        lbl1.Text = "Errore generico.<br/>";
                    }
                }
                else
                {
                    lbl1.Text = "Attenzione: non hai impostato nessun indirizzo e-mail per questo utente, quindi non è possibile resettare la password. Contatta il tuo amministratore di sistema.<br />";
                }
            }
            else
            {
                lbl1.Text = "Errore generico.<br/>";
            }
            txtUsername.Text = "";
        }
    }
}