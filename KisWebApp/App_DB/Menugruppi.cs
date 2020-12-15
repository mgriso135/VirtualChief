using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Menugruppi
    {
        [Key, Column(Order = 0)]
        public int Gruppo { get; set; }
        [Key, Column(Order = 1)]
        public int IdVoce { get; set; }
        public int Ordinamento { get; set; }

        public virtual Groupss GruppoNavigation { get; set; }
        public virtual Menuvoci IdVoceNavigation { get; set; }
    }
}
