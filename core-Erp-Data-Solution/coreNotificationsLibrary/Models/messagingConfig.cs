namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("msg.messagingConfig")]
    public partial class messagingConfig
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int messagingConfigID { get; set; }

        [Required]
        [StringLength(400)]
        public string httpMessagingUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string httpMessagingUserName { get; set; }

        [Required]
        [StringLength(30)]
        public string httpMessagingPassword { get; set; }

        [Required]
        [StringLength(10)]
        public string messagingSender { get; set; }

        public short maxMessageLength { get; set; }

        public byte maxNarationLength { get; set; }

        public short loanRepaymentNotificationCycle { get; set; }

        [Required]
        public Int16 numberOfDaysBeforeLoanOverdue { get; set; }
    }
}
