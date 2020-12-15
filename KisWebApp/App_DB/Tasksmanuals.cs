using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Tasksmanuals
    {
        [Key, Column(Order = 0)]
        public int TaskId { get; set; }

        [Key, Column(Order = 1)]
        public int TaskRev { get; set; }

        [Key, Column(Order = 2)]
        public int TaskVarianti { get; set; }

        [Key, Column(Order = 3)]
        public int ManualId { get; set; }

        [Key, Column(Order = 4)]
        public int ManualVersion { get; set; }
        public int Sequence { get; set; }
        public byte? IsActive { get; set; }

        public virtual Manuals Manual { get; set; }
        public virtual Processo Task { get; set; }
        public virtual Varianti TaskVariantiNavigation { get; set; }
    }
}
