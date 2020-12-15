using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Improvementactions
    {
        public Improvementactions()
        {
            Correctiveactions = new HashSet<Correctiveactions>();
            ImprovementactionsTeam = new HashSet<ImprovementactionsTeam>();
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public short Year { get; set; }
        public string CreatedBy { get; set; }
        public string CurrentSituation { get; set; }
        public string ExpectedResults { get; set; }
        public string RootCauses { get; set; }
        public string ClosureNotes { get; set; }
        public string Status { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<Correctiveactions> Correctiveactions { get; set; }
        public virtual ICollection<ImprovementactionsTeam> ImprovementactionsTeam { get; set; }
    }
}
