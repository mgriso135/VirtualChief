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


namespace WebApplication1.Processi
{
    public partial class showProcesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string procID = Request.QueryString["id"];
                processo padre = new processo(int.Parse(procID));
                padre.loadFigli();
                if (!Page.IsPostBack)
                {
                    lblProcID.Text = padre.processID.ToString();
                    lblProcName.Text = padre.processName;
                    lblProcDesc.Text = padre.processDescription;
                    inputProcName.Text = padre.processName;
                    inputProcDesc.Text = padre.processDescription;
                    padre.loadProcessOwners();
                    if (padre.numProcessOwners > 0)
                    {
                        processOwner.Text = padre.processOwners[0].username;
                    }
                    if (padre.isVSM == true)
                    {
                        vsm.Text = "Value-Stream";
                        inputVSM.SelectedValue = "1";
                    }
                    else
                    {
                        vsm.Text = "PERT";
                        inputVSM.SelectedValue = "0";
                    }
                    inputProcName.Visible = false;
                    inputProcDesc.Visible = false;
                    imgEdit.Visible = false;
                    imgCancel.Visible = false;
                    imgSave.Visible = false;
                    inputVSM.Visible = false;
                    ddlProcessOwner.Visible = false;
                    if (padre.processoPadre != -1)
                    {
                        lblLinkFatherProc.Text = "<a href=\"showProcesso.aspx?id=" + padre.processoPadre.ToString() + "\">Torna al processo padre</a><br/><br/>";
                    }
                    else
                    {
                        lblLinkFatherProc.Visible = false;
                    }
                }
                if (padre.isVSM == true)
                {
                    pert.Visible = false;
                    precedenzePERT.Visible = false;
                    for (int i = 0; i < padre.numSubProcessi; i++)
                    {
                        if (padre.subProcessi[i] != null)
                        {
                            padre.subProcessi[i].loadPrecedenti();
                            padre.subProcessi[i].loadSuccessivi();
                            relazione currRel;
                            if (padre.subProcessi[i].numProcessiPrec > 0)
                            {
                                currRel = padre.subProcessi[i].relazionePrec[0];
                            }
                            else
                            {
                                currRel = null;
                            }
                            CreateProcessBox(padre.subProcessi[i].processID, padre.subProcessi[i].processName, i, currRel);
                        }
                    }
                    Control newBox = LoadControl("CreateSubProcess.ascx");
                    ((newProcessBox)newBox).fatherProcID = padre.processID;
                    ProcStream.Visible = true;
                    ProcStream.Controls.Add(newBox);
                    containerVSM.Visible = true;
                }
                else
                {
                    containerVSM.Visible = false;
                    ProcStream.Visible = false;
                    if (padre.processoPadre == -1)
                    {
                        precedenzePERT.Visible = false;
                        containerVSM.Visible = true;
                    }
                    else
                    {
                        processo nonno = new processo(padre.processoPadre);
                        if (nonno.isVSM)
                        {
                            precedenzePERT.Visible = false;
                        }
                        else
                        {
                            precedenzePERT._taskID = int.Parse(procID);
                            precedenzePERT.Visible = true;
                            pert.procID = int.Parse(procID);
                            containerVSM.Visible = false;
                        }
                    }
                }
                imgEdit.Visible = true;
            }
            else
            {
                lblErr.Text = "No querystring found";
                inputVSM.Visible = false;
                vsm.Visible = false;
                inputProcName.Visible = false;
                inputProcDesc.Visible = false;
                imgEdit.Visible = false;
                imgCancel.Visible = false;
                imgSave.Visible = false;
                imgDeleteProcess.Visible = false;
                lblLinkFatherProc.Visible = false;
                containerVSM.Visible = false;
                processOwner.Visible = false;
                ddlProcessOwner.Visible = false;
                precedenzePERT.Visible = false;
                pert.Visible = false;
            }
        }

        public void CreateProcessBox(int subProcID, String procName, int procCont, relazione rel)
        {
            int posBoxRapp = procCont * 200 - 90;
            int posBoxProc = 0;
            Control c1 = LoadControl("boxProcesso.ascx");
            ((boxProcesso)c1).procID = subProcID;
            ((boxProcesso)c1).procName = procName;

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
            ProcStream.Controls.Add(c1);
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
            processOwner.Visible = false;
            UserList elencoUtenti = new UserList();
            ddlProcessOwner.Items.Insert(0, new ListItem("", ""));
            ddlProcessOwner.DataSource = elencoUtenti.elencoUtenti;
            ddlProcessOwner.DataTextField = "username";
            ddlProcessOwner.DataValueField = "username";
            ddlProcessOwner.DataBind();
            ddlProcessOwner.Visible = true;
        }

        public void imgSave_Click(object sender, EventArgs e)
        {
            lblErr.Text = "";
            lblErr.Visible = true;     
            processo proc = new processo(int.Parse(lblProcID.Text));
            proc.processName = Server.HtmlEncode(inputProcName.Text);
            proc.processDescription = Server.HtmlEncode(inputProcDesc.Text);
            lblErr.Text = "VSM: " + inputVSM.SelectedValue.ToString();
            if (inputVSM.SelectedValue == "1")
            {
                proc.isVSM = true;
            }
            else
            {
                proc.isVSM = false;
            }
            if (ddlProcessOwner.SelectedValue == "")
            {
                // cancello il processOwer
                proc.loadProcessOwners();
                for (int i = 0; i < proc.numProcessOwners; i++)
                {
                    proc.deleteProcessOwner(new User(proc.processOwners[i].username));
                }
            }
            else
            {
                // cancello il processOwer
                proc.loadProcessOwners();
                for (int i = 0; i < proc.numProcessOwners; i++)
                {
                    proc.deleteProcessOwner(new User(proc.processOwners[i].username));
                }
                // Aggiungo quello selezionato
                proc.addProcessOwner(new User(ddlProcessOwner.SelectedValue));
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
            lblErr.Text += "<span style='color:red;'>MODIFICA ESEGUITA CORRETTAMENTE</span><br/>";
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
            macroProcessi elenco = new macroProcessi();
//            int res = elenco.deleteMacroProcess(int.Parse(lblProcID.Text));
            int res = (new processo(int.Parse(lblProcID.Text))).delete();
            if (res == 0)
            {
                lblErr.Text += "<br/><span style=\"color:red;\">GENERIC ERROR: PROCESS NOT DELETED OR PROCESS NOT FOUND</span><br/>";
            }
            else if(res == 2)
            {
                lblErr.Text += "<br/><span style=\"color:red;\">ERROR: PROCESS NOT DELETED BECAUSE OF SUB-PROCESSES.<br/>YOU NEED TO DELETE ALL SUB-PROCESSES BEFORE DELETING A PROCESS.</span><br/>";
            }
            else if (res == 1)
            {
                Response.Redirect("/Processi/MacroProcessi.aspx");
            }
        }
    }

}