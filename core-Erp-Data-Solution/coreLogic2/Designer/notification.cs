//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic.Designer
{
    using System;
    using System.Collections.Generic;
    
    public partial class notification
    {
        public notification()
        {
            this.notificationRecipients = new HashSet<notificationRecipient>();
            this.notificationSchedules = new HashSet<notificationSchedule>();
        }
    
        public int notificationID { get; set; }
        public string notificationCode { get; set; }
        public string notificationName { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
    
        public virtual ICollection<notificationRecipient> notificationRecipients { get; set; }
        public virtual ICollection<notificationSchedule> notificationSchedules { get; set; }
    }
}