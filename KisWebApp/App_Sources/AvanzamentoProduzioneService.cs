using System;
using System.Collections.Generic;
using KIS.App_Code;

namespace KIS.App_Sources
{
    public enum StatoAvanzamentoArticolo
    {
        NonIniziato,
        InCorso,
        InRitardo
    }

    /// <summary>
    /// Business logic extracted from the legacy WebForms code-behind
    /// KisWebApp/Produzione/avanzamentoProduzione.aspx.cs so it can be
    /// unit-tested on Linux (Tier-1 DomainTests) without the page lifecycle.
    /// </summary>
    public static class AvanzamentoProduzioneService
    {
        /// <summary>
        /// Aggregates every article across all work orders whose status is
        /// 'N' (not yet started) or 'P' (planned, not started). Mirrors the
        /// legacy loadCommesse() of avanzamentoProduzione.aspx.cs.
        /// </summary>
        public static List<Articolo> CaricaArticoliNonPianificati(String tenant)
        {
            List<Articolo> artNP = new List<Articolo>();
            ElencoCommesse elComm = new ElencoCommesse(tenant);
            elComm.loadCommesse();
            for (int i = 0; i < elComm.Commesse.Count; i++)
            {
                elComm.Commesse[i].loadArticoli();
                for (int j = 0; j < elComm.Commesse[i].Articoli.Count; j++)
                {
                    if (elComm.Commesse[i].Articoli[j].Status == 'N' || elComm.Commesse[i].Articoli[j].Status == 'P')
                    {
                        artNP.Add(elComm.Commesse[i].Articoli[j]);
                    }
                }
            }
            return artNP;
        }

        /// <summary>
        /// Classifies an article's progress by comparing the current time
        /// against its production tasks' EarlyStart/LateStart windows.
        /// Mirrors the green/yellow/red row logic of avanzamentoProduzione.aspx.cs:
        ///   - NonIniziato: no task is in progress nor past its window.
        ///   - InCorso:     at least one task is between EarlyStart and LateStart.
        ///   - InRitardo:   at least one task is past LateStart (red wins over yellow).
        /// </summary>
        public static StatoAvanzamentoArticolo ClassificaStato(Articolo art, DateTime now)
        {
            art.loadTasksProduzione();
            List<(DateTime EarlyStart, DateTime LateStart)> finestre = new List<(DateTime, DateTime)>();
            for (int i = 0; i < art.Tasks.Count; i++)
            {
                finestre.Add((art.Tasks[i].EarlyStart, art.Tasks[i].LateStart));
            }
            return ClassificaStato(now, finestre);
        }

        /// <summary>
        /// Pure classification core: given the task time windows and a reference
        /// instant, returns the article progress status. Testable without a DB.
        /// </summary>
        public static StatoAvanzamentoArticolo ClassificaStato(DateTime now, System.Collections.Generic.IEnumerable<(DateTime EarlyStart, DateTime LateStart)> finestre)
        {
            bool checkInCorso = false;
            bool checkInRitardo = false;
            foreach (var (EarlyStart, LateStart) in finestre)
            {
                if (now >= EarlyStart && now <= LateStart)
                {
                    checkInCorso = true;
                }
                else if (now >= LateStart)
                {
                    checkInRitardo = true;
                }
            }
            if (checkInCorso == false && checkInRitardo == false)
            {
                return StatoAvanzamentoArticolo.NonIniziato;
            }
            if (checkInRitardo == true)
            {
                return StatoAvanzamentoArticolo.InRitardo;
            }
            return StatoAvanzamentoArticolo.InCorso;
        }
    }
}
