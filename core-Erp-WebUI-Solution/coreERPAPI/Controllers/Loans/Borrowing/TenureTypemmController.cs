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
    public class TenureTypemmController : ApiController
    {
        IcoreLoansEntities le;


        public TenureTypemmController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public TenureTypemmController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/tenureType
        public async Task<IEnumerable<tenureType>> Get()
        {
            return await le.tenureTypes
                .OrderBy(p => p.tenureTypeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/tenureType
        public async Task<tenureType> Get(long id)
        {
            var value =  le.tenureTypes
                .FirstOrDefault(p => p.tenureTypeID == id);

            if (value == null)
            {
                value = new tenureType();
            }

            return value;
        }

        

    }
}




  




