using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace coreSmsNotificationData.Model
{
    public class messageEventCategory
    {
        public int messageEventCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string messageEventCategoryName { get; set; }

        public bool isEnabled { get; set; }

    }
}
