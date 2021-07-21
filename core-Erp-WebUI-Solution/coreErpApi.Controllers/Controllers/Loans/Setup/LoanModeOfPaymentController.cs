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

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class LoanModeOfPaymentController : ApiController
    {
        IcoreLoansEntities le;

        public LoanModeOfPaymentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanModeOfPaymentController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/
        public IEnumerable<modeOfPayment> Get()
        {
            return le.modeOfPayments
                .OrderBy(p => p.modeOfPaymentName)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public modeOfPayment Get(int id)
        {
            return le.modeOfPayments
                .FirstOrDefault(p => p.modeOfPaymentID == id);
        }

    }
}
