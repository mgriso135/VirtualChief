using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
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
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
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

            Turno trn = new Turno(Session["ActiveWorkspace"].ToString(), idTurno);
            Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), trn.idReparto);
            if (inizio < fine && TimeZoneInfo.ConvertTimeToUtc(inizio, rp.tzFusoOrario) > DateTime.UtcNow)
            {
                //Turno trn = new Turno(idTurno);
                ElencoStraordinari rs = new ElencoStraordinari(Session["ActiveWorkspace"].ToString(), trn.idReparto);
                bool ret = rs.Add(trn.id, inizio, fine);
                if (ret == false)
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorSovra").ToString();
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorInput").ToString();
            }
        }
    }
}