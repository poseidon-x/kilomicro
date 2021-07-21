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
    public class TenureTypeController : ApiController
    {
        IcoreLoansEntities le;

        public TenureTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public TenureTypeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/tenureType
        public IEnumerable<tenureType> Get()
        {
            return le.tenureTypes
                .Where(p => !p.tenureTypeName.ToLower().Contains("day"))
                .OrderBy(p => p.tenureTypeName)
                .ToList();
        }

        // GET: api/tenureType/
        [HttpGet]
        public tenureType Get(int id)
        {
            return le.tenureTypes
                .FirstOrDefault(p => p.tenureTypeID == id);
        }

        
    }
}
