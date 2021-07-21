//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Inventory.ShrinkageManagement
//{
//    [AuthorizationFilter()]
//    public class InventoryItemController : ApiController
//    {
//        ICommerceEntities le;

//        public InventoryItemController()
//        {
//            le = new CommerceEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public InventoryItemController(ICommerceEntities lent)
//        {
//            le = lent;
//        }

//        [HttpGet]
//        // GET: api/Product
//        public IEnumerable<inventoryItem> Get()
//        {
//            return le.inventoryItems
//                .OrderBy(p => p.inventoryItemName)
//                .ToList();
//        }


//    }
//}
