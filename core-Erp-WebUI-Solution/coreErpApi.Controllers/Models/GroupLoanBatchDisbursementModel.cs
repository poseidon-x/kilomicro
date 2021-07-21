using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class groupLoanBatchDisbursementModel
    {
        public DateTime disbursementDate { get; set; }
        public IEnumerable<LoanViewModel> loans { get; set; }
    }
}