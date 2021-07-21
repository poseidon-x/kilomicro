using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Web.Http.Cors;

namespace coreERP.Controllers.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class MaritalStatusController : ApiController
    {
        IcoreLoansEntities le;

        public MaritalStatusController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public MaritalStatusController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        [HttpGet]
        public IEnumerable<maritalStatu> Get()
        {
            return le.maritalStatus
                .OrderBy(p => p.maritalStatusName)
                .ToList();
        }

        [HttpGet]
        public maritalStatu Get(int id)
        {
            var data = le.maritalStatus
                .FirstOrDefault(p => p.maritalStatusID == id);
            if (data == null)
            {
                data = new maritalStatu();
            }
            return data;
        }

        
    }
}
