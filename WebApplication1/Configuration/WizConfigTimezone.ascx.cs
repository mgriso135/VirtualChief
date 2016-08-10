using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

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
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                KISConfig cfg = new KISConfig();

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
                        FusoOrario fo = new FusoOrario();
                        //ddlTimezones.SelectedValue = fo.tzFusoOrario.Id;
                        ddlTimezones.DataBind();
                    }
                }
                else
                {
                    lbl1.Text = "Timezone is already configured and it is no longer possible to change it using this wizard.<br />"
                        + "Please log-in to the configuration section and be sure you have Admin permissions.";
                }
            }
            else
            {
                lbl1.Text = "Please <a href=\"/Login/login.aspx"
                    + "?red=/Configuration/wizConfigTimezone\">click here</a> to login as Admin User.";
            }
        }

        /*protected void ddlTimezones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlTimezones.SelectedValue.Length > 0)
            { 
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(ddlTimezones.SelectedValue);
            FusoOrario current = new FusoOrario();
            current.fusoOrario = ddlTimezones.SelectedValue;
            lbl1.Text = ddlTimezones.SelectedIndex + " - " + ddlTimezones.SelectedValue + "<br/>" + tz.Id
                + " <br />" + current.log;
            Response.Redirect("~/Configuration/MainWizConfig.aspx");
            }
            else
            {
                lbl1.Text = "Please choose a Timezone.";
            }
        }*/

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlTimezones.SelectedValue.Length > 0)
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(ddlTimezones.SelectedValue);
                FusoOrario current = new FusoOrario();
                current.fusoOrario = ddlTimezones.SelectedValue;
                lbl1.Text = ddlTimezones.SelectedIndex + " - " + ddlTimezones.SelectedValue + "<br/>" + tz.Id
                    + " <br />" + current.log;
                Response.Redirect("~/Configuration/MainWizConfig.aspx");
            }
            else
            {
                lbl1.Text = "Please choose a Timezone.";
            }
        }
    }
}