using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class ClientBirthdayCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int CLIENT_BIRTHDAY_EVENT_CATEGORY_ID = 13;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return CLIENT_BIRTHDAY_EVENT_CATEGORY_ID;
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
                ExceptionManager.LogException(x, "ClientBirthdayCollectionProcessor.process");
            }
        }

        private List<client> getClientBirthdays(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<client> clients = new List<client>();
            var startDate = DateTime.Now.Date.AddDays(-2);
            var endDate = DateTime.Now;
            var oneYearAgo = DateTime.Now.Date.AddDays(-360);

            var apps = le.clients.ToList();
            foreach (var r in apps)
            {
                if ((r.DOB.Value.Month == endDate.Month || r.DOB.Value.Month == startDate.Month)
                    && (r.DOB.Value.Day == endDate.Day || r.DOB.Value.Day == startDate.Day || r.DOB.Value.Day == startDate.AddDays(1).Day))
                {
                    var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.clientID
                        && p.messageEventCategoryID == messageEventCategoryID
                        && p.eventDate < DateTime.Now
                        && p.eventDate > oneYearAgo);
                    if (ev == null)
                    {
                        clients.Add(r);
                    }
                }
            }

            return clients;
        }
    }
}
