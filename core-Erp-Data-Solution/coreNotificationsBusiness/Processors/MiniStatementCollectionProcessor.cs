using coreLogic;
using coreNotificationsLibrary.Models;
using coreReports;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsLibrary.Processors
{
    public class MiniStatementCollectionProcessor : IEventLoanRepaymentsCollectionProcessor
    {
        private const int MINI_STATEMENT_EVENT_CATEGORY_ID = 8;
        private const string MESSAGE_SENDER = "Kilo";
        private DateTime _lastProcessTime;
        public int messageEventCategoryID
        {
            get
            {
                return MINI_STATEMENT_EVENT_CATEGORY_ID;
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
                var dayOfMonth = DateTime.Now.Day;
                var lastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var hourOfProcess = DateTime.Now.Hour;
                var dayToExec =int.Parse(ConfigurationManager.AppSettings["dayToExec"]);
                var firstDayOfMonth = 1;
                var smsSender = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SMS_SENDER"].ToString()) ? MESSAGE_SENDER : ConfigurationManager.AppSettings["SMS_SENDER"].ToString();
               // var hourToProcess = 7;
                if ((dayOfMonth == dayToExec || dayOfMonth==firstDayOfMonth))//|| dayOfMonth == lastDayOfMonth && hourOfProcess == hourToProcess)
                {
                    using (var le = new coreLoansEntities())
                    {
                        using (var ne = new coreNotificationsDAL.notificationsModel())
                        {
                            if (ne.messageEventCategories.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID).isEnabled == true)
                            {
                                int count = 1;
                                if (count <= 1)
                                {
                                    count++;
                                    var balances = GetLoanAndSavingBalances();
                                    var template = ne.messageTemplates.FirstOrDefault(p => p.messageEventCategoryID == messageEventCategoryID);
                                    foreach (var balance in balances)
                                    {

                                        if (!string.IsNullOrWhiteSpace(balance.ClientPhoneNo))
                                        {
                                            var bodyHolders = new
                                            {
                                                ClientName = balance.ClientName.FirstNameTitleCased(),
                                                LoanAmount = balance.LoanAmount,
                                                LoanBalance = balance.LoanOutstandingAmount,
                                                SecurityBalance = balance.SavingBalance,
                                                AccountNumber = balance.LoanNo.AccountNumber(),
                                                //TotalPaidOff = balance.TotalPaid,
                                                TotalPaidCount = balance.RepaymentCount,
                                                MessageDate = DateTime.Now
                                            };
                                            var body = Smart.Format(template.messageBodyTemplate, bodyHolders);
                                            var eventId = balance.LoanId + DateTime.Now.Day + DateTime.Now.Month;
                                            var newEvent = new coreNotificationsDAL.messageEvent
                                            {
                                                accountID = balance.LoanId,
                                                clientID = balance.ClientId,
                                                eventDate = DateTime.Now,
                                                eventID = +eventId,
                                                messageEventCategoryID = messageEventCategoryID,
                                                finished = false,
                                                messageBody = body,
                                                phoneNumber = balance.ClientPhoneNo,
                                                sender = smsSender
                                            };
                                            var existedEvent = ne.messageEvents?.FirstOrDefault(e => e.eventID == newEvent.eventID && e.messageEventCategoryID == newEvent.messageEventCategoryID);
                                            if (existedEvent == null)
                                            {
                                                ne.messageEvents.Add(newEvent);
                                            }
                                        }
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
                ExceptionManager.LogException(x, "MiniStatementCollectionProcessor.process");
            }
        }

        private List<OutstandingLoanAndSaving> GetLoanAndSavingBalances()
        {
            List<OutstandingLoanAndSaving> outstandings = new List<OutstandingLoanAndSaving>();
            try
            {
                reportEntities repEnt = new reportEntities();
                //var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                var endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                outstandings = repEnt.spOutstanding(endDate).
                    Where(p =>
                    ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)) > 0)
                    .Select(p => new OutstandingLoanAndSaving
                    {
                        ClientId = p.clientID,
                        ClientName = p.clientName,
                        LoanAmount = p.amountDisbursed + p.interest,
                        LoanId = p.loanID,
                        LoanNo = p.loanNo,
                        LoanOutstandingAmount = ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)),
                        ExpiryDate = p.expiryDate,
                        WriteOffAmount = p.writtenOff,
                        Collateral = p.fairValue,
                        DisbursementDate = p.disbursementDate,
                        TotalPaid = p.totalPaid,
                        Interest=p.interest,
                        Fee=p.fee
                    })
                    .ToList();
                foreach (var outstand in outstandings)
                {
                    outstand.SavingBalance = GetClientSavingBalance(endDate, outstand.ClientId);
                    outstand.ClientPhoneNo = GetClientPhoneNo(outstand.ClientId);
                    outstand.RepaymentCount = GetNumberOfRepaymentForLoan(outstand.LoanId, endDate);
                }

            }
            catch (Exception e)
            {
                ExceptionManager.LogException(e, "MiniStatementCollectionProcessor.GetLoanAndSavingBalances");
            }
            return outstandings.ToList();
        }


        private double GetClientSavingBalance(DateTime endDate, int clientId)
        {
            double savingBalance = 0;
            try
            {
                reportEntities repEnt = new reportEntities();
                var saving = repEnt.vwSavingStatements
                    .Where(p => p.date <= endDate && p.clientID == clientId)
                    .Select(r => new
                    {
                        DepositAmount = r.depositAmount,
                        PrincipalWithdrawalAmount = r.princWithdrawalAmount,
                        InterestAccruedAmount = r.interestAccruedAmount,
                        InterestWithdrawalAmount = r.intWithdrawalAmount
                    }).ToList();
                if (saving != null && saving?.Count > 0)
                {
                    var availablePrincipal = (saving.Sum(p => p.DepositAmount) - saving.Sum(p => p.PrincipalWithdrawalAmount));
                    var availableInterest = (saving.Sum(p => p.InterestAccruedAmount) - saving.Sum(p => p.InterestWithdrawalAmount));
                    savingBalance = availablePrincipal + availableInterest;
                }

            }
            catch (Exception e)
            {
                ExceptionManager.LogException(e, "MiniStatementCollectionProcessor.GetClientSavingBalance");
            }
            return savingBalance;
        }

        private string GetClientPhoneNo(int clientId)
        {
            string mobileNo = "";
            try
            {
                coreLoansEntities le = new coreLoansEntities();
                var phone = le.clientPhones.FirstOrDefault(e => e.clientID == clientId).phone;
                if (phone != null && phone.phoneTypeID == 2 && phone.phoneNo != null
                                        && phone.phoneNo.Trim() != "")
                {
                    mobileNo = phone.phoneNo.Trim();
                }

            }
            catch (Exception e)
            {
                ExceptionManager.LogException(e, "MiniStatementCollectionProcessor.GetClientPhoneNo");
            }
            return mobileNo;
        }

        private int GetNumberOfRepaymentForLoan(int loanId, DateTime repayDate)
        {
            int totalCount = 0;
            try
            {
                coreLoansEntities le = new coreLoansEntities();
                var repay = le.loanRepayments.Where(p => p.loanID == loanId && p.amountPaid > 0
                && p.repaymentDate <= repayDate && (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 || p.repaymentTypeID == 3))
                    ?.ToList()?.Count;
                if (repay != null)
                    totalCount = repay.Value;
            }
            catch (Exception e)
            {
                ExceptionManager.LogException(e, "MiniStatementCollectionProcessor.GetNumberOfRepaymentForLoan");
            }

            return totalCount;
        }

    }
}
