//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Inventory.ProductsManagement
//{
//    [AuthorizationFilter()]
//    public class InventoryMethodController : ApiController
//    {
//        CommerceEntities le;

//        public InventoryMethodController()
//        {
//            le = new CommerceEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public InventoryMethodController(CommerceEntities lent)
//        {
//            le = lent; 
//        }

        
//        [HttpGet]
//        // GET: api/inventoryMethod
//        public IEnumerable<inventoryMethod> Get()
//        {
//            return le.inventoryMethods
//                .OrderBy(p => p.inventoryMethodName)
//                .ToList();
//        }

//    }
//}
