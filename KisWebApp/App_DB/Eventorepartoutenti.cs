using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventorepartoutenti
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int RepartoId { get; set; }
        [Key, Column(Order = 2)]
        public string UserId { get; set; }

        public virtual Reparti Reparto { get; set; }
        public virtual Users User { get; set; }
    }
}
