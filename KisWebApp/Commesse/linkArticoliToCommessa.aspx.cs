using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;

namespace KIS.Produzione
{
    public partial class linkArticoliToCommessa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idCommessa = -1;
            int annoCommessa = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["anno"]))
            {
                try
                {
                    idCommessa = Int32.Parse(Request.QueryString["id"]);
                    annoCommessa = Int32.Parse(Request.QueryString["anno"]);
                }
                catch
                {
                    idCommessa = -1;
                    annoCommessa = -1;
                }
            }
            if (idCommessa != -1 && annoCommessa != -1)
            {
                Commessa cm = new Commessa(idCommessa, annoCommessa);
                cm.loadArticoli();
                if (cm.ID != -1)
                {
                    lblDataCommessa.Text = cm.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss");
                    lblIDCommessa.Text = cm.ID.ToString();
                    lblNote.Text = cm.Note;
                    frmLinkArticolo.annoComm = cm.Year;
                    frmLinkArticolo.idComm = cm.ID;
                    frmListArticoli.commID = cm.ID;
                    frmListArticoli.commYear = cm.Year;
                    frmConfigRitardo.commessaID = cm.ID;
                    frmConfigRitardo.commessaAnno = cm.Year;
                    frmConfigWarning.commID = cm.ID;
                    frmConfigWarning.commAnno = cm.Year;
                    lblExternalIDCommessa.Text = cm.ExternalID;
                }
                else
                {
                    lblDataCommessa.Visible = false;
                    lblIDCommessa.Visible = false;
                    lblNote.Visible = false;
                    frmLinkArticolo.Visible = false;
                    frmLinkArticolo.idComm = -1;
                    frmLinkArticolo.annoComm = -1;
                    frmListArticoli.commYear = -1;
                    frmListArticoli.commID = -1;
                    frmListArticoli.Visible = false;
                    lbl1.Text = "Error<br/>";
                    frmConfigRitardo.Visible = false;
                    frmConfigRitardo.commessaID = -1;
                    frmConfigRitardo.commessaAnno = -1;
                    frmConfigWarning.Visible = false;
                    frmConfigWarning.commAnno = -1;
                    frmConfigWarning.commID = -1;
                    lblExternalID.Visible = false;
                    lblExternalIDCommessa.Visible = false;
                }
            }
            else
            {
                lbl1.Text = "Error<br/>";
            }
        }
    }
}