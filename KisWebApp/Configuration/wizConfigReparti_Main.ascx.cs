using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                KISConfig cfg = new KISConfig(Session["ActiveWorkspace"].ToString());
                ElencoReparti elRep = new ElencoReparti(Session["ActiveWorkspace"].ToString());
                if (cfg.WizRepartiCompleted)
                {
                    frmAddReparto.Visible = false;
                    lbl1.Text = GetLocalResourceObject("lblDeptConfigured").ToString();

                }

                rptReparti.Visible = false;
                    if(elRep.elenco.Count > 0)
                {
                    rptReparti.Visible = true;
                    rptReparti.DataSource = elRep.elenco;
                    rptReparti.DataBind();

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
                lbl1.Text = "<a href=\"../Login/login.aspx"
                    + "?red=/Configuration/wizConfigReparti_Main.aspx\">"
                    + GetLocalResourceObject("lblLnkLogin").ToString()
                    + "</a>";
                frmAddReparto.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString());
            int rt = rp.Add(Server.HtmlEncode(nome.Text), Server.HtmlEncode(descrizione.Text), Server.HtmlEncode(ddlTimezones.SelectedValue));
            if (rt != -1)
            {
                Response.Redirect("~/Configuration/wizConfigReparti_Main.aspx");
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

        protected void rptReparti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                String sRepID = ((HiddenField)e.Item.FindControl("hdRepID")).Value;
                Image imgOk = (Image)e.Item.FindControl("imgOk");
                Image imgKo = (Image)e.Item.FindControl("imgKo");
                Image imgCfg = (Image)e.Item.FindControl("imgCfg");
                HyperLink hCfg = (HyperLink)e.Item.FindControl("lnkCfg");

                int repID = -1;
                try
                {
                    repID = Int32.Parse(sRepID);
                }
                catch(Exception ex)
                {
                    repID = -1;
                }

                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);

                hCfg.NavigateUrl = "~/Configuration/wizConfigReparti_Detail.aspx?id=" + rp.id.ToString();

                if (rp.FullyConfigured)
                {
                    imgOk.Visible = true;
                    imgKo.Visible = false;
                    imgCfg.Visible = false;
                }
                else
                {
                    imgOk.Visible = false;
                    imgKo.Visible = true;
                    imgCfg.Visible = true;
                }
            }
        }
    }
}