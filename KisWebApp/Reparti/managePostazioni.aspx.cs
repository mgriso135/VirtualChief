using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KIS.App_Code;

namespace KIS.Reparti
{
    public partial class managePostazioni : System.Web.UI.Page
    {
        public int repID, varID, procID;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Task Postazione";
            prmUser[1] = "X";
            elencoPermessi.Add(prmUser);

            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            if (checkUser == true)
            {
                if (!Page.IsPostBack)
                {
                    addPostazioni.Visible = false;
                }
                if (!String.IsNullOrEmpty(Request.QueryString["processID"]) && !String.IsNullOrEmpty(Request.QueryString["variante"]) && !String.IsNullOrEmpty(Request.QueryString["repID"]))
                {
                    try
                    {
                        procID = Int32.Parse(Request.QueryString["processID"]);
                        varID = Int32.Parse(Request.QueryString["variante"]);
                        repID = Int32.Parse(Request.QueryString["repID"]);
                    }
                    catch
                    {
                        lbl1.Text = "Error.";
                        procID = -1;
                        varID = -1;
                        repID = -1;
                        lblTitle.Visible = false;
                        caricodilavoro.Visible = false;
                        imgShowAddPostazioni.Visible = false;
                        Image1.Visible = false;
                    }

                    

                    if (procID != -1 && varID != -1 && repID != -1)
                    {
                        caricodilavoro.var = varID;
                        caricodilavoro.procID = procID;
                        caricodilavoro.repID = repID;

                        if(!Page.IsPostBack)
                        { 

                        Reparto rep = new Reparto(repID);
                        ProcessoVariante proc = new ProcessoVariante(new processo(procID), new variante(varID));
                            proc.loadReparto();
                            proc.process.loadFigli(proc.variant);
                            if (rep.id != -1 && proc.process != null & proc.variant != null)
                        {
                            lnkManageProcesso.NavigateUrl += "?id=" + proc.process.processID.ToString() + "&variante=" + proc.variant.idVariante.ToString();
                            lnkProcReparto.NavigateUrl += proc.process.processID.ToString() + "&rev=" + proc.process.revisione.ToString() + "&var=" + proc.variant.idVariante.ToString();
                            lblTitle.Text = GetLocalResourceObject("lblReparto").ToString() + ": " + rep.name 
                                    + "<br/>"+ GetLocalResourceObject("lblProdotto").ToString() + ": " 
                                    + proc.process.processName + " - " + proc.variant.nomeVariante;

                            rptTasksPostazioni.DataSource = proc.process.subProcessi;
                            rptTasksPostazioni.DataBind();
                        }
                        }
                    }
                }
                else
                {
                    lblTitle.Visible = false;
                    lbl1.Text = "Error.<br/>";
                    lblTitle.Visible = false;
                    caricodilavoro.Visible = false;
                    imgShowAddPostazioni.Visible = false;
                    Image1.Visible = false;
                }
            }
            else
            {
                lblTitle.Visible = false;
                lblTitle.Visible = false;
                caricodilavoro.Visible = false;
                imgShowAddPostazioni.Visible = false;
                Image1.Visible = false;
                lbl1.Text = "";
                lbl1.Text = GetLocalResourceObject("lblPermessoKo").ToString();
            }
        }

        protected void rptTasksPostazioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                DropDownList ddlPostazioni = ((DropDownList)e.Item.FindControl("ddlPostazioni"));
                // Ricerco la postazione già impostata
                HiddenField task = (HiddenField)e.Item.FindControl("taskID");
                int taskID = Int32.Parse(task.Value);
                Reparto rp = new Reparto(repID);
                ProcessoVariante procVar = new ProcessoVariante(new processo(procID), new variante(varID));
                procVar.loadReparto();
                procVar.process.loadFigli(procVar.variant);
                rp.loadPostazioniTask(procVar);
                String selValue = "";
                lbl1.Text = rp.err;
                for (int i = 0; i < rp.PostazioniTask.Count; i++)
                {
                    if (rp.PostazioniTask[i].proc.Task.processID == taskID && rp.PostazioniTask[i].proc.variant.idVariante == varID)
                    {
                        selValue = rp.PostazioniTask[i].Pst.id.ToString();
                    }
                }                
                ElencoPostazioni elPost = new ElencoPostazioni();
                ddlPostazioni.Items.Add(new ListItem("", ""));
                ddlPostazioni.DataSource = elPost.elenco;
                ddlPostazioni.DataValueField = "id";
                ddlPostazioni.DataTextField = "name";
                ddlPostazioni.SelectedValue = selValue;
                ddlPostazioni.DataBind();
            }

            // solo se è il pager
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // lo rendo rosso!
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#00FF00";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#00FF00'");*/
                }
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlTableRow tRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("tr1");
                if (tRow != null)
                {
                    /*tRow.BgColor = "#C0C0C0";
                    tRow.Attributes.Add("onMouseOver", "this.style.backgroundColor='#FFFF00'");
                    tRow.Attributes.Add("onMouseOut", "this.style.backgroundColor='#C0C0C0'");*/
                }
            }
        }

        protected void ddlPostazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList postazione = (DropDownList)sender;
            HtmlTableRow riga = (HtmlTableRow)(postazione.Parent.Parent);

            String post = postazione.SelectedValue;
            int taskID = Int32.Parse(((HiddenField)riga.FindControl("taskID")).Value);
            int postID;

            Reparto rp = new Reparto(repID);
            if (post == "")
            {
                // Cancello la postazione!
                bool rt = rp.DeleteLinkTaskFromPostazione(new TaskVariante(new processo(taskID), new variante(varID)));
                if (rt == false)
                {
                    lbl1.Text = "ERROR!";
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
            else
            {
                try
                {
                    postID = Int32.Parse(post);
                }
                catch
                {
                    postID = -1;
                }
                if (postID != -1)
                {
                    processo prc = new processo(taskID);
                    variante vr = new variante(varID);
                    if (prc.processID != -1 && vr.idVariante != -1)
                    {
                        TaskVariante eccolo = new TaskVariante(prc, vr);
                        rp.DeleteLinkTaskFromPostazione(eccolo);
                        lbl1.Text = prc.processID.ToString() + " " + vr.idVariante + "<br/>" + eccolo.log;
                        bool rt = rp.LinkTaskToPostazione(eccolo, new Postazione(postID));
                        if (rt == false)
                        {
                            lbl1.Text = "ERROR!";
                        }
                        else
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                    }
                }
            }
            
        }

        protected void imgShowAddPostazioni_Click(object sender, EventArgs e)
        {
            if (addPostazioni.Visible == false)
            {
                addPostazioni.Visible = true;
            }
            else
            {
                addPostazioni.Visible = true;
            }
        }
    }
}