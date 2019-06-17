using System;
using KIS.App_Code;

namespace KIS
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLicenseExpired.Visible = false;
            Logo lg = new Logo();
            kisLogo.ImageUrl = lg.filePath;
            kisLogo.Height = 50;

            KISConfig kisCfg = new KISConfig();
            DateTime licenseExpDate = kisCfg.ExpiryDate;
            if(licenseExpDate.AddDays(7) <= DateTime.UtcNow)
            {
                Response.Redirect("~/LicenseExpired.aspx");
            }
            else if (licenseExpDate <= DateTime.UtcNow)
            {
                lblLicenseExpired.Visible = true;
                TimeSpan diffDate = (DateTime.UtcNow - licenseExpDate);
                lblLicenseExpired.Text = "<br />" + GetLocalResourceObject("lblLicense3").ToString()
                    + " " + Math.Round(diffDate.TotalDays, 0) + " "
                    + GetLocalResourceObject("lblLicense2").ToString();
            }
            else if(DateTime.UtcNow.AddDays(30) >= licenseExpDate && DateTime.UtcNow <= licenseExpDate)
            {
                lblLicenseExpired.Visible = true;
                TimeSpan diffDate = (licenseExpDate - DateTime.UtcNow);
                lblLicenseExpired.Text ="<br />" + GetLocalResourceObject("lblLicense1").ToString()
                    + " " + Math.Round(diffDate.TotalDays, 0) + " "
                    + GetLocalResourceObject("lblLicense2").ToString();
            }

            // Register user action
            //String ipAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            /*if(String.IsNullOrEmpty(ipAddr))
            {
                ipAddr = Request.ServerVariables["REMOTE_ADDR"];
            }*/
            String ipAddr = Request.UserHostAddress;
            String page = Request.RawUrl;
            try
            {
                var pageS = Request.RawUrl.Split('?');
                page = pageS[0];
            }
            catch { page = Request.RawUrl; }
                

            if (Session["user"]!=null)
            {
                User curr = (User)Session["user"];
                
                Dati.Utilities.LogAction(curr.username, "Page", page, Request.QueryString.ToString(), ipAddr);
            }
            else
            {
                Dati.Utilities.LogAction(Session.SessionID, "Page", page, Request.QueryString.ToString(), ipAddr);
            }
        }
    }
}
