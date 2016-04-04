using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;

namespace KIS.Produzione
{
    public partial class listTaskAvviatiUtente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Produzione";
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
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    User curr = (User)Session["user"];
                    lblUser.Text = curr.username;
                    curr.loadTaskAvviati();
                    List<TaskProduzione> tsk = new List<TaskProduzione>();
                    for (int i = 0; i < curr.TaskAvviati.Count; i++)
                    {
                        tsk.Add(new TaskProduzione(curr.TaskAvviati[i]));
                    }
                    if (tsk.Count > 0)
                    {
                        rptTaskAvviati.DataSource = tsk;
                        rptTaskAvviati.DataBind();
                    }
                    else
                    {
                        rptTaskAvviati.Visible = false;
                        lblData.Visible = false;
                        lblTitle.Visible = false;
                    }
                }
            }
            else
            {
                lblUser.Text = "Non hai il permesso.<br/>";
            }
        }

        protected void rptTaskAvviati_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFD700'");
                        if (tsk.LateFinish <= DateTime.Now)
                        {
                            tRow.BgColor = "#FF0000";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF0000'");
                        }
                        else if (tsk.EarlyFinish <= DateTime.Now)
                        {
                            tRow.BgColor = "#FFFF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FFFF00'");
                        }
                        else
                        {
                            tRow.BgColor = "#00FF00";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
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

                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                lblData.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    User curr = (User)Session["user"];
                    lblUser.Text = curr.username;
                    curr.loadTaskAvviati();
                    List<TaskProduzione> tsk = new List<TaskProduzione>();
                    for (int i = 0; i < curr.TaskAvviati.Count; i++)
                    {
                        tsk.Add(new TaskProduzione(curr.TaskAvviati[i]));
                    }
                    if (tsk.Count > 0)
                    {
                        rptTaskAvviati.DataSource = tsk;
                        rptTaskAvviati.DataBind();
                        rptTaskAvviati.Visible = true;
                        lblData.Visible = true;
                        lblTitle.Visible = true;
                    }
                    else
                    {
                        rptTaskAvviati.Visible = false;
                        lblData.Visible = false;
                        lblTitle.Visible = false;
                    }
            }
        }
    }
}