using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Permessi
    {
        public Permessi()
        {
            Gruppipermessi = new HashSet<Gruppipermessi>();
        }
        [Key]
        public int Idpermesso { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }

        public virtual ICollection<Gruppipermessi> Gruppipermessi { get; set; }
    }
}
