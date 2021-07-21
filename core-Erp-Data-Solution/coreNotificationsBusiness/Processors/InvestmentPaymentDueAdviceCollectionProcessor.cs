using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class InvestmentPaymentDueAdviceCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int INVESTMENT_PAYMENT_DUE_EVENT_CATEGORY_ID = 11;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return INVESTMENT_PAYMENT_DUE_EVENT_CATEGORY_ID;
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
                            var deposits = getDueDeposits(le, ne);
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
                ExceptionManager.LogException(x, "depositsDepositPaymentDueCollectionProcessor.process");
            }
        }

        private List<deposit> getDueDeposits(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<deposit> deposits = new List<deposit>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(3);
            var oneMonthAgo = DateTime.Now.AddDays(-30);

            var matured = le.deposits
                    .Where(p => (p.principalRepaymentModeID == 30 || p.interestRepaymentModeID == 30)
                            && p.principalBalance + p.interestBalance > 2)
                    .ToList();
            foreach (var r in matured)
            {
                var lastPayment = r.depositWithdrawals.OrderByDescending(prop => prop.withdrawalDate)
                    .FirstOrDefault();
                var lastPaymentDate = r.firstDepositDate;
                if (lastPayment != null) lastPaymentDate = lastPayment.withdrawalDate;
                if (lastPaymentDate >= oneMonthAgo) continue;
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
