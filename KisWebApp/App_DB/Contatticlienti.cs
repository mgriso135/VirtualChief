using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Contatticlienti
    {
        public Contatticlienti()
        {
            ContatticlientiEmail = new HashSet<ContatticlientiEmail>();
            ContatticlientiPhone = new HashSet<ContatticlientiPhone>();
        }

        [Key]
        public int IdContatto { get; set; }
        public string Cliente { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Ruolo { get; set; }
        public string User { get; set; }

        public virtual Anagraficaclienti ClienteNavigation { get; set; }
        public virtual Users UserNavigation { get; set; }
        public virtual ICollection<ContatticlientiEmail> ContatticlientiEmail { get; set; }
        public virtual ICollection<ContatticlientiPhone> ContatticlientiPhone { get; set; }
    }
}
