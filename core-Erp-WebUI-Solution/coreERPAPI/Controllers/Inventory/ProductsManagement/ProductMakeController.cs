using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Inventory.ProductsManagement
{
    [AuthorizationFilter()]
    public class ProductMakeController : ApiController
    {
        ICommerceEntities le;

        public ProductMakeController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public ProductMakeController(ICommerceEntities lent)
        {
            le = lent;
        }

        [HttpGet]
        // GET: api/Product
        public IEnumerable<productMake> Get()
        {
            return le.productMakes
                .ToList();
        }


    }
}
