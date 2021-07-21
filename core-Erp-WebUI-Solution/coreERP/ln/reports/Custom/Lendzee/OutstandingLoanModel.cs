using coreReports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.ln.reports.Custom.Lendzee
{
    public class OutstandingLoanModel : OutstandingRepaymentModel
    {
        public double? WriteOffAmount { get; set; }
        public DateTime? WriteOffDate { get; set; }
    }
}