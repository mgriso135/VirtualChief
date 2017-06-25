using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class viewDetailsTaskProduzione : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int taskID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    taskID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    taskID = -1;
                }
                if (taskID != -1)
                {
                    TaskProduzione tsk = new TaskProduzione(taskID);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        frmViewDetails.taskID = tsk.TaskProduzioneID;
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblTaskNotFound").ToString();
                        frmViewDetails.Visible = false;
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorID").ToString();
                    frmViewDetails.Visible = false;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorID").ToString(); 
                frmViewDetails.Visible = false;
            }
        }
    }
}