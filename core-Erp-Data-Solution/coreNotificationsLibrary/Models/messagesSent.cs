namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("msg.messagesSent")]
    public partial class messagesSent
    {
        public int messagesSentID { get; set; }

        public int messageEventID { get; set; }

        public DateTime sentDate { get; set; }

        public virtual messageEvent messageEvent { get; set; }
    }
}
