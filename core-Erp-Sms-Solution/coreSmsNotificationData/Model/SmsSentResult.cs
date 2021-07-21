using System;
using System.Collections.Generic;
using System.Text;

namespace coreSmsNotificationData.Model
{
    public class SmsSentResult
    {
        public int StatusCode { get; set; }
        public bool IsSent { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

    }
}
