using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                rptListTasks.Visible = true;
                ElencoTasks elTsk = new ElencoTasks(true);
                var elTskProd = from q in elTsk.Elenco
                                        where q.processoPadre != -1 && q.revPadre != -1
                                        select q;
                rptListTasks.DataSource = elTskProd;
                rptListTasks.DataBind();
            }
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare l'analisi dati dei tasks";
            }
        }
    }
}