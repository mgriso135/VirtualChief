using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Commesse
{
    public partial class wzEditPERT_updtable1 : System.Web.UI.UserControl
    {
        public int procID;
        public int procRev;
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
                if (!Page.IsPostBack)
                {
                    // Inizializzo gli elementi della pagina
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
                    ddlTaskEsistenti.DataSource = curr;
                    ddlTaskEsistenti.DataTextField = "processName";
                    ddlTaskEsistenti.DataValueField = "processID";
                    ddlTaskEsistenti.DataBind();

                    loadTasks();
                }
            }
        }

        public void loadTasks()
        {
            processo padre = new processo(procID, procRev);


                    int controllo = padre.checkConsistencyPERT(new variante(varID));
                    lbl1.Text = "";
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


                    variante var = new variante(varID);
                    padre.loadFigli(new variante(varID));
                    rptTasks.DataSource = padre.subProcessi;
            
                    rptTasks.DataBind();
            
        }

        protected void rptTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                String sTask = e.CommandArgument.ToString();
                String[] task = sTask.Split(';');
                int taskID = -1;
                int taskRev = -1;

                try
                {
                    taskID = Int32.Parse(task[0]);
                    taskRev = Int32.Parse(task[1]);
                }
                catch
                {
                    taskID = -1;
                    taskRev = -1;
                }
                if (taskID != -1 && taskRev != -1)
                {
                    TaskVariante prc = new TaskVariante(new processo(taskID, taskRev), new variante(varID));
                    if (prc != null && prc.Task != null && prc.variant != null)
                    {
                             bool controllo = true;

                            // Controllo che non ci siano figli associati
                            if (controllo == true)
                            {
                                prc.Task.loadFigli();
                                if (prc.Task.subProcessi.Count == 0)
                                {
                                    controllo = true;
                                }
                                else
                                {
                                    controllo = false;
                                    lbl1.Text = "<span style='color: red'>Attenzione: ci sono dei sottoprocessi associati al task</span><br/>";
                                }
                            }

                            // Controllo che non ci siano varianti associate
                            if (controllo == true)
                            {
                                prc.Task.loadVariantiFigli();
                                if (prc.Task.variantiFigli.Count == 0)
                                {
                                    controllo = true;
                                }
                                else
                                {
                                    controllo = false;
                                    lbl1.Text = "<span style='color: red'>Attenzione: ci sono delle varianti da cancellare</span><br/>";
                                }
                            }

                            // Controllo che non ci siano tempi ciclo associati
                            if (controllo == true)
                            {
                                prc.loadTempiCiclo();
                                if (prc.Tempi.Tempi.Count > 0)
                                {
                                    controllo = false;
                                    lbl1.Text = "<span style='color: red'>Prima di procedere devi cancellare i tempi ciclo associati al task</span><br/>";
                                }
                                else
                                {
                                    controllo = true;
                                }
                            }

                            if (controllo == true)
                            {
                                prc.loadPostazioni();
                                for (int i = 0; i < prc.PostazioniDiLavoro.Count; i++)
                                {
                                    prc.deleteLinkPostazione(prc.PostazioniDiLavoro[i]);
                                }

                            }

                            // Se è tutto ok...
                            if (controllo == true)
                            {
                                bool rt = prc.Delete();
                                if (rt == true)
                                {
                                    processo prc2 = new processo(taskID, taskRev);
                                    int res = prc2.delete();
                                    lbl1.Text = "<br/><span style=\"color:red;\">Task cancellato... Attendi il refresh dei dati</span><br/>";
                                }
                                else
                                {
                                    lbl1.Text = "<br/><span style=\"color:red;\">" + prc.log + "</span><br/>";
                                }
                            }
                        }
                    }
                }
            }

        protected void rptTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
            if (tRow != null)
            {
                //HyperLink lnkAddTempoCiclo = (HyperLink)e.Item.FindControl("lnkAddTempoCiclo");
                //lnkAddTempoCiclo.NavigateUrl += varID.ToString();                

                //HyperLink lnkManagePrecedenti = (HyperLink)e.Item.FindControl("lnkManagePrecedenti");
                //lnkManagePrecedenti.NavigateUrl += varID.ToString();

                HyperLink lnkEditTask = (HyperLink)e.Item.FindControl("lnkEditTask");
                lnkEditTask.NavigateUrl += varID.ToString();

                Image imgTempoCicloOK = (Image)e.Item.FindControl("imgTempoCicloOK");
                Image imgTempoCicloKO = (Image)e.Item.FindControl("imgTempoCicloKO");

                Image imgPrecedentiOK = (Image)e.Item.FindControl("imgPrecedentiOK");
                Image imgPrecedentiKO = (Image)e.Item.FindControl("imgPrecedentiKO");

                HiddenField hTaskID = (HiddenField)e.Item.FindControl("hTaskID");
                HiddenField hRevTask = (HiddenField)e.Item.FindControl("hRevTask");

                Label lblPrecedenti = (Label)e.Item.FindControl("lblPrecedenti");
                Label lblTCDef = (Label)e.Item.FindControl("lblTCDef");

                int taskID, revTask;
                try
                {
                    taskID = Int32.Parse(hTaskID.Value);
                    revTask = Int32.Parse(hRevTask.Value);
                }
                catch
                {
                    taskID = -1;
                    revTask = -1;
                }

                TaskVariante task = new TaskVariante(new processo(taskID, revTask), new variante(varID));

                task.loadTempiCiclo();

                if (task.Tempi.Tempi.Count > 0)
                {
                    imgTempoCicloKO.Visible = false;
                    imgTempoCicloOK.Visible = true;
                    TimeSpan tDef = task.getDefaultTempo().Tempo;
                    int n_ops = task.getDefaultTempo().NumeroOperatori;
                    if (n_ops != -1)
                    {
                        lblTCDef.Text = "<br />Default: "
                            + Math.Truncate(tDef.TotalHours) + ":" + tDef.Minutes + ":" + tDef.Seconds
                            + " (" + n_ops.ToString() + " operator";
                        lblTCDef.Text += n_ops == 1 ? "e)" : "i)";
                    }
                    else
                    {
                        lblTCDef.Visible = false;
                    }
                }
                else
                {
                    imgTempoCicloKO.Visible = true;
                    imgTempoCicloOK.Visible = false;
                }

                task.Task.loadPrecedenti(task.variant);
                task.Task.loadSuccessivi(task.variant);

                lblPrecedenti.Visible = false;

                if (task.Task.processiPrec.Count > 0 || task.Task.processiSucc.Count > 0)
                {
                    imgPrecedentiKO.Visible = false;
                    imgPrecedentiOK.Visible = true;
                    if (task.Task.processiPrec.Count > 0)
                    {
                        lblPrecedenti.Visible = true;
                        //lblPrecedenti.Text += "<br />";
                        for (int i = 0; i < task.Task.processiPrec.Count; i++)
                        {
                            processo proc = new processo(task.Task.processiPrec[i], task.Task.revisionePrec[i]);
                            lblPrecedenti.Text += proc.processName
                                + " (" + Math.Truncate(task.Task.pausePrec[i].TotalHours)
                                + ":" + task.Task.pausePrec[i].Minutes.ToString()
                                + ":" + task.Task.pausePrec[i].Seconds.ToString()
                                /*+ "<a href=\"wzEditPauseTasks.aspx?prec="+task.Task.processiPrec[i]
                                +"&revPrec=" + task.Task.revisionePrec[i]
                                + "&succ=" + task.Task.processID
                                + "&revSucc=" + task.Task.revisione
                                + "&variante=" + task.variant.idVariante
                                + "\" target=\"_blank\">"*/
                                + "<img src=\"/img/iconWait.png\" width=\"20\" style=\"border: 1; cursor: pointer; cursor: hand;\" onclick=\"return pausaTasks(" 
                                + task.Task.processiPrec[i] + ", " 
                                + task.Task.revisionePrec[i] + ", " 
                                + task.Task.processID + ", "
                                + task.Task.revisione + ", "
                                + task.variant.idVariante
                                + ");\" />"
                                //+"</a>"
                                + ")" 
                                + "<br />";
                        }
                    }
                }
                else
                {
                    imgPrecedentiOK.Visible = false;
                    imgPrecedentiKO.Visible = true;
                }

                if (e.Item.ItemType == ListItemType.AlternatingItem)
                {

                }
                else
                {

                }
            }
           
        }

        protected void btnLnkTask_Click(object sender, ImageClickEventArgs e)
        {
            TaskVariante procVar = new TaskVariante(new processo(procID, procRev), new variante(varID));

            processo pr = new processo(procVar.Task.processID);
            int tskID = -1;
            try
            {
                tskID = Int32.Parse(ddlTaskEsistenti.SelectedItem.Value.ToString());
            }
            catch
            {
                tskID = -1;
            }

            if (tskID != -1)
            {
                bool rt = procVar.Task.linkProcessoVariante(new TaskVariante(new processo(tskID), procVar.variant));
                if (rt == true)
                {
                    loadTasks();
                }
                else
                {
                    lbl1.Text = "Errore. Verifica che il task non sia già stato aggiunto a questo processo produttivo.";
                }
            }
            else
            {
                lbl1.Text = "Errore!";
            }
        }

        protected void addTaskPert(object sender, EventArgs e)
        {

            processo padre = new processo(procID, procRev);
            if (padre.processID != -1 && varID != -1)
            {
                int procCreated = padre.createDefaultSubProcess(new variante(varID));
                if (procCreated >= 0)
                {
                    loadTasks();
                }
                else
                {
                    lbl1.Text = "Si è verificato un errore.";
                }
            }
            else
            {

            }

        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            loadTasks();
            lblLastUpdate.Text = "Last update: " + DateTime.Now.ToString("hh:mm:ss");
        }

    }
}