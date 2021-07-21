using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Cashier
{
    [AuthorizationFilter()]
    public class CashierTransferTypeController : ApiController
    {
        IcoreLoansEntities le;

        public CashierTransferTypeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CashierTransferTypeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public IEnumerable<cashierTransferType> Get()
        {
            return le.cashierTransferTypes
                .OrderBy(p => p.transferTypeName)
                .ToList();
        }

        [HttpGet]
        public cashierTransferType Get(int id)
        {
            return le.cashierTransferTypes
                .FirstOrDefault(p => p.cashierTransferTypeId == id);
        }

        
    }
}
