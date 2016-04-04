using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIS.Users
{
    public partial class addGruppo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Gruppi";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {

                if (!Page.IsPostBack)
                {
                    frmAddGruppo.Visible = false;
                }
            }
            else
            {
                frmAddGruppo.Visible = false;
                btnShowAddGruppo.Visible = false;
                btnShowAddGruppo.Enabled = false;
                lbl1.Text = "Non hai il permesso di creare un nuovo gruppo.<br/>";
            }
        }

        protected void btnShowAddGruppo_Click(object sender, ImageClickEventArgs e)
        {
            if (frmAddGruppo.Visible == true)
            {
                frmAddGruppo.Visible = false;
            }
            else
            {
                frmAddGruppo.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            String nomeP = Server.HtmlEncode(nomeG.Text);
            String descP = Server.HtmlEncode(descG.Text);
            if (nomeP.Length > 0 && descP.Length > 0)
            {
                bool rt = false;
                GroupList grl = new GroupList();
                rt = grl.Add(nomeP, descP);
                if (rt == true)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl1.Text = grl.log;
                }
            }
        }

        protected void btnUndo_Click(object sender, ImageClickEventArgs e)
        {
            nomeG.Text = "";
            descG.Text = "";
        }
    }
}