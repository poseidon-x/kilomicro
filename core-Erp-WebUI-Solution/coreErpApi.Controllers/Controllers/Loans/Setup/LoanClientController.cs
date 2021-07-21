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
//using coreERP.Models;

//namespace coreErpApi.Controllers.Controllers.Loans.Setup
//{
//    [AuthorizationFilter()]
//    public class LoanClientController : ApiController
//    {
//        IcoreLoansEntities le;

//        public LoanClientController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public LoanClientController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/ Returns Clients with Loan
//        public async Task<IEnumerable<ClientViewModel>> Get()
//        {
//            return await le.loans
//                .Where(p => p.closed == false && p.balance > 0 && p.repaymentModeID == 30
//                && p.disbursementDate.Value.AddMonths(Convert.ToInt32(p.loanTenure)) > DateTime.Now)
//                .Join(le.clients, l => l.clientID, c => c.clientID, (l,c) => new ClientViewModel
//                {
//                    clientID = l.clientID,
//                    clientName = c.surName + " " + c.otherNames
//                })
//                .OrderBy(p => p.clientID)
//                .ToListAsync();

//        }


//        // GET: api/ Returns Clients with Loan
//        public IEnumerable<ClientViewModel> GetAllLoanClients()
//        {
//            return le.loans.AsEnumerable()
//                .Where(p => p.closed == false && p.balance > 0
//                && p.disbursementDate.Value
//                .AddMonths(Convert.ToInt32(p.loanTenure)) > DateTime.Now)
//                .Join(le.clients, l => l.clientID , c => c.clientID, (l,c) => new ClientViewModel
//                {
//                    clientID = l.clientID,
//                    clientName = c.surName + " " + c.otherNames
//                })
//                .Distinct()
//                .OrderBy(p => p.clientName)
//                .ToList();
//        }



//    }
//}
