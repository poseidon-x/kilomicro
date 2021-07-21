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
    //[AuthorizationFilter()]
    public class IntrstTypeController : ApiController
    {
        IcoreLoansEntities le;

        public IntrstTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public IntrstTypeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<interestType> Get()
        {
            return le.interestTypes
                .OrderBy(p => p.interestTypeName)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public interestType Get(int id)
        {
            return le.interestTypes
                .FirstOrDefault(p => p.interestTypeID == id);
        }
         
    }
}
