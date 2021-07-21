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
using coreERP;

namespace coreErpApi.Controllers.Controllers.Loans.LoanType
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]

    [AuthorizationFilter()]
    public class AppStageController : ApiController
    {
        IcoreLoansEntities le;

        public AppStageController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public AppStageController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/
        public IEnumerable<loanApprovalStage> Get()
        {
            return le.loanApprovalStages
                .OrderBy(p => p.name)
                .ToList();
        }

        [HttpGet]
        // GET: api/
        public loanApprovalStage Get(int id)
        {
            return le.loanApprovalStages
                .FirstOrDefault(p =>p.loanApprovalStageId == id);
        }

    }
}









