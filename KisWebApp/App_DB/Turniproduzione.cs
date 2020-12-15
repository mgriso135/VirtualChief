using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Turniproduzione
    {
        public Turniproduzione()
        {
            Orarilavoroturni = new HashSet<Orarilavoroturni>();
            Risorseturnopostazione = new HashSet<Risorseturnopostazione>();
            Straordinarifestivita = new HashSet<Straordinarifestivita>();
        }

        [Key]
        public int Id { get; set; }
        public int Reparto { get; set; }
        public string Nome { get; set; }
        public string Colore { get; set; }

        public virtual Reparti RepartoNavigation { get; set; }
        public virtual ICollection<Orarilavoroturni> Orarilavoroturni { get; set; }
        public virtual ICollection<Risorseturnopostazione> Risorseturnopostazione { get; set; }
        public virtual ICollection<Straordinarifestivita> Straordinarifestivita { get; set; }
    }
}
