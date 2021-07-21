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
    public class PaymentTermController : ApiController
    {
        ICommerceEntities ent;


        public PaymentTermController()
        {
            ent = new CommerceEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public PaymentTermController(ICommerceEntities cent)
        {
            ent = cent;
        }


        [HttpGet]
        // GET: api/Payment Term
        public async Task<IEnumerable<paymentTerm>> Get()
        {
            return await ent.paymentTerms
                .OrderBy(p => p.paymentTerms)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/Payment Term by Id
        public async Task<IEnumerable<paymentTerm>> Get(long id)
        {
            return await ent.paymentTerms
                .Where(p => p.paymentTermID == id)
                .OrderBy(p => p.paymentTerms)
                .ToListAsync();
        }
        


    }
}




  




