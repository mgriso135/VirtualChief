using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Homeboxesregistro
    {
        public Homeboxesregistro()
        {
            Homeboxesuser = new HashSet<Homeboxesuser>();
        }

        [Key]
        public int IdHomeBox { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public string Path { get; set; }

        public virtual ICollection<Homeboxesuser> Homeboxesuser { get; set; }
    }
}
