using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Operatorireparto
    {
        [Key, Column(Order = 0)]
        public string Operatore { get; set; }
        [Key, Column(Order = 1)]
        public int Reparto { get; set; }

        public virtual Users OperatoreNavigation { get; set; }
        public virtual Reparti RepartoNavigation { get; set; }
    }
}
