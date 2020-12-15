using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Productparameterscategories
    {
        public Productparameterscategories()
        {
            Modelparameters = new HashSet<Modelparameters>();
            Modeltaskparameters = new HashSet<Modeltaskparameters>();
            Productparameters = new HashSet<Productparameters>();
            Taskparameters = new HashSet<Taskparameters>();
        }
        [Key]
        public int ParamCatId { get; set; }
        public string ParamCatName { get; set; }
        public string ParamCatDescription { get; set; }

        public virtual ICollection<Modelparameters> Modelparameters { get; set; }
        public virtual ICollection<Modeltaskparameters> Modeltaskparameters { get; set; }
        public virtual ICollection<Productparameters> Productparameters { get; set; }
        public virtual ICollection<Taskparameters> Taskparameters { get; set; }
    }
}
