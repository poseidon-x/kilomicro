using coreSmsNotificationData.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coreSmsNotificationData.Abstractions
{
    public interface ISmsDataRepository
    {
        Task<List<messageEvent>> GetAllQueuedUnsentSmsEvents();
        string GetDisbursementSmsTemplate();
        messagingConfig GetMessageConfig(int configId);
        bool MarkSmsEventAsFinished(int messageEventId);
        bool QueueSmsEvent(messageEvent input);
    }
}
