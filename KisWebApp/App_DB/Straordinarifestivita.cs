using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Straordinarifestivita
    {
        [Key]
        public int Id { get; set; }
        public string Azione { get; set; }
        public int Turno { get; set; }

        public virtual Turniproduzione TurnoNavigation { get; set; }
    }
}
