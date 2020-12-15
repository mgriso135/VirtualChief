using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Repartipostazioniattivita
    {
        [Key, Column(Order = 0)]
        public int Reparto { get; set; }
        [Key, Column(Order = 1)]
        public int Postazione { get; set; }
        [Key, Column(Order = 2)]
        public int Processo { get; set; }
        [Key, Column(Order = 3)]
        public int RevProc { get; set; }
        [Key, Column(Order = 4)]
        public int Variante { get; set; }

        public virtual Postazioni PostazioneNavigation { get; set; }
        public virtual Reparti RepartoNavigation { get; set; }
    }
}
