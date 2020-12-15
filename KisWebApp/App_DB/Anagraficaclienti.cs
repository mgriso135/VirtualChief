using System;
using System.Collections.Generic;
using KIS.App_Code;
using KIS.Commesse;
using KIS.App_DB;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Anagraficaclienti
    {
        public Anagraficaclienti()
        {
            Commesse = new HashSet<Commesse>();
            Contatticlienti = new HashSet<Contatticlienti>();
        }

        [Key]
        public string Codice { get; set; }
        public string Ragsociale { get; set; }
        public string Partitaiva { get; set; }
        public string Codfiscale { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
        public string Provincia { get; set; }
        public string Cap { get; set; }
        public string Stato { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool? KanbanManaged { get; set; }

        public virtual ICollection<Commesse> Commesse { get; set; }
        public virtual ICollection<Contatticlienti> Contatticlienti { get; set; }
    }
}
