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
    public class SalesTypeController : ApiController
    {
        ICommerceEntities ent;


        public SalesTypeController()
        {
            ent = new CommerceEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public SalesTypeController(ICommerceEntities cent)
        {
            ent = cent;
        }


        [HttpGet]
        // GET: api/Shipping Methods
        public async Task<IEnumerable<salesType>> Get()
        {
            return await ent.salesTypes
                .OrderBy(p => p.salesTypeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/Shipping Method by Id
        public async Task<IEnumerable<salesType>> Get(long id)
        {
            return await ent.salesTypes
                .Where(p => p.salesTypeID == id)
                .OrderBy(p => p.salesTypeName)
                .ToListAsync();
        }      

    }
}




  




