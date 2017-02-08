using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzCheckDeliveryDate1 : System.Web.UI.UserControl
    {
        public int idCommessa, annoCommessa, idProc, revProc, idVariante, idReparto, idProdotto, annoProdotto;
        protected void Page_Load(object sender, EventArgs e)
        {
            tblDeliveryDate.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
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
                Articolo art = new Articolo(idProdotto, annoProdotto);
                if (art.ID != -1 && art.Year != -1 && (art.Status == 'N'||art.Status=='I' || art.Status == 'P'))
                {
                    tblDeliveryDate.Visible = true;
                    if (!Page.IsPostBack)
                    {

                        for (int i = 0; i < 60; i++)
                        {
                            calMinuti.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            calSecondi.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        for (int i = 0; i < 24; i++)
                        {
                            calOre.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }

                        if (art.DataPrevistaFineProduzione >= DateTime.Now && art.DataPrevistaConsegna >= art.DataPrevistaFineProduzione)
                        {
                            calFineProd.SelectedDate = art.DataPrevistaFineProduzione;
                            calOre.SelectedValue = art.DataPrevistaFineProduzione.Hour.ToString();
                            calMinuti.SelectedValue = art.DataPrevistaFineProduzione.Minute.ToString();
                            calSecondi.SelectedValue = art.DataPrevistaFineProduzione.Second.ToString();
                        }
                        else
                        {
                            imgGoFwd.Visible = false;
                        }
                    }

                    lnkGoBack.NavigateUrl = "wzQuestionWorkLoad.aspx"
                        +"?idCommessa=" + idCommessa.ToString()
                    + "&annoCommessa=" + annoCommessa.ToString()
                    + "&idProc=" + idProc.ToString()
                    + "&revProc=" + revProc.ToString()
                    + "&idVariante=" + idVariante.ToString()
                    + "&idProdotto=" + idProdotto.ToString()
                        + "&annoProdotto=" + annoProdotto.ToString()
                        + "&idReparto=" + idReparto.ToString()
                         + "&quantita=" + art.Quantita.ToString();
                }
                else
                {
                    lbl1.Text = "Attenzione: il prodotto non esiste o è già stato pianificato in produzione.";
                }
            }
            else
            {
                
            }
        }

        protected void imgGoFwd_Click(object sender, ImageClickEventArgs e)
        {
            List<TaskConfigurato> lstTasks = new List<TaskConfigurato>();
            Articolo art = new Articolo(idProdotto, annoProdotto);
            if (art.Status == 'N')
            {
                Reparto rp = new Reparto(art.Reparto);

                art.Proc.process.loadFigli(art.Proc.variant);
                for (int i = 0; i < art.Proc.process.subProcessi.Count; i++)
                {
                    TaskVariante tskVar = new TaskVariante(new processo(art.Proc.process.subProcessi[i].processID, art.Proc.process.subProcessi[i].revisione), art.Proc.variant);
                    tskVar.loadTempiCiclo();
                    TempoCiclo tc = new TempoCiclo(tskVar.Task.processID, tskVar.Task.revisione, art.Proc.variant.idVariante, tskVar.getDefaultOperatori());
                    if (tc.Tempo != null)
                    {
                        lstTasks.Add(new TaskConfigurato(tskVar, tc, rp.id, art.Quantita));
                    }
                }

                ConfigurazioneProcesso prcCfg = new ConfigurazioneProcesso(art, lstTasks, rp, art.Quantita);
                int rt1 = prcCfg.SimulaIntroduzioneInProduzione();
                if (rt1 == 1)
                {
                    art.Planner = (User)Session["user"];
                    int rt = prcCfg.LanciaInProduzione();
                    if (rt == 1)
                    {
                        lbl1.Text = "Articolo inserito correttamente in produzione<br/>";
                        Response.Redirect("wzImpostaAllarmiArticolo.aspx?idCommessa=" + art.Commessa.ToString()
                        + "&annoCommessa=" + art.AnnoCommessa.ToString()
                        + "&idProc=" + art.Proc.process.processID.ToString()
                        + "&revProc=" + art.Proc.process.revisione.ToString()
                        + "&idVariante=" + art.Proc.variant.idVariante.ToString()
                        + "&idReparto=" + art.Reparto.ToString()
                        + "&idProdotto=" + art.ID.ToString()
                        + "&annoProdotto=" + art.Year.ToString()
                        + "&quantita=" + art.Quantita.ToString());
                    }
                    else if (rt == 3)
                    {
                        lbl1.Text = "Articolo già lanciato in produzione<br />";
                    }
                    else
                    {
                        lbl1.Text = "Si è verificato un errore: " + prcCfg.log;
                    }
                }
                else
                {
                    lbl1.Text = "Errore in fase di simulazione<br />";
                }
            }
            else if (art.Status == 'I' || art.Status == 'P')
            {
                lbl1.Text = "Prodotto già iniziato.<br/>"
                    + "Data fine produzione: " + art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy HH:mm:ss") + "<br />"
                    + "Data consegna: " + art.DataPrevistaConsegna.ToString("dd/MM/yyyy HH:mm:ss") + "<br />";
                if (art.DataPrevistaFineProduzione <= art.DataPrevistaConsegna && art.DataPrevistaFineProduzione >= DateTime.Now)
                {
                    int ret = art.SpostaPianificazione(art.DataPrevistaFineProduzione, art.DataPrevistaConsegna);
                    if (ret == 1)
                    {
                        lbl1.Text += "Pianificazione articolo spostata correttamente<br/>";
                        Response.Redirect("wzImpostaAllarmiArticolo.aspx?idCommessa=" + art.Commessa.ToString()
                        + "&annoCommessa=" + art.AnnoCommessa.ToString()
                        + "&idProc=" + art.Proc.process.processID.ToString()
                        + "&revProc=" + art.Proc.process.revisione.ToString()
                        + "&idVariante=" + art.Proc.variant.idVariante.ToString()
                        + "&idReparto=" + art.Reparto.ToString()
                        + "&idProdotto=" + art.ID.ToString()
                        + "&annoProdotto=" + art.Year.ToString());
                    }
                    else if (ret == 0)
                    {
                        lbl1.Text = "Errore generico.";
                    }
                    else if (ret == 2)
                    {
                        lbl1.Text = "Status non = I oppure articolo inesistente";
                    }
                    else if (ret == 3)
                    {
                        lbl1.Text = "Le date fine produzione e date consegna impostate non sono corrette";
                    }
                    else if (ret == 4)
                    {
                        lbl1.Text = "E' avvenuto un errore in fase di simulazione del lancio in produzione";
                    }
                }
                else
                {
                    lbl1.Text = "Errore: le date di consegna e la data di fine produzione impostate non sono coerenti.<br />";
                }
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            imgGoFwd.Visible = false;
            Articolo art = new Articolo(idProdotto, annoProdotto);
            if (art.ID != -1 && art.Year != -1 && (art.Status == 'N'||art.Status=='I' || art.Status == 'P'))
            {
                int ore, minuti, secondi;
                try
                {
                    ore = Int32.Parse(calOre.SelectedValue);
                    minuti = Int32.Parse(calMinuti.SelectedValue);
                    secondi = Int32.Parse(calSecondi.SelectedValue);
                }
                catch
                {
                    ore = -1; minuti = -1; secondi = -1;
                }
                if (ore != -1 && minuti != -1 && secondi != -1)
                {
                    DateTime dataFineProd = new DateTime(calFineProd.SelectedDate.Year, calFineProd.SelectedDate.Month, calFineProd.SelectedDate.Day, ore,minuti, secondi);
                    if (dataFineProd <= art.DataPrevistaConsegna)
                    {
                        art.DataPrevistaFineProduzione = dataFineProd;
                        lbl1.Text = "Data di fine produzione impostata correttamente.";
                        imgGoFwd.Visible = true;
                    }
                    else
                    {
                        lbl1.Text = "Attenzione: la data di fine produzione impostata è posteriore alla data di consegna richiesta.";
                    }
                }
            }
        }
    }
}