using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Varianti
    {
        public Varianti()
        {
            Precedenzeprocessi = new HashSet<Precedenzeprocessi>();
            Tasksmanuals = new HashSet<Tasksmanuals>();
            Variantiprocessi = new HashSet<Variantiprocessi>();
        }

        [Key]
        public int Idvariante { get; set; }
        public string NomeVariante { get; set; }
        public string DescVariante { get; set; }

        public virtual ICollection<Precedenzeprocessi> Precedenzeprocessi { get; set; }
        public virtual ICollection<Tasksmanuals> Tasksmanuals { get; set; }
        public virtual ICollection<Variantiprocessi> Variantiprocessi { get; set; }
    }
}
