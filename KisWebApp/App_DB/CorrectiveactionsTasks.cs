using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class CorrectiveactionsTasks
    {
        [Key, Column(Order = 0)]
        public int ImprovementActionId { get; set; }
        [Key, Column(Order = 1)]
        public short ImprovementActionYear { get; set; }
        public int CorrectiveActionId { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string User { get; set; }

        public virtual Users UserNavigation { get; set; }
    }
}
