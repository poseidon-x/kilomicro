using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class groupLoanBatchRepaymentModel
    {
        public DateTime repaymentDate { get; set; }
        public IEnumerable<BatchRepaymentViewModel> repayments { get; set; }
    }
}