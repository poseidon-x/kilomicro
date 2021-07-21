using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreNotificationsLibrary;
using coreNotificationsDAL;
using coreNotificationsLibrary.Services;

namespace coreNotificationsLibrary.Processors
{
    public class MessageEventSendingProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int LOAN_REPAYMENTS_EVENT_CATEGORY_ID = 0;
        private const int GENERAL_HTTP_SMS_SENDING_ERROR = 1;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return LOAN_REPAYMENTS_EVENT_CATEGORY_ID;
            } 
        }

        public DateTime lastProcessTime
        {
            get { return _lastProcessTime; }
        }

        public void process()
        {
            try
            {
                //IMessenger messenger = new SMSByHttpMessenger();
                //using (var ne = new notificationsModel())
                //{
                //    var events = getNewEvents(ne);
                //    foreach (var evt in events)
                //    {
                //        bool result = false;
                //        for (int i = 0; i < 3; i++)
                //        {
                //            var isMessgSent = messenger.sendMessage(evt.phoneNumber, evt.messageBody);
                //            if (isMessgSent)
                //            {
                //                evt.finished = true;
                //                ne.messagesSents.Add(new messagesSent
                //                {
                //                    messageEventID = evt.messageEventID,
                //                    sentDate = DateTime.Now
                //                });
                //                result = true;
                //                break;
                //            }
                //            System.Threading.Thread.Sleep(100);
                //        }
                //        if (result == false)
                //        {
                //            evt.finished = true;
                //            ne.messagesFaileds.Add(new messagesFailed
                //            {
                //                attemptDate = DateTime.Now,
                //                messageEventID = evt.messageEventID,
                //                messagesFailureReasonID = GENERAL_HTTP_SMS_SENDING_ERROR
                //            });
                //        }
                //    }
                //    ne.SaveChanges();
                //}
            }
            catch (Exception x)
            {
                ExceptionManager.LogException(x, "LoanRepaymentsSendingProcessor.process");
            }
        }

        private List<messageEvent> getNewEvents(coreNotificationsDAL.notificationsModel ne)
        {
            List<messageEvent> events = new List<messageEvent>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now;//.Date.AddDays(1).AddSeconds(-1);
            var evt = ne.messageEvents.Where(p => p.eventDate >= startDate && p.eventDate <= endDate
                && p.finished == false).ToList();
            events = evt;

            return events;
        }
    }
}
