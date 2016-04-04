using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;

namespace KIS.kpi
{
    public partial class showKPIRecords : System.Web.UI.UserControl
    {
        public int kpiID;
        private static bool startDateChanged;
        private static bool endDateChanged;
        private static DateTime start;
        private static DateTime end;
        private static processo proc;

        protected void Page_Load(object sender, EventArgs e)
        {
            //lblKpiID.Text = kpiID.ToString();
            if (!Page.IsPostBack)
            {
                //start = new DateTime(1900, 1, 1);
                //end = new DateTime(2100, 1, 1);
                startDateChanged = false;
                endDateChanged = false;
            }
            String kpiIDS;
            int kpiID;
            try
            {
                kpiIDS = Request.QueryString["kpiID"];
                kpiID = int.Parse(kpiIDS);
            }
            catch
            {
                kpiID = -1;
                lblKpiID.Text = "QueryString not valid";
                rptKPIRecs.Visible = false;
                dataIniziale.Visible = false;
                dataFinale.Visible = false;
                lblStartDate.Visible = false;
                lblEndDate.Visible = false;
            }
            if (kpiID != -1)
            {
                Kpi indic = new Kpi(kpiID);
                if (indic.id != -1)
                {
                    proc = new processo(indic.procID);
                    proc.loadKPIs();
                    if (!Page.IsPostBack)
                    {
                        lblMsg.Text = indic.procID + " " + indic.numData + " ";
                        indic.loadRecords();
                        if (indic.numData > 0)
                        {
                            //lblMsg.Text += "Entro. Dati > 0";
                            indic.loadRecords();
                            start = (indic.data[0].date).AddDays(-1);
                            end = (indic.data[indic.numData - 1].date).AddDays(1);
                        }
                        else
                        {
                            lblMsg.Text += "No data found";
                            start = DateTime.Now.AddDays(-1);
                            end = DateTime.Now.AddDays(+1);
                        }
                    }
                    else
                    {
                        indic.loadLimitedRecords(start, end);
                    }
                    rptKPIRecs.DataSource = indic.data;
                    rptKPIRecs.DataBind();
                }
                else
                {
                    lblKpiID.Text = "KPI not found";
                    rptKPIRecs.Visible = false;
                    dataIniziale.Visible = false;
                    dataFinale.Visible = false;
                }
            }
            else
            {
                lblKpiID.Text = "KPI not found.";
                rptKPIRecs.Visible = false;
                dataIniziale.Visible = false;
                dataFinale.Visible = false;
                lblStartDate.Visible = false;
                lblEndDate.Visible = false;
            }
        }

