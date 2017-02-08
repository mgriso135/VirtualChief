using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Reparti
{
    public partial class configOrariTrn : System.Web.UI.UserControl
    {
        public int idTurno;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Turno";
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
                if (idTurno != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        txtNomeTurno.Visible = false;
                        saveNomeTurno.Visible = false;
                        undoNomeTurno.Visible = false;
                        Turno tr = new Turno(idTurno);
                        txtNomeTurno.Text = tr.Nome;
                        nomeTurno.Text = tr.Nome;
                        formAddOrario.Visible = false;
                        rptOrari.DataSource = tr.OrariDiLavoro;
                        rptOrari.DataBind();

                        // Carico valori dropdownlist per aggiunta orario di lavoro
                        for (int i = 0; i < 24; i++)
                        {
                            ddlOInizio.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            ddlOFine.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        for (int i = 0; i < 60; i++)
                        {
                            ddlMInizio.Items.Add(new ListItem(i.ToString(), i.ToString()));
                            ddlMFine.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }

                        // Inizializzo il DDL dei colori
                        System.Drawing.Color MyColor;
                        foreach (string ColorName in Enum.GetNames(typeof(System.Drawing.KnownColor)))
                        {
                            MyColor = System.Drawing.Color.FromName(ColorName);
                            if (MyColor.IsSystemColor == false)
                            {
                                String r = MyColor.R.ToString("X2");
                                String g = MyColor.G.ToString("X2");
                                String b = MyColor.B.ToString("X2");
                                ListItem itemCol = new ListItem(ColorName, "#" + r + g + b);
                                ddlColore.Items.Add(itemCol);
                            }
                        }
                        ddlColore.SelectedValue = "#" + tr.Colore.R.ToString("X2") + tr.Colore.G.ToString("X2") + tr.Colore.B.ToString("X2");
                        ddlColore.Enabled = false;
                        lblEsempioColore.BackColor = tr.Colore;


                    }
                }
                else
                {
                    txtNomeTurno.Visible = false;
                    saveNomeTurno.Visible = false;
                    undoNomeTurno.Visible = false;
                    txtNomeTurno.Visible = false;
                    nomeTurno.Visible = false;
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di gestire i turni di lavoro.<br/>";
                frmTurno.Visible = false;
                formAddOrario.Visible = false;
                imgAddOrario.Visible = false;
            }
        }

        protected void editNomeTurno_Click(object sender, ImageClickEventArgs e)
        {
            if (txtNomeTurno.Visible == true)
            {
                txtNomeTurno.Visible = false;
                saveNomeTurno.Visible = false;
                undoNomeTurno.Visible = false;
                editNomeTurno.Visible = true;
                nomeTurno.Visible = true;
                ddlColore.Enabled = false;
            }
            else
            {                
                txtNomeTurno.Visible = true;
                saveNomeTurno.Visible = true;
                undoNomeTurno.Visible = true;
                editNomeTurno.Visible = false;
                nomeTurno.Visible = false;
                ddlColore.Enabled = true;
            }
        }

        protected void saveNomeTurno_Click(object sender, ImageClickEventArgs e)
        {
            Turno t = new Turno(idTurno);
            if (t.id != -1)
            {
                t.Nome = txtNomeTurno.Text;
                System.Drawing.Color ncol = System.Drawing.ColorTranslator.FromHtml(ddlColore.SelectedValue);
                t.Colore = ncol;
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl1.Text = "Errore.<br/>";
            }
        }

        protected void undoNomeTurno_Click(object sender, ImageClickEventArgs e)
        {
            Turno t = new Turno(idTurno);
            txtNomeTurno.Text = t.Nome;
            txtNomeTurno.Visible = false;
            saveNomeTurno.Visible = false;
            undoNomeTurno.Visible = false;
            editNomeTurno.Visible = true;
            nomeTurno.Visible = true;
            ddlColore.Enabled = false;
        }

        protected void imgAddOrario_Click(object sender, ImageClickEventArgs e)
        {
            if (formAddOrario.Visible == false)
            {
                formAddOrario.Visible = true;
            }
            else
            {
                formAddOrario.Visible = false;
            }
        }

        protected void saveOrario_Click(object sender, ImageClickEventArgs e)
        {
            Turno t = new Turno(idTurno);
            bool ok = false;
            DayOfWeek dayInizio, dayFine;
            dayInizio = new DayOfWeek();
            dayFine = new DayOfWeek();
            TimeSpan oInizio, oFine;
            try
            {
                
                dayInizio = (DayOfWeek)(Int32.Parse(ddlDInizio.SelectedValue));
                dayFine = (DayOfWeek)(Int32.Parse(ddlDFine.SelectedValue));
                oInizio = new TimeSpan(Int32.Parse(ddlOInizio.SelectedValue), Int32.Parse(ddlMInizio.SelectedValue), 0);
                oFine = new TimeSpan(Int32.Parse(ddlOFine.SelectedValue), Int32.Parse(ddlMFine.SelectedValue), 0);
                ok = true;
            }
            catch
            {
                oInizio = new TimeSpan();
                oFine = new TimeSpan();
                ok = false;
            }
            if (t.id != -1 && ok == true)
            {
                bool rt = t.AddOrario(dayInizio, oInizio, dayFine, oFine);
                if (rt == false)
                {
                    lbl1.Text = "Errore durante l'aggiunta del nuovo orario di lavoro<br />";
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
        }

        protected void rptOrari_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lbl1.Text = e.CommandArgument.ToString();
            IntervalloLavorativoTurno interv = new IntervalloLavorativoTurno(Int32.Parse(e.CommandArgument.ToString()));
            bool rt = interv.Delete();
            if (rt == true)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl1.Text += " " + interv.err + "<br />";
            }

        }

        public void rptOrari_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }
        }

    }
}