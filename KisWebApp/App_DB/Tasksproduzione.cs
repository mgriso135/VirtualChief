using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Tasksproduzione
    {
        public Tasksproduzione()
        {
            PrectasksproduzionePrecNavigation = new HashSet<Prectasksproduzione>();
            PrectasksproduzioneSuccNavigation = new HashSet<Prectasksproduzione>();
            Registroeventiproduzione = new HashSet<Registroeventiproduzione>();
            Registroeventitaskproduzione = new HashSet<Registroeventitaskproduzione>();
            Tasksproduzioneoperatornotes = new HashSet<Tasksproduzioneoperatornotes>();
            Taskstimespans = new HashSet<Taskstimespans>();
            Taskuser = new HashSet<Taskuser>();
            Warningproduzione = new HashSet<Warningproduzione>();
        }

        [Key]
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrigTask { get; set; }
        public int RevOrigTask { get; set; }
        public int Variante { get; set; }
        public int Reparto { get; set; }
        public int Postazione { get; set; }
        public string Status { get; set; }
        public int Idcommessa { get; set; }
        public short Annocommessa { get; set; }
        public int IdArticolo { get; set; }
        public short AnnoArticolo { get; set; }
        public int NOperatori { get; set; }
        public TimeSpan TempoCiclo { get; set; }
        public int QtaPrevista { get; set; }
        public int QtaProdotta { get; set; }
        public TimeSpan? LeadTime { get; set; }
        public TimeSpan? WorkingTime { get; set; }
        public TimeSpan? Delay { get; set; }

        public virtual Postazioni PostazioneNavigation { get; set; }
        public virtual Processo Processo { get; set; }
        public virtual Reparti RepartoNavigation { get; set; }
        public virtual ICollection<Prectasksproduzione> PrectasksproduzionePrecNavigation { get; set; }
        public virtual ICollection<Prectasksproduzione> PrectasksproduzioneSuccNavigation { get; set; }
        public virtual ICollection<Registroeventiproduzione> Registroeventiproduzione { get; set; }
        public virtual ICollection<Registroeventitaskproduzione> Registroeventitaskproduzione { get; set; }
        public virtual ICollection<Tasksproduzioneoperatornotes> Tasksproduzioneoperatornotes { get; set; }
        public virtual ICollection<Taskstimespans> Taskstimespans { get; set; }
        public virtual ICollection<Taskuser> Taskuser { get; set; }
        public virtual ICollection<Warningproduzione> Warningproduzione { get; set; }
    }
}
