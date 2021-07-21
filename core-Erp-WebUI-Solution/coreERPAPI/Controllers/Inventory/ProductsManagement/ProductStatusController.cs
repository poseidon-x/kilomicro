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
    public class ProductStatusController : ApiController
    {
        ICommerceEntities le;

        public ProductStatusController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public ProductStatusController(ICommerceEntities lent)
        {
            le = lent;
        }

        [HttpGet]
        // GET: api/Product
        public IEnumerable<productStatu> Get()
        {
            return le.productStatus
                .ToList();
        }


    }
}
