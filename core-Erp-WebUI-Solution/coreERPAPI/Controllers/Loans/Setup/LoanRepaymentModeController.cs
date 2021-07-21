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
    public class LoanRepaymentModeController : ApiController
    {
        IcoreLoansEntities le;

        public LoanRepaymentModeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanRepaymentModeController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/
        public IEnumerable<repaymentMode> Get()
        {
            return le.repaymentModes
                .OrderBy(p => p.repaymentModeName)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public repaymentMode Get(int id)
        {
            return le.repaymentModes
                .FirstOrDefault(p => p.repaymentModeID == id);
        }

    }
}
