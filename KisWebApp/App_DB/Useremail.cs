using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Useremail
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public string Email { get; set; }
        public bool ForAlarm { get; set; }
        public string Note { get; set; }

        public virtual Users User { get; set; }
    }
}
