using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Tempiciclo
    {
        [Key, Column(Order = 0)]
        public int Processo { get; set; }

        [Key, Column(Order = 1)]
        public int Revisione { get; set; }

        [Key, Column(Order = 3)]
        public int Variante { get; set; }

        [Key, Column(Order = 4)]
        public int NumOp { get; set; }
        public TimeSpan Setup { get; set; }
        public TimeSpan Tempo { get; set; }
        public TimeSpan Tunload { get; set; }
        public bool? Def { get; set; }
    }
}
