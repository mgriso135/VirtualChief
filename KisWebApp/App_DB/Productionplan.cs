using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Productionplan
    {
        public Productionplan()
        {
            Eventoarticoloconfig = new HashSet<Eventoarticoloconfig>();
            Eventoarticologruppi = new HashSet<Eventoarticologruppi>();
            Eventoarticoloutenti = new HashSet<Eventoarticoloutenti>();
            NoncompliancesProducts = new HashSet<NoncompliancesProducts>();
            Productparameters = new HashSet<Productparameters>();
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public short Anno { get; set; }
        public int Processo { get; set; }
        public int Revisione { get; set; }
        public int Variante { get; set; }
        public string Matricola { get; set; }
        public string Status { get; set; }
        public int? Reparto { get; set; }
        public int Commessa { get; set; }
        public short AnnoCommessa { get; set; }
        public string Planner { get; set; }
        public int Quantita { get; set; }
        public int? QuantitaProdotta { get; set; }
        public int? MeasurementUnit { get; set; }
        public string KanbanCard { get; set; }
        public TimeSpan LeadTime { get; set; }
        public TimeSpan WorkingTimePlanned { get; set; }
        public TimeSpan WorkingTime { get; set; }
        public TimeSpan Delay { get; set; }

        public virtual Commesse Commesse { get; set; }
        public virtual Measurementunits MeasurementUnitNavigation { get; set; }
        public virtual Users PlannerNavigation { get; set; }
        public virtual Reparti RepartoNavigation { get; set; }
        public virtual ICollection<Eventoarticoloconfig> Eventoarticoloconfig { get; set; }
        public virtual ICollection<Eventoarticologruppi> Eventoarticologruppi { get; set; }
        public virtual ICollection<Eventoarticoloutenti> Eventoarticoloutenti { get; set; }
        public virtual ICollection<NoncompliancesProducts> NoncompliancesProducts { get; set; }
        public virtual ICollection<Productparameters> Productparameters { get; set; }
    }
}
