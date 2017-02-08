using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using KIS.App_Code;

namespace KIS.kpi
{
    public partial class viewKPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int kpiID = -1;
                String kpiIDS = "";
                recNow.Visible = false;

                try
                {
                    kpiIDS = Request.QueryString["kpiID"];
                    kpiID = int.Parse(kpiIDS);
                }
                catch
                {
                    kpiID = -1;
                    lbl1.Text = "Error: wrong querystring";
                    imgDeleteKPI.Visible = false;
                    imgEditKPI.Visible = false;
                    nomeKPI.Visible = false;
                    descrKPI.Visible = false;
                    nomePROC.Visible = false;
                    imgRecordValue.Visible = false;
                    lblBack.Visible = false;
                    imgTrashKPI.Visible = false;
                    inputNomeKpi.Visible = false;
                    inputDescrKPI.Visible = false;
                    inputBaseVal.Visible = false;
                    imgSaveKPI.Visible = false;
                    imgIconCancel.Visible = false;

                }

                if (kpiID != -1)
                {
                    Kpi indic = new Kpi(kpiID);
                    processo proc = new processo(indic.procID);
                    proc.loadKPIs();
                    if (indic.id != -1 && proc.processID != -1)
                    {
                        lblBack.HRef = "/Processi/showProcesso.aspx?id=" + proc.processID;
                        nomeKPI.Text = indic.name;
                        descrKPI.Text = indic.description;
                        baseVal.Text = indic.baseVal.ToString();
                        inputNomeKpi.Text = indic.name;
                        inputDescrKPI.Text = indic.description;
                        inputBaseVal.Text = indic.baseVal.ToString();
                        inputBaseVal.Visible = false;
                        inputNomeKpi.Visible = false;
                        inputDescrKPI.Visible = false;
                        nomePROC.Text = proc.processName + ":&nbsp;";
                        showRecs.Visible = true;
                        showRecs.kpiID = indic.id;
                        imgSaveKPI.Visible = false;
                        imgIconCancel.Visible = false;
                    }
                    else
                    {
                        lbl1.Text = "Error: key performance indicator not found";
                        imgEditKPI.Visible = false;
                        imgDeleteKPI.Visible = false;
                        nomeKPI.Visible = false;
                        descrKPI.Visible = false;
                        nomePROC.Visible = false;
                        imgRecordValue.Visible = false;
                        lblBack.Visible = false;
                        imgTrashKPI.Visible = false;
                        inputDescrKPI.Visible = false;
                        inputNomeKpi.Visible = false;
                        inputBaseVal.Visible = false;
                        imgSaveKPI.Visible = false;
                        imgIconCancel.Visible = false;
                    }
                }
            }
        }

        protected void iconRecord_Click(object sender, EventArgs e)
        {
            imgDeleteKPI.Visible = false;
            imgEditKPI.Visible = false;
            imgTrashKPI.Visible = false;
            recNow.Visible = true;
        }

        protected void trashIcon_Click(object sender, EventArgs e)
        {
            int kpiID = -1;
            String kpiIDS = "";

            try
            {
                kpiIDS = Request.QueryString["kpiID"];
                kpiID = int.Parse(kpiIDS);
            }
            catch
            {
                kpiID = -1;
                lbl1.Text = "Error: wrong querystring";
            }

            if (kpiID != -1)
            {
                Kpi indic = new Kpi(kpiID);
                processo proc = new processo(indic.procID);
                if (indic.id != -1 && proc.processID != -1)
                {
                    lblBack.HRef = "/Processi/showProcesso.aspx?id=" + proc.processID;
                    indic.moveToTrash();
                    lbl1.Text = "KPI spostato nel cestino correttamente.<br/>";
                }
                else
                {
                    lbl1.Text = "Error: key performance indicator not found";
                }
            }
        }

        protected void deleteIcon_Click(object sender, EventArgs e)
        {
                        int kpiID = -1;
            String kpiIDS = "";

            try
            {
                kpiIDS = Request.QueryString["kpiID"];
                kpiID = int.Parse(kpiIDS);
            }
            catch
            {
                kpiID = -1;
                lbl1.Text = "Error: wrong querystring";
            }

            if (kpiID != -1)
            {
                Kpi indic = new Kpi(kpiID);
                processo proc = new processo(indic.procID);
                if (indic.id != -1 && proc.processID != -1)
                {
                    lblBack.HRef = "/Processi/showProcesso.aspx?id=" + proc.processID;
                    indic.delete();
                    lbl1.Text = "KPI e tutte le registrazioni relative eliminati.<br/>";
                }
                else
                {
                    lbl1.Text = "Error: key performance indicator not found.<br/>";
                }
            }
        }

        protected void editIcon_Click(object sender, EventArgs e)
        {
            inputNomeKpi.Visible = true;
            inputDescrKPI.Visible = true;
            inputBaseVal.Visible = true;
            imgSaveKPI.Visible = true;
            imgIconCancel.Visible = true;
            imgDeleteKPI.Visible = false;
            imgEditKPI.Visible = false;
            imgRecordValue.Visible = false;
            imgTrashKPI.Visible = false;
            nomeKPI.Visible = false;
            descrKPI.Visible = false;
            baseVal.Visible = false;
        }

        protected void saveIcon_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["kpiID"]))
            {
                String kpiIDS = Request.QueryString["kpiID"];
                int kpiID = int.Parse(kpiIDS);
                Kpi current = new Kpi(kpiID);
                current.baseVal = float.Parse(inputBaseVal.Text);
                current.name = Server.HtmlEncode(inputNomeKpi.Text);
                current.description = Server.HtmlEncode(inputDescrKPI.Text);
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void cancelSaveIcon_Click(object sender, EventArgs e)
        {
            inputBaseVal.Visible = false;
            inputDescrKPI.Visible = false;
            inputNomeKpi.Visible = false;
            nomeKPI.Visible = true;
            baseVal.Visible = true;
            descrKPI.Visible = true;
            imgIconCancel.Visible = false;
            imgSaveKPI.Visible = false;
            imgDeleteKPI.Visible = true;
            imgEditKPI.Visible = true;
            imgTrashKPI.Visible = true;
            imgRecordValue.Visible = true;
        }
    }
}