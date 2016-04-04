using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Produzione
{
    public partial class startCadenza : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int procID = Int32.Parse(Request.QueryString["id"]);
                processo curr = new processo(procID);
                curr.loadKPIs();
                double cadenza = curr.getKPIBaseValByName("Cadenza");
                produzione prod = new produzione(procID);
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Cadenza", "var cadenza = " + cadenza.ToString() + ";", true);
                if (prod.statusCadenza == 'I')
                {
                    TimeSpan diff = prod.lastEventCadenza - new DateTime(1970, 1, 1);
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "startDate", "var startDate = " + diff.TotalSeconds.ToString() + ";", true);
                }
                else
                {
                    TimeSpan diff = DateTime.Now - new DateTime(1970, 1, 1);
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "startDate", "var startDate = " + diff.TotalSeconds.ToString() + ";", true);
                    //lbl1.Text = diff.ToString() + " " + diff.TotalSeconds.ToString();
                }
                prod.startCadenza();
            }
        }
    }
}