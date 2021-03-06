using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Processi
{
    public partial class editTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int taskID = -1, revTaskID = -1, varianteID = -1;
            String sTaskID, sRevTaskID, sVarianteID;
            sTaskID = Request.QueryString["taskID"];
            sRevTaskID = Request.QueryString["revTaskID"];
            sVarianteID = Request.QueryString["varianteID"];
            frmEditTask.Visible = false;
            lblTitle.Visible = false;

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
                    TaskVariante tsk = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), taskID, revTaskID), 
                        new variante(Session["ActiveWorkspace_Name"].ToString(), varianteID));
                    if (tsk != null && tsk.Task != null && tsk.variant != null && tsk.Task.processID != -1 && tsk.variant.idVariante != -1)
                    {
                        frmEditTask.Visible = true;
                        frmEditTask.task = tsk;
                        lblTitle.Visible = true;
                        lblTitle.Text = "<h3>" + tsk.Task.processName + "</h3>";
                    }
                }

            }
        }
    }
}