using System.Web.Mvc;

namespace KIS.Areas.AccountsMgm
{
    public class AccountsMgmAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AccountsMgm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AccountsMgm_default",
                "AccountsMgm/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}