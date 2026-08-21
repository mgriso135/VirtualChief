using KisApp.App_Code;
using KisApp.App_Sources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace KisWebApp.Pages.Analysis
{
    public class ListAnalysisOperatoriModel : PageModel
    {
        public List<User> Users { get; set; }

        public void OnGet()
        {
            // Check user permissions
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Operatori Tempi";
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
                UserList usrLst = new UserList(Session["ActiveWorkspace_Name"].ToString());
                Users = usrLst.listUsers.OrderBy(x => x.cognome).ThenBy(y => y.cognome).ToList();
            }
        }
    }

    public class User
    {
        public string cognome { get; set; }
        public string name { get; set; }
        public string username { get; set; }
    }
}