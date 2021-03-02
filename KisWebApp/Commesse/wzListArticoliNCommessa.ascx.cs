using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Commesse
{
    public partial class wzListArticoliNCommessa : System.Web.UI.UserControl
    {
        public int idCommessa;
        public int annoCommessa;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (idCommessa != -1 && annoCommessa != -1)
                {
                    Commessa cm = new Commessa(Session["ActiveWorkspace"].ToString(), idCommessa, annoCommessa);
                    if (cm.ID != -1 && cm.Year != -1)
                    {
                        cm.loadArticoli();
                        var lstN = from arts in cm.Articoli
                                   where arts.Status == 'N'
                                   select arts;
                        rptArticoliStatoN.DataSource = lstN;
                        rptArticoliStatoN.DataBind();
                    }
                }
            }
        }
    }
}