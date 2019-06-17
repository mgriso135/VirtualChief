using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Analysis
{
    public partial class AnalysisHome : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            tblOptions.Visible = false;
            //boxCostCommessa.Visible = false;
            boxAnalisiOperatori.Visible = false;
            //boxCostArticolo.Visible = false;
            boxReportAvanzamentoOrdini.Visible = false;
            boxProductionHistory.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi";
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
                tblOptions.Visible = true;
                bool checkAnalisi = false;
                User curr = (User)Session["user"];
                List<String[]> perm = new List<String[]>();
                String[] prm = new String[2];
                /*prm[0] = "Analisi Commessa Costo";
                prm[1] = "R";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    //boxCostArticolo.Visible = true;
                }*/
                perm.Clear();

                boxAnalisiOperatori.Visible = false;
                checkAnalisi = false;
                prm[0] = "Analisi Operatori Tempi";
                prm[1] = "R";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    boxAnalisiOperatori.Visible = true;
                }
                perm.Clear();
                /*
                //boxCostCommessa.Visible = false;
                checkAnalisi = false;
                prm[0] = "Analisi Commessa Costo";
                prm[1] = "R";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    //boxCostCommessa.Visible = true;
                }*/
                perm.Clear();

                boxProductAnalysis2.Visible = false;
                boxProductAnalysis.Visible = false;
                checkAnalisi = false;
                prm[0] = "Analisi TipoProdotto";
                prm[1] = "R";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    boxProductAnalysis2.Visible = true;
                    boxProductAnalysis.Visible = true;
                }
                perm.Clear();

                boxAnalisiTasks.Visible = false;
                boxAnalisiTasks2.Visible = false;
                checkAnalisi = false;
                prm[0] = "Analisi Tasks";
                prm[1] = "R";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    boxAnalisiTasks.Visible = true;
                    boxAnalisiTasks2.Visible = true;
                }
                perm.Clear();

                boxAnalisiClienti.Visible = false;
                checkAnalisi = false;
                prm[0] = "Analisi Clienti";
                prm[1] = "R";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    boxAnalisiClienti.Visible = true;
                }
                perm.Clear();

                boxReportAvanzamentoOrdini.Visible = false;
                checkAnalisi = false;
                prm[0] = "Report Stato Ordini Clienti";
                prm[1] = "X";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    boxReportAvanzamentoOrdini.Visible = true;
                }
                perm.Clear();

                boxProductionHistory.Visible = false;
                checkAnalisi = false;
                prm[0] = "Analisi Articolo Costo";
                prm[1] = "R";
                perm.Add(prm);
                checkAnalisi = curr.ValidatePermessi(perm);
                if (checkAnalisi == true)
                {
                    boxProductionHistory.Visible = true;
                }
                perm.Clear();

            }
            else
            {
            }
        }
    }
}