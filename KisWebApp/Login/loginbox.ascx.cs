using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Login
{
    public partial class loginbox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (((UserAccount)Session["user"]) != null)
                {
                    forgotPassword.Visible = false;
                    tblLogin.Visible = false;
                    tblLogout.Visible = true;
                    if (((UserAccount)Session["user"]).userId.Length > 0)
                    {
                        lblInfoLogin.Text =GetLocalResourceObject("lblLoggedIn1").ToString()
                            + ": " + ((User)Session["user"]).username 
                            + "<br/>"
                            + GetLocalResourceObject("lblLoggedIn2").ToString()+": " + ((User)Session["user"]).lastLogin.ToString();
                    }
                    else
                    {
                        lblInfoLogin.Text = GetLocalResourceObject("lblNotLoggedIn").ToString()+": " + ((User)Session["user"]).username;
                    }
                }
                else
                {
                    tblLogin.Visible = true;
                    tblLogout.Visible = false;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            User current = new User(Server.HtmlEncode(usr.Text), Server.HtmlEncode(pwd.Text));
            lblInfoLogin.Text = current.authenticated.ToString() + " " + current.name + " " + current.cognome + "<br/>";
            if (current.authenticated == true)
            {
                Session["user"] = current;
                tblLogin.Visible = false;
                String redUrl = "~/";
                if (!String.IsNullOrEmpty(Request.QueryString["red"]) && (Request.QueryString["red"]).Length>0)
                {
                    redUrl += Request.QueryString["red"];
                }
                else if(current.DestinationURL.Length > 0)
                {
                    redUrl = current.DestinationURL;
                }
                Response.Redirect(redUrl);
            }
            else
            {
                lblInfoLogin.Text = GetLocalResourceObject("lblErrorWrongAccess").ToString();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Session.Abandon();
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}