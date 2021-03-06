using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Commesse
{
    public partial class wzCheckWorkLoadReparto1 : System.Web.UI.UserControl
    {
        public int idCommessa, annoCommessa, idProc, revProc, idVariante, idReparto, idProdotto, annoProdotto;
        public string matricola;

        public static List<DateTime> list = new List<DateTime>();
        public static List<int> idPostazioni = new List<int>();
        public static DateTime inizio, fine;
        public static List<Articolo> articoliNuovi = new List<Articolo>();
        public static Articolo art;
        public static Reparto rp;
        public static TimeSpan somma;
        public static List<caricoDiLavoro> carichi;
        public static double[] orePostazione;

        protected void Page_Load(object sender, EventArgs e)
        {
            lnkGoBack.Visible = false;
            imgGoBack.Visible = false;
            imgGoFwd.Visible = false;

            tblContainer.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto WorkLoad";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Reparto WorkLoad";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                tblContainer.Visible = true;
                lnkGoBack.Visible = true;
                imgGoBack.Visible = true;
                lnkGoBack.NavigateUrl = "wzQuestionWorkLoad.aspx?idCommessa=" + idCommessa.ToString()
                + "&annoCommessa="+ annoCommessa.ToString()
                + "&idProc=" + idProc.ToString()
                + "&revProc=" + revProc.ToString()
                + "&idVariante=" + idVariante.ToString()
                +"&idReparto=" + idReparto.ToString()
                + "&idProdotto=" + idProdotto.ToString()
                + "&annoProdotto=" + annoProdotto.ToString()
                + "&matricola=" + matricola.ToString();

                if (!Page.IsPostBack)
                {
                    articoliNuovi = new List<Articolo>();
                    art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), idProdotto, annoProdotto);
                    
                    articoliNuovi.Add(art);

                    for (int i = 0; i < 60; i++)
                    {
                        calMinuti.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        calSecondi.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    for (int i = 0; i < 24; i++)
                    {
                        calOre.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    if (art.DataPrevistaFineProduzione > new DateTime(1971, 1, 1))
                    {
                        txtProductDate.Text = art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy");
                        calOre.SelectedValue = art.DataPrevistaFineProduzione.ToString("HH");
                        calMinuti.SelectedValue = art.DataPrevistaFineProduzione.ToString("mm");
                        calSecondi.SelectedValue = art.DataPrevistaFineProduzione.ToString("ss");
                    }


                    dvErr.Visible = false;
                    dvInfo.Visible = false;
                    
                    chkLstPostazioni.Visible = false;
                    fine = inizio;
                    //Reparto 
                    rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                    inizio = TimeZoneInfo.ConvertTimeFromUtc( DateTime.UtcNow, rp.tzFusoOrario);
                    rp.loadPostazioni();
                    idPostazioni = new List<int>();
                    for (int i = 0; i < rp.Postazioni.Count; i++)
                    {
                        idPostazioni.Add(rp.Postazioni[i].id);
                        chkLstPostazioni.Items.Add(new ListItem(rp.Postazioni[i].name, rp.Postazioni[i].id.ToString()));
                        chkLstPostazioni.Items[i].Selected = true;
                    }
                    Chart1.Visible = false;

                    if (art.DataPrevistaFineProduzione != null && art.DataPrevistaFineProduzione > new DateTime(1970, 1, 1))
                    {
                        // Carico il grafico!
                        if (art.DataPrevistaFineProduzione >= DateTime.Now && art.DataPrevistaFineProduzione <= art.DataPrevistaConsegna)
                        {
                            imgGoFwd.Visible = true;
                            dvErr.Visible = false;
                            lblErr.Visible = false;
                            dvInfo.Visible = true;

                            bool check = true;
                            List<TaskConfigurato> tskLst = new List<TaskConfigurato>();
                            ProcessoVariante prcVar = art.Proc;
                            prcVar.process.loadFigli(art.Proc.variant);
                            for (int j = 0; j < prcVar.process.subProcessi.Count; j++)
                            {
                                TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), prcVar.process.subProcessi[j], prcVar.variant);
                                TempoCiclo tc = null;
                                tskVar.loadTempiCiclo();
                                for (int k = 0; k < tskVar.Tempi.Tempi.Count; k++)
                                {
                                    if (tskVar.Tempi.Tempi[k].Default == true)
                                    {
                                        tc = tskVar.Tempi.Tempi[k];
                                    }
                                }

                                if (tc != null)
                                {
                                    tskLst.Add(new TaskConfigurato(Session["ActiveWorkspace_Name"].ToString(), tskVar, tc, idReparto, art.Quantita));
                                }
                                else
                                {
                                    check = false;
                                }
                            }

                            ConfigurazioneProcesso cfgPrc = new ConfigurazioneProcesso(Session["ActiveWorkspace_Name"].ToString(), art, tskLst, new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto), art.Quantita);
                            int retSim = cfgPrc.SimulaIntroduzioneInProduzione();
                            lbl1.Text = retSim.ToString() + "<br />";
                            if (retSim == 1)
                            {
                                var minStart = cfgPrc.CriticalPath.Min(lateStart => lateStart.EarlyStartDate);
                                var maxStart = cfgPrc.CriticalPath.Max(lateFinish => lateFinish.LateFinishDate);
                                inizio = minStart;
                                fine = maxStart.AddDays(1);

                                lbl1.Text = inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + fine.ToString("dd/MM/yyyy HH:mm:ss");
                                loadProductionPlan();
                                caricaGrafico();
                            }
                            else if (retSim == 2)
                            {
                                lblErr.Text = GetLocalResourceObject("lblError1").ToString();
                            }
                            else if (retSim == 3)
                            {
                                lblErr.Text = GetLocalResourceObject("lblErrorGenerico").ToString();
                            }
                            else if (retSim == 4)
                            {
                                lblErr.Text = GetLocalResourceObject("lblErrorData").ToString();
                            }

                        }
                    }

                }
                else
                {
                    if (inizio < fine)
                    {
                        //caricaGrafico();
                        imgGoFwd.Visible = true;
                    }
                }
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                chkLstPostazioni.Visible = false;
                rbPostazioni.Visible = false;
            }
        }
        
        protected void rbPostazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            int res = -1;
            try
            {
                res = Int32.Parse(rbPostazioni.SelectedValue);
            }
            catch
            {
                res = -1;
            }
            if (res == 0)
            {
                chkLstPostazioni.Visible = false;
                //Reparto rp = new Reparto(idReparto);
                //rp.loadPostazioni();
                idPostazioni = new List<int>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    lbl1.Text += GetLocalResourceObject("lblAggiungo") + " " + rp.Postazioni[i].name + "<br />";
                    idPostazioni.Add(rp.Postazioni[i].id);
                }
                if (inizio < fine)
                {
                    caricaGrafico();
                }
            }
            else
            {
                chkLstPostazioni.Visible = true;
                //chkLstPostazioni.Items.Clear();
                //Reparto rp = new Reparto(idReparto);
                //rp.loadPostazioni();
                idPostazioni = new List<int>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    //chkLstPostazioni.Items.Add(new ListItem(rp.Postazioni[i].name, rp.Postazioni[i].id.ToString()));
                    //chkLstPostazioni.Items[i].Selected = true;
                    idPostazioni.Add(rp.Postazioni[i].id);
                }
                if (inizio < fine)
                {
                    caricaGrafico();
                }
            }
        }

        protected void checkFlagsPostazioni()
        {
            if (idPostazioni != null)
            {
                idPostazioni.Clear();
            }
            idPostazioni = new List<int>();
            for (int i = 0; i < chkLstPostazioni.Items.Count; i++)
            {
                if (chkLstPostazioni.Items[i].Selected == true)
                {
                    int idP = -1;
                    try
                    {
                        idP = Int32.Parse(chkLstPostazioni.Items[i].Value);
                    }
                    catch
                    {
                        idP = -1;
                    }
                    if (idP != -1)
                    {
                        idPostazioni.Add(idP);
                    }
                }
            }
        }

        protected void chkLstPostazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (idPostazioni != null)
            {
                idPostazioni.Clear();
            }
            idPostazioni = new List<int>();
            for (int i = 0; i < chkLstPostazioni.Items.Count; i++)
            {
                if (chkLstPostazioni.Items[i].Selected == true)
                {
                    int idP = -1;
                    try
                    {
                        idP = Int32.Parse(chkLstPostazioni.Items[i].Value);
                    }
                    catch
                    {
                        idP = -1;
                    }
                    if (idP != -1)
                    {
                        idPostazioni.Add(idP);
                    }
                }
            }
            if (inizio < fine)
            {
                caricaGrafico();
            }
        }

        protected void loadProductionPlan()
        {
            somma = new TimeSpan(0, 0, 0);
            //List<caricoDiLavoro> 
                carichi = new List<caricoDiLavoro>();
            if (rp.id != -1 && inizio < fine)
            {
                if (rp.Postazioni == null || rp.Postazioni.Count == 0)
                {
                    rp.loadPostazioni();
                }
                
                int cont = 0;
                
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    bool found = false;
                    // Se lo trovo nell'elenco delle postazioni selezionate...
                    // Usare LINQ
                    for (int q = 0; q < idPostazioni.Count; q++)
                    {
                        if (rp.Postazioni[i].id == idPostazioni[q])
                        {
                            found = true;
                        }
                    }

                    if (found == true)
                    {
                        caricoDiLavoro cdl = new caricoDiLavoro();
                        cdl.postazione = rp.Postazioni[i];
                        cdl.carico = rp.Postazioni[i].getCaricoDiLavoroProgrammato(rp.id, inizio, fine);
                        cdl.CaricoOre = cdl.carico.TotalHours;
                        cdl.articolo = -1;
                        cdl.articoloAnno = -1;
                        cdl.DaProgrammare = false;
                        TimeSpan carico = rp.Postazioni[i].getCaricoDiLavoroProgrammato(rp.id, inizio, fine);
                        carichi.Add(cdl);
                        somma += carico;
                    }
                }

                // Configuro i prodotti selezionati
                List<ConfigurazioneProcesso> processiConf = new List<ConfigurazioneProcesso>();
                for (int i = 0; i < articoliNuovi.Count; i++)
                {
                    if (articoliNuovi[i] != null && articoliNuovi[i].Proc != null && articoliNuovi[i].Proc.process != null && articoliNuovi[i].Proc.variant != null)
                    {
                        List<TaskConfigurato> tskLst = new List<TaskConfigurato>();
                        ProcessoVariante prcVar = articoliNuovi[i].Proc;
                        prcVar.process.loadFigli(articoliNuovi[i].Proc.variant);
                        bool check = true;
                        for (int j = 0; j < prcVar.process.subProcessi.Count; j++)
                        {
                            TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), prcVar.process.subProcessi[j], prcVar.variant);
                            TempoCiclo tc = null;
                            tskVar.loadTempiCiclo();
                            for (int k = 0; k < tskVar.Tempi.Tempi.Count; k++)
                            {
                                if (tskVar.Tempi.Tempi[k].Default == true)
                                {
                                    tc = tskVar.Tempi.Tempi[k];
                                }
                            }

                            if (tc != null)
                            {
                                tskLst.Add(new TaskConfigurato(Session["ActiveWorkspace_Name"].ToString(), tskVar, tc, articoliNuovi[i].Reparto, articoliNuovi[i].Quantita));
                            }
                            else
                            {
                                check = false;
                            }
                        }
                        if (check == true)
                        {

                            ConfigurazioneProcesso cfgPrc = new ConfigurazioneProcesso(Session["ActiveWorkspace_Name"].ToString(), articoliNuovi[i], tskLst, new Reparto(Session["ActiveWorkspace_Name"].ToString(), articoliNuovi[i].Reparto), articoliNuovi[i].Quantita);
                            int retSim = cfgPrc.SimulaIntroduzioneInProduzione();
                            processiConf.Add(cfgPrc);
                            cont = 0;
                            for (int z = 0; z < cfgPrc.Processi.Count; z++)
                            {
                                lbl1.Text += articoliNuovi[i].ID.ToString() +
                                    " " + cfgPrc.Processi[z].Task.Task.processName + " ";
                                // i 4 casi...
                                TimeSpan cdl = new TimeSpan(0, 0, 0);
                                if (cfgPrc.Processi[z].LateStartDate >= inizio && cfgPrc.Processi[z].LateFinishDate <= fine)
                                {
                                    cdl = cfgPrc.Processi[z].TempoTotale;
                                }
                                else if (cfgPrc.Processi[z].LateStartDate <= inizio && cfgPrc.Processi[z].LateFinishDate >= inizio && cfgPrc.Processi[z].LateFinishDate <= fine)
                                {
                                    rp.loadCalendario(cfgPrc.Processi[z].LateStartDate.AddDays(-1), fine.AddDays(1));

                                    for (int h = 0; h < rp.CalendarioRep.Intervalli.Count; h++)
                                    {
                                        if (rp.CalendarioRep.Intervalli[h].Fine <= inizio)
                                        {
                                            // Non aggiunto niente
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio > cfgPrc.Processi[z].LateFinishDate)
                                        {
                                            // Non aggiungo niente
                                        }
                                        else if (inizio >= rp.CalendarioRep.Intervalli[h].Inizio && rp.CalendarioRep.Intervalli[h].Fine >= inizio && rp.CalendarioRep.Intervalli[h].Fine <= cfgPrc.Processi[z].LateFinishDate)
                                        {
                                            cdl += rp.CalendarioRep.Intervalli[h].Fine - inizio;
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Fine <= cfgPrc.Processi[z].LateFinishDate)
                                        {
                                            cdl += rp.CalendarioRep.Intervalli[h].Fine - rp.CalendarioRep.Intervalli[h].Inizio;
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Inizio <= cfgPrc.Processi[z].LateFinishDate && rp.CalendarioRep.Intervalli[h].Fine > cfgPrc.Processi[z].LateFinishDate)
                                        {
                                            cdl += cfgPrc.Processi[z].LateFinishDate - rp.CalendarioRep.Intervalli[h].Inizio;
                                        }
                                    }

                                }
                                else if (cfgPrc.Processi[z].LateStartDate >= inizio && cfgPrc.Processi[z].LateStartDate <= fine && cfgPrc.Processi[z].LateFinishDate >= fine)
                                {
                                    rp.loadCalendario(inizio.AddDays(-1), cfgPrc.Processi[z].LateFinishDate.AddDays(1));
                                    for (int h = 0; h < rp.CalendarioRep.Intervalli.Count; h++)
                                    {
                                        if (rp.CalendarioRep.Intervalli[h].Fine < cfgPrc.Processi[z].LateStartDate)
                                        {
                                            // Non aggiungo niente
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio >= fine)
                                        {
                                            // Non aggiungo niente
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio <= cfgPrc.Processi[z].LateStartDate && cfgPrc.Processi[z].LateStartDate <= rp.CalendarioRep.Intervalli[h].Fine && rp.CalendarioRep.Intervalli[h].Fine <= fine)
                                        {
                                            cdl += rp.CalendarioRep.Intervalli[h].Fine - cfgPrc.Processi[z].LateStartDate;
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio >= cfgPrc.Processi[z].LateStartDate && rp.CalendarioRep.Intervalli[h].Fine <= fine)
                                        {
                                            cdl += rp.CalendarioRep.Intervalli[h].Fine - rp.CalendarioRep.Intervalli[h].Inizio;
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio >= cfgPrc.Processi[z].LateStartDate && fine <= rp.CalendarioRep.Intervalli[h].Fine && rp.CalendarioRep.Intervalli[h].Inizio <= fine)
                                        {
                                            cdl += fine - rp.CalendarioRep.Intervalli[h].Inizio;
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Fine >= fine && cfgPrc.Processi[z].LateStartDate >= rp.CalendarioRep.Intervalli[h].Inizio && cfgPrc.Processi[z].LateFinishDate >= fine)
                                        {
                                            cdl += fine - cfgPrc.Processi[z].LateStartDate;
                                        }
                                    }
                                }
                                else if (cfgPrc.Processi[z].LateStartDate <= inizio && cfgPrc.Processi[z].LateFinishDate >= fine)
                                {
                                    rp.loadCalendario(cfgPrc.Processi[z].LateStartDate.AddDays(-1), fine.AddDays(1));

                                    for (int h = 0; h < rp.CalendarioRep.Intervalli.Count; h++)
                                    {
                                        if (rp.CalendarioRep.Intervalli[h].Fine < inizio)
                                        {
                                            // Non aggiungo niente
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio > fine)
                                        {
                                            // Non aggiungo niente
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio <= inizio && rp.CalendarioRep.Intervalli[h].Fine >= inizio && rp.CalendarioRep.Intervalli[h].Fine <= fine)
                                        {
                                            cdl += rp.CalendarioRep.Intervalli[h].Fine - inizio;
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Fine <= fine)
                                        {
                                            cdl += rp.CalendarioRep.Intervalli[h].Fine - rp.CalendarioRep.Intervalli[h].Inizio;
                                        }
                                        else if (rp.CalendarioRep.Intervalli[h].Inizio <= fine && rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Fine >= fine)
                                        {
                                            cdl += fine - rp.CalendarioRep.Intervalli[h].Inizio;
                                        }
                                    }

                                }
                                bool found = false;
                                // Se lo trovo nell'elenco delle postazioni selezionate...
                                for (int q = 0; q < idPostazioni.Count; q++)
                                {
                                    if (cfgPrc.Processi[z].PostazioneDiLavoro.id == idPostazioni[q])
                                    {
                                        found = true;
                                    }
                                }

                                if (found == true)
                                {
                                    // Aggiungo il "carico di lavoro"

                                    caricoDiLavoro curr = new caricoDiLavoro();
                                    curr.postazione = cfgPrc.Processi[z].PostazioneDiLavoro;
                                    curr.carico = cdl;
                                    curr.CaricoOre = cdl.TotalHours;
                                    curr.DaProgrammare = true;
                                    curr.articolo = cfgPrc.ArticoloID;
                                    curr.articoloAnno = cfgPrc.ArticoloAnno;
                                    carichi.Add(curr);
                                }


                                if (found == true || rbPostazioni.SelectedValue == "0")
                                {
                                    somma += cdl;
                                    cont++;
                                }

                            }
                        }
                        else
                        {
                            dvInfo.Visible = true;
                            lbl1.Text += GetLocalResourceObject("lblErrorCantSimulate1").ToString()  + " " + articoliNuovi[i].ID.ToString() + "/" + articoliNuovi[i].Year.ToString() 
                                + " " + GetLocalResourceObject("lblErrorCantSimulate2").ToString() +".<br />";
                        }
                    }

                }


                // QUI CARICO LE ORE PER POSTAZIONE!
                // Ri-seleziono tutti i flag delle postazioni come checcati!
                idPostazioni = new List<int>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    idPostazioni.Add(rp.Postazioni[i].id);
                    //chkLstPostazioni.Items.Add(new ListItem(rp.Postazioni[i].name, rp.Postazioni[i].id.ToString()));
                    chkLstPostazioni.Items[i].Selected = true;
                }

                //double[] 
                orePostazione = new double[idPostazioni.Count];
                // Inserisco punti nulli in carichi per le postazioni di articoli non presenti
                for (int i = 0; i < idPostazioni.Count; i++)
                {
                    orePostazione[i] = 0;
                    bool found = false;
                    for (int j = 0; j < carichi.Count; j++)
                    {
                        if (carichi[j].articolo == -1 && carichi[j].postazione.id == idPostazioni[i])
                        {
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        caricoDiLavoro nullo = new caricoDiLavoro();
                        nullo.postazione = new Postazione(Session["ActiveWorkspace_Name"].ToString(), idPostazioni[i]);
                        nullo.articolo = -1;
                        nullo.articoloAnno = -1;
                        nullo.carico = new TimeSpan(0, 0, 1);
                        nullo.CaricoOre = 0.1;
                        carichi.Add(nullo);
                    }

                    // Ricerco turni di lavoro e verifico la capacità produttiva.
                    Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), idPostazioni[i]);

                    p.loadCalendario(inizio, fine);

                    for (int b = 0; b < p.Calendario.Intervalli.Count; b++)
                    {
                        if (p.Calendario.Intervalli[b].idReparto == idReparto && (p.Calendario.Intervalli[b].Status == 'L' || p.Calendario.Intervalli[b].Status == 'S'))
                        {
                            Turno turno = new Turno(Session["ActiveWorkspace_Name"].ToString(), p.Calendario.Intervalli[b].idTurno);
                            RisorsePostazioneTurno resPost = new RisorsePostazioneTurno(Session["ActiveWorkspace_Name"].ToString(), p, turno);

                            lblErr.Text += p.name + " " + resPost.NumRisorse.ToString() + " " + p.Calendario.Intervalli[b].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + p.Calendario.Intervalli[b].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                            lbl1.Text += p.name + " " + resPost.NumRisorse.ToString() + " " + p.Calendario.Intervalli[b].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + p.Calendario.Intervalli[b].Fine.ToString("dd/MM/yyyy HH:mm:ss");

                            TimeSpan intervallo = new TimeSpan(0, 0, 0);
                            if (p.Calendario.Intervalli[b].Fine < inizio)
                            {
                                intervallo = new TimeSpan(0, 0, 0);
                            }
                            else if (p.Calendario.Intervalli[b].Inizio <= inizio && p.Calendario.Intervalli[b].Fine > inizio && p.Calendario.Intervalli[b].Fine < fine)
                            {
                                intervallo = p.Calendario.Intervalli[b].Fine - inizio;
                            }
                            else if (p.Calendario.Intervalli[b].Inizio >= inizio && p.Calendario.Intervalli[b].Inizio < fine && p.Calendario.Intervalli[b].Fine >= inizio && p.Calendario.Intervalli[b].Fine <= fine)
                            {
                                intervallo = p.Calendario.Intervalli[b].Fine - p.Calendario.Intervalli[b].Inizio;
                            }
                            else if (p.Calendario.Intervalli[b].Inizio >= inizio && p.Calendario.Intervalli[b].Inizio <= fine && p.Calendario.Intervalli[b].Fine >= fine)
                            {
                                intervallo = fine - p.Calendario.Intervalli[b].Inizio;
                            }
                            else
                            {
                                intervallo = new TimeSpan(0, 0, 0);
                            }

                            orePostazione[i] += resPost.NumRisorse * intervallo.TotalHours;
                        }
                    }
                } // FIN QUI, OREPOSTAZIONE!
                
            }
        }

        protected void caricaGrafico()
        {
            dvInfo.Visible = false;
            Chart1.Series.Clear();
            //loadProductionPlan();
            lbl1.Text = somma.TotalHours.ToString();// + " " + carichi.Count.ToString();
            if (rp.id!=-1 && inizio < fine && carichi!=null && carichi.Count >=0)
            {
                Chart1.Visible = true;
                //Reparto rp = new Reparto(idReparto);
                Chart1.Titles.Clear();
                Chart1.Titles.Add(new System.Web.UI.DataVisualization.Charting.Title(rp.name));

                // IL BLOCCONE DI CODICE ERA QUI!
                if (rbPostazioni.SelectedValue == "0")
                {
                    Chart1.Series.Clear();
                    var arts = carichi.GroupBy(i => i.articolo)
                            .Select(g => new
                            {
                                articolo = g.Key,
                                CaricoOre = g.Sum(i => i.carico.TotalHours),
                                colore = g.First().colore,
                                articoloAnno = g.First().articoloAnno
                            });

                    Chart1.ChartAreas[0].AxisX.Minimum = 0;
                    Chart1.ChartAreas[0].AxisY.Minimum = 0;

                    foreach (var cd in arts)
                    {
       
                        Chart1.Series.Add(cd.articolo.ToString());
                        Chart1.Series[cd.articolo.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.StackedColumn;
                        Chart1.Series[cd.articolo.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                        Chart1.Series[cd.articolo.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Int32;
                        //Chart1.Series[cd.articolo.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.StackedColumn;
                        //Chart1.Series[cd.articolo.ToString()].IsValueShownAsLabel = true;
                        //Chart1.Series[cd.articolo.ToString()].EmptyPointStyle.IsValueShownAsLabel = false;
                        //Chart1.Series[cd.articolo.ToString()].EmptyPointStyle.IsVisibleInLegend = false;
                        Chart1.Series[cd.articolo.ToString()].Points.AddXY(0, cd.CaricoOre);
                        int ind = Chart1.Series[cd.articolo.ToString()].Points.Count - 1;
                        if (cd.articolo == -1)
                        {
                            Chart1.Series[cd.articolo.ToString()].Points[ind].ToolTip = GetLocalResourceObject("lblCaricoFissato").ToString() + ": " + String.Format("{0:0.00}", cd.CaricoOre) + " "+ GetLocalResourceObject("lblOre").ToString();
                        }
                        else
                        {
                            Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), cd.articolo, cd.articoloAnno);
                            Chart1.Series[cd.articolo.ToString()].Points[ind].ToolTip = art.ID.ToString() + "/" +
                                art.Year.ToString() + " - " + art.Cliente + " - " +
                                art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante +
                                " - " + GetLocalResourceObject("lblCaricoDiLavoro").ToString() + ": " + String.Format("{0:0.00}", cd.CaricoOre.ToString()) + " "+ GetLocalResourceObject("lblOre").ToString();
                            Chart1.Series[cd.articolo.ToString()].AxisLabel = art.RepartoNome;
                        }
                        Chart1.Series[cd.articolo.ToString()].Points[ind].Label = String.Format("{0:0.00}", cd.CaricoOre) + " "+ GetLocalResourceObject("lblOre").ToString();
                        Chart1.Series[cd.articolo.ToString()].Points[ind].Color = cd.colore;

                        if (cd.CaricoOre == 0)
                        {
                            Chart1.Series[cd.articolo.ToString()].Points[ind].IsValueShownAsLabel = false;
                        }
                        else
                        {
                            Chart1.Series[cd.articolo.ToString()].Points[ind].IsValueShownAsLabel = true;
                        }

                    }
                }
                else
                {
                    
                    // BLOCCO PER POSTAZIONE ERA QUI!


                    for (int z = 0; z < articoliNuovi.Count; z++)
                    {
                        for (int i = 0; i < idPostazioni.Count; i++)
                        {
                            bool found = false;
                            for (int j = 0; j < carichi.Count; j++)
                            {
                                if (carichi[j].articolo == articoliNuovi[z].ID && carichi[j].postazione.id == idPostazioni[i])
                                {
                                    found = true;
                                }
                            }
                            if (found == false)
                            {
                                caricoDiLavoro nullo = new caricoDiLavoro();
                                nullo.postazione = new Postazione(Session["ActiveWorkspace_Name"].ToString(), idPostazioni[i]);
                                nullo.articolo = articoliNuovi[z].ID;
                                nullo.articoloAnno = articoliNuovi[z].Year;
                                nullo.carico = new TimeSpan(0, 0, 0);
                                nullo.CaricoOre = 0.0;
                                carichi.Add(nullo);
                            }
                        }
                    }
                    Chart1.Series.Clear();
                    Chart1.Series.Clear();
                    Chart1.Series.Add("risorse");
                    Chart1.Series["risorse"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                    Chart1.Series["risorse"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series["risorse"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Int32;
                    Chart1.Series["risorse"].EmptyPointStyle.IsValueShownAsLabel = false;
                    Chart1.Series["risorse"].EmptyPointStyle.IsVisibleInLegend = false;

                    for (int q = 0; q < idPostazioni.Count; q++)
                    {
                        lbl1.Text += GetLocalResourceObject("lblPostazione").ToString() + ": " + q.ToString() + "<br />";
                        List<caricoDiLavoro> caricoPst = new List<caricoDiLavoro>();

                        double totOre = 0;
                        for (int i = 0; i < carichi.Count; i++)
                        {
                            if (carichi[i].postazione.id == idPostazioni[q])
                            {
                                caricoPst.Add(carichi[i]);
                                totOre += carichi[i].carico.TotalHours;
                            }
                        }

                        List<caricoDiLavoro> carico2 = caricoPst.OrderBy(art => art.articolo).ThenByDescending(art => art.carico.TotalHours).ToList();

                        List<Double> sommaOre = new List<double>();
                        //double sumH = 0;

                        Postazione p = new Postazione(Session["ActiveWorkspace_Name"].ToString(), idPostazioni[q]);

                        var arts = carico2.GroupBy(i => i.articolo)
                            .Select(g => new
                            {
                                articolo = g.Key,
                                CaricoOre = g.Sum(i => i.carico.TotalHours),
                                colore = g.First().colore,
                                articoloAnno = g.First().articoloAnno
                            });



                        //int h = 0;
                        foreach (var cd in arts)
                        {
                            try
                            {
                                Chart1.ChartAreas[0].AxisY.Minimum = 0;
                                Chart1.Series.Add(cd.articolo.ToString());
                                Chart1.Series[cd.articolo.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.StackedColumn;
                                Chart1.Series[cd.articolo.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                                Chart1.Series[cd.articolo.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Int32;
                                Chart1.Series[cd.articolo.ToString()].EmptyPointStyle.IsValueShownAsLabel = false;
                                Chart1.Series[cd.articolo.ToString()].EmptyPointStyle.IsVisibleInLegend = false;

                            }
                            catch
                            {
                            }


                            int indice = Chart1.Series[cd.articolo.ToString()].Points.AddXY(q, cd.CaricoOre);
                            Chart1.Series[cd.articolo.ToString()].Points[indice].Color = cd.colore;
                            Chart1.Series[cd.articolo.ToString()].Points[indice].Label = String.Format("{0:0.00}", cd.CaricoOre) + " "+ GetLocalResourceObject("lblOre").ToString();
                            Chart1.Series[cd.articolo.ToString()].Points[indice].AxisLabel = p.name;
                            //lbl1.Text += q.ToString() + " " + Chart1.Series[cd.articolo.ToString()].Points[ind].XValue.ToString() + " " +
                            //    Chart1.Series[cd.articolo.ToString()].Points[ind].YValues[0].ToString() + "<br />";

                            if (cd.articolo == -1)
                            {
                                Chart1.Series[cd.articolo.ToString()].Points[indice].ToolTip = GetLocalResourceObject("lblCaricoFissato").ToString()+": " + String.Format("{0:0.00}", cd.CaricoOre) + " "+ GetLocalResourceObject("lblOre").ToString();
                            }
                            else
                            {
                                //Articolo art = new Articolo(cd.articolo, cd.articoloAnno);
                                Chart1.Series[cd.articolo.ToString()].Points[indice].ToolTip = art.ID.ToString() + "/" +
                                    art.Year.ToString() + " - " + art.Cliente + " - " +
                                    art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante +
                                    " "+ GetLocalResourceObject("lblPostazione").ToString() + " " + p.name +
                                    " - "+ GetLocalResourceObject("lblCaricoDiLavoro").ToString()+": " + String.Format("{0:0.00}", cd.CaricoOre) + " "+ GetLocalResourceObject("lblOre").ToString();
                            }
                            if (cd.CaricoOre < 0.01)
                            {
                                Chart1.Series[cd.articolo.ToString()].Points[indice].IsEmpty = true;
                                Chart1.Series[cd.articolo.ToString()].Points[indice].IsValueShownAsLabel = false;
                            }
                            else
                            {
                                Chart1.Series[cd.articolo.ToString()].Points[indice].IsValueShownAsLabel = true;
                            }

                            //lbl1.Text = "Serie " + cd.articolo.ToString() + " x: " + q.ToString() + " y: " + cd.CaricoOre.ToString() + "<br />";

                        }
                        // Limite carico di lavoro
                        int indRes = Chart1.Series["risorse"].Points.AddXY(q, orePostazione[q]);
                        Chart1.Series["risorse"].Points[indRes].Color = System.Drawing.Color.Transparent;
                        Chart1.Series["risorse"].Points[indRes].BorderColor = System.Drawing.Color.Red;
                        Chart1.Series["risorse"].Points[indRes].BorderWidth = 2;
                        if (orePostazione[q] > 0)
                        {
                            Chart1.Series["risorse"].Points[indRes].Label = GetLocalResourceObject("lblLimiteCapacita").ToString()+ " " + p.name + ": " + String.Format(orePostazione[q].ToString()) + " " + GetLocalResourceObject("lblOre").ToString();
                            Chart1.Series["risorse"].Points[indRes].ToolTip = GetLocalResourceObject("lblLimiteCapacita").ToString() + " " + p.name + ": " + String.Format(orePostazione[q].ToString()) + " " + GetLocalResourceObject("lblOre").ToString();
                        }
                        Chart1.Series["risorse"].Points[indRes].AxisLabel = p.name;
                        if (orePostazione[q] < 0.01)
                        {
                            Chart1.Series["risorse"].Points[indRes].IsEmpty = true;
                            Chart1.Series["risorse"].Points[indRes].IsValueShownAsLabel = false;
                        }
                        else
                        {
                            Chart1.Series["risorse"].Points[indRes].IsValueShownAsLabel = true;
                        }
                    }

                }

                dvInfo.Visible = true;
                lbl1.Text = GetLocalResourceObject("lblCaricoComplessivo1").ToString() +" "
                    + rp.name
                    + " "+ GetLocalResourceObject("lblCaricoComplessivo2").ToString() + " " 
                    + inizio.ToString("dd/MM/yyyy") + " - " + fine.ToString("dd/MM/yyyy") + ": <b>"
                    + String.Format("{0:0.00}", somma.TotalHours) + " "+ GetLocalResourceObject("lblOre").ToString()+"</b><br />";
            }
        }

        protected void btnSaveDataFineProd_Click(object sender, ImageClickEventArgs e)
        {
            imgGoFwd.Visible = false;
            dvErr.Visible = false;
            lblErr.Visible = false;
            int ore, min, sec, gg, mm, yy;
            String[] datafineprod = new String[3];
            datafineprod = (txtProductDate.Text).Split('/');
            DateTime finePrd;
            try
            {
                ore = Int32.Parse(calOre.SelectedValue);
                min = Int32.Parse(calMinuti.SelectedValue);
                sec = Int32.Parse(calSecondi.SelectedValue);
                gg = Int32.Parse(datafineprod[0]);
                mm = Int32.Parse(datafineprod[1]);
                yy = Int32.Parse(datafineprod[2]);

                finePrd = new DateTime(yy, mm, gg, ore, min, sec);
            }
            catch
            {
                ore = -1;
                min = -1;
                sec = -1;
                gg = -1;
                mm = -1;
                yy = -1;
                finePrd = new DateTime(1970, 1, 1);
            }
            //Articolo art = new Articolo(idProdotto, annoProdotto);
            if (art!=null && art.ID!=-1 && finePrd >= DateTime.Now && finePrd <= art.DataPrevistaConsegna)
            {
                imgGoFwd.Visible = true;
                dvErr.Visible = false;
                lblErr.Visible = false;
                dvInfo.Visible = true;
                lbl1.Text += finePrd.ToString("dd/MM/yyyy HH:mm:ss") + idReparto.ToString() + "!!! ";
                art.DataPrevistaFineProduzione = finePrd;

                bool check = true;
                List<TaskConfigurato> tskLst = new List<TaskConfigurato>();
                ProcessoVariante prcVar = art.Proc;
                prcVar.process.loadFigli(art.Proc.variant);
                for (int j = 0; j < prcVar.process.subProcessi.Count; j++)
                {
                    TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), prcVar.process.subProcessi[j], prcVar.variant);
                    TempoCiclo tc = null;
                    tskVar.loadTempiCiclo();
                    for (int k = 0; k < tskVar.Tempi.Tempi.Count; k++)
                    {
                        if (tskVar.Tempi.Tempi[k].Default == true)
                        {
                            tc = tskVar.Tempi.Tempi[k];
                        }
                    }

                    if (tc != null)
                    {
                        tskLst.Add(new TaskConfigurato(Session["ActiveWorkspace_Name"].ToString(), tskVar, tc, idReparto, art.Quantita));
                    }
                    else
                    {
                        check = false;
                    }
                }

                ConfigurazioneProcesso cfgPrc = new ConfigurazioneProcesso(Session["ActiveWorkspace_Name"].ToString(), art, tskLst, new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto), art.Quantita);
                int retSim = cfgPrc.SimulaIntroduzioneInProduzione();
                if (retSim == 1)
                {
                    DateTime minStart = DateTime.UtcNow;
                    DateTime maxStart = DateTime.UtcNow.AddMonths(1);
                    try
                    {
                        minStart = cfgPrc.CriticalPath.Min(lateStart => lateStart.EarlyStartDate);
                        maxStart = cfgPrc.CriticalPath.Max(lateFinish => lateFinish.LateFinishDate);
                    }
                    catch
                    {
                        minStart = DateTime.UtcNow;
                        maxStart = DateTime.UtcNow.AddMonths(1);
                    }
                    //inizio = minStart;
                    //fine = maxStart.AddDays(1);
                    inizio = new DateTime(minStart.Year, minStart.Month, minStart.Day);
                    fine = maxStart;

                    lbl1.Text = inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - " + fine.ToString("dd/MM/yyyy HH:mm:ss");
                    loadProductionPlan();
                    caricaGrafico();
                    imgGoFwd.Visible = true;
                }
                else if(retSim == 2)
                {
                    dvErr.Visible = true;
                    lblErr.Visible = true;
                    dvInfo.Visible = false;
                    lblErr.Text = GetLocalResourceObject("lblError1").ToString();
                }
                else if (retSim == 3)
                {
                    dvErr.Visible = true;
                    lblErr.Visible = true;
                    dvInfo.Visible = false;
                    lblErr.Text = GetLocalResourceObject("lblErrorGenerico2").ToString();
                }
                else if (retSim == 4)
                {
                    dvErr.Visible = true;
                    lblErr.Visible = true;
                    dvInfo.Visible = false;
                    lblErr.Text = GetLocalResourceObject("lblErrorData").ToString();
                }
            }
            else
            {
                if (finePrd >= art.DataPrevistaConsegna)
                {
                    dvErr.Visible = true;
                    lblErr.Text = GetLocalResourceObject("lblErrorData2").ToString();
                    Chart1.Visible = false;
                    dvInfo.Visible = false;
                }
                else
                {
                    dvErr.Visible = true;
                    lblErr.Text = GetLocalResourceObject("lblErrorData3").ToString();
                    Chart1.Visible = false;
                    dvInfo.Visible = false;
                }
                dvErr.Visible = true;
                lblErr.Visible = true;
                txtProductDate.Text = art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy");
                
            }
        }

        protected void imgGoFwd_Click(object sender, ImageClickEventArgs e)
        {
            List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
            Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), idProdotto, annoProdotto);

            if (art.Status == 'N')
            {
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), art.Reparto);

                art.Proc.process.loadFigli(art.Proc.variant);
                for (int i = 0; i < art.Proc.process.subProcessi.Count; i++)
                {
                    TaskVariante tskVar = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), art.Proc.process.subProcessi[i].processID, art.Proc.process.subProcessi[i].revisione), art.Proc.variant);
                    tskVar.loadTempiCiclo();
                    TempoCiclo tc = new TempoCiclo(Session["ActiveWorkspace_Name"].ToString(), tskVar.Task.processID, tskVar.Task.revisione, art.Proc.variant.idVariante, tskVar.getDefaultOperatori());
                    if (tc.Tempo != null)
                    {
                        lstTasks.Add(new TaskConfigurato(Session["ActiveWorkspace_Name"].ToString(), tskVar, tc, rp.id, art.Quantita));
                    }
                }

                ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(Session["ActiveWorkspace_Name"].ToString(), art, lstTasks, rp, art.Quantita);
                int rt1 = prcCfg.SimulaIntroduzioneInProduzione();
                if (rt1 == 1)
                {
                    art.Planner = (User)Session["user"];
                    int rt = prcCfg.LanciaInProduzione();
                    if (rt == 1)
                    {
                        Response.Redirect("wzImpostaAllarmiArticolo.aspx?idCommessa=" + art.Commessa.ToString()
                        + "&annoCommessa=" + art.AnnoCommessa.ToString()
                        + "&idProc=" + art.Proc.process.processID.ToString()
                        + "&revProc=" + art.Proc.process.revisione.ToString()
                        + "&idVariante=" + art.Proc.variant.idVariante.ToString()
                        + "&idReparto=" + art.Reparto.ToString()
                        + "&idProdotto=" + art.ID.ToString()
                        + "&annoProdotto=" + art.Year.ToString()
                        + "&matricola=" + matricola.ToString());
                    }
                    else if (rt == 3)
                    {
                        lbl1.Text = GetLocalResourceObject("lblProdottoP").ToString();
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorGenerico3").ToString()+": " + prcCfg.log;
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorSim").ToString();
                }
            }
            else if (art.Status == 'I' || art.Status == 'P')
            {
                lblErr.Text = GetLocalResourceObject("lblProdottoI").ToString()+".<br/>"
                    + GetLocalResourceObject("lblAccDataFineProd").ToString()
                    + ": " + art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                if(art.DataPrevistaFineProduzione <= art.DataPrevistaConsegna && art.DataPrevistaFineProduzione>=DateTime.Now)
                {
                    int ret = art.SpostaPianificazione(art.DataPrevistaFineProduzione, art.DataPrevistaConsegna);
                    if (ret == 1)
                    {
                        Response.Redirect("wzImpostaAllarmiArticolo.aspx?idCommessa=" + art.Commessa.ToString()
                        + "&annoCommessa=" + art.AnnoCommessa.ToString()
                        + "&idProc=" + art.Proc.process.processID.ToString()
                        + "&revProc=" + art.Proc.process.revisione.ToString()
                        + "&idVariante=" + art.Proc.variant.idVariante.ToString()
                        + "&idReparto=" + art.Reparto.ToString()
                        + "&idProdotto=" + art.ID.ToString()
                        + "&annoProdotto=" + art.Year.ToString()
                        + "&matricola=" + matricola.ToString());
                    }
                    else if (ret == 0)
                    {
                        lblErr.Text = GetLocalResourceObject("lblErroreGenerico4").ToString();
                    }
                    else if (ret == 2)
                    {
                        lblErr.Text = GetLocalResourceObject("lblErroreGenerico4").ToString();
                    }
                    else if (ret == 3)
                    {
                        lblErr.Text = GetLocalResourceObject("lblErroreGenerico5").ToString();
                    }
                    else if (ret == 4)
                    {
                        lblErr.Text = GetLocalResourceObject("lblErroreGenerico4").ToString();
                    }
                }
                else
                {
                    lblErr.Text = GetLocalResourceObject("lblErroreGenerico6").ToString();
                }
            }

        }
    }
}