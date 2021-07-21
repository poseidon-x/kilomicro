using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using coreLogic;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using agencyAPI.Providers;

namespace agencyAPI.Controllers.ClientSetup
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class BranchController : ApiController
    {
        private readonly coreLoansEntities db;

        public BranchController()
        {//constructor
            var db2 = new coreLoansEntities();
            db2.Configuration.LazyLoadingEnabled = false;
            db2.Configuration.ProxyCreationEnabled = false;
            db = db2;
        }

        [HttpGet]
        public IEnumerable<branch> Get()
        {//get list of all entries 
            return db.branches.ToList();
        }

        [HttpGet]
        public branch Get(int id)
        {//get list of all entries 
            return db.branches
                .FirstOrDefault(p => p.branchID == id);
        }
    }
}
