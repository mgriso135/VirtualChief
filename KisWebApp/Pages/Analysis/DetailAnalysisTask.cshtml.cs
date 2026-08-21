using KisApp.App_Code;
using KisApp.App_Sources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace KisWebApp.Pages.Analysis
{
    public class DetailAnalysisTaskModel : PageModel
    {
        public void OnGet()
        {
            // Check user permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analysis";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }
        }
    }
}