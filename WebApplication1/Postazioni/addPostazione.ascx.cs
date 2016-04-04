﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;

namespace KIS.Produzione
{
    public partial class addPostazione : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
            }
            else
            {
                lblErr.Text = "Attenzione. Non hai il permesso di aggiungere postazioni di lavoro.<br/>";
                frmAddPostazione.Visible = false;
            }
        }

        protected void undo_Click(object sender, EventArgs e)
        {
            nomePost.Text = "";
            descPost.Text = "";
        }

        protected void save_Click(object sender, EventArgs e)
        {
            Postazione pst = new Postazione();
            bool rt = pst.add(Server.HtmlEncode(nomePost.Text), Server.HtmlEncode(descPost.Text));
            if (rt == true)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lblErr.Text = "Some error occured.<br/>";
            }
        }
    }
}