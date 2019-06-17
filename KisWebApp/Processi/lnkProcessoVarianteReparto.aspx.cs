using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Processi
{
    public partial class lnkProcessoVarianteReparto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idProc = -1;
            int revProc = -1;
            int idVar = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["rev"]) && !String.IsNullOrEmpty(Request.QueryString["var"]))
            {
                try
                {
                    idProc = Int32.Parse(Request.QueryString["id"]);
                    revProc = Int32.Parse(Request.QueryString["rev"]);
                    idVar = Int32.Parse(Request.QueryString["var"]);
                }
                catch
                {
                    idProc = -1;
                    revProc = -1;
                    idVar = -1;
                }
            }

            if (idProc != -1 && revProc != -1 && idVar != -1)
            {
                ProcessoVariante prcVar = new ProcessoVariante(new processo(idProc, revProc), new variante(idVar));
                prcVar.loadReparto();
                prcVar.process.loadFigli(prcVar.variant);
                if (prcVar != null && prcVar.process != null && prcVar.variant != null && prcVar.process.processID!=-1 && prcVar.variant.idVariante!=-1)
                {
                    
                        lblNomeProc.Text = prcVar.process.processName;
                        lblDescProc.Text = prcVar.process.processDescription;
                        lblNomeVar.Text = prcVar.variant.nomeVariante;
                        lblRevProc.Text = prcVar.process.revisione.ToString();
                        tblPertNavBar.Visible = true;
                        if (!Page.IsPostBack)
                        {
                            lnkManageProcesso.NavigateUrl += "?id=" + prcVar.process.processID.ToString() + "&variante=" + prcVar.variant.idVariante.ToString();
                        }
                        frmListReparti.idProc = prcVar.process.processID;
                        frmListReparti.revProc = prcVar.process.revisione;
                        frmListReparti.idVar = prcVar.variant.idVariante;
                    
                }
                else
                {
                    tblPertNavBar.Visible = false;
                }
            }
            else
            {
                tblPertNavBar.Visible = false;
            }
        }

        protected void lnkStep3_Click(object sender, EventArgs e)
        {
            if (lblTooltip.Visible == false)
            {
                dvInfo.Visible = true;
                lblTooltip.Visible = true;
            }
            else
            {
                dvInfo.Visible = false;
                lblTooltip.Visible = false;
            }
        }
    }
}