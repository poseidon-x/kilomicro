using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class InvestmentMaturityAdviceCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int INVESTMENT_MATURITY_ADVICE_EVENT_CATEGORY_ID = 10;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return INVESTMENT_MATURITY_ADVICE_EVENT_CATEGORY_ID;
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
                            var deposits = getMaturedDeposits(le, ne);
                            var template = ne.messageTemplates.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID);
                            foreach (var r in deposits)
                            {
                                var mobileNo = "";
                                foreach (var ph in r.client.clientPhones)
                                {
                                    if (ph.phone != null && ph.phoneTypeID == 2 && ph.phone.phoneNo != null
                                        && ph.phone.phoneNo.Trim() != "")
                                    {
                                        mobileNo = ph.phone.phoneNo.Trim();
                                    }
                                }
                                if (mobileNo != "")
                                {
                                    var body = template.messageBodyTemplate.Replace("$$ACCOUNT_NUMBER$$", r.depositNo.AccountNumber()) 
                                        .Replace("$$BALANCE$$", (r.principalBalance+r.interestBalance).ToString("#,##0.#0"))
                                        .Replace("$$FIRST_NAME$$", r.client.otherNames.FirstName())
                                        .Replace("$$DATE$$",r.maturityDate.Value.ToString("yyyy-MM-dd"))
                                    ;
                                    ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                    {
                                        accountID = r.depositID,
                                        clientID = r.client.clientID,
                                        eventDate = DateTime.Now,
                                        eventID = r.depositID,
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
                ExceptionManager.LogException(x, "depositsDepositCollectionProcessor.process");
            }
        }

        private List<deposit> getMaturedDeposits(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<deposit> deposits = new List<deposit>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(3);

            var apps = le.deposits.Where(p => p.maturityDate >= startDate && p.maturityDate <= endDate
                && (p.principalBalance>1|| p.interestBalance>1)).ToList();
            foreach (var r in apps)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.depositID
                    && p.messageEventCategoryID == messageEventCategoryID);
                if (ev == null)
                {
                    deposits.Add(r);
                }
            }

            return deposits;
        }
    }
}
