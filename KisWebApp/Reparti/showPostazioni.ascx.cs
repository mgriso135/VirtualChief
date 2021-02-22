using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KIS;
using System.Web.UI.HtmlControls;
using KIS.App_Code;

namespace KIS.Produzione
{
    public partial class showPostazioni : System.Web.UI.UserControl
    {
        public int procID;
        public int varID;
        protected int cont;
        protected processo procs;

        protected void Page_Load(object sender, EventArgs e)
        {
            cont = 0;
            procs = new processo(Session["ActiveWorkspace"].ToString(), procID);
            procs.loadFigli(new variante(Session["ActiveWorkspace"].ToString(), varID));
            procs.loadPostazioniFigli();
            if (!Page.IsPostBack)
            {


                if (procs.processID != -1 && procs.isVSM == false)
                {
                    rptPostazioniTasks.DataSource = procs.subProcessi;
                    rptPostazioniTasks.DataBind();
                }
                else
                {
                    rptPostazioniTasks.Visible = false;
                    lblErr.Text = GetLocalResourceObject("lblErrorProcType").ToString();
                }
            }
            
            
        }

        public void rptPostazioniTasks_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            // Carico la colonna "processi"
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                procs.loadPostazioniTask();
                ((HiddenField)e.Item.FindControl("procID")).Value = procs.subProcessi[cont].processID.ToString();
                DropDownList ddl = ((DropDownList)e.Item.FindControl("ddlPostazioni"));
                ddl.Items.Insert(0, new ListItem("", ""));
                procs.loadPostazioniFigli();
                ddl.DataSource = procs.elencoPostazioni;
                ddl.DataValueField = "id";
                ddl.DataTextField = "name";
                procs.subProcessi[cont].loadPostazioniTask();
                if (procs.subProcessi[cont].elencoPostazioniTask.Count > 0)
                {
                    ddl.SelectedValue = procs.subProcessi[cont].elencoPostazioniTask[0].id.ToString();
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


        protected void ddlPostazioni_IndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            HtmlTableRow prcID = (HtmlTableRow)(ddl.Parent.Parent);
            HiddenField lb = (HiddenField)(prcID.FindControl("procID"));
            
            int postID;
            int taskID;
            try
            {
                taskID = Int32.Parse(lb.Value);
            }
            catch
            {
                taskID = -1;
            }

            try
            {
                postID = Int32.Parse(ddl.SelectedValue);
            }
            catch
            {
                postID = -1;
            }

            if (postID != -1 && taskID != -1)
            {
                    processo task = new processo(Session["ActiveWorkspace"].ToString(), taskID);
                    Postazione pst = new Postazione(Session["ActiveWorkspace"].ToString(), postID);
                    if (task.processID != -1 && pst.id != -1)
                    {
                        task.loadPostazioniTask();
                        bool rt = false;
                        if (task.elencoPostazioniTask.Count == 0)
                        {
                            rt = task.addTaskToPostazione(pst);
                        }
                        else
                        {
                            rt = task.changeTaskFromPostazione(pst);
                        }
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                        lblErr.Text = GetLocalResourceObject("lblErrorDb").ToString();
                    }
                    }
                    else
                    {
                    lblErr.Text = GetLocalResourceObject("lblErrTaskPostNotFound").ToString();
                }
            }
            else if (taskID != -1 && ddl.SelectedValue == "")
            {
                processo task = new processo(Session["ActiveWorkspace"].ToString(), taskID);
                task.loadPostazioniTask();
                if (task.elencoPostazioniTask.Count > 0)
                {
                    Postazione pst = task.elencoPostazioniTask[0];
                    bool rt = task.deleteTaskFromPostazioni();
                    if (rt == false)
                    {
                        lblErr.Text = GetLocalResourceObject("lblErrDelTask").ToString();
                    }
                }
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblErrTaskPostNotFound").ToString();
            }
        }
    }
}