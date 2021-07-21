using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace coreErpApi.Controllers.Controllers.General.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class ClientTypeController : ApiController
    {
        IcoreLoansEntities ent;


        public ClientTypeController()
        {
            ent = new coreLoansEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public ClientTypeController(IcoreLoansEntities cent)
        {
            ent = cent;
        }

        [HttpGet]
        public IEnumerable<clientType> Get()
        {//get list of all entries 
            return ent.clientTypes.ToList();
        }

        [HttpGet]
        public clientType Get(int id)
        {//get list of all entries 
            return ent.clientTypes
                .FirstOrDefault(p => p.clientTypeId == id);
        }

    }
}




  




