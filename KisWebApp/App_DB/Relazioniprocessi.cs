using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Relazioniprocessi
    {
        public Relazioniprocessi()
        {
            Precedenzeprocessi = new HashSet<Precedenzeprocessi>();
        }

        [Key]
        public int RelazioneId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public virtual ICollection<Precedenzeprocessi> Precedenzeprocessi { get; set; }
    }
}
