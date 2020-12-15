using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventocommessagruppi
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int CommessaId { get; set; }
        [Key, Column(Order = 2)]
        public short CommessaAnno { get; set; }
        [Key, Column(Order = 3)]
        public int IdGruppo { get; set; }

        public virtual Commesse Commessa { get; set; }
        public virtual Groupss IdGruppoNavigation { get; set; }
    }
}
