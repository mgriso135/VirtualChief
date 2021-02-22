using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class wlReparto : System.Web.UI.UserControl
    {
        public int idReparto;
        public static List<DateTime> list = new List<DateTime>();
        public static List<int> idPostazioni = new List<int>();
        public static DateTime inizio, fine;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto WorkLoad";
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
                if (!Page.IsPostBack)
                {
                    chkLstPostazioni.Visible = false;
                    inizio = DateTime.Now;
                    fine = inizio;
                    Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), idReparto);
                    rp.loadPostazioni();
                    idPostazioni = new List<int>();
                    for (int i = 0; i < rp.Postazioni.Count; i++)
                    {
                        idPostazioni.Add(rp.Postazioni[i].id);
                    }
                    Chart1.Visible = false;
                }
                else
                {
                    if (inizio < fine)
                    {
                        caricaGrafico();
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                calDate.Visible = false;
                chkLstPostazioni.Visible = false;
                rbPostazioni.Visible = false;
            }
        }

        protected void calDate_SelectionChanged(object sender, EventArgs e)
        {
            lbl1.Text = "";
            
            if (Session["SelectedDates"] != null)
            {
                List<DateTime> newList = (List<DateTime>)Session["SelectedDates"];
                foreach (DateTime dt in newList)
                {
                    calDate.SelectedDates.Add(dt);
                }
                list.Clear();

                inizio = DateTime.Now.AddDays(365);
                fine = DateTime.Now.AddDays(-365);
                foreach (DateTime dt in calDate.SelectedDates)
                {
                    lbl1.Text += dt.ToShortDateString() + "<br />";
                    if (dt < inizio)
                    {
                        inizio = dt;
                    }
                    if (dt > fine)
                    {
                        fine = dt;
                    }
                }
                
                if (inizio < fine)
                {
                    caricaGrafico();
                }
            }
        }

        protected void calDate_PreRender(object sender, DayRenderEventArgs e)
        {
                e.Day.IsSelectable = true;
                if (list.Count >= 2)
                {
                    list.Clear();
                }
                if (e.Day.IsSelected == true)
                {
                    list.Add(e.Day.Date);
                }
                Session["SelectedDates"] = list;
        }

        protected void rbPostazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            int res = -1;
            try
            {
                res = Int32.Parse(rbPostazioni.SelectedValue);
            }
            catch
            {
                res = -1;
            }
            if (res == 0)
            {
                chkLstPostazioni.Visible = false;
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), idReparto);
                rp.loadPostazioni();
                idPostazioni = new List<int>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    idPostazioni.Add(rp.Postazioni[i].id);
                }
                if (inizio < fine)
                {
                    caricaGrafico();
                }
            }
            else
            {
                chkLstPostazioni.Visible = true;
                chkLstPostazioni.Items.Clear();
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), idReparto);
                rp.loadPostazioni();
                idPostazioni = new List<int>();
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    chkLstPostazioni.Items.Add(new ListItem(rp.Postazioni[i].name, rp.Postazioni[i].id.ToString()));
                    chkLstPostazioni.Items[i].Selected = true;
                    idPostazioni.Add(rp.Postazioni[i].id);
                }
                if (inizio < fine)
                {
                    caricaGrafico();
                }
            }
        }

        protected void chkLstPostazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            idPostazioni = new List<int>();
            for (int i = 0; i < chkLstPostazioni.Items.Count; i++)
            {
                if (chkLstPostazioni.Items[i].Selected == true)
                {
                    int idP = -1;
                    try
                    {
                        idP = Int32.Parse(chkLstPostazioni.Items[i].Value);
                    }
                    catch
                    {
                        idP = -1;
                    }
                    if (idP != -1)
                    {
                        idPostazioni.Add(idP);
                        if (inizio < fine)
                        {
                            caricaGrafico();
                        }
                    }
                }
            }
        }

        protected void caricaGrafico()
        {
            if (idReparto != -1)
            {
                Chart1.Visible = true;
                Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), idReparto);
                Chart1.Series.Clear();
                Chart1.Titles.Clear();
                Chart1.Titles.Add(new System.Web.UI.DataVisualization.Charting.Title(rp.name));
                Chart1.Series.Add("WorkLoad");
                Chart1.Series["WorkLoad"].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
                Chart1.Series["WorkLoad"].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.String;
                Chart1.Series["WorkLoad"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                //Chart1.Series["WorkLoad"].LabelFormat = "HH:mm:ss";
                Chart1.Series["WorkLoad"].IsValueShownAsLabel = true;

                rp.loadPostazioni();
                TimeSpan somma = new TimeSpan(0, 0, 0);
                int cont = 0;
                for (int i = 0; i < rp.Postazioni.Count; i++)
                {
                    bool found = false;
                    // Se lo trovo nell'elenco delle postazioni selezionate...
                    for (int q = 0; q < idPostazioni.Count; q++)
                    {
                        if (rp.Postazioni[i].id == idPostazioni[q])
                        {
                            found = true;
                        }
                    }

                    if (found == true)
                    {
                        TimeSpan carico = rp.Postazioni[i].getCaricoDiLavoroProgrammato(rp.id, inizio, fine.AddDays(1));
                        //lbl1.Text += carico.TotalMinutes.ToString() + carico.Hours.ToString() + carico.Minutes.ToString() + carico.Seconds.ToString() + "<br />";
                        somma += carico;
                        if (rbPostazioni.SelectedValue == "1")
                        {
                            Chart1.Series["WorkLoad"].Points.AddXY(rp.Postazioni[i].name, carico.TotalHours);
                            Chart1.Series["WorkLoad"].Points[cont].ToolTip = rp.Postazioni[i].name + " - "+GetLocalResourceObject("lblCaricoDiLavoro").ToString()+": " + carico.TotalHours.ToString() + " "+ GetLocalResourceObject("lblOre").ToString();
                            Chart1.Series["WorkLoad"].Points[cont].Label = carico.TotalHours + " " + GetLocalResourceObject("lblOre").ToString();
                            cont++;
                        }
                    }
                }

                if (rbPostazioni.SelectedValue == "0")
                {
                    Chart1.Series["WorkLoad"].Points.AddXY(rp.name, somma.TotalHours);
                    Chart1.Series["WorkLoad"].Points[cont].ToolTip = rp.name + " - " + GetLocalResourceObject("lblCaricoDiLavoro").ToString() + ": " + somma.TotalHours.ToString() + " ore";
                    Chart1.Series["WorkLoad"].Points[cont].Label = somma.TotalHours + " "+ GetLocalResourceObject("lblOre").ToString();
                }
                lbl1.Text = GetLocalResourceObject("lblCaricoComplessivoReparto").ToString()+ " "
                    + rp.name 
                    + " "+GetLocalResourceObject("lblNelPeriodo")+" " + inizio.ToString("dd/MM/yyyy") + " - " + fine.ToString("dd/MM/yyyy") + ": <b>" 
                    + somma.TotalHours.ToString() + " "+GetLocalResourceObject("lblOre").ToString()+"</b><br />";
            }
        }
    }
}