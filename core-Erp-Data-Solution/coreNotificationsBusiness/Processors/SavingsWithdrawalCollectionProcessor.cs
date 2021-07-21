using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreReports;

namespace coreNotificationsLibrary.Processors
{
    public class SavingsWithdrawalCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int SAVINGS_WITHDRAWAL_EVENT_CATEGORY_ID = 5;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return SAVINGS_WITHDRAWAL_EVENT_CATEGORY_ID;
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
                            var conf = le.savingConfigs.First();
                            foreach (var r in deposits)
                            {
                                var mobileNo = "";
                                foreach (var ph in r.saving.client.clientPhones)
                                {
                                    if (ph.phone != null && ph.phoneTypeID == 2 && ph.phone.phoneNo != null
                                        && ph.phone.phoneNo.Trim() != "")
                                    {
                                        mobileNo = ph.phone.phoneNo.Trim();
                                    }
                                }
                                if (mobileNo != "")
                                {
                                    var bal = GetBalanceAsAt(r.withdrawalDate, r.savingID, conf);
                                    if (!r.posted)
                                    {
                                        bal = bal + r.interestWithdrawal + r.principalWithdrawal;
                                    }
                                    var body = template.messageBodyTemplate.Replace("$$ACCOUNT_NUMBER$$", r.saving.savingNo.AccountNumber())
                                        .Replace("$$AMOUNT$$", (r.principalWithdrawal + r.interestWithdrawal).ToString("#,##0.#0"))
                                        .Replace("$$BALANCE$$", bal.ToString("#,##0.#0"))
                                        .Replace("$$FIRST_NAME$$", r.saving.client.otherNames.FirstName())
                                        .Replace("$$TRANSACTION_TYPE$$", ((r.modeOfPayment.modeOfPaymentID == 1) ? "Cash Withdrawal"
                                        : (r.modeOfPayment.modeOfPaymentID == 2) ? "Check Withdrawal" : "Bank Transfer"))
                                        .Replace("$$DATE$$",r.withdrawalDate.ToString("yyyy-MM-dd"))
                                        .Replace("$$NARATION$$", r.naration.Truncate(30));
                                    ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                    {
                                        accountID = r.savingID,
                                        clientID = r.saving.clientID,
                                        eventDate = DateTime.Now,
                                        eventID = r.savingWithdrawalID,
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
                ExceptionManager.LogException(x, "SavingsWithdrawalCollectionProcessor.process");
            }
        }

        private List<savingWithdrawal> getNewWithdrawals(coreLoansEntities le,
            coreNotificationsDAL.notificationsModel ne)
        {
            List<savingWithdrawal> withdrawals = new List<savingWithdrawal>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now;

            var apps = le.savingWithdrawals.Where(p => p.withdrawalDate >= startDate && p.withdrawalDate <= endDate
                                                       && p.posted == true).ToList();
            foreach (var r in apps)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.savingWithdrawalID
                                                              && p.messageEventCategoryID == messageEventCategoryID);
                if (ev == null)
                {
                    withdrawals.Add(r);
                }
            }

            return withdrawals;

        }


        private double GetBalanceAsAt(DateTime datei, int savingID, savingConfig config)
        {
            var bal = 0.0;
            var rent = new reportEntities();

            var date = datei.Date.AddDays(-config.principalBalanceLatency);
            var date2 = datei.Date.AddDays(-config.interestBalanceLatency);
            var curPrincBal = rent.vwSavingStatements.Where(p => p.loanID == savingID).Sum(p => p.depositAmount)
                              - rent.vwSavingStatements.Where(p => p.loanID == savingID)
                                  .Sum(p => p.princWithdrawalAmount + p.chargeAmount);
            var availPrincBal = rent.vwSavingStatements.Where(p => p.loanID == savingID && p.date <= date)
                .Sum(p => p.depositAmount)
                                - rent.vwSavingStatements.Where(p => p.loanID == savingID)
                                    .Sum(p => p.princWithdrawalAmount);
            var curIntBal = rent.vwSavingStatements.Where(p => p.loanID == savingID).Sum(p => p.interestAccruedAmount)
                            - rent.vwSavingStatements.Where(p => p.loanID == savingID)
                                .Sum(p => p.intWithdrawalAmount);
            var availIntBal = rent.vwSavingStatements.Where(p => p.loanID == savingID && p.date <= date2)
                .Sum(p => p.interestAccruedAmount)
                              - rent.vwSavingStatements.Where(p => p.loanID == savingID)
                                  .Sum(p => p.intWithdrawalAmount);

            bal = rent.vwSavingStatements.Where(p => p.loanID == savingID && p.date <= datei)
                .Sum(p => p.depositAmount - p.princWithdrawalAmount - p.chargeAmount);
            if (config.accrueInterestToPrincipal == true) bal = bal + availIntBal;

            if (bal < 0) bal = 0;

            return bal;
        }

    }
}
