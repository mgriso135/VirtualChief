﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KIS;
using KIS.App_Code;
namespace KIS.Reparti
{
    public partial class configProcessiVarianti : System.Web.UI.UserControl
    {
        public int repID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (repID != -1)
            {
                if (!Page.IsPostBack)
                {
                    rptAddProc.Visible = false;


                    ElencoMacroProcessiVarianti elencoMacro = new ElencoMacroProcessiVarianti(Session["ActiveWorkspace"].ToString());
                    rptAddProc.DataSource = elencoMacro.elenco;

                    rptAddProc.DataBind();

                    // Carico i processi già associati al reparto
                    Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                    rp.loadProcessiVarianti();

                    rptMacroProc.DataSource = rp.processiVarianti;
                    rptMacroProc.DataBind();
                }
            }
        }

        public void rptMacroProc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                int procID = Int32.Parse(((HiddenField)e.Item.FindControl("processID")).Value);
                int varID = Int32.Parse(((HiddenField)e.Item.FindControl("varianteID")).Value);
                ImageButton btnCancel = (ImageButton)e.Item.FindControl("delete");
                btnCancel.CommandArgument = procID.ToString() + "," + varID.ToString();

                ((HyperLink)e.Item.FindControl("lnkManagePostazione")).NavigateUrl += "?processID=" + procID.ToString() + "&variante=" + varID.ToString() + "&repID=" + repID.ToString();
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

        protected void showAddMacroProc_Click(object sender, ImageClickEventArgs e)
        {
            
            if (rptAddProc.Visible == false)
            {
                rptAddProc.Visible = true;
            }
            else
            {
                rptAddProc.Visible = false;
            }
        }

        public void rptAddProc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {

                int prcID = -1;
                int vrID = -1;
                try
                {
                    prcID = Int32.Parse(((HiddenField)e.Item.FindControl("processID")).Value);
                    vrID = Int32.Parse(((HiddenField)e.Item.FindControl("varianteID")).Value);
                }
                catch
                {
                    prcID = -1;
                    vrID = -1;
                }

                if(prcID != -1 && vrID != -1)
                {
                    ProcessoVariante modello = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), prcID), new variante(Session["ActiveWorkspace"].ToString(), vrID));
                    modello.loadReparto();
                    modello.process.loadFigli(modello.variant);
                    ImageButton aggiungi = (ImageButton)e.Item.FindControl("add");
                    ImageButton expand = (ImageButton)e.Item.FindControl("expand");
                    aggiungi.CommandArgument = modello.process.processID.ToString() + "," + modello.variant.idVariante.ToString();
                    expand.CommandArgument = modello.process.processID.ToString() + "," + modello.variant.idVariante.ToString();

                    // Controllo che il ProcessoVariante non sia già associato al reparto!!!
                    bool found = false;
                    Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                    rp.loadProcessiVarianti();
                    
                    for (int i = 0; i < rp.processiVarianti.Count; i++)
                    {
                        lbl1.Text += rp.processiVarianti[i].process.log;
                        if (rp.processiVarianti[i].process.processID == modello.process.processID && rp.processiVarianti[i].variant.idVariante == modello.variant.idVariante)
                        {
                            found = true;
                        }
                        lbl1.Text += "<br/>";
                    }


                    if (found == true)
                    {
                        aggiungi.Visible = false;
                    }

                    // Controllo che almeno un figlio abbia delle varianti
                    bool checkVarFigli = false;
                    processo prc = new processo(Session["ActiveWorkspace"].ToString(), prcID);
                    prc.loadFigli(new variante(Session["ActiveWorkspace"].ToString(), vrID));
                    for (int i = 0; i < prc.subProcessi.Count; i++)
                    {
                        prc.subProcessi[i].loadVariantiFigli();
                        if (prc.subProcessi[i].variantiFigli.Count > 0)
                        {
                            checkVarFigli = true;
                        }
                    }
                    if (checkVarFigli == false)
                    {
                        expand.Visible = false;
                    }

 
                }
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

        protected void rptAddProc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String[] splittato = e.CommandArgument.ToString().Split(',');
            int procID;
            int vrID;
            try
            {
                procID = Int32.Parse(splittato[0]);
                vrID = Int32.Parse(splittato[1]);
            }
            catch
            {
                procID = -1;
                vrID = -1;
            }
            if (procID != -1 && vrID != -1)
            {
                if (e.CommandName == "expand")
                {
                    ProcessoVariante modello = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), procID), new variante(Session["ActiveWorkspace"].ToString(), vrID));
                    modello.loadReparto();
                    modello.process.loadFigli(modello.variant);
                    ElencoProcessiVarianti el = new ElencoProcessiVarianti(Session["ActiveWorkspace"].ToString(), modello);
                    rptAddProc.DataSource = el.elencoFigli;
                    rptAddProc.DataBind();
                }
                else if (e.CommandName == "add")
                {
                    ProcessoVariante el = new ProcessoVariante(Session["ActiveWorkspace"].ToString(), new processo(Session["ActiveWorkspace"].ToString(), procID), new variante(Session["ActiveWorkspace"].ToString(), vrID));
                    el.loadReparto();
                    el.process.loadFigli(el.variant);
                    Reparto rp = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                    bool rt = rp.addProcesso(el.process, el.variant);
                    if (rt == true)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lbl1.Text = "ERROR!<br/>";
                    }
                }
            }
        }

        protected void rptMacroProc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                String[] splittato = e.CommandArgument.ToString().Split(',');
                int procID = Int32.Parse(splittato[0]);
                int varID = Int32.Parse(splittato[1]);
                Reparto rep = new Reparto(Session["ActiveWorkspace"].ToString(), repID);
                bool rt = rep.deleteProcesso(new processo(Session["ActiveWorkspace"].ToString(), procID), new variante(Session["ActiveWorkspace"].ToString(), varID));
                if (rt == true)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl1.Text = GetLocalResourceObject("lblErrorDelete").ToString()+ "<br/>" + rep.err;
                }
            }
        }
    }
}