using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Produzione
{
    public partial class HomeBoxLastProgrammedProducts : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                User usr = (User)Session["user"];
                usr.loadNextProgrammedProducts();
                rptNextProgrammedProducts.DataSource = usr.NextProgrammedProducts.Take(5);
                rptNextProgrammedProducts.DataBind();
            }
            else
            {
                rptNextProgrammedProducts.Visible = false;
            }

        }
    }
}