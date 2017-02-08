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
    public partial class postazioneWorkLoad : System.Web.UI.UserControl
    {
        public int postID;
            
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
                Postazione p = new Postazione(postID);
                if (p.id != -1)
                {
                    lblNomePost.Text = p.name;
                }
                else
                {
                    Chart1.Visible = false;
                }
            }
            else
            {
                lbl.Text = "Non hai il permesso di visualizzare il carico di lavoro della postazione.<br />";
                Chart1.Visible = false;
            }
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {
            Postazione pst = new Postazione(postID);
            Chart1.Series.Clear();
                        Chart1.Titles.Clear();
                        Chart1.Series.Add("WorkLoad");
                        Chart1.Series["WorkLoad"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
            
                        Chart1.Series["WorkLoad"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                        Chart1.Series["WorkLoad"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                        Chart1.Series["WorkLoad"].IsValueShownAsLabel = true;
                        // Carico il workload di ogni postazione
                        List<double> workLoad = new List<double>();
                        int cont = 0;
            if (pst.id != -1)
            {
                pst.LoadMainProc();
                //lbl.Text = pst.log;
                if (pst.MainProc.Count == 0)
                {
                    Chart1.Visible = false;
                }
                else
                {
                    for (int k = 0; k < pst.MainProc.Count; k++)
                    {
                        TimeSpan carico = pst.calculateWorkLoad(pst.MainProc[k]);
                        Chart1.Series["WorkLoad"].Points.AddXY(pst.MainProc[k].process.processName + " : " + pst.MainProc[k].variant.nomeVariante, Math.Round(carico.TotalHours,2));
                        Chart1.Series["WorkLoad"].Points[cont].ToolTip = pst.MainProc[k].process.processName + " : " + pst.MainProc[k].variant.nomeVariante + " - Tempo ciclo: " + carico.TotalHours.ToString() + " ore";
                        cont++;
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