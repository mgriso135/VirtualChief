using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS.App_Code;

namespace KIS.Postazioni
{
    public partial class viewCalendarioPostazione : System.Web.UI.UserControl
    {
        public int idPostazione;
        public static DateTime Inizio, Fine;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Calendario Postazione";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                Postazione pst = new Postazione(idPostazione);
                if (pst.id != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        Inizio = DateTime.UtcNow;
                        Fine = DateTime.UtcNow.AddDays(7);
                        pst.loadCalendario(Inizio, Fine);
                        loadCal(Inizio, Fine, pst);
                    }
                }
                else
                {
                    wlPostazione.Visible = false;
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                wlPostazione.Visible = false;
                frmCalInizioFine.Visible = false;
            }
        }

        protected void loadCal(DateTime start, DateTime end, Postazione p)
        {
            if (start > end)
            {
                DateTime swap = start;
                start = end;
                end = swap;
            }
            
            wlPostazione.Series.Clear();
            wlPostazione.Titles.Clear();
            wlPostazione.Titles.Add(p.name);
            wlPostazione.ChartAreas.Add("area");
            wlPostazione.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            wlPostazione.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            wlPostazione.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM HH:mm";

            wlPostazione.ChartAreas[0].AxisY.LineWidth = 0;
            wlPostazione.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            wlPostazione.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            wlPostazione.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            wlPostazione.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
            wlPostazione.ChartAreas[0].AxisY.MinorTickMark.Enabled = false;

            for (int i = 0; i < p.Calendario.Intervalli.Count; i++)
            {
                if (p.Calendario.Intervalli[i].Inizio >= Inizio && p.Calendario.Intervalli[i].Inizio <=Fine)
                {
                    wlPostazione.Series.Add("int" + i.ToString());
                    wlPostazione.Series["int" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    wlPostazione.Series["int" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    wlPostazione.Series["int" + i.ToString()].BorderWidth = 20;
                    wlPostazione.Series["int" + i.ToString()].Color = System.Drawing.Color.Green;
                    wlPostazione.Series["int" + i.ToString()].Points.AddXY(p.Calendario.Intervalli[i].Inizio.ToOADate(), 50);
                    DateTime f2;
                    if (p.Calendario.Intervalli[i].Fine <= Fine)
                    {
                        f2 = p.Calendario.Intervalli[i].Fine;
                    }
                    else
                    {
                        f2 = Fine;
                    }
                    wlPostazione.Series["int" + i.ToString()].Points.AddXY(f2.ToOADate(), 50);
                    Reparto rp = new Reparto(p.Calendario.Intervalli[i].idReparto);
                    wlPostazione.Series["int" + i.ToString()].ToolTip = GetLocalResourceObject("lblReparto").ToString() + " " + rp.name + " " + p.Calendario.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm") + " - " + p.Calendario.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm");
                }
            }

            // Carico i task di produzione
            p.Calendario.loadTasksProduzione();
            
            for (int i = 0; i < p.Calendario.IntervalliTaskProduzione.Count; i++)
            {
                wlPostazione.Series.Add("tsk" + i.ToString());
                wlPostazione.Series["tsk" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                wlPostazione.Series["tsk" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                wlPostazione.Series["tsk" + i.ToString()].BorderWidth = 10;
                wlPostazione.Series["tsk" + i.ToString()].Points.AddXY(p.Calendario.IntervalliTaskProduzione[i].Inizio.ToOADate(), 10);
                DateTime f2;
                if (p.Calendario.IntervalliTaskProduzione[i].Fine <= Fine)
                {
                    f2 = p.Calendario.IntervalliTaskProduzione[i].Fine;
                }
                else
                {
                    f2 = Fine;
                }
                wlPostazione.Series["tsk" + i.ToString()].Points.AddXY(f2.ToOADate(), 10);
                //Reparto rp = new Reparto(p.Calendario.IntervalliTaskProduzione[i].idReparto);
                TaskProduzione tsk = new TaskProduzione(p.Calendario.IntervalliTaskProduzione[i].TaskProduzioneID);
                Articolo art = new Articolo(tsk.ArticoloID, tsk.ArticoloAnno);
                Commessa cm = new Commessa(art.Commessa, art.AnnoCommessa);
                wlPostazione.Series["tsk" + i.ToString()].ToolTip = GetLocalResourceObject("lblOrdine").ToString()+" " + cm.ID + " " + cm.Cliente + " Articolo: " + art.ID + " " + art.Matricola + " Task: " + tsk.Name + " " + p.Calendario.IntervalliTaskProduzione[i].Inizio.ToString("dd/MM/yyyy HH:mm") + " - " + p.Calendario.IntervalliTaskProduzione[i].Fine.ToString("dd/MM/yyyy HH:mm");
            }
        }

        protected void btnUpdateCalendar_Click(object sender, ImageClickEventArgs e)
        {
            String sInizio, sFine;
            sInizio = txtCalInizio.Text;
            sFine = txtCalFine.Text;
            try
            {
                String[] aInizio = sInizio.Split('/');
                int iGG = Int32.Parse(aInizio[0]);
                int iMM = Int32.Parse(aInizio[1]);
                int iYY = Int32.Parse(aInizio[2]);
                Inizio = new DateTime(iYY, iMM, iGG);

                String[] aFine = sFine.Split('/');
                int fGG = Int32.Parse(aFine[0]);
                int fMM = Int32.Parse(aFine[1]);
                int fYY = Int32.Parse(aFine[2]);
                Fine = new DateTime(fYY, fMM, fGG);
            }
            catch
            {
                Inizio = DateTime.Now;
                Fine = DateTime.Now.AddDays(7);
            }

            if (Inizio < Fine)
            {
                Postazione p = new Postazione(idPostazione);
                p.loadCalendario(Inizio, Fine);
                loadCal(Inizio, Fine, p);
            }
        }
    }
}