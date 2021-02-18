using System.Web.Mvc;

namespace KIS.Areas.ProcessMining
{
    public class ProcessMiningAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ProcessMining";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ProcessMining_default",
                "ProcessMining/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}