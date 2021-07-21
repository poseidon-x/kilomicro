using coreSmsNotificationData.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coreSmsNotificationData.Abstractions
{
    public interface ISmsHttpHelper
    {
        Task<SmsSentResult> SendSmsByGetMethodAsync(string msisdn, string messageBody, string sender, int messageConfigId);
    }
}
