namespace coreNotificationsDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("noti.notificationPrivilege")]
    public partial class notificationPrivilege
    {
        public int notificationPrivilegeID { get; set; }

        public byte notificationTypeID { get; set; }

        public bool allowAll { get; set; }

        [Required]
        [StringLength(30)]
        public string userName { get; set; }

        [Required]
        [StringLength(30)]
        public string roleName { get; set; }
    }
}
