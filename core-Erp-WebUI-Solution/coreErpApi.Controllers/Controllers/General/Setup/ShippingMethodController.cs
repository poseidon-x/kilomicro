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
    public class ShippingMethodController : ApiController
    {
        ICommerceEntities ent;


        public ShippingMethodController()
        {
            ent = new CommerceEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public ShippingMethodController(ICommerceEntities cent)
        {
            ent = cent;
        }


        [HttpGet]
        // GET: api/Shipping Methods
        public async Task<IEnumerable<shippingMethod>> Get()
        {
            return await ent.shippingMethods
                .OrderBy(p => p.shippingMethodName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/Shipping Method by Id
        public async Task<IEnumerable<shippingMethod>> Get(long id)
        {
            return await ent.shippingMethods
                .Where(p => p.shippingMethodID == id)
                .OrderBy(p => p.shippingMethodName)
                .ToListAsync();
        }      

    }
}




  




