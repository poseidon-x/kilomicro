namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("noti.notificationType")]
    public partial class notificationType
    {
        public byte notificationTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string notificationTypeName { get; set; }
    }
}
