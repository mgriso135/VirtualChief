using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class CorrectiveactionsTeam
    {
        [Key, Column(Order = 0)]
        public int CorrectiveActionId { get; set; }
        [Key, Column(Order = 1)]
        public int ImprovementActionId { get; set; }
        [Key, Column(Order = 2)]
        public short ImprovementActionYear { get; set; }
        public string User { get; set; }
        public string Role { get; set; }

        public virtual Correctiveactions Correctiveactions { get; set; }
        public virtual Users UserNavigation { get; set; }
    }
}
