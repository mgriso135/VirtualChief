using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Processi
{
    public partial class managePrecedenzePERT : System.Web.UI.UserControl
    {
        public int _taskID;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                processo current = new processo(_taskID);
                current.loadPrecedenti();
                current.loadSuccessivi();
                curr.InnerText = current.processName;
                processo[] arrPrecedenti = new processo[current.numProcessiPrec];
                for (int i = 0; i < current.numProcessiPrec; i++)
                {
                    arrPrecedenti[i] = new processo(current.processiPrec[i]);
                }
                rptPrec.DataSource = arrPrecedenti;
                rptPrec.DataBind();

                processo[] arrSuccessivi = new processo[current.numProcessiSucc];
                for (int i = 0; i < current.numProcessiSucc; i++)
                {
                    arrSuccessivi[i] = new processo(current.processiSucc[i]);
                }
                rptSucc.DataSource = arrSuccessivi;
                rptSucc.DataBind();

                processo padre = new processo(current.processoPadre);
                padre.loadFigli();
                int[] idProcessiDDL = new int[padre.numSubProcessi];
                // Trovo il numero di possibili precedenti: non è già tra i precedenti, non è il processo stesso, non è tra i successivi.
                int numprocDDLPrec = 0;
                for (int i = 0; i < padre.numSubProcessi; i++)
                {
                    bool controllo = true;
                    // Check: non è il processo stesso
                    if (padre.subProcessi[i].processID == current.processID)
                    {
                        controllo = false;
                    }
                    // Check: non è già tra i precedenti
                    if (controllo == true)
                    {
                        if (current.isPrecedente(padre.subProcessi[i]))
                        {
                            controllo = false;
                        }
                    }

                    // Check: non è già tra i successivi
                    if (controllo == true)
                    {
                        if (current.isSuccessivo(padre.subProcessi[i]))
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

        protected void addPrecedenti_IndexChanged(object sender, EventArgs e)
        {
            processo current = new processo(_taskID);
            current.addProcessoPrecedente(new processo(Int32.Parse(newPrec.SelectedValue)));
            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href; window.close();</script>");
            //Response.Redirect(Request.RawUrl);
        }

        protected void addSuccessivi_IndexChanged(object sender, EventArgs e)
        {
            processo current = new processo(_taskID);
            current.addProcessoSuccessivo(new processo(Int32.Parse(newSucc.SelectedValue)));
            Response.Write("<script language='javascript'>window.opener.location.href = window.opener.location.href; window.close();</script>");
            //lblCheck.Text = newSucc.SelectedValue + " " + newSucc.SelectedIndex.ToString() + " " + newSucc.SelectedItem.ToString();
            //Response.Redirect(Request.RawUrl);
        }
    }
}