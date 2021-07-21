using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class InvestmentMaturityRolloverCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int INVESTMENT_ROLLOVER_ADVICE_EVENT_CATEGORY_ID = 12;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return INVESTMENT_ROLLOVER_ADVICE_EVENT_CATEGORY_ID;
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
                                foreach (var ph in r.deposit.client.clientPhones)
                                {
                                    if (ph.phone != null && ph.phoneTypeID == 2 && ph.phone.phoneNo != null
                                        && ph.phone.phoneNo.Trim() != "")
                                    {
                                        mobileNo = ph.phone.phoneNo.Trim();
                                    }
                                }
                                if (mobileNo != "")
                                {
                                    var body = template.messageBodyTemplate.Replace("$$ACCOUNT_NUMBER$$", r.deposit.depositNo.AccountNumber()) 
                                        .Replace("$$BALANCE$$", (r.deposit.principalBalance+r.deposit.interestBalance).ToString("#,##0.#0"))
                                        .Replace("$$FIRST_NAME$$", r.deposit.client.otherNames.FirstName())
                                        .Replace("$$MATURITY_DATE$$", r.deposit.maturityDate.Value.ToString("yyyy-MM-dd"))
                                        .Replace("$$DATE$$", r.depositDate.ToString("yyyy-MM-dd"))
                                        .Replace("$$AMOUNT$$", r.depositAmount.ToString("#,##0.#0"))
                                        .Replace("$$PERIOD$$", r.deposit.period.ToString("#,##0.#0"))
                                    ;
                                    ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                    {
                                        accountID = r.deposit.depositID,
                                        clientID = r.deposit.client.clientID,
                                        eventDate = DateTime.Now,
                                        eventID = r.depositAdditionalID,
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
                ExceptionManager.LogException(x, "depositsRolloverCollectionProcessor.process");
            }
        }

        private List<depositAdditional> getMaturedDeposits(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<depositAdditional> deposits = new List<depositAdditional>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(3);

            var apps = le.depositAdditionals.Where(p => p.creation_date >= startDate && p.creation_date <= endDate
                && (p.principalBalance>1|| p.interestBalance>1)
                && (p.naration.Contains("RollOver"))).ToList();
            foreach (var r in apps)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.depositAdditionalID
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
