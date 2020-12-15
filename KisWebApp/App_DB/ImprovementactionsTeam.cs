using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class ImprovementactionsTeam
    {
        [Key, Column(Order = 0)]
        public int ImprovementActionId { get; set; }

        [Key, Column(Order = 1)]
        public short ImprovementActionYear { get; set; }
        public string User { get; set; }
        public string Role { get; set; }

        public virtual Improvementactions ImprovementAction { get; set; }
        public virtual Users UserNavigation { get; set; }
    }
}
