using System.Web.Mvc;

namespace KIS.Areas.SalesOrders
{
    public class SalesOrdersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SalesOrders";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SalesOrders_default",
                "SalesOrders/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}