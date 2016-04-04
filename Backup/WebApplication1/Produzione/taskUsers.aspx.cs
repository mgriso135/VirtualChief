using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebApplication1.Produzione
{
    public partial class taskUsers : System.Web.UI.Page
    {
        public static int procID;
        public static int cont;
        protected static processo padre;
        protected static UserList listaUtenti;

        protected void Page_Load(object sender, EventArgs e)
        {
            cont = 0;
            if(!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                listaUtenti = new UserList();
                procID = Int32.Parse(Request.QueryString["id"]);
                padre = new processo(procID);
                procTitle.InnerText = padre.processName;
                userWorkLoadGraph.procID = padre.processID;
                padre.loadFigli();
                if (!Page.IsPostBack)
                {
                    if (padre.numSubProcessi > 0)
                    {
                        rptProcUsers.DataSource = padre.subProcessi;
                        rptProcUsers.DataBind();
                    }
                    else
                    {
                        lbl.Text = "Questo processo non ha processi figli!<br/>";
                        rptProcUsers.Visible = false;
                        userWorkLoadGraph.Visible = false;
                    }
                }
            }

        }

        public void rptProcUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                ((HiddenField)e.Item.FindControl("procID")).Value = padre.subProcessi[cont].processID.ToString();

                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlUsers");
                ddl.Items.Insert(0, new ListItem("", ""));
                ddl.DataSource = listaUtenti.elencoUtenti;
                ddl.DataValueField = "username";
                ddl.DataTextField = "username";
                padre.subProcessi[cont].loadProcessOwners();
                if (padre.subProcessi[cont].numProcessOwners > 0)
                {
                    ddl.SelectedValue = padre.subProcessi[cont].processOwners[0].username;
                }
                ddl.DataBind();
                cont++;
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

        protected void userTask_changed(object sender, EventArgs e)
        {
            cont = 0;
            DropDownList MyList = (DropDownList)sender;
            HtmlTableRow prcID = (HtmlTableRow)(MyList.Parent.Parent);
            HiddenField lb = (HiddenField)(prcID.FindControl("procID"));
            if (MyList.SelectedValue != "")
            {
                User currentUser = new User(MyList.SelectedValue);
                processo task = new processo(Int32.Parse(lb.Value));
                if (currentUser.username != "")
                {
                    task.loadProcessOwners();
                    for(int i = 0; i < task.numProcessOwners; i++)
                    {
                        task.deleteProcessOwner(new User(task.processOwners[i].username));
                    }
                    task.addProcessOwner(new User(MyList.SelectedValue));
                }
            }
            else
            {
                processo task = new processo(Int32.Parse(lb.Value));
                task.loadProcessOwners();
                for (int i = 0; i < task.numProcessOwners; i++)
                {
                    task.deleteProcessOwner(new User(task.processOwners[i].username));
                }
                
            }
            Response.Redirect(Request.RawUrl);
        }
    }
}