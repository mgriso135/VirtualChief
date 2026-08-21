using KisApp.App_Code;
using KisApp.App_Sources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace KisWebApp.Pages.Produzione
{
    public class avancamentoProduzioneModel : PageModel
    {
        public List<ProductionProgress> ProductionProgress { get; set; }

        public void OnGet()
        {
            // Check user permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Produzione";
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
                // Load production progress data
                ProductionProgress = new List<ProductionProgress>
                {
                    new ProductionProgress { WorkOrderId = 1, Progress = "50%", Status = "In Progress" },
                    new ProductionProgress { WorkOrderId = 2, Progress = "100%", Status = "Completed" }
                };
            }
        }
    }

    public class ProductionProgress
    {
        public int WorkOrderId { get; set; }
        public string Progress { get; set; }
        public string Status { get; set; }
    }
}