using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class RepaymentScheduleVM
    {
        public int repaymentScheduleID { get; set; }
        public long principalBalance { get; set; }
        public long interestBalance { get; set; }
    }
}