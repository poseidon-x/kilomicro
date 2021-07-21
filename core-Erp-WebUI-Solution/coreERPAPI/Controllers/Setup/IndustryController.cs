//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Deposits
//{
//    [AuthorizationFilter()]
//    public class IndustryController : ApiController
//    {
//        IcoreLoansEntities le;

//        public IndustryController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public IndustryController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        public IEnumerable<industry> Get()
//        {
//            return le.industries
//                .OrderBy(p => p.industryName)
//                .ToList();
//        }

//        [HttpGet]
//        public industry Get(int id)
//        {
//            var data = le.industries
//                .FirstOrDefault(p => p.industryID == id);
//            if (data == null)
//            {
//                data = new industry();
//            }
//            return data;
//        }

        
//    }
//}
