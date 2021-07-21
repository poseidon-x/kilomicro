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
using coreERP.Models;
using System.Data.Entity;
using coreERP.Providers;

namespace coreERP.Controllers.General.Setup
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class EmailTypeController : ApiController
    {
        private readonly coreLoansEntities db;

        public EmailTypeController()
        {//constructor
            var db2 = new coreLoansEntities();
            db2.Configuration.LazyLoadingEnabled = false;
            db2.Configuration.ProxyCreationEnabled = false;
            db = db2;
        }

        [HttpGet]
        public IEnumerable<emailType> Get()
        {//get list of all entries 
            return db.emailTypes.ToList();
        }

        [HttpGet]
        public emailType Get(int id)
        {//get list of all entries 
            return db.emailTypes
                .FirstOrDefault(p => p.emailTypeID == id);
        }
    }
}
