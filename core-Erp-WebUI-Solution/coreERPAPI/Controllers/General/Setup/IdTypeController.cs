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
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class IdTypeController : ApiController
    {
        private readonly coreLoansEntities db;

        public IdTypeController()
        {//constructor
            var db2 = new coreLoansEntities();
            db2.Configuration.LazyLoadingEnabled = false;
            db2.Configuration.ProxyCreationEnabled = false;
            db = db2;
        }

        [HttpGet]
        public IEnumerable<IdTypeViewModel> Get()
        {//get list of all entries 
            return db.idNoTypes
                .Select(p => new IdTypeViewModel
                {
                    idTypeId = p.idNoTypeID,
                    idTypeName = p.idNoTypeName
                })
                .ToList();
        }

        [HttpGet]
        public idNoType Get(int id)
        {//get list of all entries 
            return db.idNoTypes
                .FirstOrDefault(p => p.idNoTypeID == id);
        }
    }
}
