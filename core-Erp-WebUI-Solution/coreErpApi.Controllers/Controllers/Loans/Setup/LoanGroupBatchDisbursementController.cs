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
   //[AuthorizationFilter()]
    public class LoanGroupBatchDisbursementController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        private string ErrorToReturn = "";

        public LoanGroupBatchDisbursementController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanGroupBatchDisbursementController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        //[AuthorizationFilter()]
        [HttpGet]
        public groupLoanBatchDisbursementModel Get()
        {
            return new groupLoanBatchDisbursementModel
            {
                loans = new List<LoanViewModel>()
            };
        }

        public BatchDisburseModel GetDisbursementDate()
        {
            return new BatchDisburseModel();
        }

        // GET: api/
        [HttpPost]
        public IEnumerable<LoanViewModel> Get(BatchDisburseModel disbursm)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == disbursm.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupLoan = le.loanTypes.FirstOrDefault(p => p.loanTypeName.ToLower().Contains("group loan"));
            var groupClients = group.loanGroupClients
                .Where(p => p.loanGroupId == disbursm.groupId)
                .Select(p => p.clientId).ToList();
            int groupLoanId = groupLoan.loanTypeID;

            var lns = le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == groupLoanId
                && p.disbursementDate == null && p.finalApprovalDate <= disbursm.dibursementDate && p.loanStatusID == 3)
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

            return lns;
        }


        [AuthorizationFilter()]
        [HttpPost]
        public string DisburseBatch(groupLoanBatchDisbursementModel batch)
        {
            if (batch == null) return null;
            double totalBatchAmount = 0;
            StringBuilder lns = new StringBuilder();
            foreach (var ln in batch.loans)
            {
                //amount disbursed is greater than amount approved
                if (ln.amountDisbursed > ln.amountApproved)
                    lns.Append(ln.loanNumberWithName + "has amount disbursed greater than approved <br/>");
                //If client have savings account
                var loan = le.loans.FirstOrDefault(p => p.loanID == ln.loanId);
                var clientId = loan.clientID;
                var sav = le.savings.FirstOrDefault(p => p.clientID == clientId);
                if (sav == null)
                    lns.Append(ln.loanId + "loan client doesn't have a savings account <br/>");


                if (sav.availableInterestBalance < loan.securityDeposit)
                {
                    var s = String.Format("{0:N2}", loan.securityDeposit);
                    lns.Append(ln.loanId + "loan client doesn't have enough saving balance for security depoit " +
                               "Please make minimium deposit of " + s + " to continue < br/>");
                }
                
                //If amount to disburse is greater than user hass level
                totalBatchAmount = batch.loans.Select(p => p.amountDisbursed).Sum();
            }
            if (lns.ToString() != "")
            {
                throw new ApplicationException(lns.ToString());
            }
            var loginuser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var user =
                (new coreLogic.coreSecurityEntities()).users.First(
                    p => p.user_name.ToLower().Trim() == loginuser.ToLower().Trim());
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
                    p => p.cashiersTillID == ctl.cashiersTillID && p.tillDay == batch.disbursementDate.Date
                         && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" +
                                               User.Identity.Name + ")");
            }

            //Disburse all loans in the current batch
            foreach (var ln in batch.loans)
            {
                var currentLoan = le.loans.FirstOrDefault(p => p.loanID == ln.loanId);
                if (currentLoan != null)
                {
                    var cd = new coreLogic.cashierDisbursement
                    {
                        amount = ln.amountDisbursed,
                        bankID = ln.bankId,
                        checkNo = ln.chequeNumber,
                        clientID = currentLoan.clientID,
                        loanID = ln.loanId,
                        paymentModeID = ln.paymentModeId,
                        posted = false,
                        txDate = batch.disbursementDate,
                        cashierTillID = ctl.cashiersTillID,
                        postToSavingsAccount = true
                    };
                    le.cashierDisbursements.Add(cd);
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
