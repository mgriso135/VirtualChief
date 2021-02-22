using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
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
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
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
                List<TaskProduzione> lstTasks = new List<TaskProduzione>();
                ElencoTaskProduzione elTasksI = new ElencoTaskProduzione(Session["ActiveWorkspace"].ToString(), inizio, fine, 'I');
                ElencoTaskProduzione elTasksN = new ElencoTaskProduzione(Session["ActiveWorkspace"].ToString(), inizio, fine, 'N');
                ElencoTaskProduzione elTasksP = new ElencoTaskProduzione(Session["ActiveWorkspace"].ToString(), inizio, fine, 'P');

                lstTasks.AddRange(elTasksI.Tasks);
                lstTasks.AddRange(elTasksN.Tasks);
                lstTasks.AddRange(elTasksP.Tasks);

                var prodotti = (from t in lstTasks
                             group t by new { t.ArticoloID, t.ArticoloAnno }
             into grp
                             select new
                             {
                                 grp.Key.ArticoloID,
                                 grp.Key.ArticoloAnno,
                             }).ToList();

                if (lstTasks.Count>0)
                {
                    colSave.Visible = true;
                    lblListProd.Text = GetLocalResourceObject("lblWarnTasksPlanned").ToString()
                        + "<br />";
                for(int i = 0; i < prodotti.Count; i++)
                {
                        Articolo art = new Articolo(Session["ActiveWorkspace"].ToString(), prodotti[i].ArticoloID, prodotti[i].ArticoloAnno);
                        lblListProd.Text += art.ID.ToString() + "/" + art.Year.ToString()
                            + " - "+GetLocalResourceObject("lblWarnTasksPlanned2").ToString()+": " 
                            + art.RagioneSocialeCliente 
                            + " - "+GetLocalResourceObject("lblWarnTasksPlanned3").ToString()+": " 
                            + art.DataPrevistaConsegna.ToString("dd/MM/yyyy")
                            +"<br />";
                }
                    lblListProd.Text += GetLocalResourceObject("lblWarnTasksPlanned4").ToString();
                }
                else
                {
                    salva(inizio, fine);
                }
            }
            else
            {
                lbl1.Text = GetLocalResourceObject("lblErrorInput").ToString();
            }
        }



        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            DateTime inizio = new DateTime(inizioFest.SelectedDate.Year, inizioFest.SelectedDate.Month, inizioFest.SelectedDate.Day,
                Int32.Parse(OraI.SelectedValue), Int32.Parse(MinutoI.SelectedValue), 0);
            DateTime fine = new DateTime(fineFest.SelectedDate.Year, fineFest.SelectedDate.Month, fineFest.SelectedDate.Day,
                Int32.Parse(OraF.SelectedValue), Int32.Parse(MinutoF.SelectedValue), 0);

            if (inizio < fine && inizio > DateTime.UtcNow)
            {
                salva(inizio, fine);
            }
        }

        protected void imgUndo_Click(object sender, ImageClickEventArgs e)
        {
            OraI.SelectedValue = "0";
            OraF.SelectedValue = "0";
            MinutoI.SelectedValue = "0";
            MinutoF.SelectedValue = "0";

            colSave.Visible = false;
        }

        protected void salva(DateTime inizio, DateTime fine)
        {
            Turno trn = new Turno(Session["ActiveWorkspace"].ToString(), idTurno);
            Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), trn.idReparto);
            if (inizio < fine && inizio > TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, rp.tzFusoOrario))
            {
                ElencoFestivita rs = new ElencoFestivita(Session["ActiveWorkspace"].ToString(), trn.idReparto);
                bool ret = rs.Add(idTurno, inizio, fine);
                if (ret == false)
                {
                    lbl1.Text = GetLocalResourceObject("ErrorOrario").ToString();
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