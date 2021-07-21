using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class InvestmentWithdrawalCollecionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int INVESTMENT_WITHDRAWAL_EVENT_CATEGORY_ID = 6;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return INVESTMENT_WITHDRAWAL_EVENT_CATEGORY_ID;
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
                            var deposits = getNewWithdrawals(le, ne);
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
                                        .Replace("$$AMOUNT$$", (r.principalWithdrawal + r.interestWithdrawal).ToString("#,##0.#0"))
                                        .Replace("$$BALANCE$$", (r.deposit.principalBalance + r.deposit.interestBalance).ToString("#,##0.#0"))
                                        .Replace("$$FIRST_NAME$$", r.deposit.client.otherNames.FirstName())
                                        .Replace("$$TRANSACTION_TYPE$$", ((r.modeOfPayment.modeOfPaymentID == 1) ? "Cash Withdrawal"
                                        : (r.modeOfPayment.modeOfPaymentID == 2) ? "Check Withdrawal" : "Bank Transfer"))
                                        .Replace("$$DATE$$",r.withdrawalDate.ToString("yyyy-MM-dd"))
                                        .Replace("$$NARATION$$", r.naration.Truncate(30))
                                        .Replace("$$MATURITY_DATE$$", r.deposit.maturityDate.Value.ToString("yyyy-MM-dd"))
                                        .Replace("$$MODE_OF_PAYMENT$$", r.modeOfPayment.modeOfPaymentName);
                                    ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                    {
                                        accountID = r.deposit.depositID,
                                        clientID = r.deposit.client.clientID,
                                        eventDate = DateTime.Now,
                                        eventID = r.depositWithdrawalID,
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
                ExceptionManager.LogException(x, "depositsWithdrawalCollectionProcessor.process");
            }
        }

        private List<depositWithdrawal> getNewWithdrawals(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<depositWithdrawal> deposits = new List<depositWithdrawal>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now;

            var apps = le.depositWithdrawals.Where(p => p.withdrawalDate >= startDate && p.withdrawalDate <= endDate
                && p.posted == true).ToList();
            foreach (var r in apps)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.depositWithdrawalID
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
