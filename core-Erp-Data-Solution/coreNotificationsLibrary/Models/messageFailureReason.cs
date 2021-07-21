namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("msg.messageFailureReason")]
    public partial class messageFailureReason
    {
        public messageFailureReason()
        {
            messagesFaileds = new HashSet<messagesFailed>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int messageFailureReasonID { get; set; }

        [StringLength(400)]
        public string messageFailureReasonName { get; set; }

        public virtual ICollection<messagesFailed> messagesFaileds { get; set; }
    }
}
