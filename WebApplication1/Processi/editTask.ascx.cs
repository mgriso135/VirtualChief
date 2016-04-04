using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
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
                    lbl1.Text = "Modifiche apportate correttamente.";
                }
                else
                {
                    lbl1.Text = "Errore: non sono riuscito a trovare il task di cui mi stai parlando.";
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
                    lbl1.Text = "Errore: non sono riuscito a trovare il task di cui mi stai parlando.";
                }

        }
    }
}