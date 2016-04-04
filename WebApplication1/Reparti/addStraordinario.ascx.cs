using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Reparti
{
    public partial class addStraordinario : System.Web.UI.UserControl
    {
        public int idTurno;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto Straordinari";
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
                if (!Page.IsPostBack && idTurno != -1)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        OraI.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        OraF.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    for (int i = 0; i < 60; i++)
                    {
                        MinutoI.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        MinutoF.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                }
                else if (idTurno == -1)
                {
                    tblAddStraord.Visible = false;
                    saveStraord.Visible = false;
                }
            }
            else
            {
                lbl1.Text = "Non hai il permesso di gestire gli straordinari di reparto.<br/>";
                tblAddStraord.Visible = false;
            }
        }

        protected void saveStraord_Click(object sender, ImageClickEventArgs e)
        {
            DateTime inizio = new DateTime(inizioStraord.SelectedDate.Year, inizioStraord.SelectedDate.Month, inizioStraord.SelectedDate.Day,
                Int32.Parse(OraI.SelectedValue), Int32.Parse(MinutoI.SelectedValue), 0);
            DateTime fine = new DateTime(fineStraord.SelectedDate.Year, fineStraord.SelectedDate.Month, fineStraord.SelectedDate.Day,
                Int32.Parse(OraF.SelectedValue), Int32.Parse(MinutoF.SelectedValue), 0);
            lbl1.Text = inizio.ToString("yyyy/MM/dd HH:mm:ss") + " " + fine.ToString("yyyy/MM/dd HH:mm:ss");
            if (inizio < fine && inizio > DateTime.Now)
            {
                Turno trn = new Turno(idTurno);
                ElencoStraordinari rs = new ElencoStraordinari(trn.idReparto);
                bool ret = rs.Add(trn.id, inizio, fine);
                if (ret == false)
                {
                    lbl1.Text = "Errore: probabilmente c'è una sovrapposizione tra lo straordinario inserito ed altri straordinari / festività, o i turni di lavoro. Verificare con il grafico nella pagina precedente.<br/>";
                    lbl1.Text += rs.log;
                }
                else
                {
                    lbl1.Text += "Aggiunto correttamente";
                    lbl1.Text = rs.log;
                    Response.Redirect(Request.RawUrl);
                }
            }
            else
            {
                lbl1.Text = "Errore nei dati di input<br/>";
            }
        }
    }
}