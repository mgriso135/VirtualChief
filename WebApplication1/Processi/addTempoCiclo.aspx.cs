using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Processi
{
    public partial class addTempoCiclo1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int taskID = -1, revTaskID = -1, varianteID = -1;
            String sTaskID, sRevTaskID, sVarianteID;
            sTaskID = Request.QueryString["taskID"];
            sRevTaskID = Request.QueryString["revTaskID"];
            sVarianteID = Request.QueryString["varianteID"];
            frmAddTempoCiclo.Visible = false;
            lblTitle.Visible = false;
            frmStoricoTempi.Visible = false;
            frmStoricoTempiProdotto.Visible = false;

            if (!String.IsNullOrEmpty(sTaskID) && !String.IsNullOrEmpty(sRevTaskID) && !String.IsNullOrEmpty(sVarianteID))
            {
                try
                {
                    taskID = Int32.Parse(sTaskID);
                    revTaskID = Int32.Parse(sRevTaskID);
                    varianteID = Int32.Parse(sVarianteID);
                }
                catch
                {
                    taskID = -1;
                    revTaskID = -1;
                    varianteID = -1;
                }

                if (taskID != -1 && revTaskID != -1 && varianteID != -1)
                {
                    
                    TaskVariante tsk = new TaskVariante(new processo(taskID, revTaskID), new variante(varianteID));
                    if (tsk != null && tsk.Task!=null && tsk.variant!=null && tsk.Task.processID != -1 && tsk.variant.idVariante != -1)
                    {
                        frmAddTempoCiclo.Visible = true;
                        frmAddTempoCiclo.prc = tsk;
                        frmListTempiCiclo.task = tsk;
                        lblTitle.Visible = true;
                        lblTitle.Text = "<h3>"+tsk.Task.processName+"</h3>";

                        frmStoricoTempi.Visible = true;
                        frmStoricoTempi.processID = tsk.Task.processID;
                        frmStoricoTempi.revisione = tsk.Task.revisione;

                        frmStoricoTempiProdotto.Visible = true;
                        tsk.Task.loadPadre(tsk.variant);
                        frmStoricoTempiProdotto.taskID = tsk.Task.processID;
                        frmStoricoTempiProdotto.revTask = tsk.Task.revisione;
                        frmStoricoTempiProdotto.prodID = tsk.Task.processoPadre;
                        frmStoricoTempiProdotto.prodRev = tsk.Task.revPadre;
                        frmStoricoTempiProdotto.varianteID = tsk.variant.idVariante;
                    }
                }

            }

        }
    }
}