using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Test
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
