using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Workinstructionslabel
    {
        public Workinstructionslabel()
        {
            Manualswilabels = new HashSet<Manualswilabels>();
        }

        [Key]
        public int WilabelId { get; set; }
        public string WilabelName { get; set; }

        public virtual ICollection<Manualswilabels> Manualswilabels { get; set; }
    }
}
