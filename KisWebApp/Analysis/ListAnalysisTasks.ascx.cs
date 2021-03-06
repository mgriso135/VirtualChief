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
    public partial class ListAnalysisTasks1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptListTasks.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Tasks";
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
                rptListTasks.Visible = true;
                ElencoTasks elTsk = new ElencoTasks(Session["ActiveWorkspace_Name"].ToString(), true);
                var elTskProd = from q in elTsk.Elenco
                                        where q.processoPadre != -1 && q.revPadre != -1
                                        select q;
                rptListTasks.DataSource = elTskProd;
                rptListTasks.DataBind();
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }
    }
}