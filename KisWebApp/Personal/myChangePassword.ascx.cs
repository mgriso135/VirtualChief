using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
                    
                        UserAccount curr = (UserAccount)Session["user"];
                        curr.loadEmails();
                        if (curr.Email.Count > 0)
                        {

                        }
                        else
                        {
                            tblChangePassword.Visible = false;
                            lbl1.Text = GetLocalResourceObject("lblAddEMail").ToString();
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
                        lbl1.Text = GetLocalResourceObject("lblGenericError").ToString();
                    }
                    else if (ret == 1)
                    {
                        lbl1.Text = GetLocalResourceObject("lblPasswordChangedOK").ToString();
                    }
                    else if (ret == 2)
                    {
                        lbl1.Text = GetLocalResourceObject("lblOldPasswordNonOK").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblOldPasswordNonOK").ToString();
                }
            }
            else
            {
                tblChangePassword.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPasswordNonOK").ToString();
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