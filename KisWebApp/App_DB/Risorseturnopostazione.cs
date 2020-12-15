using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Risorseturnopostazione
    {
        [Key, Column(Order = 0)]
        public int Idturno { get; set; }
        [Key, Column(Order = 1)]
        public int Idpostazione { get; set; }
        [Key, Column(Order = 2)]
        public int Risorse { get; set; }

        public virtual Postazioni IdpostazioneNavigation { get; set; }
        public virtual Turniproduzione IdturnoNavigation { get; set; }
    }
}
