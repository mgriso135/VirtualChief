using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Produzione
{
    public partial class userWorkLoad : System.Web.UI.UserControl
    {
        public int procID;

            
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Chart1_Load(object sender, EventArgs e)
        {
            processo curr = new processo(procID);
            curr.loadFigli();
            bool areTherePO = false;
            for (int i = 0; i < curr.numSubProcessi; i++)
            {
                curr.subProcessi[i].loadProcessOwners();
                if (curr.subProcessi[i].numProcessOwners > 0)
                {
                    areTherePO = true;
                }
            }
            if (areTherePO == true)
            {
                // Looks for Users!
                List<String> uniqueUser = new List<string>();
                for (int i = 0; i < curr.numSubProcessi; i++)
                {
                        // Verifico che l'operatore non sia già inserito in lista
                        bool found = false;
                        for (int j = 0; j < uniqueUser.Count; j++)
                        {
                            if (curr.subProcessi[i].numProcessOwners > 0 && uniqueUser[j] == curr.subProcessi[i].processOwners[0].username)
                            {
                                found = true;
                                //lbl.Text += curr.subProcessi[i].processOwners[0].username + " " + found.ToString() + "<br/>";
                            }
                        }
                        //Se non l'ho trovato lo aggiungo all'array
                        if (curr.subProcessi[i].numProcessOwners > 0 && found == false)
                        {
                            uniqueUser.Add(curr.subProcessi[i].processOwners[0].username);
                        }
                }
                // Carico il workload di ogni utente
                List<double> workLoad = new List<double>();
                for (int i = 0; i < uniqueUser.Count; i++)
                {
                    double totWL = 0;
                    workLoad.Add(totWL);
                    for (int j = 0; j < curr.numSubProcessi; j++)
                    {
                        if (curr.subProcessi[j].numProcessOwners > 0 && curr.subProcessi[j].processOwners[0].username == uniqueUser[i])
                        {
                            curr.subProcessi[j].loadKPIs();
                            workLoad[i] += curr.subProcessi[j].getKPIBaseValByName("Tempo ciclo");
                        }
                    }
                }

                Chart1.Series.Clear();
                Chart1.Titles.Clear();
                Chart1.Series.Add("WorkLoad");
                Chart1.Series.Add("Cadenza");
                curr.loadKPIs();
                double cadenza = curr.getKPIBaseValByName("Cadenza");
                Chart1.Series["Cadenza"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                Chart1.Series["Cadenza"].BorderWidth = 5;
                Chart1.Series["WorkLoad"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                Chart1.Series["WorkLoad"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                Chart1.Series["WorkLoad"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                for (int i = 0; i < uniqueUser.Count; i++)
                {
                    Chart1.Series["WorkLoad"].Points.AddXY(uniqueUser[i], workLoad[i]);
                    Chart1.Series["Cadenza"].Points.AddXY(uniqueUser[i], cadenza);
                    if (workLoad[i] > cadenza)
                    {
                        lbl.Text += "<span style='color:red; font-size: 14px'>ATTENZIONE: l'operatore " + uniqueUser[i]
                            + " ha un carico di lavoro superiore alla cadenza. Probabilmente c'è stata un'assegnazione dei task non corretta?</span><br/>";
                    }
                }
            }
            else
            {
                Chart1.Visible = false;
            }
        }
    }
}