using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Noncompliances
    {
        public Noncompliances()
        {
            NoncompliancesProducts = new HashSet<NoncompliancesProducts>();
            NoncompliancescauseNc = new HashSet<NoncompliancescauseNc>();
            NoncompliancestypeNc = new HashSet<NoncompliancestypeNc>();
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        public short Year { get; set; }
        public int? Quantity { get; set; }
        public string User { get; set; }
        public string Description { get; set; }
        public string ImmediateAction { get; set; }
        public double? Cost { get; set; }
        public string Status { get; set; }

        public virtual ICollection<NoncompliancesProducts> NoncompliancesProducts { get; set; }
        public virtual ICollection<NoncompliancescauseNc> NoncompliancescauseNc { get; set; }
        public virtual ICollection<NoncompliancestypeNc> NoncompliancestypeNc { get; set; }
    }
}
