using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzEditPauseTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int taskID = -1, revTaskID = -1, varianteID = -1, prec = -1, revPrec = -1;
            String sTaskID, sRevTaskID, sVarianteID, sPrec, sRevPrec;
            sTaskID = Request.QueryString["succ"];
            sRevTaskID = Request.QueryString["revSucc"];
            sPrec = Request.QueryString["prec"];
            sRevPrec = Request.QueryString["revPrec"];
            sVarianteID = Request.QueryString["variante"];
            frmPause.Visible = false;

            if (!String.IsNullOrEmpty(sTaskID) && !String.IsNullOrEmpty(sRevTaskID) && !String.IsNullOrEmpty(sVarianteID))
            {
                try
                {
                    taskID = Int32.Parse(sTaskID);
                    revTaskID = Int32.Parse(sRevTaskID);
                    varianteID = Int32.Parse(sVarianteID);
                    prec = Int32.Parse(sPrec);
                    revPrec = Int32.Parse(sRevPrec);
                }
                catch
                {
                    taskID = -1;
                    revTaskID = -1;
                    varianteID = -1;
                    prec = -1;
                    revPrec = -1;
                }

                if (taskID != -1 && revTaskID != -1 && varianteID != -1 && prec != -1 && revPrec != -1)
                {                   

                    variante vari = new variante(varianteID);
                    TaskVariante tsk = new TaskVariante(new processo(taskID, revTaskID), vari);
                    if (tsk != null && tsk.Task != null && tsk.variant != null && tsk.Task.processID != -1 && tsk.variant.idVariante != -1)
                    {
                        frmPause.Visible = true;
                        frmPause.task = tsk.Task.processID;
                        frmPause.revTask = tsk.Task.revisione;
                        frmPause.prec = prec;
                        frmPause.revPrec = revPrec;
                        frmPause.variante = tsk.variant.idVariante;
                    }
                }
            }
        }
    }
}