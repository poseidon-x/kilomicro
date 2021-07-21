using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsDAL.Abstractions
{
    public interface ISmsDataHelper
    {
        bool QueueSmsEvent(messageEvent input);
        string GetDisbursementSmsTemplate();
    }
}
