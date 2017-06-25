using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Commesse
{
    public partial class wzVarianteDettagli : System.Web.UI.UserControl
    {
        public int idProcesso;
        public int revProcesso;
        public int idVariante;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
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
                    ProcessoVariante prcVar = new ProcessoVariante(new processo(idProcesso, revProcesso), new variante(idVariante));
                    if (prcVar != null && prcVar.process != null && prcVar.variant != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            lblNomeVariante.Text = prcVar.variant.nomeVariante;
                            txtNomeVariante.Text = prcVar.variant.nomeVariante;
                            lblDescVariante.Text = prcVar.variant.descrizioneVariante;
                            txtDescVariante.Text = prcVar.variant.descrizioneVariante;
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    lblNomeVariante.Visible = false;
                    txtNomeVariante.Visible = false;
                    imgEditNomeVariante.Visible = false;
                    tblDatiProdotto.Visible = false;
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();

                }
            
        }

        protected void imgEditNomeVariante_Click(object sender, ImageClickEventArgs e)
        {
            lblDescVariante.Visible = false;
            lblNomeVariante.Visible = false;
            txtDescVariante.Visible = true;
            txtNomeVariante.Visible = true;
            imgEditNomeVariante.Visible = false;
            imgSaveNomeVariante.Visible = true;
            imgUndoNomeVariante.Visible = true;
        }

        protected void imgSaveNomeVariante_Click(object sender, ImageClickEventArgs e)
        {
            ProcessoVariante prcVar = new ProcessoVariante(new processo(idProcesso, revProcesso), new variante(idVariante));
            if (prcVar != null && prcVar.process != null && prcVar.variant != null)
            {
                prcVar.variant.nomeVariante = Server.HtmlEncode(txtNomeVariante.Text);
                prcVar.variant.descrizioneVariante = Server.HtmlEncode(txtDescVariante.Text);
                lblNomeVariante.Text = prcVar.variant.nomeVariante;
                lblDescVariante.Text = prcVar.variant.descrizioneVariante;
                txtNomeVariante.Visible = false;
                txtDescVariante.Visible = false;
                lblNomeVariante.Visible = true;
            }
            txtNomeVariante.Visible = false;
            txtDescVariante.Visible = false;
            lblNomeVariante.Visible = true;
            lblDescVariante.Visible = true;
            imgEditNomeVariante.Visible = true;
            imgSaveNomeVariante.Visible = false;
            imgUndoNomeVariante.Visible = false;
        }

        protected void imgUndoNomeVariante_Click(object sender, ImageClickEventArgs e)
        {
            ProcessoVariante prcVar = new ProcessoVariante(new processo(idProcesso, revProcesso), new variante(idVariante));
            if (prcVar != null && prcVar.process != null && prcVar.variant != null)
            {
                txtNomeVariante.Text = prcVar.variant.nomeVariante;
                txtDescVariante.Text = prcVar.variant.descrizioneVariante;
                lblNomeVariante.Text = prcVar.variant.nomeVariante;
                lblDescVariante.Text = prcVar.variant.descrizioneVariante;
                txtNomeVariante.Visible = false;
                txtDescVariante.Visible = false;
                lblNomeVariante.Visible = true;
            }
            txtNomeVariante.Visible = false;
                txtDescVariante.Visible = false;
                lblNomeVariante.Visible = true;
                lblDescVariante.Visible = true;
            imgEditNomeVariante.Visible = true;
            imgSaveNomeVariante.Visible = false;
            imgUndoNomeVariante.Visible = false;

        }
    }
}