using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Precedenzeprocessi
    {
        [Key, Column(Order = 0)]
        public int Prec { get; set; }
        [Key, Column(Order = 1)]
        public int RevPrec { get; set; }
        [Key, Column(Order = 2)]
        public int Succ { get; set; }
        [Key, Column(Order = 3)]
        public int RevSucc { get; set; }
        [Key, Column(Order = 4)]
        public int Variante { get; set; }
        public int Relazione { get; set; }
        public TimeSpan? Pausa { get; set; }
        public int? ConstraintType { get; set; }

        public virtual Processo Processo { get; set; }
        public virtual Processo ProcessoNavigation { get; set; }
        public virtual Relazioniprocessi RelazioneNavigation { get; set; }
        public virtual Varianti VarianteNavigation { get; set; }
    }
}
