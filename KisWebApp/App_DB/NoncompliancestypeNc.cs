using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class NoncompliancestypeNc
    {
        [Key, Column(Order = 0)]
        public int TypeId { get; set; }
        [Key, Column(Order = 1)]
        public int Ncid { get; set; }
        [Key, Column(Order = 2)]
        public short Ncyear { get; set; }

        public virtual Noncompliances Nc { get; set; }
        public virtual Noncompliancestypes Type { get; set; }
    }
}
