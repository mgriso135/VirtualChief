using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Reparti
{
    public partial class addReparto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                { 
                ddlTimezones.Items.Clear();
                ddlTimezones.DataSource = TimeZoneInfo.GetSystemTimeZones();
                ddlTimezones.DataTextField = "DisplayName";
                ddlTimezones.DataValueField = "Id";
                    ddlTimezones.SelectedValue = "W. Europe Standard Time";
                    ddlTimezones.DataBind();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmAddReparto.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString());
            int rt = rp.Add(Server.HtmlEncode(nome.Text), Server.HtmlEncode(descrizione.Text), Server.HtmlEncode(ddlTimezones.SelectedValue));
            if (rt != -1)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl1.Text = rp.err;
            }
        }

        protected void reset_Click(object sender, ImageClickEventArgs e)
        {
            nome.Text = "";
            descrizione.Text = "";
        }
    }
}