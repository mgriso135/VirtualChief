using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using System.Drawing;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class configTurni : System.Web.UI.UserControl
    {
        public int repID;
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkCalendarioTotale.Visible = false;
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
                if (repID != -1)
                {
                    Reparto rep = new Reparto(repID);
                    lnkCalendarioTotale.Visible = true;
                    lnkCalendarioTotale.NavigateUrl ="showCalendarFesteStraordinari.aspx?id=" + rep.id.ToString();
                    if (!Page.IsPostBack)
                    {
                        addTurno.Visible = false;

                        if (rep.id != -1)
                        {
                            rep.loadTurni();
                            rptTurni.DataSource = rep.Turni;
                            rptTurni.DataBind();

                            // Inizializzo il DDL dei colori
                            System.Drawing.Color MyColor;
                            foreach (string ColorName in Enum.GetNames(typeof(System.Drawing.KnownColor)))
                            {
                                MyColor = System.Drawing.Color.FromName(ColorName);
                                if (MyColor.IsSystemColor == false)
                                {
                                    String r = MyColor.R.ToString("X");
                                    while (r.Length < 2)
                                    {
                                        r = "0" + r;
                                    }
                                    String g = MyColor.G.ToString("X");
                                    while (g.Length < 2)
                                    {
                                        g = "0" + g;
                                    }
                                    String b = MyColor.B.ToString("X");
                                    while (b.Length < 2)
                                    {
                                        b = "0" + b;
                                    }
                                    coloreTurno.Items.Add(new ListItem(ColorName, "#" + r + g + b));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptTurni.Visible = false;
                showAddTurno.Visible = false;
                addTurno.Visible = false;
            }
        }

        protected void showAddTurno_Click(object sender, ImageClickEventArgs e)
        {
            if (addTurno.Visible == false)
            {
                addTurno.Visible = true;
                reset.Focus();
            }
            else
            {
                addTurno.Visible = false;
            }
        }

        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            if (repID != -1)
            {
                Reparto rep = new Reparto(repID);
                if (rep.id != -1)
                {
                    bool rt = rep.addTurno(nomeTurno.Text, coloreTurno.SelectedValue);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorAddTurno").ToString();
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorRepNotFound").ToString();
                }

            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorRepNotFound").ToString();
            }
        }

        protected void reset_Click(object sender, ImageClickEventArgs e)
        {
            nomeTurno.Text = "";
            coloreTurno.ClearSelection();
            reset.Focus();
        }

        protected void rptTurni_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (repID != -1)
            {
                Reparto rep = new Reparto(repID);
                if (rep.id != -1)
                {
                    if (e.CommandName == "delete")
                    {
                        int idTurno = Int32.Parse(e.CommandArgument.ToString());
                        bool rt = rep.deleteTurno(new Turno(idTurno));
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lbl1.Text = GetLocalResourceObject("lblErrorDelTurno").ToString();
                        }
                    }
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorRepNotFound").ToString();
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorRepNotFound").ToString();
            }
        }

        protected void rptTurni_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
    }
}