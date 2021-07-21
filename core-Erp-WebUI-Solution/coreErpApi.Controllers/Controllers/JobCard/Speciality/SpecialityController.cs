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

namespace coreErpApi.Controllers.Controllers.JobCard.Speciality
{
    [AuthorizationFilter()]
    public class SpecialityController : ApiController
    {
        ICommerceEntities le;


        public SpecialityController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public SpecialityController(ICommerceEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/Speciality
        public async Task<IEnumerable<speciality>> Get()
        {
            return await le.specialities
                .OrderBy(p => p.specialityName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/speciality
        public async Task<IEnumerable<speciality>> Get(long id)
        {
            return await le.specialities
                .Where(p => p.specialityId == id)
                .OrderBy(p => p.specialityName)
                .ToListAsync();
        }

        

    }
}




  




