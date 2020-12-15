using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class ContatticlientiEmail
    {
        [Key, Column(Order = 0)]
        public int IdContatto { get; set; }

        [Key, Column(Order = 1)]
        public string Email { get; set; }
        public string Note { get; set; }

        public virtual Contatticlienti IdContattoNavigation { get; set; }
    }
}
