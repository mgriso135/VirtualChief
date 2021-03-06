using System;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS
{
    public partial class Default : System.Web.Mvc.ViewPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["ActiveWorkspace_Name"]!=null)
            { 
            KISConfig kCfg = new KISConfig(Session["ActiveWorkspace_Name"].ToString());
            Boolean FullyConfigured =
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
                lbl1.Text = ((UserAccount)Session["user"]).firstname + " " + ((UserAccount)Session["user"]).lastname +
                    "<br/>Last login: " ;
                lblBenvenuto.Visible = true;                
            }
            else
            {
                lblBenvenuto.Visible = false;
                lbl1.Text = GetLocalResourceObject("lbl1_NotLoggedIn.Text").ToString();
            }
        }
            else
            {
                Response.Redirect("~/AccountsMgm/Account/AfterLogin");
            }
        }
    }

}
