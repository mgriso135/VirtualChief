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
    public partial class showProduttivitaReparto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
                if (!Page.IsPostBack)
                {
                    carica();
                }
            
        }

        protected void carica()
        {
/*            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Warning";
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
  */              int repID = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        repID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        repID = -1;
                    }
                }
                if (repID != -1)
                {
                    Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                    //if (!Page.IsPostBack)
                    {
                        rp.loadCalendario(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-7), rp.tzFusoOrario), TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(10), rp.tzFusoOrario));
                        int curr = -1;
                        for (int i = 0; i < rp.CalendarioRep.Intervalli.Count; i++)
                        {
                            if (rp.CalendarioRep.Intervalli[i].Inizio <= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) && TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario) <= rp.CalendarioRep.Intervalli[i].Fine)
                            {
                                curr = i;
                            }
                        }

                        //lblFineProgrammate.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                        if (curr != -1)
                        {
                            ElencoArticoliAperti elArtOpen = new ElencoArticoliAperti(rp.id);

                            int finePrevista = 0;
                            for (int i = 0; i < elArtOpen.ArticoliAperti.Count; i++)
                            {
                                if (elArtOpen.ArticoliAperti[i].DataFineUltimoTask <= rp.CalendarioRep.Intervalli[curr].Inizio)
                                {
                                    finePrevista++;
                                }
                            }

                            rp.CalendarioRep.Intervalli[curr].loadArticoliDaTerminare(rp);
                            rp.CalendarioRep.Intervalli[curr].loadArticoliTerminati(rp);
                        lblFineProgrammate.Text = "<span style=\"font-size: 12px;\">"
                        + GetLocalResourceObject("lblExpectedProduction")
                            + ":</span>&nbsp;<span style=\"font-weight:bold; font-size: 24px;\">"
                                + (finePrevista + rp.CalendarioRep.Intervalli[curr].NumArticoliDaTerminare).ToString()
                                //+ rp.CalendarioRep.Intervalli[curr].NumArticoliDaTerminare.ToString()
                                + "</span><span style=\"font-size: 20px;\"></span>";
//                                + rp.CalendarioRep.Intervalli[curr].log;

                            lblFineAttuale.Text = "<span style=\"font-size: 12px;\">"+
                            GetLocalResourceObject("lblCurrentProduction")
                            +":</span>&nbsp;<span style=\"font-weight:bold; font-size: 24px;\">"
                                + rp.CalendarioRep.Intervalli[curr].ArticoliTerminati.Count.ToString()
                                + "</span><span style=\"font-size: 20px;\"></span>";
                            intervallo.Text = rp.CalendarioRep.Intervalli[curr].Inizio.ToString("dd/MM/yyyy HH:mm:ss") + " - "
                                + rp.CalendarioRep.Intervalli[curr].Fine.ToString("dd/MM/yyyy HH:mm:ss");
                            if (rp.CalendarioRep.Intervalli[curr].ArticoliTerminati.Count > 0)
                            {
                                rptElencoArticoliTerminati.Visible = true;
                                rptElencoArticoliTerminati.DataSource = rp.CalendarioRep.Intervalli[curr].ArticoliTerminati;
                                rptElencoArticoliTerminati.DataBind();
                            }
                            else
                            {
                                rptElencoArticoliTerminati.Visible = false;
                            }

                        }
                        else
                        {
                            lblFineProgrammate.Text = "<span style=\"font-size: 12px;\">"+ GetLocalResourceObject("lblNoTimeSpan").ToString()+ "</span>";
                        lblFineAttuale.Text = "";
                        }
                    }
                }
    /*        }
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare i dati del reparto.<br/>";
                upd1.Visible = false;
                frmShowDatiProdReparto.Visible = false;
            }*/
        }


        protected void time1_Tick(object sender, EventArgs e)
        {
            carica();
        }
    }
}