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
    public class PaymentMethodController : ApiController
    {
        ICommerceEntities ent;


        public PaymentMethodController()
        {
            ent = new CommerceEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public PaymentMethodController(ICommerceEntities cent)
        {
            ent = cent;
        }


        [HttpGet]
        // GET: api/Shipping Methods
        public async Task<IEnumerable<paymentMethod>> Get()
        {
            return await ent.paymentMethods
                .OrderBy(p => p.paymentMethodName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/Shipping Method by Id
        public async Task<IEnumerable<paymentMethod>> Get(long id)
        {
            return await ent.paymentMethods
                .Where(p => p.paymentMethodID == id)
                .OrderBy(p => p.paymentMethodName)
                .ToListAsync();
        }      

    }
}




  




