using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Syslog
    {
        [Key, Column(Order = 0)]
        public string User { get; set; }
        [Key, Column(Order = 1)]
        public string Module { get; set; }
        [Key, Column(Order = 2)]
        public string Itemtype { get; set; }
        [Key, Column(Order = 3)]
        public string Parameter { get; set; }
        [Key, Column(Order = 4)]
        public string Itemid { get; set; }
        [Key, Column(Order = 5)]
        public string Oldvalue { get; set; }
        [Key, Column(Order = 6)]
        public string Newvalue { get; set; }
        public string Notes { get; set; }
    }
}
