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
    public class ProductSubCategoryController : ApiController
    {
        ICommerceEntities le;

        public ProductSubCategoryController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public ProductSubCategoryController(ICommerceEntities lent)
        {
            le = lent;
        }

        [HttpGet]
        // GET: api/Product
        public IEnumerable<productSubCategory> Get()
        {
            return le.productSubCategories
                .OrderBy(p => p.productSubCategoryName)
                .ToList();
        }


    }
}
