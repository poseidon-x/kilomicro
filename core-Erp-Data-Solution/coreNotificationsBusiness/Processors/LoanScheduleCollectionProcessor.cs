using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class LoanScheduleCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int LOAN_SCHEDULE_EVENT_CATEGORY_ID = 7;
        private const string MESSAGE_SENDER = "Kilo";
        private const int DISBURSED_LOAN_STATUS_ID = 4;
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return LOAN_SCHEDULE_EVENT_CATEGORY_ID;
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
                            var schedules = getNewSchedules(le, ne);
                            var config = ne.messagingConfigs.FirstOrDefault();
                            var template = ne.messageTemplates.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID);
                            foreach (var r in schedules)
                            {
                                var mobileNo = "";
                                foreach (var ph in r.loan.client.clientPhones)
                                {
                                    if (ph.phone != null && ph.phoneTypeID == 2 && ph.phone.phoneNo != null
                                        && ph.phone.phoneNo.Trim() != "")
                                    {
                                        mobileNo = ph.phone.phoneNo.Trim();
                                    }
                                }
                                if (mobileNo != "")
                                {
                                    var balance = 0.0;
                                    GetBalanceAsAt(DateTime.Now, r.loanID, ref balance);
                                    var body = template.messageBodyTemplate.Replace("$$ACCOUNT_NUMBER$$", r.loan.loanNo.AccountNumber())
                                        .Replace("$$AMOUNT$$", balance.ToString("#,##0.#0"))
                                        .Replace("$$FIRST_NAME$$", r.loan.client.otherNames.FirstName())
                                        .Replace("$$STATUS$$", (r.repaymentDate<=DateTime.Now.AddDays(-config.numberOfDaysBeforeLoanOverdue)
                                            ?(r.repaymentDate>DateTime.Now.AddDays(3)?"":"Overdue "):""))
                                        .Replace("$$DATE$$", r.repaymentDate.ToString("yyyy-MM-dd"));
                                    ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                    {
                                        accountID = r.loanID,
                                        clientID = r.loan.clientID,
                                        eventDate = DateTime.Now,
                                        eventID = r.loanID,
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
                ExceptionManager.LogException(x, "LoanScheduleCollectionProcessor.process");
            }
        }

        private List<repaymentSchedule> getNewSchedules(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<repaymentSchedule> schedules = new List<repaymentSchedule>();
            var startDate = DateTime.Now.Date.AddYears(-1);
            var endDate = DateTime.Now.AddDays(3);
            var config = ne.messagingConfigs.FirstOrDefault();
            var sd = DateTime.Now.Date.AddMonths(-config.loanRepaymentNotificationCycle);
            var ed = DateTime.Now.Date.AddDays(3).AddSeconds(-1);

            var lastDayToCheck = DateTime.Now.AddDays(-config.numberOfDaysBeforeLoanOverdue);
            var apps = le.repaymentSchedules.Where(p => p.repaymentDate >= startDate && p.repaymentDate <= endDate
                && (p.principalBalance+p.interestBalance)>5
                && p.loan.balance>5
                && p.loan.loanStatusID == DISBURSED_LOAN_STATUS_ID)
                .OrderBy(p=> p.repaymentDate)
                .ToList();
            foreach (var r in apps)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.loanID
                    && p.messageEventCategoryID == messageEventCategoryID
                    && (p.eventDate > sd && p.eventDate < ed));
                if (ev == null)
                {
                    schedules.Add(r);
                }
            }

            return schedules;
        }

        private void GetBalanceAsAt(DateTime date, int loanID, ref double bal)
        {
            using (var rent = new coreReports.reportEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var rs = rent.vwLoanActualSchedules.Where(p => p.loanID == loanID && p.date < date).ToList();
                    var rs2 = le.loanRepayments.Where(p => p.loanID == loanID && p.repaymentDate <= date
                        && (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 || p.repaymentTypeID == 3)).ToList();
                    bal = ((rs.Count > 0) ?
                                rs.Sum(p => p.interest) : 0.0)
                        -
                                ((rs2.Count > 0) ?
                                rs2.Sum(p => p.amountPaid) : 0.0)
                        +
                        rent.vwLoanActualSchedules.Where(p => p.loanID == loanID)
                        .Max(p => p.amountDisbursed);
                    if (bal < 0)
                    {
                        bal = 0;
                    }
                }
            }
        }
    }
}
