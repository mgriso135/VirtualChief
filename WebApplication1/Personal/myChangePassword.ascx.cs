using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Personal
{
    public partial class myChangePassword : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["user"] != null)
                {
                    
                        User curr = (User)Session["user"];
                        curr.loadEmails();
                        if (curr.Email.Count > 0)
                        {

                        }
                        else
                        {
                            tblChangePassword.Visible = false;
                            lbl1.Text = "Attenzione: per cambiare la password devi impostare un indirizzo e-mail.";
                        }
                }
                else
                {
                    tblChangePassword.Visible = false;
                }
            }
        }

        protected void imgSavePassword_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["user"] != null)
            {
                String oldPass = Server.HtmlEncode(txtOldPassword.Text);
                String newPass1 = Server.HtmlEncode(txtNewPassword1.Text);
                String newPass2 = Server.HtmlEncode(txtNewPassword2.Text);

                User curr = new User(((User)Session["user"]).username, oldPass);
                if (curr.authenticated)
                {
                    int ret = curr.changePassword(oldPass, newPass1);
                    if (ret == 0)
                    {
                        lbl1.Text = "Errore generico...";
                    }
                    else if (ret == 1)
                    {
                        lbl1.Text = "Password cambiata correttamente.";
                    }
                    else if (ret == 2)
                    {
                        lbl1.Text = "Attenzione: la vecchia password fornita non è corretta.";
                    }
                }
                else
                {
                    lbl1.Text = "Vecchia password fornita non corretta.";                    
                }
            }
            else
            {
                tblChangePassword.Visible = false;
                lbl1.Text = "Password fornita non corretta.";
                ((User)Session["user"]).logout();
                Response.Redirect("~/Login/login.aspx");
            }
        }

        protected void imgUndoPassword_Click(object sender, ImageClickEventArgs e)
        {
            txtOldPassword.Text = "";
            txtNewPassword1.Text = "";
            txtNewPassword2.Text = "";
                   }

 
    }
}