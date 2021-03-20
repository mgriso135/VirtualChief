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
    public partial class configurationStatus : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["ActiveWorkspace_Id"]!=null && Session["IsWorkspaceAdmin"]!=null && Session["IsWorkspaceAdmin"].ToString() == "1")
            {
                Workspace ws = new Workspace(Int32.Parse(Session["ActiveWorkspace_Id"].ToString())); ;
                KISConfig kisCfg = new KISConfig(ws.Name);

                timezoneOK.Visible = kisCfg.WizTimezoneCompleted;
                timezoneKO.Visible = !kisCfg.WizTimezoneCompleted;

                andonOK.Visible = kisCfg.WizAndonCompleted;
                andonKO.Visible = !kisCfg.WizAndonCompleted;

                reportOK.Visible = kisCfg.WizCustomerReportCompleted;
                reportKO.Visible = !kisCfg.WizCustomerReportCompleted;

                if (kisCfg.WizLogoCompleted)
                {
                    logoOK.Visible = true;
                    logoKO.Visible = false;
                }
                else
                {
                    logoOK.Visible = false;
                    logoKO.Visible = true;
                }

                if (kisCfg.WizRepartiCompleted)
                {
                    repartoOK.Visible = true;
                    repartoKO.Visible = false;
                }
                else
                {
                    repartoOK.Visible = false;
                    repartoKO.Visible = true;
                }

                if (kisCfg.WizPostazioniCompleted)
                {
                    PostazioniOK.Visible = true;
                    PostazioniKO.Visible = false;
                }
                else
                {
                    PostazioniOK.Visible = false;
                    PostazioniKO.Visible = true;
                }

                andonOK.Visible = kisCfg.WizAndonCompleted;
                andonKO.Visible = !kisCfg.WizAndonCompleted;

                reportOK.Visible = kisCfg.WizCustomerReportCompleted;
                reportKO.Visible = !kisCfg.WizCustomerReportCompleted;

                UserAccount curr = (UserAccount)Session["user"];
                lbl1.Text = "KISConfig: " + kisCfg.Language + "<br />";
                if(curr!=null)
                {
                    lbl1.Text += "User: " + curr.Language + "<br />";
                }
                lbl1.Text += "CurrentCulture: " + System.Threading.Thread.CurrentThread.CurrentCulture + "<br />";
            }
        }
    }
}