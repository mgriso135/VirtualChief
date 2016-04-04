﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Login
{
    public partial class loginbox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (((User)Session["user"]) != null)
                {
                    tblLogin.Visible = false;
                    tblLogout.Visible = true;
                    if (((User)Session["user"]).authenticated == true)
                    {
                        lblInfoLogin.Text = "Logged in as: " + ((User)Session["user"]).username + "<br/>Last login at: " + ((User)Session["user"]).lastLogin.ToString();
                    }
                    else
                    {
                        lblInfoLogin.Text = "NOT logged in as: " + ((User)Session["user"]).username + "<br/>Last login at: " + ((User)Session["user"]).lastLogin.ToString();
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
                lblInfoLogin.Text = "Welcome, " + current.name + " " + current.cognome + "<br/>Last login at: " + ((User)Session["user"]).lastLogin.ToString();
                tblLogin.Visible = false;
            }
            else
            {
                lblInfoLogin.Text = "Errore: username non trovato o password non corretta.";
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