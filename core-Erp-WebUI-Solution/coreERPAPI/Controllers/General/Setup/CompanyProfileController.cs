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

namespace coreErpApi.Controllers.Controllers.General.Setup
{
    [AuthorizationFilter()]
    public class CompanyProfileController : ApiController
    {
        private Icore_dbEntities le;

        public CompanyProfileController()
        {
            le = new core_dbEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CompanyProfileController(Icore_dbEntities cent)
        {
            le = cent;
        }


        [HttpGet]
        // GET: api/Shipping Methods
        public async Task<IEnumerable<comp_prof>> Get()
        {
            return await le.comp_prof
                .ToListAsync();
        }
      

    }
}




  




