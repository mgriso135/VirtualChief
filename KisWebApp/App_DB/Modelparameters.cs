using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Modelparameters
    {
        [Key, Column(Order = 0)]
        public int ProcessId { get; set; }
        [Key, Column(Order = 1)]
        public int ProcessRev { get; set; }
        [Key, Column(Order = 2)]
        public int VarianteId { get; set; }
        [Key, Column(Order = 3)]
        public int ParamId { get; set; }
        public int ParamCategory { get; set; }
        public string ParamName { get; set; }
        public string ParamDescription { get; set; }
        public bool? IsFixed { get; set; }
        public bool? IsRequired { get; set; }
        public int Sequence { get; set; }

        public virtual Productparameterscategories ParamCategoryNavigation { get; set; }
    }
}
