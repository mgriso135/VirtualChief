using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KIS.App_Code;
using KIS.App_Sources;

namespace KIS.Controllers
{
    [Route("api/produzione")]
    [ApiController]
    public class ProductionLaunchController : ControllerBase
    {
        [HttpPost("lancia/{id}/{anno}")]
        public IActionResult LanciaInProduzione(int id, int anno)
        {
            string tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return Unauthorized(new { error = "Workspace non attivo" });
            }

            List<string[]> elencoPermessi = new List<string[]> { new[] { "Articoli", "W" } };
            string uidStr = User.FindFirst("uid")?.Value;
            if (!int.TryParse(uidStr, out var uid))
            {
                return Unauthorized(new { error = "Utente non autenticato" });
            }

            var user = new UserAccount(uid);
            user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
            if (!user.ValidatePermissions(tenant, elencoPermessi))
            {
                return Forbid();
            }

            try
            {
                var art = new Articolo(tenant, id, anno);
                if (art.ID == -1)
                {
                    return NotFound(new { error = "Articolo non trovato" });
                }

                if (art.Status != 'N')
                {
                    return BadRequest(new { error = "L'articolo non è in stato 'Non pianificato'", code = 3 });
                }

                if (art.Proc == null || art.Proc.process == null)
                {
                    return BadRequest(new { error = "L'articolo non ha un processo associato", code = 4 });
                }

                var prc = art.Proc;
                prc.loadReparto();
                prc.process.loadFigli(prc.variant);

                int repId = art.Reparto;
                if (repId == -1)
                {
                    var ultimoRep = prc.UltimoRepartoUtilizzato;
                    if (ultimoRep != null && ultimoRep.id != -1)
                    {
                        repId = ultimoRep.id;
                    }
                    else if (prc.RepartiProduttivi != null && prc.RepartiProduttivi.Count > 0)
                    {
                        repId = prc.RepartiProduttivi[0].id;
                    }
                    else
                    {
                        return BadRequest(new { error = "Nessun reparto produttivo associato al processo", code = 5 });
                    }
                }

                var reparto = new Reparto(tenant, repId);
                if (reparto.id == -1)
                {
                    return BadRequest(new { error = "Reparto non valido", code = 5 });
                }

                var lstTasks = new List<TaskConfigurato>();
                foreach (var subProc in prc.process.subProcessi)
                {
                    var tskVar = new TaskVariante(tenant, subProc, prc.variant);
                    tskVar.loadTempiCiclo();
                    var defaultTempo = tskVar.getDefaultTempo();
                    if (defaultTempo == null)
                    {
                        return BadRequest(new { error = $"Tempo ciclo non configurato per il task {subProc.processName}", code = 6 });
                    }
                    lstTasks.Add(new TaskConfigurato(tenant, tskVar, defaultTempo, repId, art.Quantita));
                }

                var prcCfg = new ConfigurazioneProcesso(tenant, art, lstTasks, reparto, art.Quantita);

                int simResult = prcCfg.SimulaIntroduzioneInProduzione();
                if (simResult != 1)
                {
                    return BadRequest(new { error = "Simulazione fallita", details = prcCfg.log, code = simResult });
                }

                art.Planner = user;
                int launchResult = prcCfg.LanciaInProduzione();

                if (launchResult == 1)
                {
                    return Ok(new { success = true, message = "Articolo lanciato in produzione con successo", log = prcCfg.log });
                }
                else if (launchResult == 3)
                {
                    return BadRequest(new { error = "Articolo già lanciato in produzione", code = 3 });
                }
                else
                {
                    return BadRequest(new { error = "Errore durante il lancio in produzione", details = prcCfg.log, code = launchResult });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Errore interno del server", details = ex.Message });
            }
        }

        [HttpPost("simula/{id}/{anno}")]
        public IActionResult SimulaIntroduzioneInProduzione(int id, int anno, [FromBody] SimulaRequest request)
        {
            string tenant = CurrentWorkspace.Of(User);
            if (string.IsNullOrEmpty(tenant))
            {
                return Unauthorized(new { error = "Workspace non attivo" });
            }

            List<string[]> elencoPermessi = new List<string[]> { new[] { "Articoli", "R" } };
            string uidStr = User.FindFirst("uid")?.Value;
            if (!int.TryParse(uidStr, out var uid))
            {
                return Unauthorized(new { error = "Utente non autenticato" });
            }

            var user = new UserAccount(uid);
            user.loadGroups(CurrentWorkspace.Of(User) != null ? new Workspace(CurrentWorkspace.Of(User)).id : -1);
            if (!user.ValidatePermissions(tenant, elencoPermessi))
            {
                return Forbid();
            }

            try
            {
                var art = new Articolo(tenant, id, anno);
                if (art.ID == -1)
                {
                    return NotFound(new { error = "Articolo non trovato" });
                }

                if (art.Proc == null || art.Proc.process == null)
                {
                    return BadRequest(new { error = "L'articolo non ha un processo associato", code = 4 });
                }

                var prc = art.Proc;
                prc.loadReparto();
                prc.process.loadFigli(prc.variant);

                int repId = request?.RepartoId ?? art.Reparto;
                if (repId == -1)
                {
                    var ultimoRep = prc.UltimoRepartoUtilizzato;
                    if (ultimoRep != null && ultimoRep.id != -1)
                    {
                        repId = ultimoRep.id;
                    }
                    else if (prc.RepartiProduttivi != null && prc.RepartiProduttivi.Count > 0)
                    {
                        repId = prc.RepartiProduttivi[0].id;
                    }
                    else
                    {
                        return BadRequest(new { error = "Nessun reparto produttivo associato al processo", code = 5 });
                    }
                }

                var reparto = new Reparto(tenant, repId);
                if (reparto.id == -1)
                {
                    return BadRequest(new { error = "Reparto non valido", code = 5 });
                }

                var lstTasks = new List<TaskConfigurato>();
                foreach (var subProc in prc.process.subProcessi)
                {
                    var tskVar = new TaskVariante(tenant, subProc, prc.variant);
                    tskVar.loadTempiCiclo();
                    
                    TempoCiclo tempoToUse = null;
                    if (request?.TempiCiclo != null && request.TempiCiclo.TryGetValue(subProc.processID.ToString(), out var selectedOps))
                    {
                        tempoToUse = new TempoCiclo(tenant, subProc.processID, subProc.revisione, prc.variant.idVariante, selectedOps);
                    }
                    else
                    {
                        tempoToUse = tskVar.getDefaultTempo();
                    }

                    if (tempoToUse == null)
                    {
                        return BadRequest(new { error = $"Tempo ciclo non configurato per il task {subProc.processName}", code = 6 });
                    }
                    lstTasks.Add(new TaskConfigurato(tenant, tskVar, tempoToUse, repId, art.Quantita));
                }

                var prcCfg = new ConfigurazioneProcesso(tenant, art, lstTasks, reparto, art.Quantita);

                int simResult = prcCfg.SimulaIntroduzioneInProduzione();
                if (simResult == 1)
                {
                    prcCfg.Processi.Sort((p1, p2) => p1.EarlyStartTime.CompareTo(p2.EarlyStartTime));
                    return Ok(new { success = true, processi = prcCfg.Processi, log = prcCfg.log });
                }
                else
                {
                    return BadRequest(new { error = "Simulazione fallita", details = prcCfg.log, code = simResult });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Errore interno del server", details = ex.Message });
            }
        }

        public class SimulaRequest
        {
            public int RepartoId { get; set; } = -1;
            public Dictionary<string, int> TempiCiclo { get; set; }
        }
    }
}