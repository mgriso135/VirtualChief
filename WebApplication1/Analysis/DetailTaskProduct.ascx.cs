using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Analysis
{
    public partial class DetailTaskProduct1 : System.Web.UI.UserControl
    {
        public int taskID, revTask, prodID, prodRev, varianteID;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMediaTempoDiLavoro.Visible = false;
            chartTempiLavoro.Visible = false;
            lblMediaTempiCiclo.Visible = false;
            chartTempiCiclo.Visible = false;
            accordion1.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Tasks";
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
                processo task = new processo(taskID, revTask);
                ProcessoVariante prod = new ProcessoVariante(new processo(prodID, prodRev), new variante(varianteID));
                if (task.processID != -1 && prod != null && prod.process != null && prod.variant != null && prod.process.processID != -1 && prod.variant.idVariante != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        lblProdotto.InnerText = GetLocalResourceObject("lblStoricoTempi1").ToString() + " " 
                            + task.processName + " " + GetLocalResourceObject("lblStoricoTempi2").ToString()  
                            + " " + prod.process.processName + " " + prod.variant.nomeVariante;
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblTaskNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void btnAnalizza_Click(object sender, ImageClickEventArgs e)
        {
            processo task = new processo(taskID, revTask);
            ProcessoVariante prod = new ProcessoVariante(new processo(prodID, prodRev), new variante(varianteID));
            if (task.processID != -1 && prod != null && prod.process != null && prod.variant != null && prod.process.processID != -1 && prod.variant.idVariante != -1)
            {
                DateTime inPrd = new DateTime(1970, 1, 1), finPrd = new DateTime(1970, 1, 1);
                    int ggI, ggF, mmI, mmF, yyI, yyF;
                    String[] dataI = new String[3];
                    String[] dataF = new String[3];
                    dataI = (txtProductDateStart.Text).Split('/');
                    dataF = (txtProductDateEnd.Text).Split('/');

                    try
                    {
                        ggI = Int32.Parse(dataI[0]);
                        mmI = Int32.Parse(dataI[1]);
                        yyI = Int32.Parse(dataI[2]);

                        ggF = Int32.Parse(dataF[0]);
                        mmF = Int32.Parse(dataF[1]);
                        yyF = Int32.Parse(dataF[2]);

                        inPrd = new DateTime(yyI, mmI, ggI);
                        finPrd = new DateTime(yyF, mmF, ggF);

                        if (inPrd > finPrd)
                        {
                            inPrd = new DateTime(1970, 1, 1);
                            finPrd = new DateTime(1970, 1, 1);
                        }
                    }
                    catch
                    {
                        inPrd = new DateTime(1970, 1, 1);
                        finPrd = new DateTime(1970, 1, 1);
                    }

                    if (inPrd < finPrd)
                    {
                        accordion1.Visible = true;

                        lblMediaTempoDiLavoro.Visible = true;
                        chartTempiLavoro.Visible = true;

                        ElencoTaskProduzione elTasks_c = new ElencoTaskProduzione(task, prod, inPrd, finPrd);
                        var elTasks = elTasks_c.Tasks.OrderBy(x => x.DataInizioTask);

                        // Grafico tempi di lavoro
                        chartTempiLavoro.Visible = true;
                        chartTempiLavoro.Series[0].IsValueShownAsLabel = true;
                        chartTempiLavoro.Series[0].XAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                    chartTempiLavoro.Series[0].ToolTip = "Task #VALX, #VAL " + GetLocalResourceObject("lblChartOreLav").ToString();
                        chartTempiLavoro.Series[0].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                        chartTempiLavoro.Series[0].XValueMember = "DataInizioTask";
                        chartTempiLavoro.Series[0].YValueMembers = "TempoDiLavoroEffettivoUnitarioDbl";
                        chartTempiLavoro.DataSource = elTasks;
                        chartTempiLavoro.DataBind();
                        lblMediaTempoDiLavoro.Text = "";
                        if (elTasks_c.MediaTempoLavoro.Days > 0)
                        {
                            lblMediaTempoDiLavoro.Text = elTasks_c.MediaTempoLavoro.TotalDays.ToString() + " " + GetLocalResourceObject("lblGiorni").ToString() + ", ";
                        }
                        lblMediaTempoDiLavoro.Text += elTasks_c.MediaTempoLavoro.Hours.ToString() + "HH:"
                            + elTasks_c.MediaTempoLavoro.Minutes.ToString() + "mm:"
                            + elTasks_c.MediaTempoLavoro.Seconds.ToString() + "ss";

                        rptTempiLavoro.DataSource = elTasks;
                        rptTempiLavoro.DataBind();

                        // Grafico tempi ciclo
                        chartTempiCiclo.Visible = true;
                        chartTempiCiclo.Series[0].IsValueShownAsLabel = true;
                        chartTempiCiclo.Series[0].XAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                        chartTempiCiclo.Series[0].ToolTip = "Task #VALX, #VAL " + GetLocalResourceObject("lblChartOreLav").ToString();
                        chartTempiCiclo.Series[0].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.DateTime;
                        chartTempiCiclo.Series[0].XValueMember = "DataInizioTask";
                        chartTempiCiclo.Series[0].YValueMembers = "TempoCicloEffettivoUnitarioDbl";
                        chartTempiCiclo.DataSource = elTasks;
                        chartTempiCiclo.DataBind();
                        lblMediaTempiCiclo.Visible = true;
                        lblMediaTempiCiclo.Text = "";
                        if (elTasks_c.MediaTempoCiclo.Days > 0)
                        {
                            lblMediaTempiCiclo.Text = elTasks_c.MediaTempoCiclo.TotalDays.ToString() + " " + GetLocalResourceObject("lblGiorni").ToString() + ", ";
                    }
                        lblMediaTempiCiclo.Text += elTasks_c.MediaTempoCiclo.Hours.ToString() + "HH:"
                            + elTasks_c.MediaTempoCiclo.Minutes.ToString() + "mm:"
                            + elTasks_c.MediaTempoCiclo.Seconds.ToString() + "ss";

                        rptTempiCiclo.DataSource = elTasks;
                        rptTempiCiclo.DataBind();
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorDateNonCorrette").ToString();
                        accordion1.Visible = false;
                    }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblTaskNotFound").ToString();
                accordion1.Visible = false;
            }
        }
    }
}