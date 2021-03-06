using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Produzione
{
    public partial class wlSimReparto : System.Web.UI.UserControl
    {
        public int idReparto;
        public static List<DateTime> list = new List<DateTime>();
        public static List<int> idPostazioni = new List<int>();
        public static DateTime inizio, fine;
        public static List<Articolo> articoliNuovi = new List<Articolo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtProductDateEnd.Visible = false;
            txtProductDateStart.Visible = false;
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
                txtProductDateEnd.Visible = true;
                txtProductDateStart.Visible = true;
                if (!Page.IsPostBack)
                {
                    dvErr.Visible = false;
                    dvInfo.Visible = false;
                    articoliNuovi = new List<Articolo>();
                    chkLstPostazioni.Visible = false;
                    inizio = DateTime.Now;
                    fine = inizio;
                    Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                    rp.loadPostazioni();
                    idPostazioni = new List<int>();
                    for (int i = 0; i < rp.Postazioni.Count; i++)
                    {
                        idPostazioni.Add(rp.Postazioni[i].id);
                    }
                    Chart1.Visible = false;

                    // Carico gli articoli
                    List<Articolo> elencoStatoN = new List<Articolo>();
                    ElencoCommesse elComm = new ElencoCommesse(Session["ActiveWorkspace_Name"].ToString());
                    elComm.loadCommesse();
                    for (int i = 0; i < elComm.Commesse.Count; i++)
                    {
                        elComm.Commesse[i].loadArticoli();
                        for (int j = 0; j < elComm.Commesse[i].Articoli.Count; j++)
                        {
                            bool found = false;
                            if (elComm.Commesse[i].Articoli[j].Reparto == rp.id)
                            {
                                found = true;
                            }
                            else
                            {
                                elComm.Commesse[i].Articoli[j].Proc.loadReparto();
                                for (int q = 0; q < elComm.Commesse[i].Articoli[j].Proc.RepartiProduttivi.Count; q++)
                                {
                                    if (elComm.Commesse[i].Articoli[j].Proc.RepartiProduttivi[q].id == rp.id)
                                    {
                                        found = true;
                                    }
                                }
                            }

                            if (elComm.Commesse[i].Articoli[j].Status == 'N' && found == true)
                            {
                                elencoStatoN.Add(elComm.Commesse[i].Articoli[j]);
                            }
                        }
                    }
                    elencoStatoN.Sort(delegate(Articolo p1, Articolo p2)
                    {
                        return p1.DataPrevistaConsegna.CompareTo(p2.DataPrevistaConsegna);
                    });

                    if (elencoStatoN.Count > 0)
                    {
                        rptProdotti.DataSource = elencoStatoN;
                        rptProdotti.DataBind();
                    }
                }
                else
                {
                    if (inizio < fine)
                    {
                        caricaGrafico();
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                txtProductDateEnd.Visible = false;
                txtProductDateStart.Visible = false;
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
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                rp.loadPostazioni();
                idPostazioni = new List<int>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    lbl1.Text += GetLocalResourceObject ("lblAggiungo").ToString() + " " + rp.Postazioni[i].name + "<br />";
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
                chkLstPostazioni.Items.Clear();
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                rp.loadPostazioni();
                idPostazioni = new List<int>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    chkLstPostazioni.Items.Add(new ListItem(rp.Postazioni[i].name, rp.Postazioni[i].id.ToString()));
                    chkLstPostazioni.Items[i].Selected = true;
                    idPostazioni.Add(rp.Postazioni[i].id);
                }
                if (inizio < fine)
                {
                    caricaGrafico();
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

        protected void caricaGrafico()
        {
            dvInfo.Visible = false;
            Chart1.Series.Clear();
            if (idReparto != -1 && inizio < fine)
            {
                Chart1.Visible = true;
                Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                Chart1.Titles.Clear();
                Chart1.Titles.Add(new System.Web.UI.DataVisualization.Charting.Title(rp.name));
                
                rp.loadPostazioni();
                TimeSpan somma = new TimeSpan(0, 0, 0);
                int cont = 0;
                List<caricoDiLavoro> carichi = new List<caricoDiLavoro>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    bool found = false;
                    // Se lo trovo nell'elenco delle postazioni selezionate...
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
                    if (articoliNuovi[i] != null && articoliNuovi[i].Proc!=null && articoliNuovi[i].Proc.process!=null && articoliNuovi[i].Proc.variant!=null)
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
                            
                            ConfigurazioneProcesso cfgPrc = new ConfigurazioneProcesso(Session["ActiveWorkspace_Name"].ToString(), articoliNuovi[i], tskLst, 
                                new Reparto(Session["ActiveWorkspace_Name"].ToString(), articoliNuovi[i].Reparto), articoliNuovi[i].Quantita);
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
                                    lbl1.Text += "IF1<BR/>";
                                    cdl = cfgPrc.Processi[z].Tempo.Tempo;
                                }
                                else if (cfgPrc.Processi[z].LateStartDate <= inizio && cfgPrc.Processi[z].LateFinishDate >= inizio && cfgPrc.Processi[z].LateFinishDate <= fine)
                                {
                                    lbl1.Text += "IF2<BR/>";
                                    rp.loadCalendario(cfgPrc.Processi[z].LateStartDate.AddDays(-1), fine.AddDays(1));

                                    for (int h = 0; h < rp.CalendarioRep.Intervalli.Count; h++)
                                    {
                                        if (rp.CalendarioRep.Intervalli[h].Fine <= inizio)
                                        {
                                            // Non aggiunto niente
                                        }
                                        else if(rp.CalendarioRep.Intervalli[h].Inizio > cfgPrc.Processi[z].LateFinishDate)
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
                                    lbl1.Text += "IF3<BR/>";
                                    rp.loadCalendario(inizio.AddDays(-1), cfgPrc.Processi[z].LateFinishDate.AddDays(1));
                                    for (int h = 0; h < rp.CalendarioRep.Intervalli.Count; h++)
                                    {
                                        if (rp.CalendarioRep.Intervalli[h].Fine < cfgPrc.Processi[z].LateStartDate)
                                        {
                                            // Non aggiungo niente
                                        }
                                        else if(rp.CalendarioRep.Intervalli[h].Inizio >= fine)
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
                                        else if(rp.CalendarioRep.Intervalli[h].Inizio >= inizio && rp.CalendarioRep.Intervalli[h].Fine >= fine && cfgPrc.Processi[z].LateStartDate >= rp.CalendarioRep.Intervalli[h].Inizio && cfgPrc.Processi[z].LateFinishDate >= fine)
                                        {
                                            cdl += fine - cfgPrc.Processi[z].LateStartDate;
                                        }
                                    }
                                }
                                else if (cfgPrc.Processi[z].LateStartDate <= inizio && cfgPrc.Processi[z].LateFinishDate >= fine)
                                {
                                    lbl1.Text += "IF4<BR/>";
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
                            lbl1.Text = GetLocalResourceObject("lblErrorSim1").ToString()+" "
                                + articoliNuovi[i].ID.ToString() + "/" + articoliNuovi[i].Year.ToString()
                                +" "+ GetLocalResourceObject("lblErrorSim2").ToString();
                        }
                    }

                }


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
                            Chart1.Series[cd.articolo.ToString()].Points[ind].ToolTip = GetLocalResourceObject("lblCaricoFissato").ToString() +": " + String.Format("{0:0.00}", cd.CaricoOre) + " "+ GetLocalResourceObject("lblOre").ToString();
                        }
                        else
                        {
                            Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), cd.articolo, cd.articoloAnno);
                            Chart1.Series[cd.articolo.ToString()].Points[ind].ToolTip = art.ID.ToString() + "/" +
                                art.Year.ToString() + " - " + art.Cliente + " - " +
                                art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante +
                                " - "+ GetLocalResourceObject("lblCaricodilavoro").ToString()
                                +": " + String.Format("{0:0.00}", cd.CaricoOre.ToString()) + " "+ GetLocalResourceObject("lblOre").ToString();
                            Chart1.Series[cd.articolo.ToString()].AxisLabel = art.RepartoNome;
                        }
                        Chart1.Series[cd.articolo.ToString()].Points[ind].Label = String.Format("{0:0.00}", cd.CaricoOre) 
                            + " " + GetLocalResourceObject("lblOre").ToString();
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
                    lblTurni.Text = "";
                    double[] orePostazione = new double[idPostazioni.Count];
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
                    }

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
                    Chart1.Series.Add("risorse");
                    Chart1.Series["risorse"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                    Chart1.Series["risorse"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series["risorse"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Int32;
                    Chart1.Series["risorse"].EmptyPointStyle.IsValueShownAsLabel = false;
                    Chart1.Series["risorse"].EmptyPointStyle.IsVisibleInLegend = false;

                    for (int q = 0; q < idPostazioni.Count; q++)
                    {
                        lbl1.Text += "Postazione: " + q.ToString() + "<br />";
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
                            .Select(g => new {
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
                                        Chart1.Series[cd.articolo.ToString()].Points[indice].Label = String.Format("{0:0.00}", cd.CaricoOre) + " ore";
                                        Chart1.Series[cd.articolo.ToString()].Points[indice].AxisLabel = p.name;
                                    //lbl1.Text += q.ToString() + " " + Chart1.Series[cd.articolo.ToString()].Points[ind].XValue.ToString() + " " +
                                    //    Chart1.Series[cd.articolo.ToString()].Points[ind].YValues[0].ToString() + "<br />";

                                    if (cd.articolo == -1)
                                    {
                                        Chart1.Series[cd.articolo.ToString()].Points[indice].ToolTip = GetLocalResourceObject("lblCaricoFissato").ToString()+ ": " + String.Format("{0:0.00}", cd.CaricoOre) + " "+ GetLocalResourceObject("lblOre").ToString();
                                    }
                                    else
                                    {
                                        Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), cd.articolo, cd.articoloAnno);
                                        Chart1.Series[cd.articolo.ToString()].Points[indice].ToolTip = art.ID.ToString() + "/" +
                                            art.Year.ToString() + " - " + art.Cliente + " - " +
                                            art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante +
                                            " "+GetLocalResourceObject("lblPostazione").ToString()+" " + p.name +
                                            " - "+ GetLocalResourceObject("lblCaricodilavoro").ToString() + ": " + String.Format("{0:0.00}", cd.CaricoOre) + " "+GetLocalResourceObject("lblOre").ToString();
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
                            Chart1.Series["risorse"].Points[indRes].Label = GetLocalResourceObject("lblLimiteCapacita").ToString()+ " " + p.name + ": " + String.Format(orePostazione[q].ToString()) + " "+GetLocalResourceObject("lblOre").ToString();
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
                lbl1.Text = GetLocalResourceObject("lblcaricocomplessivo").ToString() + " "
                    + rp.name 
                    + " "+GetLocalResourceObject("lblNelPeriodo").ToString()+" " + inizio.ToString("dd/MM/yyyy") 
                    + " - " + fine.ToString("dd/MM/yyyy") + ": <b>"
                    + String.Format("{0:0.00}", somma.TotalHours) + " "+GetLocalResourceObject("lblOre").ToString()+"</b><br />";
            }
        }

        protected void rptProdotti_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField itmID = (HiddenField)e.Item.FindControl("itmID");
                HiddenField itmYear = (HiddenField)e.Item.FindControl("itmYear");
                DropDownList ddlOre = (DropDownList)e.Item.FindControl("calOre");
                DropDownList ddlMin = (DropDownList)e.Item.FindControl("calMinuti");
                DropDownList ddlSec = (DropDownList)e.Item.FindControl("calSecondi");
                TextBox txtProductDate = (TextBox)e.Item.FindControl("txtProductDate");
                CheckBox chk = (CheckBox)e.Item.FindControl("chk");
                int ore, min, sec, gg, mm, yy;
                int prdID = -1;
                int prdYear = -1;
                String[] datafineprod = new String[3];
                datafineprod = (txtProductDate.Text).Split('/');
                DateTime finePrd;
                lbl1.Text = datafineprod[0] + " " + datafineprod[1] + " " + datafineprod[2];
                try
                {
                    prdID = Int32.Parse(itmID.Value);
                    prdYear = Int32.Parse(itmYear.Value);
                    ore = Int32.Parse(ddlOre.SelectedValue);
                    min = Int32.Parse(ddlMin.SelectedValue);
                    sec = Int32.Parse(ddlSec.SelectedValue);
                    gg = Int32.Parse(datafineprod[0]);
                    mm = Int32.Parse(datafineprod[1]);
                    yy = Int32.Parse(datafineprod[2]);

                    finePrd = new DateTime(yy, mm, gg, ore, min, sec);
                    lbl1.Text += finePrd.ToString("dd/mm/yyyy HH:mm:ss");
                }
                catch
                {
                    prdID = -1;
                    prdYear = -1;
                    ore =-1;
                    min =-1;
                    sec=-1;
                    gg = -1;
                    mm = -1;
                    yy = -1;
                    finePrd = new DateTime(1970, 1, 1);
                }
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), prdID, prdYear);
                if (finePrd >= DateTime.Now && finePrd <=art.DataPrevistaConsegna)
                {
                    dvErr.Visible = false;
                    lblErr.Visible = false;
                    lbl1.Text += finePrd.ToString("dd/MM/yyyy HH:mm:ss");
                    art.DataPrevistaFineProduzione = finePrd;
                    caricaGrafico();
                }
                else
                {
                    dvErr.Visible = true;
                    lblErr.Visible = true;
                    txtProductDate.Text = art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy");
                lblErr.Text = GetLocalResourceObject("lblErrorModData").ToString()+ "<br />";
                }
        }

        protected void rptProdotti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField itmID = (HiddenField)e.Item.FindControl("itmID");
                HiddenField itmYear = (HiddenField)e.Item.FindControl("itmYear");
                DropDownList ddlOre = (DropDownList)e.Item.FindControl("calOre");
                DropDownList ddlMin = (DropDownList)e.Item.FindControl("calMinuti");
                DropDownList ddlSec = (DropDownList)e.Item.FindControl("calSecondi");
                TextBox txtProductDate = (TextBox)e.Item.FindControl("txtProductDate");
                CheckBox chk = (CheckBox)e.Item.FindControl("chk");

                int prdID = -1;
                int prdYear = -1;

                try
                {
                    prdID = Int32.Parse(itmID.Value);
                    prdYear = Int32.Parse(itmYear.Value);
                }
                catch
                {
                    prdID = -1;
                    prdYear = -1;
                }
                if (prdID != -1 && prdYear != -1)
                {
                    Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), prdID, prdYear);
                    for (int i = 0; i < 24; i++)
                    {
                        ddlOre.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    for (int i = 0; i < 60; i++)
                    {
                        ddlMin.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        ddlSec.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    if (art.DataPrevistaFineProduzione != new DateTime(1970, 1, 1))
                    {
                        txtProductDate.Text = art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy");
                        ddlOre.SelectedValue = art.DataPrevistaFineProduzione.Hour.ToString();
                        ddlMin.SelectedValue = art.DataPrevistaFineProduzione.Minute.ToString();
                        ddlSec.SelectedValue = art.DataPrevistaFineProduzione.Second.ToString();
                    }

                    bool found = false;
                    for (int i = 0; i < articoliNuovi.Count; i++)
                    {
                        if (articoliNuovi[i].ID == art.ID && articoliNuovi[i].Year == art.Year)
                        {
                            found = true;
                        }
                    }

                    if (found == true)
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        chk.Checked = false;
                    }
                }
            }
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ck = (CheckBox)sender;
            HiddenField hdID = (HiddenField)ck.Parent.FindControl("itmID");
            HiddenField hdYear = (HiddenField)ck.Parent.FindControl("itmYear");
            int artID, artYear;
            try
            {
                artID = Int32.Parse(hdID.Value);
                artYear = Int32.Parse(hdYear.Value);
            }
            catch
            {
                artID = -1;
                artYear = -1;
            }
            if (artID != -1 && artYear != -1)
            {
                if (articoliNuovi == null)
                {
                    articoliNuovi = new List<Articolo>();
                }
                Articolo art = new Articolo(Session["ActiveWorkspace_Name"].ToString(), artID, artYear);
                if (ck.Checked == true)
                {
                    art.Reparto = idReparto;
                    articoliNuovi.Add(art);
                }
                else
                {
                    for (int i = 0; i < articoliNuovi.Count; i++)
                    {
                        if (articoliNuovi[i].ID == art.ID && articoliNuovi[i].Year == art.Year)
                        {
                            articoliNuovi.RemoveAt(i);
                        }
                    }
                    art.Reparto = -1;
                }
                caricaGrafico();
            }
        }

        protected struct caricoDiLavoro
        {
            public Postazione postazione;
            public int articolo;
            public int articoloAnno;
            public TimeSpan carico;
            public double caricoOre
            {
                get
                {
                    return carico.TotalHours;
                }
            }

            public double CaricoOre;
            public bool DaProgrammare;

            public System.Drawing.Color colore
            {
                get
                {
                    if (DaProgrammare == false)
                    {
                        return System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        return System.Drawing.Color.FromArgb((articolo * articoloAnno) % 255, (Math.Abs(articoloAnno - articolo)) % 255, (articoloAnno/(articolo+1)) % 255);
                    }
                }
            }
        }

        protected void btnUpdateDate_Click(object sender, ImageClickEventArgs e)
        {
            String sInizio, sFine;
            sInizio = txtProductDateStart.Text;
            sFine = txtProductDateEnd.Text;
            try
            {
                String[] aInizio = sInizio.Split('/');
                int iGG = Int32.Parse(aInizio[0]);
                int iMM = Int32.Parse(aInizio[1]);
                int iYY = Int32.Parse(aInizio[2]);
                inizio = new DateTime(iYY, iMM, iGG);

                String[] aFine = sFine.Split('/');
                int fGG = Int32.Parse(aFine[0]);
                int fMM = Int32.Parse(aFine[1]);
                int fYY = Int32.Parse(aFine[2]);
                fine = new DateTime(fYY, fMM, fGG);
            }
            catch
            {
                inizio = DateTime.Now;
                fine = DateTime.Now.AddDays(7);
            }

            if (inizio < fine)
            {
                caricaGrafico();
            }
        }

    }
}