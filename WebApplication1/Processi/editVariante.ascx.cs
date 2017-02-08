using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;
namespace KIS.Processi
{
    public partial class editVariante : System.Web.UI.UserControl
    {
        public int varianteID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
            prmUser[1] = "W";
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (varianteID != -1)
                {
                    if (!Page.IsPostBack)
                    {
                        variante var = new variante(varianteID);
                        nomeVar.Text = var.nomeVariante;
                        descVar.Text = var.descrizioneVariante;
                    }
                }
            }
            else
            {
                lblErr.Text = "Non hai il permesso di modificare la variante.<br />";
                frmEditVariante.Visible = false;
            }
        }

        protected void save_Click(object sender, EventArgs e)
        {

            if (varianteID != -1)
            {
                variante var = new variante(varianteID);
                if (var.idVariante != -1)
                {
                    var.nomeVariante = Server.HtmlEncode(nomeVar.Text);
                    var.descrizioneVariante = Server.HtmlEncode(descVar.Text);
                    Response.Redirect(Request.RawUrl);
                }
            }
            else
            {
                lblErr.Text = "Something went wrong<br/>";
            }
        }

    }
}