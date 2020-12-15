using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Taskuser
    {
        [Key, Column(Order = 0)]
        public int TaskId { get; set; }
        [Key, Column(Order = 1)]
        public string User { get; set; }
        public bool? Exclusive { get; set; }

        public virtual Tasksproduzione Task { get; set; }
        public virtual Users UserNavigation { get; set; }
    }
}
