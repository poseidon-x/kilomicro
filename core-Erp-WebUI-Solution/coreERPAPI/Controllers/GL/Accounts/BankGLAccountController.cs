using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http; 
using coreERP;
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace coreERP.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class BankGLAccountController : ApiController
    {
        core_dbEntities le = new core_dbEntities();

        public BankGLAccountController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<bank_accts> Get()
        {
            return le.bank_accts
                .OrderBy(p=> p.bank_acct_desc)
                .ToList();
        }

        // GET: api/Category
        [HttpGet]
        public IEnumerable<int> GetGlAcctIds()
        {
            return le.bank_accts
                .Select(p => p.gl_acct_id)
                .ToList();
        }

        // GET: api/Category
        public bank_accts Get(int id)
        {
            var acc = le.bank_accts
                .FirstOrDefault(p => p.bank_acct_id == id);
            return acc;
        }
        

         
    }
}
