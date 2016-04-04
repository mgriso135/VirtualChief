using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Analysis
{
    public partial class DetailAnalysisTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            lnkNavigation.Visible = false;
            frmAnalisiTask.Visible = false;
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Analisi Tasks";
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
                if (!String.IsNullOrEmpty(Request.QueryString["processID"]) && !String.IsNullOrEmpty(Request.QueryString["rev"]))
                {
                    int procID = -1, rev = -1;
                    try
                    {
                        procID = Int32.Parse(Request.QueryString["processID"]);
                        rev = Int32.Parse(Request.QueryString["rev"]);
                    }
                    catch
                    {
                        procID = -1;
                        rev = -1;
                    }

                    if (procID != -1 && rev != -1)
                    {
                        processo prc = new processo(procID, rev);
                        if (prc.processID != -1)
                        {
                            lnkNavigation.Visible = true;
                            lnkNavigation.NavigateUrl += "&processID="
                                + prc.processID.ToString()
                                + "&rev=" + prc.revisione.ToString();

                            frmAnalisiTask.Visible = true;
                            frmAnalisiTask.processID = prc.processID;
                            frmAnalisiTask.revisione = prc.revisione;
                        }
                    }
                }
                
            }
            else
            {
                lbl1.Text = "Non hai il permesso di visualizzare l'analisi dati dei tasks";
            }
        }
    }
}