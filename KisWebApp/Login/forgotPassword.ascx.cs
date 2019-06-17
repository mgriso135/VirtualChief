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
                        mMessage.Subject = GetLocalResourceObject("lblMailSubj").ToString();
                        mMessage.IsBodyHtml = true;

                        mMessage.Body = GetLocalResourceObject("lblMailBody1").ToString()
                            +GetLocalResourceObject("lblMailBody2").ToString() +": " + newPass + "<br/><br/><br/>KIS Robot";
                        SmtpClient smtpcli = new SmtpClient();
                        try
                        {
                            smtpcli.Send(mMessage);
                            lbl1.Text = GetLocalResourceObject("lblMailSent").ToString();
                        }
                        catch
                        {
                            lbl1.Text = GetLocalResourceObject("lblMailKO").ToString();
                        }


                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblGenericError").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblNoMailAddress").ToString();
                }
            }
            else
            {
                lbl1.Text = "Errore generico.<br/>";
                lbl1.Text = GetLocalResourceObject("lblGenericError").ToString();
            }
            txtUsername.Text = "";
        }
    }
}