using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIS.App_DB
{
    public partial class Manuals
    {
        public Manuals()
        {
            //Manualswilabels = new HashSet<Manualswilabels>();
            //Tasksmanuals = new HashSet<Tasksmanuals>();
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        public int Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public byte IsActive { get; set; }
        public string User { get; set; }

     /*   public virtual Users UserNavigation { get; set; }
        public virtual ICollection<Manualswilabels> Manualswilabels { get; set; }
        public virtual ICollection<Tasksmanuals> Tasksmanuals { get; set; }*/
    }
}
//'Extent1.Users_UserId