using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Processi
{
    public partial class listTempiCiclo1 : System.Web.UI.UserControl
    {
        public TaskVariante task;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                //TaskVariante editTask = new TaskVariante(new processo(taskID, revTaskID), new variante(varianteID));
                if (task.Task != null && task.Task.processID != -1 && task.variant != null && task.variant.idVariante != -1)
                {
                    // Carico i tempi ciclo.
                    task.loadTempiCiclo();
                    rptTempi.DataSource = task.Tempi.Tempi;
                    rptTempi.DataBind();
                }
            }
        }

        protected void rptTempi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int num_ops = -1;
                try
                {
                    num_ops = Int32.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    num_ops = -1;
                }

                if (num_ops != -1 && task != null && task.Task != null && task.variant != null)
                {
                    TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), task.Task.processID, task.Task.revisione, task.variant.idVariante, num_ops);
                    bool rt = tc.Delete();
                    if (rt == true)
                    {
                        lbl1.Text = GetLocalResourceObject("lblDeleteTCOK").ToString();
                        task.loadTempiCiclo();
                        rptTempi.DataSource = task.Tempi.Tempi;
                        rptTempi.DataBind();
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblError").ToString() + "<br />" + tc.log;
                    }
                }
            }
            else if (e.CommandName == "MakeDefault")
            {
                int num_ops = -1;
                try
                {
                    num_ops = Int32.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    num_ops = -1;
                }

                if (num_ops != -1 && task != null && task.Task != null && task.variant != null)
                {
                    TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), task.Task.processID, task.Task.revisione, task.variant.idVariante, num_ops);
                    TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), task.Task.processID, task.Task.revisione), new variante(Session["ActiveWorkspace_Name"].ToString(), task.variant.idVariante));
                    tskVar.loadTempiCiclo();
                    for (int i = 0; i < tskVar.Tempi.Tempi.Count; i++)
                    {
                        tskVar.Tempi.Tempi[i].Default = false;
                    }
                    tc.Default = true;

                    tskVar.loadTempiCiclo();
                    rptTempi.DataSource = task.Tempi.Tempi;
                    rptTempi.DataBind();
                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            task.loadTempiCiclo();
            rptTempi.DataSource = task.Tempi.Tempi;
            rptTempi.DataBind();
            lbl1.Text = "Last update: " + DateTime.Now.ToString("HH:mm:ss");
        }

        protected void rptTempi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgDef = (Image)e.Item.FindControl("imgIsDefault");
                ImageButton imgMakeDefault = (ImageButton)e.Item.FindControl("imgMakeDefault");
                HiddenField hNumOps = (HiddenField)e.Item.FindControl("hNumOp");
                int num_ops = -1;

                try
                {
                    num_ops = Int32.Parse(hNumOps.Value);
                }
                catch
                {
                    num_ops = -1;
                }

                if (num_ops != -1 && task != null && task.Task != null && task.variant != null)
                {
                    TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), task.Task.processID, task.Task.revisione, task.variant.idVariante, num_ops);
                    imgDef.Visible = tc.Default;
                    imgMakeDefault.Visible = !tc.Default;
                }
            }
        }
    }
}