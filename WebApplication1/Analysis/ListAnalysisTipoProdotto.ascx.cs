using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Analysis
{
    public partial class ListAnalysisTipoProdotto1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptTipiProdotto.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi TipoProdotto";
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
                rptTipiProdotto.Visible = true;
                ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                List<ProcessoVariante> elProcVar = el.elencoFigli.OrderBy(x => x.NomeCombinato).ToList();
                rptTipiProdotto.DataSource = elProcVar;
                rptTipiProdotto.DataBind();
            }
            else
            {
            }
        }
    }
}