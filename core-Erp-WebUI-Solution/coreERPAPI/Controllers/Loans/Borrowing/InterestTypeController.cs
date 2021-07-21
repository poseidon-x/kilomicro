using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace coreErpApi.Controllers.Controllers.Borrowing
{
    [AuthorizationFilter()]
    public class InterestTypeController : ApiController
    {
        IcoreLoansEntities le;


        public InterestTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public InterestTypeController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/
        public async Task<IEnumerable<interestType>> Get()
        {
            return await le.interestTypes
                .OrderBy(p => p.interestTypeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/
        public async Task<interestType> Get(long id)
        {
            var value = le.interestTypes
                .FirstOrDefault(p => p.interestTypeID == id);

            if (value == null)
            {
                value = new interestType();
            }

            return value;
        }

        

    }
}




  




