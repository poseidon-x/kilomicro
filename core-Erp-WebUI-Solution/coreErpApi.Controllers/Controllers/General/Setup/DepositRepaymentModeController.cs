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
//    public class DepositRepaymentModeController : ApiController
//    {
//        IcoreLoansEntities ent;


//        public DepositRepaymentModeController()
//        {
//            ent = new coreLoansEntities();
//            ent.Configuration.LazyLoadingEnabled = false;
//            ent.Configuration.ProxyCreationEnabled = false;
//        }

//        public DepositRepaymentModeController(IcoreLoansEntities cent)
//        {
//            ent = cent;
//        }

//        [HttpGet]
//        // GET: api/City
//        public async Task<IEnumerable<depositRepaymentMode>> Get()
//        {
//            return await ent.depositRepaymentModes
//                .OrderBy(p => p.repaymentModeName)
//                .ToListAsync();
//        }

//        [HttpGet]
//        // GET: api/City
//        public depositRepaymentMode Get(long id)
//        {
//            return ent.depositRepaymentModes
//                .FirstOrDefault(p => p.depositRepaymentModeId == id);
//        }

        

//    }
//}




  




