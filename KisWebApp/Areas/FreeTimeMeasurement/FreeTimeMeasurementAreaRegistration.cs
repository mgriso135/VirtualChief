using System.Web.Mvc;

namespace KIS.Areas.FreeTimeMeasurement
{
    public class FreeTimeMeasurementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FreeTimeMeasurement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FreeTimeMeasurement_default",
                "FreeTimeMeasurement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}