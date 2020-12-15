using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Menualbero
    {
        [Key, Column(Order = 0)]
        public int IdPadre { get; set; }
        [Key, Column(Order = 1)]
        public int IdFiglio { get; set; }
        public int Ordinamento { get; set; }

        public virtual Menuvoci IdFiglioNavigation { get; set; }
        public virtual Menuvoci IdPadreNavigation { get; set; }
    }
}
