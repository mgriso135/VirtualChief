using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KIS.App_DB;

namespace KIS.App_DB
{
    public partial class ContatticlientiPhone
    {
        [Key, Column(Order = 0)]
        public int IdContatto { get; set; }
        [Key, Column(Order = 1)]
        public string Phone { get; set; }
        [Key, Column(Order = 2)]
        public string Note { get; set; }

        public virtual Contatticlienti IdContattoNavigation { get; set; }
    }
}
