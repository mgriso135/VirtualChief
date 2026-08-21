using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace KisWebApp.Pages.HomePage
{
    public class DefaultModel : PageModel
    {
        public List<RecentActivity> RecentActivities { get; set; }

        public void OnGet()
        {
            // Initialize recent activities
            RecentActivities = new List<RecentActivity>
            {
                new RecentActivity { Date = DateTime.Now.AddDays(-1), Action = "Client Added", Details = "New client XYZ Corp" },
                new RecentActivity { Date = DateTime.Now.AddDays(-2), Action = "Work Order Created", Details = "Work order #12345" },
                new RecentActivity { Date = DateTime.Now.AddDays(-3), Action = "Report Generated", Details = "Customer portfolio report" }
            };
        }
    }

    public class RecentActivity
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
    }
}