using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Prectasksproduzione
    {
        [Key, Column(Order = 0)]
        public int Prec { get; set; }
        [Key, Column(Order = 1)]
        public int Succ { get; set; }
        [Key, Column(Order = 2)]
        public int Relazione { get; set; }
        public TimeSpan? Pausa { get; set; }
        public int? ConstraintType { get; set; }

        public virtual Tasksproduzione PrecNavigation { get; set; }
        public virtual Tasksproduzione SuccNavigation { get; set; }
    }
}
