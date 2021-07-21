using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using coreLogic.Models.Loans;

namespace coreERP.Controllers.Loans.Setups
{
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
