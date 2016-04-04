﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;

namespace KIS.Processi
{
    public partial class showPert : System.Web.UI.UserControl
    {
        //protected int contatore;
        public int procID;
        public int varID;

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
                if (procID != -1 && varID != -1)
                {
                    if (Page.IsPostBack)
                    {
                        String tskID = Request.Params.Get("__EVENTTARGET");
                        String evento = Request.Params.Get("__EVENTARGUMENT");
                        int taskID = -1;
                        int varianteID = -1;
                        String[] ids = tskID.Split(';');
                        try
                        {
                            taskID = Int32.Parse(ids[0]);
                            varianteID = Int32.Parse(ids[1]);
                        }
                        catch
                        {
                            taskID = -1;
                            varianteID = -1;
                            //lbl1.Text = "Errore nella conversione.<br />";
                        }


                        if (evento == "ShowDetails")
                        {
                            TaskVariante editTask = new TaskVariante(new processo(taskID), new variante(varianteID));
                            if (editTask.Task != null && editTask.Task.processID != -1 && editTask.variant != null && editTask.variant.idVariante != -1)
                            {
                                frmAddTempoCiclo.prc = editTask;
                                editTaskID.Value = editTask.Task.processID.ToString();
                                editTaskVarianteID.Value = editTask.variant.idVariante.ToString();
                                pnlEditTask.Visible = true;
                                editTaskID.Value = editTask.Task.processID.ToString();
                                editTaskNome.Text = Server.HtmlDecode(editTask.Task.processName);
                                editTaskDesc.Text = Server.HtmlDecode(editTask.Task.processDescription);
                                pnlEditTask.Style.Clear();
                                pnlEditTask.Style.Add("visibility", "visible");
                                pnlEditTask.Style.Add("position", "absolute");
                                pnlEditTask.Style.Add("top", svg1.Style["top"]);
                                pnlEditTask.Style.Add("width", "99%");
                                pnlEditTask.Style.Add("height", "1000px");
                                pnlEditTask.Style.Add("zIndex", "100");
                                pnlEditTask.Style.Add("text-align", "center");
                                pnlEditTask.Style.Add("vertical-align", "middle");
                                pnlEditTask.Style.Add("background", "rgba(0, 0, 0, 0.5)");
                                svg1.Style.Clear();
                                svg1.Style.Add("position", "absolute");
                                svg1.Style.Add("z-index", "-1");
                                svg1.Style.Add("width", "100%");
                                svg1.Style.Add("height", "1000px");

                                // Carico i tempi ciclo.
                                editTask.loadTempiCiclo();
                                rptTempi.DataSource = editTask.Tempi.Tempi;
                                rptTempi.DataBind();

                                editTaskSave.Focus();
                            }
                            else
                            {
                                lbl1.Text = "Sono stati passati alla funzione dei dati errati.<br />";
                            }
                        }
                    }
                    else
                    {
                        processo padre = new processo(procID);

                        int controllo = padre.checkConsistencyPERT(new variante(varID));

                        if (controllo == 0)
                        {
                            lbl1.Text = "<span style='color:red;'>GENERIC ERROR</span>";
                        }
                        else if (controllo == 2)
                        {
                            lbl1.Text = "<span style='color:red;'>ATTENZIONE: processo inconsistente. Esiste almeno un task che non ha né precedenti né successivi</span>";
                        }
                        else if (controllo == 5)
                        {
                            lbl1.Text = "<span style='color:red;'>ATTENZIONE: a qualche task manca il \"Tempo ciclo\"</span>";
                        }
                        lbl1.Text += "<br/>" + padre.log;
                        
                        //svg1.Text += "</svg>";

                        // Dropdown collega task esistente
                            ElencoTasks elTsk = new ElencoTasks();
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
                    lbl1.Text = "Errore: querystring non presente, errato o processo non trovato.<br/>";
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di gestire il PERT.<br/>";
                //imgAddTaskPert.Visible = false;
            }
        }

        protected void addTaskPert(object sender, EventArgs e)
        {
            procID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (!int.TryParse(Request.QueryString["id"], out procID))
                {
                    procID = -1;
                }
            }
            processo padre = new processo(procID);
            if (padre.processID != -1 && varID != -1)
            {
                int procCreated = padre.createDefaultSubProcess(new variante(varID));
                if (procCreated >= 0)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    //ErrorMessage
                }
            }

        }

        protected void editTaskSave_Click(object sender, ImageClickEventArgs e)
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
                    lblEdit1.Text = "Modifiche apportate correttamente.";
                }
                else
                {
                    lblEdit1.Text = "Errore: non sono riuscito a trovare il task di cui mi stai parlando.";
                }
            }
            else
            {
                lblEdit1.Text = "Errore nell'identificazione del task.";
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
                    lblEdit1.Text = "Errore: non sono riuscito a trovare il task di cui mi stai parlando.";
                }
            }
            else
            {
                lblEdit1.Text = "Errore nell'identificazione del task.";
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
                        lblEdit1.Text = "Tempo ciclo cancellato correttamente.";
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

        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            lbl1.Text = "";
            processo padre = new processo(procID);

            int controllo = padre.checkConsistencyPERT(new variante(varID));

            if (controllo == 0)
            {
                lbl1.Text = "<span style='color:red;'>GENERIC ERROR</span>";
            }
            else if (controllo == 2)
            {
                lbl1.Text = "<span style='color:red;'>ATTENZIONE: processo inconsistente. Esiste almeno un task che non ha né precedenti né successivi</span>";
            }
            else if (controllo == 5)
            {
                lbl1.Text = "<span style='color:red;'>ATTENZIONE: a qualche task manca il \"Tempo ciclo\"</span>";
            }
        }

        protected void btnLnkTask_Click(object sender, ImageClickEventArgs e)
        {
            //TaskVariante procVar = new ProcessoVariante(new processo(procID), new variante(varID));
            //lbl1.Text = "<br/><br/>" + procVar.Task.processID.ToString() + " " + procVar.variant.idVariante.ToString();

            //lbl1.Text += "<br/>" + ddlTasks.SelectedItem.Value.ToString() + " " + ddlTasks.SelectedItem.Text.ToString();
            //processo pr = new processo(procVar.Task.processID);
            processo pr = new processo(procID);
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
                bool rt = pr.linkProcessoVariante(new TaskVariante(new processo(tskID), new variante(varID)));
                if (rt == true)
                {
                    //Response.Redirect(Request.RawUrl);
                }
                else
                {
                    //lbl1.Text = procVar.Task.log;
                }
            }
            else
            {
                lbl1.Text = "Errore!";
            }
        }
    }
}