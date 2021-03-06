using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Analysis
{
    public partial class DetailAnalysisOperatore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkNavCurr.Visible = false;

            String usrID1 = Request.QueryString["usr"];
            String usrID = Server.HtmlEncode(usrID1);
            if (!String.IsNullOrEmpty(usrID))
            {
                User usr = new User(usrID);
                if (!String.IsNullOrEmpty(usr.username) && usr.username.Length > 0)
                {
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
                        lbl1.Text = usr.username;
                        lnkNavCurr.Visible = true;
                        lnkNavCurr.NavigateUrl = Request.RawUrl;
                        frmAnalisiOperatore.usrID = usr.username;
                        frmAnalisiOperatore.Visible = true;                        
                    }
                    else
                    {
                    }
                }

            }
        }

}
}