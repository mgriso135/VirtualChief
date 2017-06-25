using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Analysis
{
    public partial class OperatoreTempoTasks : System.Web.UI.UserControl
    {
        public String usrID;
        protected void Page_Load(object sender, EventArgs e)
        {
            boxMain.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
                    String[] prmUser = new String[2];
                    prmUser[0] = "Analisi Operatori Tempi";
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
                        if (!String.IsNullOrEmpty(usrID) && usrID.Length > 0)
                        {
                            User usr = new User(usrID);
                            if (!String.IsNullOrEmpty(usr.username) && usr.username.Length > 0)
                            {
                                boxMain.Visible = true;
                                if (!Page.IsPostBack)
                                {
                                    for (int i = 0; i < 24; i++)
                                    {
                                        hhStart.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                        hhEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                    }

                                    for (int i = 0; i < 60; i++)
                                    {
                                        mmStart.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                        mmEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                        ssStart.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                        ssEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                    }

                                    /*usr.loadIntervalliDiLavoroOperatore();
                                    rptDetailTasks.DataSource = usr.IntervalliDiLavoroOperatore;
                                    rptDetailTasks.DataBind();

                                    var sommaTempi = new TimeSpan(usr.IntervalliDiLavoroOperatore.Sum(o => o.DurataIntervallo.Ticks));
                                    lblTotaleTempo.Text = "";
                                    if (sommaTempi.Days > 0)
                                    {
                                        lblTotaleTempo.Text = sommaTempi.Days.ToString() + " giorni, ";
                                    }
                                    lblTotaleTempo.Text += sommaTempi.Hours.ToString() + "hh:" + sommaTempi.Minutes.ToString() + "mm:" + sommaTempi.Seconds.ToString();

                                    var listPostazioni = from lst in usr.IntervalliDiLavoroOperatore
                                                         group lst by new { lst.idPostazione, lst.NomePostazione } into g
                                                         select new
                                                         {
                                                             IDPostazione = g.Key.idPostazione,
                                                             NomePostazione = g.Key.NomePostazione,
                                                             Tempo = new TimeSpan(g.Sum(x => x.DurataIntervallo.Ticks)),

                                                         };
                                    var listPostazioniOrdinata = listPostazioni.OrderByDescending(x => x.Tempo);
                                    rptDetailsPostazione.DataSource = listPostazioniOrdinata;
                                    rptDetailsPostazione.DataBind();

                                    var listTipoTask = from lst in usr.IntervalliDiLavoroOperatore
                                                         group lst by new { lst.NomeTask } into g
                                                         select new
                                                         {
                                                             NomeTask = g.Key.NomeTask,
                                                             Tempo = new TimeSpan(g.Sum(x => x.DurataIntervallo.Ticks)),

                                                         };
                                    var listTipoTaskOrdinata = listTipoTask.OrderByDescending(x => x.Tempo);
                                    rptTipoTasks.DataSource = listTipoTaskOrdinata;
                                    rptTipoTasks.DataBind();

                                    var listGiorniPresenza = from lst in usr.IntervalliDiLavoroOperatore
                                                       group lst by new { lst.DataInizio.Year, lst.DataInizio.Month, lst.DataInizio.Day } into g
                                                       select new
                                                       {
                                                           Giorno = g.Key.Day,
                                                           Mese = g.Key.Month,
                                                           Anno = g.Key.Year,
                                                       };

                                    rptGiorniPresenza.DataSource = listGiorniPresenza;
                                    rptGiorniPresenza.DataBind();

                                    var listProdotto = from lst in usr.IntervalliDiLavoroOperatore
                                                       group lst by new { lst.IDProdotto, lst.AnnoProdotto, lst.NomeProdotto } into g
                                                       select new
                                                       {
                                                           IDProdotto = g.Key.IDProdotto,
                                                           AnnoProdotto = g.Key.AnnoProdotto,
                                                           NomeProdotto = g.Key.NomeProdotto,
                                                           Tempo = new TimeSpan(g.Sum(x => x.DurataIntervallo.Ticks)),

                                                       };
                                    var listProdottoTaskOrdinata = listProdotto.OrderByDescending(x => x.Tempo);
                                    rptProdotto.DataSource = listProdottoTaskOrdinata;
                                    rptProdotto.DataBind();*/

                                }

                            }
                        }
                    }
                    else
                    {
                    }
        }

        protected void imgCheckPeriod_Click(object sender, EventArgs e)
        {
            DateTime dataInizio = new DateTime(1970, 1, 1);
            DateTime dataFine = new DateTime(1970, 1, 1);
            try
            {
                String[] giorni = txtStart.Text.Split('/');
                int anno=-1, mese=-1, giorno=-1;
                anno = Int32.Parse(giorni[2]);
                mese = Int32.Parse(giorni[1]);
                giorno = Int32.Parse(giorni[0]);
                int hh = -1, mm = -1, ss = -1;
                hh = Int32.Parse(hhStart.SelectedValue);
                mm = Int32.Parse(mmStart.SelectedValue);
                ss = Int32.Parse(ssStart.SelectedValue);

                dataInizio = new DateTime(anno, mese, giorno, hh, mm, ss);
            }
            catch(Exception ex)
            {
                dataInizio = new DateTime(1970, 1, 1);
                lbl1.Text += ex.Message;
            }

            try
            {
                String[] giorni = txtEnd.Text.Split('/');
                int anno = -1, mese = -1, giorno = -1;
                anno = Int32.Parse(giorni[2]);
                mese = Int32.Parse(giorni[1]);
                giorno = Int32.Parse(giorni[0]);
                int hh = -1, mm = -1, ss = -1;
                hh = Int32.Parse(hhEnd.SelectedValue);
                mm = Int32.Parse(mmEnd.SelectedValue);
                ss = Int32.Parse(ssEnd.SelectedValue);
                dataFine = new DateTime(anno, mese, giorno, hh, mm, ss);
            }
            catch
            {
                dataFine = new DateTime(1970, 1, 1);
            }
            
            if (dataInizio <= dataFine)
            {
                User usr = new User(usrID);
                usr.loadIntervalliDiLavoroOperatore(dataInizio, dataFine);
                //lbl1.Text = usr.log;
                var listaIntervalli = (from interv in usr.IntervalliDiLavoroOperatore
                                       where interv.DataInizio >= dataInizio && interv.DataFine <= dataFine
                                       select interv).ToList();
                rptDetailTasks.DataSource = listaIntervalli;
                rptDetailTasks.DataBind();

                var sommaTempi = new TimeSpan(listaIntervalli.Sum(o => o.DurataIntervallo.Ticks));
                lblTotaleTempo.Text = "";
                /*if (sommaTempi.TotalDays > 0)
                {
                    lblTotaleTempo.Text = sommaTempi.Days.ToString() + " giorni, ";
                }*/
                lblTotaleTempo.Text += Math.Round(sommaTempi.TotalHours,2).ToString() + " ore";// +sommaTempi.Minutes.ToString() + "mm:" + sommaTempi.Seconds.ToString();

                var listPostazioni = from lst in listaIntervalli
                                     group lst by new { lst.idPostazione, lst.NomePostazione } into g
                                     select new
                                     {
                                         IDPostazione = g.Key.idPostazione,
                                         NomePostazione = g.Key.NomePostazione,
                                         Tempo = new TimeSpan(g.Sum(x => x.DurataIntervallo.Ticks)),
                                     };
                var listPostazioniOrdinata = listPostazioni.OrderByDescending(x => x.Tempo);
                rptDetailsPostazione.DataSource = listPostazioniOrdinata;
                rptDetailsPostazione.DataBind();

                var listTipoTask = from lst in listaIntervalli
                                   group lst by new { lst.NomeTask } into g
                                   select new
                                   {
                                       NomeTask = g.Key.NomeTask,
                                       Tempo = new TimeSpan(g.Sum(x => x.DurataIntervallo.Ticks)),

                                   };
                var listTipoTaskOrdinata = listTipoTask.OrderByDescending(x => x.Tempo);
                rptTipoTasks.DataSource = listTipoTaskOrdinata;
                rptTipoTasks.DataBind();

                var listGiorniPresenza = from lst in listaIntervalli
                                         group lst by new { lst.DataInizio.Year, lst.DataInizio.Month, lst.DataInizio.Day } into g
                                         select new
                                         {
                                             Giorno = g.Key.Day,
                                             Mese = g.Key.Month,
                                             Anno = g.Key.Year,
                                         };

                var listGiorniPresenza2 = listGiorniPresenza.OrderBy(x => x.Anno).ThenBy(y => y.Mese).ThenBy(z => z.Giorno);

                rptGiorniPresenza.DataSource = listGiorniPresenza2;
                rptGiorniPresenza.DataBind();

                var listProdotto = from lst in listaIntervalli
                                   group lst by new {lst.RagioneSocialeCliente, lst.IDProdotto, lst.AnnoProdotto, lst.NomeProdotto } into g
                                   select new
                                   {
                                       RagioneSocialeCliente = g.Key.RagioneSocialeCliente,
                                       IDProdotto = g.Key.IDProdotto,
                                       AnnoProdotto = g.Key.AnnoProdotto,
                                       NomeProdotto = g.Key.NomeProdotto,
                                       Tempo = new TimeSpan(g.Sum(x => x.DurataIntervallo.Ticks)),

                                   };
                var listProdottoTaskOrdinata = listProdotto.OrderByDescending(x => x.Tempo);
                rptProdotto.DataSource = listProdottoTaskOrdinata;
                rptProdotto.DataBind();
            }
        }

        protected void imgUndoPeriod_Click(object sender, ImageClickEventArgs e)
        {
            txtStart.Text = "";
            txtEnd.Text = "";
            hhStart.SelectedValue = "0";
            mmStart.SelectedValue = "0";
            ssStart.SelectedValue = "0";
            hhEnd.SelectedValue = "0";
            mmEnd.SelectedValue = "0";
            ssEnd.SelectedValue = "0";
        }
    
    }
}