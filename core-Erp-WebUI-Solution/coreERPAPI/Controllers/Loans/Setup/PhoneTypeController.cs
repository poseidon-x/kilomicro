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
using coreERP.Providers;

namespace coreErpApi.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class PhoneTypeController : ApiController
    {
        private readonly coreLoansEntities db;

        public PhoneTypeController()
        {//constructor
            var db2 = new coreLoansEntities();
            db2.Configuration.LazyLoadingEnabled = false;
            db2.Configuration.ProxyCreationEnabled = false;
            db = db2;
        }

        [HttpGet]
        public IEnumerable<phoneType> Get()
        {//get list of all entries 
            return db.phoneTypes.ToList();
        }

        [HttpGet]
        public phoneType Get(int id)
        {//get list of all entries 
            return db.phoneTypes
                .FirstOrDefault(p => p.phoneTypeID == id);
        }
    }
}
