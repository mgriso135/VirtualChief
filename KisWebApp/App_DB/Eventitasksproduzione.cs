using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Eventitasksproduzione
    {
        [Key, Column(Order = 0)]
        public int Task { get; set; }
        [Key, Column(Order = 1)]
        public int Cadenza { get; set; }
        [Key, Column(Order = 2)]
        public string Evento { get; set; }

        public virtual Tasksproduzione TaskNavigation { get; set; }
    }
}
