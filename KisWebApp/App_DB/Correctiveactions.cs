using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Correctiveactions
    {
        public Correctiveactions()
        {
            CorrectiveactionsTeam = new HashSet<CorrectiveactionsTeam>();
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        public int ImprovementActionId { get; set; }
        [Key, Column(Order = 2)]
        public short ImprovementActionYear { get; set; }
        public string Description { get; set; }
        public double? LeadTimeExpected { get; set; }
        public string Status { get; set; }

        public virtual Improvementactions ImprovementAction { get; set; }
        public virtual ICollection<CorrectiveactionsTeam> CorrectiveactionsTeam { get; set; }
    }
}
