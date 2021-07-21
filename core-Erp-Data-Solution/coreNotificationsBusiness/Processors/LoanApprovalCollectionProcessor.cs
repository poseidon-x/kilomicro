using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class LoanApprovalCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int LOAN_APPROVAL_EVENT_CATEGORY_ID = 2;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return LOAN_APPROVAL_EVENT_CATEGORY_ID;
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
                            var approvals = getNewApprovals(le, ne);
                            var template = ne.messageTemplates.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID);
                            foreach (var r in approvals)
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
                                    var body = template.messageBodyTemplate.Replace("$$ACCOUNT_NUMBER$$", r.loanNo.AccountNumber())
                                        .Replace("$$AMOUNT$$", r.amountApproved.ToString("#,##0.#0"))
                                        .Replace("$$FIRST_NAME$$", r.client.otherNames.AccountNumber());
                                    ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                    {
                                        accountID = r.loanID,
                                        clientID = r.clientID,
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
                ExceptionManager.LogException(x, "LoanApprovalsCollectionProcessor.process");
            }
        }

        private List<loan> getNewApprovals(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<loan> approvals = new List<loan>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now;

            var apps = le.loans.Where(p => p.finalApprovalDate >= startDate && p.finalApprovalDate <= endDate).ToList();
            foreach (var r in apps)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.loanID
                    && p.messageEventCategoryID == messageEventCategoryID);
                if (ev == null)
                {
                    approvals.Add(r);
                }
            }

            return approvals;
        }
    }
}
