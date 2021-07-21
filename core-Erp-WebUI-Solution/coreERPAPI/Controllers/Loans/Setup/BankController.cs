using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class BankController : ApiController
    {
        Icore_dbEntities le;

        public BankController()
        {
            le = new core_dbEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public BankController(Icore_dbEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<bank_accts> Get()
        {
            return le.bank_accts
                .OrderBy(p => p.bank_acct_desc)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public bank_accts Get(int id)
        {
            return le.bank_accts
                .FirstOrDefault(p => p.bank_acct_id == id);
        }
         
    }
}
