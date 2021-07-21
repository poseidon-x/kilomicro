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
    public class AddressTypeController : ApiController
    {
        private readonly coreLoansEntities db;

        public AddressTypeController()
        {//constructor
            var db2 = new coreLoansEntities();
            db2.Configuration.LazyLoadingEnabled = false;
            db2.Configuration.ProxyCreationEnabled = false;
            db = db2;
        }

        [HttpGet]
        public IEnumerable<addressType> Get()
        {//get list of all entries 
            return db.addressTypes.ToList();
        }

        [HttpGet]
        public addressType Get(int id)
        {//get list of all entries 
            return db.addressTypes
                .FirstOrDefault(p => p.addressTypeID == id);
        }
    }
}
