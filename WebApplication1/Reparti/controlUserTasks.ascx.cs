using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KIS;

namespace KIS.Produzione
{
    public partial class controlUserTasks : System.Web.UI.UserControl
    {
        public string user;
        public User utente;
        public int cont;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(user) && user.Length > 0)
            {
                cont = 0;
                //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Utente", "var utente = '" + user + "';", true);
                utente = new User(user);
            }
        }

        public void rptTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
            }



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

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            cont = 0;
            utente = new User(user);
            rptTasks.DataBind();
        }

        protected void startTask_Command(object sender, CommandEventArgs e)
        {
            //lbl1.Text = "ENTRO NELLA FUNZIONE start selezionando il task " + e.CommandArgument;
            ((LinkButton)sender).Enabled = false;
            //Task curr = new Task(Int32.Parse(e.CommandArgument.ToString()));
            //curr.start();
        }

        protected void pauseTask_Command(object sender, CommandEventArgs e)
        {
            //lbl1.Text = "ENTRO NELLA FUNZIONE pause selezionando il task " + e.CommandArgument;
            /*((LinkButton)sender).Enabled = false;
            ((LinkButton)((LinkButton)sender).Parent.FindControl("lnkCompleteTask")).Enabled = false;
            ((LinkButton)((LinkButton)sender).Parent.FindControl("lnkWarning")).Enabled = false;
            Task curr = new Task(Int32.Parse(e.CommandArgument.ToString()));
            curr.pause();*/
        }

        protected void completeTask_Command(object sender, CommandEventArgs e)
        {
            //lbl1.Text = "ENTRO NELLA FUNZIONE complete selezionando il task " + e.CommandArgument;
           /* ((LinkButton)sender).Enabled = false;
            ((LinkButton)((LinkButton)sender).Parent.FindControl("lnkPauseTask")).Enabled = false;
            Task curr = new Task(Int32.Parse(e.CommandArgument.ToString()));
            curr.stop();
        */}

        protected void warning_Command(object sender, CommandEventArgs e)
        {
            /*((LinkButton)sender).Enabled = false;
            ((LinkButton)((LinkButton)sender).Parent.FindControl("lnkCompleteTask")).Enabled = false;
            ((LinkButton)((LinkButton)sender).Parent.FindControl("lnkPauseTask")).Enabled = false;
            Task curr = new Task(Int32.Parse(e.CommandArgument.ToString()));
            curr.warning();
            lblCheck.Text = curr.padre.ToString();
            produzione prod = new produzione(curr.padre);
            prod.warningCadenza();
            processo proc = new processo(curr.padre);
            proc.loadKPIs();
            for (int i = 0; i < proc.numKPIs; i++)
            {
                if (proc.KPIs[i].name == "Warning")
                {
                    proc.KPIs[i].recordValueNow(1.0, curr.taskID);
                }
            }*/
        }
    }
}