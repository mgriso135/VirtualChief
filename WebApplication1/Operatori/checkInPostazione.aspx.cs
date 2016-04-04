using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Operatori
{
    public partial class checkInPostazione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione check-in";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {   
            }
            else
            {
                frmPostazioniAttive.Visible = false;
                frmLoginPostazione.Visible = false;
                frmListTaskAvviatiUtenti.Visible = false;
                lbl1.Text = "Non sei loggato oppure non hai i permessi necessari. Please <a href=\"/Login/login.aspx\">log in</a>.";
            }
        }
    }
}