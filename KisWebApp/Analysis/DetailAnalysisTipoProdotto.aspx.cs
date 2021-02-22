using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Analysis
{
    public partial class DetailAnalysisTipoProdotto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmAnalisiTipoProdotto.Visible = false;
            lnkNavigation.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["idProc"]) && !String.IsNullOrEmpty(Request.QueryString["rev"]) && !String.IsNullOrEmpty(Request.QueryString["idVar"]))
            {
                int idProc = -1, rev = -1, idVar = -1;
                try
                {
                    idProc = Int32.Parse(Request.QueryString["idProc"]);
                    rev = Int32.Parse(Request.QueryString["rev"]);
                    idVar = Int32.Parse(Request.QueryString["idVar"]);
                }
                catch
                {
                    idProc = -1;
                    rev = -1;
                    idVar = -1;
                }
                if (idProc != -1 && rev != -1 && idVar != -1)
                {
                    ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), idProc, rev), new variante(Session["ActiveWorkspace"].ToString(), idVar));
                    prcVar.loadReparto();
                    prcVar.process.loadFigli(prcVar.variant);
                    if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID != -1 && prcVar.process.revisione != -1 && prcVar.variant.idVariante != -1)
                    {
                        frmAnalisiTipoProdotto.Visible = true;
                        frmAnalisiTipoProdotto.idProc = prcVar.process.processID;
                        frmAnalisiTipoProdotto.rev = prcVar.process.revisione;
                        frmAnalisiTipoProdotto.idVar = prcVar.variant.idVariante;
                        lnkNavigation.Visible = true;
                        lnkNavigation.NavigateUrl += "?idProc=" + prcVar.process.processID.ToString()
                            +"&rev=" + prcVar.process.revisione.ToString()
                            + "&idVar=" + prcVar.variant.idVariante.ToString();
                    }
                }
            }
            else
            {
            }
        }
    }
}