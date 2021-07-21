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
    public class RepaymentModeController : ApiController
    {
        IcoreLoansEntities ent;


        public RepaymentModeController()
        {
            ent = new coreLoansEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public RepaymentModeController(IcoreLoansEntities cent)
        {
            ent = cent;
        }


        [HttpGet]
        // GET: api/Payment Term
        public async Task<IEnumerable<repaymentMode>> Get()
        {
            return await ent.repaymentModes
                .OrderBy(p => p.repaymentModeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/Payment Term by Id
        public async Task<IEnumerable<repaymentMode>> Get(long id)
        {
            return await ent.repaymentModes
                .Where(p => p.repaymentModeID == id)
                .OrderBy(p => p.repaymentModeName)
                .ToListAsync();
        }
        
    }
}




  




