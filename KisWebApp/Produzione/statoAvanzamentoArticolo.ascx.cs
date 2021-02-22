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
    public partial class statoAvanzamentoArticolo1 : System.Web.UI.UserControl
    {
        public int artID, artYear;
        public TimeZoneInfo rpFuso;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Articoli";
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
                if (artID != -1 && artYear != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), artID, artYear);
                        lblAnno.Text = art.Year.ToString();
                        lblID.Text = art.ID.ToString();
                        lblAnnoCommessa.Text = art.AnnoCommessa.ToString();
                        lblDataConsegna.Text = art.DataPrevistaConsegna.ToString("dd/MM/yyyy");
                        lblDataFineProduzione.Text = art.DataPrevistaFineProduzione.ToString("dd/MM/yyyy");
                        lblIDCommessa.Text = art.Commessa.ToString();
                        lblMatricola.Text = art.Matricola;
                        lblProcesso.Text = art.Proc.process.processName;
                        Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), art.Reparto);
                        rpFuso = rep.tzFusoOrario;
                        lblReparto.Text = rep.name;
                        lblStatus.Text = art.Status.ToString();
                        lblVariante.Text = art.Proc.variant.nomeVariante;
                        lblQuantita.Text = art.Quantita.ToString();
                        art.loadLeadTimes();
                        
                        if (art.Status == 'F')
                        {
                            trQtaProd.Visible = true;
                            lblQuantitaProdotta.Text = art.QuantitaProdotta.ToString();
                            trLeadTime.Visible = true;
                            lblLeadTime.Text = Math.Round(art.LeadTime.TotalHours, 2).ToString();
                        }
                        else
                        {
                            trQtaProd.Visible = false;
                            trLeadTime.Visible = false;
                        }

                        if (!Page.IsCallback)
                        {
                            art.loadTasksProduzione();
                            rptTasks.DataSource = art.Tasks;
                            rptTasks.DataBind();
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptTasks.Visible = false;
            }
        }

        protected void rptTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                HiddenField hTaskID = (HiddenField)e.Item.FindControl("hTaskID");
                Label postazione = (Label)e.Item.FindControl("lblPostazione");
                Label lblLOG = (Label)e.Item.FindControl("lblLOG");
                ImageButton btnRiesuma = (ImageButton)e.Item.FindControl("btnRiesuma");
                int taskID = -1;
                try
                {
                    taskID = Int32.Parse(hTaskID.Value.ToString());
                }
                catch
                {
                    taskID = -1;
                }

                TaskProduzione tsk = new TaskProduzione(Session["ActiveWorkspace"].ToString(), taskID);
                if (tsk.TaskProduzioneID != -1)
                {
                    Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), tsk.PostazioneID);
                    postazione.Text = pst.name;// +"<br/>" + tsk.TempoCicloEffettivo.TotalHours + " " + tsk.log;
                }
                else
                {
                    postazione.Text = "#ERROR!";
                }

                // COLORE!!!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    btnRiesuma.Visible = false;
                    if (tsk.Status == 'F')
                    {
                        tRow.BgColor = "#00FF00";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                        //lbl1.Text += tsk.TaskProduzioneID.ToString() + " " + tsk.ritardo.TotalMinutes.ToString() + " " + tsk.log + "<br />";
                        btnRiesuma.Visible = true;
                    }
                    else if (tsk.Status == 'N')
                    {
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rpFuso) > tsk.EarlyStart && tsk.LateStart > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rpFuso))
                        {
                            tRow.BgColor = "#FFFF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFF00'");
                        }
                        else if (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rpFuso) > tsk.EarlyStart && tsk.LateStart < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rpFuso))
                        {
                            tRow.BgColor = "#FF0000";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF0000'");
                        }
                        else
                        {
                            tRow.BgColor = "#FFFFFF";

                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFFFF'");
                        }
                    }
                    else
                    {
                        tRow.BgColor = "#0000FF";
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                        tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#0000FF'");
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label a = ((Label)e.Item.FindControl("lblTotTempoDiLavoro"));
                Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), artID, artYear);
                art.loadTempoDiLavoroTotale();
                a.Text = Math.Round(art.TempoDiLavoroTotale.TotalHours, 2).ToString() + " " + GetLocalResourceObject("lblHours");

                TimeSpan programmedTime = new TimeSpan(0, 0, 0);
                art.loadTasksProduzione();
                for(int i = 0; i < art.Tasks.Count; i++)
                {
                    programmedTime=programmedTime.Add(art.Tasks[i].TempoDiLavoroPrevisto);
                }
                Label b = ((Label)e.Item.FindControl("lblTotWorkingTimeProgrammed"));
                b.Text = Math.Round(programmedTime.TotalHours, 2).ToString() + " " + GetLocalResourceObject("lblHours");
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), artID, artYear);
            lblDataUpdate.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), art.Reparto);
            rpFuso = rep.tzFusoOrario;
            art.loadTasksProduzione();
            rptTasks.DataSource = art.Tasks;
            rptTasks.DataBind();
            /*for (int i = 0; i < art.Tasks.Count; i++)
            {
                lbl1.Text += art.Tasks[i].TaskProduzioneID.ToString() + " ritardo " + art.Tasks[i].ritardo.TotalHours.ToString()
                    + "<br />" + art.Tasks[i].log + "<br/><br/>";
            }*/
        }

        protected void rptTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int taskID = -1;
            try
            {
                taskID = Int32.Parse(e.CommandArgument.ToString());
            }
            catch
            {
                taskID = -1;
            }
            if (e.CommandName == "riesuma")
            {
                List<String[]> elencoPermessi = new List<String[]>();
                String[] prmUser = new String[2];
                prmUser[0] = "TaskProduzione Riesuma";
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
                    TaskProduzione tskProd = new TaskProduzione(Session["ActiveWorkspace"].ToString(), taskID);
                    if (tskProd.Status == 'F')
                    {
                        Boolean rt = tskProd.Riesuma();
                        if (rt == true)
                        {
                            Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), artID, artYear);
                            art.loadTasksProduzione();
                            rptTasks.DataSource = art.Tasks;
                            rptTasks.DataBind();
                            lbl1.Text = GetLocalResourceObject("lblRiesumaOk").ToString();
                        }
                        else
                        {
                            lblInfo.Text = GetLocalResourceObject("lblRiesumaKo").ToString(); 
                        }
                    }
                }
                else
                {
                    lblInfo.Text = GetLocalResourceObject("lblRiesumaPermessoKo").ToString();
                }
            }
        }
    }
}