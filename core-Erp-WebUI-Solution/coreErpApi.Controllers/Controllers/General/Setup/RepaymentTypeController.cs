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

namespace coreErpApi.Controllers.Controllers.General.Setup
{
    [AuthorizationFilter()]
    public class RepaymentTypeController : ApiController
    {
        IcoreLoansEntities ent;


        public RepaymentTypeController()
        {
            ent = new coreLoansEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public RepaymentTypeController(IcoreLoansEntities cent)
        {
            ent = cent;
        }


        [HttpGet]
        // GET: api/Payment Type
        public async Task<IEnumerable<repaymentType>> Get()
        {
            return await ent.repaymentTypes
                .OrderBy(p => p.repaymentTypeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/Payment Type by Id
        public async Task<IEnumerable<repaymentType>> Get(long id)
        {
            return await ent.repaymentTypes
                .Where(p => p.repaymentTypeID == id)
                .OrderBy(p => p.repaymentTypeName)
                .ToListAsync();
        }
        
    }
}




  




