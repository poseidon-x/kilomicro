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
    public class PaymentModeController : ApiController
    {
        IcoreLoansEntities ent;


        public PaymentModeController()
        {
            ent = new coreLoansEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public PaymentModeController(IcoreLoansEntities cent)
        {
            ent = cent;
        }


        [HttpGet]
        // GET: api/Payment Term
        public IEnumerable<modeOfPayment> Get()
        {
            return ent.modeOfPayments
                .OrderBy(p => p.modeOfPaymentName)
                .ToList();
        }

        [HttpGet]
        // GET: api/Payment Term by Id
        public modeOfPayment Get(long id)
        {
            return ent.modeOfPayments
                .FirstOrDefault(p => p.modeOfPaymentID == id);
        }
        


    }
}




  




