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

namespace coreErpApi.Controllers.Controllers.Loans.Borrowing
{
    //[AuthorizationFilter()]
    public class BorrowingTypeController : ApiController
    {
        IcoreLoansEntities le;


        public BorrowingTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public BorrowingTypeController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/
        public async Task<IEnumerable<borrowingType>> Get()
        {
            return await le.borrowingTypes
                .OrderBy(p => p.borrowingTypeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/
        public async Task<borrowingType> Get(long id)
        {
            var value = le.borrowingTypes
                .FirstOrDefault(p => p.borrowingTypeId == id);

            if (value == null)
            {
                value = new borrowingType();
            }

            return value;
        }

        

    }
}




  




