namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("msg.messageTemplate")]
    public partial class messageTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int messageTemplateID { get; set; }

        [Required]
        [StringLength(400)]
        public string messageBodyTemplate { get; set; }

        public int messageEventCategoryID { get; set; }

        public virtual messageEventCategory messageEventCategory { get; set; }
    }
}
