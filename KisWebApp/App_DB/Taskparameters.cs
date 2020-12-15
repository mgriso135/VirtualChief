using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Taskparameters
    {
        [Key, Column(Order = 0)]
        public int TaskId { get; set; }
        [Key, Column(Order = 1)]
        public int ParamId { get; set; }
        public int ParamCategory { get; set; }
        public string ParamName { get; set; }
        public string ParamDescription { get; set; }
        public bool? IsFixed { get; set; }
        public bool? IsRequired { get; set; }
        public int? Sequence { get; set; }
        public string CreatedBy { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Productparameterscategories ParamCategoryNavigation { get; set; }
    }
}
