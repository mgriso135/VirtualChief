using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;

namespace KIS.Commesse
{
    public partial class wzAddPERT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmTipoAddProcesso.Visible = false;
            frmListArticoliN.Visible = false;
            frmInfoPanel.Visible = false;

            if (!String.IsNullOrEmpty(Request.QueryString["idCommessa"]) && !String.IsNullOrEmpty(Request.QueryString["annoCommessa"]))
            {
                int idCommessa = -1;
                int annoCommessa = -1;
                try
                {
                    idCommessa = Int32.Parse(Request.QueryString["idCommessa"]);
                    annoCommessa = Int32.Parse(Request.QueryString["annoCommessa"]);
                }
                catch
                {
                    idCommessa = -1;
                    annoCommessa = -1;
                }

                if (idCommessa != -1 && annoCommessa != -1)
                {
                    Commessa cm = new Commessa(Session["ActiveWorkspace"].ToString(), idCommessa, annoCommessa);
                    if (cm.ID != -1 && cm.Year != -1)
                    {
                        frmTipoAddProcesso.Visible = true;
                        frmTipoAddProcesso.idCommessa = cm.ID;
                        frmTipoAddProcesso.annoCommessa = cm.Year;
                        frmListArticoliN.Visible = true;
                        frmListArticoliN.idCommessa = cm.ID;
                        frmListArticoliN.annoCommessa = cm.Year;

                        lnkAddPert.NavigateUrl += "?idCommessa=" + cm.ID.ToString()
                            + "&annoCommessa=" + cm.Year.ToString();

                        frmInfoPanel.Visible = true;
                        frmInfoPanel.idCommessa = cm.ID;
                        frmInfoPanel.annoCommessa = cm.Year;
                        frmInfoPanel.idProc = -1;
                        frmInfoPanel.idProdotto = -1;
                        frmInfoPanel.revProc = -1;
                        frmInfoPanel.idVariante = -1;
                        frmInfoPanel.idReparto = -1;
                    }
                }
            }
        }
    }
}