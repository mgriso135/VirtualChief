using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Warningproduzione
    {
        public Warningproduzione()
        {
            NoncompliancesProducts = new HashSet<NoncompliancesProducts>();
        }

        [Key] 
        public int Id { get; set; }
        public int Task { get; set; }
        public string User { get; set; }
        public string Motivo { get; set; }
        public string Risoluzione { get; set; }

        public virtual Tasksproduzione TaskNavigation { get; set; }
        public virtual Users UserNavigation { get; set; }
        public virtual ICollection<NoncompliancesProducts> NoncompliancesProducts { get; set; }
    }
}
