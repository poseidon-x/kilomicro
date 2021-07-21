using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class MetaDataTypeController : ApiController
    {
        IcoreLoansEntities le;

        public MetaDataTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public MetaDataTypeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<metaDataType> Get()
        {
            return le.metaDataTypes
                .OrderBy(p => p.metaDataTypeId)
                .ToList();
        }

        

        // GET: api/
        [HttpGet]
        public metaDataType Get(int id)
        {
            return le.metaDataTypes
                .FirstOrDefault(p => p.metaDataTypeId == id);
        }
         
    }
}
