using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Processi
{
    public partial class showProductionPlan : System.Web.UI.UserControl
    {
        public int processID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (processID != -1)
            {
                produzione linea = new produzione(processID);
                rptProdList.DataSource = linea.queueBatches;
                rptProdList.DataBind();

                processo curr = new processo(processID);
                curr.loadFigli();
                curr.loadKPIs();
                for(int i = 0; i < curr.numSubProcessi; i++)
                {
                    curr.subProcessi[i].loadKPIs();
                }

                curr.calculateCriticalPath();
                rptProd.DataSource = curr.subProcessi;
                rptProd.DataBind();
                rptCritical.DataSource = curr.CriticalPath;
                rptCritical.DataBind();
            }
        }

        public void rptProdList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }
        }

    }
}