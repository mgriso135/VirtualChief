using System;
using System.Security.Claims;
using System.Web;
using KIS.App_Code;

namespace KIS
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Boolean FullyConfigured = false;
            int activeWorkspace_id = (new Dati.Dati()).getActiveWorkspaceId();
            String activeWorkspace = Session["ActiveWorkspace_Name"] != null ? Session["ActiveWorkspace_Name"].ToString(): "";

            if (activeWorkspace.Length == 0 || activeWorkspace_id == -1)
            {
                Response.Redirect("~/AccountsMgm/Account/Login");
            }
                KISConfig kCfg = new KISConfig(activeWorkspace, activeWorkspace_id);
                FullyConfigured =
                    kCfg.WizAndonCompleted &&
                    kCfg.WizCustomerReportCompleted &&
                    kCfg.WizLogoCompleted &&
                    kCfg.WizPostazioniCompleted &&
                    kCfg.WizRepartiCompleted &&
                    kCfg.WizTimezoneCompleted
                    //&& kCfg.WizUsersCompleted
                    ;

            if (!FullyConfigured)
            {
                Response.Redirect("~/Configuration/MainWizConfig.aspx");
            }
            else
            {
                Response.Redirect("~/HomePage/Default.aspx");
            }
        }
    }

}
