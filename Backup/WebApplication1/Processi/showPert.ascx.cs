using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Processi
{
    public partial class showPert : System.Web.UI.UserControl
    {
        //protected int contatore;
        public int procID;

        protected void Page_Load(object sender, EventArgs e)
        {
            procID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (!int.TryParse(Request.QueryString["id"], out procID))
                {
                    procID = -1;
                }
            }

            if (procID != -1)
            {
                processo padre = new processo(procID);
                int controllo = padre.checkConsistency();
                if(controllo == 0)
                {
                    lbl1.Text = "<span style='color:red;'>GENERIC ERROR</span>";
                }
                else if (controllo == 2)
                {
                    lbl1.Text = "<span style='color:red;'>ATTENZIONE: processo inconsistente. Esiste almeno un task che non ha né precedenti né successivi</span>";
                }
                padre.loadFigli();
                String valori = "";
                String precSucc = "";
                int contaLinee = 0;
                for (int i = 0; i < padre.numSubProcessi; i++)
                {
                    valori += "[\"" + padre.subProcessi[i].processID.ToString() + "\", \"" + padre.subProcessi[i].processName + "\", \"" + padre.subProcessi[i].posX.ToString() + "\", \"" + padre.subProcessi[i].posY.ToString() + "\"], ";
                    
                    // Costruisco l'array dei successivi
                    for (int j = 0; j < padre.subProcessi[i].numProcessiSucc; j++)
                    {
                        precSucc += "[\"" + padre.subProcessi[i].processID.ToString() + "\", \"" + padre.subProcessi[i].processiSucc[j].ToString() + "\"], ";
                        contaLinee++;
                    }


                }
                if (valori.Length > 2)
                {
                    valori = valori.Substring(0, valori.Length - 2);
                }
                if (precSucc.Length > 2)
                {
                    precSucc = precSucc.Substring(0, precSucc.Length - 2);
                }
                Page.ClientScript.RegisterArrayDeclaration("arrTasks", valori);
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "numTasks", "var numTasks = " + padre.numSubProcessi.ToString() + ";", true);
                Page.ClientScript.RegisterArrayDeclaration("precSucc", precSucc);
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "numLinee", "var numLinee = " + contaLinee.ToString() + ";", true);

                svg1.Text += "</svg>";
            }
            else
            {
                lbl1.Text = "Errore: querystring non presente, errato o processo non trovato.<br/>";
            }
        }

        protected void addTaskPert(object sender, EventArgs e)
        {
            procID = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (!int.TryParse(Request.QueryString["id"], out procID))
                {
                    procID = -1;
                }
            }
            processo padre = new processo(procID);
            if (padre.processID != -1)
            {
                int procCreated = padre.createDefaultSubProcess();
                if (procCreated >= 0)
                {
                    lbl1.Text = "<script>window.open(\"showProcesso.aspx?id=" + procCreated + "\")</script>";
                    //Response.Redirect(Request.RawUrl);
                }
                else
                {
                    //ErrorMessage
                }
            }

        }
    }
}