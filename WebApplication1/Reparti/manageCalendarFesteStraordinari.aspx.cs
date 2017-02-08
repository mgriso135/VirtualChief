using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Reparti
{
    public partial class manageCalendarFesteStraordinari : System.Web.UI.Page
    {
        public static int idTurno;
        public static DateTime inCal, fiCal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    idTurno = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    idTurno = -1;
                }

                if (idTurno != -1)
                {
                    Turno trn = new Turno(idTurno);
                    Reparto rp = new Reparto(trn.idReparto);
                    if (rp.id != -1 && trn.id!=-1)
                    {
                        
                        if (!Page.IsPostBack)
                        {
                            lnkFestivita.NavigateUrl += trn.id.ToString();
                            lnkStraordinario.NavigateUrl += trn.id.ToString();
                            lblNomeRep.Text = rp.name + " - " + trn.Nome;
                            inCal = DateTime.Now;
                            fiCal = DateTime.Now.AddDays(7);
                        }
                        trn.loadCalendario(inCal, fiCal);
                        loadCalendario(inCal, fiCal, trn);
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
        }

        protected void loadCalendario(DateTime inizioCal, DateTime fineCal, Turno trn)
        {
            crt.Series.Clear();
            crt.Titles.Clear();
            crt.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            crt.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            crt.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM HH:mm";

            crt.ChartAreas[0].AxisY.LineWidth = 0;
            crt.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            crt.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            crt.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            crt.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
            crt.ChartAreas[0].AxisY.MinorTickMark.Enabled = false;

            for (int i = 0; i < trn.CalendarioTrn.Turni.Count; i++)
            {
                if (trn.CalendarioTrn.Turni[i].Inizio >= inizioCal && trn.CalendarioTrn.Turni[i].Fine <= fineCal)
                {
                    crt.Series.Add("Turno" + i.ToString());
                    //crt.Series["Turno" + i.ToString()].AxisLabel = "Turni";
                    crt.Series["Turno" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Turno" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Turno" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                    crt.Series["Turno" + i.ToString()].BorderWidth = 20;
                    IntervalloLavorativoTurno intTr = new IntervalloLavorativoTurno(trn.CalendarioTrn.Turni[i].idOrarioTurno);
                    Turno tr = new Turno(intTr.idTurno);
                    crt.Series["Turno" + i.ToString()].Color = tr.Colore;
                    crt.Series["Turno" + i.ToString()].BorderColor = tr.Colore;
                    crt.Series["Turno" + i.ToString()].Points.AddXY(trn.CalendarioTrn.Turni[i].Inizio.ToOADate(), 4);
                    crt.Series["Turno" + i.ToString()].Points[0].ToolTip = tr.Nome + " inizio: " + trn.CalendarioTrn.Turni[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Turno" + i.ToString()].Points.AddXY(trn.CalendarioTrn.Turni[i].Fine.ToOADate(), 4);
                    crt.Series["Turno" + i.ToString()].Points[1].ToolTip = tr.Nome + " fine: " + trn.CalendarioTrn.Turni[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                }
            }

                trn.loadStraordinari();
            for (int i = 0; i < trn.straordinari.Count; i++)
            {
                if (trn.straordinari[i].Inizio >= inizioCal && trn.straordinari[i].Inizio <= fineCal)
                {
                    crt.Series.Add("Straordinario" + i.ToString());
                    crt.Series["Straordinario" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Straordinario" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Straordinario" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Auto;
                    crt.Series["Straordinario" + i.ToString()].BorderWidth = 20;
                    crt.Series["Straordinario" + i.ToString()].Color = System.Drawing.Color.Green;
                    crt.Series["Straordinario" + i.ToString()].Points.AddXY(trn.straordinari[i].Inizio.ToOADate(), 3);
                    crt.Series["Straordinario" + i.ToString()].Points[0].ToolTip = "Straordinario inizio: " + trn.straordinari[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Straordinario" + i.ToString()].Points.AddXY(trn.straordinari[i].Fine.ToOADate(), 3);
                    crt.Series["Straordinario" + i.ToString()].Points[1].ToolTip = "Straordinario fine: " + trn.straordinari[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                    DateTime fin = trn.straordinari[i].Fine;
                    if (trn.straordinari[i].Fine >= fineCal)
                    {
                        fin = fineCal;
                    }
                    crt.Series["Straordinario" + i.ToString()].Points[1].ToolTip = "Straordinario fine: " + fin.ToString("dd/MM/yyyy HH:mm:ss");
                }

            }


            trn.loadFestivita();
            for (int i = 0; i < trn.festivita.Count; i++)
            {
                if (trn.festivita[i].Inizio >= inizioCal && trn.festivita[i].Inizio <= fineCal)
                {
                    crt.Series.Add("Festivita" + i.ToString());
                    crt.Series["Festivita" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Festivita" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Festivita" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Auto;
                    crt.Series["Festivita" + i.ToString()].BorderWidth = 20;
                    crt.Series["Festivita" + i.ToString()].Color = System.Drawing.Color.Red;
                    crt.Series["Festivita" + i.ToString()].Points.AddXY(trn.festivita[i].Inizio.ToOADate(), 2);
                    crt.Series["Festivita" + i.ToString()].Points[0].ToolTip = "Festivita inizio: " + trn.festivita[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Festivita" + i.ToString()].Points.AddXY(trn.festivita[i].Fine.ToOADate(), 2);
                    DateTime fin = trn.festivita[i].Fine;
                    if (trn.festivita[i].Fine >= fineCal)
                    {
                        fin = fineCal;
                    }
                    crt.Series["Festivita" + i.ToString()].Points[1].ToolTip = "Festivita fine: " + fin.ToString("dd/MM/yyyy HH:mm:ss");
                }
            }


            for (int i = 0; i < trn.CalendarioTrn.Intervalli.Count; i++)
            {

                if (trn.CalendarioTrn.Intervalli[i].Inizio >= inizioCal && trn.CalendarioTrn.Intervalli[i].Inizio <= fineCal)
                {
                    crt.Series.Add("Intervallo" + i.ToString());
                    crt.Series["Intervallo" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Intervallo" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Intervallo" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Auto;
                    crt.Series["Intervallo" + i.ToString()].BorderWidth = 20;
                    crt.Series["Intervallo" + i.ToString()].Color = System.Drawing.Color.GreenYellow;
                    crt.Series["Intervallo" + i.ToString()].Points.AddXY(trn.CalendarioTrn.Intervalli[i].Inizio.ToOADate(), 1);
                    crt.Series["Intervallo" + i.ToString()].Points[0].ToolTip = "Inizio: " + trn.CalendarioTrn.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Intervallo" + i.ToString()].Points.AddXY(trn.CalendarioTrn.Intervalli[i].Fine.ToOADate(), 1);
                    crt.Series["Intervallo" + i.ToString()].Points[1].ToolTip = "Fine: " + trn.CalendarioTrn.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                    DateTime fin = trn.CalendarioTrn.Intervalli[i].Fine;
                    if (trn.CalendarioTrn.Intervalli[i].Fine >= fineCal)
                    {
                        fin = fineCal;
                    }
                    crt.Series["Intervallo" + i.ToString()].Points[1].ToolTip = "Fine: " + fin.ToString("dd/MM/yyyy HH:mm:ss");
                }
            }
        }

        protected void dtStartCal_SelectionChanged(object sender, EventArgs e)
        {
            DateTime start = dtStartCal.SelectedDate;
            if (/*start >= DateTime.Now &&*/ start <= fiCal)
            {
                inCal = start;
            }
            else
            {
                inCal = DateTime.Now;
            }
            Turno trn = new Turno(idTurno);
                trn.loadCalendario(inCal, fiCal);
                this.loadCalendario(inCal, fiCal, trn);
            
        }

        protected void dtEndCal_SelectionChanged(object sender, EventArgs e)
        {
            DateTime end = dtEndCal.SelectedDate;
            if (/*end >= DateTime.Now &&*/ end >= inCal)
            {
                Turno trn = new Turno(idTurno);
                fiCal = end;
                trn.loadCalendario(inCal, fiCal);
                this.loadCalendario(inCal, fiCal,trn);
            }
        }


    }
}