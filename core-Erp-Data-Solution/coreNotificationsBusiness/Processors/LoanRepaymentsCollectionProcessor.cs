using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreNotificationsLibrary.Processors
{
    public class LoanRepaymentsCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int LOAN_REPAYMENTS_EVENT_CATEGORY_ID = 1;
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
                using (var le = new coreLoansEntities())
                {
                    using (var ne = new coreNotificationsDAL.notificationsModel())
                    {
                        if (ne.messageEventCategories.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID).isEnabled == true)
                        {
                            int count = 1;
                            if(count <= 1)
                            {
                                count++;
                                var repayments = getNewRepayments(le, ne);
                                var template = ne.messageTemplates.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID);
                                List<int> receiptIDs = new List<int>();
                                foreach (var r in repayments)
                                {
                                    var amountPaid = r.amount;
                                    var mobileNo = "";
                                    var mpc = (from r2 in le.cashierReceipts
                                               from m in le.multiPaymentClients
                                               where r2.cashierReceiptID == m.cashierReceiptID
                                                && r2.clientID == r.clientID
                                               && r2.txDate == r.txDate
                                               select m).FirstOrDefault();
                                    if (mpc != null)
                                    {
                                        if (receiptIDs.Contains(mpc.multiPaymentClientID))
                                        {
                                            continue;
                                        }
                                        receiptIDs.Add(mpc.multiPaymentClientID);
                                        amountPaid = mpc.checkAmount;
                                    }
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
                                        var body = template.messageBodyTemplate.Replace("$$ACCOUNT_NUMBER$$", r.loan.loanNo.AccountNumber())
                                            .Replace("$$AMOUNT$$", amountPaid.ToString("#,##0.#0"))
                                            .Replace("$$FIRST_NAME$$", r.loan.client.otherNames.FirstName())
                                            .Replace("$$PAYMENT_TYPE$$", "");
                                        ne.messageEvents.Add(new coreNotificationsDAL.messageEvent
                                        {
                                            accountID = r.loanID,
                                            clientID = r.loan.clientID,
                                            eventDate = DateTime.Now,
                                            eventID = r.cashierReceiptID,
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
            }
            catch (Exception x)
            {
                ExceptionManager.LogException(x, "LoanRepaymentsCollectionProcessor.process");
            }
        }

        private List<cashierReceipt> getNewRepayments(coreLoansEntities le, coreNotificationsDAL.notificationsModel ne)
        {
            List<cashierReceipt> repayments = new List<cashierReceipt>();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now;

            var rpmts = le.cashierReceipts.Where(p => p.txDate >= startDate && p.txDate <= endDate
                && p.posted == true).ToList();
            foreach (var r in rpmts)
            {
                var ev = ne.messageEvents.FirstOrDefault(p => p.eventID == r.cashierReceiptID
                    && p.messageEventCategoryID == messageEventCategoryID);
                if (ev == null)
                {
                    repayments.Add(r);
                }
            }

            return repayments;
        }
    }
}
