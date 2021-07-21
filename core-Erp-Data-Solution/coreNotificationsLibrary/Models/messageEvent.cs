namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("msg.messageEvent")]
    public partial class messageEvent
    {
        public messageEvent()
        {
            messagesFaileds = new HashSet<messagesFailed>();
            messagesSents = new HashSet<messagesSent>();
        }

        public int messageEventID { get; set; }

        public int messageEventCategoryID { get; set; }

        public int clientID { get; set; }

        public int accountID { get; set; }

        public int eventID { get; set; }

        [Required]
        [StringLength(30)]
        public string phoneNumber { get; set; }

        [Required]
        [StringLength(400)]
        public string messageBody { get; set; }

        [Required]
        [StringLength(10)]
        public string sender { get; set; }

        public DateTime eventDate { get; set; }

        public bool finished { get; set; }

        public virtual ICollection<messagesFailed> messagesFaileds { get; set; }

        public virtual ICollection<messagesSent> messagesSents { get; set; }

        public virtual messageEventCategory messageEventCategory { get; set; }
    }
}
