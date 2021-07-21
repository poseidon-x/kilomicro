//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Inventory.InventoryConfiguration
//{
//    [AuthorizationFilter()]
//    public class BrandController : ApiController
//    {
//        CommerceEntities le;

//        public BrandController()
//        {
//            le = new CommerceEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public BrandController(CommerceEntities lent)
//        {
//            le = lent;
//        }
        
//        // GET: api/location
//        public IEnumerable<brand> Get()
//        {
//            return le.brands
//                .OrderBy(p => p.brandId)
//                .ToList();
//        }
      
//        // GET: api/location/5
//        [HttpGet]
//        public brand Get(int id)
//        {
//            return le.brands
//                .FirstOrDefault(p => p.brandId == id);
//        }
//        [HttpPost]
//        public KendoResponse Post(brand input)
//        {
//            le.brands
//                .Add(input);
//            le.SaveChanges();

//            return new KendoResponse{ Data = new brand[] { input } };
//        }
//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "brandId";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.brands.AsQueryable();
//            if (whereClause != null && whereClause.Trim().Length > 0)
//            {
//                query = query.Where(whereClause, parameters.ToArray());
//            }

//            var data = query
//                .OrderBy(order.ToString())
//                .Skip(req.skip)
//                .Take(req.take)
//                .ToArray();

//            return new KendoResponse(data, query.Count());
//        }

//        [HttpPut]
//        // PUT: api/brands/5
//        public KendoResponse Put([FromBody]brand value)
//        {
//            var toBeUpdated = le.brands.First(p => p.brandId == value.brandId);

//            toBeUpdated.brandId = value.brandId;
//            toBeUpdated.brandName = value.brandName;
//            toBeUpdated.brandCode = value.brandCode;

//            le.SaveChanges();

//            return new KendoResponse { Data = new brand[] { toBeUpdated } };
//        }

//        [HttpDelete]
//        // DELETE: api/brands/5
//        public void Delete([FromBody]brand value)
//        {
//            var forDelete = le.brands.FirstOrDefault(p => p.brandId == value.brandId);
//            if (forDelete != null)
//            {
//                le.brands.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }


//    }//class
    
//}//namespace
