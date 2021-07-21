//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Threading.Tasks;

//namespace coreErpApi.Controllers.Controllers.General.Setup
//{
//    [AuthorizationFilter()]
//    public class CityController : ApiController
//    {
//        Icore_dbEntities ent;


//        public CityController()
//        {
//            ent = new core_dbEntities();
//            ent.Configuration.LazyLoadingEnabled = false;
//            ent.Configuration.ProxyCreationEnabled = false;
//        }

//        public CityController(Icore_dbEntities cent)
//        {
//            ent = cent;
//        }

//        [HttpGet]
//        // GET: api/City
//        public async Task<IEnumerable<cities>> Get()
//        {
//            return await ent.cities
//                .OrderBy(p => p.city_name)
//                .ToListAsync();
//        }

//        [HttpGet]
//        // GET: api/City
//        public async Task<IEnumerable<cities>> Get(long id)
//        {
//            return await ent.cities
//                .Where(p => p.city_id == id)
//                .OrderBy(p => p.city_name)
//                .ToListAsync();
//        }

        

//    }
//}




  




