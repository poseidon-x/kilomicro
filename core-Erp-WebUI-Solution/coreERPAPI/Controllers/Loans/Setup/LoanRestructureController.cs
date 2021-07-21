using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using coreErpApi.Controllers.Models;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class LoanRestructureController : ApiController
    {
        coreLoansEntities le;

        public LoanRestructureController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/
        [HttpGet]
        public LoanRestructureViewModel Get(int id)
        {
            if (id < 1)
            {
                LoanRestructureViewModel value = new LoanRestructureViewModel();
                return value;
            }
            return null;
        }


        [HttpPost]
        public IEnumerable<repaymentSchedule> Post(LoanRestructureViewModel restrLoan)
        {
            LoanRestructureManager restructMgr = new LoanRestructureManager();

            if (restrLoan.saveChanges)
            {
                return restructMgr.RestructureLoanAddAmountAddTenure(le, restrLoan.loan, restrLoan.additionalPrincipal,
                DateTime.Today,
                 restrLoan.bank, restrLoan.paymentMode, restrLoan.checkNo, restrLoan.additionalTenure, restrLoan.interestRate,
                LoginHelper.getCurrentUser(new coreSecurityEntities()), restrLoan.paymentMode);
            }
            
            return restructMgr.CheckLoanRestructuredSch(le, restrLoan.loan, restrLoan.additionalPrincipal,
                DateTime.Now,
                restrLoan.additionalTenure, restrLoan.interestRate, LoginHelper.getCurrentUser(new coreSecurityEntities()));   
            
            
        }

         
    }
}
