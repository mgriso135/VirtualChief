﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Configuration
{
    public partial class WizConfigTimezone1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlTimezones.Visible = false;
            btnSave.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Configurazione TimeZone";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
            try
            { 
            if (checkUser == true)
            {
                KISConfig cfg = new KISConfig(Session["ActiveWorkspace_Name"].ToString());

                if (!cfg.WizTimezoneCompleted)
                {
                    ddlTimezones.Visible = true;
                    btnSave.Visible = true;
                    if (!Page.IsPostBack)
                    {
                        ddlTimezones.Items.Clear();
                        ddlTimezones.Items.Add(new ListItem("Select Timezone", ""));
                        ddlTimezones.DataSource = TimeZoneInfo.GetSystemTimeZones();
                        ddlTimezones.DataTextField = "DisplayName";
                        ddlTimezones.DataValueField = "Id";
                        FusoOrario fo = new FusoOrario(Session["ActiveWorkspace_Name"].ToString());
                        //ddlTimezones.SelectedValue = fo.tzFusoOrario.Id;
                        ddlTimezones.DataBind();
                    }
                }
                else
                {
                    //lbl1.Text = GetLocalResourceObject("lblTimezoneConfigured").ToString();
                }
            }
            else
            {
                    lbl1.Text = "<a href=\"../Login/login.aspx"
                        + "?red=/Configuration/wizConfigTimezone.aspx\">" +
                    GetLocalResourceObject("lblLnkLoginAdmin").ToString()
                    +".</a>";
            }
            }
            catch(Exception ex)
            {
                lbl1.Visible = true;
                lbl1.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlTimezones.SelectedValue.Length > 0)
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(ddlTimezones.SelectedValue);
                FusoOrario current = new FusoOrario(Session["ActiveWorkspace_Name"].ToString());
                current.fusoOrario = ddlTimezones.SelectedValue;
                
                Response.Redirect("~/Configuration/MainWizConfig.aspx");
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblChooseTimezone").ToString();
            }
        }
    }
}