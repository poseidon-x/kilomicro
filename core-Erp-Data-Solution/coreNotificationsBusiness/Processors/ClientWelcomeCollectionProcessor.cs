using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class ClientWelcomeCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int CLIENT_WELCOME_EVENT_CATEGORY_ID = 9;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return CLIENT_WELCOME_EVENT_CATEGORY_ID;
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
                using (var le = new coreLoansEntities())
                {
                    using (var ne = new coreNotificationsDAL.notificationsModel())
                    {
                        if (ne.messageEventCategories.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID).isEnabled == true)
                        {
                            var clients = getClientBirthdays(le, ne);
                            var template = ne.messageTemplates.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID);
                            foreach (var r in clients)
                            {
                                var mobileNo = "";
                                foreach (var ph in r.clientPhones)
                                {
                                    if (ph.phone != null && ph.phoneTypeID == 2 && ph.phone.phoneNo != null
                                        && ph.phone.phoneNo.Trim() != "")
                                    {
                                        mobileNo = ph.phone.phoneNo.Trim();
                                    }
                                }
                                if (mobileNo != "")
                                {
                                    var body = template.messageBodyTemplate
                                        .Replace("$$FIRST_NAME$$", r.otherNames.FirstName());
                                    ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                    {
                                        accountID = r.clientID,
                                        clientID = r.clientID,
                                        eventDate = DateTime.Now,
                                        eventID = r.clientID,
                                        messageEventCategoryID = messageEventCategoryID,
                                        finished = false,
                                        messageBody = body,
                                        phoneNumber = mobileNo,
                                        sender = MESSAGE_SENDER
                                    });
                                }
                            }
                            ne.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception x)
            {
                ExceptionManager.LogException(x, "ClientWelcomeCollectionProcessor.process");
            }
        }

        private List<client> getClientBirthdays(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<client> clients = new List<client>();
            var startDate = DateTime.Now.Date.AddDays(-2);
            var endDate = DateTime.Now;
            var oneYearAgo = DateTime.Now.Date.AddDays(-360);

            var apps = le.clients.Where(p=> p.creation_date>=startDate && p.creation_date<= endDate).ToList();
            foreach (var r in apps)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.clientID
                    && p.messageEventCategoryID == messageEventCategoryID);
                if (ev == null)
                {
                    clients.Add(r);
                }
            }

            return clients;
        }
    }
}
