using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzInserisciDataConsegna1 : System.Web.UI.UserControl
    {
        public int idCommessa, annoCommessa, idProc, revProc, idVariante, idReparto, idProdotto, annoProdotto, quantita;
        public String matricola;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbl.Visible = false;
                lnkGoBack.Visible = false;
                btnGoFwd.Visible = false;
                matricola = (String.IsNullOrEmpty(matricola) || matricola.Length == 0) ? "" : matricola;
            }

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                Commessa cm = new Commessa(idCommessa, annoCommessa);
                ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
                Reparto rp = new Reparto(idReparto);
                if (cm.ID != -1 && prcVar.process != null && prcVar.variant != null && rp.id != -1)
                {
                    tbl.Visible = true;
                    lnkGoBack.Visible = true;
                    lnkGoBack.NavigateUrl = "wzAssociaTaskPostazioni.aspx?idCommessa=" + cm.ID.ToString()
                    + "&annoCommessa=" + cm.Year.ToString()
                    + "&idProc=" + prcVar.process.processID.ToString()
                    + "&revProc=" + prcVar.process.revisione.ToString()
                    + "&idVariante=" + prcVar.variant.idVariante.ToString()
                    + "&idReparto=" + rp.id.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                    + "&annoProdotto=" + annoProdotto.ToString()
                    + "&quantita=" + quantita.ToString()
                    +"&matricola="+matricola.ToString();

                    if (!Page.IsPostBack)
                    {
                        if (idProdotto != -1 && annoProdotto != -1)
                        {
                            Articolo art = new Articolo(idProdotto, annoProdotto);
                            if (art.ID != -1 && art.Year != -1 && art.DataPrevistaConsegna >= DateTime.Now)
                            {
                                calDate.SelectedDate = art.DataPrevistaConsegna;
                                btnGoFwd.Visible = true;
                            }
                        }
                    }
                }
                
            }
            else
            {
                
            }
        }

        protected void btnGoFwd_Click(object sender, ImageClickEventArgs e)
        {
            Reparto rp = new Reparto(idReparto);
            Commessa cm = new Commessa(idCommessa, annoCommessa);
            ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVariante));
            if (idProdotto == -1 && annoProdotto == -1)
            {
                if (cm.ID != -1 && prcVar.process != null && prcVar.variant != null && rp.id != -1)
                {
                    if (calDate.SelectedDate >= DateTime.Now)
                    {
                        int[] newArt = cm.AddArticoloInt(prcVar, calDate.SelectedDate, quantita);
                        if (newArt[0] != -1 && newArt[1] != -1)
                        {
                            Articolo art = new Articolo(newArt[0], newArt[1]);
                            art.Reparto = rp.id;
                            if(!String.IsNullOrEmpty(matricola) && matricola.Length>0)
                            { 
                                art.Matricola = matricola;
                            }

                            if (art.ID != -1 && art.Year != -1)
                            {
                                Response.Redirect("wzQuestionWorkLoad.aspx?idCommessa="
                                + cm.ID.ToString()
                                + "&annoCommessa=" + cm.Year.ToString()
                                + "&idProc=" + prcVar.process.processID.ToString()
                                + "&revProc=" + prcVar.process.revisione.ToString()
                                + "&idVariante=" + prcVar.variant.idVariante.ToString()
                                + "&idReparto=" + rp.id.ToString()
                                + "&idProdotto=" + newArt[0].ToString()
                                + "&annoProdotto=" + newArt[1].ToString()
                                + "&quantita=" + quantita.ToString()
                                + "&matricola=" + matricola.ToString());
                            }
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblError").ToString();
                        }
                    }
                }
            }
            else
            {
                Articolo art = new Articolo(idProdotto, annoProdotto);
                if (art.ID != -1 && art.Year != -1)
                {
                    if (art.ID != -1 && art.Year != -1)
                    {
                        if (calDate.SelectedDate >= DateTime.Now)
                        {
                            art.Reparto = rp.id;
                            art.DataPrevistaConsegna = calDate.SelectedDate;
                            Response.Redirect("wzQuestionWorkLoad.aspx?idCommessa="
                            + cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString()
                            + "&idProc=" + prcVar.process.processID.ToString()
                            + "&revProc=" + prcVar.process.revisione.ToString()
                            + "&idVariante=" + prcVar.variant.idVariante.ToString()
                            + "&idReparto=" + rp.id.ToString()
                            + "&idProdotto=" + art.ID.ToString()
                            + "&annoProdotto=" + art.Year.ToString()
                            + "&quantita=" + quantita.ToString()
                            + "&matricola=" + matricola.ToString());
                        }
                    }
                }
            }
        }

        protected void calDate_SelectionChanged(object sender, EventArgs e)
        {
            if (calDate.SelectedDate >= DateTime.Now)
            {
                btnGoFwd.Visible = true;
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorDate").ToString();
                calDate.SelectedDates.Clear();
            }
        }
    }
}