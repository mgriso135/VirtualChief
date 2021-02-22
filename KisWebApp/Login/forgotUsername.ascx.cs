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
                UserList lista = new UserList(Session["ActiveWorkspace"].ToString());
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
                            mMessage.From = new MailAddress("info@virtualchief.net", "Matteo@VirtualChief");
                            mMessage.To.Add(mailAddr);
                            mMessage.Bcc.Add("info@virtualchief.net");
                            mMessage.Subject = "Virtual Chief - Recupero username";
                            mMessage.IsBodyHtml = true;

                            mMessage.Body = GetLocalResourceObject("lblMail1").ToString()
                                +GetLocalResourceObject("lblMail2").ToString()
                                + ": " + lista.elencoUtenti[i].username + "<br/><br/><br/>KIS Robot";
                            SmtpClient smtpcli = new SmtpClient();
                            try
                            {
                                smtpcli.Send(mMessage);
                                lbl1.Text = GetLocalResourceObject("lblMailSendOK").ToString();
                            }
                            catch
                            {
                                lbl1.Text = GetLocalResourceObject("lblMailSendKO").ToString(); 
                            }
                        }
                    }
                }

                if (found == false)
                {
                    lbl1.Text = GetLocalResourceObject("lblMailNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblMailNotValid").ToString(); 
            }
            txtEmail.Text = "";
        }
    }
}