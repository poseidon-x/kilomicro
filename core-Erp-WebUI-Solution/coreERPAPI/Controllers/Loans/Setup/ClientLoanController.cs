using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using coreErp.Models;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class ClientLoanController : ApiController
    {
        IcoreLoansEntities le;

        public ClientLoanController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public ClientLoanController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<loan> GetClientLoans(int id)
        {
            var data =  le.loans
                .Where(p => p.closed == false && p.clientID == id && p.amountDisbursed > 0
                && p.balance > 0)
                .Include(p => p.loanType)
                .Include(p => p.client)
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        //// GET: api/
        //public IEnumerable<loan> GetClientLoansByCredLnAmt(creditLine credLin)
        //{
        //    return le.loans
        //        .Where(p => p.closed == false && p.clientID == credLin.clientId && p.amountDisbursed < credLin.amountApproved
        //        && p.balance > 0)
        //        .Include(p => p.loanType)
        //        .Include(p => p.client)
        //        .OrderBy(p => p.clientID)
        //        .ToList();
        //}

        // GET: api/
        [HttpGet]
        public loan Get(int id)
        {
            return le.loans
                .Where(p => p.closed == false)
                .FirstOrDefault(p => p.clientID == id);
        }

        [HttpGet]
        public LoanViewModel GetLoan(int id)
        {

            var loan = le.loans
                .Include(p => p.repaymentSchedules)
                .Select(p => new LoanViewModel
                {
                    loanId = p.loanID,
                    loanTypeId = p.loanTypeID,
                    loanNumber = p.loanNo,
                    interestBalance = p.repaymentSchedules.Sum(q => q.interestBalance),
                    balance = p.balance
                })
                .FirstOrDefault(p => p.loanId == id);
            //loan.balance = Math.Round(loan.balance, 2, MidpointRounding.AwayFromZero);

            if(loan == null) loan = new LoanViewModel();
            return loan;
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<LoanViewModel> GetClientRunningLoans(int id)
        {
            var loan = le.loans
                .Where(p => p.clientID == id && p.loanStatusID == 4 && p.balance > 0 )
                .Select(p => new LoanViewModel
                {
                    loanId = p.loanID,
                    loanNumber = p.loanNo,
                })
                .ToList();

            return loan;
        }

    }
}
