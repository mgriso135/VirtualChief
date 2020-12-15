using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventoarticologruppi
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }
        [Key, Column(Order = 1)]
        public int ArticoloId { get; set; }
        [Key, Column(Order = 2)]
        public short ArticoloAnno { get; set; }
        [Key, Column(Order = 3)]
        public int IdGruppo { get; set; }

        public virtual Productionplan Articolo { get; set; }
        public virtual Groupss IdGruppoNavigation { get; set; }
    }
}
