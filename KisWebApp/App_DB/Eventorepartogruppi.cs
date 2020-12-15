using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventorepartogruppi
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int IdReparto { get; set; }
        [Key, Column(Order = 2)]
        public int IdGruppo { get; set; }

        public virtual Groupss IdGruppoNavigation { get; set; }
        public virtual Reparti IdRepartoNavigation { get; set; }
    }
}
