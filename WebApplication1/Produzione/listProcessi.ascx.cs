using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Produzione
{
    public partial class listProcessi : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ElencoMacroProcessiVarianti elMacroPrVar = new ElencoMacroProcessiVarianti();
                rptProcessi.DataSource = elMacroPrVar.elenco;
                rptProcessi.DataBind();
            }
        }

        protected void rptProcessi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lbl1.Text = e.CommandName;
            String[] strPrcVar = e.CommandArgument.ToString().Split('+');
            lbl1.Text += "<br/>" + strPrcVar[0] + " " + strPrcVar[1];
            int proc, var;
            try
            {
                proc = Int32.Parse(strPrcVar[0]);
                var = Int32.Parse(strPrcVar[1]);
            }
            catch
            {
                proc = -1;
                var = -1;
            }
            if (proc != -1 && var != -1)
            {
                if (e.CommandName.ToString() == "expand")
                {
                    lbl1.Text = e.CommandArgument.ToString();
                    ProcessoVariante prc = new ProcessoVariante(new processo(proc), new variante(var));
                    ElencoProcessiVarianti elSubProc = new ElencoProcessiVarianti(prc);
                    rptProcessi.DataSource = elSubProc.elencoFigli;
                    rptProcessi.DataBind();
                }
                else if (e.CommandName.ToString() == "setup")
                {
                   /* rptProcessi.Visible = false;
                    rptConfigProcess.Visible = true;
                    processo prc = new processo(proc);
                    prc.loadFigli(new variante(var));
                    List<TaskVariante> lstFigli = new List<TaskVariante>();
                    for (int i = 0; i < prc.subProcessi.Count; i++)
                    {
                        lstFigli.Add(new TaskVariante(prc.subProcessi[i], new variante(var)));
                    }
                    rptConfigProcess.DataSource = lstFigli;
                    rptConfigProcess.DataBind();*/
                }
            }
        }
    }
}