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
    public partial class listArticoliTerminati : System.Web.UI.UserControl
    {
        public static DateTime Inizio;
        public static DateTime Fine;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            prmUser[0] = "Analisi Articolo Costo";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ckUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (ckUser == true)
            {
                if (!Page.IsPostBack)
                {
                    calInizio.SelectedDate = DateTime.Now.AddDays(-7);
                    calFine.SelectedDate = DateTime.Now;
                    Inizio = DateTime.Now.AddDays(-7);
                    Fine = DateTime.Now;
                    loadArticoliTerminati();
                }
            }
            else
            {
                lblDate.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptArticoliTerminati.Visible = false;
            }
        }

        protected void loadArticoliTerminati()
        {
            List<Articolo> lstArticoliTerminati = new List<Articolo>();
            lblDate.Text = "<span style=\"font-size: 20px; font-weight: bold\">"
                +GetLocalResourceObject("lblProdottiTerm1").ToString()+" " 
                + Inizio.ToString("dd/MM/yyyy") + " "+GetLocalResourceObject("lblProdottiTerm2").ToString()+" " 
                + Fine.ToString("dd/MM/yyyy") + "</span>";
            /*ElencoCommesse elComm = new ElencoCommesse();
            for (int i = 0; i < elComm.Commesse.Count; i++)
            {
                elComm.Commesse[i].loadArticoli();
                for (int j = 0; j < elComm.Commesse[i].Articoli.Count; j++)
                {
                    if (elComm.Commesse[i].Articoli[j].Status == 'F' && elComm.Commesse[i].Articoli[j].DataFineAttivita >= Inizio && elComm.Commesse[i].Articoli[j].DataFineAttivita <=Fine)
                    {
                        lstArticoliTerminati.Add(elComm.Commesse[i].Articoli[j]);
                    }
                }
            }

            lstArticoliTerminati.Sort(delegate(Articolo p1, Articolo p2)
            {
                return p1.DataFineAttivita.CompareTo(p2.DataFineAttivita);
            });*/

            ElencoArticoli elArt = new ElencoArticoli(Session["ActiveWorkspace"].ToString(), 'F', Inizio, Fine);
            for (int i = 0; i < elArt.ListArticoli.Count; i++)
            {
                elArt.ListArticoli[i].loadTempoDiLavoroTotale();
            }
            //rptArticoliTerminati.DataSource = lstArticoliTerminati;
            rptArticoliTerminati.DataSource = elArt.ListArticoli;
            rptArticoliTerminati.DataBind();
        }

        protected void calInizio_SelectionChanged(object sender, EventArgs e)
        {
            Inizio = calInizio.SelectedDate;
            loadArticoliTerminati();
        }

        protected void calFine_SelectionChanged(object sender, EventArgs e)
        {
            Fine = calFine.SelectedDate;
            loadArticoliTerminati();
        }

        protected void rptArticoliTerminati_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                   /* tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void rptArticoliTerminati_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String[] aProd = e.CommandArgument.ToString().Split('/');
            int idProd = -1, annoProd = -1;
            try
            {
                idProd = Int32.Parse(aProd[0]);
                annoProd = Int32.Parse(aProd[1]);
            }
            catch
            {
                idProd = -1;
                annoProd = -1;
            }

            Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), idProd, annoProd);
            if (art.ID != -1 && art.Year != -1)
            {
                switch (e.CommandName)
                {
                    case "riesuma":
                        List<String[]> elencoPermessi = new List<String[]>();
                        String[] prmUser = new String[2];
                        prmUser[0] = "Articolo Riesuma";
                        prmUser[1] = "X";
                        elencoPermessi.Add(prmUser);
                        bool ckUser = false;
                        if (Session["user"] != null)
                        {
                            User curr = (User)Session["user"];
                            ckUser = curr.ValidatePermessi(elencoPermessi);
                        }

                        if (ckUser == true)
                        {
                            Boolean ret = art.Riesuma();
                            if (ret)
                            {
                                loadArticoliTerminati();
                                lbl1.Text = GetLocalResourceObject("lblRiesumaOk").ToString();
                            }
                            else
                            {
                                lbl1.Text = GetLocalResourceObject("lblRiesumaKo").ToString();
                            }
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblPermessoRiesumaKo").ToString();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}