using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Reparti
    {
        public Reparti()
        {
            Eventorepartoconfig = new HashSet<Eventorepartoconfig>();
            Eventorepartogruppi = new HashSet<Eventorepartogruppi>();
            Eventorepartoutenti = new HashSet<Eventorepartoutenti>();
            Operatorireparto = new HashSet<Operatorireparto>();
            Productionplan = new HashSet<Productionplan>();
            Repartipostazioniattivita = new HashSet<Repartipostazioniattivita>();
            Repartiprocessi = new HashSet<Repartiprocessi>();
            Tasksproduzione = new HashSet<Tasksproduzione>();
            Turniproduzione = new HashSet<Turniproduzione>();
        }
        [Key]
        public int Idreparto { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public double? Cadenza { get; set; }
        public bool SplitTasks { get; set; }
        public int AnticipoTasks { get; set; }
        public bool? ModoCalcoloTc { get; set; }
        public string Timezone { get; set; }

        public virtual ICollection<Eventorepartoconfig> Eventorepartoconfig { get; set; }
        public virtual ICollection<Eventorepartogruppi> Eventorepartogruppi { get; set; }
        public virtual ICollection<Eventorepartoutenti> Eventorepartoutenti { get; set; }
        public virtual ICollection<Operatorireparto> Operatorireparto { get; set; }
        public virtual ICollection<Productionplan> Productionplan { get; set; }
        public virtual ICollection<Repartipostazioniattivita> Repartipostazioniattivita { get; set; }
        public virtual ICollection<Repartiprocessi> Repartiprocessi { get; set; }
        public virtual ICollection<Tasksproduzione> Tasksproduzione { get; set; }
        public virtual ICollection<Turniproduzione> Turniproduzione { get; set; }
    }
}
