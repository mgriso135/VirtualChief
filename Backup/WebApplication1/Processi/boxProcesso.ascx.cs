using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Collections;

namespace MyUserControls
{
    public partial class boxProcesso : System.Web.UI.UserControl
    {
        public int procID;
        public String procName;
        public String relName;
        public String relImgPath;
        public int posWidthBox;
        public int posWidthRel;
        public bool tblRelVisible;

        protected void Page_Load(object sender, EventArgs e)
        {
                ProcN.Text = "<a href=\"showProcesso.aspx?id=" + procID.ToString() + "\">" + procName + "</a>";
                tblRelation.Visible = tblRelVisible;
                imgRel.ImageUrl = relImgPath;
                tblRelation.Style["float"] = "left";
                tblBoxProc.Style["float"] = "left";
                //tblRelation.Style["position"] = "relative";
                //tblBoxProc.Style["position"] = "relative";
                tblBoxProc.Style["width"] = "100px";
                tblBoxProc.Style["height"] = "200px";
                tblRelation.Style["height"] = "200px";
                //tblBoxProc.Style["border"] = "1px dashed blue";
                tblRelation.Style["margin"] = "0px";
                tblRelation.Style["padding"] = "0px";
                tblBoxProc.Style["margin"] = "0px";
                tblBoxProc.Style["padding"] = "0px";
                tblBoxProc.Style["left"] = "20px";
                tblRelation.Style["left"] = "20px";

                processo proc = new processo(procID);
                proc.loadKPIs();
                for (int i = 0; i < proc.numKPIs; i++)
                {
                    lstKPIS.Text += proc.KPIs[i].name + "<br/>";
                }

                // populate relations dropdownlist
                relations elenco = new relations();
                ArrayList items = new ArrayList();
                for (int i = 0; i < elenco.numRelations; i++)
                {
                    items.Add(new relazione(elenco.list[i].relationID));
                }
                ddlRelations.DataSource = items;
                ddlRelations.DataTextField = "Name";
                ddlRelations.DataValueField = "relationID";
                ddlRelations.DataBind();

        }

        protected void imgDelete_Click(object sender, EventArgs e)
        {
            processo proc = new processo(procID);
            
            int res = proc.delete();
            if (res == 0)
            {
                lbl2.Text += "<br/><span style=\"color:red;\">GENERIC ERROR: PROCESS NOT DELETED OR PROCESS NOT FOUND</span><br/>";
            }
            else if(res == 2)
            {
                lbl2.Text += "<br/><span style=\"color:red;\">ERROR: PROCESS NOT DELETED BECAUSE OF SUB-PROCESSES.<br/>YOU NEED TO DELETE ALL SUB-PROCESSES BEFORE DELETING A PROCESS.</span><br/>";
            }
            else if (res == 1)
            {
                Response.Redirect(Request.RawUrl);
            }
        }

       protected void imgMoveLeft_Click(object sender, EventArgs e)
        {
            processo proc = new processo(procID);
            if (proc.processID != -1)
            {
                proc.loadPrecedenti();
                proc.loadSuccessivi();
                // Lo faccio solo se trattasi di flusso lineare
                if (proc.numProcessiPrec == 1 && proc.numProcessiSucc == 1)
                {
                    // Se processo con un precedente e un successivo
                    processo successivo = new processo(proc.processiSucc[0]);
                    successivo.loadPrecedenti();
                    successivo.loadSuccessivi();
                    processo precedente = new processo(proc.processiPrec[0]);
                    precedente.loadPrecedenti();
                    precedente.loadSuccessivi();
                    int idPrecPrec = -1;
                    if (precedente.numProcessiPrec > 0)
                    {
                        idPrecPrec = precedente.processiPrec[0];
                    }

                    proc.addProcessoSuccessivo(precedente);
                    precedente.addProcessoSuccessivo(successivo);
                    if (idPrecPrec != -1)
                    {
                        proc.addProcessoPrecedente(new processo(idPrecPrec));
                        precedente.deleteProcessoPrecedente(new processo(idPrecPrec));
                    }
                    proc.deleteProcessoPrecedente(precedente);
                    proc.deleteProcessoSuccessivo(successivo);
                    Response.Redirect(Request.RawUrl);
                }
                else if (proc.numProcessiPrec == 1 && proc.numProcessiSucc == 0)
                {
                    // E' l'ultimo processo dello stream
                    processo precedente = new processo(proc.processiPrec[0]);
                    precedente.loadPrecedenti();
                    precedente.loadSuccessivi();
                    
                    proc.addProcessoSuccessivo(precedente);
                    if (precedente.numProcessiPrec > 0)
                    {
                        int idPrecPrec = precedente.processiPrec[0];
                        proc.addProcessoPrecedente(new processo(idPrecPrec));
                        precedente.deleteProcessoPrecedente(new processo(idPrecPrec));
                    }
                    proc.deleteProcessoPrecedente(precedente);
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl2.Text += "Il processo NON ha un solo precedente e un solo successivo.<br/>";
                }
            }
            else
            {
                lbl2.Text += "Process not found<br/>";
            }
            Response.Redirect(Request.RawUrl);
        }

