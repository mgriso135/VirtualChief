using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzEditPauseTasks1 : System.Web.UI.UserControl
    {
        public int task, revTask, prec, revPrec, variante;

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            lblInfo.Text = "Saving...";
            if (task != -1 && revTask != -1 && variante != -1 && prec != -1 && revPrec != -1)
            {
                variante vari = new variante(variante);
                TaskVariante tsk = new TaskVariante(new processo(task, revTask), vari);
                if (tsk != null && tsk.Task != null && tsk.variant != null && tsk.Task.processID != -1 && tsk.variant.idVariante != -1)
                {
                    tsk.Task.loadPrecedenti(new variante(variante));
                    bool found = false;
                    int index = -1;
                    try
                    {
                        var controllo = tsk.Task.processiPrec.Where(x => x == prec).First();
                        index = tsk.Task.processiPrec.IndexOf(prec);
                        found = true;
                    }
                    catch
                    {
                        found = false;
                    }

                    if (found)
                    {
                        TaskVariante tskPrec = new TaskVariante(new processo(prec, revPrec), new variante(variante));

                        int ore = -1, minuti = -1, secondi = -1;
                        try
                        {
                            ore = Int32.Parse(ddlHour.SelectedValue);
                            minuti = Int32.Parse(ddlMinutes.SelectedValue);
                            secondi = Int32.Parse(ddlSeconds.SelectedValue);
                        }
                        catch
                        {
                            ore = -1;
                            minuti = -1;
                            secondi = -1;
                        }

                        if(ore >=0 && minuti>=0 && minuti < 60 && secondi>=0 &&secondi < 60)
                        { 
                        TimeSpan pausa = new TimeSpan(ore, minuti, secondi);
                        Boolean ret = tsk.Task.setPausaPrec(tskPrec, pausa);

                        if (ret)
                        {
                                lblInfo.Text = GetLocalResourceObject("lblModificaOk").ToString(); 
                                Response.Write("<script language='javascript'>window.close();</script>");
                            }
                        else
                        {
                            lblInfo.Text = "Error " + tsk.Task.log;
                        }
                        }
                        else
                        {
                            lblInfo.Text = GetLocalResourceObject("lblErrorData").ToString()+ ": " + ore + ":" + minuti + ":" + secondi;
                        }
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorDipendenza1").ToString()+ " " + prec + ", " + revPrec + " "
                            + GetLocalResourceObject("lblErrorDipendenza2").ToString()
                            + " "
                            + tsk.Task.processName + ", " + tsk.variant.nomeVariante;
                        tblPausa.Visible = false;
                    }
                }
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            if (task != -1 && revTask != -1 && variante != -1 && prec != -1 && revPrec != -1)
            {
                variante vari = new variante(variante);
                TaskVariante tsk = new TaskVariante(new processo(task, revTask), vari);
                if (tsk != null && tsk.Task != null && tsk.variant != null && tsk.Task.processID != -1 && tsk.variant.idVariante != -1)
                {
                    tsk.Task.loadPrecedenti(new variante(variante));
                    bool found = false;
                    int index = -1;
                    try
                    {
                        var controllo = tsk.Task.processiPrec.Where(x => x == prec).First();
                        index = tsk.Task.processiPrec.IndexOf(prec);
                        found = true;
                    }
                    catch
                    {
                        found = false;
                    }

                    if (found)
                    {
                        TaskVariante tskPrec = new TaskVariante(new processo(prec, revPrec), new variante(variante));

                        lbl1.Text = "";
                        tblPausa.Visible = true;
                            ddlHour.SelectedValue = Math.Truncate(tsk.Task.pausePrec[index].TotalHours).ToString();
                            ddlMinutes.SelectedValue = tsk.Task.pausePrec[index].Minutes.ToString();
                            ddlSeconds.SelectedValue = tsk.Task.pausePrec[index].Seconds.ToString();
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorDipendenza1").ToString()+" " 
                            + prec + ", " + revPrec + " "+ GetLocalResourceObject("lblErrorDipendenza2").ToString()
                            +" "
                            + tsk.Task.processName + ", " + tsk.variant.nomeVariante;
                        tblPausa.Visible = false;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tblPausa.Visible = false;
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
                if (task != -1 && revTask != -1 && variante != -1 && prec != -1 && revPrec != -1)
                {
                    variante vari = new variante(variante);
                    TaskVariante tsk = new TaskVariante(new processo(task, revTask), vari);
                    if (tsk != null && tsk.Task != null && tsk.variant != null && tsk.Task.processID != -1 && tsk.variant.idVariante != -1)
                    {
                        tsk.Task.loadPrecedenti(new variante(variante));
                        bool found = false;
                        int index = -1;
                        try
                        {
                            var controllo = tsk.Task.processiPrec.Where(x => x == prec).First();
                            index = tsk.Task.processiPrec.IndexOf(prec);
                            found = true;
                        }
                        catch
                        {
                            found = false;
                        }

                        if (found)
                        {
                            TaskVariante tskPrec = new TaskVariante(new processo(prec, revPrec), new variante(variante));

                            lbl1.Text = "";
                            tblPausa.Visible = true;
                            if(!Page.IsPostBack)
                            {
                                lblNomeSucc.Text = tsk.Task.processName;
                                lblNomePrec.Text = tskPrec.Task.processName;
                                for(int i = 0; i <=1000; i++)
                                {
                                    ddlHour.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                }
                                for (int i = 0; i < 60; i++)
                                {
                                    ddlMinutes.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                    ddlSeconds.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                }

                                ddlHour.SelectedValue = Math.Truncate(tsk.Task.pausePrec[index].TotalHours).ToString();
                                ddlMinutes.SelectedValue = tsk.Task.pausePrec[index].Minutes.ToString();
                                ddlSeconds.SelectedValue = tsk.Task.pausePrec[index].Seconds.ToString();
                            }
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrorDipendenza1").ToString()
                                +" " + prec + ", " + revPrec 
                                + " " + GetLocalResourceObject("lblErrorDipendenza2").ToString()
                                +" "
                                + tsk.Task.processName + ", " + tsk.variant.nomeVariante;
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
            }
    }
}