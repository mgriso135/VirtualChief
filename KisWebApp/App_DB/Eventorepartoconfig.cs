using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventorepartoconfig
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int Reparto { get; set; }
        public TimeSpan RitardoMinimoDaSegnalare { get; set; }

        public virtual Reparti RepartoNavigation { get; set; }
    }
}
