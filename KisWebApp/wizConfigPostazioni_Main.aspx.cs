using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                frmAddPostazione.Visible = true;
                frmListPostazioni.Visible = true;
            }
            else
            {
                lbl1.Text = "<a href=\"~/Login/login.aspx"
                    + "?red=/Configuration/wizConfigPostazioni_Main.aspx\">"
                    + GetLocalResourceObject("lblLnkLoginAdmin").ToString()
                    +"</a>";
                frmAddPostazione.Visible = false;
                frmListPostazioni.Visible = false;
            }
        }
    }
}