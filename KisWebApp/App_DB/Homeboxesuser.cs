using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Homeboxesuser
    {
        [Key]
        public int IdHomeBox { get; set; }
        public string User { get; set; }
        public int Ordine { get; set; }

        public virtual Homeboxesregistro IdHomeBoxNavigation { get; set; }
        public virtual Users UserNavigation { get; set; }
    }
}
