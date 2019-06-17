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
    public partial class listTaskAvviabili : System.Web.UI.UserControl
    {
        public int idPostazione;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Produzione";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ckUser = curr.ValidatePermessi(elencoPermessi);
            }

            Postazione pst = new Postazione(idPostazione);
            if (ckUser == true && pst.id != -1)
            {
                if (!Page.IsPostBack)
                {
                    pst.loadTaskProduzioneAvviabili();
                    //lbl1.Text = pst.log.ToString();
                    
                    List<TaskProduzione> TaskAvviabili = new List<TaskProduzione>();
                    for (int i = 0; i < pst.IdTaskProduzioneAvviabili.Count; i++)
                    {
                        //lbl1.Text += "Creazione TaskProduzione tsk<br/>";
                        TaskProduzione tsk = new TaskProduzione(pst.IdTaskProduzioneAvviabili[i]);
                        // Controllo che l'utente non l'abbia già avviato
                        bool checkUser = false;
                        tsk.loadUtentiAttivi();
                        for(int k = 0; k < tsk.UtentiAttivi.Count; k++)
                        {
                            if(tsk.UtentiAttivi[k] == ((User)Session["user"]).username)
                            {
                                checkUser = true;
                            }
                        }
                        if(checkUser == false)
                        {
                            TaskAvviabili.Add(tsk);
                        }
                    }
                    rptTasks.DataSource = TaskAvviabili;
                    rptTasks.DataBind();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptTasks.Visible = false;
                lblData.Visible = false;
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                FusoOrario fuso = new FusoOrario();
                lblData.Text = "Last update: " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso.tzFusoOrario).ToString("dd/MM/yyyy HH:mm:ss");
                Postazione pst = new Postazione(idPostazione);
                pst.loadTaskProduzioneAvviabili();
                List<TaskProduzione> TaskAvviabili = new List<TaskProduzione>();
                for (int i = 0; i < pst.IdTaskProduzioneAvviabili.Count; i++)
                {
                    TaskProduzione tsk = new TaskProduzione(pst.IdTaskProduzioneAvviabili[i]);
                    // Controllo che l'utente non l'abbia già avviato
                    bool checkUser = false;
                    tsk.loadUtentiAttivi();
                    for (int k = 0; k < tsk.UtentiAttivi.Count; k++)
                    {
                        if (tsk.UtentiAttivi[k] == ((User)Session["user"]).username)
                        {
                            checkUser = true;
                        }
                    }
                    if (checkUser == false)
                    {
                        TaskAvviabili.Add(tsk);
                    }
                }
                rptTasks.DataSource = TaskAvviabili;
                rptTasks.DataBind();
            }
        }

        protected void rptTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hID = (HiddenField)e.Item.FindControl("taskID");
                Label lblUtentiAttivi = (Label)e.Item.FindControl("lblUtentiAttivi");
                Label lblCommessa = (Label)e.Item.FindControl("lblCommessa");
                Label lblAnnoCommessa = (Label)e.Item.FindControl("lblAnnoCommessa");
                Label lblCliente = (Label)e.Item.FindControl("lblCliente");
                Label lblProdotto = (Label)e.Item.FindControl("lblProdotto");
                Label lblProcesso = (Label)e.Item.FindControl("lblProcesso");
                Label lblMatricola = (Label)e.Item.FindControl("lblMatricola");
                Label lblQuantita = (Label)e.Item.FindControl("lblQuantita");

                int id = -1;
                try
                {
                    id = Int32.Parse(hID.Value.ToString());
                }
                catch
                {
                    id = -1;
                }

                if (id != -1)
                {
                    TaskProduzione tsk = new TaskProduzione(id);
                    Reparto rp = new App_Code.Reparto(tsk.RepartoID);
                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFD700'");
                        if (tsk.LateStart <= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario))
                        {
                            tRow.BgColor = "#FF0000";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF0000'");
                        }
                        else if (tsk.EarlyStart <= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario))
                        {
                            tRow.BgColor = "#FFFF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFF00'");
                        }
                        else
                        {
                            tRow.BgColor = "#FFFFFF";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                        }
                        
                        
                    }

                    tsk.loadUtentiAttivi();
                    for (int i = 0; i < tsk.UtentiAttivi.Count; i++)
                    {
                        lblUtentiAttivi.Text += tsk.UtentiAttivi[i];
                        if (i < tsk.UtentiAttivi.Count - 1)
                        {
                            lblUtentiAttivi.Text += "<br/>";
                        }
                    }

                    Articolo art = new Articolo(tsk.ArticoloID, tsk.ArticoloAnno);
                    Commessa comms = new Commessa(art.Commessa, art.AnnoCommessa);
                    lblAnnoCommessa.Text = comms.Year.ToString();
                    lblCliente.Text = comms.Cliente;
                    lblCommessa.Text = comms.ID.ToString();
                    lblProcesso.Text = art.Proc.process.processName + " - " + art.Proc.variant.nomeVariante;
                    lblMatricola.Text = art.Matricola;
                    lblProdotto.Text = art.ID.ToString() + "/" + art.Year.ToString();
                    lblQuantita.Text = art.Quantita.ToString();
                }
            }
        }

        protected void rptTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "start")
            {
                int id = -1;
                try
                {
                    id = Int32.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    id = -1;
                }
                if (id != -1)
                {
                    TaskProduzione tsk = new TaskProduzione(id);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        bool rt = tsk.Start((User)Session["user"]);
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = tsk.log + "<br />";
                            Reparto rp = new Reparto(tsk.RepartoID);
                            User curr = (User)Session["user"];
                            curr.loadTaskAvviati();
                            if (rp.TasksAvviabiliContemporaneamenteDaOperatore > 0 && curr.TaskAvviati.Count >= rp.TasksAvviabiliContemporaneamenteDaOperatore)
                            {
                                lbl1.Text = GetLocalResourceObject("lblTooMuchTasks").ToString();
                            }
                        }
                    }
                }
            }
        }

        /*protected void btnStart_Command(object sender, CommandEventArgs e)
        {
            lbl1.Text = "Fire2";
        }*/
    }
}