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
    public class ClientChargeTypeController : ApiController
    {
        IcoreLoansEntities ent;


        public ClientChargeTypeController()
        {
            ent = new coreLoansEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public ClientChargeTypeController(IcoreLoansEntities cent)
        {
            ent = cent;
        }

        [HttpGet]
        // GET: api/clientServiceCharge
        public IEnumerable<chargeType> Get()
        {
            return ent.chargeTypes
                .OrderBy(p => p.chargeTypeName)
                .ToList();
        }

        [HttpGet]
        // GET: api/City
        public chargeType Get(int id)
        {
            var data =  ent.chargeTypes
                .FirstOrDefault(p => p.chargeTypeID == id);
            if(data == null)data = new chargeType();
            return data;
        }

    }
}




  




