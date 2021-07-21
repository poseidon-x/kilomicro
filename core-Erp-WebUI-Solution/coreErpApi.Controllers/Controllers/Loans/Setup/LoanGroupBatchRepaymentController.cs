using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using coreData.Constants;
using coreData.ErrorLog;
using coreReports.fa;
using coreErpApi.Controllers.Models;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    public class LoanGroupBatchRepaymentController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        //BatchRepayModel

        private string ErrorToReturn = "";

        public LoanGroupBatchRepaymentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanGroupBatchRepaymentController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public groupLoanBatchDisbursementModel Get()
        {
            return new groupLoanBatchDisbursementModel
            {
                loans = new List<LoanViewModel>()
            };
        }

        [HttpGet]
        public BatchRepayModel GetModel()
        {
            return new BatchRepayModel
            {
                repayments = new List<BatchRepaymentViewModel>()
            };
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<LoanViewModel> Get(int id)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == id);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            return le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == 10
                && p.disbursementDate == null)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new LoanViewModel
                {
                    loanId = l.loanID,
                    clientId = c.clientID,
                    clientName = c.surName + " " + c.otherNames,
                    loanNumberWithName = l.loanNo +" - " + c.surName + " " + c.otherNames,
                    amountApproved = l.amountApproved,
                    amountDisbursed = 0.00
                })
                .OrderBy(p => p.loanNumberWithName)
                .ToList();
        }

        //Get Group Loans by group Id
        [HttpPost]
        public IEnumerable<BatchRepaymentViewModel> Get(BatchRepayModel repayM)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == repayM.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var data = le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == 10
                && p.disbursementDate != null)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new BatchRepaymentViewModel
                //.Join(le.repaymentSchedules, p => p.l.loanID, rs => rs.loanID, (p, rs) => new BatchRepaymentViewModel
                {
                    loanId = l.loanID,
                    loanNumberWithClient = c.surName + " " + c.otherNames + ", " + l.loanNo,
                    amountDisbursed = l.amountDisbursed,
                })
                //.Distinct()
                .OrderBy(p => p.loanNumberWithClient)
                .ToList();

            List<BatchRepaymentViewModel> dataToReturn = new List<BatchRepaymentViewModel>();

            foreach (var record in data)
            {
                var schd = le.repaymentSchedules.FirstOrDefault(p => p.loanID == record.loanId
                && p.interestBalance > 0 && p.principalBalance > 0);
                if (schd != null)
                {
                    record.amountDue = schd.principalPayment + schd.interestPayment;
                    dataToReturn.Add(record);
                }

            }
            return dataToReturn;
        }



        [AuthorizationFilter()]
        [HttpPost]
        public string PayBatch(BatchRepayModel batchRepayments)
        {
            if (batchRepayments == null) return null;

            StringBuilder lns = new StringBuilder();
            foreach (var repay in batchRepayments.repayments)
            {
                //amount paid is less than one(1)
                if (repay.paymentAmount < 1)
                    lns.Append(repay.loanNumberWithClient + "has repayment amount is less than one(1) <br/>");
            }
            if (lns.ToString() != "")
            {
                throw new ApplicationException(lns.ToString());
            }
            var loginUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
                var currentUser =
                    (new coreLogic.coreSecurityEntities()).users
                    .First(p => p.user_name.ToLower().Trim() == loginUser.ToLower().Trim());
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());

                if (ct == null)
                {
                    throw new ApplicationException("There is no till defined for the currently logged in user (" +
                                                   User.Identity.Name + ")");
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == batchRepayments.repaymentDate
                    && p.open == true);
                if (ctd == null)
                {
                    throw new ApplicationException("The till for the selected date has not been opened for this user (" +
                                                   User.Identity.Name + ")");
                }

            foreach (var repay in batchRepayments.repayments)
            {

                var currentLoan = le.loans.FirstOrDefault(p => p.loanID == repay.loanId);
                var currentSchedule =
                    le.repaymentSchedules.FirstOrDefault(p => p.repaymentScheduleID == repay.repaymentScheduleId);
                var cond = currentSchedule != null;
                if (currentLoan != null)
                {
                    var crtp = new coreLogic.cashierReceipt
                    {
                        amount = repay.paymentAmount,
                        bankID = repay.bankId,
                        checkNo = repay.chequeNumber,
                        clientID = currentLoan.clientID,
                        loanID = repay.loanId,
                        paymentModeID = repay.paymentModeId,
                        posted = false,
                        txDate = batchRepayments.repaymentDate,
                        cashierTillID = ct.cashiersTillID,
                        repaymentTypeID = repay.paymentTypeId
                    };
                    //check payment Type apply the amount paid
                    if (repay.paymentTypeId == 2) crtp.principalAmount = repay.paymentAmount;
                    else if (repay.paymentTypeId == 3) crtp.interestAmount = repay.paymentAmount;
                    else if (repay.paymentTypeId == 6) crtp.feeAmount = repay.paymentAmount;
                    else if (repay.paymentTypeId == 7) crtp.addInterestAmount = repay.paymentAmount;
                    le.cashierReceipts.Add(crtp);

                    var sav = le.savings.FirstOrDefault(p => p.clientID == currentLoan.clientID);

                    if (sav.savingID > 0)
                    {
                        sav.amountInvested += repay.cashCollateral;
                        var da = new coreLogic.savingAdditional
                        {
                            checkNo = repay.chequeNumber,
                            savingAmount = repay.cashCollateral,
                            naration = "Cash Collateral",
                            bankID = repay.bankId,
                            fxRate = 0,
                            localAmount = repay.cashCollateral,
                            interestBalance = 0,
                            savingDate = batchRepayments.repaymentDate,
                            creation_date = DateTime.Now,
                            creator = loginUser,
                            principalBalance = repay.cashCollateral,
                            modeOfPaymentID = repay.paymentModeId,
                            posted = false,
                            closed = false
                        };
                        sav.principalBalance += repay.cashCollateral;
                        sav.availablePrincipalBalance += repay.cashCollateral;
                        sav.savingAdditionals.Add(da);

                        sav.modification_date = DateTime.Now;
                        sav.last_modifier = User.Identity.Name;

                    }

                }
            }

            try
                {
                    le.SaveChanges();
                }
                catch (Exception x)
                {
                    throw x;
                }

                return "Batch disbursement has been received successfully";
        }


    }
}
