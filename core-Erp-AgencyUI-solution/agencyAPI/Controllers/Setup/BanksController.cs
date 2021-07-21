using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using coreLogic.Models.Loans;
using agencyAPI.Providers;

namespace agencyAPI.Controllers.Setup
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class BanksController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();
        core_dbEntities ent = new core_dbEntities();

        public BanksController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        public IEnumerable<BankViewModel> Get()
        {
            return ent.banks.Select(p => new BankViewModel
            {
                bankId = p.bank_id,
                bankName = p.bank_name
            });
        }
    }
}
