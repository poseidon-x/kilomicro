namespace coreNotificationsDAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class notificationsModel : DbContext
    {
        public notificationsModel()
            : base("name=notificationsModelConnectionString")
        {
        }

        public virtual DbSet<messageEvent> messageEvents { get; set; }
        public virtual DbSet<messageEventCategory> messageEventCategories { get; set; }
        public virtual DbSet<messageFailureReason> messageFailureReasons { get; set; }
        public virtual DbSet<messagesFailed> messagesFaileds { get; set; }
        public virtual DbSet<messagesSent> messagesSents { get; set; }
        public virtual DbSet<messageTemplate> messageTemplates { get; set; }
        public virtual DbSet<messagingConfig> messagingConfigs { get; set; }
        public virtual DbSet<notificationPrivilege> notificationPrivileges { get; set; }
        public virtual DbSet<notificationType> notificationTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<messageEvent>()
                .HasMany(e => e.messagesFaileds)
                .WithRequired(e => e.messageEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<messageEvent>()
                .HasMany(e => e.messagesSents)
                .WithRequired(e => e.messageEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<messageEventCategory>()
                .HasMany(e => e.messageEvents)
                .WithRequired(e => e.messageEventCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<messageEventCategory>()
                .HasMany(e => e.messageTemplates)
                .WithRequired(e => e.messageEventCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<messageFailureReason>()
                .HasMany(e => e.messagesFaileds)
                .WithRequired(e => e.messageFailureReason)
                .HasForeignKey(e => e.messagesFailureReasonID)
                .WillCascadeOnDelete(false);
        }
    }
}
