using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using KIS.App_Code;

namespace KIS.Reparti
{
    public partial class configRepartoTimezone : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto Timezone";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            ddlTimezones.Visible = false;
            if (checkUser == true)
            {
                ddlTimezones.Visible = true;
                if (!Page.IsPostBack)
                {
                    Reparto rp = new Reparto(idReparto);
                    if(rp.id!=-1 && idReparto!=-1)
                    {                        
                        ddlTimezones.Items.Clear();
                    ddlTimezones.DataSource = TimeZoneInfo.GetSystemTimeZones();
                    ddlTimezones.DataTextField = "DisplayName";
                    ddlTimezones.DataValueField = "Id";
                    ddlTimezones.SelectedValue = rp.tzFusoOrario.Id;
                    ddlTimezones.DataBind();
                    }
                }
            }
            else
            {
                ddlTimezones.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void ddlTimezones_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(ddlTimezones.SelectedValue);
            Reparto rp = new Reparto(idReparto);
            if (rp.id != -1)
            {
                rp.fusoOrario = ddlTimezones.SelectedValue;
                lblInfo.Text = ddlTimezones.SelectedIndex + " - " + ddlTimezones.SelectedValue + "<br/>" + tz.Id
                    + " <br />" + rp.log;
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblError").ToString();
            }
        }
    }
}