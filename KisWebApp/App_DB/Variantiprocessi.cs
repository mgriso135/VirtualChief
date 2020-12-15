using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Variantiprocessi
    {
        [Key, Column(Order = 0)]
        public int Variante { get; set; }
        [Key, Column(Order = 1)]
        public int Processo { get; set; }
        
        [Key, Column(Order = 2)]
        public int RevProc { get; set; }
        public string ExternalId { get; set; }
        public int MeasurementUnit { get; set; }

        public virtual Measurementunits MeasurementUnitNavigation { get; set; }
        public virtual Processo ProcessoNavigation { get; set; }
        public virtual Varianti VarianteNavigation { get; set; }
    }
}
