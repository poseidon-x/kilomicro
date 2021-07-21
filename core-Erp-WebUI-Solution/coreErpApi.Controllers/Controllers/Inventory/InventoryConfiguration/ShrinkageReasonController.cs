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
//    public class ShrinkageReasonController : ApiController
//    {
//        CommerceEntities le;

//        public ShrinkageReasonController()
//        {
//            le = new CommerceEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public ShrinkageReasonController(CommerceEntities lent)
//        {
//            le = lent;
//        }
        
//        // GET: api/location
//        public IEnumerable<shrinkageReason> Get()
//        {
//            return le.shrinkageReasons
//                .OrderBy(p => p.shrinkageReasonId)
//                .ToList();
//        }
      
//        // GET: api/location/5
//        [HttpGet]
//        public shrinkageReason Get(int id)
//        {
//            return le.shrinkageReasons
//                .FirstOrDefault(p => p.shrinkageReasonId == id);
//        }
        
//        [HttpPost]
//        public KendoResponse Post(shrinkageReason input)
//        {
//            le.shrinkageReasons
//                .Add(input);
//            le.SaveChanges();

//            return new KendoResponse { Data = new shrinkageReason[] { input } };
//        }
//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "shrinkageReasonCode";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.shrinkageReasons.AsQueryable();
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
//        public KendoResponse Put([FromBody]shrinkageReason value)
//        {
//            var toBeUpdated = le.shrinkageReasons.First(p => p.shrinkageReasonId == value.shrinkageReasonId);

//            toBeUpdated.shrinkageReasonId = value.shrinkageReasonId;
//            toBeUpdated.shrinkageReasonName = value.shrinkageReasonName;
//            toBeUpdated.shrinkageReasonCode = value.shrinkageReasonCode;

//            le.SaveChanges();

//            return new KendoResponse { Data = new shrinkageReason[] { toBeUpdated } };
//        }

//        [HttpDelete]
//        // DELETE: api/brands/5
//        public void Delete([FromBody]shrinkageReason value)
//        {
//            var forDelete = le.shrinkageReasons.FirstOrDefault(p => p.shrinkageReasonId == value.shrinkageReasonId);
//            if (forDelete != null)
//            {
//                le.shrinkageReasons.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }


//    }//class
    
//}//namespace
