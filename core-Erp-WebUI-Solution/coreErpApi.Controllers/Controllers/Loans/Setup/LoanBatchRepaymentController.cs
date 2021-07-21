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
    [AuthorizationFilter()]
    public class LoanBatchRepaymentController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        private string ErrorToReturn = "";

        public LoanBatchRepaymentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanBatchRepaymentController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        [HttpGet]
        public BatchRepayModel GetModel()
        {
            return new BatchRepayModel();
        }

        public groupLoanBatchRepaymentModel Get()
        {
            return new groupLoanBatchRepaymentModel
            {
                repayments = new List<BatchRepaymentViewModel>()
            };
        }



        [HttpPost]
        public string DisburseBatch(groupLoanBatchDisbursementModel batch)
        {
            double totalBatchAmount = 0;
            StringBuilder lns = new StringBuilder();
            foreach (var ln in batch.loans)
            {
                //amount disbursed is greater than amount approved
                if (ln.amountDisbursed > ln.amountApproved)
                    lns.Append(ln.loanNumberWithName + "has amount disbursed greater than approved <br/>");
                //If client have savings account
                var sav = le.savings.FirstOrDefault(p => p.clientID == ln.clientId);
                if (sav == null)
                    lns.Append(ln.loanNumberWithName + "loan client doesn't have account <br/>");
                //If amount to disburse is greater than user hass level
                totalBatchAmount = batch.loans.Select(p => p.amountDisbursed).Sum();
            }
            if (lns.ToString() != "")
            {
                throw new ApplicationException("The follwing loans has amount disbursed greater than approved <br/>");
            }
            var user =
                (new coreLogic.coreSecurityEntities()).users.First(
                    p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
            if (user.accessLevel.disbursementLimit < totalBatchAmount)
            {
                throw new ApplicationException("The amount to be disbursed is beyond your access level");
            }
            if (user.accessLevel.disbursementLimit < totalBatchAmount)
            {
                throw new ApplicationException("The amount to be disbursed is beyond your access level");
            }
            var ctl =
                le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ctl == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" +
                                               User.Identity.Name + ")");
            }
            var ctd =
                le.cashiersTillDays.FirstOrDefault(
                    p => p.cashiersTillID == ctl.cashiersTillID && p.tillDay == batch.disbursementDate
                         && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" +
                                               User.Identity.Name + ")");
            }

            foreach (var ln in batch.loans)
            {
                //int? bankID = null;
                //if (ln.bankId != null && ln.bankId > 0)
                //    bankID = ln.bankId.Value;
                var cd = new coreLogic.cashierDisbursement
                {
                    amount = ln.amountDisbursed,
                    bankID = ln.bankId,
                    checkNo = ln.chequeNumber,
                    clientID = ln.clientId,
                    loanID = ln.loanId,
                    paymentModeID = ln.paymentModeId,
                    posted = false,
                    txDate = batch.disbursementDate,
                    cashierTillID = ctl.cashiersTillID,
                    postToSavingsAccount = true
                };
                le.cashierDisbursements.Add(cd);
            }
            le.SaveChanges();
            return "Batch disbursement has been received successfully";
        }


    }
}
