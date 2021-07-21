using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace coreSmsNotificationData.Model
{
    public class messageTemplate
    {
        public int messageTemplateID { get; set; }

        [Required]
        [StringLength(400)]
        public string messageBodyTemplate { get; set; }

        public int messageEventCategoryID { get; set; }

    }
}
