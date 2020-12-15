using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KIS.App_DB;

namespace KIS.App_DB
{
    public partial class Eventocommessaconfig
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int CommessaId { get; set; }
        [Key, Column(Order = 2)]
        public short CommessaAnno { get; set; }
        [Key, Column(Order = 3)]
        public TimeSpan RitardoMinimoDaSegnalare { get; set; }

        public virtual Commesse Commessa { get; set; }
    }
}
