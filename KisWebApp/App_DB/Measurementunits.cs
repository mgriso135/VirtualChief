using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Measurementunits
    {
        public Measurementunits()
        {
            Productionplan = new HashSet<Productionplan>();
            Variantiprocessi = new HashSet<Variantiprocessi>();
        }
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool? IsDefault { get; set; }

        public virtual ICollection<Productionplan> Productionplan { get; set; }
        public virtual ICollection<Variantiprocessi> Variantiprocessi { get; set; }
    }
}
