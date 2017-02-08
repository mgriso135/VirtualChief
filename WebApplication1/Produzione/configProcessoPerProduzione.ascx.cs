using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KIS.Commesse;
using KIS.App_Code;

namespace KIS.Produzione
{
    public partial class configProcessoPerProduzione : System.Web.UI.UserControl
    {
        public int artID, procID, varID, artYear;
        public static int repID;
        public static ConfigurazioneProcesso configProc;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                procID = -1;
                varID = -1;
                Articolo art = null;
                if (artID != -1)
                {
                    art = new Articolo(artID, artYear);
                    if (art.ID != -1 && art.Proc != null)
                    {
                        procID = art.Proc.process.processID;
                        varID = art.Proc.variant.idVariante;
                    }
                }

                if (procID != -1 && varID != -1 && art != null)
                {
                    ProcessoVariante prc = new ProcessoVariante(new processo(procID), new variante(varID));
                    if (prc.process != null && prc.variant != null && prc.RepartoProduttivo != null)
                    {
                        if (prc.process.isVSM == false)
                        {
                            List<TaskConfigurato> lst = new List<TaskConfigurato>();
                            if (!Page.IsPostBack)
                            {
                                lblDataPrevistaConsegna.Text = art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
                                lblDataPrevistaFP.Text = art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy HH:mm:ss");
                                lblNomeProc.Text = prc.process.processName;
                                lblNomeVariante.Text = prc.variant.nomeVariante;
                                lblRevProc.Text = "Revisione: " + prc.process.revisione.ToString();
                                lblQuantita.Text = "Quantità: " + art.Quantita.ToString();
                                prc.loadReparto();
                                for (int i = 0; i < prc.RepartiProduttivi.Count; i++)
                                {
                                    ddlRepartoProduttivo.Items.Add(new ListItem(prc.RepartiProduttivi[i].name, prc.RepartiProduttivi[i].id.ToString()));
                                }

                            }
                            /*else
                            {
                            }*/
                        }
                        else
                        {
                            lbl1.Text = "Errore tipo di processo<br/>";
                        }
                    }
                    else
                    {
                        ddlRepartoProduttivo.Visible = false;
                        if (prc.RepartoProduttivo != null)
                        {
                            lbl1.Text = "Errore generico<br/>";
                        }
                        else
                        {
                            lbl1.Text = "<div class='alert alert-error'>Attenzione: è necessario associare il prodotto a uno o più reparti produttivi.</div>";
                        }
                    }
                }
                else
                {
                    lbl1.Text = "Errore<br/>";
                }
            }
            else
            {
                lbl1.Text = "Errore: non hai il permesso di lanciare un articolo in produzione.<br/>";
                rptTasks.Visible = false;
                ddlRepartoProduttivo.Visible = false;
            }
        }

        protected void rptTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                int taskID = Int32.Parse(((HiddenField)e.Item.FindControl("taskID")).Value);
                DropDownList ddlTempi =(DropDownList)e.Item.FindControl("ddlTempi");
                
                int ind = -1;
                for (int i = 0; i < configProc.Processi.Count; i++)
                {
                    if (taskID == configProc.Processi[i].Task.Task.processID)
                    {
                        ind = i;
                    }
                }

                if (ind != -1)
                {
                    TaskVariante tsk = new TaskVariante(new processo(taskID), new variante(varID));
                    tsk.loadTempiCiclo();
                    ddlTempi.DataSource = tsk.Tempi.Tempi;
                    ddlTempi.DataValueField = "NumeroOperatori";
                    ddlTempi.DataValueField = "NumeroOperatori";
                    ddlTempi.SelectedValue = configProc.Processi[ind].Tempo.NumeroOperatori.ToString();
                    ddlTempi.DataBind();

                    Label tc = (Label)e.Item.FindControl("tc");
                    int num_op = Int32.Parse(ddlTempi.SelectedValue);
                    TimeSpan tempo = new TimeSpan(0, 0, 0);
                    TimeSpan tempoSetup = new TimeSpan(0,0,0);
                    for (int i = 0; i < tsk.Tempi.Tempi.Count; i++)
                    {
                        if (tsk.Tempi.Tempi[i].NumeroOperatori == num_op)
                        {
                            tempo = TimeSpan.FromTicks(tsk.Tempi.Tempi[i].Tempo.Ticks * configProc.Quantita);
                            tempoSetup = tsk.Tempi.Tempi[i].TempoSetup;
                        }
                    }
                    tc.Text = tempo.Hours.ToString() + ":" + tempo.Minutes.ToString() + ":" + tempo.Seconds.ToString();
                    Label setup = (Label)e.Item.FindControl("setup");
                    setup.Text = tempoSetup.Hours.ToString() + ":" + tempoSetup.Minutes.ToString() + ":" + tempoSetup.Seconds.ToString();

                    Label prec = (Label)e.Item.FindControl("lblPrecedenti");
                    for (int i = 0; i < configProc.Processi[ind].Precedenti.Count; i++)
                    {
                        processo s = new processo(configProc.Processi[ind].Precedenti[i]);

                        prec.Text += s.processID.ToString() + " " + s.processName 
                            + "("+ configProc.Processi[ind].PrecedentiPausa[i].TotalHours.ToString()
                            +  ")"+"<br/>";
                    }

                    Label succ = (Label)e.Item.FindControl("lblSuccessivi");
                    for (int i = 0; i < configProc.Processi[ind].Successivi.Count; i++)
                    {
                        processo s = new processo(configProc.Processi[ind].Successivi[i]);

                        succ.Text += s.processID.ToString() + " " + s.processName + "<br/>";
                    }
                }
            }

            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void ddlTempi_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            int numOps = Int32.Parse(ddl.SelectedValue.ToString());
            HtmlTableRow tr = (HtmlTableRow)ddl.Parent.Parent;
            HiddenField tsk = (HiddenField)tr.FindControl("taskID");
            int task = Int32.Parse(tsk.Value);
            Articolo curr = new Articolo(artID, artYear);
            for (int i = 0; i < configProc.Processi.Count; i++)
            {
                if (configProc.Processi[i].Task.Task.processID == task)
                {
                    TempoCiclo t = new TempoCiclo(task, configProc.Processi[i].Task.Task.revisione, varID, numOps);
                    configProc.Processi[i] = new TaskConfigurato(new TaskVariante(new processo(task), new variante(varID)), t, configProc.RepartoProduttivo.id, curr.Quantita);
                }
            }

            ProcessoVariante prc = new ProcessoVariante(new processo(procID), new variante(varID));
            configProc = new ConfigurazioneProcesso(curr, configProc.Processi, new Reparto(repID), curr.Quantita);
            configProc.calculateCriticalPath();
            rptTasks.DataSource = configProc.Processi;
            rptTasks.DataBind();

        }


        protected void rptTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "checkConfigurazione" && e.CommandArgument.ToString() == "CLICK")
            {
                Articolo currArt = new Articolo(artID, artYear);
                List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
                Repeater rpt = (Repeater)source;
                foreach (RepeaterItem rptItm in rpt.Items)
                {
                    HiddenField tsk = (HiddenField)rptItm.FindControl("taskID");
                    DropDownList ops = (DropDownList)rptItm.FindControl("ddlTempi");
                    int tskID = -1;
                    int opsID = -1;
                    try
                    {
                        tskID = Int32.Parse(tsk.Value);
                        opsID = Int32.Parse(ops.SelectedValue);
                    }
                    catch
                    {
                        tskID = -1;
                        opsID = -1;
                    }
                    
                    TaskVariante tskVar = new TaskVariante(new processo(tskID), new variante(varID));
                    tskVar.loadTempiCiclo();
                    TempoCiclo tc = new TempoCiclo(tskVar.Task.processID, tskVar.Task.revisione, varID, opsID);
                    if (tc.Tempo != null)
                    {
                        lstTasks.Add(new TaskConfigurato(tskVar, tc, repID, currArt.Quantita));
                    }
                }
                ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(currArt, lstTasks, new Reparto(repID), currArt.Quantita);

                int rt = prcCfg.SimulaIntroduzioneInProduzione();
                
                if (rt == 1)
                {
                    rptControllo.Visible = true;
                    prcCfg.Processi.Sort(delegate(TaskConfigurato p1, TaskConfigurato p2)
                    {
                        return p1.EarlyStartDate.CompareTo(p2.EarlyStartDate);
                    });
                    rptControllo.DataSource = prcCfg.Processi;
                    rptControllo.DataBind();
                }
                else
                {
                    rptControllo.Visible = false;
                    if (rt == 2)
                    {
                        lbl1.Text += "Attenzione: tempo a disposizione inferiore al Critical Path. Variare la data presunta di fine produzione. Verificare anche la data di consegna prevista.";
                    }
                    else if (rt == 3)
                    {
                        lbl1.Text += "Attenzione: non sono riuscito a fissare delle date per tutti i task. Variare la data di fine produzione oppure variare l'impostazione relativa alla suddivisione dei task tra gli intervalli lavorativi.";
                    }
                }
            }
        }

        protected void rptControllo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr2");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr2");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptControllo_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            lbl1.Text = "Inserisco questi task in produzione!<br/>";
            if (e.CommandName == "ProductionLaunch" && e.CommandArgument.ToString() == "OK")
            {
                Articolo tobePlanned = new Articolo(artID, artYear);
                ImageButton btnLancia = (ImageButton)e.Item.FindControl("btnLANCIA");
                btnLancia.Enabled = false;
                List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
                Repeater rpt = (Repeater)sender;
                foreach (RepeaterItem rptItm in rpt.Items)
                {
                    HiddenField tsk = (HiddenField)rptItm.FindControl("taskID");
                    HiddenField ops = (HiddenField)rptItm.FindControl("numOps");
                    int tskID = -1;
                    int opsID = -1;
                    try
                    {
                        tskID = Int32.Parse(tsk.Value);
                        opsID = Int32.Parse(ops.Value);
                    }
                    catch
                    {
                        tskID = -1;
                        opsID = -1;
                    }

                    TaskVariante tskVar = new TaskVariante(new processo(tskID), new variante(varID));
                    tskVar.loadTempiCiclo();
                    TempoCiclo tc = new TempoCiclo(tskVar.Task.processID, tskVar.Task.revisione, varID, opsID);
                    if (tc.Tempo != null)
                    {
                        lstTasks.Add(new TaskConfigurato(tskVar, tc, repID, tobePlanned.Quantita));
                    }
                }

                ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(tobePlanned, lstTasks, new Reparto(repID), tobePlanned.Quantita);
                int rt1 = prcCfg.SimulaIntroduzioneInProduzione();
                if (rt1 == 1)
                {
                    tobePlanned.Planner = (User)Session["user"];
                    int rt = prcCfg.LanciaInProduzione();
                    if (rt == 1)
                    {
                        lbl1.Text = "Articolo inserito correttamente in produzione<br/>";
                        btnLancia.Enabled = false;
                    }
                    else if(rt==3)
                    {
                        lbl1.Text = "Articolo già lanciato in produzione<br />";
                    }
                    else
                    {
                        lbl1.Text = "Si è verificato un errore: " + prcCfg.log;
                    }
                }
                else
                {
                    lbl1.Text = "Errore in fase di simulazione<br />";
                }

            }
        }

        protected void ddlRepartoProduttivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Articolo art = new Articolo(artID, artYear);
            List<TaskConfigurato> lst = new List<TaskConfigurato>();
            ProcessoVariante prc = new ProcessoVariante(new processo(procID), new variante(varID));
            repID = -1;
            try
            {
                repID = Int32.Parse(ddlRepartoProduttivo.SelectedValue);
            }
            catch
            {
                repID = -1;
            }

            if(repID!=-1)
            {
                Reparto rp = new Reparto(repID);
                // controllo che il reparto esista, e che appartenga al processo selezionato
                bool checkReparto = false;
                if(rp.id!=-1)
                {
                    prc.loadReparto();
                    for(int i = 0; i < prc.RepartiProduttivi.Count; i++)
                    {
                        if(prc.RepartiProduttivi[i].id == rp.id)
                        {
                            checkReparto = true;
                        }
                    }
                }
                else
                {
                    checkReparto = false;
                }

                if(checkReparto == true)
                {
                    art.Reparto = repID;
                    ddlRepartoProduttivo.Enabled = false;
            prc.process.loadFigli(prc.variant);
            for (int i = 0; i < prc.process.subProcessi.Count; i++)
            {
                TaskVariante tskVar = new TaskVariante(prc.process.subProcessi[i], prc.variant);
                if (tskVar.getDefaultTempo() != null)
                {
                    lst.Add(new TaskConfigurato(tskVar, tskVar.getDefaultTempo(), rp.id, art.Quantita));
                }
                else
                {
                    lbl1.Text = "<span style='color:red;'>Errore: il task " + tskVar.Task.processName.ToString() + " non ha nemmeno un tempo ciclo definito. Non posso inserirlo tra le scelte possibili.</span>";
                }
            }

            configProc = new ConfigurazioneProcesso(art, lst, new Reparto(repID), art.Quantita);
            int consistenza = configProc.checkConsistency();
            if (consistenza == 1)
            {
                configProc.Processi.Sort(delegate(TaskConfigurato p1, TaskConfigurato p2)
                {
                    return p1.EarlyStartTime.CompareTo(p2.EarlyStartTime);
                });

                configProc.calculateCriticalPath();

                rptTasks.DataSource = configProc.Processi;
                rptTasks.DataBind();
            }
            else
            {
                /* Returns:
                 * 0 if generic error
                 * 1 if all is fine
                 * 2 if there is some task without link between other tasks
                 * 3 if diagram type is NOT Pert
                 * 5 if some subtask is missing Kpi called "Tempo ciclo"
                 * 6 if some subtask is missing la postazione
                 */
                if (consistenza == 0)
                {
                    lbl1.Text = "Generico error<br/>";
                }
                else if (consistenza == 2)
                {
                    lbl1.Text = "C'è qualche task \"orfano\" di almeno un precedente o un successivo<br/>";
                }
                else if (consistenza == 3)
                {
                    lbl1.Text = "Il tipo di digramma non è un PERT. Impossibile lanciare in produzione un digramma di questo tipo<br/>";
                }
                else if (consistenza == 6)
                {
                    lbl1.Text = "A qualche task non è stata assegnata una postazione di lavoro di default<br/>";
                }

            }
            }
            }
        }
    }
}