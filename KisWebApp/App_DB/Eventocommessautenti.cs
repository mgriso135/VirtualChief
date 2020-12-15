using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventocommessautenti
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int CommessaId { get; set; }
        [Key, Column(Order = 2)]
        public short CommessaAnno { get; set; }
        public string UserId { get; set; }

        public virtual Commesse Commessa { get; set; }
        public virtual Users User { get; set; }
    }
}
