//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Inventory.Setup
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

//        public IEnumerable<inventoryMethod> Get()
//        {
//            return le.inventoryMethods
//                .OrderBy(p => p.inventoryMethodId)
//                .ToList();
//        }
 
//        // GET: api/location/5
//        [HttpGet]
//        public inventoryMethod Get(int id)
//        {
//            return le.inventoryMethods
//                .FirstOrDefault(p => p.inventoryMethodId == id);
//        }
//        [HttpPost]
//        public KendoResponse Post(inventoryMethod input)
//        {
//            le.inventoryMethods
//                .Add(input);
//            le.SaveChanges();

//            return new KendoResponse{Data = new inventoryMethod[] {input}};
//        }
//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "inventoryMethodId";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<inventoryMethod>(req, parameters);

//            var query = le.inventoryMethods.AsQueryable();
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
//        // PUT: api/inventoryMethods/5
//        public KendoResponse Put([FromBody]inventoryMethod value)
//        {
//            var toBeUpdated = le.inventoryMethods.First(p => p.inventoryMethodId == value.inventoryMethodId);

//            toBeUpdated.inventoryMethodId = value.inventoryMethodId;
//            toBeUpdated.inventoryMethodName = value.inventoryMethodName;
         
//            le.SaveChanges();

//            return new KendoResponse { Data = new inventoryMethod[] { toBeUpdated } };
//        }

//        [HttpDelete]
//        // DELETE: api/inventoryMethods/5
//        public void Delete([FromBody]inventoryMethod value)
//        {
//            var forDelete = le.inventoryMethods.FirstOrDefault(p => p.inventoryMethodId == value.inventoryMethodId);
//            if (forDelete != null)
//            {
//                le.inventoryMethods.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }


//    }//class
    
//}//namespace
