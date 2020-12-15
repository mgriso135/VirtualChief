using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Tasksproduzioneoperatornotes
    {
        [Key, Column(Order = 0)]
        public int TaskId { get; set; }
        [Key, Column(Order = 1)]
        public int CommentId { get; set; }
        public string User { get; set; }
        public string Notes { get; set; }

        public virtual Tasksproduzione Task { get; set; }
    }
}
