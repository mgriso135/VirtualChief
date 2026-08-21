using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Reparti
{
    public partial class showCalendarFesteStraordinari : System.Web.UI.Page
    {
        public static int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            /*crt.Visible = false;
            txtDateEnd.Visible = false;
            txtDateStart.Visible = false;
            btnShowCalendar.Visible = false;
            */idReparto = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                try
                {
                    idReparto = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    idReparto = -1;
                }

                if (idReparto != -1)
                {
                    Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
                    if (rp.id != -1)
                    {/*
                        crt.Visible = true;
                        txtDateEnd.Visible = true;
                        txtDateStart.Visible = true;
                        btnShowCalendar.Visible = true;*/
                        if (!Page.IsPostBack)
                        {
                            lblNomeRep.Text = rp.name;
                            DateTime inCal = DateTime.Now;
                            DateTime fiCal = DateTime.Now.AddDays(7);
                            rp.loadCalendario(inCal, fiCal);
                            loadCalendario(inCal, fiCal, rp);
                        }
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

        protected void loadCalendario(DateTime inizioCal, DateTime fineCal, Reparto rp)
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

            for (int i = 0; i < rp.CalendarioRep.Turni.Count; i++)
            {
                if (rp.CalendarioRep.Turni[i].Inizio >= inizioCal && rp.CalendarioRep.Turni[i].Fine <= fineCal)
                {
                    crt.Series.Add("Turno" + i.ToString());
                    //crt.Series["Turno" + i.ToString()].AxisLabel = "Turni";
                    crt.Series["Turno" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Turno" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Turno" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                    crt.Series["Turno" + i.ToString()].BorderWidth = 20;
                    IntervalloLavorativoTurno intTr = new IntervalloLavorativoTurno(Session["ActiveWorkspace_Name"].ToString(), rp.CalendarioRep.Turni[i].idOrarioTurno);
                    Turno tr = new Turno(Session["ActiveWorkspace_Name"].ToString(), intTr.idTurno);
                    crt.Series["Turno" + i.ToString()].Color = tr.Colore;
                    crt.Series["Turno" + i.ToString()].BorderColor = tr.Colore;
                    crt.Series["Turno" + i.ToString()].Points.AddXY(rp.CalendarioRep.Turni[i].Inizio.ToOADate(), 4);
                    crt.Series["Turno" + i.ToString()].Points[0].ToolTip = tr.Nome + " "+GetLocalResourceObject("lblInizio").ToString()+": " + rp.CalendarioRep.Turni[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Turno" + i.ToString()].Points.AddXY(rp.CalendarioRep.Turni[i].Fine.ToOADate(), 4);
                    crt.Series["Turno" + i.ToString()].Points[1].ToolTip = tr.Nome + " " + GetLocalResourceObject("lblFine").ToString() + ": " + rp.CalendarioRep.Turni[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                }
            }
            ElencoStraordinari elStraord = new ElencoStraordinari(Session["ActiveWorkspace_Name"].ToString(), rp.id);

            for (int i = 0; i < elStraord.Straordinari.Count; i++)
            {
                if (elStraord.Straordinari[i].Inizio >= inizioCal && elStraord.Straordinari[i].Inizio <= fineCal)
                {
                    crt.Series.Add("Straordinario" + i.ToString());
                    crt.Series["Straordinario" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Straordinario" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Straordinario" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Auto;
                    crt.Series["Straordinario" + i.ToString()].BorderWidth = 20;
                    crt.Series["Straordinario" + i.ToString()].Color = System.Drawing.Color.Green;
                    crt.Series["Straordinario" + i.ToString()].Points.AddXY(elStraord.Straordinari[i].Inizio.ToOADate(), 3);
                    crt.Series["Straordinario" + i.ToString()].Points[0].ToolTip = GetLocalResourceObject("lblStraordinarioInizio").ToString()+ ": " + elStraord.Straordinari[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Straordinario" + i.ToString()].Points.AddXY(elStraord.Straordinari[i].Fine.ToOADate(), 3);
                    crt.Series["Straordinario" + i.ToString()].Points[1].ToolTip = GetLocalResourceObject("lblStraordinarioFine").ToString() + ": " + elStraord.Straordinari[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                    DateTime fin = elStraord.Straordinari[i].Fine;
                    if (elStraord.Straordinari[i].Fine >= fineCal)
                    {
                        fin = fineCal;
                    }
                    crt.Series["Straordinario" + i.ToString()].Points[1].ToolTip = GetLocalResourceObject("lblStraordinarioFine").ToString() + ": " + fin.ToString("dd/MM/yyyy HH:mm:ss");
                }

            }


            ElencoFestivita elFest = new ElencoFestivita(Session["ActiveWorkspace_Name"].ToString(), rp.id);
            for (int i = 0; i < elFest.feste.Count; i++)
            {
                if (elFest.feste[i].Inizio >= inizioCal && elFest.feste[i].Inizio <= fineCal)
                {
                    crt.Series.Add("Festivita" + i.ToString());
                    crt.Series["Festivita" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Festivita" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Festivita" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Auto;
                    crt.Series["Festivita" + i.ToString()].BorderWidth = 20;
                    crt.Series["Festivita" + i.ToString()].Color = System.Drawing.Color.Red;
                    crt.Series["Festivita" + i.ToString()].Points.AddXY(elFest.feste[i].Inizio.ToOADate(), 2);
                    crt.Series["Festivita" + i.ToString()].Points[0].ToolTip = GetLocalResourceObject("lblFestivitaInizio").ToString()+": " + elFest.feste[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Festivita" + i.ToString()].Points.AddXY(elFest.feste[i].Fine.ToOADate(), 2);
                    DateTime fin = elFest.feste[i].Fine;
                    if (elFest.feste[i].Fine >= fineCal)
                    {
                        fin = fineCal;
                    }
                    crt.Series["Festivita" + i.ToString()].Points[1].ToolTip = GetLocalResourceObject("lblFestivitaFine").ToString() + ": " + fin.ToString("dd/MM/yyyy HH:mm:ss");
                }
            }


            for (int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
            {

                if (rp.CalendarioRep.Intervalli[i].Inizio >= inizioCal && rp.CalendarioRep.Intervalli[i].Inizio <= fineCal)
                {
                    crt.Series.Add("Intervallo" + i.ToString());
                    crt.Series["Intervallo" + i.ToString()].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    crt.Series["Intervallo" + i.ToString()].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    crt.Series["Intervallo" + i.ToString()].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Auto;
                    crt.Series["Intervallo" + i.ToString()].BorderWidth = 20;
                    crt.Series["Intervallo" + i.ToString()].Color = System.Drawing.Color.GreenYellow;
                    crt.Series["Intervallo" + i.ToString()].Points.AddXY(rp.CalendarioRep.Intervalli[i].Inizio.ToOADate(), 1);
                    crt.Series["Intervallo" + i.ToString()].Points[0].ToolTip = GetLocalResourceObject("lblInizio").ToString()+": " + rp.CalendarioRep.Intervalli[i].Inizio.ToString("dd/MM/yyyy HH:mm:ss");
                    crt.Series["Intervallo" + i.ToString()].Points.AddXY(rp.CalendarioRep.Intervalli[i].Fine.ToOADate(), 1);
                    crt.Series["Intervallo" + i.ToString()].Points[1].ToolTip = GetLocalResourceObject("lblFine").ToString() + ": " + rp.CalendarioRep.Intervalli[i].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                    DateTime fin = rp.CalendarioRep.Intervalli[i].Fine;
                    if (rp.CalendarioRep.Intervalli[i].Fine >= fineCal)
                    {
                        fin = fineCal;
                    }
                    crt.Series["Intervallo" + i.ToString()].Points[1].ToolTip = GetLocalResourceObject("lblFine").ToString() + ": " + fin.ToString("dd/MM/yyyy HH:mm:ss");
                }
            }
        }

        protected void btnShowCalendar_Click(object sender, ImageClickEventArgs e)
        {
            DateTime start;
            DateTime end;
            String inizio = txtDateStart.Text;
            String fine = txtDateEnd.Text;
            try
            {
                String[] aInizio= inizio.Split('/');
                String[] aFine=fine.Split('/');
                int iYY, iMM, iGG;
                int fYY, fMM, fGG;
                iGG = Int32.Parse(aInizio[0]);
                iMM = Int32.Parse(aInizio[1]);
                iYY = Int32.Parse(aInizio[2]);
                fGG = Int32.Parse(aFine[0]);
                fMM = Int32.Parse(aFine[1]);
                fYY = Int32.Parse(aFine[2]);
                start = new DateTime(iYY, iMM, iGG);
                end = new DateTime(fYY, fMM, fGG);
            }
            catch
            {
                start = DateTime.Now;
                end = DateTime.Now.AddDays(7);
            }


            start = start > end ? DateTime.Now : start;

            Reparto rp = new Reparto(Session["ActiveWorkspace_Name"].ToString(), idReparto);
            rp.loadCalendario(start, end);
            this.loadCalendario(start, end, rp);
        }

    }
}