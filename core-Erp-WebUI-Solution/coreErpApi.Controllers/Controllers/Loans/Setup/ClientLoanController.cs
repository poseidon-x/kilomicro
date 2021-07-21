using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

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
            return le.loans
                .Where(p => p.closed == false && p.clientID == id && p.amountDisbursed > 0
                && p.balance > 0)
                .Include(p => p.loanType)
                .Include(p => p.client)
                .OrderBy(p => p.clientID)
                .ToList();
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
         
    }
}
