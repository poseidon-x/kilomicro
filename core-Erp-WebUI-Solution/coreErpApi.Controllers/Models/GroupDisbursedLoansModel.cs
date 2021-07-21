using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class GroupLoanBatchDisbursementModel
    {
        //client id
        public int groupId { get; set; }
        public string groupNumber { get; set; }
        public IEnumerable<groupLoanBatchDisbursementModel> disbursedLoans { get; set; }
    }
}