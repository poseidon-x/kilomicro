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
    public class LoanRepaymentTypeController : ApiController
    {
        IcoreLoansEntities le;

        public LoanRepaymentTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanRepaymentTypeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<repaymentType> Get()
        {
            return le.repaymentTypes
                .Where(p => p.repaymentTypeName.ToLower().Contains("principal")
                || p.repaymentTypeName.ToLower().Contains("interest"))
                .OrderBy(p => p.repaymentTypeName)
                .ToList();
        }
         
    }
}
