using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.Commesse;
using KIS.App_Code;
namespace KIS.Analysis
{
    public partial class DetailCostCommessa1 : System.Web.UI.UserControl
    {
        public int commID, commYear;
        protected void Page_Load(object sender, EventArgs e)
        {
            tblCommessa.Visible = false;
            rptArticoli.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Commessa Costo";
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
                Commessa comm = new Commessa(commID, commYear);
                if (comm.ID != -1 && comm.Year != -1)
                {
                    tblCommessa.Visible = true;
                    rptArticoli.Visible = true;
                    lblCliente.Text = comm.Cliente;
                    lblIDComm.Text = comm.ID.ToString() + "/" + comm.Year.ToString();
                    lblDataInserimento.Text = comm.DataInserimento.ToString("dd/MM/yyyy");
                    lblNote.Text = comm.Note;
                    comm.loadArticoli();
                    TimeSpan somma = new TimeSpan(0, 0, 0);
                    for (int i = 0; i < comm.Articoli.Count; i++)
                    {
                        comm.Articoli[i].loadTempoDiLavoroTotale();
                        somma = somma.Add(comm.Articoli[i].TempoDiLavoroTotale);
                    }
                    if (somma.Days > 0)
                    {
                        lblOreTot.Text = somma.Days.ToString() + " giorni, ";
                    }
                    lblOreTot.Text = Math.Round(somma.TotalHours, 2).ToString() + " ore";
                    rptArticoli.DataSource = comm.Articoli;
                    rptArticoli.DataBind();
                }
            }
            else
            {
                lbl1.Text = "Non hai i permessi per visualizzare la costificazione della commessa.<br />";
            }
        }
    }
}