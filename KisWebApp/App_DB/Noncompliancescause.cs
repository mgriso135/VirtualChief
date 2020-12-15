using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Noncompliancescause
    {
        public Noncompliancescause()
        {
            NoncompliancescauseNc = new HashSet<NoncompliancescauseNc>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<NoncompliancescauseNc> NoncompliancescauseNc { get; set; }
    }
}
