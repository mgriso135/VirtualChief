using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Tipipermessi
    {
        [Key]
        public string Id { get; set; }
        public string Descrizione { get; set; }
    }
}
