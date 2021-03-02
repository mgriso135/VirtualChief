using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
                UserAccount curr = (UserAccount)Session["user"];
                ckUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }

            if (ckUser == true)
            {
                if (!Page.IsPostBack)
                {
                    Warning wrn = new Warning(Session["ActiveWorkspace"].ToString(), idWarning);
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
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void imgUndo_Click(object sender, ImageClickEventArgs e)
        {
            txtCausa.Text = "";
            txtRisoluzione.Text = "";
        }

        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            lbl1.Text = GetLocalResourceObject("lblSave").ToString();
            Warning wrn = new Warning(Session["ActiveWorkspace"].ToString(), idWarning);
            wrn.DataRisoluzione = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, (new FusoOrario(Session["ActiveWorkspace"].ToString())).tzFusoOrario);
            wrn.MotivoChiamata = Server.HtmlEncode(txtCausa.Text);
            wrn.Risoluzione = Server.HtmlEncode(txtRisoluzione.Text);
            txtRisoluzione.Enabled = false;
            txtCausa.Enabled = false;
            imgSave.Enabled = false;
            imgUndo.Enabled = false;
        }
    }
}