using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Produzione
{
    public partial class frmSolveProblem : System.Web.UI.UserControl
    {
        public int idWarning;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Warning";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool ckUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                ckUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (ckUser == true)
            {
                if (!Page.IsPostBack)
                {
                    Warning wrn = new Warning(idWarning);
                    if (wrn.ID != -1 && wrn.isOpen)
                    {
                    }
                    else
                    {
                        tblResolution.Visible = false;
                    }
                }
            }
            else
            {
                tblResolution.Visible = false;
                lbl1.Text = "Non hai il permesso di registrare la risoluzione di un problema.<br/>";
            }
        }

        protected void imgUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtCausa.Text = "";
            txtRisoluzione.Text = "";
        }

        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            lbl1.Text = "Salvo.";
            Warning wrn = new Warning(idWarning);
            wrn.DataRisoluzione = DateTime.Now;
            wrn.MotivoChiamata = Server.HtmlEncode(txtCausa.Text);
            wrn.Risoluzione = Server.HtmlEncode(txtRisoluzione.Text);
            txtRisoluzione.Enabled = false;
            txtCausa.Enabled = false;
            imgSave.Enabled = false;
            imgUndo.Enabled = false;
        }
    }
}