        public void rptKPIRecs_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }
        }

        protected void startDate_Click(object sender, EventArgs e)
        {
            startDateChanged = true;
            start = startDate.SelectedDate;
            Chart1_Load(Chart1, EventArgs.Empty);
        }

        protected void endDate_Click(object sender, EventArgs e)
        {
            endDateChanged = true;
            end = endDate.SelectedDate;
            Chart1_Load(Chart1, EventArgs.Empty);
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {
            String kpiIDS;
            int kpiID;
            try
            {
                kpiIDS = Request.QueryString["kpiID"];
                kpiID = int.Parse(kpiIDS);
            }
            catch
            {
                kpiID = -1;
                Chart1.Visible = false;
            }
            if (kpiID != -1)
            {
                Kpi indic = new Kpi(kpiID);
                if (indic.id != -1)
                {
                    processo proc = new processo(indic.procID);
                    proc.loadKPIs();

                    try
                    {
                        Chart1.Series.Clear();
                        Chart1.Titles.Clear();
                        Chart1.Legends.Clear();
                        //Chart1.Series.Remove(proc.processName);
                    }
                    catch
                    {
                    }
                    //if (startDateChanged == false && endDateChanged == false)
                    //{
                        Chart1.Series.Add(proc.processName);
                        Chart1.Series.Add(proc.processName + " mean");
                        Chart1.Series.Add(proc.processName + " LCL");
                        Chart1.Series.Add(proc.processName + " UCL");
                        Chart1.Series.Add(proc.processName + " default value");
                        Chart1.Legends.Add("Legend");
                        Chart1.Legends["Legend"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Bottom;

                        Chart1.Titles.Add("0");
                    //}

                    

                    indic.loadLimitedRecords(start, end);
                    
                    dataIniziale.Text = start.ToString("yyyy-MM-dd");
                    dataFinale.Text = end.ToString("yyyy-MM-dd");
                    if (startDateChanged == true)
                    {
                        startDateChanged = false;
                    }

                    if (endDateChanged == true)
                    {
                        endDateChanged = false;
                    }
                    
                    Chart1.Series[proc.processName].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    
                    Chart1.Titles[0].Text = proc.processName + ": " + indic.name;
                    Chart1.Titles[0].Alignment = System.Drawing.ContentAlignment.TopCenter;
                    Chart1.Titles[0].TextStyle = System.Web.UI.DataVisualization.Charting.TextStyle.Shadow;
                    Chart1.Titles[0].Font = new System.Drawing.Font("Verdana", 20);

                    // Adding series for values of KPI
                    Chart1.Series[proc.processName].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    Chart1.Series[proc.processName].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series[proc.processName].XValueMember = "date";
                    Chart1.Series[proc.processName].YValueMembers = "valore";
                    Chart1.Series[proc.processName].Color = System.Drawing.Color.Black;
                    Chart1.Series[proc.processName].BorderWidth = 4;
                    Chart1.Series[proc.processName].LegendText = indic.name;

                    // Setto il valore minimo da mostrare sull'asse Y.
                    double min = 0;
                    if(indic.LCL < indic.baseVal)
                    {
                        min = indic.LCL;
                    }
                    else
                    {
                        min = indic.baseVal;
                    }
                    for (int i = 0; i < indic.numData; i++)
                    {
                        if (indic.data[i].valore < min)
                        {
                            min = indic.data[i].valore;
                        }
                    }

                    Chart1.ChartAreas[0].AxisY.Minimum = (min) * 0.8;

                    // Adding series for mean
                    Chart1.Series[proc.processName + " mean"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    Chart1.Series[proc.processName + " mean"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    Chart1.Series[proc.processName + " mean"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series[proc.processName + " mean"].LegendText = "Mean";
                    Chart1.Series[proc.processName + " mean"].Color = System.Drawing.Color.Blue;
                    Chart1.Series[proc.processName + " mean"].BorderWidth = 2;

                    // Adding series for baseVal
                    Chart1.Series[proc.processName + " default value"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    Chart1.Series[proc.processName + " default value"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    Chart1.Series[proc.processName + " default value"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series[proc.processName + " default value"].LegendText = "Default";
                    Chart1.Series[proc.processName + " default value"].Color = System.Drawing.Color.Gold;
                    Chart1.Series[proc.processName + " default value"].BorderWidth = 2;

                    // Adding series for lower control limit
                    Chart1.Series[proc.processName + " LCL"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    Chart1.Series[proc.processName + " LCL"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    Chart1.Series[proc.processName + " LCL"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series[proc.processName + " LCL"].LegendText = "Lower control Limit";
                    Chart1.Series[proc.processName + " LCL"].Color = System.Drawing.Color.Red;
                    Chart1.Series[proc.processName + " LCL"].BorderWidth = 2;

                    // Adding series for upper control limit
                    Chart1.Series[proc.processName + " UCL"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    Chart1.Series[proc.processName + " UCL"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                    Chart1.Series[proc.processName + " UCL"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series[proc.processName + " UCL"].LegendText = "Upper control Limit";
                    Chart1.Series[proc.processName + " UCL"].Color = System.Drawing.Color.Red;
                    Chart1.Series[proc.processName + " UCL"].BorderWidth = 2;

                    for (int i = 0; i < indic.numData; i++)
                    {
                        Chart1.Series[proc.processName + " mean"].Points.AddXY(indic.data[i].date.ToOADate(), indic.mean);
                        Chart1.Series[proc.processName + " UCL"].Points.AddXY(indic.data[i].date.ToOADate(), indic.UCL);
                        Chart1.Series[proc.processName + " LCL"].Points.AddXY(indic.data[i].date.ToOADate(), indic.LCL);
                        Chart1.Series[proc.processName + " default value"].Points.AddXY(indic.data[i].date.ToOADate(), indic.baseVal);
                    }

                    Chart1.DataSource = indic.data;
                    
                }
                else
                {
                    Chart1.Visible = false;
                }
            }
        }

        protected void lblStartDate_Click(object sender, EventArgs e)
        {
            if (startDate.Visible == true)
                startDate.Visible = false;
            else
                startDate.Visible = true;
        }

        protected void lblEndDate_Click(object sender, EventArgs e)
        {
            if (endDate.Visible == true)
                endDate.Visible = false;
            else
                endDate.Visible = true;
        }

 
    }
}