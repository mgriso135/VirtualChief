﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblPermessoKo").ToString();
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
            Postazione pst = new Postazione(Session["ActiveWorkspace_Name"].ToString());
            Boolean barcodeAutoCheckIn = ddlAutoCheckIn.SelectedValue == "1" ? true : false;
            bool rt = pst.add(Server.HtmlEncode(nomePost.Text), Server.HtmlEncode(descPost.Text), barcodeAutoCheckIn);
            if (rt == true)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblGenericError").ToString();
            }
        }
    }
}