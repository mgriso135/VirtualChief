using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Repartiprocessi
    {
        [Key, Column(Order = 0)]
        public int IdReparto { get; set; }

        [Key, Column(Order = 1)]
        public int ProcessId { get; set; }

        [Key, Column(Order = 2)]
        public int Revisione { get; set; }

        [Key, Column(Order = 3)]
        public int Variante { get; set; }

        public virtual Reparti IdRepartoNavigation { get; set; }
    }
}
