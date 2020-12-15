using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Registroeventiproduzione
    {
        [Key, Column(Order = 0)]
        public string TipoEvento { get; set; }

        [Key, Column(Order = 1)]
        public int TaskId { get; set; }
        public bool? Segnalato { get; set; }

        public virtual Tasksproduzione Task { get; set; }
    }
}
