using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Processipadrifigli
    {
        [Key, Column(Order = 0)]
        public int Task { get; set; }
        [Key, Column(Order = 1)]
        public int RevTask { get; set; }
        [Key, Column(Order = 2)]
        public int Padre { get; set; }
        [Key, Column(Order = 3)]
        public int RevPadre { get; set; }
        [Key, Column(Order = 4)]
        public int Variante { get; set; }
        public int Posx { get; set; }
        public int Posy { get; set; }
    }
}
