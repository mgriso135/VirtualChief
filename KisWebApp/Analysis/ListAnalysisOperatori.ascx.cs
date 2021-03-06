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
    public partial class ListAnalysisOperatori1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptOperatori.Visible = false;
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
                rptOperatori.Visible = true;
                UserList usrLst = new UserList(Session["ActiveWorkspace_Name"].ToString());
                List<User> lista = usrLst.listUsers.OrderBy(x => x.cognome).ThenBy(y=>y.cognome).ToList();
                rptOperatori.DataSource = lista;
                rptOperatori.DataBind();
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }
    }
}