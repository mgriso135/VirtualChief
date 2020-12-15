using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Orarilavoroturni
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        public int IdTurno { get; set; }
        public int GiornoInizio { get; set; }
        public TimeSpan OraInizio { get; set; }
        public int GiornoFine { get; set; }
        public TimeSpan OraFine { get; set; }

        public virtual Turniproduzione IdTurnoNavigation { get; set; }
    }
}
