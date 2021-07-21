using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setups
{
    public class BankAccountController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();
        core_dbEntities ent = new core_dbEntities();

        public BankAccountController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        public IEnumerable<Models.LookupEntry> Get()
        {
            var bas = ent.bank_accts
                .OrderBy(p => p.bank_acct_desc);
            var mps = new List<Models.LookupEntry>();

            foreach (var ba in bas)
            {
                mps.Add(new Models.LookupEntry
                {
                    ID = ba.bank_acct_id,
                    Description = ba.bank_acct_desc
                });
            }

            return mps;
        }
    }
}
