using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS.App_Code;
namespace KIS.Analysis
{
    public partial class DetailTaskProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmDetailTaskProduct.Visible = false;
            int taskID=-1, revTask=-1, prodID=-1, prodRev=-1, varianteID=-1;
            String strTaskID, strRevTask, strProdID, strProdRev, strVarianteID;
            strTaskID = Request.QueryString["taskID"];
            strRevTask = Request.QueryString["revTask"];
            strProdID = Request.QueryString["prodID"];
            strProdRev = Request.QueryString["prodRev"];
            strVarianteID = Request.QueryString["varianteID"];

            if (!String.IsNullOrEmpty(strTaskID) && !String.IsNullOrEmpty(strRevTask) && !String.IsNullOrEmpty(strProdID) && !String.IsNullOrEmpty(strProdRev) && !String.IsNullOrEmpty(strVarianteID))
            {
                try
                {
                    taskID = Int32.Parse(strTaskID);
                    revTask = Int32.Parse(strRevTask);
                    prodID = Int32.Parse(strProdID);
                    prodRev = Int32.Parse(strProdRev);
                    varianteID = Int32.Parse(strVarianteID);
                }
                catch
                {
                    taskID = -1;
                    revTask=-1;
                    prodID=-1;
                    prodRev = -1;
                    varianteID = -1;
                }

                if (taskID != -1 && revTask != -1 && prodID != -1 && prodRev != -1 && varianteID != -1)
                {
                    processo prd = new processo(prodID, prodRev);
                    prd.loadFigli(new variante(varianteID));
                    processo tsk = new processo(taskID, revTask);
                    bool found = false;
                    if (prd.processID != -1 && tsk.processID != -1 && prd.subProcessi.Count > 0)
                    {
                        for (int i = 0; i < prd.subProcessi.Count; i++)
                        {
                            if (prd.subProcessi[i].processID == tsk.processID && prd.subProcessi[i].revisione == tsk.revisione)
                            {
                                found = true;
                            }
                        }
                        
                    }
                    if (found == true)
                    {
                        frmDetailTaskProduct.Visible = true;
                        frmDetailTaskProduct.taskID = tsk.processID;
                        frmDetailTaskProduct.revTask = tsk.revisione;
                        frmDetailTaskProduct.prodID = prd.processID;
                        frmDetailTaskProduct.prodRev = prd.revisione;
                        frmDetailTaskProduct.varianteID = varianteID;
                        lnkNavigation.NavigateUrl = "DetailAnalysisTask.aspx?processID=" + tsk.processID.ToString()
                        + "&rev=" + tsk.revisione.ToString();

                        lnkNavigation2.NavigateUrl = "DetailTaskProduct.aspx?taskID=" + tsk.processID.ToString()
                            + "&revTask=" + tsk.revisione.ToString()
                            + "&prodID=" + prd.processID.ToString()
                            + "&prodRev=" + prd.revisione.ToString()
                            + "&varianteID=" + varianteID.ToString();
                    }
                    else
                    {
                        lbl1.Text = GetLocalResourceObject("lblErrorData").ToString();
                    }
                }
            }
        }
    }
}