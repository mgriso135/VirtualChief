using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
using KIS.App_Sources;

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
                    UserAccount curr = (UserAccount)Session["user"];
                    checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
                }

                if (checkUser == true)
                {
                    ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), idProcesso, revProcesso), new variante(Session["ActiveWorkspace"].ToString(), idVariante));
                prcVar.loadReparto();
                prcVar.process.loadFigli(prcVar.variant);
                if (prcVar != null && prcVar.process != null && prcVar.variant != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            lblNomeVariante.Text = prcVar.variant.nomeVariante;
                            txtNomeVariante.Text = prcVar.variant.nomeVariante;
                            lblDescVariante.Text = prcVar.variant.descrizioneVariante;
                            txtDescVariante.Text = prcVar.variant.descrizioneVariante;
                            txtExternalID.Text = prcVar.ExternalID;
                            lblExternalID.Text = prcVar.ExternalID;

                        lblMeasurementUnit.Visible = true;
                        ddlMeasurementUnits.Enabled = false;

                        MeasurementUnits muList = new MeasurementUnits(Session["ActiveWorkspace"].ToString());
                        muList.loadMeasurementUnits();
                        ddlMeasurementUnits.DataSource = muList.UnitsList;
                        ddlMeasurementUnits.DataValueField = "ID";
                        ddlMeasurementUnits.DataTextField = "Type";
                        ddlMeasurementUnits.SelectedValue = prcVar.MeasurementUnitID.ToString();
                        ddlMeasurementUnits.DataBind();
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

                lblMeasurementUnit.Visible = false;
                ddlMeasurementUnits.Visible = false;

            }
            
        }

        protected void imgEditNomeVariante_Click(object sender, ImageClickEventArgs e)
        {
            lblExternalID.Visible = false;
            lblDescVariante.Visible = false;
            lblNomeVariante.Visible = false;
            txtDescVariante.Visible = true;
            txtNomeVariante.Visible = true;
            txtExternalID.Visible = true;
            imgEditNomeVariante.Visible = false;
            imgSaveNomeVariante.Visible = true;
            imgUndoNomeVariante.Visible = true;
            ddlMeasurementUnits.Enabled = true;
        }

        protected void imgSaveNomeVariante_Click(object sender, ImageClickEventArgs e)
        {
            ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), idProcesso, revProcesso), new variante(Session["ActiveWorkspace"].ToString(), idVariante));
            prcVar.loadReparto();
            prcVar.process.loadFigli(prcVar.variant);
            if (prcVar != null && prcVar.process != null && prcVar.variant != null)
            {
                prcVar.variant.nomeVariante = Server.HtmlEncode(txtNomeVariante.Text);
                prcVar.variant.descrizioneVariante = Server.HtmlEncode(txtDescVariante.Text);
                prcVar.ExternalID = Server.HtmlEncode(txtExternalID.Text);
                lblNomeVariante.Text = prcVar.variant.nomeVariante;
                lblDescVariante.Text = prcVar.variant.descrizioneVariante;
                lblExternalID.Text = prcVar.ExternalID;
                txtNomeVariante.Visible = false;
                txtDescVariante.Visible = false;
                lblNomeVariante.Visible = true;
                int muID = -1;
                try
                {
                    muID = Int32.Parse(ddlMeasurementUnits.SelectedValue);
                }
                catch
                {
                    muID = -1;
                }
                if(muID!=-1)
                { 
                prcVar.MeasurementUnitID = muID;
                }

            }
            txtNomeVariante.Visible = false;
            txtDescVariante.Visible = false;
            lblNomeVariante.Visible = true;
            lblDescVariante.Visible = true;
            txtExternalID.Visible = false;
            lblExternalID.Visible = true;
            imgEditNomeVariante.Visible = true;
            imgSaveNomeVariante.Visible = false;
            imgUndoNomeVariante.Visible = false;
            ddlMeasurementUnits.Enabled = false;
        }

        protected void imgUndoNomeVariante_Click(object sender, ImageClickEventArgs e)
        {
            ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), idProcesso, revProcesso), new variante(Session["ActiveWorkspace"].ToString(), idVariante));
            prcVar.loadReparto();
            prcVar.process.loadFigli(prcVar.variant);
            if (prcVar != null && prcVar.process != null && prcVar.variant != null)
            {
                txtNomeVariante.Text = prcVar.variant.nomeVariante;
                txtDescVariante.Text = prcVar.variant.descrizioneVariante;
                lblNomeVariante.Text = prcVar.variant.nomeVariante;
                lblDescVariante.Text = prcVar.variant.descrizioneVariante;
                lblExternalID.Text = prcVar.ExternalID;
                txtNomeVariante.Visible = false;
                txtDescVariante.Visible = false;
                lblNomeVariante.Visible = true;
                txtExternalID.Visible = false;
                lblExternalID.Visible = true;
                ddlMeasurementUnits.SelectedValue = prcVar.MeasurementUnitID.ToString();
            }
            txtNomeVariante.Visible = false;
                txtDescVariante.Visible = false;
                lblNomeVariante.Visible = true;
                lblDescVariante.Visible = true;
            imgEditNomeVariante.Visible = true;
            imgSaveNomeVariante.Visible = false;
            imgUndoNomeVariante.Visible = false;
            ddlMeasurementUnits.Enabled = false;
        }
    }
}