using System.Web.Mvc;

namespace KIS.Areas.WorkInstructions
{
    public class WorkInstructionsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WorkInstructions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WorkInstructions_default",
                "WorkInstructions/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}