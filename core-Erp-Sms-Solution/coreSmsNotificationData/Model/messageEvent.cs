using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace coreSmsNotificationData.Model
{
    public class messageEvent
    {

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

    }
}
