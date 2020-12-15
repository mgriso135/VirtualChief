using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Processo
    {
        public Processo()
        {
            KpiDescription = new HashSet<KpiDescription>();
            PrecedenzeprocessiProcesso = new HashSet<Precedenzeprocessi>();
            PrecedenzeprocessiProcessoNavigation = new HashSet<Precedenzeprocessi>();
            Processowners = new HashSet<Processowners>();
            Tasksmanuals = new HashSet<Tasksmanuals>();
            Tasksproduzione = new HashSet<Tasksproduzione>();
            Variantiprocessi = new HashSet<Variantiprocessi>();
        }

        [Key, Column(Order = 0)]
        public int ProcessId { get; set; }
        [Key, Column(Order = 1)]
        public int Revisione { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVsm { get; set; }
        public int Posx { get; set; }
        public int Posy { get; set; }
        public bool Attivo { get; set; }

        public virtual ICollection<KpiDescription> KpiDescription { get; set; }
        public virtual ICollection<Precedenzeprocessi> PrecedenzeprocessiProcesso { get; set; }
        public virtual ICollection<Precedenzeprocessi> PrecedenzeprocessiProcessoNavigation { get; set; }
        public virtual ICollection<Processowners> Processowners { get; set; }
        public virtual ICollection<Tasksmanuals> Tasksmanuals { get; set; }
        public virtual ICollection<Tasksproduzione> Tasksproduzione { get; set; }
        public virtual ICollection<Variantiprocessi> Variantiprocessi { get; set; }
    }
}
