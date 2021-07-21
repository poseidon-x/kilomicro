using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Web.Http.Cors;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    //[AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class LoanGroupDayController : ApiController
    {
        IcoreLoansEntities le;

        public LoanGroupDayController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanGroupDayController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<loanGroupDay> Get()
        {
            return le.loanGroupDays
                .OrderBy(p => p.loanGroupDayId)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public loanGroupDay Get(int id)
        {
            return le.loanGroupDays
                .FirstOrDefault(p => p.loanGroupDayId == id);
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<loanGroup> GetGroupsByDate(int id)
        {
            var grps = le.loanGroups
                .Where(p => p.loanGroupDayId == id)
                .Include(p => p.loanGroupClients)
                .ToList();
            return grps;
        }

    }
}
