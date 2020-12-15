using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Gruppipermessi
    {
        [Key, Column(Order = 0)]
        public int Idgroup { get; set; }

        [Key, Column(Order = 1)]
        public int Idpermesso { get; set; }
        public bool? R { get; set; }
        public bool? W { get; set; }
        public bool? X { get; set; }

        public virtual Groupss IdgroupNavigation { get; set; }
        public virtual Permessi IdpermessoNavigation { get; set; }
    }
}
