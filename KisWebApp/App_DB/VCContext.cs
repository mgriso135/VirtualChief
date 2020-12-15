using MySql.Data.EntityFramework;
using System;
using System.Data.Entity;

namespace KIS.App_DB
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]

    public partial class VCContext : DbContext
    {
        public VCContext(string connString) : base ("masterDB")
        {
        }

        /*public VCContext(DbContextOptions<VCContext> options)
            : base("longato")
        {
        }*/

        public virtual DbSet<Anagraficaclienti> Anagraficaclienti { get; set; }
        public virtual DbSet<Commesse> Commesse { get; set; }
        public virtual DbSet<Configurazione> Configurazione { get; set; }
        public virtual DbSet<Contatticlienti> Contatticlienti { get; set; }
        public virtual DbSet<ContatticlientiEmail> ContatticlientiEmail { get; set; }
        public virtual DbSet<ContatticlientiPhone> ContatticlientiPhone { get; set; }
        public virtual DbSet<Correctiveactions> Correctiveactions { get; set; }
        public virtual DbSet<CorrectiveactionsTasks> CorrectiveactionsTasks { get; set; }
        public virtual DbSet<CorrectiveactionsTeam> CorrectiveactionsTeam { get; set; }
        public virtual DbSet<Eventitasksproduzione> Eventitasksproduzione { get; set; }
        public virtual DbSet<Eventoarticoloconfig> Eventoarticoloconfig { get; set; }
        public virtual DbSet<Eventoarticologruppi> Eventoarticologruppi { get; set; }
        public virtual DbSet<Eventoarticoloutenti> Eventoarticoloutenti { get; set; }
        public virtual DbSet<Eventocommessaconfig> Eventocommessaconfig { get; set; }
        public virtual DbSet<Eventocommessagruppi> Eventocommessagruppi { get; set; }
        public virtual DbSet<Eventocommessautenti> Eventocommessautenti { get; set; }
        public virtual DbSet<Eventorepartoconfig> Eventorepartoconfig { get; set; }
        public virtual DbSet<Eventorepartogruppi> Eventorepartogruppi { get; set; }
        public virtual DbSet<Eventorepartoutenti> Eventorepartoutenti { get; set; }
        public virtual DbSet<Groupss> Groupss { get; set; }
        public virtual DbSet<Groupusers> Groupusers { get; set; }
        public virtual DbSet<Gruppipermessi> Gruppipermessi { get; set; }
        public virtual DbSet<Homeboxesregistro> Homeboxesregistro { get; set; }
        public virtual DbSet<Homeboxesuser> Homeboxesuser { get; set; }
        public virtual DbSet<Improvementactions> Improvementactions { get; set; }
        public virtual DbSet<ImprovementactionsTeam> ImprovementactionsTeam { get; set; }
        public virtual DbSet<KpiDescription> KpiDescription { get; set; }
        public virtual DbSet<Manuals> Manuals { get; set; }
        public virtual DbSet<Manualswilabels> Manualswilabels { get; set; }
        public virtual DbSet<Measurementunits> Measurementunits { get; set; }
        public virtual DbSet<Menualbero> Menualbero { get; set; }
        public virtual DbSet<Menugruppi> Menugruppi { get; set; }
        public virtual DbSet<Menuvoci> Menuvoci { get; set; }
        public virtual DbSet<Modelparameters> Modelparameters { get; set; }
        public virtual DbSet<Modeltaskparameters> Modeltaskparameters { get; set; }
        public virtual DbSet<Noncompliances> Noncompliances { get; set; }
        public virtual DbSet<NoncompliancesProducts> NoncompliancesProducts { get; set; }
        public virtual DbSet<Noncompliancescause> Noncompliancescause { get; set; }
        public virtual DbSet<NoncompliancescauseNc> NoncompliancescauseNc { get; set; }
        public virtual DbSet<NoncompliancestypeNc> NoncompliancestypeNc { get; set; }
        public virtual DbSet<Noncompliancestypes> Noncompliancestypes { get; set; }
        public virtual DbSet<Operatorireparto> Operatorireparto { get; set; }
        public virtual DbSet<Orarilavoroturni> Orarilavoroturni { get; set; }
        public virtual DbSet<Permessi> Permessi { get; set; }
        public virtual DbSet<Postazioni> Postazioni { get; set; }
        public virtual DbSet<Precedenzeprocessi> Precedenzeprocessi { get; set; }
        public virtual DbSet<Prectasksproduzione> Prectasksproduzione { get; set; }
        public virtual DbSet<Processipadrifigli> Processipadrifigli { get; set; }
        public virtual DbSet<Processo> Processo { get; set; }
        public virtual DbSet<Processowners> Processowners { get; set; }
        public virtual DbSet<Productionplan> Productionplan { get; set; }
        public virtual DbSet<Productparameters> Productparameters { get; set; }
        public virtual DbSet<Productparameterscategories> Productparameterscategories { get; set; }
        public virtual DbSet<Registroeventiproduzione> Registroeventiproduzione { get; set; }
        public virtual DbSet<Registroeventitaskproduzione> Registroeventitaskproduzione { get; set; }
        public virtual DbSet<Relazioniprocessi> Relazioniprocessi { get; set; }
        public virtual DbSet<Reparti> Reparti { get; set; }
        public virtual DbSet<Repartipostazioniattivita> Repartipostazioniattivita { get; set; }
        public virtual DbSet<Repartiprocessi> Repartiprocessi { get; set; }
        public virtual DbSet<Risorseturnopostazione> Risorseturnopostazione { get; set; }
        public virtual DbSet<Straordinarifestivita> Straordinarifestivita { get; set; }
        public virtual DbSet<Syslog> Syslog { get; set; }
        public virtual DbSet<Taskparameters> Taskparameters { get; set; }
        public virtual DbSet<Tasksmanuals> Tasksmanuals { get; set; }
        public virtual DbSet<Tasksproduzione> Tasksproduzione { get; set; }
        public virtual DbSet<Tasksproduzioneoperatornotes> Tasksproduzioneoperatornotes { get; set; }
        public virtual DbSet<Taskstimespans> Taskstimespans { get; set; }
        public virtual DbSet<Taskuser> Taskuser { get; set; }
        public virtual DbSet<Taskusermodel> Taskusermodel { get; set; }
        public virtual DbSet<Tempiciclo> Tempiciclo { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<Tipipermessi> Tipipermessi { get; set; }
        public virtual DbSet<Turniproduzione> Turniproduzione { get; set; }
        public virtual DbSet<Useremail> Useremail { get; set; }
        public virtual DbSet<Userphonenumbers> Userphonenumbers { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Varianti> Varianti { get; set; }
        public virtual DbSet<Variantiprocessi> Variantiprocessi { get; set; }
        public virtual DbSet<Warningproduzione> Warningproduzione { get; set; }
        public virtual DbSet<Workinstructionslabel> Workinstructionslabel { get; set; }

        // Unable to generate entity type for table 'longato.kpi_record'. Please see the warning messages.
        // Unable to generate entity type for table 'longato.registrooperatoripostazioni'. Please see the warning messages.
        // Unable to generate entity type for table 'longato.taskreschedulelog'. Please see the warning messages.
        // Unable to generate entity type for table 'longato.userslog'. Please see the warning messages.

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
                // See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=abcABC123+++;database=longato");
            }
        }*/
        
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anagraficaclienti>(entity =>
            {
                entity.HasKey(e => e.Codice)
                    .HasName("PRIMARY");

                entity.ToTable("anagraficaclienti");

                entity.Property(e => e.Codice)
                    .HasColumnName("codice")
                    .HasMaxLength(255);

                entity.Property(e => e.Cap)
                    .HasColumnName("CAP")
                    .HasMaxLength(255);

                entity.Property(e => e.Citta)
                    .HasColumnName("citta")
                    .HasMaxLength(255);

                entity.Property(e => e.Codfiscale)
                    .HasColumnName("codfiscale")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Indirizzo)
                    .HasColumnName("indirizzo")
                    .HasMaxLength(255);

                entity.Property(e => e.KanbanManaged)
                    .IsRequired()
                    .HasColumnName("kanbanManaged")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Partitaiva)
                    .HasColumnName("partitaiva")
                    .HasMaxLength(255);

                entity.Property(e => e.Provincia)
                    .HasColumnName("provincia")
                    .HasMaxLength(255);

                entity.Property(e => e.Ragsociale)
                    .IsRequired()
                    .HasColumnName("ragsociale")
                    .HasMaxLength(255);

                entity.Property(e => e.Stato)
                    .HasColumnName("stato")
                    .HasMaxLength(255);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Commesse>(entity =>
            {
                entity.HasKey(e => new { e.Idcommesse, e.Anno })
                    .HasName("PRIMARY");

                entity.ToTable("commesse");

                entity.HasIndex(e => e.Cliente)
                    .HasName("FK_cliente_idx");

                entity.Property(e => e.Idcommesse)
                    .HasColumnName("idcommesse")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Anno)
                    .HasColumnName("anno")
                    .HasColumnType("year(4)");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasColumnName("cliente")
                    .HasMaxLength(255);

                entity.Property(e => e.Confirmed)
                    .IsRequired()
                    .HasColumnName("confirmed")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.ConfirmedBy)
                    .HasColumnName("confirmedBy")
                    .HasMaxLength(45);

                entity.Property(e => e.ExternalId)
                    .HasColumnName("ExternalID")
                    .HasMaxLength(255);

                entity.Property(e => e.Note).HasColumnName("note");

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.Commesse)
                    .HasForeignKey(d => d.Cliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cliente");
            });

            modelBuilder.Entity<Configurazione>(entity =>
            {
                entity.HasKey(e => new { e.Sezione, e.Id, e.Parametro })
                    .HasName("PRIMARY");

                entity.ToTable("configurazione");

                entity.Property(e => e.Sezione).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Parametro)
                    .HasColumnName("parametro")
                    .HasMaxLength(255);

                entity.Property(e => e.Valore)
                    .IsRequired()
                    .HasColumnName("valore")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Contatticlienti>(entity =>
            {
                entity.HasKey(e => e.IdContatto)
                    .HasName("PRIMARY");

                entity.ToTable("contatticlienti");

                entity.HasIndex(e => e.Cliente)
                    .HasName("FKcontatto_cliente_idx");

                entity.HasIndex(e => e.User)
                    .HasName("FKcontatticliente_user_idx");

                entity.Property(e => e.IdContatto)
                    .HasColumnName("idContatto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasColumnName("cliente")
                    .HasMaxLength(255);

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasMaxLength(255);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(255);

                entity.Property(e => e.Ruolo)
                    .HasColumnName("ruolo")
                    .HasMaxLength(255);

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.Contatticlienti)
                    .HasForeignKey(d => d.Cliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKcontatto_cliente");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Contatticlienti)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FKcontatticliente_user");
            });

            modelBuilder.Entity<ContatticlientiEmail>(entity =>
            {
                entity.HasKey(e => new { e.IdContatto, e.Email })
                    .HasName("PRIMARY");

                entity.ToTable("contatticlienti_email");

                entity.HasIndex(e => e.IdContatto)
                    .HasName("FK_contatto_email_idx");

                entity.Property(e => e.IdContatto)
                    .HasColumnName("idContatto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(255);

                entity.HasOne(d => d.IdContattoNavigation)
                    .WithMany(p => p.ContatticlientiEmail)
                    .HasForeignKey(d => d.IdContatto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_contatto_email");
            });

            modelBuilder.Entity<ContatticlientiPhone>(entity =>
            {
                entity.HasKey(e => new { e.IdContatto, e.Phone })
                    .HasName("PRIMARY");

                entity.ToTable("contatticlienti_phone");

                entity.HasIndex(e => e.IdContatto)
                    .HasName("contattiphone_FK_contatto_idx");

                entity.Property(e => e.IdContatto)
                    .HasColumnName("idContatto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(255);

                entity.HasOne(d => d.IdContattoNavigation)
                    .WithMany(p => p.ContatticlientiPhone)
                    .HasForeignKey(d => d.IdContatto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contattiphone_FK_contatto");
            });

            modelBuilder.Entity<Correctiveactions>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ImprovementActionId, e.ImprovementActionYear })
                    .HasName("PRIMARY");

                entity.ToTable("correctiveactions");

                entity.HasIndex(e => new { e.ImprovementActionId, e.ImprovementActionYear })
                    .HasName("ImprovementActions_AC_FK_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImprovementActionId)
                    .HasColumnName("ImprovementActionID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImprovementActionYear).HasColumnType("year(4)");

                entity.Property(e => e.LeadTimeExpected).HasComment("giorni di lavoro previsti");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("{O = Aperta, C = Chiusa}");

                entity.HasOne(d => d.ImprovementAction)
                    .WithMany(p => p.Correctiveactions)
                    .HasForeignKey(d => new { d.ImprovementActionId, d.ImprovementActionYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ImprovementActions_AC_FK");
            });

            modelBuilder.Entity<CorrectiveactionsTasks>(entity =>
            {
                entity.HasKey(e => new { e.ImprovementActionId, e.ImprovementActionYear, e.CorrectiveActionId, e.TaskId })
                    .HasName("PRIMARY");

                entity.ToTable("correctiveactions_tasks");

                entity.HasIndex(e => e.User)
                    .HasName("CA_Tasks_user_FK");

                entity.Property(e => e.ImprovementActionId)
                    .HasColumnName("ImprovementActionID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImprovementActionYear).HasColumnType("year(4)");

                entity.Property(e => e.CorrectiveActionId)
                    .HasColumnName("CorrectiveActionID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.CorrectiveactionsTasks)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CA_Tasks_user_FK");
            });

            modelBuilder.Entity<CorrectiveactionsTeam>(entity =>
            {
                entity.HasKey(e => new { e.CorrectiveActionId, e.ImprovementActionId, e.ImprovementActionYear, e.User })
                    .HasName("PRIMARY");

                entity.ToTable("correctiveactions_team");

                entity.HasIndex(e => e.User)
                    .HasName("CA_Team_user_FK");

                entity.Property(e => e.CorrectiveActionId)
                    .HasColumnName("CorrectiveActionID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImprovementActionId)
                    .HasColumnName("ImprovementActionID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImprovementActionYear).HasColumnType("year(4)");

                entity.Property(e => e.User).HasMaxLength(255);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.CorrectiveactionsTeam)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CA_Team_user_FK");

                entity.HasOne(d => d.Correctiveactions)
                    .WithMany(p => p.CorrectiveactionsTeam)
                    .HasForeignKey(d => new { d.CorrectiveActionId, d.ImprovementActionId, d.ImprovementActionYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CorrectiveAction_Team_FK");
            });

            modelBuilder.Entity<Eventitasksproduzione>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("eventitasksproduzione");

                entity.HasIndex(e => e.Task)
                    .HasName("task");

                entity.Property(e => e.Cadenza)
                    .HasColumnName("cadenza")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Evento)
                    .IsRequired()
                    .HasColumnName("evento")
                    .HasMaxLength(1);

                entity.Property(e => e.Task)
                    .HasColumnName("task")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.TaskNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Task)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventitasksproduzione_ibfk_1");
            });

            modelBuilder.Entity<Eventoarticoloconfig>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.ArticoloId, e.ArticoloAnno })
                    .HasName("PRIMARY");

                entity.ToTable("eventoarticoloconfig");

                entity.HasIndex(e => new { e.ArticoloId, e.ArticoloAnno })
                    .HasName("eventoarticoloconfig_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.ArticoloId)
                    .HasColumnName("ArticoloID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ArticoloAnno).HasColumnType("year(4)");

                entity.HasOne(d => d.Articolo)
                    .WithMany(p => p.Eventoarticoloconfig)
                    .HasForeignKey(d => new { d.ArticoloId, d.ArticoloAnno })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventoarticoloconfig_FK1");
            });

            modelBuilder.Entity<Eventoarticologruppi>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.IdGruppo, e.ArticoloAnno, e.ArticoloId })
                    .HasName("PRIMARY");

                entity.ToTable("eventoarticologruppi");

                entity.HasIndex(e => e.IdGruppo)
                    .HasName("eventoarticologruppi_FK2");

                entity.HasIndex(e => new { e.ArticoloId, e.ArticoloAnno })
                    .HasName("eventoarticologruppi_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.IdGruppo)
                    .HasColumnName("idGruppo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ArticoloAnno).HasColumnType("year(4)");

                entity.Property(e => e.ArticoloId)
                    .HasColumnName("ArticoloID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdGruppoNavigation)
                    .WithMany(p => p.Eventoarticologruppi)
                    .HasForeignKey(d => d.IdGruppo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventoarticologruppi_FK2");

                entity.HasOne(d => d.Articolo)
                    .WithMany(p => p.Eventoarticologruppi)
                    .HasForeignKey(d => new { d.ArticoloId, d.ArticoloAnno })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventoarticologruppi_FK1");
            });

            modelBuilder.Entity<Eventoarticoloutenti>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.UserId, e.ArticoloAnno, e.ArticoloId })
                    .HasName("PRIMARY");

                entity.ToTable("eventoarticoloutenti");

                entity.HasIndex(e => e.UserId)
                    .HasName("eventoarticoloutenti_FK2");

                entity.HasIndex(e => new { e.ArticoloId, e.ArticoloAnno })
                    .HasName("eventoarticoloutenti_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(255);

                entity.Property(e => e.ArticoloAnno).HasColumnType("year(4)");

                entity.Property(e => e.ArticoloId)
                    .HasColumnName("ArticoloID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Eventoarticoloutenti)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventoarticoloutenti_FK2");

                entity.HasOne(d => d.Articolo)
                    .WithMany(p => p.Eventoarticoloutenti)
                    .HasForeignKey(d => new { d.ArticoloId, d.ArticoloAnno })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventoarticoloutenti_FK1");
            });

            modelBuilder.Entity<Eventocommessaconfig>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.CommessaAnno, e.CommessaId })
                    .HasName("PRIMARY");

                entity.ToTable("eventocommessaconfig");

                entity.HasIndex(e => new { e.CommessaId, e.CommessaAnno })
                    .HasName("eventocommessaconfig_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.CommessaAnno).HasColumnType("year(4)");

                entity.Property(e => e.CommessaId)
                    .HasColumnName("CommessaID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Commessa)
                    .WithMany(p => p.Eventocommessaconfig)
                    .HasForeignKey(d => new { d.CommessaId, d.CommessaAnno })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventocommessaconfig_FK1");
            });

            modelBuilder.Entity<Eventocommessagruppi>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.CommessaId, e.CommessaAnno, e.IdGruppo })
                    .HasName("PRIMARY");

                entity.ToTable("eventocommessagruppi");

                entity.HasIndex(e => e.IdGruppo)
                    .HasName("eventocommessagruppi_FK2");

                entity.HasIndex(e => new { e.CommessaId, e.CommessaAnno })
                    .HasName("eventocommessagruppi_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.CommessaId)
                    .HasColumnName("CommessaID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommessaAnno).HasColumnType("year(4)");

                entity.Property(e => e.IdGruppo)
                    .HasColumnName("idGruppo")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdGruppoNavigation)
                    .WithMany(p => p.Eventocommessagruppi)
                    .HasForeignKey(d => d.IdGruppo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventocommessagruppi_FK2");

                entity.HasOne(d => d.Commessa)
                    .WithMany(p => p.Eventocommessagruppi)
                    .HasForeignKey(d => new { d.CommessaId, d.CommessaAnno })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventocommessagruppi_FK1");
            });

            modelBuilder.Entity<Eventocommessautenti>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.CommessaId, e.CommessaAnno, e.UserId })
                    .HasName("PRIMARY");

                entity.ToTable("eventocommessautenti");

                entity.HasIndex(e => e.UserId)
                    .HasName("eventocommessautenti_FK2");

                entity.HasIndex(e => new { e.CommessaId, e.CommessaAnno })
                    .HasName("eventocommessautenti_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.CommessaId)
                    .HasColumnName("CommessaID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommessaAnno).HasColumnType("year(4)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Eventocommessautenti)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventocommessautenti_FK2");

                entity.HasOne(d => d.Commessa)
                    .WithMany(p => p.Eventocommessautenti)
                    .HasForeignKey(d => new { d.CommessaId, d.CommessaAnno })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventocommessautenti_FK1");
            });

            modelBuilder.Entity<Eventorepartoconfig>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.Reparto })
                    .HasName("PRIMARY");

                entity.ToTable("eventorepartoconfig");

                entity.HasIndex(e => e.Reparto)
                    .HasName("eventorepartoconfig_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.Reparto).HasColumnType("int(11)");

                entity.HasOne(d => d.RepartoNavigation)
                    .WithMany(p => p.Eventorepartoconfig)
                    .HasForeignKey(d => d.Reparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("eventorepartoconfig_FK1");
            });

            modelBuilder.Entity<Eventorepartogruppi>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.IdReparto, e.IdGruppo })
                    .HasName("PRIMARY");

                entity.ToTable("eventorepartogruppi");

                entity.HasIndex(e => e.IdGruppo)
                    .HasName("evRepGruppi_FK2");

                entity.HasIndex(e => e.IdReparto)
                    .HasName("evRepGruppi_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.IdReparto)
                    .HasColumnName("idReparto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdGruppo)
                    .HasColumnName("idGruppo")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdGruppoNavigation)
                    .WithMany(p => p.Eventorepartogruppi)
                    .HasForeignKey(d => d.IdGruppo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("evRepGruppi_FK2");

                entity.HasOne(d => d.IdRepartoNavigation)
                    .WithMany(p => p.Eventorepartogruppi)
                    .HasForeignKey(d => d.IdReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("evRepGruppi_FK1");
            });

            modelBuilder.Entity<Eventorepartoutenti>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.RepartoId, e.UserId })
                    .HasName("PRIMARY");

                entity.ToTable("eventorepartoutenti");

                entity.HasIndex(e => e.RepartoId)
                    .HasName("evrepartoutenti_FK1");

                entity.HasIndex(e => e.UserId)
                    .HasName("evrepartoutenti_FK2");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.RepartoId)
                    .HasColumnName("repartoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Reparto)
                    .WithMany(p => p.Eventorepartoutenti)
                    .HasForeignKey(d => d.RepartoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("evrepartoutenti_FK1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Eventorepartoutenti)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("evrepartoutenti_FK2");
            });

            modelBuilder.Entity<Groupss>(entity =>
            {
                entity.ToTable("groupss");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descrizione).HasColumnName("descrizione");

                entity.Property(e => e.NomeGruppo)
                    .IsRequired()
                    .HasColumnName("nomeGruppo")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Groupusers>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.User })
                    .HasName("PRIMARY");

                entity.ToTable("groupusers");

                entity.HasIndex(e => e.User)
                    .HasName("user");

                entity.Property(e => e.GroupId)
                    .HasColumnName("groupID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Groupusers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("groupusers_ibfk_1");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Groupusers)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("groupusers_ibfk_2");
            });

            modelBuilder.Entity<Gruppipermessi>(entity =>
            {
                entity.HasKey(e => new { e.Idgroup, e.Idpermesso })
                    .HasName("PRIMARY");

                entity.ToTable("gruppipermessi");

                entity.HasIndex(e => e.Idgroup)
                    .HasName("gruppipermessi_FK1");

                entity.HasIndex(e => e.Idpermesso)
                    .HasName("gruppipermessi_FK2");

                entity.Property(e => e.Idgroup)
                    .HasColumnName("idgroup")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idpermesso)
                    .HasColumnName("idpermesso")
                    .HasColumnType("int(11)");

                entity.Property(e => e.R)
                    .HasColumnName("r")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.W)
                    .HasColumnName("w")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.X)
                    .HasColumnName("x")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.IdgroupNavigation)
                    .WithMany(p => p.Gruppipermessi)
                    .HasForeignKey(d => d.Idgroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("gruppipermessi_FK1");

                entity.HasOne(d => d.IdpermessoNavigation)
                    .WithMany(p => p.Gruppipermessi)
                    .HasForeignKey(d => d.Idpermesso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("gruppipermessi_FK2");
            });

            modelBuilder.Entity<Homeboxesregistro>(entity =>
            {
                entity.HasKey(e => e.IdHomeBox)
                    .HasName("PRIMARY");

                entity.ToTable("homeboxesregistro");

                entity.Property(e => e.IdHomeBox)
                    .HasColumnName("idHomeBox")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descrizione).HasColumnName("descrizione");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Homeboxesuser>(entity =>
            {
                entity.HasKey(e => new { e.IdHomeBox, e.User })
                    .HasName("PRIMARY");

                entity.ToTable("homeboxesuser");

                entity.HasIndex(e => e.IdHomeBox)
                    .HasName("FK_boxes_box_idx");

                entity.HasIndex(e => e.User)
                    .HasName("FK_boxes_user_idx");

                entity.Property(e => e.IdHomeBox)
                    .HasColumnName("idHomeBox")
                    .HasColumnType("int(11)");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.Property(e => e.Ordine)
                    .HasColumnName("ordine")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdHomeBoxNavigation)
                    .WithMany(p => p.Homeboxesuser)
                    .HasForeignKey(d => d.IdHomeBox)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_boxes_box");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Homeboxesuser)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_boxes_user");
            });

            modelBuilder.Entity<Improvementactions>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Year })
                    .HasName("PRIMARY");

                entity.ToTable("improvementactions");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Year).HasColumnType("year(4)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ModifiedBy).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("{1 = Aperto, 2 = Chiuso}");
            });

            modelBuilder.Entity<ImprovementactionsTeam>(entity =>
            {
                entity.HasKey(e => new { e.ImprovementActionId, e.ImprovementActionYear, e.User })
                    .HasName("PRIMARY");

                entity.ToTable("improvementactions_team");

                entity.HasIndex(e => e.User)
                    .HasName("Improvement_user_FK");

                entity.Property(e => e.ImprovementActionId)
                    .HasColumnName("ImprovementActionID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImprovementActionYear).HasColumnType("year(4)");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ImprovementactionsTeam)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Improvement_user_FK");

                entity.HasOne(d => d.ImprovementAction)
                    .WithMany(p => p.ImprovementactionsTeam)
                    .HasForeignKey(d => new { d.ImprovementActionId, d.ImprovementActionYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Improvement_Team_FK");
            });

            modelBuilder.Entity<KpiDescription>(entity =>
            {
                entity.ToTable("kpi_description");

                entity.HasIndex(e => new { e.Idprocesso, e.Revisione })
                    .HasName("process_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Attivo)
                    .IsRequired()
                    .HasColumnName("attivo")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.Baseval).HasColumnName("baseval");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Idprocesso)
                    .HasColumnName("idprocesso")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Revisione)
                    .HasColumnName("revisione")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Processo)
                    .WithMany(p => p.KpiDescription)
                    .HasForeignKey(d => new { d.Idprocesso, d.Revisione })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("process_FK");
            });

            modelBuilder.Entity<Manuals>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Version })
                    .HasName("PRIMARY");

                entity.ToTable("manuals");

                entity.HasIndex(e => e.User)
                    .HasName("user_FK1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Version).HasColumnType("int(11)");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasMaxLength(255);

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Manuals)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_FK10");
            });

            modelBuilder.Entity<Manualswilabels>(entity =>
            {
                entity.HasKey(e => new { e.ManualId, e.ManualVersion, e.LabelId })
                    .HasName("PRIMARY");

                entity.ToTable("manualswilabels");

                entity.HasIndex(e => e.LabelId)
                    .HasName("Label_FK1_idx");

                entity.Property(e => e.ManualId)
                    .HasColumnName("ManualID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ManualVersion).HasColumnType("int(11)");

                entity.Property(e => e.LabelId)
                    .HasColumnName("LabelID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Label)
                    .WithMany(p => p.Manualswilabels)
                    .HasForeignKey(d => d.LabelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Label_FK1");

                entity.HasOne(d => d.Manual)
                    .WithMany(p => p.Manualswilabels)
                    .HasForeignKey(d => new { d.ManualId, d.ManualVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Manual_FK2");
            });

            modelBuilder.Entity<Measurementunits>(entity =>
            {
                entity.ToTable("measurementunits");

                entity.HasIndex(e => e.Type)
                    .HasName("type_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.IsDefault)
                    .HasColumnName("isDefault")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Menualbero>(entity =>
            {
                entity.HasKey(e => new { e.IdPadre, e.IdFiglio })
                    .HasName("PRIMARY");

                entity.ToTable("menualbero");

                entity.HasIndex(e => e.IdFiglio)
                    .HasName("menualbero_FK2");

                entity.HasIndex(e => e.IdPadre)
                    .HasName("menualbero_FK1");

                entity.Property(e => e.IdPadre)
                    .HasColumnName("idPadre")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdFiglio)
                    .HasColumnName("idFiglio")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ordinamento)
                    .HasColumnName("ordinamento")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdFiglioNavigation)
                    .WithMany(p => p.MenualberoIdFiglioNavigation)
                    .HasForeignKey(d => d.IdFiglio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("menualbero_FK2");

                entity.HasOne(d => d.IdPadreNavigation)
                    .WithMany(p => p.MenualberoIdPadreNavigation)
                    .HasForeignKey(d => d.IdPadre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("menualbero_FK1");
            });

            modelBuilder.Entity<Menugruppi>(entity =>
            {
                entity.HasKey(e => new { e.Gruppo, e.IdVoce })
                    .HasName("PRIMARY");

                entity.ToTable("menugruppi");

                entity.HasIndex(e => e.Gruppo)
                    .HasName("menugruppi_Fk1");

                entity.HasIndex(e => e.IdVoce)
                    .HasName("menugruppi_FK3");

                entity.Property(e => e.Gruppo)
                    .HasColumnName("gruppo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdVoce)
                    .HasColumnName("idVoce")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ordinamento)
                    .HasColumnName("ordinamento")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.GruppoNavigation)
                    .WithMany(p => p.Menugruppi)
                    .HasForeignKey(d => d.Gruppo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("menugruppi_Fk1");

                entity.HasOne(d => d.IdVoceNavigation)
                    .WithMany(p => p.Menugruppi)
                    .HasForeignKey(d => d.IdVoce)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("menugruppi_FK3");
            });

            modelBuilder.Entity<Menuvoci>(entity =>
            {
                entity.ToTable("menuvoci");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descrizione)
                    .IsRequired()
                    .HasColumnName("descrizione");

                entity.Property(e => e.Titolo)
                    .IsRequired()
                    .HasColumnName("titolo")
                    .HasMaxLength(255);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Modelparameters>(entity =>
            {
                entity.HasKey(e => new { e.ProcessId, e.ProcessRev, e.VarianteId, e.ParamId })
                    .HasName("PRIMARY");

                entity.ToTable("modelparameters");

                entity.HasIndex(e => e.ParamCategory)
                    .HasName("paramCategory");

                entity.Property(e => e.ProcessId)
                    .HasColumnName("processID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProcessRev)
                    .HasColumnName("processRev")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VarianteId)
                    .HasColumnName("varianteID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParamId)
                    .HasColumnName("paramID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsFixed)
                    .HasColumnName("isFixed")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IsRequired)
                    .HasColumnName("isRequired")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.ParamCategory)
                    .HasColumnName("paramCategory")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParamDescription).HasColumnName("paramDescription");

                entity.Property(e => e.ParamName)
                    .HasColumnName("paramName")
                    .HasMaxLength(255);

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ParamCategoryNavigation)
                    .WithMany(p => p.Modelparameters)
                    .HasForeignKey(d => d.ParamCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("modelparameters_ibfk_2");
            });

            modelBuilder.Entity<Modeltaskparameters>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.TaskRev, e.VarianteId, e.ParamId })
                    .HasName("PRIMARY");

                entity.ToTable("modeltaskparameters");

                entity.HasIndex(e => e.ParamCategory)
                    .HasName("paramCategory");

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TaskRev)
                    .HasColumnName("taskRev")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VarianteId)
                    .HasColumnName("varianteID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParamId)
                    .HasColumnName("paramID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsFixed)
                    .HasColumnName("isFixed")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IsRequired)
                    .HasColumnName("isRequired")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.ParamCategory)
                    .HasColumnName("paramCategory")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParamDescription).HasColumnName("paramDescription");

                entity.Property(e => e.ParamName)
                    .HasColumnName("paramName")
                    .HasMaxLength(255);

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ParamCategoryNavigation)
                    .WithMany(p => p.Modeltaskparameters)
                    .HasForeignKey(d => d.ParamCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("modeltaskparameters_ibfk_2");
            });

            modelBuilder.Entity<Noncompliances>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Year })
                    .HasName("PRIMARY");

                entity.ToTable("noncompliances");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Year).HasColumnType("year(4)");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("{O = aperta, C = chiusa}");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("Utente che ha rilevato la non conformita' (se Warning = utente che ha sparato, altrimenti utente che apre la nc)");
            });

            modelBuilder.Entity<NoncompliancesProducts>(entity =>
            {
                entity.HasKey(e => new { e.NonComplianceId, e.NonComplianceYear, e.ProductId, e.ProductYear })
                    .HasName("PRIMARY");

                entity.ToTable("noncompliances_products");

                entity.HasIndex(e => e.WarningId)
                    .HasName("NC_Products_Warning_FK");

                entity.HasIndex(e => e.Workstation)
                    .HasName("NC_Products_Postazione_FK");

                entity.HasIndex(e => new { e.ProductId, e.ProductYear })
                    .HasName("NC_Products_Prod_FK");

                entity.Property(e => e.NonComplianceId)
                    .HasColumnName("NonComplianceID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NonComplianceYear).HasColumnType("year(4)");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductYear).HasColumnType("year(4)");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.WarningId)
                    .HasColumnName("WarningID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Workstation)
                    .HasColumnType("int(11)")
                    .HasComment(@"Se Source = Warning, e' la ostazione in cui e' stata rilevata la non conformita'.
-1 altrimenti");

                entity.HasOne(d => d.Warning)
                    .WithMany(p => p.NoncompliancesProducts)
                    .HasForeignKey(d => d.WarningId)
                    .HasConstraintName("NC_Products_Warning_FK");

                entity.HasOne(d => d.WorkstationNavigation)
                    .WithMany(p => p.NoncompliancesProducts)
                    .HasForeignKey(d => d.Workstation)
                    .HasConstraintName("NC_Products_Postazione_FK");

                entity.HasOne(d => d.NonCompliance)
                    .WithMany(p => p.NoncompliancesProducts)
                    .HasForeignKey(d => new { d.NonComplianceId, d.NonComplianceYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NC_Products_NC_FK");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.NoncompliancesProducts)
                    .HasForeignKey(d => new { d.ProductId, d.ProductYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NC_Products_Prod_FK");
            });

            modelBuilder.Entity<Noncompliancescause>(entity =>
            {
                entity.ToTable("noncompliancescause");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NoncompliancescauseNc>(entity =>
            {
                entity.HasKey(e => new { e.CauseId, e.Ncid, e.Ncyear })
                    .HasName("PRIMARY");

                entity.ToTable("noncompliancescause_nc");

                entity.HasIndex(e => new { e.Ncid, e.Ncyear })
                    .HasName("NonCompliances_Cause_FK_idx");

                entity.Property(e => e.CauseId)
                    .HasColumnName("CauseID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ncid)
                    .HasColumnName("NCID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ncyear)
                    .HasColumnName("NCYear")
                    .HasColumnType("year(4)");

                entity.HasOne(d => d.Cause)
                    .WithMany(p => p.NoncompliancescauseNc)
                    .HasForeignKey(d => d.CauseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NonCompliances_Cause1_FK");

                entity.HasOne(d => d.Nc)
                    .WithMany(p => p.NoncompliancescauseNc)
                    .HasForeignKey(d => new { d.Ncid, d.Ncyear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NonCompliances_Cause_FK");
            });

            modelBuilder.Entity<NoncompliancestypeNc>(entity =>
            {
                entity.HasKey(e => new { e.TypeId, e.Ncid, e.Ncyear })
                    .HasName("PRIMARY");

                entity.ToTable("noncompliancestype_nc");

                entity.HasIndex(e => new { e.Ncid, e.Ncyear })
                    .HasName("NonCompliance_FK_idx");

                entity.Property(e => e.TypeId)
                    .HasColumnName("TypeID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ncid)
                    .HasColumnName("NCID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ncyear)
                    .HasColumnName("NCYear")
                    .HasColumnType("year(4)");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.NoncompliancestypeNc)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Type_FK");

                entity.HasOne(d => d.Nc)
                    .WithMany(p => p.NoncompliancestypeNc)
                    .HasForeignKey(d => new { d.Ncid, d.Ncyear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NonCompliance_FK");
            });

            modelBuilder.Entity<Noncompliancestypes>(entity =>
            {
                entity.ToTable("noncompliancestypes");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Operatorireparto>(entity =>
            {
                entity.HasKey(e => new { e.Operatore, e.Reparto })
                    .HasName("PRIMARY");

                entity.ToTable("operatorireparto");

                entity.HasIndex(e => e.Operatore)
                    .HasName("FK_operatore");

                entity.HasIndex(e => e.Reparto)
                    .HasName("FK_reparto");

                entity.Property(e => e.Operatore)
                    .HasColumnName("operatore")
                    .HasMaxLength(255);

                entity.Property(e => e.Reparto)
                    .HasColumnName("reparto")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.OperatoreNavigation)
                    .WithMany(p => p.Operatorireparto)
                    .HasForeignKey(d => d.Operatore)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_operatore");

                entity.HasOne(d => d.RepartoNavigation)
                    .WithMany(p => p.Operatorireparto)
                    .HasForeignKey(d => d.Reparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reparto");
            });

            modelBuilder.Entity<Orarilavoroturni>(entity =>
            {
                entity.ToTable("orarilavoroturni");

                entity.HasIndex(e => e.IdTurno)
                    .HasName("orariLavoroTurni_FK1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GiornoFine)
                    .HasColumnName("giornoFine")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GiornoInizio)
                    .HasColumnName("giornoInizio")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdTurno)
                    .HasColumnName("idTurno")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OraFine).HasColumnName("oraFine");

                entity.Property(e => e.OraInizio).HasColumnName("oraInizio");

                entity.HasOne(d => d.IdTurnoNavigation)
                    .WithMany(p => p.Orarilavoroturni)
                    .HasForeignKey(d => d.IdTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orariLavoroTurni_FK1");
            });

            modelBuilder.Entity<Permessi>(entity =>
            {
                entity.HasKey(e => e.Idpermesso)
                    .HasName("PRIMARY");

                entity.ToTable("permessi");

                entity.Property(e => e.Idpermesso)
                    .HasColumnName("idpermesso")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descrizione).HasColumnName("descrizione");

                entity.Property(e => e.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Postazioni>(entity =>
            {
                entity.HasKey(e => e.Idpostazioni)
                    .HasName("PRIMARY");

                entity.ToTable("postazioni");

                entity.Property(e => e.Idpostazioni)
                    .HasColumnName("idpostazioni")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BarcodeAutoCheckIn)
                    .IsRequired()
                    .HasColumnName("barcodeAutoCheckIn")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Precedenzeprocessi>(entity =>
            {
                entity.HasKey(e => new { e.Prec, e.RevPrec, e.Succ, e.RevSucc, e.Variante })
                    .HasName("PRIMARY");

                entity.ToTable("precedenzeprocessi");

                entity.HasIndex(e => e.Relazione)
                    .HasName("precedenzeprocessi_ibfk_3");

                entity.HasIndex(e => e.Variante)
                    .HasName("precedenzeprocessi_variante");

                entity.HasIndex(e => new { e.Succ, e.RevSucc })
                    .HasName("precedenzeprocessi_ibfk_2");

                entity.Property(e => e.Prec)
                    .HasColumnName("prec")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevPrec)
                    .HasColumnName("revPrec")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Succ)
                    .HasColumnName("succ")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevSucc)
                    .HasColumnName("revSucc")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ConstraintType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Pausa)
                    .HasColumnName("pausa")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Relazione)
                    .HasColumnName("relazione")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.RelazioneNavigation)
                    .WithMany(p => p.Precedenzeprocessi)
                    .HasForeignKey(d => d.Relazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("precedenzeprocessi_ibfk_3");

                entity.HasOne(d => d.VarianteNavigation)
                    .WithMany(p => p.Precedenzeprocessi)
                    .HasForeignKey(d => d.Variante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("precedenzeprocessi_ibfk_4");

                entity.HasOne(d => d.Processo)
                    .WithMany(p => p.PrecedenzeprocessiProcesso)
                    .HasForeignKey(d => new { d.Prec, d.RevPrec })
                    .HasConstraintName("precedenzeprocessi_ibfk_1");

                entity.HasOne(d => d.ProcessoNavigation)
                    .WithMany(p => p.PrecedenzeprocessiProcessoNavigation)
                    .HasForeignKey(d => new { d.Succ, d.RevSucc })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("precedenzeprocessi_ibfk_2");
            });

            modelBuilder.Entity<Prectasksproduzione>(entity =>
            {
                entity.HasKey(e => new { e.Prec, e.Succ })
                    .HasName("PRIMARY");

                entity.ToTable("prectasksproduzione");

                entity.HasIndex(e => e.Prec)
                    .HasName("prectasksproduzione_FK_prec");

                entity.HasIndex(e => e.Succ)
                    .HasName("prectasksproduzione_FK_succ");

                entity.Property(e => e.Prec)
                    .HasColumnName("prec")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Succ)
                    .HasColumnName("succ")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ConstraintType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Pausa)
                    .HasColumnName("pausa")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Relazione)
                    .HasColumnName("relazione")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.PrecNavigation)
                    .WithMany(p => p.PrectasksproduzionePrecNavigation)
                    .HasForeignKey(d => d.Prec)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("prectasksproduzione_FK_prec");

                entity.HasOne(d => d.SuccNavigation)
                    .WithMany(p => p.PrectasksproduzioneSuccNavigation)
                    .HasForeignKey(d => d.Succ)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("prectasksproduzione_FK_succ");
            });

            modelBuilder.Entity<Processipadrifigli>(entity =>
            {
                entity.HasKey(e => new { e.Task, e.Variante, e.RevPadre, e.Padre, e.RevTask })
                    .HasName("PRIMARY");

                entity.ToTable("processipadrifigli");

                entity.HasIndex(e => new { e.Task, e.RevTask })
                    .HasName("FK_Task");

                entity.HasIndex(e => new { e.Padre, e.RevPadre, e.Variante })
                    .HasName("FK_PadreVariante");

                entity.Property(e => e.Task)
                    .HasColumnName("task")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevPadre)
                    .HasColumnName("revPadre")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Padre)
                    .HasColumnName("padre")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevTask)
                    .HasColumnName("revTask")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Posx)
                    .HasColumnName("posx")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'100'");

                entity.Property(e => e.Posy)
                    .HasColumnName("posy")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'100'");
            });

            modelBuilder.Entity<Processo>(entity =>
            {
                entity.HasKey(e => new { e.ProcessId, e.Revisione })
                    .HasName("PRIMARY");

                entity.ToTable("processo");

                entity.Property(e => e.ProcessId)
                    .HasColumnName("ProcessID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Revisione)
                    .HasColumnName("revisione")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Attivo)
                    .HasColumnName("attivo")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IsVsm)
                    .HasColumnName("isVSM")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Posx)
                    .HasColumnName("posx")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Posy)
                    .HasColumnName("posy")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Processowners>(entity =>
            {
                entity.HasKey(e => new { e.User, e.Process, e.RevProc })
                    .HasName("PRIMARY");

                entity.ToTable("processowners");

                entity.HasIndex(e => new { e.Process, e.RevProc })
                    .HasName("process");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.Property(e => e.Process)
                    .HasColumnName("process")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevProc)
                    .HasColumnName("revProc")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Processowners)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("processowners_ibfk_1");

                entity.HasOne(d => d.Processo)
                    .WithMany(p => p.Processowners)
                    .HasForeignKey(d => new { d.Process, d.RevProc })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("processowners_ibfk_2");
            });

            modelBuilder.Entity<Productionplan>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Anno })
                    .HasName("PRIMARY");

                entity.ToTable("productionplan");

                entity.HasIndex(e => e.MeasurementUnit)
                    .HasName("measurementUnit_FK1_idx");

                entity.HasIndex(e => e.Planner)
                    .HasName("productionplan_FKplanner_idx");

                entity.HasIndex(e => e.Reparto)
                    .HasName("reparto_FK1");

                entity.HasIndex(e => new { e.Commessa, e.AnnoCommessa })
                    .HasName("commesse_FK1");

                entity.HasIndex(e => new { e.Processo, e.Revisione, e.Variante })
                    .HasName("processo_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Anno)
                    .HasColumnName("anno")
                    .HasColumnType("year(4)");

                entity.Property(e => e.AnnoCommessa)
                    .HasColumnName("annoCommessa")
                    .HasColumnType("year(4)");

                entity.Property(e => e.Commessa)
                    .HasColumnName("commessa")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Delay).HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.KanbanCard)
                    .HasColumnName("kanbanCard")
                    .HasMaxLength(255);

                entity.Property(e => e.LeadTime).HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Matricola)
                    .HasColumnName("matricola")
                    .HasMaxLength(255);

                entity.Property(e => e.MeasurementUnit)
                    .HasColumnName("measurementUnit")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Planner)
                    .HasColumnName("planner")
                    .HasMaxLength(255);

                entity.Property(e => e.Processo)
                    .HasColumnName("processo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantita)
                    .HasColumnName("quantita")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.QuantitaProdotta)
                    .HasColumnName("quantitaProdotta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Reparto)
                    .HasColumnName("reparto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Revisione)
                    .HasColumnName("revisione")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(1);

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WorkingTime).HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.WorkingTimePlanned).HasDefaultValueSql("'00:00:00'");

                entity.HasOne(d => d.MeasurementUnitNavigation)
                    .WithMany(p => p.Productionplan)
                    .HasForeignKey(d => d.MeasurementUnit)
                    .HasConstraintName("measurementUnit_FK2");

                entity.HasOne(d => d.PlannerNavigation)
                    .WithMany(p => p.Productionplan)
                    .HasForeignKey(d => d.Planner)
                    .HasConstraintName("productionplan_FKplanner");

                entity.HasOne(d => d.RepartoNavigation)
                    .WithMany(p => p.Productionplan)
                    .HasForeignKey(d => d.Reparto)
                    .HasConstraintName("reparto_FK1");

                entity.HasOne(d => d.Commesse)
                    .WithMany(p => p.Productionplan)
                    .HasForeignKey(d => new { d.Commessa, d.AnnoCommessa })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("commesse_FK1");
            });

            modelBuilder.Entity<Productparameters>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ProductYear, e.ParamId, e.ParamCategory })
                    .HasName("PRIMARY");

                entity.ToTable("productparameters");

                entity.HasIndex(e => e.ParamCategory)
                    .HasName("productparameters_category_idx");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductYear)
                    .HasColumnName("productYear")
                    .HasColumnType("year(4)");

                entity.Property(e => e.ParamId)
                    .HasColumnName("paramID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParamCategory)
                    .HasColumnName("paramCategory")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsFixed)
                    .HasColumnName("isFixed")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.ParamDescription).HasColumnName("paramDescription");

                entity.Property(e => e.ParamName)
                    .HasColumnName("paramName")
                    .HasMaxLength(255);

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ParamCategoryNavigation)
                    .WithMany(p => p.Productparameters)
                    .HasForeignKey(d => d.ParamCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productparameters_category");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Productparameters)
                    .HasForeignKey(d => new { d.ProductId, d.ProductYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productparameters_ibfk_1");
            });

            modelBuilder.Entity<Productparameterscategories>(entity =>
            {
                entity.HasKey(e => e.ParamCatId)
                    .HasName("PRIMARY");

                entity.ToTable("productparameterscategories");

                entity.Property(e => e.ParamCatId)
                    .HasColumnName("paramCatID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParamCatDescription).HasColumnName("paramCatDescription");

                entity.Property(e => e.ParamCatName)
                    .HasColumnName("paramCatName")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Registroeventiproduzione>(entity =>
            {
                entity.HasKey(e => new { e.TipoEvento, e.TaskId })
                    .HasName("PRIMARY");

                entity.ToTable("registroeventiproduzione");

                entity.HasIndex(e => e.TaskId)
                    .HasName("registroeventi_FK1");

                entity.Property(e => e.TipoEvento).HasMaxLength(255);

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Segnalato)
                    .IsRequired()
                    .HasColumnName("segnalato")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Registroeventiproduzione)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("registroeventi_FK1");
            });

            modelBuilder.Entity<Registroeventitaskproduzione>(entity =>
            {
                entity.ToTable("registroeventitaskproduzione");

                entity.HasIndex(e => e.Task)
                    .HasName("registroEventiTask_FK2");

                entity.HasIndex(e => e.User)
                    .HasName("registroEventiTask_FK1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Evento)
                    .IsRequired()
                    .HasColumnName("evento")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Task)
                    .HasColumnName("task")
                    .HasColumnType("int(11)");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.HasOne(d => d.TaskNavigation)
                    .WithMany(p => p.Registroeventitaskproduzione)
                    .HasForeignKey(d => d.Task)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("registroEventiTask_FK2");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Registroeventitaskproduzione)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("registroEventiTask_FK1");
            });

            modelBuilder.Entity<Relazioniprocessi>(entity =>
            {
                entity.HasKey(e => e.RelazioneId)
                    .HasName("PRIMARY");

                entity.ToTable("relazioniprocessi");

                entity.Property(e => e.RelazioneId)
                    .HasColumnName("RelazioneID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImgUrl)
                    .IsRequired()
                    .HasColumnName("imgUrl")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Reparti>(entity =>
            {
                entity.HasKey(e => e.Idreparto)
                    .HasName("PRIMARY");

                entity.ToTable("reparti");

                entity.Property(e => e.Idreparto)
                    .HasColumnName("idreparto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnticipoTasks)
                    .HasColumnName("anticipoTasks")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cadenza).HasColumnName("cadenza");

                entity.Property(e => e.Descrizione)
                    .HasColumnName("descrizione")
                    .HasMaxLength(255);

                entity.Property(e => e.ModoCalcoloTc)
                    .IsRequired()
                    .HasColumnName("ModoCalcoloTC")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255);

                entity.Property(e => e.SplitTasks)
                    .HasColumnName("splitTasks")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Timezone)
                    .HasColumnName("timezone")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Repartipostazioniattivita>(entity =>
            {
                entity.HasKey(e => new { e.Reparto, e.Postazione, e.Processo, e.RevProc, e.Variante })
                    .HasName("PRIMARY");

                entity.ToTable("repartipostazioniattivita");

                entity.HasIndex(e => e.Postazione)
                    .HasName("postazioniprocessi_FK_1");

                entity.HasIndex(e => e.Reparto)
                    .HasName("repartipostazioniattivita_FK_2");

                entity.HasIndex(e => new { e.Processo, e.RevProc })
                    .HasName("postazioniprocessi_FK_2");

                entity.HasIndex(e => new { e.Processo, e.RevProc, e.Variante })
                    .HasName("repartipostazioniattivita_FK_3");

                entity.Property(e => e.Reparto)
                    .HasColumnName("reparto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Postazione)
                    .HasColumnName("postazione")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processo)
                    .HasColumnName("processo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevProc)
                    .HasColumnName("revProc")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.PostazioneNavigation)
                    .WithMany(p => p.Repartipostazioniattivita)
                    .HasForeignKey(d => d.Postazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("postazioniprocessi_FK_1");

                entity.HasOne(d => d.RepartoNavigation)
                    .WithMany(p => p.Repartipostazioniattivita)
                    .HasForeignKey(d => d.Reparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("repartipostazioniattivita_FK_2");
            });

            modelBuilder.Entity<Repartiprocessi>(entity =>
            {
                entity.HasKey(e => new { e.IdReparto, e.Variante, e.Revisione, e.ProcessId })
                    .HasName("PRIMARY");

                entity.ToTable("repartiprocessi");

                entity.HasIndex(e => e.IdReparto)
                    .HasName("repartiprocessi_FK_2");

                entity.HasIndex(e => e.Variante)
                    .HasName("repartiprocessi_FK_3");

                entity.HasIndex(e => new { e.ProcessId, e.Revisione, e.Variante })
                    .HasName("repartiprocessi_FK_1");

                entity.Property(e => e.IdReparto)
                    .HasColumnName("idReparto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Revisione)
                    .HasColumnName("revisione")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProcessId)
                    .HasColumnName("processID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdRepartoNavigation)
                    .WithMany(p => p.Repartiprocessi)
                    .HasForeignKey(d => d.IdReparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("repartiprocessi_FK_2");
            });

            modelBuilder.Entity<Risorseturnopostazione>(entity =>
            {
                entity.HasKey(e => new { e.Idturno, e.Idpostazione })
                    .HasName("PRIMARY");

                entity.ToTable("risorseturnopostazione");

                entity.HasIndex(e => e.Idpostazione)
                    .HasName("FK_postazione_res_idx");

                entity.HasIndex(e => e.Idturno)
                    .HasName("FK_Turno_res_idx");

                entity.Property(e => e.Idturno)
                    .HasColumnName("idturno")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idpostazione)
                    .HasColumnName("idpostazione")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Risorse)
                    .HasColumnName("risorse")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdpostazioneNavigation)
                    .WithMany(p => p.Risorseturnopostazione)
                    .HasForeignKey(d => d.Idpostazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_postazione_res");

                entity.HasOne(d => d.IdturnoNavigation)
                    .WithMany(p => p.Risorseturnopostazione)
                    .HasForeignKey(d => d.Idturno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Turno_res");
            });

            modelBuilder.Entity<Straordinarifestivita>(entity =>
            {
                entity.ToTable("straordinarifestivita");

                entity.HasIndex(e => e.Turno)
                    .HasName("reparto_FK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Azione)
                    .IsRequired()
                    .HasColumnName("azione")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("azione: + è straordinario, - è festività");

                entity.Property(e => e.Turno)
                    .HasColumnName("turno")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.TurnoNavigation)
                    .WithMany(p => p.Straordinarifestivita)
                    .HasForeignKey(d => d.Turno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("turno_straord_fest_FK");
            });

            modelBuilder.Entity<Syslog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("syslog");

                entity.Property(e => e.Itemid)
                    .IsRequired()
                    .HasColumnName("itemid")
                    .HasMaxLength(45);

                entity.Property(e => e.Itemtype)
                    .IsRequired()
                    .HasColumnName("itemtype")
                    .HasMaxLength(255);

                entity.Property(e => e.Module)
                    .IsRequired()
                    .HasColumnName("module")
                    .HasMaxLength(255);

                entity.Property(e => e.Newvalue)
                    .IsRequired()
                    .HasColumnName("newvalue")
                    .HasMaxLength(255);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasMaxLength(255);

                entity.Property(e => e.Oldvalue)
                    .IsRequired()
                    .HasColumnName("oldvalue")
                    .HasMaxLength(255);

                entity.Property(e => e.Parameter)
                    .IsRequired()
                    .HasColumnName("parameter")
                    .HasMaxLength(255);

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Taskparameters>(entity =>
            {
                entity.HasKey(e => new { e.ParamId, e.ParamCategory, e.TaskId })
                    .HasName("PRIMARY");

                entity.ToTable("taskparameters");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("taskParam_user_idx");

                entity.HasIndex(e => e.ParamCategory)
                    .HasName("Taskparameters_category");

                entity.Property(e => e.ParamId)
                    .HasColumnName("paramID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParamCategory)
                    .HasColumnName("paramCategory")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy).HasMaxLength(255);

                entity.Property(e => e.IsFixed)
                    .HasColumnName("isFixed")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IsRequired)
                    .HasColumnName("isRequired")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.ParamDescription)
                    .HasColumnName("paramDescription")
                    .HasMaxLength(255);

                entity.Property(e => e.ParamName)
                    .HasColumnName("paramName")
                    .HasMaxLength(255);

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Taskparameters)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("Taskparameters_user");

                entity.HasOne(d => d.ParamCategoryNavigation)
                    .WithMany(p => p.Taskparameters)
                    .HasForeignKey(d => d.ParamCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Taskparameters_category");
            });

            modelBuilder.Entity<Tasksmanuals>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.TaskRev, e.TaskVarianti, e.ManualId, e.ManualVersion, e.Sequence })
                    .HasName("PRIMARY");

                entity.ToTable("tasksmanuals");

                entity.HasIndex(e => e.TaskVarianti)
                    .HasName("Variant_FK1_idx");

                entity.HasIndex(e => new { e.ManualId, e.ManualVersion })
                    .HasName("Manual_FK1_idx");

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TaskRev)
                    .HasColumnName("taskRev")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TaskVarianti)
                    .HasColumnName("taskVarianti")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ManualId)
                    .HasColumnName("manualID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ManualVersion)
                    .HasColumnName("manualVersion")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.TaskVariantiNavigation)
                    .WithMany(p => p.Tasksmanuals)
                    .HasForeignKey(d => d.TaskVarianti)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Variant_FK1");

                entity.HasOne(d => d.Manual)
                    .WithMany(p => p.Tasksmanuals)
                    .HasForeignKey(d => new { d.ManualId, d.ManualVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Manual_FK1");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Tasksmanuals)
                    .HasForeignKey(d => new { d.TaskId, d.TaskRev })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Task_FK1");
            });

            modelBuilder.Entity<Tasksproduzione>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PRIMARY");

                entity.ToTable("tasksproduzione");

                entity.HasIndex(e => e.Postazione)
                    .HasName("tasksproduzione_FK_2");

                entity.HasIndex(e => e.Reparto)
                    .HasName("tasksproduzione_FK_4");

                entity.HasIndex(e => new { e.OrigTask, e.RevOrigTask })
                    .HasName("tasksproduzione_FK_proc");

                entity.HasIndex(e => new { e.Idcommessa, e.Annocommessa, e.IdArticolo, e.AnnoArticolo })
                    .HasName("FK_articoliCommesse");

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnnoArticolo)
                    .HasColumnName("annoArticolo")
                    .HasColumnType("year(4)");

                entity.Property(e => e.Annocommessa)
                    .HasColumnName("annocommessa")
                    .HasColumnType("year(4)");

                entity.Property(e => e.Delay).HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.IdArticolo)
                    .HasColumnName("idArticolo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idcommessa)
                    .HasColumnName("idcommessa")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LeadTime).HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.NOperatori)
                    .HasColumnName("nOperatori")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.OrigTask)
                    .HasColumnName("origTask")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Postazione)
                    .HasColumnName("postazione")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QtaPrevista)
                    .HasColumnName("qtaPrevista")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QtaProdotta)
                    .HasColumnName("qtaProdotta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Reparto)
                    .HasColumnName("reparto")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevOrigTask)
                    .HasColumnName("revOrigTask")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(1);

                entity.Property(e => e.TempoCiclo).HasColumnName("tempoCiclo");

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WorkingTime).HasDefaultValueSql("'00:00:00'");

                entity.HasOne(d => d.PostazioneNavigation)
                    .WithMany(p => p.Tasksproduzione)
                    .HasForeignKey(d => d.Postazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tasksproduzione_FK_2");

                entity.HasOne(d => d.RepartoNavigation)
                    .WithMany(p => p.Tasksproduzione)
                    .HasForeignKey(d => d.Reparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tasksproduzione_FK_4");

                entity.HasOne(d => d.Processo)
                    .WithMany(p => p.Tasksproduzione)
                    .HasForeignKey(d => new { d.OrigTask, d.RevOrigTask })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tasksproduzione_FK_proc");
            });

            modelBuilder.Entity<Tasksproduzioneoperatornotes>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.CommentId })
                    .HasName("PRIMARY");

                entity.ToTable("tasksproduzioneoperatornotes");

                entity.HasIndex(e => e.User)
                    .HasName("taskNotes_users_FK2_idx");

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommentId)
                    .HasColumnName("commentID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasColumnName("notes");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Tasksproduzioneoperatornotes)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("taskNotes_taskID_FK1");
            });

            modelBuilder.Entity<Taskstimespans>(entity =>
            {
                entity.ToTable("taskstimespans");

                entity.HasIndex(e => e.Endeventid)
                    .HasName("timespans_fkevent2_idx");

                entity.HasIndex(e => e.Starteventid)
                    .HasName("timespans_fkevent1_idx");

                entity.HasIndex(e => e.Taskid)
                    .HasName("timespans_fktask_idx");

                entity.HasIndex(e => e.Userid)
                    .HasName("user_timespans_fk1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DurationSec).HasColumnName("duration_sec");

                entity.Property(e => e.Endeventid)
                    .HasColumnName("endeventid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Endeventtype)
                    .IsRequired()
                    .HasColumnName("endeventtype")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Starteventid)
                    .HasColumnName("starteventid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Starteventtype)
                    .IsRequired()
                    .HasColumnName("starteventtype")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Taskid)
                    .HasColumnName("taskid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasColumnName("userid")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Endevent)
                    .WithMany(p => p.TaskstimespansEndevent)
                    .HasForeignKey(d => d.Endeventid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("timespans_fkevent2");

                entity.HasOne(d => d.Startevent)
                    .WithMany(p => p.TaskstimespansStartevent)
                    .HasForeignKey(d => d.Starteventid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("timespans_fkevent1");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Taskstimespans)
                    .HasForeignKey(d => d.Taskid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("timespans_fktask");
            });

            modelBuilder.Entity<Taskuser>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.User })
                    .HasName("PRIMARY");

                entity.ToTable("taskuser");

                entity.HasIndex(e => e.User)
                    .HasName("usertask_FK");

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.Property(e => e.Exclusive)
                    .HasColumnName("exclusive")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Taskuser)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("taskprodid");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Taskuser)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usertask_FK");
            });

            modelBuilder.Entity<Taskusermodel>(entity =>
            {
                entity.HasKey(e => new { e.Taskid, e.Taskrev, e.Variantid, e.User })
                    .HasName("PRIMARY");

                entity.ToTable("taskusermodel");

                entity.HasIndex(e => e.User)
                    .HasName("userfk_idx");

                entity.Property(e => e.Taskid)
                    .HasColumnName("taskid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Taskrev)
                    .HasColumnName("taskrev")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Variantid)
                    .HasColumnName("variantid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.Property(e => e.Exclusive)
                    .HasColumnName("exclusive")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Taskusermodel)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userTaskModel_FK");
            });

            modelBuilder.Entity<Tempiciclo>(entity =>
            {
                entity.HasKey(e => new { e.Processo, e.Revisione, e.Variante, e.NumOp })
                    .HasName("PRIMARY");

                entity.ToTable("tempiciclo");

                entity.HasIndex(e => new { e.Processo, e.Revisione, e.Variante })
                    .HasName("FK_processovariante");

                entity.Property(e => e.Processo)
                    .HasColumnName("processo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Revisione)
                    .HasColumnName("revisione")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NumOp)
                    .HasColumnName("num_op")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Def)
                    .IsRequired()
                    .HasColumnName("def")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Setup)
                    .HasColumnName("setup")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Tempo)
                    .HasColumnName("tempo")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Tunload)
                    .HasColumnName("tunload")
                    .HasDefaultValueSql("'00:00:00'");
            });

            modelBuilder.Entity<Tipipermessi>(entity =>
            {
                entity.ToTable("tipipermessi");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(1);

                entity.Property(e => e.Descrizione).HasColumnName("descrizione");
            });

            modelBuilder.Entity<Turniproduzione>(entity =>
            {
                entity.ToTable("turniproduzione");

                entity.HasIndex(e => e.Reparto)
                    .HasName("turniproduzione_FK_1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Colore)
                    .HasColumnName("colore")
                    .HasMaxLength(7)
                    .HasDefaultValueSql("'#000000'");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Reparto)
                    .HasColumnName("reparto")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.RepartoNavigation)
                    .WithMany(p => p.Turniproduzione)
                    .HasForeignKey(d => d.Reparto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("turniproduzione_FK_1");
            });

            modelBuilder.Entity<Useremail>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Email })
                    .HasName("PRIMARY");

                entity.ToTable("useremail");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_FK1");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.ForAlarm)
                    .HasColumnName("forAlarm")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Useremail)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_FK1");
            });

            modelBuilder.Entity<Userphonenumbers>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PhoneNumber })
                    .HasName("PRIMARY");

                entity.ToTable("userphonenumbers");

                entity.HasIndex(e => e.UserId)
                    .HasName("userphone_FK1");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(255);

                entity.Property(e => e.ForAlarm)
                    .HasColumnName("forAlarm")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userphonenumbers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userphone_FK1");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasIndex(e => e.Id)
                    .HasName("ID_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(255);

                entity.Property(e => e.Checksum)
                    .HasColumnName("checksum")
                    .HasMaxLength(45);

                entity.Property(e => e.Cognome)
                    .IsRequired()
                    .HasColumnName("cognome");

                entity.Property(e => e.DestinationUrl)
                    .HasColumnName("destinationURL")
                    .HasMaxLength(255);

                entity.Property(e => e.Enabled)
                    .HasColumnName("enabled")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasColumnName("language")
                    .HasMaxLength(5)
                    .HasDefaultValueSql("'en'");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(45);

                entity.Property(e => e.TipoUtente)
                    .IsRequired()
                    .HasColumnName("tipoUtente")
                    .HasMaxLength(255);

                entity.Property(e => e.Verified)
                    .HasColumnName("verified")
                    .HasColumnType("bit(1)");
            });

            modelBuilder.Entity<Varianti>(entity =>
            {
                entity.HasKey(e => e.Idvariante)
                    .HasName("PRIMARY");

                entity.ToTable("varianti");

                entity.Property(e => e.Idvariante)
                    .HasColumnName("idvariante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DescVariante).HasColumnName("descVariante");

                entity.Property(e => e.NomeVariante)
                    .IsRequired()
                    .HasColumnName("nomeVariante")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Variantiprocessi>(entity =>
            {
                entity.HasKey(e => new { e.Variante, e.Processo, e.RevProc })
                    .HasName("PRIMARY");

                entity.ToTable("variantiprocessi");

                entity.HasIndex(e => e.ExternalId)
                    .HasName("ExternalID_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MeasurementUnit)
                    .HasName("measurementunit_FK1_idx");

                entity.HasIndex(e => e.Processo)
                    .HasName("processo_FK");

                entity.HasIndex(e => e.Variante)
                    .HasName("variante_FK");

                entity.HasIndex(e => new { e.Processo, e.RevProc })
                    .HasName("variantiprocessi_processo_FK");

                entity.Property(e => e.Variante)
                    .HasColumnName("variante")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Processo)
                    .HasColumnName("processo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RevProc)
                    .HasColumnName("revProc")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExternalId)
                    .HasColumnName("ExternalID")
                    .HasMaxLength(255);

                entity.Property(e => e.MeasurementUnit)
                    .HasColumnName("measurementUnit")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.MeasurementUnitNavigation)
                    .WithMany(p => p.Variantiprocessi)
                    .HasForeignKey(d => d.MeasurementUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("measurementunit_FK1");

                entity.HasOne(d => d.VarianteNavigation)
                    .WithMany(p => p.Variantiprocessi)
                    .HasForeignKey(d => d.Variante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("variante_FK");

                entity.HasOne(d => d.ProcessoNavigation)
                    .WithMany(p => p.Variantiprocessi)
                    .HasForeignKey(d => new { d.Processo, d.RevProc })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("variantiprocessi_processo_FK");
            });

            modelBuilder.Entity<Warningproduzione>(entity =>
            {
                entity.ToTable("warningproduzione");

                entity.HasIndex(e => e.Task)
                    .HasName("warning_FK1");

                entity.HasIndex(e => e.User)
                    .HasName("warning_FK2");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Motivo).HasColumnName("motivo");

                entity.Property(e => e.Risoluzione).HasColumnName("risoluzione");

                entity.Property(e => e.Task)
                    .HasColumnName("task")
                    .HasColumnType("int(11)");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasMaxLength(255);

                entity.HasOne(d => d.TaskNavigation)
                    .WithMany(p => p.Warningproduzione)
                    .HasForeignKey(d => d.Task)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("warning_FK1");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Warningproduzione)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("warning_FK2");
            });

            modelBuilder.Entity<Workinstructionslabel>(entity =>
            {
                entity.HasKey(e => e.WilabelId)
                    .HasName("PRIMARY");

                entity.ToTable("workinstructionslabel");

                entity.HasIndex(e => e.WilabelName)
                    .HasName("wilabelName_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.WilabelId)
                    .HasColumnName("wilabelID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WilabelName)
                    .IsRequired()
                    .HasColumnName("wilabelName")
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        */
        /*
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);*/
    }
}
