namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("msg.messageEventCategory")]
    public partial class messageEventCategory
    {
        public messageEventCategory()
        {
            messageEvents = new HashSet<messageEvent>();
            messageTemplates = new HashSet<messageTemplate>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int messageEventCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string messageEventCategoryName { get; set; }

        public bool isEnabled { get; set; }

        public virtual ICollection<messageEvent> messageEvents { get; set; }

        public virtual ICollection<messageTemplate> messageTemplates { get; set; }
    }
}
