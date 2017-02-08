using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Configuration
{
    public partial class wizConfigPostazioni_Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmAddPostazione.Visible = false;
            frmListPostazioni.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                frmAddPostazione.Visible = true;
                frmListPostazioni.Visible = true;
            }
            else
            {
                lbl1.Text = "Please <a href=\"/Login/login.aspx"
                    + "?red=/Configuration/wizConfigPostazioni_Main\">click here</a> to login as Admin User.";
                frmAddPostazione.Visible = false;
                frmListPostazioni.Visible = false;
            }
        }
    }
}