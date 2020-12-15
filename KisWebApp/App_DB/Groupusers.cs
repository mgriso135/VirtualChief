using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Groupusers
    {
        [Key, Column(Order = 0)]
        public int GroupId { get; set; }

        [Key, Column(Order = 1)]
        public string User { get; set; }

        public virtual Groupss Group { get; set; }
        public virtual Users UserNavigation { get; set; }
    }
}
