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
        public int processID, varianteID;
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
                        variante var = new variante(Session["ActiveWorkspace"].ToString(), varianteID);
                        nomeVar.Text = var.nomeVariante;
                        descVar.Text = var.descrizioneVariante;
                        MeasurementUnits muList = new MeasurementUnits(Session["ActiveWorkspace"].ToString());
                        muList.loadMeasurementUnits();
                        ddlMeasurementUnits.DataSource = muList.UnitsList;
                        ddlMeasurementUnits.DataValueField = "ID";
                        ddlMeasurementUnits.DataTextField = "Type";

                        ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), processID), var);
                        if (prcVar!=null && prcVar.process!=null && prcVar.process.processID!=-1 &&
                            prcVar.variant!=null && prcVar.variant.idVariante!=-1)
                        {
                            txtExternalID.Text = prcVar.ExternalID;
                            ddlMeasurementUnits.SelectedValue = prcVar.MeasurementUnitID.ToString();
                        }
                        ddlMeasurementUnits.DataBind();
                        
                    }
                }
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                frmEditVariante.Visible = false;
            }
        }

        protected void save_Click(object sender, EventArgs e)
        {

            if (varianteID != -1)
            {
                variante var = new variante(Session["ActiveWorkspace"].ToString(), varianteID);
                if (var.idVariante != -1)
                {
                    var.nomeVariante = Server.HtmlEncode(nomeVar.Text);
                    var.descrizioneVariante = Server.HtmlEncode(descVar.Text);
                    ProcessoVariante prcVar = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), processID), var);
                    if (prcVar != null && prcVar.process != null && prcVar.process.processID != -1 &&
                        prcVar.variant != null && prcVar.variant.idVariante != -1)
                    {
                        prcVar.ExternalID = Server.HtmlEncode(txtExternalID.Text);
                        int muID = -1;
                        try
                        {
                            muID = Int32.Parse(ddlMeasurementUnits.SelectedValue);
                        }
                        catch
                        {
                            muID = -1;
                        }
                        if (muID != -1)
                        {
                            prcVar.MeasurementUnitID = muID;
                        }
                    }
                    Response.Redirect(Request.RawUrl);
                }
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblError").ToString();
            }
        }

    }
}