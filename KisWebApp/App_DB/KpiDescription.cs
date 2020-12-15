using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class KpiDescription
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Idprocesso { get; set; }
        public int Revisione { get; set; }
        public bool? Attivo { get; set; }
        public float? Baseval { get; set; }

        public virtual Processo Processo { get; set; }
    }
}
