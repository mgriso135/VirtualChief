using KisApp.App_Code;
using KisApp.App_Sources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace KisWebApp.Pages.Analysis
{
    public class CustomerPortfolioModel : PageModel
    {
        public List<Customer> Customers { get; set; }

        public void OnGet()
        {
            // Check user permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Customer Portfolio";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                // Load customers from database via App_Sources
                // Using Analysis class from App_Sources
                Analysis analysis = new Analysis(Session["ActiveWorkspace_Name"].ToString());
                Customers = analysis.GetCustomers(); // Assuming this method exists
            }
        }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Revenue { get; set; }
        public string OrderHistory { get; set; }
    }
}