using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class NoncompliancesProducts
    {
        [Key, Column(Order = 0)]
        public int NonComplianceId { get; set; }
        [Key, Column(Order = 1)]
        public short NonComplianceYear { get; set; }
        [Key, Column(Order = 2)]
        public int ProductId { get; set; }
        [Key, Column(Order = 3)]
        public short ProductYear { get; set; }
        public string Source { get; set; }
        public int? WarningId { get; set; }
        public int? Workstation { get; set; }
        public int Quantity { get; set; }

        public virtual Noncompliances NonCompliance { get; set; }
        public virtual Productionplan Product { get; set; }
        public virtual Warningproduzione Warning { get; set; }
        public virtual Postazioni WorkstationNavigation { get; set; }
    }
}
