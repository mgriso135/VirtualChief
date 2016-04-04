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
    public partial class CustomerPareto1 : System.Web.UI.UserControl
    {
        public static PortafoglioClienti portClienti;

        protected void Page_Load(object sender, EventArgs e)
        {
            Chart1.Visible = false;
            tblSelectDate.Visible = false;
            rptCustomers.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Clienti";
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
                tblSelectDate.Visible = true;
                if (!Page.IsPostBack)
                {
                    portClienti = new PortafoglioClienti();
                }
            }
            else
            {
                Chart1.Visible = false;
                tblSelectDate.Visible = false;
                rptCustomers.Visible = false;
            }
        }

        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
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
                portClienti = new PortafoglioClienti(startPeriod, endPeriod);
                portClienti.loadTempoDiLavoroTotale(startPeriod, endPeriod);
                for (int i = 0; i < portClienti.Elenco.Count; i++)
                {
                    portClienti.Elenco[i].loadTempoDiLavoro(startPeriod, endPeriod);
                }

                var clientiOrdered = portClienti.Elenco.OrderByDescending(x => x.TempoDiLavoro);

                rptCustomers.DataSource = clientiOrdered;
                rptCustomers.DataBind();
                rptCustomers.Visible = true;

                int qtaClienti = portClienti.Elenco.Count;
                Chart1.Width = (qtaClienti * 100);
                Chart1.Visible = true;
                loadChart();
            }
            else
            {
                Chart1.Visible = false;
            }
        }

        protected void Chart1_Load(object sender, EventArgs e)
        {
            loadChart();
        }

        public void loadChart()
        {
            if (portClienti != null && portClienti.Elenco != null)
            {
                var clientiOrdered = portClienti.Elenco.OrderByDescending(x => x.TempoDiLavoro);
                //Chart1.Visible = true;
                Chart1.Series["customers"].XValueMember = "RagioneSociale";
                Chart1.Series["customers"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                Chart1.Series["customers"].YValueMembers = "TempoDiLavoroDbl";
                Chart1.Series["customers"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                Chart1.Series["customers"].ToolTip = "#VALX: #VALY ore";
                Chart1.DataSource = clientiOrdered;
                Chart1.DataBind();
            }
        }
    }
}