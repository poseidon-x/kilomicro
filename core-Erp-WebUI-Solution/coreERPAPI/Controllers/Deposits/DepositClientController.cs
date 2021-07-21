using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using System.Threading.Tasks;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    //[AuthorizationFilter()]
    public class DepositClientController : ApiController
    {
        IcoreLoansEntities le;

        public DepositClientController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public DepositClientController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/ Returns Clients with Deposit
        [HttpGet]
        public async Task<IEnumerable<ClientViewModel>> GetAllDepositClients()
        {
            return await le.deposits
                .Where(p => p.principalBalance > 0)
                .Join(le.clients, d => d.clientID, c => c.clientID, (d, c) => new ClientViewModel
                {
                    clientID = c.clientID,
                    clientName = c.surName + " " + c.otherNames
                })
                .Distinct()
                .ToListAsync();
        }


        //// GET: api/ Returns Clients with Cashed Checks
        //public async Task<IEnumerable<ClientViewModel>> GetAllClientsWithCashedChecks()
        //{
        //    return await le.clientCheckDetails.Where(p => p.cashed && p.balance > 0)
        //        .Join(le.clients, ch => ch.clientCheck.clientId, c => c.clientID, (ch, c) => new ClientViewModel
        //        {
        //            clientID = c.clientID,
        //            clientName = c.surName + " " + c.otherNames
        //        })
        //        .Distinct()
        //        .ToListAsync();
        //}
    }
}
