﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using KIS;
using KIS.App_Code;
namespace KIS.Processi
{
    public partial class managePrecedenzePERT : System.Web.UI.UserControl
    {
        public int taskID;
        public int varID;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<String[]> elencoPermessi = new List<String[]>();
            String[] prmUser = new String[2];
            prmUser[0] = "Processo Variante";
            prmUser[1] = "W";
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
                    // Trovo il padre del processo

                    processo current = new processo(taskID);
                    variante var = new variante(varID);
                    current.loadPrecedenti(var);
                    current.loadSuccessivi(var);
                    curr.InnerText = current.processName;
                    processo[] arrPrecedenti = new processo[current.processiPrec.Count];
                    for (int i = 0; i < current.processiPrec.Count; i++)
                    {
                        arrPrecedenti[i] = new processo(current.processiPrec[i]);
                    }
                    rptPrec.DataSource = arrPrecedenti;
                    rptPrec.DataBind();

                    processo[] arrSuccessivi = new processo[current.processiSucc.Count];
                    for (int i = 0; i < current.processiSucc.Count; i++)
                    {
                        arrSuccessivi[i] = new processo(current.processiSucc[i]);
                    }
                    rptSucc.DataSource = arrSuccessivi;
                    rptSucc.DataBind();

                    int[] idPadre = new int[2];
                    //lblCheck.Text = idPadre[0] + " " + idPadre[1] + "<br/>";
                    idPadre = current.getPadre(var);
                    processo padre = new processo(idPadre[0], idPadre[1]);
                    padre.loadFigli(var);
                    int[] idProcessiDDL = new int[padre.subProcessi.Count];
                    // Trovo il numero di possibili precedenti: non è già tra i precedenti, non è il processo stesso, non è tra i successivi.
                    int numprocDDLPrec = 0;
                    for (int i = 0; i < padre.subProcessi.Count; i++)
                    {
                        //lblCheck.Text += padre.subProcessi[i].processID.ToString() + "<br/>";
                        bool controllo = true;
                        // Check: non è il processo stesso
                        if (padre.subProcessi[i].processID == current.processID)
                        {
                            controllo = false;
                        }
                        // Check: non è già tra i precedenti
                        if (controllo == true)
                        {
                            if (current.isPrecedente(padre.subProcessi[i], var))
                            {
                                controllo = false;
                            }
                        }

                        // Check: non è già tra i successivi
                        if (controllo == true)
                        {
                            if (current.isSuccessivo(padre.subProcessi[i], var))
                            {
                                controllo = false;
                            }
                        }

                        // se il controllo non è false, allora posso tenerlo tra i processi precedenti e/o successivi da aggiungere
                        if (controllo == true)
                        {
                            idProcessiDDL[numprocDDLPrec] = padre.subProcessi[i].processID;
                            numprocDDLPrec++;
                        }
                    }

                    // Costruisco gli array per le dropdownlist
                    if (numprocDDLPrec > 0)
                    {
                        processo[] arrProcessiDDL = new processo[numprocDDLPrec + 1];
                        arrProcessiDDL[0] = new processo();
                        for (int i = 1; i < numprocDDLPrec + 1; i++)
                        {
                            arrProcessiDDL[i] = new processo(idProcessiDDL[i - 1]);
                        }
                        newPrec.DataSource = arrProcessiDDL;
                        newPrec.DataTextField = "processName";
                        newPrec.DataValueField = "processID";
                        newSucc.DataSource = arrProcessiDDL;
                        newSucc.DataTextField = "processName";
                        newSucc.DataValueField = "processID";
                        newSucc.DataBind();
                        newPrec.DataBind();
                    }
                    else
                    {
                        newPrec.Visible = false;
                        newSucc.Visible = false;
                    }
                }
            }
            else
            {
                lblCheck.Text = GetLocalResourceObject("lblPermessoKo").ToString();
                tblPrecedenze.Visible = false;
            }
        }

        protected void addPrecedenti_IndexChanged(object sender, EventArgs e)
        {
            processo current = new processo(taskID);
            current.addProcessoPrecedente(new processo(Int32.Parse(newPrec.SelectedValue)), new variante(varID));
            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href; window.close();</script>");
            //Response.Redirect(Request.RawUrl);
        }

        protected void addSuccessivi_IndexChanged(object sender, EventArgs e)
        {
            processo current = new processo(taskID);
            current.addProcessoSuccessivo(new processo(Int32.Parse(newSucc.SelectedValue)), new variante(varID));
            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href; window.close();</script>");
            //lblCheck.Text = newSucc.SelectedValue + " " + newSucc.SelectedIndex.ToString() + " " + newSucc.SelectedItem.ToString();
            //Response.Redirect(Request.RawUrl);
        }
    }
}