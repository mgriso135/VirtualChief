using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Manualswilabels
    {
        [Key, Column(Order = 0)]
        public int ManualId { get; set; }
        [Key, Column(Order = 1)]
        public int ManualVersion { get; set; }
        [Key, Column(Order = 2)]
        public int LabelId { get; set; }

        public virtual Workinstructionslabel Label { get; set; }
        public virtual Manuals Manual { get; set; }
    }
}
