using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Reparti
{
    public partial class addFestivita : System.Web.UI.UserControl
    {
        public int idTurno;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Reparto Festivita";
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
            }
            else
            {
                lbl1.Text = "Non hai il permesso di gestire le festività.<br/>";
                tblAddFest.Visible = false;
            }
        }

        protected void saveFest_Click(object sender, ImageClickEventArgs e)
        {
            DateTime inizio = new DateTime(inizioFest.SelectedDate.Year, inizioFest.SelectedDate.Month, inizioFest.SelectedDate.Day,
                Int32.Parse(OraI.SelectedValue), Int32.Parse(MinutoI.SelectedValue), 0);
            DateTime fine = new DateTime(fineFest.SelectedDate.Year, fineFest.SelectedDate.Month, fineFest.SelectedDate.Day,
                Int32.Parse(OraF.SelectedValue), Int32.Parse(MinutoF.SelectedValue), 0);
            
            if (inizio < fine && inizio > DateTime.UtcNow)
            {
                Turno trn = new Turno(idTurno);
                ElencoFestivita rs = new ElencoFestivita(trn.idReparto);
                bool ret = rs.Add(idTurno, inizio, fine);
                if (ret == false)
                {
                    lbl1.Text += "Errore: " + rs.log;
                }
                else
                {
                    lbl1.Text = "Ok";
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