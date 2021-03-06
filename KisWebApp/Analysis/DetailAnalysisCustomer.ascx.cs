using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using System.Globalization;
using KIS.App_Sources;

namespace KIS.Analysis
{
    public partial class DetailAnalysisCustomer1 : System.Web.UI.UserControl
    {
        public static Cliente customer;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Clienti";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace_Name"].ToString(), elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    lnkDays.Visible = false;
                    lnkMonths.Visible = false;
                    Chart1.Visible = false;
                 String customerID = Request.QueryString["customerID"];
                customer = new Cliente(Session["ActiveWorkspace_Name"].ToString(), customerID);
                if (customer != null && customer.CodiceCliente.Length > 0)
                {

                }
                else
                {
                    Chart1.Visible = false;
                    rptIntervalliDiLavoro.Visible = false;
                    rptMonths.Visible = false;
                }
                }
            }
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            lnkMonths.Visible = false;
            lnkDays.Visible = false;
            Chart1.Visible = false;
            rptIntervalliDiLavoro.Visible = false;
            rptMonths.Visible = false;

            int ggI, ggF, mmI, mmF, yyI, yyF;
            String[] dataI = new String[3];
            String[] dataF = new String[3];
            dataI = (txtDateStart.Text).Split('/');
            dataF = (txtDateEnd.Text).Split('/');
            DateTime startPeriod = new DateTime(1970, 1, 1);
            DateTime endPeriod = new DateTime(1970, 1, 1);

            try
            {
                ggI = Int32.Parse(dataI[0]);
                mmI = Int32.Parse(dataI[1]);
                yyI = Int32.Parse(dataI[2]);

                ggF = Int32.Parse(dataF[0]);
                mmF = Int32.Parse(dataF[1]);
                yyF = Int32.Parse(dataF[2]);

                startPeriod = new DateTime(yyI, mmI, ggI);
                endPeriod = new DateTime(yyF, mmF, ggF);

                if (startPeriod > endPeriod)
                {
                    startPeriod = new DateTime(1970, 1, 1);
                    endPeriod = new DateTime(1970, 1, 1);
                }
            }
            catch
            {
                startPeriod = new DateTime(1970, 1, 1);
                endPeriod = new DateTime(1970, 1, 1);
            }

            if (startPeriod < endPeriod && startPeriod > new DateTime(1970, 1, 1))
            {
                if (customer != null && customer.CodiceCliente.Length > 0)
                {
                    customer.loadIntervalliDiLavoro(startPeriod, endPeriod);
                    var intervSorted = customer.IntervalliDiLavoro.OrderBy(x => x.DataInizio);
                    rptIntervalliDiLavoro.DataSource = intervSorted;
                    rptIntervalliDiLavoro.DataBind();
                    loadChartDay();

                    customer.loadTempoDiLavoro(startPeriod, endPeriod);
                    lblMonths.Visible = true;
                    lblMonths.Text = Math.Round(customer.TempoDiLavoro.TotalHours,2).ToString();

                    lnkMonths.Visible = true;
                    lnkDays.Visible = true;
                    Chart1.Visible = true;
                    rptMonths.Visible = true;
                }
            }
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {
         //   loadChartDay();
        }

        public void loadChartWeek()
        {
            if (customer != null && customer.IntervalliDiLavoro != null && customer.IntervalliDiLavoro.Count > 0)
            {
                Chart1.Series.Clear();
                Chart1.Series.Add("interv");
                Chart1.Series["interv"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                var intervalliGrouped = from t in customer.IntervalliDiLavoro
                                        group t by new { t.DataInizio.Year, t.DataInizio.Month} into ut
                                        select new
                                        {
                                            Data = new DateTime(ut.Key.Year, ut.Key.Month, 1),
                                            Year = ut.Key.Year,
                                            Month = ut.Key.Month,
                                            TempoDiLavoro = (Double)ut.Sum(t => t.DurataIntervallo.TotalHours)
                                        };

                var intervSorted = intervalliGrouped.OrderBy(x => x.Data);
                Chart1.Series["interv"].XValueMember = "Data";
                Chart1.Series["interv"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                Chart1.Series["interv"].YValueMembers = "TempoDiLavoro";
                Chart1.Series["interv"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                Chart1.Series["interv"].ToolTip = "#VALX: #VALY ore";
                Chart1.DataSource = intervSorted.ToList();
                int qtaIntervalli = intervSorted.ToList().Count * 30;
                if (qtaIntervalli < 600)
                {
                    qtaIntervalli = 600;
                }
                Chart1.Width = (qtaIntervalli);
                Chart1.DataBind();
                Chart1.Visible = true;

                lnkMonths.Visible = true;
                lnkDays.Visible = true;
            }
        }

        public void loadChartDay()
        {
            if (customer != null && customer.IntervalliDiLavoro!= null && customer.IntervalliDiLavoro.Count > 0)
            {
                Chart1.Series.Clear();
                Chart1.Series.Add("interv");
                Chart1.Series["interv"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                var intervalliGrouped = from t in customer.IntervalliDiLavoro
                                        group t by new { t.DataInizio.Year, t.DataInizio.Month, t.DataInizio.Day } into ut
                                        select new
                                        {
                                            Data = new DateTime(ut.Key.Year, ut.Key.Month, ut.Key.Day),
                                            Year = ut.Key.Year,
                                            Month = ut.Key.Month,
                                            Day = ut.Key.Day,
                                            DayOfYear = ut.Select( x=> x.DataInizio.DayOfYear).ToString(),
                                            TempoDiLavoro = (Double)ut.Sum(t => t.DurataIntervallo.TotalHours)
                                        };
                //Chart1.Visible = true;

                var intervSorted = intervalliGrouped.OrderBy(x => x.Data);
                Chart1.Series["interv"].XValueMember = "Data";
                Chart1.Series["interv"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                Chart1.Series["interv"].YValueMembers = "TempoDiLavoro";
                Chart1.Series["interv"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                Chart1.Series["interv"].ToolTip = "#VALX: #VALY " + GetLocalResourceObject("lblOre");
                Chart1.DataSource = intervSorted.ToList();
                int qtaIntervalli = intervSorted.ToList().Count * 30;
                if (qtaIntervalli < 600)
                {
                    qtaIntervalli = 600;
                }
                Chart1.Width = (qtaIntervalli);
                Chart1.DataBind();
                Chart1.Visible = true;
                rptMonths.DataSource = intervSorted;
                rptMonths.DataBind();

                lnkMonths.Visible = true;
                lnkDays.Visible = true;
            }
        }

        public void loadChartMonth()
        {
            if (customer != null && customer.IntervalliDiLavoro != null && customer.IntervalliDiLavoro.Count > 0)
            {
                Chart1.Series.Clear();
                Chart1.Series.Add("interv");
                Chart1.Series["interv"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                var intervalliGrouped = from t in customer.IntervalliDiLavoro
                                        group t by new { t.DataInizio.Year, t.DataInizio.Month } into ut
                                        select new
                                        {
                                            Data = new DateTime(ut.Key.Year, ut.Key.Month,1),
                                            DataString = ut.Key.Month.ToString() + "/" + ut.Key.Year.ToString(),
                                            Year = ut.Key.Year,
                                            Month = ut.Key.Month,
                                            TempoDiLavoro = (Double)ut.Sum(t => t.DurataIntervallo.TotalHours)
                                        };
                //Chart1.Visible = true;

                var intervSorted = intervalliGrouped.OrderBy(x => x.Data);
                Chart1.Series["interv"].XValueMember = "DataString";
                Chart1.Series["interv"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Auto;
                Chart1.Series["interv"].YValueMembers = "TempoDiLavoro";
                Chart1.Series["interv"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                Chart1.Series["interv"].ToolTip = "#VALX: #VALY " + GetLocalResourceObject("lblOre").ToString();
                Chart1.DataSource = intervSorted.ToList();
                int qtaIntervalli = intervSorted.ToList().Count * 30;
                if (qtaIntervalli < 600)
                {
                    qtaIntervalli = 600;
                }
                Chart1.Width = (qtaIntervalli);
                Chart1.DataBind();
                Chart1.Visible = true;
                rptMonths.DataSource = intervSorted;
                rptMonths.DataBind();

                lnkMonths.Visible = true;
                lnkDays.Visible = true;
            }
        }

        protected void lnkDays_Click(object sender, EventArgs e)
        {
            loadChartDay();
        }

        protected void lnkMonths_Click(object sender, EventArgs e)
        {
            loadChartMonth();
        }
    }
}