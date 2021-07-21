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

namespace coreErpApi.Controllers.Loans.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class FinancialTypeController : ApiController
    {
        IcoreLoansEntities le;


        public FinancialTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public FinancialTypeController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/
        public async Task<IEnumerable<financialType>> Get()
        {
            return await le.financialTypes
                .OrderBy(p => p.financialTypeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/
        public financialType Get(long id)
        {
            var value = le.financialTypes
                .FirstOrDefault(p => p.financialTypeID == id);

            value = value ?? new financialType();
           
            return value;
        }

        

    }
}




  




