using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Productparameters
    {
        [Key, Column(Order = 0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public short ProductYear { get; set; }
        [Key, Column(Order = 2)]
        public int ParamId { get; set; }
        public int ParamCategory { get; set; }
        public string ParamName { get; set; }
        public string ParamDescription { get; set; }
        public bool? IsFixed { get; set; }
        public int Sequence { get; set; }

        public virtual Productparameterscategories ParamCategoryNavigation { get; set; }
        public virtual Productionplan Product { get; set; }
    }
}
