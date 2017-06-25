﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Configuration
{
    public partial class wizConfigAndon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmFormatoUsername.Visible = false;
            frmViewFields.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "AndonCompleto VisualizzazioneNomiUtente";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "AndonCompleto CampiDaVisualizzare";
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
                frmFormatoUsername.Visible = true;
                frmViewFields.Visible = true;
                if (!Page.IsPostBack)
                {

                }
            }
            else
            {
                lbl1.Text = "<a href=\"/Login/login.aspx"
                    + "?red=/Configuration/wizConfigAndon\">"+ GetLocalResourceObject("lblLnkLogin").ToString() + ".</a>";
                frmFormatoUsername.Visible = false;
                frmViewFields.Visible = false;
                accordion1.Visible = false;
            }
        }
    }
}