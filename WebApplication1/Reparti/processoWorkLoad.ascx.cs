using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class processoWorkLoad : System.Web.UI.UserControl
    {
        public int procID;
        public int var;
        public int repID;
            
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Postazione";
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
            }
            else
            {
                lbl.Text = "Non hai il permesso di visualizzare il carico di lavoro della postazione.<br />";
                Chart1.Visible = false;
            }
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {
            loadChart();
            lbl.Text = "Last update: " + DateTime.Now.ToString("HH:mm:ss");
        }

        protected void loadChart()
        {
            Reparto rep = new Reparto(repID);
            processo curr = new processo(procID);
            variante vr1 = new variante(var);
            ProcessoVariante prc = new ProcessoVariante(curr, vr1);
            bool rt = rep.loadPostazioniTask(prc);
            //!Page.IsPostBack && 
            if (prc.process != null && prc.variant != null)
            {
                if (rep!=null && rep.id!=-1 && rep.PostazioniTask.Count > 0)
                {
                    // Looks for Postazioni!
                    List<int> posts = new List<int>();
                    for (int i = 0; i < rep.PostazioniTask.Count; i++)
                    {
                        // Verifico che la postazione non sia già inserito in lista
                        bool found = false;
                        for (int j = 0; j < posts.Count; j++)
                        {
                            if (posts[j] == rep.PostazioniTask[i].Pst.id)
                            {
                                found = true;
                            }
                        }
                        //Se non l'ho trovata la aggiungo all'array
                        if (found == false)
                        {
                            posts.Add(rep.PostazioniTask[i].Pst.id);
                        }
                    }

                    Chart1.Series.Clear();
                    Chart1.Titles.Clear();
                    Chart1.Series.Add("WorkLoad");
                    Chart1.Series.Add("Cadenza");
                    Chart1.Series["Cadenza"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                    Chart1.Series["Cadenza"].BorderWidth = 5;
                    Chart1.Series["WorkLoad"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                    Chart1.Series["WorkLoad"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                    Chart1.Series["WorkLoad"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                    Chart1.Series["WorkLoad"].IsValueShownAsLabel = true;

                    // Carico il workload di ogni postazione
                    List<double> workLoad = new List<double>();
                    int cont = 0;
                    for (int i = 0; i < posts.Count; i++)
                    {
                        Postazione pst = new Postazione(posts[i]);
                        if (pst.id != -1)
                        {
                            TimeSpan carico = pst.calculateWorkLoad(prc);
                            Chart1.Series["WorkLoad"].Points.AddXY(pst.name, carico.TotalHours);
                            Chart1.Series["WorkLoad"].Points[cont].ToolTip = pst.name + " - Tempo ciclo: " + carico.TotalHours.ToString() + " ore";
                            cont++;
                            if (rep.Cadenza != null && rep.Cadenza.TotalSeconds > 0)
                            {
                                Chart1.Series["Cadenza"].Points.AddXY(pst.name, rep.Cadenza.TotalHours);
                                if (carico > rep.Cadenza)
                                {
                                    lbl.Text += "<span style='color:red'>Attenzione: per il processo " + prc.process.processName
                                        + " - variante " + prc.variant.nomeVariante + " il carico di lavoro della postazione " + pst.name
                                            + " è superiore alla cadenza. Ci saranno dei vincoli sul mix da rispettare.</span><br/>";
                                }
                            }
                        }
                    }
                }
                else
                {
                    Chart1.Visible = false;
                }
            }
            else
            {
                // idVariante o processID != -1
            }
        }
        
    
    }
}