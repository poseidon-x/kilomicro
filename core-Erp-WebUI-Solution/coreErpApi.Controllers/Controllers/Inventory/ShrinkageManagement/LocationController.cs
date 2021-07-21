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
//    public class LocationController : ApiController
//    {
//        ICommerceEntities le;

//        public LocationController()
//        {
//            le = new CommerceEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public LocationController(ICommerceEntities lent)
//        {
//            le = lent;
//        }

//        [HttpGet]
//        // GET: api/Product
//        public IEnumerable<location> Get()
//        {
//            return le.locations
//                .OrderBy(p => p.locationName)
//                .ToList();
//        }

        

//    }
//}
