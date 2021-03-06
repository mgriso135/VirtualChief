using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Processi
{
    public partial class editTask1 : System.Web.UI.UserControl
    {
        public TaskVariante task;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    if (task!=null && task.Task != null && task.Task.processID != -1 && task.variant != null && task.variant.idVariante != -1)
                    {
                        editTaskNome.Text = task.Task.processName;
                        editTaskDesc.Text = task.Task.processDescription;
                    }
                }
            }
        }

        protected void editTaskSave_Click(object sender, ImageClickEventArgs e)
        {
                if (task != null && task.Task != null && task.Task.processID != -1 && task.variant != null && task.variant.idVariante != -1)
                {
                    task.Task.processName = Server.HtmlEncode(editTaskNome.Text);
                    task.Task.processDescription = Server.HtmlEncode(editTaskDesc.Text);
                lbl1.Text = GetLocalResourceObject("lblModificheOk").ToString();
                }
                else
                {
                lbl1.Text = GetLocalResourceObject("lblTaskNotFound").ToString();
                }
        }

        protected void editTaskUndo_Click(object sender, ImageClickEventArgs e)
        {
            
                if (task != null && task.Task != null && task.Task.processID != -1 && task.variant != null && task.variant.idVariante != -1)
                {
                    editTaskNome.Text = task.Task.processName;
                    editTaskDesc.Text = task.Task.processDescription;
                }
                else
                {
                lbl1.Text = GetLocalResourceObject("lblTaskNotFound").ToString();
            }

        }
    }
}