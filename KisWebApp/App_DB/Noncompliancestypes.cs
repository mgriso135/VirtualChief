using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Noncompliancestypes
    {
        public Noncompliancestypes()
        {
            NoncompliancestypeNc = new HashSet<NoncompliancestypeNc>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<NoncompliancestypeNc> NoncompliancestypeNc { get; set; }
    }
}
