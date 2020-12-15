using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Registroeventitaskproduzione
    {
        public Registroeventitaskproduzione()
        {
            TaskstimespansEndevent = new HashSet<Taskstimespans>();
            TaskstimespansStartevent = new HashSet<Taskstimespans>();
        }

        [Key]
        public int Id { get; set; }
        public string User { get; set; }
        public int Task { get; set; }
        public string Evento { get; set; }
        public string Note { get; set; }

        public virtual Tasksproduzione TaskNavigation { get; set; }
        public virtual Users UserNavigation { get; set; }
        public virtual ICollection<Taskstimespans> TaskstimespansEndevent { get; set; }
        public virtual ICollection<Taskstimespans> TaskstimespansStartevent { get; set; }
    }
}
