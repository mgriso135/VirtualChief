using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class AndonRepartoGANTT : System.Web.UI.UserControl
    {
        public int idReparto;
        protected void Page_Load(object sender, EventArgs e)
        {
            /*List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Andon Reparto";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {*/
                //lbl1.Text = idReparto.ToString();
                loadChart(idReparto);
                //lbl1.Text = idReparto.ToString() + "&nbsp;";
            /*}
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare l'Andon";
                chart1.Visible = false;
            }*/
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            /*List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Andon Reparto";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {*/
                lblData.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //lbl1.Text += "idReparto: " + idReparto.ToString() + "<br />";
                loadChart(idReparto);
            /*}
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare l'Andon";
                chart1.Visible = false;
            }*/
        }

        public void loadChart(int idRep)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            KIS.App_Code.AndonReparto andRep = new KIS.App_Code.AndonReparto(idRep);
            if (andRep.RepartoID != -1)
            {
                //lbl1.Text = "Numero tasks: " + andRep.ElencoTasks.Count.ToString() + "<br />";
                if (andRep.ElencoTasks.Count > 0)
                {
                    chart1.Visible = true;
                    chart1.ChartAreas.Add("main");
                    chart1.DataSource = andRep.ElencoTasks;
                    var tskN = andRep.ElencoTasks.Where(p => p.Status == 'N');
                    var tskP = andRep.ElencoTasks.Where(p => p.Status == 'P');
                    var tskI = andRep.ElencoTasks.Where(p => p.Status == 'I');
                    chart1.Series.Add("0");
                    chart1.Series["0"].XValueMember = "Name";
                    chart1.Series["0"].YValueMembers = "LateStart, LateFinish";
                    chart1.Series["0"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.RangeBar;
                    chart1.Series["0"].YValuesPerPoint = 1;
                    chart1.Series["0"].YAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                    chart1.DataBind();

                    chart1.ChartAreas["main"].AxisY.Interval = 1;
                    chart1.ChartAreas["main"].AxisX.Interval = 1;
                }
                else
                {
                    chart1.Visible = false;
                }
            }
            else
            {
                chart1.Visible = false;
            }
           
        }
    
    
        public struct StructIntervallo
        {
            public DateTime inizio;
            public DateTime fine;
        }

        public List<StructIntervallo> splitTask(int taskID)
        {
            List<StructIntervallo> intv = new List<StructIntervallo>();
            TaskProduzione tsk = new TaskProduzione(taskID);
            Reparto rp = new Reparto(tsk.RepartoID);
            DateTime inizio = tsk.LateStart;
            DateTime fine = tsk.LateFinish;

            rp.loadCalendario(inizio.AddDays(-2), fine.AddDays(2));
            for (int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
            {
                if (inizio >= rp.CalendarioRep.Intervalli[i].Inizio && fine <= rp.CalendarioRep.Intervalli[i].Fine)
                {
                    StructIntervallo stru;
                    stru.inizio = inizio;
                    stru.fine = fine;
                    intv.Add(stru);
                }
                else if (inizio >= rp.CalendarioRep.Intervalli[i].Inizio && rp.CalendarioRep.Intervalli[i].Fine >= inizio && rp.CalendarioRep.Intervalli[i].Fine <= fine)
                {
                    StructIntervallo stru;
                    stru.inizio = inizio;
                    stru.fine = rp.CalendarioRep.Intervalli[i].Fine;
                    intv.Add(stru);
                }
                else if (rp.CalendarioRep.Intervalli[i].Inizio >= inizio && rp.CalendarioRep.Intervalli[i].Inizio <= fine && rp.CalendarioRep.Intervalli[i].Fine >= fine)
                {
                    StructIntervallo stru;
                    stru.inizio = rp.CalendarioRep.Intervalli[i].Inizio;
                    stru.fine = fine;
                    intv.Add(stru);
                }
            }
            return intv;
        }
    }


}