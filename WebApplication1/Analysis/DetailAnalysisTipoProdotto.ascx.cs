using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS.App_Code;
namespace KIS.Analysis
{
    public partial class DetailAnalysisTipoProdotto1 : System.Web.UI.UserControl
    {
        public int idProc, rev, idVar;
        protected ElencoArticoli elArticoli;
        protected void Page_Load(object sender, EventArgs e)
        {
            container.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi TipoProdotto";
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
                container.Visible = true;
                if (!Page.IsPostBack)
                {
                    accordion1.Visible = false;
                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, rev), new variante(idVar));
                    lblNomeTipoProdotto.Text = prcVar.process.processName + " - " + prcVar.variant.nomeVariante;
                    lblDescTipoProdotto.Text = prcVar.variant.descrizioneVariante;
                }
            }
            else
            {
                lbl1.Text = "Non hai i permessi per visualizzare l'analisi dati del tipo di prodotto.";
                container.Visible = false;
            }
        }

        protected void rptLeadTimes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label a = ((Label)e.Item.FindControl("lblLTMedio"));
                if (elArticoli.ListArticoli.Count > 0)
                {
                    a.Text = elArticoli.LeadTimeMedio.TotalHours.ToString("F2") + " ore";
                }
                else
                {
                    a.Text = "N.A.";
                }
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DateTime start = new DateTime(1970, 1, 1);
            DateTime end = new DateTime(1970, 1, 1);
            try
            {
                String[] sStart = txtStart.Text.Split('/');
                int yy, gg, mm;
                gg = Int32.Parse(sStart[0]);
                mm = Int32.Parse(sStart[1]);
                yy = Int32.Parse(sStart[2]);
                start = new DateTime(yy, mm, gg);

                String[] sEnd = txtEnd.Text.Split('/');
                gg = Int32.Parse(sEnd[0]);
                mm = Int32.Parse(sEnd[1]);
                yy = Int32.Parse(sEnd[2]);
                end = new DateTime(yy, mm, gg);
            }
            catch
            {
                start = new DateTime(1970, 1, 1);
                end = new DateTime(1970, 1, 1);
            }

            ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, rev), new variante(idVar));
            if (start > new DateTime(1970,1,1) && end > start && prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.process.revisione != -1 && prcVar.variant.idVariante != -1)
            {
                elArticoli = new ElencoArticoli(prcVar, 'F', start, end);

                if (elArticoli.ListArticoli.Count > 0)
                {
                   
                    accordion1.Visible = true;
                    chartTempi.Visible = true;
                    chartLeadTimes.Visible = true;
                    rptLeadTimes.Visible = true;
                    rptProdotti.Visible = true;
                    rptTempiDiLavoro.Visible = true;
                    List<TaskProduzione> eltasks = new List<TaskProduzione>();
                    for (int i = 0; i < elArticoli.ListArticoli.Count; i++)
                    {
                        elArticoli.ListArticoli[i].loadTempoDiLavoroTotale();
                        elArticoli.ListArticoli[i].loadLeadTimes();
                        for (int j = 0; j < elArticoli.ListArticoli[i].Tasks.Count; j++)
                       {
                            eltasks.Add(elArticoli.ListArticoli[i].Tasks[j]);
                        }
                    }
                   
                    chartTempi.Width = elArticoli.ListArticoli.Count < 12 ? 600 : chartTempi.Width = 50 * elArticoli.ListArticoli.Count;

                    chartTempi.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                    chartTempi.Series[0].IsValueShownAsLabel = true;
                    chartTempi.Series[0].XAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                    chartTempi.Series[0].ToolTip = "Prodotto #VALX, #VAL{0.00} ore di lavoro";
                    chartTempi.Series[0].XValueMember = "IDCombinato";
                    chartTempi.Series[0].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    chartTempi.Series[0].YValueMembers = "TempoDiLavoroUnitarioHoursDbl";//"TempoDiLavoroTotaleDbl";
                    chartTempi.Series[0].Label = "#VALY{0.00} ore";

                    chartTempi.DataSource = elArticoli.ListArticoli;
                    chartTempi.DataBind();

                    chartTempi.Series.Add("media");
                    chartTempi.Series["media"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    chartTempi.Series["media"].IsValueShownAsLabel = false;
                    chartTempi.Series["media"].XAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                    chartTempi.Series["media"].BorderWidth = 4;

                    for (int q = 0; q < elArticoli.ListArticoli.Count; q++)
                    {
                        chartTempi.Series["media"].Points.AddY(elArticoli.TempoDiLavoroMedio.TotalHours);
                        chartTempi.Series["media"].ToolTip = "Media: #VAL{0.00} ore";
                    }
                    rptTempiDiLavoro.DataSource = elArticoli.ListArticoli;
                    rptTempiDiLavoro.DataBind();

                    rptLeadTimes.DataSource = elArticoli.ListArticoli;
                    rptLeadTimes.DataBind();

                    rptProdotti.DataSource = elArticoli.ListArticoli;
                    rptProdotti.DataBind();

                    lblMediaTempoDiLavoro.Text = Math.Round(elArticoli.TempoDiLavoroMedio.TotalHours, 2).ToString() + " ore";
                    
                    // Lead times
                    chartLeadTimes.Width = elArticoli.ListArticoli.Count < 12 ? 600 : chartTempi.Width = 50 * elArticoli.ListArticoli.Count;
                    chartLeadTimes.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                    chartLeadTimes.Series[0].IsValueShownAsLabel = true;
                    chartLeadTimes.Series[0].XAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                    chartLeadTimes.Series[0].ToolTip = "Prodotto #VALX, #VAL{0.00} ore di lavoro";
                    chartLeadTimes.Series[0].XValueMember = "IDCombinato";
                    chartLeadTimes.Series[0].YValueMembers = "LeadTimeDbl";
                    chartLeadTimes.Series[0].Label = "#VALY{0.00} ore";
                    chartLeadTimes.DataSource = elArticoli.ListArticoli;
                    chartLeadTimes.DataBind();

                    chartLeadTimes.Series.Add("media");
                    chartLeadTimes.Series["media"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    chartLeadTimes.Series["media"].IsValueShownAsLabel = false;
                    chartLeadTimes.Series["media"].XAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                    chartLeadTimes.Series["media"].BorderWidth = 4;

                    for (int q = 0; q < elArticoli.ListArticoli.Count; q++)
                    {
                        chartLeadTimes.Series["media"].Points.AddY(elArticoli.LeadTimeMedio.TotalHours);
                        chartLeadTimes.Series["media"].ToolTip = "Media: #VAL{0.00} ore";
                    }

                    lblMediaLeadTimes.Text = Math.Round(elArticoli.LeadTimeMedio.TotalHours, 2).ToString() + " ore";

                    // CARICO rptTempiLavoroTasks
                    // Raggruppo i task
                   
                    var result =
                    (
                        from task in eltasks
                        group task by new { task.OriginalTask, task.OriginalTaskRevisione, task.VarianteID } into g
                        select new
                        {
                            processID = g.Key.OriginalTask,
                            revisione = g.Key.OriginalTaskRevisione,
                            varianteID=g.Key.VarianteID,
                            tempoDbl = g.Average(x=>x.TempoDiLavoroEffettivoUnitario.Ticks),
                            tempo = new TimeSpan((long)g.Average(x=>x.TempoDiLavoroEffettivoUnitario.Ticks)),
                            quantita = g.Sum(y=> y.QuantitaProdotta),
                            nomeTask = g.First().Name
                        } 
                    ).ToList();

                    rptTempiLavoroTasks.DataSource = result;
                    rptTempiLavoroTasks.DataBind();

    
                    lbl1.Text = "Data loaded.<br />";
                    
                }
                else
                {
                    chartTempi.Visible = false;
                    chartLeadTimes.Visible = false;
                    rptLeadTimes.Visible = false;
                    rptProdotti.Visible = false;
                    rptTempiDiLavoro.Visible = false;
                    lbl1.Text = "Nessun prodotto trovato.<br />";
                }
            }
        }
    }
}

