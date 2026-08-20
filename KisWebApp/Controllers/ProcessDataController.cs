using System;
using System.Collections.Generic;
using System.Web.Http;
using KIS.App_Code;

namespace KIS.Controllers
{
    /// <summary>
    /// Replaces Processi/getProcessData.asmx – AJAX endpoint for the PERT editor (cycle times, precedences).
    /// Contract kept identical for AJAX callers.
    /// </summary>
    public class ProcessDataController : ApiController
    {
        // GET: api/ProcessData?procID=..&rev=..&varID=..
        [HttpGet]
        public IHttpActionResult Get(int procID, int rev, int varID)
        {
            return Ok(new
            {
                tempiCiclo = loadTempiCiclo(procID, rev, varID),
                precedenze = loadPrecedenze(procID, rev, varID)
            });
        }

        // GET: api/ProcessData/LoadTempiCiclo?procID=..&rev=..&varID=..
        [HttpGet]
        public List<String[]> loadTempiCiclo(int procID, int rev, int varID)
        {
            List<String[]> ret = new List<String[]>();
            String wsName = WebEnv.ActiveWorkspaceName;
            if (wsName.Length > 0)
            {
                processo padre = new processo(wsName, procID);
                variante var = new variante(wsName, varID);
                padre.loadFigli(new variante(wsName, varID));

                for (int i = 0; i < padre.subProcessi.Count; i++)
                {
                    String[] element = new String[5];
                    TaskVariante tsk = new TaskVariante(wsName, padre.subProcessi[i], var);
                    TimeSpan tc = tsk.getDefaultTempoCiclo();
                    int n_ops = tsk.getDefaultOperatori();

                    element[0] = padre.subProcessi[i].processID.ToString();
                    element[1] = padre.subProcessi[i].processName;
                    element[2] = padre.subProcessi[i].posX.ToString();
                    element[3] = padre.subProcessi[i].posY.ToString();
                    element[4] = Math.Truncate(tc.TotalHours).ToString() + ":" + tc.Minutes.ToString() + ":"
                        + tc.Seconds.ToString() + " (" + n_ops + ")";
                    ret.Add(element);
                }
            }
            return ret;
        }

        // GET: api/ProcessData/LoadPrecedenze?procID=..&rev=..&varID=..
        [HttpGet]
        public List<int[]> loadPrecedenze(int procID, int rev, int varID)
        {
            List<int[]> ret = new List<int[]>();
            String wsName = WebEnv.ActiveWorkspaceName;
            if (wsName.Length > 0)
            {
                processo padre = new processo(wsName, procID, rev);
                variante var = new variante(wsName, varID);
                padre.loadFigli(var);
                for (int i = 0; i < padre.subProcessi.Count; i++)
                {
                    padre.subProcessi[i].loadSuccessivi(new variante(wsName, varID));
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

        // GET: api/ProcessData/AddDefaultSubProcess?procID=..&rev=..&varID=..
        [HttpGet]
        public int addDefaultSubProcess(int procID, int rev, int varID)
        {
            int ret = -1;
            String wsName = WebEnv.ActiveWorkspaceName;
            if (wsName.Length > 0)
            {
                processo padre = new processo(wsName, procID);
                if (padre.processID != -1 && varID != -1)
                {
                    int procCreated = padre.createDefaultSubProcess(new variante(wsName, varID));
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

        // GET: api/ProcessData/LinkExistingSubProcess?procID=..&rev=..&varID=..&taskID=..&taskRev=..
        [HttpGet]
        public bool linkExistingSubProcess(int procID, int rev, int varID, int taskID, int taskRev)
        {
            bool rt = false;
            String wsName = WebEnv.ActiveWorkspaceName;
            if (wsName.Length > 0)
            {
                processo pr = new processo(wsName, procID, rev);

                if (taskID != -1)
                {
                    rt = pr.linkProcessoVariante(new TaskVariante(wsName, new processo(wsName, taskID), new variante(wsName, varID)));
                }
                else
                {
                    rt = false;
                }
            }
            return rt;
        }

        // GET: api/ProcessData/DeleteSubProcess?procID=..&rev=..&varID=..&taskID=..&taskRev=..
        [HttpGet]
        public bool deleteSubProcess(int procID, int rev, int varID, int taskID, int taskRev)
        {
            bool controllo = true;
            String wsName = WebEnv.ActiveWorkspaceName;
            if (wsName.Length > 0)
            {
                processo prc = new processo(wsName, taskID);
                variante var = new variante(wsName, varID);
                TaskVariante tsk = new TaskVariante(wsName, prc, var);

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