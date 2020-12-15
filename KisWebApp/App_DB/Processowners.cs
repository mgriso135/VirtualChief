using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Processowners
    {
        [Key, Column(Order = 0)]
        public string User { get; set; }
        [Key, Column(Order = 1)]
        public int Process { get; set; }
        [Key, Column(Order = 2)]
        public int RevProc { get; set; }

        public virtual Processo Processo { get; set; }
        public virtual Users UserNavigation { get; set; }
    }
}
