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
    public class LnTypeController : ApiController
    {
        IcoreLoansEntities le;

        public LnTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LnTypeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<loanType> Get()
        {
            return le.loanTypes
                .OrderBy(p => p.loanTypeName)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public loanType Get(int id)
        {
            return le.loanTypes
                .FirstOrDefault(p => p.loanTypeID == id);
        }
         
    }
}
