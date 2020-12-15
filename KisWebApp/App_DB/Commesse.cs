using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KIS.App_DB;

namespace KIS.App_DB
{
    public partial class Commesse
    {
        public Commesse()
        {
            Eventocommessaconfig = new HashSet<Eventocommessaconfig>();
            Eventocommessagruppi = new HashSet<Eventocommessagruppi>();
            Eventocommessautenti = new HashSet<Eventocommessautenti>();
            Productionplan = new HashSet<Productionplan>();
        }
        [Key, Column(Order = 0)]
        public int Idcommesse { get; set; }

        [Key, Column(Order = 1)]
        public short Anno { get; set; }
        public string Cliente { get; set; }
        public string Note { get; set; }
        public bool? Confirmed { get; set; }
        public string ConfirmedBy { get; set; }
        public string ExternalId { get; set; }

        public virtual Anagraficaclienti ClienteNavigation { get; set; }
        public virtual ICollection<Eventocommessaconfig> Eventocommessaconfig { get; set; }
        public virtual ICollection<Eventocommessagruppi> Eventocommessagruppi { get; set; }
        public virtual ICollection<Eventocommessautenti> Eventocommessautenti { get; set; }
        public virtual ICollection<Productionplan> Productionplan { get; set; }
    }
}
