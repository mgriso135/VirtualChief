using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Taskusermodel
    {
        [Key, Column(Order = 0)]
        public int Taskid { get; set; }

        [Key, Column(Order = 1)]
        public int Taskrev { get; set; }

        [Key, Column(Order = 2)]
        public int Variantid { get; set; }

        [Key, Column(Order = 3)]
        public string User { get; set; }
        public bool? Exclusive { get; set; }

        public virtual Users UserNavigation { get; set; }
    }
}
