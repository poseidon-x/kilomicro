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
using coreErpApi.Controllers.Models;

namespace coreErpApi.Controllers.Controllers.Loans.Borrowing
{
    [AuthorizationFilter()]
    public class ScheduleController : ApiController
    {
        coreLoansEntities le;

        public ScheduleController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }


        [HttpPost]
        public IEnumerable<borrowingRepaymentSchedule> GetSampleRepaySchedule(borrowing brw)
        {
            var bsm = new BorrowingScheduleManager(); 
            var sched = bsm.GenerateBorrowingRepaymentSch(le, brw, brw.amountRequested,
                System.DateTime.Today, (int)brw.borrowingTenure, (int)brw.interestRate);

            return sched;
        }



    }
}