       protected void imgMoveRight_Click(object sender, EventArgs e)
       {
            processo proc = new processo(procID);
            proc.loadPrecedenti();
            proc.loadSuccessivi();
            if (proc.processID != -1)
            {
                // Lo faccio solo se trattasi di flusso lineare
                if (proc.numProcessiPrec == 1 && proc.numProcessiSucc == 1)
                {
                    // Se processo con un precedente e un successivo
                    processo successivo = new processo(proc.processiSucc[0]);
                    successivo.loadPrecedenti();
                    successivo.loadSuccessivi();
                    processo precedente = new processo(proc.processiPrec[0]);
                    precedente.loadPrecedenti();
                    precedente.loadSuccessivi();
                    int idSuccSucc = -1;
                    if (successivo.numProcessiSucc > 0)
                    {
                        idSuccSucc = successivo.processiSucc[0];
                    }
                    precedente.addProcessoSuccessivo(successivo);
                    successivo.addProcessoSuccessivo(proc);

                    if (idSuccSucc != -1)
                    {
                        proc.addProcessoSuccessivo(new processo(idSuccSucc));
                    }

                    proc.deleteProcessoPrecedente(precedente);
                    proc.deleteProcessoSuccessivo(successivo);
                    if (idSuccSucc != -1)
                    {
                        successivo.deleteProcessoSuccessivo(new processo(idSuccSucc));
                    }
                    Response.Redirect(Request.RawUrl);
                }
                else if (proc.numProcessiPrec == 0 && proc.numProcessiSucc == 1)
                {
                    lbl2.Text += "Primo processo dello stream.<br/>";
                    // E' il primo processo dello stream
                    processo successivo = new processo(proc.processiSucc[0]);
                    successivo.loadPrecedenti();
                    successivo.loadSuccessivi();
                    proc.addProcessoPrecedente(successivo);
                    if (successivo.numProcessiSucc > 0)
                    {
                        proc.addProcessoSuccessivo(new processo(successivo.processiSucc[0]));
                        successivo.deleteProcessoSuccessivo(new processo(successivo.processiSucc[0]));
                    }
                    proc.deleteProcessoSuccessivo(successivo);
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lbl2.Text += "Il processo NON ha un solo precedente e un solo successivo.<br/>";
                }
            }
            else
            {
                lbl2.Text += "Process not found<br/>";
            }
                
        }

        protected void showRelationList(object sender, EventArgs e)
        {
            ddlRelations.Visible = true;
            imgSaveRel.Visible = true;
            imgCanModRel.Visible = true;
            imgEdit.Visible = false;
        }

        protected void saveRelation(object sender, EventArgs e)
        {
            processo proc = new processo(procID);
            proc.loadPrecedenti();
            proc.loadSuccessivi();
            if (proc.numProcessiPrec > 0)
            {
                int rel = int.Parse(ddlRelations.SelectedValue);
                proc.changeRelationPrec(new processo(proc.processiPrec[0]), new relazione(rel));
                ddlRelations.Visible = false;
                imgSaveRel.Visible = false;
                imgCanModRel.Visible = false;
                imgEdit.Visible = true;
                ddlRelations.Visible = false;
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl2.Text = "ERROR!";
            }
        }

        protected void cancelModRelation(object sender, EventArgs e)
        {
            ddlRelations.Visible = false;
            imgSaveRel.Visible = false;
            imgCanModRel.Visible = false;
            imgEdit.Visible = true;
        }
    }
}