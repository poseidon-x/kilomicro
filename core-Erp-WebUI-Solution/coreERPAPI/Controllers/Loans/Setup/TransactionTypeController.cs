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

namespace coreErpApi.Controllers.Controllers.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class TransactionTypeController : ApiController
    {
        IcoreLoansEntities le;

        public TransactionTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public TransactionTypeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public IEnumerable<transactionType> Get()
        {
            return le.transactionTypes
                .OrderBy(p => p.transactionTypeName)
                .ToList();
        }

        [HttpGet]
        public transactionType Get(int id)
        {
            var data = le.transactionTypes
                .FirstOrDefault(p => p.transactionTypeId == id);
            if (data == null)
            {
                data = new transactionType();
            }
            return data;
        }

        
    }
}
