using System;
using KIS.App_Code;

namespace KIS
{
    public partial class Default : System.Web.Mvc.ViewPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            KISConfig kCfg = new KISConfig(Session["ActiveWorkspace"].ToString());
            Boolean FullyConfigured =
                kCfg.WizAdminUserCompleted &&
                kCfg.WizAndonCompleted &&
                kCfg.WizCustomerReportCompleted &&
                kCfg.WizLogoCompleted &&
                kCfg.WizPostazioniCompleted &&
                kCfg.WizRepartiCompleted &&
                kCfg.WizTimezoneCompleted &&
                kCfg.WizUsersCompleted;

            if (!FullyConfigured)
            {
                Response.Redirect("~/Configuration/MainWizConfig.aspx");
            }

            lblBenvenuto.Visible = false;

            if (Session["user"] != null)
            {
                lbl1.Text = ((User)Session["user"]).name + " " + ((User)Session["user"]).cognome +
                    "<br/>Last login: " + ((User)Session["user"]).lastLogin.ToString();
                lblBenvenuto.Visible = true;                
            }
            else
            {
                lblBenvenuto.Visible = false;
                lbl1.Text = GetLocalResourceObject("lbl1_NotLoggedIn.Text").ToString();
            }
        }
    }

}
