using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventoarticoloconfig
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int ArticoloId { get; set; }
        [Key, Column(Order = 2)]
        public short ArticoloAnno { get; set; }
        public TimeSpan RitardoMinimoDaSegnalare { get; set; }

        public virtual Productionplan Articolo { get; set; }
    }
}
