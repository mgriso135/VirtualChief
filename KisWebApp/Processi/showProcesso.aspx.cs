using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using Dati;
using MyUserControls;
using KIS;
using KIS.App_Code;

namespace KIS.Processi
{
    public partial class showProcesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblExternalIDVal.Visible = false;
            lblMeasurementUnitVal.Visible = false;

            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
            prmUser[1] = "W";
            elencoPermessi.Add(prmUser);
            String[] prmUser1 = new String[2];
            prmUser1[0] = "Processo Variante";
            prmUser1[1] = "R";
            elencoPermessi.Add(prmUser1);
            String[] prmUser2 = new String[2];
            prmUser2[0] = "Processo";
            prmUser2[1] = "W";
            elencoPermessi.Add(prmUser2);
            String[] prmUser3 = new String[2];
            prmUser3[0] = "Processo";
            prmUser3[1] = "R";
            elencoPermessi.Add(prmUser3);
            bool checkUser = false;
            if (Session["user"] != null)
            {
                User curr = (User)Session["user"];
                checkUser = curr.ValidatePermessi(elencoPermessi);
            }

            ProcessID.Value = "-1";
            ProcessRev.Value = "-1";
            VariantID.Value = "-1";

            if (checkUser == true)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string procID = Request.QueryString["id"];
                    processo padre = new processo(int.Parse(procID));
                    ProcessID.Value = padre.processID.ToString();
                    ProcessRev.Value = padre.revisione.ToString();
                    if (!Page.IsPostBack)
                    {
                        //frmLinkSubProc.Visible = false;
                        tblPertNavBar.Visible = false;
                        showEditVariante.Visible = false;
                        editVariante.Visible = false;
                        showEditVariante.Visible = false;
                        deleteVariante.Visible = false;
                        lblProcID.Text = padre.processID.ToString();
                        lblProcName.Text = padre.processName;
                        lblProcDesc.Text = padre.processDescription;
                        inputProcName.Text = padre.processName;
                        inputProcDesc.Text = padre.processDescription;
                        lblRevisione.Text = padre.revisione.ToString();
                        lblDataRevisione.Text = padre.dataRevisione.ToString("dd/MM/yyyy");
                        //newRevisionCopy.Enabled = false;
                        if (padre.isVSM == true)
                        {
                            vsm.Text = GetLocalResourceObject("lblValueStream").ToString();
                            inputVSM.SelectedValue = "1";
                            ddlCopiaPERT.Visible = false;
                            btnCopiaPERT.Visible = false;
                            lblCopiaPERT.Visible = false;
                        }
                        else
                        {
                            vsm.Text = GetLocalResourceObject("lblPERT").ToString();
                            inputVSM.SelectedValue = "0";
                        }
                        inputProcName.Visible = false;
                        inputProcDesc.Visible = false;
                        imgCancel.Visible = false;
                        imgSave.Visible = false;
                        inputVSM.Visible = false;
                        padre.loadPadre();
                        if (padre.processoPadre != -1)
                        {
                            lblLinkFatherProc.Visible = true;
                            lblLinkFatherProc.Text = "<a href=\"showProcesso.aspx?id=" + padre.processoPadre.ToString() + "\">"
                                + GetLocalResourceObject("lblGoBackToFather").ToString()
                                +"</a><br/><br/>";
                        }
                        else
                        {
                            lblLinkFatherProc.Text = "<a href=\"MacroProcessi.aspx\">"
                                + GetLocalResourceObject("lblGoToLineaProdotti").ToString()
                                + "</a><br/><br/>";
                        }

                        // Carico il pannello delle varianti

                        padre.loadVarianti();
                        var lstVarianti = padre.variantiProcesso.OrderBy(x=>x.nomeVariante).ToList();
                        if (lstVarianti.Count > 0)
                        {
                            lblVarianti.Text += "<br />";
                            for (int i = 0; i < lstVarianti.Count; i++)
                            {
                                lblVarianti.Text += "<a href=\"showProcesso.aspx?id=" + padre.processID.ToString() + "&variante=" + lstVarianti[i].idVariante.ToString() + "\">" + lstVarianti[i].nomeVariante + "</a><br />";
                            }
                        }
                        else
                        {
                            lblVarianti.Visible = false;
                            lblTitoloVarianti.Visible = false;
                        }

                        // Carico il dropdownlist di Copia PERT
                        ElencoProcessiVarianti el = new ElencoProcessiVarianti(true);
                        ddlCopiaPERT.DataSource = el.elencoFigli;
                        ddlCopiaPERT.DataValueField = "IDCombinato";
                        ddlCopiaPERT.DataTextField = "NomeCombinato";
                        ddlCopiaPERT.DataBind();
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["variante"]))
                    {
                        int idVariante;
                        try
                        {
                            idVariante = Int32.Parse(Request.QueryString["variante"]);
                        }
                        catch
                        {
                            idVariante = -1;
                        }
                        if (idVariante != -1)
                        {
                            variante var = new variante(idVariante);
                            if (var.idVariante != -1)
                            {
                                VariantID.Value = var.idVariante.ToString();
                                if (!Page.IsPostBack)
                                {
                                    ProductParametersCategories parList = new ProductParametersCategories();
                                    parList.loadCategories();
                                    ddlParamCategory.AppendDataBoundItems = true;
                                    ddlParamCategory.Items.Clear();
                                    ddlParamCategory.Items.Add(new ListItem("", "-1"));
                                    for(int i =0; i < parList.Categories.Count; i++)
                                    {
                                        ddlParamCategory.Items.Add(new ListItem(
                                            Server.HtmlDecode(Server.HtmlDecode(parList.Categories[i].Name)),
                                            parList.Categories[i].ID.ToString()));
                                    }
                                    ddlParamCategory.DataBind();
                                }
                                deleteVariante.Visible = true;
                                lblTitoloVariante.Text = GetLocalResourceObject("lblProdotto").ToString()+" " + var.nomeVariante;
                                lblDescrizioneVariante.Text = var.descrizioneVariante;
                                ProcessoVariante prcVar1 = new ProcessoVariante(padre, var);
                                if (prcVar1 != null && prcVar1.process != null && prcVar1.process.processID != -1 &&
                                    prcVar1.variant != null && prcVar1.variant.idVariante != -1)
                                {
                                    lblExternalIDProcessoVariante.Text = prcVar1.ExternalID;
                                    prcVar1.loadMeasurementUnit();
                                    lblMeasurementUnit.Text = prcVar1.measurementUnit.Type;
                                    lblExternalIDVal.Visible = true;
                                    lblMeasurementUnitVal.Visible = true;
                                }
                                showEditVariante.Visible = true;
                                editVariante.varianteID = var.idVariante;
                                editVariante.processID = padre.processID;
                                padre.loadFigli(var);

                                // Se tipo di grafico = VSM
                                if (padre.isVSM == true)
                                {
                                    /*pert.Visible = false;
                                    int indexFirst = -1;
                                    for (int i = 0; i < padre.subProcessi.Count; i++)
                                    {
                                        // Trovo il primo processo dello stream
                                        if (padre.subProcessi[i] != null)
                                        {
                                            padre.subProcessi[i].loadPrecedenti(var);
                                            padre.subProcessi[i].loadSuccessivi(var);
                                            if (padre.subProcessi[i].processiPrec.Count == 0)
                                            {
                                                indexFirst = i;
                                            }
                                        }
                                    }
                                    // Ora ricostruisco lo stream
                                    int nextIndex = indexFirst;
                                    if (indexFirst > -1 && nextIndex < padre.subProcessi.Count)
                                    {
                                        for (int i = 0; i < padre.subProcessi.Count; i++)
                                        {
                                            relazione currRel = null;
                                            padre.subProcessi[i].loadPrecedenti(var);
                                            padre.subProcessi[i].loadSuccessivi(var);
                                            if (padre.subProcessi[nextIndex].processiPrec.Count > 0)
                                            {
                                                currRel = padre.subProcessi[nextIndex].relazionePrec[0];
                                            }
                                            else
                                            {
                                                currRel = null;
                                            }
                                            CreateProcessBox(padre.subProcessi[nextIndex].processID, padre.subProcessi[nextIndex].processName, i, currRel, var);
                                            // Ricerco l'indice del processo successivo.
                                            bool found = false;
                                            for (int j = 0; j < padre.subProcessi.Count && found == false; j++)
                                            {
                                                if (padre.subProcessi[nextIndex].processiSucc.Count > 0 && padre.subProcessi[j].processID == padre.subProcessi[nextIndex].processiSucc[0])
                                                {
                                                    nextIndex = j;
                                                    found = true;
                                                }
                                            }
                                        }
                                    }

                                    Control newBox = LoadControl("CreateSubProcess.ascx");
                                    ((newProcessBox)newBox).fatherProcID = padre.processID;
                                    ((newProcessBox)newBox).variante = var.idVariante;
                                    ProcStream.Visible = true;
                                    ProcStream.Controls.Add(newBox);
                                    containerVSM.Visible = true;*/
                                }
                                else
                                {
                                    // Se tipo di grafico è un PERT
                                    tblPertNavBar.Visible = true;
                                    if (!Page.IsPostBack)
                                    {
                                        lnkLinkReparto.NavigateUrl += "?id=" + padre.processID.ToString() + "&rev=" + padre.revisione.ToString() + "&var=" + var.idVariante.ToString();
                                    }
                                    pert.varID = var.idVariante;
                                    pert.procID = padre.processID;
                                    /*containerVSM.Visible = false;
                                    ProcStream.Visible = false;*/
                                    TaskVariante prcVar = new TaskVariante(padre, var);
                                    //frmLinkSubProc.Visible = true;
                                    //frmLinkSubProc.procVar = prcVar;
                                    // lblErr.Text = prcVar.Task.processID.ToString() + " " + prcVar.Task.revisione.ToString() + " " + prcVar.variant.idVariante.ToString();
                                }
                            }
                            else
                            {
                                // Querystring variante presente ma non esiste nessuna variante con quell'id
                                lblErr.Text = GetLocalResourceObject("lblErrorQueryString").ToString();
                                /*ProcStream.Visible = false;
                                containerVSM.Visible = false;*/
                                pert.Visible = false;
                                editVariante.Visible = false;
                            }

                        }
                        else
                        {
                            // Querystring variante presente ma non numerico
                            lblErr.Text = GetLocalResourceObject("lblErrorQueryString").ToString();
                            /*containerVSM.Visible = false;
                            ProcStream.Visible = false;*/
                            editVariante.Visible = false;
                            pert.Visible = false;
                        }

                    }
                    else
                    {
                        // Querystring variante non presente
                        inputVSM.Visible = false;
                        inputProcName.Visible = false;
                        inputProcDesc.Visible = false;
                        /*containerVSM.Visible = false;
                        ProcStream.Visible = false;*/
                        pert.Visible = false;
                        showEditVariante.Visible = false;
                        editVariante.Visible = false;
                        lblDescrizioneVariante.Visible = false;
                        showEditVariante.Visible = false;
                    }

                }
                else
                {
                    lblErr.Text = GetLocalResourceObject("lblErrorQueryString").ToString();
                    inputVSM.Visible = false;
                    vsm.Visible = false;
                    inputProcName.Visible = false;
                    inputProcDesc.Visible = false;
                    imgEdit.Visible = false;
                    imgCancel.Visible = false;
                    imgSave.Visible = false;
                    imgDeleteProcess.Visible = false;
                    lblLinkFatherProc.Visible = false;
                    //containerVSM.Visible = false;
                    pert.Visible = false;
                    lblTitoloVarianti.Visible = false;
                    lblVarianti.Visible = false;
                    lblRevisione.Visible = false;
                    editVariante.Visible = false;
                    showEditVariante.Visible = false;
                    addVariante.Visible = false;
                    //frmLinkSubProc.Visible = false;
                    deleteVariante.Visible = false;
                    newRevision.Visible = false;
                    ddlCopiaPERT.Visible = false;
                    btnCopiaPERT.Visible = false;
                    lblCopiaPERT.Visible = false;
                    lblCopiaPERT.Visible = false;
                }
            }
            else
            {
                lblErr.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                inputVSM.Visible = false;
                vsm.Visible = false;
                inputProcName.Visible = false;
                inputProcDesc.Visible = false;
                imgEdit.Visible = false;
                imgCancel.Visible = false;
                imgSave.Visible = false;
                imgDeleteProcess.Visible = false;
                lblLinkFatherProc.Visible = false;
                //containerVSM.Visible = false;
                pert.Visible = false;
                lblTitoloVarianti.Visible = false;
                lblVarianti.Visible = false;
                lblRevisione.Visible = false;
                editVariante.Visible = false;
                showEditVariante.Visible = false;
                addVariante.Visible = false;
                //frmLinkSubProc.Visible = false;
                deleteVariante.Visible = false;
                newRevision.Visible = false;
                lblCopiaPERT.Visible = false;
                trCopiaPERT.Visible = false;
            }
        }

        public void CreateProcessBox(int subProcID, String procName, int procCont, relazione rel, variante var)
        {
            /*int posBoxRapp = procCont * 200 - 90;
            int posBoxProc = 0;
            Control c1 = LoadControl("boxProcesso.ascx");
            ((boxProcesso)c1).procID = subProcID;
            ((boxProcesso)c1).procName = procName;
            ((boxProcesso)c1).varID = var.idVariante;

            if (rel != null)
            {
                ((boxProcesso)c1).relImgPath = rel.imgURL;
            }

            if (procCont > 0)
            {
                posBoxProc = procCont * 200;
                ((boxProcesso)c1).tblRelVisible = true;
            }
            else
            {
                ((boxProcesso)c1).tblRelVisible = false;
            }
            
            ((boxProcesso)c1).posWidthBox = posBoxProc;
            ((boxProcesso)c1).posWidthRel = posBoxRapp;
            ProcStream.Controls.Add(c1);*/
        }

        public void imgEdit_Click(object sender, EventArgs e)
        {
            lblErr.Text = "";
            lblProcName.Visible = false;
            lblProcDesc.Visible = false;
            inputProcDesc.Visible = true;
            inputProcName.Visible = true;
            imgEdit.Visible = false;
            imgCancel.Visible = true;
            imgSave.Visible = true;
            imgDeleteProcess.Visible = false;
            vsm.Visible = false;
            inputVSM.Visible = true;
        }

        public void imgSave_Click(object sender, EventArgs e)
        {
            lblErr.Text = "";
            lblErr.Visible = true;     
            processo proc = new processo(int.Parse(lblProcID.Text));
            proc.processName = Server.HtmlEncode(inputProcName.Text);
            proc.processDescription = Server.HtmlEncode(inputProcDesc.Text);
            lblErr.Text = GetLocalResourceObject("lblVSM").ToString() + ": " + inputVSM.SelectedValue.ToString();
            if (inputVSM.SelectedValue == "1")
            {
                proc.isVSM = true;
            }
            else
            {
                proc.isVSM = false;
            }
            lblProcName.Text = proc.processName;
            lblProcDesc.Text = proc.processDescription;
            inputProcName.Visible = false;
            inputProcDesc.Visible = false;
            imgSave.Visible = false;
            imgCancel.Visible = false;
            imgEdit.Visible = true;
            lblProcName.Visible = true;
            lblProcDesc.Visible = true;
            lblErr.Text += "<span style='color:red;'>"+GetLocalResourceObject("lblModificaOK").ToString()+"</span><br/>";
            Response.Redirect(Request.RawUrl);
        }

        public void imgCancel_Click(object sender, EventArgs e)
        {
                inputProcName.Text = lblProcName.Text;
                inputProcDesc.Text = lblProcDesc.Text;
                lblProcName.Visible = true;
                lblProcDesc.Visible = true;
                inputProcName.Visible = false;
                inputProcDesc.Visible = false;
                imgCancel.Visible = false;
                imgSave.Visible = false;
                imgEdit.Visible = true;
                imgDeleteProcess.Visible = true;
                vsm.Visible = true;
                inputVSM.Visible = false;
        }

        protected void imgDeleteProcess_Click(object sender, EventArgs e)
        {
                int idVariante, idProc;
                idVariante = -1;
                idProc = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["variante"]))
                {
                    try
                    {
                        idVariante = Int32.Parse(Request.QueryString["variante"]);
                    }
                    catch
                    {
                        idVariante = -1;
                    }
                }
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    try
                    {
                        idProc = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        idProc = -1;
                    }
                }
                lblErr.Text = idProc.ToString() + " " + idVariante.ToString() + "<br/>";

                /*if (idVariante != -1 && idProc!=-1)
                {
                    
                    processo prc = new processo(idProc);
                    variante var = new variante(idVariante);
                    TaskVariante tsk = new TaskVariante(prc, var);

                    bool controllo = true;
                    
                    // Controllo che non ci siano figli associati
                    if (controllo == true)
                    {
                        tsk.Task.loadFigli();
                        if (tsk.Task.subProcessi.Count == 0)
                        {
                            controllo = true;
                        }
                        else
                        {
                            controllo = false;
                            lblErr.Text = "<span style='color: red'>Attenzione: ci sono dei sottoprocessi associati al task</span><br/>";
                        }
                    }

                    // Controllo che non ci siano varianti associate
                    if (controllo == true)
                    {
                        tsk.Task.loadVariantiFigli();
                        if (tsk.Task.variantiFigli.Count == 0)
                        {
                            controllo = true;
                        }
                        else
                        {
                            controllo = false;
                            lblErr.Text = "<span style='color: red'>Attenzione: ci sono delle varianti da cancellare</span><br/>";
                        }
                    }

                    // Controllo che non ci siano tempi ciclo associati
                    if (controllo == true)
                    {
                        tsk.loadTempiCiclo();
                        if (tsk.Tempi.Tempi.Count > 0)
                        {
                            controllo = false;
                            lblErr.Text = "<span style='color: red'>Prima di procedere devi cancellare i tempi ciclo associati al task</span><br/>";
                        }
                        else
                        {
                            controllo = true;
                        }
                    }

                    if (controllo == true)
                    {
                        tsk.loadPostazioni();
                        if (tsk.PostazioniDiLavoro.Count > 0)
                        {
                            lblErr.Text += "<span style='color: red'>Attenzione: il task è associato alle seguenti postazioni:<br/>";
                            for (int i = 0; i < tsk.Task.elencoPostazioniTask.Count; i++)
                            {
                                lblErr.Text += "-&nbsp;" + tsk.Task.elencoPostazioniTask[i].name + "<br/>";
                            }
                            lblErr.Text += "Rimuovere l'associazione prima di cancellare il task</span><br/>";
                            controllo = false;
                        }
                        else
                        {

                            controllo = true;
                        }
                    }

                    // Se è tutto ok...
                    if (controllo == true)
                    {
                        bool rt = tsk.Delete();
                        if (rt == true)
                        {
                            int res = prc.delete();
                            lblErr.Text += "<br/><span style=\"color:red;\">Task cancellato</span><br/>";

                        }
                        else
                        {
                            lblErr.Text += "<br/><span style=\"color:red;\">" + tsk.log + "</span><br/>";
                        }
                    }
                    if (var.idVariante != -1)
                    {
                        
                    }
                }*/
                            if (idProc != -1 && idVariante == -1)
                {
                    processo prc = new processo(idProc);
                    int rt = prc.delete();
                    if (rt == 0)
                    {
                    lblErr.Text = GetLocalResourceObject("lblErrorUnknown").ToString();
                    }
                    else if(rt == 1)
                    {
                        lblErr.Text = GetLocalResourceObject("lblDeleteOK").ToString();
                    }
                    else if (rt == 2)
                    {
                        lblErr.Text = GetLocalResourceObject("lblDeleteKO1").ToString();
                    }
                    else if (rt == 4)
                    {
                        lblErr.Text = GetLocalResourceObject("lblDeleteKO2").ToString();
                    }
                }
                else
                {
                    lblErr.Text = GetLocalResourceObject("lblDeleteKO3").ToString();
                }

        }

        protected void addVariante_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int procID = -1;
                try
                {
                    procID = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    procID = -1;
                }
                if(procID != -1)
                {
                    processo curr = new processo(procID);
                    if (curr.processID != -1)
                    {
                        
                        variante var = new variante();
                        int varID = var.add(GetLocalResourceObject("lblNuovoProcDefault").ToString(), GetLocalResourceObject("lblNuovoProcDefault").ToString());
                        var = new variante(varID);
                        if (var.idVariante != -1)
                        {
                            bool rt = curr.addVariante(var);
                            if (rt == true)
                            {
                                Response.Redirect("showProcesso.aspx?id=" + curr.processID.ToString() + "&variante=" + varID.ToString());
                            }
                            else
                            {
                                var.delete();
                                lblErr.Text = GetLocalResourceObject("Si è verificato un errore imprevisto.").ToString();
                            }
                        }
                        else
                        {
                            lblErr.Text = GetLocalResourceObject("Si è verificato un errore imprevisto.").ToString();
                        }
                    }
                }
            }
        }

        protected void showEditVariante_Click(object sender, EventArgs e)
        {
            int varID = -1;
            int procID = -1;
            if (editVariante.Visible == true)
            {
                editVariante.Visible = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(Request.QueryString["variante"]) && !String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    
                    try
                    {
                        varID = Int32.Parse(Request.QueryString["variante"]);
                        procID = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        varID = -1;
                        procID = -1;
                    }
                    
                    if (varID != -1)
                    {
                        variante var = new variante(varID);
                        if (var.idVariante != -1)
                        {
                            editVariante.Visible = true;
                            editVariante.varianteID = var.idVariante;
                            editVariante.processID = procID;
                            lblErr.Text = editVariante.varianteID.ToString();
                        }
                    }
                }
            }
        }

        protected void newRevision_Click(object sender, EventArgs e)
        {
            int prcID = -1;
            try
            {
                prcID = int.Parse(lblProcID.Text);
            }
            catch
            {
                prcID = -1;
            }
            if (prcID != -1)
            {
                processo proc = new processo(prcID);
                if (proc.processID != -1)
                {
                    bool ret = proc.buildNewBlankRevision();
                    if (ret == true)
                    {
                        lblErr.Text = "OK!";
                    }
                    else
                    {
                        lblErr.Text = proc.log;
                    }
                }
            }
        }

        protected void newRevisionCopy_Click(object sender, EventArgs e)
        {
            int prcID = -1;
            try
            {
                prcID = int.Parse(lblProcID.Text);
            }
            catch
            {
                prcID = -1;
            }
            if (prcID != -1)
            {
                processo proc = new processo(prcID);
                if (proc.processID != -1)
                {
                    lblErr.Text = proc.buildNewRevisionCopy().ToString() + "<br/>" + proc.err;
                   // Response.Redirect(Request.RawUrl);
                }
            }
        }

        protected void deleteVariante_Click(object sender, ImageClickEventArgs e)
        {
            int varID = -1;
            int procID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["variante"]) && !String.IsNullOrEmpty(Request.QueryString["id"]))
            {

                try
                {
                    procID = Int32.Parse(Request.QueryString["id"]);
                    varID = Int32.Parse(Request.QueryString["variante"]);
                }
                catch
                {
                    procID = -1;
                    varID = -1;
                }
            }

            if (varID != -1 && procID!=-1)
            {
                variante var = new variante(varID);
                processo prc = new processo(procID);
                var.loadProcessi();
                ProcessoVariante prcVar = new ProcessoVariante(prc, var);
                prcVar.loadReparto();
                prcVar.process.loadFigli(prcVar.variant);
                bool controllo = true;

                // Controllo se ci sono sub processi
                if (controllo == true)
                {
                    if (prcVar.process.subProcessi.Count == 0)
                    {
                        controllo = true;
                    }
                    else
                    {
                        lblLogVariante.Text = GetLocalResourceObject("lblDeleteKO1").ToString();
                        controllo = false;
                    }
                }

                // Controllo se ci sono reparti associati
                if (controllo == true)
                {
                    prcVar.loadReparto();
                    if (prcVar.RepartiProduttivi.Count == 0)
                    {
                        controllo = true;
                    }
                    else
                    {
                        lblLogVariante.Text = GetLocalResourceObject("lblDeleteKO2").ToString();
                        controllo = false;
                    }
                }

                if (controllo == true)
                {
                    bool ret = var.DeleteLinkToProcesso(prc);
                    if (ret == true)
                    {
                        var.delete();
                        Response.Redirect(Request.Path+ "?id=" + prc.processID.ToString());
                    }
                    else
                    {
                        lblLogVariante.Text += var.log;
                    }
                }
            }
        }

        protected void btnCopiaPERT_Click(object sender, ImageClickEventArgs e)
        {
            int procID = -1;
            int varID = -1;
            int currProc = -1;

            String[] splitted = ddlCopiaPERT.SelectedValue.Split(',');
            try
            {
                procID = Int32.Parse(splitted[0]);
                varID = Int32.Parse(splitted[1]);
            }
            catch
            {
                procID = -1;
                varID = -1;
            }

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {

                try
                {
                    currProc = Int32.Parse(Request.QueryString["id"]);
                }
                catch
                {
                    currProc = -1;
                }
            }

            if (currProc != -1 && procID != -1 && varID != -1)
            {
                ProcessoVariante daCopiare = new ProcessoVariante(new processo(procID), new variante(varID));
                daCopiare.loadReparto();
                daCopiare.process.loadFigli(daCopiare.variant);
                processo curr = new processo(currProc);
                if (curr != null && daCopiare != null && curr.processID!=-1 && daCopiare.process != null && daCopiare.variant != null)
                {
                    lblErr.Text = "curr: " + curr.processID.ToString() + "<br />"
                        + "daCopiare: " + daCopiare.process.processID.ToString() + " - " + daCopiare.variant.idVariante.ToString() + "<br />";
                    bool flagCopia = !chkCopia.Checked;
                    bool flagCopiaTC = chkCopiaTC.Checked;
                    bool flagCopiaReparto = chkCopiaReparti.Checked;
                    bool flagCopiaPostazioni = chkCopiaPostazioni.Checked;
                    bool flagCopyParameters = chkCopyParameters.Checked;
                    bool flagCopyWorkInstruction = chkCopyWorkInstructions.Checked;
                    if (chkCopiaPostazioni.Checked && !chkCopiaReparti.Checked)
                    {
                        lblErr.Text = "Errore: se copio le postazioni devo anche copiare i reparti";
                    }
                    else
                    {
                        bool rt = daCopiare.CopyTo(curr, flagCopia, flagCopiaTC, flagCopiaReparto, flagCopiaPostazioni, flagCopyParameters, flagCopyWorkInstruction);
                        if (rt == true)
                        {
                            Response.Redirect(Request.RawUrl);
                        }
                        else
                        {
                            lblCopiaPERTLog.Text = GetLocalResourceObject("lblErrorCopy").ToString();
                        }
                    }
                }
                else
                {
                    lblErr.Text = GetLocalResourceObject("lblErrorCreatingProcess").ToString();
                }
            }
        }

        protected void chkCopiaPostazioni_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopiaPostazioni.Checked == true)
            {
                chkCopiaReparti.Checked = true;
            }
        }

        protected void chkCopiaReparti_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopiaReparti.Checked == false)
            {
                chkCopiaPostazioni.Checked = false;
            }
        }
    }

}