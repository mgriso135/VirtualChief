using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Groupss
    {
        public Groupss()
        {
            Eventoarticologruppi = new HashSet<Eventoarticologruppi>();
            Eventocommessagruppi = new HashSet<Eventocommessagruppi>();
            Eventorepartogruppi = new HashSet<Eventorepartogruppi>();
            Groupusers = new HashSet<Groupusers>();
            Gruppipermessi = new HashSet<Gruppipermessi>();
            Menugruppi = new HashSet<Menugruppi>();
        }

        [Key]
        public int Id { get; set; }
        public string NomeGruppo { get; set; }
        public string Descrizione { get; set; }

        public virtual ICollection<Eventoarticologruppi> Eventoarticologruppi { get; set; }
        public virtual ICollection<Eventocommessagruppi> Eventocommessagruppi { get; set; }
        public virtual ICollection<Eventorepartogruppi> Eventorepartogruppi { get; set; }
        public virtual ICollection<Groupusers> Groupusers { get; set; }
        public virtual ICollection<Gruppipermessi> Gruppipermessi { get; set; }
        public virtual ICollection<Menugruppi> Menugruppi { get; set; }
    }
}
