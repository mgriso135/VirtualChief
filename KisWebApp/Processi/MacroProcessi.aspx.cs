using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using Dati;
using KIS;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS
{
    public partial class MacroProcessi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo";
            prmUser[1] = "R";
            elencoPermessi.Add(prmUser);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                UserAccount curr = (UserAccount)Session["user"];
                checkUser = curr.ValidatePermissions(Session["ActiveWorkspace"].ToString(), elencoPermessi);
            }
            
            if (checkUser == true)
            {
                macroProcessi el = new macroProcessi(Session["ActiveWorkspace"].ToString());
                rptMacroProc.DataSource = el.Elenco;
                rptMacroProc.DataBind();
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                rptMacroProc.Visible = false;
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
                    //tRow.BgColor = "#00FF00";
                    //tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    //tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    //tRow.BgColor = "#C0C0C0";
                    //tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    //tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");
                }
            }
        }

    }
}