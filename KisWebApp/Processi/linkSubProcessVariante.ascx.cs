using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Processi
{
    public partial class linkSubProcessVariante : System.Web.UI.UserControl
    {
        public TaskVariante procVar;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (procVar!=null && procVar.Task != null && procVar.variant != null)
                {
                    ElencoTasks elTsk = new ElencoTasks(Session["ActiveWorkspace"].ToString());
                    List<processo> curr = new List<processo>();
                    for (int i = 0; i < elTsk.Elenco.Count; i++)
                    {
                        elTsk.Elenco[i].loadPadre();
                        if (elTsk.Elenco[i].processoPadre != -1 && elTsk.Elenco[i].revPadre != -1)
                        {
                            curr.Add(elTsk.Elenco[i]);
                        }
                    }
                    ddlTasks.DataSource = curr;
                    ddlTasks.DataTextField = "processName";
                    ddlTasks.DataValueField = "processID";
                    ddlTasks.DataBind();
                }
            }
        }

        protected void btnLnkTask_Click(object sender, ImageClickEventArgs e)
        {
            lbl1.Text = "<br/><br/>"+procVar.Task.processID.ToString() + " " + procVar.variant.idVariante.ToString();

            lbl1.Text += "<br/>" + ddlTasks.SelectedItem.Value.ToString() + " " + ddlTasks.SelectedItem.Text.ToString();
            processo pr = new processo(Session["ActiveWorkspace"].ToString(), procVar.Task.processID);
            int tskID = -1;
            try
            {
                tskID = Int32.Parse(ddlTasks.SelectedItem.Value.ToString());
            }
            catch
            {
                tskID = -1;
            }

            if (tskID != -1)
            {
                bool rt = procVar.Task.linkProcessoVariante(new TaskVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), tskID), procVar.variant));
                if (rt == true)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl1.Text = procVar.Task.log;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblError").ToString();
            }
        }
    }
}