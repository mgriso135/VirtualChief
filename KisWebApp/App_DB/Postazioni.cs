using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Postazioni
    {
        public Postazioni()
        {
            NoncompliancesProducts = new HashSet<NoncompliancesProducts>();
            Repartipostazioniattivita = new HashSet<Repartipostazioniattivita>();
            Risorseturnopostazione = new HashSet<Risorseturnopostazione>();
            Tasksproduzione = new HashSet<Tasksproduzione>();
        }
        [Key]
        public int Idpostazioni { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? BarcodeAutoCheckIn { get; set; }

        public virtual ICollection<NoncompliancesProducts> NoncompliancesProducts { get; set; }
        public virtual ICollection<Repartipostazioniattivita> Repartipostazioniattivita { get; set; }
        public virtual ICollection<Risorseturnopostazione> Risorseturnopostazione { get; set; }
        public virtual ICollection<Tasksproduzione> Tasksproduzione { get; set; }
    }
}
