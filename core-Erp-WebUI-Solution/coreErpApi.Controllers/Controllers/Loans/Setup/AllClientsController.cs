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
//using coreERP.Models;

//namespace coreERP.Controllers.Loans.Setup
//{
//    //[AuthorizationFilter()]
//    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
//    public class AllClientsController : ApiController
//    {
//        IcoreLoansEntities le;

//        public AllClientsController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public AllClientsController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/
//        [HttpGet]
//        public IEnumerable<ClientViewModel> Get()
//        {
//            return le.clients
//                .Select(p => new ClientViewModel
//                {
//                    clientID = p.clientID,
//                    clientName = p.surName + ", " + p.otherNames
//                })
//                .OrderBy(p => p.clientID)
//                .ToList();

//        }

//        // GET: api/
//        [HttpGet]
//        public IEnumerable<ClientViewModel> GetGroupClients()
//        {
//            var data =  le.clients
//                .Where(p => p.clientTypeID == 7)
//                .Select(p => new ClientViewModel
//                {
//                    clientID = p.clientID,
//                    clientName = p.surName + " " + p.otherNames,
//                    clientNameWithAccountNO = p.surName + " " + p.otherNames+", "+p.accountNumber
//                })
//                .Distinct()
//                .OrderBy(p => p.clientID)
//                .ToList();

//            return data;
            
//        }

//        // GET: api/
//        [HttpGet]
//        public IEnumerable<ClientViewModel> GetClientWithoutGroup()
//        {
//            return le.clients
//                .Where(p => p.clientTypeID == 7 && !p.loanGroupClients.Any())
//                .Select(p => new ClientViewModel
//                {
//                    clientID = p.clientID,
//                    clientName = p.surName + " " + p.otherNames,
//                    clientNameWithAccountNO = p.surName + " " + p.otherNames + ", " + p.accountNumber
//                })
//                .Distinct()
//                .OrderBy(p => p.clientID)
//                .ToList();
//        }

//        // GET: api/
//        [HttpGet]
//        public IEnumerable<ClientViewModel> GetAllGroupClients()
//        {
//            return le.clients
//                .Where(p => p.clientTypeID == 7)
//                .Select(p => new ClientViewModel
//                {
//                    clientID = p.clientID,
//                    clientName = p.surName + " " + p.otherNames,
//                    clientNameWithAccountNO = p.surName + " " + p.otherNames + ", " + p.accountNumber
//                })
//                .Distinct()
//                .OrderBy(p => p.clientID)
//                .ToList();
//        }

//        // GET: api/
//        [HttpGet]
//        public IEnumerable<ClientViewModel> GetCreditLineClients()
//        {
//            var clientIds = le.creditLines
//                .Select(p => p.clientId)
//                .ToList();

//            return le.clients
//                .Where(p => clientIds.Contains(p.clientID))
//                .Select(p => new ClientViewModel
//                {
//                    clientID = p.clientID,
//                    clientName = p.surName + " " + p.otherNames
//                })
//                .Distinct()
//                .OrderBy(p => p.clientID)
//                .ToList();

//        }

//        // GET: api/clients
//        [HttpGet]
//        public ClientViewModel Get(int id)
//        {
//            return le.clients
//                .Select(p => new ClientViewModel
//                {
//                    clientID = p.clientID,
//                    clientName =  p.surName + " " + p.otherNames
//                })
//                .FirstOrDefault(p => p.clientID == id);
//        }
         
//    }
//}
