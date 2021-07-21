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
//using coreErpApi.Controllers.Models;

//namespace coreErpApi.Controllers.Controllers.Loans.Borrowing
//{
//    //[AuthorizationFilter()]
//    public class BorrowingClientsController : ApiController
//    {
//        IcoreLoansEntities le;

//        public BorrowingClientsController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public BorrowingClientsController(IcoreLoansEntities cent)
//        {
//            le = cent;
//        }

//        [HttpGet]
//        // GET: api/
//        public  IEnumerable<ClientViewModel> Get()
//        {
//            var cl = le.borrowings.ToList();

//            var clients = le.borrowings
//                .Where(p => p.amountApproved < 1)
//                .Join(le.clients, b => b.clientId, c => c.clientID, (b,c) => new ClientViewModel
//                {
//                    clientID = b.clientId,
//                    clientName = c.surName + " " + c.otherNames
//                })
//                .Distinct()
//                .OrderBy(p => p.clientName)
//                .ToList();

//            return clients;
//        }


//        [HttpGet]
//        // GET: api/
//        public IEnumerable<ClientViewModel> GetApprovedBorrowingClient()
//        {
//            var cl = le.borrowings.ToList();

//            var clients = le.borrowings
//                .Where(p => p.amountApproved > 0)
//                .Join(le.clients, b => b.clientId, c => c.clientID, (b, c) => new ClientViewModel
//                {
//                    clientID = b.clientId,
//                    clientName = c.surName + " " + c.otherNames
//                })
//                .Distinct()
//                .OrderBy(p => p.clientName)
//                .ToList();

//            return clients;
//        }

//        [HttpGet]
//        // GET: api/
//        public IEnumerable<ClientViewModel> GetDisbursedBorrowingClient()
//        {
//            var cl = le.borrowings.ToList();

//            var clients = le.borrowings
//                .Where(p => p.amountDisbursed > 0 && p.balance > 0)
//                .Join(le.clients, b => b.clientId, c => c.clientID, (b, c) => new ClientViewModel
//                {
//                    clientID = b.clientId,
//                    clientName = c.surName + " " + c.otherNames
//                })
//                .Distinct()
//                .OrderBy(p => p.clientName)
//                .ToList();

//            return clients;
//        }



//    }
//}









