using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzEditPERT1 : System.Web.UI.UserControl
    {
        public int procID;
        public int procRev;
        public int varID;
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessID.Value = procID.ToString();
            ProcessRev.Value = procRev.ToString();
            VariantID.Value = varID.ToString();

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
                if (procID != -1 && varID != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        ProductParametersCategories parList = new ProductParametersCategories(Session["ActiveWorkspace"].ToString());
                        parList.loadCategories();
                        ddlParamCategory.AppendDataBoundItems = true;
                        ddlParamCategory.Items.Clear();
                        ddlParamCategory.Items.Add(new ListItem("", "-1"));
                        for (int i = 0; i < parList.Categories.Count; i++)
                        {
                            ddlParamCategory.Items.Add(new ListItem(
                                Server.HtmlDecode(Server.HtmlDecode(parList.Categories[i].Name)),
                                parList.Categories[i].ID.ToString()));
                        }
                        ddlParamCategory.DataBind();

                        processo padre = new processo(Session["ActiveWorkspace"].ToString(), procID);

                        int controllo = padre.checkConsistencyPERT(new variante(Session["ActiveWorkspace"].ToString(), varID));

                        if (controllo == 0)
                        {
                            lbl1.Text = "<span style='color:red;'>"
                                + GetLocalResourceObject("lblGenericError").ToString() +"</span>";
                        }
                        else if (controllo == 2)
                        {
                            lbl1.Text = "<span style='color:red;'>"+
                                GetLocalResourceObject("lblErrorProcNotConsistent").ToString()
                                + "</span>";
                        }
                        else if (controllo == 5)
                        {
                            lbl1.Text = "<span style='color:red;'>"+
                                GetLocalResourceObject("lblErrorTCMissing").ToString()
                                + "</span>";
                        }
                        lbl1.Text += "<br/>" + padre.log;

                        //svg1.Text += "</svg>";

                        // Dropdown collega task esistente
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
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorQueryString").ToString()+".<br/>";
                }
            }
            else
            {
                svg1.Visible = false;
                tblAddTask.Visible = false;
                //pnlEditTask.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString()+".<br/>";
                
            }
        }

        /*protected void editTaskSave_Click(object sender, ImageClickEventArgs e)
        {
            int taskID = -1;
            int varianteID = -1;
            try
            {
                taskID = Int32.Parse(editTaskID.Value);
                varianteID = Int32.Parse(editTaskVarianteID.Value);
            }
            catch
            {
                taskID = -1;
                varianteID = -1;
            }

            if (taskID != -1 && varianteID != -1)
            {
                TaskVariante tsk = new TaskVariante(new processo(taskID), new variante(varianteID));
                if (tsk != null && tsk.Task != null && tsk.Task.processID != -1 && tsk.variant != null && tsk.variant.idVariante != -1)
                {
                    tsk.Task.processName = Server.HtmlEncode(editTaskNome.Text);
                    tsk.Task.processDescription = Server.HtmlEncode(editTaskDesc.Text);
                    lblEdit1.Text = GetLocalResourceObject("lblModificheOk").ToString() + ".";
                }
                else
                {
                    lblEdit1.Text = GetLocalResourceObject("lblErrorTaskNotFound").ToString();
                }
            }
            else
            {
                lblEdit1.Text = GetLocalResourceObject("lblErrorTaskNotFound").ToString();
            }
            editTaskSave.Focus();
        }

        protected void editTaskUndo_Click(object sender, ImageClickEventArgs e)
        {
            int taskID = -1;
            int varianteID = -1;
            try
            {
                taskID = Int32.Parse(editTaskID.Value);
                varianteID = Int32.Parse(editTaskVarianteID.Value);
            }
            catch
            {
                taskID = -1;
                varianteID = -1;
            }

            if (taskID != -1 && varianteID != -1)
            {
                TaskVariante tsk = new TaskVariante(new processo(taskID), new variante(varianteID));
                if (tsk != null && tsk.Task != null && tsk.Task.processID != -1 && tsk.variant != null && tsk.variant.idVariante != -1)
                {
                    editTaskNome.Text = tsk.Task.processName;
                    editTaskDesc.Text = tsk.Task.processDescription;
                }
                else
                {
                    lblEdit1.Text = GetLocalResourceObject("lblErrorTaskNotFound").ToString();
                }
            }
            else
            {
                lblEdit1.Text = GetLocalResourceObject("lblErrorTaskNotFound").ToString();
            }
            editTaskSave.Focus();
        }

        protected void rptTempi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int taskID = -1;
                int varianteID = -1;

                int num_ops = -1;
                try
                {
                    num_ops = Int32.Parse(e.CommandArgument.ToString());
                    taskID = Int32.Parse(editTaskID.Value);
                    varianteID = Int32.Parse(editTaskVarianteID.Value);
                }
                catch
                {
                    num_ops = -1;
                    taskID = -1;
                    varianteID = -1;
                }
                TaskVariante prc = new TaskVariante(new processo(taskID), new variante(varianteID));
                if (num_ops != -1 && prc != null && prc.Task != null && prc.variant != null)
                {
                    TempoCiclo tc = new TempoCiclo(prc.Task.processID, prc.Task.revisione, prc.variant.idVariante, num_ops);
                    bool rt = tc.Delete();
                    if (rt == true)
                    {
                        lblEdit1.Text = GetLocalResourceObject("lblTCDeleted").ToString();
                        prc.loadTempiCiclo();
                        rptTempi.DataSource = prc.Tempi.Tempi;
                        rptTempi.DataBind();
                    }
                    else
                    {
                        lbl1.Text = tc.log;
                    }
                }
            }
            else if (e.CommandName == "MakeDefault")
            {
                int taskID = -1;
                int varianteID = -1;
                int num_ops = -1;
                try
                {
                    num_ops = Int32.Parse(e.CommandArgument.ToString());
                    taskID = Int32.Parse(editTaskID.Value);
                    varianteID = Int32.Parse(editTaskVarianteID.Value);
                }
                catch
                {
                    num_ops = -1;
                    taskID = -1;
                    varianteID = -1;
                }
                TaskVariante task = new TaskVariante(new processo(taskID), new variante(varianteID));
                if (num_ops != -1 && task != null && task.Task != null && task.variant != null)
                {
                    TempoCiclo tc = new TempoCiclo(task.Task.processID, task.Task.revisione, task.variant.idVariante, num_ops);
                    TaskVariante tskVar = new TaskVariante(new processo(task.Task.processID, task.Task.revisione), new variante(task.variant.idVariante));
                    tskVar.loadTempiCiclo();
                    for (int i = 0; i < tskVar.Tempi.Tempi.Count; i++)
                    {
                        tskVar.Tempi.Tempi[i].Default = false;
                    }
                    tc.Default = true;
                    task.loadTempiCiclo();
                    rptTempi.DataSource = task.Tempi.Tempi;
                    rptTempi.DataBind();
                }
            }
        }
        */
        protected void timer1_Tick(object sender, EventArgs e)
        {
            lbl1.Text = "";
            processo padre = new processo(Session["ActiveWorkspace"].ToString(), procID);

            int controllo = padre.checkConsistencyPERT(new variante(Session["ActiveWorkspace"].ToString(), varID));

            if (controllo == 0)
            {
                lbl1.Text = "<span style='color:red;'>"
                    + GetLocalResourceObject("lblGenericError").ToString() + "</span>";
            }
            else if (controllo == 2)
            {
                lbl1.Text = "<span style='color:red;'>" +
                    GetLocalResourceObject("lblErrorProcNotConsistent").ToString()
                    + "</span>";
            }
            else if (controllo == 5)
            {
                lbl1.Text = "<span style='color:red;'>" +
                    GetLocalResourceObject("lblErrorTCMissing").ToString()
                    + "</span>";
            }

        }

        /*protected void rptTempi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgDef = (Image)e.Item.FindControl("imgIsDefault");
                ImageButton imgMakeDefault = (ImageButton)e.Item.FindControl("imgMakeDefault");
                HiddenField hNumOps = (HiddenField)e.Item.FindControl("hNumOp");

                int taskID = -1;
                int varianteID = -1;
                int num_ops = -1;
                try
                {
                    num_ops = Int32.Parse(hNumOps.Value);
                    taskID = Int32.Parse(editTaskID.Value);
                    varianteID = Int32.Parse(editTaskVarianteID.Value);
                }
                catch
                {
                    num_ops = -1;
                    taskID = -1;
                    varianteID = -1;
                }
                TaskVariante task = new TaskVariante(new processo(taskID), new variante(varianteID));

                if (num_ops != -1 && task != null && task.Task != null && task.variant != null)
                {
                    TempoCiclo tc = new TempoCiclo(task.Task.processID, task.Task.revisione, task.variant.idVariante, num_ops);
                    imgDef.Visible = tc.Default;
                    imgMakeDefault.Visible = !tc.Default;
                }
            }
        }*/
    }
}