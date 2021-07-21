using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [AuthorizationFilter()]
    public class DepositPeriodInDaysController : ApiController
    {
        IcoreLoansEntities le;

        public DepositPeriodInDaysController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public DepositPeriodInDaysController(IcoreLoansEntities lent)
        {
            le = lent; 
        }
   
        // GET: api/tenureType
        public IEnumerable<depositPeriodInDay> Get()
        {
            return le.depositPeriodInDays
                .OrderBy(p => p.period)
                .ToList();
        }

        // GET: api/tenureType/
        [HttpGet]
        public depositPeriodInDay Get(int id)
        {
            return le.depositPeriodInDays
                .FirstOrDefault(p => p.depositPeriodInDaysId == id);
        }

        
    }
}
