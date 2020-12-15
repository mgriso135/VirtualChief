using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Configurazione
    {
        [Key, Column(Order = 0)]
        public string Sezione { get; set; }
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        [Key, Column(Order = 2)]
        public string Parametro { get; set; }
        public string Valore { get; set; }
    }
}
