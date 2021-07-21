//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Web.Http.Cors;
//using coreErpApi.Controllers.Models;

//namespace coreErpApi.Controllers.Controllers.Staff
//{
//    //[AuthorizationFilter()]
//    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
//    public class StaffController : ApiController
//    {
//        IcoreLoansEntities le;

//        public StaffController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public StaffController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/
//        public IEnumerable<StaffViewModel> Get()
//        {
//            return le.staffs
//                .Select(p => new StaffViewModel
//                {
//                    staffId = p.staffID,
//                    staffName = p.surName+", "+p.otherNames,
//                    staffNo = p.staffNo
//                })
//                .OrderBy(i => i.staffName)
//                .ToList();
//        }

//        // GET: api/
//        [HttpGet]
//        public StaffViewModel Get(int id)
//        {
//            return le.staffs
//                .Select(p => new StaffViewModel
//                {
//                    staffId = p.staffID,
//                    staffName = p.surName + ", " + p.otherNames,
//                    staffNo = p.staffNo
//                })
//                .FirstOrDefault(i => i.staffId == id);
//        }
         
//    }
//}
