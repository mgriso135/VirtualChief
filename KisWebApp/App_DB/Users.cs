using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIS.App_DB
{
    public partial class Users
    {
        public Users()
        {
            Contatticlienti = new HashSet<Contatticlienti>();
            CorrectiveactionsTasks = new HashSet<CorrectiveactionsTasks>();
            CorrectiveactionsTeam = new HashSet<CorrectiveactionsTeam>();
            Eventoarticoloutenti = new HashSet<Eventoarticoloutenti>();
            Eventocommessautenti = new HashSet<Eventocommessautenti>();
            Eventorepartoutenti = new HashSet<Eventorepartoutenti>();
            Groupusers = new HashSet<Groupusers>();
            Homeboxesuser = new HashSet<Homeboxesuser>();
            ImprovementactionsTeam = new HashSet<ImprovementactionsTeam>();
            Manuals = new HashSet<Manuals>();
            Operatorireparto = new HashSet<Operatorireparto>();
            Processowners = new HashSet<Processowners>();
            Productionplan = new HashSet<Productionplan>();
            Registroeventitaskproduzione = new HashSet<Registroeventitaskproduzione>();
            Taskparameters = new HashSet<Taskparameters>();
            Taskuser = new HashSet<Taskuser>();
            Taskusermodel = new HashSet<Taskusermodel>();
            Useremail = new HashSet<Useremail>();
            Userphonenumbers = new HashSet<Userphonenumbers>();
            Warningproduzione = new HashSet<Warningproduzione>();
        }

        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string TipoUtente { get; set; }
        public int Id { get; set; }
        public string Language { get; set; }
        public string Region { get; set; }
        public bool Verified { get; set; }
        public string Checksum { get; set; }
        public string DestinationUrl { get; set; }
        public bool? Enabled { get; set; }

        public virtual ICollection<Contatticlienti> Contatticlienti { get; set; }
        public virtual ICollection<CorrectiveactionsTasks> CorrectiveactionsTasks { get; set; }
        public virtual ICollection<CorrectiveactionsTeam> CorrectiveactionsTeam { get; set; }
        public virtual ICollection<Eventoarticoloutenti> Eventoarticoloutenti { get; set; }
        public virtual ICollection<Eventocommessautenti> Eventocommessautenti { get; set; }
        public virtual ICollection<Eventorepartoutenti> Eventorepartoutenti { get; set; }
        public virtual ICollection<Groupusers> Groupusers { get; set; }
        public virtual ICollection<Homeboxesuser> Homeboxesuser { get; set; }
        public virtual ICollection<ImprovementactionsTeam> ImprovementactionsTeam { get; set; }
        public virtual ICollection<Manuals> Manuals { get; set; }
        public virtual ICollection<Operatorireparto> Operatorireparto { get; set; }
        public virtual ICollection<Processowners> Processowners { get; set; }
        public virtual ICollection<Productionplan> Productionplan { get; set; }
        public virtual ICollection<Registroeventitaskproduzione> Registroeventitaskproduzione { get; set; }
        public virtual ICollection<Taskparameters> Taskparameters { get; set; }
        public virtual ICollection<Taskuser> Taskuser { get; set; }
        public virtual ICollection<Taskusermodel> Taskusermodel { get; set; }
        public virtual ICollection<Useremail> Useremail { get; set; }
        public virtual ICollection<Userphonenumbers> Userphonenumbers { get; set; }
        public virtual ICollection<Warningproduzione> Warningproduzione { get; set; }
    }
}
