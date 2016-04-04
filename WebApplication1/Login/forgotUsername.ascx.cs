using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;

namespace KIS.Login
{
    public partial class forgotUsername : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnUsername_Click(object sender, EventArgs e)
        {
            System.Net.Mail.MailAddress mailAddr;
            try
            {
                mailAddr = new System.Net.Mail.MailAddress(Server.HtmlEncode(txtEmail.Text));
            }
            catch
            {
                mailAddr = null;
            }

            if (mailAddr != null)
            {
                bool found = false;
                UserList lista = new UserList();
                for (int i = 0; i < lista.numUsers; i++)
                {
                    lista.elencoUtenti[i].loadEmails();
                    for (int j = 0; j < lista.elencoUtenti[i].Email.Count; j++)
                    {
                        if (mailAddr.Address == lista.elencoUtenti[i].Email[j].Email)
                        {
                            found = true;
                            // Invio l'e-mail
                            System.Net.Mail.MailMessage mMessage = new System.Net.Mail.MailMessage();
                            mMessage.From = new MailAddress("tools@kaizenpeople.it", "robot@kis");
                            mMessage.To.Add(mailAddr);
                            mMessage.Bcc.Add("tools@kaizenpeople.it");
                            mMessage.Subject = "KIS - Recupero username";
                            mMessage.IsBodyHtml = true;

                            mMessage.Body = "Tu, o qualcuno per conto tuo, ha richiesto l'invio del tuo username.<br/>"
                                + "Per motivi di sicurezza questo viene inviato esclusivamente alla tua e-mail.<br />"
                                + "Lo username richiesto è: " + lista.elencoUtenti[i].username + "<br/><br/><br/>KIS Robot";
                            SmtpClient smtpcli = new SmtpClient();
                            try
                            {
                                smtpcli.Send(mMessage);
                                lbl1.Text = "Abbiamo inviato l'informazione richiesta al tuo indirizzo e-mail.<br />";
                            }
                            catch
                            {
                                lbl1.Text = "E' avvenuto un errore imprevisto. Verificare l'indirizzo e-mail e riprovare.<br />";
                            }
                        }
                    }
                }

                if (found == false)
                {
                    lbl1.Text = "Non abbiamo trovato nessun utente con l'indirizzo e-mail da te segnalato. Per favore verifica che sia quello corretto.<br/>";
                }
            }
            else
            {
                lbl1.Text = "L'indirizzo e-mail inserito non è valido.<br />";
            }
            txtEmail.Text = "";
        }
    }
}