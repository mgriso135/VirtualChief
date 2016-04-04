using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.ProduzioneUI
{
    public partial class produzioneUI : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                macroProcessi el = new macroProcessi();
                rptMacroProc.DataSource = el.elenco;
                rptMacroProc.DataBind();
                lnkProcPadre.Visible = false;
            }
            else
            {
                int procID = Int32.Parse(Request.QueryString["id"]);
                processo padre = new processo(procID);
                padre.loadFigli();
                if (padre.numSubProcessi > 0)
                {
                    rptMacroProc.DataSource = padre.subProcessi;
                    rptMacroProc.DataBind();
                    lnkProcPadre.Visible = true;
                    if (padre.processoPadre != -1)
                    {
                        lnkProcPadre.NavigateUrl += "?id=" + padre.processoPadre.ToString();
                    }
                }
                else
                {
                    lbl1.Text = "Spiacente, il processo selezionato non contiene processi figli";
                    
                }
            }
        }

        public void rptMacroProc_ItemCreated(object sender, RepeaterItemEventArgs e)
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