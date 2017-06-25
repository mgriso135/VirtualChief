using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Configuration
{
    public partial class wizConfigReparti_Main1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmAddReparto.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto";
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
                ElencoReparti elRep = new ElencoReparti();
                if (cfg.WizRepartiCompleted)
                {
                    frmAddReparto.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblDeptConfigured").ToString();

                }

                    String repartiNC = "";
                    
                    for (int i = 0; i < elRep.elenco.Count; i++)
                    {

                        if (!elRep.elenco[i].FullyConfigured)
                        {
                            repartiNC += "<a href =\"/Configuration/wizConfigReparti_Detail.aspx"
                                + "?id=" + elRep.elenco[i].id.ToString() + "\" target=\"_blank\">"
                                + elRep.elenco[i].name + "</a><br />";
                        }
                    }

                    if (repartiNC.Length > 0)
                    {
                        lbl1.Text += "<br /><br />"+GetLocalResourceObject("lblDeptConfigured2") +":<br />" + repartiNC
                            + " <br /><br /><br />";
                    }

                frmAddReparto.Visible = true;
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
                lbl1.Text = "<a href=\"/Login/login.aspx"
                    + "?red=/Configuration/wizConfigReparti_Main\">"
                    + GetLocalResourceObject("lblLnkLogin").ToString()
                    + "</a>";
                frmAddReparto.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            Reparto rp = new Reparto();
            int rt = rp.Add(Server.HtmlEncode(nome.Text), Server.HtmlEncode(descrizione.Text), Server.HtmlEncode(ddlTimezones.SelectedValue));
            if (rt != -1)
            {
                Response.Redirect("~/Configuration/wizConfigReparti_Detail.aspx?id=" + rt.ToString());
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