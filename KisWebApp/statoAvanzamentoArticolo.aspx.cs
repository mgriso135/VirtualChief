using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Produzione
{
    public partial class statoAvanzamentoArticolo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["anno"]))
            {
                int artID = -1;
                int artYear = -1;
                try
                {
                    artID = Int32.Parse(Request.QueryString["id"]);
                    artYear = Int32.Parse(Request.QueryString["anno"]);
                }
                catch
                {
                    artID = -1;
                    artYear = -1;
                }

                if (artYear != -1 && artID != -1)
                {
                    frmShowStatoArticolo.artID = artID;
                    frmShowStatoArticolo.artYear = artYear;
                    frmRitardo.Visible = true;
                    frmRitardo.articoloID = artID;
                    frmRitardo.articoloAnno = artYear;
                    frmWarning.Visible = true;
                    frmWarning.articoloID = artID;
                    frmWarning.articoloAnno = artYear;
                }
                else
                {
                    frmShowStatoArticolo.Visible = false;
                    frmRitardo.Visible = false;
                    frmRitardo.articoloID = -1;
                    frmRitardo.articoloAnno = -1;
                    frmWarning.Visible = false;
                    frmWarning.articoloID = -1;
                    frmWarning.articoloAnno = -1;
                }
                
            }
            else
            {
                frmShowStatoArticolo.Visible = false;
                frmShowStatoArticolo.artID = -1;
                frmShowStatoArticolo.artYear = -1;
                frmRitardo.Visible = false;
                frmRitardo.articoloID = -1;
                frmRitardo.articoloAnno = -1;
                frmWarning.Visible = false;
                frmWarning.articoloID = -1;
                frmWarning.articoloAnno = -1;
            }
        }
    }
}