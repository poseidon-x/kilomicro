using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Web.Http.Cors;

namespace coreERP.Controllers.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class ClientCategoryController : ApiController
    {
        IcoreLoansEntities le;

        public ClientCategoryController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public ClientCategoryController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public IEnumerable<category> Get()
        {
            return le.categories
                .OrderBy(p => p.categoryName)
                .ToList();
        }

        [HttpGet]
        public category Get(int id)
        {
            var data = le.categories
                .FirstOrDefault(p => p.categoryID == id);
            if (data == null)
            {
                data = new category();
            }
            return data;
        }

        
    }
}
