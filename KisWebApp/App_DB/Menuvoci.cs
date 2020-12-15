using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Menuvoci
    {
        public Menuvoci()
        {
            MenualberoIdFiglioNavigation = new HashSet<Menualbero>();
            MenualberoIdPadreNavigation = new HashSet<Menualbero>();
            Menugruppi = new HashSet<Menugruppi>();
        }

        [Key]
        public int Id { get; set; }
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Menualbero> MenualberoIdFiglioNavigation { get; set; }
        public virtual ICollection<Menualbero> MenualberoIdPadreNavigation { get; set; }
        public virtual ICollection<Menugruppi> Menugruppi { get; set; }
    }
}
