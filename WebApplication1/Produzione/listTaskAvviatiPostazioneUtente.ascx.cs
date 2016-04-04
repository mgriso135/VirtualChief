using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS;

namespace KIS.Produzione
{
    public partial class listTaskAvviatiPostazioneUtente : System.Web.UI.UserControl
    {
        public int idPostazione;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Produzione";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            
            Postazione pst = new Postazione(idPostazione);
            if (checkUser == true && pst.id != -1)
            {
                if (!Page.IsPostBack)
                {
                    List<TaskProduzione> lstTaskAvviati = new List<TaskProduzione>();
                    User curr = (User)Session["user"];
                    pst.loadTaskAvviati(curr);
                    for (int i = 0; i < pst.TaskAvviatiUtente.Count; i++)
                    {
                        lstTaskAvviati.Add(new TaskProduzione(pst.TaskAvviatiUtente[i]));
                    }
                    if (lstTaskAvviati.Count > 0)
                    {
                        rptTaskAvviati.DataSource = lstTaskAvviati;
                        rptTaskAvviati.DataBind();
                    }
                    else
                    {
                        lbl1.Visible = false;
                        lblData.Visible = false;
                        lblTitle.Visible = false;
                        rptTaskAvviati.Visible = false;
                    }
                }
            }
            else
            {
                rptTaskAvviati.Visible = false;
                lblData.Visible = false;
                lbl1.Text = "Non hai il permesso di visualizzare i task da te avviati.<br />";
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            lblData.Text = "Last update: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Postazione pst = new Postazione(idPostazione);
            List<TaskProduzione> lstTaskAvviati = new List<TaskProduzione>();
            User curr = (User)Session["user"];
            pst.loadTaskAvviati(curr);
            for (int i = 0; i < pst.TaskAvviatiUtente.Count; i++)
            {
                lstTaskAvviati.Add(new TaskProduzione(pst.TaskAvviatiUtente[i]));
            }
            if (lstTaskAvviati.Count > 0)
            {
                rptTaskAvviati.DataSource = lstTaskAvviati;
                rptTaskAvviati.DataBind();
            }
            else
            {
                lbl1.Visible = false;
                lblData.Visible = false;
                lblTitle.Visible = false;
                rptTaskAvviati.Visible = false;
            }
        }

        protected void rptTaskAvviati_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            
            if (e.CommandName == "pause")
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
                        bool rt = tsk.Pause((User)Session["user"]);
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = tsk.log;
                        }
                    }
                }
            }
            else if (e.CommandName == "end")
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
                lbl1.Text = id.ToString();
                if (id != -1)
                {
                    TaskProduzione tsk = new TaskProduzione(id);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        bool rt = tsk.Complete((User)Session["user"]);
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text += "Entro qui" + tsk.log;
                        }
                    }
                }

            }
            else if (e.CommandName == "warning")
            {
                lbl1.Text = "Genero uno warning!";
                int id = -1;
                try
                {
                    id = Int32.Parse(e.CommandArgument.ToString());
                }
                catch
                {
                    id = -1;
                }
                if (id != -1 && Session["user"] != null)
                {
                    TaskProduzione tsk = new TaskProduzione(id);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        bool rt = tsk.generateWarning((User)Session["user"]);
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = tsk.log;
                        }
                    }
                }
            }
            else if (e.CommandName == "ChangeQty")
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
                if (id != -1 && Session["user"] != null)
                {
                    TaskProduzione tsk = new TaskProduzione(id);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        TextBox txtNewQty = (TextBox)e.Item.FindControl("txtChangeQty");
                        ImageButton imgQtySave = (ImageButton)e.Item.FindControl("txtChangeQtySave");
                        ImageButton imgQtyUndo = (ImageButton)e.Item.FindControl("txtChangeQtyUndo");
                        if (txtNewQty.Visible == false)
                        {
                            txtNewQty.Text = tsk.QuantitaProdotta.ToString();
                            txtNewQty.Visible = true;
                            imgQtySave.Visible = true;
                            imgQtyUndo.Visible = true;
                        }
                        else
                        {
                            imgQtySave.Visible = false;
                            imgQtyUndo.Visible = false;
                            txtNewQty.Visible = false;
                        }
                    }
                }
            }
            else if (e.CommandName == "ChangeQtySave")
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
                if (id != -1 && Session["user"] != null)
                {
                    TaskProduzione tsk = new TaskProduzione(id);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        TextBox txtNewQty = (TextBox)e.Item.FindControl("txtChangeQty");
                        ImageButton imgQtySave = (ImageButton)e.Item.FindControl("txtChangeQtySave");
                        ImageButton imgQtyUndo = (ImageButton)e.Item.FindControl("txtChangeQtyUndo");
                        Label lblQuantita = (Label)e.Item.FindControl("lblQuantita");
                        int newQty = -1;
                        try
                        {
                            newQty = Int32.Parse(txtNewQty.Text);
                            tsk.QuantitaProdotta = newQty;
                            lblQuantita.Text = newQty.ToString() + "(" + tsk.QuantitaPrevista.ToString() + ")";
                        }
                        catch
                        {
                            newQty = -1;
                        }
                            imgQtySave.Visible = false;
                            imgQtyUndo.Visible = false;
                            txtNewQty.Visible = false;
                    }
                }
            }
            else if (e.CommandName == "ChangeQtyUndo")
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
                if (id != -1 && Session["user"] != null)
                {
                    TaskProduzione tsk = new TaskProduzione(id);
                    if (tsk.TaskProduzioneID != -1)
                    {
                        TextBox txtNewQty = (TextBox)e.Item.FindControl("txtChangeQty");
                        ImageButton imgQtySave = (ImageButton)e.Item.FindControl("txtChangeQtySave");
                        ImageButton imgQtyUndo = (ImageButton)e.Item.FindControl("txtChangeQtyUndo");
                        txtNewQty.Text = tsk.QuantitaPrevista.ToString();
                        imgQtySave.Visible = false;
                        imgQtyUndo.Visible = false;
                        txtNewQty.Visible = false;
                    }
                }
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
                Label lblQuantita = (Label)e.Item.FindControl("lblQuantita");
                Label lblMatricola = (Label)e.Item.FindControl("lblMatricola");
                ImageButton btnCallWarning = (ImageButton)e.Item.FindControl("btnWarning");
                Image imgWarningCalled = (Image)e.Item.FindControl("imgWarningCalled");

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
                    tsk.loadWarningAperti();
                    System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                    if (tRow != null)
                    {
                        tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFD700'");
                        if (tsk.WarningAperti.Count > 0)
                        {
                            tRow.BgColor = "#FF0000";
                            tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#FF0000'");
                            imgWarningCalled.Visible = true;
                            btnCallWarning.Visible = false;
                        }
                        else if (tsk.LateFinish <= DateTime.Now)
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
                    if (tsk.QuantitaProdotta == tsk.QuantitaPrevista)
                    {
                        lblQuantita.Text = tsk.QuantitaPrevista.ToString();
                    }
                    else
                    {
                        lblQuantita.Text = tsk.QuantitaProdotta.ToString()+"("+tsk.QuantitaPrevista.ToString()+")";
                    }
                }
            }
        }

    }
}