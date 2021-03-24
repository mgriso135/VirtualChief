using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using KIS.App_Code;
namespace KIS.Processi
{
    /// <summary>
    /// Descrizione di riepilogo per getProcessData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    [System.Web.Script.Services.ScriptService]
    public class getProcessData : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "CIAO";
        }

        [WebMethod]
        public List<String[]> loadTempiCiclo(int procID, int rev, int varID)
        {
            List<String[]> ret = new List<String[]>();
            if(Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            { 
                processo padre = new processo(Session["ActiveWorkspace_Name"].ToString(), procID);
                variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), varID);
                padre.loadFigli(new variante(Session["ActiveWorkspace_Name"].ToString(), varID));

                for (int i = 0; i < padre.subProcessi.Count; i++)
                {
                    String[] element = new String[5];
                    TaskVariante tsk = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), padre.subProcessi[i], var);
                    TimeSpan tc = tsk.getDefaultTempoCiclo();
                    int n_ops = tsk.getDefaultOperatori();

                    element[0] = padre.subProcessi[i].processID.ToString();
                    element[1] = padre.subProcessi[i].processName;
                    element[2] = padre.subProcessi[i].posX.ToString();
                    element[3] = padre.subProcessi[i].posY.ToString();
                    element[4] = Math.Truncate(tc.TotalHours).ToString() + ":" +tc.Minutes.ToString() +":"
                        +tc.Seconds.ToString() + " ("+n_ops+")";
                    ret.Add(element);
                }
            }
            return ret;
            
        }
        
        [WebMethod]
        public List<int[]> loadPrecedenze(int procID, int rev, int varID)
        {
            List<int[]> ret = new List<int[]>();
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo padre = new processo(Session["ActiveWorkspace_Name"].ToString(), procID, rev);
                variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), varID);
                padre.loadFigli(var);
                for (int i = 0; i < padre.subProcessi.Count; i++)
                {
                    padre.subProcessi[i].loadSuccessivi(new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                    // Costruisco l'array dei successivi
                    for (int j = 0; j < padre.subProcessi[i].processiSucc.Count; j++)
                    {
                        int[] elem = new int[5];
                        elem[0] = padre.subProcessi[i].processID;
                        elem[1] = padre.subProcessi[i].processiSucc[j];
                        elem[2] = Convert.ToInt32(padre.subProcessi[i].pauseSucc[j].TotalSeconds);
                        elem[3] = padre.subProcessi[i].revisione;
                        elem[3] = padre.subProcessi[i].revisioneSucc[j];
                        ret.Add(elem);
                    }
                }
            }
            return ret;
        }

        [WebMethod]
        public int addDefaultSubProcess(int procID, int rev, int varID)
        {
            int ret = -1;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo padre = new processo(Session["ActiveWorkspace_Name"].ToString(), procID);
                if (padre.processID != -1 && varID != -1)
                {
                    int procCreated = padre.createDefaultSubProcess(new variante(Session["ActiveWorkspace_Name"].ToString(), varID));
                    if (procCreated >= 0)
                    {
                        ret = procCreated;
                    }
                    else
                    {
                        ret = procCreated;
                    }
                }
            }
            return ret;
        }

        [WebMethod]
        public bool linkExistingSubProcess(int procID, int rev, int varID, int taskID, int taskRev)
        {
            bool rt = false;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo pr = new processo(Session["ActiveWorkspace_Name"].ToString(), procID, rev);

                if (taskID != -1)
                {
                    rt = pr.linkProcessoVariante(new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), new processo(Session["ActiveWorkspace_Name"].ToString(), taskID), new variante(Session["ActiveWorkspace_Name"].ToString(), varID)));
                }
                else
                {
                    rt = false;
                }
            }
            return rt;
        }

        [WebMethod]
        public bool deleteSubProcess(int procID, int rev, int varID, int taskID, int taskRev)
        {
            bool controllo = true;
            if (Session["ActiveWorkspace_Name"] != null && Session["ActiveWorkspace_Name"].ToString().Length > 0)
            {
                processo prc = new processo(Session["ActiveWorkspace_Name"].ToString(), taskID);
                variante var = new variante(Session["ActiveWorkspace_Name"].ToString(), varID);
                TaskVariante tsk = new TaskVariante(Session["ActiveWorkspace_Name"].ToString(), prc, var);

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
                    }
                }

                // Controllo che non ci siano tempi ciclo associati
                if (controllo == true)
                {
                    tsk.loadTempiCiclo();
                    if (tsk.Tempi.Tempi.Count > 0)
                    {
                        controllo = false;
                    }
                    else
                    {
                        controllo = true;
                    }
                }

                if (controllo == true)
                {
                    tsk.loadPostazioni();
                    for (int i = 0; i < tsk.PostazioniDiLavoro.Count; i++)
                    {
                        tsk.deleteLinkPostazione(tsk.PostazioniDiLavoro[i]);
                    }

                }

                // Se è tutto ok...
                if (controllo == true)
                {
                    bool rt = tsk.Delete();
                    if (rt == true)
                    {
                        int res = prc.delete();
                        controllo = true;
                    }
                    else
                    {
                        controllo = false;
                    }

                }
            }
            return controllo;
            }
        }
    }

