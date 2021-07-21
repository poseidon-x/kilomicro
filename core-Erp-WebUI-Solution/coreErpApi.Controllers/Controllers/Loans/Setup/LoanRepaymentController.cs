using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using coreErpApi.Controllers.Models;
using coreERP.Models;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class LoanRepaymentController : ApiController
    {
        IcoreLoansEntities le;

        public LoanRepaymentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanRepaymentController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public LoanRestructureViewModel Get(int id)
        {
            var loan = le.loans
                .FirstOrDefault(p => p.loanID == id);

            var principalAndInterestId = le.repaymentTypes
                .Where(p => p.repaymentTypeName.ToLower().Contains("principal and interest"))
                .FirstOrDefault().repaymentTypeID;

            var penaltyId = le.repaymentTypes
                .Where(p => p.repaymentTypeName.ToLower().Contains("penalty"))
                .FirstOrDefault().repaymentTypeID;

            var repayments = le.loanRepayments
                .Where(p => p.repaymentTypeID == principalAndInterestId && p.loanID == id)
                .OrderBy(p => p.loanRepaymentID)
                .ToList();

            var panaltyPayments = le.loanRepayments
                .Where(p => p.repaymentTypeID == penaltyId && p.loanID == id)
                .OrderBy(p => p.loanRepaymentID)
                .ToList();


            var penalties = le.loanPenalties
                .Where(p => p.loanID == id)
                .OrderByDescending(p => p.loanPenaltyID)
                .ToList();

            var dataToReturn = new LoanRestructureViewModel
            {
                loanTotalPrincipal = loan.amountDisbursed,
                loanTotalInterest = loan.amountDisbursed*(loan.interestRate/100),
                loanBalance = loan.balance,
                totalPrinPaid = 0,
                totalIntrPaid = 0,
                totalyPenalties = 0,
                penaltyBalance = 0,
                totalPenaltyPayms = 0
            };

            double loanOverallBalanace = 0;


            if (panaltyPayments.Any())
            {
                foreach (var record in panaltyPayments)
                {
                    dataToReturn.totalPenaltyPayms += record.penaltyPaid;
                }
                var length = penalties.Count - 1;
                dataToReturn.totalyPenalties = penalties[length].penaltyBalance;
            }

            foreach (var record in repayments)
            {
                dataToReturn.totalPrinPaid += record.principalPaid;
                dataToReturn.totalIntrPaid += record.interestPaid;
            }

            
            dataToReturn.penaltyBalance = dataToReturn.totalyPenalties - dataToReturn.totalPenaltyPayms;

            return dataToReturn;
        }

         
    }
}
