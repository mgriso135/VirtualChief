using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Taskstimespans
    {
        [Key]
        public int Id { get; set; }
        public string Userid { get; set; }
        public int Taskid { get; set; }
        public int Starteventid { get; set; }
        public string Starteventtype { get; set; }
        public int Endeventid { get; set; }
        public string Endeventtype { get; set; }
        public double DurationSec { get; set; }

        public virtual Registroeventitaskproduzione Endevent { get; set; }
        public virtual Registroeventitaskproduzione Startevent { get; set; }
        public virtual Tasksproduzione Task { get; set; }
    }
}
