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
    public class CountriesController : ApiController
    {
        Icore_dbEntities ent;


        public CountriesController()
        {
            ent = new core_dbEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public CountriesController(Icore_dbEntities cent)
        {
            ent = cent;
        }

        [HttpGet]
        // GET: api/City
        public async Task<IEnumerable<countries>> Get()
        {
            return await ent.countries
                .OrderBy(p => p.country_name)
                .ToListAsync();
        }


        [HttpGet]
        // GET: api/City
        public async Task<IEnumerable<countries>> Get(long id)
        {
            return await ent.countries
                .Where(p => p.country_id == id)
                .OrderBy(p => p.country_name)
                .ToListAsync();
        }

        

    }
}




  




