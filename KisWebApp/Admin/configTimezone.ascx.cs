using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Admin
{
    public partial class configTimezone : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlTimezones.Visible = false;
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
                ddlTimezones.Visible = true;
                if (!Page.IsPostBack)
                {
                    ddlTimezones.Items.Clear();
                    ddlTimezones.DataSource = TimeZoneInfo.GetSystemTimeZones();
                    ddlTimezones.DataTextField = "DisplayName";
                    ddlTimezones.DataValueField = "Id";
                    FusoOrario fo = new FusoOrario();
                    ddlTimezones.SelectedValue = fo.tzFusoOrario.Id;
                    ddlTimezones.DataBind();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermesso_KO").ToString();
            }
        }

        protected void ddlTimezones_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(ddlTimezones.SelectedValue);
            FusoOrario current = new FusoOrario();
            current.fusoOrario = ddlTimezones.SelectedValue;
            lbl1.Text = ddlTimezones.SelectedIndex + " - " + ddlTimezones.SelectedValue + "<br/>" + tz.Id 
                + " <br />" + current.log;
            
        }
    }
}