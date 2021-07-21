using coreNotificationsDAL;
using coreNotificationsDAL.Abstractions;
using coreNotificationsDAL.Helpers;

namespace coreNotificationConsoleApp.Helpers
{
    public class SmsQueueHelper
    {
        private ISmsDataHelper smsHelper;
        public SmsQueueHelper()
        {
            smsHelper = new SmsDataHelper();
        }

        public bool QueueSms(messageEvent input)
        {
            var body = smsHelper.GetDisbursementSmsTemplate();
            input.messageBody = body;
            var isQueued = smsHelper.QueueSmsEvent(input);
            return isQueued;
        }
    }
}
