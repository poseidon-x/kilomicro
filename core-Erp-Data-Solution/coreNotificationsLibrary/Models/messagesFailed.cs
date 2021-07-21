namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("msg.messagesFailed")]
    public partial class messagesFailed
    {
        public int messagesFailedID { get; set; }

        public int messageEventID { get; set; }

        public DateTime attemptDate { get; set; }

        public int messagesFailureReasonID { get; set; }

        public virtual messageEvent messageEvent { get; set; }

        public virtual messageFailureReason messageFailureReason { get; set; }
    }
}
